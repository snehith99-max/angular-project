using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq; 
using System.Web;
using MySqlX.XDevAPI;
using Google.Protobuf.WellKnownTypes;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Threading;


namespace ems.hrm.DataAccess
{
    public class DaManualRegulation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        int mnResult, mnResult1, importcount;
        string lsleavegrade_code, lsleavegrade_name, lsattendance_startdate, lsattendance_enddate, lsleavetype_gid, lsleavetype_name, lstotal_leavecount, lsavailable_leavecount, lsleave_limit, lsholidaygrade_gid, lsholiday_gid, lsholiday_date;
        string msUserGid, msEmployeeGID, msBiometricGID, msGetemployeetype, msTemporaryAddressGetGID, msPermanentAddressGetGID, usercode, lsuser_gid, lsemployee_gid, lsuser_code, lscountry_gid2, lscountry_gid, msGetGIDN;
        HttpPostedFile httpPostedFile;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int ErrorCount;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift, msdocument_gid;

        public void DaManualRegulationsummary(string fromdate, string todate, MdlManualRegulation values)
        {
            try
            {
                // Parse fromdate and todate strings into DateTime objects
                DateTime fromDate = DateTime.ParseExact(fromdate, "yyyy-MM-dd", null);
                DateTime toDate = DateTime.ParseExact(todate, "yyyy-MM-dd", null);

                // Initialize count variable
                int count = 1;
                List<string> dynamicDayNames = new List<string>();
                var getdaysList = new List<daylist>();
                string msSQL = "SELECT /*+ MAX_EXECUTION_TIME(900000) */ DISTINCT a.employee_gid, " +
                               "c.user_code, CONCAT(c.user_firstname, ' ', c.user_lastname) AS user_name, " +
                               "br.branch_gid, ";

                for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                {
                    getdaysList.Add(new daylist
                    {
                        days = "Day" + count.ToString(),
                    });
                    dynamicDayNames.Add("day" + count.ToString());
                    // Dynamically construct the column name for each day in the SQL query
                    msSQL += "(SELECT attendance FROM hrm_trn_manualregulation_V x " +
                             "WHERE a.employee_gid = x.employee_gid AND x.shift_date = '" + date.ToString("yyyy-MM-dd") + "') AS day" + count + ", ";

                    count++;
                }

                msSQL += "br.branch_name FROM hrm_mst_temployee a " +
                         "INNER JOIN adm_mst_tuser c ON a.user_gid = c.user_gid " +
                         "INNER JOIN hrm_mst_tbranch br ON a.branch_gid = br.branch_gid " +
                         "LEFT JOIN hrm_trn_temployeetypedtl h ON a.employee_gid = h.employee_gid " +
                         "LEFT JOIN hrm_mst_tsectionassign2employee m ON m.employee_gid = a.employee_gid " +
                         "LEFT JOIN hrm_mst_tsection i ON i.section_gid = m.section_gid " +
                         "LEFT JOIN hrm_mst_tblock z ON z.block_gid = m.block_gid " +
                         "LEFT JOIN hrm_mst_tunit n ON n.unit_gid = m.unit_gid " +
                         "WHERE c.user_status = 'Y' AND a.attendance_flag = 'Y' " +
                         "ORDER BY LENGTH(c.user_code), c.user_code ASC";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<manuallist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var getModuleList1 = new List<daydatalist>();

                        foreach (string dynamicDayName in dynamicDayNames)
                        {
                            // Check if the column exists before accessing it to prevent exceptions
                            if (dt.Table.Columns.Contains(dynamicDayName))
                            {
                                string attendanceValue = dt[dynamicDayName].ToString();

                                // Add this row's data to getModuleList1
                                getModuleList1.Add(new daydatalist
                                {
                                    dayi = dynamicDayName,
                                    attendance = attendanceValue
                                });
                            }
                            else
                            {
                                // Handle case where dynamicDayName column does not exist in the DataTable
                                // This could happen if dynamicDayNames and dt_datatable structure mismatches
                                // You may log an error, skip adding this day, or handle it based on your requirement
                                // Example: Log.Error($"Column {dynamicDayName} not found in the DataTable.");
                            }
                        }

                        // Add the complete data for this row to getModuleList
                        getModuleList.Add(new manuallist
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            daydatalist = getModuleList1
                        });
                    }

                    // After the loops, getModuleList should contain all the necessary data structures
                    // Assign the lists to the 'values' object outside the foreach loop
                    values.dayslist = getdaysList;
                    values.manuallist = getModuleList;


                }
                // Dispose the DataTable outside of the if statement to ensure it's always disposed
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message +
                    "***********" + ex.StackTrace + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void updateManualRegulation(updateday_list values, string user_gid)
        {
            try
            {
                

                DateTime lsattendancedate = DateTime.ParseExact(values.fromdate, "yyyy-MM-dd", null);

                // Assign lsdate
                foreach (var data in values.daydatalist)
                {


                    if (data.attendance == "AA")

                    {

                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                " attendance_type= 'A', " +
                                " employee_attendance= 'Absent', " +
                                " login_time=null, " +
                                " logout_time=null, " +
                                " lunch_out = null, " +
                                " lunch_in = null, " +
                                " update_flag='N', " +
                                " halfdayabsent_flag='N'," +
                                " halfdayleave_flag='N'," +
                                " regulate_flag='C'," +
                                " attendance_source='Manual Regulation'," +
                                " updated_by='" + user_gid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where employee_gid='" + values.employee_gid + "' " +
                                " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);

                        }
                    }
                    else if (data.attendance == "XX")
                    {
                        msSQL = "select attendance_gid,login_scheduled,logout_scheduled from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();
                            DateTime lslogintime = Convert.ToDateTime(objMySqlDataReader["login_scheduled"].ToString());
                            DateTime lslogouttime = Convert.ToDateTime(objMySqlDataReader["logout_scheduled"].ToString());



                            msSQL = " update hrm_trn_tattendance set " +
                                " attendance_type= 'P', " +
                                " employee_attendance= 'Present', " +
                                " login_time='" + lslogintime.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                " logout_time='" + lslogouttime.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                " lunch_out = null, " +
                                " lunch_in = null, " +
                                " update_flag='N', " +
                                " halfdayabsent_flag='N'," +
                                " halfdayleave_flag='N'," +
                                " regulate_flag='C'," +
                                " attendance_source='Manual Regulation'," +
                                " updated_by='" + user_gid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where employee_gid='" + values.employee_gid + "' " +
                                " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "WH")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                " attendance_type= 'WH', " +
                                " employee_attendance= 'Weekoff', " +
                                " login_time=null, " +
                                " logout_time=null, " +
                                " lunch_out = null, " +
                                " lunch_in = null, " +
                                " update_flag='N', " +
                                " halfdayabsent_flag='N'," +
                                " halfdayleave_flag='N'," +
                                " regulate_flag='C'," +
                                " attendance_source='Manual Regulation'," +
                                " updated_by='" + user_gid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where employee_gid='" + values.employee_gid + "' " +
                                " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "LL")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                " attendance_type= 'LL', " +
                                " employee_attendance= 'Leave', " +
                                " login_time=null, " +
                                " logout_time=null, " +
                                " lunch_out = null, " +
                                " lunch_in = null, " +
                                " update_flag='N', " +
                                " halfdayabsent_flag='N'," +
                                " halfdayleave_flag='N'," +
                                " regulate_flag='C'," +
                                " attendance_source='Manual Regulation'," +
                                " updated_by='" + user_gid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where employee_gid='" + values.employee_gid + "' " +
                                " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "NH")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                " attendance_type= 'NH', " +
                                " employee_attendance= 'Holiday', " +
                                " login_time=null, " +
                                " logout_time=null, " +
                                " lunch_out = null, " +
                                " lunch_in = null, " +
                                " update_flag='N', " +
                                " halfdayabsent_flag='N'," +
                                " halfdayleave_flag='N'," +
                                " regulate_flag='C'," +
                                " attendance_source='Manual Regulation'," +
                                " updated_by='" + user_gid + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where employee_gid='" + values.employee_gid + "' " +
                                " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    } 
                    else if (data.attendance == "CO")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                        " employee_attendance='Compoff', " +
                                        " attendance_type= 'Compoff', " +
                                        " halfdayabsent_flag='N'," +
                                        "regulate_flag='C'," +
                                        " attendance_source='Manual Regulation'," +
                                        " logout_time=null, " +
                                        " lunch_out=null, " +
                                        " lunch_in = null, " +
                                        " login_time=null, " +
                                        " updated_by='" + user_gid + "', " +
                                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                        " where employee_gid='" + values.employee_gid + "' " +
                                        " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "OD")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                        " employee_attendance='Present', " +
                                        " attendance_type= 'OD', " +
                                        " halfdayabsent_flag='N'," +
                                        "regulate_flag='C'," +
                                        " attendance_source='Manual Regulation'," +
                                        " logout_time=null, " +
                                        " lunch_out=null, " +
                                        " lunch_in = null, " +
                                        " login_time=null, " +
                                        " updated_by='" + user_gid + "', " +
                                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                        " where employee_gid='" + values.employee_gid + "' " +
                                        " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' "; mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "CL")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                        " employee_attendance='Leave', " +
                                        " attendance_type= 'CL001', " +
                                        " halfdayabsent_flag='N'," +
                                        "regulate_flag='C'," +
                                        " attendance_source='Manual Regulation'," +
                                        " logout_time=null, " +
                                        " lunch_out=null, " +
                                        " lunch_in = null, " +
                                        " login_time=null, " +
                                        " updated_by='" + user_gid + "', " +
                                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                        " where employee_gid='" + values.employee_gid + "' " +
                                        " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' "; mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "SL")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                        " employee_attendance='Leave', " +
                                        " attendance_type= 'SL001', " +
                                        " halfdayabsent_flag='N'," +
                                        "regulate_flag='C'," +
                                        " attendance_source='Manual Regulation'," +
                                        " logout_time=null, " +
                                        " lunch_out=null, " +
                                        " lunch_in = null, " +
                                        " login_time=null, " +
                                        " updated_by='" + user_gid + "', " +
                                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                        " where employee_gid='" + values.employee_gid + "' " +
                                        " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' "; mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objMySqlDataReader.Close();
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }


                    else if (data.attendance == "AL")
                    {
                        msSQL = "select attendance_gid,login_scheduled,logout_scheduled from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();
                            DateTime lslogintime = Convert.ToDateTime(objMySqlDataReader["login_scheduled"].ToString());
                            DateTime lslogouttime = Convert.ToDateTime(objMySqlDataReader["logout_scheduled"].ToString());




                            msSQL = " update hrm_trn_tattendance set " +
                                            " employee_attendance='Absent', " +
                                            " attendance_type= 'AL', " +
                                            " logtout_time='" + lslogouttime.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            " lunch_in = null, " +
                                            " lunch_out= null, " +
                                            " login_time='" + lslogintime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                                    msSQL += " updated_by='" + user_gid + "', " +
                                              " update_flag='N', " +
                                              "regulate_flag='C'," +
                                              " attendance_source='Manual Regulation'," +
                                              " halfdayabsent_flag='Y'," +
                                              " halfdayleave_flag='Y'," +
                                              " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                              " where employee_gid='" + values.employee_gid + "' " +
                                              " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' "; 
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "LA")
                    {
                        msSQL = "select attendance_gid from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();



                            msSQL = " update hrm_trn_tattendance set " +
                                             " employee_attendance='Absent', " +
                                             " attendance_type= 'LA', " +
                                             " login_time=null, " +
                                             " logout_time=null, " +
                                             " lunch_in=null, " +
                                             " lunch_out = null, ";


                                    msSQL += " updated_by='" + user_gid + "', " +
                                              " update_flag='N', " +
                                              " regulate_flag='C'," +
                                              " attendance_source='Manual Regulation'," +
                                              " halfdayabsent_flag='Y'," +
                                              " halfdayleave_flag='Y', " +
                                              " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                              " where employee_gid='" + values.employee_gid + "' " +
                                              " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "LX")
                    {
                        msSQL = "select attendance_gid,login_scheduled,logout_scheduled from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'  ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();
                            DateTime lslogintime = Convert.ToDateTime(objMySqlDataReader["login_scheduled"].ToString());
                            DateTime lslogouttime = Convert.ToDateTime(objMySqlDataReader["logout_scheduled"].ToString());



                            msSQL = " update hrm_trn_tattendance set " +
                                             " employee_attendance='Present', " +
                                             " attendance_type= 'LX', " +
                                             " login_time='" + lslogintime.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                             " logout_time= '" + lslogouttime.ToString("yyyy-MM-dd HH:mm:ss") + "', "+
                                             " lunch_in=null, " +
                                             " lunch_out = null, ";


                            msSQL += " updated_by='" + user_gid + "', " +
                                      " update_flag='N', " +
                                      " regulate_flag='C'," +
                                      " attendance_source='Manual Regulation'," +
                                      " halfdayabsent_flag='Y'," +
                                      " halfdayleave_flag='Y', " +
                                      " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                      " where employee_gid='" + values.employee_gid + "' " +
                                      " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }


                    else if (data.attendance == "XA")
                    {
                        msSQL = "select attendance_gid,login_scheduled,logout_scheduled from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "' ";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();
                            DateTime lslogintime = Convert.ToDateTime(objMySqlDataReader["login_scheduled"].ToString());
                            DateTime lslogouttime = Convert.ToDateTime(objMySqlDataReader["logout_scheduled"].ToString());


                            msSQL = " update hrm_trn_tattendance set " +
                                            " employee_attendance='Present', " +
                                            " attendance_type= 'XA', " +
                                            " logout_time='" + lslogouttime.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            " lunch_in = null, " +
                                            " lunch_out= null, " +
                                            " login_time='" + lslogintime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";

                            msSQL += " updated_by='" + user_gid + "', " +
                                      " update_flag='N', " +
                                      "regulate_flag='C'," +
                                      " attendance_source='Manual Regulation'," +
                                      " halfdayabsent_flag='Y'," +
                                      " halfdayleave_flag='Y'," +
                                      " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                      " where employee_gid='" + values.employee_gid + "' " +
                                      " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "AX")
                    {
                        msSQL = "select attendance_gid,login_scheduled,logout_scheduled from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'  and  employee_gid='" + values.employee_gid + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();
                            DateTime lslogintime = Convert.ToDateTime(objMySqlDataReader["login_scheduled"].ToString());
                            DateTime lslogouttime = Convert.ToDateTime(objMySqlDataReader["logout_scheduled"].ToString());




                            msSQL = " update hrm_trn_tattendance set " +
                                            " employee_attendance='Present', " +
                                            " attendance_type= 'AX', " +
                                            " logout_time='" + lslogouttime.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            " lunch_in = null, " +
                                            " lunch_out= null, " +
                                            " login_time='" + lslogintime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";

                            msSQL += " updated_by='" + user_gid + "', " +
                                      " update_flag='N', " +
                                      "regulate_flag='C'," +
                                      " attendance_source='Manual Regulation'," +
                                      " halfdayabsent_flag='Y'," +
                                      " halfdayleave_flag='Y'," +
                                      " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                      " where employee_gid='" + values.employee_gid + "' " +
                                      " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else if (data.attendance == "XL")
                    {
                        msSQL = "select attendance_gid,login_scheduled,logout_scheduled from hrm_trn_tattendance where attendance_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "'and employee_gid='" + values.employee_gid + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            string lsattendance_gid = objMySqlDataReader["attendance_gid"].ToString();
                            DateTime lslogintime = Convert.ToDateTime(objMySqlDataReader["login_scheduled"].ToString());
                            DateTime lslogouttime = Convert.ToDateTime(objMySqlDataReader["logout_scheduled"].ToString());




                            msSQL = " update hrm_trn_tattendance set " +
                                            " employee_attendance='Present', " +
                                            " attendance_type= 'XL', " +
                                            " logout_time='" + lslogouttime.ToString("yyyy-MM-dd HH:mm:ss") + "', "+
                                            " lunch_in = null, " +
                                            " lunch_out= null, " +
                                            " login_time='" + lslogintime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                            msSQL += " updated_by='" + user_gid + "', " +
                                      " update_flag='N', " +
                                      "regulate_flag='C'," +
                                      " attendance_source='Manual Regulation'," +
                                      " halfdayabsent_flag='Y'," +
                                      " halfdayleave_flag='Y'," +
                                      " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                      " where employee_gid='" + values.employee_gid + "' " +
                                      " and shift_date='" + lsattendancedate.ToString("yyyy-MM-dd") + "' and attendance_gid= '" + lsattendance_gid + "' and attendance_date= '" + lsattendancedate.ToString("yyyy-MM-dd") + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            lsattendancedate = lsattendancedate.AddDays(1);
                        }
                    }
                    else

                    {
                        //msGetGid = objcmnfunctions.GetMasterGID("HATP");
                        //msSQL = " insert into hrm_trn_tattendance ( " +
                        //        " attendance_gid, " +
                        //        " employee_gid, " +
                        //        " shift_date, " +
                        //        " attendance_date, " +
                        //        " login_scheduled, " +
                        //        " logout_scheduled, " +
                        //        " lunch_in_scheduled, " +
                        //        " lunch_out_scheduled, " +
                        //        " grace_time, " +
                        //        " day_mode, " +
                        //        " OT_mode, " +
                        //        " employee_attendance, " +
                        //        " attendance_type, " +
                        //        " halfdayabsent_flag," +
                        //        " halfdayleave_flag," +
                        //        " regulate_flag," +
                        //        " attendance_source," +
                        //        " login_time, " +
                        //        " logout_time, " +
                        //        " lunch_out, " +
                        //        " lunch_in , " +
                        //        " created_by, " +
                        //        " shifttype_gid," +
                        //        " employee2shifttypedtl_gid," +
                        //        " created_date) " +
                        //        " values( " +
                        //        "'" + msGetGid + "', " +
                        //        "'" + values.employee_gid + "'," +
                        //        "'" + lsattendancedate + "'," +
                        //        "'" + lsattendancedate + "'," +
                        //        "'" +  + "'," +
                        //        "'" + lstotime + "'," +
                        //        "'" + lslunchin + "'," +
                        //        "'" + lslunchout + "'," +
                        //        "'" + grace_time + "'," +
                        //        "'S'," +
                        //        "'S'," +
                        //        "'" + lsattendance + "'," +
                        //        "'A'," +
                        //        "'N'," +
                        //        "'N'," +
                        //        "'C'," +
                        //        "'Manual Regulation'," +
                        //        " null," +
                        //        " null," +
                        //        " null," +
                        //        " null," +
                        //        "'" + Session("employee_gid") + "'," +
                        //        " '" + lsshift + "'," +
                        //        " '" + lsemployee2shift + "'," +
                        //        " '" + Format(Now(), "yyyy-MM-dd HH:mm:ss") + "') "
                        //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL)
                        //objODBCDataReader.Close()
                    }
                    }
                    if (mnResult != 0)
                     {
                    values.status = true;
                    values.message = " Attendance Updated Successfully";
                    }
                    else
                    {
                    values.status = false;
                    values.message = "Error While Updating";
                    }


            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public result DaMonthlyAttendanceImport(HttpRequest httpRequest, string user_gid, result objResult, employee_lists values)

        {

            result result = new result();

            try

            {

                HttpFileCollection httpFileCollection;

                HttpPostedFile httpPostedFile;




                HttpContext ctx = HttpContext.Current;

                System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() =>

                {

                    HttpContext.Current = ctx;

                    DaMonthlyAttendanceImports(httpRequest, user_gid, objResult, values);

                }));

                t.Start();



                objResult.status = true;

                objResult.message = " Attendence Imported Inprogress Kindly Check the Error log";

            }

            catch (Exception ex)
            {
                objResult.message = "Error while Sending Message";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;

        }
        public void DaMonthlyAttendanceImports(HttpRequest httpRequest, string user_gid, result objResult, employee_lists values)
        {
            string lscompany_code;
            string excelRange, endRange, lstotalshifthours, lshalfdaymaxhours, lshalfdayminhours, lsortminhours, lsotmaxhours;
            int rowCount, columnCount;

            try
            {
                int insertCount = 0;
                HttpFileCollection httpFileCollection;
                DataTable dt = null;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + " Import_Excel/Hrm_Module/EmployeeExcels/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();

                    msSQL = " insert into hrm_trn_tattendanceuploadexcellog(" +
                            " uploadexcellog_gid," +
                            " fileextenssion," +
                            " uploaded_by, " +
                            " uploaded_date)" +
                            " values(" +
                            " '" + msdocument_gid + "'," +
                            " '" + FileExtension + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.ToString();
                    return;
                }
                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {

                    }
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            using (OleDbCommand command = new OleDbCommand())
                            {
                                command.Connection = connection;
                                command.CommandText = "select * from [Attendance$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                // Upload document
                                importcount = 0;
                                char[] charsToReplace = { '*', ' ', '/', '@', '$', '#', '!', '^', '%', '(', ')', '\'' };

                                // Get the header names from the CSV file
                                List<string> headers = dataTable.Columns.Cast<DataColumn>().Select(column =>
                                    string.Join("", column.ColumnName.Split(charsToReplace, StringSplitOptions.RemoveEmptyEntries))
                                        .ToLower()).ToList();
                                if (dataTable.Rows.Count == 0)
                                {
                                    values.message = "No data found ";
                                    values.status = false;
                                    return;
                                }
                                string[] list = (dataTable.Columns[0].ToString()).Split('/');
                                string month = list[1];
                                string year = list[2];
                                DataTable DT_shiftdtl;
                                List<mdlshiftdetails> mdlshift_dtl= new List<mdlshiftdetails>();
                                int month1 = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;

                                msSQL = " select concat(shift_fromhours, ':', shift_fromminutes, ':', '00') as scheduled_intime," +
                                        " shifttypedtl_name,employee_gid,concat(shift_tohours, ':', shift_tominutes, ':', '00') " +
                                        " as scheduled_outtime,a.shifttype_gid ,shifttypedtl_name from hrm_trn_temployee2shifttype a  " +
                                        " left join hrm_mst_tshifttypedtl b  on a.shifttype_gid = b.shifttype_gid ";
                                DT_shiftdtl = objdbconn.GetDataTable(msSQL);
                                mdlshift_dtl= cmnfunctions.ConvertDataTable<mdlshiftdetails>(DT_shiftdtl);
                                for (int i = 0; i <= dataTable.Rows.Count; i++)
                                {
                                    string employee_code2 = dataTable.Rows[i][0].ToString();
                                    msSQL = "select employee_gid from hrm_mst_temployee a" +
                                            " left join  adm_mst_tuser b on a.user_gid=b.user_gid" +
                                            " where user_code='"+ employee_code2 +"'";
                                    lsemployee_gid=objdbconn.GetExecuteScalar(msSQL);
                                    for (int j = 1; j <= dataTable.Columns.Count -1; j++)
                                    {
                                        string columdata = dataTable.Columns[j].ToString();
                                        string rowdata = dataTable.Rows[i][j].ToString();
                                        string lsdate = columdata + "-" + month1 + "-" + year;
                                        string lsattendance_date = objcmnfunctions.GetDateFormat(lsdate);
                                        string lsshifttype_gid=null, login_time=null, logout_time=null;
                                        //msSQL = " select concat(shift_fromhours, ':', shift_fromminutes, ':', '00') as scheduled_intime," +
                                        //         " concat(shift_tohours, ':', shift_tominutes, ':', '00') as scheduled_outtime,a.shifttype_gid " +
                                        //         " from hrm_trn_temployee2shifttype a " +
                                        //         " left join hrm_mst_tshifttypedtl b  on a.shifttype_gid = b.shifttype_gid " +
                                        //         " where a.employee_gid = '"+ lsemployee_gid +"' and shifttypedtl_name = dayname('" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "') ";
                                        //objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                        //if(objMySqlDataReader.HasRows==true)
                                        //{
                                        //    lsshifttype_gid = objMySqlDataReader["shifttype_gid"].ToString();
                                        //    login_time = DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + " " +objMySqlDataReader["scheduled_intime"].ToString() ;
                                        //    logout_time = DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + " " + objMySqlDataReader["scheduled_outtime"].ToString();
                                        //}
                                        //else
                                        //{
                                        //    lsshifttype_gid = null;
                                        //    login_time = lsattendance_date;
                                        //    logout_time = lsattendance_date;
                                        //}
                                       
                                            string lsattendance_day = DateTime.Parse(lsattendance_date).DayOfWeek.ToString();
                                            List<mdlshiftdetails> employee_shif = mdlshift_dtl.Where(a => a.employee_gid ==  lsemployee_gid
                                            && a.shifttypedtl_name==lsattendance_day).ToList();
                                        foreach(var dr in employee_shif)
                                        {
                                            lsshifttype_gid = dr.shifttype_gid;
                                            login_time = DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + " " + dr.scheduled_intime;
                                            logout_time = DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + " " + dr.scheduled_outtime;
                                        }
                                        if (lsshifttype_gid == "" && lsshifttype_gid==null)
                                        {
                                            lsshifttype_gid = null;
                                            login_time = lsattendance_date;
                                            logout_time = lsattendance_date;
                                        }


                                        msSQL = "select count(attendance_gid) as attendance_gid from hrm_trn_tattendance" +
                                                   " where attendance_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' " +
                                                   " and employee_gid='" +lsemployee_gid + "' ";
                                        string lsgid = objdbconn.GetExecuteScalar(msSQL);
                                        if (lsgid == "0")
                                        {
                                            string msGetGid = objcmnfunctions.GetMasterGID("HATP");
                                            msSQL = " insert into hrm_trn_tattendance ( " +
                                                    " attendance_gid, " +
                                                    " employee_gid, " +
                                                    " shift_date, " +
                                                    " attendance_date, " +
                                                    " login_scheduled, " +
                                                    " logout_scheduled, " +
                                                    " day_mode, " +
                                                    " OT_mode, " +
                                                    " OT_duration, " +
                                                    " employee_attendance, " +
                                                    " attendance_type, " +
                                                    " halfdayabsent_flag," +
                                                    " halfdayleave_flag," +
                                                    " regulate_flag," +
                                                    " attendance_source," +
                                                    " login_time, " +
                                                    " logout_time, " +
                                                    " OT_in, " +
                                                    " OT_out, " +
                                                    " lunch_out, " +
                                                    " lunch_in , " +
                                                    " created_by, " +
                                                    " shifttype_gid," +
                                                    " created_date) " +
                                                    " values( " +
                                                    "'" + msGetGid + "', " +
                                                    "'" + lsemployee_gid + "'," +
                                                    "'" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + login_time +"'," +
                                                    "'" + logout_time + "'," +
                                                    "'S'," +
                                                    "'S'," +
                                                    "null," +
                                                    "'Absent'," +
                                                    "'A'," +
                                                    "'N'," +
                                                    "'N'," +
                                                    "'C'," +
                                                    "'Import Excel'," +
                                                    " null," +
                                                    " null," +
                                                     " null," +
                                                     "null," +
                                                    " null," +
                                                    " null," +
                                                    "'" + lsemployee_gid + "'," +
                                                    " '" + lsshifttype_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                            importcount++;
                                            switch (rowdata)
                                            {
                                            case "XX":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                    " attendance_type= 'P', " +
                                                    " employee_attendance= 'Present', " +
                                                    " login_time='" + DateTime.Parse(login_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    " logout_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    " lunch_out = null, " +
                                                    " lunch_in = null, " +
                                                    " update_flag='N', " +
                                                    " halfdayabsent_flag='N'," +
                                                    " halfdayleave_flag='N'," +
                                                    " regulate_flag='C'," +
                                                    " attendance_source='Manual Regulation'," +
                                                    " updated_by='" + user_gid + "', " +
                                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                    " where employee_gid='" + lsemployee_gid + "' " +
                                                    " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "'" +
                                                    " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";
                                               
                                                break;
                                            case "AA":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                    " attendance_type= 'A', " +
                                                    " employee_attendance= 'Absent', " +
                                                    " login_time='" + DateTime.Parse(login_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    " logout_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    " lunch_out = null, " +
                                                    " lunch_in = null, " +
                                                    " update_flag='N', " +
                                                    " halfdayabsent_flag='N'," +
                                                    " halfdayleave_flag='N'," +
                                                    " regulate_flag='C'," +
                                                    " attendance_source='Manual Regulation'," +
                                                    " updated_by='" + user_gid + "', " +
                                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                    " where employee_gid='" + lsemployee_gid + "' " +
                                                    " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "'" +
                                                    " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";

                                                break;
                                            case "AX":
                                                    msSQL = " update hrm_trn_tattendance set " +
                                                    " attendance_type= 'AX', " +
                                                    " employee_attendance= 'Present', " +
                                                    " login_time = null, " +
                                                    " logout_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                    " lunch_out = null, " +
                                                    " lunch_in = null, " +
                                                    " update_flag='N', " +
                                                    " halfdayabsent_flag='Y'," +
                                                    " halfdayleave_flag='N'," +
                                                    " regulate_flag='C'," +
                                                    " attendance_source='Manual Regulation'," +
                                                    " updated_by='" + user_gid + "', " +
                                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                    " where employee_gid='" + lsemployee_gid + "' " +
                                                    " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' "+
                                                    " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";
                                                    
                                                    break;
                                            case "XA":
                                                    msSQL = " update hrm_trn_tattendance set " +
                                                    " attendance_type= 'XA', " +
                                                    " employee_attendance= 'Present', " +
                                                    " login_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    " logout_time = null, " +
                                                    " lunch_out = null, " +
                                                    " lunch_in = null, " +
                                                    " update_flag='N', " +
                                                    " halfdayabsent_flag='Y'," +
                                                    " halfdayleave_flag='N'," +
                                                    " regulate_flag='C'," +
                                                    " attendance_source='Manual Regulation'," +
                                                    " updated_by='" + user_gid + "', " +
                                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                    " where employee_gid='" + lsemployee_gid + "' " +
                                                    " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";
                                                    
                                                    break;
                                            case "CL":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                " attendance_type= 'CL001', " +
                                                " employee_attendance= 'Leave', " +
                                                " login_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                " logout_time = null, " +
                                                " lunch_out = null, " +
                                                " lunch_in = null, " +
                                                " update_flag='N', " +
                                                " halfdayabsent_flag='Y'," +
                                                " halfdayleave_flag='N'," +
                                                " regulate_flag='C'," +
                                                " attendance_source='Manual Regulation'," +
                                                " updated_by='" + user_gid + "', " +
                                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "' " +
                                                " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";

                                                break;
                                            case "SL":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                " attendance_type= 'SL001', " +
                                                " employee_attendance= 'Leave', " +
                                                " login_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                " logout_time = null, " +
                                                " lunch_out = null, " +
                                                " lunch_in = null, " +
                                                " update_flag='N', " +
                                                " halfdayabsent_flag='Y'," +
                                                " halfdayleave_flag='N'," +
                                                " regulate_flag='C'," +
                                                " attendance_source='Manual Regulation'," +
                                                " updated_by='" + user_gid + "', " +
                                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "' " +
                                                " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";

                                                break;
                                            case "AL":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                " attendance_type= 'AL', " +
                                                " employee_attendance= 'Absent', " +
                                                " login_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                " logout_time = null, " +
                                                " lunch_out = null, " +
                                                " lunch_in = null, " +
                                                " update_flag='N', " +
                                                " halfdayabsent_flag='Y'," +
                                                " halfdayleave_flag='N'," +
                                                " regulate_flag='C'," +
                                                " attendance_source='Manual Regulation'," +
                                                " updated_by='" + user_gid + "', " +
                                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "' " +
                                                " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";

                                                break;
                                            case "LA":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                " attendance_type= 'LA', " +
                                                " employee_attendance= 'Absent', " +
                                                " login_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                " logout_time = null, " +
                                                " lunch_out = null, " +
                                                " lunch_in = null, " +
                                                " update_flag='N', " +
                                                " halfdayabsent_flag='Y'," +
                                                " halfdayleave_flag='N'," +
                                                " regulate_flag='C'," +
                                                " attendance_source='Manual Regulation'," +
                                                " updated_by='" + user_gid + "', " +
                                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "' " +
                                                " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";

                                                break;
                                            case "LX":
                                                    msSQL = " update hrm_trn_tattendance set " +
                                                   " attendance_type= 'LX', " +
                                                   " employee_attendance= 'Present', " +
                                                   " login_time = null, " +
                                                   " logout_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                                   " lunch_out = null, " +
                                                   " lunch_in = null, " +
                                                   " update_flag='N', " +
                                                   " halfdayabsent_flag='N'," +
                                                   " halfdayleave_flag='Y'," +
                                                   " regulate_flag='C'," +
                                                   " attendance_source='Manual Regulation'," +
                                                   " updated_by='" + user_gid + "', " +
                                                   " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                   " where employee_gid='" + lsemployee_gid + "' " +
                                                   " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' " +
                                                   " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";
                                                    
                                                break;
                                            case "XL":
                                                    msSQL = " update hrm_trn_tattendance set " +
                                                   " attendance_type= 'XL', " +
                                                   " employee_attendance= 'Present', " +
                                                   " login_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                   " logout_time = null, " +
                                                   " lunch_out = null, " +
                                                   " lunch_in = null, " +
                                                   " update_flag='N', " +
                                                   " halfdayabsent_flag='N'," +
                                                   " halfdayleave_flag='Y'," +
                                                   " regulate_flag='C'," +
                                                   " attendance_source='Manual Regulation'," +
                                                   " updated_by='" + user_gid + "', " +
                                                   " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                   " where employee_gid='" + lsemployee_gid + "' " +
                                                   " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";
                                                    
                                                    break;
                                            case "LL":
                                                msSQL = " update hrm_trn_tattendance set " +
                                               " attendance_type= 'LL', " +
                                               " employee_attendance= 'Leave', " +
                                               " login_time='" + DateTime.Parse(login_time).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                               " logout_time='" + DateTime.Parse(logout_time).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                               " lunch_out = null, " +
                                               " lunch_in = null, " +
                                               " update_flag='N', " +
                                               " halfdayabsent_flag='N'," +
                                               " halfdayleave_flag='N'," +
                                               " regulate_flag='C'," +
                                               " attendance_source='Manual Regulation'," +
                                               " updated_by='" + user_gid + "', " +
                                               " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                               " where employee_gid='" + lsemployee_gid + "' " +
                                               " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' and attendance_date= '" + lsattendance_date + "' ";
                                                
                                                break;
                                            case "WH":
                                                    msSQL = " update hrm_trn_tattendance set " +
                                                    " attendance_type= 'WH', " +
                                                    " employee_attendance= 'Weekoff', " +
                                                    " login_time = null, " +
                                                    " logout_time = null, " +
                                                    " lunch_out = null, " +
                                                    " lunch_in = null, " +
                                                    " update_flag='N', " +
                                                    " halfdayabsent_flag='N'," +
                                                    " halfdayleave_flag='N'," +
                                                    " regulate_flag='C'," +
                                                    " attendance_source='Manual Regulation'," +
                                                    " updated_by='" + user_gid + "', " +
                                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                    " where employee_gid='" + lsemployee_gid + "' " +
                                                    " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' " +
                                                    " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";
                                                    break;
                                            case "NH":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                " attendance_type= 'NH', " +
                                                " employee_attendance= 'Holiday', " +
                                                " login_time = null, " +
                                                " logout_time = null, " +
                                                " lunch_out = null, " +
                                                " lunch_in = null, " +
                                                " update_flag='N', " +
                                                " halfdayabsent_flag='N'," +
                                                " halfdayleave_flag='N'," +
                                                " regulate_flag='C'," +
                                                " attendance_source='Manual Regulation'," +
                                                " updated_by='" + user_gid + "', " +
                                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "' " +
                                                " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' " +
                                                " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";
                                                break;
                                            case "OD":
                                                msSQL = " update hrm_trn_tattendance set " +
                                                " attendance_type= 'OD', " +
                                                " employee_attendance= 'Onduty', " +
                                                " login_time = null, " +
                                                " logout_time = null, " +
                                                " lunch_out = null, " +
                                                " lunch_in = null, " +
                                                " update_flag='N', " +
                                                " halfdayabsent_flag='N'," +
                                                " halfdayleave_flag='N'," +
                                                " regulate_flag='C'," +
                                                " attendance_source='Manual Regulation'," +
                                                " updated_by='" + user_gid + "', " +
                                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " where employee_gid='" + lsemployee_gid + "' " +
                                                " and shift_date='" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' " +
                                                " and attendance_date= '" + DateTime.Parse(lsattendance_date).ToString("yyyy-MM-dd") + "' ";
                                                break;
                                            }
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                }
                                if (mnResult == 1)
                                {

                                    objResult.status = true;
                                    objResult.message = "Monthly Attendance Added Sucessfully - " + importcount + " out of" + dataTable.Rows.Count;

                                }
                            }
                        }
                    }

                }
                 catch (Exception ex)
                {
                    msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                           " importcount = " + importcount + ", " +
                         " errorlog =' " + ex.Message.ToString() + msSQL + "' " +
                          " where uploadexcellog_gid='" + msdocument_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    objResult.status = false;
                    objResult.message = "Error While Adding Attendance";
                    return;
                }
            }

            catch (Exception ex)
            {
                msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                         " importcount = " + importcount + ", " +
                         " errorlog =' " + ex.Message.ToString()+ msSQL + "' " +
                        " where uploadexcellog_gid='" + msdocument_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                  " importcount = " + importcount + ", " +
                  " errorlog = '" + importcount + "Attendance imported successfully' " +
                  " where uploadexcellog_gid='" + msdocument_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (importcount == 0)
            {

                objResult.status = false;
                objResult.message = "No Attendance data has been imported so Please check the error log.";
            }
            else
            {
                msSQL = " update  hrm_trn_tattendanceuploadexcellog set " +
                 " importcount = " + importcount + ", " +
                 " errorlog = '" + importcount + "Attendance imported successfully' " +
                 " where uploadexcellog_gid='" + msdocument_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                objResult.status = true;
                objResult.message = importcount + "  Attendance data has been imported";
            }
        }

    }
}