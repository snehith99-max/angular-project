using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
namespace ems.hrm.DataAccess
{
    public class DaHrmTrnExitRequisition
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsemployee_name, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsuser_password, lsuser_firstname, lsbranch_gid, lsdesignation_gid, lsdepartment_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetExitRequisitionSummary(string employee_gid, MdlHrmtrnExitRequisition values)
        {
            try
            {
                msSQL = "select a.exitemployee_gid,e.employee_gid,date_format(a.created_date, '%d-%m-%Y') as created_date,concat(f.user_firstname,' ',f.user_lastname) as employee_name,f.user_code,b.branch_name,c.department_name, " +
                   " a.overall_status,d.designation_name,a.remarks from hrm_trn_texitemployee a " +
                   " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                   " left join hrm_mst_tdepartment c on a.department_gid=c.department_gid " +
                   " left join adm_mst_tdesignation d on a.designation_gid=d.designation_gid " +
                   " left join hrm_mst_temployee e on a.employee_gid=e.employee_gid " +
                   " left join adm_mst_tuser f on e.user_gid=f.user_gid " +
                   " where a.employee_gid='" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<exitrequisition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new exitrequisition_list
                        {
                            exitemployee_gid = dt["exitemployee_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            remarks = dt["remarks"].ToString(),


                        });
                        values.exitrequisition_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetExitEmployee(string user_gid, MdlHrmtrnExitRequisition values)
        {
            try
            {
                msSQL = " SELECT a.user_code, CONCAT(a.user_firstname, ' ', a.user_lastname) AS employee_name, " +
                        " c.branch_name, d.department_name, DATE_FORMAT(b.employee_joiningdate, '%d-%m-%Y') AS employee_joiningdate, " +
                        " e.designation_name FROM adm_mst_tuser a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.user_gid = b.user_gid " +
                        " LEFT JOIN hrm_mst_tbranch c ON b.branch_gid = c.branch_gid " +
                        " LEFT JOIN hrm_mst_tdepartment d ON b.department_gid = d.department_gid " +
                        " LEFT JOIN adm_mst_tdesignation e ON b.designation_gid = e.designation_gid " +
                        " WHERE a.user_gid = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetExitEmployee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetExitEmployee
                        {
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            designation_name = dt["designation_name"].ToString(),

                        });
                        values.GetExitEmployee = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostExitRequisition(string employee_gid, exitrequisition_list values)
        {
            try
            {

                msSQL = " select employee_gid from hrm_trn_texitemployee where  employee_gid='" + employee_gid + "' " + " and overall_status in('Approved','Pending') ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Already Applied";
                    return;
                }

                msSQL = " select a.branch_gid, a.designation_gid,department_gid " + " from hrm_mst_temployee a " + " where a.employee_gid='" + employee_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    objMySqlDataReader.Read();
                    lsbranch_gid = objMySqlDataReader["branch_gid"].ToString();
                    lsdesignation_gid = objMySqlDataReader["designation_gid"].ToString();
                    lsdepartment_gid = objMySqlDataReader["department_gid"].ToString();
                }

                string uiDateStr = values.relieving_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlrelieving_date = uiDate.ToString("yyyy-MM-dd");
                

                    msGetGid = objcmnfunctions.GetMasterGID("EXTE");



                    msSQL = " insert into hrm_trn_texitemployee ( " +
                    " exitemployee_gid, " +
                    " remarks, " +
                    " branch_gid, " +
                    " designation_gid, " +
                    " department_gid, " +
                    " employee_gid, " +
                    " overall_status, " +
                    " exit_date, " +
                    " created_by, " +
                    " created_date " +
                    " ) values ( " +
                    " '" + msGetGid + "', " +
                    " '" + values.reason + "', " +
                    " '" + lsbranch_gid + "', " +
                    " '" + lsdesignation_gid + "', " +
                    " '" + lsdepartment_gid + "', " +
                     " '" + employee_gid + "', " +
                    " 'Pending', " +
                    " '" + mysqlrelieving_date + "', " +
                    "'" + employee_gid + "', " +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Exit Requisition Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Exit Requisition";
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
