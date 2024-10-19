﻿using ems.system.Models;
using ems.utilities.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Xml.Linq;



namespace ems.system.DataAccess
{
    public class DaUser
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_levelone, dt_leveltwo, dt_levelthree, dt_levelfour;
        string menu_ind_up_first = string.Empty;
        string menu_ind_down_first = string.Empty;
        string menu_ind_up_second = string.Empty;
        string menu_ind_down_second = string.Empty;
        //public void loadTopMenuFromDB(string user_gid, menu_response values)
        //{
        //    var dt_data = new DataTable();
        //    var getmenu = new List<sys_menu>();
        //    try
        //    {

        //        msSQL = " SELECT a.module_gid,b.module_name,b.angular_sref,b.sref,b.icon,b.menu_level FROM adm_mst_tprivilege a " +
        //                " LEFT JOIN adm_mst_tmodule b ON a.module_gid = b.module_gid" +
        //                " WHERE user_gid = '" + user_gid + "' AND menu_level=1 AND b.lw_flag='Y'  group by a.module_gid order by b.display_order asc";
        //        dt_levelone = objdbconn.GetDataTable(msSQL);
        //        if (dt_levelone.Rows.Count != 0)
        //        {
        //            foreach (DataRow drOne in dt_levelone.Rows)
        //            {
        //                getmenu.Add(new sys_menu
        //                {
        //                    text = drOne["module_name"].ToString(),
        //                    sref = drOne["sref"].ToString(),
        //                    icon = drOne["icon"].ToString(),
        //                });
        //                values.menu_list = getmenu;
        //            }
        //            values.status = true;
        //        }
        //        dt_levelone.Clear();
        //    }
        //    catch
        //    {
        //        values.status = false;
        //    }
        //}

        //public void loadTopMenuFromDB(string user_gid, menu_response values)
        //{
        //    var dt_data = new DataTable();
        //    var getmenu = new List<sys_menu>();
        //    try
        //    {

        //        msSQL = " SELECT a.module_gid,b.module_name,b.angular_sref,b.sref,b.icon,b.menu_level FROM adm_mst_tprivilege a " +
        //                     " LEFT JOIN adm_mst_tmodule b ON a.module_gid = b.module_gid" +
        //                     " WHERE user_gid = '" + user_gid + "' AND menu_level=1 AND b.lw_flag='Y'  group by a.module_gid order by b.display_order asc";
        //        dt_levelone = objdbconn.GetDataTable(msSQL);
        //        if (dt_levelone.Rows.Count != 0)
        //        {
        //            foreach (DataRow drOne in dt_levelone.Rows)
        //            {


        //                msSQL = " SELECT a.module_gid,b.module_name,b.angular_sref,b.sref,b.icon,b.menu_level FROM adm_mst_tprivilege a " +
        //                        " LEFT JOIN adm_mst_tmodule b ON a.module_gid = b.module_gid" +
        //                        " WHERE user_gid = '" + user_gid + "' " +
        //                        " AND b.menu_level = 2 AND b.module_gid_parent = '" + drOne["module_gid"].ToString() + "' AND b.lw_flag='Y' " +
        //                        "  group by a.module_gid order by b.display_order asc";
        //                dt_leveltwo = objdbconn.GetDataTable(msSQL);
        //                var getmenu2 = new List<sys_submenu>();
        //                if (dt_leveltwo.Rows.Count != 0)
        //                {
        //                    foreach (DataRow drTwo in dt_leveltwo.Rows)
        //                    {

        //                        msSQL = " SELECT a.module_gid,b.module_name,b.angular_sref,b.sref,b.icon,b.menu_level FROM adm_mst_tprivilege a " +
        //                                " LEFT JOIN adm_mst_tmodule b ON a.module_gid = b.module_gid WHERE user_gid = '" + user_gid + "' " +
        //                                " AND b.menu_level = 3 AND b.module_gid_parent= '" + drTwo["module_gid"].ToString() + "' AND b.lw_flag='Y' " +
        //                                "  group by a.module_gid order by b.display_order asc";
        //                        dt_levelthree = objdbconn.GetDataTable(msSQL);
        //                        var getmenu3 = new List<sys_sub1menu>();
        //                        if (dt_levelthree.Rows.Count != 0)
        //                        {
        //                            foreach (DataRow drthree in dt_levelthree.Rows)
        //                            {
        //                                msSQL = " SELECT a.module_gid,b.module_name,b.angular_sref,b.sref,b.icon,b.menu_level FROM adm_mst_tprivilege a " +
        //                                " LEFT JOIN adm_mst_tmodule b ON a.module_gid = b.module_gid WHERE user_gid = '" + user_gid + "' " +
        //                                " AND b.menu_level = 4 AND b.module_gid_parent='" + drthree["module_gid"].ToString() + "' AND b.lw_flag='Y' " +
        //                                "  group by a.module_gid order by b.display_order asc";
        //                                dt_levelfour = objdbconn.GetDataTable(msSQL);
        //                                var getmenu4 = new List<sys_sub2menu>();
        //                                if (dt_levelfour.Rows.Count != 0)
        //                                {

        //                                    getmenu4 = dt_levelfour.AsEnumerable().Select(row =>
        //                                      new sys_sub2menu
        //                                      {

        //                                          text = row["module_name"].ToString(),
        //                                          angular_sref = row["angular_sref"].ToString(),
        //                                          sref = row["sref"].ToString(),
        //                                          icon = row["icon"].ToString(),
        //                                      }).ToList();
        //                                }
        //                                dt_levelfour.Clear();
        //                                getmenu3.Add(new sys_sub1menu
        //                                {
        //                                    text = drthree["module_name"].ToString(),
        //                                    angular_sref = drthree["angular_sref"].ToString(),
        //                                    sref = drthree["sref"].ToString(),
        //                                    sub2menu = getmenu4
        //                                });
        //                            }

        //                        }
        //                        dt_levelthree.Clear();
        //                        getmenu2.Add(new sys_submenu
        //                        {
        //                            text = drTwo["module_name"].ToString(),
        //                            angular_sref = drTwo["angular_sref"].ToString(),
        //                            sref = drTwo["sref"].ToString(),
        //                            ennableState = true,
        //                            activeState = true,
        //                            sub1menu = getmenu3
        //                        });

        //                    }

        //                    dt_leveltwo.Clear();
        //                }
        //                else
        //                {
        //                    menu_ind_up_first = "";
        //                    menu_ind_down_first = "";
        //                }
        //                //var getmenu = new List<sys_menu>();
        //                getmenu.Add(new sys_menu
        //                {
        //                    text = drOne["module_name"].ToString(),
        //                    angular_sref = drOne["angular_sref"].ToString(),
        //                    sref = drOne["sref"].ToString(),
        //                    icon = drOne["icon"].ToString(),
        //                    label = "label label-success",
        //                    submenu = getmenu2
        //                });
        //                values.menu_list = getmenu;
        //            }

        //        }

        //        dt_levelone.Clear();
        //    }
        //    catch
        //    {
        //        values.status = false;
        //    }
        //    finally
        //    {
        //    }

        //}
        public void loadMenuFromDB(string user_gid, menu_response values)
        {
            var dt_data = new DataTable();
            List<sys_menu> getmenu = new List<sys_menu>();
            List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();



            msSQL = " CALL adm_mst_spGetMenuDataangular('" + user_gid + "')";
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
                             List<mdlMenuData> getSecondLevel = mdlMenuData.Where(a => a.menu_level == "2" 
                                    && a.module_gid_parent == i.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                    .Select(group => new mdlMenuData
                                    { 
                                      module_gid = group.Key,
                                      module_name = group.First().module_name,  
                                      sref = group.First().sref,
                                      icon = group.First().icon,
                                      menu_level = group.First().menu_level,
                                      module_gid_parent = group.First().module_gid_parent,
                                      display_order = group.First().display_order   
                                    }).ToList(); 
                            List<sys_submenu> getmenu2 = new List<sys_submenu>(); 
                            if (getSecondLevel != null)
                            {
                                foreach (var j in getSecondLevel)
                                {
                                    List<mdlMenuData> getThirdLevel = mdlMenuData.Where(a => a.menu_level == "3" && a.sref !=null && a.sref != ""
                                    && a.module_gid_parent == j.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                    .Select(group => new mdlMenuData
                                    {
                                        module_gid = group.Key,
                                        module_name = group.First().module_name,
                                        sref = group.First().sref,
                                        icon = group.First().icon,
                                        menu_level = group.First().menu_level,
                                        module_gid_parent = group.First().module_gid_parent,
                                        display_order = group.First().display_order
                                    }).ToList(); 
                                    List<sys_sub1menu> getmenu3 = new List<sys_sub1menu>();
                                    if (getThirdLevel != null)
                                    {
                                        foreach (var k in getThirdLevel)
                                        {
                                            var getFourthLevel = mdlMenuData.Where(a => a.menu_level == "4"
                                                                 && a.module_gid_parent == k.module_gid)
                                                                 .OrderBy(a => a.display_order)
                                                                 .GroupBy(a => a.module_gid).ToList();
                                            List<sys_sub2menu> getmenu4 = new List<sys_sub2menu>();
                                            if (getFourthLevel != null)
                                            {
                                                menu_ind_up_second = "fa fa-angle-up";
                                                menu_ind_down_second = "fa fa-angle-down";
                                                getmenu4 = getFourthLevel.SelectMany(group => group).Select(row => new sys_sub2menu
                                                {
                                                    text = row.module_name,
                                                    sref = row.sref,
                                                    icon = row.icon,
                                                }).ToList();
                                            }
                                            getmenu3.Add(new sys_sub1menu
                                            {
                                                text = k.module_name,
                                                sref = k.sref,
                                                sub2menu = getmenu4,
                                            }); 
                                        }
                                    }
                                    getmenu2.Add(new sys_submenu
                                    {
                                        text = j.module_name, 
                                        sref = j.sref, 
                                        sub1menu = getmenu3
                                    });
                                }
                            }
                            else
                            {
                                menu_ind_up_first = "";
                                menu_ind_down_first = "";
                            }
                            getmenu.Add(new sys_menu
                            {
                                text = i.module_name,
                                sref = i.sref,
                                icon = i.icon,
                                menu_indication = menu_ind_up_first,
                                menu_indication1 = menu_ind_down_first,
                                label = "label label-success",
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

        public void DaDashboardprivilegelevel(string user_gid, menu_response values)
        {
            List<sys_menu> getmenu = new List<sys_menu>();
            List<mdlMenuData> mdlMenuDataList = new List<mdlMenuData>();

            msSQL = " SELECT t1.module_gid as module_gid, t2.module_gid as access_module_gid, " +
                    " case when t1.module_gid = t2.module_gid then 'true' else 'false' end as menu_access, " +
                    " t1.module_name as module_name, t1.sref as sref, t1.menu_level as menu_level, " +
                    " t1.module_gid_parent as module_gid_parent, t1.display_order as display_order " +
                    " FROM( " +
                    " SELECT module_gid, module_name, sref, menu_level, module_gid_parent, display_order " +
                    " FROM adm_mst_tmodule " +
                    " WHERE lw_flag = 'Y' " +
                    " ) AS t1 " +
                    " left JOIN( " +
                    " SELECT module_gid " +
                    " FROM adm_mst_tprivilege " +
                    " WHERE user_gid = '" + user_gid + "' " +
                    " ) AS t2 " +
                    " ON t1.module_gid = t2.module_gid ";
            dt_levelone = objdbconn.GetDataTable(msSQL);
            if (dt_levelone != null)
            {
                mdlMenuDataList = cmnfunctions.ConvertDataTable<mdlMenuData>(dt_levelone);
                try
                {
                    List<mdlMenuData> getFirstLevel = mdlMenuDataList.Where(a => a.menu_level == "1").ToList();
                    if (getFirstLevel.Count != 0)
                    {
                        foreach (var i in getFirstLevel)
                        {
                            List<mdlMenuData> getSecondLevel = mdlMenuDataList.Where(a => a.menu_level == "2"
                                   && a.module_gid_parent == i.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                   .Select(group => new mdlMenuData
                                   {
                                       module_gid = group.Key,
                                       module_name = group.First().module_name,
                                       sref = group.First().sref,
                                       icon = group.First().icon,
                                       menu_level = group.First().menu_level,
                                       module_gid_parent = group.First().module_gid_parent,
                                       display_order = group.First().display_order
                                   }).ToList();
                            List<sys_submenu> getmenu2 = new List<sys_submenu>();
                            if (getSecondLevel != null)
                            {
                                foreach (var j in getSecondLevel)
                                {
                                    List<mdlMenuData> getThirdLevel = mdlMenuDataList.Where(a => a.menu_level == "3"
                                    && a.module_gid_parent == j.module_gid).OrderBy(a => a.display_order).GroupBy(a => a.module_gid)
                                    .Select(group => new mdlMenuData
                                    {
                                        module_gid = group.Key,
                                        module_name = group.First().module_name,
                                        menu_access = group.First().menu_access,
                                        sref = group.First().sref,
                                        icon = group.First().icon,
                                        menu_level = group.First().menu_level,
                                        module_gid_parent = group.First().module_gid_parent,
                                        display_order = group.First().display_order,
                                    }).ToList();
                                    List<sys_sub1menu> getmenu3 = new List<sys_sub1menu>();
                                    if (getThirdLevel != null)
                                    {
                                        foreach (var k in getThirdLevel)
                                        {
                                            var getFourthLevel = mdlMenuDataList.Where(a => a.menu_level == "4"
                                                                 && a.module_gid_parent == k.module_gid)
                                                                 .OrderBy(a => a.display_order)
                                                                 .GroupBy(a => a.module_gid).ToList();
                                            List<sys_sub2menu> getmenu4 = new List<sys_sub2menu>();
                                            if (getFourthLevel != null)
                                            {
                                                menu_ind_up_second = "fa fa-angle-up";
                                                menu_ind_down_second = "fa fa-angle-down";
                                                getmenu4 = getFourthLevel.SelectMany(group => group).Select(row => new sys_sub2menu
                                                {
                                                    text = row.module_name,
                                                    sref = row.sref,
                                                    icon = row.icon,
                                                    menu_access = row.menu_access,
                                                }).ToList();
                                            }
                                            getmenu3.Add(new sys_sub1menu
                                            {
                                                text = k.module_name,
                                                sref = k.sref,
                                                menu_access = k.menu_access,
                                                sub2menu = getmenu4,
                                            });
                                        }
                                    }
                                    getmenu2.Add(new sys_submenu
                                    {
                                        text = j.module_name,
                                        sref = j.sref,
                                        menu_access = j.menu_access,
                                        sub1menu = getmenu3
                                    });
                                }
                            }
                            else
                            {
                                menu_ind_up_first = "";
                                menu_ind_down_first = "";
                            }
                            getmenu.Add(new sys_menu
                            {
                                text = i.module_name,
                                sref = i.sref,
                                icon = i.icon,
                                menu_access = i.menu_access,
                                menu_indication = menu_ind_up_first,
                                menu_indication1 = menu_ind_down_first,
                                label = "label label-success",
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

    }
}