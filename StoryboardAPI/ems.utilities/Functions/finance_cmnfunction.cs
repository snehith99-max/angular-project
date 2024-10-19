using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Principal;
using System.Web;

namespace ems.utilities.Functions
{
    public class finance_cmnfunction
    {
        ems.utilities.Functions.dbconn objdbconn = new ems.utilities.Functions.dbconn();
        ems.utilities.Functions.cmnfunctions objcmnfunctions = new ems.utilities.Functions.cmnfunctions();
        string msGetGID, msGetDlGID, msGetDlGID1, msGetDlGID2, msGetSEDlGID;
        string msSQL, finyear, jounrnal_date, journal_type, reference_gid, lsaccount_gid;
        OdbcDataReader objODBCDataReader;
        DataTable dt_datatable;
        string journal_month, journal_year, journal_Day, account_gid, msgetgid = string.Empty;
        List<string> journal_monthandyear = new List<string>();
        string lswthaccount_gid, lsWHTjournal_type, customer_name, journal_gid = string.Empty;
        int mnResult;
        string lsADaccount_GID, lsADjournal_type =string.Empty;
        string SEjournal_type, LSTransactionCode, LSTransactionType, msGetD3GID, lscustomergid,msGetD2GID,  vendor_name,
        journal_date, LSReferenceGID, lsSEAccountname, lsSEaccount_gid = string.Empty;
        string msAccGetGID, lsaccoungroup_gid = string.Empty, LSLedgerType = string.Empty, LSDisplayType = string.Empty, lsreferenceType = string.Empty;

        public string finance_payment(string payment_type, string payment_mode, string bank_name, string payment_date,
        double payment_amount, string branch_gid, string screen_name, string module_name, string account_gid,
        string remarks, string refno, double tds, double adjustment_amount, double paid_amount, string transaction_gid)
        {
            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msGetDlGID1 = objcmnfunctions.GetMasterGID_SP("FPCD");
            msGetDlGID2 = objcmnfunctions.GetMasterGID_SP("FPCD");

            // invoice_date = DateTime.ParseExact(payment_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            jounrnal_date = payment_date;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            string lsFinanceActivationDate = DateTime.Now.ToString("yyyy-MM-dd"); 
            
            DateTime previous_date = DateTime.Parse("2013-09-01");  
            
            DateTime active_date = DateTime.Parse(lsFinanceActivationDate);
            if (active_date > previous_date)
            {
                if (payment_type == "Purchase")
                {
                    msSQL= " select a.account_gid,a.account_name from acp_mst_ttaxsegment a"+
                         " left join acp_mst_tvendor b on a.taxsegment_gid=b.taxsegment_gid"+
                         " where b.vendor_gid='" + account_gid + "'";
                    objODBCDataReader=objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {
                        reference_gid= objODBCDataReader["account_gid"].ToString();
                        journal_type = "dr";
                    }
                   
                    msSQL = "select account_gid from acp_mst_tvendor where vendor_gid='" + account_gid + "'";
                    string lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }

                else if (payment_type == "Sales")
                {
                    msSQL = " select a.account_gid,a.account_name from acp_mst_ttaxsegment a" +
                         " left join crm_mst_tcustomer b on a.taxsegment_gid=b.taxsegment_gid" +
                         " where b.customer_gid='" + account_gid + "'";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows == true)
                    {
                        reference_gid = objODBCDataReader["account_gid"].ToString();
                        journal_type = "cr";
                    }

                    msSQL = "select account_gid from crm_mst_tcustomer where customer_gid='" + account_gid + "'";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else if (payment_type == "Tax")
                {
                    journal_type = "dr";
                    reference_gid = "FCOA000023";   
                    msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + account_gid + "'";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else if (payment_type == "Payroll")
                {
                    journal_type = "dr";
                    reference_gid = "FCOA000024";
                    msSQL = " select account_gid from hrm_mst_temployee where employee_gid='" + account_gid + "'";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else if (payment_type == "SalesAdvance")
                {
                    journal_type = "cr";
                    msSQL = "select account_gid from acc_mst_accountmapping where" +
                        " screen_name='" + screen_name + "' and module_name='" + module_name + "'" +
                        " and field_name='Advance' ";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else if (payment_type == "PurchaseAdvance")
                {
                    journal_type = "dr";
                    msSQL = "select account_gid from acc_mst_accountmapping where" +
                        " screen_name='" + screen_name + "' and module_name='" + module_name + "'" +
                        " and field_name='Advance' ";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else if (payment_type == "PayrollLoan")
                {
                    journal_type = "dr";
                    msSQL = "select account_gid from acc_mst_accountmapping where" +
                        " screen_name='" + screen_name + "' and module_name='" + module_name + "'" +
                        " and field_name='Loan' ";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else if (payment_type == "Expense")
                {
                    journal_type = "dr";
                    msSQL = "select account_gid from pmr_mst_tproducttype where producttype_gid='" + account_gid + "'";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    journal_type = "cr";
                    reference_gid = "FCOA000022";
                    msSQL = "select account_gid from crm_mst_tcustomer where customer_gid='" + account_gid + "'";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = " select a.account_gid,b.display_type from acc_mst_accountmapping a" +
                    " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                    " where a.screen_name='" + screen_name + "' and a.module_name='" + module_name + "' and a.field_name='With Hold Tax' ";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    objODBCDataReader.Read();
                    lswthaccount_gid = objODBCDataReader["account_gid"].ToString();
                    if (objODBCDataReader["display_type"].ToString() == "Y")
                    {
                        lsWHTjournal_type = "dr";
                    }
                    else
                    {
                        lsWHTjournal_type = "cr";
                    }
                    objODBCDataReader.Close();
                }
                if (lswthaccount_gid != "")
                {
                    if (tds != 0 || tds != 0.00) { 
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                  " (journaldtl_gid, " +
                  " journal_gid, " +
                  " account_gid," +
                  " journal_type," +
                  " remarks, " +
                  " transaction_amount) " +
                  " values (" +
                  "'" + msGetDlGID1 + "', " +
                  "'" + msGetGID + "'," +
                  "'" + lswthaccount_gid + "'," +
                  "'" + lsWHTjournal_type + "'," +
                  "'" + remarks + "', " +
                  "'" + tds + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                msSQL = " select a.account_gid,b.display_type from acc_mst_accountmapping a" +
                    " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                    " where a.screen_name='" + screen_name + "' and a.module_name='" + module_name + "' " +
                    "and a.field_name='Adjustment Amount' ";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    objODBCDataReader.Read();
                    lsADaccount_GID = objODBCDataReader["account_gid"].ToString();
                    if (objODBCDataReader["display_type"].ToString() == "Y")
                    {
                        lsADjournal_type = "cr";
                    }
                    else
                    {
                        lsADjournal_type = "dr";
                    }
                    objODBCDataReader.Close();
                }
                if (lsADaccount_GID != "")
                {
                    if (adjustment_amount != 0 || adjustment_amount != 0.00)
                    {
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                      " (journaldtl_gid, " +
                      " journal_gid, " +
                      " account_gid," +
                      " journal_type," +
                      " remarks, " +
                      " transaction_amount) " +
                      " values (" +
                      "'" + msGetDlGID2 + "', " +
                      "'" + msGetGID + "'," +
                      "'" + lsADaccount_GID + "'," +
                      "'" + lsADjournal_type + "'," +
                      "'" + remarks + "', " +
                      "'" + adjustment_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (journal_type == "cr")
                {
                    SEjournal_type = "dr";
                }
                else
                {
                    SEjournal_type = "cr";
                }
                if (payment_mode == "Cash")
                {
                    LSTransactionCode = "CC001";
                    LSTransactionType = "Cash Book";
                    LSReferenceGID = branch_gid;
                }
                else if (payment_mode == "CREDITCARD")
                {
                    LSTransactionType = "Credit Card Book";
                    LSReferenceGID = bank_name;
                }
                else if (payment_mode == "Advance Receipt")
                {
                    LSTransactionType = "Advance Receipt";
                    LSTransactionCode = "SADV001";
                    lsSEAccountname = "Loan and Advance Receipt";
                    LSReferenceGID = branch_gid;
                }
                else if (payment_mode == "Advance Payment")
                {
                    LSTransactionType = "Advance Payment";
                    LSTransactionCode = "PADV001";
                    lsSEAccountname = "Loan and Advance Payment";
                    LSReferenceGID = branch_gid;
                }
                else if (payment_mode == "Credit Note")
                {
                    LSTransactionType = "Credit Note";
                    LSTransactionCode = "CRN001";
                    lsSEAccountname = "Sales Discounts";
                    LSReferenceGID = branch_gid;
                }
                else if (payment_mode == "Debit Note")
                {
                    LSTransactionType = "Debit Note";
                    LSTransactionCode = "DBN001";
                    lsSEAccountname = "Other Income";
                    LSReferenceGID = branch_gid;
                }
                else
                {
                    LSTransactionType = "Bank Book";
                    LSReferenceGID = bank_name;
                }
                msGetSEDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                if (bank_name == "")
                {
                    msSQL = "select account_gid from acc_mst_tchartofaccount where accountgroup_name = 'CASH IN HAND'";
                    lsSEaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                    lsSEAccountname = "CASH IN HAND";
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " transaction_gid," +
                        " journal_type," +
                        " remarks, " +
                        " transaction_amount) " +
                        " values (" +
                        "'" + msGetSEDlGID + "', " +
                        "'" + msGetGID + "'," +
                        "'" + lsSEaccount_gid + "'," +
                        "'" + lsaccount_gid + "'," +
                        "'" + SEjournal_type + "'," +
                        "'" + remarks + "', " +
                        "'" + paid_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " transaction_gid," +
                               " journal_type," +
                               " remarks, " +
                               " transaction_amount) " +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + lsaccount_gid + "'," +
                               "'" + lsSEaccount_gid + "'," +
                               "'" + journal_type + "'," +
                               "'" + remarks + "', " +
                               "'" + payment_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    DateTime paymentdate = Convert.ToDateTime(payment_date);
                    msSQL = " Insert into acc_trn_journalentry " +
                              " (journal_gid, " +
                              " transaction_date, " +
                              " remarks, " +
                              " transaction_type," +
                              " branch_gid," +
                              " reference_type, " +
                              " reference_gid, " +
                              " journal_from," +
                              " transaction_code, " +
                              " transaction_gid, " +
                              " journal_year, " +
                              " journal_month, " +
                              " journal_day, " +
                              " journal_refno)" +
                              " values (" +
                              "'" + msGetGID + "', " +
                              "'" + paymentdate.ToString("yyyy-MM-dd") + "', " +
                              "'" + remarks + "'," +
                              "'" + LSTransactionType + "'," +
                              "'" + branch_gid + "', " +
                              "'" + lsSEAccountname + "', " +
                              "'" + LSReferenceGID + "', " +
                              "'" + payment_type + "'," +
                              "'" + LSTransactionCode + "'," +
                              "'" + transaction_gid + "', " +
                              "'" + journal_year + "', " +
                              "'" + journal_month + "', " +
                              "'" + journal_Day + "', " +
                              "'" + refno + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else if (payment_mode == "Advance Receipt" || payment_mode == "Advance Payment")
                {
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "' and " +
                        "module_name='" + module_name + "' and field_name='Advance' ";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                             " (journaldtl_gid, " +
                             " journal_gid, " +
                             " account_gid," +
                             " transaction_gid," +
                             " journal_type," +
                             " remarks, " +
                             " transaction_amount) " +
                             " values (" +
                             "'" + msGetSEDlGID + "', " +
                             "'" + msGetGID + "'," +
                             "'" + lsSEaccount_gid + "'," +
                             "'" + lsaccount_gid + "'," +
                             "'" + SEjournal_type + "'," +
                             "'" + remarks + "', " +
                             "'" + payment_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else if (payment_mode == "Credit Note" || payment_mode == "Debit Note")
                {
                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "' and " +
                       "module_name='" + module_name + "' and field_name='Note' ";
                    lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " Insert into acc_trn_journalentrydtl " +
                             " (journaldtl_gid, " +
                             " journal_gid, " +
                             " account_gid," +
                             " transaction_gid," +
                             " journal_type," +
                             " remarks, " +
                             " transaction_amount) " +
                             " values (" +
                             "'" + msGetSEDlGID + "', " +
                             "'" + msGetGID + "'," +
                             "'" + lsSEaccount_gid + "'," +
                             "'" + lsaccount_gid + "'," +
                             "'" + SEjournal_type + "'," +
                             "'" + remarks + "', " +
                             "'" + payment_amount + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    if (payment_type == "Purchase")
                    {
                      
                        msSQL = "select account_gid from acp_mst_tvendor where vendor_gid='" + account_gid + "'";
                        string lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = "select account_gid,bank_code,bank_name from acc_mst_tbank where bank_gid='" + bank_name + "'";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDataReader.HasRows == true)
                        {
                            objODBCDataReader.Read();
                            lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                            LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                            lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                            objODBCDataReader.Close();
                        }
                        msSQL = "select account_gid,bank_code,bank_name from acc_mst_tcreditcard where bank_gid='" + bank_name + "'";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDataReader.HasRows == true)
                        {
                            objODBCDataReader.Read();
                            lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                            LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                            lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                            objODBCDataReader.Close();
                        }
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                                " (journaldtl_gid, " +
                                " journal_gid, " +
                                " account_gid," +
                                " transaction_gid," +
                                " journal_type," +
                                " remarks, " +
                                " transaction_amount) " +
                                " values (" +
                                "'" + msGetSEDlGID + "', " +
                                "'" + msGetGID + "'," +
                                "'" + lsSEaccount_gid + "'," +
                                "'" + lsaccount_gid + "'," +
                                "'" + SEjournal_type + "'," +
                                "'" + remarks + "', " +
                                "'" + payment_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " Insert into acc_trn_journalentrydtl " +
                                " (journaldtl_gid, " +
                                " journal_gid, " +
                                " account_gid," +
                                " transaction_gid," +
                                " journal_type," +
                                " remarks, " +
                                " transaction_amount) " +
                                " values (" +
                                "'" + msGetDlGID + "', " +
                                "'" + msGetGID + "'," +
                                "'" + lsaccount_gid + "'," +
                                "'" + lsSEaccount_gid + "'," +
                                "'" + journal_type + "'," +
                                "'" + remarks + "', " +
                                "'" + paid_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        DateTime paymentdate = Convert.ToDateTime(payment_date);
                        msSQL = " Insert into acc_trn_journalentry " +
                                  " (journal_gid, " +
                                  " transaction_date, " +
                                  " remarks, " +
                                  " transaction_type," +
                                  " branch_gid," +
                                  " reference_type, " +
                                  " reference_gid, " +
                                  " journal_from," +
                                  " transaction_code, " +
                                  " transaction_gid, " +
                                  " journal_year, " +
                                  " journal_month, " +
                                  " journal_day, " +
                                  " journal_refno)" +
                                  " values (" +
                                  "'" + msGetGID + "', " +
                                  "'" + paymentdate.ToString("yyyy-MM-dd") + "', " +
                                  "'" + remarks + "'," +
                                  "'" + LSTransactionType + "'," +
                                  "'" + branch_gid + "', " +
                                  "'" + lsSEAccountname + "', " +
                                  "'" + LSReferenceGID + "', " +
                                  "'" + payment_type + "'," +
                                  "'" + LSTransactionCode + "'," +
                                  "'" + transaction_gid + "', " +
                                  "'" + journal_year + "', " +
                                  "'" + journal_month + "', " +
                                  "'" + journal_Day + "', " +
                                  "'" + refno + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {

                        msSQL = "select account_gid,bank_code,bank_name from acc_mst_tbank where bank_gid='" + bank_name + "'";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDataReader.HasRows == true)
                        {
                            objODBCDataReader.Read();
                            lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                            LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                            lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                            objODBCDataReader.Close();
                        }
                        msSQL = "select account_gid,bank_code,bank_name from acc_mst_tcreditcard where bank_gid='" + bank_name + "'";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);
                        if (objODBCDataReader.HasRows == true)
                        {
                            objODBCDataReader.Read();
                            lsSEaccount_gid = objODBCDataReader["account_gid"].ToString();
                            LSTransactionCode = objODBCDataReader["bank_code"].ToString();
                            lsSEAccountname = objODBCDataReader["bank_name"].ToString();
                            objODBCDataReader.Close();
                        }
                        msSQL = " Insert into acc_trn_journalentrydtl " +
                                " (journaldtl_gid, " +
                                " journal_gid, " +
                                " account_gid," +
                                " transaction_gid," +
                                " journal_type," +
                                " remarks, " +
                                " transaction_amount) " +
                                " values (" +
                                "'" + msGetSEDlGID + "', " +
                                "'" + msGetGID + "'," +
                                "'" + lsSEaccount_gid + "'," +
                                "'" + lsaccount_gid + "'," +
                                "'" + SEjournal_type + "'," +
                                "'" + remarks + "', " +
                                "'" + paid_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " Insert into acc_trn_journalentrydtl " +
                                " (journaldtl_gid, " +
                                " journal_gid, " +
                                " account_gid," +
                                " transaction_gid," +
                                " journal_type," +
                                " remarks, " +
                                " transaction_amount) " +
                                " values (" +
                                "'" + msGetDlGID + "', " +
                                "'" + msGetGID + "'," +
                                "'" + lsaccount_gid + "'," +
                                "'" + lsSEaccount_gid + "'," +
                                "'" + journal_type + "'," +
                                "'" + remarks + "', " +
                                "'" + payment_amount + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        DateTime paymentdate = Convert.ToDateTime(payment_date);
                        msSQL = " Insert into acc_trn_journalentry " +
                                  " (journal_gid, " +
                                  " transaction_date, " +
                                  " remarks, " +
                                  " transaction_type," +
                                  " branch_gid," +
                                  " reference_type, " +
                                  " reference_gid, " +
                                  " journal_from," +
                                  " transaction_code, " +
                                  " transaction_gid, " +
                                  " journal_year, " +
                                  " journal_month, " +
                                  " journal_day, " +
                                  " journal_refno)" +
                                  " values (" +
                                  "'" + msGetGID + "', " +
                                  "'" + paymentdate.ToString("yyyy-MM-dd") + "', " +
                                  "'" + remarks + "'," +
                                  "'" + LSTransactionType + "'," +
                                  "'" + branch_gid + "', " +
                                  "'" + lsSEAccountname + "', " +
                                  "'" + LSReferenceGID + "', " +
                                  "'" + payment_type + "'," +
                                  "'" + LSTransactionCode + "'," +
                                  "'" + transaction_gid + "', " +
                                  "'" + journal_year + "', " +
                                  "'" + journal_month + "', " +
                                  "'" + journal_Day + "', " +
                                  "'" + refno + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
            }
            else
            {
                return "Ledger can't be posted";
            }
            return "Entry Successfully added to journal voucher";
        }
        public void jn_bankcharge(string payment_gid  , string remarks  , double amount  , string bank_gid  , string screenname  , string module_name  )
        {
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "' and module_name='" + module_name + "' and field_name='Bank Charges' ";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if(objODBCDataReader.HasRows == true)
            {
                account_gid = objODBCDataReader["account_gid"].ToString();
            }
            if (amount != 0 || amount != 0.00)
            {

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select journal_gid from acc_trn_journalentry where journal_refno='" + payment_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if(objODBCDataReader.HasRows == true)
            {
                msgetgid = objODBCDataReader["journal_gid"].ToString();
            }
            
            msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID + "', " +
                            "'" + msgetgid + "'," +
                            "'" + account_gid + "'," +
                            "'" + remarks + "', " +
                            "'cr'," +
                            "'" + amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
        }
        public void jn_exchange(string payment_gid , string remarks , double amount , string screenname , string module_name , string type )
        {
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "' and module_name='" + module_name + "' and field_name='" + type + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                account_gid = objODBCDataReader["account_gid"].ToString();
            }
            if (amount != 0 || amount != 0.00)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if(type == "Exchange Gain")
            {
                journal_type = "cr";
            }
            else
            {
                journal_type = "dr";
            }
           

                msSQL = "select journal_gid from acc_trn_journalentry where journal_refno='" + payment_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if(objODBCDataReader.HasRows == true)
            {
                msgetgid = objODBCDataReader["journal_gid"].ToString();
            }
            msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID + "', " +
                            "'" + msgetgid + "'," +
                            "'" + account_gid + "'," +
                            "'" + remarks + "', " +
                            "'" + journal_type + "'," +
                            "'" + amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
        }
        public void finance_advancepayment(string payment_type , string payment_mode ,string bank_gid, string payment_date,
        double payment_amount , string branch_gid , string screenname , string module_name ,string account_gid , 
        string remarks , string refno, double tds_amount , double adjustment_amount , double paid_amount, string transaction_gid )
        {
            // DateTime journal_date = Convert.ToDateTime(payment_date);
            //invoicedate.ToString("yyyy-MM-dd")
            jounrnal_date = payment_date;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if(msGetDlGID == "E")
            {
                throw new Exception("Error while getting journal gid");
            }
            msGetD2GID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if (msGetD2GID == "E")
            {
                throw new Exception("Error while getting journal gid");
            }
            msGetD3GID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if (msGetD3GID == "E")
            {
                throw new Exception("Error while getting journal gid");
            }
            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            if (msGetGID == "E")
            {
                throw new Exception("Error while getting voucher gid");
            }
            journal_type = "dr";
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Advance' and module_name='" + module_name + "' and field_name='Advance'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if(objODBCDataReader.HasRows == true)
            {
                lsaccount_gid = objODBCDataReader["account_gid"].ToString();
            }
            msSQL = " Insert into acc_trn_journalentrydtl " +
                " (journaldtl_gid, " +
                " journal_gid, " +
                " account_gid," +
                " journal_type," +
                " remarks, " +
                " transaction_amount) " +
                " values (" +
                "'" + msGetDlGID + "', " +
                "'" + msGetGID + "'," +
                "'" + lsaccount_gid + "'," +
                "'" + journal_type + "'," +
                "'" + remarks.Replace("'","").Trim() + "', " +
                "'" + adjustment_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msSQL = " select account_gid from crm_mst_tcustomer where customer_gid='" + account_gid + "'";
            if (objODBCDataReader.HasRows == true)
            {
                journal_type = "cr";
                lscustomergid = objODBCDataReader["account_gid"].ToString();
            }
            msSQL = " Insert into acc_trn_journalentrydtl " +
               " (journaldtl_gid, " +
               " journal_gid, " +
               " account_gid," +
               " journal_type," +
               " remarks, " +
               " transaction_amount) " +
               " values (" +
               "'" + msGetD2GID + "', " +
               "'" + msGetGID + "'," +
               "'" + lscustomergid + "'," +
               "'" + journal_type + "'," +
               "'" + remarks.Replace("'", "").Trim() + "', " +
               "'" + adjustment_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


           DateTime paymentdate = Convert.ToDateTime(payment_date);

            msSQL = " Insert into acc_trn_journalentry " +
          " (journal_gid, " +
          " transaction_date, " +
          " remarks, " +
          " transaction_type," +
          " branch_gid," +
          " reference_type, " +
          " reference_gid, " +
          " journal_from," +
          " transaction_code, " +
          " transaction_gid, " +
          " journal_year, " +
          " journal_month, " +
          " journal_day, " +
          " journal_refno)" +
          " values (" +
          "'" + msGetGID + "', " +
          "'" + paymentdate.ToString("yyyy-MM-dd") + "', " +
          "'" + remarks.Replace("'", "").Trim() + "'," +
          "'Advance Receipt'," +
          "'" + branch_gid + "', " +
          "'Loan and Advance Receipt', " +
          "'" + branch_gid + "', " +
          "'" + payment_type + "'," +
          "'SADV001'," +
          "'" + transaction_gid + "', " +
          "'" + journal_year + "', " +
          "'" + journal_month + "', " +
          "'" + journal_Day + "', " +
          "'" + refno + "')";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        }
        public string jn_invoice(string invoice_date, string invoiceremarks, string branch, string invoice_ref, string invoice_gid,
        double basic_amount, double addon, double add_discount, double grandtotal, string customer_gid, string screen_name, string module_name,
        string sales_type, double roundoff, double frieght_charges, double buyback_charges, double packing_charges, double insurance_charges,string taxamount4 , string tax_gid)
        {
            jounrnal_date = invoice_date;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msSQL = "select account_gid,customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                customer_name = objODBCDataReader["customer_name"].ToString();
            }
            else
            {

            }
            string invoice_remarks = "";
            if (invoiceremarks != null)
            {
                invoice_remarks = invoiceremarks.Replace("'", "\\\'");
            }




            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            msSQL = " Insert into acc_trn_journalentry " +
                " (journal_gid, " +
                " journal_refno, " +
                " transaction_code, " +
                " transaction_date, " +
                " remarks, " +
                " transaction_type," +
                " reference_type," +
                " reference_gid," +
                " transaction_gid, " +
                " journal_from," +
                " journal_year, " +
                " journal_month, " +
                " journal_day, " +
                " branch_gid" +
                " ) values (" +
                "'" + msGetGID + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_date + "'," +
                "'" + invoice_remarks + "'," +
                "'Journal'," +
                "'" + customer_name + "'," +
                "'" + customer_gid + "'," +
                "'" + invoice_gid + "'," +
                "'Sales'," +
                "'" + journal_year + "'," +
                "'" + journal_month + "'," +
                "'" + journal_Day + "'," +
                "'" + branch + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid + "'," +
                           "'" + invoice_remarks + "'," +
                           "'dr'," +
                           "'" + grandtotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='COGS' ";
            string account_gid1 = objdbconn.GetExecuteScalar(msSQL);

            if (sales_type != "" && sales_type != null)
            {
                msSQL= "select account_gid from smr_trn_tsalestype where salestype_gid ='"+ sales_type + "'";
                account_gid1 = objdbconn.GetExecuteScalar(msSQL);
                //account_gid1 = sales_type;
            }
            else
            {
                account_gid1 = account_gid;
        }

        msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid1 + "'," +
                           "'" + invoice_remarks + "'," +
                           "'cr'," +
                           "'" + basic_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Addon Amount' ";
            string account_gid2 = objdbconn.GetExecuteScalar(msSQL);
            if (addon != 0)
            {
                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid2 + "'," +
                          "'" + invoice_remarks + "'," +
                          "'cr'," +
                          "'" + addon + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
               " and module_name='" + module_name + "' and field_name='Additional Discount' ";
            string account_gid3 = objdbconn.GetExecuteScalar(msSQL);
            if (add_discount != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid3 + "'," +
                        "'" + invoice_remarks + "'," +
                        "'cr'," +
                        "'" +  - add_discount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + tax_gid + "'";
               
            string account_gid9 = objdbconn.GetExecuteScalar(msSQL);
            if (taxamount4 != "0")
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid9 + "'," +
                          "'" + invoice_remarks + "'," +
                          "'cr'," +
                          "'" + taxamount4 + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Round Off' ";
            string account_gid4 = objdbconn.GetExecuteScalar(msSQL);
            if (roundoff != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid4 + "'," +
                        "'" + invoice_remarks + "'," +
                        "'cr'," +
                        "'" + roundoff + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where " +
                "screen_name='" + screen_name + "' and module_name='" + module_name + "' and field_name='Frieght Charges' ";
            string account_gid5 = objdbconn.GetExecuteScalar(msSQL);
            if (frieght_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid," +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid5 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + frieght_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='BuyBack/Scrap Value' ";
            string account_gid6 = objdbconn.GetExecuteScalar(msSQL);
            if (buyback_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid6 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'dr'," +
                               "'" + buyback_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);                
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "' and" +
                " module_name='" + module_name + "' and field_name='Packing Charges' ";
            string account_gid7 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid7 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + packing_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Insurance' ";
            string account_gid8 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid8 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            return "";
        }
        public string jn_updatedinvoice(string invoice_date, string invoice_remarks, string branch, string invoice_ref, string invoice_gid,
        double basic_amount, double addon, double add_discount, double grandtotal, string customer_gid, string screen_name, string module_name,
        string sales_type, double roundoff, double frieght_charges, double buyback_charges, double packing_charges, double insurance_charges, string taxamount4, string tax_gid)
        {

            jounrnal_date = invoice_date;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msSQL = "select account_gid,customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                customer_name = objODBCDataReader["customer_name"].ToString();
            }
            else
            {

            }
          
            msSQL = "select journal_gid from acc_trn_journalentry where journal_refno= '" + invoice_ref + "'";
            string ls_journalgid=objdbconn.GetExecuteScalar(msSQL);

            msSQL = "delete from acc_trn_journalentry where journal_refno= '" + invoice_ref + "'";
            mnResult=objdbconn.ExecuteNonQuerySQL(msSQL);
            msSQL = "delete from acc_trn_journalentrydtl where journal_gid= '" + ls_journalgid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            msSQL = " Insert into acc_trn_journalentry " +
                " (journal_gid, " +
                " journal_refno, " +
                " transaction_code, " +
                " transaction_date, " +
                " remarks, " +
                " transaction_type," +
                " reference_type," +
                " reference_gid," +
                " transaction_gid, " +
                " journal_from," +
                " journal_year, " +
                " journal_month, " +
                " journal_day, " +
                " branch_gid" +
                " ) values (" +
                "'" + msGetGID + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_date + "'," +
                "'" + invoice_remarks + "'," +
                "'Journal'," +
                "'" + customer_name + "'," +
                "'" + customer_gid + "'," +
                "'" + invoice_gid + "'," +
                "'Sales'," +
                "'" + journal_year + "'," +
                "'" + journal_month + "'," +
                "'" + journal_Day + "'," +
                "'" + branch + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid + "'," +
                           "'" + invoice_remarks + "'," +
                           "'dr'," +
                           "'" + grandtotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='COGS' ";
            string account_gid1 = objdbconn.GetExecuteScalar(msSQL);

            if (sales_type != "" && sales_type != null)
            {
                msSQL = "select account_gid from smr_trn_tsalestype where salestype_gid ='" + sales_type + "'";
                account_gid1 = objdbconn.GetExecuteScalar(msSQL);
                //account_gid1 = sales_type;
            }
            else
            {
                account_gid1 = account_gid;
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "'," +
                               "'" + msGetGID + "'," +
                               "'" + account_gid1 + "'," +
                               "'" + invoice_remarks + "'," +
                               "'cr'," +
                               "'" + basic_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Addon Amount' ";
            string account_gid2 = objdbconn.GetExecuteScalar(msSQL);
            if (addon != 0)
            {
                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid2 + "'," +
                          "'" + invoice_remarks + "'," +
                          "'cr'," +
                          "'" + addon + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
               " and module_name='" + module_name + "' and field_name='Additional Discount' ";
            string account_gid3 = objdbconn.GetExecuteScalar(msSQL);
            if (add_discount != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid3 + "'," +
                        "'" + invoice_remarks + "'," +
                        "'cr'," +
                        "'" + -add_discount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + tax_gid + "'";

            string account_gid9 = objdbconn.GetExecuteScalar(msSQL);
            if (taxamount4 != "0")
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid9 + "'," +
                          "'" + invoice_remarks + "'," +
                          "'cr'," +
                          "'" + taxamount4 + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Round Off' ";
            string account_gid4 = objdbconn.GetExecuteScalar(msSQL);
            if (roundoff != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid4 + "'," +
                        "'" + invoice_remarks + "'," +
                        "'cr'," +
                        "'" + roundoff + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where " +
                "screen_name='" + screen_name + "' and module_name='" + module_name + "' and field_name='Frieght Charges' ";
            string account_gid5 = objdbconn.GetExecuteScalar(msSQL);
            if (frieght_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid," +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid5 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + frieght_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='BuyBack/Scrap Value' ";
            string account_gid6 = objdbconn.GetExecuteScalar(msSQL);
            if (buyback_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid6 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'dr'," +
                               "'" + buyback_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "' and" +
                " module_name='" + module_name + "' and field_name='Packing Charges' ";
            string account_gid7 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid7 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + packing_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Insurance' ";
            string account_gid8 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid8 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            return "";
        }
        public void jn_sales_tax(string invoice_gid , string invoice_ref, string remarks, double tax_amount , string tax_gid )
        {
            msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + tax_gid + "' ";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);

            if (objODBCDataReader.HasRows == true)
            {
                account_gid = objODBCDataReader["account_gid"].ToString();
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if(msGetDlGID == "E")
            {
                return;
            }
            msSQL = "select journal_gid from acc_trn_journalentry where transaction_gid='" + invoice_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                msgetgid = objODBCDataReader["journal_gid"].ToString() ;
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID +"', " +
                            "'" + msgetgid +"'," +
                            "'" + account_gid +"'," +
                            "'" + remarks + "', " +
                            "'cr'," +
                            "'" +tax_amount +"')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        }
        public void jn_creditdebit_tax(string invoice_gid, string invoice_ref, string remarks, double tax_amount, string tax_gid)
        {
            msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + tax_gid + "' ";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);

            if (objODBCDataReader.HasRows == true)
            {
                account_gid = objODBCDataReader["account_gid"].ToString();
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if (msGetDlGID == "E")
            {
                return;
            }
            msSQL = "select journal_gid from acc_trn_journalentry where transaction_gid='" + invoice_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                msgetgid = objODBCDataReader["journal_gid"].ToString();
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID + "', " +
                            "'" + msgetgid + "'," +
                            "'" + account_gid + "'," +
                            "'" + remarks + "', " +
                            "'dr'," +
                            "'" + tax_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        }


        public string employee_payrun(string salary_gid, string payrun_date, string employee_gid, string employee_code,
        string employee_name, double gross_salary, double employeecomponent_amount, double statutory_amount,
        string branch, string journal_refno, double loan_amount, double advance_amount,
        double other_addition, double other_deduction, string refno,string month,string year)
        {
            string msGetGID, msGetdlGID;
            DateTime journal_date;
            int journal_year, journal_month, journal_day;
            string account_gid;
            double grandtotal = 0.0;
            //string month = payrunmonth;
            //string year = payrunyear;
            grandtotal = gross_salary + statutory_amount - employeecomponent_amount;
            DateTime payrundate = Convert.ToDateTime(payrun_date);
            DateTime date = DateTime.Now; // Example date
            journal_month = date.Month;
            journal_day = date.Day;
            journal_year = date.Year;

            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");

            msSQL = " Insert into acc_trn_journalentry " +
                 " (journal_gid, " +
                 " journal_refno, " +
                 " transaction_code, " +
                 " transaction_date, " +
                 " transaction_type," +
                 " reference_type," +
                 " reference_gid," +
                 " transaction_gid, " +
                 " journal_from," +
                 " journal_year, " +
                 " journal_month, " +
                 " journal_day, " +
                 " remarks, " +
                 " branch_gid)" +
                 " values (" +
                 "'" + msGetGID + "', " +
                 "'" + month + year + "-" + refno + "'," +
                 "'" + employee_code + "'," +
                 "'" + payrun_date + "', " +
                 "'Journal', " +
                 "'" + employee_name + "', " +
                 "'" + employee_gid + "', " +
                 "'" + salary_gid + "', " +
                 "'Payroll'," + "'" +
                 journal_year + "', " +
                 "'" + journal_month + "', " +
                 "'" + journal_day + "', " +
                 " 'Salary Payrun for the employee" + " " + employee_name + " " + "on this" + " " + month + "-" + year + "', " +
                 "'" + branch + "') ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);

            if (objODBCDataReader.HasRows)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                objODBCDataReader.Close();

                double total = (grandtotal - statutory_amount) + other_addition - other_deduction;

                msSQL = " Insert into acc_trn_journalentrydtl " +
                                    " (journaldtl_gid, " +
                                    " journal_gid, " +
                                    " account_gid," +
                                    " journal_type," +
                                    " transaction_amount)" +
                                    " values (" +
                                    "'" + msGetDlGID + "', " +
                                    "'" + msGetGID + "'," +
                                    "'" + account_gid + "'," +
                                    "'cr'," +
                                    "'" + total + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                    double total1 = (grandtotal - statutory_amount) +other_addition - other_deduction;

                    msSQL = " Insert into acc_trn_journalentrydtl " +
                                        " (journaldtl_gid, " +
                                        " journal_gid, " +
                                        " account_gid," +
                                        " journal_type," +
                                        " transaction_amount)" +
                                        " values (" +
                                        "'" + msGetDlGID + "', " +
                                        "'" + msGetGID + "'," +
                                        "'" + account_gid + "'," +
                                        "'dr'," +
                                        "'" + total1 + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                    msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                    msSQL = "select account_gid from acc_mst_accountmapping where screen_name='Loan' and module_name='PAY' and field_name='Loan' ";
                    objODBCDataReader = objdbconn.GetDataReader(msSQL);

                if (objODBCDataReader.HasRows)
                {
                    objODBCDataReader.Read();
                    account_gid = objODBCDataReader["account_gid"].ToString();
                    objODBCDataReader.Close();

                    msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "', " +
                          "'" + msGetGID + "'," +
                          "'" + account_gid + "'," +
                          "'dr'," +
                          "'" + (loan_amount + advance_amount) + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                        msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                        msSQL = "select account_gid from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
                        objODBCDataReader = objdbconn.GetDataReader(msSQL);

                        if (objODBCDataReader.HasRows)
                        {
                            objODBCDataReader.Read();
                            account_gid = objODBCDataReader["account_gid"].ToString();
                            objODBCDataReader.Close();

                            msSQL = " Insert into acc_trn_journalentrydtl " +
                              " (journaldtl_gid, " +
                              " journal_gid, " +
                              " account_gid," +
                              " journal_type," +
                              " transaction_amount)" +
                              " values (" +
                              "'" + msGetDlGID + "', " +
                              "'" + msGetGID + "'," +
                              "'" + account_gid + "'," +
                              "'cr'," +
                              "'" + (loan_amount + advance_amount) + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        //popstatutory_amount(employee_gid, month, year, msGetGID);
            }
            return null;
        }
        //public string popstatutory_amount (string employee_gid,string month,string year,string journal_gid)
        //{
        //    msSQL = " Select a.component_name,(earned_salarycomponent_amount+earnedemployer_salarycomponentamount) as statutory_amount, " +
        //            " b.salarydtl_gid,b.salarycomponent_gid,a.account_gid from pay_mst_tsalarycomponent a " +
        //            " left join pay_trn_tsalarydtl b on a.salarycomponent_gid=b.salarycomponent_gid " +
        //            " left join pay_trn_tsalary c on b.salary_gid=c.salary_gid " +
        //            " where c.employee_gid='" + employee_gid + "' and c.month='" + month + "' and c.year='" + year + "' and a.statutory_flag='Y'";
        //    dt_datatable = objdbconn.GetDataTable(msSQL);
        //    if (dt_datatable.Rows.Count > 0)
        //    {
        //        foreach (DataRow dt in dt_datatable.Rows)
        //        {
        //            if (dt["account_gid"].ToString() != "")
        //            {
        //                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

        //                msSQL = " Insert into acc_trn_journalentrydtl " +
        //                 " (journaldtl_gid, " +
        //                 " journal_gid, " +
        //                 " account_gid," +
        //                 " journal_type," +
        //                 " transaction_amount)" +
        //                 " values (" +
        //                 "'" + msGetDlGID + "', " +
        //                 "'" + journal_gid + "'," +
        //                 "'" + account_gid + "'," +
        //                 "'cr'," +
        //                 "'" + dt["statutory_amount"] + "')";
        //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //            }
        //        }
        //    }
        //    return "";
        //}
        public List<string> journal_function(string invoice_date)
        {
            DateTime finyear = DateTime.MinValue;

            msSQL = "SELECT cast(date_format(fyear_start,'%Y-%m-%d')as char) as fyear_start,finyear_gid FROM" +
                " adm_mst_tyearendactivities ORDER BY finyear_gid DESC LIMIT 1";            
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true) 
            { 
                objODBCDataReader.Read();
                finyear = Convert.ToDateTime(objODBCDataReader["fyear_start"].ToString());
            }
            DateTime invoicedate = Convert.ToDateTime(invoice_date);

            msSQL = " select timestampdiff(Month,'" + finyear.ToString("yyyy-MM-dd") + "','" + invoicedate.ToString("yyyy-MM-dd") + "') as month, " +
                " timestampdiff(day,'" + finyear.ToString("yyyy-MM-dd") + "','" + invoicedate.ToString("yyyy-MM-dd") + "') as day";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                journal_month = objODBCDataReader["month"].ToString();
                journal_Day = objODBCDataReader["day"].ToString();
                journal_year = finyear.Year.ToString();
                objODBCDataReader.Close();
            }
            
            if (journal_month.Length == 1)
            {
                journal_month = "0" + (Convert.ToInt32(journal_month) + 1).ToString();
            }
            else
            {
                journal_month = (Convert.ToInt32(journal_month) + 1).ToString();
            }

            if (journal_Day.Length == 1)
            {
                journal_Day = "0" + (Convert.ToInt32(journal_Day) + 1).ToString();
            }
            else
            {
                journal_Day = (Convert.ToInt32(journal_Day) + 1).ToString();
            }

            journal_monthandyear.Add(journal_year);
            journal_monthandyear.Add(journal_month);
            journal_monthandyear.Add(journal_Day);
            return journal_monthandyear;
        }
        public string finance_vendor_debitor(string reference_type, string account_code, string account_name, string reference_gid, string user_gid)
        {
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FCOA");

            if(msGetDlGID == "E")
            {
                return "";
            }            

            if(reference_type == "Sales")
            {
                msSQL = "select a.account_gid,a.account_name from acp_mst_ttaxsegment a" +
                      " left join crm_mst_Tcustomer b on a.taxsegment_gid=b.taxsegment_gid" +
                      " where b.customer_gid='" + reference_gid + "'";
                objODBCDataReader=objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    lsreferenceType = objODBCDataReader["account_name"].ToString();
                    lsaccoungroup_gid = objODBCDataReader["account_gid"].ToString();
                    LSLedgerType = "N";
                    LSDisplayType = "Y";
                }
               
            }
            else if(reference_type == "Purchase")
            {
                msSQL = "select a.account_gid,a.account_name from acp_mst_ttaxsegment a" +
                      " left join acp_mst_tvendor b on a.taxsegment_gid=b.taxsegment_gid" +
                      " where b.vendor_gid='" + reference_gid + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    lsreferenceType = objODBCDataReader["account_name"].ToString();
                    lsaccoungroup_gid = objODBCDataReader["account_gid"].ToString();
                    LSLedgerType = "N";
                    LSDisplayType = "Y";
                }
            }
            else if(reference_type == "Payroll")
            {
                lsreferenceType = "Salary Payable";
                lsaccoungroup_gid = "FCOA000024";
                LSLedgerType = "N";
                LSDisplayType = "N";
            }
            else if(reference_type == "PurchaseTax")
            {
                lsreferenceType = "GST";
                lsaccoungroup_gid = "FCOA2408257";
                LSLedgerType = "N";
                LSDisplayType = "N";
            }
            else if(reference_type == "Expense")
            {
                lsreferenceType = "Accounts Payable";
                lsaccoungroup_gid = "FCOA000026";
                LSLedgerType = "Y";
                LSDisplayType = "N";
            }
            else if(reference_type == "Additional Expense")
            {
                lsreferenceType = "Accounts Payable";
                lsaccoungroup_gid = "FCOA000026";
                LSLedgerType = "Y";
                LSDisplayType = "N";
            }
            else if(reference_type == "Expense Discount")
            {
                lsreferenceType = "Indirect Income";
                lsaccoungroup_gid = "FCOA1404070071";
                LSLedgerType = "N";
                LSDisplayType = "Y";
            }
            else if(reference_type == "Salary Component")
            {
                lsreferenceType = "Statutory Payables";
                lsaccoungroup_gid = "FCOA000026";
                LSLedgerType = "N";
                LSDisplayType = "N";
            }

            msSQL = " insert into acc_mst_tchartofaccount (" +
                    " account_gid," +
                    " accountgroup_gid," +
                    " accountgroup_name," +
                    " account_code," +
                    " account_name," +
                    " has_child," +
                    " ledger_type," +
                    " display_type," +
                    " Created_Date, " +
                    " Created_By, " +
                    " gl_code) " +
                    " values (" +
                    "'" + msGetDlGID + "'," +
                    "'" + lsaccoungroup_gid + "'," +
                    "'" + lsreferenceType + "'," +
                    "'" + account_code + "'," +
                    "'" + account_name.Replace("'","\\\'") + "'," +
                    "'N'," +
                    "'" + LSLedgerType + "'," +
                    "'" + LSDisplayType + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'" + user_gid + "'," +
                    "'" + account_name.Replace("'", "\\\'") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (reference_type == "Sales")
            {
                msSQL = " update crm_mst_tcustomer set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where customer_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else if(reference_type == "PurchaseTax")
            {
                msSQL = " update acp_mst_ttax set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where tax_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else if(reference_type == "Payroll")
            {
                msSQL = " update hrm_mst_temployee set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where employee_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else if (reference_type == "Expense")
            {
                msSQL = " update pmr_mst_tproduct set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where product_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else if (reference_type == "Additional Expense")
            {
                msSQL = " update pmr_trn_tadditional set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where additional_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else if (reference_type == "Expense Discount")
            {
                msSQL = " update pmr_trn_tdiscount set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where discount_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else if (reference_type == "Salary Component")
            {
                msSQL = " update pay_mst_tsalarycomponent set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where salarycomponent_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else
            {
                msSQL = " update acp_mst_tvendor set " +
                        " account_gid = '" + msGetDlGID + "'" +
                        " where vendor_gid='" + reference_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }           

            return "Account Successfully Added";
        }

        public string finance_vendor_debitor_edit(string account_name, string account_code,string reference_gid)
        {
            msSQL = " update acc_mst_tchartofaccount set  account_name = '" + account_name + "', account_code='" + account_code + "',  gl_code = '" + account_code + "' where account_gid='" + reference_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            return "Account Successfully Updated";
        }
        public string invoice_cancel(string invoice_gid)
        {
            msSQL = " select journal_gid from acc_trn_journalentry where transaction_gid='" + invoice_gid + "'";
            journal_gid = objdbconn.GetExecuteScalar(msSQL);
            msSQL = "delete from acc_trn_journalentrydtl where journal_gid='" + journal_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                msSQL = "delete from acc_trn_journalentry where journal_gid='" + journal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            return "";
        }

        public string jn_purchase_invoice(string invoicedate, string remarks, string branch_name, string invoice_ref, string invoice_gid,
        double basic_amount, double addonCharges, double additionaldiscountAmount, double grandtotal, string vendor_gid, string screenname, string module_name, string purchase_type,
        double roundoff, double freight, double buy_back,string overall_taxname, double overalltax_amount,
        double packing_charges, double insurance_charges)
        {
            jounrnal_date = invoicedate;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msSQL = "select account_gid,vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                vendor_name = objODBCDataReader["vendor_companyname"].ToString();
            }
            else
            {
                return "No account gid available.Cannot raise journal entry";
            }
            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
         

            msSQL = " Insert into acc_trn_journalentry " +
                " (journal_gid, " +
                " journal_refno, " +
                " transaction_code, " +
                " transaction_date, " +
                " remarks, " +
                " transaction_type," +
                " reference_type," +
                " reference_gid," +
                " transaction_gid, " +
                " journal_from," +
                " journal_year, " +
                " journal_month, " +
                " journal_day, " +
                " branch_gid" +
                " ) values (" +
                "'" + msGetGID + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoicedate + "'," +
                "'" + remarks + "'," +
                "'Journal'," +
                "'" + vendor_name + "'," +
                "'" + vendor_gid + "'," +
                "'" + invoice_gid + "'," +
                "'Purchase'," +
                "'" + journal_year + "'," +
                "'" + journal_month + "'," +
                "'" + journal_Day + "'," +
                "'" + branch_name + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid + "'," +
                           "'" + remarks + "'," +
                           "'cr'," +
                           "'" + grandtotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='COGS' ";
            string account_gid1 = objdbconn.GetExecuteScalar(msSQL);

            if (purchase_type != "" && purchase_type != null)
            {
                msSQL = "select account_gid from pmr_trn_tpurchasetype where purchasetype_gid ='" + purchase_type + "'";
                account_gid1 = objdbconn.GetExecuteScalar(msSQL);
            }
            else
            {
                account_gid1 = account_gid1;
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid1 + "'," +
                           "'" + remarks + "'," +
                           "'dr'," +
                           "'" + basic_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='Addon Amount' ";
            string account_gid2 = objdbconn.GetExecuteScalar(msSQL);
            if (addonCharges != 0)
            {
                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid2 + "'," +
                          "'" + remarks + "'," +
                          "'dr'," +
                          "'" + addonCharges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
               " and module_name='" + module_name + "' and field_name='Additional Discount' ";
            string account_gid3 = objdbconn.GetExecuteScalar(msSQL);
            if (additionaldiscountAmount != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid3 + "'," +
                        "'" + remarks + "'," +
                        "'dr'," +
                        "'" + - additionaldiscountAmount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='Round Off' ";
            string account_gid4 = objdbconn.GetExecuteScalar(msSQL);
            if (roundoff != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid4 + "'," +
                        "'" + remarks + "'," +
                        "'dr'," +
                        "'" + roundoff + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where " +
                "screen_name='" + screenname + "' and module_name='" + module_name + "' and field_name='Frieght Charges' ";
            string account_gid5 = objdbconn.GetExecuteScalar(msSQL);
            if (freight != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid," +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid5 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + freight + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='BuyBack/Scrap Value' ";
            string account_gid6 = objdbconn.GetExecuteScalar(msSQL);
            if (buy_back != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid6 + "'," +
                               "'" + remarks + "', " +
                               "'cr'," +
                               "'" + buy_back + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "' and" +
                " module_name='" + module_name + "' and field_name='Packing Charges' ";
            string account_gid7 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid7 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + packing_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='Insurance' ";
            string account_gid8 = objdbconn.GetExecuteScalar(msSQL);
            if (insurance_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid8 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }


            msSQL = "select account_gid from acp_mst_ttax where tax_prefix='" + overall_taxname + "'";                            
            string account_gid9 = objdbconn.GetExecuteScalar(msSQL);
            if (!string.IsNullOrEmpty(overall_taxname))
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid9 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + overalltax_amount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            return "";
        }
        public string jn_purchase_tax(string invoice_gid, string ls_referenceno, string remarks, string lstaxamount, string lstax_gid)
        {
            if (lstaxamount != "0" || lstaxamount != "0.00")
            {

                msSQL = "select account_gid from acp_mst_ttax where tax_gid ='" + lstax_gid + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    objODBCDataReader.Read();
                    account_gid = objODBCDataReader["account_gid"].ToString();

                }
                else
                {
                    return "No account gid available.Cannot raise journal entry";
                }

                msSQL = "select journal_gid from acc_trn_journalentry where transaction_gid ='" + invoice_gid + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    objODBCDataReader.Read();
                    msgetgid = objODBCDataReader["journal_gid"].ToString();

                }
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msgetgid + "'," +
                               "'" + account_gid + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + lstaxamount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            return "";

        }


        public string jn_purchase_invoiceupdate(string invoicedate, string remarks, string branch_name, string invoice_ref, string invoice_gid,
      double basic_amount, double addonCharges, double additionaldiscountAmount, double grandtotal, string vendor_gid, string screenname, string module_name, string purchase_type,
      double roundoff, double freight, double buy_back, string overall_taxname, double overalltax_amount,
      double packing_charges, double insurance_charges)
        {
            jounrnal_date = invoicedate;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msSQL = "select account_gid,vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                vendor_name = objODBCDataReader["vendor_companyname"].ToString();
            }
            else
            {
                return "No account gid available.Cannot raise journal entry";
            }
            //msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            msSQL = " select journal_gid from acc_trn_journalentry where transaction_gid='" + invoice_gid + "'";
            journal_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "UPDATE acc_trn_journalentry SET " +
                     "journal_refno = '" + invoice_ref + "', " +
                     "transaction_code = '" + invoice_ref + "', " +
                     "transaction_date = '" + invoicedate + "', " +
                     "remarks = '" + remarks + "', " +
                     "transaction_type = 'Journal', " +
                     "reference_type = '" + vendor_name + "', " +
                     "reference_gid = '" + vendor_gid + "', " +
                     "transaction_gid = '" + invoice_gid + "', " +
                     "journal_from = 'Purchase', " +
                     "journal_year = '" + journal_year + "', " +
                     "journal_month = '" + journal_month + "', " +
                     "journal_day = '" + journal_Day + "', " +
                     "branch_gid = '" + branch_name + "' " +
                     "WHERE journal_gid = '" + journal_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msSQL = "  delete from acc_trn_journalentrydtl where journal_gid='" + journal_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + journal_gid + "'," +
                           "'" + account_gid + "'," +
                           "'" + remarks + "'," +
                           "'cr'," +
                           "'" + grandtotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='COGS' ";
            string account_gid1 = objdbconn.GetExecuteScalar(msSQL);

            if (purchase_type != "" && purchase_type != null)
            {
                account_gid1 = purchase_type;
            }
            else
            {
                account_gid1 = account_gid;
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + journal_gid + "'," +
                           "'" + account_gid1 + "'," +
                           "'" + remarks + "'," +
                           "'dr'," +
                           "'" + basic_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='Addon Amount' ";
            string account_gid2 = objdbconn.GetExecuteScalar(msSQL);
            if (addonCharges != 0)
            {
                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + journal_gid + "'," +
                          "'" + account_gid2 + "'," +
                          "'" + remarks + "'," +
                          "'dr'," +
                          "'" + addonCharges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
               " and module_name='" + module_name + "' and field_name='Additional Discount' ";
            string account_gid3 = objdbconn.GetExecuteScalar(msSQL);
            if (additionaldiscountAmount != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + journal_gid + "'," +
                        "'" + account_gid3 + "'," +
                        "'" + remarks + "'," +
                        "'dr'," +
                        "'" + -additionaldiscountAmount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='Round Off' ";
            string account_gid4 = objdbconn.GetExecuteScalar(msSQL);
            if (roundoff != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + journal_gid + "'," +
                        "'" + account_gid4 + "'," +
                        "'" + remarks + "'," +
                        "'dr'," +
                        "'" + roundoff + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where " +
                "screen_name='" + screenname + "' and module_name='" + module_name + "' and field_name='Frieght Charges' ";
            string account_gid5 = objdbconn.GetExecuteScalar(msSQL);
            if (freight != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid," +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + journal_gid + "'," +
                               "'" + account_gid5 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + freight + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='BuyBack/Scrap Value' ";
            string account_gid6 = objdbconn.GetExecuteScalar(msSQL);
            if (buy_back != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + journal_gid + "'," +
                               "'" + account_gid6 + "'," +
                               "'" + remarks + "', " +
                               "'cr'," +
                               "'" + buy_back + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "' and" +
                " module_name='" + module_name + "' and field_name='Packing Charges' ";
            string account_gid7 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + journal_gid + "'," +
                               "'" + account_gid7 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + packing_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='Insurance' ";
            string account_gid8 = objdbconn.GetExecuteScalar(msSQL);
            if (insurance_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + journal_gid + "'," +
                               "'" + account_gid8 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }


            msSQL = "select account_gid from acp_mst_ttax where tax_prefix='" + overall_taxname + "'";
            string account_gid9 = objdbconn.GetExecuteScalar(msSQL);
            if (!string.IsNullOrEmpty(overall_taxname))
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + journal_gid + "'," +
                               "'" + account_gid9 + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + overalltax_amount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            return "";
        }
        public string jn_purchase_taxupdate(string invoice_gid, string ls_referenceno, string remarks, string lstaxamount, string lstax_gid)
        {
            if (lstaxamount != "0" || lstaxamount != "0.00")
            {

                msSQL = "select account_gid from acp_mst_ttax where tax_gid ='" + lstax_gid + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    objODBCDataReader.Read();
                    account_gid = objODBCDataReader["account_gid"].ToString();

                }
                else
                {
                    return "No account gid available.Cannot raise journal entry";
                }

                msSQL = "select journal_gid from acc_trn_journalentry where transaction_gid ='" + invoice_gid + "'";
                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                if (objODBCDataReader.HasRows == true)
                {
                    objODBCDataReader.Read();
                    msgetgid = objODBCDataReader["journal_gid"].ToString();

                }
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + journal_gid + "'," +
                               "'" + account_gid + "'," +
                               "'" + remarks + "', " +
                               "'dr'," +
                               "'" + lstaxamount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            return "";

        }

        public void jn_exchange_purchase(string payment_gid, string remarks, double amount, string screenname, string module_name, string type)
        {
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "' and module_name='" + module_name + "' and field_name='" + type + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                account_gid = objODBCDataReader["account_gid"].ToString();
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if (type == "Exchange Gain")
            {
                journal_type = "dr";
            }
            else
            {
                journal_type = "cr";
            }
            msSQL = "select journal_gid from acc_trn_journalentry where journal_refno='" + payment_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                msgetgid = objODBCDataReader["journal_gid"].ToString();
            }
            if (amount != 0 || amount != 0.00)
            {
                msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID + "', " +
                            "'" + msgetgid + "'," +
                            "'" + account_gid + "'," +
                            "'" + remarks + "', " +
                            "'" + journal_type + "'," +
                            "'" + amount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
        }

        public string jn_credit_debit_note(string invoice_date, string invoice_remarks, string branch, string invoice_ref, string invoice_gid,
        double basic_amount, double addon, double add_discount, double grandtotal, string customer_gid, string screen_name, string module_name,
        string sales_type, double roundoff, double frieght_charges, double buyback_charges, double packing_charges, double insurance_charges, double taxamount4, string tax_gid)
        {
            jounrnal_date = invoice_date;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msSQL = "select account_gid,customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                customer_name = objODBCDataReader["customer_name"].ToString();
            }
            else
            {

            }



            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");
            msSQL = " Insert into acc_trn_journalentry " +
                " (journal_gid, " +
                " journal_refno, " +
                " transaction_code, " +
                " transaction_date, " +
                " remarks, " +
                " transaction_type," +
                " reference_type," +
                " reference_gid," +
                " transaction_gid, " +
                " journal_from," +
                " journal_year, " +
                " journal_month, " +
                " journal_day, " +
                " branch_gid" +
                " ) values (" +
                "'" + msGetGID + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_date + "'," +
                "'" + invoice_remarks + "'," +
                "'Journal'," +
                "'" + customer_name + "'," +
                "'" + customer_gid + "'," +
                "'" + invoice_gid + "'," +
                "'Sales'," +
                "'" + journal_year + "'," +
                "'" + journal_month + "'," +
                "'" + journal_Day + "'," +
                "'" + branch + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid + "'," +
                           "'" + invoice_remarks + "'," +
                           "'cr'," +
                           "'" + grandtotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='COGS' ";
            string account_gid1 = objdbconn.GetExecuteScalar(msSQL);

            if (sales_type != "" && sales_type != null)
            {
                msSQL = "select account_gid from smr_trn_tsalestype where salestype_gid ='" + sales_type + "'";
                account_gid1 = objdbconn.GetExecuteScalar(msSQL);
                //account_gid1 = sales_type;
            }
            else
            {
                account_gid1 = account_gid;
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "'," +
                               "'" + msGetGID + "'," +
                               "'" + account_gid1 + "'," +
                               "'" + invoice_remarks + "'," +
                               "'dr'," +
                               "'" + basic_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Addon Amount' ";
            string account_gid2 = objdbconn.GetExecuteScalar(msSQL);
            if (addon != 0)
            {
                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid2 + "'," +
                          "'" + invoice_remarks + "'," +
                          "'dr'," +
                          "'" + addon + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
               " and module_name='" + module_name + "' and field_name='Additional Discount' ";
            string account_gid3 = objdbconn.GetExecuteScalar(msSQL);
            if (add_discount != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid3 + "'," +
                        "'" + invoice_remarks + "'," +
                        "'dr'," +
                        "'" + -add_discount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + tax_gid + "'";

            string account_gid9 = objdbconn.GetExecuteScalar(msSQL);
            if (taxamount4 != 0||taxamount4!=0.00)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

                msSQL = " Insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                          " journal_gid, " +
                          " account_gid," +
                          " remarks," +
                          " journal_type," +
                          " transaction_amount)" +
                          " values (" +
                          "'" + msGetDlGID + "'," +
                          "'" + msGetGID + "'," +
                          "'" + account_gid9 + "'," +
                          "'" + invoice_remarks + "'," +
                          "'dr'," +
                          "'" + taxamount4 + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Round Off' ";
            string account_gid4 = objdbconn.GetExecuteScalar(msSQL);
            if (roundoff != 0)
            {
                msSQL = "Insert into acc_trn_journalentrydtl " +
                        " (journaldtl_gid, " +
                        " journal_gid, " +
                        " account_gid," +
                        " remarks," +
                        " journal_type," +
                        " transaction_amount)" +
                        " values (" +
                        "'" + msGetDlGID + "'," +
                        "'" + msGetGID + "'," +
                        "'" + account_gid4 + "'," +
                        "'" + invoice_remarks + "'," +
                        "'dr'," +
                        "'" + roundoff + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where " +
                "screen_name='" + screen_name + "' and module_name='" + module_name + "' and field_name='Frieght Charges' ";
            string account_gid5 = objdbconn.GetExecuteScalar(msSQL);
            if (frieght_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid," +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid5 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'dr'," +
                               "'" + frieght_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='BuyBack/Scrap Value' ";
            string account_gid6 = objdbconn.GetExecuteScalar(msSQL);
            if (buyback_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid6 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'cr'," +
                               "'" + buyback_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "' and" +
                " module_name='" + module_name + "' and field_name='Packing Charges' ";
            string account_gid7 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid7 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'dr'," +
                               "'" + packing_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screen_name + "'" +
                " and module_name='" + module_name + "' and field_name='Insurance' ";
            string account_gid8 = objdbconn.GetExecuteScalar(msSQL);
            if (packing_charges != 0)
            {
                msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
                msSQL = " Insert into acc_trn_journalentrydtl " +
                               " (journaldtl_gid, " +
                               " journal_gid, " +
                               " account_gid," +
                               " remarks," +
                               " journal_type," +
                               " transaction_amount)" +
                               " values (" +
                               "'" + msGetDlGID + "', " +
                               "'" + msGetGID + "'," +
                               "'" + account_gid8 + "'," +
                               "'" + invoice_remarks + "', " +
                               "'dr'," +
                               "'" + insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            return "";
        }




        public string jn_debit_note(string invoicedate, string remarks, string branch_name, string invoice_ref, string invoice_gid,
 double basic_amount, double grandtotal, string vendor_gid, string screenname, string module_name, string purchase_type )
        {
            jounrnal_date = invoicedate;
            journal_function(jounrnal_date);
            journal_year = journal_monthandyear[0];
            journal_month = journal_monthandyear[1];
            journal_Day = journal_monthandyear[2];

            msSQL = "select account_gid,vendor_companyname from acp_mst_tvendor where vendor_gid ='" + vendor_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                objODBCDataReader.Read();
                account_gid = objODBCDataReader["account_gid"].ToString();
                vendor_name = objODBCDataReader["vendor_companyname"].ToString();
            }
            else
            {
                return "No account gid available.Cannot raise journal entry";
            }
            msGetGID = objcmnfunctions.GetMasterGID_SP("FPCC");


            msSQL = " Insert into acc_trn_journalentry " +
                " (journal_gid, " +
                " journal_refno, " +
                " transaction_code, " +
                " transaction_date, " +
                " remarks, " +
                " transaction_type," +
                " reference_type," +
                " reference_gid," +
                " transaction_gid, " +
                " journal_from," +
                " journal_year, " +
                " journal_month, " +
                " journal_day, " +
                " branch_gid" +
                " ) values (" +
                "'" + msGetGID + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoice_ref + "'," +
                "'" + invoicedate + "'," +
                "'" + remarks + "'," +
                "'Journal'," +
                "'" + vendor_name + "'," +
                "'" + vendor_gid + "'," +
                "'" + invoice_gid + "'," +
                "'Purchase'," +
                "'" + journal_year + "'," +
                "'" + journal_month + "'," +
                "'" + journal_Day + "'," +
                "'" + branch_name + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid + "'," +
                           "'" + remarks + "'," +
                           "'dr'," +
                           "'" + grandtotal + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            msSQL = "select account_gid from acc_mst_accountmapping where screen_name='" + screenname + "'" +
                " and module_name='" + module_name + "' and field_name='COGS' ";
            string account_gid1 = objdbconn.GetExecuteScalar(msSQL);

            if (purchase_type != "" && purchase_type != null)
            {
                msSQL = "select account_gid from pmr_trn_tpurchasetype where purchasetype_gid ='" + purchase_type + "'";
                account_gid1 = objdbconn.GetExecuteScalar(msSQL);
            }
            else
            {
                account_gid1 = account_gid1;
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                           " (journaldtl_gid, " +
                           " journal_gid, " +
                           " account_gid," +
                           " remarks," +
                           " journal_type," +
                           " transaction_amount)" +
                           " values (" +
                           "'" + msGetDlGID + "'," +
                           "'" + msGetGID + "'," +
                           "'" + account_gid1 + "'," +
                           "'" + remarks + "'," +
                           "'cr'," +
                           "'" + basic_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");

        

            return "";
        }


        public void jn_debit_tax(string invoice_gid, string invoice_ref, string remarks, double tax_amount, string tax_gid)
        {
            msSQL = "select account_gid from acp_mst_ttax where tax_gid='" + tax_gid + "' ";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);

            if (objODBCDataReader.HasRows == true)
            {
                account_gid = objODBCDataReader["account_gid"].ToString();
            }
            msGetDlGID = objcmnfunctions.GetMasterGID_SP("FPCD");
            if (msGetDlGID == "E")
            {
                return;
            }
            msSQL = "select journal_gid from acc_trn_journalentry where transaction_gid='" + invoice_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objODBCDataReader.HasRows == true)
            {
                msgetgid = objODBCDataReader["journal_gid"].ToString();
            }

            msSQL = " Insert into acc_trn_journalentrydtl " +
                            " (journaldtl_gid, " +
                            " journal_gid, " +
                            " account_gid," +
                            " remarks," +
                            " journal_type," +
                            " transaction_amount)" +
                            " values (" +
                            "'" + msGetDlGID + "', " +
                            "'" + msgetgid + "'," +
                            "'" + account_gid + "'," +
                            "'" + remarks + "', " +
                            "'cr'," +
                            "'" + tax_amount + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        }


    }
}