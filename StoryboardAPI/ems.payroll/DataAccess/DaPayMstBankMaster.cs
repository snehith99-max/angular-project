using ems.payroll.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using MySql.Data.MySqlClient;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;

namespace ems.payroll.DataAccess
{
    public class DaPayMstBankMaster
    {
        HttpPostedFile httpPostedFile;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult1, mnResult2, mnResult3;
        string msGetGid, msGetGid1, msGetGid2, msGetDlGID2, msGetGID1, lsempoyeegid, exemployee_code, lsemployeegid, lsbankname;

        // Module Master Summary

        public void DaGetBankMasterSummary(MdlPayMstBankMaster values)
        {
            try
            {
                msSQL = " SELECT distinct a.bank_gid, a.bank_code,b.branch_name,a.bank_name, " +
                    " a.gl_code,a.account_no,a.account_type,a.ifsc_code,a.neft_code,a.swift_code,format(a.openning_balance,2) as openning_balance" +
                    " FROM acc_mst_tbank a " +
                    " left join hrm_mst_tbranch b on b.branch_gid=a.branch_gid " +
                    " where 1=1" + " group by bank_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBankMaster_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBankMaster_list
                        {

                            bank_gid = dt["bank_gid"].ToString(),
                            bank_code = dt["bank_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            bank_name = dt["bank_name"].ToString(),
                            account_type = dt["account_type"].ToString(),
                            account_no = dt["account_no"].ToString(),
                            ifsc_code = dt["ifsc_code"].ToString(),
                            neft_code = dt["neft_code"].ToString(),
                            swift_code = dt["swift_code"].ToString(),
                            openning_balance = dt["openning_balance"].ToString(),


                        });
                        values.GetBankMaster_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank Master!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetBankName(MdlPayMstBankMaster values)
        {
            try
            {
                msSQL = " select concat(bank_prefix_code,' ','/',' ',bank_name) as bank_name From acc_mst_tallbank ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBankName>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBankName
                        {
                            bank_name = dt["bank_name"].ToString(),
                        });
                        values.GetBankName = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank Name!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetAccountType(MdlPayMstBankMaster values)
        {
            try
            {
                msSQL = "select account_gid,account_type " +
                         " from  acc_mst_tbank   ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAccountType>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAccountType
                        {
                            account_gid = dt["account_gid"].ToString(),
                            account_type = dt["account_type"].ToString(),
                        });
                        values.GetAccountType = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Account Type!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetAccountGroup(MdlPayMstBankMaster values)
        {
            try
            {
                msSQL = "select accountgroup_gid,accountgroup_name " +
                         " from  acc_mst_tchartofaccount ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAccountGroup>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAccountGroup
                        {
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),
                        });
                        values.GetAccountGroup = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Account Group!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBranchName(MdlPayMstBankMaster values)
        {
            try
            {
                msSQL = "select branch_gid,branch_name " +
                         " from  hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchName>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchName
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetBranchName = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Branch Name!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostBankMaster(string user_gid, GetBankMaster_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("FATG");
                msGetGid2 = objcmnfunctions.GetMasterGID("FPCC");
                msGetGid1 = objcmnfunctions.GetMasterGID("FPCD");
                msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid ='" + values.branch_name + "'";
                string lsbranch_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select account_type from  acc_mst_tbank where account_type='" + values.account_type + "'";
                string lsaccount_type = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select accountgroup_name from  acc_mst_tchartofaccount where accountgroup_gid='" + values.accountgroup_name + "'";
                string lsaccountgroup_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT bank_code FROM acc_mst_tbank WHERE bank_code='" + values.bank_code + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
     
                    msSQL = " insert into acc_mst_tbank  (" +
                        " bank_gid, " +
                        " bank_code, " +
                        " bank_name, " +
                        " account_no, " +
                        " account_type, " +
                        " ifsc_code, " +
                        " neft_code, " +
                        " swift_code, " +
                        " branch_gid, " +
                        " account_gid, " +
                        " created_date, " +
                        " openning_balance)" +
                        " values (" +
                        "'" + msGetGid + "', " +
                        "'" + values.bank_code + "', " +
                        "'" + values.bank_name + "'," +
                        "'" + values.account_no + "'," +
                        "'" + lsaccount_type + "'," +
                        "'" + values.ifsc_code + "'," +
                        "'" + values.neft_code + "'," +
                        "'" + values.swift_code + "'," +
                        "'" + values.branch_name + "'," +
                        "'" + values.account_type + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + values.openning_balance + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                   
                        msSQL = "select accountgroup_gid,accountgroup_name " +
                             " from  acc_mst_tchartofaccount where  accountgroup_gid = '" + values.accountgroup_name + "'";
                        string accountgroup_name = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = " insert into acc_mst_tchartofaccount   " +
                                " (account_gid, " +
                                " account_name, " +
                                " accountgroup_gid, " +
                                " accountgroup_name," +
                                " ledger_type," +
                                " has_child," +
                                " gl_code," +
                                " Created_Date, " +
                                " Created_By) " +
                                " values (" +
                                "'" + msGetGid2 + "', " +
                                "'" + values.bank_name + "'," +
                                "'" + lsaccountgroup_name + "'," +
                                "'" + accountgroup_name + "'," +
                                "'" + 'N' + "'," +
                                "'" + 'N' + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + 'Y' + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                
                    string date1 = values.created_date;
                    string[] components = date1.Split('-');
                    string year = components[0];
                    string[] components1 = { "January", "February", "March", "April" };
                    string month = components1[1];
                    string[] components2 = { "1", "2", "3", "4" };
                    string day = components2[2];
                    msSQL = " insert into acc_trn_journalentry    " +
                            " (journal_gid, " +
                          " journal_refno, " +
                          " transaction_code, " +
                          " transaction_date, " +
                          " branch_gid," +
                          " reference_type," +
                          " reference_gid," +
                          " transaction_gid, " +
                          " remarks," +
                          " journal_year, " +
                          " journal_month, " +
                          " journal_day, " +
                          " transaction_type)" +
                    " values (" +
                    "'" + msGetGid1 + "', " +
                    "'" + values.bank_code + "'," +
                    "'" + values.bank_code + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'" + values.branch_gid + "'," +
                    "'" + values.bank_name + "'," +
                    "'" + msGetGid + "'," +
                    "'" + msGetGid1 + "'," +
                    "'" + values.remarks + "'," +
                    "'" + day + "'," +
                    "'" + month + "'," +
                    "'" + year + "'," +
                    "'" + "Bank Opening Balance" + "')";
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);


                
                    msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");


                    msSQL = " insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                             " journal_gid, " +
                             " transaction_amount," +
                             " journal_type, " +
                             " remarks, " +
                             " transaction_gid," +
                             " account_gid) " +
                            " values (" +
                            "'" + msGetDlGID2 + "', " +
                            "'" + msGetGid1 + "'," +
                            "'" + values.openning_balance + "'," +
                            "'" + "cr" + "'," +
                            "'" + values.remarks + "'," +
                            "'" + msGetGid + "'," +
                             "'" + msGetGid2 + "')";
                    mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);


                

                if (mnResult3 != 0)
                {
                    values.status = true;
                    values.message = "Bank Master Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Bank Master";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank Master!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetBankMasterDetail(string bank_gid, MdlPayMstBankMaster values)
        {
            try
            {
                msSQL = " SELECT distinct d.branch_name,date_format(a.created_date, '%d-%m-%Y') as created_date,e.accountgroup_name,e.accountgroup_gid,a.bank_gid," +
                        " a.bank_code,a.bank_name,a.account_no,a.account_type,a.ifsc_code,a.account_gid," +
                        " a.neft_code,a.swift_code," +
                        " format(a.openning_balance,2) as openning_balance,g.journal_refno,g.remarks,a.branch_gid " +
                        " FROM  acc_mst_tbank a " +
                        " left join hrm_mst_tbranch d on d.branch_gid=a.branch_gid " +
                        " left join acc_trn_journalentry g on a.branch_gid=g.branch_gid " +
                        " left join acc_mst_tchartofaccount e on a.account_gid=e.account_gid " +
                        " where a.bank_gid = '" + bank_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditBankMaster_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditBankMaster_list
                        {

                            bank_gid = dt["bank_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            accountgroup_gid = dt["accountgroup_gid"].ToString(),
                            bank_code = dt["bank_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            bank_name = dt["bank_name"].ToString(),
                            account_type = dt["account_type"].ToString(),
                            account_no = dt["account_no"].ToString(),
                            ifsc_code = dt["ifsc_code"].ToString(),
                            neft_code = dt["neft_code"].ToString(),
                            swift_code = dt["swift_code"].ToString(),
                            openning_balance = dt["openning_balance"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            accountgroup_name = dt["accountgroup_name"].ToString(),

                        });
                        values.GetEditBankMaster_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank Master Detail!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostBankMasterUpdate(string user_gid, GetEditBankMaster_list values)
        {
            try
            {

                msGetGid1 = objcmnfunctions.GetMasterGID("FPCD");

                msSQL = "select branch_name from  hrm_mst_tbranch where branch_gid ='" + values.branch_gid + "'";
                string lsbranch_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select account_gid from  acc_mst_tbank where bank_gid='" + values.bank_gid + "'";
                string account_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select account_type from  acc_mst_tbank where account_gid='" + account_gid + "'";
                string lsaccount_type = objdbconn.GetExecuteScalar(msSQL);



                //msSQL = "select accountgroup_gid from  acc_mst_tchartofaccount where accountgroup_gid='" + values.accountgroup_name + "'";
                //string lsaccountgroup_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " UPDATE acc_mst_tbank SET " +
                      " bank_name ='" + values.bank_name + "'," +
                      " account_no ='" + values.account_no + "'," +
                      " account_type ='" + lsaccount_type + "'," +
                      " ifsc_code ='" + values.ifsc_code + "'," +
                      " neft_code ='" + values.neft_code + "'," +
                      " swift_code ='" + values.swift_code + "'," +
                      " openning_balance ='" + values.openning_balance + "' " +
                      " WHERE " +
                      " bank_gid='" + values.bank_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msSQL = " update acc_mst_tchartofaccount set" +
                            " account_name='" + values.bank_name + "'," +
                            " accountgroup_name='" + values.accountgroup_name + "'," +
                            " Updated_Date='" + DateTime.Now.ToString("yyyy-mm-dd") + "'," +
                            " Updated_By='" + user_gid + "'," +
                            " gl_code='" + values.bank_name + "'" +
                            " where account_gid='" + values.account_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    string date1 = values.created_date;
                    string[] components = date1.Split('-');
                    string year = components[0];
                    string[] components1 = { "January", "February", "March", "April" };
                    string month = components1[1];
                    string[] components2 = { "1", "2", "3", "4" };
                    string day = components2[2];
                    msSQL = " insert into acc_trn_journalentry    " +
                            " (journal_gid, " +
                          " journal_refno, " +
                          " transaction_code, " +
                          " transaction_date, " +
                          " branch_gid," +
                          " reference_type," +
                          " reference_gid," +
                          " transaction_gid, " +
                          " remarks," +
                          " journal_year, " +
                          " journal_month, " +
                          " journal_day, " +
                          " transaction_type)" +
                    " values (" +
                    "'" + msGetGid1 + "', " +
                    "'" + values.bank_code + "'," +
                    "'" + values.bank_code + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'" + values.branch_gid + "'," +
                    "'" + values.bank_name + "'," +
                    "'" + msGetGid + "'," +
                    "'" + msGetGid1 + "'," +
                    "'" + values.remarks + "'," +
                    "'" + day + "'," +
                    "'" + month + "'," +
                    "'" + year + "'," +
                    "'" + "Bank Opening Balance" + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                }

                if (mnResult != 0)
                {
                    msGetDlGID2 = objcmnfunctions.GetMasterGID("FPCD");


                    msSQL = " insert into acc_trn_journalentrydtl " +
                          " (journaldtl_gid, " +
                             " journal_gid, " +
                             " transaction_amount," +
                             " journal_type, " +
                             " remarks, " +
                             " transaction_gid," +
                             " account_gid) " +
                            " values (" +
                            "'" + msGetDlGID2 + "', " +
                            "'" + msGetGid1 + "'," +
                            "'" + values.openning_balance + "'," +
                            "'" + "cr" + "'," +
                            "'" + values.remarks + "'," +
                            "'" + msGetGid + "'," +
                             "'" + msGetGid2 + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                }


                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Bank Master Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Bank Master ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Bank Master Detail!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

    }
}

