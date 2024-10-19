using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ems.system.Models;
using ems.utilities.Functions;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;

namespace ems.system.DataAccess
{
    public class DaUserprivilege
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_dataTable, objtbl, dt_levelone;
        int mnResult;
        string lsuser_gid;

        public void DaGetEmployeedropdown(MdlUserprivilege values)
        {
            try
            {

                msSQL = "select concat(a.user_code,' || ',a.user_firstname, '  ', a.user_lastname) as employee_name,a.user_gid from adm_mst_tuser a " +
                        "left join hrm_mst_temployee b on a.user_gid = b.user_gid ";

                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeelists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new employeelists
                        {
                            employee_name = dt["employee_name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),

                        });
                        values.employeelists = getModuleList;

                    }

                }
                dt_dataTable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetOnChangeEmployee(MdlUserprivilege values, string user_gid)
        {
            try
            {

                msSQL = "select a.user_gid from adm_mst_tuser a" +
                        " Left Join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                        " where b.user_gid='" + user_gid + "' ";

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();

                    msSQL = "  select a.module_gid from adm_mst_tmodule a " +
                       " left join adm_mst_tprivilege b on b.module_gid=a.module_gid " +
                       " where menu_level = 3 and user_gid='" + lsuser_gid + "'";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    

                }
            }


            catch (Exception ex)


            {
                values.message = "Exception occured while loading Change Customer!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
 "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    public void DaUserPrivilegeList(MdlUserprivilege objvalues, menu_response values, string user_gid)

        {

            //var ModuleAssigned_user = new DataTable();

            List<sys_menu> getmenu = new List<sys_menu>();

            List<mdlMenuData> mdlMenuData = new List<mdlMenuData>();

            List<mdlMenuData> mdlUserAssignedData = new List<mdlMenuData>();

            //msSQL = " select m.module_gid, m.module_name, m.menu_level, m.display_order, m.module_gid_parent, " +

            //        " case when m.module_gid  in (select module_gid from adm_mst_tprivilegeangular where user_gid = '" + objvalues.user_gid + "')" +

            //        " then 'Y' else 'N' end as menu_access from adm_mst_tmoduleangular m " +

            //        " left join  adm_mst_tprivilegeangular p on m.module_gid = p.module_gid";

            msSQL = " select c.module_gid,a.menu_level,a.module_gid_parent, a.module_name, a.display_order from adm_mst_tmoduleangular a" +

                    " left join adm_mst_tprivilegeangular c on c.module_gid = a.module_gid " +

                    " where c.user_gid ='" + user_gid + "' order by display_order ";

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

        public void DaGetUserPrivilegeSummary(MdlUserprivilege values)
        {
            try
            {

                msSQL = " Select distinct a.user_gid,concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name, c.designation_gid, " +
                        " d.designation_name, c.employee_gid, e.branch_name, c.employee_level, " +
                        " c.department_gid, c.branch_gid, g.department_name FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                        " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                        " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                        " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                        " left join adm_mst_tentity b on b.entity_gid = c.entity_gid " +
                        " where a.user_status = 'Y' order by c.employee_gid asc ";

                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeelists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new employeelists
                        {
                            user_name = dt["user_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),

                        });
                        values.employeelists = getModuleList;

                    }

                }
                dt_dataTable.Dispose();
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
