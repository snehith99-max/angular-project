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
using OfficeOpenXml.Drawing.Slicer.Style;
using Org.BouncyCastle.Asn1.Ocsp;
namespace ems.hrm.DataAccess
{
    public class DaWeekoff
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty, msGetGID;
        OdbcDataReader objOdbcDataReader;
        string lsemployee_gid;
        DataTable dt_dataTable, dt_dataTable1, objtbl;
        int mnResult;



        public void DagetWeekOffSummary(MdlWeekoff values)
        {
            try
            {
                msSQL = " select a.employee_gid, concat(b.user_code, ' / ',b.user_firstname,' ',b.user_lastname) as user_name,e.department_name,d.branch_gid,e.department_gid, " +
                    " c.designation_name,d.branch_name " + " from hrm_mst_temployee a " + " left join adm_mst_tuser b on a.user_gid=b.user_gid " +
                    " left join adm_mst_tdesignation c on a.designation_gid=c.designation_gid  " + " left join hrm_mst_tbranch d on a.branch_gid=d.branch_gid " +
                    " left join hrm_mst_tdepartment e on a.department_gid=e.department_gid" + " left join hrm_trn_temployeetypedtl j on a.employee_gid=j.employee_gid " +
                    " where user_status='Y' ";



                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<WeekOffLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new WeekOffLists
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            //employee_code = dt["user_code"].ToString(),
                            employee_name = dt["user_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.WeekOffLists = getModuleList;
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
        public void DaGetbranchdropdownlist(MdlWeekoff values)
        {
            try
            {

                msSQL = " Select branch_name,branch_gid  " +
                    " from hrm_mst_tbranch ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbranchdropdownlists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getbranchdropdownlists
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.Getbranchdropdownlists = getModuleList;
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
        public void DaGetdepartmentdropdownlists(MdlWeekoff values)
        {
            try
            {

                msSQL = " Select department_name,department_gid  " +
                    " from hrm_mst_tdepartment ";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getdepartmentdropdownlists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Getdepartmentdropdownlists
                        {
                            department_name = dt["department_name"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.Getdepartmentdropdownlists = getModuleList;
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
        public void DagetWeekOffSummarySearch(string branch_name, string department_name, MdlWeekoff values)
        {
            try
            {
                msSQL = " select a.employee_gid, b.user_code, concat(b.user_firstname,' ',b.user_lastname) as user_name,e.department_name,d.branch_gid,e.department_gid, " +
                    " c.designation_name,d.branch_name " + " from hrm_mst_temployee a " + " left join adm_mst_tuser b on a.user_gid=b.user_gid " +
                    " left join adm_mst_tdesignation c on a.designation_gid=c.designation_gid  " + " left join hrm_mst_tbranch d on a.branch_gid=d.branch_gid " +
                    " left join hrm_mst_tdepartment e on a.department_gid=e.department_gid" + " left join hrm_trn_temployeetypedtl j on a.employee_gid=j.employee_gid " +
                    " where user_status='Y' and d.branch_gid='" + branch_name + "' and e.department_gid='" + department_name + "'";



                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<WeekOffLists>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new WeekOffLists
                        {
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            employee_code = dt["user_code"].ToString(),
                            employee_name = dt["user_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.WeekOffLists = getModuleList;
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
        public void daupdateweekoffemployee(string user_gid, weekoff_list values)
        {
            try
            {
                if (values.employee_gid1.Count > 1)
                {

                    for (int i = 0; i < values.employee_gid1.Count; i++)
                    {
                        msSQL = "select employee_gid from hrm_mst_tweekoffemployee where employee_gid='" + values.employee_gid1[i] + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            msSQL = " update hrm_mst_tweekoffemployee set " +
                                    " sunday='" + values.sunday + "', " +
                                    " monday='" + values.monday + "', " +
                                    " tuesday='" + values.tuesday + "', " +
                                    " wednesday='" + values.wednesday + "', " +
                                    " thursday='" + values.thursday + "', " +
                                    " friday='" + values.friday + "', " +
                                    " saturday='" + values.saturday + "', " +
                                    " updated_by='" + user_gid + "', " +
                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " where employee_gid='" + values.employee_gid1[i] + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {


                            msGetGID = objcmnfunctions.GetMasterGID("WKOE");
                            msSQL = "insert into hrm_mst_tweekoffemployee (" +
                                  "weekoffemployee_gid," +
                                  "employee_gid," +
                                  "sunday," +
                                  "monday," +
                                  "tuesday," +
                                   "wednesday," +
                                    "thursday," +
                                     "friday," +
                                      "saturday," +
                                  "created_date," +
                                  "created_by)" +
                                  "values(" +
                                  "'" + msGetGID + "'," +
                                  "'" + values.employee_gid1[i] + "'," +
                                  "'" + values.sunday + "'," +
                                  "'" + values.monday + "'," +
                                "'" + values.tuesday + "'," +
                                 "'" + values.wednesday + "'," +
                                  "'" + values.thursday + "'," +
                                   "'" + values.friday + "'," +
                                    "'" + values.saturday + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                  "'" + user_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                else if (values.employee_gid1.Count == 1)
                {
                    for (int i = 0; i < values.employee_gid1.Count; i++)
                    {
                        msSQL = "select employee_gid from hrm_mst_tweekoffemployee where employee_gid='" + values.employee_gid1[i] + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            msSQL = " update hrm_mst_tweekoffemployee set " +
                                    " sunday='" + values.sunday + "', " +
                                    " monday='" + values.monday + "', " +
                                    " tuesday='" + values.tuesday + "', " +
                                    " wednesday='" + values.wednesday + "', " +
                                    " thursday='" + values.thursday + "', " +
                                    " friday='" + values.friday + "', " +
                                    " saturday='" + values.saturday + "', " +
                                    " updated_by='" + user_gid + "', " +
                                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " where employee_gid='" + values.employee_gid1[i] + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {


                            msGetGID = objcmnfunctions.GetMasterGID("WKOE");
                            msSQL = "insert into hrm_mst_tweekoffemployee (" +
                                  "weekoffemployee_gid," +
                                  "employee_gid," +
                                  "sunday," +
                                  "monday," +
                                  "tuesday," +
                                   "wednesday," +
                                    "thursday," +
                                     "friday," +
                                      "saturday," +
                                  "created_date," +
                                  "created_by)" +
                                  "values(" +
                                  "'" + msGetGID + "'," +
                                  "'" + values.employee_gid1[i] + "'," +
                                  "'" + values.sunday + "'," +
                                  "'" + values.monday + "'," +
                                "'" + values.tuesday + "'," +
                                 "'" + values.wednesday + "'," +
                                  "'" + values.thursday + "'," +
                                   "'" + values.friday + "'," +
                                    "'" + values.saturday + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                  "'" + user_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                    
                

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Weekoff created Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while creating weekoff";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DagetViewWeekoffSummary(string employee_gid, MdlWeekoff values)
        {
            try
            {
                msSQL = " select a.weekoffemployee_gid, a.employee_gid, a.sunday, a.monday, a.tuesday, a.wednesday, a.thursday, a.friday, a.saturday, " +
                        " date_format(a.created_date,'%d-%m-%Y') as created_date, a.created_by from hrm_mst_tweekoffemployee a " +
                        " where a.employee_gid = '" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<weekoffview_list>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new weekoffview_list
                        {
                            weekoffemployee_gid = dt["weekoffemployee_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            sunday = dt["sunday"].ToString(),
                            monday = dt["monday"].ToString(),
                            tuesday = dt["tuesday"].ToString(),
                            wednesday = dt["wednesday"].ToString(),
                            thursday = dt["thursday"].ToString(),
                            friday = dt["friday"].ToString(),
                            saturday = dt["saturday"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                        });
                        values.weekoffview_list = getModuleList;

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

        public void DaGetEmployeename(MdlWeekoff values, string employee_gid)
        {
            try
            {

                msSQL = " SELECT a.employee_gid, CONCAT(user_code, ' / ' ,u.user_firstname, ' ', u.user_lastname) AS employee_name " +
                        " FROM adm_mst_tuser u " +
                        " LEFT JOIN hrm_mst_temployee a ON u.user_gid = a.user_gid " +
                        " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_dataTable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Employee_type>();
                if (dt_dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_dataTable.Rows)
                    {
                        getModuleList.Add(new Employee_type
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.Employee_type = getModuleList;
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
    }
}