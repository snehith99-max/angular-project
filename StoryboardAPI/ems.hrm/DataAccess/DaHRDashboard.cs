using CrystalDecisions.Shared.Json;
//using DocumentFormat.OpenXml.Math;
using ems.hrm.Models;
using ems.utilities.Functions;
using MySqlX.XDevAPI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
//using DocumentFormat.OpenXml.Math;

namespace ems.hrm.DataAccess
{
    public class DaHRDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcConnection objODBCconnection;
        OdbcDataReader objMySqlDataReader, objMySqlDataReader1, objMySqlDataReader2;
        DataTable dt_datatable, objTblemployee;
        string msSQL;
        string msGetGID, lsdate, msGetdtlGID, gid, lsemployee_gid;
        int mnResult, mnResult1;
        string lslop;
        public void DaGetTotalActiveEmployeeCount(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select count(case when employee_attendance = 'Present' then 'P' end) as present_count, " +
                        " count(case when employee_attendance = 'Absent' then 'A' end) as absent_count, " +
                        " count(case when employee_attendance = 'Leave' then 'L' end) as leave_count, " +
                        " count(employee_gid) as employee_count " +
                        " from hrm_trn_tattendance where attendance_date = curdate() ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TotalActiveEmployeeCount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TotalActiveEmployeeCount
                        {
                            present_count = dt["present_count"].ToString(),
                            absent_count = dt["absent_count"].ToString(),
                            leave_count = dt["leave_count"].ToString(),
                            employee_count = dt["employee_count"].ToString(),
                        });
                        values.TotalActiveEmployeeCount = getModuleList;
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
        public void DaGetEmployeePresentList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select concat(c.user_code, ' / ', c.user_firstname,' ',c.user_lastname) as employee, a.attendance_date, " +
                        " time_format(a.login_time, '%h:%i %p') as login_time, time_format(a.logout_time, '%h:%i %p') as logout_time, " +
                        " a.employee_attendance from hrm_trn_tattendance a left join hrm_mst_temployee b on b.employee_gid = a.employee_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where c.user_status = 'Y' and a.employee_attendance = 'Present' " +
                        " and a.attendance_date = curdate() order by c.user_code ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TotalActiveEmployeeList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TotalActiveEmployeeList
                        {
                            employee = dt["employee"].ToString(),
                            login_time = dt["login_time"].ToString(),
                            logout_time = dt["logout_time"].ToString(),
                            employee_attendance = dt["employee_attendance"].ToString(),
                        });
                        values.TotalActiveEmployeeList = getModuleList;
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
        public void DaGetEmployeeAbsentList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select concat(c.user_code, ' / ', c.user_firstname,' ',c.user_lastname) as employee, a.attendance_date, " +
                        " time_format(a.login_time, '%h:%i %p') as login_time, time_format(a.logout_time, '%h:%i %p') as logout_time, " +
                        " a.employee_attendance from hrm_trn_tattendance a left join hrm_mst_temployee b on b.employee_gid = a.employee_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where c.user_status = 'Y' and a.employee_attendance = 'Absent' " +
                        " and a.attendance_date = curdate() order by c.user_code ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TotalActiveEmployeeList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TotalActiveEmployeeList
                        {
                            employee = dt["employee"].ToString(),
                            login_time = dt["login_time"].ToString(),
                            logout_time = dt["logout_time"].ToString(),
                            employee_attendance = dt["employee_attendance"].ToString(),
                        });
                        values.TotalActiveEmployeeList = getModuleList;
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
        public void DaGetEmployeeLeaveList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select concat(c.user_code, ' / ', c.user_firstname,' ',c.user_lastname) as employee, a.attendance_date, " +
                        " time_format(a.login_time, '%h:%i %p') as login_time, time_format(a.logout_time, '%h:%i %p') as logout_time, " +
                        " a.employee_attendance from hrm_trn_tattendance a left join hrm_mst_temployee b on b.employee_gid = a.employee_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid  where c.user_status = 'Y' and a.employee_attendance = 'Leave' " +
                        " and a.attendance_date = curdate() order by c.user_code ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TotalActiveEmployeeList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TotalActiveEmployeeList
                        {
                            employee = dt["employee"].ToString(),
                            login_time = dt["login_time"].ToString(),
                            logout_time = dt["logout_time"].ToString(),
                            employee_attendance = dt["employee_attendance"].ToString(),
                        });
                        values.TotalActiveEmployeeList = getModuleList;
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
        public void DaGetTotalActiveEmployeeList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select concat(c.user_code, ' / ', c.user_firstname,' ',c.user_lastname) as employee, a.attendance_date, " +
                        " time_format(a.login_time, '%h:%i %p') as login_time, time_format(a.logout_time, '%h:%i %p') as logout_time, " +
                        " (case when a.employee_attendance = 'Present' then 'Present' when a.employee_attendance = 'Absent' then 'Absent' " +
                        " when a.employee_attendance = 'Leave' then 'Leave' end) as employee_attendance from hrm_trn_tattendance a " +
                        " left join hrm_mst_temployee b on b.employee_gid = a.employee_gid left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where c.user_status = 'Y' and a.attendance_date = curdate() order by a.employee_attendance, c.user_code; ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TotalActiveEmployeeList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TotalActiveEmployeeList
                        {
                            employee = dt["employee"].ToString(),
                            login_time = dt["login_time"].ToString(),
                            logout_time = dt["logout_time"].ToString(),
                            employee_attendance = dt["employee_attendance"].ToString(),
                        });
                        values.TotalActiveEmployeeList = getModuleList;
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
        public void DaGetTodayBirthdayCount(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select count(a.employee_gid) as today_birthdaycount from hrm_mst_temployee a " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where month(a.employee_dob) = month(now()) and day(a.employee_dob) = day(now()) and user_status = 'Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TodayBirthdayCount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TodayBirthdayCount
                        {
                            today_birthdaycount = dt["today_birthdaycount"].ToString(),
                        });
                        values.TodayBirthdayCount = getModuleList;
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
        public void DaGetTodayBirthdayList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code,' / ',b.user_firstname,' ',b.user_lastname) as employee, employee_photo " +
                        " from hrm_mst_temployee a left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where month(a.employee_dob) = month(now()) and day(a.employee_dob) = day(now()) and user_status = 'Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TodayBirthdayList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TodayBirthdayList
                        {
                            employee = dt["employee"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                        });
                        values.TodayBirthdayList = getModuleList;
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
        public void DaGetUpcomingBirthdayCount(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select count(a.employee_gid) as upcoming_birthdaycount from hrm_mst_temployee a " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where date_format(a.employee_dob, '%m%d') " +
                        " between date_format(date_add(now(), interval 1 day),'%m%d') " +
                        " and date_format(date_add(now(), interval 31 day),'%m%d') and user_status = 'Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<UpcomingBirthdayCount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new UpcomingBirthdayCount
                        {
                            upcoming_birthdaycount = dt["upcoming_birthdaycount"].ToString(),
                        });
                        values.UpcomingBirthdayCount = getModuleList;
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
        public void DaGetUpcomingBirthdayList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code,' / ',b.user_firstname,' ',b.user_lastname) as employee, employee_photo, " +
                        " concat(day(a.employee_dob), case when day(a.employee_dob) in (1, 21, 31) then 'st' when day(a.employee_dob) in (2, 22) then 'nd' " +
                        " when day(a.employee_dob) in (3, 23) then 'rd' else 'th' end, ' ', date_format(a.employee_dob, '%M')) as employee_dob, " +
                        " a.employee_emailid, day(a.employee_dob) as date, month(a.employee_dob) as month, year(a.employee_dob) as year " +
                        " from hrm_mst_temployee a left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where date_format(a.employee_dob, '%m%d') between date_format(date_add(now(), interval 1 day), '%m%d') " +
                        " and date_format(date_add(now(), interval 31 day), '%m%d') " +
                        " and user_status = 'Y' order by month, date, user_code asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<UpcomingBirthdayList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new UpcomingBirthdayList
                        {
                            employee = dt["employee"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                            employee_dob = dt["employee_dob"].ToString(),
                        });
                        values.UpcomingBirthdayList = getModuleList;
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
        public void DaGetWorkAnniversaryCount(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select count(year(curdate())-year(employee_joiningdate)) as workanniversarycount from hrm_mst_temployee a " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where month(employee_joiningdate)= month(curdate()) " +
                        " and day(employee_joiningdate)= day(curdate())  and b.user_status = 'Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<WorkAnniversaryCount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new WorkAnniversaryCount
                        {
                            workanniversarycount = dt["workanniversarycount"].ToString(),
                        });
                        values.WorkAnniversaryCount = getModuleList;
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
        public void DaGetWorkAnniversaryList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code,' / ',b.user_firstname,' ',b.user_lastname) as employee, employee_photo, " +
                        " c.designation_name, a.employee_gender, year(curdate())-year(a.employee_joiningdate) as total_experience " +
                        " from hrm_mst_temployee a left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join adm_mst_tdesignation c on c.designation_gid = a.designation_gid " +
                        " where month(a.employee_joiningdate)=month(curdate()) and day(a.employee_joiningdate)=day(curdate()) and " +
                        " b.user_status = 'Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<WorkAnniversaryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new WorkAnniversaryList
                        {
                            employee = dt["employee"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            total_experience = dt["total_experience"].ToString(),
                        });
                        values.WorkAnniversaryList = getModuleList;
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
        public void DaGetOnProbationCount(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select count(a.employee_gid) as total_probationemployee from hrm_mst_temployee a " +
                        " left join adm_mst_tuser b on b.user_gid = a.user_gid " +
                        " where a.probationary_until<> '' and a.probationary_until<> '0000-00-00' and b.user_status = 'Y' " +
                        " order by b.user_code asc; ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<OnProbationCount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new OnProbationCount
                        {
                            total_probationemployee = dt["total_probationemployee"].ToString(),
                        });
                        values.OnProbationCount = getModuleList;
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
        public void DaGetOnProbationList(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code,' / ',b.user_firstname,' ', b.user_lastname) as employee, " +
                        " e.department_name, d.designation_name, date_format(a.employee_joiningdate,'%d-%b-%Y') as employee_joiningdate, date_format(a.probationary_until,'%d-%b-%Y') as probationary_until from hrm_mst_temployee a " +
                        " left join adm_mst_tuser b on b.user_gid = a.user_gid left join hrm_mst_trole c on c.role_gid = a.role_gid " +
                        " left join adm_mst_tdesignation d on d.designation_gid = a.designation_gid left join hrm_mst_tdepartment e on e.department_gid = a.department_gid " +
                        " where a.probationary_until <> '' and a.probationary_until <> '0000-00-00' and b.user_status = 'Y' " +
                        " order by b.user_code asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<OnProbationList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new OnProbationList
                        {
                            employee = dt["employee"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            probationary_until = dt["probationary_until"].ToString(),
                        });
                        values.OnProbationList = getModuleList;
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
        public void DaGetEmpCountbyLocation(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select a.branch_name, count(b.employee_gid) as employee_count from hrm_mst_tbranch a " +
                        " left join hrm_mst_temployee b on b.branch_gid = a.branch_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " left join hrm_trn_tattendance d on d.employee_gid = b.employee_gid " +
                        " where c.user_status = 'Y' and d.attendance_date = curdate() " +
                        " group by a.branch_gid order by a.branch_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<EmpCountbyLocation>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new EmpCountbyLocation
                        {
                            branch_name = dt["branch_name"].ToString(),
                            employee_count = dt["employee_count"].ToString(),
                        });
                        values.EmpCountbyLocation = getModuleList;
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
        public void DaGetTotalActiveEmployees(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select a.user_gid, concat(a.user_code,' / ', a.user_firstname,' ',a.user_lastname) as employee, c.designation_name " +
                        " from adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid = a.user_gid " +
                        " left join adm_mst_tdesignation c on c.designation_gid = b.designation_gid " +
                        " where user_status = 'Y'  order by a.user_code ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<TotalActiveEmployees>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TotalActiveEmployees
                        {
                            employee = dt["employee"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                        });
                        values.TotalActiveEmployees = getModuleList;
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
        public void DaGetToDoListCount(MdlHRDashboard values, string leave_gid)
        {
            try
            {
                msSQL = " SELECT COUNT(a.leave_gid) AS pending_leaves,c.user_firstname,d.department_name,DATE_FORMAT(a.leave_applydate, '%d-%m-%Y') AS  leave_applydate," +
                         " DATE_FORMAT(a.leave_fromdate, '%d-%m-%Y') AS leave_fromdate,DATE_FORMAT(a.leave_todate, '%d-%m-%Y') AS leave_todate, a.leave_reason,a.leave_remarks, h.leavetype_name from hrm_trn_tleave a " +
                         " LEFT JOIN hrm_mst_temployee b ON b.employee_gid = a.employee_gid" +
                         " LEFT JOIN hrm_mst_tdepartment d ON d.department_gid = b.department_gid " +
                         " LEFT JOIN hrm_trn_tleave e ON e.leavetype_gid = b.department_gid " +
                         " LEFT JOIN adm_mst_tuser c ON c.user_gid = b.user_gid " +
                         " left join hrm_mst_tleavetype h on a.leavetype_gid = h.leavetype_gid" +
                         "left join adm_mst_tmodule2employee g on g.employee_gid = a.employee_gid"+
                         "WHERE a.leave_status = 'Pending' AND c.user_status = 'Y' and a.leave_gid='" + leave_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<ToDoListCount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ToDoListCount
                        {
                            pending_leaves = dt["pending_leaves"].ToString(),
                            leave_applydate = dt["leave_applydate"].ToString(),
                            leave_fromdate = dt["leave_fromdate"].ToString(),
                            leave_todate = dt["leave_todate"].ToString(),
                            leave_reason = dt["leave_reason"].ToString(),
                            leave_remarks = dt["leave_remarks"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),

                        });
                        values.ToDoListCount = getModuleList;
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
        public void DaGetToDoList(string employee_gid,MdlHRDashboard values)
        {
            try
            {
                if (employee_gid =="E1" ||employee_gid== "SERM1709250001" || employee_gid == "SERM1709250002")
                {
                    msSQL = " select a.leave_gid,concat(c.user_firstname, ' ', c.user_lastname, ' / ', c.user_code, " +
                        " ' Applied ', d.leavetype_name, ' on ', date_format(a.leave_applydate,'%d-%b-%Y'), " +
                        " '  for ',a.leave_noofdays,' days ', ' - ', " +
                        " ' From ', date_format(a.leave_fromdate, '%d-%b-%Y'), ' (', dayname(a.leave_fromdate), ') - ', " +
                        " ' To ', date_format(a.leave_todate, '%d-%b-%Y'), ' (', dayname(a.leave_todate), ') - ', a.leave_reason) as leave_details " +
                        " from hrm_trn_tleave a " +
                        " left join hrm_mst_temployee b on b.employee_gid = a.employee_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " left join hrm_mst_tleavetype d on d.leavetype_gid = a.leavetype_gid " +
                        " where a.leave_status = 'Pending' and c.user_status = 'Y'  order by a.leave_gid ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<ToDoList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new ToDoList
                            {
                                leave_details = dt["leave_details"].ToString(),
                                leave_gid = dt["leave_gid"].ToString(),
                            });
                            values.ToDoList = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }              

                
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetLeaveHistoryDetails(MdlHRDashboard values)
        {
            try
            {
                msSQL = " SELECT DISTINCT a.leave_status, a.leave_gid, " +
                        " CAST(CONCAT( " +
                        " f.user_code, ' / ', " +
                        " f.user_firstname, ' ', " +
                        " f.user_lastname, ' Applied ', " +
                        " d.leavetype_name, ' From ', " +
                        " DATE_FORMAT(leave_fromdate, '%d-%m-%Y'), " +
                        " ' To ', " +
                        " DATE_FORMAT(leave_todate, '%d-%m-%Y'), " +
                        " ' (', " +
                        " leave_noofdays, " +
                        " CASE " +
                        " WHEN leave_noofdays = '1' THEN ' Day)' " +
                        " WHEN leave_noofdays = '0.5' THEN ' Day)' " +
                        " ELSE ' Days)' " +
                        " END, ' On ', " +
                        " DATE_FORMAT(a.leave_applydate, '%d-%m-%Y'), " +
                        " ' for ', " +
                        " a.leave_reason, " +
                        " CASE " +
                        " WHEN a.leave_status IS NULL THEN '' " +
                        " WHEN a.leave_status = 'Approved' THEN ' Approved by ' " +
                        " WHEN a.leave_status = 'Rejected' THEN ' Rejected by ' " +
                        " ELSE a.leave_status " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN a.leave_approvedby IS NULL THEN '' " +
                        " ELSE l.user_firstname " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN a.leave_status IS NULL THEN '' " +
                        " WHEN a.leave_status = 'Approved' THEN ' On ' " +
                        " WHEN a.leave_status = 'Rejected' THEN ' On ' " +
                        " ELSE a.leave_status " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN leave_approveddate IS NULL THEN '' " +
                        " ELSE DATE_FORMAT(a.leave_approveddate, '%d-%m-%Y') " +
                        " END " +
                        " ) AS CHAR) AS employee_details, " +
                        " a.leave_gid, " +
                        " a.employee_gid, " +
                        " a.leave_noofdays, " +
                        " a.leave_applydate " +
                        " FROM hrm_trn_tleave a " +
                        " LEFT JOIN hrm_trn_tleavedtl b ON a.leave_gid = b.leave_gid " +
                        " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
                        " LEFT JOIN hrm_mst_tleavetype d ON a.leavetype_gid = d.leavetype_gid " +
                        " LEFT JOIN adm_mst_tdesignation e ON c.designation_gid = e.designation_gid " +
                        " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
                        " LEFT JOIN hrm_mst_tbranch g ON c.branch_gid = g.branch_gid " +
                        " LEFT JOIN hrm_mst_temployee k ON k.employee_gid = a.leave_approvedby " +
                        " LEFT JOIN adm_mst_tuser l ON l.user_gid = k.user_gid " +
                        " LEFT JOIN hrm_trn_temployeetypedtl j ON c.employee_gid = j.employee_gid " +
                        " WHERE a.leave_status != 'Pending' " +
                        " ORDER BY DATE(a.leave_applydate) DESC, a.leave_applydate ASC, a.leave_gid DESC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Leave_HistoryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Leave_HistoryList
                        {
                            employee_details = dt["employee_details"].ToString(),
                            leave_status = dt["leave_status"].ToString(),
                            leave_gid = dt["leave_gid"].ToString(),
                        });
                        values.Leave_HistoryList = getModuleList;
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

        public void DaGetLoginHistoryDetails(MdlHRDashboard values)
        {
            try
            {
                msSQL = " SELECT distinct k.attendancelogintmp_gid,k.employee_gid,k.status,K.attendance_date, " +
                        " cast(concat(n.user_code, ' / ', n.user_firstname, ' ', n.user_lastname, ' ', 'applied for a', ' Login Time Requisition on ', date_format(k.attendance_date, '%d-%m-%Y'), ' at ', " +
                        " time_format(k.login_time, '%H:%i %p'), ' for ', k.remarks, ' On ', " +
                        " ' for the date ', ' ', date_format(k.created_date, '%d-%m-%Y'), ' ', " +
                        " CASE " +
                        " WHEN k.status IS NULL THEN '' " +
                        " WHEN k.status = 'Approved' THEN ' Approved by ' " +
                        " WHEN k.status = 'Rejected' THEN ' Rejected by ' " +
                        " WHEN k.status = 'Pending' THEN ' Approval Pending' " +
                        "  ELSE k.status " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN k.approved_by IS NULL THEN '' " +
                        " ELSE za.user_firstname " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN k.status IS NULL THEN '' " +
                        " WHEN k.status = 'Approved' THEN ' On ' " +
                        " WHEN k.status = 'Rejected' THEN ' On ' " +
                        "  WHEN k.status = 'Pending' THEN '.' " +
                        " ELSE k.status " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN k.approved_date IS NULL THEN '' " +
                        " ELSE DATE_FORMAT(k.approved_date, '%d-%m-%Y') " +
                        " END " +
                        " ) as char " +
                        " ) as employee_name, e.designation_name FROM hrm_tmp_tattendancelogin k " +
                        " left join hrm_mst_temployee c on k.employee_gid = c.employee_gid " +
                        " left join hrm_mst_tdepartment m on m.department_gid = c.department_gid " +
                        " left join adm_mst_tdesignation e on c.designation_gid = e.designation_gid " +
                        "  left join adm_mst_tuser n on n.user_gid = c.user_gid " +
                        " left join hrm_mst_temployee z ON z.employee_gid = k.approved_by " +
                        " left join adm_mst_tuser za ON za.user_gid = z.user_gid " +
                        " WHERE k.status != 'Pending' " +
                        " order by date(k.created_date) desc, k.attendancelogintmp_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Login_HistoryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Login_HistoryList
                        {
                            employee_name = dt["employee_name"].ToString(),
                            StatusLogin = dt["status"].ToString(),
                            attendancelogintmp_gid = dt["attendancelogintmp_gid"].ToString(),
                        });
                        values.Login_HistoryList = getModuleList;
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

        public void DaGetLogoutHistoryDetails(MdlHRDashboard values)
        {
            try
            {
                msSQL = " SELECT DISTINCT " +
                        " a.attendancetmp_gid, a.employee_gid, a.status, " +
                        " CAST( " +
                        " CONCAT( " +
                        " f.user_code, ' / ', f.user_firstname, ' ', f.user_lastname, ' ', " +
                        " ' Applied Logout Time Requisition For ', DATE_FORMAT(a.attendance_date, '%d-%m-%Y'), ' ', " +
                        " TIME_FORMAT(a.logout_time, '%H:%i %p'), ' for ', a.remarks, ' On ', " +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y'), ' ', " +
                        " CASE " +
                        " WHEN a.status IS NULL THEN '' " +
                        " WHEN a.status = 'Approved' THEN ' Approved by ' " +
                        " WHEN a.status = 'Rejected' THEN ' Rejected by ' " +
                        " WHEN a.status = 'Pending' THEN ' Approval Pending' " +
                        " ELSE a.status " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN a.approvedby IS NULL THEN '' " +
                        " ELSE za.user_firstname " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN a.status IS NULL THEN '' " +
                        " WHEN a.status = 'Approved' THEN ' On ' " +
                        " WHEN a.status = 'Rejected' THEN ' On ' " +
                        " WHEN a.status = 'Pending' THEN '.' " +
                        " ELSE a.status " +
                        " END, ' ', " +
                        " CASE " +
                        " WHEN a.approveddate IS NULL THEN '' " +
                        " ELSE DATE_FORMAT(a.approveddate, '%d-%m-%Y') " +
                        " END " +
                        " ) AS CHAR " +
                        " ) AS employee_logoutdetails, " +
                        " e.designation_name FROM hrm_tmp_tattendance a " +
                        " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
                        " LEFT JOIN adm_mst_tdesignation e ON c.designation_gid = e.designation_gid " +
                        " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
                        " LEFT JOIN hrm_mst_temployee z ON z.employee_gid = a.approvedby " +
                        " left join adm_mst_tuser za ON za.user_gid = z.user_gid " +
                        " WHERE a.status != 'Pending' " +
                        " order by date(a.created_date) desc, a.attendancetmp_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Logout_HistoryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Logout_HistoryList
                        {
                            employee_logoutdetails = dt["employee_logoutdetails"].ToString(),
                            StatusLogout = dt["status"].ToString(),
                            attendancetmp_gid = dt["attendancetmp_gid"].ToString(),
                        });
                        values.Logout_HistoryList = getModuleList;
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

        public void DaGetOdHistoryDetails(MdlHRDashboard values) 
        {
            try
            {
                msSQL = " SELECT b.employee_mobileno,a.ondutytracker_gid, a.ondutytracker_status, a.onduty_duration,a.onduty_fromtime, a.onduty_totime, DATE_FORMAT(a.ondutytracker_date, '%d-%m-%Y') AS ondutytracker_date," +
                        " CONCAT( " +
                        " c.user_code, ' / ', c.user_firstname, ' ', c.user_lastname, " +
                        " ' Applied for a On Duty on ', DATE_FORMAT(a.ondutytracker_date, '%d-%m-%Y'), " +
                        " ' From ', a.onduty_fromtime, ':', " +
                        " CASE " +
                        " WHEN a.from_minutes IS NULL THEN '00' " +
                        " ELSE a.from_minutes " +
                        " END, " +
                        " ':00 To ', a.onduty_totime, ':',  " +
                        " CASE " +
                        " WHEN a.to_minutes IS NULL THEN '00' " +
                        " ELSE a.to_minutes " +
                        " END, " +
                        " ':00 (', a.onduty_duration, ') On ', " +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y'), " +
                        " '  ', a.onduty_reason, " +
                        " CASE " +
                        " WHEN a.ondutytracker_status IS NULL THEN '' " +
                        " WHEN a.ondutytracker_status = 'Approved' THEN ' Approved by ' " +
                        " WHEN a.ondutytracker_status = 'Rejected' THEN ' Rejected by ' " +
                        " WHEN a.ondutytracker_status = 'Pending' THEN ' Approval Pending' " +
                        " ELSE a.ondutytracker_status " +
                        " END, " +
                        " CASE " +
                        " WHEN a.onduty_approveby IS NULL THEN '' " +
                        " ELSE c.user_firstname " +
                        " END, " +
                        " CASE " +
                        " WHEN a.ondutytracker_status IS NULL THEN '' " +
                        " WHEN a.ondutytracker_status = 'Approved' THEN ' On ' " +
                        " WHEN a.ondutytracker_status = 'Rejected' THEN ' On ' " +
                        " WHEN a.ondutytracker_status = 'Pending' THEN '.' " +
                        " ELSE a.ondutytracker_status " +
                        " END, " +
                        " CASE " +
                        " WHEN a.onduty_approvedate IS NULL THEN '' " +
                        " ELSE DATE_FORMAT(a.onduty_approvedate, '%d-%m-%Y') " +
                        " END " +
                        ") AS username, " +
                        " a.onduty_reason, a.ondutytracker_date FROM hrm_trn_tondutytracker a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.employee_gid = b.employee_gid " +
                        " LEFT JOIN adm_mst_tuser c ON c.user_gid = b.user_gid " +
                        " LEFT JOIN hrm_mst_tdepartment d ON b.department_gid = d.department_gid " +
                        " WHERE a.ondutytracker_status != 'Pending' " +
                        " order by date(a.created_date) desc, a.ondutytracker_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<od_HistoryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new od_HistoryList
                        {
                            username = dt["username"].ToString(),
                            StatusOnduty = dt["ondutytracker_status"].ToString(),
                            ondutytracker_gid = dt["ondutytracker_gid"].ToString(),
                        });
                        values.od_HistoryList = getModuleList;
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

        public void DaGetCompoffHistoryDetails(MdlHRDashboard values) 
        {
            try
            {
                msSQL = " SELECT DISTINCT a.compensatoryoff_gid, a.compensatoryoff_status, " +
                        " CAST(" +
                        " CONCAT(" +
                        " f.user_code, ' / ', f.user_firstname, ' ', f.user_lastname, " +
                        " ' ',' Applied for a Comp-Off on ', DATE_FORMAT(a.compensatoryoff_applydate, '%d-%m-%Y'), " +
                        " ' and my actual worked date is ', DATE_FORMAT(a.actualworking_fromdate, '%d-%m-%Y'), " +
                        " ' ', a.compensatoryoff_reason, " +
                        " CASE " +
                        " WHEN a.compensatoryoff_status IS NULL THEN '' " +
                        " WHEN a.compensatoryoff_status = 'Approved' THEN ' Approved by ' " +
                        " WHEN a.compensatoryoff_status = 'Rejected' THEN ' Rejected by ' " +
                        " WHEN a.compensatoryoff_status = 'Pending' THEN ' Approval Pending ' " +
                        " ELSE a.compensatoryoff_status" +
                        " END," +
                        " CASE " +
                        " WHEN a.compensatoryoff_approveby IS NULL THEN '' " +
                        " ELSE p.user_firstname " +
                        " END," +
                        " CASE " +
                        " WHEN a.compensatoryoff_status IS NULL THEN '' " +
                        " WHEN a.compensatoryoff_status = 'Approved' THEN ' On ' " +
                        " WHEN a.compensatoryoff_status = 'Rejected' THEN ' On ' " +
                        " WHEN a.compensatoryoff_status = 'Pending' THEN '.' " +
                        " ELSE a.compensatoryoff_status " +
                        " END," +
                        " CASE " +
                        " WHEN a.compensatoryoff_approvedate IS NULL THEN '' " +
                        " ELSE DATE_FORMAT(a.compensatoryoff_approvedate, '%d-%m-%Y') " +
                        " END " +
                        ") AS CHAR " +
                        ") AS employee_details, " +
                        " a.employee_gid, a.compensatoryoff_gid " +
                        " FROM hrm_trn_tcompensatoryoff a " +
                        " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
                        " LEFT JOIN hrm_mst_tdepartment g ON c.department_gid = g.department_gid " +
                        " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
                        " LEFT JOIN hrm_mst_temployee o ON o.employee_gid = a.compensatoryoff_approveby " +
                        " LEFT JOIN adm_mst_tuser p ON p.user_gid = o.user_gid " +
                        " WHERE a.compensatoryoff_status != 'Pending' " +
                        " order by date(a.created_date) desc, a.compensatoryoff_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Compoff_HistoryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Compoff_HistoryList
                        {
                            employee_details = dt["employee_details"].ToString(),
                            StatusCompoff = dt["compensatoryoff_status"].ToString(),
                            compensatoryoff_gid = dt["compensatoryoff_gid"].ToString(),
                        });
                        values.Compoff_HistoryList = getModuleList;
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

        public void DaGetPermissionHistoryDetails(MdlHRDashboard values)
        {
            msSQL = "select distinct a.permissiondtl_gid,a.permission_gid,a.permission_status, cast(concat(d.user_code,' / ',d.user_firstname,' ',d.user_lastname, " +
                         " ' ',' Applied for a Permission on ',date_format(a.permission_date,'%d-%m-%Y'),' From ',a.permission_fromhours,':',case when a.permission_frommins is null then '00' else permission_frommins end,':','00', " +
                          " ' To ',a.permission_tohours,':',case when a.permission_tomins is null then '00' else permission_tomins end,':','00 (', " +
                          " concat(a.permission_totalhours,':',a.total_mins,':','00'), " +
                          " ')on',' ',date_format(a.permission_applydate,'%d-%m-%Y'),' ',a.permission_reason, " +
                          " CASE " +
                          " WHEN a.permission_status IS NULL THEN '' " +
                          " WHEN a.permission_status = 'Approved' THEN ' Approved by ' " +
                          " WHEN a.permission_status = 'Rejected' THEN ' Rejected by ' " +
                          " WHEN a.permission_status = 'Pending' THEN ' Approval Pending ' " +
                          " ELSE a.permission_status " +
                          " END, " +
                          " CASE " +
                          " WHEN a.permission_approvedby IS NULL THEN '' " +
                          " ELSE p.user_firstname " +
                          " END, " +
                          " CASE " +
                          " WHEN a.permission_status IS NULL THEN '' " +
                          " WHEN a.permission_status = 'Approved' THEN ' On ' " +
                          " WHEN a.permission_status = 'Rejected' THEN ' On ' " +
                          " WHEN a.permission_status = 'Pending' THEN '.' " +
                          " ELSE a.permission_status " +
                          " END, " +
                          " CASE " +
                          " WHEN a.permission_approveddate IS NULL THEN '' " +
                          " ELSE DATE_FORMAT(a.permission_approveddate, '%d-%m-%Y') " +
                          " END " +
                          " )as char) as employee_name " +
                          " from hrm_trn_tpermissiondtl a left join hrm_mst_temployee i on a.employee_gid=i.employee_gid " +
                          " left join hrm_mst_tdepartment e on e.department_gid=i.department_gid " +
                          " left join adm_mst_Tuser d on d.user_gid=i.user_gid " +
                          " left join hrm_trn_temployeetypedtl j on i.employee_gid=j.employee_gid " +
                          " LEFT JOIN hrm_mst_temployee o ON o.employee_gid = a.permission_approvedby " +
                          " LEFT JOIN adm_mst_tuser p ON p.user_gid = o.user_gid " +
                          " WHERE a.permission_status != 'Pending' " +
                          " order by date(a.created_date) desc, a.permissiondtl_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<permission_HistoryList>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new permission_HistoryList 
                    {
                        employee_name = dt["employee_name"].ToString(),
                        StatusPermission = dt["permission_status"].ToString(),
                        permissiondtl_gid = dt["permissiondtl_gid"].ToString(),
                    });
                    values.permission_HistoryList = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetToDoLoginList(string employee_gid,MdlHRDashboard values)
        {
            try
            {
                if (employee_gid == "E1" || employee_gid == "SERM1709250001" || employee_gid == "SERM1709250002")
                {
                    msSQL = " SELECT distinct k.attendancelogintmp_gid,k.employee_gid,k.status,K.attendance_date, " +
                        " cast(concat(n.user_code, ' / ', n.user_firstname, ' ', n.user_lastname, ' ', 'applied for a', ' Login Time Requisition on ', date_format(k.attendance_date, '%d-%m-%Y'), ' at ', " +
                        " time_format(k.login_time, '%H:%i %p'), " +
                        " ' for the date ', ' ', date_format(k.created_date, '%d-%m-%Y'),' ', k.remarks,'.') as char) as employee_name, e.designation_name " +
                        " FROM hrm_tmp_tattendancelogin k " +
                        " left join hrm_mst_temployee c on k.employee_gid = c.employee_gid " +
                        " left join hrm_mst_tdepartment m on m.department_gid = c.department_gid " +
                        " left join adm_mst_tdesignation e on c.designation_gid = e.designation_gid " +
                        " left join adm_mst_tuser n on n.user_gid = c.user_gid " +
                        " where k.status = 'Pending' and n.user_status = 'Y' order by k.attendancelogintmp_gid ";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<ToDoLoginList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new ToDoLoginList
                            {
                                employee_name = dt["employee_name"].ToString(),
                                attendancelogintmp_gid = dt["attendancelogintmp_gid"].ToString(),
                            });
                            values.ToDoLoginList = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetToDoLogoutList(string employee_gid, MdlHRDashboard values)
        {
            try
            {
                if (employee_gid == "E1" || employee_gid == "SERM1709250001" || employee_gid == "SERM1709250002")
                {
                    msSQL = " SELECT DISTINCT " +
                " a.attendancetmp_gid, a.employee_gid, a.status, " +
                " CAST( " +
                " CONCAT( " +
                " f.user_code, ' / ', " +
                " f.user_firstname, ' ', " +
                " f.user_lastname, ' ', " +
                " 'Applied for a Logout Time Requisition on ', " +
                " DATE_FORMAT(a.attendance_date, '%d-%m-%Y'), " +
                " ' at ', " +
                " TIME_FORMAT(a.logout_time, '%H:%i %p'), " +
                " ' for the date ', " +
                " DATE_FORMAT(a.created_date, '%d-%m-%Y'), " +
                " ' ', " +
                " a.remarks,'.' " +
                " ) AS CHAR " +
                " ) AS employee_logoutdetails, " +
                " e.designation_name FROM hrm_tmp_tattendance a " +
                " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
                " LEFT JOIN adm_mst_tdesignation e ON c.designation_gid = e.designation_gid " +
                " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
                " LEFT JOIN hrm_mst_temployee z ON z.employee_gid = a.approvedby " +
                " where a.status='Pending'and f.user_status = 'Y' order by a.attendancetmp_gid ";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<ToDoLogoutList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new ToDoLogoutList
                            {
                                employee_logoutdetails = dt["employee_logoutdetails"].ToString(),
                                attendancetmp_gid = dt["attendancetmp_gid"].ToString(),
                            });
                            values.ToDoLogoutList = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetToDoODList(string employee_gid, MdlHRDashboard values)
        {
            try
            {
                if (employee_gid == "E1" || employee_gid == "SERM1709250001" || employee_gid == "SERM1709250002")
                {
                    msSQL = " SELECT b.employee_mobileno,a.ondutytracker_gid, a.onduty_duration,a.onduty_fromtime, a.onduty_totime, DATE_FORMAT(a.ondutytracker_date, '%d-%m-%Y') AS ondutytracker_date," +
                        " CONCAT( " +
                        " c.user_code, ' / ', c.user_firstname, ' ', c.user_lastname, " +
                        " ' Applied for a On Duty on ', DATE_FORMAT(a.ondutytracker_date, '%d-%m-%Y'), " +
                        " ' From ', a.onduty_fromtime, ':', " +
                        " CASE " +
                        " WHEN a.from_minutes IS NULL THEN '00' " +
                        " ELSE a.from_minutes " +
                        " END, " +
                        " ':00 To ', a.onduty_totime, ':',  " +
                        " CASE " +
                        " WHEN a.to_minutes IS NULL THEN '00' " +
                        " ELSE a.to_minutes " +
                        " END, " +
                        " ':00 (', a.onduty_duration, ') On ', " +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y'), " +
                        " '  ', a.onduty_reason,'.' " +
                        ") AS username, " +
                        " a.onduty_reason, a.ondutytracker_date FROM hrm_trn_tondutytracker a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.employee_gid = b.employee_gid " +
                        " LEFT JOIN adm_mst_tuser c ON c.user_gid = b.user_gid " +
                        " LEFT JOIN hrm_mst_tdepartment d ON b.department_gid = d.department_gid " +
                        " where ondutytracker_status='pending' and c.user_status = 'Y' order by a.ondutytracker_gid";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<ToDoODList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new ToDoODList
                            {
                                username = dt["username"].ToString(),
                                ondutytracker_gid = dt["ondutytracker_gid"].ToString(),
                            });
                            values.ToDoODList = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetToDoPermissionList(string employee_gid, MdlHRDashboard values)
        {
            try
            {
                if (employee_gid == "E1" || employee_gid == "SERM1709250001" || employee_gid == "SERM1709250002")
                {
                    msSQL = " select distinct a.permissiondtl_gid,a.permission_gid,a.permission_status, cast(concat(d.user_code,' / ',d.user_firstname,' ',d.user_lastname," +
                        " ' ',' Applied for a Permission on ',date_format(a.permission_date,'%d-%m-%Y'),' From ',a.permission_fromhours,':',case when a.permission_frommins is null then '00' else permission_frommins end,':','00'," +
                        " ' To ',a.permission_tohours,':',case when a.permission_tomins is null then '00' else permission_tomins end,':','00 ('," +
                        " concat(a.permission_totalhours,':',a.total_mins,':','00')," +
                        " ')on',' ',date_format(a.permission_applydate,'%d-%m-%Y'),' ',a.permission_reason,'.')as char) as employee_name " +
                        " from hrm_trn_tpermissiondtl a left join hrm_mst_temployee i on a.employee_gid=i.employee_gid" +
                        " left join hrm_mst_tdepartment e on e.department_gid=i.department_gid " +
                        " left join adm_mst_Tuser d on d.user_gid=i.user_gid" +
                        " left join hrm_trn_temployeetypedtl j on i.employee_gid=j.employee_gid " +
                        " where a.permission_status='pending' and d.user_status='Y' order by a.permissiondtl_gid desc ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<ToDoPermissionList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new ToDoPermissionList
                            {
                                employee_name = dt["employee_name"].ToString(),
                                permissiondtl_gid = dt["permissiondtl_gid"].ToString(),
                            });
                            values.ToDoPermissionList = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetToDoCompoffList(string employee_gid, MdlHRDashboard values)
        {
            try
            {
                if (employee_gid == "E1" || employee_gid == "SERM1709250001" || employee_gid == "SERM1709250002")
                {
                    msSQL = " SELECT DISTINCT a.compensatoryoff_gid, a.compensatoryoff_status, " +
                        " CAST(" +
                        " CONCAT(" +
                        " f.user_code, ' / ', f.user_firstname, ' ', f.user_lastname, " +
                        " ' ',' Applied for a Comp-Off on ', DATE_FORMAT(a.compensatoryoff_applydate, '%d-%m-%Y'), " +
                        " ' and my actual worked date is ', DATE_FORMAT(a.actualworking_fromdate, '%d-%m-%Y'), " +
                        " ' ', a.compensatoryoff_reason,'.' " +
                        ") AS CHAR " +
                        ") AS employee_details, " +
                        " a.employee_gid, a.compensatoryoff_gid " +
                        " FROM hrm_trn_tcompensatoryoff a " +
                        " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
                        " LEFT JOIN hrm_mst_tdepartment g ON c.department_gid = g.department_gid " +
                        " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
                        " LEFT JOIN hrm_mst_temployee o ON o.employee_gid = a.compensatoryoff_approveby " +
                        " LEFT JOIN adm_mst_tuser p ON p.user_gid = o.user_gid " +
                        " where a.compensatoryoff_status='pending' and f.user_status='Y' order by a.compensatoryoff_gid desc ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<ToDoCompOffList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new ToDoCompOffList
                            {
                                employee_details = dt["employee_details"].ToString(),
                                compensatoryoff_gid = dt["compensatoryoff_gid"].ToString(),
                            });
                            values.ToDoCompOffList = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaempStatistics(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select date_format(month, '%b - %y') as months, count(case when event_type = 'joining' then 1 end) as emp_joining_count, count(case when event_type = 'exit' then 1 end) as emp_exit_count from " +
                        " ( select a.employee_joiningdate as month, 'joining' as event_type from hrm_mst_temployee a left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where a.employee_joiningdate is not null and a.employee_joiningdate <> '' and b.user_status = 'Y' and a.employee_joiningdate between date_sub(now(), interval 6 month) and NOW() " +
                        " union all " +
                        " select a.exit_date as month, 'exit' as event_type from hrm_mst_temployee a left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " where a.exit_date is not null and a.exit_date <> '' and b.user_status = 'N' and a.exit_date between date_sub(now(), interval 6 month) and NOW() ) as combined_events " +
                        " group by year(month), month(month) order by year(month) asc, month(month) asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<empStatistics_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new empStatistics_list
                        {
                            months = dt["months"].ToString(),
                            emp_joining_count = dt["emp_joining_count"].ToString(),
                            emp_exit_count = dt["emp_exit_count"].ToString(),
                        });
                        values.empStatistics_list = getModuleList;
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
        public void DaempActivecount(MdlHRDashboard values)
        {
            try
            {
                msSQL = " select date_format(c.attendance_date, '%b - %y') as months, count(distinct b.employee_gid) as employee_count " +
                        " from adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid = a.user_gid " +
                        " left join hrm_trn_tattendance c on c.employee_gid = b.employee_gid " +
                        " where a.user_status = 'Y' and c.attendance_date between date_sub(curdate(), interval 5 month) and now() " +
                        " group by date_format(c.attendance_date, '%b - %y') order by min(c.attendance_date) ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<empActivecount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new empActivecount_list
                        {
                            months = dt["months"].ToString(),
                            employee_count = dt["employee_count"].ToString(),
                        });
                        values.empActivecount_list = getModuleList;
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
        public void Daapproveleavesubmit(string employee_gid, approveleave values)
        {
            //try
            //{
            //    msGetGID = objcmnfunctions.GetMasterGID("HLVP");

            //    msSQL = " insert into hrm_trn_tleave " +
            //            " ( leave_gid,  " +
            //            " employee_gid , " +
            //            " leavetype_gid , " +
            //            " leave_applydate , " +
            //            " leave_fromdate, " +
            //            " leave_todate , " +
            //            " leave_noofdays , " +
            //            " leave_reason, " +
            //            " leave_status ," +
            //            " created_by, " +
            //            " created_date) " +
            //            " values ( " +
            //            " '" + msGetGID2 + "', " +
            //            " '" + employee_gid + "', " +
            //            " '" + values.leavetype_gid + "', " +
            //            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
            //            " '" + values.leave_from.ToString("yyyy-MM-dd") + "'," +
            //            " '" + values.leave_to.ToString("yyyy-MM-dd") + "', " +
            //            " '" + values.leave_days + "'," +
            //            " '" + values.leave_reason.Replace("'", "") + "'," +
            //            " 'Pending'," +
            //            " '" + user_gid + "'," +
            //            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            //    if (mnResult == 1)
            //    {
            //        values.status = true;
            //        values.message = "Leave Applied Successfully";
            //        values.leavetypegidfrodocupload = msGetGID2;


            //        //    string lsapprovedby;
            //        //    string message;
            //        //    string employee_mailid = null;
            //        //    string employeename = null;
            //        //    string applied_by = null;
            //        //    string supportmail = null;
            //        //    string pwd = null;
            //        //    string reason = null;
            //        //    string days = null;
            //        //    string fromhours = null;
            //        //    string tohours = null;
            //        //    string emailpassword = null;
            //        //    string trace_comment;
            //        //    string permission_date = null;
            //        //    string todate = null;                    
            //        //    string fromdate = null;
            //        //    string lsleavetypename = null;
            //        //    int MailFlag;

            //        //    msSQL = "select pop_username,pop_password from adm_mst_tcompany";
            //        //        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            //        //        if (objMySqlDataReader.HasRows == true)
            //        //        {
            //        //            supportmail = objMySqlDataReader["pop_username"].ToString();
            //        //            emailpassword = objMySqlDataReader["pop_password"].ToString();
            //        //        }

            //        //        if (supportmail != "")
            //        //        {
            //        //            msSQL =    " select b.employee_emailid, (date_format(a.leave_fromdate,'%d/%m/%y')) as fromdate, " +
            //        //                       " (date_format(a.leave_todate,'%d/%m/%y')) as todate, " +
            //        //                       " concat(c.user_firstname,' ',c.user_lastname) as username, a.leave_noofdays, a.leave_reason, d.leavetype_name " +
            //        //                       " from hrm_trn_tleave a " +
            //        //                       " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
            //        //                       " left join adm_mst_tuser c on c.user_gid=b.user_gid " +
            //        //                       " left join hrm_mst_tleavetype d on d.leavetype_gid = a.leavetype_gid " +
            //        //                       " where a.leave_gid='" + msGetGID2 + "' ";
            //        //            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            //        //            if (objMySqlDataReader.HasRows == true)
            //        //            {
            //        //                employeename = objMySqlDataReader["username"].ToString();
            //        //                reason = objMySqlDataReader["leave_reason"].ToString();
            //        //                days = objMySqlDataReader["leave_noofdays"].ToString();
            //        //                fromdate = objMySqlDataReader["fromdate"].ToString();
            //        //                todate = objMySqlDataReader["todate"].ToString();
            //        //                lsleavetypename = objMySqlDataReader["leavetype_name"].ToString();

            //        //            }                           
            //        //                msSQL = " select a.created_by," +
            //        //                        " Concat(c.user_firstname,' ',c.user_lastname) as username " +
            //        //                        " from hrm_trn_tleave a " +
            //        //                        " left join adm_mst_tuser c on a.created_by=c.user_gid " +
            //        //                        " where a.leave_gid='" + msGetGID2 + "'";
            //        //                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            //        //                if (objMySqlDataReader.HasRows == true)
            //        //                {
            //        //                    applied_by = objMySqlDataReader["username"].ToString();
            //        //                }

            //        //                //lsSubject = "" + lsleavetypename + "Leave Application";
            //        //                //message = "Dear Sir/Madam,<br/>";
            //        //                //message = message + "<br/>";
            //        //                //message = message + "I hope this message finds you well. I am writing to request a <b>"+lsleavetypename+"</b><br/>";
            //        //                //message = message + "for <b>"+days+"</b> days<br/> ";
            //        //                //message = message + "from <b>"+fromdate+"<b>&nbsp; &nbsp;to <b>"+todate+"</b><br/>";
            //        //                //message = message + "The reason for my leave is <b>"+reason+"</b><br/>";
            //        //                //message = message + "<br/>";
            //        //                //message = message + "<b>Thanks and Regards</b><br/>";
            //        //                //message = message + ""+applied_by+"<br/>";

            //        //                lsSubject = "" + lsleavetypename + "Application";

            //        //                message = "Dear Team,<br/>";
            //        //                message = message + "<br/>";                                

            //        //                message = message + "Applied For:<b>" + lsleavetypename + "</b> <br/>";
            //        //                message = message + "<br/>";

            //        //                message = message + "<b>From:</b>" + fromdate + " &nbsp; &nbsp;<b>To: </b>" + todate + "<br/>";
            //        //                message = message + "<br/>";

            //        //                message = message + "<b>Total No.of Days:</b>" + days + "<br/>";
            //        //                message = message + "<br/>";

            //        //                message = message + "<b>Leave Reason:</b>" + reason + "<br/>";
            //        //                message = message + "<br/>";

            //        //                message = message + "<b>Thanks and Regards</b><br/>";
            //        //                message = message + "" + applied_by + "<br/>";

            //        //        try
            //        //                {
            //        //                    MailFlag = objcmnfunctions.SendSMTP2(supportmail, emailpassword, "accounts@vcidex.com", lsSubject, message, "", "", "");
            //        //                }
            //        //                catch
            //        //                {

            //        //                }
            //        //            }
            //        //        }            
            //        //else
            //        //{
            //        //    values.status = false;
            //        //    values.message = "Error occured while applying leave";
            //        //}                
            //    }
            //}
            //catch (Exception ex)
            //{
            //    values.status = false;

            //    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
            //    "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            //}
        }


        public void DaApproveevent(string employee_gid, approvesubmit values)
        {
            try
            {
                string lsleavestatus = "Approved";

                msSQL = " update hrm_trn_tleavedtl set " +
               " leave_status = '" + lsleavestatus + "'," +
               " updated_by = '" + employee_gid + "'," +
               " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
               " where leave_gid = '" + values.leave_gid + "'";
                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " update hrm_trn_tleave set " +
                            " leave_status = '" + lsleavestatus + "'," +
                            " leave_approvedby = '" + employee_gid + "'," +
                            " leave_approveddate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " approval_remarks = '" + values.remarks + "'," +
                            " updated_by = '" + employee_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                            " where leave_gid = '" + values.leave_gid + "'";
                    int mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult3 == 1)
                    {
                        string monthNumber = DateTime.Now.Month.ToString();
                        string leave_year = DateTime.Now.Year.ToString();
                        DateTime lsstartdate = DateTime.MinValue;
                        DateTime lsenddate = DateTime.MinValue;
                        string lsemployee_gid = string.Empty;
                        string branch = string.Empty;

                        // Retrieve leave information
                        string msSQL = "SELECT leave_fromdate,leave_todate, YEAR(leave_fromdate) AS lsyear, employee_gid, MONTH(leave_fromdate) AS month " +
                                       "FROM hrm_trn_tleave WHERE leave_gid='" + values.leave_gid + "'";

                        using (var objODBCDataReader = objdbconn.GetDataReader(msSQL))
                        {
                            if (objODBCDataReader.HasRows)
                            {
                                objODBCDataReader.Read();
                                lsstartdate = Convert.ToDateTime(objODBCDataReader["leave_fromdate"]);
                                lsenddate = Convert.ToDateTime(objODBCDataReader["leave_todate"]);
                                monthNumber = objODBCDataReader["month"].ToString();
                                leave_year = objODBCDataReader["lsyear"].ToString();
                                lsemployee_gid = objODBCDataReader["employee_gid"].ToString();
                            }
                        }

                        // Retrieve branch_gid
                        msSQL = "SELECT branch_gid FROM hrm_mst_temployee WHERE employee_gid='" + lsemployee_gid + "'";

                        using (var objODBCDataReader = objdbconn.GetDataReader(msSQL))
                        {
                            if (objODBCDataReader.HasRows)
                            {
                                objODBCDataReader.Read();
                                branch = objODBCDataReader["branch_gid"].ToString();
                            }
                        }

                        // Assuming Getleave expects DateTime values for start and end dates

                        string tloop = Getleave(lsemployee_gid, branch, lsstartdate, lsenddate, monthNumber, leave_year);
                        objdbconn.OpenConn();
                    }

                }

                //-----------------------------half day LOP Calculation----------------------

                msSQL = " update hrm_trn_tattendance set " +
                        " halfdayabsent_flag='Y'," +
                        " halfdayleave_flag ='Y'" +
                        " where day_session <> 'NA' and login_time is null and logout_time is null and employee_attendance='Leave'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                //-----------------------------end half day LOP Calculation----------------------

                msSQL = " Select date_format(x.leavedtl_date,'%Y-%m-%d') as leavedate,y.employee_gid from hrm_trn_tleavedtl x " +
                         " left join hrm_trn_tleave y on x.leave_gid=y.leave_gid " +
                         " where x.leave_gid = '" + values.leave_gid + "'";
                objTblemployee = objdbconn.GetDataTable(msSQL);
                if (objTblemployee.Rows.Count > 0)
                {
                    foreach (DataRow objtblemploteedatarow in objTblemployee.Rows)
                    {
                        string lsdate = "";
                        string lsleavetype = "";
                        string lsemployee;
                        string lshalfday = "";
                        string lshalfsession = null;

                        lsdate = objtblemploteedatarow["leavedate"].ToString();
                        lsemployee = objtblemploteedatarow["employee_gid"].ToString();

                        msSQL = " Select y.employee_gid,z.leavetype_code,x.half_day,x.half_session from hrm_trn_tleavedtl x " +
                        " left join hrm_trn_tleave y on x.leave_gid=y.leave_gid " +
                        " left join hrm_mst_tleavetype z on y.leavetype_gid=z.leavetype_gid " +
                         " where x.leave_gid = '" + values.leave_gid + "' and x.leavedtl_date='" + lsdate + "' ";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows)
                        {
                            lsleavetype = objMySqlDataReader["leavetype_code"].ToString();
                            lshalfday = objMySqlDataReader["half_day"].ToString();
                            lshalfsession = objMySqlDataReader["half_session"].ToString();
                            string lstype = lsleavetype;
                        }

                        msSQL = "Select employee_gid from hrm_trn_tattendance " +
                            "where attendance_date='" + lsdate + "' and employee_gid='" + lsemployee + "'";
                        objTblemployee = objdbconn.GetDataTable(msSQL);
                        if (objTblemployee.Rows.Count > 0)
                        {
                            msSQL = "update hrm_trn_tattendance set " +
                                "employee_attendance='Leave', " +
                                "attendance_type='" + lsleavetype + "', " +
                                 " day_session='" + lshalfsession + "', " +
                                " update_flag='N'" +
                                 "where attendance_date='" + lsdate + "' and employee_gid='" + lsemployee + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            msGetGID = objcmnfunctions.GetMasterGID("HATP");
                            msSQL = "Insert Into hrm_trn_tattendance" +
                                    "(attendance_gid," +
                                    " employee_gid," +
                                    " attendance_date," +
                                    " shift_date," +
                                    " employee_attendance," +
                                    " day_session, " +
                                    " attendance_type)" +
                                    " VALUES ( " +
                                    "'" + msGetGID + "', " +
                                    "'" + lsemployee + "'," +
                                    "'" + lsdate + "'," +
                                     "'" + lsdate + "'," +
                                    "'Leave'," +
                                      "'" + lshalfsession + "', " +
                                    "'" + lsleavetype + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        msSQL = " Select a.leavetype_gid,b.leavedtl_gid from hrm_trn_tleave a" +
                    " left join hrm_trn_tleavedtl b on a.leave_gid=b.leave_gid" +
                    " where a.leave_gid = '" + values.leave_gid + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        while (objMySqlDataReader.Read())
                        {
                            msSQL = " Select compensatoryoffdtl_gid from hrm_trn_tcompensatoryoffdtl" +
                            " where leave_status='Leave Applied' and" +
                            " leavedtl_gid='" + objMySqlDataReader["leavedtl_gid"].ToString() + "'";
                            objMySqlDataReader1 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader1.HasRows == true)
                            {
                                msSQL = " update hrm_trn_tcompensatoryoffdtl set leave_status='Approved'" +
                                " where compensatoryoffdtl_gid='" + objMySqlDataReader1["compensatoryoffdtl_gid"] + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Leave Approved In Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Leave Approved ";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public string Getleave(string employee_gid, string branch_gid, DateTime startdate, DateTime enddate, string leave_month, string leave_year)
        {
            try
            {
                string hrsetting_gid = null;
                double total_lop = 0.0;
                double final_lop = 0.0;
                double lsabsent = 0.0;
                string leave_name;
                int f = 0;
                double leave_taken = 0.0, ls_limit = 0.0;
                double lsleave_available = 0.0;
                string lscarry;
                string month = "0";
                string lsyear = "0";
                double leave_carry = 0.0;
                double carry_update = 0.0;
                DataTable objtb1;
                double credits_update = 0.0;
                double leavecarry_month = 0.0;
                double lsleave_carry = 0.0;
                int leavemonth;
                string lslength = "0";

                if (leave_month.Length == 1)
                {
                    leave_month = "0" + leave_month;
                }
                else
                {
                    leave_month = leave_month;

                }

                objdbconn.OpenConn();

                string mssql = "select hrsetting_gid from hrm_mst_tleavecreditsdtl where employee_gid='" + employee_gid + "'";
                using (var objODBCDataReader = objdbconn.GetDataReader(mssql))
                {
                    if (objODBCDataReader.HasRows)
                    {
                        objODBCDataReader.Read();
                        hrsetting_gid = objODBCDataReader["hrsetting_gid"].ToString();
                    }
                    else
                    {
                        hrsetting_gid = "0";
                    }
                }

                mssql = "select attendance_startdate, attendance_enddate from hrm_mst_tmastersettings where branch_gid='" + branch_gid + "' and hrsetting_gid='" + hrsetting_gid + "'";
                DataTable objTblemployee = objdbconn.GetDataTable(mssql);
                DateTime lsdate = DateTime.MinValue;
                DateTime lsenddate = DateTime.MinValue;

                if (objTblemployee.Rows.Count > 0)
                {
                    foreach (DataRow objtblemploteedatarow in objTblemployee.Rows)
                    {
                        lsdate = Convert.ToDateTime(objtblemploteedatarow["attendance_startdate"]);
                        lsenddate = Convert.ToDateTime(objtblemploteedatarow["attendance_enddate"]);
                    }
                }

                if (!string.IsNullOrEmpty(employee_gid))
                {
                    mssql = "select a.employee_gid, a.leavegrade_gid, a.leavegrade_code, a.leavegrade_name, b.leavetype_gid, c.leavetype_name, " +
                            "b.total_leavecount, b.available_leavecount, b.leave_limit, c.carryforward, c.accrud " +
                            "from hrm_trn_tleavegrade2employee a " +
                            "left join hrm_mst_tleavegradedtl b on a.leavegrade_gid = b.leavegrade_gid " +
                            "left join hrm_mst_tleavetype c on c.leavetype_gid = b.leavetype_gid " +
                            "where a.employee_gid='" + employee_gid + "'";
                    objTblemployee = objdbconn.GetDataTable(mssql);

                    if (objTblemployee.Rows.Count > 0)
                    {
                        foreach (DataRow objtblemploteedatarow in objTblemployee.Rows)
                        {
                            lsleave_available = 0.0;
                            lscarry = objtblemploteedatarow["carryforward"].ToString();
                            string lsaccrual = objtblemploteedatarow["accrud"].ToString();
                            ls_limit = 0.0;
                            double leavecarry_count = 0.0;
                            string lsleavegrade_name = objtblemploteedatarow["leavegrade_name"].ToString();
                            string lsleavegrade_gid = objtblemploteedatarow["leavegrade_gid"].ToString();
                            string lsleavetype_gid = objtblemploteedatarow["leavetype_gid"].ToString();
                            leave_name = objtblemploteedatarow["leavetype_name"].ToString();
                            double lsavailable_leave = Convert.ToDouble(objtblemploteedatarow["available_leavecount"]);
                            ls_limit = Convert.ToDouble(objtblemploteedatarow["leave_limit"]);
                            double lstotal_leave = Convert.ToDouble(objtblemploteedatarow["total_leavecount"]);

                            if (lsaccrual == "N")
                            {
                                mssql = "SELECT sum(b.leave_count) as totalleave FROM hrm_trn_tleave a " +
                                        "left join hrm_trn_tleavedtl b on a.leave_gid = b.leave_gid " +
                                        "where a.leavetype_gid = '" + lsleavetype_gid + "' " +
                                        "and a.employee_gid = '" + employee_gid + "' " +
                                        "and a.leave_status = 'Approved' " +
                                        "group by a.leavetype_gid";
                                DataTable objDtable = objdbconn.GetDataTable(mssql);

                                if (objDtable.Rows.Count > 0)
                                {
                                    foreach (DataRow objTblrow in objDtable.Rows)
                                    {
                                        leave_taken = objTblrow["totalleave"] == DBNull.Value ? 0.0 : Convert.ToDouble(objTblrow["totalleave"]);
                                        lsleave_available = lstotal_leave - leave_taken;
                                    }
                                }
                                else
                                {
                                    leave_taken = 0.0;
                                    lsleave_available = lstotal_leave - leave_taken;
                                }
                            }
                            else
                            {
                                mssql = "SELECT sum(b.leave_count) as totalleave " +
                                        "FROM hrm_trn_tleave a " +
                                        "left join hrm_trn_tleavedtl b on a.leave_gid = b.leave_gid " +
                                        "where a.leavetype_gid = '" + lsleavetype_gid + "' " +
                                        "and a.employee_gid = '" + employee_gid + "' " +
                                        "and a.leave_status = 'Approved' " +
                                        "and date_format(b.leavedtl_date, '%m') <= '" + leave_month + "' and " +
                                        "date_format(b.leavedtl_date, '%Y') <= '" + leave_year + "' " +
                                        "group by a.leavetype_gid";
                                dt_datatable = objdbconn.GetDataTable(mssql);

                                if (dt_datatable.Rows.Count > 0)
                                {
                                    foreach (DataRow objTblrow in dt_datatable.Rows)
                                    {
                                        leave_taken = objTblrow["totalleave"] == DBNull.Value ? 0.0 : Convert.ToDouble(objTblrow["totalleave"]);
                                    }
                                }
                                else
                                {
                                    leave_taken = 0.0;
                                }
                            }

                            if (lsaccrual == "Y")
                            {
                                if (lscarry == "N")
                                {
                                    mssql = "select sum(leavecarry_count) as leavecarry_count from hrm_mst_tleavecreditsdtl where employee_gid='" + employee_gid + "' " +
                                            "and leavetype_gid='" + lsleavetype_gid + "' and year='" + leave_year + "' and month <= '" + leave_month + "'";
                                    using (var objODBCDataReader = objdbconn.GetDataReader(mssql))
                                    {
                                        if (objODBCDataReader.HasRows)
                                        {
                                            lscarry = objODBCDataReader["leavecarry_count"] == DBNull.Value ? "0" : objODBCDataReader["leavecarry_count"].ToString();
                                        }
                                    }
                                    lsavailable_leave = Convert.ToDouble(lscarry) - lsavailable_leave;

                                    mssql = "update hrm_mst_tleavecreditsdtl set " +
                                            "leave_taken='" + leave_taken + "', " +
                                            "available_leavecount='" + lsavailable_leave + "' " +
                                            "where employee_gid='" + employee_gid + "' and leavetype_gid='" + lsleavetype_gid + "' and month='" + leave_month + "' and year='" + leave_year + "'";
                                    objdbconn.ExecuteNonQuerySQL(mssql);
                                }
                                else
                                {
                                    // Add any specific logic for carry forward == "Y" if needed
                                }
                            }
                            else
                            {
                                if (lscarry == "N")
                                {
                                    mssql = "select sum(leavecarry_count) as leavecarry_count from hrm_mst_tleavecreditsdtl where employee_gid='" + employee_gid + "' " +
                                            "and leavetype_gid='" + lsleavetype_gid + "' and year='" + leave_year + "' and month <= '" + leave_month + "'";
                                    using (var objODBCDataReader = objdbconn.GetDataReader(mssql))
                                    {
                                        if (objODBCDataReader.HasRows)
                                        {
                                            lscarry = objODBCDataReader["leavecarry_count"] == DBNull.Value ? "0" : objODBCDataReader["leavecarry_count"].ToString();
                                        }
                                    }
                                    lsavailable_leave = lstotal_leave - leave_taken;
                                    mssql = "update hrm_mst_tleavecreditsdtl set " +
                                            "leave_taken='" + leave_taken + "', " +
                                            "available_leavecount='" + lsavailable_leave + "' " +
                                            "where employee_gid='" + employee_gid + "' and leavetype_gid='" + lsleavetype_gid + "' and month='" + leave_month + "' and year='" + leave_year + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                                }
                                else
                                {
                                    mssql = "select sum(leavecarry_count) as leavecarry_count from hrm_mst_tleavecreditsdtl where employee_gid='" + employee_gid + "' " +
                                            "and leavetype_gid='" + lsleavetype_gid + "' and year='" + leave_year + "' and month <= '" + leave_month + "'";
                                    using (var objODBCDataReader = objdbconn.GetDataReader(mssql))
                                    {
                                        if (objODBCDataReader.HasRows)
                                        {
                                            lscarry = objODBCDataReader["leavecarry_count"] == DBNull.Value ? "0" : objODBCDataReader["leavecarry_count"].ToString();
                                        }
                                    }
                                    lsavailable_leave = lstotal_leave - leave_taken;
                                    mssql = "update hrm_mst_tleavecreditsdtl set " +
                                            "leave_taken='" + leave_taken + "', " +
                                            "available_leavecount='" + lsavailable_leave + "' " +
                                            "where employee_gid='" + employee_gid + "' and leavetype_gid='" + lsleavetype_gid + "' and month='" + leave_month + "' and year='" + leave_year + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                                }
                            }
                        }
                    }
                }

                objdbconn.CloseConn();

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                            " DataAccess: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                                            " Error: " + ex.Message,
                                            "ErrorLog/HR/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return null;
        }

        public void DaRejectleaveevent(string employee_gid, approvesubmit values)
        {
            try
            {

                string lsleavestatus = "Rejected";

                msSQL = " update hrm_trn_tapproval set " +
                              " approval_flag = 'R', " +
                              " approved_date =  '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                              " where approved_by = '" + employee_gid + "'" +
                              " and leaveapproval_gid = '" + values.leave_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update hrm_trn_tleave set " +
                       " leave_status = '" + lsleavestatus + "'," +
                       " leave_approvedby ='" + employee_gid + "'," +
                       " leave_approveddate =  '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                           " approval_remarks= ' approval_remarks '," +
                           " updated_by = '" + employee_gid + "'," +
                           " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where leave_gid = '" + values.leave_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = " update hrm_trn_tleavedtl set " +
                            " leave_status = '" + lsleavestatus + "'," +
                            " updated_by = '" + employee_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                            " where leave_gid = '" + values.leave_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                msSQL = " Select a.leavetype_gid,b.leavedtl_gid from hrm_trn_tleave a" +
                        " left join hrm_trn_tleavedtl b on a.leave_gid=b.leave_gid" +
                        " where a.leavetype_gid='LT1203060069' and a.leave_gid = '" + values.leave_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    while (objMySqlDataReader.Read())
                    {

                        msSQL = " Select compensatoryoffdtl_gid from hrm_trn_tcompensatoryoffdtl" +
                                " where leave_status='Leave Applied' and" +
                                " leavedtl_gid = '" + objMySqlDataReader["leavedtl_gid"] + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);


                        if (objMySqlDataReader.HasRows == true)
                        {
                            msSQL = " update hrm_trn_tcompensatoryoffdtl set leave_status='not taken'," +
                                        " leave_date=null,leavedtl_gid=''" +
                                        " where compensatoryoffdtl_gid='" + objMySqlDataReader["compensatoryoffdtl_gid"] + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Rejected In Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Rejected ";
                }

            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }


        public void DaLoginApprove(string employee_gid, loginapprove values)
        {
            try
            {

                string lsleavestatus = "Approved";
                string lslogin = "";
                string lsshift_gid = "";
                string lsattendance_date = "";
                string lsemployee_gid = "";

                msSQL = " update hrm_tmp_tattendancelogin set " +
                        " status = '" + lsleavestatus + "'," +
                        " approved_by = '" + employee_gid + "'," +
                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where attendancelogintmp_gid = '" + values.attendancelogintmp_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {

                    msSQL = "SELECT login_time,employee_gid,attendance_date,remarks FROM hrm_tmp_tattendancelogin " +
                               "WHERE attendancelogintmp_gid='" + values.attendancelogintmp_gid + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        DateTime logintime = Convert.ToDateTime(objMySqlDataReader["login_time"]);
                        lslogin = logintime.ToString("yyyy-MM-dd HH:mm:ss");
                        lsemployee_gid = objMySqlDataReader["employee_gid"].ToString();
                        DateTime attendanceDate = Convert.ToDateTime(objMySqlDataReader["attendance_date"]);
                        lsattendance_date = attendanceDate.ToString("yyyy-MM-dd");
                    }


                    msSQL = "SELECT attendance_gid FROM hrm_trn_tattendance " +
                             "WHERE employee_gid='" + lsemployee_gid + "' AND attendance_date='" + lsattendance_date + "'";

                    var objTblemployee = objdbconn.GetDataTable(msSQL);

                    if (objTblemployee.Rows.Count > 0)
                    {

                        msSQL = "UPDATE hrm_trn_tattendance SET " +
                                "login_time='" + lslogin + "', " +
                                "login_time_audit='" + lslogin + "', " +
                                "attendance_source='Requisition', " +
                                "approved_by='" + employee_gid + "', " +
                                "employee_attendance='Present', " +
                                "attendance_type='P', " +
                                "update_flag='N' " +
                                "WHERE employee_gid='" + lsemployee_gid + "' AND " +
                                "attendance_date='" + lsattendance_date + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    else
                    {
                        msSQL = "SELECT employee2shifttypedtl_gid FROM hrm_trn_temployee2shifttypedtl WHERE employee_gid='" + lsemployee_gid + "' " +
                                "AND employee2shifttype_name='" + lsattendance_date + "' AND shift_status='Y'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows)
                        {
                            lsshift_gid = objMySqlDataReader["employee2shifttypedtl_gid"].ToString();

                        }
                        string msGetGID = objcmnfunctions.GetMasterGID("HATP");

                        msSQL = "INSERT INTO hrm_trn_tattendance " +
                                "(attendance_gid, employee_gid, attendance_date, shift_date, attendance_source, " +
                                "login_time, login_time_audit, employee_attendance, shifttype_gid, attendance_type) " +
                                "VALUES (" +
                                "'" + msGetGID + "', " +
                                "'" + lsemployee_gid + "', " +
                                "'" + lsattendance_date + "', " +
                                "'" + lsattendance_date + "', " +
                                "'Requisition', " +
                                "'" + lslogin + "', " +
                                "'" + lslogin + "', " +
                                "'Present', " +
                                "'" + lsshift_gid + "', " +
                                "'P')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Login Time Approved Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Login Time Approved ";
                }


            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaLoginreject(string employee_gid, loginapprove values)
        {
            try
            {

                string lsleavestatus = "Rejected";
                string lslogin = "";
                string lsshift_gid = "";
                string lsattendance_date = "";
                string lsemployee_gid = "";

                msSQL = " update hrm_tmp_tattendancelogin set " +
                        " status = '" + lsleavestatus + "'," +
                        " approved_by = '" + employee_gid + "'," +
                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where attendancelogintmp_gid = '" + values.attendancelogintmp_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Login Time Rejected Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Login Time Rejected ";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaLogoutApprove(string employee_gid, logoutapprove values)
        {
            try
            {

                string lsleavestatus = "Approved";
                string lslogout = "";
                string lsshift_gid = "";
                string lsattendance_date = "";
                string lsemployee_gid = "";

                msSQL = " update hrm_tmp_tattendance set " +
                        " status = '" + lsleavestatus + "'," +
                        " approvedby = '" + employee_gid + "'," +
                        " approveddate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where attendancetmp_gid = '" + values.attendancetmp_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {

                    msSQL = "SELECT logout_time,employee_gid,attendance_date FROM hrm_tmp_tattendance " +
                               "WHERE attendancetmp_gid='" + values.attendancetmp_gid + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                    if (objMySqlDataReader.HasRows == true)
                    {
                        DateTime logouttime = Convert.ToDateTime(objMySqlDataReader["logout_time"]);
                        lslogout = logouttime.ToString("yyyy-MM-dd HH:mm:ss");
                        lsemployee_gid = objMySqlDataReader["employee_gid"].ToString();
                        DateTime attendanceDate = Convert.ToDateTime(objMySqlDataReader["attendance_date"]);
                        lsattendance_date = attendanceDate.ToString("yyyy-MM-dd");
                    }

                    msSQL = "UPDATE hrm_trn_tattendance SET " +
                            "logout_time = '" + lslogout + "', " +
                            "logout_time_audit = '" + lslogout + "', " +
                            "update_flag = 'N' " +
                            "WHERE attendance_date = '" + lsattendance_date + "' AND employee_gid = '" + lsemployee_gid + "' " +
                            "AND login_time IS NOT NULL";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    TimeSpan time1 = TimeSpan.Zero;
                    TimeSpan time2 = TimeSpan.Zero;
                    TimeSpan lslunch = TimeSpan.Zero;
                    TimeSpan lstotal = TimeSpan.Zero;
                    string lslogouttime = "";

                    msSQL = "SELECT TIME(login_time) AS logintime, TIME(logout_time) AS logouttime, TIME(lunch_out_scheduled) AS lunch_out_scheduled " +
                            "FROM hrm_trn_tattendance " +
                            "WHERE employee_gid = '" + lsemployee_gid + "' AND attendance_date LIKE '" + lsattendance_date + "'";

                    var objODBCDataReader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDataReader.HasRows)
                    {
                        time1 = TimeSpan.Parse(objODBCDataReader["logintime"].ToString());
                        time2 = TimeSpan.Parse(objODBCDataReader["logouttime"].ToString());
                        lslunch = TimeSpan.Parse(objODBCDataReader["lunch_out_scheduled"].ToString());
                        //DateTime logoutTime = Convert.ToDateTime(objODBCDataReader["logouttime"]);
                        // lslogouttime = logoutTime.ToString("HH:mm:ss");

                        //if (lslogouttime < "14:00:00")

                        //{
                        //    lstotal = time2 - time1;
                        //}
                        //else
                        //{
                        //    lstotal = time2 - time1 - lslunch;
                        //}
                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Login out Approved Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Login out Approved ";
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


        public void DaLogoutreject(string employee_gid, logoutapprove values)
        {
            try
            {

                string lsleavestatus = "Rejected";

                msSQL = " update hrm_tmp_tattendance set " +
                        " status = '" + lsleavestatus + "'," +
                        " approvedby = '" + employee_gid + "'," +
                        " approveddate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where attendancetmp_gid = '" + values.attendancetmp_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Login Out Rejected Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Login Out Rejected ";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaCompoffApprove(string employee_gid, compoffapprove values)
        {
            try
            {
                string lsdate_compoff = "";
                string lsemployee = "";
                string lsleavestatus = "Approved";
                string status = "Approved";

                msSQL = " update hrm_trn_tcompensatoryoff set " +
                        " compensatoryoff_status = '" + status + "'," +
                        " compensatoryoff_approveby = '" + employee_gid + "'," +
                        " compensatoryoff_approvedate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " where compensatoryoff_gid = '" + values.compensatoryoff_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "Update hrm_trn_tcompensatoryoffdtl set " +
                                " status='" + lsleavestatus + "'," +
                                " updated_by='" + employee_gid + "'," +
                                " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                " where compensatoryoff_gid = '" + values.compensatoryoff_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " Select compensatoryoff_applydate, actualworking_fromdate, compensatoryoff_applydate, compensatoryoff_reason, employee_gid from hrm_trn_tcompensatoryoff where compensatoryoff_gid = '" + values.compensatoryoff_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {

                    lsemployee = objMySqlDataReader["employee_gid"].ToString();
                    DateTime attendanceDate = Convert.ToDateTime(objMySqlDataReader["compensatoryoff_applydate"]);
                    lsdate_compoff = attendanceDate.ToString("yyyy-MM-dd");

                }

                msSQL = "Select employee_gid from hrm_trn_tattendance " +
                        "where attendance_date='" + lsdate_compoff + "' " +
                        "and employee_gid='" + lsemployee + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    msSQL = "update hrm_trn_tattendance set " +
                            "employee_attendance='Compoff', " +
                            "attendance_type='Compoff', " +
                            " update_flag='N'" +
                             "where attendance_date='" + lsdate_compoff + "' " +
                             "and employee_gid='" + lsemployee + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    string msGetGID = objcmnfunctions.GetMasterGID("HATP");

                    msSQL = "Insert Into hrm_trn_tattendance" +
                                    "(attendance_gid," +
                                    " employee_gid," +
                                    " attendance_date," +
                                     " shift_date," +
                                    " employee_attendance," +
                                    " attendance_type)" +
                                    " VALUES ( " +
                                    "'" + msGetGID + "', " +
                                    "'" + lsemployee + "'," +
                                    "'" + lsdate_compoff + "'," +
                                    "'" + lsdate_compoff + "'," +
                                    "'Compoff'," +
                                    "'Compoff')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "CompOff Approved Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While CompOff Approved ";
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

        public void DaCompoffreject(string employee_gid, compoffapprove values) 
        {
            try
            {
                string status = "Rejected";
                string lsleavestatus = "Rejected";

                msSQL = " update hrm_trn_tcompensatoryoff set " +
                           " compensatoryoff_status = '" + status + "'," +
                           " compensatoryoff_approveby = '" + employee_gid + "'," +
                           " compensatoryoff_approvedate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                           " updated_by = '" + employee_gid + "'," +
                           " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where compensatoryoff_gid = '" + values.compensatoryoff_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "Update hrm_trn_tcompensatoryoffdtl set " +
                                   " status='" + lsleavestatus + "'," +
                                   " updated_by='" + employee_gid + "'," +
                                   " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where compensatoryoff_gid = '" + values.compensatoryoff_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Compoff Rejected Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Compoff Rejected ";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaApproveOD(string employee_gid, approveOD_list values)
        {
            try
            {
                msSQL = "SELECT CAST(CONCAT(ondutytracker_date, ' ', REPLACE(CONCAT(onduty_fromtime,':00'), ' ', '')) AS CHAR) AS login_time, " +
                               "CAST(CONCAT(ondutytracker_date, ' ', REPLACE(CONCAT(onduty_totime,':00'), ' ', '')) AS CHAR) AS logout_time, " +
                               "half_day, CAST(half_session AS CHAR) AS half_session, DATE_FORMAT(ondutytracker_date, '%Y-%m-%d') AS ondutytracker_date, employee_gid " +
                               "FROM hrm_trn_tondutytracker " +
                               "WHERE ondutytracker_gid = '" + values.ondutytracker_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    msSQL = "UPDATE hrm_trn_tattendance SET " +
                            "attendance_type = '" + objMySqlDataReader["half_session"] + "', " +
                            "employee_attendance = 'Onduty', " +
                            "login_time = '" + objMySqlDataReader["login_time"] + "', " +
                            "logout_time = '" + objMySqlDataReader["logout_time"] + "', " +
                            "update_flag = 'N' " +
                            "WHERE attendance_date = '" + objMySqlDataReader["ondutytracker_date"] + "' " +
                           "AND employee_gid = '" + objMySqlDataReader["employee_gid"] + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                msSQL = " update hrm_trn_tondutytracker set " +
                           " ondutytracker_status = 'Approved'," +
                           " onduty_approveby = '" + employee_gid + "'," +
                           " onduty_approvedate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where ondutytracker_gid = '" + values.ondutytracker_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Onduty Approved Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Onduty Approved ";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaRejectOD(string employee_gid, approveOD_list values)
        {
            try
            {
                msSQL = " update hrm_trn_tondutytracker set " +
                           " ondutytracker_status = 'Rejected'," +
                           " onduty_approveby = '" + employee_gid + "'," +
                           " onduty_approvedate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where ondutytracker_gid = '" + values.ondutytracker_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Onduty Rejected Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Onduty Rejected ";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPermissiontapprove(string employee_gid, permissionapprove_list values)
        {
            try
            {
                string lsleavestatus = "Approved";
                msSQL = "UPDATE hrm_trn_tpermissiondtl SET " +
                       "permission_status = '" + lsleavestatus + "', " +
                       "permission_approvedby = '" + employee_gid + "', " +
                       "permission_approveddate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       "updated_by = '" + employee_gid + "', " +
                       "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                       "WHERE permissiondtl_gid = '" + values.permissiondtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                TimeSpan lstotaltime = TimeSpan.Zero;
                string lsemployee_gid = "";
                string lspermission_date = "";

                msSQL = "SELECT permission_gid, employee_gid, permission_date, " +
                        "CONCAT(permission_totalhours, ':', total_mins, ':00') AS totaltime " +
                        "FROM hrm_trn_tpermissiondtl WHERE permissiondtl_gid = '" + values.permissiondtl_gid + "' " +
                       "AND permission_status = 'Approved'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    while (objMySqlDataReader.Read())
                    {
                        lsemployee_gid = objMySqlDataReader["employee_gid"].ToString();
                        lspermission_date = objMySqlDataReader["permission_date"].ToString();
                    }
                }

                msSQL = "SELECT permission_gid, employee_gid, permission_totalhours, total_mins, " +
                        "CONCAT(permission_totalhours, ':', total_mins, ':00') AS totaltime " +
                        "FROM hrm_trn_tpermissiondtl WHERE employee_gid = '" + lsemployee_gid + "' " +
                        "AND permission_date = '" + DateTime.Parse(lspermission_date).ToString("yyyy-MM-dd") + "' " +
                        "AND permission_status = 'Approved'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    while (objMySqlDataReader.Read())
                    {
                        TimeSpan lstime = TimeSpan.Parse(objMySqlDataReader["totaltime"].ToString());
                        lstotaltime = lstotaltime.Add(lstime);
                    }

                    string totalhr = lstotaltime.Hours.ToString("D2");
                    string totalmin = lstotaltime.Minutes.ToString("D2");

                    msSQL = "UPDATE hrm_trn_tpermission SET " +
                            "permission_totalhours = '" + totalhr + "', " +
                            "total_mins = '" + totalmin + "', " +
                            "permission_status = 'Approved' " +
                            "WHERE employee_gid = '" + lsemployee_gid + "' " +
                            "AND permission_date = '" + DateTime.Parse(lspermission_date).ToString("yyyy-MM-dd") + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }



                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Permission Approved Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Permission Approved ";
                }

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPermissionReject(string employee_gid, permissionapprove_list values)
        {
            try
            {
                string lsleavestatus = "Rejected";
                msSQL = "UPDATE hrm_trn_tpermissiondtl SET " +
                       "permission_status = '" + lsleavestatus + "', " +
                       "permission_approvedby = '" + employee_gid + "', " +
                       "permission_approveddate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       "updated_by = '" + employee_gid + "', " +
                       "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                       "WHERE permissiondtl_gid = '" + values.permissiondtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Permission Rejected Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Permission Rejected ";
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