using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;
using System.Globalization;
using System.Web.DynamicData;

namespace ems.finance.DataAccess
{
    public class DaAccMstCreditCard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable;
        string  msGetGid, msGetGID2, msGetGID1, lsaccount_name, msGetDlGID2;
        int mnResult;
        public void DaGetAccountGroupName(string user_gid, MdlAccMstCreditCard values)
        {
            msSQL = " SELECT distinct a.account_gid, CONCAT(UCASE(substring(account_name,1,1)),LCASE(SUBSTRING(account_name,2))) as account_name " +
                    " FROM acc_mst_tchartofaccount a " +
                    " where has_child='Y'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var acctgroupname_List = new List<Getacctgroupname_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    acctgroupname_List.Add(new Getacctgroupname_List
                    {
                        account_gid = dt["account_gid"].ToString(),
                        account_name = dt["account_name"].ToString()

                    });
                    values.Getacctgroupname_List = acctgroupname_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCreditCardSummary(string user_gid, MdlAccMstCreditCard values)
        {
            msSQL = " SELECT distinct a.cardholder_name,a.creditcard_no,a.bank_gid, a.bank_code, a.bank_name, " +
                     " a.gl_code,a.account_no,a.account_type,a.ifsc_code,a.neft_code,a.swift_code, " +
                     " format(a.openning_balance, 2) as openning_balance,default_flag,status_flag " +
                     " FROM acc_mst_tcreditcard a where 1 = 1 " +
                     " order by bank_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var creditcard_List = new List<Getcreditcard_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    creditcard_List.Add(new Getcreditcard_List
                    {
                        bank_gid = dt["bank_gid"].ToString(),
                        bank_code = dt["bank_code"].ToString(),
                        bank_name = dt["bank_name"].ToString(),
                        cardholder_name = dt["cardholder_name"].ToString(),
                        creditcard_no = dt["creditcard_no"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        default_flag = dt["default_flag"].ToString(),
                        status_flag = dt["status_flag"].ToString()
                    });
                    values.Getcreditcard_List = creditcard_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostCreditCardDetails(string user_gid, creditcard_list values)
        {
            string dateString = values.date_value;
            DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string formattedDate = date.ToString("yyyy-MM-dd");
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            try
            {
                msSQL = " select case when exists (select creditcard_no from acc_mst_tcreditcard " +
                        " where creditcard_no = '" + values.card_number + "') then 1 else 0 end as card_no";
                string card_no = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select case when exists (select bank_code from acc_mst_tcreditcard " +
                        " where bank_code = '" + values.bank_code + "') then 1 else 0 end as bank_code";
                string bank_code = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select case when exists (select cardholder_name from acc_mst_tcreditcard " +
                        " where cardholder_name= '" + values.cardholder_name + "') then 1 else 0 end as cardholder_name";
                string cardholder_name = objdbconn.GetExecuteScalar(msSQL);
                if (card_no == "1" || bank_code == "1" || cardholder_name == "1")
                {
                   if(card_no == "1" && bank_code == "1")
                    {
                        values.status = false;
                        values.message = "The Same Card Number With This Bank Code Already Exists !!";
                    }
                   else if(card_no =="1" && cardholder_name == "1")
                    {
                        values.status = false;
                        values.message = "The Same Card Number With This Card Holder Name Already Exists !!";
                    }
                   else if(bank_code == "1" && cardholder_name == "1")
                    {
                        values.status = false;
                        values.message = "The Same Bank Code With This Card Holder Name Already Exists !!";
                    }
                   else if(card_no == "1")
                    {
                        values.status = false;
                        values.message = "Card Number Already Exist !!";
                    }
                   else if(bank_code == "1")
                    {
                        values.status = false;
                        values.message = "Bank Code Already Exist !!";
                    }
                   else if(cardholder_name == "1")
                    {
                        values.status = false;
                        values.message = "Card Holder Name Already Exist !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "The Same Card Number, Bank Code and Card Number Already Exists";
                    }
                }
                else
                {
                    msGetGID2 = objcmnfunctions.GetMasterGID("FPCC");
                    msGetGid = objcmnfunctions.GetMasterGID("FATG");

                    msSQL = " INSERT INTO acc_mst_tcreditcard (" +
                            " bank_gid, " +
                            " branch_gid, " +
                            " bank_code, " +
                            " bank_name, " +
                            " creditcard_no, " +
                            " cardholder_name, " +
                            " account_gid, " +
                            " openning_balance, " +
                            " created_by, " +
                            " created_date, " +
                            " gl_code)" +
                            " VALUES (" +
                            "'" + msGetGid + "', " +
                            "'" + values.branch_name + "', " +
                            "'" + values.bank_code + "', " +
                            "'" + values.bank_name + "'," +
                            "'" + values.card_number + "'," +
                            "'" + values.cardholder_name + "'," +
                            "'" + msGetGID2 + "'," +
                            "'" + values.opening_balance.Replace(",","") + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + values.bank_name + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msGetGID1 = objcmnfunctions.GetMasterGID("FPCC");

                        msSQL = " Insert into acc_mst_tchartofaccount " +
                                " (account_gid, " +
                                " account_name, " +
                                " accountgroup_gid, " +
                                " accountgroup_name," +
                                " ledger_type," +
                                " has_child," +
                                " gl_code," +
                                " Created_Date, " +
                                " Created_By, " +
                                " display_type)" +
                                " values (" +
                                "'" + msGetGID2 + "', " +
                                "'" + values.bank_name + "/" + values.cardholder_name + "', " +
                                "'" + values.account_group + "'," +
                                "'" + lsaccount_name + "', " +
                                "'N', " +
                                "'N', " +
                                "'" + values.bank_name + "/" + values.cardholder_name + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                "'" + user_gid + "'," +
                                "'N')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " Insert into acc_trn_journalentry " +
                                  " (journal_gid, " +
                                  " journal_refno," +
                                  " transaction_code, " +
                                  " transaction_date, " +
                                  " transaction_type," +
                                  " reference_type," +
                                  " reference_gid," +
                                  " transaction_gid, " +
                                  " remarks," +
                                  " journal_year, " +
                                  " journal_month, " +
                                  " journal_day, " +
                                  " created_by, " +
                                  " created_date, " +
                                  " branch_gid) " +
                                  " values (" +
                                  "'" + msGetGID1 + "', " +
                                  "'" + values.bank_code + "', " +
                                  "'" + values.bank_code + "', " +
                                  "'" + formattedDate + "', " +
                                  "'Card Opening Balance', " +
                                  "'" + values.bank_name + "'," +
                                  "'" + msGetGid + "'," +
                                  "'" + msGetGid + "',";
                        if (values.remarks == null || values.remarks == "")
                        {
                            msSQL += "'',";
                        }
                        else
                        {
                            msSQL += "'" + values.remarks.Replace("'", "\\\'") + "',";
                        }
                        msSQL += " '" + year + "', " +
                                  " '" + month + "', " +
                                  " '" + day + "', " +
                                  " '" + user_gid + "', " +
                                  " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                  "'" + values.branch_name + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            if (values.transaction_type == "Debit")
                            {
                                msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");

                                msSQL = " Insert into acc_trn_journalentrydtl " +
                                        " (journaldtl_gid, " +
                                        " journal_gid, " +
                                        " transaction_gid," +
                                        " account_gid, " +
                                        " created_by, " +
                                        " created_date, " +
                                        " journal_type, " +
                                        " remarks, " +
                                        " transaction_amount) " +
                                        " values (" +
                                        "'" + msGetDlGID2 + "'," +
                                        "'" + msGetGID1 + "'," +
                                        "'" + msGetGid + "'," +
                                        "'" + msGetGID2 + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        "'dr',";
                                if (values.remarks == null || values.remarks == "")
                                {
                                    msSQL += "'',";
                                }
                                else
                                {
                                    msSQL += "'" + values.remarks.Replace("'", "\\\'") + "',";
                                }
                                msSQL += "'" + values.opening_balance + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Credit Card Details Added Successfully !!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Adding Credit Card Details !!";
                                }
                            }
                            else
                            {
                                msSQL = " Insert into acc_trn_journalentrydtl " +
                                        " (journaldtl_gid, " +
                                        " journal_gid, " +
                                        " account_gid, " +
                                        " transaction_gid," +
                                        " created_by," +
                                        " created_date," +
                                        " journal_type, " +
                                        " remarks, " +
                                        " transaction_amount) " +
                                        " values (" +
                                        "'" + msGetDlGID2 + "'," +
                                        "'" + msGetGID1 + "'," +
                                        "'" + msGetGID2 + "'," +
                                        "'" + msGetGid + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        "'cr',";
                                if (values.remarks == null || values.remarks == "")
                                {
                                    msSQL += "'',";
                                }
                                else
                                {
                                    msSQL += "'" + values.remarks.Replace("'", "\\\'") + "',";
                                }
                                msSQL += "'" + values.opening_balance + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Credit Card Details Added Successfully !!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Adding Credit Card Details !!";
                                }
                            }
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Credit Card Details !!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Credit Card Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetupdatecreditcarddtls(string user_gid, creditcarddtledit_list values)
        {
            try
            {
                msSQL = " select bank_gid from acc_mst_tcreditcard " +
                        " where creditcard_no = '" + values.editcard_number + "' or cardholder_name = '" + values.editcardholder_name + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    msSQL = "select bank_gid from acc_mst_tcreditcard where creditcard_no = '" + values.editcard_number + "'";
                    string gid_based_on_cardno = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = "select bank_gid from acc_mst_tcreditcard where cardholder_name = '" + values.editcardholder_name + "'";
                    string gid_based_on_cardholder = objdbconn.GetExecuteScalar(msSQL);

                    if(gid_based_on_cardholder == values.bank_gid && gid_based_on_cardno == null 
                       || gid_based_on_cardno == "")
                    {
                        msSQL = " update acc_mst_tcreditcard set " +
                        " bank_name ='" + values.editbank_name + "'," +
                        " creditcard_no ='" + values.editcard_number + "'," +
                        " updated_by ='" + user_gid + "'," +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " default_flag ='" + values.default_account + "'" +
                        " where bank_gid='" + values.bank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Credit Card Details Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Credit Card Details !!";
                        }
                    }
                    else if(gid_based_on_cardno == values.bank_gid && gid_based_on_cardholder == null
                       || gid_based_on_cardholder == "")
                    {
                        msSQL = " update acc_mst_tcreditcard set " +
                        " bank_name ='" + values.editbank_name + "'," +
                        " cardholder_name ='" + values.editcardholder_name + "'," +
                        " updated_by ='" + user_gid + "'," +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " default_flag ='" + values.default_account + "'" +
                        " where bank_gid='" + values.bank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Credit Card Details Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Credit Card Details !!";
                        }
                    }
                    else if (gid_based_on_cardholder == values.bank_gid && gid_based_on_cardno == values.bank_gid)
                    {
                        msSQL = " update acc_mst_tcreditcard set " +
                        " bank_name ='" + values.editbank_name + "'," +
                        " updated_by ='" + user_gid + "'," +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " default_flag ='" + values.default_account + "'" +
                        " where bank_gid='" + values.bank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Credit Card Details Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Credit Card Details !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "The Same Card Number With this Card Holder Already Exists!!";
                    }
                }
                else
                { 
                   msSQL = " update acc_mst_tcreditcard set " +
                        " bank_name ='" + values.editbank_name + "'," +
                        " creditcard_no ='" + values.editcard_number + "'," +
                        " cardholder_name ='" + values.editcardholder_name + "'," +
                        " updated_by ='" + user_gid + "'," +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " default_flag ='" + values.default_account + "'" +
                        " where bank_gid='" + values.bank_gid + "'";
                  mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                  if (mnResult == 1)
                  {
                    values.status = true;
                    values.message = "Credit Card Details Updated Successfully !!";
                  }
                  else
                  {
                    values.status = false;
                    values.message = "Error While Updating Credit Card Details !!";
                  }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Source Details";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostCreditCardStatus(string user_gid,string status_flag, string bank_gid, result values)
        {
            try
            {
                msSQL = " Update acc_mst_tcreditcard set " +
                        " status_flag ='" + status_flag + "', " +
                        " updated_date ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where bank_gid='" + bank_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1 && status_flag == "Y")
                {
                    values.status = true;
                    values.message = "Credit Card Activated Successfully !!";
                }
                else if (mnResult == 1 && status_flag == "N")
                {
                    values.status = true;
                    values.message = "Credit Card Deactivated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Activating Bank Master Status";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Credit Card !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Updating Bank Master!! " + " *******" + msSQL + "*******Apiref********", "Finance/ErrorLog/Master/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}