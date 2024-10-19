using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web;
using System.Data.Odbc;
using static OfficeOpenXml.ExcelErrorValue;
using Org.BouncyCastle.Asn1.Ocsp;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using MySqlX.XDevAPI;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Linq;
using MySqlX.XDevAPI.Relational;
using System.Globalization;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnLeaveManage
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable dt1 = new DataTable();
        DataTable DataTable2 = new DataTable();
        string company_logo_path, lsholiday, lstholiday, lsday, lslop, lsWeekOff_flag, lstweekoff, lsleave, lsleavefromdate, lsleavetodate, onduty_period, lsalreadyPermission_Date, lsalreadyOnDuty_Date, permission_date, lstotalhours, msGetGid, lshalfday, lshalfsession, lstype, lscount, half_day;
        string lsweekoff_applicable, emp_code, lspermissioned_date, lspermission_date, lsalreadyLeave_Date, Permission_date, lsholidaydate, lsemployee_gid, lscompensatoryoff_gid, lsnoworking, halfDayValue, msGetleavedtlGid, msGetGid2, lsstartdate, lsnorecorcd, user_gid, lsdate, lsholiday_applicable, leavetype_gid, leave_session, half_session, lscompoff_leavecount;
        int compcount = 5;
        int j = 2;
        public void DaGetLeaveManageSummary(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " SELECT DISTINCT a.leave_gid, a.employee_gid, SUM(b.leave_count) AS leave_noofdays, date_format(a.leave_applydate, '%d-%m-%Y') as leave_applydate, date_format(a.leave_fromdate, '%d-%m-%Y') as leave_fromdate, " +
                        " g.branch_name, g.branch_prefix, date_format(a.leave_todate, '%d-%m-%Y') as leave_todate, a.leave_approveddate, b.leavedtl_date, d.leavetype_name, c.user_gid, f.user_code, " +
                        " CONCAT(f.user_code, ' / ', f.user_firstname, ' ', f.user_lastname) AS user_fullname, a.leave_status, h.department_name, e.designation_name, d.leavetype_gid, date_format(a.created_date, '%d-%m-%Y') as created_date, a.leave_reason " +
                        " FROM hrm_trn_tleave a " +
                        " LEFT JOIN hrm_trn_tleavedtl b ON a.leave_gid = b.leave_gid " +
                        " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
                        " LEFT JOIN hrm_mst_tleavetype d ON a.leavetype_gid = d.leavetype_gid " +
                        " LEFT JOIN adm_mst_tdesignation e ON c.designation_gid = e.designation_gid " +
                        " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
                        " LEFT JOIN hrm_mst_tbranch g ON c.branch_gid = g.branch_gid " +
                        " LEFT JOIN hrm_mst_tdepartment h ON c.department_gid = h.department_gid " +
                        " LEFT JOIN hrm_trn_temployeetypedtl j ON c.employee_gid = j.employee_gid " +
                        " WHERE 0 = 0 " +
                        " GROUP BY a.leave_gid, a.employee_gid, a.leave_applydate,  a.leave_fromdate, g.branch_name,  a.leave_todate, " +
                        " a.leave_approveddate, b.leavedtl_date,  d.leavetype_name, c.user_gid, f.user_code, user_firstname, " +
                        " a.leave_status, e.designation_name, d.leavetype_name, d.leavetype_gid ORDER BY a.created_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavemanage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavemanage_list
                        {
                            leave_gid = dt["leave_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            leave_noofdays = dt["leave_noofdays"].ToString(),
                            leave_applydate = dt["leave_applydate"].ToString(),
                            leave_fromdate = dt["leave_fromdate"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            leave_todate = dt["leave_todate"].ToString(),
                            leave_approveddate = dt["leave_approveddate"].ToString(),
                            leavedtl_date = dt["leavedtl_date"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_fullname = dt["user_fullname"].ToString(),
                            leave_status = dt["leave_status"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            leave_reason = dt["leave_reason"].ToString(),
                        });
                        values.leavemanagelist = getModuleList;

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

        public void DaGetPermissionSummary(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " select a.permission_gid,a.employee_gid,date_format(a.permission_date, '%d-%m-%Y') as permission_date," +
                    " concat(permission_fromhours) as permission_fromhours," +
                    " concat(permission_tohours) as permission_tohours," +
                    " a.permission_totalhours,a.permission_status,c.user_code, concat(c.user_code, ' / ', user_firstname,' ',user_lastname) as employee_name, " +
                    " d.branch_name, d.branch_prefix, e.designation_name, h.department_name, date_format(a.created_date, '%d-%m-%Y') as created_date, a.permission_reason from hrm_trn_tpermission a " +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                    " left join  adm_mst_tdesignation e ON b.designation_gid = e.designation_gid " +
                    " left join hrm_mst_tdepartment h ON b.department_gid = h.department_gid " +
                    " left join hrm_mst_tbranch d on b.branch_gid=d.branch_gid where permission_status='Approved' ORDER BY a.created_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<permissionname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new permissionname_list
                        {
                            permission_gid = dt["permission_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            permission_date = dt["permission_date"].ToString(),
                            permission_fromhours = dt["permission_fromhours"].ToString(),
                            permission_tohours = dt["permission_tohours"].ToString(),
                            permission_totalhours = dt["permission_totalhours"].ToString(),
                            permission_status = dt["permission_status"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            permission_reason = dt["permission_reason"].ToString(),

                        });
                        values.permissionnamelist = getModuleList;
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

        public void DaGetOnDutySummary(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " Select a.ondutytracker_gid, a.employee_gid, concat(a.onduty_fromtime,':',case when a.from_minutes is null then '00' else from_minutes end,':','00') as onduty_fromtime , " +
                     " concat(a.onduty_totime,':',case when a.to_minutes is null then '00' else to_minutes end,':','00') as onduty_totime, date_format(a.created_date, '%d-%m-%Y') as created_date, " +
                     " a.ondutytracker_status, d.branch_prefix, a.onduty_duration, a.onduty_reason, h.department_name, e.designation_name, date_format(a.ondutytracker_date, '%d-%m-%Y') as ondutytracker_date , concat(c.user_code, ' / ', c.user_firstname,' ',c.user_lastname) as user_firstname,c.user_code,d.branch_name " +
                     " from hrm_trn_tondutytracker a left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                     " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                     " left join hrm_mst_tbranch d on b.branch_gid=d.branch_gid " +
                     " left join  adm_mst_tdesignation e ON b.designation_gid = e.designation_gid  " +
                     " left join hrm_mst_tdepartment h on b.department_gid=h.department_gid " +
                     " left join hrm_trn_temployeetypedtl j on b.employee_gid=j.employee_gid where a.ondutytracker_status='Approved' ORDER BY a.ondutytracker_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ondutyname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ondutyname_list
                        {
                            ondutytracker_gid = dt["ondutytracker_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            onduty_fromtime = dt["onduty_fromtime"].ToString(),
                            onduty_totime = dt["onduty_totime"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            ondutytracker_status = dt["ondutytracker_status"].ToString(),
                            onduty_duration = dt["onduty_duration"].ToString(),
                            onduty_reason = dt["onduty_reason"].ToString(),
                            ondutytracker_date = dt["ondutytracker_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.ondutynamelist = getModuleList;
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
        public void DaGetBranchDtl(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " Select branch_name,branch_gid  " +
                        " from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbranchdetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getbranchdetail
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.Getbranch_detail = getModuleList;
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

        public void DaGetDepartmentDtl(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " Select department_name,department_gid  " +
                        " from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getdepartmentdetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdepartmentdetail
                        {
                            department_name = dt["department_name"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.Getdepartment_detail = getModuleList;
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

        public void DaGetEmployeeDtl(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = "select a.employee_gid,concat(b.user_code,'/',b.user_firstname,' ',b.user_lastname) as employee_name from hrm_mst_temployee a " +
                        " inner join adm_mst_tuser b on b.user_gid=a.user_gid" +
                        " inner join hrm_mst_tbranch c on c.branch_gid=a.branch_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeedtl>();
                if (dt_datatable.Rows.Count != 0)
                {

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeedtl
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetEmployee_dtl = getModuleList;
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

        public void DaGetDateOfJoin(string employee_gid, MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " select employee_gid, date_format(employee_joiningdate, '%d-%m-%Y') as employee_joiningdate from hrm_mst_temployee " +
                " where employee_gid = '" + employee_gid + "'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDateJoin>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDateJoin
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                        });
                        values.GetDate_Join = getModuleList;
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

        public void DaGetLeaveAvailableDtl(MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " select leavetype_gid, leavetype_name from hrm_mst_tleavetype "; 
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLeaveAvailable>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLeaveAvailable
                        {
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                          

                        });
                        values.GetLeave_Available = getModuleList;
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
        public void DaGetLeavebalance(MdlHrmTrnLeaveManage values,string employee_gid)
        {
            try
            {
                msSQL = " select a.employee_gid,b.leavetype_name,a.available_leavecount from hrm_trn_tleavecreditsdtl a "+
                        " left join hrm_mst_tleavetype b   on a.leavetype_gid=b.leavetype_gid"+
                        " where employee_gid= '" + employee_gid + "'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLeaveBalance>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLeaveBalance
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            availableleave_count = dt["available_leavecount"].ToString(),


                        });
                        values.GetLeaveBalance = getModuleList;
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

        public void DaGetLeaveManage(string branch_gid, string department_gid, string leavetype, MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = " select distinct a.leave_gid, a.employee_gid, sum(b.leave_count) as leave_noofdays, date_format(a.leave_applydate, '%d-%m-%Y') as leave_applydate, " +
                         " date_format(a.leave_fromdate, '%d-%m-%Y') as leave_fromdate, g.branch_name, date_format(a.leave_todate, '%d-%m-%Y') as leave_todate, " +
                         " date_format(a.leave_approveddate,'%d-%m-%Y') as leave_approveddate, date_format(b.leavedtl_date,'%d-%m-%Y') as leavedtl_date, " +
                         " d.leavetype_name, c.user_gid, f.user_code, concat(f.user_firstname,' ',f.user_lastname) as user_firstname, " +
                         " a.leave_status,e.designation_name,d.leavetype_name,d.leavetype_gid " +
                         " from hrm_trn_tleave a " +
                         " left join hrm_trn_tleavedtl b on a.leave_gid = b.leave_gid " +
                         " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid " +
                         " left join hrm_mst_tleavetype d on a.leavetype_gid = d.leavetype_gid " +
                         " left join adm_mst_tdesignation e on c.designation_gid = e.designation_gid " +
                         " left join adm_mst_tuser f on f.user_gid = c.user_gid " +
                         " left join hrm_mst_tbranch g on c.branch_gid = g.branch_gid " +
                         " left join hrm_mst_tdepartment h on c.department_gid = h.department_gid " +
                         " left join hrm_trn_temployeetypedtl j on c.employee_gid = j.employee_gid " +
                         " where 0 = 0 g.branch_gid = '" + branch_gid + "' and h.department_gid = '" + department_gid + "'" +
                         " group by leave_gid  order by date(a.leave_applydate) desc,a.leave_applydate asc,a.leave_gid desc ";
                         
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavemanage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavemanage_list
                        {
                            leave_gid = dt["leave_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            leave_noofdays = dt["leave_noofdays"].ToString(),
                            leave_applydate = dt["leave_applydate"].ToString(),
                            leave_fromdate = dt["leave_fromdate"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            leave_todate = dt["leave_todate"].ToString(),
                            leave_approveddate = dt["leave_approveddate"].ToString(),
                            leavedtl_date = dt["leavedtl_date"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            leave_status = dt["leave_status"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                        });
                        values.leavemanagelist = getModuleList;
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

        //public void DaGetLeaveManage(string branch, string department, string fromdate, string leavetype, MdlHrmTrnLeaveManage values)
        //{
        //    try
        //    {
        //        msSQL = " SELECT DISTINCT a.leave_gid, a.employee_gid, SUM(b.leave_count) AS leave_noofdays, date_format(a.leave_applydate, '%d-%m-%Y') as leave_applydate, date_format(a.leave_fromdate, '%d-%m-%Y') as leave_fromdate, " +
        //                   " g.branch_name, date_format(a.leave_todate, '%d-%m-%Y') as leave_todate, a.leave_approveddate, b.leavedtl_date, d.leavetype_name, c.user_gid, f.user_code, " +
        //                   " CONCAT(f.user_firstname, ' ', f.user_lastname) AS user_firstname, a.leave_status, h.department_name, e.designation_name, d.leavetype_gid " +
        //                   " FROM hrm_trn_tleave a " +
        //                   " LEFT JOIN hrm_trn_tleavedtl b ON a.leave_gid = b.leave_gid " +
        //                   " LEFT JOIN hrm_mst_temployee c ON a.employee_gid = c.employee_gid " +
        //                   " LEFT JOIN hrm_mst_tleavetype d ON a.leavetype_gid = d.leavetype_gid " +
        //                   " LEFT JOIN adm_mst_tdesignation e ON c.designation_gid = e.designation_gid " +
        //                   " LEFT JOIN adm_mst_tuser f ON f.user_gid = c.user_gid " +
        //                   " LEFT JOIN hrm_mst_tbranch g ON c.branch_gid = g.branch_gid " +
        //                   " LEFT JOIN hrm_mst_tdepartment h ON c.department_gid = h.department_gid " +
        //                   " LEFT JOIN hrm_trn_temployeetypedtl j ON c.employee_gid = j.employee_gid " +
        //                   " where g.branch_name = '" + branch + "' and h.department_name = '" + department + "' and " +
        //                   " leave_fromdate between DATE_FORMAT(STR_TO_DATE('" + fromdate + "', '%d-%m-%Y'), '%Y-%m-%d') and d.leavetype_name = '" + leavetype + "'" +
        //                   " GROUP BY a.leave_gid, a.employee_gid, a.leave_applydate,  a.leave_fromdate, g.branch_name,  a.leave_todate, " +
        //                   " a.leave_approveddate, b.leavedtl_date,  d.leavetype_name, c.user_gid, f.user_code, user_firstname, " +
        //                   " a.leave_status, e.designation_name, d.leavetype_name, d.leavetype_gid ";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<leavemanage_list>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new leavemanage_list
        //                {
        //                    leave_gid = dt["leave_gid"].ToString(),
        //                    employee_gid = dt["employee_gid"].ToString(),
        //                    leave_noofdays = dt["leave_noofdays"].ToString(),
        //                    leave_applydate = dt["leave_applydate"].ToString(),
        //                    leave_fromdate = dt["leave_fromdate"].ToString(),
        //                    branch_name = dt["branch_name"].ToString(),
        //                    department_name = dt["department_name"].ToString(),
        //                    leave_todate = dt["leave_todate"].ToString(),
        //                    leave_approveddate = dt["leave_approveddate"].ToString(),
        //                    leavedtl_date = dt["leavedtl_date"].ToString(),
        //                    leavetype_name = dt["leavetype_name"].ToString(),
        //                    user_gid = dt["user_gid"].ToString(),
        //                    user_code = dt["user_code"].ToString(),
        //                    user_firstname = dt["user_firstname"].ToString(),
        //                    leave_status = dt["leave_status"].ToString(),
        //                    designation_name = dt["designation_name"].ToString(),
        //                    leavetype_gid = dt["leavetype_gid"].ToString(),
        //                });
        //                values.leavemanagelist = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }
        //}

        public void DaDeleteLeaveManage(string leave_gid, MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = "  delete from hrm_trn_tleave where leave_gid ='" + leave_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Leave Manage Deleted Successfully";
                }
                else
                {
                    
                     values.status = false;
                     values.message = "Error While Deleting Leave Manage";
                   
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaDeletePermission(string permission_gid, MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = "  delete from hrm_trn_tpermission  where permission_gid  ='" + permission_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Permission Deleted Successfully";
                }
                else
                {
                    
                    values.status = false;
                    values.message = "Error While Deleting Permission";
                    
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaDeleteOnDuty(string ondutytracker_gid, MdlHrmTrnLeaveManage values)
        {
            try
            {
                msSQL = "  delete from hrm_trn_tondutytracker   where ondutytracker_gid   ='" + ondutytracker_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "OnDuty Deleted Successfully";
                }
                else
                {
                    
                    values.status = false;
                    values.message = "Error While Deleting OnDuty";
                    
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaLeaveSubmit(string employee_gid, leavemanage_list values)
        {
            try
            {
              
                if (values.days_name == "H")
                {
                    lshalfday = "Y";
                    lshalfsession = values.session_leave;
                }
                else
                {
                    lshalfday = "N";
                }
                msSQL = "select leavetype_code, consider_as from hrm_mst_tleavetype where leavetype_gid='" + values.leavetype_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lstype = objOdbcDataReader["leavetype_code"].ToString();
                }
                msGetGid = objcmnfunctions.GetMasterGID("HLVP");
                if (msGetGid == "E")
                {
                    values.message = "Error While Generating Gid";
                }

                msSQL = " select a.leavedtl_gid,a.half_session,date_format(a.leavedtl_date,'%Y-%m-%d') as leave_date from hrm_trn_tleavedtl a " +
                          "  left join hrm_trn_tleave b on a.leave_gid=b.leave_gid " +
                          " where a.leavedtl_date >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and " +
                          " a.leavedtl_date<='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and b.employee_gid='" + values.employeegid + "' and a.leave_status<>'Cancelled'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsalreadyLeave_Date = objOdbcDataReader["leave_date"].ToString();
                    if (lsalreadyLeave_Date == values.leave_date)
                    {
                        values.message = "Already Leave Applied For This Date";
                        return;
                    }
                }


                msSQL1 = " Insert into hrm_trn_tleavedtl" +
                        " (leavedtl_gid," +
                        " leave_gid ," +
                        " leavetype_gid," +
                        " leavedtl_date," +
                        " created_date," +
                        " created_by," +
                        " weekoff_flag," +
                        " holiday, " +
                        " leave_count," +
                        " leave_status, " +
                        " half_day," +
                        " half_session," +
                        " lop) Values ";

                DateTime lsleavefromdate = values.leave_datefrom;
                DateTime lsleavetodate = values.leave_dateto;
                int daysDifference = (lsleavetodate - lsleavefromdate).Days;

                for (int j = 0; j <= daysDifference; j++)
                {
                    lsstartdate = lsleavefromdate.ToString("yyyy-MM-dd");
                    lsleave = lsleavefromdate.ToString("yyyy-MM-dd");
                    lsWeekOff_flag = "N";
                    lstweekoff = "";
                    msSQL = "select " + lsleavefromdate.DayOfWeek.ToString() + " as lsday from hrm_mst_tweekoffemployee where employee_gid='" + values.employeegid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            lsday = dt["lsday"].ToString();
                            if (lsday == "Non-working Day")
                            {
                                lsWeekOff_flag = "Y";
                                lstweekoff = lsday;
                            }
                            else
                            {
                                lsWeekOff_flag = "N";
                            }
                        }
                    }
                    lsholiday = "N";
                    lstholiday = "";
                    msSQL = " select date(a.holiday_date) as holiday from hrm_mst_tholiday a " +
                     " inner join hrm_mst_tholiday2employee b on a.holiday_gid=b.holiday_gid " +
                     " where b.employee_gid='" + values.employeegid + "' and a.holiday_date='" + lsleave + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsholiday = "Y";
                    }
                    else
                    {
                        lsholiday = "N";
                    }
                    msSQL = " select weekoff_applicable,holiday_applicable from hrm_mst_tleavetype where leavetype_gid='" + values.leavetype_gid + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lsweekoff_applicable = objOdbcDataReader["weekoff_applicable"].ToString();
                        lsholiday_applicable = objOdbcDataReader["holiday_applicable"].ToString();
                    }
                    msGetleavedtlGid = objcmnfunctions.GetMasterGID("HLVC");

                    if (j == compcount && leavetype_gid == "LT1203060069")
                    {
                        leavetype_gid = "LT1203060069";
                        msSQL = " update hrm_trn_tcompensatoryoffdtl set " +
                                " leave_date = '" + lsleave + "'," +
                                " leavedtl_gid='" + msGetleavedtlGid + "'" +
                                " where employee_gid='" + values.employeegid + "' " +
                                " and leave_status='not taken' and leave_date is null limit 1 ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (leavetype_gid == "LT1203060069")
                        {
                            leavetype_gid = "LT1203060066";
                        }
                        else
                        {
                            leavetype_gid = "";
                        }
                        lscount = "1";
                        leave_session = "NA";
                        if (values.days_name == "H")
                        {
                            if (values.days_name == "F")
                            {
                                string halfDayValue = "N";
                                lscount = "1";
                            }
                            else
                            {
                                string halfDayValue = "Y";
                                leave_session = "";
                                lscount = "0.5";
                            }
                        }
                        else
                        {
                            List<string> listOfhalfDays = new List<string> { "Y", "N", "Y", "N", "Y" };
                            string halfDayValue = listOfhalfDays[j];
                            if (j >= 0 && j < listOfhalfDays.Count)
                            {
                                half_session = "Nill";
                            }
                            else
                            {
                                List<string> listOfhalfsession = new List<string> { "Y", "N", "Y", "N", "Y" };
                                half_session = listOfhalfsession[j];



                                if (halfDayValue == "Y")
                                {
                                    lscount = "0.5";
                                    leave_session = "F";
                                }
                                else if (half_session == "FN")
                                {
                                    lscount = "0.5";
                                    leave_session = "F";
                                }
                                else if (halfDayValue == "Y")
                                {
                                    lscount = "0.5";
                                    leave_session = "AL";
                                }
                                else if (half_session == "AN")
                                {
                                    lscount = "0.5";
                                    leave_session = "AL";
                                }
                                else
                                {
                                    lscount = "1";
                                    leave_session = "NA";
                                }
                            }
                           
                        }

                    }
                    else
                    {
                        lscount = "1";
                        leave_session = "NA";
                    }

                    if ((lsday == "Non-working Day" && lsweekoff_applicable == "Y") || (lsholiday == "Y" && lsholiday_applicable == "Y"))
                    {
                        msSQL1 += " ('" + msGetleavedtlGid + "'," +
                                  " '" + msGetGid + "'," +
                                  " '" + leavetype_gid + "'," +
                                  " '" + lsleave + "'," +
                                  " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                  " '" + user_gid + "'," +
                                  " '" + lsWeekOff_flag + "'," +
                                  " '" + lsholiday + "'," +
                                  " '" + lscount + "'," +
                                  " 'Approved'," +
                                  " '" + values.days_name + "'," +
                                  " '" + values.session_leave + "'";
                        if (lslop != "")
                        {
                            msSQL1 += ",'Y'),";
                        }
                        else
                        {
                            msSQL1 += ",'N'),";
                        }
                    }
                    else
                    {
                        if (lsday != "Non-working Day" && lsholiday == "N")
                        {
                            msSQL1 += " ('" + msGetleavedtlGid + "'," +
                                    " '" + msGetGid + "'," +
                                    " '" + values.leavetype_gid + "'," +
                                    " '" + lsleave + "'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    " '" + user_gid + "'," +
                                    " '" + lsWeekOff_flag + "'," +
                                    " '" + lsholiday + "'," +
                                    " '" + lscount + "'," +
                                    $" 'Approved'," +
                                    " '" + values.days_name + "'," +
                                    " '" + values.session_leave + "'";
                            if (!string.IsNullOrEmpty(lslop))
                            {
                                msSQL1 += ",'Y'),";
                            }
                            else
                            {
                                msSQL1 += ",'N'),";
                            }
                        }
                        else
                        {
                            lsnorecorcd = "Y";
                        }
                    }
                    lsleavefromdate = lsleavefromdate.AddDays(1);
                    
                    if (mnResult2 == 0)
                    {
                        if (lsnorecorcd == "Y")
                        {
                            values.message = "There is no Working days for applied days.";

                        }
                    }

                    if (leavetype_gid == "Compensatory off")
                    {
                        msSQL = "select compensatoryleave_count from hrm_trn_tleavecredits where employee_gid='" + values.employeegid + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            objOdbcDataReader.Read();
                            if (Convert.IsDBNull(objOdbcDataReader["compensatoryleave_count"]) == true)
                            {
                                lscompoff_leavecount = "0";
                            }
                            else if (Convert.ToString(objOdbcDataReader["compensatoryleave_count"]) == "")
                            {
                                lscompoff_leavecount = "0";
                            }
                            else
                            {
                                lscompoff_leavecount = Convert.ToString(objOdbcDataReader["compensatoryleave_count"]);
                            }
                        }
                    }

                    string lscomoffcount = (Convert.ToDouble(lscompoff_leavecount) - Convert.ToDouble(lsnoworking)).ToString();
                    msSQL = "update hrm_trn_tcompensatoryoff set " +
                            "compensatoryoff_remarks ='Compoff Taken' " +
                            "where compensatoryoff_gid = '" + lscompensatoryoff_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                msSQL1 = msSQL1.Substring(0, msSQL1.Length - 1);
                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL1);

                 



                msSQL = " insert into hrm_trn_tleave " +
                           " ( leave_gid,  " +
                           " employee_gid , " +
                           " leavetype_gid  , " +
                           " leave_fromdate, " +
                           " leave_todate , " +
                           " leave_noofdays , " +
                           " leave_reason, " +
                           " leave_status ," +
                           " created_by, " +
                           " created_date) " +
                           " values ( " +
                           " '" + msGetGid + "', " +
                           " '" + values.employeegid + "', " +
                           " '" + values.leavetype_gid + "', " +
                           " '" + values.leave_datefrom.ToString("yyyy-MM-dd") + "'," +
                           " '" + values.leave_dateto.ToString("yyyy-MM-dd") + "', " +
                           " '" + values.leave_days + "'," +
                           " '" + values.reason + "'," +
                           " 'Approved'," +
                           "'" + employee_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Leave Applied Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Applying Leave";
                }


                if (mnResult == 1)
                {

                    lsdate = Convert.ToDateTime(lsleavefromdate).ToString("yyyy-MM-dd");
                    for (int j = 0; j <= daysDifference; j++)
                    {
                        lsleave = Convert.ToDateTime(lsdate).ToString("yyyy-MM-dd");
                        msSQL = "select b.half_session from hrm_trn_tleave a " +
                                " inner join hrm_trn_tleavedtl b on a.leave_gid=b.leave_gid " +
                                " inner join hrm_mst_tleavetype c on a.leavetype_gid=c.leavetype_gid " +
                                " where a.employee_gid='" + values.employeegid + "' and b.leavedtl_date='" + lsleave + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            leave_session = objOdbcDataReader["half_session"].ToString();
                        }

                        else
                        {
                            leave_session = "NA";
                        }


                    }
                }
                msSQL = " select b.half_session from hrm_trn_tleave a " +
                              " inner join hrm_trn_tleavedtl b on a.leave_gid=b.leave_gid " +
                              " inner join hrm_mst_tleavetype c on a.leavetype_gid=c.leavetype_gid " +
                              " where a.employee_gid='" + values.employeegid + "' and b.leavedtl_date='" + lsleave + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    msSQL = " select attendance_date from hrm_trn_tattendance where attendance_date='" + lsleave + "'" +
                            " and employee_gid='" + values.employeegid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msSQL = " update hrm_trn_tattendance set employee_attendance='Leave', " +
                                            " attendance_type= '" + lstype + "', " +
                                            " day_session='" + leave_session + "', " +
                                            " update_flag='N' " +
                                            " where  employee_gid = '" + values.employeegid + "' " +
                                            " and attendance_date='" + lsleave + "'";
                            mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                    else
                    {
                        msGetGid2 = objcmnfunctions.GetMasterGID("HATP");
                        msSQL = " insert into hrm_trn_tattendance" +
                                " ( attendance_gid, " +
                                " employee_gid," +
                                " attendance_date," +
                                " shift_date," +
                                " employee_attendance," +
                                " attendance_type, " +
                                " created_by," +
                                " day_session," +
                                " created_date )" +
                                " Values (" +
                                " '" + msGetGid2 + "', " +
                                " '" + values.employeegid + "', " +
                                " '" + lsleave + "', " +
                                " '" + lsleave + "', " +
                                " 'Leave'," +
                                " '" + lstype + "', " +
                                "'" + employee_gid + "'," +
                                " '" + leave_session + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult2 != 0)
                        {
                            values.status = true;
                            values.message = "Leave Details Applied Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Applying Leave Details";
                        }
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

        public void DaPermissionSubmit(string employee_gid, permissionname_list values)
        {
            try
            {
               
                msSQL = " select date_format(permission_date,'%Y-%m-%d') as permission_date from " +
                        " hrm_trn_tpermission " +
                        " where employee_gid='" + values.employeenamegid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsalreadyPermission_Date = objOdbcDataReader["permission_date"].ToString();
                    if (lsalreadyPermission_Date == values.permission_date)
                    {

                        values.message = "Permission has already been applied on this date";
                        return;

                    }


                }

                msSQL = " select a.holiday_date as holiday from hrm_mst_tholiday a " +
                        " inner join hrm_mst_tholiday2employee b on a.holiday_gid=b.holiday_gid " +
                        " where b.employee_gid='" + values.employeenamegid + "' and a.holiday_date='" + values.permission_date + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    values.message = "It is a holiday,You can't apply Permission on that day";
                    return;
                }

                    msGetGid = objcmnfunctions.GetMasterGID("HPNP");
                    if (msGetGid == "E")
                    {
                        values.message = "Error While Generating Gid";
                    }

               

                    msSQL = " Insert into hrm_trn_tpermission" +
                                " (permission_gid, " +
                                " employee_gid," +
                                " permission_date," +
                                " permission_totalhours, " +
                                " permission_reason," +
                                " permission_fromhours, " +
                                " permission_tohours, " +
                                " permission_status," +
                                " created_by," +
                                " created_date" +
                                " ) Values  " +
                                " ('" + msGetGid + "', " +
                                " '" + values.employeenamegid + "'," +
                                " '" + values.permission_date + "', " +
                                " '" + values.total_duration + "', " +
                                " '" + values.reason_permission + "'," +
                                " '" + values.from_hrs + "', " +
                                "'" + values.to_hrs + "', " +
                                " 'Approved'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Permission Applied Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Applying Permission";
                    }
                
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaOnDutySubmit(string employee_gid, ondutyname_list values)
        {
            try
            {
                msSQL = " select date_format(ondutytracker_date,'%Y-%m-%d') as ondutytracker_date from " +
                       " hrm_trn_tondutytracker " +
                       " where employee_gid='" + values.employeedetailgid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsalreadyOnDuty_Date = objOdbcDataReader["ondutytracker_date"].ToString();
                    if (lsalreadyOnDuty_Date == values.onduty_date)
                    {

                        values.message = "OnDuty has already been applied on this date";
                        return;

                    }


                }

                CheckBox onduty_period = new CheckBox();
                if (onduty_period.Checked)
                {
                    lshalfday = "Y";
                    lshalfsession = "";
                }
                else
                {
                    lshalfday = "N";
                }

                if (lshalfday == "Y")
                {
                    if (lshalfsession == "DX")
                    {
                        lstype = "DX";
                    }
                    else if (lshalfsession == "XD")
                    {
                        lstype = "XD";
                    }
                }
                else
                {
                    lstype = "OD";
                }

                if (!onduty_period.Checked)
                {
                    half_day = "N";
                    lscount = "1";
                }
                else
                {
                    half_day = "Y";
                    lscount = "0.5";
                }

                msGetGid = objcmnfunctions.GetMasterGID("HODP");
                if (msGetGid == "E")
                {
                    values.message = "Error While Generating Gid";
                }

                msSQL = " Insert into hrm_trn_tondutytracker" +
                            " (ondutytracker_gid, " +
                            " employee_gid," +
                            " onduty_fromtime," +
                            " onduty_totime, " +
                            " onduty_duration," +
                            " onduty_reason," +
                            " ondutytracker_date , " +
                            " ondutytracker_status," +
                            " half_day, " +
                            " half_session, " +
                            " onduty_count, " +
                            " created_by," +
                            " created_date)" +
                            "  Values  " +
                            " ('" + msGetGid + "', " +
                            " '" + values.employeedetailgid + "'," +
                            "'" + values.from_hrsod + "', " +
                            "'" + values.to_hrsod + " '," +
                            "'" + values.total_durationod + "', " +
                            "'" + values.reason_onduty + " '," +
                            "'" + values.onduty_date + "', " +
                            "'Approved'," +
                            "'" + lshalfday + "', " +
                            "'" + lstype + "', " +
                            "'" + lscount + "', " +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "OnDuty Applied Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Applying OnDuty";
                }


            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaPermissionImport(HttpRequest httpRequest, string employee_gid, result objResult)
        {
            string lscompany_code, msdocument_gid = "";
            string excelRange, endRange, lstotalshifthours, lshalfdaymaxhours, lshalfdayminhours, lsortminhours, lsotmaxhours;
            int rowCount, columnCount, importcount = 0;

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

                    msSQL = " insert into hrm_trn_temployeeuploadexcellog(" +
                            " uploadexcellog_gid," +
                            " fileextenssion," +
                            " uploaded_by, " +
                            " uploaded_date)" +
                            " values(" +
                            " '" + msdocument_gid + "'," +
                            " '" + FileExtension + "'," +
                            " '" + employee_gid + "'," +
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
                                command.CommandText = "select * from [Permission$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                              
                                if (dataTable.Rows.Count == 0)
                                {
                                    objResult.message = "No data found ";
                                    objResult.status = false;
                                    return;
                                }

                               
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    string EmpId = row["EmpId"].ToString();
                                    string From = row["From(dd-mm-yyyy)"].ToString();
                                    string FromTime = row["FromTime(HH:mm)"].ToString();
                                    string ToTime = row["ToTime(HH:mm)"].ToString();
                                    string TotalTime = row["TotalTime"].ToString();
                                    string Reason = row["Reason"].ToString();
                                    msSQL = "select b.employee_gid from adm_mst_Tuser a left join hrm_mst_temployee b on a.user_gid=b.user_gid where a.user_code='" + EmpId + "'";
                                    string lsemployeegid = objdbconn.GetExecuteScalar(msSQL);

                                    msGetGid = objcmnfunctions.GetMasterGID("HPNP");

                                    if (msGetGid == "E")
                                    {
                                        objResult.status = false;
                                        objResult.message = "Sequence code not generated for Code";
                                        return;

                                    }
                                    msSQL = " select b.employee_gid,a.user_code from adm_mst_tuser a " +
                                            " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                                            " where a.user_code='" + EmpId + "' ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        objOdbcDataReader.Read();
                                        employee_gid = objOdbcDataReader["employee_gid"].ToString();
                                        emp_code = objOdbcDataReader["user_code"].ToString();


                                    }

                               
                                    msSQL = " Insert into hrm_trn_tpermission" +
                                            " (permission_gid, " +
                                            " employee_gid," +
                                            " permission_date," +
                                            " permission_totalhours, " +
                                            " permission_reason," +
                                            " permission_fromhours, " +
                                            " permission_tohours, " +
                                            " permission_status," +
                                            " created_by," +
                                            " created_date" +
                                            " ) Values  " +
                                            " ('" + msGetGid + "', " +
                                            " '" + lsemployeegid + "'," +
                                            " '" + Convert.ToDateTime(From).ToString("yyyy-MM-dd") + "', " +
                                            " '" + TotalTime + "', " +
                                            " '" + Reason + "'," +
                                            " '" + FromTime + "', " +
                                            "'" + ToTime + "', " +
                                            " 'Approved'," +
                                            "'" + employee_gid + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        importcount++;
                                    }
                                }
                                if (mnResult != 0)
                                {
                                    objResult.status = true;
                                    objResult.message = "Permission Added " + importcount + " Out of " + dataTable.Rows.Count + " Successfully";
                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Error While Adding Permission";
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Permission";
                    return;
                }
            }

            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void DaLeaveImport(HttpRequest httpRequest, string employee_gid, result objResult)
        {
            string lscompany_code, msdocument_gid = "";
            string excelRange, endRange, lstotalshifthours, lshalfdaymaxhours, lshalfdayminhours, lsortminhours, lsotmaxhours;
            int rowCount, columnCount, importcount = 0;

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

                    msSQL = " insert into hrm_trn_temployeeuploadexcellog(" +
                            " uploadexcellog_gid," +
                            " fileextenssion," +
                            " uploaded_by, " +
                            " uploaded_date)" +
                            " values(" +
                            " '" + msdocument_gid + "'," +
                            " '" + FileExtension + "'," +
                            " '" + employee_gid + "'," +
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
                                command.CommandText = "select * from [Leave$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                if (dataTable.Rows.Count == 0)
                                {
                                    objResult.message = "No data found ";
                                    objResult.status = false;
                                    return;
                                }

                               
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    string EmpId = row["EmpId"].ToString();
                                    string LeaveType = row["LeaveType(code)"].ToString();
                                    string FromDate = row["FromDate(MM/DD/YYYY)"].ToString();
                                    string ToDate = row["ToDate(MM/DD/YYYY)"].ToString();
                                    string Reason = row["Reason"].ToString();
                                    msSQL = "select b.employee_gid from adm_mst_Tuser a left join hrm_mst_temployee b on a.user_gid=b.user_gid where a.user_code='" + EmpId + "'";
                                    string lsemployeegid = objdbconn.GetExecuteScalar(msSQL);


                                    msGetGid = objcmnfunctions.GetMasterGID("HLVP");

                                    if (msGetGid == "E")
                                    {
                                        objResult.status = false;
                                        objResult.message = "Sequence code not generated for Code";
                                        return;

                                    }

                                    msSQL = " select b.employee_gid,a.user_code from adm_mst_tuser a " +
                                          " inner join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                                          " where a.user_code='" + EmpId + "' ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        objOdbcDataReader.Read();
                                        employee_gid = objOdbcDataReader["employee_gid"].ToString();
                                        emp_code = objOdbcDataReader["user_code"].ToString();


                                    }

                                    msSQL = " insert into hrm_trn_tleave " +
                                            " ( leave_gid,  " +
                                            " employee_gid , " +
                                            " leavetype_gid  , " +
                                            " leave_fromdate, " +
                                            " leave_todate , " +
                                            " leave_reason, " +
                                            " created_by, " +
                                            " created_date) " +
                                            " values ( " +
                                            " '" + msGetGid + "', " +
                                            " '" + lsemployeegid + "', " +
                                            " '" + LeaveType + "', " +
                                            " '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "', " +
                                            " '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "', " +
                                            " '" + Reason + "'," +
                                            " '" + employee_gid + "'," +
                                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        importcount++;
                                    }
                                }
                                if (mnResult != 0)
                                {
                                    objResult.status = true;
                                    objResult.message = "Leave Added " + importcount + " Out of " + dataTable.Rows.Count + " Successfully";
                                }
                                else
                                {
                                    objResult.status = false;
                                    objResult.message = "Error While Adding Leave";
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = "Error While Adding Leave";
                    return;
                }
            }

            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

    }
}
