﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.hrm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
//using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Text;
using MySql.Data.MySqlClient;



namespace ems.hrm.DataAccess
{
    public class DaIAttendance
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
       string  strClientIP="";
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        DataTable dt_datatable2;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGETGid, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetAttendancedata(string employee_gid,MdlIAttendance values)
        {
            try
            {
                
                msSQL = " select a.* " +
                " from hrm_trn_tattendance a left join hrm_mst_temployee b  on a.employee_gid = b.employee_gid " +
                " where b.employee_gid ='" + employee_gid + "' and " +
                " a.attendance_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' and login_time is not null";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Iattendance_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Iattendance_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            attendance_date = dt["attendance_date"].ToString(),
                            login_time = dt["login_time"].ToString(),
                        });
                        values.Iattendance_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaPostSignIn(string employee_gid, Iattendance_list values)
        {
            try
            {
                
                msSQL = " select * " +
                        " from hrm_trn_tattendance a left join hrm_mst_temployee b on a.employee_gid = b.employee_gid  " +
                        " where b.employee_gid ='" + employee_gid + "' and" +
                        " a.attendance_date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";

                dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable2.Rows.Count != 0)
                {


                    foreach (DataRow dt in dt_datatable2.Rows)
                    {

                        msSQL = " update hrm_trn_tattendance set " +
                            " logout_time = '" + DateTime.Now.ToString("yyyy-MM-dd") +
                            "', logout_ip='" + strClientIP + "', " +
                            " logout_time_audit = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + " " +
                             " logout_location='" + values.location + "'," +
                             " update_flag='N'" + "," +
                             " halfdayabsent_flag='N'" +
                             " where employee_gid = '" + employee_gid + "' and " +
                             " attendance_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    dt_datatable2.Dispose();
                }

                else
                {
                    msGETGid = objcmnfunctions.GetMasterGID("HATP");
                    msSQL = "Insert Into hrm_trn_tattendance" +
                        "(attendance_gid," +
                        " employee_gid," +
                        " attendance_date," +
                        " attendance_source," +
                        " login_time," +
                        " login_time_audit," +
                        " employee_attendance," +
                        " login_ip, " +
                        " location," +
                        " created_date," +
                        " attendance_type)" +
                        " VALUES ( " +
                        "'" + msGETGid + "', " +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'Manual'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'Present'," +
                        "'" + strClientIP + "'," +
                        "'" + values.location + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'P')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Punch In Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Unable to PunchIn";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostSignOut(string employee_gid, Iattendance_list values)
        {
            try
            {
                
                msSQL = " update hrm_trn_tattendance set " +
                    " logout_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    " logout_ip='" + strClientIP + "', " +
                    " logout_time_audit = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    " logout_location='" + values.location + "'," +
                    " update_flag='N'" +
                    " where employee_gid = '" + employee_gid + "' and " +
                    " attendance_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = "  Select (time(login_time)) as logintime,(time(logout_time)) as logouttime, " +
                        " (time(lunch_out_scheduled)) as lunch_out_scheduled from hrm_trn_tattendance " +
                        " where employee_gid = '" + employee_gid + "' and " +
                        " attendance_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader != null)
                {

                }
                else
                {
                    msSQL = " update hrm_trn_tattendance set " +
                " total_duration = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                " update_flag='N'" + "," +
                " halfdayabsent_flag='N'" +
                " where employee_gid = '" + employee_gid + "' and " +
                " attendance_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "PunchOut Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Unable to PunchOut";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
  
























            