using ems.utilities.Functions;
using System;
using ems.kot.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.kot.DataAccess
{
    public class DaUserManagementSummary
    {
        string msSQL = string.Empty;
        DataTable dt_datatable;
        string msSQL1 = string.Empty;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader objOdbcDataReader;
        int mnResult;
        string msUserGid, msEmployeeGID, msBiometricGID, msGetemployeetype, msTemporaryAddressGetGID, msPermanentAddressGetGID, usercode, lsuser_gid, lsemployee_gid, lsuser_code, lscountry_gid2, lscountry_gid, lsdefault_screen;

        public void DaPostuserdetails(user_list values, string user_gid)
        {
            try
            {
                msSQL = " SELECT user_code FROM adm_mst_tuser where user_code = '" + values.user_code + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsuser_code = objOdbcDataReader["user_code"].ToString();
                }
                if (lsuser_code != null && lsuser_code != "")
                {
                    lsuser_code = lsuser_code.ToUpper();
                }
                else
                {
                    lsuser_code = null;
                }
                string uppercaseString = values.user_code.ToUpper();
                if (uppercaseString != lsuser_code)
                {

                    msUserGid = objcmnfunctions.GetMasterGID("SUSM");
                    msSQL = " insert into adm_mst_tuser(" +
                            " user_gid," +
                            " user_code," +
                            " user_firstname," +
                            " user_lastname," +
                            " user_password, " +
                            " user_type, " +
                            " kot_user, " +
                            " user_status, " +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msUserGid + "'," +
                            " '" + values.user_code + "'," +
                            " '" + values.first_name + "'," +
                            " ' '," +
                            " '" + objcmnfunctions.ConvertToAscii(values.password) + "'," +
                            " '" + values.user_type + "'," +
                            "'Y'," +
                            "'Y'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        if(values.user_type=="Kitchen Manager")
                        {
                            lsdefault_screen = "KOTWAO";
                        }
                        else if(values.user_type== "Front Desk")
                        {
                            lsdefault_screen = "KOTFDWAO";

                        }
                        msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                        msBiometricGID = objcmnfunctions.GetBiometricGID();
                        msSQL1 = " Insert into hrm_mst_temployee " +
                                    " (employee_gid , " +
                                    " user_gid," +
                                    " employee_mobileno , " +
                                    " entity_gid," +
                                    " engagement_type," +
                                    " attendance_flag, " +
                                    " branch_gid, " +
                                    " default_screen, " +
                                    " biometric_id, " +
                                    " created_by, " +
                                    " created_date " +
                                    " )values( " +
                                    "'" + msEmployeeGID + "', " +
                                    "'" + msUserGid + "', " +
                                    "'" + values.mobile + "'," +
                                    "'" + values.entityname + "'," +
                                    "'Direct'," +
                                    "'Y'," +
                                    " '" + values.branchname + "'," +
                                    " '" + lsdefault_screen + "'," +
                                    "'" + msBiometricGID + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                        if (mnResult == 1)
                        {
                            msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");
                            msSQL = " insert into hrm_trn_temployeetypedtl(" +
                                      " employeetypedtl_gid," +
                                      " employee_gid," +
                                      " workertype_gid," +
                                      " systemtype_gid, " +
                                      " branch_gid, " +
                                      " wagestype_gid, " +
                                      " employeetype_name, " +
                                      " created_by, " +
                                      " created_date)" +
                                      " values(" +
                                      " '" + msGetemployeetype + "'," +
                                      " '" + msEmployeeGID + "'," +
                                      " 'null'," +
                                      " 'Audit'," +
                                      " '" + values.branchname + "'," +
                                      " 'wg001'," +
                                      "'Roll'," +
                                      "'" + user_gid + "'," +
                                      "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "User Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding User !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "User Code Already Exists";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateuserdetails(user_list values, string user_gid)
        {
            try
            {
                msSQL = " SELECT user_gid FROM hrm_mst_temployee where employee_gid = '" + values.employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                }

                msSQL = " update  adm_mst_tuser set " +
                        " user_firstname = '" + values.first_name + "'," +
                        " user_code = '" + values.user_code + "'," +
                        " user_type = '" + values.user_type + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " update  hrm_mst_temployee set " +
                            " employee_mobileno = '" + values.mobile + "'," +
                            " entity_gid = '" + values.entityname + "'," +
                            " branch_gid = '" + values.branchname + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where employee_gid='" + values.employee_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " update hrm_trn_temployeetypedtl set " +
                                " wagestype_gid='wg001', " +
                                " systemtype_gid='Audit', " +
                                " branch_gid='" + values.branchname + "', " +
                                " employeetype_name='Roll', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " updated_by='" + user_gid + "'" +
                                " where employee_gid = '" + values.employee_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "User Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While User Employee !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetUserSummary(MdlUserManagementSummary values)
        {
            try
            {
                msSQL = " select a.user_gid,a.user_code,a.user_firstname,b.employee_gid,a.user_type,c.branch_name from  adm_mst_tuser a  " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid left join hrm_mst_tbranch c on b.branch_gid = c.branch_gid " +
                        " where kot_user='Y' order by a.user_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<usersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new usersummary_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_name = dt["user_firstname"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            user_type = dt["user_type"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.usersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetViewuserSummary(string employee_gid, MdlUserManagementSummary values)
           {
             try
               {
                msSQL = " select a.user_gid,d.entity_gid,c.branch_gid,a.user_code,a.user_firstname,d.entity_name,b.employee_mobileno,b.employee_gid,a.user_type,c.branch_name from  adm_mst_tuser a  " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid left join hrm_mst_tbranch c on b.branch_gid = c.branch_gid " +
                        " left join adm_mst_tentity d on b.entity_gid = d.entity_gid " +
                        " where b.employee_gid = '" + employee_gid + "' order by a.user_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<userviewsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new userviewsummary_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            entity_gid = dt["entity_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            user_type = dt["user_type"].ToString(),
                            branchname = dt["branch_name"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            entityname = dt["entity_name"].ToString(),
                        });

                        values.userviewsummary_list = getModuleList;

                    }

                }

                dt_datatable.Dispose();

            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading User Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetdeleteuser(string employee_gid, MdlUserManagementSummary values)
        {
            try
            {
                msSQL = " select user_gid from hrm_mst_temployee where employee_gid='" + employee_gid + " '";
                string lsuser_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "delete from adm_mst_tuser " +
                    " where user_gid='" + lsuser_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = "delete from hrm_mst_temployee " +
                    " where employee_gid='" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                if (mnResult != 0)
                {
                   msSQL = "delete from hrm_trn_temployeetypedtl " +
                            " where employee_gid='" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "User Deleted successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting User";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting User!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }



    }
}