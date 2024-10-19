using ems.system.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;

namespace ems.system.DataAccess
{
    public class DaEmployeelist
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult, importcount;
        string msUserGid, msEmployeeGID,  msBiometricGID, msGetemployeetype, msTemporaryAddressGetGID, msPermanentAddressGetGID, usercode, lsuser_gid, lsemployee_gid, lsuser_code, lscountry_gid2, lscountry_gid;    
        HttpPostedFile httpPostedFile;
        string lstemcountry_gid, msdocument_gid, lspcountry_gid, lsentity_gid, lsdepartment_gid, module_gid, lsbranch_gid, uppercasedbvalue, lsdesignation_gid;
        int ErrorCount;
        public void DaPostEmployeedetails(employee_lists values,string user_gid)
        {
            try
            {
                msSQL = "SELECT user_code FROM adm_mst_tuser " +
                      "WHERE LOWER(user_code) = LOWER('" + values.user_code + "') " +
                      "OR UPPER(user_code) = UPPER('" + values.user_code + "')";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Employee Code already exist";
                    return;
                }

                msUserGid = objcmnfunctions.GetMasterGID("SUSM");

                    msSQL = " insert into adm_mst_tuser(" +
                            " user_gid," +
                            " user_code," +
                            " user_firstname," +
                            " user_lastname, " +
                            " user_password, " +
                            " user_status, " +
                            " usergroup_gid, " +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msUserGid + "'," +
                            " '" + values.user_code + "'," +
                            " '" + values.first_name + "'," +
                            " '" + values.last_name + "'," +
                            " '" + objcmnfunctions.ConvertToAscii(values.password) + "'," +
                            "'" + values.active_flag + "'," +
                            "'" + values.usergrouptemplate + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");

                        msBiometricGID = objcmnfunctions.GetBiometricGID();
                        
                        msSQL1 =    " Insert into hrm_mst_temployee " +
                                    " (employee_gid , " +
                                    " user_gid," +
                                    " designation_gid," +
                                    " employee_mobileno , " +
                                    " employee_emailid , " +
                                    " employee_companyemailid, " +
                                    " employee_gender , " +
                                    " department_gid," +
                                    " entity_gid," +
                                    " employee_photo," +
                                    " useraccess," +
                                    " engagement_type," +
                                    " attendance_flag, " +
                                    " branch_gid, " +
                                    " biometric_id, " +
                                    " created_by, " + 
                                    " created_date " +
                                    " )values( " +
                                    "'" + msEmployeeGID + "', " +
                                    "'" + msUserGid + "', " +
                                    "'" + values.designationname + "'," +
                                    "'" + values.mobile + "'," +
                                    "'" + values.email + "'," +
                                    "'" + values.comp_email+ "'," +
                                    "'" + values.gender + "'," +
                                    "'" + values.departmentname + "'," +
                                    "'" + values.entityname + "'," +
                                    "'" + null + "'," +
                                    "'" + values.active_flag + "'," +
                                    "'Direct'," +
                                    "'Y'," +
                                    " '" + values.branchname + "'," +
                                    "'" + msBiometricGID + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                        if (mnResult == 1)
                        {
                            msSQL = " update  hrm_mst_temployee set " +
                                    " employee_photo = '/assets/media/images/Employee_defaultimage.png'" +
                                    " where employee_gid='" + msEmployeeGID + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            
                            msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");
                            
                            msSQL =   " insert into hrm_trn_temployeetypedtl(" +
                                      " employeetypedtl_gid," +
                                      " employee_gid," +
                                      " workertype_gid," +
                                      " systemtype_gid, " +
                                      " branch_gid, " +
                                      " wagestype_gid, " +
                                      " department_gid, " +
                                      " employeetype_name, " +
                                      " designation_gid, " +
                                      " created_by, " +
                                      " created_date)" +
                                      " values(" +
                                      " '" + msGetemployeetype + "'," +
                                      " '" + msEmployeeGID + "'," +
                                      " 'null'," +
                                      " 'Audit'," +
                                      " '" + values.branchname + "'," +
                                      " 'wg001'," +
                                      " '" + values.departmentname + "'," +
                                      "'Roll'," +
                                      " '" + values.designationname + "'," +
                                      "'" + user_gid + "'," +
                                      "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        
                            string permanent_address1 = values.permanent_address1.Replace("'", "\\'");
                            string permanent_address2 = values.permanent_address2.Replace("'", "\\'");
                            string temporary_address1 = values.temporary_address1.Replace("'", "\\'");
                            string temporary_address2 = values.temporary_address2.Replace("'", "\\'");

                            if (values.permanent_address1 == "" || values.permanent_address1 == null)
                            {
                                permanent_address1 = null; 
                            }
                            else
                            {
                                permanent_address1 = values.permanent_address1.Replace("'", "\\'");

                            }

                            if(values.permanent_address2 == "" || values.permanent_address2 == null)
                            {
                                permanent_address2 = null;
                            }
                            else
                            {
                                permanent_address2 = values.permanent_address2.Replace("'", "\\'");
                            }

                            if (values.temporary_address1 == "" || values.temporary_address1 == null)
                            {
                                temporary_address1 = null;
                            }
                            else
                            {
                                temporary_address1 = values.temporary_address1.Replace("'", "\\'");

                            }

                            if (values.temporary_address2 == "" || values.temporary_address2 == null)
                            {
                                temporary_address2 = null;
                            }
                            else
                            {
                                temporary_address2 = values.temporary_address2.Replace("'", "\\'");
                            }



                            //string permanent_address2 = values.permanent_address2.Replace("'", "\\'");
                            //string temporary_address1 = values.temporary_address1.Replace("'", "\\'");
                            //string temporary_address2 = values.temporary_address2.Replace("'", "\\'");

                            if (mnResult == 1)
                            {
                                msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");
                                msTemporaryAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

                                msSQL = " insert into adm_mst_taddress(" +
                                        " address_gid," +
                                        " parent_gid," +
                                        " country_gid," +
                                        " address1, " +
                                        " address2, " +
                                        " city, " +
                                        " state, " +
                                        " address_type, " +
                                        " postal_code, " +
                                        " created_by, " +
                                        " created_date)" +
                                        " values(" +
                                        " '" + msPermanentAddressGetGID + "'," +
                                        " '" + msEmployeeGID + "'," +
                                        " '" + values.country + "'," +
                                        " '" + permanent_address1 + "'," +
                                        " '" + permanent_address2 + "'," +
                                        " '" + values.permanent_city.Replace("'", "\\'") + "'," +
                                        " '" + values.permanent_state.Replace("'", "\\'") + "'," +
                                        " 'Permanent'," +
                                        " '" + values.permanent_postal + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 1)
                                {
                                    msSQL = " insert into adm_mst_taddress(" +
                                            " address_gid," +
                                            " parent_gid," +
                                            " country_gid," +
                                            " address1, " +
                                            " address2, " +
                                            " city, " +
                                            " state, " +
                                            " address_type, " +
                                            " postal_code, " +
                                            " created_by, " +
                                            " created_date)" +
                                            " values(" +
                                            " '" + msTemporaryAddressGetGID + "'," +
                                            " '" + msEmployeeGID + "'," +
                                            " '" + values.countryname + "'," +
                                            " '" + temporary_address1 + "'," +
                                            " '" + temporary_address2 + "'," +
                                            " '" + values.temporary_city.Replace("'", "\\'") + "'," +
                                            " '" + values.temporary_state.Replace("'", "\\'") + "'," +
                                            " 'Temporary'," +
                                            "'" + values.temporary_postal + "'," +
                                            "'" + user_gid + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                    }
                    if(mnResult == 1)
                {
                    msSQL = "select usergrouptemplatedtl_gid, usergrouptemplate_gid, module_gid, menu_level," +
                         " created_by, created_date, updated_by, updated_date from adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid='" + values.usergrouptemplate + "'";
                    dt_datatable=objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            string msgetprivilegefid = objcmnfunctions.GetMasterGID("SPGM");

                            msSQL = " insert into adm_mst_tprivilegeangular(" +
                                         " privilege_gid," +
                                         " usergroup_gid," +
                                         " module_gid," +
                                         " user_gid, " +                                      
                                         " created_by, " +
                                         " created_date)" +
                                         " values(" +
                                         " '" + msgetprivilegefid + "'," +
                                         " '" + dt["usergrouptemplate_gid"].ToString() + "'," +
                                         " '" + dt["module_gid"].ToString() + "'," +
                                         " '" + msUserGid + "'," +                                
                                         "'" + user_gid + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Employee Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Employee";
                    }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetentitydropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = " select  entity_gid, entity_name " +
                        " from adm_mst_tentity a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by order by entity_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getentitydropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getentitydropdown
                        {
                            entity_gid = dt["entity_gid"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                        });
                        values.Getentitydropdown = getModuleList;
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
        public void DaGetdesignationdropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = " Select designation_name,designation_gid  " +
                        " from adm_mst_tdesignation "+
                        " WHERE designation_flag = 'Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdesignationdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdesignationdropdown
                        {
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                        });
                        values.Getdesignationdropdown = getModuleList;
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
        public void DaGetcountrydropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = " Select country_name,country_gid  " +
                        " from adm_mst_tcountry ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getcountrydropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcountrydropdown
                        {
                            country_name = dt["country_name"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                        });
                        values.Getcountrydropdown = getModuleList;
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
        public void DaGetcountry2dropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = " Select country_name as country_names,country_gid as country_gids  " +
                        " from adm_mst_tcountry ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getcountry2dropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcountry2dropdown
                        {
                            country_names = dt["country_names"].ToString(),
                            country_gids = dt["country_gids"].ToString(),
                        });
                        values.Getcountry2dropdown = getModuleList;
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
        public void DaGetbranchdropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = " Select branch_name,branch_gid  " +
                        " from hrm_mst_tbranch "+
                       " WHERE status = 'Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbranchdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getbranchdropdown
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.Getbranchdropdown = getModuleList;
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
        public void DaGetusergrouptempdropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = "SELECT usergrouptemplate_gid, usergrouptemplate_code, usergrouptemplate_name " +
                        "FROM adm_mst_tusergrouptemplate"+
                        " WHERE status = 'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getusergrouptempdropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getusergrouptempdropdown
                        {
                            usergrouptemplate_gid = dt["usergrouptemplate_gid"].ToString(),
                            usergrouptemplate_code = dt["usergrouptemplate_code"].ToString(),
                            usergrouptemplate_name = dt["usergrouptemplate_name"].ToString(),
                        });
                    }

                    values.Getusergrouptempdropdown = getModuleList;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetdepartmentdropdown(MdlEmployeelist values)
        {
            try
            {
                msSQL = " Select department_name,department_gid  " +
                        " from hrm_mst_tdepartment " +
                          " WHERE status = 'Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdepartmentdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdepartmentdropdown
                        {
                            department_name = dt["department_name"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.Getdepartmentdropdown = getModuleList;
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
        public void DaGetEmployeeSummary(MdlEmployeelist values)
        {
            try
            {
                msSQL = " select distinct a.user_gid, c.useraccess, concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name, c.employee_joiningdate, " +
                        " c.employee_gender, concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                        " d.designation_name, c.designation_gid, c.employee_gid, concat(n.entity_prefix,' / ',e.branch_prefix) as entity_name, case when a.user_status = 'Y' " +
                        " then 'Active' when a.user_status = 'N' then 'Inactive' end as user_status, c.department_gid, c.branch_gid, e.branch_name, g.department_name  " +
                        " from adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid left join adm_mst_tentity n on n.entity_gid = c.entity_gid " +
                        " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                        " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                        " left join adm_mst_tcountry k on j.country_gid = k.country_gid left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid where a.kot_user !='Y'  " +
                        " group by c.employee_gid order by c.employee_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getemployee_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getemployee_lists
                        {
                            user_gid = dt["user_gid"].ToString(),
                            useraccess = dt["useraccess"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            emp_address = dt["emp_address"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.Getemployee_lists = getModuleList;
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
        public void DaEmployeeProfileUpload(HttpRequest httpRequest, result objResult, string user_gid)
        {

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            string branch = httpRequest.Form[0];
            string department = httpRequest.Form[1];
            string designation = httpRequest.Form[2];
            string active_flag = httpRequest.Form[3];
            string user_code = httpRequest.Form[4];
            string password = httpRequest.Form[5];
            string first_name = httpRequest.Form[7];
            string last_name = httpRequest.Form[8];
            string gender = httpRequest.Form[9];
            string email = httpRequest.Form[10];
            string mobile = httpRequest.Form[11];
            string permanent_address1 = httpRequest.Form[12];
            string permanent_address2 = httpRequest.Form[13];
            string country = httpRequest.Form[14];
            string permanent_city = httpRequest.Form[15];
            string permanent_state = httpRequest.Form[16];
            string permanent_postal = httpRequest.Form[17];
            string temporary_address1 = httpRequest.Form[18];
            string temporary_address2 = httpRequest.Form[19];
            string countryname = httpRequest.Form[20];
            string temporary_city = httpRequest.Form[21];
            string temporary_state = httpRequest.Form[22];
            string temporary_postal = httpRequest.Form[23];
            string usergrouptemplate = httpRequest.Form[24];
            string comp_email = httpRequest.Form[25];
            //string usergrouptemplate_name = httpRequest.Form[26];

            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;                        
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);                        
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        string final_path = lspath + msdocument_gid + FileExtension;
                        msSQL = " SELECT user_code FROM adm_mst_tuser where user_code = '" + user_code + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                             lsuser_code = objOdbcDataReader["user_code"].ToString();
                        }
                        if (lsuser_code != null && lsuser_code != "")
                        {
                            lsuser_code = lsuser_code.ToUpper();
                        }
                        else
                        {
                            lsuser_code = null;

                        }
                        
                        string uppercaseString = user_code.ToUpper();

                        if (uppercaseString != lsuser_code)
                        {
                            msUserGid = objcmnfunctions.GetMasterGID("SUSM");

                            msSQL = " insert into adm_mst_tuser(" +
                                    " user_gid," +
                                    " user_code," +
                                    " user_firstname," +
                                    " user_lastname, " +
                                    " user_password, " +
                                    " user_status, " +                                 
                                    " usergroup_gid, " +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msUserGid + "'," +
                                    " '" + user_code + "'," +
                                    " '" + first_name + "'," +
                                    " '" + last_name + "'," +
                                    " '" + objcmnfunctions.ConvertToAscii(password) + "'," +
                                    "'" + active_flag + "'," +
                                    "'" + usergrouptemplate + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                                msBiometricGID = objcmnfunctions.GetBiometricGID();
                                msSQL1 =    " Insert into hrm_mst_temployee " +
                                            " (employee_gid , " +
                                            " user_gid," +
                                            " designation_gid," +
                                            " employee_mobileno , " +
                                            " employee_emailid , " +
                                            " employee_companyemailid ," +
                                            " employee_gender , " +
                                            " department_gid," +
                                            " employee_photo," +
                                            " useraccess," +
                                            " engagement_type," +
                                            " attendance_flag, " +
                                            " branch_gid, " +
                                            " biometric_id, " +
                                            " created_by, " +
                                            " file_name, " +
                                            " created_date " +
                                            " )values( " +
                                            "'" + msEmployeeGID + "', " +
                                            "'" + msUserGid + "', " +
                                            "'" + designation + "'," +
                                            "'" + mobile + "'," +
                                            "'" + email + "'," +
                                            "'" + comp_email + "'," +
                                            "'" + gender + "'," +
                                            "'" + department + "'," +
                                            "'" + final_path + "'," +
                                            "'" + active_flag + "'," +
                                            "'Direct'," +
                                            "'Y'," +
                                            " '" + branch + "'," +
                                            "'" + msBiometricGID + "'," +
                                            "'" + user_gid + "'," +
                                            "'" + httpPostedFile.FileName + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                                if (mnResult == 1)
                                {
                                    msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");
                                    msSQL = " insert into hrm_trn_temployeetypedtl(" +
                                  " employeetypedtl_gid," +
                                  " employee_gid," +
                                  " workertype_gid," +
                                  " systemtype_gid, " +
                                  " branch_gid, " +
                                  " wagestype_gid, " +
                                  " department_gid, " +
                                  " employeetype_name, " +
                                  " designation_gid, " +
                                  " created_by, " +
                                  " created_date)" +
                                  " values(" +
                                  " '" + msGetemployeetype + "'," +
                                  " '" + msEmployeeGID + "'," +
                                  " 'null'," +
                                  " 'Audit'," +
                                  " '" + branch + "'," +
                                  " 'wg001'," +
                                  " '" + department + "'," +
                                    "'Roll'," +
                                  " '" + designation + "'," +
                                   "'" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        //msSQL = "  SELECT country_gid FROM adm_mst_tcountry where country_name='" + country + "' ";
                                        //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        //if (objOdbcDataReader.HasRows)
                                        //{
                                        //    lscountry_gid = objOdbcDataReader["country_gid"].ToString();
                                        //}
                                      
                                        msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");
                                        msTemporaryAddressGetGID = objcmnfunctions.GetMasterGID("SADM");
                                        msSQL = " insert into adm_mst_taddress(" +
                                    " address_gid," +
                                    " parent_gid," +
                                    " country_gid," +
                                    " address1, " +
                                    " address2, " +
                                    " city, " +
                                    " state, " +
                                    " address_type, " +
                                    " postal_code, " +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msPermanentAddressGetGID + "'," +
                                    " '" + msEmployeeGID + "'," +
                                    " '" + country + "'," +
                                    " '" + permanent_address1 + "'," +
                                    " '" + permanent_address2 + "'," +
                                    " '" + permanent_city + "'," +
                                    " '" + permanent_state + "'," +
                                    "'Permanent'," +
                                    "'" + permanent_postal + "',";
                                        msSQL += "'" + user_gid + "'," +
                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            //msSQL = "  SELECT country_gid FROM adm_mst_tcountry where country_name='" + countryname + "' ";
                                            //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                            //if (objOdbcDataReader.HasRows)
                                            //{
                                            //    lscountry_gid2 = objOdbcDataReader["country_gid"].ToString();
                                            //}
                                            msSQL = " insert into adm_mst_taddress(" +
                                    " address_gid," +
                                    " parent_gid," +
                                    " country_gid," +
                                    " address1, " +
                                    " address2, " +
                                    " city, " +
                                    " state, " +
                                    " address_type, " +
                                    " postal_code, " +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msTemporaryAddressGetGID + "'," +
                                    " '" + msEmployeeGID + "'," +
                                    " '" + countryname + "'," +
                                    " '" + temporary_address1 + "'," +
                                    " '" + temporary_address2 + "'," +
                                    " '" + temporary_city + "'," +
                                    " '" + temporary_state + "'," +
                                    "'Temporary'," +
                                    "'" + temporary_postal + "',";
                                            msSQL += "'" + user_gid + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }

                                    }

                                    if (mnResult == 1)
                                    {
                                        msSQL = "select usergrouptemplatedtl_gid, usergrouptemplate_gid, module_gid, menu_level," +
                                                " created_by, created_date, updated_by, updated_date from adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid='" + usergrouptemplate + "'";
                                        dt_datatable = objdbconn.GetDataTable(msSQL);
                                        if (dt_datatable.Rows.Count != 0)
                                        {
                                            foreach (DataRow dt in dt_datatable.Rows)
                                            {
                                                string msgetprivilegefid = objcmnfunctions.GetMasterGID("SPGM");

                                                msSQL = " insert into adm_mst_tprivilegeangular(" +
                                                             " privilege_gid," +
                                                             " usergroup_gid," +
                                                             " module_gid," +
                                                             " user_gid, " +
                                                             " created_by, " +
                                                             " created_date)" +
                                                             " values(" +
                                                             " '" + msgetprivilegefid + "'," +
                                                             " '" + dt["usergrouptemplate_gid"].ToString() + "'," +
                                                             " '" + dt["module_gid"].ToString() + "'," +
                                                             " '" + msUserGid + "'," +
                                                             "'" + user_gid + "'," +
                                                             "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }
                                        }
                                    }


                                }
                            }

                            if (mnResult != 0)
                            {
                                objResult.status = true;
                                objResult.message = "Employee Added Successfully";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Adding Employee";
                            }
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Employee User Code Already Exist";



                        }


                    }
                }
            }
            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            //return true;

        }
        public void DaGetEditEmployeeSummary(string employee_gid, MdlEmployeelist values)
        {
            try
            {
                msSQL = " select b.user_password,a.employee_gid,a.employee_gender,z.entity_name,a.identity_no,date_format(a.employee_dob,'%d-%m-%Y') as employee_dob,a.employee_sign,a.bloodgroup, " +
                   " a.employee_image,a.employee_photo, a.file_name," +
                   " a.employee_emailid,a.employee_companyemailid,a.employee_mobileno,a.employee_qualification,a.employee_documents, " +
                   " (select address1 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_address1, " +
                   " (select address2 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_address2, " +
                   " (select city from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_city, " +
                   " (select state from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_state, " +
                   " (select postal_code from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_postalcode, " +
                   " (select address_gid from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_addressgid, " +
                   " (select n.country_name from adm_mst_taddress m LEFT JOIN adm_mst_tcountry n ON m.country_gid = n.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_country, " +
                   " (select r.country_gid from adm_mst_taddress q LEFT JOIN adm_mst_tcountry r ON q.country_gid = r.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_countrygid, " +
                   " (select address1 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_address1, " +
                   " (select address2 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_address2, " +
                   " (select city from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_city, " +
                   " (select state from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_state, " +
                   " (select postal_code from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_postalcode, " +
                   " (select address_gid from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_addressgid, " +
                   " (select p.country_name from adm_mst_taddress o LEFT JOIN adm_mst_tcountry p ON o.country_gid = p.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_country, " +
                   " (select t.country_gid from adm_mst_taddress s LEFT JOIN adm_mst_tcountry t ON s.country_gid = t.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_countrygid, " +
                   " a.employee_experience,a.employee_experiencedtl, a.employeereporting_to , a.employment_type , " +
                   " b.user_code,b.user_firstname,b.user_lastname, case when b.user_status = 'Y' then 'Active' when b.user_status = 'N' then 'Inactive' end as user_status,b.usergroup_gid,c.usergroup_code,a.entity_gid,a.designation_gid,a.department_gid, " +
                   " a.branch_gid,d.branch_name,  e.department_name,f.designation_name,  " +
                   " (select i.user_firstname from adm_mst_tuser i ,  hrm_mst_temployee j where i.user_gid = j.user_gid " +
                   " and a.employeereporting_to = j.employee_gid)  as approveby_name,(date_format(a.employee_joiningdate,'%d/%m/%Y')) as employee_joiningdate, " +
                  " ( Select k.user_firstname from adm_mst_tuser k ,hrm_mst_temployee l " +
                  "  where k.user_gid = l.user_gid and l.employee_gid = '" + employee_gid + "')  as approver_name,a.nationality,a.nric_no,t.usergrouptemplate_gid,t.usergrouptemplate_name " +
                   " FROM hrm_mst_temployee a  LEFT JOIN adm_mst_tuser b ON a.user_gid = b.user_gid " +
                   " LEFT JOIN adm_mst_tusergroup c ON b.usergroup_gid = c.usergroup_gid " +
                   " LEFT JOIN hrm_mst_tbranch d ON a.branch_gid = d.branch_gid " +
                   " LEFT JOIN hrm_mst_tdepartment e ON a.department_gid = e.department_gid  " +
                   " LEFT JOIN adm_mst_tdesignation f ON a.designation_gid = f.designation_gid " +
                   " LEFT JOIN hrm_mst_tjobtype g ON a.jobtype_gid = g.jobtype_gid " +
                   " left join adm_mst_tentity z on z.entity_gid=a.entity_gid " +
                   " LEFT JOIN adm_mst_tusergrouptemplate t ON b.usergroup_gid = t.usergrouptemplate_gid " +
                   " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditEmployeeSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditEmployeeSummary
                        {


                            employee_gid = dt["employee_gid"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            identity_no = dt["identity_no"].ToString(),
                            employee_dob = dt["employee_dob"].ToString(),
                            employee_sign = dt["employee_sign"].ToString(),
                            bloodgroup = dt["bloodgroup"].ToString(),
                            entity_gid = dt["entity_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                            employee_image = dt["employee_image"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            comp_email = dt["employee_companyemailid"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            employee_documents = dt["employee_documents"].ToString(),
                            employee_experience = dt["employee_experience"].ToString(),
                            employee_experiencedtl = dt["employee_experiencedtl"].ToString(),
                            employeereporting_to = dt["employeereporting_to"].ToString(),
                            employment_type = dt["employment_type"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_lastname = dt["user_lastname"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            usergroup_gid = dt["usergroup_gid"].ToString(),
                            usergroup_code = dt["usergroup_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            approveby_name = dt["approveby_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            approver_name = dt["approver_name"].ToString(),
                            nationality = dt["nationality"].ToString(),
                            permanent_countrygid = dt["permanent_countrygid"].ToString(),
                            temporary_countrygid = dt["temporary_countrygid"].ToString(),
                            nric_no = dt["nric_no"].ToString(),
                            permanent_address1 = dt["permanent_address1"].ToString(),
                            permanent_address2 = dt["permanent_address2"].ToString(),
                            permanent_city = dt["permanent_city"].ToString(),
                            permanent_state = dt["permanent_state"].ToString(),
                            permanent_postalcode = dt["permanent_postalcode"].ToString(),
                            permanent_country = dt["permanent_country"].ToString(),
                            permanent_addressgid = dt["permanent_addressgid"].ToString(),
                            temporary_address1 = dt["temporary_address1"].ToString(),
                            temporary_address2 = dt["temporary_address2"].ToString(),
                            temporary_city = dt["temporary_city"].ToString(),
                            temporary_postalcode = dt["temporary_postalcode"].ToString(),
                            temporary_country = dt["temporary_country"].ToString(),
                            temporary_state = dt["temporary_state"].ToString(),
                            temporary_addressgid = dt["temporary_addressgid"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            user_password = dt["user_password"].ToString(),
                            file_name = dt["file_name"].ToString(),
                            usergrouptemplate_gid = dt["usergrouptemplate_gid"].ToString(),
                            usergrouptemplate_name = dt["usergrouptemplate_name"].ToString()
                        });
                        values.GetEditEmployeeSummary = getModuleList;      
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
        public void DaUpdateEmployeeProfileUpload(HttpRequest httpRequest, result objResult, string user_gid)
        {

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            //string entity = httpRequest.Form[0];
            string branch = httpRequest.Form[0];
            string department = httpRequest.Form[1];
            string designation = httpRequest.Form[2];
            string active_flag = httpRequest.Form[3];
            string user_code = httpRequest.Form[4];
            string first_name = httpRequest.Form[7];
            string last_name = httpRequest.Form[8];
            string gender = httpRequest.Form[9];
            string email = httpRequest.Form[10];
            string mobile = httpRequest.Form[11];
            string permanent_address1 = httpRequest.Form[12];
            string permanent_address2 = httpRequest.Form[13];
            string country = httpRequest.Form[14];
            string permanent_city = httpRequest.Form[15];
            string permanent_state = httpRequest.Form[16];
            string permanent_postal = httpRequest.Form[17];
            string temporary_address1 = httpRequest.Form[18];
            string temporary_address2 = httpRequest.Form[19];
            string countryname = httpRequest.Form[20];
            string temporary_city = httpRequest.Form[21];
            string temporary_state = httpRequest.Form[22];
            string temporary_postal = httpRequest.Form[23];
            string permanent_addressgid = httpRequest.Form[24];
            string temporary_addressgid = httpRequest.Form[25];
            string employee_gid = httpRequest.Form[26];
            string usergrouptemplate = httpRequest.Form[27];
            string comp_email = httpRequest.Form[28];

            string lsusergrouptemplate_gid = "";
            if (usergrouptemplate != null && usergrouptemplate != "")
            {
                msSQL = " select usergrouptemplate_gid from adm_mst_tusergrouptemplate where usergrouptemplate_name = '" + usergrouptemplate + "'";
                 lsusergrouptemplate_gid = objdbconn.GetExecuteScalar(msSQL);
            }
            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        //string local_path = "E:/Angular15/AngularUI/src";
                        ms.Close();
                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        string final_path = lspath + msdocument_gid + FileExtension;

                        msSQL = " SELECT user_gid FROM hrm_mst_temployee where employee_gid = '" + employee_gid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                        }
                        objOdbcDataReader.Close();  
                        msSQL = " update  adm_mst_tuser set " +
                                " user_firstname = '" + first_name + "'," +
                                " user_lastname = '" + last_name + "'," +
                                " user_status = '" + active_flag + "'," +
                                " usergroup_gid = '" + lsusergrouptemplate_gid + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {

                            msSQL = " update  hrm_mst_temployee set " +
                            " designation_gid = '" + designation + "'," +
                            " employee_mobileno = '" + mobile + "'," +
                            "employee_emailid = '" + email + "'," +
                            "employee_companyemailid = '" + comp_email +"'," +
                            "employee_gender = '" + gender + "'," +
                            "department_gid = '" + department + "'," +
                             "employee_photo = '" + final_path + "'," +
                             "file_name = '" + httpPostedFile.FileName + "'," +
                            "useraccess = '" + active_flag + "'," +
                            "branch_gid = '" + branch + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where employee_gid='" + employee_gid + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {

                                msSQL = " update hrm_trn_temployeetypedtl set " +
                                " wagestype_gid='wg001', " +
                                " systemtype_gid='Audit', " +
                                " branch_gid='" + branch + "', " +
                                " employeetype_name='Roll', " +
                              " department_gid='" + department + "', " +
                              " designation_gid='" + designation + "', " +
                              " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                              " updated_by='" + user_gid + "'" +
                              " where employee_gid = '" + employee_gid + "' ";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    //msSQL = "  SELECT country_gid FROM adm_mst_tcountry where country_name='" + country + "' ";
                                    //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    //if (objOdbcDataReader.HasRows)
                                    //{
                                    //    lscountry_gid = objOdbcDataReader["country_gid"].ToString();
                                    //}
                                    msSQL = " update adm_mst_taddress SET " +
                                    " country_gid = '" + country + "', " +
                                    " address1 =  '" + permanent_address1 + "', " +
                                    " address2 = '" + permanent_address2 + "'," +
                                    " city = '" + permanent_city + "', " +
                                    " state = '" + permanent_state + "', " +
                                    " postal_code = '" + permanent_postal + "'" +
                                    " where address_gid = '" + permanent_addressgid + "' and " +
                                    " parent_gid = '" + employee_gid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {

                                        msSQL = " update adm_mst_taddress SET " +
                                        " country_gid = '" + countryname + "', " +
                                        " address1 =  '" + temporary_address1 + "', " +
                                        " address2 = '" + temporary_address2 + "'," +
                                        " city = '" + temporary_city + "', " +
                                        " state = '" + temporary_state + "', " +
                                        " postal_code = '" + temporary_postal + "'" +
                                        " where address_gid = '" + temporary_addressgid + "' and " +
                                        " parent_gid = '" + employee_gid + "'";


                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                }


                            }
                        }

                        if (mnResult == 1)
                        {
                            if (usergrouptemplate != null && usergrouptemplate != "")
                            {
                                msSQL = "select usergrouptemplate_gid from adm_mst_tusergrouptemplate where usergrouptemplate_name = '" + usergrouptemplate + "'";
                                string lsusergrouptemplategid=objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "delete from adm_mst_tprivilegeangular where user_gid='" + lsuser_gid + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                msSQL = "select usergrouptemplatedtl_gid, usergrouptemplate_gid, module_gid, menu_level," +
                                     " created_by, created_date, updated_by, updated_date from adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid='" + lsusergrouptemplategid + "'";
                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                if (dt_datatable.Rows.Count != 0)
                                {
                                    foreach (DataRow dt in dt_datatable.Rows)
                                    {
                                        string msgetprivilegefid = objcmnfunctions.GetMasterGID("SPGM");

                                        msSQL = " insert into adm_mst_tprivilegeangular(" +
                                                     " privilege_gid," +
                                                     " usergroup_gid," +
                                                     " module_gid," +
                                                     " user_gid, " +
                                                     " created_by, " +
                                                     " created_date)" +
                                                     " values(" +
                                                     " '" + msgetprivilegefid + "'," +
                                                     " '" + lsusergrouptemplategid + "'," +
                                                     " '" + dt["module_gid"].ToString() + "'," +
                                                     " '" + lsuser_gid + "'," +
                                                     "'" + user_gid + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                }
                            }
                        }

                        if (mnResult != 0)
                        {
                            objResult.status = true;
                            objResult.message = "Employee Updated Successfully";
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error While Updating Employee";
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            //return true;

        }
        public void DaUpdateEmployeedetails(employee_lists values, string user_gid)
        {
            try
            {
                string permanent_address1 = values.permanent_address1.Replace("'", "\\'");
                string permanent_address2 = values.permanent_address2.Replace("'", "\\'");
                string temporary_address1 = values.temporary_address1.Replace("'", "\\'");
                string temporary_address2 = values.temporary_address2.Replace("'", "\\'");
                msSQL = " SELECT user_gid FROM hrm_mst_temployee where employee_gid = '" + values.employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                }

                msSQL = "Select usergrouptemplate_gid from adm_mst_tusergrouptemplate where usergrouptemplate_name = '" + values.usergrouptemplate + "'";
                string lsusergroup_gid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " update  adm_mst_tuser set " +
                                   " user_firstname = '" + values.first_name + "'," +
                                   " user_lastname = '" + values.last_name + "'," +
                                   " user_status = '" + values.active_flag + "'," +
                                   " usergroup_gid = '" + lsusergroup_gid + "'," +
                                   " updated_by = '" + user_gid + "'," +
                                   " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = " update  hrm_mst_temployee set " +
                                    " designation_gid = '" + values.designationname + "'," +
                                    " employee_mobileno = '" + values.mobile + "'," +
                                    "employee_emailid = '" + values.email + "'," +
                                    "employee_companyemailid = '" + values.comp_email + "'," +
                                    "employee_gender = '" + values.gender + "'," +
                                    "department_gid = '" + values.departmentname + "'," +
                                    "entity_gid = '" + values.entityname + "'," +
                                    "useraccess = '" + values.active_flag + "'," +
                                    "branch_gid = '" + values.branchname + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where employee_gid='" + values.employee_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = " update hrm_trn_temployeetypedtl set " +
                                                               " wagestype_gid='wg001', " +
                                                               " systemtype_gid='Audit', " +
                                                               " branch_gid='" + values.branchname + "', " +
                                                               " employeetype_name='Roll', " +
                                                             " department_gid='" + values.departmentname + "', " +
                                                             " designation_gid='" + values.designationname + "', " +
                                                             " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                             " updated_by='" + user_gid + "'" +
                                                             " where employee_gid = '" + values.employee_gid + "' ";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {

                            msSQL = " update adm_mst_taddress SET " +
                                               " country_gid = '" + values.country + "', " +
                                               " address1 =  '" + permanent_address1 + "', " +
                                               " address2 = '" + permanent_address2 + "'," +
                                               " city = '" + values.permanent_city.Replace("'", "\\'") + "', " +
                                               " state = '" + values.permanent_state.Replace("'", "\\'") + "', " +
                                               " postal_code = '" + values.permanent_postal + "'" +
                                               " where address_gid = '" + values.permanent_addressgid + "' and " +
                                               " parent_gid = '" + values.employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                //msSQL = "  SELECT country_gid FROM adm_mst_tcountry where country_name='" + values.countryname + "' ";
                                msSQL = " update adm_mst_taddress SET " +
                                                " country_gid = '" + values.countryname + "', " +
                                                " address1 =  '" + temporary_address1 + "', " +
                                                " address2 = '" + temporary_address2 + "'," +
                                                " city = '" + values.temporary_city.Replace("'", "\\'") + "', " +
                                                " state = '" + values.temporary_state.Replace("'", "\\'") + "', " +
                                                " postal_code = '" + values.temporary_postal + "'" +
                                                " where address_gid = '" + values.temporary_addressgid + "' and " +
                                                " parent_gid = '" + values.employee_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                }
                if (mnResult == 1)
                {
                    if (lsusergroup_gid != "" && lsusergroup_gid != null)
                    {
                        msSQL = "delete from adm_mst_tprivilegeangular where user_gid='" + lsuser_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = "select usergrouptemplatedtl_gid, usergrouptemplate_gid, module_gid, menu_level," +
                             " created_by, created_date, updated_by, updated_date from adm_mst_tusergrouptemplatedtl where usergrouptemplate_gid='" + lsusergroup_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                string msgetprivilegefid = objcmnfunctions.GetMasterGID("SPGM");

                                msSQL = " insert into adm_mst_tprivilegeangular(" +
                                             " privilege_gid," +
                                             " usergroup_gid," +
                                             " module_gid," +
                                             " user_gid, " +
                                             " created_by, " +
                                             " created_date)" +
                                             " values(" +
                                             " '" + msgetprivilegefid + "'," +
                                             " '" + dt["usergrouptemplate_gid"].ToString() + "'," +
                                             " '" + dt["module_gid"].ToString() + "'," +
                                             " '" + lsuser_gid + "'," +
                                             "'" + user_gid + "'," +
                                             "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Employee Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Employee";
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetresetpassword(string user_gid, employee_lists values)
        {
            try
            {
                msSQL = " select user_gid from hrm_mst_temployee where employee_gid = '" + values.employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                }

                msSQL = " update  adm_mst_tuser set " +
                 " user_password = '" + objcmnfunctions.ConvertToAscii(values.password) + "'," +
                 " updated_by = '" + user_gid + "'," +
                 " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Password Reset successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Resetting Password";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetupdateusercode(string user_gid, employee_lists values)
        {
            try
            {
                msSQL = " select user_gid from hrm_mst_temployee where employee_gid = '" + values.employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                }
                msSQL = " SELECT user_code FROM adm_mst_tuser where user_code = '" + values.user_code + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lsuser_code = objOdbcDataReader["user_code"].ToString();
                    if (lsuser_code != null && lsuser_code != "")
                    {
                        usercode = lsuser_code.ToUpper();
                    }
                    else
                    {
                        usercode = null;


                    }
                }
                string uppercaseString = values.user_code.ToUpper();

                if (uppercaseString != usercode)
                {
                    msSQL = " update  adm_mst_tuser set " +
                 " user_code = '" + values.user_code + "'," +
                 " updated_by = '" + user_gid + "'," +
                 " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "User Code Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating User Code";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "User Code Already Exist";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //public void DaGetEditEmployeeSummary(string employee_gid, MdlEmployeelist values)
        //{


        //    msSQL = " Select a.address1,a.address2,a.city, " +
        //        " a.postal_code,a.state,b.country_name" +
        //        " from adm_mst_taddress a,adm_mst_tcountry b " +
        //        " where  address_gid = '" & lspermanentaddressGID & "' and " +
        //        " a.country_gid = b.country_gid and " +
        //       " WHERE a.parent_gid = '" + employee_gid + "'";
        //    dt_datatable = objdbconn.GetDataTable(msSQL);
        //    var getModuleList = new List<GetEditEmployeeSummary>();
        //    if (dt_datatable.Rows.Count != 0)
        //    {
        //        foreach (DataRow dt in dt_datatable.Rows)
        //        {
        //            getModuleList.Add(new GetEditEmployeeSummary
        //            {


        //                employee_gid = dt["employee_gid"].ToString(),
        //                employee_gender = dt["employee_gender"].ToString(),
        //                entity_name = dt["entity_name"].ToString(),
        //                identity_no = dt["identity_no"].ToString(),
        //                employee_dob = dt["employee_dob"].ToString(),
        //                employee_sign = dt["employee_sign"].ToString(),
        //                bloodgroup = dt["bloodgroup"].ToString(),


        //            });
        //            values.GetEditEmployeeSummary = getModuleList;
        //        }
        //    }
        //    dt_datatable.Dispose();
        //}
        public void DaGetupdateuserdeactivate(string user_gid, employee_lists values)
        {
            try
            {

                msSQL = " select user_gid from hrm_mst_temployee where employee_gid = '" + values.employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                }

                msSQL = "update adm_mst_tuser set user_status='N' where user_gid='" + lsuser_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string remarks;

                if (values.remarks == "" || values.remarks == null)
                {
                    remarks = null;
                }
                else
                {
                    remarks = values.remarks.Replace("'", "\\'");
                }

                msSQL = " update  hrm_mst_temployee set " +
                        //" exit_date = '" + values.deactivation_date + "'," +
                        " exit_date = str_to_date('" + values.deactivation_date + "', '%d-%m-%Y')," +
                        " remarks  = '" + remarks + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "User Deactivated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deactivating User";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaEmployeeImport(HttpRequest httpRequest, string user_gid, result objResult, employee_lists values)
        {
            string lscompany_code;
            string excelRange, endRange;
            int rowCount, columnCount;

            try
            {
                int insertCount = 0;
                HttpFileCollection httpFileCollection;
                DataTable dt = null;

                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + " Import_Excel/Hrm_Module/EmployeeExcels/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();

                    msSQL = " insert into hrm_trn_temployeeuploadexcellog(" +
                            " uploadexcellog_gid," +
                            " fileextenssion," +
                            " uploaded_by, " +
                            " uploaded_date)" +
                            " values(" +
                            " '" + msdocument_gid + "'," +
                            " '" + FileExtension + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.ToString();
                    return;
                }
                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {

                    }
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            bool sheetExists = schemaTable.AsEnumerable().Any(row => row["TABLE_NAME"].ToString().Equals("Sheet1$", StringComparison.OrdinalIgnoreCase));
                            if (!sheetExists)
                            {
                                objResult.status = false;
                                objResult.message = "Invalid Excel sheet";
                                return;
                            }

                            using (OleDbCommand command = new OleDbCommand())
                            {
                                command.Connection = connection;
                                command.CommandText = "select * from [Sheet1$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                // Upload document
                                importcount = 0;

                                foreach (DataRow row in dataTable.Rows)
                                {
                                    string firstname = row["First Name"].ToString();
                                    string lastname = row["Last Name"].ToString();
                                    //string entity = row["Entity"].ToString();
                                    string branch = row["Branch"].ToString();
                                    string department = row["Department"].ToString();
                                    string designation = row["Designation"].ToString();
                                    string employeeacess = row["Employee Access(Y or N)"].ToString();
                                    string usercode = row["User Code"].ToString();
                                    string password = row["User Password"].ToString();
                                    string gender = row["Gender(Male or Female)"].ToString();
                                    string email = row["Personal Email Address"].ToString();
                                    string comp_email = row["Official Email Address"].ToString();
                                    string phno = row["Personal Phone Number"].ToString();
                                    string percity = row["Permanent_City"].ToString();
                                    string perstate = row["Permanent_State"].ToString();
                                    string percountry = row["Permanent_Country"].ToString();
                                    string perpincode = row["Permanent_Postal Code"].ToString();
                                    string peraddress = row["Permanent_Address"].ToString();
                                    string tepcity = row["Temporary_City"].ToString();
                                    string tepstate = row["Temporary_State"].ToString();
                                    string tepcountry = row["Temporary_Country"].ToString();
                                    string teopincode = row["Temporary_Postal Code"].ToString();
                                    string tepaddress = row["Temporary_Address"].ToString();
                                    string level = row["employee_level"].ToString();
                                    ErrorCount = 0;

                                    if (!string.IsNullOrEmpty(usercode))
                                    {

                                        if (string.IsNullOrEmpty(phno))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Phone number is empty' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        if (!(phno.Length >= 10 && phno.Length <= 12 && phno.All(char.IsDigit)))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Phone number is not correct' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                        }
                                        if (perpincode != null && perpincode != "" && !(perpincode.Length > 5 && perpincode.Length < 7 && perpincode.All(char.IsDigit)))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'The permanent pin code is not correct. It must be a 6-digit number' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                        }
                                        if (teopincode != null && teopincode != "" && !(teopincode.Length > 5 && teopincode.Length < 7 && teopincode.All(char.IsDigit)))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'The temporary pin code is not correct. It must be a 6-digit number.' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;

                                        }
                                        if (ValidatePassword(password))
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " ' Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                                        if (Regex.IsMatch(email, emailPattern))
                                        {
                                            Console.WriteLine("Valid email address.");
                                        }

                                        else
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " ' Email must contain a valid format (e.g., example@domain.com)' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }




                                        string emailPattern1 = @"^[^@\s]+@(?!gmail\.com$)(?!yahoo\.com$)(?!hotmail\.com$)(?!outlook\.com$)(?!live\.com$)[a-z0-9._%+-]+\.[a-z]{2,100}$";

                                        if (Regex.IsMatch(comp_email, emailPattern1))
                                        {
                                            Console.WriteLine("Valid email address.");
                                        }

                                        else
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " ' Invalid email format or public domain is not allowed' ," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }


                                        // getting country_gids
                                        msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + percountry.Replace("'", "\'").Trim() + "'";
                                        lspcountry_gid = objdbconn.GetExecuteScalar(msSQL);
                                        if (lspcountry_gid == "")
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                     " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Permanent country not found' ," +
                                                     " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        else { lspcountry_gid = lspcountry_gid; }

                                        msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + tepcountry.Replace("'", "\'").Trim() + "'";
                                        lstemcountry_gid = objdbconn.GetExecuteScalar(msSQL);
                                        if (lstemcountry_gid == "")
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                      " created_by, " +
                                                    " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Temporary country not found' ," +
                                                     " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        else { lstemcountry_gid = lstemcountry_gid; }

                                        //getting Entity Gid
                                        //msSQL = "select entity_gid from adm_mst_tentity where entity_name = '" + entity.Replace("'", "\'").Trim() + "'";
                                        //lsentity_gid = objdbconn.GetExecuteScalar(msSQL);
                                        //if (lsentity_gid == "")
                                        //{
                                        //    string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                        //    msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                        //            " uploaderrorlog_gid ," +
                                        //            " uploadexcellog_gid, " +
                                        //            " first_name, " +
                                        //            " last_name, " +
                                        //            " remarks, " +
                                        //              " created_by, " +
                                        //             " created_date, " +
                                        //            " user_code)" +
                                        //            " values(" +
                                        //            " '" + MstGid + "'," +
                                        //            " '" + msdocument_gid + "'," +
                                        //            " '" + firstname + "'," +
                                        //            " '" + lastname + "'," +
                                        //            " 'Entity not found' ," +
                                        //             " '" + user_gid + "'," +
                                        //              " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                        //            "'" + usercode + "')";
                                        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        //    ErrorCount++;
                                        //}
                                        //else { lsentity_gid = lsentity_gid; }

                                        //getting branch_gid
                                        msSQL = "select branch_gid from hrm_mst_tbranch where branch_name = '" + branch.Replace("'", "\'").Trim() + "'";
                                        lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);
                                        if (lsbranch_gid == "")
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                      " created_by, " +
                                                     " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Branch not found' ," +
                                                     " '" + user_gid + "'," +
                                                     " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        else { lsbranch_gid = lsbranch_gid; }

                                        //getting department_gid
                                        msSQL = "select department_gid from hrm_mst_tdepartment where department_name = '" + department.Replace("'", "\'").Trim() + "'";
                                        lsdepartment_gid = objdbconn.GetExecuteScalar(msSQL);
                                        if (lsdepartment_gid == "")
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                      " created_by, " +
                                                      " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Department not found' ," +
                                                     " '" + user_gid + "'," +
                                                     " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        else { lsdepartment_gid = lsdepartment_gid; }

                                        //getting designation_gid
                                        msSQL = "select designation_gid from adm_mst_tdesignation where designation_name = '" + designation.Replace("'", "\'").Trim() + "'";
                                        lsdesignation_gid = objdbconn.GetExecuteScalar(msSQL);
                                        if (lsdesignation_gid == "")
                                        {
                                            string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                            msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                    " uploaderrorlog_gid ," +
                                                    " uploadexcellog_gid, " +
                                                    " first_name, " +
                                                    " last_name, " +
                                                    " remarks, " +
                                                      " created_by, " +
                                                     " created_date, " +
                                                    " user_code)" +
                                                    " values(" +
                                                    " '" + MstGid + "'," +
                                                    " '" + msdocument_gid + "'," +
                                                    " '" + firstname + "'," +
                                                    " '" + lastname + "'," +
                                                    " 'Designation not found' ," +
                                                     " '" + user_gid + "'," +
                                                      " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    "'" + usercode + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            ErrorCount++;
                                        }
                                        else { lsdesignation_gid = lsdesignation_gid; }

                                        msSQL = " SELECT user_code FROM adm_mst_tuser where user_code = '" + usercode.Replace("'", "\'").Trim() + "' ";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows)
                                        {
                                            lsuser_code = objOdbcDataReader["user_code"].ToString();
                                        }

                                        if (lsuser_code != null)
                                        {
                                            uppercasedbvalue = lsuser_code.ToUpper();
                                        }

                                        string uppercaseString = usercode.ToUpper();
                                        if (uppercaseString != uppercasedbvalue && ErrorCount == 0)
                                        {
                                            msUserGid = objcmnfunctions.GetMasterGID("SUSM");
                                            msSQL = " insert into adm_mst_tuser(" +
                                                    " user_gid," +
                                                    " user_code," +
                                                    " user_firstname," +
                                                    " user_lastname, " +
                                                    " user_password, " +
                                                    " user_status, " +
                                                    " created_by, " +
                                                    " created_date)" +
                                                    " values(" +
                                                    " '" + msUserGid + "'," +
                                                    " '" + uppercaseString + "'," +
                                                    " '" + firstname.Replace("'", "\'").Trim() + "'," +
                                                    " '" + lastname.Replace("'", "\'").Trim() + "'," +
                                                    " '" + objcmnfunctions.ConvertToAscii(password) + "'," +
                                                    "'Y',";
                                            msSQL += "'" + user_gid + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            importcount++;
                                            if (mnResult == 1)
                                            {
                                                msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                                                msBiometricGID = objcmnfunctions.GetBiometricGID();
                                                msSQL1 = " Insert into hrm_mst_temployee " +
                                                            " (employee_gid , " +
                                                            " user_gid," +
                                                            " designation_gid," +
                                                            " employee_mobileno , " +
                                                            " employee_emailid , " +
                                                            " employee_companyemailid , " +
                                                            " employee_gender , " +
                                                            " department_gid," +
                                                            //" entity_gid," +
                                                            " employee_photo," +
                                                            " useraccess," +
                                                            " engagement_type," +
                                                            " attendance_flag, " +
                                                            " branch_gid, " +
                                                            " biometric_id, " +
                                                            " employee_level ," +
                                                            " created_by, " +
                                                            " created_date " +
                                                            " )values( " +
                                                            "'" + msEmployeeGID + "', " +
                                                            "'" + msUserGid + "', " +
                                                            "'" + lsdesignation_gid + "'," +
                                                            "'" + phno.Replace("'", "\'").Trim() + "'," +
                                                            "'" + email + "'," +
                                                            "'" + comp_email + "'," +
                                                            "'" + gender.Replace("'", "\'").Trim() + "'," +
                                                            "'" + lsdepartment_gid + "'," +
                                                            //"'" + lsentity_gid + "'," +
                                                            "'/assets/media/images/Employee_defaultimage.png'," +
                                                            "'" + employeeacess.Replace("'", "\'").Trim() + "'," +
                                                            "'Direct'," +
                                                            "'Y'," +
                                                            " '" + lsbranch_gid + "'," +
                                                            "'" + msBiometricGID + "'," +
                                                            "'" + level + "'," +
                                                            "'" + user_gid + "'," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                                                if (mnResult == 1)
                                                {
                                                    msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");
                                                    msSQL = " insert into hrm_trn_temployeetypedtl(" +
                                                              " employeetypedtl_gid," +
                                                              " employee_gid," +
                                                              " workertype_gid," +
                                                              " systemtype_gid, " +
                                                              " branch_gid, " +
                                                              " wagestype_gid, " +
                                                              " department_gid, " +
                                                              " employeetype_name, " +
                                                              " designation_gid, " +
                                                              " created_by, " +
                                                              " created_date)" +
                                                              " values(" +
                                                              " '" + msGetemployeetype + "'," +
                                                              " '" + msEmployeeGID + "'," +
                                                              " 'null'," +
                                                              " 'Audit'," +
                                                              " '" + lsbranch_gid + "'," +
                                                              " 'wg001'," +
                                                              " '" + lsdepartment_gid + "'," +
                                                              "'Roll'," +
                                                              " '" + lsdesignation_gid + "'," +
                                                              "'" + user_gid + "'," +
                                                              "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    if (mnResult == 1)
                                                    {
                                                        msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");
                                                        msTemporaryAddressGetGID = objcmnfunctions.GetMasterGID("SADM");
                                                        msSQL = " insert into adm_mst_taddress(" +
                                                                " address_gid," +
                                                                " parent_gid," +
                                                                " country_gid," +
                                                                " address1, " +
                                                                " city, " +
                                                                " state, " +
                                                                " address_type, " +
                                                                " postal_code, " +
                                                                " created_by, " +
                                                                " created_date)" +
                                                                " values(" +
                                                                " '" + msPermanentAddressGetGID + "'," +
                                                                " '" + msEmployeeGID + "'," +
                                                                " '" + lspcountry_gid + "'," +
                                                                " '" + peraddress.Replace("'", "\'").Trim() + "'," +
                                                                " '" + percity.Replace("'", "\'").Trim() + "'," +
                                                                " '" + perstate.Replace("'", "\'").Trim() + "'," +
                                                                " 'Permanent'," +
                                                                "'" + perpincode.Replace("'", "\'").Trim() + "',";
                                                        msSQL += "'" + user_gid + "'," +
                                                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        if (mnResult == 1)
                                                        {
                                                            msSQL = " insert into adm_mst_taddress(" +
                                                                    " address_gid," +
                                                                    " parent_gid," +
                                                                    " country_gid," +
                                                                    " address1, " +
                                                                    " city, " +
                                                                    " state, " +
                                                                    " address_type, " +
                                                                    " postal_code, " +
                                                                    " created_by, " +
                                                                    " created_date)" +
                                                                    " values(" +
                                                                    " '" + msTemporaryAddressGetGID + "'," +
                                                                    " '" + msEmployeeGID + "'," +
                                                                    " '" + lstemcountry_gid + "'," +
                                                                    " '" + tepaddress.Replace("'", "\'").Trim() + "'," +
                                                                    " '" + tepcity.Replace("'", "\'").Trim() + "'," +
                                                                    " '" + tepstate.Replace("'", "\'").Trim() + "'," +
                                                                    " 'Temporary'," +
                                                                    "'" + teopincode.Replace("'", "\'").Trim() + "',";
                                                            msSQL += "'" + user_gid + "'," +
                                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ErrorCount == 0)
                                            {
                                                string MstGid = objcmnfunctions.GetMasterGID("UPEE");
                                                msSQL = " insert into hrm_trn_temployeeuploadexcelerrorlog (" +
                                                        " uploaderrorlog_gid ," +
                                                        " uploadexcellog_gid, " +
                                                        " first_name, " +
                                                        " last_name, " +
                                                        " remarks, " +
                                                        " created_by, " +
                                                        " created_date, " +
                                                        " user_code)" +
                                                        " values(" +
                                                        " '" + MstGid + "'," +
                                                        " '" + msdocument_gid + "'," +
                                                        " '" + firstname + "'," +
                                                        " '" + lastname + "'," +
                                                        " 'user_code already exist' ," +
                                                        " '" + user_gid + "'," +
                                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                        "'" + usercode + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.ToString();
                    return;
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            msSQL = " update  hrm_trn_temployeeuploadexcellog set " +
                    " importcount = " + importcount + " " +
                    " where uploadexcellog_gid='" + msdocument_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (importcount == 0)
            {
                objResult.status = false;
                objResult.message = "No employee data has been imported so Please check the error log";
            }
            else
            {
                objResult.status = true;
                objResult.message = importcount + "  employee data has been imported";
            }
        }
        public void DaEmployeeerrorlogSummary(string user_gid, MdlEmployeelist values)
        {
            try
            {
                msSQL = " select concat(a.user_code,' / ',a.first_name,'',a.last_name) as user_name, a.remarks, " +
                        " concat(c.user_firstname,' ',c.user_lastname) as created_by, date_format(a.created_date, '%d-%b-%Y %H:%i:%s') as created_date" +
                        " from hrm_trn_temployeeuploadexcelerrorlog a " +
                        " left join adm_mst_Tuser c on a.created_by = c.user_gid where 1 = 1 "+
                          " order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<employee_list10>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employee_list10
                        {
                            //user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            remarks = dt["remarks"].ToString(),

                        });
                        values.employee_list = getModuleList;
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
        public static bool ValidatePassword(string password)
        {
            // Minimum password length
            const int MinLength = 8;

            // Check if password meets length requirement
            if (password.Length < MinLength)
                return true;

            // Check if password contains at least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return true;

            // Check if password contains at least one lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
                return true;

            // Check if password contains at least one digit
            if (!Regex.IsMatch(password, @"\d"))
                return true;

            // Check if password contains at least one special character
            if (!Regex.IsMatch(password, @"[\W_]"))
                return true;

            return false;
        }
}
}

