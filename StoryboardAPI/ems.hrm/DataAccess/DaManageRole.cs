using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.DataAccess;
using ems.hrm.Models;
using System.Web.Http;
using static OfficeOpenXml.ExcelErrorValue;
using MySql.Data.MySqlClient;


namespace ems.hrm.DataAccess
{
    public class DaManageRole : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetGid, lsrole_code;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnResult6;
        public bool DaRoleSummary(rolelist objrolelist)
        {
            try
            {
                
                msSQL = " select role_gid, role_code, role_name, reporting_to, reportingto_gid, job_description, " +
                        " probation_period ,role_responsible from hrm_mst_trole ORDER BY created_date DESC, role_gid DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getrole_list = new List<role>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        getrole_list.Add(new role
                        {
                            role_gid = dr_row["role_gid"].ToString(),
                            role_code = dr_row["role_code"].ToString(),
                            role_name = dr_row["role_name"].ToString(),
                            role_responsible = dr_row["role_responsible"].ToString(),
                            reportingto_gid = dr_row["reportingto_gid"].ToString(),
                            job_description = dr_row["job_description"].ToString(),
                            reporting_to = dr_row["reporting_to"].ToString(),
                            probation_period = dr_row["probation_period"].ToString()
                        });
                    }
                    objrolelist.role = getrole_list;
                    objrolelist.status = true;
                    return true;
                }
                else
                {
                    objrolelist.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objrolelist.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                return false;
            }
        }
        public void DaRoleAdd(role values, string user_gid)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("HRLE");
                if (values.Code_Generation == "N")
                {
                    {
                        msSQL = " SELECT role_code  FROM " +
                           " hrm_mst_trole WHERE role_code = '" + values.rolecode_manual + "'";
                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    }
                    if (objMySqlDataReader.HasRows)
                    {
                        values.status = false;
                        values.message = "Role code already Exist";
                    }
                    lsrole_code = values.rolecode_manual;
                }


                else
                {                   
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='HRLE' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    lsrole_code = "RC" + "000" + lsCode;
                }
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = "Role Gid Not genarated";
                    return;
                }
                else
                {
                    msSQL = "insert into hrm_mst_trole(" +
                          " role_gid," +
                          " role_code," +
                          " role_name," +
                          " reporting_to," +
                          " reportingto_gid," +
                          " job_description," +
                          " role_responsible," +
                          " probation_period," +
                          " created_by," +
                          " created_date) " +
                          " values(" +
                          "'" + msGetGid + "'," +
                          "'" + lsrole_code + "'," +
                          "'" + values.role_name + "'," +
                          "'" + values.reporting_to + "'," +
                          "'" + values.reportingto_gid + "'," +
                          "'" + values.job_description + "'," +
                          "'" + values.role_responsible + "'," +
                          "'" + values.probation_period + "'," +
                          "'" + user_gid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.message = "Role Added Successfully";
                        values.status = true;
                    }
                    else
                    {
                        values.message = "Error While Adding Role";
                        values.status = false;
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
        public bool DaRoleEdit(role objrole, string user_gid, string role_gid)
        {
            try
            {
                
                msSQL = "select role_gid, role_code, role_name, reporting_to, reportingto_gid, job_description," +
                       " probation_period,role_responsible from hrm_mst_trole where role_gid='" + role_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    objrole.role_gid = objMySqlDataReader["role_gid"].ToString();
                    objrole.role_code = objMySqlDataReader["role_code"].ToString();
                    objrole.role_name = objMySqlDataReader["role_name"].ToString();
                    objrole.role_responsible = objMySqlDataReader["role_responsible"].ToString();
                    objrole.reportingto_gid = objMySqlDataReader["reportingto_gid"].ToString();
                    objrole.job_description = objMySqlDataReader["job_description"].ToString();
                    objrole.reporting_to = objMySqlDataReader["reporting_to"].ToString();
                    objrole.probation_period = objMySqlDataReader["probation_period"].ToString();
                    objrole.status = true;
                    return true;
                }
                else
                {
                    objrole.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objrole.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }

        }
        public bool DaRoleUpdate(role objrole, string user_gid)
        {
            try
            {
                
                msSQL = "select role_name from hrm_mst_trole where role_gid='" + objrole.reportingto_gid + "'";
                objrole.reporting_to = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Update hrm_mst_trole set " +
                        " role_name = '" + objrole.role_name + "', " +
                        " role_code = '" + objrole.role_code + "', " +
                        " role_responsible='" + objrole.role_responsible + "'," +
                        " reportingto_gid='" + objrole.reportingto_gid + "' ," +
                        " probation_period='" + objrole.probation_period + "' ," +
                        " job_description='" + objrole.job_description + "'," +
                        " reporting_to='" + objrole.reporting_to + "'" +
                        " where role_gid='" + objrole.role_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    objrole.message = "Role Updated Successfully";
                    objrole.status = true;
                    return true;
                }
                else
                {
                    objrole.message = "Error while Updating Role";
                    objrole.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objrole.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }
        public bool DaPopRoleRepotingtoAdd(rolereporting_to_list objrolereporting_to_list)
        {
            try
            {
                
                msSQL = "select role_gid,concat(role_code,' || ',role_name) as role_name from hrm_mst_trole where 1=1";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getrole_reportingto = new List<rolereporting_to>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        getrole_reportingto.Add(new rolereporting_to
                        {
                            role_gid = dr_row["role_gid"].ToString(),
                            role_name = dr_row["role_name"].ToString()
                        });
                    }
                    objrolereporting_to_list.rolereporting_to = getrole_reportingto;
                    objrolereporting_to_list.status = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                objrolereporting_to_list.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }

        }
        public bool DaPopRoleReportingtoEdit(rolereporting_to_listEdit objrolereporting_to_list, string role_gid)
        {
            try
            {
                
                msSQL = "select role_gid,concat(role_code,' || ',role_name) as role_name" +
                        " from hrm_mst_trole where role_gid<>'" + role_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getrole_reportingto = new List<rolereporting_toEdit>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        getrole_reportingto.Add(new rolereporting_toEdit
                        {
                            rolereporting_to_gid = dr_row["role_gid"].ToString(),
                            rolereporting_to_name = dr_row["role_name"].ToString()
                        });
                    }
                    objrolereporting_to_list.rolereporting_toEdit = getrole_reportingto;
                    objrolereporting_to_list.status = true;
                    return true;
                }
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                objrolereporting_to_list.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }
        public bool DaRoleDelete(string role_gid, role objrole)
        {
            try
            {
                
                msSQL = "delete from hrm_mst_trole where role_gid='" + role_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    objrole.message = "Role Deleted Successfully";
                    objrole.status = true;
                    return true;
                }
                else
                {
                    objrole.message = "Error while Deleting the Role";
                    objrole.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objrole.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }
    }
}