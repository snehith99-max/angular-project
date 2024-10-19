using System.Collections.Generic;
using System;
using ems.system.Models;
using ems.utilities.Functions;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using static OfficeOpenXml.ExcelErrorValue;
using System.Data.Odbc;

namespace ems.system.DataAccess
{
    public class DaSysMstUserGroupTemp
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL2 = string.Empty;
        DataTable dt_datatable, dt_levelone, dt_levelone1;
        OdbcDataReader objGetReaderScalar;
        OdbcDataReader objGetReaderData, objGetReaderData1;
        int mnResult, lshierarchycount, lstranshierarchy;
        String msGetGid = string.Empty;
        DataSet ds_dataset;
        public void DaUserMenuList(MdlSysMstUserGroupTemp objvalues, menu_response values)
        {
            try
            {
                List<sys_menu> getmenu = new List<sys_menu>();
                List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();
                List<mdlMenuData> mdlUserAssignedData = new List<mdlMenuData>();

                msSQL = " select module_gid, module_name, menu_level, display_order, module_gid_parent from adm_mst_tmoduleangular ";
                dt_levelone = objdbconn.GetDataTable(msSQL);

                if (dt_levelone != null)
                {
                    mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone);
                    try
                    {
                        List<mdlMenuData> getFirstLevel = mdlMenuData.Where(a => a.menu_level == "1").ToList();
                        if (getFirstLevel.Count != 0)
                        {
                            foreach (var i in getFirstLevel)
                            {
                                List<mdlMenuData> getSecondLevel = mdlMenuData.Where(a => a.menu_level == "2" && a.module_gid_parent == i.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                    .Select(group => new mdlMenuData
                                    {
                                        module_gid = group.Key,
                                        module_name = group.First().module_name,
                                        menu_level = group.First().menu_level,
                                        display_order = group.First().display_order,
                                        menu_access = group.First().menu_access
                                    }).ToList();
                                List<sys_submenu> getmenu2 = new List<sys_submenu>();

                                if (getSecondLevel != null)
                                {
                                    foreach (var j in getSecondLevel)
                                    {
                                        List<mdlMenuData> getThirdLevel = mdlMenuData.Where(a => a.menu_level == "3" && a.module_gid_parent == j.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                        .Select(group => new mdlMenuData
                                        {
                                            module_gid = group.Key,
                                            module_name = group.First().module_name,
                                            menu_level = group.First().menu_level,
                                            display_order = group.First().display_order,
                                            menu_access = group.First().menu_access

                                        }).ToList();
                                        List<sys_sub1menu> getmenu3 = new List<sys_sub1menu>();

                                        if (getThirdLevel != null)
                                        {
                                            foreach (var k in getThirdLevel)
                                            {
                                                List<sys_sub2menu> getmenu4 = new List<sys_sub2menu>();

                                                getmenu3.Add(new sys_sub1menu
                                                {
                                                    text = k.module_name,
                                                    module_gid = k.module_gid,
                                                    sub2menu = getmenu4,
                                                    menu_access = k.menu_access
                                                });
                                            }
                                        }
                                        getmenu2.Add(new sys_submenu
                                        {
                                            text = j.module_name,
                                            module_gid = j.module_gid,
                                            sub1menu = getmenu3
                                        });
                                    }
                                }
                                else
                                {

                                }
                                getmenu.Add(new sys_menu
                                {
                                    text = i.module_name,
                                    module_gid = i.module_gid,
                                    submenu = getmenu2
                                });
                                values.menu_list = getmenu;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = ex.ToString();
                        values.status = false;
                    }
                    finally
                    {

                    }
                    dt_levelone.Dispose();
                    values.status = true;
                    return;
                }
                values.message = "No data Found";
                dt_levelone.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostUserGroupTemp(string user_gid, MdlSysMstUserGroupTemp objvalues)
        {
            string lssecondlevelmenu = "";

            msSQL = "SELECT usergrouptemplate_code FROM adm_mst_tusergrouptemplate " +
                       "WHERE LOWER(usergrouptemplate_code) = LOWER('" + objvalues.user_group_temp_code + "') " +
                       "OR UPPER(usergrouptemplate_code) = UPPER('" + objvalues.user_group_temp_code + "')";

            DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL);

            if (dt_datatable4.Rows.Count > 0)

            {

                objvalues.status = false;

                objvalues.message = "User group template Code already exist";

                return;

            }


            msSQL = " select usergrouptemplate_name from adm_mst_tusergrouptemplate " +
            "WHERE LOWER(usergrouptemplate_name) = LOWER('" + objvalues.user_group_temp_name.Replace("'", "\\'") + "') " +
            "OR UPPER(usergrouptemplate_name) = UPPER('" + objvalues.user_group_temp_name.Replace("'", "\\'") + "') ";
            DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);

            if (dt_datatable2.Rows.Count > 0)
            {
                objvalues.status = false;
                objvalues.message = "User group template name already exist";
                return;
            }

            msGetGid = objcmnfunctions.GetMasterGID("SUGP");

            msSQL = " insert into adm_mst_tusergrouptemplate  ( " +
                    " usergrouptemplate_gid, " +
                    " usergrouptemplate_code, " +
                    " usergrouptemplate_name, " +
                    " created_by, " +
                    " created_date ) " +
                    " values ( " +
                    " '" + msGetGid + "', " +
                    " '" + objvalues.user_group_temp_code + "', " +
                    " '" + objvalues.user_group_temp_name.Replace("'", "\\'") + "'," +
                    " '" + user_gid + "', " +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ) ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();

            msSQL = " select module_gid_parent, menu_level from adm_mst_tmoduleangular where module_gid in ('" + (objvalues.module_gid).Replace(",", "','") + "') group by module_gid_parent ";
            dt_levelone = objdbconn.GetDataTable(msSQL);

            if (dt_levelone != null)
            {
                mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone);

                try
                {  
                    msSQL = $@" INSERT INTO adm_mst_tusergrouptemplatedtl (usergrouptemplate_gid, module_gid, menu_level, created_by, created_date) VALUES ";

                    List<string> valueRows1 = new List<string>();

                    foreach (var i in mdlMenuData)
                    {
                        
                            lssecondlevelmenu += i.module_gid_parent + ","; 
                            valueRows1.Add($"('{msGetGid}','{i.module_gid_parent}', '{2}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");
                        
                    }

                    string msSQL1 = " select module_gid_parent, menu_level from adm_mst_tmoduleangular where module_gid in ('" + (lssecondlevelmenu).Replace(",", "','") + "') group by module_gid_parent ";
                    dt_levelone1 = objdbconn.GetDataTable(msSQL1);

                    mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone1);

                    foreach (var i in mdlMenuData)
                    {

                        lssecondlevelmenu += i.module_gid_parent + ",";
                        valueRows1.Add($"('{msGetGid}','{i.module_gid_parent}', '{1}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");

                    }

                    string msSQL2 = " select  module_gid, module_gid_parent, menu_level from adm_mst_tmoduleangular where module_gid in ('" + (objvalues.module_gid).Replace(",", "','") + "') ";
                    DataTable dt_leveltwo = objdbconn.GetDataTable(msSQL2);

                    if (dt_leveltwo != null)
                    {
                        mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_leveltwo);
                        foreach (var j in mdlMenuData)
                        {
                            if (!string.IsNullOrWhiteSpace(j.module_gid))
                            {
                                valueRows1.Add($"('{msGetGid}','{j.module_gid}', '{j.menu_level}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");
                            }
                        }
                    }

                    msSQL += string.Join(", ", valueRows1);
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                catch (Exception ex)
                {
                    objvalues.message = ex.ToString();
                    objvalues.status = false;
                }

                dt_levelone.Dispose();
                objvalues.message = "User Group Template Created Successfully";
                objvalues.status = true;
            }

            objvalues.message = "User Group Template Created Successfully";
            dt_levelone.Dispose();
        }
        public void DaUserGroupTempSummary(MdlSysMstUserGroupTemp objmodulelist)
        {
            try
            {
                msSQL = " select usergrouptemplate_gid, date_format(a.created_date,'%d-%b-%Y') as created_date, a.usergrouptemplate_code, a.usergrouptemplate_name,  a.status as UsergroupStatus, " +
                     " case when a.status = 'Y' then 'Active' when a.status IS NULL OR a.status = '' OR a.status = 'N' then 'InActive' END AS status," +
                        " concat(b.user_firstname, b.user_lastname) as created_by from adm_mst_tusergrouptemplate a " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid order by a.usergrouptemplate_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var get_ModuleList = new List<MdlSysMstUserGroupList>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_ModuleList.Add(new MdlSysMstUserGroupList
                        {
                            usergrouptemplate_gid = dr_datarow["usergrouptemplate_gid"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            usergrouptemplate_code = dr_datarow["usergrouptemplate_code"].ToString(),
                            usergrouptemplate_name = dr_datarow["usergrouptemplate_name"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            UsergroupStatus = dr_datarow["status"].ToString(),
                        });
                    }
                    objmodulelist.MdlSysMstUserGroupList = get_ModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public void DaGetEditUserGroupTempSummary(string usergrouptemplate_gid, MdlSysMstUserGroupTemp values)
        {
            try
            {

                msSQL = "select usergrouptemplate_gid, usergrouptemplate_name, usergrouptemplate_code FROM adm_mst_tusergrouptemplate WHERE usergrouptemplate_gid = '" + usergrouptemplate_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var get_ModuleList = new List<MdlEditUserGroupTempListNew>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        get_ModuleList.Add(new MdlEditUserGroupTempListNew
                        {
                            usergrouptemplate_gid = dt["usergrouptemplate_gid"].ToString(),
                            usergrouptemplate_name = dt["usergrouptemplate_name"].ToString(),
                            usergrouptemplate_code = dt["usergrouptemplate_code"].ToString(),
                        });
                        values.MdlEditUserGroupTempListNew = get_ModuleList;
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

    public void DaEditUserMenuList(MdlSysMstUserGroupTemp values,string usergrouptemplate_gid)
        {
            try
            {

                List<sys_menu> getmenu = new List<sys_menu>();
                List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();
                List<mdlMenuData> mdlUserAssignedData = new List<mdlMenuData>();

                msSQL = " select module_gid, module_name, menu_level, display_order, module_gid_parent," +
                      " CASE WHEN module_gid  in (select module_gid from adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid='" + usergrouptemplate_gid + "')" +
                      " THEN 'Y' ELSE 'N'END AS menu_access from adm_mst_tmoduleangular ";
                dt_levelone = objdbconn.GetDataTable(msSQL);

                if (dt_levelone != null)
                {
                    mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone);
                    try
                    {

                        List<mdlMenuData> getFirstLevel = mdlMenuData.Where(a => a.menu_level == "1").ToList();
                        if (getFirstLevel.Count != 0)
                        {
                            foreach (var i in getFirstLevel)
                            {
                                List<mdlMenuData> getSecondLevel = mdlMenuData.Where(a => a.menu_level == "2" && a.module_gid_parent == i.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                    .Select(group => new mdlMenuData
                                    {
                                        module_gid = group.Key,
                                        module_name = group.First().module_name,
                                        menu_level = group.First().menu_level,
                                        display_order = group.First().display_order,
                                        menu_access = group.First().menu_access
                                    }).ToList();
                                List<sys_submenu> getmenu2 = new List<sys_submenu>();

                                if (getSecondLevel != null)
                                {
                                    foreach (var j in getSecondLevel)
                                    {
                                        List<mdlMenuData> getThirdLevel = mdlMenuData.Where(a => a.menu_level == "3" && a.module_gid_parent == j.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                        .Select(group => new mdlMenuData
                                        {
                                            module_gid = group.Key,
                                            module_name = group.First().module_name,
                                            menu_level = group.First().menu_level,
                                            display_order = group.First().display_order,
                                            menu_access = group.First().menu_access

                                        }).ToList();
                                        List<sys_sub1menu> getmenu3 = new List<sys_sub1menu>();

                                        if (getThirdLevel != null)
                                        {
                                            foreach (var k in getThirdLevel)
                                            {
                                                List<sys_sub2menu> getmenu4 = new List<sys_sub2menu>();

                                                getmenu3.Add(new sys_sub1menu
                                                {
                                                    text = k.module_name,
                                                    module_gid = k.module_gid,
                                                    sub2menu = getmenu4,
                                                    menu_access = k.menu_access
                                                });
                                            }
                                        }
                                        getmenu2.Add(new sys_submenu
                                        {
                                            text = j.module_name,
                                            module_gid = j.module_gid,
                                            sub1menu = getmenu3
                                        });
                                    }
                                }
                                else
                                {

                                }
                                getmenu.Add(new sys_menu
                                {
                                    text = i.module_name,
                                    module_gid = i.module_gid,
                                    submenu = getmenu2
                                });
                                values.menu_list = getmenu;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        values.message = ex.ToString();
                        values.status = false;
                    }
                    finally
                    {

                    }
                    dt_levelone.Dispose();
                    values.status = true;
                    return;
                }
                values.message = "No data Found";
                dt_levelone.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

            public void DaUpdateUserGroupTemp(string user_gid, MdlSysMstUserGroupTemp objvalues)
        {

            string lssecondlevelmenu = "";

            //msSQL = "SELECT usergrouptemplate_code FROM adm_mst_tusergrouptemplate " +
            //           "WHERE LOWER(usergrouptemplate_code) = LOWER('" + objvalues.usergrouptemplate_code + "') " +
            //           "OR UPPER(usergrouptemplate_code) = UPPER('" + objvalues.usergrouptemplate_code + "')" +
            //            "AND usergrouptemplate_gid != '" + objvalues.usergrouptemplate_gid + "'";

            //DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL);

            //if (dt_datatable4.Rows.Count > 0)
            //{
            //    objvalues.status = false;
            //    objvalues.message = "User group template Code already exist";
            //    return;
            //}

            msSQL = "SELECT usergrouptemplate_code FROM adm_mst_tusergrouptemplate " +
        "WHERE (LOWER(usergrouptemplate_code) = LOWER('" + objvalues.usergrouptemplate_code.Replace("'", "\\'") + "') " +
        "OR UPPER(usergrouptemplate_code) = UPPER('" + objvalues.usergrouptemplate_code.Replace("'", "\\'") + "')) " +
        "AND usergrouptemplate_gid != '" + objvalues.usergrouptemplate_gid + "'";

            DataTable dt_datatable4 = objdbconn.GetDataTable(msSQL);

            if (dt_datatable4.Rows.Count > 0)
            {
                objvalues.status = false;
                objvalues.message = "User group template Code already exists";
                return;
            }



            msSQL = "SELECT usergrouptemplate_name FROM adm_mst_tusergrouptemplate " +
          "WHERE (LOWER(usergrouptemplate_name) = LOWER('" + objvalues.usergrouptemplate_name.Replace("'", "\\'") + "') " +
          "OR UPPER(usergrouptemplate_name) = UPPER('" + objvalues.usergrouptemplate_name.Replace("'", "\\'") + "')) " +
          "AND usergrouptemplate_gid != '" + objvalues.usergrouptemplate_gid + "'";

            DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);


            if (dt_datatable2.Rows.Count > 0)
            {
                objvalues.status = false;
                objvalues.message = "User group template name already exist";
                return;
            }


            msSQL = " update adm_mst_tusergrouptemplate set " +
                    " usergrouptemplate_code = '" + objvalues.usergrouptemplate_code + "', " +
                    " usergrouptemplate_name = '" + objvalues.usergrouptemplate_name.Replace("'", "\\'") + "', " +
                    " created_by = '" + user_gid + "', " +
                    " created_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    " where usergrouptemplate_gid = '" + objvalues.usergrouptemplate_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult == 1)
            {

                msSQL = "DELETE FROM adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid = '" + objvalues.usergrouptemplate_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

                    List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();

            msSQL = " select module_gid_parent, menu_level from adm_mst_tmoduleangular where module_gid in ('" + (objvalues.module_gid).Replace(",", "','") + "') group by module_gid_parent ";
            dt_levelone = objdbconn.GetDataTable(msSQL);

            if (dt_levelone != null)
            {
                mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone);

                try
                {
                    msSQL = $@" INSERT INTO adm_mst_tusergrouptemplatedtl (usergrouptemplate_gid, module_gid, menu_level, created_by, created_date) VALUES ";

                    List<string> valueRows1 = new List<string>();

                    foreach (var i in mdlMenuData)
                    {

                        lssecondlevelmenu += i.module_gid_parent + ",";
                        valueRows1.Add($"('{objvalues.usergrouptemplate_gid}','{i.module_gid_parent}', '{2}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");

                    }

                    string msSQL1 = " select module_gid_parent, menu_level from adm_mst_tmoduleangular where module_gid in ('" + (lssecondlevelmenu).Replace(",", "','") + "') group by module_gid_parent ";
                    dt_levelone1 = objdbconn.GetDataTable(msSQL1);

                    mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone1);

                    foreach (var i in mdlMenuData)
                    {

                        lssecondlevelmenu += i.module_gid_parent + ",";
                        valueRows1.Add($"('{objvalues.usergrouptemplate_gid}','{i.module_gid_parent}', '{1}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");

                    }

                    string msSQL2 = " select  module_gid, module_gid_parent, menu_level from adm_mst_tmoduleangular where module_gid in ('" + (objvalues.module_gid).Replace(",", "','") + "') ";
                    DataTable dt_leveltwo = objdbconn.GetDataTable(msSQL2);

                    if (dt_leveltwo != null)
                    {
                        mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_leveltwo);
                        foreach (var j in mdlMenuData)
                        {
                            if (!string.IsNullOrWhiteSpace(j.module_gid))
                            {
                                valueRows1.Add($"('{objvalues.usergrouptemplate_gid}','{j.module_gid}', '{j.menu_level}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");
                            }
                        }
                    }

                    msSQL += string.Join(", ", valueRows1);
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                catch (Exception ex)
                {
                    objvalues.message = ex.ToString();
                    objvalues.status = false;
                }

                dt_levelone.Dispose();
                objvalues.message = "User Group Template Updated Successfully";
                objvalues.status = true;
            }

            objvalues.message = "User Group Template Updated Successfully";
            dt_levelone.Dispose();
        }

        public void DaUsergroupActive(string usergrouptemplate_gid, MdlSysMstUserGroupTemp objvalues)
        {
            try
            {
                msSQL = " update adm_mst_tusergrouptemplate set" +
                       " status='Y'" +
                       " where usergrouptemplate_gid = '" + usergrouptemplate_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    objvalues.status = true;
                    objvalues.message = "Usergroup Template Activated Successfully";

                }
                else
                {

                    objvalues.status = false;
                    objvalues.message = "Error while Usergroup Template activeted";

                }
            }
            catch (Exception ex)
            {
                objvalues.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaUsergroupInactive(string usergrouptemplate_gid, MdlSysMstUserGroupTemp objvalues)
        {
            try
            {
                msSQL = " update adm_mst_tusergrouptemplate set" +
                        " status='N'" +
                        " where usergrouptemplate_gid = '" + usergrouptemplate_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    objvalues.status = true;
                    objvalues.message = "Usergroup Template Inactivated Successfully";

                }

                else
                {

                    objvalues.status = false;
                    objvalues.message = "Error while Usergroup Template Inactivated";

                }
            }

            catch (Exception ex)
            {
                objvalues.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetuesrgroupdetails(string usergrouptemplate_gid, MdlSysMstUserGroupTemp objvalues)
        {
            try
            {
                List<sys_menu> get_menu = new List<sys_menu>();          

                msSQL = " SELECT a.usergrouptemplate_gid, a.usergrouptemplate_name, b.module_gid, c.module_code, c.module_name, c.menu_level, c.module_gid_parent, c.display_order " +
                        " FROM adm_mst_tusergrouptemplate a " +
                        " LEFT JOIN adm_mst_tusergrouptemplatedtl b ON a.usergrouptemplate_gid = b.usergrouptemplate_gid " +
                        " LEFT JOIN adm_mst_tmoduleangular c ON b.module_gid = c.module_gid " +
                        " WHERE a.usergrouptemplate_gid = '" + usergrouptemplate_gid + "'" +
                        " ORDER BY c.menu_level, c.display_order";
                         dt_datatable = objdbconn.GetDataTable(msSQL);

                List<MdlSysMstUserGroupList> mdlMenuData = cmnfunctions.ConvertDataTable<MdlSysMstUserGroupList>(dt_datatable);
                List<MdlSysMstUserGroupList> getFirstLevel = mdlMenuData.Where(a => a.menu_level == "1").ToList();

                if (getFirstLevel.Count != 0)
                {
                    foreach (var levelOne in getFirstLevel)
                    {
                        List<sys_submenu> getmenu2 = new List<sys_submenu>();
                        List<MdlSysMstUserGroupList> getSecondLevel = mdlMenuData
                            .Where(a => a.menu_level == "2" && a.module_gid_parent == levelOne.module_gid)
                            .OrderBy(a => a.display_order).ToList();

                        foreach (var levelTwo in getSecondLevel)
                        {
                            List<sys_sub1menu> getmenu3 = new List<sys_sub1menu>();
                            List<MdlSysMstUserGroupList> getThirdLevel = mdlMenuData
                                .Where(a => a.menu_level == "3" && a.module_gid_parent == levelTwo.module_gid)
                                .OrderBy(a => a.display_order).ToList();

                            foreach (var levelThree in getThirdLevel)
                            {
                                getmenu3.Add(new sys_sub1menu
                                {
                                    text = levelThree.module_name,
                                    module_gid = levelThree.module_gid,
                                    sub2menu = new List<sys_sub2menu>(), // Assuming further levels if needed
                                    menu_access = levelThree.menu_access
                                });
                            }
                            getmenu2.Add(new sys_submenu
                            {
                                text = levelTwo.module_name,
                                module_gid = levelTwo.module_gid,
                                sub1menu = getmenu3
                            });
                        }
                        get_menu.Add(new sys_menu
                        {
                            text = levelOne.module_name,
                            module_gid = levelOne.module_gid,
                            submenu = getmenu2
                        });
                    }
                }

                objvalues.menu_list = get_menu;

            }

            catch (Exception ex)
            {
                objvalues.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaDeleteUserGroupDetails(string usergrouptemplate_gid, MdlSysMstUserGroupTemp values)
        {
            try
            {
                msSQL = " delete a, b from adm_mst_tusergrouptemplate a left join adm_mst_tusergrouptemplatedtl b " +
                        " on a.usergrouptemplate_gid = b.usergrouptemplate_gid where a.usergrouptemplate_gid = '" + usergrouptemplate_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Usergroup Template Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while deleting Usergroup template details";
                }
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