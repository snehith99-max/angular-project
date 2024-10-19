using ems.task.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;

namespace ems.task.DataAccess
{
    public class DaTskTrnTaskManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objodbcDataReader;
        string lsmaster_value, lschecklist_name, lsdocumentgid, lspath;
        OdbcDataReader objODBCDatareader;
        Dictionary<string, object> objGetReaderScalar;
        string msGetGid, msGetGid1, msGetcadteamcode, msGetCode, lsemployee_status;
        DataTable dt_datatable;
        int mnResult, mnResult1, mnResult2;
        public bool Dataskadd(taskaddlist values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("TMCG");
                msGetCode = objcmnfunctions.GetMasterGID("TMCC");
                msGetCode = "TMCC" + msGetCode;
                msSQL = " insert into tsk_mst_ttaskmanagement  (" +
                        " task_gid," +
                        " task_code," +
                        " task_name," +
                        " module_name," +
                        " module_gid ," +
                        " module_name_gid ," +
                        " severity_name," +
                        " severity_gid," +
                        " task_typename," +
                        " task_typegid," +
                        " estimated_hours," +
                        " task_status," +
                        " completed_flag, " +
                        " remarks," +
                        " functionality_name," +
                         " functionality_gid," +
                        " created_by," +
                        " created_date)" +
                        " VALUES(" +
                        "'" + msGetGid + "'," +
                        "'" + msGetCode + "'," +
                        "'" + values.task_name.Replace("'", "''") + "'," +
                        "'" + values.module_name.Replace("'", "''") + "'," +
                        "'" + values.module_gid + "'," +
                        "'" + values.module_name_gid + "'," +
                        "'" + values.severity_name.Replace("'", "''") + "'," +
                        "'" + values.severity_gid + "'," +
                        "'" + values.task_typename.Replace("'", "''") + "'," +
                        "'" + values.task_typegid + "'," +
                        "'" + values.estimated_hours.Replace("'", "''") + "'," +
                        "'Pending'," +
                         "'N'," +
                        "'" + values.remark.Replace("'", "''") + "'," +
                        "'" + values.functionality_name.Replace("'", "''") + "'," +
                        "'" + values.functionality_gid + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Task Added Successfully";
                    values.status = true;
                    values.task_gid = msGetGid;
                }
                else
                {
                    values.message = "Error While Adding Task";
                    values.status = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public bool Dashowstoppersummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " SELECT (SELECT COUNT(project_name) FROM tsk_mst_ttask2client AS t2c WHERE t2c.task_gid = a.task_gid)AS client_count,a.module_gid,a.module_gid,a.employee_status,a.task_gid,a.task_code,a.module_name,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.severity_name='Show Stopper' and a.employee_status<>'Completed In Live' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.show_stopper = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            client_count = dt["client_count"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Dacompletedsummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " SELECT a.module_gid,a.module_gid,a.employee_status,a.task_gid,a.task_code,a.module_name,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.completedlive_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.completedlive_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.employee_status='Completed In Live' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.completed = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Damandatorysummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " SELECT (SELECT COUNT(project_name) FROM tsk_mst_ttask2client AS t2c WHERE t2c.task_gid = a.task_gid)AS client_count,a.module_gid,a.employee_status,a.task_gid,a.task_code,a.module_name,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.severity_name='Critical Mandatory' and a.employee_status<>'Completed In Live' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.mandatory = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            client_count = dt["client_count"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Danonmandatorysummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " SELECT (SELECT COUNT(project_name) FROM tsk_mst_ttask2client AS t2c WHERE t2c.task_gid = a.task_gid)AS client_count,a.module_gid,a.employee_status,a.task_gid,a.task_code,a.module_name,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.severity_name='Critical Non-Mandatory' and a.employee_status<>'Completed In Live' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.non_mandatory = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            client_count = dt["client_count"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Danicetohavesummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " SELECT (SELECT COUNT(project_name) FROM tsk_mst_ttask2client AS t2c WHERE t2c.task_gid = a.task_gid)AS client_count,a.module_gid,a.employee_status,a.task_gid,a.task_code,a.module_name,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.severity_name='Nice To Have' and a.employee_status<>'Completed In Live'" +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.nice_to_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            client_count = dt["client_count"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public void Datasklist(taskteamlist values)
        {
            try
            {
                msSQL = " select task_name from tsk_mst_ttaskmanagement  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getinactivehistory_list = new List<taskdetail_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getinactivehistory_list.Add(new taskdetail_list
                        {
                            task_name = dr_datarow["task_name"].ToString(),
                        });
                    }
                    values.taskdetail_list = getinactivehistory_list;
                }

                values.status = true;
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Dacheck(string taskname, taskaddlist values)
        {
            try
            {
                msSQL = "select task_gid from tsk_mst_ttaskmanagement" +
                    " where  task_name='" + taskname.Replace("'", "''") + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.status = false;
                    return;
                }
                values.status = true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                              "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;

            }
        }
        public void Dataskbind(string taskname, taskaddlist values)
        {
            try
            {
                msSQL = "select task_gid from tsk_mst_ttaskmanagement where task_name ='" + taskname.Replace("'", "''") + "'";
                values.task_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select functionality_gid,functionality_name,hold_flag,employee_status,remarks,task_gid,task_name,task_code,module_name,severity_name,task_typename,task_typegid," +
                    "module_gid,severity_gid,estimated_hours,task_status from tsk_mst_ttaskmanagement" +
                        " where task_gid='" + values.task_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.task_gid = objODBCDatareader["task_gid"].ToString();
                    values.task_name = objODBCDatareader["task_name"].ToString();
                    values.task_code = objODBCDatareader["task_code"].ToString();
                    values.remarks = objODBCDatareader["remarks"].ToString();
                    values.task_status = objODBCDatareader["task_status"].ToString();
                    values.estimated_hours = objODBCDatareader["estimated_hours"].ToString();
                    values.functionality_name = objODBCDatareader["functionality_name"].ToString();
                    values.task_typegid = objODBCDatareader["task_typegid"].ToString();
                    values.hold_flag = objODBCDatareader["hold_flag"].ToString();
                    values.severity_gid = objODBCDatareader["severity_gid"].ToString();
                    values.module_gid = objODBCDatareader["module_gid"].ToString();
                    //values.client_gid = objODBCDatareader["client_gid"].ToString();
                    values.task_typename = objODBCDatareader["task_typename"].ToString();
                    //values.client_name = objODBCDatareader["client_name"].ToString();
                    values.severity_name = objODBCDatareader["severity_name"].ToString();
                    values.functionality_gid = objODBCDatareader["functionality_gid"].ToString();
                    values.module_name = objODBCDatareader["module_name"].ToString();
                    values.employee_status = objODBCDatareader["employee_status"].ToString();

                }
                msSQL = " select docupload_type,docupload_name,file_path,taskdocumentupload_gid from tsk_mst_ttaskmanagementdocument " +
                        " where task_gid='" + values.task_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getverticalsList = new List<documentdata_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getverticalsList.Add(new documentdata_list
                        {
                            document_name = (dr_datarow["docupload_name"].ToString()),
                            taskdocumentupload_gid = (dr_datarow["taskdocumentupload_gid"].ToString()),
                            file_name = (dr_datarow["docupload_type"].ToString()),

                            file_path = (dr_datarow["file_path"].ToString()),
                            //file_path = objcmnstorage.EncryptData(dr_datarow["file_path"].ToString()),
                        });

                        values.documentdata_list = getverticalsList;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Dataskedit(string task_gid, taskaddlist values)
        {
            try
            {
                msSQL = " select retesting_flag,actualcompleted_hrs,actualdevelopment_hrs,functionality_name,hold_flag,employee_status,remarks,task_gid,task_name,task_code,module_name,severity_name,task_typename,task_typegid," +
                    "module_gid,severity_gid,estimated_hours,task_status from tsk_mst_ttaskmanagement" +
                        " where task_gid='" + task_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.task_gid = objODBCDatareader["task_gid"].ToString();
                    values.task_name = objODBCDatareader["task_name"].ToString();
                    values.task_code = objODBCDatareader["task_code"].ToString();
                    values.remarks = objODBCDatareader["remarks"].ToString();
                    values.task_status = objODBCDatareader["task_status"].ToString();
                    values.retesting_flag = objODBCDatareader["retesting_flag"].ToString();
                    values.estimated_hours = objODBCDatareader["estimated_hours"].ToString();
                    values.functionality_name = objODBCDatareader["functionality_name"].ToString();
                    values.task_typegid = objODBCDatareader["task_typegid"].ToString();
                    values.hold_flag = objODBCDatareader["hold_flag"].ToString();
                    values.severity_gid = objODBCDatareader["severity_gid"].ToString();
                    values.module_gid = objODBCDatareader["module_gid"].ToString();
                    //values.client_gid = objODBCDatareader["client_gid"].ToString();
                    values.task_typename = objODBCDatareader["task_typename"].ToString();
                    values.task_typename = objODBCDatareader["task_typename"].ToString();
                    //values.client_name = objODBCDatareader["client_name"].ToString();
                    values.severity_name = objODBCDatareader["severity_name"].ToString();
                    values.module_name = objODBCDatareader["module_name"].ToString();
                    values.employee_status = objODBCDatareader["employee_status"].ToString();
                    values.actualdevelopment_hrs = objODBCDatareader["actualdevelopment_hrs"].ToString();
                    values.actualcompleted_hrs = objODBCDatareader["actualcompleted_hrs"].ToString();

                }
                msSQL = " select GROUP_CONCAT(project_name SEPARATOR ',') AS project_name FROM tsk_mst_ttask2client " +
                      " where task_gid='" + task_gid + "'";
                values.client_name = objdbconn.GetExecuteScalar(msSQL);
                //msSQL = " select GROUP_CONCAT(sub_task SEPARATOR ',') AS sub_task FROM tsk_trn_ttasksheet " +
                //       " where task_gid='" + task_gid + "'";
                //values.sub_task = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select a.status,IFNULL(a.remarks, '-') AS remarks,IFNULL(a.actualdevelopment_hrs, '-') AS actualdevelopment_hrs,IFNULL(a.actualcompleted_hrs, '-') AS actualcompleted_hrs,DATE_FORMAT(a.updated_date, '%d-%m-%Y %H:%i:%s') AS updated_date," +
                       "  IFNULL(CONCAT(h.user_firstname, ' ', h.user_lastname, ' || ', h.user_code), '-') AS update_by from  tsk_mst_ttaskstatuslog a LEFT JOIN adm_mst_tuser h ON a.updated_by = h.user_gid where a.task_gid='" + task_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var hold = new List<statuslog_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        hold.Add(new statuslog_list
                        {
                            actualdevelopment_hrs = (dr_datarow["actualdevelopment_hrs"].ToString()),
                            actualcompleted_hrs = (dr_datarow["actualcompleted_hrs"].ToString()),
                            update_by = (dr_datarow["update_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                            task_status = (dr_datarow["status"].ToString()),

                        });

                        values.statuslog_list = hold;
                    }
                }
                msSQL = " select sub_task,hrs_taken FROM tsk_trn_ttasksheet " +
                        " where task_gid='" + task_gid + "' and (sub_task IS NOT NULL AND sub_task != '')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var subtask = new List<subtask>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        subtask.Add(new subtask
                        {
                            subtask_name = (dr_datarow["sub_task"].ToString()),
                            hrs_taken = (dr_datarow["hrs_taken"].ToString()),
                        });

                        values.subtask = subtask;
                    }
                }
                msSQL = " select actualcompleted_hrs,actualdevelopment_hrs,total_hrs,DATE_FORMAT(deployment_date, '%d-%m-%Y') as deployment_date" +
                        "  from  tsk_mst_ttaskstatuslog where status='Completed In Live' and  task_gid='" + task_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var completed_datails = new List<completedlive_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        completed_datails.Add(new completedlive_list
                        {
                            actualdevelopment_hrs = (dr_datarow["actualdevelopment_hrs"].ToString()),
                            actualcompleted_hrs = (dr_datarow["actualcompleted_hrs"].ToString()),
                            total_hrs = (dr_datarow["total_hrs"].ToString()),
                            deployment_date = (dr_datarow["deployment_date"].ToString()),

                        });

                        values.completedlive_list = completed_datails;
                    }
                }
                msSQL = " select docupload_type,docupload_name,file_path,taskdocumentupload_gid from tsk_mst_ttaskmanagementdocument " +
                         " where task_gid='" + task_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getverticalsList = new List<documentdata_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getverticalsList.Add(new documentdata_list
                        {
                            docupload_name = (dr_datarow["docupload_name"].ToString()),
                            taskdocumentupload_gid = (dr_datarow["taskdocumentupload_gid"].ToString()),
                            docupload_type = (dr_datarow["docupload_type"].ToString()),

                            file_path = (dr_datarow["file_path"].ToString()),
                            //file_path = objcmnstorage.EncryptData(dr_datarow["file_path"].ToString()),
                        });

                        values.documentdata_list = getverticalsList;
                    }
                }
                msSQL = " select a.assigned_member,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,CONCAT(c.user_firstname, ' ', c.user_lastname, ' || ', c.user_code) AS created_by  from tsk_mst_ttask2member a LEFT JOIN  adm_mst_tuser c ON a.created_by = c.user_gid" +
                        " LEFT JOIN  hrm_mst_temployee b ON b.user_gid = c.user_gid where  a.created_date is not null and a.task_gid='" + task_gid + "' order by created_date";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var assigned = new List<assigned_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        assigned.Add(new assigned_list
                        {
                            assigned_date = (dr_datarow["created_date"].ToString()),
                            assigned_by = (dr_datarow["created_by"].ToString()),
                            assigned_to = (dr_datarow["assigned_member"].ToString()),

                        });

                        values.assigned_list = assigned;
                    }
                }
                msSQL = " select a.previous_member,a.reassigned_member,DATE_FORMAT(a.updated_date, '%d-%m-%Y') AS updated_date,CONCAT(c.user_firstname, ' ', c.user_lastname, ' || ', c.user_code) AS updated_by  from tsk_mst_ttransferlog a LEFT JOIN  adm_mst_tuser c ON a.updated_by = c.user_gid" +
                        " LEFT JOIN  hrm_mst_temployee b ON b.user_gid = c.user_gid where  a.updated_date is not null and a.task_gid='" + task_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var transfer = new List<transfer_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        transfer.Add(new transfer_list
                        {
                            previous_member = (dr_datarow["previous_member"].ToString()),
                            reassigned_member = (dr_datarow["reassigned_member"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),


                        });

                        values.transfer_list = transfer;
                    }
                }
                dt_datatable.Dispose();
                msSQL = "select a.task_remarks,DATE_FORMAT(a.completedlive_date, '%d-%m-%Y') AS completedlive_date,DATE_FORMAT(a.hold_date, '%d-%m-%Y') AS hold_date, DATE_FORMAT(a.completed_date, '%d-%m-%Y') AS completed_date,DATE_FORMAT(a.taskupdated_date, '%d-%m-%Y') AS taskupdated_date," +
                    "DATE_FORMAT(a.updated_date, '%d-%m-%Y') AS updated_date, CONCAT(c.user_firstname, ' ', c.user_lastname, ' || ', c.user_code) AS hold_by, CASE " +
                    " WHEN a.taskupdated_by IS NOT NULL THEN CONCAT(f.user_firstname, ' ', f.user_lastname, ' || ', f.user_code) ELSE NULL END AS taskupdated_by, CASE " +
                     "  WHEN a.completed_by IS NOT NULL THEN CONCAT(d.user_firstname, ' ', d.user_lastname, ' || ', d.user_code) ELSE NULL END AS completed_by,CASE " +
                     " WHEN a.updated_by IS NOT NULL THEN CONCAT(h.user_firstname, ' ', h.user_lastname, ' || ', h.user_code) ELSE NULL END AS updated_by," +
                     " case WHEN a.completedlive_by IS NOT NULL THEN CONCAT(h.user_firstname, ' ', h.user_lastname, ' || ', h.user_code) ELSE NULL END AS completedlive_by from tsk_mst_ttaskmanagement a  " +
                     "LEFT JOIN  adm_mst_tuser c ON a.hold_by = c.user_gid LEFT JOIN adm_mst_tuser f ON a.taskupdated_by = f.user_gid LEFT JOIN " +
                     " adm_mst_tuser d ON a.completed_by = d.user_gid LEFT JOIN adm_mst_tuser h ON a.updated_by = h.user_gid LEFT JOIN hrm_mst_temployee g ON g.user_gid = f.user_gid " +
                     " LEFT JOIN  hrm_mst_temployee b ON b.user_gid = c.user_gid  " +
                     " where a.task_gid='" + task_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.completed_date = objODBCDatareader["completed_date"].ToString();
                    values.taskupdated_date = objODBCDatareader["taskupdated_date"].ToString();
                    values.updated_date = objODBCDatareader["updated_date"].ToString();
                    values.hold_date = objODBCDatareader["hold_date"].ToString();
                    values.hold_by = objODBCDatareader["hold_by"].ToString();
                    values.txtremarks = objODBCDatareader["task_remarks"].ToString();
                    values.taskupdated_by = objODBCDatareader["taskupdated_by"].ToString();
                    values.completed_by = objODBCDatareader["completed_by"].ToString();
                    values.updated_by = objODBCDatareader["updated_by"].ToString();
                    values.completedlive_by = objODBCDatareader["completedlive_by"].ToString();
                    values.completedlive_date = objODBCDatareader["completedlive_date"].ToString();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Dataskassigndelete(string task_gid, string employee_gid, taskaddlist values)
        {
            try
            {
                msSQL = "select a.assigned_flag from tsk_mst_ttaskmanagement a where a.task_gid='" + task_gid + "'";
                string completed_flag = objdbconn.GetExecuteScalar(msSQL);
                if (completed_flag == "N")
                {
                    values.status = false;
                    values.message = "Task is Tagged to the Member So..you can't Delete..!";
                    return;
                }
                msSQL = "delete from tsk_mst_ttaskmanagement where task_gid='" + task_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Task Deleted Successfully";
                    values.status = true;
                }
                else
                {
                    values.message = "Error while Deleting Customer";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }

        public void Daaddtaskdocument(HttpRequest httpRequest, string user_gid, result objResult)
        {

            HttpFileCollection httpFileCollection;
            string final_path, httpsUrl;
            MemoryStream ms_stream = new MemoryStream();
            string lscompany_code = string.Empty;
            string task_gid = httpRequest.Form["task_gid"];
            string filelist = httpRequest.Form["filelist"];
            string[] fileListArray = filelist.Split('+');
            String path = lspath;
            HttpPostedFile httpPostedFile;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            path = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "TSK/TaskDocument/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

            if ((!System.IO.Directory.Exists(path)))
                System.IO.Directory.CreateDirectory(path);

            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {

                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string project_flag = httpRequest.Form["project_flag"].ToString();
                        FileExtension = Path.GetExtension(FileExtension).ToLower();

                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);

                        byte[] bytes = ms.ToArray();
                        if ((objcmnstorage.CheckIsValidfilename(FileExtension, project_flag) == false) || (objcmnstorage.CheckIsExecutable(bytes) == true))
                        {
                            objResult.status = false;
                            objResult.message = "File format is not supported";
                            return;
                        }

                        bool status;
                        status = objcmnstorage.UploadStream("erpdocument", lscompany_code + "/" + "TSK/TaskDocument/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, ms);
                        ms.Close();
                        //lspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "TSK/TaskDocument/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        //objcmnfunctions.uploadFile(lspath, FileExtension);
                        path = "erpdocument" + "/" + lscompany_code + "/" + "TSK/TaskDocument/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        bool status1;

                        //status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        //final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        //httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                        //                        '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                        //                        '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];


                        msGetGid = objcmnfunctions.GetMasterGID("TD2D");

                        msSQL = " insert into tsk_mst_ttaskmanagementdocument(" +
                                    " taskdocumentupload_gid," +
                                    " task_gid ," +
                                    " docupload_type ," +
                                    " docupload_name ," +
                                    " file_path," +
                                    " created_by," +
                                    " created_date" +
                                    " )values(" +
                                    "'" + msGetGid + "'," +
                                    "'" + task_gid + "'," +
                                    "'" + httpPostedFile.FileName.Replace("'", @"\'") + "'," +
                                    "'" + fileListArray[i] + "'," +
                                    "'" + path + msdocument_gid + FileExtension + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    if (mnResult != 0)
                    {
                        objResult.status = true;
                        objResult.message = "Document Uploaded Successfully";

                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error Occured While Uploading the document";

                    }

                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                objResult.status = false;

            }
        }
        public void Dacustomer_list(string team_gid, projectlist values)
        {
            try
            {
                msSQL = " select project_name,project_gid from tsk_mst_tproject2module " +
                        " where team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getverticalsList = new List<customer_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getverticalsList.Add(new customer_list
                        {
                            project_name = dt["project_name"].ToString(),
                            project_gid = dt["project_gid"].ToString(),
                        });

                        values.customer_list = getverticalsList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public bool DaManagercompletedsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT f.assigned_member_gid,DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s') as assigned_Date,f.assigned_member,CASE WHEN DATEDIFF(a.completedlive_date, a.taskupdated_date) > 0 THEN CONCAT(DATEDIFF(a.completedlive_date, a.taskupdated_date), ' days')" +
                    " WHEN TIMESTAMPDIFF(HOUR, a.taskupdated_date, a.completedlive_date) > 0 THEN CONCAT(TIMESTAMPDIFF(HOUR, a.taskupdated_date, a.completedlive_date), ' hours') " +
                    "ELSE CONCAT(TIMESTAMPDIFF(MINUTE, a.taskupdated_date, a.completedlive_date), ' minutes') END AS time_taken_to_complete ,a.estimated_hours,a.employee_status,a.task_gid,a.task_code,a.module_name," +
                        "a.module_gid,a.actualcompleted_hrs,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid " +
                        " left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' and a.hold_flag='Y' and a.task_status='Completed In Live' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.completed_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            assigned_Date = dt["assigned_Date"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_member_gid = dt["assigned_member_gid"].ToString(),
                            actualcompleted_hrs = dt["actualcompleted_hrs"].ToString(),
                            time_taken_to_complete = dt["time_taken_to_complete"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaManagerpendingsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.estimated_hours,a.employee_status,a.task_gid,a.task_code,a.module_name," +
                        "a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' and a.hold_flag='Y' and  a.task_status='Pending' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.pending_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaManagerholdsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT f.assigned_member_gid,DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s') as assigned_Date,f.assigned_member,DATE_FORMAT(a.hold_date, '%d-%m-%Y %H:%i:%s') AS hold_date,a.estimated_hours,a.employee_status,a.task_gid,a.task_code,a.module_name," +
                        "a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid " +
                        " left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' and a.hold_flag='N' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.holdmanager_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            hold_date = dt["hold_date"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            assigned_Date = dt["assigned_Date"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_member_gid = dt["assigned_member_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaManagerintestingsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT f.assigned_member_gid,DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s') as assigned_Date,f.assigned_member,a.actualdevelopment_hrs,a.estimated_hours,a.employee_status,a.task_gid,a.task_code,a.module_name," +
                        "a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.completed_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid " +
                        " left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' and a.hold_flag='Y' and a.task_status='In Testing' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.testmanager_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            assigned_Date = dt["assigned_Date"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_member_gid = dt["assigned_member_gid"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            actualdevelopment_hrs = dt["actualdevelopment_hrs"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }

        public bool DaManagerassignedsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                //msSQL = " SELECT a.task_gid,a.task_code,a.estimated_hours,DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s') as assigned_Date,f.assigned_member,f.assigned_member_gid,g.assignmanager_name,a.module_name,a.module_gid,a.task_name,a.severity_name,a.employee_status,a.task_typename,a.task_status," +
                //      "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                //      " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                //      "  left join tsk_mst_ttask2member f on f.task_gid=a.task_gid left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                //      " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' and a.hold_flag='Y' and a.task_status='Assigned' " +
                //      " ORDER  BY task_gid DESC";
                msSQL = " SELECT a.task_gid,a.task_code,a.estimated_hours,DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s') as assigned_Date,f.assigned_member,f.assigned_member_gid,g.assignmanager_name,a.module_name,a.module_gid,a.task_name,a.severity_name,a.employee_status,a.task_typename,a.task_status," +
                      "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                      " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                      " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid " +
                      "left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                      " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' and a.hold_flag='Y' and a.task_status='Assigned' " +
                      " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskpendinglist>();
                values.assigned_count = dt_datatable.Rows.Count;
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            assigned_Date = dt["assigned_Date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_member_gid = dt["assigned_member_gid"].ToString(),
                            assignmanager_name = dt["assignmanager_name"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public void DaUpdateTask(string user_gid, taskteamlist values)
        {
            try
            {
                if (values.employee_status == "Revoke")
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                    msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
                   "taskstatuslog_gid, " +
                   "task_gid, " +
                   "status, " +
                   "remarks, " +
                   "updated_by, " +
                   "updated_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + values.task_gid + "'," +
                   " '" + values.employee_status + "'," +
                   "'" + values.remarks.Replace("'", "") + "'," +
                    " '" + user_gid + " '," +
                     " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    //msSQL = " update tsk_mst_ttask2hold set " +
                    //        " revoke_reason ='" + values.remarks + "'," +
                    //       " revoke_by = '" + user_gid + "'," +
                    //      " revoke_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update tsk_mst_ttaskmanagement set " +
                            " hold_flag ='Y'," +
                            " revoke_by = '" + user_gid + "'," +
                            " revoke_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

                if (values.employee_status == "In Progress")
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                    msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
                   "taskstatuslog_gid, " +
                   "task_gid, " +
                   "status, " +
                   "updated_by, " +
                   "updated_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + values.task_gid + "'," +
                   " '" + values.employee_status + "'," +
                    " '" + user_gid + " '," +
                     " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update tsk_mst_ttaskmanagement set " +
                            " employee_status ='" + values.employee_status + "'," +
                            " taskupdated_by = '" + user_gid + "'," +
                            " taskupdated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (values.employee_status == "Re-Testing")
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                    msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
                   "taskstatuslog_gid, " +
                   "task_gid, " +
                   "status, " +
                    "remarks, " +
                   "updated_by, " +
                   "updated_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + values.task_gid + "'," +
                   " '" + values.employee_status + "'," +
                    " '" + values.remarks.Replace("'", "") + "'," +
                    " '" + user_gid + " '," +
                     " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update tsk_mst_ttaskmanagement set " +
                            " retesting_flag ='Y'," +
                            " task_status ='Assigned'," +
                           " employee_status ='In Progress'," +
                            " taskupdated_by = '" + user_gid + "'," +
                            " taskupdated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (values.employee_status == "Hold")
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                    msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
                   "taskstatuslog_gid, " +
                   "task_gid, " +
                   "status, " +
                   "remarks, " +
                   "updated_by, " +
                   "updated_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + values.task_gid + "'," +
                   " '" + values.employee_status + "'," +
                   "'" + values.remarks.Replace("'", "") + "'," +
                   " '" + user_gid + " '," +
                   " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                   mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //msGetGid = objcmnfunctions.GetMasterGID("T2HD");

                    //msSQL = "insert into tsk_mst_ttask2hold(" +
                    //       " task2hold_gid ," +
                    //       " task_gid," +
                    //       " allocationhold_reason," +
                    //       " created_by," +
                    //       " created_date)" +
                    //       " values(" +
                    //       "'" + msGetGid + "'," +
                    //       "'" + values.task_gid + "'," +
                    //       "'" + values.remarks.Replace("'", "''") + "'," +
                    //       "'" + user_gid + "'," +
                    //       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update tsk_mst_ttaskmanagement set " +
                             " hold_flag ='N'," +
                           " task_remarks ='" + values.remarks + "'," +
                           " hold_by = '" + user_gid + "'," +
                           " hold_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (values.employee_status == "In Testing")
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                    msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
                   "taskstatuslog_gid, " +
                   "task_gid, " +
                   "status, " +
                   "updated_by, " +
                   "updated_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + values.task_gid + "'," +
                   " '" + values.employee_status + "'," +
                   " '" + user_gid + " '," +
                   " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update tsk_mst_ttaskmanagement set " +
                           " task_status ='" + values.employee_status + "'," +
                           " employee_status ='" + values.employee_status + "'," +
                           " task_remarks ='" + values.remarks + "'," +
                           " completed_by = '" + user_gid + "'," +
                           " completed_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (values.employee_status == "Completed In Live")
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                    string datefordeployment = objcmnfunctions.GetDateFormat(values.deployment_date);
                    msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
                   "taskstatuslog_gid, " +
                   "task_gid, " +
                   "status, " +
                   "actualdevelopment_hrs, " +
                   "actualcompleted_hrs, " +
                   "deployment_date, " +
                   "total_hrs, " +
                   "updated_by, " +
                   "updated_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + values.task_gid + "'," +
                   " '" + values.employee_status + "'," +
                   "'" + values.actualdevelopment_hrs.Replace("'", "") + "'," +
                   "'" + values.actualcompleted_hrs.Replace("'", "") + "'," +
                   "'" + datefordeployment + "'," +
                   "'" + values.total_hrs.Replace("'", "") + "'," +
                   " '" + user_gid + " '," +
                   " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update tsk_mst_ttaskmanagement set " +
                            " task_status ='" + values.employee_status + "'," +
                             " employee_status ='" + values.employee_status + "'," +
                           " actualcompleted_hrs ='" + values.actualcompleted_hrs.Replace("'", "''") + "'," +
                           " actualdevelopment_hrs ='" + values.actualdevelopment_hrs.Replace("'", "''") + "'," +
                            " completed_flag ='Y', " +
                           " task_remarks ='" + values.remarks + "'," +
                           " completedlive_by = '" + user_gid + "'," +
                           " completedlive_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Task Status successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Tsk Status";
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
            }
        }
        public void DaAddmember(string user_gid, taskteamlist values)
        {
            try
            {
                msSQL = " update tsk_mst_ttaskmanagement set " +
                        " task_status ='Assigned', " +
                        " assigned_flag ='N', " +
                        " assigned_member = '" + values.assigned_member + "'," +
                        " assigned_member_gid = '" + values.assigned_member_gid + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //msSQL = "delete from tsk_mst_ttask2member where task_gid='" + values.task_gid + "'";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {

                    //for (var i = 0; i < values.assignmem_list.Count; i++)
                    //{
                    msGetGid1 = objcmnfunctions.GetMasterGID("T2MM");
                    msSQL = "INSERT INTO tsk_mst_ttask2member (" +
                   "task2member_gid, " +
                   "task_gid, " +
                   "team_gid, " +
                   "assigned_member_gid, " +
                   "assigned_member, " +
                   "created_by, " +
                   "created_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                    " '" + values.task_gid + "'," +
                   " '" + values.team_gid + "'," +
                   " '" + values.assigned_member_gid + "'," +
                   " '" + values.assigned_member + "'," +
                   " '" + user_gid + " '," +
                   " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Member assigned to task successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Member Assign";
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
            }
        }
        public void Dareassign(string user_gid, taskteamlist values)
        {
            try
            {
                msSQL = "select assigned_member from tsk_mst_ttaskmanagement where task_gid='" + values.task_gid + "'";
                string assigned_member = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " update tsk_mst_ttaskmanagement set " +
                    " assigned_member = '" + values.assigned_member + "'," +
                    " assigned_member_gid = '" + values.assigned_member_gid + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msGetGid1 = objcmnfunctions.GetMasterGID("TRFL");
                msSQL = "INSERT INTO tsk_mst_ttransferlog (" +
               "transferlog_gid, " +
               "task_gid, " +
               "previous_member, " +
               "reassigned_member, " +
               "updated_by, " +
               "updated_date) " +
               "VALUES (" +
               " '" + msGetGid1 + "'," +
                " '" + values.task_gid + "'," +
               " '" + assigned_member + "'," +
               " '" + values.assigned_member + "'," +
               " '" + user_gid + " '," +
               " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //msSQL = "delete from tsk_mst_ttask2member where task_gid='" + values.task_gid + "'";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("T2MM");
                    msSQL = "INSERT INTO tsk_mst_ttask2member (" +
                   "task2member_gid, " +
                   "task_gid, " +
                   "team_gid, " +
                   "assigned_member_gid, " +
                   "assigned_member, " +
                   "created_by, " +
                   "created_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                    " '" + values.task_gid + "'," +
                   " '" + values.team_gid + "'," +
                   " '" + values.assigned_member_gid + "'," +
                   " '" + values.assigned_member + "'," +
                   " '" + user_gid + " '," +
                   " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Member assigned to task successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Member Assign";
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
            }
        }
        public bool DaMemberprogresssummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.employee_status,a.task_gid,a.task_code,a.module_name,a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) g on g.task_gid = a.task_gid " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.employee_status='In Progress' and a.hold_flag='Y' and  g.assigned_member_gid= '" + employee_gid + "' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.progress_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaMembertestingsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.employee_status,a.task_gid,a.task_code,a.module_name,a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        "  left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) g on g.task_gid = a.task_gid  " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.employee_status='In Testing' and a.hold_flag='Y' and  g.assigned_member_gid= '" + employee_gid + "' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskpendinglist>();
                values.testing_count = dt_datatable.Rows.Count;
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaMemberHoldsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.employee_status,a.task_gid,a.task_code,a.module_name,a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        "  left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) g on g.task_gid = a.task_gid  " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where  a.hold_flag='N' and  g.assigned_member_gid= '" + employee_gid + "' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskpendinglist>();
                values.hold_count = dt_datatable.Rows.Count;
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaMemberlivesummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.employee_status,a.task_gid,a.task_code,a.module_name,a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) g on g.task_gid = a.task_gid  " +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.employee_status='Completed In Live' and a.hold_flag='Y' and  g.assigned_member_gid= '" + employee_gid + "' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskpendinglist>();
                values.live_count = dt_datatable.Rows.Count;
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool DaMemberpendingsummary(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT a.employee_status,a.task_gid,a.task_code,a.module_name,a.module_gid,a.task_name,a.severity_name,a.task_typename,a.task_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,DATE_FORMAT(a.updated_date, '%d-%m-%Y %H:%i:%s') AS update_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as updated_by,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser f on a.updated_by = f.user_gid left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        "   left join (SELECT task_gid,MAX(created_date) AS max_created_date  FROM  tsk_mst_ttask2member GROUP BY  task_gid ) f_max ON f_max.task_gid = a.task_gid LEFT JOIN  tsk_mst_ttask2member g ON g.task_gid = a.task_gid AND g.created_date = f_max.max_created_date" +
                        " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.employee_status='Pending' and a.hold_flag='Y' and  g.assigned_member_gid= '" + employee_gid + "' " +
                        " ORDER  BY task_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskpendinglist>();
                values.pending_count = dt_datatable.Rows.Count;
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_gid = dt["task_gid"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            task_status = dt["task_status"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            update_by = dt["updated_by"].ToString(),
                            update_date = dt["update_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public void DaGetAssignedclientlist(string team_gid, string task_gid, taskteamlist values)
        {
            try
            {
                msSQL = " select distinct '" + task_gid + "' as task_gid ,a.project_name,a.project_gid,'" + team_gid + "' as team_gid" +
                   "  from tsk_mst_tproject2module a" +
                   " where team_gid= '" + team_gid + "' and a.project_gid in " +
                   " (select project_gid from" +
                   " tsk_mst_ttask2client" +
                   " where task_gid= '" + task_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedclient_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedclient_list
                        {
                            project_name = dt["project_name"].ToString(),
                            project_gid = dt["project_gid"].ToString(),
                            team_gid = dt["team_gid"].ToString(),
                            task_gid = dt["task_gid"].ToString(),

                        });
                        values.GetAssignedclient_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public void DaGetUnassignedclientlist(string team_gid,string task_gid, taskteamlist values)
        {
            try
            {

                msSQL = " select a.project_name,a.project_gid,'" + team_gid + "' as team_gid" +
                           " from tsk_mst_tproject2module a" +
                           " where team_gid= '" + team_gid + "' and a.project_gid not in" +
                           " (select project_gid from" +
                           " tsk_mst_ttask2client" +
                           " where task_gid= '" + task_gid + "') order by project_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassignedclientlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassignedclientlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            project_name = dt["project_name"].ToString(),
                            project_gid = dt["project_gid"].ToString(),
                        });
                        values.GetUnassignedclientlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public void Dagetclient(string user_gid, projectlist values)
        {
            try
            {
                msSQL = "delete from tsk_mst_ttask2client where task_gid='" + values.task_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    for (var i = 0; i < values.client_list.Count; i++)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("T2CL");
                        msSQL = " insert into tsk_mst_ttask2client (" +
                                                       " task2client_gid," +
                                                       " team_gid," +
                                                       " task_gid," +
                                                       " project_gid," +
                                                       " project_name," +
                                                       " created_by, " +
                                                       " created_date)" +
                                                       " values(" +
                                                       " '" + msGetGid1 + "'," +
                                                       " '" + values.team_gid + "'," +
                                                       " '" + values.task_gid + "'," +
                                                       " '" + values.client_list[i].project_gid + "'," +
                                                       " '" + values.client_list[i].project_name + "'," +
                                                       "'" + user_gid + "'," +
                                                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Client assigned successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while updating Client";
                }


            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                ex.StackTrace.ToString();
                values.status = false;
            }
        }
        public void Dacount(MdlTskTrnTaskManagement values,string employee_gid)
        {
            try
            {

                msSQL = " SELECT COUNT(*) AS row_count" +
                         " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                         " left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) g on g.task_gid = a.task_gid " +
                         " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assigned_member_gid= '" + employee_gid + "' ";
                values.row_count = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public void Damanagerallcount(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {

                msSQL = " SELECT COUNT(*) AS row_count" +
                         " FROM tsk_mst_ttaskmanagement a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                         " left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid " +
                         " left join hrm_mst_temployee b on b.employee_gid = c.user_gid where g.assignmanager_gid= '" + employee_gid + "' ";
                values.row_count = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public void Damanagercount(MdlTskTrnTaskManagement values, string employee_gid)
        {
            try
            {

                msSQL = "select count(a.severity_name) from tsk_mst_ttaskmanagement a left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid  where a.severity_name='Show Stopper' and a.employee_status<>'Completed In Live' and  g.assignmanager_gid='" + employee_gid + "'";
                values.show_stopper_count = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select count(a.severity_name) from tsk_mst_ttaskmanagement a left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid  where a.severity_name='Critical Mandatory' and a.employee_status<>'Completed In Live' and  g.assignmanager_gid='" + employee_gid + "'";
                values.mandatory_count = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select count(a.severity_name) from tsk_mst_ttaskmanagement a left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid  where a.severity_name='Critical Non-Mandatory' and a.employee_status<>'Completed In Live' and  g.assignmanager_gid='" + employee_gid + "'";
                values.non_mandatory_count = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select count(a.severity_name) from tsk_mst_ttaskmanagement a left join tsk_mst_tteam2manager g on g.team_gid=a.module_gid  where a.severity_name='Nice To Have' and a.employee_status<>'Completed In Live' and  g.assignmanager_gid='" + employee_gid + "'";
                values.nice_to_have_count = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public void Daconditionclient(string task_gid, taskteamlist values)
        {
            try
            {
                msSQL = "select a.completed_flag from tsk_mst_ttaskmanagement a where a.task_gid='" + task_gid + "'";
                string completed_flag = objdbconn.GetExecuteScalar(msSQL);
                if (completed_flag == "N")
                {
                    values.status = false;
                    values.message = "Client is Tagged to the Member So..you can't Delete..!";
                    return;
                }
                else
                {
                    values.status = true;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
      public void Damodulelist(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = "select distinct a.teamname_gid,a.team_name,a.team_gid from tsk_mst_tteam a left join tsk_mst_tteam2manager b on b.team_gid=a.team_gid" +
                        " WHERE b.assignmanager_name IS NOT NULL ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<moduledropdown_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new moduledropdown_list
                        {
                            team_name = dt["team_name"].ToString(),
                            teamname_gid = dt["teamname_gid"].ToString(),
                            team_gid = dt["team_gid"].ToString(),
                        });
                        values.moduledropdown_list = getteammemberslist;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }

        public bool DaGetHoldtask(string task_gid, string taskhold_reason,string status, string user_gid, taskteamlist values)
        {
            try
            {
                msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
               "taskstatuslog_gid, " +
               "task_gid, " +
               "status, " +
               "remarks, " +
               "updated_by, " +
               "updated_date) " +
               "VALUES (" +
               " '" + msGetGid1 + "'," +
               " '" + task_gid + "'," +
               " '" + status + "'," +
               "'" + taskhold_reason.Replace("'", "") + "'," +
               " '" + user_gid + " '," +
               " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //msGetGid = objcmnfunctions.GetMasterGID("T2HD");

                //msSQL = "insert into tsk_mst_ttask2hold(" +
                //       " task2hold_gid ," +
                //       " task_gid," +
                //       " allocationhold_reason," +
                //       " created_by," +
                //       " created_date)" +
                //       " values(" +
                //       "'" + msGetGid + "'," +
                //       "'" + task_gid + "'," +
                //       "'" + taskhold_reason.Replace("'", "''") + "'," +
                //       "'" + user_gid + "'," +
                //       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " update tsk_mst_ttaskmanagement set " +
                          " hold_flag ='N'," +
                         " task_remarks ='" + taskhold_reason + "'," +
                         " hold_by = '" + user_gid + "'," +
                         " hold_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + task_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1) {
                    values.message = "Task Hold Successfully..!";
                    values.status = true;
                    return true;
                }
                else
                {
                    values.message = "Error Occured..!";
                    values.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }

        }
        public bool DaGetrevoketask(string user_gid, taskteamlist values)
        {
            try
            {
                msGetGid1 = objcmnfunctions.GetMasterGID("TLGI");
                msSQL = "INSERT INTO tsk_mst_ttaskstatuslog (" +
               "taskstatuslog_gid, " +
               "task_gid, " +
               "status, " +
               "remarks, " +
                "updated_by, " +
               "updated_date) " +
               "VALUES (" +
               " '" + msGetGid1 + "'," +
               " '" + values.task_gid + "'," +
               " '" + values.employee_status + "'," +
               "'" + values.remarks.Replace("'", "") + "'," +
                " '" + user_gid + " '," +
                 " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                //msSQL = " update tsk_mst_ttask2hold set " +
                //        " revoke_reason ='" + values.remarks + "'," +
                //       " revoke_by = '" + user_gid + "'," +
                //      " revoke_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update tsk_mst_ttaskmanagement set " +
                        " hold_flag ='Y'," +
                        " revoke_by = '" + user_gid + "'," +
                        " revoke_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where task_gid='" + values.task_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Task Status Updated Successfully..!";
                    values.status = true;
                    return true;
                }
                else
                {
                    values.message = "Error Occured..!";
                    values.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public void Daleveltwo_menu(string module_gid,taskteamlist values)
        {
            try
            {
                msSQL = "select module_name,module_gid from adm_mst_tmoduleangular where module_gid_parent='" + module_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<menulevel>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new menulevel
                        {
                            module_name = dt["module_name"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                        });
                        values.menulevel = getteammemberslist;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Datask_list(string employee_gid, taskteamlist values)
        {
            try
            {
                msSQL = "select a.task_name,a.task_gid from tsk_mst_ttaskmanagement a left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) b on b.task_gid = a.task_gid " +
                    " where b.assigned_member_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<taskdetail_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new taskdetail_list
                        {
                            task_name = dt["task_name"].ToString(),
                            task_gid = dt["task_gid"].ToString(),
                        });
                        values.taskdetail_list = getteammemberslist;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public bool DaTasksheetadd(taskaddlist values, string user_gid)
        {
            try
            {
                string taskdate = objcmnfunctions.GetDateFormat(values.task_date);
                msGetGid = objcmnfunctions.GetMasterGID("TTSH");
                msSQL = " insert into tsk_trn_ttasksheet  (" +
                        " tasksheet_gid," +
                        " task_gid," +
                        " task_name," +
                        " module_name," +
                        " module_gid ," +
                        " module_name_gid ," +
                        " task_detail ," +
                        " task_type_name ," +
                        " task_type_gid," +
                        " status," +
                        " hrs_taken," +
                         " sub_task," +
                        " task_date," +
                        " created_by," +
                        " created_date)" +
                        " VALUES(" +
                        "'" + msGetGid + "'," +
                        "'" + values.task_gid + "'," +
                        "'" + values.task_name.Replace("'", "''") + "'," +
                        "'" + values.module_name.Replace("'", "''") + "'," +
                        "'" + values.module_gid + "'," +
                        "'" + values.module_name_gid + "'," +
                       "'" + values.task_detail + "'," +
                        "'" + values.task_typename.Replace("'", "''") + "'," +
                        "'" + values.task_typegid + "'," +
                        "'" + values.sheetstatus.Replace("'", "''") + "'," +
                        "'" + values.hrs_taken.Replace("'", "''") + "'," +
                        "'" + values.sub_task.Replace("'", "''") + "'," +
                        "'" + taskdate + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Task Added Successfully";
                    values.status = true;
                    values.task_gid = msGetGid;
                }
                else
                {
                    values.message = "Error While Adding Task";
                    values.status = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public bool Daupdatetasksheet(taskaddlist values, string user_gid)
        {
            try
            {
                string taskdate = objcmnfunctions.GetDateFormat(values.task_date);
                msSQL = " update tsk_trn_ttasksheet set " +
                        " module_name ='" + values.module_name.Replace("'", "''") + "', " +
                        " module_gid ='" + values.module_gid + "', " +
                        " task_name ='" + values.task_name.Replace("'", "''") + "', " +
                        " task_gid ='" + values.task_gid + "', " +
                        " module_name_gid ='" + values.module_name_gid + "', " +
                        " task_detail ='" + values.task_detail.Replace("'", "''") + "', " +
                        " task_type_name ='" + values.task_typename.Replace("'", "''") + "', " +
                        " task_type_gid ='" + values.task_typegid + "', " +
                        " status ='" + values.sheetstatus + "', " +
                        " hrs_taken ='" + values.hrs_taken.Replace("'", "''") + "', " +
                        " sub_task ='" + values.sub_task.Replace("'", "''") + "', " +
                        " task_date ='" + taskdate + "', " +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where tasksheet_gid='" + values.tasksheet_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Task Updated Successfully";
                    values.status = true;
                    values.task_gid = msGetGid;
                }
                else
                {
                    values.message = "Error While Update Task";
                    values.status = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public bool Datasksheetsummary(MdlTskTrnTaskManagement values,string user_gid)
        {
            try
            {
                msSQL = " SELECT CASE WHEN a.sub_task IS NULL OR a.sub_task = '' THEN '-' ELSE a.sub_task END AS sub_task,a.task_type_gid,a.task_detail,a.module_gid,a.tasksheet_gid,a.task_gid,a.module_gid,a.module_name_gid,a.module_name,a.task_name,a.task_type_name,a.status,DATE_FORMAT(a.task_date, '%d-%m-%Y')as task_date,a.hrs_taken," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_trn_ttasksheet a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.created_by='" + user_gid + "' and DATE(a.task_date) = CURDATE() " +
                        " ORDER  BY tasksheet_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.show_stopper = dt_datatable.Rows.Count;
                var getdocumentlist = new List<tasksheet_list>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new tasksheet_list
                        {
                            task_gid = dt["task_gid"].ToString(),
                            hrs_taken = dt["hrs_taken"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                            tasksheet_gid = dt["tasksheet_gid"].ToString(),
                            module_name_gid = dt["module_name_gid"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            task_detail = dt["task_detail"].ToString(),
                            task_date = dt["task_date"].ToString(),
                            task_typename = dt["task_type_name"].ToString(),
                            status = dt["status"].ToString(),
                            task_typegid = dt["task_type_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            sub_task = dt["sub_task"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.tasksheet_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Datasksheethistorysummary(MdlTskTrnTaskManagement values, string user_gid)
        {
            try
            {
                msSQL = " SELECT CASE WHEN a.sub_task IS NULL OR a.sub_task = '' THEN '-' ELSE a.sub_task END AS sub_task,a.module_gid,a.tasksheet_gid,a.task_gid,a.module_name,a.task_name,a.task_type_name,a.status,DATE_FORMAT(a.task_date, '%d-%m-%Y')as task_date,a.hrs_taken," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_trn_ttasksheet a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid where a.created_by='" + user_gid + "' and DATE(a.task_date) != CURDATE() " +
                        " ORDER  BY tasksheet_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.show_stopper = dt_datatable.Rows.Count;
                var getdocumentlist = new List<tasksheet_list>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new tasksheet_list
                        {
                            task_gid = dt["task_gid"].ToString(),
                            hrs_taken = dt["hrs_taken"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            tasksheet_gid = dt["tasksheet_gid"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            task_date = dt["task_date"].ToString(),
                            task_typename = dt["task_type_name"].ToString(),
                            status = dt["status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            sub_task = dt["sub_task"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.tasksheet_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Dataskwisesummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                    msSQL = " SELECT IFNULL(DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s'), '-') AS assigned_date,IFNULL(f.assigned_member, 'Not Assigned') as assigned_member,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by,a.module_name,a.task_name,a.estimated_hours,a.task_typename,a.severity_name,a.functionality_name,a.employee_status,DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date " +
                         " FROM tsk_mst_ttaskmanagement a left join (SELECT task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid left join adm_mst_tuser c on a.created_by = c.user_gid where a.task_status<>'Completed In Live' " +
                         " ORDER  BY a.task_code DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.show_stopper = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            severity_name = dt["severity_name"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            functionality_name = dt["functionality_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_date = dt["assigned_date"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Dadeploywisesummary(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " SELECT f.assigned_member,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as completedlive_by,a.module_name,a.task_name,a.estimated_hours,a.task_typename,a.severity_name,a.functionality_name,a.employee_status,DATE_FORMAT(a.completedlive_date, '%d-%m-%Y %H:%i:%s') AS created_date " +
                        " FROM tsk_mst_ttaskmanagement a left join  (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid left join adm_mst_tuser c on a.completedlive_by = c.user_gid where a.task_status='Completed In Live' " +
                        " ORDER  BY a.task_code DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.show_stopper = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            severity_name = dt["severity_name"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            functionality_name = dt["functionality_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            completedlive_by = dt["completedlive_by"].ToString(),
                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public bool Dateamwisesummary(MdlTskTrnTaskManagement values,string module_gid)
        {
            try
            {
                msSQL = " SELECT IFNULL(DATE_FORMAT(f.created_date, '%d-%m-%Y %H:%i:%s'), '-') AS assigned_date,IFNULL(f.assigned_member, 'Not Assigned') as assigned_member,a.task_name,a.task_typename,a.severity_name,a.functionality_name,a.employee_status,DATE_FORMAT(a.completedlive_date, '%d-%m-%Y %H:%i:%s') AS created_date " +
                        " FROM tsk_mst_ttaskmanagement a left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) f on f.task_gid = a.task_gid left join adm_mst_tuser c on a.completedlive_by = c.user_gid where a.module_gid='" + module_gid +"' and a.task_status<>'Completed In Live' " +
                        " ORDER  BY a.task_code DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.show_stopper = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            severity_name = dt["severity_name"].ToString(),
                            functionality_name = dt["functionality_name"].ToString(),
                            task_name = dt["task_name"].ToString(),
                            employee_status = dt["employee_status"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_date = dt["assigned_date"].ToString(),

                        });
                    }
                    values.taskpending_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public void Dagettask(MdlTskTrnTaskManagement objmodulelist, string task_date)
        {
            try
            {
                string taskdate = objcmnfunctions.GetDateFormat(task_date);
                msSQL = " select distinct a.created_by as task_gid,DATE_FORMAT(a.task_date, '%d-%m-%Y')as task_date," +
                        "concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by from tsk_trn_ttasksheet a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                        " where a.task_date= '" + taskdate + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                objmodulelist.member = dt_datatable.Rows.Count;
                var get_ModuleList = new List<mdlheir1>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_ModuleList.Add(new mdlheir1
                        {
                            created_by = dr_datarow["created_by"].ToString(),
                            task_gid = dr_datarow["task_gid"].ToString(),
                            task_date = dr_datarow["task_date"].ToString(),
                        });
                    }
                    objmodulelist.mdlheir1 = get_ModuleList;
                }
                dt_datatable.Dispose();
                msSQL = " select a.created_by as task_gid,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by ,CASE WHEN a.sub_task IS NULL OR a.sub_task = '' THEN '-' ELSE a.sub_task END AS sub_task,a.tasksheet_gid,DATE_FORMAT(a.task_date, '%d-%m-%Y')as task_date,a.task_gid,a.task_name,a.module_name,a.task_type_name,a.hrs_taken,status " +
                         " from tsk_trn_ttasksheet a left join adm_mst_tuser c on a.created_by = c.user_gid" +
                         " where a.task_date= '" + taskdate + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_List = new List<subfolders>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_List.Add(new subfolders
                        {
                            task_name = dr_datarow["task_name"].ToString(),
                            task_gid = dr_datarow["task_gid"].ToString(),
                            module_name = dr_datarow["module_name"].ToString(),
                            task_type_name = dr_datarow["task_type_name"].ToString(),
                            hrs_taken = dr_datarow["hrs_taken"].ToString(),
                            taskstatus = dr_datarow["status"].ToString(),
                            sub_task = dr_datarow["sub_task"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            tasksheet_gid = dr_datarow["tasksheet_gid"].ToString(),

                        });
                    }
                    objmodulelist.subfolders = get_List;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                  "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                objmodulelist.status = false;

            }
        }
        public void Daclientview(string task_gid, MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " select project_gid,project_name from tsk_mst_ttask2client " +
                        " where task_gid='" + task_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getverticalsList = new List<clientview>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getverticalsList.Add(new clientview
                        {
                            project_gid = dt["project_gid"].ToString(),
                            project_name = dt["project_name"].ToString(),
                        });

                        values.clientview = getverticalsList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public bool DaTeamlistSummary(mdltaskteam values,string employee_gid)
        {
            try
            {
                msSQL = "select count(*) as count from tsk_mst_ttaskmanagement where employee_status<>'Completed In Live'";
                values.task_total_count = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select count(assigned_member) from tsk_mst_ttaskmanagement";
                values.assigned = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT a.team_gid ,a.team_name," +
                       "(select GROUP_CONCAT(assignmanager_name SEPARATOR ',') FROM tsk_mst_tteam2manager as  c2m WHERE c2m.team_gid = a.team_gid) AS assignmanager_name ," +
                       "(select COUNT(assigned_member)  FROM tsk_mst_tteam2member as  c2e WHERE c2e.team_gid = a.team_gid) AS total_member_count " +
                       " FROM tsk_mst_tteam a left join tsk_mst_tteam2manager b on b.team_gid = a.team_gid where b.assignmanager_gid='"+ employee_gid + "'" +
                       " order by a.team_code  desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                values.team_count = dt_datatable.Rows.Count;
                var getdocumentlist = new List<taskteamlist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskteamlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            team_name = dt["team_name"].ToString(),
                            assignmanager_name = dt["assignmanager_name"].ToString(),
                            total_member_count = dt["total_member_count"].ToString(),
                        });
                    }
                    values.team_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public void Dadetailsview(string employee_gid,string module_gid, MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " select a.estimated_hours,DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') as created_date,a.task_code,a.task_typename,a.functionality_name,a.employee_status" +
                    ",a.task_name,a.severity_name, DATE_FORMAT(g.created_date, '%d-%m-%Y %H:%i:%s') as assigned_Date " +
                    "  from tsk_mst_ttaskmanagement a left join (SELECT  task_gid,assigned_member,assigned_member_gid,created_date FROM tsk_mst_ttask2member WHERE (task_gid, created_date) IN (SELECT task_gid,MAX(created_date) FROM  tsk_mst_ttask2member GROUP BY  task_gid )) g on g.task_gid = a.task_gid  " +
                        " where g.assigned_member_gid='" + employee_gid + "' and a.module_gid = '"+ module_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskpendinglist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskpendinglist
                        {
                            task_name = dt["task_name"].ToString(),
                            task_code = dt["task_code"].ToString(),
                            estimated_hours = dt["estimated_hours"].ToString(),
                            functionality_name = dt["functionality_name"].ToString(),
                            task_typename = dt["task_typename"].ToString(),
                            severity_name = dt["severity_name"].ToString(),
                            assigned_Date = dt["assigned_Date"].ToString(),
                            employee_status = dt["employee_status"].ToString(),

                        });
                    }
                    values.taskpending_list = getdocumentlist;

                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Daallmemberview(string assigned_member_gid,MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = " select module_name,count(task_name) as task_count, SUM(CASE WHEN employee_status = 'In Progress' THEN 1 ELSE 0 END) AS progress_count," +
                    " SUM(CASE WHEN employee_status = 'Completed In Live' THEN 1 ELSE 0 END) AS live_count,SUM(CASE WHEN employee_status = 'Hold' THEN 1 ELSE 0 END) AS hold_count," +
                    "  SUM(CASE WHEN employee_status = 'In Testing' THEN 1 ELSE 0 END) AS testing_count,SUM(CASE WHEN employee_status = 'Pending' THEN 1 ELSE 0 END) AS new_count" +
                    " from tsk_mst_ttaskmanagement " +
                    " where assigned_member_gid='" + assigned_member_gid + "'GROUP BY module_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<allmember_list>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new allmember_list
                        {
                            module_name = dt["module_name"].ToString(),
                            hold_count = dt["hold_count"].ToString(),
                            live_count = dt["live_count"].ToString(),
                            testing_count = dt["testing_count"].ToString(),
                            progress_count = dt["progress_count"].ToString(),
                            task_count = dt["task_count"].ToString(),
                            new_count = dt["new_count"].ToString(),
                        });
                    }
                    values.allmember_list = getdocumentlist;

                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Daallmember(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = "select distinct assigned_member,assigned_member_gid from tsk_mst_ttaskmanagement where assigned_member is not null and assigned_member_gid is not null";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<memberdropdown_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new memberdropdown_list
                        {
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_member_gid = dt["assigned_member_gid"].ToString(),
                        });
                        values.memberdropdown_list = getteammemberslist;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Daallsmember(MdlTskTrnTaskManagement values)
        {
            try
            {
                msSQL = "select distinct a.employee_gid as assigned_member_gid,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code)AS assigned_member " +
                    " from hrm_mst_temployee a  left join adm_mst_tuser c on a.user_gid=c.user_gid where user_status<>'N'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<memberdropdown_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new memberdropdown_list
                        {
                            assigned_member = dt["assigned_member"].ToString(),
                            assigned_member_gid = dt["assigned_member_gid"].ToString(),
                        });
                        values.memberdropdown_list = getteammemberslist;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                               "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Dalinechartcount(string MonthYear, taskteamlist values)
        {
            try
            {

                msSQL = " select MONTHNAME(created_date) AS month_name,SUM(CASE WHEN severity_name = 'Show Stopper' THEN 1 ELSE 0 END) AS show_stopper_count,SUM(CASE WHEN severity_name = 'Nice To Have' THEN 1 ELSE 0 END) AS nice_to_have_count," +
                           " SUM(CASE WHEN severity_name = 'Critical Non-Mandatory' THEN 1 ELSE 0 END) AS critical_non_mandatory_count,SUM(CASE WHEN severity_name = 'Critical Mandatory' THEN 1 ELSE 0 END) AS critical_mandatory_count" +
                           " from tsk_mst_ttaskmanagement where employee_status <> 'Completed In Live' AND YEAR(created_date) = '"+ MonthYear + "' GROUP BY MONTH(created_date) ORDER BY MONTH(created_date);";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chart_count>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chart_count
                        {
                            show_stopper_count = dt["show_stopper_count"].ToString(),
                            critical_mandatory_count = dt["critical_mandatory_count"].ToString(),
                            critical_non_mandatory_count = dt["critical_non_mandatory_count"].ToString(),
                            nice_to_have_count = dt["nice_to_have_count"].ToString(),
                            month_name = dt["month_name"].ToString(),
                        });
                        values.chart_count = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public void Dagraphchartcount(string module_gid, taskteamlist values)
        {
            try
            {
                if (module_gid == "undefined")
                {
                    msSQL = " select  COUNT(*) AS total_count,YEAR(completedlive_date) AS created_year" +
                            " from tsk_mst_ttaskmanagement where employee_status = 'Completed In Live' GROUP BY YEAR(completedlive_date) ORDER BY created_year ASC ";
                }
                else {
                    msSQL = " select  COUNT(*) AS total_count,YEAR(completedlive_date) AS created_year" +
                            " from tsk_mst_ttaskmanagement where employee_status = 'Completed In Live' AND module_gid = '" + module_gid + "' GROUP BY YEAR(completedlive_date) ORDER BY created_year ASC ";
                }                
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<graph_count>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new graph_count
                        {
                            total_count = dt["total_count"].ToString(),
                            created_year = dt["created_year"].ToString(),
                        });
                        values.graph_count = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;
            }
        }
        public bool Dadeployadd(tracker_list values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("DPTR");
                msSQL = " insert into tsk_mst_tdeployment_tracker  (" +
                        " deployment_trackergid," +
                        "version,"+
                        " version_number," +
                        " file_name ," +
                        " file_gid ," +
                        " dll_status," +
                        " dependency_status," +
                        " routes_status," +
                        " script_status," +
                        " approval_name, " +
                        " file_description, " +
                        " created_by," +
                        " created_date)" +
                        " VALUES(" +
                        "'" + msGetGid + "'," +
                       "'" + values.version.Replace("'", "''") + "'," +
                        "'" + values.version_number + "'," +
                        "'" + values.file_name.Replace("'", "''") + "'," +
                        "'" + values.file_gid + "'," +
                        "'" + values.dll_status.Replace("'", "''") + "'," +
                        "'" + values.dependency_status + "'," +
                        "'" + values.routes_status.Replace("'", "''") + "'," +
                        "'" + values.script_status.Replace("'", "''") + "'," +
                        "'" + values.approval_name.Replace("'", "''") + "'," +
                        "'" + values.description.Replace("'", "''") + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                for (var i = 0; i < values.module_list3.Count; i++)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("D2ME");
                    msSQL = "INSERT INTO tsk_mst_tdeploy2module (" +
                   "deploymodule_gid, " +
                   "deployment_trackergid, " +
                   "module_gid, " +
                   "module_name, " +
                   "created_by, " +
                   "created_date) " +
                   "VALUES (" +
                   " '" + msGetGid1 + "'," +
                   " '" + msGetGid + "'," +
                   " '" + values.module_list3[i].team_gid + "'," +
                   " '" + values.module_list3[i].team_name + "'," +
                   " '" + user_gid + " '," +
                   " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (values.depmodule_list !=null)
                {
                    for (var i = 0; i < values.depmodule_list.Count; i++)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("DEME");
                        msSQL = "INSERT INTO tsk_mst_tdependencymodule (" +
                       "depency_gid, " +
                       "deployment_trackergid, " +
                       "dependency_module_gid, " +
                       "dependency_module, " +
                       "created_by, " +
                       "created_date) " +
                       "VALUES (" +
                       " '" + msGetGid1 + "'," +
                       " '" + msGetGid + "'," +
                       " '" + values.depmodule_list[i].team_gid + "'," +
                       " '" + values.depmodule_list[i].team_name + "'," +
                       " '" + user_gid + " '," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult == 1)
                {
                    values.message = "Submitted Successfully";
                    values.status = true;
                    values.deployment_trackergid = msGetGid;
                }
                else
                {
                    values.message = "Error While Adding Task";
                    values.status = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public void Dascriptattach(HttpRequest httpRequest, string user_gid, result objResult)
        {

            HttpFileCollection httpFileCollection;
            string final_path, httpsUrl;
            MemoryStream ms_stream = new MemoryStream();
            string lscompany_code = string.Empty;
            string deployment_trackergid = httpRequest.Form["deployment_trackergid"];
            string filelist = httpRequest.Form["filelist"];
            string[] fileListArray = filelist.Split('+');
            String path = lspath;
            HttpPostedFile httpPostedFile;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            path = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "TSK/Script/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

            if ((!System.IO.Directory.Exists(path)))
                System.IO.Directory.CreateDirectory(path);

            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {

                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string project_flag = httpRequest.Form["project_flag"].ToString();
                        FileExtension = Path.GetExtension(FileExtension).ToLower();

                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);

                        byte[] bytes = ms.ToArray();
                        if ((objcmnstorage.CheckIsValidfilename(FileExtension, project_flag) == false) || (objcmnstorage.CheckIsExecutable(bytes) == true))
                        {
                            objResult.status = false;
                            objResult.message = "File format is not supported";
                            return;
                        }

                        bool status;
                        status = objcmnstorage.UploadStream("erpdocument", lscompany_code + "/" + "TSK/Script/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, ms);
                        ms.Close();
                        path = "erpdocument" + "/" + lscompany_code + "/" + "TSK/Script/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        bool status1;
                        msGetGid = objcmnfunctions.GetMasterGID("SCPG");

                        msSQL = " insert into tsk_mst_tscript(" +
                                    " script_gid," +
                                    " deployment_trackergid ," +
                                    " docupload_type ," +
                                    " file_path," +
                                    " created_by," +
                                    " created_date" +
                                    " )values(" +
                                    "'" + msGetGid + "'," +
                                    "'" + deployment_trackergid + "'," +
                                    "'" + httpPostedFile.FileName.Replace("'", @"\'") + "'," +
                                    "'" + path + msdocument_gid + FileExtension + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult != 0)
                    {
                        objResult.status = true;
                        objResult.message = "Document Uploaded Successfully";

                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error Occured While Uploading the document";

                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                objResult.status = false;

            }
        }
        public bool Dadeploysummary(tracker_list values)
        {
            try
            {
                msSQL = " SELECT a.file_description,(select COUNT(module_name)  FROM tsk_mst_tdeploy2module as  c2m WHERE c2m.deployment_trackergid = a.deployment_trackergid) AS total_module_count,a.routes_status,a.version,a.version_number,a.deployment_trackergid,a.file_name,a.file_gid,a.dependency_status,a.script_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_tdeployment_tracker a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid " +
                        " ORDER  BY created_date DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<depmodule_summary>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new depmodule_summary
                        {
                            routes_status = dt["routes_status"].ToString(),
                            version = dt["version"].ToString(),
                            version_number = dt["version_number"].ToString(),
                            deployment_trackergid = dt["deployment_trackergid"].ToString(),
                            file_name = dt["file_name"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            dependency_status = dt["dependency_status"].ToString(),
                            script_status = dt["script_status"].ToString(),
                            description = dt["file_description"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            total_module_count = dt["total_module_count"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                    }
                    values.depmodule_summary = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;

            }
        }
        public void Dadeployview(string deployment_trackergid, tracker_list values)
        {
            try
            {
                msSQL = " select a.file_description,a.dll_status,a.approval_name,a.routes_status,a.version,a.version_number,a.deployment_trackergid,a.file_name,a.file_gid,a.dependency_status,a.script_status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y %H:%i:%s') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by" +
                        " FROM tsk_mst_tdeployment_tracker a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid " +
                        " where deployment_trackergid='" + deployment_trackergid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.routes_status = objODBCDatareader["routes_status"].ToString();
                    values.version = objODBCDatareader["version"].ToString();
                    values.version_number = objODBCDatareader["version_number"].ToString();
                    values.file_name = objODBCDatareader["file_name"].ToString();
                    values.dll_status = objODBCDatareader["dll_status"].ToString();
                    values.file_gid = objODBCDatareader["file_gid"].ToString();
                    values.dependency_status = objODBCDatareader["dependency_status"].ToString();
                    values.script_status = objODBCDatareader["script_status"].ToString();
                    values.description = objODBCDatareader["file_description"].ToString();
                    values.created_date = objODBCDatareader["created_date"].ToString();
                    values.approval_name = objODBCDatareader["approval_name"].ToString();
                    values.created_by = objODBCDatareader["created_by"].ToString();
                }
                msSQL = " select script_gid,docupload_type,file_path from tsk_mst_tscript " +
                         " where deployment_trackergid='" + deployment_trackergid + "'";
                           dt_datatable = objdbconn.GetDataTable(msSQL);
                var getscript = new List<scriptattach_file>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getscript.Add(new scriptattach_file
                        {
                            docupload_type = (dr_datarow["docupload_type"].ToString()),
                            file_path = (dr_datarow["file_path"].ToString()),
                            script_gid = (dr_datarow["script_gid"].ToString()),
                        });

                        values.scriptattach_file = getscript;
                    }
                }
                msSQL = " select module_name,module_gid from tsk_mst_tdeploy2module " +
                        " where deployment_trackergid='" + deployment_trackergid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodule = new List<deploy_module>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmodule.Add(new deploy_module
                        {
                            team_gid = (dr_datarow["module_gid"].ToString()),
                            team_name = (dr_datarow["module_name"].ToString()),
                        });

                        values.deploy_module = getmodule;
                    }
                }
                msSQL = " select dependency_module,dependency_module_gid from tsk_mst_tdependencymodule " +
                      " where deployment_trackergid='" + deployment_trackergid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdependency = new List<deploydependcy_module>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getdependency.Add(new deploydependcy_module
                        {
                            team_gid = (dr_datarow["dependency_module_gid"].ToString()),
                            team_name = (dr_datarow["dependency_module"].ToString()),
                        });

                        values.deploydependcy_module = getdependency;
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;

            }
        }
        public bool Dadeploy(tracker_list values, string user_gid)
        {
            try
            {
                msSQL = " update tsk_mst_tdeployment_tracker set " +
                        " version ='" + values.version.Replace("'", "''") + "', " +
                        " version_number ='" + values.version_number.Replace("'", "''") + "', " +
                        " file_name ='" + values.file_name.Replace("'", "''") + "', " +
                        " file_gid ='" + values.file_gid + "', " +
                        " dll_status ='" + values.dll_status.Replace("'", "''") + "', " +
                        " dependency_status ='" + values.dependency_status.Replace("'", "''") + "', " +
                        " routes_status ='" + values.routes_status.Replace("'", "''") + "', " +
                        " script_status ='" + values.script_status.Replace("'", "''") + "', " +
                        " approval_name ='" + values.approval_name.Replace("'", "''") + "', " +
                        " file_description ='" + values.description.Replace("'", "''") + "', " +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where deployment_trackergid='" + values.deployment_trackergid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "delete from tsk_mst_tdeploy2module where deployment_trackergid='" + values.deployment_trackergid + "'";
                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult1 == 1)
                {
                    for (var i = 0; i < values.module_list3.Count; i++)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("D2ME");
                        msSQL = "INSERT INTO tsk_mst_tdeploy2module (" +
                       "deploymodule_gid, " +
                       "deployment_trackergid, " +
                       "module_gid, " +
                       "module_name, " +
                       "created_by, " +
                       "created_date) " +
                       "VALUES (" +
                       " '" + msGetGid1 + "'," +
                       " '" + values.deployment_trackergid + "'," +
                       " '" + values.module_list3[i].team_gid + "'," +
                       " '" + values.module_list3[i].team_name + "'," +
                       " '" + user_gid + " '," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }
                msSQL = "delete from tsk_mst_tdependencymodule where deployment_trackergid='" + values.deployment_trackergid + "'";
                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult2 == 1)
                {
                    if (values.depmodule_list != null)
                {
                        for (var i = 0; i < values.depmodule_list.Count; i++)
                        {
                            msGetGid1 = objcmnfunctions.GetMasterGID("DEME");
                            msSQL = "INSERT INTO tsk_mst_tdependencymodule (" +
                           "depency_gid, " +
                           "deployment_trackergid, " +
                           "dependency_module_gid, " +
                           "dependency_module, " +
                           "created_by, " +
                           "created_date) " +
                           "VALUES (" +
                           " '" + msGetGid1 + "'," +
                           " '" + values.deployment_trackergid + "'," +
                           " '" + values.depmodule_list[i].team_gid + "'," +
                           " '" + values.depmodule_list[i].team_name + "'," +
                           " '" + user_gid + " '," +
                           " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                if (mnResult == 1)
                {
                    values.message = "Updated Successfully";
                    values.status = true;
                }
                else
                {
                    values.message = "Error While Update";
                    values.status = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public void Daversioncheck(string version_number, taskaddlist values)
        {
            try
            {
                msSQL = "select deployment_trackergid from tsk_mst_tdeployment_tracker" +
                    " where  version_number='" + version_number.Replace("'", "''") + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.status = false;
                    return;
                }
                values.status = true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                              "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;

            }
        }
        public void Daversioneditcheck(string version_number,string deployment_trackergid, taskaddlist values)
        {
            try
            {
                msSQL = "select version_number from tsk_mst_tdeployment_tracker" +
                    " where  version_number='" + version_number.Replace("'", "''") + "' and team_gid !='" + deployment_trackergid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.status = false;
                    return;
                }
                values.status = true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                              "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return;

            }
        }
        public void Dascriptdelete(string script_gid, string employee_gid, projectlist values)
        {
            try
            {
                msSQL = "delete from tsk_mst_tscript where script_gid='" + script_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Script Deleted Successfully";
                    values.status = true;
                }
                else
                {
                    values.message = "Error while Deleting Script";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
    }
}