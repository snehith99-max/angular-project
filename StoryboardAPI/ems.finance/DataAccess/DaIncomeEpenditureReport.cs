using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Globalization;

namespace ems.finance.DataAccess
{
    public class DaIncomeEpenditureReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsbankgid, lsbank_gid,lsmonth,lsyear, from,to, lsemployee_gid, lsbranch_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGVcreditNeedDataSource(string branch, string from_date, string to_date, MdlIncomeEpenditureReport values)
        {
            try
            {
                if (from_date != "null")
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != "null")
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                string msSQL = "SELECT CAST(SUM(a.transaction_amount) AS DECIMAL) AS debit_amount, " +
                               "DATE_FORMAT(a.transaction_date, '%M') AS month, YEAR(a.transaction_date) AS year " +
                               "FROM acc_trn_vjournaltransactions a WHERE a.ledger_type = 'I' ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += "AND MONTH(a.transaction_date) >= MONTH(DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH) ";
                }
                else
                {
                    msSQL += "AND a.transaction_date >= '" + fromdate + "' AND a.transaction_date <= '" + todate + "' ";
                }

                msSQL += "AND a.branch_gid IN (" + branch + ")";
                msSQL += " GROUP BY YEAR(a.transaction_date), MONTHNAME(a.transaction_date) ORDER BY a.transaction_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GVcreditNeedDataSource_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GVcreditNeedDataSource_list
                        {
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),


                        });
                        values.GVcreditNeedDataSource_list = getModuleList;
                    }
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
        public void DaGVcreditDetailTable(string branch, string from_date, string to_date, string month, string year, MdlIncomeEpenditureReport values)
        {
            try
            {
                if (from_date != "null")
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != "null")
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                msSQL = "SELECT DATE_FORMAT(transaction_date, '%d-%m-%Y') AS  transaction_date, journal_refno, reference_type, account_name, remarks, CAST(transaction_amount AS DECIMAL) AS transaction_amount, " +
                                     "journal_type, journaldtl_gid, journal_gid, branch_gid FROM acc_trn_vjournaltransactions WHERE ledger_type = 'I' " +
                                     "AND DATE_FORMAT(transaction_date, '%M') = '" + month + "' AND YEAR(transaction_date) = '" + year + "' ";
                if (txtfrom_date == null && txtto_date == null)
                {
                    // msSQL += " AND transaction_date >= DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH";
                }
                else
                {
                    msSQL += " AND transaction_date >= '" + fromdate + "' AND transaction_date <= '" + todate + "' ";
                }
                msSQL += " AND branch_gid IN (" + branch + ")";

                msSQL += " ORDER BY transaction_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GVcreditDetailTable_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GVcreditDetailTable_list
                        {
                            transaction_date = dt["transaction_date"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            reference_type = dt["reference_type"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            transaction_amount = FormatNumber(dt["transaction_amount"].ToString()),
                            journal_type = dt["journal_type"].ToString(),


                        });
                        values.GVcreditDetailTable_list = getModuleList;
                    }
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
        private string TruncateRemarks(string remarks, int maxLength)
        {
            return (remarks.Length > maxLength) ? remarks.Substring(0, maxLength) + "..." : remarks;
        }

        public void DaGetIncomeReport(string branch, string from_date, string to_date, MdlIncomeEpenditureReport values)
        {

            try
            {

                if (from_date != null)
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != null)
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                StringBuilder stringBuilder = new StringBuilder();
                msSQL = " select cast(sum(a.transaction_amount) as DECIMAL) as debit_amount, " +
                           " date_format(a.transaction_date,'%M') as month,year(a.transaction_date) as year " +
                           " from acc_trn_vjournaltransactions a  where a.ledger_type='I'  ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += "  and month(a.transaction_date)>=month(DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH) ";
                }
                else
                {
                   
                    msSQL += " and a.transaction_date>='" + fromdate + "' and a.transaction_date<='" + todate + "' ";
                }

                msSQL += " and a.branch_gid in (" + branch + ")";
                msSQL += " group by year(a.transaction_date),monthname(a.transaction_date) order by a.transaction_date desc ";
                System.Data.DataTable objtb1 = objdbconn.GetDataTable(msSQL);
                if (objtb1.Rows.Count > 0)
                {
                    //stringBuilder.Append("<table width='100%' style='margin-bottom: 20px;'><tr><td align='center' style='color:maroon; font-weight:bold;' colspan='5'><font color='blue'><b>INCOME</b></font></td></tr></table>");

                    foreach (DataRow objtb1row in objtb1.Rows)
                    {
                        lsyear = objtb1row["year"].ToString();
                        lsmonth = objtb1row["month"].ToString();
                        stringBuilder.Append("<table width='100%' align='center'>");
                        stringBuilder.Append("<tr  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#4E7DB6'>");
                        stringBuilder.Append("<th width='20%' align='center'>Year</th>");
                        stringBuilder.Append("<th width='20%' align='center'>Month</th>");
                        stringBuilder.Append("<th width='30%' align='center'>Amount</th>");
                        stringBuilder.Append("</tr>");
                        stringBuilder.Append("<tr  style='font-family:Arial; font-size:12px;font-weight: bold;'>");
                        stringBuilder.Append("<td align='center'>" + objtb1row["year"] + "</td>");
                        stringBuilder.Append("<td align='center'>" + objtb1row["month"] + "</td>");
                        stringBuilder.Append("<td align='right'>" + FormatNumber(objtb1row["debit_amount"]) + "</td>");
                        stringBuilder.Append("</tr>");
                        stringBuilder.Append("</table>");
                        msSQL = "select DATE_FORMAT(transaction_date, '%d/%m/%Y') as transaction_date, journal_refno, reference_type, account_name, remarks, cast(transaction_amount as DECIMAL) as transaction_amount, " +
                                         "journal_type, journaldtl_gid, journal_gid, branch_gid " +
                        "from acc_trn_vjournaltransactions " +
                        "where ledger_type='I' " +
                                         $"and date_format(transaction_date,'%M')='{lsmonth}' and year(transaction_date)='{lsyear}' ";

                        if (txtfrom_date == null && txtto_date == null)
                        {
                            // msSQL += " and transaction_date >= DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH";
                        }
                        else
                        {
                          
                            msSQL += $" and transaction_date>='{fromdate}' and transaction_date<='{todate}' ";
                        }

                        msSQL += " and branch_gid in (" + branch + ")";
                        msSQL += " order by transaction_date desc ";
                        System.Data.DataTable objtb11 = objdbconn.GetDataTable(msSQL);
                        if (objtb11.Rows.Count > 0)
                        {
                            int rowCount = objtb11.Rows.Count;
                            for (int i = 0; i < rowCount; i++)
                            {
                                DataRow objtb1row1 = objtb11.Rows[i];
                                if (i == 0)
                                {
                                    stringBuilder.Append("<table width='100%' align='center'>");
                                    stringBuilder.Append("<tr>");
                                    stringBuilder.Append("<th  width='20%' align='center'>  </th>");
                                    stringBuilder.Append("<th style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4' width='20%' align='center'>Date</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='20%' align='center'>Journal Ref.No</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Account Name</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Reference</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Journal Type</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Remarks</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Amount</th>");
                                    stringBuilder.Append("</tr>");
                                }

                                stringBuilder.Append("<tr  style='font-family:Arial; font-size:12px;font-weight: bold;'>");
                                stringBuilder.Append("<td align='center'>   </td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["transaction_date"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["journal_refno"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["account_name"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + TruncateRemarks(objtb1row1["reference_type"].ToString(), 1020) + "</td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["journal_type"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + TruncateRemarks(objtb1row1["remarks"].ToString(), 1020) + "</td>"); // Truncate remarks if too long
                                stringBuilder.Append("<td align='right'>" + FormatNumber(objtb1row1["transaction_amount"]) + "</td>");
                                stringBuilder.Append("</tr>");
                            }
                            stringBuilder.Append("</table>");

                        }
                    }



                }
                string htmlcode = stringBuilder.ToString();
                values.GetIncomeExcel_list = new List<GetIncomeExcel_list>
      {
          new GetIncomeExcel_list
          {
              html_content = htmlcode.ToString(),
          }
      };
            }
            catch (Exception ex)
            {
                // Log error details
                string errorMessage = $"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} - {ex.Message} - SQL: {msSQL} - ApiRef";
                objcmnfunctions.LogForAudit(errorMessage, "ErrorLog/FinanceLog" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGVdebitNeedDataSource(string branch, string from_date, string to_date, MdlIncomeEpenditureReport values)
        {

            try
            {


                if (from_date != "null")
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != "null")
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                string msSQL = "SELECT CAST(SUM(a.transaction_amount) AS DECIMAL) AS debit_amount, " +
                               "DATE_FORMAT(a.transaction_date, '%M') AS month, YEAR(a.transaction_date) AS year " +
                               "FROM acc_trn_vjournaltransactions a WHERE a.ledger_type = 'E' ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += "AND MONTH(a.transaction_date) >= MONTH(DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH) ";
                }
                else
                {
                    msSQL += "AND a.transaction_date >= '" + fromdate + "' AND a.transaction_date <= '" + todate + "' ";
                }

                msSQL += "AND a.branch_gid IN (" + branch + ")";
                msSQL += " GROUP BY YEAR(a.transaction_date), MONTHNAME(a.transaction_date) ORDER BY a.transaction_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GVdebitNeedDataSource_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GVdebitNeedDataSource_list
                        {
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),


                        });
                        values.GVdebitNeedDataSource_list = getModuleList;
                    }
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
        public void DaGVdebitDetailTable(string branch, string from_date, string to_date, string month, string year, MdlIncomeEpenditureReport values)
        {
            try
            {
                if (from_date != "null")
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != "null")
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                msSQL = "SELECT DATE_FORMAT(transaction_date, '%d-%m-%Y') AS  transaction_date, journal_refno, reference_type, account_name, remarks, CAST(transaction_amount AS DECIMAL) AS transaction_amount, " +
                                         "journal_type, journaldtl_gid, journal_gid, branch_gid FROM acc_trn_vjournaltransactions WHERE ledger_type = 'E' " +
                                         "AND DATE_FORMAT(transaction_date, '%M') = '" + month + "' AND YEAR(transaction_date) = '" + year + "' ";
                if (txtfrom_date == null && txtto_date == null)
                {
                    // msSQL += " AND transaction_date >= DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH";
                }
                else
                {
                    msSQL += " AND transaction_date >= '" + fromdate + "' AND transaction_date <= '" + todate + "' ";
                }
                msSQL += " AND branch_gid IN (" + branch + ")";

                msSQL += " ORDER BY transaction_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GVdebitDetailTable_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GVdebitDetailTable_list
                        {
                            transaction_date = dt["transaction_date"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            reference_type = dt["reference_type"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            transaction_amount = FormatNumber(dt["transaction_amount"].ToString()),
                            journal_type = dt["journal_type"].ToString(),


                        });
                        values.GVdebitDetailTable_list = getModuleList;
                    }
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
        public void DaGetExpenseReport(string branch, string from_date, string to_date, MdlIncomeEpenditureReport values)
        {

            try
            {

                if (from_date != null)
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != null)
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                StringBuilder stringBuilder = new StringBuilder();
                msSQL = " select cast(sum(a.transaction_amount) as DECIMAL) as debit_amount, " +
                           " date_format(a.transaction_date,'%M') as month,year(a.transaction_date) as year " +
                           " from acc_trn_vjournaltransactions a  where a.ledger_type='E'  ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += "  and month(a.transaction_date)>=month(DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH) ";
                }
                else
                {
                   
                    msSQL += " and a.transaction_date>='" + fromdate + "' and a.transaction_date<='" + todate + "' ";
                }

                msSQL += " and a.branch_gid in (" + branch + ")";
                msSQL += " group by year(a.transaction_date),monthname(a.transaction_date) order by a.transaction_date desc ";
                System.Data.DataTable objtb1 = objdbconn.GetDataTable(msSQL);
                if (objtb1.Rows.Count > 0)
                {
                    //stringBuilder.Append("<table width='100%' style='margin-bottom: 20px;'><tr><td align='center' style='color:maroon; font-weight:bold;' colspan='5'><font color='blue'><b>INCOME</b></font></td></tr></table>");

                    foreach (DataRow objtb1row in objtb1.Rows)
                    {
                        lsyear = objtb1row["year"].ToString();
                        lsmonth = objtb1row["month"].ToString();
                        stringBuilder.Append("<table width='100%' align='center'>");
                        stringBuilder.Append("<tr  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#4E7DB6'>");
                        stringBuilder.Append("<th width='20%' align='center'>Year</th>");
                        stringBuilder.Append("<th width='20%' align='center'>Month</th>");
                        stringBuilder.Append("<th width='30%' align='center'>Amount</th>");
                        stringBuilder.Append("</tr>");
                        stringBuilder.Append("<tr  style='font-family:Arial; font-size:12px;font-weight: bold;'>");
                        stringBuilder.Append("<td align='center'>" + objtb1row["year"] + "</td>");
                        stringBuilder.Append("<td align='center'>" + objtb1row["month"] + "</td>");
                        stringBuilder.Append("<td align='right'>" + FormatNumber(objtb1row["debit_amount"]) + "</td>");
                        stringBuilder.Append("</tr>");
                        stringBuilder.Append("</table>");
                        msSQL = "select DATE_FORMAT(transaction_date, '%d/%m/%Y') as transaction_date, journal_refno, reference_type, account_name, remarks, cast(transaction_amount as DECIMAL) as transaction_amount, " +
                                         "journal_type, journaldtl_gid, journal_gid, branch_gid " +
                        "from acc_trn_vjournaltransactions " +
                        "where ledger_type='E' " +
                                         $"and date_format(transaction_date,'%M')='{lsmonth}' and year(transaction_date)='{lsyear}' ";

                        if (txtfrom_date == null && txtto_date == null)
                        {
                            // msSQL += " and transaction_date >= DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH";
                        }
                        else
                        {
                           
                            msSQL += $" and transaction_date>='{fromdate}' and transaction_date<='{todate}' ";
                        }

                        msSQL += " and branch_gid in (" + branch + ")";
                        msSQL += " order by transaction_date desc ";
                        System.Data.DataTable objtb11 = objdbconn.GetDataTable(msSQL);
                        if (objtb11.Rows.Count > 0)
                        {
                            int rowCount = objtb11.Rows.Count;
                            for (int i = 0; i < rowCount; i++)
                            {
                                DataRow objtb1row1 = objtb11.Rows[i];
                                if (i == 0)
                                {
                                    stringBuilder.Append("<table width='100%' align='center'>");
                                    stringBuilder.Append("<tr>");
                                    stringBuilder.Append("<th  width='20%' align='center'>  </th>");
                                    stringBuilder.Append("<th style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4' width='20%' align='center'>Date</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='20%' align='center'>Journal Ref.No</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Account Name</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Reference</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Journal Type</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Remarks</th>");
                                    stringBuilder.Append("<th  style='font-family:Arial; font-size:12px; color:white;' bgcolor='#1dcde4'  width='30%' align='center'>Amount</th>");
                                    stringBuilder.Append("</tr>");
                                }

                                stringBuilder.Append("<tr  style='font-family:Arial; font-size:12px;font-weight: bold;'>");
                                stringBuilder.Append("<td align='center'>   </td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["transaction_date"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["journal_refno"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["account_name"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + TruncateRemarks(objtb1row1["reference_type"].ToString(), 1020) + "</td>");
                                stringBuilder.Append("<td align='center'>" + objtb1row1["journal_type"] + "</td>");
                                stringBuilder.Append("<td align='center'>" + TruncateRemarks(objtb1row1["remarks"].ToString(), 1020) + "</td>"); // Truncate remarks if too long
                                stringBuilder.Append("<td align='right'>" + FormatNumber(objtb1row1["transaction_amount"]) + "</td>");
                                stringBuilder.Append("</tr>");
                            }
                            stringBuilder.Append("</table>");

                        }
                    }



                }
                string htmlcode = stringBuilder.ToString();
                values.GetExpenseExcel_list = new List<GetExpenseExcel_list>
      {
          new GetExpenseExcel_list
          {
              html_content = htmlcode.ToString(),
          }
      };
            }
            catch (Exception ex)
            {
                // Log error details
                string errorMessage = $"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} - {ex.Message} - SQL: {msSQL} - ApiRef";
                objcmnfunctions.LogForAudit(errorMessage, "ErrorLog/FinanceLog" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBarChartIncomeexpene(string branch, string from_date, string to_date, MdlIncomeEpenditureReport values)
        {
            try
            {

                if (from_date != "null")
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != "null")
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                string msSQL = "SELECT SUM(a.transaction_amount) AS expense_amount, (SELECT SUM(x.transaction_amount) AS income_amount FROM acc_trn_vjournaltransactions x WHERE x.ledger_type='I' ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += " AND MONTHNAME(x.transaction_date) = MONTHNAME(a.transaction_date) AND YEAR(x.transaction_date) = YEAR(a.transaction_date) ";
                }
                else
                {
                    msSQL += $" AND x.transaction_date >= '{fromdate}' AND x.transaction_date <= '{todate}' ";
                }
                msSQL += $" AND x.branch_gid IN  (" + branch + ")";
           msSQL += $") AS income_amount, CAST(CONCAT(SUBSTRING(DATE_FORMAT(a.transaction_date, '%M'), 1, 3), ' ', YEAR(a.transaction_date)) AS CHAR) AS month_name " +
             "FROM acc_trn_vjournaltransactions a WHERE a.ledger_type='E' ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += " AND a.transaction_date >= DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH";
                }
                else
                {
                    msSQL += $" AND a.transaction_date >= '{fromdate}' AND a.transaction_date <= '{todate}' ";
                }
                msSQL += $" AND a.branch_gid IN  (" + branch + ")";
           msSQL += " GROUP BY YEAR(a.transaction_date), MONTHNAME(a.transaction_date) ORDER BY a.transaction_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBarChartIncomeexpene_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBarChartIncomeexpene_list
                        {

                            expense_amount = dt["expense_amount"].ToString(),
                            income_amount = dt["income_amount"].ToString(),
                            month_name = dt["month_name"].ToString(),



                        });
                        values.GetBarChartIncomeexpene_list = getModuleList;
                    }
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
        public void DaGVPopTransaction(string branch, string from_date, string to_date, MdlIncomeEpenditureReport values)
        {
            try
            {
                if (from_date != "null")
                {
                    DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    from = originalDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    from = null;
                }
                if (from_date != "null")
                {
                    DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    // Convert the date to the desired format
                    to = originalDate1.ToString("yyyy-MM-dd");
                }
                else
                {
                    to = null;
                }
                string txtfrom_date = from;
                string txtto_date = to;
                string fromdate = txtfrom_date;
                string todate = txtto_date;
                msSQL = "SELECT FORMAT(expense_amount, 2) AS expense_amount, FORMAT(income_amount, 2) AS income_amount, " +
                              "month, year, FORMAT((income_amount - expense_amount), 2) AS net_amount " +
                              "FROM ( " +
                              "SELECT SUM(a.transaction_amount) AS expense_amount, " +
                              "(SELECT SUM(x.transaction_amount) FROM acc_trn_vjournaltransactions x " +
                              "WHERE x.ledger_type = 'I' ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += "AND MONTHNAME(x.transaction_date) = MONTHNAME(a.transaction_date) AND YEAR(x.transaction_date) = YEAR(a.transaction_date) ";
                }
                else
                {
                    msSQL += "AND x.transaction_date >= '" + fromdate + "' AND x.transaction_date <= '" + todate + "' ";
                }

                msSQL += ") AS income_amount, " +
                         "SUBSTRING(DATE_FORMAT(a.transaction_date, '%M'), 1, 3) AS month, YEAR(a.transaction_date) AS year " +
                         "FROM acc_trn_vjournaltransactions a " +
                         "WHERE a.ledger_type = 'E' ";

                if (txtfrom_date == null && txtto_date == null)
                {
                    msSQL += "AND MONTH(a.transaction_date) >= MONTH(DATE_FORMAT(CURDATE(), '%Y-%m-%d') - INTERVAL 6 MONTH) ";
                }
                else
                {
                    msSQL += "AND a.transaction_date >= '" + fromdate + "' AND a.transaction_date <= '" + todate + "' ";
                }
                msSQL += "AND a.branch_gid IN (" + branch + ") ";
                msSQL += "GROUP BY YEAR(a.transaction_date), MONTHNAME(a.transaction_date) " +
                         "ORDER BY a.transaction_date DESC) x";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GVPoptransaction_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GVPoptransaction_list
                        {
                            year = dt["year"].ToString(),
                            month = dt["month"].ToString(),
                            income_amount = FormatNumber(dt["income_amount"].ToString()),
                            expense_amount = FormatNumber(dt["expense_amount"].ToString()),
                            net_amount = FormatNumber(dt["net_amount"].ToString()),


                        });
                        values.GVPoptransaction_list = getModuleList;
                    }
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


    }
}