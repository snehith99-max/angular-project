﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ems.utilities.Functions;
using ems.system.Models;
//using ems.storage.Functions;
using System.Data.Odbc;


namespace ems.system.DataAccess
{
    public class DaSystemMaster
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        DataTable dt_datatable;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        string msSQL, msGetGid, msGet_LocationGid, clusterGID, msGet_clusterGid, regionGID, msGet_regionGid, msGetTaskCode, msGetUserCode,
            msGetTask2AssignedToGid, msGetTask2EscalationMailToGid, msGetHRCode, msGetHR2NotifyToGid, msGetAPICode, msGetsystem_ownername_gid;
        int mnResult, mnResultSub1, mnResultSub2;
        string lslevelonemodule_gid, module_gid, lsleveltwomodule_gid, module_gid_parent, lsleveltwomodulestatus_gid, lslevelonemodulestatus_gid;
        string lsmaster_value, lslms_code, lsbureau_code, lsbase_value, lssalutation_value, lsproject_value, lsbloodgroup_value, lsdocumentgid;
        string lsleveloneparent_gid, lsleveltwoparent_gid, lslevelthreeparent_gid, lsleveltwo_name, lslevelone_name,
           lslevelthree_name1, lsleveltwo_name1, lslevelone_name1;
        string lscreated_date, lscreated_by, lsleveltwomodule1_gid, lslevelthreeparent1_gid, lsleveltwoparent1_gid, lsleveloneparent1_gid,
             lsleveltwomodulemenu_gid, lslevelonemodulemenu_gid, lsuser_gid;
        string lsemployee_gid, lsemployee_name, lsemployeegroup_gid, lsemployeegroup_name, lslevelfourparent_gid, lslevelthree_name, lslevelthreemodule_gid;
        string lsuser_code, lsexternalsystem_name;

        public void DaGetFirstLevelMenu(menu objmaster)
        {
            try
            {
                msSQL = " SELECT module_gid,module_name FROM adm_mst_tmodule where module_gid_parent='$'" +
                        " order by display_order asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmenu_list = new List<menu_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmenu_list.Add(new menu_list
                        {
                            module_gid = (dr_datarow["module_gid"].ToString()),
                            module_name = (dr_datarow["module_name"].ToString()),
                        });
                    }
                    objmaster.menu_list = getmenu_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;
            }
        }
        public void DaGetSecondLevelMenu(menu objmaster, string module_gid_parent)
        {
            try
            {
                msSQL = " SELECT module_gid,module_name FROM adm_mst_tmodule where module_gid_parent='" + module_gid_parent + "'" +
                        " order by display_order asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmenu_list = new List<menu_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmenu_list.Add(new menu_list
                        {
                            module_gid = (dr_datarow["module_gid"].ToString()),
                            module_name = (dr_datarow["module_name"].ToString()),
                        });
                    }
                    objmaster.menu_list = getmenu_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch
            {
                objmaster.status = false;
            }
        }
        public void DaGetThirdLevelMenu(menu objmaster, string module_gid_parent)
        {
            try
            {
                msSQL = " SELECT module_gid,module_name FROM adm_mst_tmodule where module_gid_parent='" + module_gid_parent + "'" +
                        "  order by display_order asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmenu_list = new List<menu_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmenu_list.Add(new menu_list
                        {
                            module_gid = (dr_datarow["module_gid"].ToString()),
                            module_name = (dr_datarow["module_name"].ToString()),
                        });
                    }
                    objmaster.menu_list = getmenu_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch
            {
                objmaster.status = false;
            }
        }

        public void DaGetFourthLevelMenu(menu objmaster, string module_gid_parent)
        {
            try
            {
                msSQL = " SELECT module_gid,module_name FROM adm_mst_tmodule where module_gid_parent='" + module_gid_parent + "'" +
                        " order by display_order asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmenu_list = new List<menu_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmenu_list.Add(new menu_list
                        {
                            module_gid = (dr_datarow["module_gid"].ToString()),
                            module_name = (dr_datarow["module_name"].ToString()),
                        });
                    }
                    objmaster.menu_list = getmenu_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch
            {
                objmaster.status = false;
            }
        }

        public void DaPostMenudAdd(menu values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT module_gid_parent FROM adm_mst_tmodule where module_gid='" + values.module_gid + "'";
                lslevelfourparent_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_gid_parent FROM adm_mst_tmodule where module_gid='" + lslevelfourparent_gid + "'";
                lslevelthreeparent_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_gid_parent FROM adm_mst_tmodule where module_gid='" + lslevelthreeparent_gid + "'";
                lsleveltwoparent_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_gid_parent FROM adm_mst_tmodule where module_gid='" + lsleveltwoparent_gid + "'";
                lsleveloneparent_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_name FROM adm_mst_tmodule where module_gid='" + lslevelfourparent_gid + "'";
                lslevelthree_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_name FROM adm_mst_tmodule where module_gid='" + lslevelthreeparent_gid + "'";
                lsleveltwo_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_name FROM adm_mst_tmodule where module_gid='" + lsleveltwoparent_gid + "'";
                lslevelone_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + lsleveltwoparent_gid + "'";
                lslevelonemodule_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + lslevelthreeparent_gid + "'";
                lsleveltwomodule_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + lslevelfourparent_gid + "'";
                lslevelthreemodule_gid = objdbconn.GetExecuteScalar(msSQL);

                if (String.IsNullOrEmpty(lslevelonemodule_gid))
                {
                    msGetGid = objcmnfunctions.GetMasterGID("MENU");
                    msSQL = " insert into sys_mst_tmenumapping(" +
                            " menu_gid," +
                            " module_gid_parent ," +
                            " module_gid ," +
                            " module_name," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + lsleveloneparent_gid + "'," +
                            "'" + lsleveltwoparent_gid + "'," +
                            "'" + lslevelone_name + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + lsleveltwoparent_gid + "' and status ='Y'";
                    lslevelonemodulestatus_gid = objdbconn.GetExecuteScalar(msSQL);

                    if (String.IsNullOrEmpty(lslevelonemodulestatus_gid))
                    {

                        msSQL = " Update sys_mst_tmenumapping set status ='Y' where module_gid='" + lsleveltwoparent_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                if (String.IsNullOrEmpty(lsleveltwomodule_gid))
                {
                    msGetGid = objcmnfunctions.GetMasterGID("MENU");
                    msSQL = " insert into sys_mst_tmenumapping(" +
                            " menu_gid," +
                            " module_gid_parent ," +
                            " module_gid ," +
                            " module_name," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + lsleveltwoparent_gid + "'," +
                            "'" + lslevelthreeparent_gid + "'," +
                            "'" + lsleveltwo_name + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + lslevelthreeparent_gid + "'and status ='Y'";
                    lsleveltwomodulestatus_gid = objdbconn.GetExecuteScalar(msSQL);

                    if (String.IsNullOrEmpty(lsleveltwomodulestatus_gid))
                    {

                        msSQL = " Update sys_mst_tmenumapping set status ='Y' where module_gid='" + lslevelthreeparent_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (String.IsNullOrEmpty(lslevelthreemodule_gid))
                {
                    msGetGid = objcmnfunctions.GetMasterGID("MENU");
                    msSQL = " insert into sys_mst_tmenumapping(" +
                            " menu_gid," +
                            " module_gid_parent ," +
                            " module_gid ," +
                            " module_name," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + lslevelthreeparent_gid + "'," +
                            "'" + lslevelfourparent_gid + "'," +
                            "'" + lslevelthree_name + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + lslevelfourparent_gid + "'and status ='Y'";
                    lsleveltwomodulestatus_gid = objdbconn.GetExecuteScalar(msSQL);

                    if (String.IsNullOrEmpty(lsleveltwomodulestatus_gid))
                    {

                        msSQL = " Update sys_mst_tmenumapping set status ='Y' where module_gid='" + lslevelfourparent_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                msGetAPICode = objcmnfunctions.GetApiMasterGID("MMAC");
                msGetGid = objcmnfunctions.GetMasterGID("MENU");
                msSQL = " insert into sys_mst_tmenumapping(" +
                        " menu_gid," +
                        " api_code," +
                        " module_gid_parent," +
                        " module_gid ," +
                        " module_name," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + msGetGid + "'," +
                        "'" + msGetAPICode + "'," +
                        "'" + lslevelfourparent_gid + "'," +
                        "'" + values.module_gid + "'," +
                        "'" + values.module_name + "'," +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Menu Added successfully";
                }
                else
                {
                    values.message = "Error Occured while Adding";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMenuMappingSummary(menu objmaster)
        {
            try
            {
                msSQL = " SELECT a.menu_gid,a.module_gid ,a.module_name, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by,api_code," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_tmenumapping a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where module_gid like '%_________%' order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmenusummary_list = new List<menusummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmenusummary_list.Add(new menusummary_list
                        {
                            menu_gid = (dr_datarow["menu_gid"].ToString()),
                            module_gid = (dr_datarow["module_gid"].ToString()),
                            module_name = (dr_datarow["module_name"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                            api_code = (dr_datarow["api_code"].ToString())
                        });
                    }
                    objmaster.menusummary_list = getmenusummary_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch
            {
                objmaster.status = false;
            }
        }

        public void DaGetMenuMappingEdit(string menu_gid, menu values)
        {
            try
            {
                msSQL = " select menu_gid, module_gid_parent, module_gid, module_name, status as Status from sys_mst_tmenumapping " +
                        " where menu_gid='" + menu_gid + "' ";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.menu_gid = objOdbcDataReader["menu_gid"].ToString();
                    values.module_gid_parent = objOdbcDataReader["module_gid_parent"].ToString();
                    values.module_gid = objOdbcDataReader["module_gid"].ToString();
                    values.module_name = objOdbcDataReader["module_name"].ToString();
                    values.Status = objOdbcDataReader["Status"].ToString();
                }
                objOdbcDataReader.Close();
                values.status = true;

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMenuMappingInactivate(menu values, string employee_gid)
        {
            try
            {
                msSQL = " update sys_mst_tmenumapping set status='" + values.rbo_status + "'" +
                        " where menu_gid ='" + values.menu_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    msSQL = " SELECT module_gid_parent FROM sys_mst_tmenumapping where menu_gid='" + values.menu_gid + "'";
                    module_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " SELECT module_gid_parent FROM sys_mst_tmenumapping where module_gid='" + module_gid + "'";
                    module_gid_parent = objdbconn.GetExecuteScalar(msSQL);
                    if (values.rbo_status == 'N')
                    {
                        msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid_parent='" + module_gid + "' and status='Y'";
                        lsleveltwoparent_gid = objdbconn.GetExecuteScalar(msSQL);

                        if (String.IsNullOrEmpty(lsleveltwoparent_gid))
                        {
                            msSQL = " update sys_mst_tmenumapping set status='" + values.rbo_status + "'" +
                           " where module_gid ='" + module_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " SELECT module_gid_parent FROM sys_mst_tmenumapping where module_gid_parent='" + module_gid_parent + "' and status='Y'";
                            lsleveloneparent_gid = objdbconn.GetExecuteScalar(msSQL);

                            if (String.IsNullOrEmpty(lsleveloneparent_gid))
                            {
                                msSQL = " update sys_mst_tmenumapping set status='" + values.rbo_status + "'" +
                               " where module_gid ='" + module_gid_parent + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                    else
                    {
                        msSQL = " SELECT module_gid FROM sys_mst_tmenumapping where module_gid='" + module_gid + "' and status='Y'";
                        lsleveltwoparent_gid = objdbconn.GetExecuteScalar(msSQL);

                        if (String.IsNullOrEmpty(lsleveltwoparent_gid))
                        {
                            msSQL = " update sys_mst_tmenumapping set status='" + values.rbo_status + "'" +
                           " where module_gid ='" + module_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " SELECT module_gid_parent FROM sys_mst_tmenumapping where module_gid='" + module_gid_parent + "' and status='Y'";
                            lsleveloneparent_gid = objdbconn.GetExecuteScalar(msSQL);

                            if (String.IsNullOrEmpty(lsleveloneparent_gid))
                            {
                                msSQL = " update sys_mst_tmenumapping set status='" + values.rbo_status + "'" +
                               " where module_gid ='" + module_gid_parent + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }



                    msGetGid = objcmnfunctions.GetMasterGID("UMNU");

                    msSQL = " insert into sys_mst_tmenumappinginactivelog (" +
                          " menuinactive_gid, " +
                          " menu_gid," +
                          " module_name," +
                          " status," +
                          " remarks," +
                          " created_by," +
                          " created_date) " +
                          " values (" +
                          " '" + msGetGid + "'," +
                          " '" + values.menu_gid + "'," +
                          " '" + values.module_name + "'," +
                          " '" + values.rbo_status + "'," +
                          " '" + values.remarks.Replace("'", "") + "'," +
                          " '" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (values.rbo_status == 'N')
                    {
                        values.status = true;
                        values.message = "Menu Inactivated Successfully";
                    }
                    else
                    {
                        values.status = true;
                        values.message = "Menu Type Activated Successfully";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMenuMappingInactivateview(string menu_gid, menu values)
        {
            try
            {
                msSQL = " SELECT menu_gid,date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as Status, a.remarks" +
                        " FROM sys_mst_tmenumappinginactivelog a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where menu_gid ='" + menu_gid + "' order by a.menuinactive_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getapplication_list = new List<menusummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getapplication_list.Add(new menusummary_list
                        {
                            menu_gid = (dr_datarow["menu_gid"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["Status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                        });
                    }
                    values.menusummary_list = getapplication_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEmployeelist(mdlemployee objmaster)
        {
            try
            {
                msSQL = " SELECT a.user_firstname,a.user_gid ,concat(a.user_firstname,' ',a.user_lastname,' || ',a.user_code) as employee_name,b.employee_gid from adm_mst_tuser a " +
                   " LEFT JOIN hrm_mst_temployee b ON a.user_gid=b.user_gid " +
                   " where user_status<>'N' order by a.user_firstname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employee = new List<employeelist>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    objmaster.employeelist = dt_datatable.AsEnumerable().Select(row =>
                      new employeelist
                      {
                          employee_gid = row["employee_gid"].ToString(),
                          employee_name = row["employee_name"].ToString()
                      }
                    ).ToList();
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;
            }
        }
    }

}