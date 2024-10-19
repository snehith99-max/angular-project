using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.system.Models;
using System.Text;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Security.Cryptography.X509Certificates;
//using Org.BouncyCastle.Asn1.Ocsp;
using System.Web.UI.WebControls;
using static OfficeOpenXml.ExcelErrorValue;
using System.Web.UI.HtmlControls;

namespace ems.system.DataAccess
{
    public class DaSysMstOriganisationHierarchy
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        StringBuilder strStringBuilder = new StringBuilder();

        LinkButton objImageButton = new LinkButton();

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        DataSet ds_dataset;
        int mnResult, lshierarchycount, lstranshierarchy;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift, summaryTable, lsbranchgid;
        public void DaOriganisationHierarchysummary(string module_gid, MdlSysMstOriganisationHierarchy values)
        {
            try
            {
                msSQL = " SELECT max(hierarchy_level) as level FROM adm_mst_tmodule2employee where module_gid = '" + module_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lshierarchycount = int.Parse(objMySqlDataReader["level"].ToString());

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

                    summaryTable += "</tr>";
                    summaryTable += loopNodes(module_gid, lshierarchycount);
                    summaryTable += "</table>";
                    values.level_list = summaryTable;
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public string loopNodes(string module_gid, int lshierarchycount)
        {
           
                string employee_gid = "";
                summaryTable = "<table width=100% class=Summary>";

                msSQL = " select b.employee_gid, b.user_gid, b.branch_gid, concat(c.user_code,' || ',c.user_firstname,' ',c.user_lastname) as user_firstname,c.user_code, a.hierarchy_level as hierarchy,d.designation_name,e.department_name from adm_mst_tmodule2employee a" +
                        " inner join hrm_mst_temployee b on b.employee_gid = a.employee_gid " +
                        " inner join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " left join adm_mst_tdesignation d on b.designation_gid=d.designation_gid " +
                        " left join hrm_mst_tdepartment e on b.department_gid=e.department_gid " +
                        " where a.hierarchy_level = 1 and a.module_gid = '" + module_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    employee_gid = objMySqlDataReader["employee_gid"].ToString();

                    summaryTable += "<tr class=Primary>";
                    summaryTable += "<td nowrap='nowrap' width='200px' height='20px' align='center'>";
                    summaryTable += "<td> <b>Name : </b>" + objMySqlDataReader["user_firstname"] + " <br/> <b>Designation : </b>" + objMySqlDataReader["designation_name"] + " <br/> <b>Department : </b>" + objMySqlDataReader["department_name"] + "</td>";

                    for (int i = 1; i < lshierarchycount; lshierarchycount--)
                    {
                        summaryTable += "<td></td>";
                    }

                    summaryTable += "</tr>";
                    summaryTable += childloop(employee_gid, module_gid);
                }
                else
                {

                }
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
        public void DaClearHierarchy(string module_gid, MdlSysMstOriganisationHierarchy values)
        {
            try
            {
                msSQL = " update adm_mst_tmodule2employee " +
                        " set hierarchy_level = '-1', employeereporting_to = '' " +
                        " where hierarchy_level > '1' and module_gid = '" + module_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "All Hierarchy levels cleared successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while clearing hierarchy levels";
                }
            }

            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}