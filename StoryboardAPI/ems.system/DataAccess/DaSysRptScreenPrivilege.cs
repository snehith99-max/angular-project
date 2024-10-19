using ems.system.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.system.DataAccess
{
    public class DaSysRptScreenPrivilege
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsuser_name;

        public void DaGetScreenPrivilegeSummary(MdlSysRptScreenPrivilege values)
        {
            try
            {
                msSQL = " select privilege_gid,f.branch_name,d.department_name,c.user_code," +
                        " concat(c.user_firstname,' ',c.user_lastname)as Employee_name,e.designation_name from  adm_mst_tprivilege a " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                        " left join adm_mst_tuser c on c.user_gid=b.user_gid " +
                        " left join hrm_mst_tdepartment d on d.department_gid=b.department_gid " +
                        " left join adm_mst_tdesignation e on e.designation_gid=b.designation_gid " +
                        " left join hrm_mst_tbranch f on f.branch_gid=b.branch_gid ";
                //" Where a.module_gid ='" + module_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<screenprivilegedata_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new screenprivilegedata_list
                        {
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            Employee_name = dt["Employee_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),

                        });
                        values.screenprivilegedatalist = getModuleList;
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

        public void DaGetLevel1Menu(MdlSysRptScreenPrivilege values)
        {
            try
            {
                msSQL = " select module_name,module_gid from adm_mst_tmodule " +
                        " where menu_level = 1 " +
                        " Order by module_name ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetModuledropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetModuledropdown
                        {
                            module_name = dt["module_name"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                        });
                        values.GetLevel1Menu = getModuleList;
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

        public void DaGetLevel2Menu(MdlSysRptScreenPrivilege values)
        {
            try
            {
                msSQL = " select module_name,module_gid from adm_mst_tmodule " +
                        " WHERE menu_level = 2 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetModule2dropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetModule2dropdown
                        {
                            module_name = dt["module_name"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                        });
                        values.GetLevel2Menu = getModuleList;
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

        public void DaGetLevel3Menu(MdlSysRptScreenPrivilege values)
        {
            try
            {
                msSQL = " select module_name,module_gid from adm_mst_tmodule " +
                        " where menu_level = 3 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetModule3dropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetModule3dropdown
                        {
                            module_name = dt["module_name"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                        });
                        values.GetLevel3Menu = getModuleList;
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

        public void DaGetEmployeeLevel1Detail(string module_gid, MdlSysRptScreenPrivilege values)
        {
            try
            {
                msSQL = " select module_name,module_gid from adm_mst_tmodule " +
                        " WHERE menu_level = 2 and module_gid = '" + module_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetModule3dropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetModule3dropdown
                        {
                            module_name = dt["module_name"].ToString(),
                            module_gid = dt["module_gid"].ToString(),
                        });
                        values.GetLevel3Menu = getModuleList;
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
