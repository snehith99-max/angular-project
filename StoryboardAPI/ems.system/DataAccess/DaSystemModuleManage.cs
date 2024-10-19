
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
using System.Security.Cryptography;


namespace ems.system.DataAccess
{
    public class DaSysMstModuleManage
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL1, msSQL2 = string.Empty;
        DataTable dt_datatable, dt_levelone;
        // Dictionary<string, object> objGetReaderScalar;
        OdbcDataReader objGetReaderScalar;
        OdbcDataReader objGetReaderData;
        //List<Dictionary<string, object>> objGetReaderData;
        int mnResult, lshierarchycount, lstranshierarchy, mnResult1;
        String msGetGid = string.Empty;
        DataSet ds_dataset;

        string msGetGid1, lsempoyeegid, msgetshift, summaryTable, lsbranchgid;

        public bool DaGetModuleListSummary(mdlModuleList objmodulelist)
        {
            try
            {
                //msSQL = " select a.module_gid, a.module_name, CONCAT( COALESCE(c.user_firstname, ''), COALESCE(c.user_lastname, ''), " +
                //        " CASE WHEN c.user_code IS NOT NULL THEN CONCAT(' / ', c.user_code)  ELSE '' END) as module_manager, " +
                //        " a.modulemanager_gid from (select adm_mst_tmoduleangular a  " +
                //        " left join hrm_mst_temployee b on a.modulemanager_gid = b.employee_gid " +
                //        " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                //        " where menu_level = 1 and status<> 0 " +
                //        " Order by menu_level asc";
                msSQL = "select a.module_gid, a.module_name, a.module_code,CONCAT(COALESCE(c.user_firstname, ''), " +
                        "COALESCE(c.user_lastname, ''), CASE WHEN c.user_code IS NOT NULL THEN CONCAT(' / ', c.user_code)  ELSE '' END) as module_manager,"+
                        "a.modulemanager_gid,  (select count(employee_gid)from adm_mst_tmodule2employee where module_gid = a.module_gid) as employee_total from adm_mst_tmoduleangular a "+
                        "left join hrm_mst_temployee b on a.modulemanager_gid = b.employee_gid left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                        "where menu_level = 1 and status<> 0  Order by menu_level asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_ModuleList = new List<mdlModuleDtl>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_ModuleList.Add(new mdlModuleDtl
                        {
                            module_gid = dr_datarow["module_gid"].ToString(),
                            module_code = dr_datarow["module_code"].ToString(),
                            module_name = dr_datarow["module_name"].ToString(),
                            employee_total = dr_datarow["employee_total"].ToString(),
                            module_manager = dr_datarow["module_manager"].ToString(),
                            modulemanager_gid = dr_datarow["modulemanager_gid"].ToString(),
                        });
                    }
                    objmodulelist.mdlModuleDtl = get_ModuleList;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        public bool DaPostManagerAssign(mdlManagerAssignDtl objvalues)
        {
            string lshierarchy = "";
            msSQL = " select max(hierarchy_level) as hierarchy_level from adm_mst_tmodule2employee " +
                     " where module_gid = '" + objvalues.module_gid + "'";
            lshierarchy = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "SELECT COUNT(*) FROM adm_mst_tmoduleangular " +
            "WHERE module_gid = '" + objvalues.module_gid + "' " +
            "AND modulemanager_gid = '" + objvalues.modulemanager_gid + "'";
            int existingManagerCount = Convert.ToInt32(objdbconn.GetExecuteScalar(msSQL));

            if (existingManagerCount > 0)
            {
                objvalues.status = false;
                objvalues.message = "The manager is already assigned to this module";
                return false;
            }

            if (lshierarchy == "1" || lshierarchy == "0")
            {
                msSQL = " Update adm_mst_tmoduleangular set " +
                        " modulemanager_gid='" + objvalues.modulemanager_gid + "'" +
                        " where module_gid = '" + objvalues.module_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " update adm_mst_tmodule2employee set " +
                            " hierarchy_level='-1'" +
                            " where module_gid = '" + objvalues.module_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        objvalues.status = false;
                        objvalues.message = "Error Occured While Assigning Module Manager";
                        return false;
                    }

                    msSQL = " select employeereporting_to from adm_mst_tmodule2employee" +
                            " where employeereporting_to='EM1006040001' and module_gid='" + objvalues.module_gid + "'";
                    objGetReaderScalar = objdbconn.GetDataReader(msSQL);
                    if (objGetReaderScalar != null)
                    {
                        msSQL = " update adm_mst_tmodule2employee set " +
                                " hierarchy_level='1'" +
                                " where employeereporting_to = 'EM1006040001'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            objvalues.status = false;
                            objvalues.message = "Error Occured While Assigning Module Manager";
                            return false;
                        }
                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SMEM");
                        msSQL = " insert into adm_mst_tmodule2employee " +
                                " (module2employee_gid, " +
                                " hierarchy_level, " +
                                " employee_gid, " +
                                " employeereporting_to, " +
                                " module_gid) " +
                                " values ( " +
                                "'" + msGetGid + "'," +
                                "'1'," +
                                "'" + objvalues.modulemanager_gid + "'," +
                                "'EM1006040001'," +
                                "'" + objvalues.module_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    objvalues.message = "Module Manager Assigned Successsfully";
                    objvalues.status = true;
                    return true;
                }
                else
                {

                    objvalues.status = false;
                    objvalues.message = "Error Occured While Assigning Module Manager";
                    return false;
                }
            }
            else if (lshierarchy == "")
            {
                msGetGid = objcmnfunctions.GetMasterGID("SMEM");
                msSQL = " insert into adm_mst_tmodule2employee " +
                        " (module2employee_gid, " +
                        " hierarchy_level, " +
                        " employee_gid, " +
                        " employeereporting_to, " +
                        " module_gid) " +
                        " values ( " +
                        "'" + msGetGid + "'," +
                        "'1'," +
                        "'" + objvalues.modulemanager_gid + "'," +
                        "'EM1006040001'," +
                        "'" + objvalues.module_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    objvalues.message = "Module Manager Assigned Successsfully";
                    objvalues.status = true;
                    return true;
                }
                else
                {
                    objvalues.status = false;
                    objvalues.message = "Error Occured While Assigning Module Manager";
                    return false;
                }


            }
            else
            {
                objvalues.status = false;
                objvalues.message = "If you want to change manager, kindly remove hierarchy";
                return false;
            }
        }

        public void DaGetEmployeeAssignlist(string module_gid, mdlemployee objmaster)
        {
            try
            {
                msSQL = " SELECT a.user_firstname,a.user_gid ,concat(a.user_firstname,' ',a.user_lastname,' || ',a.user_code) as employee_name,b.employee_gid from adm_mst_tuser a " +
                   " LEFT JOIN hrm_mst_temployee b ON a.user_gid=b.user_gid " +
                   " where user_status<>'N' and " +
                   " employee_gid not in (select employee_gid from adm_mst_tmodule2employee " +
                   " where module_gid='" + module_gid + "' and hierarchy_level<>'-1') order by a.user_firstname asc";

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
        public void DaUserGroupTemplist(string module_gid, mdlModuleList objmaster)
        {
            try
            {
                msSQL = " select usergrouptemplate_gid, usergrouptemplate_code, usergrouptemplate_name from adm_mst_tusergrouptemplate ";
                        

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employee = new List<usergrouptemplist>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    objmaster.usergrouptemplist = dt_datatable.AsEnumerable().Select(row =>
                      new usergrouptemplist
                      {
                          usergrouptemplate_gid = row["usergrouptemplate_gid"].ToString(),
                          usergrouptemplate_code = row["usergrouptemplate_code"].ToString(),
                          usergrouptemplate_name = row["usergrouptemplate_name"].ToString()
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
        public bool DaGetModuleAssignedEmployee(string module_gid, mdlModuleAssignedList objmodulelist)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code,'/',b.user_firstname,' ',b.user_lastname) as user_name " +
                        " from adm_mst_tmodule2employee a" +
                        " left join hrm_mst_temployee c on a.employee_gid=c.employee_gid" +
                        " left join adm_mst_tuser b on c.user_gid=b.user_gid" +
                        " where a.module_gid='" + module_gid + "' and a.hierarchy_level<>'-1' and b.user_status='Y' " +
                        " order by b.user_firstname asc,a.hierarchy_level asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_dataList = new List<mdlModuleHierarchy>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_dataList.Add(new mdlModuleHierarchy
                        {
                            employee_gid = dr_datarow["employee_gid"].ToString(),
                            user_name = dr_datarow["user_name"].ToString(),
                        });
                    }
                    objmodulelist.mdlModuleHierarchy = get_dataList;
                }
                dt_datatable.Dispose();

                msSQL = "  SELECT a.employee_gid, c.user_gid,c.user_code, " +
                        " concat(c.user_firstname,' ',c.user_lastname) as user_name, " +
                        " CASE WHEN c.user_status = 'Y' THEN 'Active'  WHEN c.user_status = 'N' THEN 'Inactive' " +
                        " END as user_status, " +
                        " (select count(module_gid) from adm_mst_tprivilegeangular where module_gid= '" + module_gid + "' " +
                        " and user_gid = c.user_gid) as menuaccess FROM adm_mst_tmodule2employee a " +
                        " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where a.module_gid = '" + module_gid + "' order by a.module2employee_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_ModuleList = new List<mdlModuleAssigneddtl>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_ModuleList.Add(new mdlModuleAssigneddtl
                        {
                            user_gid = dr_datarow["user_gid"].ToString(),
                            user_code = dr_datarow["user_code"].ToString(),
                            user_name = dr_datarow["user_name"].ToString(),
                            user_status = dr_datarow["user_status"].ToString(),
                            menuaccess = dr_datarow["menuaccess"].ToString(),
                        });
                    }
                    objmodulelist.mdlModuleAssigneddtl = get_ModuleList;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public bool DaPostModuleEmployeeAssign(mdlModuleemployeedtl objvalues, string user_gid)
        {
            try
            {
                msSQL = " Select modulemanager_gid from adm_mst_tmoduleangular Where module_gid = '" + objvalues.module_gid + "' ";
                string lsmodulemanager_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select hierarchy_level from adm_mst_tmodule2employee " +
                        " where employee_gid='" + objvalues.assign_hierarchy + "' " +
                        " and module_gid='" + objvalues.module_gid + "'";
                int lshierarchy_level = Convert.ToInt16(objdbconn.GetExecuteScalar(msSQL));
                int hlevel = lshierarchy_level + 1;

                msSQL1 = $@" INSERT INTO adm_mst_tmodule2employee (module2employee_gid,module_gid, employee_gid,hierarchy_level, " +
                          " employeereporting_to,created_by,created_date) VALUES ";
                List<string> valueRows = new List<string>();

                foreach (var i in objvalues.Mdlassignemployeelist)
                {
                    if (!string.IsNullOrWhiteSpace(i.employee_gid))
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SMEM");
                        valueRows.Add($"('{msGetGid}','{objvalues.module_gid}', '{i.employee_gid}', '{hlevel}', '{objvalues.assign_hierarchy}','{user_gid}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");

                        msSQL = " Select user_gid from hrm_mst_temployee where employee_gid = '" + i.employee_gid + "' ";
                        string lsuser_gid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select usergrouptemplate_gid, module_gid from adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid = '" + objvalues.usergrouptemplate_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        msGetGid = objcmnfunctions.GetMasterGID("SPGM");

                        msSQL = $@" INSERT INTO adm_mst_tprivilegeangular (privilege_gid, module_gid, user_gid, created_by, created_date) VALUES ";

                        List<string> valueRows1 = new List<string>();

                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            if (!string.IsNullOrWhiteSpace(objvalues.usergrouptemplate_gid))
                            {
                                msGetGid = objcmnfunctions.GetMasterGID("SMEM");
                                valueRows1.Add($"('{msGetGid}','{dt["module_gid"].ToString()}', '{lsuser_gid}', '{user_gid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");
                            }
                        }
                        msSQL += string.Join(", ", valueRows1);
                        msSQL.TrimEnd(',');
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                msSQL1 += string.Join(", ", valueRows);
                msSQL1.TrimEnd(',');
                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL1);

                

                if (mnResult1 == 1)
                {
                    objvalues.status = true;
                    objvalues.message = "Employee Assigned Successfully";
                    return true;
                }
                else
                {
                    objvalues.status = false;
                    objvalues.message = "Error Occurred While Inserting Module";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public void DaGetUserRoleList(MdlSelectedModule objvalues, menu_response values)
        {
            var ModuleAssigned_user = new DataTable();
            List<sys_menu> getmenu = new List<sys_menu>();
            List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();
            List<mdlMenuData> mdlUserAssignedData = new List<mdlMenuData>();

            if (!string.IsNullOrWhiteSpace(objvalues.user_gid))
            {
                msSQL = " select  module_gid, module_parent_gid as module_gid_parent from adm_mst_tprivilegeangular " +
                       " where user_gid = '" + objvalues.user_gid + "' and module_parent_gid = '" + objvalues.module_parentgid + "'";
                ModuleAssigned_user = objdbconn.GetDataTable(msSQL);
                if (ModuleAssigned_user != null)
                    mdlUserAssignedData = cmnfunctions.ConvertDataTable<mdlMenuData>(ModuleAssigned_user);
            }

            //msSQL = " EXEC dbo.adm_mst_spGetMenuListByModuleGid " +
            //        " @module_gid='" + objvalues.module_parentgid + "'";
            //msSQL = " SELECT m.module_gid, m.module_name, m.menu_level,m.display_order, m.module_gid_parent FROM adm_mst_tmodule m" +
            //       " where m.module_gid like '" + objvalues.module_parentgid + "%'";
            msSQL = " SELECT m.module_gid, m.module_name, m.menu_level,m.display_order, m.module_gid_parent,CASE WHEN m.module_gid  in (select module_gid from adm_mst_tprivilegeangular where user_gid='" + objvalues.user_gid + "')" +
                   " THEN 'Y' ELSE 'N'END AS menu_access FROM  adm_mst_tmoduleangular m" +
                   " LEFT JOIN  adm_mst_tprivilegeangular p ON m.module_gid = p.module_gid" +
                   " WHERE m.module_gid like '" + objvalues.module_parentgid + "%' group by m.module_gid";

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
                                            //var getFourthLevel = mdlMenuData.Where(a => a.menu_level == "4" && a.module_gid_parent == k.module_gid)
                                            //                     .OrderBy(a => a.display_order)
                                            //                     .GroupBy(a => a.module_gid).ToList();
                                            List<sys_sub2menu> getmenu4 = new List<sys_sub2menu>();
                                            //if (getFourthLevel != null)
                                            //{
                                            //    getmenu4 = getFourthLevel.SelectMany(group => group).Select(row => new sys_sub2menu
                                            //    {
                                            //        text = row.module_name,
                                            //        module_gid = row.module_gid,
                                            //        module_checked = mdlUserAssignedData?.Where(z => z.module_gid == row.module_gid && z.module_gid_parent == objvalues.module_parentgid).ToList()?.Any() ?? false
                                            //    }).ToList();
                                            //}
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

        public bool DaPostPrivilege(MdlSelectedModule objvalues)
        {
            string msGet_PrivGID = "";

            msSQL = " delete from adm_mst_tprivilegeangular where user_gid = '" + objvalues.user_gid + "' " +
                    " and module_parent_gid like '" + objvalues.module_parentgid + "%'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGet_PrivGID = objcmnfunctions.GetMasterGID("SPGM");
            if (msGet_PrivGID == "E")
            {
                return false;
            }
            //string selectedModuleGid = "(" + string.Join(",", objvalues.module_gid.Select(id => $"{id}")) + ")";
            List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();
            msSQL = " select module_gid_parent as module_gid from adm_mst_tmoduleangular where module_gid in ('" + (objvalues.module_gid).Replace(",", "','") + "') group by module_gid_parent";
            dt_levelone = objdbconn.GetDataTable(msSQL);
            //msSQL2 = " update  adm_mst_tmodule set lw_flag = 'Y'  " +
            //         " where module_gid = '" + objvalues.module_parentgid + "'";

            //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL2);
            if (dt_levelone != null)
            {
                mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone);
                try
                {
                    msSQL = $@" INSERT INTO adm_mst_tprivilegeangular (privilege_gid,module_gid, user_gid,module_parent_gid) VALUES ";
                    List<string> valueRows = new List<string>();
                    foreach (var i in mdlMenuData)
                    {
                        if (!string.IsNullOrWhiteSpace(i.module_gid))
                        {
                            msGet_PrivGID = objcmnfunctions.GetMasterGID("SPGM");
                            valueRows.Add($"('{msGet_PrivGID}','{i.module_gid}', '{objvalues.user_gid}', '{objvalues.module_parentgid}')");
                            //msSQL2 = " update  adm_mst_tmodule set lw_flag = 'Y'  " +
                            //" where module_gid = '" + i.module_gid + "'";

                            //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL2);
                        }
                    }
                    msGet_PrivGID = objcmnfunctions.GetMasterGID("SPGM");

                    valueRows.Add($"('{msGet_PrivGID}','{objvalues.module_parentgid}', '{objvalues.user_gid}', '{objvalues.module_parentgid}')");
                    string msSQL1 = " select  module_gid,module_gid_parent from adm_mst_tmoduleangular where module_gid in ('" + (objvalues.module_gid).Replace(",", "','") + "') ";
                    DataTable dt_leveltwo = objdbconn.GetDataTable(msSQL1);
                    if (dt_leveltwo != null)
                    {
                        mdlMenuData = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_leveltwo);
                        foreach (var j in mdlMenuData)
                        {
                            if (!string.IsNullOrWhiteSpace(j.module_gid))
                            {
                                msGet_PrivGID = objcmnfunctions.GetMasterGID("SPGM");
                                valueRows.Add($"('{msGet_PrivGID}','{j.module_gid}', '{objvalues.user_gid}', '{j.module_gid_parent}')");
                                //msSQL2 = " update  adm_mst_tmodule set lw_flag = 'Y'  " +
                                //         " where module_gid = '" + j.module_gid + "'";

                                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL2);
                            }
                        }


                    }

                    msSQL += string.Join(", ", valueRows);

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                catch (Exception ex)
                {
                    objvalues.message = ex.ToString();
                    objvalues.status = false;
                    return false;
                }
                dt_levelone.Dispose();
                objvalues.message = "User Role Assigned Successfully";
                objvalues.status = true;
                return true;
            }
            objvalues.message = "No data Found";
            dt_levelone.Dispose();
            return true;
        }


        //public void Dadeletehierarchy(mdlManagerAssignDtl objvalues,string employee_gid)
        //{
        //    try
        //    {
                       
        //        if (employee_gid == "E1")
        //        {
        //            msSQL = " delete from adm_mst_tmodule2employee" +
        //                    " where employeereporting_to<>'EM1006040001' and module_gid='" + objvalues.module_gid + "'";
        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //            if (mnResult != 0)
        //            {
        //                objvalues.status = true;
        //                objvalues.message = "Hierarchy cleared successfully";
        //            }
        //            else
        //            {
        //                objvalues.status = false;
        //                objvalues.message = "Failed to clear hierarchy";
        //            }
                   
        //        }
        //        else
        //        {
        //            objvalues.status = false;
        //            objvalues.message = "Please contact superadmin to clear the Hierarchy ";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objvalues.message = ex.ToString();
        //        objvalues.status = false;
        //    }

        //}




        public void Dadeletehierarchy(mdlManagerAssignDtl objvalues, string employee_gid)
        {
            try
            {

                if (employee_gid == "E1")
                {
                    msSQL = " delete from adm_mst_tmodule2employee" +
                            " where employeereporting_to<>'EM1006040001' and module_gid='" + objvalues.module_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        // Step 2: Clear the module manager by setting modulemanager_gid to NULL
                       
                        msSQL = " delete from adm_mst_tprivilegeangular " +
                                " where module_gid  ='" + objvalues.module_gid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            objvalues.status = true;
                            objvalues.message = "Clear Hierarchy Successfully";
                        }
                    }
                    else
                    {
                        objvalues.status = false;
                        objvalues.message = "Failed to clear hierarchy";
                    }

                }
                else
                {
                    objvalues.status = false;
                    objvalues.message = "Please contact superadmin to clear the Hierarchy ";
                }
            }
            catch (Exception ex)
            {
                objvalues.message = ex.ToString();
                objvalues.status = false;
            }

        }


        //    msSQL = " delete from adm_mst_tmodule2employee" +
        //        " where employeereporting_to<>'EM1006040001' and module_gid='" + objvalues.module_gid + "'";

        //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //if (mnResult != 0)
        //{
        //    objvalues.status = true;
        //    objvalues.message = "Hierarchy Cleared Successfully";
        //}
        //else
        //{
        //    objvalues.status = false;
        //    objvalues.message = "Error Occured While Clearing Hierarchy";
        //}

        //        }
        //    catch (Exception ex)
        //    {
        //        objvalues.message = ex.ToString();
        //objvalues.status = false;
        //    }
        //}

        public void DaApprovalSummary(string module_gid, mdlModuleList values)
        {
            try
            {

                msSQL = "SELECT a.module_gid,a.module_name,a.approval_type,a.approval_limit FROM adm_mst_tmoduleangular a WHERE module_gid LIKE '" + module_gid + "%' AND approval_flag = 'Y'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Approvalsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Approvalsummary
                        {
                            module_gid = dt["module_gid"].ToString(),
                            module_name = dt["module_name"].ToString(),
                            approval_type = dt["approval_type"].ToString(),
                            approval_limit = dt["approval_limit"].ToString(),
                        });
                        values.Approvalsummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = ex.ToString();
                values.status = false;
            }
        }

        public void DaApprovalsubmit(string user_gid, Approvalsubmit values)
        {
            try
            {


                msSQL = " update adm_mst_tmoduleangular set " +
                   " approval_type='" + values.approval_type + "'," +
                   " approval_limit='" + values.approval_limit + "'" +
                   " where module_gid= '" + values.module_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Approval Type Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Approval Type";
                }
            }
            catch (Exception ex)
            {
                values.message = ex.ToString();
                values.status = false;
            }
        }

        public void DaGetApprovalAssignHierarchysummary(string module_gid, mdlModuleList values)
        {
            try
            {
                msSQL = " SELECT max(hierarchy_level) as level FROM adm_mst_tmodule2employee where module_gid = '" + module_gid.Substring(0, 3) + "' ";

                objGetReaderData = objdbconn.GetDataReader(msSQL);

                if (objGetReaderData.HasRows == true)
                {
                    lshierarchycount = int.Parse(objGetReaderData["level"].ToString());

                    summaryTable = "<table width=100% class=Summary>";
                    summaryTable += "<tr class=Heading>";

                    if (lshierarchycount != 0)
                    {
                        for (int i = 0; i < lshierarchycount; i++)
                        {
                            summaryTable += "<td align=center> <b> Level " + (i + 1) + " <b> </td>";
                        }
                    }
                    else
                    {
                        summaryTable += "<td>Levels</td>";
                    }

                    summaryTable += loopNodes(module_gid.Substring(0, 3), lshierarchycount);
                    summaryTable += "</table>";
                    values.level_list = summaryTable;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/system/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public string loopNodes(string module_gid, int lshierarchycount)
        {
            string employee_gid = "";
            summaryTable = "<table width=100% class=Summary>";

            // Fetch assigned employees from adm_mst_tsubmodule table
            msSQL = "SELECT b.employee_gid, CONCAT(c.user_code, ' || ', c.user_firstname, ' ', c.user_lastname) AS user_firstname, c.user_code, " +
                    "a.hierarchy_level AS hierarchy, d.designation_name, e.department_name " +
                    "FROM adm_mst_tsubmodule a " +
                    "INNER JOIN hrm_mst_temployee b ON a.employee_gid = b.employee_gid " +
                    "INNER JOIN adm_mst_tuser c ON c.user_gid = b.user_gid " +
                    "LEFT JOIN adm_mst_tdesignation d ON b.designation_gid = d.designation_gid " +
                    "LEFT JOIN hrm_mst_tdepartment e ON b.department_gid = e.department_gid " +
                    "WHERE a.module_gid = '" + module_gid + "'";

            objGetReaderScalar = objdbconn.GetDataReader(msSQL);

            while (objGetReaderScalar.Read())
            {
                summaryTable += "<tr class=Primary>";
                summaryTable += "<td nowrap='nowrap' width='200px' height='20px' align='center'>";
                summaryTable += "<td> <b style='color: blue;'>Name : </b>" + objGetReaderScalar["user_firstname"] + " <br/> <b>Designation : </b>" + objGetReaderScalar["designation_name"] + " <br/> <b>Department : </b>" + objGetReaderScalar["department_name"] + "</td>";

                for (int i = 1; i < lshierarchycount; lshierarchycount--)
                {
                    summaryTable += "<td></td>";
                }

                summaryTable += "</tr>";
            }

            objGetReaderScalar.Close();

            return summaryTable;
        }

        public string childloop(string employee_gid, string module_gid)
        {
            msSQL = " select b.employee_gid, concat(c.user_code,' || ',c.user_firstname,' ',c.user_lastname) as user_firstname,c.user_code, a.hierarchy_level as hierarchy,d.designation_name,e.department_name " +
                    " from adm_mst_tmodule2employee a " +
                    " inner join hrm_mst_temployee b on b.employee_gid = a.employee_gid " +
                    " inner join adm_mst_tuser c on c.user_gid = b.user_gid " +
                    " left join adm_mst_tdesignation d on b.designation_gid=d.designation_gid " +
                    " left join hrm_mst_tdepartment e on b.department_gid=e.department_gid " +
                    " where a.employeereporting_to = '" + employee_gid + "' and " +
                    " a.module_gid='" + module_gid + "' and a.hierarchy_level >1 and a.visible='T'";

            ds_dataset = objdbconn.GetDataSet(msSQL, "adm_mst_tmodule2employee");
            dt_datatable = ds_dataset.Tables["adm_mst_tmodule2employee"];

            foreach (DataRow dr in dt_datatable.Rows)
            {
                lstranshierarchy = int.Parse(dr["hierarchy"].ToString());

                summaryTable += "<tr class=Primary>";
                summaryTable += "<td nowrap='nowrap' width='200px' height='20px' align='center'>";

                for (int i = 1; i < lstranshierarchy; lstranshierarchy--)
                {
                    summaryTable += "<td></td>";
                }

                summaryTable += "<td> <b>Name : </b>" + dr["user_firstname"] + " <br/> <b>Designation : </b>" + dr["designation_name"] + " <br/> <b>Department : </b>" + dr["department_name"] + "</td>";

                for (int i = 1; i < lshierarchycount - lstranshierarchy; lshierarchycount--)
                {
                    summaryTable += "<td></td>";
                }

                summaryTable += "</tr>";

                childloop(dr["employee_gid"].ToString(), module_gid);
            }
            return summaryTable;
        }
        public void DaPostEmployeeAssignSubmit(submoudule_list objvalues, string user_gid)
        {
            try
            {
                foreach (var data in objvalues.employeelist)
                {
                    msSQL = "select a.module_gid,substring(a.module_gid,1,3) as moduleparentgid,a.module_gid_parent,a.module_code,a.module_name  from adm_mst_tmodule a" +
                            "  where a.module_gid= '" + objvalues.module_gid + "'";
                    objGetReaderScalar = objdbconn.GetDataReader(msSQL);

                    if (objGetReaderScalar.HasRows == true)
                    {
                        msSQL = "Select hierarchy_level from adm_mst_tmodule2employee" +
                        " where module_gid= '" + objGetReaderScalar["moduleparentgid"].ToString() + "' and employee_gid='" + data.employee_gid + "'";
                        string lshierarchy_level = objdbconn.GetExecuteScalar(msSQL);
                        if (lshierarchy_level == "")
                        {
                            lshierarchy_level = "1";
                        }
                        msGetGid = objcmnfunctions.GetMasterGID("BLBP");
                        msSQL = " insert into adm_mst_tsubmodule (" +
                        " submodule_gid," +
                        " submodule_id," +
                        " submodule_name," +
                        " module_gid," +
                        " employee_gid," +
                        " hierarchy_level," +
                        " created_by," +
                        " created_date)" +
                        " values (" +
                        " '" + msGetGid + "', " +
                        " '" + objGetReaderScalar["module_gid"].ToString() + "', " +
                        " '" + objGetReaderScalar["module_name"].ToString() + "', " +
                        " '" + objGetReaderScalar["module_gid_parent"].ToString() + "', " +
                        " '" + data.employee_gid + "'," +
                        " '" + lshierarchy_level + "'," +
                        " '" + user_gid + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }


                if (mnResult == 1)
                {
                    objvalues.status = true;
                    objvalues.message = "Employee Assigned Successfully";
                }
                else
                {
                    objvalues.status = false;
                    objvalues.message = "Error Occurred While Inserting Module";
                }
            }
            catch (Exception ex)
            {
                objvalues.message = ex.ToString();
                objvalues.status = false;
            }



        }
    }
}
