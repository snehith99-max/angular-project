using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Windows.Media;
using System.EnterpriseServices.CompensatingResourceManager;

namespace ems.hrm.DataAccess
{
    public class DaLeaveopening
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string strClientIP = "", lssickleave,lscasualleave, lscompensatoryoff;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable, dt_datatable1;
        string query, subquery, msEmployeeGID, msUserGid, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGETGid, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsleavetype_gid, leavetype_name;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        

        public void DaGetLeaveopening(MdlOpeningleave values)
        {
            try
            {
              
                   msSQL = " select leavetype_gid,leavetype_name from hrm_mst_tleavetype where leavetype_status='Y'  group by leavetype_gid order by leavetype_gid asc ";
                   dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    string msSQL1 = "SELECT a.employee_gid, concat(c.user_code,' / ', c.user_firstname,' ',c.user_lastname) as Employee,";
                    string subquery = "";
                    string subquerySeparator = "";
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                         lsleavetype_gid = dt["leavetype_gid"].ToString();
                         leavetype_name = dt["leavetype_name"].ToString();

                        subquery += subquerySeparator + "(SELECT SUM(b.available_leavecount) AS available_leavecount " +
                                    "FROM hrm_mst_tleavecreditsdtl b " +
                                    "WHERE b.month='" + DateTime.Now.ToString("MM") + "' " +
                                    "AND b.year='" + DateTime.Now.ToString("yyyy") + "' " +
                                    "AND b.leavetype_gid='" + lsleavetype_gid + "' " +
                                    "AND a.employee_gid=b.employee_gid) AS '" + leavetype_name + "'";

                       
                            subquerySeparator = ", ";
                    }
                    msSQL1 += subquery + " FROM hrm_mst_temployee a " +
                                        "LEFT JOIN adm_mst_tuser c ON a.user_gid=c.user_gid " +
                                        "WHERE c.user_status='Y' AND a.attendance_flag='Y' " +
                                        "GROUP BY a.employee_gid " +
                                        "ORDER BY a.employee_gid DESC";
                    dt_datatable1 = objdbconn.GetDataTable(msSQL1);
                    values.datatablejson = JsonConvert.SerializeObject(dt_datatable1,Formatting.Indented);

                }

                var getModuleList = new List<leaveopening_list>();

                if (dt_datatable1.Rows.Count != 0)
                {

                    foreach (DataRow dt in dt_datatable1.Rows)
                    {

                        getModuleList.Add(new leaveopening_list
                        {

                            user_name = dt["Employee"].ToString(),
                            Sick_Leave = dt[leavetype_name].ToString(),
                            Casual_Leave = lscasualleave,
                            Compensatory_off = lscompensatoryoff,
                            employee_gid = dt["employee_gid"].ToString(),
                        });
                        values.leaveopening_list = getModuleList;
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
        public void DaGetleavegradeopeningdropdown(MdlOpeningleave values)
        {
            try
            {
                
                msSQL = " select leavegrade_gid,leavegrade_name " +
                    "from hrm_mst_tleavegrade ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leave_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leave_list
                        {
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),
                        });
                        values.leave_list = getModuleList;
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

        public void Daeditleaveopening(MdlOpeningleave values, string leavegrade_gid)
        {
            try
            {
                
                msSQL = " select a.leavetype_gid,c.leavetype_name,a.total_leavecount,a.available_leavecount,a.leave_limit,b.leavegrade_gid from hrm_mst_tleavegradedtl a " +
               " left join hrm_mst_tleavegrade b on a.leavegrade_gid=b.leavegrade_gid " +
               " left join hrm_mst_tleavetype c on a.leavetype_gid=c.leavetype_gid " +
               " where a.leavegrade_gid='" + leavegrade_gid + "' and a.active_flag='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leaveopeningbalance_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leaveopeningbalance_list
                        {
                            leavetype_name = dt["leavetype_name"].ToString(),
                            total_leavecount = dt["total_leavecount"].ToString(),
                            available_leavecount = dt["available_leavecount"].ToString(),
                            leave_limit = dt["leave_limit"].ToString(),
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),

                        });
                        values.leaveopeningbalance_list = getModuleList;
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

        public void DaPostleavebalance(leavebalance_list values)
        {
            try
            {
                
                if (values.flag == "1")
                {
                    msSQL = "  Delete from hrm_trn_tleavegrade2employee where employee_gid = '" + values.employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (values.total_leavecount != "" && values.available_leavecount != "")
                {
                    msGETGid = objcmnfunctions.GetMasterGID("LE2G");
                    msSQL = "Insert Into hrm_trn_tleavegrade2employee" +
                          "(leavegrade2employee_gid," +
                          " branch_gid," +
                          " employee_gid," +
                          " employee_name," +
                          " leavegrade_gid," +
                          " leavegrade_code," +
                          " leavegrade_name," +
                          " attendance_startdate," +
                          " attendance_enddate," +
                          " total_leavecount," +
                          " available_leavecount," +
                          " leave_limit)" +

                          " VALUES ( " +
                          "'" + msGETGid + "', " +
                          "'" + values.branch_gid + "'," +
                          "'" + values.employee_gid + "'," +
                          "'" + values.employee_name + "'," +
                          "'" + values.leavegrade_gid + "'," +
                          "'" + values.leavegrade_code + "'," +
                          "'" + values.leavegrade_name + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                          "'" + values.total_leavecount + "'," +
                          "'" + values.available_leavecount + "'," +
                          "'" + values.leave_limit + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;

                        values.message = "Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occur While Updating";
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
    }
}