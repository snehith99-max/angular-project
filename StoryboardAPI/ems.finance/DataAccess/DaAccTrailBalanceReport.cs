using ems.finance.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using OfficeOpenXml.Style;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Text.RegularExpressions;

namespace ems.finance.DataAccess
{
    public class DaAccTrailBalanceReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL, msSQL12, msSQL13, msSQL14, msGetDlGID = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msGetGid, lsyear, year;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetTrialBalanceTransactionDtls(string account_gid, MdlAccTrailBalanceReport values)
        {
            msSQL = " select a.journal_gid,a.transaction_date,a.journal_refno,a.remarks, " +
                    " cast((ifnull(case when a.transaction_type not like '%Opening%' " +
                    " and b.journal_type = 'cr' then b.transaction_amount end,0.00))as decimal)  as credit_amount, " +
                    " cast((ifnull(case when a.transaction_type not like '%Opening%'  and b.journal_type = 'dr' " +
                    " then b.transaction_amount end,0.00)) as decimal)as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where b.account_gid = '" + account_gid + "'" +
                    " and b.transaction_amount<>'0.00' and a.invoice_flag<>'R' " +
                    " order by a.transaction_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var TrialBalance_list = new List<GetTrialBalance_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    TrialBalance_list.Add(new GetTrialBalance_list
                    {
                        journal_gid = dt["journal_gid"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        debit_amount = dt["debit_amount"].ToString()
                    });
                    values.GetTrialBalance_list = TrialBalance_list;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetTrialBalanceSummary(string branch, string year_gid, MdlAccTrailBalanceReport values)
        {
            try
            {
                msSQL = "  SELECT cast(date_format(fyear_start,'%Y-%m-%d')as char) as fyear_start,cast(if(fyear_end is null,curdate(),fyear_end) as char) as finyear_end   from adm_mst_tyearendactivities where finyear_gid='" + year_gid + "' limit 1 ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows)
                {
                    year = objODBCDatareader["fyear_start"].ToString();
                    lsyear = objODBCDatareader["finyear_end"].ToString();
                }
                objODBCDatareader.Close();

                msSQL = " Select c.accountgroup_gid, upper(c.accountgroup_name) as accountgroup_name, CAST(SUM(IFNULL(CASE WHEN " +
                        " a.journal_type = 'cr' THEN a.transaction_amount END, 0.00)) AS decimal) " +
                        " AS credit_amount, CAST(SUM(IFNULL(CASE WHEN a.journal_type = 'dr' THEN a.transaction_amount " +
                        " END, 0.00)) AS decimal) AS debit_amount " +
                        " FROM acc_trn_journalentrydtl a " +
                        " left join acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                        " left join acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                        " where (transaction_amount<> 0.00) AND b.branch_gid = '" + branch + "' AND " +
                        " b.transaction_date " +
                        " BETWEEN '" + year + "' AND '" + lsyear + "' group by c.accountgroup_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TrialBalanceFolders>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TrialBalanceFolders
                        {
                            account_name = dt["accountgroup_name"].ToString(),
                            credit_amount = FormatNumber(dt["credit_amount"].ToString()),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),
                            account_gid = dt["accountgroup_gid"].ToString(),
                        });
                        values.parent_folders = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                var getsub_folders = new List<TrialBalanceFolders>();

                msSQL = " Select c.account_gid,c.accountgroup_gid, upper(c.account_name) as account_name, " +
                        " CAST(SUM(IFNULL(CASE WHEN a.journal_type = 'cr' THEN a.transaction_amount END, 0.00)) AS decimal) AS credit_amount, " +
                        " CAST(SUM(IFNULL(CASE WHEN a.journal_type = 'dr' THEN a.transaction_amount END, 0.00)) AS decimal) AS debit_amount " +
                        " from acc_trn_journalentrydtl a " +
                        " left join acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                        " left join acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                        " WHERE(transaction_amount<> 0.00)  AND b.branch_gid = '" + branch + "' AND " +
                        " b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY c.account_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getsub_folders.Add(new TrialBalanceFolders
                        {
                            account_name = dt["account_name"].ToString(),
                            credit_amount = FormatNumber(dt["credit_amount"].ToString()),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                        });
                    }
                    values.sub_folders1 = getsub_folders;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                // Log error details
                string errorMessage = $"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} - {ex.Message} - SQL: {msSQL} - ApiRef";
                objcmnfunctions.LogForAudit(errorMessage, "ErrorLog/FinanceLog" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        private string FormatNumber(object value)
        {
            if (value != DBNull.Value)
            {
                decimal balance;
                if (decimal.TryParse(value.ToString(), out balance))
                {
                    return balance.ToString("N2"); // Formats the decimal value with two decimal places and comma separators
                }
                // Handle parsing error if necessary
            }
            // Handle DBNull.Value or other non-numeric values if necessary
            return string.Empty; // Return empty string if value is null or not convertible to decimal
        }
    }
}
