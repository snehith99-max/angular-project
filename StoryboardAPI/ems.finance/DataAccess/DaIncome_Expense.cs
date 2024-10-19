using ems.finance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Text.RegularExpressions;

namespace ems.finance.DataAccess
{
    public class DaIncome_Expense
    {
        string msSQL, msSQL1, msSQL2 = string.Empty;
        DataTable dt_datatable;
        dbconn objdbconn = new dbconn();
        OdbcDataReader objOdbcDataReader;
        string mainGroup_name, subgroup_name;
        public void DaGetIncomeSummary(MdlIncome_Expanse values)
        {
            try
            {
                msSQL = " SELECT a.account_gid, a.account_code, upper(a.account_name) as account_name, a.accountgroup_gid, upper(a.accountgroup_name) as accountgroup_name, " +
                        " date_format(c.transaction_date, '%M') AS transaction_year, " +
                        " format(SUM(b.transaction_amount),2) AS total_transaction_amount, d.salestype_name " +
                        " FROM acc_mst_tchartofaccount a " +
                        " LEFT JOIN acc_trn_journalentrydtl b ON b.account_gid = a.account_gid " +
                        " LEFT JOIN acc_trn_journalentry c ON c.journal_gid = b.journal_gid " +
                        " LEFT JOIN smr_trn_tsalestype d ON d.account_gid = b.account_gid " +
                        " WHERE a.ledger_type = 'Y' AND a.display_type = 'Y' AND a.accountgroup_name<> '$' " +
                        " AND c.transaction_date >= DATE_SUB(CURRENT_DATE(), INTERVAL 5 MONTH) " +
                        " GROUP BY a.account_name, transaction_year, d.salestype_name " +
                        " ORDER BY account_name, c.transaction_date desc, transaction_year ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<IncomeSummary_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    string msSQL1 = @"
                    SELECT 
                        account_gid, 
                        accountgroup_gid, 
                        upper(accountgroup_name) as accountgroup_name 
                    FROM acc_mst_tchartofaccount";
                    DataTable dtAccountGroups = objdbconn.GetDataTable(msSQL1);

                    var subgroupNameDict = new Dictionary<string, string>();
                    var mainGroupNameDict = new Dictionary<string, string>();

                    foreach (DataRow row in dtAccountGroups.Rows)
                    {
                        string account_gid = row["account_gid"].ToString();
                        string accountgroup_gid = row["accountgroup_gid"].ToString();
                        string accountgroup_name = row["accountgroup_name"].ToString();

                        if (!subgroupNameDict.ContainsKey(account_gid))
                        {
                            subgroupNameDict[account_gid] = accountgroup_name;
                        }

                        if (!mainGroupNameDict.ContainsKey(accountgroup_gid))
                        {
                            var mainGroupRow = dtAccountGroups.AsEnumerable()
                                .FirstOrDefault(r => r["account_gid"].ToString() == accountgroup_gid);

                            string mainGroupName = mainGroupRow != null ? mainGroupRow["accountgroup_name"].ToString() : null;

                            if (mainGroupName == "$")
                            {
                                mainGroupName = accountgroup_name;
                            }
                            mainGroupNameDict[accountgroup_gid] = mainGroupName;
                        }
                    }

                    foreach (DataRow ds in dt_datatable.Rows)
                    {
                        string account_gid = ds["account_gid"].ToString();
                        string accountgroup_gid = ds["accountgroup_gid"].ToString();

                        subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                        mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";

                        if (mainGroup_name == "$")
                        {
                            mainGroup_name = subgroup_name;
                            subgroup_name = "-";
                        }

                        getModuleList.Add(new IncomeSummary_list
                        {
                            account_code = ds["account_code"].ToString(),
                            account_name = ds["account_name"].ToString(),
                            accountgroup_gid = accountgroup_gid,
                            accountgroup_name = ds["accountgroup_name"].ToString(),
                            account_gid = account_gid,
                            subgroup_name = subgroup_name,
                            MainGroup_name = mainGroup_name,
                            salestype_name = ds["salestype_name"].ToString(),
                            transaction_year = ds["transaction_year"].ToString(),
                            transaction_amount = ds["total_transaction_amount"].ToString(),
                       });
                    }
                    values.IncomeSummary_list = getModuleList;
                }
            }
            catch (Exception ex) 
            {

            }
        }
        public void DaGetExpenseSummary(MdlIncome_Expanse values)
        {
            try
            {
                msSQL = " select a.account_code, upper(a.account_name) as account_name, a.accountgroup_gid, upper(a.accountgroup_name) as accountgroup_name, " +
                        " a.account_gid, e.purchasetype_name, date_format(c.transaction_date,'%M') AS transaction_year, format(SUM(b.transaction_amount),2) AS total_transaction_amount " +
                        " from acc_mst_tchartofaccount a " +
                        " left join acc_trn_journalentrydtl b on a.account_gid = b.account_gid " +
                        " left join acc_trn_journalentry c on b.journal_gid = c.journal_gid " +
                        " left join pmr_trn_tpurchasetype e on e.account_gid=b.account_gid " +
                        " where a.ledger_type='Y' and a.display_type='N' and a.accountgroup_name <> '$' " +
                        " and c.transaction_date >= date_sub(current_date(), interval 5 month) " +
                        " group by a.account_name order by account_name, c.transaction_date desc, transaction_year ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<ExpenseSummary_list>();

                if (dt_datatable.Rows.Count > 0)
                {
                    string msSQL1 = @"
                    SELECT 
                        account_gid, 
                        accountgroup_gid, 
                        upper(accountgroup_name) as accountgroup_name
                    FROM acc_mst_tchartofaccount";
                    DataTable dtAccountGroups = objdbconn.GetDataTable(msSQL1);

                    var subgroupNameDict = new Dictionary<string, string>();
                    var mainGroupNameDict = new Dictionary<string, string>();

                    foreach (DataRow row in dtAccountGroups.Rows)
                    {
                        string account_gid = row["account_gid"].ToString();
                        string accountgroup_gid = row["accountgroup_gid"].ToString();
                        string accountgroup_name = row["accountgroup_name"].ToString();

                        if (!subgroupNameDict.ContainsKey(account_gid))
                        {
                            subgroupNameDict[account_gid] = accountgroup_name;
                        }

                        if (!mainGroupNameDict.ContainsKey(accountgroup_gid))
                        {
                            var mainGroupRow = dtAccountGroups.AsEnumerable()
                                .FirstOrDefault(r => r["account_gid"].ToString() == accountgroup_gid);

                            string mainGroupName = mainGroupRow != null ? mainGroupRow["accountgroup_name"].ToString() : null;

                            if (mainGroupName == "$")
                            {
                                mainGroupName = accountgroup_name;
                            }
                            mainGroupNameDict[accountgroup_gid] = mainGroupName;
                        }
                    }

                    foreach (DataRow ds in dt_datatable.Rows)
                    {
                        string account_gid = ds["account_gid"].ToString();
                        string accountgroup_gid = ds["accountgroup_gid"].ToString();

                        subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                        mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";

                        if (mainGroup_name == "$")
                        {
                            mainGroup_name = subgroup_name;
                            subgroup_name = "-";
                        }

                        getModuleList.Add(new ExpenseSummary_list
                        {
                            account_code = ds["account_code"].ToString(),
                            account_name = ds["account_name"].ToString(),
                            accountgroup_gid = accountgroup_gid,
                            accountgroup_name = ds["accountgroup_name"].ToString(),
                            account_gid = account_gid,
                            subgroup_name = subgroup_name,
                            MainGroup_name = mainGroup_name,
                            purchasetype_name = ds["purchasetype_name"].ToString(),
                            transaction_year = ds["transaction_year"].ToString(),
                            transaction_amount = ds["total_transaction_amount"].ToString(),
                        });
                    }
                    values.ExpenseSummary_list = getModuleList;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DaIncomeExpenseGraph(MdlIncome_Expanse values)
        {
            try
            {
                msSQL = " select date_format(a.transaction_date,'%b-%Y') as transaction_date, sum(transaction_amount) as transaction_amount, 'Sales' as source from acc_trn_journalentry a " +
                        " left join acc_trn_journalentrydtl b on b.journal_gid = a.journal_gid " +
                        " left join acc_mst_tchartofaccount c on c.account_gid = b.account_gid " +
                        " where a.transaction_date >= date_sub(current_date(), interval 5 month) and c.ledger_type = 'Y' and c.display_type = 'Y' group by month(a.transaction_date) " +
                        " union " +
                        " select date_format(a.transaction_date,'%b-%Y') as transaction_date, sum(transaction_amount) as transaction_amount,'Purchase' as source   from acc_trn_journalentry a " +
                        " left join acc_trn_journalentrydtl b on b.journal_gid = a.journal_gid " +
                        " left join acc_mst_tchartofaccount c on c.account_gid = b.account_gid " +
                        " where a.transaction_date >= date_sub(current_date(), interval 5 month) and c.ledger_type = 'Y' and c.display_type = 'N' group by month(a.transaction_date) order by STR_TO_DATE(transaction_date, '%b-%Y') ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetIncome_Expense = new List<GetIncome_Expense_list>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt_datatable.Rows)
                    {
                        GetIncome_Expense.Add(new GetIncome_Expense_list
                        {
                            transaction_amount = dr["transaction_amount"].ToString(),
                            transaction_date = dr["transaction_date"].ToString(),
                            source = dr["source"].ToString(),
                        });
                    }
                    values.GetIncome_Expense_list = GetIncome_Expense;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}