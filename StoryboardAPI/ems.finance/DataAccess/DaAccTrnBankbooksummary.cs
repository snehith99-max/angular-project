using ems.finance.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Principal;
using System.Web;

namespace ems.finance.DataAccess
{
    public class DaAccTrnBankbooksummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL, msSQL12, msSQL2, msSQL13, msSQL14, msSQL1, msGetDlGID = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable, dt_datatable1, dt_datatable2;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lssopening_amount, lsCode, lsuser_code;
        string msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lstax_amount, lstax_amount2, lstax_amount3, lsclosing_amount;
        string lstax_total, lstotal_tax, lsfinyear, lsfinyear_start, lsfinyear_end, lsaccountname, lsbranch_gid;
        string lstransaction_code, lsbank_name, lsdr_cr, lsdrcr_value, lsbdr_cr, lsaccount_gid, lsjournal_desc, lsaaccount_gid;
        string lstransaction_amount, customer_name, customer_id,lsstax_amount, lsstax_amount2, lsstax_amount3, lsstotal_tax, lsfyear_startyear, lsfyear_endyear;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        decimal closecredit_amount, closedebit_amount, openingcredit_amount, openingdebit_amount;
        double closingbal = 0;
        string lsstart_date = "", lsend_date = "";
        DataSet ds_dataset;
        double tds_amount = 0.00;
        public static string GetCurrentFinancialYear(string fin_date)
        {
            int CurrentYear = DateTime.Parse(fin_date).Year;

            int PreviousYear = DateTime.Parse(fin_date).Year - 1;

            int NextYear = DateTime.Parse(fin_date).Year + 1;

            string PreYear = PreviousYear.ToString();

            string NexYear = NextYear.ToString();

            string CurYear = CurrentYear.ToString();

            string FinYear = null;

            if (DateTime.Parse(fin_date).Month > 3)

                FinYear = CurYear + "-" + NexYear;

            else

                FinYear = PreYear + "-" + CurYear;

            return FinYear.Trim();
        }
        public void DaGetBankBookSummary(MdlAccTrnBankbooksummary values)
        {
            msSQL = " select a.bank_gid, a.bank_code, a.bank_name, d.branch_prefix as branch_name, a.account_gid, a.account_no, a.ifsc_code, a.neft_code, " +
                    " a.swift_code, format(a.openning_balance,2) as openning_balance, format(a.closing_amount,2) as closing_amount " +
                    " from acc_mst_tbank a " +
                    " left join hrm_mst_tbranch d on d.branch_gid = a.branch_gid " +
                    " where a.default_flag = 'Y' group by a.bank_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Getbankbook_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getbankbook_list
                    {
                        bank_gid = dt["bank_gid"].ToString(),
                        bank_code = dt["bank_code"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        closing_amount = dt["closing_amount"].ToString(),
                    });

                    string lsstart_date = "";

                    lsstart_date = DateTime.Now.ToString("yyyy-MM-dd");

                    string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

                    string[] lssplit = startdate_lsfinyear.Split('-');

                    double current_openingbal = 0;

                    msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + dt["account_gid"] + "' and financial_year = '" + lssplit[0] + "' ";
                    string openning_balance = objdbconn.GetExecuteScalar(msSQL);

                    if (openning_balance == "" || openning_balance == null)
                    {
                        openning_balance = "0.00";
                    }

                    msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                            " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <='" + lsstart_date + "' " +
                            " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + dt["account_gid"] + "' ";
                    string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

                    if (sum_creditamount == "" || sum_creditamount == null)
                    {
                        sum_creditamount = "0.00";
                    }

                    msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                            " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <='" + lsstart_date + "' " +
                            " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + dt["account_gid"] + "' ";
                    string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

                    if (sum_debitamount == "" || sum_debitamount == null)
                    {
                        sum_debitamount = "0.00";
                    }

                    current_openingbal = double.Parse(openning_balance) - double.Parse(sum_debitamount) + double.Parse(sum_creditamount);

                    msSQL = " update acc_mst_tbank set closing_amount = '" + current_openingbal + "' where bank_gid = '" + dt["bank_gid"] + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.Getbankbook_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAllBankBookSummary(MdlAccTrnBankbooksummary values)
        {
            msSQL = " select a.bank_gid, a.bank_code, a.bank_name, d.branch_prefix as branch_name, a.account_gid, a.account_no, a.ifsc_code, a.neft_code, " +
                    " a.swift_code, format(a.openning_balance,2) as openning_balance, format(a.closing_amount,2) as closing_amount " +
                    " from acc_mst_tbank a " +
                    " left join hrm_mst_tbranch d on d.branch_gid = a.branch_gid " +
                    " group by a.bank_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Getbankbook_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getbankbook_list
                    {
                        bank_gid = dt["bank_gid"].ToString(),
                        bank_code = dt["bank_code"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        closing_amount = dt["closing_amount"].ToString(),
                    });

                    string lsstart_date = "";

                    lsstart_date = DateTime.Now.ToString("yyyy-MM-dd");

                    string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

                    string[] lssplit = startdate_lsfinyear.Split('-');

                    double current_openingbal = 0;

                    msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + dt["account_gid"] + "' and financial_year = '" + lssplit[0] + "' ";
                    string openning_balance = objdbconn.GetExecuteScalar(msSQL);

                    if (openning_balance == "" || openning_balance == null)
                    {
                        openning_balance = "0.00";
                    }

                    msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                            " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <='" + lsstart_date + "' " +
                            " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + dt["account_gid"] + "' ";
                    string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

                    if (sum_creditamount == "" || sum_creditamount == null)
                    {
                        sum_creditamount = "0.00";
                    }

                    msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                            " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <='" + lsstart_date + "' " +
                            " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + dt["account_gid"] + "' ";
                    string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

                    if (sum_debitamount == "" || sum_debitamount == null)
                    {
                        sum_debitamount = "0.00";
                    }

                    current_openingbal = double.Parse(openning_balance) - double.Parse(sum_debitamount) + double.Parse(sum_creditamount);

                    msSQL = " update acc_mst_tbank set closing_amount = '" + current_openingbal + "' where bank_gid = '" + dt["bank_gid"] + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.Getbankbook_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetSubBankBook(MdlAccTrnBankbooksummary values, string bank_gid, string from_date, string to_date)
        {
            msSQL = "select account_gid from acc_mst_tbank where bank_gid='" + bank_gid + "'";
            string account_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select bank_name from acc_mst_tbank where bank_gid='" + bank_gid + "'";
            string lsbank_name = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select account_no from acc_mst_tbank where bank_gid='" + bank_gid + "'";
            string lsaccount_no = objdbconn.GetExecuteScalar(msSQL);

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) - double.Parse(sum_debitamount) + double.Parse(sum_creditamount);

            double openning_balance_fromdate = current_openingbal;

            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, date_format(a.transaction_date, '%d-%m-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                    " d.account_name, a.remarks, " +
                    " b.journal_type, a.transaction_type, c.bank_name, " +
                    "format((select transaction_amount from acc_trn_journalentrydtl where journal_type ='dr'" +
                    " and transaction_gid = d.account_gid and journal_gid=a.journal_gid), 2) as transaction_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.transaction_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Getsubbankbook_list>();

            int i = 0;

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    double lsdebit_amount = 0, lscredit_amount = 0;

                    if (dt["journal_type"].ToString() == "cr")
                    {
                        closingbal = openning_balance_fromdate + double.Parse(dt["transaction_amount"].ToString());

                        lscredit_amount = double.Parse(dt["transaction_amount"].ToString());
                        lsdebit_amount = 0;
                    }
                    else
                    {
                        closingbal = openning_balance_fromdate - double.Parse(dt["transaction_amount"].ToString());

                        lscredit_amount = 0;
                        lsdebit_amount = double.Parse(dt["transaction_amount"].ToString());

                        string formattedAmount1 = lscredit_amount.ToString("N2");
                    }

                    i += 1;

                    getModuleList.Add(new Getsubbankbook_list
                    {
                        s_no = i,
                        journal_gid = dt["journal_gid"].ToString(),
                        journaldtl_gid = dt["journaldtl_gid"].ToString(),
                        transaction_gid = dt["transaction_gid"].ToString(),
                        bank_gid = dt["bank_gid"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        account_no = lsaccount_no,
                        remarks = dt["remarks"].ToString(),
                        journal_type = dt["journal_type"].ToString(),
                        transaction_type = dt["transaction_type"].ToString(),
                        bank_name = lsbank_name,
                        transaction_amount = dt["transaction_amount"].ToString(),
                        credit_amount = lscredit_amount.ToString("N2"),
                        debit_amount = lsdebit_amount.ToString("N2"),
                        opening_balance = openning_balance_fromdate.ToString("N2"),
                        closing_balance = closingbal.ToString("N2"),
                    });
                    openning_balance_fromdate = closingbal;

                    values.Getsubbankbook_list = getModuleList;
                    values.Getsubbankbook_list = getModuleList.OrderByDescending(a => a.s_no).ToList();
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetBankBookAddSummary(string bank_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select a.bank_code,a.bank_name,a.bank_gid, a.gl_code, a.account_no, a.account_type," +
                    " a.ifsc_code, a.neft_code, a.swift_code, a.account_gid " +
                    " from acc_mst_tbank a " +
                    " where a.bank_gid = '" + bank_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Getbank_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getbank_list
                    {
                        account_gid = dt["account_gid"].ToString(),
                        bank_gid = dt["bank_gid"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        account_type = dt["account_type"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                    });
                    values.addbank_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccTrnGroupDtl(MdlAccTrnBankbooksummary values)
        {
            msSQL = " select a.accountgroup_gid, a.accountgroup_name " +
                    " from acc_mst_tchartofaccount a ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAccountGroupDropdown>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountGroupDropdown
                    {
                        accountgroup_gid = dt["accountgroup_gid"].ToString(),
                        accountgroup_name = dt["accountgroup_name"].ToString(),
                    });
                    values.GetAccTrnGroupDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccTrnNameDtl(MdlAccTrnBankbooksummary values)
        {
            msSQL = " select a.account_gid, a.account_name " +
                    " from acc_mst_tchartofaccount a ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAccountNameDropdown>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAccountNameDropdown
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.GetAccTrnNameDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostProductGroupSummary(string user_gid, accountfetch_list values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("FPCT");

            msSQL = " insert into acc_ses_journalentry (" +
                    " session_id," +
                    " account_gid," +
                    " account_desc," +
                    " dr_cr," +
                    " transaction_amount," +
                    " journal_desc," +
                    " created_by, " +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid + "'," +
                    " '" + values.accountgroup_name + "'," +
                    " '" + values.account_name + "'," +
                    " '" + values.dr_cr + "'," +
                    " '" + values.transaction_amount + "'," +
                    " '" + values.journal_desc + "'," +
                    " '" + user_gid + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Account Group Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Account Group";
            }
        }
        public void DaGetInputTaxSummary(string user_gid, MdlAccTrnBankbooksummary values, string from_date, string to_date)
        {
            // Parse the original date string using custom date format
            DateTime originalDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime originalDate1 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Convert the date to the desired format
            string fromdate = originalDate.ToString("yyyy-MM-dd");
            string todate = originalDate1.ToString("yyyy-MM-dd");

            msSQL = " select a.invoice_gid, date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, a.invoice_refno, b.vendor_companyname, b.gst_number, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='0' " +
                    " and x.tax_name like 'SGST%'),2) as SGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5' from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='2.5' and x.tax_name like 'SGST%'),2) as SGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' from pbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage ='6'and x.tax_name like 'SGST%'),2) as SGST_6, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                    " from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='9' and x.tax_name like 'SGST%'),2) as SGST_9, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='14' and " +
                    " x.tax_name like 'SGST%'),2) as SGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and " +
                    " x.tax_percentage = '0' and x.tax_name like 'CGST%'),2) as CGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' from pbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage ='2.5' and x.tax_name like 'CGST%'),2) as CGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as CGST_6 " +
                    " from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='6'  and x.tax_name like 'CGST%'),2) as CGST_6, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as CGST_9 from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='9' and " +
                    " x.tax_name like 'CGST%'),2) as CGST_9, format((select ifnull(sum(x.tax_amount),0.00) as CGST_14 from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='14'and x.tax_name like 'CGST%'),2) as CGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from pbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage ='0' and x.tax_name like 'IGST%'),2) as IGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' " +
                    " from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='5' and x.tax_name like 'IGST%'),2) as IGST_5, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='12' " +
                    " and x.tax_name like 'IGST%'),2) as IGST_12, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from pbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='18' and x.tax_name like 'IGST%'),2) as IGST_18, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from pbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage ='28' and x.tax_name like 'IGST%'),2) as IGST_28, format(a.invoice_amount,2) as invoice_amount, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) from pbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount, " +
                    " format((select ifnull(sum((qty_invoice * product_price)-x.discount_amount),0.00) + " +
                    " ifnull((additionalcharges_amount+freightcharges+round_off), 0.00) - ifnull((y.discount_amount+buybackorscrap),0.00) from acp_trn_tinvoicedtl x " +
                    " inner join acp_trn_tinvoice y on x.invoice_gid = y.invoice_gid where y.invoice_gid = a.invoice_gid group by y.invoice_gid),2) as Non_Taxable_Amount from acp_trn_tinvoice a " +
                    " inner join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid where invoice_status<> 'Invoice Cancelled' " +
                    " and a.invoice_date >= DATE_FORMAT('" + fromdate + "', '%Y-%m-%d') and " +
                    " a.invoice_date <= DATE_FORMAT('" + todate + "', '%Y-%m-%d') " +
                    " order by a.invoice_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var InputTaxSummaryList = new List<GetInputTaxSummaryList>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    InputTaxSummaryList.Add(new GetInputTaxSummaryList
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        vendor_name = dt["vendor_companyname"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        SGST_0 = dt["SGST_0"].ToString(),
                        SGST_2point5 = dt["SGST_2point5"].ToString(),
                        SGST_6 = dt["SGST_6"].ToString(),
                        SGST_9 = dt["SGST_9"].ToString(),
                        SGST_14 = dt["SGST_14"].ToString(),
                        CGST_0 = dt["CGST_0"].ToString(),
                        CGST_2point5 = dt["CGST_2point5"].ToString(),
                        CGST_6 = dt["CGST_6"].ToString(),
                        CGST_9 = dt["CGST_9"].ToString(),
                        CGST_14 = dt["CGST_14"].ToString(),
                        IGST_0 = dt["IGST_0"].ToString(),
                        IGST_5 = dt["IGST_5"].ToString(),
                        IGST_12 = dt["IGST_12"].ToString(),
                        IGST_18 = dt["IGST_18"].ToString(),
                        IGST_28 = dt["IGST_28"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        Taxable_Amount = dt["Taxable_Amount"].ToString(),
                        Non_Taxable_Amount = dt["Non_Taxable_Amount"].ToString()
                    });
                    values.GetInputTaxSummaryList = InputTaxSummaryList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetOutputTaxSummary(string user_gid, MdlAccTrnBankbooksummary values, string from_date, string to_date)
        {
            // Parse the original date string using custom date format
            DateTime originalDate2 = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime originalDate3 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Convert the date to the desired format
            string fromdateout = originalDate2.ToString("yyyy-MM-dd");
            string todateout = originalDate3.ToString("yyyy-MM-dd");            

            msSQL = " select a.invoice_gid, date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, a.invoice_refno, a.customer_name, b.gst_number, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='0' " +
                    " and x.tax_name like 'SGST%'),2) as SGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='2.5' and x.tax_name like 'SGST%'),2) as SGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage='6' and x.tax_name like 'SGST%'),2) as SGST_6, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='9'and x.tax_name like 'SGST%'),2) as SGST_9, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='14' and " +
                    " x.tax_name like 'SGST%'),2) as SGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='0' and x.tax_name like 'CGST%'),2) as CGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' and x.tax_name like 'CGST%'),2) as CGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 6' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage='6' and x.tax_name like 'CGST%'),2) as CGST_6, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 9' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage='9' and " +
                    " x.tax_name like 'CGST%'),2) as CGST_9, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 14' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage='14'  and x.tax_name like 'CGST%'),2) as CGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name like 'IGST%'),2) as IGST_5, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage='12' " +
                    " and x.tax_name like 'IGST%'),2) as IGST_12, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid " +
                    " and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as IGST_18, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as IGST_28, format(a.invoice_amount,2) as invoice_amount, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount, " +
                    " format((select sum(x.qty_invoice * (x.product_price-x.discount_amount))+ (ifnull((y.freight_charges+y.packing_charges + y.insurance_charges+y.additionalcharges_amount+roundoff),0.00))-ifnull(y.buyback_charges,0.00)-ifnull(y.discount_amount,0.00) " +
                    " from rbl_trn_tinvoicedtl x inner join rbl_trn_tinvoice y on x.invoice_gid=y.invoice_gid where a.invoice_gid=x.invoice_gid group by y.invoice_gid ),2) as Non_Taxable_Amount " +
                    " from rbl_trn_tinvoice a inner join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                    " where invoice_status<> 'Invoice Cancelled' " +
                    " and a.invoice_date >= date_format('" + fromdateout + "', '%Y-%m-%d') and " +
                    " a.invoice_date <= date_format('" + todateout + "', '%Y-%m-%d') " +
                    " group by a.invoice_gid order by a.invoice_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var OutputTaxSummaryList = new List<GetOutputTaxSummaryList>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    OutputTaxSummaryList.Add(new GetOutputTaxSummaryList
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        SGST_0 = dt["SGST_0"].ToString(),
                        SGST_2point5 = dt["SGST_2point5"].ToString(),
                        SGST_6 = dt["SGST_6"].ToString(),
                        SGST_9 = dt["SGST_9"].ToString(),
                        SGST_14 = dt["SGST_14"].ToString(),
                        CGST_0 = dt["CGST_0"].ToString(),
                        CGST_2point5 = dt["CGST_2point5"].ToString(),
                        CGST_6 = dt["CGST_6"].ToString(),
                        CGST_9 = dt["CGST_9"].ToString(),
                        CGST_14 = dt["CGST_14"].ToString(),
                        IGST_0 = dt["IGST_0"].ToString(),
                        IGST_5 = dt["IGST_5"].ToString(),
                        IGST_12 = dt["IGST_12"].ToString(),
                        IGST_18 = dt["IGST_18"].ToString(),
                        IGST_28 = dt["IGST_28"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        Taxable_Amount = dt["Taxable_Amount"].ToString(),
                        Non_Taxable_Amount = dt["Non_Taxable_Amount"].ToString()
                    });
                    values.GetOutputTaxSummaryList = OutputTaxSummaryList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCreditNoteTaxSummary(string user_gid, MdlAccTrnBankbooksummary values, string from_date, string to_date)
        {
            // Parse the original date string using custom date format
            DateTime originalDate4 = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime originalDate5 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // Convert the date to the desired format
            string fromdatecredit = originalDate4.ToString("yyyy-MM-dd");
            string todatecredit = originalDate5.ToString("yyyy-MM-dd");

            msSQL = " select a.creditnote_gid, b.invoice_gid, concat(a.creditnote_gid, '  ', b.invoice_refno) as invoice_refno, date_format(a.credit_date, '%d-%m-%Y') as credit_date, " +
                    " c.customer_name, c.gst_number, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='0' and x.tax_name like 'SGST%'),2) as SGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage ='2.5' and x.tax_name like 'SGST%'),2) as SGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage='6' and x.tax_name like 'SGST%'),2) as SGST_6, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='9'and " +
                    " x.tax_name like 'SGST%'),2) as SGST_9, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='14' and x.tax_name like 'SGST%'),2) as SGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage ='0'and x.tax_name like 'CGST%'),2) as CGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' and x.tax_name like 'CGST%'),2) as CGST_2point5, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 6' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid and x.tax_percentage='6' " +
                    " and x.tax_name like 'CGST%'),2) as CGST_6, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 9' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage='9' and x.tax_name like 'CGST%'),2) as CGST_9, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 14' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage='14'  and x.tax_name like 'CGST%'),2) as CGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and " +
                    " x.tax_name like 'IGST%'),2) as IGST_5, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from rbl_trn_vinvoicetax x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage='12' and x.tax_name like 'IGST%'),2) as IGST_12, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as IGST_18, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' " +
                    " from rbl_trn_vinvoicetax x where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as IGST_28, " +
                    " format(b.invoice_amount, 2) as invoice_amount, format((select ifnull(sum(x.tax_amount),0.00) from rbl_trn_vinvoicetax x " +
                    " where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount from rbl_trn_tcreditnote a left join rbl_trn_tinvoice b on a.invoice_gid = b.invoice_gid " +
                    " left join crm_mst_tcustomer c on b.customer_gid = c.customer_gid  left join rbl_trn_tinvoicedtl d on a.invoice_gid = d.invoice_gid " +
                    " where b.invoice_gid not in (select invoice_gid from rbl_trn_tinvoicedtl d  where d.tax_amount = '0' and d.tax_amount2 = '0'  and d.tax_amount3 = '0') and " +
                    " b.taxfiling_flag<>'Y' and(('" + fromdatecredit + "' IS NOT NULL  and  '" + todatecredit + "' IS NOT NULL " +
                    " and a.credit_date >= DATE_FORMAT('" + fromdatecredit + "', '%Y-%m-%d') and a.credit_date <= DATE_FORMAT('" + todatecredit + "', '%Y-%m-%d'))) " +
                    " group by a.invoice_gid order by b.invoice_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var CreditNoteSummaryList = new List<GetCreditNoteTaxSummaryList>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CreditNoteSummaryList.Add(new GetCreditNoteTaxSummaryList
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        credit_date = dt["credit_date"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        SGST_0 = dt["SGST_0"].ToString(),
                        SGST_2point5 = dt["SGST_2point5"].ToString(),
                        SGST_6 = dt["SGST_6"].ToString(),
                        SGST_9 = dt["SGST_9"].ToString(),
                        SGST_14 = dt["SGST_14"].ToString(),
                        CGST_0 = dt["CGST_0"].ToString(),
                        CGST_2point5 = dt["CGST_2point5"].ToString(),
                        CGST_6 = dt["CGST_6"].ToString(),
                        CGST_9 = dt["CGST_9"].ToString(),
                        CGST_14 = dt["CGST_14"].ToString(),
                        IGST_0 = dt["IGST_0"].ToString(),
                        IGST_5 = dt["IGST_5"].ToString(),
                        IGST_12 = dt["IGST_12"].ToString(),
                        IGST_18 = dt["IGST_18"].ToString(),
                        IGST_28 = dt["IGST_28"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        Taxable_Amount = dt["Taxable_Amount"].ToString()
                    });
                    values.GetCreditNoteTaxSummaryList = CreditNoteSummaryList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetDebitNoteTaxSummary(string user_gid, MdlAccTrnBankbooksummary values, string from_date, string to_date)
        {
            // Parse the original date string using custom date format
            DateTime originalDate6 = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime originalDate7 = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // Convert the date to the desired format
            string fromdatedebit = originalDate6.ToString("yyyy-MM-dd");
            string todatedebit = originalDate7.ToString("yyyy-MM-dd");

            msSQL = " select a.debitnote_gid, b.invoice_gid, date_format(a.debit_date, '%d-%m-%Y') as debit_date, c.vendor_companyname, c.gst_number, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='0' " +
                    " and x.tax_name like 'SGST%'),2) as SGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='2.5' and x.tax_name like 'SGST%'),2) as SGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' from acp_trn_tinvoicedtl x " +
                    " where x.invoice_gid = a.invoice_gid and x.tax_percentage='6' and x.tax_name like 'SGST%'),2) as SGST_6, format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                    " from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='9'and x.tax_name like 'SGST%'),2) as SGST_9, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid and x.tax_percentage ='14' and " +
                    " x.tax_name like 'SGST%'),2) as SGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage ='0'and x.tax_name2 like 'CGST%'),2) as CGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' from acp_trn_tinvoicedtl x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' and x.tax_name2 like 'CGST%'),2) as CGST_2point5, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 6' " +
                    " from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid and x.tax_percentage='6' and x.tax_name2 like 'CGST%'),2) as CGST_6, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 9' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid and x.tax_percentage='9' and " +
                    " x.tax_name2 like 'CGST%'),2) as CGST_9, format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 14' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid " +
                    " and x.tax_percentage='14'  and x.tax_name2 like 'CGST%'),2) as CGST_14, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from acp_trn_tinvoicedtl x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' " +
                    " from acp_trn_tinvoicedtl x where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name3 like 'IGST%'),2) as IGST_5, " +
                    " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from acp_trn_tinvoicedtl x where x.invoice_gid = a.invoice_gid and x.tax_percentage='12' " +
                    " and x.tax_name3 like 'IGST%'),2) as IGST_12, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from acp_trn_tinvoicedtl x where x.invoice_gid=a.invoice_gid " +
                    " and x.tax_percentage='18' and x.tax_name3 like 'IGST%'),2) as IGST_18, format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from acp_trn_tinvoicedtl x " +
                    " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name3 like 'IGST%'),2) as IGST_28," +
                    " format((select ifnull(sum((x.tax_amount) + (x.tax_amount2) + (x.tax_amount3)),0.00) from acp_trn_tinvoicedtl x where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount, " +
                    " format(b.total_amount,2) as total_amount " +
                    " from pmr_trn_tdebitnote a " +
                    " left join acp_trn_tinvoice b on a.invoice_gid = b.invoice_gid " +
                    " left join acp_mst_tvendor c on b.vendor_gid = c.vendor_gid " +
                    " where a.invoice_gid not in (select invoice_gid from acp_trn_tinvoicedtl c" +
                    " where c.tax_amount = '0' and c.tax_amount2 = '0' " +
                    " and c.tax_amount3 = '0') and b.taxfiling_flag<>'Y' " +
                    " AND(('" + fromdatedebit + "' IS NOT NULL " +
                    " AND '" + todatedebit + "' IS NOT NULL " +
                    " AND b.invoice_date >= DATE_FORMAT('" + fromdatedebit + "', '%Y-%m-%d') AND " +
                    " b.invoice_date <= DATE_FORMAT('" + todatedebit + "', '%Y-%m-%d'))) " +
                    " order by b.invoice_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var DebitNoteSummaryList = new List<GetDebitNoteTaxSummaryList>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    DebitNoteSummaryList.Add(new GetDebitNoteTaxSummaryList
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_refno = dt["debitnote_gid"].ToString(),
                        debit_date = dt["debit_date"].ToString(),
                        vendor_name = dt["vendor_companyname"].ToString(),
                        SGST_0 = dt["SGST_0"].ToString(),
                        SGST_2point5 = dt["SGST_2point5"].ToString(),
                        SGST_6 = dt["SGST_6"].ToString(),
                        SGST_9 = dt["SGST_9"].ToString(),
                        SGST_14 = dt["SGST_14"].ToString(),
                        CGST_0 = dt["CGST_0"].ToString(),
                        CGST_2point5 = dt["CGST_2point5"].ToString(),
                        CGST_6 = dt["CGST_6"].ToString(),
                        CGST_9 = dt["CGST_9"].ToString(),
                        CGST_14 = dt["CGST_14"].ToString(),
                        IGST_0 = dt["IGST_0"].ToString(),
                        IGST_5 = dt["IGST_5"].ToString(),
                        IGST_12 = dt["IGST_12"].ToString(),
                        IGST_18 = dt["IGST_18"].ToString(),
                        IGST_28 = dt["IGST_28"].ToString(),
                        Taxable_Amount = dt["Taxable_Amount"].ToString(),
                        invoice_amount = dt["total_amount"].ToString(),
                        gst_number = dt["gst_number"].ToString()
                    });
                    values.GetDebitNoteTaxSummaryList = DebitNoteSummaryList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetDebitorReportSummary(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL =  " SELECT distinct a.customer_name as customer,a.account_gid,  a.customer_id,a.customer_gid, concat(a.customer_id,' / ',customer_name) as customer_name," +
                     " concat(case when k.customercontact_name is null then '' else k.customercontact_name end,'/',case when k.email is null then '' else k.email end," +
                     " '/',case when k.mobile is null then '' else k.mobile end) as contact, concat(datediff(current_date(), min(a.created_date)), ' days') as customer_since," +
                     " format(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then " +
                     " b.transaction_amount end),2) " +
                     " as credit_amount,format(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' " +
                     " then b.transaction_amount end),2) as debit_amount, " +
                     " format(case when cast(c.financial_year as unsigned) = year(current_date()) then c.opening_balance else '0.00' end + " +
                     " sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr'" +
                     " then b.transaction_amount end) -sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then " +
                     " b.transaction_amount end),2) as closing_amount, " +
                     " format(case when cast(c.financial_year as unsigned) = year(current_date()) then c.opening_balance else '0.00' end,2) as opening_amount " +
                     " FROM crm_mst_tcustomer a " +
                     " left join crm_mst_tcustomercontact k on a.customer_gid=k.customer_gid " +
                     " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                     " left join acc_trn_topeningbalance c on c.account_gid = b.account_gid " +
                     " where a.account_gid in " +
                     " (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null) " +
                     " group by account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var Debitorreport_List = new List<GetDebitorreport_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = "select sum(a.transaction_amount) as tds  from acc_trn_journalentrydtl a  " +
                      " left join acc_mst_accountmapping b on b.account_gid = a.account_gid " +
                      " left join acc_trn_journalentry c on a.journal_gid=c.journal_gid  " +
                      " left join rbl_trn_tpayment d on d.payment_gid=c.journal_refno  " +
                      " left join rbl_trn_tinvoice e on d.invoice_gid = e.invoice_gid " +
                      " left join crm_mst_tcustomer f on f.customer_gid=e.customer_gid  " +
                      " where b.field_name='With Hold Tax' and b.module_name='RBL' and screen_name='Receipt'  and " +
                      " f.customer_gid='" + dt["customer_gid"].ToString() + "'";
                    string tds = objdbconn.GetExecuteScalar(msSQL);
                    if (tds == "")
                    {
                        tds_amount = 0.00;
                    }
                    else
                    {
                        tds_amount = Convert.ToDouble(tds);
                    }
                    double closing_amount = Convert.ToDouble(dt["opening_amount"].ToString()) +
                        Convert.ToDouble(dt["debit_amount"].ToString()) -
                        Convert.ToDouble(dt["credit_amount"].ToString()) - tds_amount;
                    string FormattedClosingAmount = closing_amount.ToString("N2");

                    Debitorreport_List.Add(new GetDebitorreport_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        opening_amount = dt["opening_amount"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        closing_amount = FormattedClosingAmount,
                        customer_id = dt["customer_id"].ToString(),
                        contact = dt["contact"].ToString(),
                        customer_since = dt["customer_since"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        tds = tds_amount.ToString("N2"),
                    });                  
                }
            }
            values.GetDebitorreport_List = Debitorreport_List;
            dt_datatable.Dispose();
        }
        public void DaGetDebitorReportView(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select concat(a.journal_type, ' ', e.account_name, '/', b.transaction_type) as account_desc, " +
                    " DATE_FORMAT(b.transaction_date, '%d-%m-%Y') AS transaction_date, b.transaction_type as transaction_type,b.journal_refno as journal_refno," +
                    " format((case when a.journal_type = 'dr' then '0.00' when a.journal_type = 'cr' then a.transaction_amount end),2)  as credit_amount," +
                    " format((case when a.journal_type = 'cr' then '0.00' when a.journal_type = 'dr'  then a.transaction_amount end),2) as debit_amount," +
                    " format((case when a.journal_type = 'dr' then '0.00' when a.journal_type = 'cr' then a.transaction_amount end),2)  as closecredit_amount," +
                    " format((case when a.journal_type = 'cr' then '0.00' when a.journal_type = 'dr'  then a.transaction_amount end),2) as closedebit_amount," +
                    " format((case when f.credit_amount is null then '0.00' when f.credit_amount is not null then f.credit_amount end),2) as openingcredit_amount," +
                    " format((case when f.debit_amount is null then '0.00' when f.debit_amount is not null then f.debit_amount end),2) as openingdebit_amount," +
                    " a.remarks,a.account_gid from acc_trn_journalentrydtl a" +
                    " left join acc_trn_journalentry b on b.journal_gid = a.journal_gid" +
                    " left join acc_mst_tchartofaccount e on e.account_gid = a.account_gid" +
                    " left join acc_trn_topenningbalance f on f.account_gid = a.account_gid" +
                    " where a.account_gid = '" + account_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetDebitorreportView_List> DebitorreportView_List = new List<GetDebitorreportView_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                decimal closingbalance = 0;

                for (int i = 0; i < dt_datatable.Rows.Count; i++)
                {
                    decimal closecredit_amount = string.IsNullOrEmpty(dt_datatable.Rows[i]["closecredit_amount"].ToString()) ? 0 : decimal.Parse(dt_datatable.Rows[i]["closecredit_amount"].ToString());
                    decimal closedebit_amount = string.IsNullOrEmpty(dt_datatable.Rows[i]["closedebit_amount"].ToString()) ? 0 : decimal.Parse(dt_datatable.Rows[i]["closedebit_amount"].ToString());
                    decimal openingcredit_amount = string.IsNullOrEmpty(dt_datatable.Rows[i]["openingcredit_amount"].ToString()) ? 0 : decimal.Parse(dt_datatable.Rows[i]["openingcredit_amount"].ToString());
                    decimal openingdebit_amount = string.IsNullOrEmpty(dt_datatable.Rows[i]["openingdebit_amount"].ToString()) ? 0 : decimal.Parse(dt_datatable.Rows[i]["openingdebit_amount"].ToString());
                    decimal creditAmount = 0;
                    decimal debitAmount = 0;
                    if (i == 0)
                    {
                        if (decimal.TryParse(dt_datatable.Rows[i]["credit_amount"].ToString(), out creditAmount) &&
                            decimal.TryParse(dt_datatable.Rows[i]["debit_amount"].ToString(), out debitAmount))
                        {
                            openingbalance = creditAmount - debitAmount;
                            closingbalance = openingbalance;
                        }
                    }
                    else
                    {
                        openingbalance = closingbalance;
                    }

                    closingbalance = openingbalance + closedebit_amount - closecredit_amount;

                    DebitorreportView_List.Add(new GetDebitorreportView_List
                    {
                        openingbalance = openingbalance.ToString(),
                        closingbalance = closingbalance.ToString(),
                        transaction_type = dt_datatable.Rows[i]["transaction_type"].ToString(),
                        journal_refno = dt_datatable.Rows[i]["journal_refno"].ToString(),
                        transaction_date = dt_datatable.Rows[i]["transaction_date"].ToString(),
                        customer_name = dt_datatable.Rows[i]["account_desc"].ToString(),
                        remarks = dt_datatable.Rows[i]["remarks"].ToString(),
                        debit_amount = dt_datatable.Rows[i]["debit_amount"].ToString(),
                        credit_amount = dt_datatable.Rows[i]["credit_amount"].ToString(),
                    });
                }
                values.GetDebitorreportView_List = DebitorreportView_List;                
            }
            dt_datatable.Dispose();
        }
        public void DaGetDebitorReportCustomerView(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select a.customer_name from crm_mst_tcustomer a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " where b.account_gid = '" + account_gid + "' group by b.account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var DebitorreportCustomerView_List = new List<GetDebitorreportCustomerView_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    DebitorreportCustomerView_List.Add(new GetDebitorreportCustomerView_List
                    {
                        customer_name = dt["customer_name"].ToString()
                    });
                    values.GetDebitorreportCustomerView_List = DebitorreportCustomerView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCreditorReportSummary(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " SELECT distinct a.vendor_gid,a.vendor_code,a.vendor_companyname  as vendor,a.account_gid, concat(a.vendor_code,' / ',a.vendor_companyname) as vendor_companyname," +
                    " concat(a.contactperson_name, '/',a.email_id,'/',a.contact_telephonenumber) as contact_details, " +
                    " format(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then " +
                    " b.transaction_amount end),2) " +
                    " as credit_amount,format(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' " +
                    " then b.transaction_amount end),2) as debit_amount, " +
                    " format(case when cast(c.financial_year as unsigned) = year(current_date()) then c.opening_balance else '0.00' end +  " +
                    " sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end)" +
                    " -sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then  b.transaction_amount end),2) as closing_amount, " +
                    " format(case when cast(c.financial_year as unsigned) = year(current_date()) then c.opening_balance else '0.00' end,2) as opening_amount  " +
                    " FROM acp_mst_tvendor a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " left join acc_trn_topeningbalance c on c.account_gid = b.account_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = a.account_gid " +
                    " where a.account_gid  in " +
                    " (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null) " +
                    " group by account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var Creditorreport_List = new List<GetCreditorreport_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    Creditorreport_List.Add(new GetCreditorreport_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        vendor_gid = dt["vendor_gid"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        vendor = dt["vendor"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        closing_amount = dt["closing_amount"].ToString(),
                        opening_amount = dt["opening_amount"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                    });
                    values.GetCreditorreport_List = Creditorreport_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCreditorReportView(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select concat(a.journal_type, ' ', e.account_name, '/', b.transaction_type) as account_desc, " +
                    " a.remarks,concat(case when c.bank_gid is null then b.journal_from else b.reference_type end) as account_name," +
                    " DATE_FORMAT(b.transaction_date, '%d-%m-%Y') AS transaction_date, b.transaction_type as transaction_type," +
                    " b.journal_refno as journal_refno, " +
                    " format((case when a.journal_type = 'dr' then '0.00' when a.journal_type = 'cr' then " +
                    " a.transaction_amount end),2) " +
                    " as credit_amount,format((case when a.journal_type = 'cr' then '0.00' when a.journal_type = 'dr' " +
                    " then a.transaction_amount end),2) as debit_amount, " +
                    " a.remarks,a.account_gid," +
                    " format((case when a.journal_type = 'dr' then '0.00' when a.journal_type = 'cr' then a.transaction_amount end),2)  as closecredit_amount," +
                    " format((case when a.journal_type = 'cr' then '0.00' when a.journal_type = 'dr'  then a.transaction_amount end),2) as closedebit_amount " +
                    " from acc_trn_journalentrydtl a " +
                    " left join acc_trn_journalentry b on b.journal_gid = a.journal_gid " +
                    " left join acc_mst_tchartofaccount e on e.account_gid = a.account_gid " +
                    " left join acc_trn_topeningbalance f on f.account_gid = a.account_gid" +
                    " left join acc_mst_tbank c on c.bank_gid = b.reference_gid  " +
                    " where a.account_gid = '" + account_gid + "'  order by b.transaction_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetCreditorreportView_List> CreditorreportView_List = new List<GetCreditorreportView_List>();
            
            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                decimal closingbalance = 0;

                for (int i = 0; i < dt_datatable.Rows.Count; i++)
                {
                    decimal closecredit_amount = string.IsNullOrEmpty(dt_datatable.Rows[i]["closecredit_amount"].ToString()) ? 0 : decimal.Parse(dt_datatable.Rows[i]["closecredit_amount"].ToString());
                    decimal closedebit_amount = string.IsNullOrEmpty(dt_datatable.Rows[i]["closedebit_amount"].ToString()) ? 0 : decimal.Parse(dt_datatable.Rows[i]["closedebit_amount"].ToString());
                    
                    decimal creditAmount = 0;
                    decimal debitAmount = 0;

                    if (i == 0)
                    {
                        if (decimal.TryParse(dt_datatable.Rows[i]["credit_amount"].ToString(), out creditAmount) &&
                            decimal.TryParse(dt_datatable.Rows[i]["debit_amount"].ToString(), out debitAmount))
                        {
                            openingbalance = creditAmount - debitAmount;
                            closingbalance = openingbalance;
                        }
                    }
                    else
                    {
                        openingbalance = closingbalance;
                    }

                    closingbalance = openingbalance + closecredit_amount - closedebit_amount;

                    CreditorreportView_List.Add(new GetCreditorreportView_List
                    {
                        openingbalance = openingbalance.ToString(),
                        closingbalance = closingbalance.ToString("N2"),
                        transaction_type = dt_datatable.Rows[i]["transaction_type"].ToString(),
                        journal_refno = dt_datatable.Rows[i]["journal_refno"].ToString(),
                        transaction_date = dt_datatable.Rows[i]["transaction_date"].ToString(),
                        customer_name = dt_datatable.Rows[i]["account_desc"].ToString(),
                        remarks = dt_datatable.Rows[i]["remarks"].ToString(),
                        debit_amount = dt_datatable.Rows[i]["debit_amount"].ToString(),
                        credit_amount = dt_datatable.Rows[i]["credit_amount"].ToString(),
                        account_name = dt_datatable.Rows[i]["account_name"].ToString(),
                    });
                }
                values.GetCreditorreportView_List = CreditorreportView_List;
            }
            dt_datatable.Dispose();
        }
        public void DaGetCreditorReportVendorView(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select a.vendor_code,a.vendor_companyname from acp_mst_tvendor a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " where b.account_gid = '" + account_gid + "' group by b.account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var CreditorreportVendorView_List = new List<GetCreditorreportVendorView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CreditorreportVendorView_List.Add(new GetCreditorreportVendorView_List
                    {
                        vendor_name = dt["vendor_companyname"].ToString(),
                        vendor_code = dt["vendor_code"].ToString()
                    });
                    values.GetCreditorreportVendorView_List = CreditorreportVendorView_List;
                }
            }
            dt_datatable.Dispose();
        }        
        public void DaGetdeletebankbookdtls(MdlAccTrnBankbooksummary values, string journal_gid, string journaldtl_gid, string account_gid)
        {
            try
            {
                msSQL = " delete from acc_trn_journalentrydtl where journaldtl_gid  = '" + journaldtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " delete from acc_trn_journalentrydtl where transaction_gid  = '" + account_gid + "' " +
                            " and journal_gid='" + journal_gid + "'";
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult2 != 0)
                    {
                        msSQL = " select * from acc_trn_journalentrydtl where journal_gid='" + journal_gid + "'";
                        objODBCDatareader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDatareader.HasRows == false)
                        {
                            msSQL = " delete from  acc_trn_journalentry where journal_gid='" + journal_gid + "'";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult3 != 0)
                            {
                                values.status = true;
                                values.message = "Bank Book Deleted Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Deleting Bank Book !!";
                            }
                        }
                        else
                        {
                            values.status = true;
                            values.message = "Bank Book Deleted Successfully !!";
                        }
                        objODBCDatareader.Close();
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Bank Book !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Bank Book !!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Bank Book";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetFinancialYear(MdlAccTrnBankbooksummary values)
        {
            msSQL = " select year(fyear_start) as finyear,finyear_gid from adm_mst_tyearendactivities " +
                    " order by finyear_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var FinancialYear_List = new List<GetFinancialYear_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    FinancialYear_List.Add(new GetFinancialYear_List
                    {
                        finyear_gid = dt["finyear_gid"].ToString(),
                        finyear = dt["finyear"].ToString(),
                    });
                    values.GetFinancialYear_List = FinancialYear_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetBankBookEntryView(string user_gid, MdlAccTrnBankbooksummary values, string bank_gid)
        {
            msSQL = " select bank_code,bank_name, gl_code, account_no, account_type, ifsc_code, " +
                    " neft_code, swift_code, account_gid " +
                    " from acc_mst_tbank where bank_gid='" + bank_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var BankBookEntryView_List = new List<GetBankBookEntryView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    BankBookEntryView_List.Add(new GetBankBookEntryView_List
                    {
                        bank_code = dt["bank_code"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        account_no = dt["account_no"].ToString(),
                        account_type = dt["account_type"].ToString(),
                        ifsc_code = dt["ifsc_code"].ToString(),
                        neft_code = dt["neft_code"].ToString(),
                        swift_code = dt["swift_code"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                    });
                    values.GetBankBookEntryView_List = BankBookEntryView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountGroupList(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select distinct accountgroup_gid, CONCAT(accountgroup_name,' - ', UCASE(substring(account_name,1,1)), " +
                    " LCASE(SUBSTRING(account_name, 2))) as accountgroup_name from acc_mst_tchartofaccount where 0=0 and accountgroup_gid <> '$' and has_child <> 'N' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var AccountGroup_List = new List<GetAccountGroup_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    AccountGroup_List.Add(new GetAccountGroup_List
                    {
                        accountgroup_gid = dt["accountgroup_gid"].ToString(),
                        accountgroup_name = dt["accountgroup_name"].ToString(),
                    });
                    values.GetAccountGroup_List = AccountGroup_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountNameList(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select distinct account_gid,account_name " +
                    " from acc_mst_tchartofaccount where has_child='N' order by account_name";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var Account_List = new List<GetAccount_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    Account_List.Add(new GetAccount_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                    values.GetAccount_List = Account_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountMulAddDtl(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " Select distinct a.session_id, format(a.transaction_amount,2) as transaction_amount ," +
                    " format(a.credit_amount,2) as credit_amount,format(a.debit_amount,2) as debit_amount," +
                    " b.accountgroup_name, b.account_name,a.journal_desc as remarks,a.dr_cr " +
                    " from acc_ses_journalentry a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " where a.userid = '" + user_gid + "' order by a.session_id desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var AccountMulAdd_List = new List<GetAccountMulAdd_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    AccountMulAdd_List.Add(new GetAccountMulAdd_List
                    {
                        session_id = dt["session_id"].ToString(),
                        transaction_amount = dt["transaction_amount"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        accountgroup_name = dt["accountgroup_name"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        dr_cr = dt["dr_cr"].ToString(),
                    });
                    values.GetAccountMulAdd_List = AccountMulAdd_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostAccountMulAddDtls(string user_gid, acctmuladddtl_list values)
        {
            try
            {
                msSQL = " SELECT distinct a.account_name" +
                        " FROM acc_mst_tchartofaccount a  " +
                        " where account_gid = '" + values.account_name + "'";
                lsaccountname = objdbconn.GetExecuteScalar(msSQL);

                if (values.transaction_type == "Deposit")
                {
                    msGetGid = objcmnfunctions.GetMasterGID("FPCT");

                    msSQL = " insert into acc_ses_journalentry( " +
                            " session_id, " +
                            " account_gid, " +
                            " account_desc," +
                            " journal_desc," +
                            " dr_cr," +
                            " credit_amount," +
                            " transaction_amount, " +
                            " created_by, " +
                            " created_date, " +
                            " userid) " +
                            " values( " +
                            "'" + msGetGid + "', " +
                            "'" + values.account_name + "'," +
                            "'" + lsaccountname + "'," +
                            "'" + values.txtremarks + "'," +
                            "'Deposit'," +
                            "'" + values.transaction_amount + "'," +
                            "'" + values.transaction_amount + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Bank Entry Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Bank Entry !!";
                    }
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("FPCT");

                    msSQL = " insert into acc_ses_journalentry( " +
                           " session_id, " +
                           " account_gid, " +
                           " account_desc," +
                           " journal_desc," +
                           " dr_cr," +
                           " debit_amount," +
                           " transaction_amount, " +
                           " created_by, " +
                           " created_date, " +
                           " userid) " +
                           " values( " +
                           "'" + msGetGid + "', " +
                           "'" + values.account_name + "'," +
                           "'" + lsaccountname + "'," +
                           "'" + values.txtremarks + "'," +
                           "'Withdraw'," +
                           "'" + values.transaction_amount + "'," +
                           "'" + values.transaction_amount + "'," +
                           "'" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                           "'" + user_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Bank Entry Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Bank Entry !!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Bank Entry!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDeleteMulBankDtls(MdlAccTrnBankbooksummary values, string session_id)
        {
            try
            {
                msSQL = " delete from acc_ses_journalentry  where session_id = '" + session_id + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Bank Entry Deleted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Bank Entry !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Bank Entry";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostDirectBankBookEntry(string user_gid, bankbookadd_list values)
        {
            string dateString = values.transaction_date;
            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            string lsbalance = "0.00";

            try
            {
                msSQL = " Select a.session_id " +
                        " from acc_ses_journalentry a " +
                        " where a.userid = '" + user_gid + "' order by a.session_id desc";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == false)
                {
                    values.status = false;
                    values.message = "Add atleast one Entry to Create Voucher !!";
                }
                else
                {
                    msSQL = " select bank_name,bank_code,branch_gid,account_gid " +
                            " from acc_mst_tbank where bank_gid='" + values.bank_gid + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows == true)
                    {
                        lsbranch_gid = objODBCDatareader["branch_gid"].ToString();
                        lstransaction_code = objODBCDatareader["bank_code"].ToString();
                    }
                    objODBCDatareader.Close();

                    msSQL = " select bank_name " +
                            " from acc_mst_tbank where bank_gid='" + values.bank_gid + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows == true)
                    {
                        lsbank_name = objODBCDatareader["bank_name"].ToString();
                    }
                    objODBCDatareader.Close();

                    msSQL = " select account_gid " +
                            " from acc_mst_tbank where bank_gid='" + values.bank_gid + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows == true)
                    {
                        lsaccount_gid = objODBCDatareader["account_gid"].ToString();
                    }
                    objODBCDatareader.Close();

                    msGetGid = objcmnfunctions.GetMasterGID("FPCC");

                    msSQL = " Insert into acc_trn_journalentry " +
                            " (journal_gid, " +
                            " journal_refno, " +
                            " transaction_date, " +
                            " transaction_type," +
                            " transaction_code," +
                            " remarks," +
                            " reference_type," +
                            " reference_gid," +
                            " transaction_gid, " +
                            " journal_year, " +
                            " journal_month, " +
                            " journal_day, " +
                            " created_by, " +
                            " created_date, " +
                            " branch_gid)" +
                            " values (" +
                            "'" + msGetGid + "', " +
                            "'" + values.acct_refno + "', " +
                            "'" + Convert.ToDateTime(values.transaction_date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            "'Bank Book', " +
                            "'" + lstransaction_code + "', ";
                    if (values.direct_remarks == null || values.direct_remarks == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + values.direct_remarks.Replace("'", "\\\'") + "',";
                    }
                    msSQL += "'" + lsbank_name + "', " +
                            "'" + lsbank_name + "', " +
                            "'" + values.bank_gid + "', " +
                            "'" + year + "', " +
                            "'" + month + "', " +
                            "'" + day + "', " +
                            "'" + user_gid + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            "'" + lsbranch_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL = " select distinct a.session_id, " +
                                " a.voucher_no,format(a.debit_amount,2) as debit_amount,format(a.credit_amount,2) as credit_amount," +
                                " a.account_desc,a.journal_desc," +
                                " a.dr_cr,a.transaction_amount,a.account_gid,a.total_transaction_amt,a.userid," +
                                " a.accountgroup_gid from acc_ses_journalentry a " +
                                " where a.userid = '" + user_gid + "' order by session_id asc";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetSubbank_list>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetSubbank_list
                                {
                                    dr_cr = dt["dr_cr"].ToString(),
                                    journal_desc = dt["journal_desc"].ToString(),
                                    transaction_amount = dt["transaction_amount"].ToString(),
                                });
                            }
                            for (int i = 0; i < getModuleList.Count; i++)
                            {
                                string lsdr_cr = getModuleList[i].dr_cr;
                                string lsjournal_desc = getModuleList[i].journal_desc;
                                string lstransaction_amount = getModuleList[i].transaction_amount;

                                if (lsdr_cr == "Deposit")
                                {
                                    lsdrcr_value = "cr";
                                }
                                else
                                {
                                    lsdrcr_value = "dr";
                                }
                                objODBCDatareader.Close();

                                msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                                msSQL = " Insert into acc_trn_journalentrydtl " +
                                        " (journaldtl_gid, " +
                                        " journal_gid, " +
                                        " account_gid," +
                                        " transaction_gid," +
                                        " journal_type," +
                                        " remarks," +
                                        " created_by," +
                                        " created_date," +
                                        " transaction_amount)" +
                                        " values (" +
                                        "'" + msGetDlGID + "', " +
                                        "'" + msGetGid + "'," +
                                        "'" + lsaccount_gid + "'," +
                                        "'" + lsaccount_gid + "'," +
                                        "'" + lsdrcr_value + "'," +
                                        "'" + lsjournal_desc + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        "'" + lstransaction_amount + "')";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult1 != 0)
                                {
                                    msSQL = " update acc_mst_tbank set closing_amount='" + lsbalance + "', " +
                                            " updated_by = '"+ user_gid + "', " +
                                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            " where bank_gid='" + values.bank_gid + "'";
                                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }

                            if (mnResult2 != 0)
                            {
                                msSQL = " delete from acc_ses_journalentry " +
                                        " where userid = '" + user_gid + "'";
                                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult3 == 1)
                                {
                                    values.status = true;
                                    values.message = "Bank Book Entry Added Successfully !!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Bank Book Entry !!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Bank Book Entry !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Bank Book Entry !!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Bank Book Entry!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCreditorReportOpeningBlnc(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then " +
                    " b.transaction_amount end) - sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' " +
                    " then b.transaction_amount end)) as opening_amount " +
                    " from acc_trn_journalentrydtl b " +
                    " left join acc_trn_journalentry a on a.journal_gid = b.journal_gid " +
                    " where b.account_gid = '" + account_gid + "' and a.transaction_type like '%opening%'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var CreditorreportOpening_List = new List<GetCreditorreportOpening_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                string lsopening_amount = dt_datatable.Rows[0]["opening_amount"].ToString();
                if (lsopening_amount == "")
                {
                    lssopening_amount = "0.00";
                }
                else
                {
                    lssopening_amount = dt_datatable.Rows[0]["opening_amount"].ToString();
                }

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CreditorreportOpening_List.Add(new GetCreditorreportOpening_List
                    {
                        opening_balance = lssopening_amount

                    });
                    values.GetCreditorreportOpening_List = CreditorreportOpening_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetDebtorReportOpeningBlnc(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then " +
                    " b.transaction_amount end) - sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' " +
                    " then b.transaction_amount end)) as opening_amount from acc_trn_journalentrydtl b " +
                    " left join acc_trn_journalentry a on a.journal_gid = b.journal_gid " +
                    " where b.account_gid = '" + account_gid + "' and a.transaction_type like '%opening%'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var DebtorreportOpening_List = new List<GetDebtorreportOpening_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                string lsopening_amount = dt_datatable.Rows[0]["opening_amount"].ToString();
                if (lsopening_amount == "")
                {
                    lssopening_amount = "0.00";
                }
                else
                {
                    lssopening_amount = dt_datatable.Rows[0]["opening_amount"].ToString();
                }

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    DebtorreportOpening_List.Add(new GetDebtorreportOpening_List
                    {
                        opening_balance = lssopening_amount

                    });
                    values.GetDebtorreportOpening_List = DebtorreportOpening_List;
                }
            }
            dt_datatable.Dispose();
        }        
        public void DaGetFinanceDashboardCount(string employee_gid, string user_gid, MdlAccTrnBankbooksummary values)
        {
            try
            {

                msSQL = " select (select count(bank_gid) from acc_mst_tbank " +
                    " where 1=1) as bank_count, " +
                    " (select count(bank_gid) from acc_mst_tcreditcard " +
                    " where 1=1) as creditcard_count," +
                    " (select count(bank_gid) from acc_mst_tbank " +
                    " where 1=1 and default_flag='Y') as bankbook_count," +
                    " (select count(DISTINCT a.branch_gid) from hrm_mst_tbranch a " +
                    " left join acc_trn_journalentry b on a.branch_gid=b.branch_gid" +
                    " left join acc_trn_journalentrydtl c on c.journal_gid=b.journal_gid" +
                    " where 1=1) as cashbook_count," +
                    " (select count(journal_gid) from acc_trn_journalentry " +
                    " where invoice_flag='Y' and journal_gid in (select journal_gid from acc_trn_journalentrydtl)) as journalentry_count," +
                    " (select count(taxfiling_gid) from acc_trn_ttaxfiling " +
                    " ) as tax_count," +
                    " (select count(customer_gid) from crm_mst_tcustomer a " +
                    " where a.account_gid in (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null)) as totaldebtor_count," +
                    " (select count(vendor_gid) from acp_mst_tvendor a" +
                    " where a.account_gid in (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null)) as totalcreditor_count, " +
                    " (select count(pettycashdtl_gid) from acc_trn_tpettycashdtl a" +
                    " LEFT JOIN acc_trn_tpettycash b ON a.pettycash_gid = b.pettycash_gid " +
                    " where b.transaction_mode = 'Fund Transfer') as fundtransfer_count, " +
                    " (select count(pettycash_gid) from acc_trn_tpettycash " +
                    " where approval_flag = 'N') as fundpending_count," +
                    " (select count(pettycash_gid) from acc_trn_tpettycash " +
                    " where approval_flag = 'Y') as fundapproved_count," +
                    " (select count(pettycash_gid) from acc_trn_tpettycash " +
                    " where approval_flag = 'R') as fundrejected_count";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getDashboardList = new List<getFinanceDashboardCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardList.Add(new getFinanceDashboardCount_List
                        {
                            bank_count = (dt["bank_count"].ToString()),
                            creditcard_count = (dt["creditcard_count"].ToString()),
                            bankbook_count = (dt["bankbook_count"].ToString()),
                            cashbook_count = (dt["cashbook_count"].ToString()),
                            journalentry_count = (dt["journalentry_count"].ToString()),
                            tax_count = (dt["tax_count"].ToString()),
                            totaldebtor_count = (dt["totaldebtor_count"].ToString()),
                            totalcreditor_count = (dt["totalcreditor_count"].ToString()),
                            fundtransfer_count = (dt["fundtransfer_count"].ToString()),
                            fundpending_count = (dt["fundpending_count"].ToString()),
                            fundapproved_count = (dt["fundapproved_count"].ToString()),
                            fundrejected_count = (dt["fundrejected_count"].ToString()),
                        });
                        values.getFinanceDashboardCount_List = getDashboardList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Dashboard count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDebtorDetailedReport(string account_gid, string customer_gid, string from_date, string to_date, MdlAccTrnBankbooksummary values)
        {
            try
            {
                msSQL = " select customer_name, customer_id from crm_mst_tcustomer where customer_gid='" + customer_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows == true)
                {
                    customer_name = objODBCDatareader["customer_name"].ToString();
                    customer_id = objODBCDatareader["customer_id"].ToString();
                }

                if ((from_date == null) || (to_date == null))
                {
                    lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                    lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    //-- from date
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    //-- to date
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }

                string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

                string[] lssplit = startdate_lsfinyear.Split('-');

                double current_openingbal = 0, closingbal = 0;

                msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
                string openning_balance = objdbconn.GetExecuteScalar(msSQL);

                if (openning_balance == "" || openning_balance == null)
                {
                    openning_balance = "0.00";
                }

                msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                        " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                        " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.account_gid = '" + account_gid + "' ";
                string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

                if (sum_creditamount == "" || sum_creditamount == null)
                {
                    sum_creditamount = "0.00";
                }

                msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                        " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                        " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.account_gid = '" + account_gid + "' ";
                string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

                if (sum_debitamount == "" || sum_debitamount == null)
                {
                    sum_debitamount = "0.00";
                }

                current_openingbal = double.Parse(openning_balance) + double.Parse(sum_debitamount) - double.Parse(sum_creditamount);

                double openning_balance_fromdate = current_openingbal;

                msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                        " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                        " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                        " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                        " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                        " from acc_trn_journalentry a " +
                        " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                        " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                        " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                        " order by a.transaction_date asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getsubbankbook_list>();

                int i = 0;

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = "select   FORMAT(g.transaction_amount, 2)  as tds from acc_trn_journalentrydtl g " +
                        " left join acc_mst_accountmapping h  on h.account_gid = g.account_gid " +
                        " left join acc_trn_journalentry j  on j.journal_gid = g.journal_gid " +
                        " where h.field_name = 'With Hold Tax' and h.module_name = 'RBL' AND h.screen_name = 'Receipt' and g.journal_gid = '"+ dt["journal_gid"].ToString() + "'";
                        string tds = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select journaldtl_gid, account_gid from acc_trn_journalentrydtl where journal_gid='" + dt["journal_gid"].ToString() +"' LIMIT 1 offset 1";
                        string journaldtl_gid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "select  concat(case when c.bank_name is null  then d.salestype_name else b.reference_type end) as account_name from " +
                        " acc_trn_journalentrydtl a" +
                        " left join acc_trn_journalentry b on a.journal_gid= b.journal_gid " +
                        " left join acc_mst_tbank c on c.bank_gid=b.reference_gid  " +
                        " left join smr_trn_tsalestype d on d.account_gid=a.account_gid " +
                        " where a.journaldtl_gid='" + journaldtl_gid + "';";
                        string account_name = objdbconn.GetExecuteScalar(msSQL);
                        
                        if (tds == "")
                        {
                            tds_amount = 0.00;
                        }
                        else
                        {
                            tds_amount = double.Parse(tds);
                        }
                        double lsdebit_amount = 0, lscredit_amount = 0;

                        closingbal = openning_balance_fromdate + double.Parse(dt["debit_amount"].ToString()) - double.Parse(dt["credit_amount"].ToString()) - tds_amount;

                        i += 1;
                        
                        getModuleList.Add(new Getsubbankbook_list
                        {
                            s_no = i,
                            journal_gid = dt["journal_gid"].ToString(),
                            journaldtl_gid = dt["journaldtl_gid"].ToString(),
                            transaction_gid = dt["transaction_gid"].ToString(),
                            bank_gid = dt["bank_gid"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            account_name = account_name,
                            remarks = dt["remarks"].ToString(),
                            journal_type = dt["journal_type"].ToString(),
                            transaction_type = dt["transaction_type"].ToString(),
                            bank_name = lsbank_name,
                            transaction_amount = dt["transaction_amount"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            opening_balance = openning_balance_fromdate.ToString("N2"),
                            closing_balance = closingbal.ToString("N2"),
                            customer_name = customer_name,
                            customer_id = customer_id,
                            tds = tds_amount.ToString("N2")
                        });
                        openning_balance_fromdate = closingbal;

                        values.Getsubbankbook_list = getModuleList;
                        values.Getsubbankbook_list = getModuleList.OrderByDescending(a => a.s_no).ToList();
                    }
                }
                else if (dt_datatable.Rows.Count == 0 && from_date == null && to_date == null)
                {
                    msSQL = "select a.transaction_date " +
                            " from acc_trn_journalentry a " +
                            " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            "where a.transaction_type not like '%Opening%' and b.account_gid= '" + account_gid + "' " +
                            "order by transaction_date asc";
                    string lsdate = objdbconn.GetExecuteScalar(msSQL);
                    DateTime parsedDate = Convert.ToDateTime(lsdate);
                    string From_date = parsedDate.ToString("yyyy-MM-dd");
                    string To_date = parsedDate.AddDays(-90).ToString("yyyy-MM-dd");

                    msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                            " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                            " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                            " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                            " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                            " from acc_trn_journalentry a " +
                            " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                            " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                            " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + To_date + "' and a.transaction_date <= '" + From_date + "' " +
                            " order by a.transaction_date asc ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msSQL = "select   FORMAT(g.transaction_amount, 2)  as tds from acc_trn_journalentrydtl g " +
                            " left join acc_mst_accountmapping h  on h.account_gid = g.account_gid " +
                            " left join acc_trn_journalentry j  on j.journal_gid = g.journal_gid " +
                            " where h.field_name = 'With Hold Tax' and h.module_name = 'RBL' AND h.screen_name = 'Receipt' and g.journal_gid = '" + dt["journal_gid"].ToString() + "'";
                            string tds = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " select journaldtl_gid, account_gid from acc_trn_journalentrydtl where journal_gid='" + dt["journal_gid"].ToString() + "' LIMIT 1 offset 1";
                            string journaldtl_gid = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "select  concat(case when c.bank_name is null  then d.salestype_name else b.reference_type end) as account_name from " +
                            " acc_trn_journalentrydtl a" +
                            " left join acc_trn_journalentry b on a.journal_gid= b.journal_gid " +
                            " left join acc_mst_tbank c on c.bank_gid=b.reference_gid  " +
                            " left join smr_trn_tsalestype d on d.account_gid=a.account_gid " +
                            " where a.journaldtl_gid='" + journaldtl_gid + "';";
                            string account_name = objdbconn.GetExecuteScalar(msSQL);

                            if (tds == "")
                            {
                                tds_amount = 0.00;
                            }
                            else
                            {
                                tds_amount = double.Parse(tds);
                            }
                            double lsdebit_amount = 0, lscredit_amount = 0;

                            closingbal = openning_balance_fromdate + double.Parse(dt["debit_amount"].ToString()) - double.Parse(dt["credit_amount"].ToString()) - tds_amount;

                            i += 1;

                            getModuleList.Add(new Getsubbankbook_list
                            {
                                s_no = i,
                                journal_gid = dt["journal_gid"].ToString(),
                                journaldtl_gid = dt["journaldtl_gid"].ToString(),
                                transaction_gid = dt["transaction_gid"].ToString(),
                                bank_gid = dt["bank_gid"].ToString(),
                                transaction_date = dt["transaction_date"].ToString(),
                                journal_refno = dt["journal_refno"].ToString(),
                                account_name = account_name,
                                remarks = dt["remarks"].ToString(),
                                journal_type = dt["journal_type"].ToString(),
                                transaction_type = dt["transaction_type"].ToString(),
                                bank_name = lsbank_name,
                                transaction_amount = dt["transaction_amount"].ToString(),
                                credit_amount = dt["credit_amount"].ToString(),
                                debit_amount = dt["debit_amount"].ToString(),
                                opening_balance = openning_balance_fromdate.ToString("N2"),
                                closing_balance = closingbal.ToString("N2"),
                                customer_name = customer_name,
                                customer_id = customer_id,
                                tds = tds_amount.ToString("N2")
                            });
                            openning_balance_fromdate = closingbal;

                            values.Getsubbankbook_list = getModuleList;
                            values.Getsubbankbook_list = getModuleList.OrderByDescending(a => a.s_no).ToList();
                        }
                    }
                }
                dt_datatable.Dispose();
                objODBCDatareader.Close();
            }
            catch (Exception ex)
            {

            }
        }
        //public void DaGetDebtorDetailedReport(string account_gid, string customer_gid, string from_date, string to_date, MdlAccTrnBankbooksummary values)
        //{
        //    try
        //    {
        //        // Consolidate queries
        //        string startdate_lsfinyear = GetCurrentFinancialYear(DateTime.Now.ToString("yyyy-MM-dd"));
        //        string[] lssplit = startdate_lsfinyear.Split('-');

        //        // Optimize date calculation
        //        string lsstart_date = from_date ?? DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");
        //        string lsend_date = to_date ?? DateTime.Now.ToString("yyyy-MM-dd");

        //        // Fetch opening balance and journal entries in one go
        //        msSQL = $@"
        //SELECT 
        //    c.customer_name, c.customer_id, ob.opening_balance,
        //    SUM(CASE WHEN b.journal_type = 'cr' THEN b.transaction_amount ELSE 0 END) AS credit_amount,
        //    SUM(CASE WHEN b.journal_type = 'dr' THEN b.transaction_amount ELSE 0 END) AS debit_amount,
        //    a.journal_gid, b.journaldtl_gid, b.transaction_gid, a.transaction_date, a.journal_refno,
        //    COALESCE(b.journal_type, '') as journal_type, b.transaction_amount
        //FROM crm_mst_tcustomer c
        //LEFT JOIN acc_trn_topeningbalance ob ON ob.account_gid = '{account_gid}' AND ob.financial_year = '{lssplit[0]}'
        //LEFT JOIN acc_trn_journalentry a ON a.transaction_date >= '{lsstart_date}' AND a.transaction_date <= '{lsend_date}'
        //LEFT JOIN acc_trn_journalentrydtl b ON a.journal_gid = b.journal_gid AND b.account_gid = '{account_gid}'
        //LEFT JOIN acc_mst_tbank bk ON bk.bank_gid = a.reference_gid
        //WHERE c.customer_gid = '{customer_gid}'
        //GROUP BY c.customer_name, c.customer_id, ob.opening_balance, a.journal_gid, b.journaldtl_gid, b.transaction_gid, a.transaction_date, a.journal_refno, b.journal_type, b.transaction_amount
        //ORDER BY a.transaction_date ASC";

        //        DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

        //        // Process data efficiently
        //        double current_openingbal = 0;
        //        var getModuleList = new List<Getsubbankbook_list>();

        //        if (dt_datatable.Rows.Count > 0)
        //        {
        //            var customerRow = dt_datatable.Rows[0];  // Extract customer info and opening balance from the first row
        //            string customer_name = customerRow["customer_name"].ToString();
        //            string customer_id = customerRow["customer_id"].ToString();
        //            current_openingbal = Convert.ToDouble(customerRow["opening_balance"].ToString());

        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                double debit_amount = Convert.ToDouble(dt["debit_amount"].ToString());
        //                double credit_amount = Convert.ToDouble(dt["credit_amount"].ToString());
        //                double closingbal = current_openingbal + debit_amount - credit_amount;

        //                getModuleList.Add(new Getsubbankbook_list
        //                {
        //                    s_no = getModuleList.Count + 1,
        //                    journal_gid = dt["journal_gid"].ToString(),
        //                    journaldtl_gid = dt["journaldtl_gid"].ToString(),
        //                    transaction_gid = dt["transaction_gid"].ToString(),
        //                    transaction_date = dt["transaction_date"].ToString(),
        //                    journal_refno = dt["journal_refno"].ToString(),
        //                    journal_type = dt["journal_type"].ToString(),
        //                    transaction_amount = dt["transaction_amount"].ToString(),
        //                    opening_balance = current_openingbal.ToString("N2"),
        //                    closing_balance = closingbal.ToString("N2"),
        //                    customer_name = customer_name,
        //                    customer_id = customer_id
        //                });

        //                current_openingbal = closingbal;  // Update for next iteration
        //            }

        //            values.Getsubbankbook_list = getModuleList.OrderByDescending(a => a.s_no).ToList();
        //        }

        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error
        //    }
        //}
        public void DaGetCreditorReportView1(MdlAccTrnBankbooksummary values, string account_gid, string from_date, string to_date)
        {
            string lsstart_date = "", lsend_date = "";

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) + double.Parse(sum_creditamount) - double.Parse(sum_debitamount);

            double openning_balance_fromdate = current_openingbal;

            
            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                    " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                    " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                    " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                    " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetCreditorreportView_List> CreditorreportView_List = new List<GetCreditorreportView_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                double closingbalance = 0;

                foreach(DataRow dt in dt_datatable.Rows)
                {
                    closingbalance = openning_balance_fromdate + double.Parse(dt["credit_amount"].ToString()) - double.Parse(dt["debit_amount"].ToString());

                    CreditorreportView_List.Add(new GetCreditorreportView_List
                    {
                        openingbalance = openingbalance.ToString("N2"),
                        closingbalance = closingbalance.ToString("N2"),
                        transaction_type = dt["transaction_type"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        account_name = dt["account_name"].ToString(),
                    });
                }
                values.GetCreditorreportView_List = CreditorreportView_List;
            }
            else if (dt_datatable.Rows.Count == 0 && from_date == null && to_date == null)
            {
                msSQL = " select a.transaction_date " +
                        " from acc_trn_journalentry a " +
                        " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " where a.transaction_type not like '%Opening%' and b.account_gid= '" + account_gid + "' " +
                        " order by a.transaction_date asc";
                string lsdate = objdbconn.GetExecuteScalar(msSQL);
                DateTime parsedDate = Convert.ToDateTime(lsdate);
                string From_date = parsedDate.ToString("yyyy-MM-dd");
                string To_date = parsedDate.AddDays(-90).ToString("yyyy-MM-dd");

                msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                        " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                        " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                        " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                        " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                        " from acc_trn_journalentry a " +
                        " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                        " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                        " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + To_date + "' and a.transaction_date <= '" + From_date + "' " +
                        " order by a.transaction_date asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    decimal openingbalance = 0;
                    double closingbalance = 0;

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        closingbalance = openning_balance_fromdate + double.Parse(dt["credit_amount"].ToString()) - double.Parse(dt["debit_amount"].ToString());

                        CreditorreportView_List.Add(new GetCreditorreportView_List
                        {
                            openingbalance = openingbalance.ToString("N2"),
                            closingbalance = closingbalance.ToString("N2"),
                            transaction_type = dt["transaction_type"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            account_name = dt["account_name"].ToString(),
                        });
                    }
                    values.GetCreditorreportView_List = CreditorreportView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaJournalEntryBookReport(MdlAccTrnBankbooksummary values)
        {
            var GetJournalEntrybook_list = new List<GetJournalEntrybook_list>();
           
            string msSQL = "CALL acc_rpt_spjournalentry()";
            DataTable dtJournalBookEntries = objdbconn.GetDataTable(msSQL);

            if (dtJournalBookEntries.Rows.Count > 0)
            {
               
                string msSQL1 = @"
            SELECT 
                account_gid, 
                accountgroup_gid, 
                accountgroup_name 
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
               
                foreach (DataRow ds in dtJournalBookEntries.Rows)
                {
                    string account_gid = ds["account_gid"].ToString();
                    string accountgroup_gid = ds["accountgroup_gid"].ToString();

                    string subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                    string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";
                   
                    if (mainGroup_name == "$")
                    {
                        mainGroup_name = subgroup_name;
                        subgroup_name = "-";
                    }

                    GetJournalEntrybook_list.Add(new GetJournalEntrybook_list
                    {
                        account_code = ds["account_code"].ToString(),
                        account_name = ds["account_name"].ToString(),
                        accountgroup_gid = accountgroup_gid,
                        accountgroup_name = ds["accountgroup_name"].ToString(),
                        account_gid = account_gid,
                        debit_amount = ds["debit_amount"].ToString(),
                        credit_amount = ds["credit_amount"].ToString(),
                        closing_amount = ds["closing_amount"].ToString(),
                        account_subgroup = subgroup_name,
                        opening_amount = ds["opening_amount"].ToString(),
                    });
                }               
                values.GetJournalEntrybook_list = GetJournalEntrybook_list;
            }
        }
        public void DaLiabilityLedgerNameDropdown(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select account_gid, account_code, account_name, concat(account_code, ' - ', upper(account_name)) as account, gl_code, accountgroup_name, " +
                    " case when(ledger_type= 'N' and display_type = 'N') then 'Liability' end as display_type, if (has_child = 'Y','Y','N') as has_child, " +
                    " if (ledger_type = 'Y','P/L','BS') as ledger_type, IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                    " from acc_mst_tchartofaccount where ledger_type = 'N' and display_type = 'N' and accountgroup_name = '$' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count != 0)
            {
                var accountname_List = new List<GetLiabilityaccountname_List>();

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select account_gid, account_name, accountgroup_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "' ";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable1.Rows)
                        {
                            msSQL = " select account_gid, concat(account_code,' || ', upper(account_name)) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt1["account_gid"] + "' ";
                            dt_datatable2 = objdbconn.GetDataTable(msSQL);

                            if (dt_datatable2.Rows.Count != 0)
                            {
                                foreach (DataRow dt2 in dt_datatable2.Rows)
                                {
                                    accountname_List.Add(new GetLiabilityaccountname_List
                                    {
                                        account_gid = dt2["account_gid"].ToString(),
                                        account_name = dt2["account_name"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                }
                values.GetLiabilityaccountname_List = accountname_List;
            }
            dt_datatable.Dispose();
        }
        public void DaAssetLedgerNameDropdown(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code, accountgroup_name, " +
                    " case when (ledger_type='N' and display_type='N') then 'Asset' end as display_type, if(has_child='Y','Y','N') as has_child, " +
                    " if(ledger_type='Y','P/L','BS') as ledger_type, IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                    " from acc_mst_tchartofaccount where ledger_type='N' and display_type='Y' and accountgroup_name='$' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count != 0)
            {
                var accountname_List = new List<GetAssetaccountname_List>();

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select account_gid, account_name, accountgroup_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "' ";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable1.Rows)
                        {
                            msSQL = " select account_gid, concat(account_code,' || ', upper(account_name)) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt1["account_gid"] + "' ";
                            dt_datatable2 = objdbconn.GetDataTable(msSQL);

                            if (dt_datatable2.Rows.Count != 0)
                            {
                                foreach (DataRow dt2 in dt_datatable2.Rows)
                                {
                                    accountname_List.Add(new GetAssetaccountname_List
                                    {
                                        account_gid = dt2["account_gid"].ToString(),
                                        account_name = dt2["account_name"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                }
                values.GetAssetaccountname_List = accountname_List;
            }
            dt_datatable.Dispose();
        }
        public void DaIncomeLedgerNameDropdown(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code, accountgroup_name, " +
                    " case when (ledger_type='Y' and display_type='Y') then 'Income' end as display_type,if(has_child='Y','Y','N') as has_child, " +
                    " if(ledger_type='Y','P/L','BS') as ledger_type, IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                    " from acc_mst_tchartofaccount where ledger_type='Y' and display_type='Y' and accountgroup_name='$' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count != 0)
            {
                var accountname_List = new List<GetIncomeaccountname_List>();

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select account_gid, account_name, accountgroup_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "' ";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable1.Rows)
                        {
                            msSQL = " select account_gid, concat(account_code,' || ', upper(account_name)) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt1["account_gid"] + "' ";
                            dt_datatable2 = objdbconn.GetDataTable(msSQL);

                            if (dt_datatable2.Rows.Count != 0)
                            {
                                foreach (DataRow dt2 in dt_datatable2.Rows)
                                {
                                    accountname_List.Add(new GetIncomeaccountname_List
                                    {
                                        account_gid = dt2["account_gid"].ToString(),
                                        account_name = dt2["account_name"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                }
                values.GetIncomeaccountname_List = accountname_List;
            }
            dt_datatable.Dispose();
        }
        public void DaExpenseLedgerNameDropdown(string user_gid, MdlAccTrnBankbooksummary values)
        {
            msSQL = " select account_gid, account_code, account_name, concat(account_code,' - ',upper(account_name)) as account, gl_code,accountgroup_name, " +
                    " case when (ledger_type='Y' and display_type='N') then 'Expense' end as display_type, if(has_child='Y','Y','N') as has_child, " +
                    " if(ledger_type='Y','P/L','BS') as ledger_type, IF(accountgroup_gid = '$', NULL, accountgroup_gid) as group_id " +
                    " from acc_mst_tchartofaccount where ledger_type='Y' and display_type='N' and accountgroup_name='$' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count != 0)
            {
                var accountname_List = new List<GetExpenseaccountname_List>();

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select account_gid, account_name, accountgroup_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt["account_gid"].ToString() + "' ";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable1.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable1.Rows)
                        {
                            msSQL = " select account_gid, concat(account_code,' || ', upper(account_name)) as account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + dt1["account_gid"] + "' ";
                            dt_datatable2 = objdbconn.GetDataTable(msSQL);

                            if (dt_datatable2.Rows.Count != 0)
                            {
                                foreach (DataRow dt2 in dt_datatable2.Rows)
                                {
                                    accountname_List.Add(new GetExpenseaccountname_List
                                    {
                                        account_gid = dt2["account_gid"].ToString(),
                                        account_name = dt2["account_name"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                }
                values.GetExpenseaccountname_List = accountname_List;
            }
            dt_datatable.Dispose();
        }
        public void DaIncomeLedgerReportSummary(MdlAccTrnBankbooksummary values)
        {
            var getModuleList = new List<IncomeLedgerReport_list>();

            msSQL = " select a.journal_gid,a.transaction_date,a.branch_gid,a.journal_year,a.transaction_gid, " +
                    " format(sum(b.transaction_amount),2) as transaction_amount,b.account_gid, concat(c.account_code, ' || ', c.account_name) account_name, c.accountgroup_gid,c.accountgroup_name, " +
                    " format(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end),2) as credit_amount, " +
                    " format(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then b.transaction_amount end),2) as debit_amount, " +
                    " format(coalesce(d.opening_balance, 0.00) + " +
                    " sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end) - sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then " +
                    " b.transaction_amount end),2) as closing_amount, " +
                    " format(coalesce(d.opening_balance, 0.00), 2) AS opening_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tchartofaccount c on c.account_gid = b.account_gid " +
                    " left join acc_trn_topeningbalance d on d.account_gid = b.account_gid " +
                    " where c.ledger_type='Y' and c.display_type='Y' group by c.account_name order by a.transaction_date desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count > 0)
            {

                string msSQL1 = @"
                                SELECT 
                                account_gid, 
                                accountgroup_gid, 
                                accountgroup_name 
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

                    string subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                    string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                    if (mainGroup_name == "$")
                    {
                        mainGroup_name = subgroup_name;
                        subgroup_name = "-";
                    }

                    getModuleList.Add(new IncomeLedgerReport_list
                    {
                        journal_gid = ds["journal_gid"].ToString(),
                        transaction_date = ds["transaction_date"].ToString(),
                        branch_gid = ds["branch_gid"].ToString(),
                        journal_year = ds["journal_year"].ToString(),
                        transaction_gid = ds["transaction_gid"].ToString(),
                        transaction_amount = ds["transaction_amount"].ToString(),
                        account_gid = ds["account_gid"].ToString(),
                        account_name = ds["account_name"].ToString(),
                        opening_amount = ds["opening_amount"].ToString(),
                        debit_amount = ds["debit_amount"].ToString(),
                        credit_amount = ds["credit_amount"].ToString(),
                        closing_amount = ds["closing_amount"].ToString(),
                        subgroup_name = subgroup_name,
                        MainGroup_name = mainGroup_name,

                    });
                }


                values.IncomeLedgerReport_list = getModuleList;
            }
            dt_datatable.Dispose();
        }
        public void DaExpenseLedgerReportSummary(MdlAccTrnBankbooksummary values)
        {
            var getModuleList = new List<ExpenseLedgerReport_list>();

            msSQL = " select a.journal_gid,a.transaction_date,a.branch_gid,a.journal_year,a.transaction_gid, " +
                    " format(sum(b.transaction_amount),2) as transaction_amount,b.account_gid, concat(c.account_code,' || ',c.account_name) as account_name,c.accountgroup_gid,c.accountgroup_name, " +
                    " format(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end),2) as credit_amount, " +
                    " format(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then b.transaction_amount end),2) as debit_amount, " +
                    " format(coalesce(d.opening_balance, 0.00) + " +
                    " sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end) - sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then " +
                    " b.transaction_amount end),2) as closing_amount, format(coalesce(d.opening_balance, 0.00), 2) AS opening_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tchartofaccount c on c.account_gid = b.account_gid " +
                    " left join acc_trn_topeningbalance d on d.account_gid = b.account_gid " +
                    " where c.ledger_type='Y' and c.display_type='N' group by c.account_name order by a.transaction_date desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count > 0)
            {

                string msSQL1 = @"
                                SELECT 
                                account_gid, 
                                accountgroup_gid, 
                                accountgroup_name 
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

                    string subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                    string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                    if (mainGroup_name == "$")
                    {
                        mainGroup_name = subgroup_name;
                        subgroup_name = "-";
                    }

                    getModuleList.Add(new ExpenseLedgerReport_list
                    {
                        journal_gid = ds["journal_gid"].ToString(),
                        transaction_date = ds["transaction_date"].ToString(),
                        branch_gid = ds["branch_gid"].ToString(),
                        journal_year = ds["journal_year"].ToString(),
                        transaction_gid = ds["transaction_gid"].ToString(),
                        transaction_amount = ds["transaction_amount"].ToString(),
                        account_gid = ds["account_gid"].ToString(),
                        account_name = ds["account_name"].ToString(),
                        opening_amount = ds["opening_amount"].ToString(),
                        debit_amount = ds["debit_amount"].ToString(),
                        credit_amount = ds["credit_amount"].ToString(),
                        closing_amount = ds["closing_amount"].ToString(),
                        subgroup_name = subgroup_name,
                        MainGroup_name = mainGroup_name,

                    });
                }


                values.ExpenseLedgerReport_list = getModuleList;
            }

            dt_datatable.Dispose();
        }
        public void DaAssetLedgerReportSummary(string user_gid, MdlAccTrnBankbooksummary values)
        {
            var Assetreport_List = new List<GetAssetLedgerReportSummary_List>();

            msSQL = " SELECT distinct a.customer_name as customer, a.account_gid,d.accountgroup_gid, a.customer_id, a.customer_gid, concat(a.customer_id,' || ',upper(customer_name)) as customer_name," +
                     " concat(case when k.customercontact_name is null then '' else k.customercontact_name end,'/',case when k.email is null then '' else k.email end," +
                     " '/',case when k.mobile is null then '' else k.mobile end) as contact, concat(datediff(current_date(), min(a.created_date)), ' days') as customer_since," +
                     " format(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then " +
                     " b.transaction_amount end),2) " +
                     " as credit_amount,format(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' " +
                     " then b.transaction_amount end),2) as debit_amount, " +
                     " format(case when cast(c.financial_year as unsigned) = year(current_date()) then c.opening_balance else '0.00' end + " +
                     " sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr'" +
                     " then b.transaction_amount end) -sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then " +
                     " b.transaction_amount end),2) as closing_amount, " +
                     " format(case when cast(c.financial_year as unsigned) = year(current_date()) then c.opening_balance else '0.00' end,2) as opening_amount " +
                     " FROM crm_mst_tcustomer a " +
                     " left join crm_mst_tcustomercontact k on a.customer_gid=k.customer_gid " +
                     " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                     " left join acc_trn_topeningbalance c on c.account_gid = b.account_gid " +
                     " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid  " +
                     " where a.account_gid in " +
                     " (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null) " +
                     " group by account_gid order by a.customer_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);


            //if (dt_datatable.Rows.Count != 0)
            //{
            //    foreach (DataRow dt in dt_datatable.Rows)
            //    {
            //        msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + dt["account_gid"] + "'";
            //        objODBCDatareader = objdbconn.GetDataReader(msSQL1);

            //        if (objODBCDatareader.HasRows == true)
            //        {
            //            string subgroup_name = objODBCDatareader["accountgroup_name"].ToString();
            //            string accountgroup_gid = objODBCDatareader["accountgroup_gid"].ToString();

            //            msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
            //            string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);

            //            if (MainGroup_name == "$")
            //            {
            //                MainGroup_name = subgroup_name;
            //                subgroup_name = "-";
            //            }

            //            Assetreport_List.Add(new GetAssetLedgerReportSummary_List
            //            {
            //                account_gid = dt["account_gid"].ToString(),
            //                customer_name = dt["customer_name"].ToString(),
            //                opening_amount = dt["opening_amount"].ToString(),
            //                debit_amount = dt["debit_amount"].ToString(),
            //                credit_amount = dt["credit_amount"].ToString(),
            //                closing_amount = dt["closing_amount"].ToString(),
            //                customer_id = dt["customer_id"].ToString(),
            //                contact = dt["contact"].ToString(),
            //                customer_since = dt["customer_since"].ToString(),
            //                customer_gid = dt["customer_gid"].ToString(),
            //                subgroup_name = subgroup_name,
            //                MainGroup_name = MainGroup_name,
            //            });
            //            values.GetAssetLedgerReportSummary_List = Assetreport_List;
            //        }
            //    }
            //    objODBCDatareader.Close();
            //}

            if (dt_datatable.Rows.Count > 0)
            {

                string msSQL1 = @"
                    SELECT 
                    account_gid, 
                    accountgroup_gid, 
                    accountgroup_name 
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

                    string subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                    string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                    if (mainGroup_name == "$")
                    {
                        mainGroup_name = subgroup_name;
                        subgroup_name = "-";
                    }

                    Assetreport_List.Add(new GetAssetLedgerReportSummary_List
                    {
                        account_gid = ds["account_gid"].ToString(),
                        customer_name = ds["customer_name"].ToString(),
                        opening_amount = ds["opening_amount"].ToString(),
                        debit_amount = ds["debit_amount"].ToString(),
                        credit_amount = ds["credit_amount"].ToString(),
                        closing_amount = ds["closing_amount"].ToString(),
                        customer_id = ds["customer_id"].ToString(),
                        contact = ds["contact"].ToString(),
                        customer_since = ds["customer_since"].ToString(),
                        customer_gid = ds["customer_gid"].ToString(),
                        subgroup_name = subgroup_name,
                        MainGroup_name = mainGroup_name,

                    });
                }


                values.GetAssetLedgerReportSummary_List = Assetreport_List;
            }

            dt_datatable.Dispose();
        }
        public void DaAssetLedgerReportDetails(string account_gid, string customer_gid, string from_date, string to_date, MdlAccTrnBankbooksummary values)
        {
            try
            {
                msSQL = " select customer_name, customer_id from crm_mst_tcustomer where customer_gid='" + customer_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    customer_name = objODBCDatareader["customer_name"].ToString();
                    customer_id = objODBCDatareader["customer_id"].ToString();
                }

                if ((from_date == null) || (to_date == null))
                {
                    lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                    lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    //-- from date
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    //-- to date
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }

                string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

                string[] lssplit = startdate_lsfinyear.Split('-');

                double current_openingbal = 0, closingbal = 0;

                msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
                string openning_balance = objdbconn.GetExecuteScalar(msSQL);

                if (openning_balance == "" || openning_balance == null)
                {
                    openning_balance = "0.00";
                }

                msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                        " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                        " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.account_gid = '" + account_gid + "' ";
                string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

                if (sum_creditamount == "" || sum_creditamount == null)
                {
                    sum_creditamount = "0.00";
                }

                msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                        " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                        " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.account_gid = '" + account_gid + "' ";
                string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

                if (sum_debitamount == "" || sum_debitamount == null)
                {
                    sum_debitamount = "0.00";
                }

                current_openingbal = double.Parse(openning_balance) + double.Parse(sum_creditamount) - double.Parse(sum_debitamount)  ;

                double openning_balance_fromdate = current_openingbal;

                msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                        " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                        " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                        " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                        " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount,d.account_gid,d.accountgroup_gid " +
                        " from acc_trn_journalentry a " +
                        " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                        " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                        " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                        " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                        " order by a.transaction_date asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getassetledgerreport_list>();

                int i = 0;

                if (dt_datatable.Rows.Count != 0)
                {
                    string msSQL1 = @"
                    SELECT 
                    account_gid, 
                    accountgroup_gid, 
                    accountgroup_name 
                    FROM acc_mst_tchartofaccount";
                    DataTable dtAccountGroups = objdbconn.GetDataTable(msSQL1);


                    var subgroupNameDict = new Dictionary<string, string>();
                    var mainGroupNameDict = new Dictionary<string, string>();


                    foreach (DataRow row in dtAccountGroups.Rows)
                    {
                        string account_gid1 = row["account_gid"].ToString();
                        string accountgroup_gid = row["accountgroup_gid"].ToString();
                        string accountgroup_name = row["accountgroup_name"].ToString();


                        if (!subgroupNameDict.ContainsKey(account_gid1))
                        {
                            subgroupNameDict[account_gid1] = accountgroup_name;
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

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string account_gid1 = dt["account_gid"].ToString();
                        string accountgroup_gid = dt["accountgroup_gid"].ToString();

                        string subgroup_name = subgroupNameDict.ContainsKey(account_gid1) ? subgroupNameDict[account_gid1] : "-";
                        string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                        if (mainGroup_name == "$")
                        {
                            mainGroup_name = subgroup_name;
                            subgroup_name = "-";
                        }

                        double lsdebit_amount = 0, lscredit_amount = 0;

                        closingbal = openning_balance_fromdate + double.Parse(dt["credit_amount"].ToString()) - double.Parse(dt["debit_amount"].ToString());

                        i += 1;
                            getModuleList.Add(new Getassetledgerreport_list
                            {
                                s_no = i,
                                journal_gid = dt["journal_gid"].ToString(),
                                journaldtl_gid = dt["journaldtl_gid"].ToString(),
                                transaction_gid = dt["transaction_gid"].ToString(),
                                bank_gid = dt["bank_gid"].ToString(),
                                transaction_date = dt["transaction_date"].ToString(),
                                journal_refno = dt["journal_refno"].ToString(),
                                account_name = dt["account_name"].ToString(),
                                remarks = dt["remarks"].ToString(),
                                journal_type = dt["journal_type"].ToString(),
                                transaction_type = dt["transaction_type"].ToString(),
                                bank_name = lsbank_name,
                                transaction_amount = dt["transaction_amount"].ToString(),
                                credit_amount = dt["credit_amount"].ToString(),
                                debit_amount = dt["debit_amount"].ToString(),
                                opening_balance = openning_balance_fromdate.ToString("N2"),
                                closing_balance = closingbal.ToString("N2"),
                                customer_name = customer_name,
                                customer_id = customer_id,
                                subgroup_name = subgroup_name,
                                MainGroup_name = mainGroup_name,
                            });
                            openning_balance_fromdate = closingbal;

                            values.Getassetledgerreport_list = getModuleList;
                            values.Getassetledgerreport_list = getModuleList.OrderByDescending(a => a.s_no).ToList();
                        
                    }
                   
                }
                else if (dt_datatable.Rows.Count == 0 && from_date == null && to_date == null)
                {
                    msSQL = " select a.transaction_date " +
                      " from acc_trn_journalentry a " +
                      " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                      " where a.transaction_type not like '%Opening%' and b.account_gid= '" + account_gid + "' " +
                      " order by a.transaction_date asc";
                    string lsdate = objdbconn.GetExecuteScalar(msSQL);
                    DateTime parsedDate = Convert.ToDateTime(lsdate);
                    string From_date = parsedDate.ToString("yyyy-MM-dd");
                    string To_date = parsedDate.AddDays(-90).ToString("yyyy-MM-dd");

                    msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                            " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                            " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                            " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                            " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount,d.account_gid,d.accountgroup_gid " +
                            " from acc_trn_journalentry a " +
                            " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                            " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                            " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                            " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + To_date + "' and a.transaction_date <= '" + From_date + "' " +
                            " order by a.transaction_date asc ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable.Rows.Count != 0)
                    {
                        string msSQL1 = @"
                    SELECT 
                    account_gid, 
                    accountgroup_gid, 
                    accountgroup_name 
                    FROM acc_mst_tchartofaccount";
                        DataTable dtAccountGroups = objdbconn.GetDataTable(msSQL1);


                        var subgroupNameDict = new Dictionary<string, string>();
                        var mainGroupNameDict = new Dictionary<string, string>();


                        foreach (DataRow row in dtAccountGroups.Rows)
                        {
                            string account_gid1 = row["account_gid"].ToString();
                            string accountgroup_gid = row["accountgroup_gid"].ToString();
                            string accountgroup_name = row["accountgroup_name"].ToString();


                            if (!subgroupNameDict.ContainsKey(account_gid1))
                            {
                                subgroupNameDict[account_gid1] = accountgroup_name;
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

                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            string account_gid1 = dt["account_gid"].ToString();
                            string accountgroup_gid = dt["accountgroup_gid"].ToString();

                            string subgroup_name = subgroupNameDict.ContainsKey(account_gid1) ? subgroupNameDict[account_gid1] : "-";
                            string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                            if (mainGroup_name == "$")
                            {
                                mainGroup_name = subgroup_name;
                                subgroup_name = "-";
                            }

                            double lsdebit_amount = 0, lscredit_amount = 0;

                            closingbal = openning_balance_fromdate + double.Parse(dt["credit_amount"].ToString()) - double.Parse(dt["debit_amount"].ToString());

                            i += 1;
                            getModuleList.Add(new Getassetledgerreport_list
                            {
                                s_no = i,
                                journal_gid = dt["journal_gid"].ToString(),
                                journaldtl_gid = dt["journaldtl_gid"].ToString(),
                                transaction_gid = dt["transaction_gid"].ToString(),
                                bank_gid = dt["bank_gid"].ToString(),
                                transaction_date = dt["transaction_date"].ToString(),
                                journal_refno = dt["journal_refno"].ToString(),
                                account_name = dt["account_name"].ToString(),
                                remarks = dt["remarks"].ToString(),
                                journal_type = dt["journal_type"].ToString(),
                                transaction_type = dt["transaction_type"].ToString(),
                                bank_name = lsbank_name,
                                transaction_amount = dt["transaction_amount"].ToString(),
                                credit_amount = dt["credit_amount"].ToString(),
                                debit_amount = dt["debit_amount"].ToString(),
                                opening_balance = openning_balance_fromdate.ToString("N2"),
                                closing_balance = closingbal.ToString("N2"),
                                customer_name = customer_name,
                                customer_id = customer_id,
                                subgroup_name = subgroup_name,
                                MainGroup_name = mainGroup_name,
                            });
                            openning_balance_fromdate = closingbal;

                            values.Getassetledgerreport_list = getModuleList;
                            values.Getassetledgerreport_list = getModuleList.OrderByDescending(a => a.s_no).ToList();

                        }

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void DaLiabilityLedgerReportSummary(string user_gid, MdlAccTrnBankbooksummary values)
        {
            var Liabilityreport_List = new List<GetLiabilityLedgerReportSummary_List>();

            msSQL = "SELECT distinct a.vendor_gid,a.vendor_code, concat(a.vendor_code,' || ',upper(a.vendor_companyname)) as vendor_companyname,a.account_gid,d.accountgroup_gid, " +
                    " format(sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end),2) as credit_amount, " +
                    " format(sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then b.transaction_amount end),2) as debit_amount, " +
                    " format(coalesce(c.opening_balance, 0.00) + sum(case when b.journal_type = 'dr' then '0.00' when b.journal_type = 'cr' then b.transaction_amount end) " +
                    " -sum(case when b.journal_type = 'cr' then '0.00' when b.journal_type = 'dr' then  b.transaction_amount end),2) as closing_amount, " +
                    " format(coalesce(c.opening_balance, 0.00), 2) AS opening_amount " +
                    " FROM acp_mst_tvendor a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " left join acc_trn_topeningbalance c on c.account_gid = b.account_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = a.account_gid " +
                    " where a.account_gid  in " +
                    " (select b.account_gid from acc_trn_journalentrydtl b where b.account_gid is not null) " +
                    " group by account_gid order by a.vendor_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (dt_datatable.Rows.Count > 0)
            {

                string msSQL1 = @"
                                SELECT 
                                account_gid, 
                                accountgroup_gid, 
                                accountgroup_name 
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

                    string subgroup_name = subgroupNameDict.ContainsKey(account_gid) ? subgroupNameDict[account_gid] : "-";
                    string mainGroup_name = mainGroupNameDict.ContainsKey(accountgroup_gid) ? mainGroupNameDict[accountgroup_gid] : "-";


                    if (mainGroup_name == "$")
                    {
                        mainGroup_name = subgroup_name;
                        subgroup_name = "-";
                    }

                    Liabilityreport_List.Add(new GetLiabilityLedgerReportSummary_List
                    {
                        account_gid = ds["account_gid"].ToString(),
                        vendor_gid = ds["vendor_gid"].ToString(),
                        vendor_companyname = ds["vendor_companyname"].ToString(),
                        credit_amount = ds["credit_amount"].ToString(),
                        debit_amount = ds["debit_amount"].ToString(),
                        closing_amount = ds["closing_amount"].ToString(),
                        opening_amount = ds["opening_amount"].ToString(),
                        vendor_code = ds["vendor_code"].ToString(),
                        subgroup_name = subgroup_name,
                        MainGroup_name = mainGroup_name,

                    });
                }


                values.GetLiabilityLedgerReportSummary_List = Liabilityreport_List;
            }

            dt_datatable.Dispose();
        }
        public void DaLiabilityLedgerReportDetails(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select a.vendor_code,a.vendor_companyname from acp_mst_tvendor a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " where b.account_gid = '" + account_gid + "' group by b.account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var LiabilityLedgerReportDetails_List = new List<GetLiabilityLedgerReportDetails_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    LiabilityLedgerReportDetails_List.Add(new GetLiabilityLedgerReportDetails_List
                    {
                        vendor_name = dt["vendor_companyname"].ToString(),
                        vendor_code = dt["vendor_code"].ToString()

                    });
                    values.GetLiabilityLedgerReportDetails_List = LiabilityLedgerReportDetails_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaLiabilityLedgerReportDetailsList(MdlAccTrnBankbooksummary values, string account_gid, string from_date, string to_date)
        {
            string lsstart_date = "", lsend_date = "";

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) + double.Parse(sum_debitamount) - double.Parse(sum_creditamount);

            double openning_balance_fromdate = current_openingbal;


            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                    " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                    " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                    " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                    " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var LiabilityLedgerReportView_List = new List<GetLiabilityLedgerReportView_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                double closingbalance = 0;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                        closingbalance = openning_balance_fromdate + double.Parse(dt["debit_amount"].ToString()) - double.Parse(dt["credit_amount"].ToString());

                        LiabilityLedgerReportView_List.Add(new GetLiabilityLedgerReportView_List
                        {
                            openingbalance = openingbalance.ToString(),
                            closingbalance = closingbalance.ToString("N2"),
                            transaction_type = dt["transaction_type"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            account_name = dt["account_name"].ToString(),
                        });
                    
                    values.GetLiabilityLedgerReportView_List = LiabilityLedgerReportView_List;
                }
            }
            else if (dt_datatable.Rows.Count == 0 && from_date == null && to_date == null)
            {
                msSQL = " select a.transaction_date " +
                       " from acc_trn_journalentry a " +
                       " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                       " where a.transaction_type not like '%Opening%' and b.account_gid= '" + account_gid + "' " +
                       " order by a.transaction_date asc";
                string lsdate = objdbconn.GetExecuteScalar(msSQL);
                DateTime parsedDate = Convert.ToDateTime(lsdate);
                string From_date = parsedDate.ToString("yyyy-MM-dd");
                string To_date = parsedDate.AddDays(-90).ToString("yyyy-MM-dd");

                msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, c.account_no, " +
                   " d.account_name as acc_name, a.remarks,concat(case when c.bank_gid is null then a.journal_from else a.reference_type end) as account_name, " +
                   " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount," +
                   " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                   " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                   " from acc_trn_journalentry a " +
                   " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                   " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
                   " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                   " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + To_date + "' and a.transaction_date <= '" + From_date + "' " +
                   " order by a.transaction_date asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {
                    decimal openingbalance = 0;
                    double closingbalance = 0;

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        closingbalance = openning_balance_fromdate + double.Parse(dt["debit_amount"].ToString()) - double.Parse(dt["credit_amount"].ToString());

                        LiabilityLedgerReportView_List.Add(new GetLiabilityLedgerReportView_List
                        {
                            openingbalance = openingbalance.ToString(),
                            closingbalance = closingbalance.ToString("N2"),
                            transaction_type = dt["transaction_type"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            account_name = dt["account_name"].ToString(),
                        });

                        values.GetLiabilityLedgerReportView_List = LiabilityLedgerReportView_List;
                    }
                }
            }
            dt_datatable.Dispose();
           
        }
        public void DaGetLedgerBookReport(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select journaldtl_gid,DATE_FORMAT(transaction_date, '%d-%m-%Y') as transaction_date ," +
                    " account_name as account_desc,remarks,format(credit_amount, 2) as credit_amount, " +
                    " format(debit_amount, 2) as debit_amount,account_gid,reference_gid,bank_name,account_no," +
                    " journal_gid,journal_refno,invoice_from,closing_amount from " +
                    " (select journaldtl_gid, transaction_date, account_name, remarks, credit_amount, debit_amount, " +
                    " account_gid, reference_gid, bank_name, account_no, journal_gid, journal_refno, " +
                    " format((@runtot := (credit_amount - debit_amount + @runtot)),2) as closing_amount,invoice_from from " +
                    " (select a.journaldtl_gid, b.transaction_date, e.account_name, a.remarks, a.account_gid, b.reference_gid, c.bank_name, " +
                    " c.account_no, a.journal_gid, b.journal_refno, (case when a.journal_type = 'dr' then '0.00' when a.journal_type = 'cr' then " +
                    " transaction_amount end)as credit_amount,(case when a.journal_type = 'cr' then '0.00' when a.journal_type = 'dr' " +
                    " then transaction_amount end) as debit_amount,(case when b.transaction_type<> 'Journal' " +
                    " then 'Payment' else b.journal_from end) as invoice_from " +
                    " from acc_trn_journalentrydtl a " +
                    " left join acc_trn_journalentry b on a.journal_gid = b.journal_gid " +
                    " left join acc_mst_tbank c on c.bank_gid = b.reference_gid " +
                    " left join hrm_mst_tbranch f on f.branch_gid = b.reference_gid " +
                    " left join acc_mst_tchartofaccount e on e.account_gid = a.account_gid, " +
                    " (SELECT @runtot:= 0) d " +
                    " where(a.account_gid<> '" + account_gid + "' and a.transaction_gid = '" + account_gid + "') " +
                    " order by b.transaction_date asc, a.journaldtl_gid asc) x) y " +
                    " order by date(y.transaction_date) desc, date(y.transaction_date) asc,y.journaldtl_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var Ledgerbook_List = new List<GetLedgerbook_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    Ledgerbook_List.Add(new GetLedgerbook_List
                    {
                        transaction_date = dt["transaction_date"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        account_desc = dt["account_desc"].ToString(),
                        type = dt["invoice_from"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        closing_amount = dt["closing_amount"].ToString()

                    });
                    values.GetLedgerbook_List = Ledgerbook_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaIncomeLedgerReportDetailsList(MdlAccTrnBankbooksummary values, string account_gid, string from_date, string to_date)
        {
            string lsstart_date = "", lsend_date = "";

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) + double.Parse(sum_creditamount) - double.Parse(sum_debitamount);

            double openning_balance_fromdate = current_openingbal;


            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.customer_code, c.customer_name, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, " +
                    " d.account_name, a.remarks, a.reference_gid, " +
                    " b.journal_type, a.transaction_type, format(b.transaction_amount, 2) as transaction_amount," +
                    " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                    " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join crm_mst_tcustomer c on c.customer_gid = a.reference_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetIncomeLedgerReportView_List> IncomeLedgerReportView_List = new List<GetIncomeLedgerReportView_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                double closingbalance = 0;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    //msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + account_gid + "'";
                    //objODBCDatareader = objdbconn.GetDataReader(msSQL1);

                    //if (objODBCDatareader.HasRows == true)
                    //{
                    //    string subgroup_name = objODBCDatareader["accountgroup_name"].ToString();
                    //    string accountgroup_gid = objODBCDatareader["accountgroup_gid"].ToString();

                    //    msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                    //    string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);

                    //    if (MainGroup_name == "$")
                    //    {
                    //        MainGroup_name = subgroup_name;
                    //        subgroup_name = "-";
                    //    }
                    //}

                        closingbalance = openning_balance_fromdate + double.Parse(dt["credit_amount"].ToString()) - double.Parse(dt["debit_amount"].ToString());

                        IncomeLedgerReportView_List.Add(new GetIncomeLedgerReportView_List
                        {
                            openingbalance = openingbalance.ToString(),
                            closingbalance = closingbalance.ToString("N2"),
                            transaction_type = dt["transaction_type"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                        });
                    
                    values.GetIncomeLedgerReportView_List = IncomeLedgerReportView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaIncomeLedgerReportDetails(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select a.customer_code, a.customer_name from crm_mst_tcustomer a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " where b.account_gid = '" + account_gid + "' group by b.account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var IncomeLedgerReportDetails_List = new List<GetIncomeLedgerReportDetails_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    IncomeLedgerReportDetails_List.Add(new GetIncomeLedgerReportDetails_List
                    {
                        customer_code = dt["customer_code"].ToString(),
                        customer_name = dt["customer_name"].ToString()

                    });
                    values.GetIncomeLedgerReportDetails_List = IncomeLedgerReportDetails_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaIncomeReportMonthwise(MdlAccTrnBankbooksummary values, string account_gid, string from_date, string to_date)
        {
            string lsstart_date = "", lsend_date = "";

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) + double.Parse(sum_creditamount) - double.Parse(sum_debitamount);

            double openning_balance_fromdate = current_openingbal;

            msSQL = " select a.journal_gid, a.reference_gid, b.journaldtl_gid, b.transaction_gid, a.reference_type,date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, " +
                    " d.account_name, a.remarks, c.customer_code, c.customer_name," +
                    " b.journal_type, a.transaction_type, format(b.transaction_amount, 2) as transaction_amount," +
                    " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                    " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join crm_mst_tcustomer c on c.customer_gid = a.reference_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetIncomeReportMonthwise_List> IncomeReportMonthwise_List = new List<GetIncomeReportMonthwise_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                double closingbalance = 0;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    closingbalance = openning_balance_fromdate + double.Parse(dt["credit_amount"].ToString()) - double.Parse(dt["debit_amount"].ToString());

                    IncomeReportMonthwise_List.Add(new GetIncomeReportMonthwise_List
                    {
                        openingbalance = openingbalance.ToString(),
                        closingbalance = closingbalance.ToString("N2"),
                        transaction_type = dt["transaction_type"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        customer_code = dt["customer_code"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                    });
                }
                values.GetIncomeReportMonthwise_List = IncomeReportMonthwise_List;
            }
            dt_datatable.Dispose();
        }
        public void DaExpenseLedgerReportDetailsList(MdlAccTrnBankbooksummary values, string account_gid, string from_date, string to_date)
        {
            string lsstart_date = "", lsend_date = "";

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) + double.Parse(sum_debitamount) - double.Parse(sum_creditamount) ;

            double openning_balance_fromdate = current_openingbal;


            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.vendor_code, c.vendor_companyname, a.reference_type, " +
                    " date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, d.account_name, a.remarks, " +
                    " b.journal_type, a.transaction_type, format(b.transaction_amount, 2) as transaction_amount," +
                    " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                    " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acp_mst_tvendor c on c.account_gid = b.account_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetExpenseLedgerReportView_List> ExpenseLedgerReportView_List = new List<GetExpenseLedgerReportView_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                double closingbalance = 0;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    //msSQL1 = "select account_gid, accountgroup_gid, upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + account_gid + "'";
                    //objODBCDatareader = objdbconn.GetDataReader(msSQL1);

                    //if (objODBCDatareader.HasRows == true)
                    //{
                    //    string subgroup_name = objODBCDatareader["accountgroup_name"].ToString();
                    //    string accountgroup_gid = objODBCDatareader["accountgroup_gid"].ToString();

                    //    msSQL2 = "select upper(accountgroup_name) as accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + accountgroup_gid + "'";
                    //    string MainGroup_name = objdbconn.GetExecuteScalar(msSQL2);

                    //    if (MainGroup_name == "$")
                    //    {
                    //        MainGroup_name = subgroup_name;
                    //        subgroup_name = "-";
                    //    }
                    //}

                        closingbalance = openning_balance_fromdate +  double.Parse(dt["debit_amount"].ToString()) - double.Parse(dt["credit_amount"].ToString());

                        ExpenseLedgerReportView_List.Add(new GetExpenseLedgerReportView_List
                        {
                            openingbalance = openingbalance.ToString(),
                            closingbalance = closingbalance.ToString("N2"),
                            transaction_type = dt["transaction_type"].ToString(),
                            journal_refno = dt["journal_refno"].ToString(),
                            transaction_date = dt["transaction_date"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            debit_amount = dt["debit_amount"].ToString(),
                            credit_amount = dt["credit_amount"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                        });
                    
                    values.GetExpenseLedgerReportView_List = ExpenseLedgerReportView_List;
                }
                //objODBCDatareader.Close();
            }
            dt_datatable.Dispose();
        }
        public void DaExpenseLedgerReportDetails(string user_gid, MdlAccTrnBankbooksummary values, string account_gid)
        {
            msSQL = " select a.vendor_code, a.vendor_companyname from acp_mst_tvendor a " +
                    " left join acc_trn_journalentrydtl b on b.account_gid = a.account_gid " +
                    " where b.account_gid = '" + account_gid + "' group by b.account_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var ExpenseLedgerReportDetails_List = new List<GetExpenseLedgerReportDetails_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    ExpenseLedgerReportDetails_List.Add(new GetExpenseLedgerReportDetails_List
                    {
                        vendor_code = dt["vendor_code"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString()

                    });
                    values.GetExpenseLedgerReportDetails_List = ExpenseLedgerReportDetails_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaExpenseReportMonthwise(MdlAccTrnBankbooksummary values, string account_gid, string from_date, string to_date)
        {
            string lsstart_date = "", lsend_date = "";

            if ((from_date == null) || (to_date == null))
            {
                lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //-- from date
                DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsstart_date = from_date1.ToString("yyyy-MM-dd");

                //-- to date
                DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                lsend_date = lsDateto.ToString("yyyy-MM-dd");
            }

            string startdate_lsfinyear = GetCurrentFinancialYear(lsstart_date);

            string[] lssplit = startdate_lsfinyear.Split('-');

            double current_openingbal = 0, closingbal = 0;

            msSQL = " select opening_balance from acc_trn_topeningbalance where account_gid = '" + account_gid + "' and financial_year = '" + lssplit[0] + "' ";
            string openning_balance = objdbconn.GetExecuteScalar(msSQL);

            if (openning_balance == "" || openning_balance == null)
            {
                openning_balance = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as credit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='cr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_creditamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_creditamount == "" || sum_creditamount == null)
            {
                sum_creditamount = "0.00";
            }

            msSQL = " select format(sum(b.transaction_amount),2) as debit_amount " +
                    " from acc_trn_journalentry a  left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " where transaction_date >='" + lssplit[0] + "-04-01' and transaction_date <'" + lsstart_date + "' " +
                    " and a.transaction_type not like '%Opening%' and b.journal_type='dr' and b.transaction_gid = '" + account_gid + "' ";
            string sum_debitamount = objdbconn.GetExecuteScalar(msSQL);

            if (sum_debitamount == "" || sum_debitamount == null)
            {
                sum_debitamount = "0.00";
            }

            current_openingbal = double.Parse(openning_balance) + double.Parse(sum_debitamount) - double.Parse(sum_creditamount);

            double openning_balance_fromdate = current_openingbal;

            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.vendor_code, c.vendor_companyname, a.reference_type, " +
                    " date_format(a.transaction_date, '%d-%b-%Y') as transaction_date, a.journal_refno, d.account_name, a.remarks, " +
                    " b.journal_type, a.transaction_type, format(b.transaction_amount, 2) as transaction_amount," +
                    " format(case when b.journal_type='dr' then 0.00 when b.journal_type='cr' then b.transaction_amount end,2) as credit_amount," +
                    " format(case when b.journal_type='cr' then 0.00 when b.journal_type='dr' then b.transaction_amount end,2) as debit_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join acp_mst_tvendor c on c.account_gid = b.account_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " where a.transaction_type not like '%Opening%' and b.account_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            List<GetExpenseReportMonthwise_List> ExpenseReportMonthwise_List = new List<GetExpenseReportMonthwise_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                decimal openingbalance = 0;
                double closingbalance = 0;

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    closingbalance = openning_balance_fromdate + double.Parse(dt["debit_amount"].ToString()) - double.Parse(dt["credit_amount"].ToString());

                    ExpenseReportMonthwise_List.Add(new GetExpenseReportMonthwise_List
                    {
                        openingbalance = openingbalance.ToString(),
                        closingbalance = closingbalance.ToString("N2"),
                        transaction_type = dt["transaction_type"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        debit_amount = dt["debit_amount"].ToString(),
                        credit_amount = dt["credit_amount"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                    });
                }
                values.GetExpenseReportMonthwise_List = ExpenseReportMonthwise_List;
            }
            dt_datatable.Dispose();
        }
        public void DaGetAccountGroupNameDropdown(MdlAccTrnBankbooksummary values)
        {
            try
            {
                msSQL = " select account_gid,concat(accountgroup_name,' | ',account_name) account_name,accountgroup_name " +
                        " from acc_mst_tchartofaccount " +
                        " where accountgroup_gid in (select account_gid " +
                        " from acc_mst_tchartofaccount " +
                        " where (ledger_type='Y' and display_type='N' and accountgroup_name='$') or " +
                        " (ledger_type='Y' and display_type='Y' and accountgroup_name='$') or " +
                        " (ledger_type='N' and display_type='N' and accountgroup_name='$') or " +
                        " (ledger_type='N' and display_type='Y' and accountgroup_name='$')) ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetAccountNameDropdown = new List<GetAccountNameDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetAccountNameDropdown.Add(new GetAccountNameDropdown
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString()

                        });
                        values.GetAccountNameDropdown = GetAccountNameDropdown;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Account Group Name Drop Down!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLedgerNameDropDownList (MdlAccTrnBankbooksummary values, string Subgroup_gid)
        {
            try
            {
                msSQL = "Select account_gid, account_name from acc_mst_tchartofaccount where accountgroup_gid = '" + Subgroup_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var Getledgername_List = new List<Getledgername_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        Getledgername_List.Add(new Getledgername_List
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString()

                        });
                        values.Getledgername_List = Getledgername_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Ledger Name Drop Down!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}