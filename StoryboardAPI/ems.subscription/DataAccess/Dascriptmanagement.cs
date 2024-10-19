using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using ems.subscription.Models;
using System.Data;
using System.Web;
using System;
using System.Web.Http;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using Stripe.Forwarding;

namespace ems.subscription.DataAccess
{
    public class Dascriptmanagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objODBCdatareader, objOdbcDataReader;
        DataTable dt_datatable;

        int mnresult;
        string msGetGid;
        public void DaGetdatabasenamedropdown(Mdlscriptmanagement values)
        {
            try
            {
                msSQL = "select company_code,server_gid from adm_mst_tconsumer";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {
                            company_code = dt["company_code"].ToString(),
                            server_gid = dt["server_gid"].ToString(),
                        });
                        values.serverlists = getModuleList;

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
        public void DaGetserverdropdown(Mdlscriptmanagement values)
        {
            try
            {

                msSQL = "select server_name,server_gid from adm_mst_tserver";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {
                            server_name = dt["server_name"].ToString(),
                            server_gid = dt["server_gid"].ToString(),
                        });
                        values.serverlists = getModuleList;

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
        public void DaGetScriptSummary(Mdlscriptmanagement values)
       {
            try
            {
                msSQL = "select a.dbscriptmanagementdocument_gid,a.company_code,a.file_name,a.file_path,a.server_name,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tdbscriptmanagementdocument a " +
                    " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid " ;
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {
                            company_code = dt["company_code"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            file_name = dt["file_name"].ToString(),
                            dbscriptmanagementdocument_gid = dt["dbscriptmanagementdocument_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.serverlists = getModuleList;
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

        public void DaGetScriptViewSummary(Mdlscriptmanagement values, string dbscriptmanagementdocument_gid)
        {
            try
            {
                msSQL = "select a.company_code,a.server_name,a.script_name,a.execute_query,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tdbscriptmanagement a " +
                     " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                    " where dbscriptmanagementdocument_gid='" + dbscriptmanagementdocument_gid + "'";
                                
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {

                            company_code = dt["company_code"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            script_name = dt["script_name"].ToString(),
                            execute_query = dt["execute_query"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.serverlists = getModuleList;
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
        public void DaGetScriptexceptionViewSummary(Mdlscriptmanagement values, string dbscriptmanagementdocument_gid)
        {
            try
            {
                msSQL = "select a.company_code,a.server_name,a.script_name,a.execute_query,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by from sub_mst_tdbscripterrormanagement a " +                
                    " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    "  left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                    " where dbscriptmanagementdocument_gid='" + dbscriptmanagementdocument_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {

                            company_code = dt["company_code"].ToString(),
                            server_name = dt["server_name"].ToString(),
                            script_name = dt["script_name"].ToString(),
                            execute_query = dt["execute_query"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),

                        });
                        values.serverlists = getModuleList;
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
        public void DaGetserverdatabasenamedropdown(string server_gid,Mdlscriptmanagement values)
        {
            try
            {
                msSQL = "select company_code,server_gid from adm_mst_tconsumer where server_gid='" + server_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<serverlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new serverlists
                        {
                            company_code = dt["company_code"].ToString(),
                            server_gid = dt["server_gid"].ToString(),
                        });
                        values.serverlists = getModuleList;

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