﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using MySql.Data.MySqlClient;


namespace ems.hrm.DataAccess
{
    public class DaProbationperiod
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, msGETGid, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetProbationperiodSummary(MdlProbationperiod values)
        {
            try
            {
                
                {
                    msSQL = " Select distinct a.user_gid,c.useraccess,f.probation_period,m.probation_status,date_format(m.probationary_until,'%d-%m-%Y') as probationary_until," +
                    " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,date_format(c.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate,l.jobtype_name,c.probation_flag," +
                    " c.employee_gender," + " d.designation_name,c.designation_gid,c.employee_gid,e.branch_name," +
                    " CASE" +
                    " WHEN a.user_status = 'Y' THEN 'Active'" +
                    " WHEN a.user_status = 'N' THEN 'Inactive'" +
                    " END as user_status," +
                    " c.department_gid,c.branch_gid, e.branch_name, g.department_name" +
                    " FROM adm_mst_tuser a" +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid" +
                    " left join hrm_mst_trole f on f.role_gid = c.role_gid " +
                    " left join hrm_trn_probationperiod m on c.employee_gid=m.employee_gid" +
                    " left join hrm_mst_tjobtype l on c.jobtype_gid=l.jobtype_gid" +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid" +
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid" +
                    " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid" +
                    " where m.probation_status='Yes' and m.probationary_until > '"+DateTime.Now.ToString("yyyy-MM-dd")+"' " +
                    " group by c.employee_gid" + " order by c.employee_gid desc";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<employee_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new employee_list
                            {

                                branch_name = dt["branch_name"].ToString(),
                                department_name = dt["department_name"].ToString(),
                                designation_name = dt["designation_name"].ToString(),
                                user_code = dt["user_code"].ToString(),
                                user_name = dt["user_name"].ToString(),
                                employee_gender = dt["employee_gender"].ToString(),
                                employee_joiningdate = dt["employee_joiningdate"].ToString(),
                                probationary_until = dt["probationary_until"].ToString(),
                                probation_status = dt["probation_status"].ToString(),
                                designation_gid = dt["designation_gid"].ToString(),
                                employee_gid = dt["employee_gid"].ToString(),
                                department_gid = dt["department_gid"].ToString(),
                                branch_gid = dt["branch_gid"].ToString(),
                                probation_period = dt["probation_period"].ToString(),

                            });
                            values.employee_list = getModuleList;
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

        public void DaGetProbationhistorySummary(MdlProbationperiod values, string employee_gid)
        {
            try
            {
                
                {
                    msSQL = " Select distinct a.user_gid,m.probation_status,m.department_assginment,m.activity,m.job_training,m.e_training,m.key_eveluation," +
                            " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,m.employee_joiningdate,l.jobtype_name,m.probationary_until,date_format(m.extend_date,'%d-%M-%Y') as extend," +
                            "  d.designation_name, m.designation_gid, m.employee_gid, e.branch_name," +
                            " m.department_gid, m.branch_gid, e.branch_name, g.department_name" +
                            " FROM hrm_trn_probationperiod m" +
                            " left join adm_mst_tuser a on m.user_gid=a.user_gid" +
                            " left join hrm_mst_tjobtype l on m.jobtype_gid=l.jobtype_gid" +
                            " left join adm_mst_tdesignation d on m.designation_gid = d.designation_gid" +
                            " left join hrm_mst_tbranch e on m.branch_gid = e.branch_gid" +
                            " left join hrm_mst_tdepartment g on g.department_gid = m.department_gid" +
                            " WHERE m.employee_gid = '" + employee_gid + "'" +
                            " group by m.employee_gid" +
                            " order by m.employee_gid asc";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<employee_list1>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new employee_list1
                            {

                                extend = dt["extend"].ToString(),
                                probation_status = dt["probation_status"].ToString(),
                                department_assginment = dt["department_assginment"].ToString(),
                                activity = dt["activity"].ToString(),
                                job_training = dt["job_training"].ToString(),
                                e_training = dt["e_training"].ToString(),
                                key_eveluation = dt["key_eveluation"].ToString(),
                                user_gid = dt["user_gid"].ToString(),
                                designation_gid = dt["designation_gid"].ToString(),
                                employee_gid = dt["employee_gid"].ToString(),
                                department_gid = dt["department_gid"].ToString(),
                                branch_gid = dt["branch_gid"].ToString()

                            });
                            values.employee_list1 = getModuleList;
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

        public void DaGetleavegradedropdown(MdlProbationperiod values)
        {
            try
            {
                
                msSQL = " select leavegrade_gid,leavegrade_name " +
                    "from hrm_mst_tleavegrade ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getleavegradedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getleavegradedropdown
                        {
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),
                        });
                        values.Getleavegradedropdown = getModuleList;
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

        public void DaGetjobtypedropdown(MdlProbationperiod values)
        {
            try
            {
                
                msSQL = " select jobtype_gid,jobtype_name " +
                   "from hrm_mst_tjobtype ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getjobtypedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getjobtypedropdown
                        {
                            jobtype_gid = dt["jobtype_gid"].ToString(),
                            jobtype_name = dt["jobtype_name"].ToString(),
                        });
                        values.Getjobtypedropdown = getModuleList;
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

        public void DaGetleavegradeSummary(MdlProbationperiod values, string leavegrade_gid)
        {
            try
            {
                
                msSQL = " select a.leavetype_gid,c.leavetype_name,a.total_leavecount,a.available_leavecount,a.leave_limit,b.leavegrade_gid from hrm_mst_tleavegradedtl a " +
               " left join hrm_mst_tleavegrade b on a.leavegrade_gid=b.leavegrade_gid " +
               " left join hrm_mst_tleavetype c on a.leavetype_gid=c.leavetype_gid " +
               " where a.leavegrade_gid='" + leavegrade_gid + "' and a.active_flag='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavegrade_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavegrade_list
                        {
                            leavetype_name = dt["leavetype_name"].ToString(),
                            total_leavecount = dt["total_leavecount"].ToString(),
                            available_leavecount = dt["available_leavecount"].ToString(),
                            leave_limit = dt["leave_limit"].ToString(),
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),

                        });
                        values.leavegrade_list = getModuleList;
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
    }
}

