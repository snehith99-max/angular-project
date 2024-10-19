using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using System.Web.Security;
using System.Xml.Linq;
using System.Text;
namespace ems.hrm.DataAccess
{
    public class DaRelievingLetter
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_dataTable, objtbl;
        int mnResult;


        public void DagetRelievingLetterSummary(MdlRelievingLetter values)
        {
            try
            {
                msSQL = " select a.releiving_gid, a.employee_gid, d.branch_prefix, e.department_name, concat(c.user_code,' / ', c.user_firstname,' ', c.user_lastname) as employee_name, " +
                        " f.designation_name from hrm_trn_treleivingletter a left join hrm_mst_temployee b on a.employee_gid = b.employee_gid left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                        " left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid left join hrm_mst_tdepartment e on b.department_gid = e.department_gid " +
                        " left join adm_mst_tdesignation f on b.designation_gid = f.designation_gid left join hrm_trn_temployeetypedtl j on a.employee_gid = j.employee_gid where user_status ='N' order by a.releiving_gid desc ";                
                dt_dataTable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<RelievingLetterLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new RelievingLetterLists
                        {
                            releiving_gid = dt["releiving_gid"].ToString(),
                            Branch = dt["branch_prefix"].ToString(),
                            Department = dt["department_name"].ToString(),
                            Employee_Name = dt["employee_name"].ToString(),
                            Designation = dt["designation_name"].ToString(),
                            
                        });
                        values.RelievingLetterLists = getModuleList;
                    }
                }
                dt_dataTable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteRelievingLetter(string releiving_gid, RelievingLetterLists values)

        {
            msSQL = "  delete from hrm_trn_treleivingletter where releiving_gid='" + releiving_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Releiving Letter Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Releivingletter";
                }
            }

        }


        public void DaDeleteRelievingLetter(string releiving_gid, MdlRelievingLetter values)
        {
            try
            {
                msSQL = "  delete from hrm_trn_treleivingletter where releiving_gid='" + releiving_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Relieving Letter Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Relieving Letter";
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

    }
}