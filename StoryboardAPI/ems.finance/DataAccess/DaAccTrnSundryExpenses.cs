using ems.finance.Models;
using ems.utilities.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ems.finance.DataAccess
{
    public class DaAccTrnSundryExpenses
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        string msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, msExpensedtlGid, lsentity_code, lsdesignation_code, lsjournal_type, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid, msGetDlGID, lsparent_gid, msGetGID2, msGetGID1, lsaccount_name, msGetDlGID2, lsdrcr_value;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetSundryExpenseSummary(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select format(c.Amount,2) as Amount,a.expense_gid,date_format(a.expense_date,'%d-%m-%Y') as expense_date,a.expense_reference,a.vendor_gid,a.due_date, " +
                        " b.vendor_companyname from acc_trn_tsundryexpenses a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join acc_trn_tsundryexpensesdtl c on c.expense_gid = a.expense_gid group by a.expense_gid " +
                        " order by a.expense_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<expense_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new expense_list
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            expense_reference = dt["expense_reference"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            Amount = dt["Amount"].ToString(),
                        });
                        values.expense_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }        
        public void DaGetAccountGroup(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " SELECT distinct account_gid, account_code, CONCAT(UPPER(account_name)) as account_name " +
                        " FROM acc_mst_tchartofaccount where ledger_type='Y' and display_type='N' and has_child='N' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count > 0)
                {
                    var getModuleList = new List<GetAccGroup>();

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAccGroup
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString()
                        });
                    }
                    values.GetAccGroup = getModuleList;
                    dt_datatable.Dispose();
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
        public void daonchangeaccountgroup(string account_gid, MdlAccTrnSundryExpenses values)
        {
            msSQL = " select accountgroup_gid, accountgroup_name from acc_mst_tchartofaccount where account_gid = '" + account_gid + "' ";
            string subgroup_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select account_gid, concat(upper(accountgroup_name), ' || ' , upper(account_name)) as account_name from acc_mst_tchartofaccount where account_gid = '" + subgroup_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var ParentName_List = new List<GetAccGroup>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable.Rows)
                        {
                            ParentName_List.Add(new GetAccGroup
                            {
                                account_gid = dt1["account_gid"].ToString(),
                                account_name = dt1["account_name"].ToString(),
                            });
                        }
                    }
                }
                values.GetAccGroup = ParentName_List;
            }
            dt_datatable.Dispose();
        }        
        public void DaGetBranchName(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select branch_name, branch_gid from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetBranchname>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchname
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetBranchname = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading expense!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetproducttype(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select producttype_gid, producttype_code , producttype_name from pmr_mst_tproducttype";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getproducttype>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproducttype
                        {
                            producttype_gid = dt["producttype_gid"].ToString(),
                            producttype_code = dt["producttype_code"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                        });
                        values.Getproducttype = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetVendornamedropDown(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = "Select vendor_gid,vendor_companyname from acp_mst_tvendor where blacklist_flag <>'Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetVendorname>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendorname
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                        });
                        values.GetVendorname = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading expense!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetcurrencyCodedropdown(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select distinct a.currencyexchange_gid,a.currency_code,a.default_currency,a.exchange_rate from  crm_trn_tcurrencyexchange a " +
                        " left join acp_mst_tvendor b on a.currencyexchange_gid = b.currencyexchange_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL, "crm_trn_tcurrencyexchange");

                var getModuleList = new List<Getcurrencycode>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcurrencycode
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                        });
                        values.Getcurrencycode = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting currecny!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetOnChangeVendor(string vendor_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select b.address1, b.address2, b.city, b.state, b.postal_code, b.fax,a.contactperson_name, a.vendor_companyname,a.payment_terms," +
                        " a.contact_telephonenumber,a.gst_no, a.taxsegment_gid,c.country_name,a.email_id,a.currencyexchange_gid from acp_mst_tvendor a " +
                        " left join adm_mst_taddress b on b.address_gid = a.address_gid " +
                        " left join adm_mst_tcountry c on c.country_gid = b.country_gid" +
                        " where a.vendor_gid  ='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetVendorChange>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendorChange
                        {
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            gst_no = dt["gst_no"].ToString(),
                        });
                        values.GetVendorChange = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                        " where currencyexchange_gid='" + currencyexchange_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetOnchangeCurrency>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangeCurrency
                        {
                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnchangeCurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing currecy!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostProductsundryexpenses(string employee_gid, posttempledger_list values)
        {
            try
            {
                //string dateString = values.expense_date;
                //DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //int day = date.Day;
                //int month = date.Month;
                //int year = date.Year;

                //msGetGid = objcmnfunctions.GetMasterGID("FPCC");

                //msSQL = " Insert into acc_trn_journalentry " +
                //            " (journal_gid, " +
                //            " journal_refno, " +
                //            " transaction_date, " +
                //            " transaction_type," +
                //            " transaction_code," +
                //            " remarks," +
                //            " reference_type," +
                //            " reference_gid," +
                //            " transaction_gid, " +
                //            " journal_year, " +
                //            " journal_month, " +
                //            " journal_day, " +
                //            " created_by, " +
                //            " created_date, " +
                //            " branch_gid)" +
                //            " values (" +
                //            "'" + msGetGid + "', " +
                //            "'" + values.expense_ref_no + "', " +
                //            "'" + Convert.ToDateTime(values.expense_date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                //            "'Sundry Expense', " +
                //            "null, ";
                //if (values.remarks == null || values.remarks == "")
                //{
                //    msSQL += "'',";
                //}
                //else
                //{
                //    msSQL += "'" + values.remarks.Replace("'", "\\\'") + "',";
                //}
                //msSQL += "'', " +
                //        "'', " +
                //        "'', " +
                //        "'" + year + "', " +
                //        "'" + month + "', " +
                //        "'" + day + "', " +
                //        "'" + employee_gid + "', " +
                //        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                //        "'" + values.branch_gid + "')";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //if (mnResult != 0)
                //{
                //    msSQL = " select distinct a.session_id, " +
                //                " a.voucher_no,format(a.debit_amount,2) as debit_amount,format(a.credit_amount,2) as credit_amount," +
                //                " a.account_desc,a.journal_desc," +
                //                " a.dr_cr,a.transaction_amount,a.account_gid,a.total_transaction_amt,a.userid," +
                //                " a.accountgroup_gid from acc_ses_journalentry a " +
                //                " where a.userid = '" + employee_gid + "' order by session_id asc";
                //    dt_datatable = objdbconn.GetDataTable(msSQL);

                //    var getModuleList = new List<GetSubbank_list>();

                //    if (dt_datatable.Rows.Count != 0)
                //    {
                //        foreach (DataRow dt in dt_datatable.Rows)
                //        {
                //            getModuleList.Add(new GetSubbank_list
                //            {
                //                dr_cr = dt["dr_cr"].ToString(),
                //                journal_desc = dt["journal_desc"].ToString(),
                //                transaction_amount = dt["transaction_amount"].ToString(),
                //            });
                //        }
                //        for (int i = 0; i < getModuleList.Count; i++)
                //        {
                //            string lsdr_cr = getModuleList[i].dr_cr;
                //            string lsjournal_desc = getModuleList[i].journal_desc;
                //            string lstransaction_amount = getModuleList[i].transaction_amount;

                //            if (lsdr_cr == "Deposit")
                //            {
                //                lsdrcr_value = "cr";
                //            }
                //            else
                //            {
                //                lsdrcr_value = "dr";
                //            }

                //            msGetDlGID = objcmnfunctions.GetMasterGID("FPCD");
                //            msSQL = " Insert into acc_trn_journalentrydtl " +
                //                    " (journaldtl_gid, " +
                //                    " journal_gid, " +
                //                    " account_gid," +
                //                    " transaction_gid," +
                //                    " journal_type," +
                //                    " remarks," +
                //                    " created_by," +
                //                    " created_date," +
                //                    " transaction_amount)" +
                //                    " values (" +
                //                    "'" + msGetDlGID + "', " +
                //                    "'" + msGetGid + "'," +
                //                    "'" + values.account_gid + "'," +
                //                    "'" + values.account_gid + "'," +
                //                    "'" + lsdrcr_value + "'," +
                //                    "'" + lsjournal_desc + "'," +
                //                    "'" + employee_gid + "'," +
                //                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                //                    "'" + lstransaction_amount + "')";
                //            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                //        }
                //    }
                //}                    

                msGetGid = objcmnfunctions.GetMasterGID("SUED");

                msSQL = " insert into acc_tmp_tsundryexpensesdtl(" +
                        " expensedtl_gid," +
                        " account_gid," +
                        " account_name," +
                        " maingroup_gid," +
                        " maingroup_name," +
                        " subgroup_gid," +
                        " subgroup_name," +
                        " Amount," +
                        " remarks," +
                        " created_by," +
                        " created_date )" +
                        " values( " +
                        "'" + msGetGid + "', " +
                        "'" + values.Account_name + "', " +
                        "(select account_name from acc_mst_tchartofaccount where account_gid= '" + values.Account_name + "') ," +
                        "(select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + values.Account_grp + "'), " +
                        "(select accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + values.Account_grp + "'), " +
                        "'" + values.Account_grp+ "', " +
                        "(select account_name from acc_mst_tchartofaccount where account_gid= '" + values.Account_grp+ "'), " +
                        "'" + values.total_amount + "', " +
                        "'" + values.remarks + "', "  +
                        "'" + employee_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Account Details Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Account Details";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // temp product summary
        public void DaGetTempProductsSummary(string employee_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = "select expensedtl_gid, account_gid, account_name, maingroup_gid, maingroup_name, subgroup_gid, subgroup_name, format(Amount, 2) as Amount, " +
                        "remarks, created_by, created_date from acc_tmp_tsundryexpensesdtl where created_by ='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Gettemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    { 
                        getModuleList.Add(new Gettemporarysummary
                        {
                            expensedtl_gid = dt["expensedtl_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            maingroup_gid = dt["maingroup_gid"].ToString(),
                            maingroup_name = dt["maingroup_name"].ToString(),
                            subgroup_gid = dt["subgroup_gid"].ToString(),
                            subgroup_name = dt["subgroup_name"].ToString(),
                            Amount = dt["Amount"].ToString(),
                            remarks = dt["remarks"].ToString(),
                        });
                    }
                    values.Gettemporarysummary = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostDirectsundryexpenses(string user_gid, string employee_gid, sundryexpenses_list values)
        {
            try
            {
                msSQL = " select vendor_gid from acc_trn_tsundryexpenses where vendor_gid = '" + values.vendor_companyname + "' ";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                if(dt_datatable.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Vendor name already exist";
                    return;
                }

                msGetGid = objcmnfunctions.GetMasterGID("SUEP");

                string uiDateStr = values.expense_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string expense_date = uiDate.ToString("yyyy-MM-dd");

                msSQL = " insert into acc_trn_tsundryexpenses(" +
                         "expense_gid," +
                         "expense_date," +
                         "vendor_gid," +
                         "vendor_address," +
                         "address2," +
                         "expense_remarks," +
                         "expense_reference," +
                         "exchange_rate," +
                         "currency_code," +
                         "due_date," +
                         "expense_amount," +
                         "payment_term," +
                         "branch_gid," +
                         "created_by," +
                         "expense_status," +
                         "created_date)" +
                         " values( " +
                         "'" + msGetGid + "', " +
                         "'" + expense_date + "', " +
                         "'" + values.vendor_companyname.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "', " +
                         "'" + values.address1.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "', " +
                         "'" + values.shipping_address + "', " +
                         "'" + values.invoice_remarks.Replace("'", "\\'").Replace("）", ")").Replace("（", "(") + "', " +
                         "'" + values.expense_ref_no + "', " +
                         "'" + values.exchange_rate + "', " +
                         "'" + values.currency_code + "', " +
                         "'" + values.due_date + "', " +
                         "'" + values.grandtotal + "', " +
                         "'" + values.payment_term + "', " +
                         "'" + values.branch_name + "', " +
                         "'" + user_gid + "', " +
                         "'Expense Approved', " +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "select expensedtl_gid, account_gid, account_name, maingroup_gid, maingroup_name, subgroup_gid, subgroup_name, " +
                            "Amount, remarks, created_by, created_date from acc_tmp_tsundryexpensesdtl where created_by ='" + employee_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<Gettemporarysummary>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msSQL = "insert into acc_trn_tsundryexpensesdtl (" +
                               " expensedtl_gid," +
                               " expense_gid," +
                               " account_gid," +
                               " account_name," +
                               " maingroup_gid," +
                               " maingroup_name," +
                               " subgroup_gid," +
                               " subgroup_name," +
                               " Amount," +
                               " remarks," +
                               " created_by," +
                               " created_date )" +
                                "values (" +
                                " '" + dt["expensedtl_gid"].ToString() + "'," +
                                " '" + msGetGid + "'," +
                                " '" + dt["account_gid"].ToString() + "'," +
                                " '" + dt["account_name"].ToString() + "'," +
                                " '" + dt["maingroup_gid"].ToString() + "'," +
                                " '" + dt["maingroup_name"].ToString() + "'," +
                                " '" + dt["subgroup_gid"].ToString() + "'," +
                                " '" + dt["subgroup_name"].ToString() + "'," +
                                " '" + dt["Amount"].ToString() + "'," +
                                " '" + dt["remarks"].ToString() + "'," +
                                " '" + dt["created_by"].ToString() + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        if (mnResult == 1)
                        {
                            msSQL = "delete from acc_tmp_tsundryexpensesdtl where created_by ='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Expense raised successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while raising Expenses";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetSundryExpenseView(string expense_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = "select a.payment_term,a.expense_gid,date_format(a.expense_date,'%d-%m-%Y') as expense_date,a.expense_reference,a.vendor_gid," +
                    " a.due_date, b.vendor_companyname " +
                    ",d.address1, d.address2,d. city,d. state,b.contactperson_name,b.contact_telephonenumber,b.email_id," +
                    " d. postal_code,a.address2 as ship_to,a.expense_remarks," +
                    " f.currency_code,f.exchange_rate from acc_trn_tsundryexpenses a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid" +
                    " left join adm_mst_taddress d on d.address_gid = b.address_gid" +
                    " left join acc_trn_tsundryexpensesdtl c on c.expense_gid = a.expense_gid" +
                    " left join crm_trn_tcurrencyexchange f on f.currencyexchange_gid = a.currency_code" +
                    " where a.expense_gid='" + expense_gid + "'  group by a.expense_gid order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<expense_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new expense_list
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            expense_reference = dt["expense_reference"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            invoice_remarks = dt["expense_remarks"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            ship_to = dt["ship_to"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                        });
                        values.expense_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetSundryExpenseViewProducts(string expense_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = " select expensedtl_gid, expense_gid, account_gid, account_name, maingroup_gid, maingroup_name, subgroup_gid, " +
                        "subgroup_name, format(Amount, 2) as Amount, remarks, created_by, created_date from acc_trn_tsundryexpensesdtl where expense_gid ='" + expense_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<sundryledgerview_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new sundryledgerview_list
                        {
                            expensedtl_gid = dt["expensedtl_gid"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            maingroup_name = dt["maingroup_name"].ToString(),
                            subgroup_name = dt["subgroup_name"].ToString(),
                            Amount = dt["Amount"].ToString(),
                            remarks = dt["remarks"].ToString(),
                        });
                        values.sundryledgerview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // edit
        public void DaGetSundryExpenseEdit(string expense_gid, string employee_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = "select a.payment_term,a.expense_gid,date_format(a.expense_date,'%d-%m-%Y') as expense_date,a.expense_reference,a.vendor_gid," +
                    " a.due_date, b.vendor_companyname ,a.branch_gid" +
                    ",d.address1, d.address2,d. city,d. state,b.contactperson_name,b.contact_telephonenumber,b.email_id," +
                    " d. postal_code,a.address2 as ship_to,a.expense_remarks," +
                    " f.currencyexchange_gid,f.exchange_rate from acc_trn_tsundryexpenses a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid" +
                    " left join adm_mst_taddress d on d.address_gid = b.address_gid" +
                    " left join acc_trn_tsundryexpensesdtl c on c.expense_gid = a.expense_gid" +
                    " left join crm_trn_tcurrencyexchange f on f.currencyexchange_gid = a.currency_code" +
                    " where a.expense_gid='" + expense_gid + "' order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<expense_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new expense_list
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            expense_reference = dt["expense_reference"].ToString(),
                            expense_date = dt["expense_date"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            invoice_remarks = dt["expense_remarks"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            ship_to = dt["ship_to"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            currency_code = dt["currencyexchange_gid"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            branch_gid = dt["branch_gid"].ToString()
                        });
                        values.expense_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetSundryExpenseEditProducts(string expense_gid, string employee_gid, MdlAccTrnSundryExpenses values)
        {

            try
            {
                double grand_total = 0.00;

                msSQL = "select expensedtl_gid, expense_gid, account_gid, account_name, maingroup_gid, maingroup_name, subgroup_gid, " +
                        "subgroup_name, Amount, remarks, created_by, created_date from acc_trn_tsundryexpensesdtl where expense_gid ='" + expense_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<sundryledgerview_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new sundryledgerview_list
                        {
                            expensedtl_gid = dt["expensedtl_gid"].ToString(),
                            expense_gid = dt["expense_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            maingroup_name = dt["maingroup_name"].ToString(),
                            subgroup_gid = dt["subgroup_gid"].ToString(),
                            subgroup_name = dt["subgroup_name"].ToString(),
                            Amount = dt["Amount"].ToString(),
                            remarks = dt["remarks"].ToString(),
                        });
                        values.sundryledgerview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // UPDATE
        public void DaUpdateSundryExpenses(string employee_gid, sundryexpenses_list values)
        {
            try
            {
                string inputDate = values.expense_date;
                DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string expense_date = uiDate.ToString("yyyy-MM-dd");

                msSQL = "UPDATE acc_trn_tsundryexpenses  SET " +
                        "expense_date = '" + expense_date + "', " +
                        "vendor_gid = '" + values.vendor_companyname + "', " +
                        "vendor_address = '" + values.address1 + "', " +
                        "address2 = '" + values.shipping_address + "', " +
                        "expense_remarks = '" + values.invoice_remarks + "', " +
                        "expense_reference = '" + values.expense_ref_no + "', " +
                        "exchange_rate = '" + values.exchange_rate + "', " +
                        "currency_code = '" + values.currency_code + "', " +
                        "due_date = '" + values.due_date + "', " +
                        "payment_term = '" + values.payment_term + "', " +
                        "expense_amount = '" + values.grandtotal + "', " +
                        "branch_gid = '" + values.branch_name + "', " +
                        "created_by = '" + employee_gid + "', " +
                        "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                        "where expense_gid = '" + values.expense_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    foreach (var val in values.ledgersEdit_list)
                    {
                        msSQL = "Update acc_trn_tsundryexpensesdtl Set " +
                                "account_gid = '" +val.account_gid + "'," +
                                "account_name = (select account_name from acc_mst_tchartofaccount where account_gid = '" + val.account_gid + "')," +
                                "maingroup_gid = (select accountgroup_gid from acc_mst_tchartofaccount where account_gid= '" + val.subgroup_gid + "')," +
                                "maingroup_name = (select accountgroup_name from acc_mst_tchartofaccount where account_gid= '" + val.subgroup_gid + "')," +
                                "subgroup_gid = '" + val.subgroup_gid + "'," +
                                "subgroup_name = (select account_name from acc_mst_tchartofaccount where account_gid = '" + val.subgroup_gid + "')," +
                                "Amount = '" + val.Amount + "'," +
                                "remarks = '" + val.remarks + "'," +
                                "updated_by = '" + employee_gid + "', " +
                                "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                "where expensedtl_gid = '" + val.expensedtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Expense Updated Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Expenses";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Expenses !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // product update 
        public void DaPostProductUpdatesundryexpenses(string employee_gid, sundryexpenses_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("SUED");
                msGetGid1 = objcmnfunctions.GetMasterGID("SUDE");
                string lsrefno = "";
                
                msSQL = " SELECT product_name FROM pmr_mst_tproduct where product_gid = '" + values.product_name + "'";
                string lsproductname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " insert into acc_tmp_tsundryexpensesdtl(" +                   
                        " expensedtl_gid," +
                        " expense_gid," +
                        " product_gid," +
                        " qty_expense," +
                        " discount_percentage," +
                        " discount_amount," +
                        " tax_amount," +
                        " product_total," +
                        " tax_amount2," +
                        " tax_name," +
                        " tax_name2," +
                        " display_field," +
                        " tax1_gid," +
                        " tax2_gid," +
                        " product_name," +
                        " account_gid," +
                        " account_name," +
                        " tax_percentage," +
                        " tax_percentage2," +
                        " created_by," +
                        " product_price )" +
                        " values( " +                       
                        "'" + msGetGid + "', " +
                        "'" + values.expense_gid + "', " +
                        "'" + values.product_name + "', " +
                        "'" + values.productquantity + "', ";

                if (values.discount_percentage == "" || values.discount_percentage == null)
                {
                    msSQL += "0.00,";
                }
                else
                {
                    msSQL += "'" + values.discount_percentage + "', ";
                }

                msSQL += "'" + values.discount_amount + "', ";

                if (values.taxamount1 == "" || values.taxamount1 == null)
                {
                    msSQL += "0.00,";
                }
                else
                {
                    msSQL += "'" + values.taxamount1 + "', ";
                }

                msSQL += "'" + values.producttotal_amount + "', " +
                        "'" + values.taxamount2 + "', " +
                        "'" + values.tax_name + "', " +
                        "'" + values.taxname2 + "', " +
                        "'" + values.product_remarks + "', " +
                        "'" + values.taxgid1 + "', " +
                        "'" + values.taxgid2 + "', " +
                        "'" + lsproductname + "', " +
                        "'" + values.Account_grp + "', " +
                        "'" + values.Account_name + "', " +
                        "'" + values.taxprecentage1 + "', " +
                        "'" + values.taxprecentage2 + "', " +
                        "'" + employee_gid + "', " +
                        "'" + values.unitprice + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetEditTempProductsSummary(string employee_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                double grand_total = 0.00;

                msSQL = " select a.expensedtl_gid, a.expense_gid, a.product_gid,a. qty_expense,a. product_price, " +
                        " a.discount_percentage,a. discount_amount, a.tax_percentage, a.tax_amount,a. excise_percentage," +
                        " a.excise_amount, a.product_totalprice, a.product_total, a.uom_gid, a.expense_from, a.tax_percentage2, " +
                        " a.tax_amount2, a.tax_percentage3, a.tax_amount3, a.tax_name, a.tax_name2, a.tax_name3, a.display_field," +
                        " a.product_remarks, a.product_price_L, a.discount_amount_L, a.tax_amount1_L, a.tax_amount2_L, a.tax_amount3_L," +
                        " a.tax1_gid, a.tax2_gid, a.tax3_gid, a.productgroup_code, a.productgroup_name, a.product_code, " +
                        " a.productuom_code, a.productuom_name, a.vendor_refnodate, a.producttype_gid, a.created_by, a.created_date," +
                        " a.updated_by, a.updated_date, a.expensedtlref_no, a.asset_flag, a.itc, a.expense_document," +
                        " a.tds_gid, a.tds_amount, a.assetpart, a.taxsegment_gid, a.taxsegmenttax_gid, a.account_gid," +
                        " a.accountgroup_gid, b.account_name,b.accountgroup_name,c.product_name,c.product_desc  " +
                        " from acc_tmp_tsundryexpensesdtl a" +
                        " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid" +
                        " left join pmr_mst_tproduct c on c.product_gid = a.product_gid where a.created_by ='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Gettemporarysummary>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        getModuleList.Add(new Gettemporarysummary
                        {
                            //expensedtl_gid = dt["expensedtl_gid"].ToString(),
                            //product_name = dt["product_name"].ToString(),
                            //discount_amount = dt["discount_amount"].ToString(),
                            //discount_percentage = dt["discount_percentage"].ToString(),
                            //product_price = dt["product_price"].ToString(),
                            //quantity = dt["qty_expense"].ToString(),
                            //product_total = dt["product_total"].ToString(),
                            //tax_name = dt["tax_name"].ToString(),
                            //tax_name2 = dt["tax_name2"].ToString(),
                            //tax_amount = dt["tax_amount"].ToString(),
                            //tax_amount2 = dt["tax_amount2"].ToString(),
                            //product_desc = dt["product_desc"].ToString(),
                            //account_group = dt["accountgroup_name"].ToString(),
                            //account_name = dt["account_name"].ToString()
                        });
                    }
                    values.Gettemporarysummary = getModuleList;
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaSummaryProductDelete(sundryexpenses_list values)
        {
            try
            {
                msSQL = "  delete from  acc_trn_tsundryexpenses where expense_gid='" + values.expense_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "Delete from acc_trn_tsundryexpensesdtl where expense_gid = '" + values.expense_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Expense Deleted Successfully";
                }
                else
                {
                    values.message = "Error While Deleting Expense";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Expense Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            if (objOdbcDataReader != null)
                objOdbcDataReader.Close();
        }
        public void DaTempProductDelete(sundryexpenses_list values)
        {
            try
            {
                msSQL = "  delete from  acc_tmp_tsundryexpensesdtl where expensedtl_gid='" + values.expensedtl_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Acoount Details Deleted Successfully";
                }
                else
                {
                    values.message = "Error While Deleting Account Details";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            if (objOdbcDataReader != null)
                objOdbcDataReader.Close();
        }
        public void DaEditTempProductDelete(sundryexpenses_list values)
        {
            try
            {
                msSQL = "  delete from  acc_trn_tsundryexpensesdtl where expensedtl_gid='" + values.expensedtl_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Deleted Successfully";
                }
                else
                {
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            if (objOdbcDataReader != null)
                objOdbcDataReader.Close();
        }
        //product search
        public void DaGetProductsearchSummary(string producttype_gid, string product_name, string customer_gid, MdlAccTrnSundryExpenses values)
        {
            try
            {
                StringBuilder sqlQuery = new StringBuilder("SELECT a.product_name, a.product_code, a.product_gid, " +
                    " a.cost_price, b.productuom_gid, b.productuom_name, c.productgroup_name, c.productgroup_gid, " +
                    " a.productuom_gid, d.producttype_gid FROM pmr_mst_tproduct a " +
                    " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
                    " LEFT JOIN pmr_mst_tproducttype d ON d.producttype_gid = a.producttype_gid WHERE 1=1");

                if (!string.IsNullOrEmpty(producttype_gid) && producttype_gid != "undefined" && producttype_gid != "null")
                {
                    sqlQuery.Append(" AND a.producttype_gid = '").Append(producttype_gid).Append("'");
                }

                if (!string.IsNullOrEmpty(product_name) && product_name != "null")
                {
                    sqlQuery.Append(" AND a.product_name LIKE '%").Append(product_name).Append("%'");
                }

                dt_datatable = objdbconn.GetDataTable(sqlQuery.ToString());
                var getModuleList = new List<GetProductsearchlist>();
                var allTaxSegmentsList = new List<GetTaxSegmentListDetails>(); // Create list to store all tax segments

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new GetProductsearchlist
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_persentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);

                        if (!string.IsNullOrEmpty(customer_gid) && customer_gid != "undefined")
                        {
                            string productGid = product.product_gid;
                            string productName = product.product_name;

                            StringBuilder taxSegmentQuery = new StringBuilder("SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                                " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                                " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                                " a.cost_price FROM acp_mst_ttaxsegment2product d " +
                                " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                                " LEFT JOIN crm_mst_tcustomer f ON f.taxsegment_gid = e.taxsegment_gid " +
                                " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid = '");
                            taxSegmentQuery.Append(productGid).Append("' AND f.customer_gid = '").Append(customer_gid).Append("'");

                            dt_datatable = objdbconn.GetDataTable(taxSegmentQuery.ToString());

                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt1 in dt_datatable.Rows)
                                {
                                    allTaxSegmentsList.Add(new GetTaxSegmentListDetails
                                    {
                                        product_name = productName,
                                        product_gid = productGid,
                                        taxsegment_gid = dt1["taxsegment_gid"].ToString(),
                                        taxsegment_name = dt1["taxsegment_name"].ToString(),
                                        tax_name = dt1["tax_name"].ToString(),
                                        tax_percentage = dt1["tax_percentage"].ToString(),
                                        tax_gid = dt1["tax_gid"].ToString(),
                                        mrp_price = dt1["mrp_price"].ToString(),
                                        cost_price = dt1["cost_price"].ToString(),
                                        tax_amount = dt1["tax_amount"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                    values.GetProductsearchlist = getModuleList; // Assign GetProductsearchlist to values.GetProductsearchlist
                }
                values.GetTaxSegmentListDetails = allTaxSegmentsList; // Assign allTaxSegmentsList to values.GetTaxSegmentList
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetAllLedgerList(MdlAccTrnSundryExpenses values)
        {
            try
            {
                msSQL = "select account_name, account_gid from acc_mst_tchartofaccount where accountgroup_gid <> '$'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAccGroup>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAccGroup
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                        });
                        values.GetAccGroup = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch(Exception ex) 
            {
                values.message = "Exception occured while Deleting Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}


//public void DaGetAccountGroup(MdlAccTrnSundryExpenses values)
//{
//    msSQL = " select account_gid, UCASE(account_name) as account_name, accountgroup_name from acc_mst_tchartofaccount " +
//            " where accountgroup_gid = '$'";
//    dt_datatable = objdbconn.GetDataTable(msSQL);

//    var ParentName_List = new List<GetAccGroup>();
//    if (dt_datatable.Rows.Count != 0)
//    {
//        foreach (DataRow dt in dt_datatable.Rows)
//        {
//            msSQL1 = " select account_gid,concat(accountgroup_name,' || ',account_name) as account_name from acc_mst_tchartofaccount " +
//                     " where accountgroup_gid= '" + dt["account_gid"].ToString() + "'";
//            DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);
//            if (dt_datatable1.Rows.Count != 0)
//            {
//                foreach (DataRow dt1 in dt_datatable1.Rows)
//                {
//                    ParentName_List.Add(new GetAccGroup
//                    {
//                        account_gid = dt1["account_gid"].ToString(),
//                        account_name = dt1["account_name"].ToString(),
//                    });
//                }
//            }
//        }
//        values.GetAccGroup = ParentName_List;
//    }
//    dt_datatable.Dispose();
//}

//public void daonchangeaccountgroup(string account_gid, MdlAccTrnSundryExpenses values)
//{
//    try
//    {
//        msSQL = " select account_gid, account_name, accountgroup_name from acc_mst_tchartofaccount " +
//                " where accountgroup_gid = '" + account_gid + "' ";
//        dt_datatable = objdbconn.GetDataTable(msSQL);

//        if (dt_datatable.Rows.Count > 0)
//        {
//            var getModuleList = new List<GetAccGroup>();

//            foreach (DataRow dt in dt_datatable.Rows)
//            {
//                getModuleList.Add(new GetAccGroup
//                {
//                    account_gid = dt["account_gid"].ToString(),
//                    account_name = dt["account_name"].ToString()
//                });
//            }
//            values.GetAccGroup = getModuleList;
//            dt_datatable.Dispose();
//        }
//    }
//    catch (Exception ex)
//    {
//        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
//                                     "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
//                                     " * **********" + ex.Message.ToString() + "***********" + msSQL +
//                                     "*******Apiref********", "ErrorLog/Finance " + "Log" +
//                                     DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
//    }
//}