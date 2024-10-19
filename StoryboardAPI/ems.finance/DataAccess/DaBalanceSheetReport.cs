using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net;
using System.EnterpriseServices.CompensatingResourceManager;
//using ems.finance.Data;
using System.Configuration;

namespace ems.finance.DataAccess
{
    public class DaBalanceSheetReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        //Downloadpdf wordtopdffile = new Downloadpdf();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, income_closebalance, year, lsyear, lblpandlvalue, lblpandl,lsyear_end, lsfyear_start, lsfyear_end, expense_closebalance, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public StringBuilder strbuild = new StringBuilder();
        public void DaGetBalanceSheetLiability(string branch, string year_gid, MdlBalanceSheetReport values)
        {
            try
            {
                StringBuilder stringbuilder = new StringBuilder();

                msSQL = "  SELECT cast(date_format(fyear_start,'%Y-%m-%d')as char) as fyear_start,cast(if(fyear_end is null,curdate(),fyear_end) as char) as finyear_end   from adm_mst_tyearendactivities where finyear_gid='" + year_gid + "' limit 1 ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows)
                {
                    year = objODBCDatareader["fyear_start"].ToString();
                    lsyear = objODBCDatareader["finyear_end"].ToString();
                }
                objODBCDatareader.Close();

                stringbuilder.Append("<table width='100%'><tr><td width='100%' align='left' style='color:maroon;font-weight:bold' >Liability</td></tr></table>");
                
                msSQL = "SELECT accountgroup_gid, accountgroup_name, credit_amount, debit_amount, opening_balance, ((opening_balance + credit_amount) - debit_amount) AS closing_balance " +
                        "FROM (SELECT c.accountgroup_gid, c.accountgroup_name, " +
                        "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, " +
                        "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, " +
                        "CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance " +
                        "FROM acc_trn_journalentrydtl a " +
                        "LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                        "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                        "WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'N') ";
                msSQL += " AND b.branch_gid='" + branch + "' ";
                msSQL += " AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY c.accountgroup_gid) x ";
                DataTable objtb1 = objdbconn.GetDataTable(msSQL);

                if (objtb1.Rows.Count > 0)
                {
                    foreach (DataRow objtb1row in objtb1.Rows)
                    {
                        stringbuilder.Append("<table width='100%' align='center'><tr style='font-family:Arial; font-size:14px; color:white;' bgcolor='#4E7DB6'>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='23%'>Accountgroup Name</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='18%'>Opening Balance</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='20%'>Credit</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='20%'>Debit</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='19%'>Closing Balance</td></tr>");

                        stringbuilder.Append("<tr><td style='font-size:small;font-weight:bold;color:black;' align='left'>" + objtb1row["accountgroup_name"] + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["opening_balance"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["credit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["debit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["closing_balance"]) + "</td></tr></table>");

                        stringbuilder.Append("<table width='99%' align='center'><tr style='font-family:Arial; font-size:14px; color:white;' bgcolor='#4E7DB6'>");
                        stringbuilder.Append("<td width='23%'></td>");
                        stringbuilder.Append("<td width='18%'></td>");
                        stringbuilder.Append("<td width='20%'></td>");
                        stringbuilder.Append("<td width='20%'></td>");
                        stringbuilder.Append("<td width='19%'></td></tr>");

                        msSQL = "SELECT account_gid, account_name, credit_amount, debit_amount, opening_balance, ((opening_balance + credit_amount) - debit_amount) AS closing_balance " +
                                "FROM (SELECT c.account_gid, c.account_name, " +
                                "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, " +
                                "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, " +
                                "CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance " +
                                "FROM acc_trn_journalentrydtl a " +
                                "LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                                "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                                "WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'N') " +
                                "AND c.accountgroup_gid='" + objtb1row["accountgroup_gid"] + "' ";
                        msSQL += " AND b.branch_gid='" + branch + "' ";
                        msSQL += " AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY c.account_gid) x ";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDatareader.HasRows)
                        {
                            while (objODBCDatareader.Read())
                            {
                                stringbuilder.Append("<tr><td style='font-size:small;font-weight:bold;color:black;' align='left'>");
                                //stringbuilder.Append($"<a  class=\"button-link\"  [routerLink]=\"['/finance/AccRptProfitandLostDetails', '{objODBCDatareader["account_gid"]}', 'PL']\">{objODBCDatareader["account_name"]}</a>");
                                stringbuilder.Append($"<a class=\"button-link\" type=\"button\" data-path=\"/finance/AccRptProfitandLostDetails\" data-param1=\"{objODBCDatareader["account_gid"]}\" data-param2=\"BS\">{objODBCDatareader["account_name"]}</a></td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["opening_balance"]) + "</td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["credit_amount"]) + "</td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["debit_amount"]) + "</td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["closing_balance"]) + "</td></tr>");
                            }
                        }
                        objODBCDatareader.Close();
                        stringbuilder.Append("</table>");
                    }

                    stringbuilder.Append("<table width='99%' align='center'><tr style='font-family:Arial; font-size:14px; color:maroon;font-weight:bold'>");
                    stringbuilder.Append("<td width='23%'></td>");
                    stringbuilder.Append("<td width='18%'></td>");
                    stringbuilder.Append("<td width='20%'></td>");
                    stringbuilder.Append("<td width='20%'></td>");
                    stringbuilder.Append("<td width='19%'></td></tr>");

                    msSQL = "SELECT accountgroup_gid, accountgroup_name, credit_amount, debit_amount, opening_balance, ((opening_balance + credit_amount) - debit_amount) AS closing_balance " +
                            "FROM (SELECT c.accountgroup_gid, c.accountgroup_name, " +
                            "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, " +
                            "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, " +
                            "CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance " +
                            "FROM acc_trn_journalentrydtl a " +
                            "LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                            "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                            "WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'N') ";
                    msSQL += " AND b.branch_gid='" + branch + "' ";
                    msSQL += " AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY (c.ledger_type = 'N' AND display_type = 'N') ) x ";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows)
                    {
                        stringbuilder.Append("<tr><td style='font-family:Arial; font-size:14px; color:maroon;font-weight:bold' align='center' width='23%'>TOTAL</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='18%'>" + FormatNumber(objODBCDatareader["opening_balance"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='20%'>" + FormatNumber(objODBCDatareader["credit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='20%'>" + FormatNumber(objODBCDatareader["debit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='19%'>" + FormatNumber(objODBCDatareader["closing_balance"]) + "</td></tr>");
                        expense_closebalance = objODBCDatareader["closing_balance"].ToString();
                    }

                    objODBCDatareader.Close();
                    stringbuilder.Append("</table>");
                    var getModuleList = new List<BalanceSheetliability_list>();
                    getModuleList.Add(new BalanceSheetliability_list
                    {
                        html_content = stringbuilder.ToString(),
                        expense_closebal = expense_closebalance.ToString(),
                    });
                    values.BalanceSheetliability_list = getModuleList;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetBalanceSheetAsset(string branch, string year_gid, MdlBalanceSheetReport values)
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

                StringBuilder stringbuilder = new StringBuilder();

                stringbuilder.Append("<table width='100%'><tr><td width='100%' align='left' style='color:maroon;font-weight:bold' >Asset</td></tr></table>");
                
                msSQL = "SELECT accountgroup_gid, accountgroup_name, credit_amount, debit_amount, opening_balance, ((opening_balance + credit_amount) - debit_amount) AS closing_balance " +
                        "FROM (SELECT c.accountgroup_gid, c.accountgroup_name, " +
                        "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, " +
                        "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, " +
                        "CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance " +
                        "FROM acc_trn_journalentrydtl a " +
                        "LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                        "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                        "WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'Y') ";
                msSQL += " AND b.branch_gid='" + branch + "' ";
                msSQL += " AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY c.accountgroup_gid) x ";
                DataTable objtb1 = objdbconn.GetDataTable(msSQL);

                if (objtb1.Rows.Count > 0)
                {
                    foreach (DataRow objtb1row in objtb1.Rows)
                    {
                        stringbuilder.Append("<table width='100%' align='center'><tr style='font-family:Arial; font-size:14px; color:white;' bgcolor='#4E7DB6'>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='23%'>Accountgroup Name</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='18%'>Opening Balance</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='20%'>Credit</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='20%'>Debit</td>");
                        stringbuilder.Append("<td style='font-size:16px;font-family:calibri;' align='center' width='19%'>Closing Balance</td></tr>");

                        stringbuilder.Append("<tr><td style='font-size:small;font-weight:bold;color:black;' align='left'>" + objtb1row["accountgroup_name"] + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["opening_balance"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["credit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["debit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objtb1row["closing_balance"]) + "</td></tr></table>");

                        stringbuilder.Append("<table width='99%' align='center'><tr style='font-family:Arial; font-size:14px; color:white;' bgcolor='#4E7DB6'>");
                        stringbuilder.Append("<td width='23%'></td>");
                        stringbuilder.Append("<td width='18%'></td>");
                        stringbuilder.Append("<td width='20%'></td>");
                        stringbuilder.Append("<td width='20%'></td>");
                        stringbuilder.Append("<td width='19%'></td></tr>");

                        msSQL = "SELECT account_gid, account_name, credit_amount, debit_amount, opening_balance, ((opening_balance + credit_amount) - debit_amount) AS closing_balance " +
                                "FROM (SELECT c.account_gid, c.account_name, " +
                                "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, " +
                                "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, " +
                                "CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance " +
                                "FROM acc_trn_journalentrydtl a " +
                                "LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                                "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                                "WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'Y') " +
                                "AND c.accountgroup_gid='" + objtb1row["accountgroup_gid"] + "' ";
                        msSQL += " AND b.branch_gid='" + branch + "' ";
                        msSQL += " AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY c.account_gid) x ";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDatareader.HasRows)
                        {
                            while (objODBCDatareader.Read())
                            {
                                stringbuilder.Append("<tr><td style='font-size:small;font-weight:bold;color:black;' align='left'>");
                                //stringbuilder.Append($"<a  class=\"button-link\"  [routerLink]=\"['/finance/AccRptProfitandLostDetails', '{objODBCDatareader["account_gid"]}', 'PL']\">{objODBCDatareader["account_name"]}</a>");
                                stringbuilder.Append($"<a class=\"button-link\" type=\"button\" data-path=\"/finance/AccRptProfitandLostDetails\" data-param1=\"{objODBCDatareader["account_gid"]}\" data-param2=\"BS\">{objODBCDatareader["account_name"]}</a></td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["opening_balance"]) + "</td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["credit_amount"]) + "</td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["debit_amount"]) + "</td>");
                                stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right'>" + FormatNumber(objODBCDatareader["closing_balance"]) + "</td></tr>");
                            }
                        }
                        objODBCDatareader.Close();
                        stringbuilder.Append("</table>");
                    }

                    stringbuilder.Append("<table width='99%' align='center'><tr style='font-family:Arial; font-size:14px; color:maroon;font-weight:bold'>");
                    stringbuilder.Append("<td width='23%'></td>");
                    stringbuilder.Append("<td width='18%'></td>");
                    stringbuilder.Append("<td width='20%'></td>");
                    stringbuilder.Append("<td width='20%'></td>");
                    stringbuilder.Append("<td width='19%'></td></tr>");

                    msSQL = "SELECT accountgroup_gid, accountgroup_name, credit_amount, debit_amount, opening_balance, ((opening_balance + credit_amount) - debit_amount) AS closing_balance " +
                            "FROM (SELECT c.accountgroup_gid, c.accountgroup_name, " +
                            "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, " +
                            "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, " +
                            "CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance " +
                            "FROM acc_trn_journalentrydtl a " +
                            "LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid " +
                            "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid " +
                            "WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'Y') ";
                    msSQL += " AND b.branch_gid='" + branch + "' ";
                    msSQL += " AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "' GROUP BY (c.ledger_type = 'N' AND display_type = 'Y') ) x ";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows)
                    {
                        stringbuilder.Append("<tr><td style='font-family:Arial; font-size:14px; color:maroon;font-weight:bold' align='center' width='23%'><b>TOTAL</b></td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='18%'>" + FormatNumber(objODBCDatareader["opening_balance"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='20%'>" + FormatNumber(objODBCDatareader["credit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='20%'>" + FormatNumber(objODBCDatareader["debit_amount"]) + "</td>");
                        stringbuilder.Append("<td style='font-size:small;font-weight:bold;color:black;' align='right' width='19%'>" + FormatNumber(objODBCDatareader["closing_balance"]) + "</td></tr>");
                        income_closebalance = objODBCDatareader["closing_balance"].ToString();
                    }

                    objODBCDatareader.Close();
                    stringbuilder.Append("</table>");
                    var getModuleList = new List<BalanceSheetasset_list>();
                    getModuleList.Add(new BalanceSheetasset_list
                    {
                        html_content = stringbuilder.ToString(),
                        income_closebal = income_closebalance.ToString(),
                    });
                    values.BalanceSheetasset_list = getModuleList;
                }
            }

            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
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
        public void DaGetSummaryLiability(string branch, string year_gid, MdlFinanceLiabilityFolders values)
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

                msSQL = "SELECT accountgroup_gid, accountgroup_name, credit_amount, debit_amount, opening_balance,  " +
                "((opening_balance+credit_amount)-debit_amount)  AS closing_balance FROM (SELECT c.accountgroup_gid, c.accountgroup_name,  " +
                "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal)  " +
                "AS credit_amount, CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END)  " +
                "AS decimal) AS debit_amount, CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal)  " +
                "AS opening_balance FROM acc_trn_journalentrydtl a LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid  " +
                "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid WHERE (transaction_amount <> 0.00) AND " +
                " (c.ledger_type = 'N' AND display_type = 'N')  AND b.branch_gid='" + branch + "'  AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "'  GROUP BY c.accountgroup_gid) x ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<LiabilityFolders>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new LiabilityFolders
                        {
                            account_name = dt["accountgroup_name"].ToString(),
                            credit_amount = FormatNumber(dt["credit_amount"].ToString()),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),
                            opening_balance = FormatNumber(dt["opening_balance"].ToString()),
                            closing_balance = FormatNumber(dt["closing_balance"].ToString()),
                            account_gid = dt["accountgroup_gid"].ToString(),
                        });
                        values.parentfoldersliability = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                var getsubfolders = new List<LiabilityFolders>();

                msSQL = " SELECT account_gid, account_name,accountgroup_gid, credit_amount, debit_amount, opening_balance, ((opening_balance+credit_amount)-debit_amount)  " +
                       " AS closing_balance FROM (SELECT c.account_gid, c.account_name,c.accountgroup_gid, CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' " +
                       " THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr'  " +
                       " THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance FROM acc_trn_journalentrydtl a " +
                       " LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'N' ) AND c.accountgroup_gid !='$'  " +
                       " AND b.branch_gid='" + branch + "'   AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "'  GROUP BY c.account_gid order by  c.account_gid asc) x  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getsubfolders.Add(new LiabilityFolders
                        {
                            account_name = dt["account_name"].ToString(),
                            credit_amount = FormatNumber(dt["credit_amount"].ToString()),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),
                            opening_balance = FormatNumber(dt["opening_balance"].ToString()),
                            closing_balance = FormatNumber(dt["closing_balance"].ToString()),
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                        });
                    }
                    values.subfolders3 = getsubfolders;
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
        public void DaGetSummaryAsset(string branch, string year_gid, MdlFinanceLiabilityFolders values)
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
                
                msSQL = "SELECT accountgroup_gid, accountgroup_name, credit_amount, debit_amount, opening_balance,  " +
                        "((opening_balance+debit_amount)-credit_amount)   AS closing_balance FROM (SELECT c.accountgroup_gid, c.accountgroup_name,  " +
                        "CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal)  " +
                        "AS credit_amount, CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END)  " +
                        "AS decimal) AS debit_amount, CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal)  " +
                        "AS opening_balance FROM acc_trn_journalentrydtl a LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid  " +
                        "LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid WHERE (transaction_amount <> 0.00) AND " +
                        " (c.ledger_type = 'N' AND display_type = 'Y')  AND b.branch_gid='" + branch + "'  AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "'  GROUP BY c.accountgroup_gid) x ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<AssetFolders>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new AssetFolders
                        {
                            account_name = dt["accountgroup_name"].ToString(),
                            credit_amount = FormatNumber(dt["credit_amount"].ToString()),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),
                            opening_balance = FormatNumber(dt["opening_balance"].ToString()),
                            closing_balance = FormatNumber(dt["closing_balance"].ToString()),
                            account_gid = dt["accountgroup_gid"].ToString(),
                        });
                        values.parentfoldersasset = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                var getsubfolders = new List<AssetFolders>();

                msSQL =    " SELECT account_gid, account_name,accountgroup_gid, credit_amount, debit_amount, opening_balance, ((opening_balance+debit_amount)-credit_amount) " +
                           " AS closing_balance FROM (SELECT c.account_gid, c.account_name,c.accountgroup_gid, CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' " +
                           " THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr'  " +
                           " THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance FROM acc_trn_journalentrydtl a " +
                           " LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid WHERE (transaction_amount <> 0.00) AND (c.ledger_type = 'N' AND display_type = 'Y' ) AND c.accountgroup_gid !='$'  " +
                           " AND b.branch_gid='" + branch + "'   AND b.transaction_date BETWEEN '" + year + "' AND '" + lsyear + "'  GROUP BY c.account_gid order by  c.account_gid asc) x  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getsubfolders.Add(new AssetFolders
                        {
                            account_name = dt["account_name"].ToString(),
                            credit_amount = FormatNumber(dt["credit_amount"].ToString()),
                            debit_amount = FormatNumber(dt["debit_amount"].ToString()),
                            opening_balance = FormatNumber(dt["opening_balance"].ToString()),
                            closing_balance = FormatNumber(dt["closing_balance"].ToString()),
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                        });
                    }
                    values.subfolders4 = getsubfolders;
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
        public void DaGetNetAmountDetails(string branch, string year_gid, MdlBalanceSheetReport values)
        {
            double expenseClosingBalance = 0.0;
            double incomeClosingBalance = 0.0;
            double incomeCredit = 0.0;
            double expenseAmount = 0.0;
            double incomeAmount = 0.0;
            msSQL = "  SELECT cast(date_format(fyear_start,'%Y-%m-%d')as char) as fyear_start,cast(if(fyear_end is null,curdate(),fyear_end) as char) as finyear_end   from adm_mst_tyearendactivities where finyear_gid='" + year_gid + "' limit 1 ";
            objODBCDatareader = objdbconn.GetDataReader(msSQL);
            if (objODBCDatareader.HasRows)
            {
                year = objODBCDatareader["fyear_start"].ToString();
                lsyear = objODBCDatareader["finyear_end"].ToString();

            }
            objODBCDatareader.Close();

                msSQL = $@"
                    SELECT SUM(closing_balance) AS total_closing_balance
                    FROM (
                            SELECT 
                            accountgroup_gid, 
                            accountgroup_name, 
                            credit_amount, 
                            debit_amount, 
                            opening_balance, 
                            ((opening_balance+debit_amount)-credit_amount) AS closing_balance 
                            FROM (
                                    SELECT 
                                      c.accountgroup_gid, 
                                      c.accountgroup_name, 
                                      CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, 
                                      CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, 
                                      CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance 
                                    FROM 
                                      acc_trn_journalentrydtl a 
                                      LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid 
                                      LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid 
                                    WHERE 
                                      (transaction_amount <> 0.00) 
                                      AND (c.ledger_type = 'Y' AND display_type = 'N') 
                                      AND b.branch_gid='{branch}'
                                      AND b.transaction_date BETWEEN   '{year}' AND '{lsyear}'  
                                    GROUP BY 
                                      c.accountgroup_gid
                                   ) x
                    ) y; ";
                expenseClosingBalance = Convert.ToDouble(objdbconn.GetExecuteScalar(msSQL));

                msSQL = $@"
                    SELECT SUM(closing_balance) AS total_closingbalance
                    FROM (
                          SELECT 
                            accountgroup_gid, 
                            accountgroup_name, 
                            credit_amount, 
                            debit_amount, 
                            opening_balance, 
                            ((opening_balance+credit_amount)-debit_amount) AS closing_balance 
                          FROM (
                                SELECT 
                                  c.accountgroup_gid, 
                                  c.accountgroup_name, 
                                  CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='cr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS credit_amount, 
                                  CAST(SUM(CASE WHEN b.transaction_type NOT LIKE '%Opening%' AND a.journal_type='dr' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS debit_amount, 
                                  CAST(SUM(CASE WHEN b.transaction_type LIKE '%Openin%' THEN a.transaction_amount ELSE 0.00 END) AS decimal) AS opening_balance 
                                FROM 
                                  acc_trn_journalentrydtl a 
                                  LEFT JOIN acc_trn_journalentry b ON a.journal_gid = b.journal_gid 
                                  LEFT JOIN acc_mst_tchartofaccount c ON a.account_gid = c.account_gid 
                                WHERE 
                                  (transaction_amount <> 0.00) 
                                  AND (c.ledger_type = 'Y' AND display_type = 'Y') 
                                  AND b.branch_gid='{branch}'
                                  AND b.transaction_date BETWEEN  '{year}' AND '{lsyear}'  
                                GROUP BY 
                                  c.accountgroup_gid
                              ) x
                            ) y;";
                incomeClosingBalance = Convert.ToDouble(objdbconn.GetExecuteScalar(msSQL));

            if (expenseClosingBalance < incomeClosingBalance)
            {
                lblpandl = "Net Profit:";
                lblpandlvalue = FormatNumber(incomeClosingBalance - expenseClosingBalance);
            }
            else
            {
                lblpandl = "Net Loss:";
                lblpandlvalue = FormatNumber(expenseClosingBalance - incomeClosingBalance);
            }
            var getsubfolders = new List<Balancesheetoverallnetvalue>();
            getsubfolders.Add(new Balancesheetoverallnetvalue
            {
                lblpandl = lblpandl,
                lblpandlvalue = lblpandlvalue,
            });
            values.Balancesheetoverallnetvalue = getsubfolders;
        }
        private string FormatNumber(double number)
        {
            return number.ToString("N2"); // Formats the number to 2 decimal places
        }
    }
}