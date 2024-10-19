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

namespace ems.system.DataAccess
{
    public class DaTskMstCustomer
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
        string msGetGid, msGetGid1, msGetcadteamcode, msGetCode;
        DataTable dt_datatable;
        int mnResult, mnResult1;
        public bool DaCustomerSummary(MdlTskMstCustomer values)
        {
            try
            {
                msSQL = " SELECT a.project_gid,a.project_code,a.project_name,a.status,case when a.status='N' then 'Inactive' else 'Active' end as status," +
                        "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by," +
                         "(select COUNT(team_name)  FROM tsk_mst_tproject2module as  c2m WHERE c2m.project_gid = a.project_gid) AS total_module_count " +
                        " FROM tsk_mst_tproject a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid " +
                        " ORDER  BY project_code DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<projectlist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new projectlist
                        {
                            project_gid = dt["project_gid"].ToString(),
                            project_code = dt["project_code"].ToString(),
                            project_name = dt["project_name"].ToString(),
                            status_log = dt["status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            total_module_count = dt["total_module_count"].ToString(),
                        });
                    }
                    values.project_list = getdocumentlist;
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
        public bool DaCustomeradd(projectlist values, string user_gid)
        {
            try
            {
                msSQL = " select project_name from tsk_mst_tproject " +
                        " where LOWER (project_name) = '" + values.project_name.Replace("'", "\\'").ToLower() + "'" +
                        " and project_gid !='" + values.project_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.message = "Customer Already Exists";
                    values.status = false;
                    return false;
                }

                msGetGid = objcmnfunctions.GetMasterGID("PRTG");
                msGetCode = objcmnfunctions.GetMasterGID("PRTC");
                msGetCode = "PRTC" + msGetCode;
                msSQL = " insert into tsk_mst_tproject  (" +
                        " project_gid," +
                        " project_code," +
                        " project_name," +
                        " projectname_gid," +
                        " created_by," +
                        " created_date)" +
                        " VALUES(" +
                        "'" + msGetGid + "'," +
                        "'" + msGetCode + "'," +
                        "'" + values.project_name.Replace("'", "''") + "'," +
                        "'" + values.projectname_gid + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Customer Added Successfully";
                    values.status = true;
                }
                else
                {
                    values.message = "Error While Adding Customer";
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
        public void Daprojectlist(MdlTskMstCustomer values)
        {
            try
            {
                msSQL = " select customer_gid,customer_name from crm_mst_tcustomer " +
                       " where status='Active'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getverticalsList = new List<customerlist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getverticalsList.Add(new customerlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
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
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public void DaCustomeredit(string project_gid, projectlist values)
        {
            try
            {
                msSQL = " select project_gid,projectname_gid,project_name,project_code,status as Status from tsk_mst_tproject" +
                        " where project_gid='" + project_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.project_gid = objODBCDatareader["project_gid"].ToString();
                    values.project_code = objODBCDatareader["project_code"].ToString();
                    values.project_name = objODBCDatareader["project_name"].ToString();
                    values.projectname_gid = objODBCDatareader["projectname_gid"].ToString();
                    values.status_log = objODBCDatareader["Status"].ToString();
                }
                msSQL = " select project2module_gid ,team_gid,team_name from tsk_mst_tproject2module " +
                        " where project_gid='" + project_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getverticalsList = new List<listteam>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getverticalsList.Add(new listteam
                        {
                            team_gid = dt["team_gid"].ToString(),
                            team_name = dt["team_name"].ToString(),
                        });

                        values.listteam = getverticalsList;
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
        public void DaUpdatecustomer(string user_gid, projectlist values)
        {
            try
            {
                msSQL = " select project_name from tsk_mst_tproject " +
                     " where LOWER (project_name) = '" + values.project_name.Replace("'", "\\'").ToLower() + "'" +
                     " and project_gid !='" + values.project_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {

                    values.status = false;
                    values.message = "Customer Already Exist";
                    return;
                }
                msSQL = " select project_name from  tsk_mst_tproject where project_gid ='" + values.project_gid + "' ";
                string lsproject = objdbconn.GetExecuteScalar(msSQL);

                if (values.project_name == lsproject)
                {
                    values.status = false;
                    values.message = "No changes in Customer Name";
                    return;
                }
                msSQL = " update tsk_mst_tproject set " +
                        " projectname_gid ='" + values.projectname_gid.Replace("'", "''") + "', " +
                        " project_name ='" + values.project_name.Replace("'", "''") + "', " +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where project_gid='" + values.project_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Customer Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Customer";
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
        public void DaCustomerdelete(string project_gid, string employee_gid, projectlist values)
        {
            try
            {
                msSQL = " select team_gid FROM tsk_mst_ttask2client " +
                     " where project_gid='" + project_gid + "'";
                values.team_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select CASE WHEN EXISTS (SELECT 1 FROM tsk_mst_ttaskmanagement a where a.module_gid='" + values.team_gid + "' AND a.completed_flag = 'N')   THEN 'N' ELSE 'Y'   END AS completed_flag";
                //msSQL = "select a.completed_flag from tsk_mst_ttaskmanagement a where a.module_gid='" + values.team_gid + "'";
                string completed_flag = objdbconn.GetExecuteScalar(msSQL);
                if (completed_flag == "N")
                {
                    values.status = false;
                    values.message = "Customer is Tagged to the Task So..you can't Delete..!";
                    return;
                }
                msSQL = "delete from tsk_mst_tproject where project_gid='" + project_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "delete from tsk_mst_tproject2module where project_gid='" + project_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Customer Deleted Successfully";
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
                ex.StackTrace.ToString();
                values.status = false;

            }
        }
        public void Dagetassignteam(string user_gid, projectlist values)
        {
            try
            {
                string lsproject_name = objdbconn.GetExecuteScalar("select project_name from tsk_mst_tproject where project_gid='" + values.project_gid + "'");
                msSQL = "delete from tsk_mst_tproject2module where project_gid='" + values.project_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    for (var i = 0; i < values.listteam.Count; i++)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("PM2G");
                        msSQL = " insert into tsk_mst_tproject2module (" +
                                                       " project2module_gid," +
                                                       " team_gid," +
                                                       " team_name," +
                                                       " project_gid," +
                                                       " project_name," +
                                                       " updated_by, " +
                                                       " updated_date)" +
                                                       " values(" +
                                                       " '" + msGetGid1 + "'," +
                                                       " '" + values.listteam[i].team_gid + "'," +
                                                       " '" + values.listteam[i].team_name + "'," +
                                                       " '" + values.project_gid + "'," +
                                                       " '" + lsproject_name.Replace("'", "''") + "', " +
                                                       "'" + user_gid + "'," +
                                                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Module assigned successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while updating Module";
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
        public void DaGetUnassignedModulelist(string project_gid, MdlTskMstCustomer values)
        {
            try
            {

                msSQL = " select a.teamname_gid,a.team_name,a.team_gid,'" + project_gid + "' as project_gid" +
              " from tsk_mst_tteam a" +
              " where status<>'N' and a.team_gid not in" +
              " (select team_gid from" +
              " tsk_mst_tproject2module" +
              " where project_gid= '" + project_gid + "') order by team_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassignedmodulelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassignedmodulelist
                        {
                            project_gid = dt["project_gid"].ToString(),
                            teamname_gid = dt["teamname_gid"].ToString(),
                            team_name = dt["team_name"].ToString(),
                            team_gid = dt["team_gid"].ToString(),
                        });
                        values.GetUnassignedmodule_list = getModuleList;
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
        public void DaGetAssignedmodulelist(string project_gid, MdlTskMstCustomer values)
        {
            try
            {

                msSQL = " select distinct  a.teamname_gid,a.team_name,a.team_gid,'" + project_gid + "' as project_gid" +
                    " from tsk_mst_tteam a" +
                    " where status<>'N' and a.team_gid in " +
                    " (select team_gid from" +
                    " tsk_mst_tproject2module" +
                    " where project_gid= '" + project_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedmodulelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedmodulelist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            team_name = dt["team_name"].ToString(),
                            project_gid = dt["project_gid"].ToString(),
                            teamname_gid = dt["teamname_gid"].ToString(),
                        });
                        values.GetAssignedmodule_list = getModuleList;
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

        //team
        public bool DaTeamSummary(mdltaskteam values)
        {
            try
            {
                msSQL = " SELECT a.teamname_gid,a.team_code,a.team_gid ,a.status,a.team_name,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date," +
                       " concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code) as created_by, " +
                       "(select COUNT(assignmanager_name)  FROM tsk_mst_tteam2manager as  c2m WHERE c2m.team_gid = a.team_gid) AS total_manager_count ," +
                       "(select COUNT(assigned_member)  FROM tsk_mst_tteam2member as  c2e WHERE c2e.team_gid = a.team_gid) AS total_member_count," +
                       " case when a.status='Inactive' then 'Inactive' else 'Active' end as status" +
                       " FROM tsk_mst_tteam a" +
                       " left join hrm_mst_temployee b on a.created_by = b.user_gid" +
                       " left join adm_mst_tuser c on c.user_gid = b.user_gid" +
                       " order by a.team_code  desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<taskteamlist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new taskteamlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            team_code = dt["team_code"].ToString(),
                            team_name = dt["team_name"].ToString(),
                            teamname_gid = dt["teamname_gid"].ToString(),
                            total_manager_count = dt["total_manager_count"].ToString(),
                            total_member_count = dt["total_member_count"].ToString(),
                            status_log = dt["status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
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
        public bool DaTeamList(mdltaskteam values)
        {
            try
            {
                msSQL = " SELECT a.team_code,a.team_gid,a.team_name" +
                       " FROM tsk_mst_tteam a" +
                       " left join tsk_mst_ttaskmanagement b on b.module_gid=a.team_gid where a.team_gid not in (select module_gid from tsk_mst_ttaskmanagement)" +
                       " order by a.team_gid  desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<module_list>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new module_list
                        {
                            team_gid = dt["team_gid"].ToString(),
                            team_code = dt["team_code"].ToString(),
                            team_name = dt["team_name"].ToString(),
                        });
                    }
                    values.module_list = getdocumentlist;
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
        public bool Dateamadd(taskteamlist values, string user_gid)
        {
            try
            {
                msSQL = " select team_name from tsk_mst_tteam " +
                        " where LOWER (team_name) = '" + values.team_name.Replace("'", "\\'").ToLower() + "'" +
                        " and team_gid !='" + values.team_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.message = "Module Already Exists";
                    values.status = false;
                    return false;
                }

                msGetGid = objcmnfunctions.GetMasterGID("TSTG");
                msGetCode = objcmnfunctions.GetMasterGID("TSTC");
                msGetCode = "TSTC" + msGetCode;
                msSQL = " insert into tsk_mst_tteam  (" +
                        " team_gid," +
                        " team_code," +
                        " team_name," +
                         " teamname_gid," +
                        " process," +
                        " created_by," +
                        " created_date)" +
                        " VALUES(" +
                        "'" + msGetGid + "'," +
                        "'" + msGetCode + "'," +
                        "'" + values.team_name.Replace("'", "''") + "'," +
                        "'" + values.teamname_gid + "'," +
                        "'" + values.process + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Module Added Successfully";
                    values.status = true;
                }
                else
                {
                    values.message = "Error While Adding Module";
                    values.status = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string logMessage = $"*******Date*****{DateTime.Now.ToString("yyyy - MM - dd HH:mm:ss")} ***********" +
                                      $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name} ***********" +
                                      $" ******** {msSQL} ********" +
                                      $" {ex.Message.ToString()} ***********";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;
                return false;
            }
        }
        public void Dateamedit(string team_gid, taskteamlist values)
        {
            try
            {
                msSQL = " select process,teamname_gid,team_gid,team_name,team_code,status as Status from tsk_mst_tteam" +
                        " where team_gid='" + team_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.team_gid = objODBCDatareader["team_gid"].ToString();
                    values.teamname_gid = objODBCDatareader["teamname_gid"].ToString();
                    values.team_code = objODBCDatareader["team_code"].ToString();
                    values.team_name = objODBCDatareader["team_name"].ToString();
                    values.process = objODBCDatareader["process"].ToString();
                    values.status_log = objODBCDatareader["Status"].ToString();
                }
                msSQL = "SELECT assignmanager_gid,assignmanager_name " +
                 "FROM tsk_mst_tteam2manager  " +
                 " where team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammanagerlist = new List<assignman_list>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammanagerlist.Add(new assignman_list
                        {
                            employee_gid = dt["assignmanager_gid"].ToString(),
                            employee_name = dt["assignmanager_name"].ToString(),
                        });
                        values.assignman_list = getteammanagerlist;
                    }
                }
                msSQL = "SELECT assigned_member_gid,assigned_member " +
                        "FROM tsk_mst_tteam2member  " +
                        " where team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<assignmemlist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new assignmemlist
                        {
                            employee_gid = dt["assigned_member_gid"].ToString(),
                            employee_name = dt["assigned_member"].ToString(),
                        });
                        values.assignmem_list = getteammemberslist;
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
        public void DaUpdateteam(string user_gid, taskteamlist values)
        {
            try
            {
                msSQL = " select team_name from tsk_mst_tteam " +
                     " where LOWER (team_name) = '" + values.team_name.Replace("'", "\\'").ToLower() + "'" +
                     " and team_gid !='" + values.team_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {

                    values.status = false;
                    values.message = "Module Already Exist";
                    return;
                }
                msSQL = " update tsk_mst_tteam set " +
                        " team_name ='" + values.team_name.Replace("'", "''") + "', " +
                          " teamname_gid ='" + values.teamname_gid + "', " +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where team_gid='" + values.team_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Module Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Module";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/TSK/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                values.status = false;

            }
        }
        public void Dataskdelete(string team_gid, string employee_gid, projectlist values)
        {
            try
            {
                msSQL = "select CASE WHEN EXISTS (SELECT 1 FROM tsk_mst_ttaskmanagement a where a.module_gid='" + team_gid + "' AND a.completed_flag = 'N')   THEN 'N' ELSE 'Y'   END AS completed_flag";

                //msSQL = "select a.completed_flag from tsk_mst_ttaskmanagement a where a.module_gid='" + team_gid + "'";
                string completed_flag = objdbconn.GetExecuteScalar(msSQL);
                if (completed_flag == "N")
                {
                    values.status = false;
                    values.message = "Member is Tagged to the Task So..you can't Delete..!";
                    return;
                }

                msSQL = "delete from tsk_mst_tteam where team_gid='" + team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "DELETE FROM tsk_mst_tteam2manager WHERE team_gid='" + team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "DELETE FROM tsk_mst_tteam2member WHERE team_gid='" + team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.message = "Module Deleted Successfully";
                    values.status = true;
                }
                else
                {
                    values.message = "Error while Deleting Module";
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
        public void DaAddassignmanager(string user_gid, taskteamlist values)
        {
            try
            {
                msSQL = "delete from tsk_mst_tteam2manager where team_gid='" + values.team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {

                    for (var i = 0; i < values.assignman_list.Count; i++)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("TA2M");
                        msSQL = "INSERT INTO tsk_mst_tteam2manager (" +
                       "team2manager_gid, " +
                       "team_gid, " +
                       "assignmanager_gid, " +
                       "assignmanager_name, " +
                       "created_by, " +
                       "created_date) " +
                       "VALUES (" +
                       " '" + msGetGid1 + "'," +
                       " '" + values.team_gid + "'," +
                       " '" + values.assignman_list[i].employee_gid + "'," +
                       " '" + values.assignman_list[i].employee_name + "'," +
                       " '" + user_gid + " '," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Manager assigned to Module successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Manager Module";
                        }
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
        public void DaGetAssignedlist(string team_gid, taskteamlist values)
        {
            try
            {

                msSQL = " select distinct fg.team2member_gid, fg.team_gid,c.user_gid,a.employee_gid,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code)AS employee_name,'" + team_gid + "' as team_gid" +
                   " from hrm_mst_temployee a" +
                   " left join adm_mst_tuser c on a.user_gid=c.user_gid  left join tsk_mst_tteam2member g on g.assigned_member_gid=a.employee_gid INNER JOIN tsk_mst_tteam2member fg ON fg.assigned_member_gid = a.employee_gid" +
                   " where user_status<>'N' and fg.team_gid= '" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            user_gid = dt["team2member_gid"].ToString(),

                        });
                        values.GetAssignedlist = getModuleList;
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
        public void DaGetUnassignedlist(string team_gid, taskteamlist values)
        {
            try
            {

                msSQL = " select c.user_gid,a.employee_gid,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code)AS employee_name,'" + team_gid + "' as team_gid" +
              " from hrm_mst_temployee a" +
              " left join adm_mst_tuser c on a.user_gid=c.user_gid" +
              " where user_status<>'N' and a.employee_gid not in" +
              " (select assigned_member_gid from" +
              " tsk_mst_tteam2member" +
              " where team_gid= '" + team_gid + "') order by employee_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassignedlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),

                        });
                        values.GetUnassignedlist = getModuleList;
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
        public void DaAddassignmember(string user_gid, taskteamlist values)
        {
            try
            {
                msSQL = "delete from tsk_mst_tteam2member where team_gid='" + values.team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {

                    for (var i = 0; i < values.assignmem_list.Count; i++)
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("T2AM");
                        msSQL = "INSERT INTO tsk_mst_tteam2member (" +
                       "team2member_gid, " +
                       "team_gid, " +
                       "assigned_member_gid, " +
                       "assigned_member, " +
                       "created_by, " +
                       "created_date) " +
                       "VALUES (" +
                       " '" + msGetGid1 + "'," +
                       " '" + values.team_gid + "'," +
                       " '" + values.assignmem_list[i].employee_gid + "'," +
                       " '" + values.assignmem_list[i].employee_name + "'," +
                       " '" + user_gid + " '," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Member assigned to Module successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Member Assign";
                        }
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
        public void DaGetUnassignedmanagerlist(string team_gid, taskteamlist values)
        {
            try
            {

                msSQL = " select a.employee_gid,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code)AS employee_name,'" + team_gid + "' as team_gid" +
              " from hrm_mst_temployee a" +
              " left join adm_mst_tuser c on a.user_gid=c.user_gid" +
              " where user_status<>'N' and a.employee_gid not in" +
              " (select assignmanager_gid from" +
              " tsk_mst_tteam2manager" +
              " where team_gid= '" + team_gid + "') order by employee_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassignedlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetUnassignedlist = getModuleList;
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
        public void DaGetAssignedmanagerlist(string team_gid, taskteamlist values)
        {
            try
            {

                msSQL = " select distinct a.employee_gid,concat(c.user_firstname,' ',c.user_lastname,' || ',c.user_code)AS employee_name,'" + team_gid + "' as team_gid" +
                    " from hrm_mst_temployee a" +
                    " left join adm_mst_tuser c on a.user_gid=c.user_gid" +
                    " where user_status<>'N' and a.employee_gid in " +
                    " (select assignmanager_gid from" +

                    " tsk_mst_tteam2manager" +
                    " where team_gid= '" + team_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedlist
                        {
                            team_gid = dt["team_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetAssignedlist = getModuleList;
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

        // task creation
        public void Daeditmanager(string team_gid, taskteamlist values)
        {
            try
            {
                msSQL = "SELECT assigned_member_gid,assigned_member " +
                        "FROM tsk_mst_ttask2member  " +
                        " where team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<assignmemlist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new assignmemlist
                        {
                            employee_gid = dt["assigned_member_gid"].ToString(),
                            employee_name = dt["assigned_member"].ToString(),
                        });
                        values.assignmem_list = getteammemberslist;
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
        public void Datransferlist(string team_gid, taskteamlist values,string employee_gid)
        {
            try
            {
                msSQL = "SELECT DISTINCT a.assignmanager_name as member_name,a.assignmanager_gid as member_gid from tsk_mst_tteam2manager a LEFT JOIN tsk_mst_ttask2member c on c.assigned_member_gid=a.assignmanager_gid where a.assignmanager_gid " +
                    " not in (select assigned_member_gid from tsk_mst_ttask2member where assigned_member_gid = '"+ employee_gid +"') and a.team_gid='" + team_gid + "'" +
                    " union  SELECT DISTINCT a.assigned_member as member_name,a.assigned_member_gid as member_gid   " +
                    " FROM tsk_mst_tteam2member a LEFT JOIN tsk_mst_ttask2member c on c.assigned_member_gid=a.assigned_member_gid where a.assigned_member_gid not in (select assigned_member_gid from tsk_mst_ttask2member where assigned_member_gid = '"+ employee_gid +"')" +
                        " and a.team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<assignmemlist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new assignmemlist
                        {
                            employee_gid = dt["member_gid"].ToString(),
                            employee_name = dt["member_name"].ToString(),
                        });
                        values.assignmem_list = getteammemberslist;
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
        public void Damanagerlist(string team_gid, taskteamlist values)
        {
            try
            {
                msSQL = "select DISTINCT assignmanager_name as member_name,assignmanager_gid as member_gid from tsk_mst_tteam2manager where team_gid='" + team_gid + "' union  SELECT DISTINCT assigned_member as member_name,assigned_member_gid as member_gid " +
                        "FROM tsk_mst_tteam2member  " +
                        " where team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammemberslist = new List<assignmemlist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammemberslist.Add(new assignmemlist
                        {
                            employee_gid = dt["member_gid"].ToString(),
                            employee_name = dt["member_name"].ToString(),
                        });
                        values.assignmem_list = getteammemberslist;
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
        public void Daconditionmember(string team_gid, taskteamlist values)
        {
            try
            {

                string lsmember_gid = objdbconn.GetExecuteScalar("select distinct assigned_member_gid from tsk_mst_ttask2member  where team_gid='" + team_gid + "'");
                //string lsuser_gid = objdbconn.GetExecuteScalar("select a.user_gid from  adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid = a.user_gid where b.employee_gid='" + lsmember_gid + "'");

                msSQL = "select a.assigned_member_gid from tsk_mst_tteam2member a where a.assigned_member_gid='" + lsmember_gid + "'";
                objODBCDatareader = objdbconn.GetDataReader(msSQL);
                if (objODBCDatareader.HasRows)
                {
                    values.status = false;
                    values.message = "Member is Tagged to the Task So..you can't UnAssign..!";
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
        public void Dalevelone_menu(taskteamlist values)
        {
            try
            {
                msSQL = "select module_name,module_gid from adm_mst_tmoduleangular where menu_level=1";
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
    }
}