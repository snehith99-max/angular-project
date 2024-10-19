using ems.finance.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.finance.DataAccess
{
    public class DaBSIEReports
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        string msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        public void DaGetLiabilitySummary(string entity_gid, string finyear, MdlBSIEReports values)
        {
            msSQL = " select b.account_code,format(a.opening_balance, 2) AS transaction_amount, a.financial_year, b.account_name, b.accountgroup_name,a.opening_balance_gid " +
                    " ,a.account_gid,a.subgroup_account_gid,a.group_account_gid from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " where b.ledger_type = 'N' and display_type = 'N' " +
                    " and a.financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "' order by a.group_account_gid,subgroup_account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Liability_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);

                    if (objOdbcDataReader.HasRows == true)
                    {
                        string subgroup_name = objOdbcDataReader["accountgroup_name"].ToString();
                        string accountgroup_gid = objOdbcDataReader["accountgroup_gid"].ToString();

                        msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                        string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);
                        string sum_of_maingroup;
                        if (MainGroup_name == "$")
                        {
                            MainGroup_name = subgroup_name;
                            subgroup_name = "-";
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + accountgroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        else
                        {
                            msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                            string Maingroup_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + Maingroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        getModuleList.Add(new Liability_list
                        {
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            openingfinancial_year = dt["financial_year"].ToString(),
                            credit_amount = dt["transaction_amount"].ToString(),
                            opening_balance_gid = dt["opening_balance_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            subgroup_name = subgroup_name,
                            MainGroup_name = MainGroup_name,
                            sum_of_maingroup = sum_of_maingroup,
                        });
                        values.Liability_list = getModuleList;
                    }
                    objOdbcDataReader.Close();
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAssetSummary(string entity_gid, string finyear, MdlBSIEReports values)
        {
            msSQL = " select b.account_code,format(a.opening_balance, 2) AS transaction_amount, a.financial_year, b.account_name, b.accountgroup_name,a.opening_balance_gid " +
                             " ,a.account_gid,a.subgroup_account_gid,a.group_account_gid from acc_trn_topeningbalance a " +
                             " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                             " where b.ledger_type = 'N' and display_type = 'Y' " +
                             " and a.financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "' order by a.group_account_gid,subgroup_account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Asset_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        string subgroup_name = objOdbcDataReader["accountgroup_name"].ToString();
                        string accountgroup_gid = objOdbcDataReader["accountgroup_gid"].ToString();

                        msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                        string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);
                        string sum_of_maingroup;
                        if (MainGroup_name == "$")
                        {
                            MainGroup_name = subgroup_name;
                            subgroup_name = "-";
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + accountgroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        else
                        {
                            msSQL = "select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                            string Maingroup_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select format(sum(opening_balance),2) from acc_trn_topeningbalance where group_account_gid= '" + Maingroup_gid + "' and financial_year = '" + finyear + "' and entity_gid='" + entity_gid + "'";
                            sum_of_maingroup = objdbconn.GetExecuteScalar(msSQL);
                        }
                        getModuleList.Add(new Asset_list
                        {
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            openingfinancial_year = dt["financial_year"].ToString(),
                            debit_amount = dt["transaction_amount"].ToString(),
                            opening_balance_gid = dt["opening_balance_gid"].ToString(),
                            account_code = dt["account_code"].ToString(),
                            subgroup_name = subgroup_name,
                            MainGroup_name = MainGroup_name,
                            sum_of_maingroup = sum_of_maingroup,
                        });
                        values.Asset_list = getModuleList;
                    }
                    objOdbcDataReader.Close();
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetLedgerView(string account_gid, string finyear, string branch_name, MdlBSIEReports values)
        {
            msSQL = "select a.opening_balance_gid,a.account_gid,b.account_name,a.opening_balance,a.account_ref_no,a.created_date from acc_trn_topeningbalance a " +
                    " left join acc_mst_tchartofaccount b on b.account_gid = a.account_gid " +
                    " where a.account_gid = '" + account_gid + "' and a.entity_gid = '" + branch_name + "' and a.financial_year = '" + finyear + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<LedgerView_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new LedgerView_list
                    {
                        opening_balance_gid = dt["opening_balance_gid"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        opening_balance = dt["opening_balance"].ToString(),
                        account_ref_no = dt["account_ref_no"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.LedgerView_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetIncomeSummary(string finyear, MdlBSIEReports values)
        {
            msSQL = "select a.journal_gid,a.transaction_date,a.branch_gid,a.journal_year,a.transaction_gid, " +
                    " format(sum(b.transaction_amount),2) as transaction_amount,b.account_gid,c.account_name,c.accountgroup_gid,c.accountgroup_name " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tchartofaccount c on c.account_gid = b.account_gid " +
                    " where c.ledger_type='Y' and c.display_type='Y' and a.journal_year='" + finyear + "' group by c.account_name";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Income_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        string subgroup_name = objOdbcDataReader["accountgroup_name"].ToString();
                        string accountgroup_gid = objOdbcDataReader["accountgroup_gid"].ToString();

                        msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                        string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);
                        if (MainGroup_name == "$")
                        {
                            MainGroup_name = subgroup_name;
                            subgroup_name = "-";
                        }
                        getModuleList.Add(new Income_list
                        {
                            journal_gid = dt["journal_gid"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            journal_year = dt["journal_year"].ToString(),
                            transaction_gid = dt["transaction_gid"].ToString(),
                            transaction_amount = dt["transaction_amount"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            subgroup_name = subgroup_name,
                            MainGroup_name = MainGroup_name,
                        });
                        values.Income_list = getModuleList;
                    }
                    objOdbcDataReader.Close();
                }
            }
        }
        public void DaGetExpenseSummary(string finyear, MdlBSIEReports values)
        {
            msSQL = "select a.journal_gid,a.transaction_date,a.branch_gid,a.journal_year,a.transaction_gid, " +
                    " format(sum(b.transaction_amount),2) as transaction_amount,b.account_gid,c.account_name,c.accountgroup_gid,c.accountgroup_name " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tchartofaccount c on c.account_gid = b.account_gid " +
                    " where c.ledger_type='Y' and c.display_type='N' and a.journal_year='" + finyear + "' group by c.account_name";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Expense_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        string subgroup_name = objOdbcDataReader["accountgroup_name"].ToString();
                        string accountgroup_gid = objOdbcDataReader["accountgroup_gid"].ToString();

                        msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                        string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);
                        if (MainGroup_name == "$")
                        {
                            MainGroup_name = subgroup_name;
                            subgroup_name = "-";
                        }
                        getModuleList.Add(new Expense_list
                        {
                            journal_gid = dt["journal_gid"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            journal_year = dt["journal_year"].ToString(),
                            transaction_gid = dt["transaction_gid"].ToString(),
                            transaction_amount = dt["transaction_amount"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            subgroup_name = subgroup_name,
                            MainGroup_name = MainGroup_name,
                        });
                        values.Expense_list = getModuleList;
                    }
                    objOdbcDataReader.Close();
                }
            }
        }
    }
}