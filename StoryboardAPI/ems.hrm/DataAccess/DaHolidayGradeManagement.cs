using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.hrm.DataAccess
{
    public class DaHolidayGradeManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string lsholidaygrade_code, lsholidaygrade_name;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable, dt_datatable1, dt_datatable2;
        DataTable objTbl;
        int mnResult;
        string msGetGid, msGetGid1, lsholidaygrade_gid, lsholiday_gid, poyeegid, msgetshift;
        DateTime lsholiday_date;

        public void DaHolidayGradeSummary(MdlHolidaygradeManagement values)
        {
            try
            {

                msSQL = "select holidaygrade_gid, holidaygrade_code, holidaygrade_name from hrm_mst_tholidaygrade order by holidaygrade_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidaysummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidaysummary_list
                        {
                            holidaygrade_gid = dt["holidaygrade_gid"].ToString(),
                            holidaygrade_code = dt["holidaygrade_code"].ToString(),
                            holidaygrade_name = dt["holidaygrade_name"].ToString(),
                        });
                        values.holidaysummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaAddHolidayGradesubmit(addholidaygrade_list values)
        {
            try
            {
                //msSQL = " SELECT holiday_name  FROM " +
                //        " hrm_mst_tholiday WHERE holiday_name = '" + values.holiday_name + "'";

                //DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable1.Rows.Count > 0)
                //{
                //    values.status = false;
                //    values.message = "Holiday Name already Exist";
                //    return;
                //}

                string uiDateStr = values.holiday_date;
                DateTime holiday_date = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string holidaymysqlDate = holiday_date.ToString("yyyy-MM-dd");

                msGetGid = objcmnfunctions.GetMasterGID("HHDM");
                {
                    msSQL = " Insert into hrm_mst_tholiday( " +
                        " holiday_gid," +
                        " holiday_name, " +
                        " holiday_type, " +
                        " holiday_date," +
                        " holiday_remarks" + " )" +
                        "values( " +
                        " '" + msGetGid + "'," +
                        "'" + values.holiday_name + "'," +
                        "'" + values.holiday_type + "'," +
                        "'" + holidaymysqlDate + "'," +
                        "'" + values.holiday_remarks + "')";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Holiday Added Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Holiday";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaAddholidaysummary(Addholidayassign_list values)
        {
            try
            {

                msSQL = " select holiday_gid, holiday_date," +
                  " left(holiday_remarks,15) as holidayremarks,holiday_remarks,holiday_name,holiday_type " +
                 " from hrm_mst_tholiday where year(holiday_date)>='" + DateTime.Now.ToString("yyyy") + "' order by holiday_date desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidaygrade1_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidaygrade1_list
                        {
                            holiday_gid = dt["holiday_gid"].ToString(),
                            holiday_date = Convert.ToDateTime(dt["holiday_date"]),
                            holidayremarks = dt["holidayremarks"].ToString(),
                            holiday_remarks = dt["holiday_remarks"].ToString(),
                            holiday_name = dt["holiday_name"].ToString(),
                            holiday_type = dt["holiday_type"].ToString(),
                        });
                        values.holidaygrade1_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaHolidayAssignSubmit(string employee_gid, Addholidayassign_list values)
        {
            try
            {

                msSQL = "select holidaygrade_code,holidaygrade_name from hrm_mst_tholidaygrade";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsholidaygrade_code = objMySqlDataReader["holidaygrade_code"].ToString();
                    lsholidaygrade_name = objMySqlDataReader["holidaygrade_name"].ToString();
                }
                msGetGid = objcmnfunctions.GetMasterGID("HOGD");

                msSQL = "select holidaygrade_code from hrm_mst_tholidaygrade  where holidaygrade_code = '" + values.holidaygrade_code + "'";
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);

                msSQL = "select holidaygrade_name from hrm_mst_tholidaygrade  where holidaygrade_name = '" + values.holidaygrade_name + "'";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

                if (dt_datatable1.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Holiday Grade Code already Exist";
                    return;
                }
                else if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Holiday Grade Name already Exist";
                    return;
                }

                msSQL = " insert into hrm_mst_tholidaygrade( " +
                             " holidaygrade_gid , " +
                             " holidaygrade_code, " +
                             " holidaygrade_name, " +
                             " created_by, " +
                             " created_date ) " +
                             " values( " +
                             " '" + msGetGid + "'," +
                             "'" + values.holidaygrade_code + "'," +
                             "'" + values.holidaygrade_name + "'," +
                             "'" + employee_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                foreach (var data in values.holidaygrade1_list)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("HO2G"); msSQL = " insert into hrm_mst_tholiday2grade ( " +
                                         " holiday2gradedtl_gid, " +
                                         " holidaygrade_gid, " +
                                          " holiday_gid, " +
                                          " holiday_date, " +
                                           " holiday_name) " +
                                         " values ( " +
                                         "'" + msGetGid1 + "', " +
                                         "'" + msGetGid + "', " +
                                         " '" + data.holiday_gid + "'," +
                                        "'" + data.holiday_date.ToString("yyy-MM-dd") + "'," +
                                        "'" + data.holiday_name + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Holiday Grade Assigned Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Assign Holiday Grade";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaDeleteholiday(string holiday_gid, MdlHolidaygradeManagement values)
        {
            try
            {
                msSQL = " select holiday_gid from hrm_mst_tholiday2employee where holiday_gid= '" + holiday_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Holiday Assigned to Employee!!";
                    return;
                }
                else
                {
                    msSQL = "  delete from hrm_mst_tholiday where holiday_gid='" + holiday_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Holiday Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Holiday Adding";
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // Assign Summary
        public void DaHolidaygradeAssignemployee(string holidaygrade_gid, MdlHolidaygradeManagement values)
        {

            try
            {

                msSQL = " select a.employee_gid,b.user_code,concat(b.user_firstname,' ',b.user_lastname) as empname,c.branch_name,d.department_name, " +
                " e.designation_name from hrm_mst_temployee a " +
                " inner join adm_mst_tuser b on a.user_gid=b.user_gid " +
                " inner join hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                " inner join hrm_mst_tdepartment d on a.department_gid=d.department_gid " +
                " inner join adm_mst_tdesignation e on a.designation_gid=e.designation_gid " +
                " where b.user_status='Y' and a.employee_gid not in(select employee_gid  " +
                " from hrm_mst_tholiday2employee where holidaygrade_gid='" + holidaygrade_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidayassignemployee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidayassignemployee
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            empname = dt["empname"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                        });
                        values.holidayassignemployee = getModuleList;
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

        public void DaHolidayAssignSubmitemploye(Holidayemployeesumbit values, string employee_gid)
        {
            try
            {
                msSQL = "select holiday_gid, holiday_date from hrm_mst_tholiday2grade where holidaygrade_gid='" + values.holidaygrade_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                List<holiday> holidays=new List<holiday>();
                holidays = cmnfunctions.ConvertDataTable<holiday>(dt_datatable);

                foreach (var data in values.holidayassignemployee)
                {

                    foreach(var dr in holidays)
                    {
                        lsholiday_gid = dr.holiday_gid;
                        lsholiday_date = Convert.ToDateTime(dr.holiday_date);

                        msGetGid = objcmnfunctions.GetMasterGID_SP("HYTE");
                        if (msGetGid == "E")
                        {
                            values.status = false;
                            values.message = "Error with Sequence Code Generation";
                            return;
                        }
                        msSQL = " insert into hrm_mst_tholiday2employee ( " +
                                " holiday2employee, " +
                                " holidaygrade_gid, " +
                                " holiday_gid, " +
                                " employee_gid, " +
                                " holiday_date, " +
                                " created_by, " +
                                " created_date ) " +
                                " values ( " +
                                "'" + msGetGid + "', " +
                                "'" + values.holidaygrade_gid + "', " +
                                " '" + lsholiday_gid + "'," +
                                "'" + data.employee_gid + "', " +
                                "'" + lsholiday_date.ToString("yyy-MM-dd") + "', " +
                                "'" + employee_gid + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Holiday Assigned Successfully";
                    return;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Holiday Assign";
                    return;
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        // Assign Summary
        public void DaHolidaygradeUnAssignemployee(string holidaygrade_gid, MdlHolidaygradeManagement values)
        {

            try
            {

                msSQL = " select a.employee_gid,b.user_code,concat(b.user_firstname,' ',b.user_lastname) as empname,c.branch_name,d.department_name, " +
                " e.designation_name from hrm_mst_temployee a " +
                " inner join adm_mst_tuser b on a.user_gid=b.user_gid " +
                " inner join hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                " inner join hrm_mst_tdepartment d on a.department_gid=d.department_gid " +
                " inner join adm_mst_tdesignation e on a.designation_gid=e.designation_gid " +
                " where b.user_status='Y' and a.employee_gid  in(select employee_gid  " +
                " from hrm_mst_tholiday2employee where holidaygrade_gid='" + holidaygrade_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidayunassign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidayunassign
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            empname = dt["empname"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                        });
                        values.holidayunassign = getModuleList;
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

        public void DaHolidayUnAssignSubmit(holidayunassignemployeesubmit values, string employee_gid)
        {
            try
            {
                foreach (var data in values.holidayunassign)
                {
                    msSQL = " delete from hrm_mst_tholiday2employee where holidaygrade_gid='" + values.holidaygrade_gid + "' and employee_gid='" + data.employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Holiday Unassigned Successfully";
                    }
                    else
                    {
                            values.status = false;
                            values.message = "Error While Holiday Unassign";
                        
                    }

                }

            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaHolidayEditAssign(string holidaygrade_gid, MdlHolidaygradeManagement values)
        {

            try
            {

                msSQL = " select holidaygrade_gid,holiday_gid,date_format(holiday_date, '%d-%m-%Y') as holiday_date,holiday_name " +
                        " from hrm_mst_tholiday2grade where YEAR(holiday_date) and holidaygrade_gid= '" + holidaygrade_gid + "' order by DATE(holiday_date) asc, holiday_date desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidayeditassign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidayeditassign
                        {
                            holidaygrade_gid = dt["holidaygrade_gid"].ToString(),
                            holiday_gid = dt["holiday_gid"].ToString(),
                            holiday_date = dt["holiday_date"].ToString(),
                            holiday_name = dt["holiday_name"].ToString()

                        });
                        values.holidayeditassign = getModuleList;
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

        public void DaHolidayEditUnassign(MdlHolidaygradeManagement values)
        {

            try
            {

                msSQL = " select holiday_gid,holiday_date,holiday_name from hrm_mst_tholiday where holiday_gid not in ( select holiday_gid from hrm_mst_tholiday2grade) and year(holiday_date)>=year(now()) order by  holiday_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidayeditunassign>();
                if (dt_datatable.Rows.Count != 0)   
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidayeditunassign
                        {
                            holiday_gid = dt["holiday_gid"].ToString(),
                            holiday_date = Convert.ToDateTime(dt["holiday_date"]),
                            holiday_name = dt["holiday_name"].ToString()

                        });
                        values.holidayeditunassign = getModuleList;
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

        public void DaHolidayassign(MdlHolidaygradeManagement values, string holidaygrade_gid)
        {
            try
            {

                msSQL = " select holidaygrade_gid, holidaygrade_code, holidaygrade_name from hrm_mst_tholidaygrade where holidaygrade_gid = '" + holidaygrade_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Holidayassign_type>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Holidayassign_type
                        {
                            holidaygrade_gid = dt["holidaygrade_gid"].ToString(),
                            holidaygrade_code = dt["holidaygrade_code"].ToString(),
                            holidaygrade_name = dt["holidaygrade_name"].ToString(),
                        });
                        values.Holidayassign_type = getModuleList;
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
        public void HolidayEditUnAssignsubmit(HolidayEditUnassignsubmit values, string employee_gid)
        {
            try
            {

                foreach (var data in values.holidayeditunassign)
                {



                    msGetGid = objcmnfunctions.GetMasterGID("HO2G");

                    msSQL = " insert into hrm_mst_tholiday2grade  ( " +
                            " holiday2gradedtl_gid, " +
                            " holidaygrade_gid, " +
                            " holiday_gid, " +
                            " holiday_date, " +
                            " holiday_name ) " +
                            " values ( " +
                            "'" + msGetGid + "', " +
                            "'" + values.holidaygrade_gid + "', " +
                            " '" + data.holiday_gid + "'," +
                            "'" + data.holiday_date.ToString("yyy-MM-dd") + "', " +
                            "'" + data.holiday_name + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Holiday Assigned Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Assigning Holiday";
                    }

                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DeleteEditholiday(string holiday_gid, MdlHolidaygradeManagement values) 
        {
            try
            {

                    msSQL = "delete from hrm_mst_tholiday2grade where holiday_gid = '" + holiday_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
               
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Holiday Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Holiday";
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaHolidayGradeViewSummary(string holidaygrade_gid, MdlHolidaygradeManagement values)
        {
            try
            {
                msSQL = " select holidaygrade_gid, holiday_gid, date_format(holiday_date, '%d-%m-%Y') as holiday_date,holiday_name from hrm_mst_tholiday2grade " +
                        " where year(holiday_date) and holidaygrade_gid = '" + holidaygrade_gid + "' order by DATE(holiday_date) asc, holiday_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<holidayview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new holidayview_list 
                        {
                            holiday_gid = dt["holiday_gid"].ToString(),
                            holidaygrade_gid = dt["holidaygrade_gid"].ToString(),
                            holiday_date = dt["holiday_date"].ToString(),
                            holiday_name = dt["holiday_name"].ToString(),
                        });
                        values.holidayview_list = getModuleList;

                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}