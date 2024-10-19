using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;


using global::ems.utilities.Functions;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web.Http.Results;

namespace ems.system.DataAccess
{
    public class DaSysRptAuditReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        HttpPostedFile httpPostedFile;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsdepartment_code, lsdepartment_name, lsdepartment_prefix, lsdepartment_name_edit;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetAuditReportSummary(MdlSysRptAuditReport values)
        {
            try
            {
                msSQL = " select audit_gid, session_id, d.user_gid, ipaddress, date_format(login_time,'%d-%m-%Y %h:%i:%s %p') as login, user_name, password, companycode,b.department_name," +
                        " date_format(logout_time,'%d-%m-%Y %h:%i:%s %p') as logout, CONCAT(d.user_code,' / ',d.user_firstname,' ', d.user_lastname) AS full_name, last_click  from ( select audit_gid, session_id," +
                        " ipaddress, login_time, UPPER(user_name) as user_name, password, companycode, logout_time," +
                        " last_click from acp_mst_taudit  order by audit_gid desc) a" +
                        " left join adm_mst_tuser d on  user_name = d.user_code " +
                        " left join hrm_mst_temployee c on d.user_gid= c.user_gid " +
                        " left join hrm_mst_tdepartment b on c.department_gid = b.department_gid " +
                        " group by user_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<audit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new audit_list
                        {
                            audit_gid = dt["audit_gid"].ToString(),
                            session_id = dt["session_id"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            ipaddress = dt["ipaddress"].ToString(),
                            login = dt["login"].ToString(),
                            password = dt["password"].ToString(),
                            companycode = dt["companycode"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            logout = dt["logout"].ToString(),
                            full_name = dt["full_name"].ToString()

                        });
                        values.auditreport_list = getModuleList;
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

        public void DaGetAuditReportHistorySummary(string user_gid, MdlSysRptAuditReport values)
        {
            try
            {
                msSQL = " select audit_gid,session_id,ipaddress,date_format(login_time,'%d-%m-%Y %h:%i:%s %p') as login,user_name, " +
                        " companycode,password,date_format(logout_time,'%d-%m-%Y %h:%i:%s %p') as logout,CONCAT(b.user_code,' / ',b.user_firstname,' ', b.user_lastname) AS full_name, d.department_name " +
                        " from acp_mst_taudit a " +
                        " left join adm_mst_tuser b on  a.user_name = b.user_code " +
                        " left join hrm_mst_temployee c on b.user_gid= c.user_gid " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        " where b.user_gid='" + user_gid + "' " +
                        " order by audit_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<audithistory_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new audithistory_list
                        {
                            audit_gid = dt["audit_gid"].ToString(),
                            session_id = dt["session_id"].ToString(),
                            ipaddress = dt["ipaddress"].ToString(),
                            login = dt["login"].ToString(),
                            password = dt["password"].ToString(),
                            companycode = dt["companycode"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            logout = dt["logout"].ToString(),
                            full_name = dt["full_name"].ToString()
                        });
                        values.audithistoryreport_list = getModuleList;
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
    }
}

