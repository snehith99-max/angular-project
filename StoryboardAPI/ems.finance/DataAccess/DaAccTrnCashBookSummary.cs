using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.finance.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Text;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Globalization;
using System.Runtime.Remoting;

namespace ems.finance.DataAccess
{
    public class DaAccTrnCashBookSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL14 = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable;
        string msGetGid, msGetGID1 , msGetDlGID2, msGetDlGID, lsfinyear, lsfinyear_start, lstran_gid, lsdrcr_value, lsreference_gid, lsfinyear_end, lsremarks,lsclosing_amount, lsaccount_gid, lsparent_name,account_gid, lsjournaldtl_gid, lsjournal_gid, lsbranch_code, lsbranch_name, lsgl_code, lsaccountname;
        int mnResult, mnResult1, mnResult2, mnResult3;
        string lsstart_date = "", lsend_date = "";
        public void DaGetAccTrnCashbooksummary(MdlAccTrnCashBookSummary values)
         {
            msSQL14 = " select year(fyear_start) as finyear," +
                      " cast(date_format(fyear_start,'%Y-%m-%d') as char) as finyear_start," +
                      " cast(ifnull(fyear_end,date_format(now(),'%Y-%m-%d')) as char) as finyear_end " +
                      " from adm_mst_tyearendactivities " +
                      " order by finyear_gid desc limit 0,1";
            objODBCDatareader = objdbconn.GetDataReader(msSQL14);

            if (objODBCDatareader.HasRows)
            {
                lsfinyear = objODBCDatareader["finyear"].ToString();
                lsfinyear_start = objODBCDatareader["finyear_start"].ToString();
                lsfinyear_end = objODBCDatareader["finyear_end"].ToString();
            }
            objODBCDatareader.Close();

            msSQL = " select a.branch_gid,a.branch_prefix as branch_name,a.branch_code,a.gl_code, " +
                    " format(a.openning_balance,2) as openning_balance,a.account_gid " +
                    " from hrm_mst_tbranch a " +
                    " left join acc_trn_journalentry b on a.branch_gid=b.branch_gid " +
                    " left join acc_trn_journalentrydtl c on c.journal_gid=b.journal_gid " +
                    " where 1=1 group by a.branch_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<CashBook_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    lsaccount_gid = "FCOA1404070080";
                    msSQL = " select format(((credit_amount-debit_amount) + '" + dt["openning_balance"].ToString() + "'),2) as closing_amount from(" +
                            " select ifnull(sum(case when b.transaction_type  not like '%Opening%'  and a.journal_type='cr' " +
                            " then a.transaction_amount end),0.00)  as credit_amount, " +
                            " ifnull(sum(case when b.transaction_type  not like '%Opening%'  and a.journal_type='dr' " +
                            " then a.transaction_amount end),0.00)  as debit_amount " +
                            " from acc_trn_journalentrydtl a" +
                            " left join acc_trn_journalentry b on a.journal_gid=b.journal_gid" +
                            " left join hrm_mst_tbranch c on c.branch_gid=b.branch_gid " +
                            " left join acc_mst_tchartofaccount e on e.account_gid=a.account_gid" +
                            " ,(SELECT @runtot:=0) d" +
                            " where a.account_gid <> '" + lsaccount_gid + "' and a.transaction_gid='" + lsaccount_gid + "'" +
                            " and b.transaction_date >= '" + lsfinyear_start + "' and b.transaction_date <= '" + lsfinyear_end + "'" +
                            " and c.branch_gid='" + dt["branch_gid"].ToString() + "' order by b.transaction_date asc,a.journaldtl_gid asc)x ";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows)
                    {
                        lsclosing_amount = objODBCDatareader["closing_amount"].ToString();
                    }
                    objODBCDatareader.Close();

                    msSQL = " update hrm_mst_tbranch  set closing_amount='" + lsclosing_amount.Replace(",", "") + "' where " +
                            " branch_gid = '" + dt["branch_gid"].ToString() + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    objdbconn.CloseConn();

                    getModuleList.Add(new CashBook_list
                    {
                        branch_code = dt["branch_code"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        closing_amount = lsclosing_amount
                    });
                    values.CashBook_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }        
        public void DaGetCashBookDtlEdit(string user_gid, MdlAccTrnCashBookSummary values, string branch_gid)
        {
            msSQL = " select a.branch_code ,a.branch_prefix as branch_name, a.gl_code, a.openning_balance,b.remarks, " +
                    " a.externalgl_code, date_format(b.transaction_date,'%d-%m-%Y') as transaction_date " +
                    " from hrm_mst_tbranch a " +
                    " left join acc_trn_journalentry b on b.branch_gid = a.branch_gid " +
                    " where a.branch_gid = '" + branch_gid + "'and b.reference_gid='FCOA1404070080'" +
                    " group by a.branch_gid ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var cashbookdtl_List = new List<Getcashbookdtl_List>();

            if (dt_datatable.Rows.Count != 0)
            { 
                 lsparent_name = "CASH ON HAND";
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    cashbookdtl_List.Add(new Getcashbookdtl_List
                    {
                        branch_code = dt["branch_code"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        openning_balance = dt["openning_balance"].ToString(),
                        externalgl_code = dt["externalgl_code"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        parent_name = lsparent_name,
                        remarks = dt["remarks"].ToString(),
                    });
                    values.Getcashbookdtl_List = cashbookdtl_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateCashBookDtls(string user_gid, cashbookedit_list values)
        {
            try
            {
                string dateString = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                int day = date.Day;
                int month = date.Month;
                int year = date.Year;
                
                if (values.remarks == null || values.remarks == "")
                {
                    lsremarks = "";
                }
                else
                {
                    lsremarks = values.remarks.Replace("'", "\\\'");
                }

                msSQL = " select a.branch_code ,a.branch_prefix as branch_name, a.gl_code " +
                       " from hrm_mst_tbranch a " +
                       " left join acc_trn_journalentry b on b.branch_gid = a.branch_gid " +
                       " where a.branch_gid = '" + values.branch_gid + "'and b.reference_gid='FCOA1404070080'" +
                       " group by a.branch_gid ";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    lsbranch_code = objODBCDatareader["branch_code"].ToString();
                    lsbranch_name = objODBCDatareader["branch_name"].ToString();
                    lsgl_code = objODBCDatareader["gl_code"].ToString();
                }
                objODBCDatareader.Close();

                msSQL = " select a.account_gid from hrm_mst_tbranch a" +
                        " left join acc_mst_tchartofaccount b on b.account_gid=a.account_gid" +
                        " where a.branch_gid='" + values.branch_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader.HasRows == true)
                {
                    lsaccount_gid = objODBCDatareader["account_gid"].ToString();               

                if(lsaccount_gid == "")
                {
                    account_gid = objcmnfunctions.GetMasterGID("FCOA");

                    msSQL = " Insert into acc_mst_tchartofaccount " +
                            " (account_gid, " +
                            " account_name, " +
                            " accountgroup_gid, " +
                            " accountgroup_name," +
                            " ledger_type," +
                            " has_child," +
                            " gl_code," +
                            " external_gl_code," +
                            " Created_Date, " +
                            " Created_By, " +
                            " display_type)" +
                            " values (" +
                            "'" + account_gid + "', " +
                            "'" + lsbranch_name + "', " +
                            "' FCOA1404070080 '," +
                            "'Cash on Hand', " +
                            "'N', " +
                            "'N', " +
                            "'" + lsgl_code + "', " +
                            "'" + values.externalgl_code + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "'," +
                            "'Y')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    account_gid = lsaccount_gid;
                }
                    objODBCDatareader.Close();
                }                
                else
                {
                    account_gid = objcmnfunctions.GetMasterGID("FCOA");

                    msSQL = " Insert into acc_mst_tchartofaccount " +
                              " (account_gid, " +
                              " account_name, " +
                              " accountgroup_gid, " +
                              " accountgroup_name," +
                              " ledger_type," +
                              " has_child," +
                              " gl_code," +
                              " external_gl_code," +
                              " Created_Date, " +
                              " Created_By, " +
                              " display_type)" +
                              " values (" +
                              "'" + account_gid + "', " +
                              "'" + lsbranch_name + "', " +
                              "' FCOA1404070080 '," +
                              "'Cash on Hand', " +
                              "'N', " +
                              "'N', " +
                              "'" + lsgl_code + "', " +
                              "'" + values.externalgl_code + "', " +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                              "'" + user_gid + "'," +
                              "'Y')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                msSQL = " select transaction_amount,a.journaldtl_gid,b.journal_gid from acc_trn_journalentrydtl a " +
                        " left join acc_trn_journalentry b on a.journal_gid=b.journal_gid " +
                        " where b.reference_gid='FCOA1404070080' and b.transaction_type='Cash Opening Balance' " +
                        " and b.branch_gid='" + values.branch_gid + "'";
                objODBCDatareader1 = objdbconn.GetDataReader(msSQL);

                if (objODBCDatareader1.HasRows == true)
                {
                    lsjournaldtl_gid = objODBCDatareader1["journaldtl_gid"].ToString();
                    lsjournal_gid = objODBCDatareader1["journal_gid"].ToString();

                    msSQL = " update acc_trn_journalentrydtl set transaction_amount='" + values.openning_balance.Replace("'", "\'") + "', " +
                            " transaction_gid='FCOA1404070080', updated_by = '" + user_gid + "', updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',  remarks= '" + lsremarks + "'" +
                            " where journaldtl_gid='" + lsjournaldtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update acc_trn_journalentry set  " +
                            " transaction_date='" + Convert.ToDateTime(values.transaction_date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            " updated_by = '" + user_gid + "', updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', remarks= '" + lsremarks + "'" +
                            " where journal_gid='" + lsjournal_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msGetGID1 = objcmnfunctions.GetMasterGID("FCOA");
                    msSQL = " Insert into acc_trn_journalentry " +
                             " (journal_gid, " +
                             " journal_refno, " +
                             " transaction_code, " +
                             " transaction_date, " +
                             " transaction_type," +
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
                             "'" + msGetGID1 + "', " +
                             "'" + lsbranch_code + "', " +
                             "'" + lsbranch_code + "', " +
                             "'" + Convert.ToDateTime(values.transaction_date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                             "'Cash Opening Balance', " +
                             "'" + lsremarks + "', " +
                             "'Cash on Hand', " +
                             "'FCOA1404070080', " +
                             "'" + msGetGID1 + "', " +
                             "'" + year + "', " +
                             "'" + month + "', " +
                             "'" + day + "', " +
                             "'" + user_gid + "', " +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                             "'" + values.branch_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");

                          msSQL = " Insert into acc_trn_journalentrydtl " +
                                  " (journaldtl_gid, " +
                                  " journal_gid, " +
                                  " transaction_amount," +
                                  " remarks," +
                                  " account_gid," +
                                  " transaction_gid," +
                                  " created_by," +
                                  " created_date," +
                                  " journal_type"  +
                                  " )values (" + 
                                  "'" + msGetDlGID2 + "', " +
                                  "'" + msGetGID1 + "'," +
                                  "'" + values.openning_balance + "'," +
                                  "'" + lsremarks + "'," +
                                  "'" + lsaccount_gid + "'," +
                                  "'FCOA1404070080'," +
                                  "'" + user_gid + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                  "'cr')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While while Updating !!";
                    }
                }
                objODBCDatareader1.Close();
                if (mnResult != 0)
                {
                    msSQL = " update hrm_mst_tbranch set" +
                            " gl_code = '" + lsgl_code + "', " +
                            " externalgl_code = '" + values.externalgl_code + "'," +
                            " openning_balance = '" + values.openning_balance + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            " account_gid = '" + lsaccount_gid + "'" +
                            " where branch_gid = '" + values.branch_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Record Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While while Updating !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While while Updating !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDeleteCashBookDtls(MdlAccTrnCashBookSummary values, string journal_gid, string journaldtl_gid, string account_gid)
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
                                values.message = "Cash Book Deleted Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Deleting Cash Book !!";
                            }
                        }
                        else
                        {
                            values.status = true;
                            values.message = "Cash Book Deleted Successfully !!";
                        }
                        objODBCDatareader.Close();
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Cash Book !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Cash Book !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Cash Book";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCashBookEntryView(string user_gid, MdlAccTrnCashBookSummary values, string branch_gid)
        {
            msSQL = "select branch_code, branch_prefix as branch_name, gl_code,account_type,account_gid " +
                    " from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var CashBookEntryView_List = new List<GetCashBookEntryView_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CashBookEntryView_List.Add(new GetCashBookEntryView_List
                    {
                        branch_code = dt["branch_code"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        gl_code = dt["gl_code"].ToString(),
                        account_type = dt["account_type"].ToString(),
                        account_gid = dt["account_gid"].ToString(),
                    });
                    values.GetCashBookEntryView_List = CashBookEntryView_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetCashBookEnterBy(string user_gid, MdlAccTrnCashBookSummary values)
        {
            msSQL = " select concat(a.user_firstname,' - ',c.department_name) as employee_name, " +
                    " b.employee_emailid, b.employee_phoneno, c.department_name " +
                    " from adm_mst_tuser a " +
                    " left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                    " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                    " where a.user_gid = '" + user_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var CashBookEnterBy_List = new List<GetCashBookEnterBy_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CashBookEnterBy_List.Add(new GetCashBookEnterBy_List
                    {
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetCashBookEnterBy_List = CashBookEnterBy_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostAccountMulAddDtls(string user_gid, acctmuladd_list values)
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
                        values.message = "Cash Book Entry Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Cash Book Entry !!";
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
                        values.message = "Cash Book Entry Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Cash Book Entry !!";
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Cash Book Entry!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCashAccountMulAddDtl(string user_gid, MdlAccTrnCashBookSummary values)
        {
            msSQL = " Select distinct a.session_id, format(a.transaction_amount,2) as transaction_amount ," +
                    " format(a.credit_amount,2) as credit_amount,format(a.debit_amount,2) as debit_amount," +
                    " b.accountgroup_name, b.account_name,a.journal_desc as remarks,a.dr_cr " +
                    " from acc_ses_journalentry a " +
                    " left join acc_mst_tchartofaccount b on a.account_gid = b.account_gid " +
                    " where a.userid = '" + user_gid + "' order by a.session_id desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var CashAccountMulAdd_List = new List<GetCashAccountMulAdd_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    CashAccountMulAdd_List.Add(new GetCashAccountMulAdd_List
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
                    values.GetCashAccountMulAdd_List = CashAccountMulAdd_List;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetDeleteMulBankDtls(MdlAccTrnCashBookSummary values, string session_id)
        {
            try
            {
                msSQL = " delete from acc_ses_journalentry  where session_id = '" + session_id + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Cash Book Entry Deleted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Cash Book Entry !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Cash Book Entry";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostDirectCashBookEntry(string user_gid, cashbookadd_list values)
        {
            string dateString = values.transaction_date;
            DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            string lsbalance = "0.00";

            try
            {
                msSQL = "Select bank_gid from acc_mst_tbank where account_gid='" + account_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows == true)
                {
                    lsreference_gid = objODBCDatareader["bank_gid"].ToString();
                }
                else
                {
                    lsreference_gid = values.branch_gid;
                }
                objODBCDatareader.Close();

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
                    msSQL = " select distinct a.account_gid from acc_ses_journalentry a " +
                            " where a.userid = '" + user_gid + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);

                    if (objODBCDatareader.HasRows == true)
                    {
                        lsaccount_gid = objODBCDatareader["account_gid"].ToString();
                    }
                    objODBCDatareader.Close();

                        msSQL = " select distinct a.session_id, " +
                                " a.voucher_no,format(a.debit_amount,2) as debit_amount," +
                                " format(a.credit_amount,2) as credit_amount," +
                                " a.account_desc,a.journal_desc," +
                                " a.dr_cr,a.transaction_amount,a.account_gid,a.total_transaction_amt,a.userid," +
                                " a.accountgroup_gid from acc_ses_journalentry a " +
                                " where a.userid = '" + user_gid + "' order by session_id asc";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<GetSubCash_list>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetSubCash_list
                                {
                                    dr_cr = dt["dr_cr"].ToString(),
                                    journal_desc = dt["journal_desc"].ToString(),
                                    transaction_amount = dt["transaction_amount"].ToString(),
                                    account_gid = dt["account_gid"].ToString(),
                                    account_desc = dt["account_desc"].ToString(),
                                });
                            
                            }
                            for (int i = 0; i < getModuleList.Count; i++)
                            {
                                string lsdr_cr = getModuleList[i].dr_cr;
                                string lsjournal_desc = getModuleList[i].journal_desc;
                                string lstransaction_amount = getModuleList[i].transaction_amount;
                                string lsaccount_desc = getModuleList[i].account_desc;                           

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
                                    "'Cash Book', " +
                                    "'CC001', ";
                                    if (values.direct_remarks == null || values.direct_remarks == "")
                                    {
                                        msSQL += "'',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + values.direct_remarks.Replace("'", "\\\'") + "',";
                                    }                       
                            msSQL += "'" + lsaccount_desc + "', " +
                                     "'" + lsreference_gid + "', " +
                                     "'" + msGetGid + "', " +
                                     "'" + year + "', " +
                                     "'" + month + "', " +
                                     "'" + day + "', " +
                                     "'" + user_gid + "', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                     "'" + values.branch_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            lstran_gid = "FCOA1404070080";

                            msSQL = "select branch_code from hrm_mst_tbranch where branch_gid='" + values.branch_gid+ "'";
                            objODBCDatareader = objdbconn.GetDataReader(msSQL);
                            if (objODBCDatareader.HasRows == true)
                            {
                                lsbranch_code = objODBCDatareader["branch_code"].ToString();
                            }                            
                            objODBCDatareader.Close();

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
                                        "'" + lstran_gid + "'," +
                                        "'" + lsdrcr_value + "'," +
                                        "'" + lsjournal_desc + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        "'" + lstransaction_amount + "')";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            if (mnResult1 != 0)
                            {
                                msSQL = " delete from acc_ses_journalentry " +
                                        " where userid = '" + user_gid + "'";
                                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult3 == 1)
                                {
                                    values.status = true;
                                    values.message = "Cash Book Entry Added Successfully !!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Cash Book Entry !!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Cash Book Entry !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Cash Book Entry !!";
                        }                    
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Cash Book Entry!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

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

        public void DaGetAccTrnCashbookSelect(MdlAccTrnCashBookSummary values, string branch_gid, string from_date, string to_date)
        {
            msSQL = "select branch_name from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
            string lsbranch_name = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select account_gid from acc_trn_topeningbalance where entity_gid='" + branch_gid + "'";
            string account_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select account_name from acc_mst_tchartofaccount where account_gid='" + account_gid + "'";
            string account_name = objdbconn.GetExecuteScalar(msSQL);

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

            //msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.bank_gid, date_format(a.transaction_date, '%d-%m-%Y') as transaction_date, a.journal_refno, c.account_no, " +
            //        " d.account_name, a.remarks, " +
            //        " b.journal_type, a.transaction_type, c.bank_name, format(b.transaction_amount, 2) as transaction_amount " +
            //        " from acc_trn_journalentry a " +
            //        " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
            //        " left join acc_mst_tbank c on c.bank_gid = a.reference_gid " +
            //        " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
            //        " where a.transaction_type not like '%Opening%' and b.transaction_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
            //        " order by a.transaction_date asc ";
            //dt_datatable = objdbconn.GetDataTable(msSQL);

            msSQL = " select a.journal_gid, b.journaldtl_gid, b.transaction_gid, c.branch_gid, date_format(a.transaction_date, '%d-%m-%Y') as transaction_date, a.journal_refno, " +
                    " d.account_name, a.remarks, format(coalesce(e.opening_balance,0),2)as opening_balance, " +
                    " b.journal_type, a.transaction_type, c.branch_name, format(b.transaction_amount, 2) as transaction_amount " +
                    " from acc_trn_journalentry a " +
                    " left join acc_trn_journalentrydtl b on a.journal_gid = b.journal_gid " +
                    " left join hrm_mst_tbranch c on c.branch_gid = a.branch_gid " +
                    " left join acc_mst_tchartofaccount d on d.account_gid = b.account_gid " +
                    " left join acc_trn_topeningbalance e on e.account_gid = b.account_gid  " +
                    " where a.transaction_type not like '%Opening%' and b.transaction_gid='" + account_gid + "' and a.transaction_date >= '" + lsstart_date + "' and a.transaction_date <= '" + lsend_date + "' " +
                    " order by a.transaction_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<CashBookSelect_list>();

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
                    double balance = double.Parse(dt["opening_balance"].ToString()) + lscredit_amount - lsdebit_amount;
                    i += 1;

                    getModuleList.Add(new CashBookSelect_list
                    {
                        s_no = i,
                        journal_gid = dt["journal_gid"].ToString(),
                        journaldtl_gid = dt["journaldtl_gid"].ToString(),
                        transaction_gid = dt["transaction_gid"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        transaction_date = dt["transaction_date"].ToString(),
                        journal_refno = dt["journal_refno"].ToString(),
                        account_name = dt["account_name"].ToString(),
                        //account_no = lsaccount_no,
                        remarks = dt["remarks"].ToString(),
                        journal_type = dt["journal_type"].ToString(),
                        transaction_type = dt["transaction_type"].ToString(),
                        branch_name = lsbranch_name,
                        transaction_amount = dt["transaction_amount"].ToString(),
                        credit_amount = lscredit_amount.ToString("N2"),
                        debit_amount = lsdebit_amount.ToString("N2"),
                        opening_balance = dt["opening_balance"].ToString(),
                        closing_balance = balance.ToString("N2"),
                    });
                    openning_balance_fromdate = closingbal;

                    values.CashBookSelect_list = getModuleList;
                    values.CashBookSelect_list = getModuleList.OrderByDescending(a => a.s_no).ToList();
                }
            }
            dt_datatable.Dispose();
        }

    }
}







//public void DaGetAccTrnCashbookSelect(MdlAccTrnCashBookSummary values, string branch_gid, string finyear_gid)
//{
//    msSQL = " select year(fyear_start) as finyear from adm_mst_tyearendactivities " +
//            " where finyear_gid='" + finyear_gid + "'";
//    objODBCDatareader = objdbconn.GetDataReader(msSQL);
//    if (objODBCDatareader.HasRows)
//    {
//        lsfinyear = objODBCDatareader["finyear"].ToString();
//    }
//    objODBCDatareader.Close();

//    msSQL = " select cast(date_format(fyear_start,'%Y-%m-%d') as char)as finyear_start," +
//           " cast(ifnull(fyear_end,date_format(now(),'%Y-%m-%d')) as char) as finyear_end  " +
//           " from adm_mst_tyearendactivities  " +
//           " where year(fyear_start)='" + lsfinyear + "'";
//    objODBCDatareader = objdbconn.GetDataReader(msSQL);
//    if (objODBCDatareader.HasRows)
//    {
//        lsfinyear_start = objODBCDatareader["finyear_start"].ToString();
//        lsfinyear_end = objODBCDatareader["finyear_end"].ToString();
//    }
//    objODBCDatareader.Close();

//    //msSQL = "select account_gid from hrm_mst_tbranch where branch_gid='" + branch_gid + "' ";
//    //string lsaccount_gid = objdbconn.GetExecuteScalar(msSQL);
//    //if (lsaccount_gid != "")
//    //{
//    //    account_gid = lsaccount_gid;
//    //}
//    //else
//    //{
//    //    account_gid = "FCOA1404070080";
//    //}
//    account_gid = "FCOA1404070080";

//    msSQL = "select ifnull(openning_balance,0.00) as openning_balance from hrm_mst_tbranch where branch_gid='" + branch_gid + "' ";
//    string openning_balance = objdbconn.GetExecuteScalar(msSQL);

//    msSQL = " select journaldtl_gid,DATE_FORMAT(transaction_date, '%d-%m-%Y') AS transaction_date,account_name as account_desc," +
//            " remarks,format(credit_amount,2) as credit_amount," +
//            " format(debit_amount,2) as debit_amount,account_gid,reference_gid,branch_name,journal_gid,journal_refno,closing_amount from" +
//            " (select journaldtl_gid,transaction_date,account_name,remarks,credit_amount,debit_amount," +
//            " account_gid, reference_gid, branch_name,journal_gid,journal_refno, " +
//            " format(" + (openning_balance) + " +(@runtot := (credit_amount-debit_amount + @runtot)),2) as closing_amount  from" +
//            " (select a.journaldtl_gid,b.transaction_date,e.account_name,a.remarks,a.account_gid,b.reference_gid,c.branch_name," +
//            " a.journal_gid,b.journal_refno, ifnull(case when b.transaction_type  not like '%Opening%'  and a.journal_type='cr' " +
//            " then a.transaction_amount end,0.00) as credit_amount, " +
//            " ifnull(case when b.transaction_type  not like '%Opening%'  and a.journal_type='dr' then a.transaction_amount end,0.00) " +
//            " as debit_amount" +
//            " from acc_trn_journalentrydtl a" +
//            " left join acc_trn_journalentry b on a.journal_gid=b.journal_gid" +
//            " left join hrm_mst_tbranch c on c.branch_gid=b.branch_gid" +
//            " left join acc_mst_tchartofaccount e on e.account_gid=a.account_gid" +
//            " ,(SELECT @runtot:=0) d" +
//            " where a.account_gid <> '" + account_gid + "' and a.transaction_gid='" + account_gid + "'" +
//            " and b.transaction_date >= '" + lsfinyear_start + "' " +
//            " and b.transaction_date <= '" + lsfinyear_end + "' and c.branch_gid='" + branch_gid + "' " +
//            " order by b.transaction_date asc,a.journaldtl_gid asc) x) y" +
//            " order by date(y.transaction_date) desc, date(y.transaction_date) asc,y.journaldtl_gid desc";
//    dt_datatable = objdbconn.GetDataTable(msSQL);
//    var getModuleList = new List<CashBookSelect_list>();
//    if (dt_datatable.Rows.Count != 0)
//    {
//        foreach (DataRow dt in dt_datatable.Rows)
//        {
//            getModuleList.Add(new CashBookSelect_list
//            {
//                journal_gid = dt["journal_gid"].ToString(),
//                journaldtl_gid = dt["journaldtl_gid"].ToString(),
//                account_gid = dt["account_gid"].ToString(),
//                transaction_date = dt["transaction_date"].ToString(),
//                journal_refno = dt["journal_refno"].ToString(),
//                branch_name = dt["branch_name"].ToString(),
//                account_desc = dt["account_desc"].ToString(),
//                remarks = dt["remarks"].ToString(),
//                credit_amount = dt["credit_amount"].ToString(),
//                debit_amount = dt["debit_amount"].ToString(),
//                closing_amount = dt["closing_amount"].ToString(),
//            });
//            values.CashBookSelect_list = getModuleList;
//        }
//    }
//    dt_datatable.Dispose();
//}