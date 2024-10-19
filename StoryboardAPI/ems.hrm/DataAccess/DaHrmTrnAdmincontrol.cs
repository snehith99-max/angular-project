using System;
using System.Collections.Generic;
using System.Linq;
using ems.hrm.Models;
using ems.utilities.Functions;
using System.Data;
using System.Configuration;
using System.Data.Odbc;
using System.IO;
using System.Drawing;
using System.Web;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using System.Globalization;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;
using System.Data.Common;
using System.Runtime.Remoting;
using CrystalDecisions.Shared.Json;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Security.AccessControl;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnAdmincontrol
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string lsbloodgroupname;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objtbl;
        HttpPostedFile httpPostedFile;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable dt1 = new DataTable();
        string lsconverted_date;

        DataTable DataTable2 = new DataTable();

        string company_logo_path;

        Image company_logo;

        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, mnResult6, mnResult7, mnResult8, mnResult9, mnResult10, importcount, ErrorCount, mnResult21, yearsDifference;
        string lsleavegrade_code, lsleavegrade_name, lsattendance_startdate, lsattendance_enddate, lsleavetype_gid, lsleavetype_name, lstotal_leavecount, lsavailable_leavecount;
        string lsleave_limit, lsholidaygrade_gid, lsholiday_gid, lsholiday_date, msUserGid, msEmployeeGID, msBiometricGID, msGetemployeetype, msTemporaryAddressGetGID;
        string msPermanentAddressGetGID, usercode, lsuser_gid, lsemployee_gid, lsuser_code, lscountry_gid2, lscountry_gid, msGetGIDN, lstemcountry_gid, msdocument_gid;
        string lspcountry_gid, lsentity_gid, lsdepartment_gid, lsbranch_gid, uppercasedbvalue, lsdesignation_gid, lsrole, lsrole_gid;
        string Branch, Department, Designation, JobType, ReportingTo, FirstName, LastName, EmployeeLoginID, LoginPassword, Gender, Mobile, DOB, age, Email, Qualification, Experience;
        string EmployeeAccess, JoiningDate, EmployeeType, RollType, TagId, HideAttendance, BloodGroup, Employee_Name, Shift, ShiftType_Gid, leavegrade, City, PinCode, State;
        string msGetbranchGID, msgetaddressGID, msGetdepartmentGID, msGetdesignationGID, msGetjobtypeGID, msgetUserGid, msgetEmployeeGID, msGetShiftGID, msgetassign2employee_gid, msHolidayEmpGetGID;
        string leavegrade_gid, leavegrade_code, leavegrade_name, attendance_startdate, attendance_enddate, leavetype_gid, leavetype_name, total_leavecount, available_leavecount, leave_limit;
        string holidaygrade, msGetleaveGID, WageType, wagestype_gid, workertype_gid, WorkerType, Country, msGetcountryGID, CountryGID, Address, AddressFirstLine, AddressSecondLine;
        string msEmployeedtlGid, level, entity, user_status, msGetprobationGID, photo_path, signature_path, dobValue, lsdobvalue, nowdate, JoiningDatetime, lsJoiningDate;

        public void DaEmpCountChart(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select 'Total Users' as category, count(a.user_gid) as count from adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid = a.user_gid " +
                        " union all " +
                        " select 'Inactive Users' as category, count(case when a.user_status = 'N' then 'Inactive' end) as count from adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid = a.user_gid " +
                        " union all " +
                        " select 'Active Users' as category, count(case when a.user_status = 'Y' then 'Active' end) as count from adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid = a.user_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<EmpCountChart>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new EmpCountChart
                        {
                            category = dt["category"].ToString(),
                            count = dt["count"].ToString(),
                        });
                        values.EmpCountChart = getModuleList;
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
        public void DaGetEmployeeActiveSummary(string user_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select distinct a.user_gid," +
                        " concat(b.entity_prefix,' / ',e.branch_prefix) as entity_name, " +
                        " concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name, date_format(c.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate, c.employee_gender, " +
                        " concat(j.address1,' ',j.address2,' / ',j.city,' / ',j.state,' / ',k.country_name,' / ',j.postal_code) as emp_address, c.designation_gid, " +
                        " d.designation_name, c.employee_gid, e.branch_name, c.employee_level, c.useraccess, CASE WHEN a.user_status = 'Y' THEN 'Active' WHEN a.user_status = 'N' THEN 'Inactive' END as user_status, " +
                        " c.department_gid, c.branch_gid, g.department_name FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                        " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid left join hrm_mst_tdepartment g on g.department_gid = c.department_gid left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                        " left join adm_mst_tcountry k on j.country_gid = k.country_gid left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid left join adm_mst_tentity b on b.entity_gid = c.entity_gid " +
                        " where a.user_status = 'Y' group by c.employee_gid order by c.employee_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employee_list_active>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employee_list_active
                        {
                            user_gid = dt["user_gid"].ToString(),
                            useraccess = dt["useraccess"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            //user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            emp_address = dt["emp_address"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            employee_level = dt["employee_level"].ToString(),
                        });
                        values.employee_list_active = getModuleList;
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
        public void DaGetEmployeeInActiveSummary(string user_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select distinct a.user_gid," +
                        " concat(b.entity_prefix,' / ',e.branch_prefix) as entity_name, " +
                        " concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name, date_format(c.employee_joiningdate, '%d-%m-%Y') as employee_joiningdate, c.employee_gender, " +
                        " concat(j.address1,' ',j.address2,' / ',j.city,' / ',j.state,' / ',k.country_name,' / ',j.postal_code) as emp_address, c.designation_gid, " +
                        " d.designation_name, c.employee_gid, e.branch_name, c.employee_level, c.useraccess, CASE WHEN a.user_status = 'Y' THEN 'Active' WHEN a.user_status = 'N' THEN 'Inactive' END as user_status, " +
                        " c.department_gid, c.branch_gid, g.department_name, date_format(c.exit_date,'%d-%m-%Y') as exit_date FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                        " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid left join hrm_mst_tdepartment g on g.department_gid = c.department_gid left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                        " left join adm_mst_tcountry k on j.country_gid = k.country_gid left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid left join adm_mst_tentity b on b.entity_gid = c.entity_gid " +
                        " where a.user_status = 'N' group by c.employee_gid order by c.employee_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employee_list_inactive>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employee_list_inactive
                        {
                            user_gid = dt["user_gid"].ToString(),
                            useraccess = dt["useraccess"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            //user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            emp_address = dt["emp_address"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            employee_level = dt["employee_level"].ToString(),
                            exit_date = dt["exit_date"].ToString(),
                        });
                        values.employee_list_inactive = getModuleList;
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
        public void DaGetEmployeedtlSummary(string user_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select distinct a.user_gid," +
                        " concat(b.entity_prefix,' / ',e.branch_prefix) as entity_name, " +
                        " concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name, date_format(c.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate, c.employee_gender, " +
                        " concat(j.address1,' ',j.address2,' / ',j.city,' / ',j.state,' / ',k.country_name,' / ',j.postal_code) as emp_address, c.designation_gid, " +
                        " d.designation_name, c.employee_gid, e.branch_name, c.employee_level, c.useraccess, CASE WHEN a.user_status = 'Y' THEN 'Active' WHEN a.user_status = 'N' THEN 'Inactive' END as user_status, " +
                        " c.department_gid, c.branch_gid, g.department_name, date_format(c.exit_date,'%d-%m-%Y') as exit_date FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                        " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid left join hrm_mst_tdepartment g on g.department_gid = c.department_gid left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                        " left join adm_mst_tcountry k on j.country_gid = k.country_gid left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid left join adm_mst_tentity b on b.entity_gid = c.entity_gid where a.kot_user !='Y' " +
                        " group by c.employee_gid order by c.employee_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employee_list10>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employee_list10
                        {
                            user_gid = dt["user_gid"].ToString(),
                            useraccess = dt["useraccess"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            //user_code = dt["user_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            emp_address = dt["emp_address"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            employee_level = dt["employee_level"].ToString(),
                            exit_date = dt["exit_date"].ToString(),
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetDocumentlist(string user_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select a.uploadexcellog_gid,concat(b.user_firstname, b.user_lastname) as updated_by," +
                        " a.uploaded_date,a.importcount from hrm_trn_temployeeuploadexcellog a " +
                        " left join adm_mst_tuser b on b.user_gid = a.uploaded_by";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<document_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new document_list
                        {
                            document_name = dt["uploadexcellog_gid"].ToString(),
                            updated_by = dt["updated_by"].ToString(),
                            uploaded_date = dt["uploaded_date"].ToString(),
                            importcount = dt["importcount"].ToString(),
                        });
                        values.document_list = getModuleList;
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
        public void DaGetDocumentDtllist(string document_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select first_name,last_name,user_code,remarks from hrm_trn_temployeeuploadexcelerrorlog " +
                        " where uploadexcellog_gid = '" + document_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<documentdtl_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new documentdtl_list
                        {
                            user_code = dt["user_code"].ToString(),
                            first_name = dt["first_name"].ToString(),
                            last_name = dt["last_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                        });
                        values.documentdtl_list = getModuleList;
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
        public void DaGetEmployeeerrorlogSummary(string user_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select a.user_code, concat(a.first_name,'',a.last_name) as user_name, a.remarks," +
                        " concat(c.user_firstname,' ',c.user_lastname) as created_by, a.created_date " +
                        " from hrm_trn_temployeeuploadexcelerrorlog a" +
                        " left join adm_mst_Tuser c on a.created_by = c.user_gid" +
                        " where 1 = 1 ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employee_list10>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employee_list10
                        {
                            user_code = dt["user_code"].ToString(),
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostEmployeedetails(employee_lists values, string user_gid)
        {
            try
            {
                string lsno_of_user_purchased, lsutilized;
                int lsused;

                msSQL = "select ifnull(no_of_user,0) as no_of_user from adm_mst_tcompany";
                lsno_of_user_purchased = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select ifnull(count(*),0) as count from adm_mst_tuser";
                lsutilized = objdbconn.GetExecuteScalar(msSQL);
                lsused = int.Parse(lsutilized);

                if (lsused >= int.Parse(lsno_of_user_purchased))
                {
                    values.message = "User Count has been reached purchased count,Please Contact your Admin";
                    values.status = false;
                    return;
                }

                if (values.bloodgroup != "" && values.bloodgroup != null)
                {
                    msSQL = "select bloodgroup_name from sys_mst_tbloodgroup where bloodgroup_gid='" + values.bloodgroup + "'";
                    lsbloodgroupname = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = " SELECT user_code FROM adm_mst_tuser where user_code = '" + values.user_code + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    lsuser_code = objOdbcDataReader["user_code"].ToString();
                    values.status = false;
                    return;
                }

                string uppercaseString = values.user_code.ToUpper();

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
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msUserGid + "'," +
                            " '" + values.user_code + "'," +
                            " '" + values.first_name + "'," +
                            " '" + values.last_name + "'," +
                            " '" + objcmnfunctions.ConvertToAscii(values.password) + "'," +
                            " '" + values.active_flag + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult1 == 1)
                    {
                        msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                        msBiometricGID = objcmnfunctions.GetBiometricGID();

                        msSQL = " Insert into hrm_mst_temployee " +
                                " (employee_gid , " +
                                " user_gid," +
                                " designation_gid," +
                                " employee_mobileno , " +
                                " employee_personalno , " +
                                " employee_dob , " +
                                " employee_emailid , " +
                                " employee_gender , " +
                                " department_gid," +
                                " entity_gid," +
                                " employee_photo," +
                                  " employee_sign," +
                                " employee_qualification," +
                                " age," +
                                " father_name, " +
                                " jobtype_gid, " +
                                " employeereporting_to, " +
                                " remarks , " +
                                " employee_diffabled , " +
                                " employee_companyemailid , " +
                                " employee_tagid , " +
                                " role_gid," +
                                " bloodgroup_name," +
                                " employee_joiningdate," +
                                " useraccess," +
                                " engagement_type," +
                                " attendance_flag, " +
                                " identity_no, " +
                                " branch_gid, " +
                                " biometric_id, " +
                                " created_by, " +
                                " created_date " +
                                " )values( " +
                                " '" + msEmployeeGID + "', " +
                                " '" + msUserGid + "', " +
                                " '" + values.designationname + "'," +
                                " '" + values.mobileno + "'," +
                                " '" + values.mobile + "'," +
                                " '" + values.dob + "'," +
                                " '" + values.email + "'," +
                                " '" + values.gender + "'," +
                                " '" + values.departmentname + "'," +
                                " '" + values.entityname + "'," +
                                " '/assets/media/images/Employee_defaultimage.png'," +
                                 " '/assets/media/images/DefaultSignature.png'," +
                                " '" + values.qualification + "'," +
                                " '" + values.age + "'," +
                                " '" + values.father_spouse + "'," +
                                " '" + values.jobtype + "'," +
                                " '" + values.reportingto + "'," +
                                " '" + values.remarks + "'," +
                                " '" + values.differentlyabled + "'," +
                                " '" + values.comp_email + "'," +
                                " '" + values.tagid + "'," +
                                " '" + values.role + "'," +
                                " '" + lsbloodgroupname + "'," +
                                " '" + values.employee_joiningdate + "'," +
                                " '" + values.active_flag + "'," +
                                " 'Direct'," +
                                " 'Y'," +
                                " '" + values.aadhar_no + "'," +
                                " '" + values.branchname + "'," +
                                " '" + msBiometricGID + "'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult2 == 1)
                    {
                        msGetprobationGID = objcmnfunctions.GetMasterGID("HRPB");
                        msSQL = " insert into hrm_trn_probationperiod(" +
                                " probation_gid," +
                                " employee_gid," +
                                " user_gid," +
                                " employee_joiningdate, " +
                                " probationary_until, " +
                                " branch_gid, " +
                                " designation_gid, " +
                                " department_gid, " +
                                " jobtype_gid, " +
                                " probation_status, " +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetprobationGID + "'," +
                                " '" + msEmployeeGID + "'," +
                                " '" + user_gid + "'," +
                                " '" + values.employee_joiningdate + "'," +
                                " '" + values.probationenddate + "'," +
                                " '" + values.branchname + "'," +
                                " '" + values.designationname + "'," +
                                " '" + values.departmentname + "'," +
                                " 'Probationary'," +
                                " 'Yes'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult21 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult2 == 1)
                    {
                        msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");
                        msSQL = " insert into hrm_trn_temployeetypedtl(" +
                                " employeetypedtl_gid," +
                                " employee_gid," +
                                " workertype_gid," +
                                " systemtype_gid, " +
                                " branch_gid, " +
                                " department_gid, " +
                                " employeetype_name, " +
                                " designation_gid, " +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetemployeetype + "'," +
                                " '" + msEmployeeGID + "'," +
                                " '" + values.workertype + "'," +
                                " 'Audit'," +
                                " '" + values.branchname + "'," +
                                " '" + values.departmentname + "'," +
                                " 'Roll'," +
                                " '" + values.designationname + "'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult3 == 1)
                    {
                        string msGetGID_shift = objcmnfunctions.GetMasterGID("HESC");
                        msSQL = " insert into hrm_trn_temployee2shifttype( " +
                                " employee2shifttype_gid, " +
                                " employee_gid,  " +
                                " shifttype_gid, " +
                                " shifteffective_date ," +
                                " shiftactive_flag ," +
                                " created_by," +
                                " created_date) " +
                                " values( " +
                                " '" + msGetGID_shift + "'," +
                                " '" + msEmployeeGID + "'," +
                                " '" + values.shift + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " 'Y'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult4 == 1)
                        {
                            msSQL = " select * from hrm_trn_temployee2shifttypedtl " +
                                    " where employee_gid='" + msEmployeeGID + "' " +
                                    " and shifttype_gid='" + values.shift + "' ";
                            objtbl = objdbconn.GetDataTable(msSQL);

                            if (objtbl.Rows.Count > 0)
                            {
                                msSQL = " update hrm_trn_temployee2shifttypedtl set shift_status='Y' where employee_gid='" + msEmployeeGID + "' " +
                                        " and shifttype_gid='" + values.shift + "' ";
                                mnResult5 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else
                            {
                                msSQL = " select a.shifttypedtl_gid,a.shifttype_gid,a.shifttypedtl_name,a.shift_fromhours,a.shift_tohours, " +
                                        " a.lunchout_hours,a.lunchout_minutes,a.lunchin_hours,a.lunchin_minutes, " +
                                        " a.shift_fromminutes, a.shift_tominutes from hrm_mst_tshifttypedtl a " +
                                        " inner join hrm_mst_tshifttype b on a.shifttype_gid=b.shifttype_gid " +
                                        " where b.shifttype_gid='" + values.shift + "' ";
                                objtbl = objdbconn.GetDataTable(msSQL);

                                if (objtbl.Rows.Count > 0)
                                {
                                    foreach (DataRow dt in objtbl.Rows)
                                    {
                                        msGetGIDN = objcmnfunctions.GetMasterGID("HEST");
                                        msSQL = " insert into hrm_trn_temployee2shifttypedtl (" +
                                                " employee2shifttypedtl_gid," +
                                                " employee2shifttype_gid," +
                                                " shifttype_gid," +
                                                " shifttypedtl_gid," +
                                                " employee2shifttype_name," +
                                                " shift_fromhours," +
                                                " shift_fromminutes," +
                                                " shift_tohours," +
                                                " shift_tominutes," +
                                                " lunchout_hours," +
                                                " lunchout_minutes," +
                                                " lunchin_hours," +
                                                " lunchin_minutes," +
                                                " employee_gid," +
                                                " created_by," +
                                                " shift_status, " +
                                                " created_date)" +
                                                " values (" +
                                                " '" + msGetGIDN + "', " +
                                                " '" + msGetGID_shift + "', " +
                                                " '" + values.shift + "', " +
                                                " '" + dt["shifttypedtl_gid"].ToString() + "', " +
                                                " '" + dt["shifttypedtl_name"].ToString() + "'," +
                                                " '" + dt["shift_fromhours"].ToString() + "'," +
                                                " '" + dt["shift_fromminutes"].ToString() + "'," +
                                                " '" + dt["shift_tohours"].ToString() + "'," +
                                                " '" + dt["shift_tominutes"].ToString() + "'," +
                                                " '" + dt["lunchout_hours"].ToString() + "'," +
                                                " '" + dt["lunchout_minutes"].ToString() + "'," +
                                                " '" + dt["lunchin_hours"].ToString() + "'," +
                                                " '" + dt["lunchin_minutes"].ToString() + "'," +
                                                " '" + msEmployeeGID + "'," +
                                                "'" + user_gid + "'," +
                                                "'Y'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult6 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                }
                            }
                        }
                    }

                    msSQL = " select  c.leavetype_name,b.leavetype_gid,a.leavegrade_gid ,a.leavegrade_code, a.leavegrade_name ,c.leavetype_name ,a.attendance_startdate, " +
                            " a.attendance_enddate, format(sum(b.total_leavecount),2)as total_leavecount , format(sum(b.available_leavecount),2)as available_leavecount, " +
                            " format(sum(b.leave_limit),2)as leave_limit from hrm_mst_tleavegrade a " +
                            " left join hrm_mst_tleavegradedtl b on a.leavegrade_gid=b.leavegrade_gid  " +
                            " left join hrm_mst_tleavetype c on b.leavetype_gid=c.leavetype_gid where a.leavegrade_gid='" + values.leavegrade + "' group by a.leavegrade_gid ";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows == true)
                    {
                        lsleavegrade_code = objOdbcDataReader["leavegrade_code"].ToString();
                        lsleavegrade_name = objOdbcDataReader["leavegrade_name"].ToString();
                        lsattendance_startdate = objOdbcDataReader["attendance_startdate"].ToString();
                        lsattendance_enddate = objOdbcDataReader["attendance_enddate"].ToString();
                        lsleavetype_gid = objOdbcDataReader["leavetype_gid"].ToString();
                        lsleavetype_name = objOdbcDataReader["leavetype_name"].ToString();
                        lstotal_leavecount = objOdbcDataReader["total_leavecount"].ToString();
                        lsavailable_leavecount = objOdbcDataReader["available_leavecount"].ToString();
                        lsleave_limit = objOdbcDataReader["leave_limit"].ToString();

                        string msgetassign2employee_gid = objcmnfunctions.GetMasterGID("LE2G");

                        msSQL = " insert into hrm_trn_tleavegrade2employee ( " +
                                " leavegrade2employee_gid," +
                                " branch_gid ," +
                                " employee_gid," +
                                " employee_name," +
                                " leavegrade_gid," +
                                " leavegrade_code," +
                                " leavegrade_name, " +
                                " attendance_startdate, " +
                                " attendance_enddate, " +
                                " total_leavecount, " +
                                " available_leavecount, " +
                                " leave_limit " +
                                " ) Values ( " +
                                " '" + msgetassign2employee_gid + "', " +
                                " '" + values.branchname + "', " +
                                " '" + msEmployeeGID + "', " +
                                " '" + values.first_name + "', " +
                                " '" + values.leavegrade + "', " +
                                " '" + lsleavegrade_code + "'," +
                                " '" + lsleavegrade_name + "'," +
                                " '" + Convert.ToDateTime(lsattendance_startdate).ToString("yyyy-MM-dd") + "'," +
                                " '" + Convert.ToDateTime(lsattendance_enddate).ToString("yyyy-MM-dd") + "'," +
                                " '" + lstotal_leavecount + "'," +
                                " '" + lsavailable_leavecount + "'," +
                                " '" + lsleave_limit + "')";
                        mnResult7 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (values.holidaygrade != "" || values.holidaygrade != null)
                    {
                        msSQL = "select a.holidaygrade_gid,b.holiday_gid,b.holiday_date from hrm_mst_tholidaygrade a " +
                               " left join hrm_mst_tholiday2grade b on b.holidaygrade_gid=a.holidaygrade_gid " +
                               " where a.holidaygrade_gid='" + values.holidaygrade + "' ";
                        DataTable objtblgrade = objdbconn.GetDataTable(msSQL);

                        if (objtblgrade.Rows.Count > 0)
                        {
                            foreach (DataRow dt in objtblgrade.Rows)
                            {
                                lsholidaygrade_gid = dt["holidaygrade_gid"].ToString();
                                lsholiday_gid = dt["holiday_gid"].ToString();
                                lsholiday_date = dt["holiday_date"].ToString();

                                string msGetGID = objcmnfunctions.GetMasterGID("HYTE");
                                msSQL = " insert into hrm_mst_tholiday2employee ( " +
                                        " holiday2employee, " +
                                        " holidaygrade_gid, " +
                                        " holiday_gid, " +
                                        " employee_gid, " +
                                        " holiday_date, " +
                                        " created_by, " +
                                        " created_date ) " +
                                        " values ( " +
                                        " '" + msGetGID + "', " +
                                        " '" + lsholidaygrade_gid + "', " +
                                        " '" + lsholiday_gid + "'," +
                                        " '" + msEmployeeGID + "', " +
                                        " '" + Convert.ToDateTime(lsholiday_date).ToString("yyyy-MM-dd") + "', " +
                                        " '" + user_gid + "', " +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                                mnResult8 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }

                    msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

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
                            " '" + values.permanent_address1 + "'," +
                            " '" + values.permanent_address2 + "'," +
                            " '" + values.permanent_city + "'," +
                            " '" + values.permanent_state + "'," +
                            " 'Permanent'," +
                            " '" + values.permanent_postal + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult9 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult9 == 1)
                    {
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
                                " '" + msTemporaryAddressGetGID + "'," +
                                " '" + msEmployeeGID + "'," +
                                " '" + values.countryname + "'," +
                                " '" + values.temporary_address1 + "'," +
                                " '" + values.temporary_address2 + "'," +
                                " '" + values.temporary_city + "'," +
                                " '" + values.temporary_state + "'," +
                                " 'Temporary'," +
                                " '" + values.temporary_postal + "'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult10 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult2 != 0)
                    {
                        values.status = true;
                        values.message = "Employee Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Employee !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Employee User Code Already Exist";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetentitydropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select entity_gid, entity_name " +
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetbloodgroupdropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " SELECT bloodgroup_name,bloodgroup_gid FROM sys_mst_tbloodgroup";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbloodgroupdropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getbloodgroupdropdown
                        {
                            bloodgroup_gid = dt["bloodgroup_gid"].ToString(),
                            bloodgroup_name = dt["bloodgroup_name"].ToString(),
                        });
                        values.Getbloodgroupdropdown = getModuleList;
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
        public void DaGetdesignationdropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select designation_name,designation_gid from adm_mst_tdesignation ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getdesignationdropdown1>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdesignationdropdown1
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetcountrydropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select country_name,country_gid from adm_mst_tcountry ";
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetcountry2dropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select country_name as country_names,country_gid as country_gids from adm_mst_tcountry ";
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetbranchdropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select branch_name, branch_gid from hrm_mst_tbranch ";
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetworkertypedropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select workertype_gid, workertype_name from hrm_mst_tworkertype ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getworkertypedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getworkertypedropdown
                        {
                            workertype_name = dt["workertype_name"].ToString(),
                            workertype_gid = dt["workertype_gid"].ToString(),
                        });
                        values.Getworkertypedropdown = getModuleList;
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
        public void DaGetroledropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select role_gid, role_name from hrm_mst_trole ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getroledropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getroledropdown
                        {
                            role_name = dt["role_name"].ToString(),
                            role_gid = dt["role_gid"].ToString(),
                        });
                        values.Getroledropdown = getModuleList;
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
        public void DaGetholidaygradedropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select holidaygrade_gid,holidaygrade_name from hrm_mst_tholidaygrade ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getholidaygradedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getholidaygradedropdown
                        {
                            holidaygrade_name = dt["holidaygrade_name"].ToString(),
                            holidaygrade_gid = dt["holidaygrade_gid"].ToString(),
                        });
                        values.Getholidaygradedropdown = getModuleList;
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
        public void DaGetjobtypedropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select jobtype_gid, jobtype_name from hrm_mst_tjobtype ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getjobtypenamedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getjobtypenamedropdown
                        {
                            jobtype_name = dt["jobtype_name"].ToString(),
                            jobtype_gid = dt["jobtype_gid"].ToString(),
                        });
                        values.Getjobtypenamedropdown = getModuleList;
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
        public void DaGetshifttypedropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select shifttype_name, shifttype_gid from hrm_mst_tshifttype ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getshifttypenamedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getshifttypenamedropdown
                        {
                            shifttype_name = dt["shifttype_name"].ToString(),
                            shifttype_gid = dt["shifttype_gid"].ToString(),
                        });
                        values.Getshifttypenamedropdown = getModuleList;
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
        public void DaGetleavegradedropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select leavegrade_name,leavegrade_gid from hrm_mst_tleavegrade ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getleavegradenamedropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getleavegradenamedropdown
                        {
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),
                        });
                        values.Getleavegradenamedropdown = getModuleList;
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
        public void DaGetdepartmentdropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " Select department_name,department_gid from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getdepartmentdropdown1>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdepartmentdropdown1
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetreportingtodropdown(MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select distinct  a.employee_gid, concat(b.user_firstname,' ',b.user_lastname) as employee_name from hrm_mst_temployee a " +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join adm_mst_tmodule2employee c on c.employee_gid = a.employee_gid where c.hierarchy_level > 0 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getreportingtodropdown>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getreportingtodropdown
                        {
                            employee_name = dt["employee_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                        });
                        values.Getreportingtodropdown = getModuleList;
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
        public void DaEmployeeProfileUpload(HttpRequest httpRequest, result objResult, string user_gid)
        {
            HttpFileCollection httpFileCollection;
            string lscompany_code = string.Empty;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);

            string entity = httpRequest.Form[0];
            string branch = httpRequest.Form[1];
            string department = httpRequest.Form[2];
            string designation = httpRequest.Form[3];
            string active_flag = httpRequest.Form[4];
            string user_code = httpRequest.Form[5];
            string password = httpRequest.Form[6];
            string first_name = httpRequest.Form[8];
            string last_name = httpRequest.Form[9];
            string gender = httpRequest.Form[10];
            string email = httpRequest.Form[11];
            string mobile = httpRequest.Form[12];
            string permanent_address1 = httpRequest.Form[14];
            string permanent_address2 = httpRequest.Form[14];
            string country = httpRequest.Form[15];
            string permanent_city = httpRequest.Form[16];
            string permanent_state = httpRequest.Form[17];
            string permanent_postal = httpRequest.Form[18];
            string temporary_address1 = httpRequest.Form[19];
            string temporary_address2 = httpRequest.Form[20];
            string countryname = httpRequest.Form[21];
            string temporary_city = httpRequest.Form[22];
            string temporary_state = httpRequest.Form[23];
            string temporary_postal = httpRequest.Form[24];
            string workertype = httpRequest.Form[25];
            string role = httpRequest.Form[26];
            string jobtype = httpRequest.Form[27];
            string employee_joiningdate = httpRequest.Form[28]; 
            string reportingto = httpRequest.Form[29];
            string shift = httpRequest.Form[30];
            string leavegrade = httpRequest.Form[31];
            string tagid = httpRequest.Form[32];
            string holidaygrade = httpRequest.Form[33];
            string dob = httpRequest.Form[34];
            string age = httpRequest.Form[35];
            string aadhar_no = httpRequest.Form[36];
            string father_spouse = httpRequest.Form[37];
            string mobileno = httpRequest.Form[38];
            string comp_email = httpRequest.Form[39];
            string bloodgroup = httpRequest.Form[40];
            string qualification = httpRequest.Form[41];
            string remarks = httpRequest.Form[42];
            string differentlyabled = httpRequest.Form[43];
            string probationenddate = httpRequest.Form[44];

            MemoryStream ms = new MemoryStream();

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


                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "EmployeeDetailfiles/Post/HRM/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        {
                            if ((!System.IO.Directory.Exists(lspath)))
                                System.IO.Directory.CreateDirectory(lspath);
                        }
                        string lsfile_gid1 = Path.Combine(lspath, lsfile_gid);
                        using (Stream fileStream = httpPostedFile.InputStream)
                        {
                            using (FileStream fs = System.IO.File.Create(lsfile_gid1))
                            {
                                fileStream.CopyTo(fs);
                            }
                        }

                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "EmployeeDetailfiles/Post/HRM/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        if (photo_path == null)
                        {
                            photo_path = lspath + msdocument_gid + FileExtension;
                        }
                        else
                        {
                            signature_path = lspath + msdocument_gid + FileExtension;
                        }

                    }

                    ms.Close();

                    string lsno_of_user_purchased, lsutilized;
                    int lsused;

                    msSQL = "select ifnull(no_of_user,0) as no_of_user from adm_mst_tcompany";
                    lsno_of_user_purchased = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select ifnull(count(*),0) as count from adm_mst_tuser";
                    lsutilized = objdbconn.GetExecuteScalar(msSQL);
                    lsused = int.Parse(lsutilized);

                    if (lsused >= int.Parse(lsno_of_user_purchased))
                    {
                        objResult.message = "User Count has been reached purchased count,Please Contact your Admin";
                        objResult.status = false;
                        return;
                    }
                    if (bloodgroup != "" && bloodgroup != null)
                    {
                        msSQL = "select bloodgroup_name from sys_mst_tbloodgroup where bloodgroup_gid='" + bloodgroup + "'";
                        lsbloodgroupname = objdbconn.GetExecuteScalar(msSQL);
                    }
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
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msUserGid + "'," +
                                " '" + user_code + "'," +
                                " '" + first_name + "'," +
                                " '" + last_name + "'," +
                                " '" + objcmnfunctions.ConvertToAscii(password) + "'," +
                                " '" + active_flag + "'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult1 == 1)
                        {
                            msEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                            msBiometricGID = objcmnfunctions.GetBiometricGID();

                            msSQL = " Insert into hrm_mst_temployee " +
                                    " (employee_gid , " +
                                    " user_gid," +
                                    " designation_gid," +
                                    " employee_mobileno , " +
                                    " employee_personalno , " +
                                    " employee_dob , " +
                                    " employee_emailid , " +
                                    " employee_gender , " +
                                    " department_gid," +
                                    " entity_gid," +
                                    " employee_photo," +
                                    " employee_sign," +
                                    " employee_qualification," +
                                    " age," +
                                    " father_name, " +
                                    " jobtype_gid, " +
                                    " employeereporting_to, " +
                                    " remarks , " +
                                    " employee_diffabled , " +
                                    " employee_companyemailid , " +
                                    " employee_tagid , " +
                                    " role_gid," +
                                    " bloodgroup_name," +
                                    " employee_joiningdate," +
                                    " useraccess," +
                                    " engagement_type," +
                                    " attendance_flag, " +
                                    " identity_no, " +
                                    " branch_gid, " +
                                    " biometric_id, " +
                                    " created_by, " +
                                    " created_date " +
                                    " )values( " +
                                    " '" + msEmployeeGID + "', " +
                                    " '" + msUserGid + "', " +
                                    " '" + designation + "'," +
                                    " '" + mobile + "'," +
                                    " '" + mobileno + "'," +
                                    " '" + dob + "'," +
                                    " '" + email + "'," +
                                    " '" + gender + "'," +
                                    " '" + department + "'," +
                                    " '" + entity + "'," +
                                    " '" + photo_path + "'," +
                                    " '" + signature_path + "'," +
                                    " '" + qualification + "'," +
                                    " '" + age + "'," +
                                    " '" + father_spouse + "'," +
                                    " '" + jobtype + "'," +
                                    " '" + reportingto + "'," +
                                    " '" + remarks + "'," +
                                    " '" + differentlyabled + "'," +
                                    " '" + comp_email + "'," +
                                    " '" + tagid + "'," +
                                    " '" + role + "'," +
                                    " '" + lsbloodgroupname + "'," +
                                    " '" + employee_joiningdate + "'," +
                                    " '" + active_flag + "'," +
                                    " 'Direct'," +
                                    " 'Y'," +
                                    " '" + aadhar_no + "'," +
                                    " '" + branch + "'," +
                                    " '" + msBiometricGID + "'," +
                                    " '" + user_gid + "'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult2 == 1)
                            {
                                msGetprobationGID = objcmnfunctions.GetMasterGID("HRPB");
                                msSQL = " insert into hrm_trn_probationperiod(" +
                                        " probation_gid," +
                                        " employee_gid," +
                                        " user_gid," +
                                        " employee_joiningdate, " +
                                        " probationary_until, " +
                                        " branch_gid, " +
                                        " designation_gid, " +
                                        " department_gid, " +
                                        " jobtype_gid, " +
                                        " probation_status, " +
                                        " created_by, " +
                                        " created_date)" +
                                        " values(" +
                                        " '" + msGetprobationGID + "'," +
                                        " '" + msEmployeeGID + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + employee_joiningdate + "'," +
                                        " '" + probationenddate + "'," +
                                        " '" + branch + "'," +
                                        " '" + designation + "'," +
                                        " '" + department + "'," +
                                        " 'Probationary'," +
                                        " 'Yes'," +
                                        " '" + user_gid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult21 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            if (mnResult2 == 1 && mnResult21 == 1)
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
                                         " '" + workertype + "'," +
                                        " 'Audit'," +
                                        " '" + branch + "'," +
                                        " 'wg001'," +
                                        " '" + department + "'," +
                                        " 'Roll'," +
                                        " '" + designation + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult3 == 1)
                                {
                                    string msGetGID_shift = objcmnfunctions.GetMasterGID("HESC");

                                    msSQL = " insert into hrm_trn_temployee2shifttype( " +
                                            " employee2shifttype_gid, " +
                                            " employee_gid,  " +
                                            " shifttype_gid, " +
                                            " shifteffective_date ," +
                                            " shiftactive_flag ," +
                                            " created_by," +
                                            " created_date) " +
                                            " values( " +
                                            " '" + msGetGID_shift + "'," +
                                            " '" + msEmployeeGID + "'," +
                                            " '" + shift + "'," +
                                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                            " 'Y'," +
                                            " '" + user_gid + "'," +
                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult4 == 1)
                                    {
                                        msSQL = " select * from hrm_trn_temployee2shifttypedtl " +
                                                " where employee_gid='" + msEmployeeGID + "' " +
                                                " and shifttype_gid='" + shift + "' ";
                                        objtbl = objdbconn.GetDataTable(msSQL);

                                        if (objtbl.Rows.Count > 0)
                                        {
                                            msSQL = " update hrm_trn_temployee2shifttypedtl set shift_status='Y' where employee_gid='" + msEmployeeGID + "' " +
                                                    " and shifttype_gid='" + shift + "' ";
                                            mnResult5 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                        else
                                        {
                                            msSQL = " select a.shifttypedtl_gid,a.shifttype_gid,a.shifttypedtl_name,a.shift_fromhours,a.shift_tohours, " +
                                                    " a.lunchout_hours,a.lunchout_minutes,a.lunchin_hours,a.lunchin_minutes, " +
                                                    " a.shift_fromminutes, a.shift_tominutes from hrm_mst_tshifttypedtl a " +
                                                    " inner join hrm_mst_tshifttype b on a.shifttype_gid=b.shifttype_gid " +
                                                    " where b.shifttype_gid='" + shift + "' ";
                                            objtbl = objdbconn.GetDataTable(msSQL);

                                            if (objtbl.Rows.Count > 0)
                                            {
                                                foreach (DataRow dt in objtbl.Rows)
                                                {
                                                    msGetGIDN = objcmnfunctions.GetMasterGID("HEST");

                                                    msSQL = " insert into hrm_trn_temployee2shifttypedtl (" +
                                                            " employee2shifttypedtl_gid," +
                                                            " employee2shifttype_gid," +
                                                            " shifttype_gid," +
                                                            " shifttypedtl_gid," +
                                                            " employee2shifttype_name," +
                                                            " shift_fromhours," +
                                                            " shift_fromminutes," +
                                                            " shift_tohours," +
                                                            " shift_tominutes," +
                                                            " lunchout_hours," +
                                                            " lunchout_minutes," +
                                                            " lunchin_hours," +
                                                            " lunchin_minutes," +
                                                            " employee_gid," +
                                                            " created_by," +
                                                            " shift_status, " +
                                                            " created_date)" +
                                                            " values (" +
                                                            " '" + msGetGIDN + "', " +
                                                            " '" + msGetGID_shift + "', " +
                                                            " '" + shift + "', " +
                                                            " '" + dt["shifttypedtl_gid"].ToString() + "', " +
                                                            " '" + dt["shifttypedtl_name"].ToString() + "'," +
                                                            " '" + dt["shift_fromhours"].ToString() + "'," +
                                                            " '" + dt["shift_fromminutes"].ToString() + "'," +
                                                            " '" + dt["shift_tohours"].ToString() + "'," +
                                                            " '" + dt["shift_tominutes"].ToString() + "'," +
                                                            " '" + dt["lunchout_hours"].ToString() + "'," +
                                                            " '" + dt["lunchout_minutes"].ToString() + "'," +
                                                            " '" + dt["lunchin_hours"].ToString() + "'," +
                                                            " '" + dt["lunchin_minutes"].ToString() + "'," +
                                                            " '" + msEmployeeGID + "'," +
                                                            " '" + user_gid + "'," +
                                                            " 'Y'," +
                                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                    mnResult6 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }
                                            }
                                        }
                                    }

                                    msSQL = " select  c.leavetype_name,b.leavetype_gid,a.leavegrade_gid ,a.leavegrade_code, a.leavegrade_name ,c.leavetype_name ,a.attendance_startdate,a.attendance_enddate, " +
                                            " format(sum(b.total_leavecount),2)as total_leavecount , format(sum(b.available_leavecount),2)as available_leavecount, " +
                                            " format(sum(b.leave_limit),2)as leave_limit from hrm_mst_tleavegrade a " +
                                            " left join hrm_mst_tleavegradedtl b on a.leavegrade_gid=b.leavegrade_gid  " +
                                            " left join hrm_mst_tleavetype c on b.leavetype_gid=c.leavetype_gid where a.leavegrade_gid='" + leavegrade + "' group by a.leavegrade_gid ";

                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        lsleavegrade_code = objOdbcDataReader["leavegrade_code"].ToString();
                                        lsleavegrade_name = objOdbcDataReader["leavegrade_name"].ToString();
                                        lsattendance_startdate = objOdbcDataReader["attendance_startdate"].ToString();
                                        lsattendance_enddate = objOdbcDataReader["attendance_enddate"].ToString();
                                        lsleavetype_gid = objOdbcDataReader["leavetype_gid"].ToString();
                                        lsleavetype_name = objOdbcDataReader["leavetype_name"].ToString();
                                        lstotal_leavecount = objOdbcDataReader["total_leavecount"].ToString();
                                        lsavailable_leavecount = objOdbcDataReader["available_leavecount"].ToString();
                                        lsleave_limit = objOdbcDataReader["leave_limit"].ToString();
                                        string msgetassign2employee_gid = objcmnfunctions.GetMasterGID("LE2G");

                                        msSQL = " insert into hrm_trn_tleavegrade2employee ( " +
                                                " leavegrade2employee_gid," +
                                                " branch_gid ," +
                                                " employee_gid," +
                                                " employee_name," +
                                                " leavegrade_gid," +
                                                " leavegrade_code," +
                                                " leavegrade_name, " +
                                                " attendance_startdate, " +
                                                " attendance_enddate, " +
                                                " total_leavecount, " +
                                                " available_leavecount, " +
                                                " leave_limit " +
                                                " ) Values ( " +
                                                " '" + msgetassign2employee_gid + "', " +
                                                " '" + branch + "', " +
                                                " '" + msEmployeeGID + "', " +
                                                " '" + first_name + "', " +
                                                " '" + leavegrade + "', " +
                                                " '" + lsleavegrade_code + "'," +
                                                " '" + lsleavegrade_name + "'," +
                                                " '" + Convert.ToDateTime(lsattendance_startdate).ToString("yyyy-MM-dd") + "'," +
                                                " '" + Convert.ToDateTime(lsattendance_enddate).ToString("yyyy-MM-dd") + "'," +
                                                " '" + lstotal_leavecount + "'," +
                                                " '" + lsavailable_leavecount + "'," +
                                                " '" + lsleave_limit + "')";
                                        mnResult7 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                    if (holidaygrade != "" || holidaygrade != null)
                                    {
                                        msSQL = "select a.holidaygrade_gid,b.holiday_gid,b.holiday_date from hrm_mst_tholidaygrade a " +
                                               " left join hrm_mst_tholiday2grade b on b.holidaygrade_gid=a.holidaygrade_gid " +
                                               " where a.holidaygrade_gid='" + holidaygrade + "' ";
                                        DataTable objtblgrade = objdbconn.GetDataTable(msSQL);

                                        if (objtblgrade.Rows.Count > 0)
                                        {
                                            foreach (DataRow dt in objtblgrade.Rows)
                                            {
                                                lsholidaygrade_gid = dt["holidaygrade_gid"].ToString();
                                                lsholiday_gid = dt["holiday_gid"].ToString();
                                                lsholiday_date = dt["holiday_date"].ToString();

                                                string msGetGID = objcmnfunctions.GetMasterGID("HYTE");
                                                msSQL = " insert into hrm_mst_tholiday2employee ( " +
                                                           " holiday2employee, " +
                                                           " holidaygrade_gid, " +
                                                           " holiday_gid, " +
                                                           " employee_gid, " +
                                                           " holiday_date, " +
                                                           " created_by, " +
                                                           " created_date ) " +
                                                           " values ( " +
                                                           "'" + msGetGID + "', " +
                                                           "'" + lsholidaygrade_gid + "', " +
                                                           "'" + lsholiday_gid + "'," +
                                                           "'" + msEmployeeGID + "', " +
                                                           "'" + Convert.ToDateTime(lsholiday_date).ToString("yyyy-MM-dd") + "', " +
                                                           "'" + user_gid + "', " +
                                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                                                mnResult8 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }
                                        }
                                    }

                                    msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

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
                                            " 'Permanent'," +
                                            " '" + permanent_postal + "'," +
                                            " '" + user_gid + "'," +
                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult9 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult9 == 1)
                                    {
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
                                                " '" + msTemporaryAddressGetGID + "'," +
                                                " '" + msEmployeeGID + "'," +
                                                " '" + countryname + "'," +
                                                " '" + temporary_address1 + "'," +
                                                " '" + temporary_address2 + "'," +
                                                " '" + temporary_city + "'," +
                                                " '" + temporary_state + "'," +
                                                " 'Temporary'," +
                                                " '" + temporary_postal + "'," +
                                                " '" + user_gid + "'," +
                                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult10 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                }
                            }

                            if (mnResult2 != 0)
                            {
                                objResult.status = true;
                                objResult.message = "Employee Added Successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Adding Employee !!";
                            }
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Employee User Code Already Exist !!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaupdateEmployeeProfileUpload(HttpRequest httpRequest, result objResult, string user_gid)
        {
            HttpFileCollection httpFileCollection;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string lspath1;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);

            string employee_gid = httpRequest.Form[0];

            MemoryStream ms = new MemoryStream();

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


                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "EmployeeDetailfiles/Post/HRM/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        {
                            if ((!System.IO.Directory.Exists(lspath)))
                                System.IO.Directory.CreateDirectory(lspath);
                        }
                        string lsfile_gid1 = Path.Combine(lspath, lsfile_gid);
                        using (Stream fileStream = httpPostedFile.InputStream)
                        {
                            using (FileStream fs = System.IO.File.Create(lsfile_gid1))
                            {
                                fileStream.CopyTo(fs);
                            }
                        }

                        lspath = "erp_documents" + "/" + lscompany_code + "/" + "EmployeeDetailfiles/Post/HRM/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        if (photo_path == null)
                        {
                            photo_path = lspath + msdocument_gid + FileExtension;
                        }
                        else
                        {
                            signature_path = lspath + msdocument_gid + FileExtension;
                        }


                    }
                    ms.Close();

                }
                if (photo_path != null)
                {
                    msSQL = "select employee_photo from hrm_mst_temployee where employee_gid ='" + employee_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        msSQL = "update hrm_mst_temployee set employee_photo='NULL' where employee_gid = '" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update hrm_mst_temployee set " +
                                "employee_photo = '" + photo_path + "'" +
                                " where employee_gid = '" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                    else
                    {
                        msSQL = "update hrm_mst_temployee set " +
                               "employee_photo = '" + photo_path + "'" +
                               " where employee_gid = '" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }

                if (signature_path != null)
                {
                    msSQL = "select employee_sign from hrm_mst_temployee where employee_gid ='" + employee_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        msSQL = "update hrm_mst_temployee set employee_sign='NULL' where employee_gid = '" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update hrm_mst_temployee set " +
                                "employee_sign = '" + signature_path + "'" +
                                " where employee_gid = '" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                    else
                    {
                        msSQL = "update hrm_mst_temployee set " +
                               "employee_sign = '" + signature_path + "'" +
                               " where employee_gid = '" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    objResult.status = true;
                    objResult.message = "employee document updated successfully";
                }
                objResult.status = true;
                objResult.message = "employee document updated successfully";

            }
            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEditEmployeeSummary(string employee_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {

                msSQL = " select a.employee_gid,a.employee_gender, u.shifttype_name, x.holidaygrade_name, a.biometric_id,a.employee_diffabled,a.age,a.role_gid,a.employee_personalno,z.entity_name," +
                           " a.employeereporting_to, a.employeereporting_to,a.jobtype_gid,j.workertype_gid, a.identity_no,date_format(a.employee_dob,'%d-%m-%Y') as employee_dob,a.employee_sign,a.bloodgroup_name,t.employeepreviouscompany_name, " +
                           " a.passport_no,q.role_name,b.user_password,a.fin_no,a.workpermit_no,date_format(a.passport_expiredate,'%d-%m-%Y') as passport_expiredate, a.branch_gid,a.father_name, " +
                           " date_format(a.workpermit_expiredate,'%d-%m-%Y') as workpermit_expiredate,a.employeepreviouscompany_gid,l.perhour_rate,l.permonth_rate," +
                           " date_format(a.finno_expiredate,'%d-%m-%Y')as finno_expiredate, a.department_gid, m.daysalary_rate as perdayrate," +
                           " concat(b.user_firstname, ' ', b.user_lastname)as employee_name,a.employee_photo,b.user_gid,date_format(a.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate,b.regional_username, " +
                           " h.employeetype_name,i.wagetype_name,j.workertype_name,k.roll_name,a.employee_tagid,h.wagestype_gid,h.employeetype_name,  " +
                           " case when a.employee_hideattendance='Y' then 'Yes' else 'No' end as emp_hideattendance, " +
                            " case when b.user_status='Y' then 'Yes' else 'No' end as emp_active_flag, " +
                           " case when a.employee_diffabled='Y' then 'Yes' else 'No' end as employee_diffabled , " +
                           " a.employee_emailid,a.employee_companyemailid,a.employee_mobileno,a.employee_personalno,a.employee_qualification,a.employee_sign,a.remarks, " +
                           " a.employee_experience,a.employee_experiencedtl, a.employeereporting_to , a.employment_type ," +
                           " b.user_code,b.user_firstname,b.user_lastname,y.leavegrade_name, h.workertype_gid, a.designation_gid, b.user_status,b.usergroup_gid,c.usergroup_code, " +
                           " d.branch_name,  e.department_name,f.designation_name, g.jobtype_name,s.section_name,a.section_gid, " +
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
                           " (select address1 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_address1, " +
                           " (select i.user_firstname from adm_mst_tuser i ,  hrm_mst_temployee j where i.user_gid = j.user_gid " +
                           " and a.employeereporting_to = j.employee_gid)  as approveby_name,a.rolltype_gid, " +
                           " a.nationality,a.nric_no,a.skill_set " +
                           " FROM hrm_mst_temployee a  LEFT JOIN adm_mst_tuser b ON a.user_gid = b.user_gid " +
                           " left join pay_trn_temployee2wage m on a.employee_gid = m.employee_gid " +
                           " LEFT JOIN pay_mst_tdaysalarymaster l on m.daysalary_gid = l.daysalary_gid" +
                           " LEFT JOIN adm_mst_tusergroup c ON b.usergroup_gid = c.usergroup_gid " +
                           " LEFT JOIN hrm_mst_tbranch d ON a.branch_gid = d.branch_gid " +
                           " LEFT JOIN hrm_mst_tdepartment e ON a.department_gid = e.department_gid  " +
                           " LEFT JOIN adm_mst_tdesignation f ON a.designation_gid = f.designation_gid " +
                           " LEFT JOIN hrm_mst_tjobtype g ON a.jobtype_gid = g.jobtype_gid " +
                           " LEFT JOIN hrm_trn_temployeetypedtl h ON a.employee_gid = h.employee_gid" +
                           " LEFT JOIN hrm_mst_twagestype i ON h.wagestype_gid = i.wagestype_gid" +
                           " LEFT JOIN hrm_mst_tworkertype j ON h.workertype_gid = j.workertype_gid" +
                           " LEFT JOIN hrm_mst_temployeerolltype k ON h.systemtype_gid = k.systemtype_name" +
                           " LEFT JOIN hrm_mst_tsection s ON a.section_gid= s.section_gid " +
                           " left join hrm_mst_tholiday2employee r on r.employee_gid = a.employee_gid " +
                           " left join adm_mst_tentity z on z.entity_gid=a.entity_gid " +
                           " left join hrm_mst_trole q on q.role_gid = a.role_gid " +
                           " left join hrm_trn_temployee2shifttype o on o.employee_gid = a.employee_gid " +
                           " left join hrm_mst_tshifttype u on u.shifttype_gid = o.shifttype_gid " +
                           " left join hrm_trn_tleavegrade2employee y on y.employee_gid = a.employee_gid " +
                           " left join hrm_mst_tholidaygrade x on x.holidaygrade_gid = r.holidaygrade_gid " +
                           " left join hrm_mst_temployeepreviouscompany t on a.employeepreviouscompany_gid = t.employeepreviouscompany_gid" +
                           " WHERE a.employee_gid = '" + employee_gid + "'" +
                           " GROUP BY a.employee_gid";
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
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            identity_no = dt["identity_no"].ToString(),
                            employee_dob = dt["employee_dob"].ToString(),
                            employee_sign = dt["employee_sign"].ToString(),
                            bloodgroup_name = dt["bloodgroup_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            jobtype_gid = dt["jobtype_gid"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            employee_companyemailid = dt["employee_companyemailid"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            employee_personalno = dt["employee_personalno"].ToString(),
                            employee_documents = dt["employee_sign"].ToString(),
                            employee_experience = dt["employee_experience"].ToString(),
                            employee_experiencedtl = dt["employee_experiencedtl"].ToString(),
                            employeereporting_to = dt["employeereporting_to"].ToString(),
                            employment_type = dt["employment_type"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            holidaygrade_name = dt["holidaygrade_name"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_lastname = dt["user_lastname"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            user_status1 = dt["emp_active_flag"].ToString(),
                            usergroup_gid = dt["usergroup_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_hideattendance = dt["emp_hideattendance"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            approveby_name = dt["approveby_name"].ToString(),
                            nationality = dt["nationality"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            role_name = dt["role_gid"].ToString(),
                            role = dt["role_name"].ToString(),
                            workertype_gid = dt["workertype_gid"].ToString(),
                            jobtype_name = dt["jobtype_name"].ToString(),
                            workertype_name = dt["workertype_name"].ToString(),
                            biometric_id = dt["biometric_id"].ToString(),
                            user_password = dt["user_password"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                            shift = dt["shifttype_name"].ToString(),
                            tagid = dt["employee_tagid"].ToString(),
                            father_name = dt["father_name"].ToString(),
                            employee_qualification = dt["employee_qualification"].ToString(),
                            employee_diffabled = dt["employee_diffabled"].ToString(),
                            age = dt["age"].ToString(),
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
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
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
            string entity = httpRequest.Form[0];
            string branch = httpRequest.Form[1];
            string department = httpRequest.Form[2];
            string designation = httpRequest.Form[3];
            string active_flag = httpRequest.Form[4];
            string user_code = httpRequest.Form[5];
            string first_name = httpRequest.Form[6];
            string last_name = httpRequest.Form[7];
            string gender = httpRequest.Form[8];
            string email = httpRequest.Form[9];
            string mobile = httpRequest.Form[10];
            string permanent_address1 = httpRequest.Form[11];
            string permanent_address2 = httpRequest.Form[12];
            string country = httpRequest.Form[13];
            string permanent_city = httpRequest.Form[14];
            string permanent_state = httpRequest.Form[15];
            string permanent_postal = httpRequest.Form[16];
            string temporary_address1 = httpRequest.Form[17];
            string temporary_address2 = httpRequest.Form[18];
            string countryname = httpRequest.Form[19];
            string temporary_city = httpRequest.Form[20];
            string temporary_state = httpRequest.Form[21];
            string temporary_postal = httpRequest.Form[22];
            string permanent_addressgid = httpRequest.Form[23];
            string temporary_addressgid = httpRequest.Form[24];
            string employee_gid = httpRequest.Form[25];
            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
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
                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        lspath = "assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "Employee/Profile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string final_path = lspath + msdocument_gid + FileExtension;

                        msSQL = " SELECT user_gid FROM hrm_mst_temployee where employee_gid = '" + employee_gid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows)
                        {
                            lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                        }

                        msSQL = " update  adm_mst_tuser set " +
                                   " user_firstname = '" + first_name + "'," +
                                   " user_lastname = '" + last_name + "'," +
                                   " user_status = '" + active_flag + "'," +
                                   " updated_by = '" + user_gid + "'," +
                                   " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            msSQL = " update  hrm_mst_temployee set " +
                                    " designation_gid = '" + designation + "'," +
                                    " employee_mobileno = '" + mobile + "'," +
                                    " employee_emailid = '" + email + "'," +
                                    " employee_gender = '" + gender + "'," +
                                    " department_gid = '" + department + "'," +
                                    " employee_photo = '" + final_path + "'," +
                                    " entity_gid = '" + entity + "'," +
                                    " useraccess = '" + active_flag + "'," +
                                    " branch_gid = '" + branch + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where employee_gid='" + employee_gid + "' ";
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
                        if (mnResult != 0)
                        {
                            objResult.status = true;
                            objResult.message = "Employee Updated Successfully !!";
                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Error While Updating  Employee !!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateEmployeedetails(employee_lists values, string user_gid)
        {
            try
            {
                string lsbloodgroupgid = "";

                msSQL = "select entity_gid from adm_mst_tentity where entity_name='" + values.entityname + "'";
                string lsentityname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + values.branchname + "'";
                string lsbranchname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select designation_gid from adm_mst_tdesignation where designation_name= '" + values.designationname + "'";
                string lsdesignationname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + values.countryname + "'";
                string lscountryname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + values.country + "'";
                string lscountry = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select workertype_gid from hrm_mst_tworkertype where workertype_name = '" + values.workertype + "'";
                string lsworkertypename = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select jobtype_gid from hrm_mst_tjobtype where jobtype_name = '" + values.jobtype + "'";
                string lsjotype = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select shifttype_gid from hrm_mst_tshifttype where shifttype_name = '" + values.shift + "'";
                string lsshift = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select department_gid from hrm_mst_tdepartment where department_name = '" + values.departmentname + "' ";
                string lsdepartmentname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select leavegrade_gid from hrm_mst_tleavegrade where leavegrade_name = '" + values.leavegrade + "' ";
                string lsleavegrade = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select holidaygrade_gid from hrm_mst_tholidaygrade where holidaygrade_name = '" + values.holidaygrade + "'";
                string lsholidaygrade = objdbconn.GetExecuteScalar(msSQL);

                if (values.bloodgroup != "" && values.bloodgroup != null)
                {
                    msSQL = "select bloodgroup_gid from sys_mst_tbloodgroup where bloodgroup_name = '" + values.bloodgroup + "'";
                    lsbloodgroupgid = objdbconn.GetExecuteScalar(msSQL);

                }

                msSQL = " SELECT user_gid FROM hrm_mst_temployee where employee_gid = '" + values.employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows)
                {
                    lsuser_gid = objOdbcDataReader["user_gid"].ToString();
                }

                msSQL = " update  adm_mst_tuser set " +
                        " user_firstname = '" + values.first_name + "'," +
                        " user_code = '" + values.user_code + "'," +
                        " user_lastname = '" + values.last_name + "'," +
                        " user_status = '" + values.active_flag + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    string uiDateStr = values.dob;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string mysqldobdate = uiDate.ToString("yyyy-MM-dd"); 

                    string uiDateStr1 = values.employee_joiningdate;
                    DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string mysqljoiningdate = uiDate1.ToString("yyyy-MM-dd");

                    msSQL = " update  hrm_mst_temployee set " +
                            " designation_gid = '" + lsdesignationname + "'," +
                            " jobtype_gid = '" + lsjotype + "'," +
                            " bloodgroup_name = '" + lsbloodgroupgid + "'," +
                            " age = '" + values.age + "'," +
                              " employee_dob = '" + mysqldobdate + "'," +
                              " employee_gender = '" + values.gender + "'," +
                              " remarks = '" + values.remarks + "'," +
                              " employee_diffabled = '" + values.differentlyabled + "'," +
                              " employee_companyemailid = '" + values.comp_email + "'," +
                              " employee_qualification = '" + values.qualification + "'," +
                              " employee_tagid = '" + values.tagid + "'," +
                              " employee_joiningdate = '" + mysqljoiningdate + "'," +
                            " employee_mobileno = '" + values.mobileno + "'," +
                             " employee_personalno = '" + values.mobile + "'," +
                            " employee_emailid = '" + values.email + "'," +
                            " employee_gender = '" + values.gender + "'," +
                            " department_gid = '" + lsdepartmentname + "'," +
                              " employeereporting_to = '" + values.reportingto + "'," +
                            " entity_gid = '" + lsentityname + "'," +
                            " role_gid = '" + values.role + "'," +
                              " employee_tagid = '" + values.tagid + "'," +
                            " father_name = '" + values.father_spouse + "'," +
                            " useraccess = '" + values.active_flag + "'," +
                            " branch_gid = '" + lsbranchname + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where employee_gid='" + values.employee_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = " update hrm_trn_temployeetypedtl set " +
                                " wagestype_gid='wg001', " +
                                " systemtype_gid='Audit', " +
                                " branch_gid='" + lsbranchname + "', " +
                                " workertype_gid='" + lsworkertypename + "', " +
                                " employeetype_name='Roll', " +
                                " department_gid='" + lsdepartmentname + "', " +
                                " designation_gid='" + lsdesignationname + "', " +
                                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " updated_by='" + user_gid + "'" +
                                " where employee_gid = '" + values.employee_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            msSQL = " select parent_gid from adm_mst_taddress where parent_gid='" + values.employee_gid + "' and address_type='Permanent' ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                            if (objOdbcDataReader.HasRows)
                            {
                                msSQL = " update adm_mst_taddress SET " +
                                        " country_gid = '" + lscountry + "', " +
                                        " address1 =  '" + values.permanent_address1 + "', " +
                                        " address2 = '" + values.permanent_address2 + "'," +
                                        " city = '" + values.permanent_city + "', " +
                                        " state = '" + values.permanent_state + "', " +
                                        " postal_code = '" + values.permanent_postal + "'" +
                                        " where address_gid = '" + values.permanent_addressgid + "' and " +
                                        " parent_gid = '" + values.employee_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else
                            {
                                msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

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
                                        " '" + values.employee_gid + "'," +
                                        " '" + lscountry + "'," +
                                        " '" + values.permanent_address1 + "'," +
                                        " '" + values.permanent_address2 + "'," +
                                        " '" + values.permanent_city + "'," +
                                        " '" + values.permanent_state + "'," +
                                        " 'Permanent'," +
                                        " '" + values.permanent_postal + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            if (mnResult == 1)
                            {
                                msSQL = "select parent_gid from adm_mst_taddress where parent_gid='" + values.employee_gid + "' and address_type='Temporary' ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                if (objOdbcDataReader.HasRows)
                                {
                                    msSQL = " update adm_mst_taddress SET " +
                                            " country_gid = '" + lscountryname + "', " +
                                            " address1 =  '" + values.temporary_address1 + "', " +
                                            " address2 = '" + values.temporary_address2 + "'," +
                                            " city = '" + values.temporary_city + "', " +
                                            " state = '" + values.temporary_state + "', " +
                                            " postal_code = '" + values.temporary_postal + "'" +
                                            " where address_gid = '" + values.temporary_addressgid + "' and " +
                                            " parent_gid = '" + values.employee_gid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                                else
                                {
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
                                            " '" + msTemporaryAddressGetGID + "'," +
                                            " '" + values.employee_gid + "'," +
                                            " '" + lscountryname + "'," +
                                            " '" + values.temporary_address1 + "'," +
                                            " '" + values.temporary_address2 + "'," +
                                            " '" + values.temporary_city + "'," +
                                            " '" + values.temporary_state + "'," +
                                            " 'Temporary'," +
                                            " '" + values.temporary_postal + "'," +
                                            " '" + user_gid + "'," +
                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                    }

                    msSQL = " delete from hrm_trn_temployee2shifttype where employee_gid = '" + values.employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    string msGetGID_shift = objcmnfunctions.GetMasterGID("HESC");

                    msSQL = " insert into hrm_trn_temployee2shifttype( " +
                            " employee2shifttype_gid, " +
                            " employee_gid,  " +
                            " shifttype_gid, " +
                            " shifteffective_date ," +
                            " shiftactive_flag ," +
                            " created_by," +
                            " created_date) " +
                            " values( " +
                            "'" + msGetGID_shift + "'," +
                            "'" + values.employee_gid + "'," +
                            "'" + lsshift + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'Y'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = " select * from hrm_trn_temployee2shifttypedtl where employee_gid ='" + values.employee_gid + "' and shifttype_gid ='" + lsshift + "' ";
                        objtbl = objdbconn.GetDataTable(msSQL);

                        if (objtbl.Rows.Count > 0)
                        {
                            msSQL = " update hrm_trn_temployee2shifttypedtl set shift_status = 'Y' where employee_gid = '" + values.employee_gid + "' " +
                                    " and shifttype_gid='" + lsshift + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            msSQL = " delete from hrm_trn_temployee2shifttypedtl " +
                                    " where employee_gid='" + values.employee_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " select a.shifttypedtl_gid,a.shifttype_gid,a.shifttypedtl_name,a.shift_fromhours,a.shift_tohours, " +
                                    " a.lunchout_hours,a.lunchout_minutes,a.lunchin_hours,a.lunchin_minutes, " +
                                    " a.shift_fromminutes, a.shift_tominutes from hrm_mst_tshifttypedtl a " +
                                    " inner join hrm_mst_tshifttype b on a.shifttype_gid=b.shifttype_gid " +
                                    " where b.shifttype_gid='" + lsshift + "' ";
                            objtbl = objdbconn.GetDataTable(msSQL);

                            if (objtbl.Rows.Count > 0)
                            {
                                foreach (DataRow dt in objtbl.Rows)
                                {
                                    msGetGIDN = objcmnfunctions.GetMasterGID("HEST");
                                    msSQL = " insert into hrm_trn_temployee2shifttypedtl (" +
                                            " employee2shifttypedtl_gid," +
                                            " employee2shifttype_gid," +
                                            " shifttype_gid," +
                                            " shifttypedtl_gid," +
                                            " employee2shifttype_name," +
                                            " shift_fromhours," +
                                            " shift_fromminutes," +
                                            " shift_tohours," +
                                            " shift_tominutes," +
                                            " lunchout_hours," +
                                            " lunchout_minutes," +
                                            " lunchin_hours," +
                                            " lunchin_minutes," +
                                            " employee_gid," +
                                            " created_by," +
                                            " shift_status, " +
                                            " created_date)" +
                                            " values (" +
                                            " '" + msGetGIDN + "', " +
                                            " '" + msGetGID_shift + "', " +
                                            " '" + lsshift + "', " +
                                            " '" + dt["shifttypedtl_gid"].ToString() + "', " +
                                            " '" + dt["shifttypedtl_name"].ToString() + "'," +
                                            " '" + dt["shift_fromhours"].ToString() + "'," +
                                            " '" + dt["shift_fromminutes"].ToString() + "'," +
                                            " '" + dt["shift_tohours"].ToString() + "'," +
                                            " '" + dt["shift_tominutes"].ToString() + "'," +
                                            " '" + dt["lunchout_hours"].ToString() + "'," +
                                            " '" + dt["lunchout_minutes"].ToString() + "'," +
                                            " '" + dt["lunchin_hours"].ToString() + "'," +
                                            " '" + dt["lunchin_minutes"].ToString() + "'," +
                                            " '" + values.employee_gid + "'," +
                                            " '" + user_gid + "'," +
                                            " 'Y'," +
                                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }
                            }
                        }
                    }

                    msSQL = " delete from hrm_trn_tleavegrade2employee " +
                               " where employee_gid = '" + values.employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select c.leavetype_name, b.leavetype_gid, a.leavegrade_gid, a.leavegrade_code, a.leavegrade_name, c.leavetype_name, a.attendance_startdate, a.attendance_enddate, " +
                            " format(sum(b.total_leavecount),2) as total_leavecount, format(sum(b.available_leavecount),2) as available_leavecount, " +
                            " format(sum(b.leave_limit),2) as leave_limit from hrm_mst_tleavegrade a " +
                            " left join hrm_mst_tleavegradedtl b on a.leavegrade_gid = b.leavegrade_gid  " +
                            " left join hrm_mst_tleavetype c on b.leavetype_gid = c.leavetype_gid where a.leavegrade_gid ='" + lsleavegrade + "' group by a.leavegrade_gid ";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        lsleavegrade_code = objOdbcDataReader["leavegrade_code"].ToString();
                        lsleavegrade_name = objOdbcDataReader["leavegrade_name"].ToString();
                        lsattendance_startdate = objOdbcDataReader["attendance_startdate"].ToString();
                        lsattendance_enddate = objOdbcDataReader["attendance_enddate"].ToString();
                        lsleavetype_gid = objOdbcDataReader["leavetype_gid"].ToString();
                        lsleavetype_name = objOdbcDataReader["leavetype_name"].ToString();
                        lstotal_leavecount = objOdbcDataReader["total_leavecount"].ToString();
                        lsavailable_leavecount = objOdbcDataReader["available_leavecount"].ToString();
                        lsleave_limit = objOdbcDataReader["leave_limit"].ToString();

                        string msgetassign2employee_gid = objcmnfunctions.GetMasterGID("LE2G");
                        msSQL = " insert into hrm_trn_tleavegrade2employee ( " +
                                " leavegrade2employee_gid," +
                                " branch_gid ," +
                                " employee_gid," +
                                " employee_name," +
                                " leavegrade_gid," +
                                " leavegrade_code," +
                                " leavegrade_name, " +
                                " attendance_startdate, " +
                                " attendance_enddate, " +
                                " total_leavecount, " +
                                " available_leavecount, " +
                                " leave_limit " +
                                " ) Values ( " +
                                " '" + msgetassign2employee_gid + "', " +
                                " '" + lsbranchname + "', " +
                                " '" + values.employee_gid + "', " +
                                " '" + values.first_name + "', " +
                                " '" + lsleavegrade + "', " +
                                " '" + lsleavegrade_code + "'," +
                                " '" + lsleavegrade_name + "'," +
                                " '" + Convert.ToDateTime(lsattendance_startdate).ToString("yyyy-MM-dd") + "'," +
                                " '" + Convert.ToDateTime(lsattendance_enddate).ToString("yyyy-MM-dd") + "'," +
                                " '" + lstotal_leavecount + "'," +
                                " '" + lsavailable_leavecount + "'," +
                                " '" + lsleave_limit + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = " delete from hrm_mst_tholiday2employee " +
                                   " where employee_gid='" + values.employee_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select a.holidaygrade_gid, b.holiday_gid, b.holiday_date from hrm_mst_tholidaygrade a " +
                            " left join hrm_mst_tholiday2grade b on b.holidaygrade_gid = a.holidaygrade_gid " +
                            " where a.holidaygrade_gid ='" + lsholidaygrade + "' ";
                    DataTable objtblgrade = objdbconn.GetDataTable(msSQL);

                    if (objtblgrade.Rows.Count > 0)
                    {
                        foreach (DataRow dt in objtblgrade.Rows)
                        {
                            lsholidaygrade_gid = dt["holidaygrade_gid"].ToString();
                            lsholiday_gid = dt["holiday_gid"].ToString();
                            lsholiday_date = dt["holiday_date"].ToString();

                            string msGetGID = objcmnfunctions.GetMasterGID("HYTE");



                            msSQL = " insert into hrm_mst_tholiday2employee ( " +
                                    " holiday2employee, " +
                                    " holidaygrade_gid, " +
                                    " holiday_gid, " +
                                    " employee_gid, " +
                                    " holiday_date, " +
                                    " created_by, " +
                                    " created_date ) " +
                                    " values ( " +
                                    " '" + msGetGID + "', " +
                                    " '" + lsholidaygrade_gid + "', " +
                                    " '" + lsholiday_gid + "'," +
                                    " '" + values.employee_gid + "', " +
                                    " '" + Convert.ToDateTime(lsholiday_date).ToString("yyyy-MM-dd") + "', " +
                                    " '" + user_gid + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Employee Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Employee !!";
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
                    values.message = "Password Reset Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Reset Password !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
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
                             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "' ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "User Code Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While User Code Updated !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "User Code Already Exist !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
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

                msSQL = " update  hrm_mst_temployee set " +
                        //" exit_date = '" + values.deactivation_date.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " exit_date = str_to_date('" + values.deactivation_date + "', '%d-%m-%Y')," +
                        " remarks  = '" + values.remarks + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + lsuser_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "User Deactivated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deactivating User !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
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

                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
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

                            using (OleDbCommand command = new OleDbCommand())
                            {
                                command.Connection = connection;
                                command.CommandText = "select * from [Sheet1$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                importcount = 0;

                                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                                {
                                    DataRow row = dataTable.Rows[i];
                                    if (row.ItemArray.All(field => string.IsNullOrWhiteSpace(field?.ToString())))
                                    {
                                        dataTable.Rows.Remove(row);
                                    }
                                }

                                foreach (DataRow row in dataTable.Rows)
                                {
                                    Branch = row["Branch"].ToString();

                                    msSQL = " select branch_gid from hrm_mst_tbranch where branch_name='" + Branch + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetbranchGID = objOdbcDataReader["branch_gid"].ToString();
                                    }

                                    else
                                    {
                                        msGetbranchGID = objcmnfunctions.GetMasterGID("HBHM");
                                        msSQL = " insert into hrm_mst_tbranch (" +
                                                " branch_gid, " +
                                                " branch_code, " +
                                                " branch_name, " +
                                                " address1, " +
                                                " city, " +
                                                " state, " +
                                                " postal_code) " +
                                                " values (" +
                                                " '" + msGetbranchGID + "'," +
                                                " '" + Branch.Substring(0, 3) + "'," +
                                                " '" + Branch.ToString().Replace(",", "") + "', " +
                                                " Null, " +
                                                " Null," +
                                                " Null, " +
                                                " Null)";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        msgetaddressGID = objcmnfunctions.GetMasterGID("SADM");
                                        msSQL = " insert into adm_mst_taddress ( " +
                                                " address_gid ," +
                                                " parent_gid," +
                                                " country_gid, " +
                                                " address1, " +
                                                " city, " +
                                                " state, " +
                                                " postal_code) " +
                                                " values ( " +
                                                " '" + msgetaddressGID + "', " +
                                                " '" + msGetbranchGID + "', " +
                                                " Null, " +
                                                " Null, " +
                                                " Null, " +
                                                " Null, " +
                                                " Null)";
                                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                    Department = row["Department"].ToString();

                                    msSQL = "select department_gid from hrm_mst_tdepartment where department_name='" + Department + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetdepartmentGID = objOdbcDataReader["department_gid"].ToString();
                                    }
                                    else
                                    {
                                        msGetdepartmentGID = objcmnfunctions.GetMasterGID("HDPM");
                                        msSQL = " insert into hrm_mst_tdepartment (" +
                                                " department_gid, " +
                                                " department_code, " +
                                                " department_name," +
                                                " created_by," +
                                                " created_date )" +
                                                " values (" +
                                                " '" + msGetdepartmentGID + "', " +
                                                " '" + Department.Substring(0, 3) + "'," +
                                                " '" + Department.ToString().Replace(",", "") + "', " +
                                                " '" + user_gid + "', " +
                                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }


                                    lsrole = row["Role(Name)"].ToString();

                                    msSQL = "select role_gid from hrm_mst_trole where role_name='" + lsrole + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        lsrole_gid = row["Role(Name)"].ToString();
                                    }
                                    else
                                    {
                                        lsrole_gid = objcmnfunctions.GetMasterGID("HRLE");
                                        msSQL = " insert into hrm_mst_trole(" +
                                                " role_gid," +
                                                " role_code," +
                                                " role_name," +
                                                " created_by," +
                                                " created_date) " +
                                                " values(" +
                                                " '" + lsrole_gid + "'," +
                                                " '" + lsrole_gid + "'," +
                                                " '" + lsrole + "'," +
                                                " '" + user_gid + "'," +
                                                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                    Designation = row["Designation"].ToString();

                                    msSQL = "select designation_gid from adm_mst_tdesignation where designation_name='" + Designation + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        msGetdesignationGID = objOdbcDataReader["designation_gid"].ToString();
                                    }
                                    else
                                    {
                                        msGetdesignationGID = objcmnfunctions.GetMasterGID("SDGM");
                                        msSQL = " insert into adm_mst_tdesignation (" +
                                                " designation_gid, " +
                                                " designation_code, " +
                                                " role_gid, " +
                                                " designation_name)" +
                                                " values (" +
                                                " '" + msGetdesignationGID + "', " +
                                                " '" + Designation.Substring(0, 3) + "'," +
                                                " '" + lsrole_gid + "'," +
                                                " '" + Designation.ToString().Replace(",", "") + "')";
                                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }

                                    JobType = row["JobType"].ToString();

                                    if (!string.IsNullOrEmpty(JobType))
                                    {
                                        string jobTypeCode = JobType.Length >= 3 ? JobType.Substring(0, 3) : JobType;

                                        msSQL = "select jobtype_gid from hrm_mst_tjobtype where jobtype_name='" + JobType + "'";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                        if (objOdbcDataReader.HasRows == true)
                                        {
                                            msGetjobtypeGID = objOdbcDataReader["jobtype_gid"].ToString();
                                        }
                                        else
                                        {
                                            msGetjobtypeGID = objcmnfunctions.GetMasterGID("HJTM");

                                            msSQL = " insert into hrm_mst_tjobtype (" +
                                                    " created_by," +
                                                    " created_Date," +
                                                    " jobtype_gid, " +
                                                    " jobtype_code, " +
                                                    " jobtype_name)" +
                                                    " values (" +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    " '" + msGetjobtypeGID + "', " +
                                                    " '" + JobType.Substring(0, 3) + "'," +
                                                    " '" + JobType.ToString().Replace(",", "") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                    }


                                    EmployeeLoginID = row["EmployeeLoginID"].ToString();
                                    FirstName = row["FirstName"].ToString();
                                    LastName = row["LastName"].ToString();
                                    LoginPassword = row["LoginPassword"].ToString();

                                    msSQL = " SELECT user_code FROM adm_mst_tuser where user_code = '" +EmployeeLoginID + "' ";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objOdbcDataReader.HasRows)
                                    {
                                        lsuser_code = objOdbcDataReader["user_code"].ToString();
                                        objResult.status = false;
                                        objResult.message = "Employee User Code Already Exist";
                                        return;
                                    }
                                    else
                                    {

                                        msgetUserGid = objcmnfunctions.GetMasterGID("SUSM");
                                        msSQL = " Insert into adm_mst_tuser ( " +
                                                " user_gid , " +
                                                " user_code , " +
                                                " user_firstname , " +
                                                " user_lastname , " +
                                                " user_password , " +
                                                " user_status)  " +
                                                " values ( " +
                                                " '" + msgetUserGid + "'," +
                                                " '" + EmployeeLoginID + "'," +
                                                " '" + FirstName + "'," +
                                                " '" + LastName + "'," +
                                                " '" + objcmnfunctions.ConvertToAscii(LoginPassword) + "'," +
                                                " 'Y')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        Mobile = row["Mobile"].ToString();
                                        DOB = row["DOB(dd/mm/yyyy)"].ToString();
                                        Email = row["Email"].ToString();
                                        Qualification = row["Qualification"].ToString();    
                                        Experience = row["Experience"].ToString();
                                        Gender = row["Gender"].ToString();
                                        EmployeeAccess = row["EmployeeAccess(Yes or No)"].ToString();
                                        JoiningDate = row["JoiningDate(dd/mm/yyyy)"].ToString();
                                        EmployeeType = row["EmployeeType(Roll or Non Roll)"].ToString();
                                        RollType = row["RollType(Own or Contract)"].ToString();
                                        TagId = row["TagId"].ToString();
                                        HideAttendance = row["HideAttendance(Yes or No)"].ToString();
                                        BloodGroup = row["BloodGroup"].ToString();

                                        msgetEmployeeGID = objcmnfunctions.GetMasterGID("SERM");
                                        string lsbiometricid = "";

                                        if (row["Biometric Id"].ToString().ToLower() == "nil")

                                        {
                                            lsbiometricid = null;
                                        }
                                        else
                                        {
                                            lsbiometricid = row["Biometric Id"].ToString();
                                        }




                                        string reportingToCode = row["ReportingTo(Code)"].ToString();
                                        if (string.IsNullOrEmpty(reportingToCode) || reportingToCode.ToLower()  == "nil")

                                        {
                                            ReportingTo = null;
                                        }
                                        else
                                        {
                                            string employeecode = row["ReportingTo(Code)"].ToString();
                                            msSQL = "select b.employee_gid from adm_mst_Tuser a left join hrm_mst_temployee b on a.user_gid=b.user_gid where a.user_code='" + employeecode + "'";
                                            ReportingTo = objdbconn.GetExecuteScalar(msSQL);
                                        }

                                        if(!string.IsNullOrEmpty(DOB))
                                        {
                                             dobValue = row["DOB(dd/mm/yyyy)"].ToString();

                                             lsdobvalue = GetDateFormat(dobValue);

                                             nowdate = DateTime.Now.ToString("yyyy-MM-dd");

                                            DateTime birthdate = DateTime.Parse(lsdobvalue);

                                            DateTime nowdatetime = DateTime.Parse(nowdate);

                                             yearsDifference = nowdatetime.Year - birthdate.Year;
                                        }

                                        if (!string.IsNullOrEmpty(JoiningDate))
                                        {
                                            JoiningDatetime = row["JoiningDate(dd/mm/yyyy)"].ToString();

                                            lsJoiningDate = GetDateFormat(JoiningDatetime);

                                        }


                                        msSQL = " Insert into hrm_mst_temployee ( " +
                                                    " employee_gid , " +
                                                    " user_gid," +
                                                    " branch_gid , " +
                                                    " jobtype_gid," +
                                                    " employeereporting_to," +
                                                    " designation_gid," +
                                                    " employee_mobileno , " +
                                                    " employee_dob , " +
                                                    " age , " +
                                                    " employee_emailid , " +
                                                    " employee_qualification , " +
                                                    " employee_experience , " +
                                                    " employee_gender , " +
                                                    " department_gid," +
                                                    " employee_image," +
                                                    " employee_sign," +
                                                    " useraccess," +
                                                    " created_by, " +
                                                    " created_date, " +
                                                    " employee_joiningdate, " +
                                                    " biometric_id, " +
                                                    " employeetype_gid , " +
                                                    " rolltype_gid , " +
                                                    " employee_tagid, " +
                                                    " employee_hideattendance, " +
                                                    " bloodgroup_name" +
                                                    " ) values ( " +
                                                    " '" + msgetEmployeeGID + "', " +
                                                    " '" + msgetUserGid + "', " +
                                                    " '" + msGetbranchGID + "'," +
                                                    " '" + msGetjobtypeGID + "'," +
                                                    " '" + ReportingTo + "'," +
                                                    " '" + msGetdesignationGID + "'," +
                                                    " '" + Mobile.ToString().Replace(",", "") + "',";


                                                    if ((lsdobvalue == null) || (lsdobvalue == ""))
                                                    {
                                                        msSQL +=null;
                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + lsdobvalue + "',";
                                                    }
                                                    if (yearsDifference == 0)
                                                    {
                                                        msSQL += null;
                                                    }
                                                    else
                                                    {
                                                        msSQL += "'" + yearsDifference + "',";
                                                    }

                                        //if (!string.IsNullOrEmpty(dobValue))
                                        //{

                                        //    DateTime uiDate2 = DateTime.ParseExact(dobValue, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                                        //    string parsedDOB = uiDate2.ToString("yyyy-MM-dd");
                                        //    msSQL += "'" + parsedDOB + "', ";
                                        //    msSQL += "TIMESTAMPDIFF(YEAR, '" + parsedDOB + "', CURDATE()), ";
                                        //}

                                        //    else
                                        //    {
                                        //        msSQL += "null, ";
                                        //        msSQL += "null, ";
                                        //    }


                                        msSQL += "'" + Email.ToString().Replace(",", "") + "'," +
                                                     "'" + Qualification.ToString().Replace(",", "") + "'," +
                                                     "'" + Experience.ToString().Replace(",", "") + "'," +
                                                     "'" + Gender + "'," +
                                                     "'" + msGetdepartmentGID + "'," +
                                                     "''," +
                                                     "''," +
                                                     "'" + EmployeeAccess + "'," +
                                                     "'" + user_gid + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";

                                            if ((lsJoiningDate == null) || (lsJoiningDate == ""))
                                            {
                                            msSQL += "null,";
                                        }
                                            else
                                            {
                                                msSQL += "'" + lsJoiningDate + "',";
                                            }
                                        if ((lsbiometricid == null) || (lsbiometricid == ""))
                                        {
                                            msSQL += "null,";
                                        }
                                        else
                                        {
                                            msSQL += "'" + lsbiometricid + "',";
                                        }

                                            

                                            if (EmployeeType != "")
                                            {
                                                msSQL += "'" + EmployeeType + "',";
                                            }
                                            else
                                            {
                                                msSQL += "'ROLL',";
                                            }

                                            msSQL += "'" + RollType + "'," +
                                                     "'" + TagId + "', " +
                                                     "'" + HideAttendance + "', " +
                                                     "'" + BloodGroup.ToString().Replace("'", "") +
                                                     "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            if (mnResult == 1)
                                            {
                                                msSQL = " select concat(user_firstname,' ',user_lastname) as employee_name from adm_mst_tuser where user_gid='" + user_gid + "'";
                                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                                if (objOdbcDataReader.HasRows == true)
                                                {
                                                    Employee_Name = objOdbcDataReader["employee_name"].ToString();
                                                }
                                            }

                                            if (mnResult == 1)
                                            {

                                            msBiometricGID = objcmnfunctions.GetBiometricGID();
                                                msSQL = " Insert into hrm_mst_tbiometric" +
                                                         " (biometric_gid," +
                                                         " biometric_name," +
                                                         " biometric_status," +
                                                         " biometric_register, " +
                                                         " employee_gid) " +
                                                         " values( " +
                                                         " '" + msBiometricGID + "', " +
                                                         " '" + FirstName + "', " +
                                                         " 'N', " +
                                                         " 'N', " +
                                                         " '" + msgetEmployeeGID + "') ";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }

                                            Shift = row["Shift"].ToString();


                                            if (Shift != "NIL")
                                            {
                                                msSQL = " select shifttype_gid, shifttype_name from hrm_mst_tshifttype where shifttype_name='" + Shift + "'";
                                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                                if (objOdbcDataReader.HasRows == true)
                                                {
                                                    ShiftType_Gid = objOdbcDataReader["shifttype_gid"].ToString();

                                                    msGetShiftGID = objcmnfunctions.GetMasterGID("HESC");

                                                    msGetShiftGID = objcmnfunctions.GetMasterGID("HESC");
                                                    msSQL = " insert into hrm_trn_temployee2shifttype( " +
                                                            " employee2shifttype_gid, " +
                                                            " employee_gid,  " +
                                                            " shifttype_gid, " +
                                                            " shifteffective_date ," +
                                                            " shiftactive_flag ," +
                                                            " created_by," +
                                                            " created_date) " +
                                                            " values( " +
                                                            "'" + msGetShiftGID + "'," +
                                                            "'" + msgetEmployeeGID + "'," +
                                                            "'" + ShiftType_Gid + "'," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                            "'Y'," +
                                                            "'" + user_gid + "'," +
                                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                    mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }

                                                if (mnResult3 == 1)
                                                {
                                                    msSQL = " select * from hrm_trn_temployee2shifttypedtl where employee_gid = '" + msgetEmployeeGID + "' and shifttype_gid = '" + msGetShiftGID + "' ";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                                    if (objOdbcDataReader.HasRows == true)
                                                    {
                                                        msSQL = " update hrm_trn_temployee2shifttypedtl set shift_status = 'Y' where employee_gid ='" + msgetEmployeeGID + "' " +
                                                                " and shifttype_gid='" + msGetShiftGID + "' ";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }
                                                    else
                                                    {
                                                        msSQL = " select a.shifttypedtl_gid, a.shifttype_gid, a.shifttypedtl_name, a.shift_fromhours, a.shift_tohours, a.lunchout_hours, " +
                                                                " a.lunchout_minutes, a.lunchin_hours, a.lunchin_minutes, a.shift_fromminutes, a.shift_tominutes from hrm_mst_tshifttypedtl a " +
                                                                " inner join hrm_mst_tshifttype b on a.shifttype_gid=b.shifttype_gid where b.shifttype_gid ='" + msGetShiftGID + "' ";
                                                        dt_datatable = objdbconn.GetDataTable(msSQL);

                                                        foreach (DataRow dt1 in dt_datatable.Rows)
                                                        {
                                                            msGetGIDN = objcmnfunctions.GetMasterGID("HEST");
                                                            msSQL = " insert into hrm_trn_temployee2shifttypedtl (" +
                                                                    " employee2shifttypedtl_gid," +
                                                                    " employee2shifttype_gid," +
                                                                    " shifttype_gid," +
                                                                    " shifttypedtl_gid," +
                                                                    " employee2shifttype_name," +
                                                                    " shift_fromhours," +
                                                                    " shift_fromminutes," +
                                                                    " shift_tohours," +
                                                                    " shift_tominutes," +
                                                                    " lunchout_hours," +
                                                                    " lunchout_minutes," +
                                                                    " lunchin_hours," +
                                                                    " lunchin_minutes," +
                                                                    " employee_gid," +
                                                                    " created_by," +
                                                                    " shift_status, " +
                                                                    " created_date)" +
                                                                    " values (" +
                                                                    " '" + msGetGIDN + "', " +
                                                                    " '" + msGetShiftGID + "', " +
                                                                    " '" + ShiftType_Gid + "', " +
                                                                    " '" + ShiftType_Gid + "', " +
                                                                    " '" + dt1["shifttypedtl_gid"].ToString() + "', " +
                                                                    " '" + dt1["shifttypedtl_name"].ToString() + "'," +
                                                                    " '" + dt1["shift_fromhours"].ToString() + "'," +
                                                                    " '" + dt1["shift_fromminutes"].ToString() + "'," +
                                                                    " '" + dt1["shift_tohours"].ToString() + "'," +
                                                                    " '" + dt1["shift_tominutes"].ToString() + "'," +
                                                                    " '" + dt1["lunchout_hours"].ToString() + "'," +
                                                                    " '" + dt1["lunchout_minutes"].ToString() + "'," +
                                                                    " '" + dt1["lunchin_hours"].ToString() + "'," +
                                                                    " '" + dt1["lunchin_minutes"].ToString() + "'," +
                                                                    " '" + msgetEmployeeGID + "'," +
                                                                    " '" + user_gid + "'," +
                                                                    " 'Y'," +
                                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        }
                                                    }
                                                }
                                            }

                                            leavegrade = row["Leave Grade(Name)"].ToString();
                                            if (leavegrade != "NIL")
                                            {

                                                msSQL = " select c.leavetype_name, b.leavetype_gid, a.leavegrade_gid, a.leavegrade_code, a.leavegrade_name, c.leavetype_name, a.attendance_startdate, a.attendance_enddate, " +
                                                        " format(sum(b.total_leavecount),2) as total_leavecount, format(sum(b.available_leavecount),2) as available_leavecount, format(sum(b.leave_limit),2) as leave_limit " +
                                                        " from hrm_mst_tleavegrade a left join hrm_mst_tleavegradedtl b on a.leavegrade_gid=b.leavegrade_gid " +
                                                        " left join hrm_mst_tleavetype c on b.leavetype_gid = c.leavetype_gid where a.leavegrade_name='" + leavegrade + "' group by a.leavegrade_gid ";
                                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                                if (objOdbcDataReader.HasRows == true)
                                                {
                                                    leavegrade_gid = objOdbcDataReader["leavegrade_gid"].ToString();
                                                    leavegrade_code = objOdbcDataReader["leavegrade_code"].ToString();
                                                    leavegrade_name = objOdbcDataReader["leavegrade_name"].ToString();
                                                    attendance_startdate = objOdbcDataReader["attendance_startdate"].ToString();
                                                    attendance_enddate = objOdbcDataReader["attendance_enddate"].ToString();
                                                    leavetype_gid = objOdbcDataReader["leavetype_gid"].ToString();
                                                    leavetype_name = objOdbcDataReader["leavetype_name"].ToString();
                                                    total_leavecount = objOdbcDataReader["total_leavecount"].ToString();
                                                    available_leavecount = objOdbcDataReader["available_leavecount"].ToString();
                                                    leave_limit = objOdbcDataReader["leave_limit"].ToString();
                                                }

                                                msgetassign2employee_gid = objcmnfunctions.GetMasterGID("LE2G");
                                                msSQL = " insert into hrm_trn_tleavegrade2employee ( " +
                                                        " leavegrade2employee_gid," +
                                                        " branch_gid ," +
                                                        " employee_gid," +
                                                        " employee_name," +
                                                        " leavegrade_gid," +
                                                        " leavegrade_code," +
                                                        " leavegrade_name, " +
                                                        " attendance_startdate, " +
                                                        " attendance_enddate, " +
                                                        " total_leavecount, " +
                                                        " available_leavecount, " +
                                                        " leave_limit " +
                                                        " ) Values ( " +
                                                        " '" + msgetassign2employee_gid + "', " +
                                                        " '" + msGetbranchGID + "', " +
                                                        " '" + msgetEmployeeGID + "', " +
                                                        " '" + Employee_Name + "', " +
                                                        " '" + leavegrade_gid + "', " +
                                                        " '" + leavegrade_code + "'," +
                                                        " '" + leavegrade_name + "'," +
                                                        " '" + Convert.ToDateTime(attendance_startdate).ToString("yyyy-MM-dd") + "', " +
                                                        " '" + Convert.ToDateTime(attendance_enddate).ToString("yyyy-MM-dd") + "'," +
                                                        " '" + total_leavecount + "'," +
                                                        " '" + available_leavecount + "'," +
                                                        " '" + leave_limit + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }

                                            holidaygrade = row["Holiday Grade(Name)"].ToString();

                                            if (holidaygrade != "NIL" || holidaygrade != null)
                                            {
                                                msSQL = "select a.holidaygrade_gid,b.holiday_gid,b.holiday_date from hrm_mst_tholidaygrade a " +
                                                       " left join hrm_mst_tholiday2grade b on b.holidaygrade_gid=a.holidaygrade_gid " +
                                                       " where a.holidaygrade_name='" + holidaygrade + "' ";
                                                dt_datatable = objdbconn.GetDataTable(msSQL);


                                                if (dt_datatable.Rows.Count > 0)
                                                {
                                                    foreach (DataRow dt2 in dt_datatable.Rows)
                                                    {
                                                        lsholidaygrade_gid = dt2["holidaygrade_gid"].ToString();
                                                        lsholiday_gid = dt2["holiday_gid"].ToString();
                                                        lsholiday_date = dt2["holiday_date"].ToString();

                                                        string msGetGID = objcmnfunctions.GetMasterGID("HYTE");
                                                        msSQL = " insert into hrm_mst_tholiday2employee ( " +
                                                                   " holiday2employee, " +
                                                                   " holidaygrade_gid, " +
                                                                   " holiday_gid, " +
                                                                   " employee_gid, " +
                                                                   " holiday_date, " +
                                                                   " created_by, " +
                                                                   " created_date ) " +
                                                                   " values ( " +
                                                                   "'" + msGetGID + "', " +
                                                                   "'" + lsholidaygrade_gid + "', " +
                                                                   "'" + lsholiday_gid + "'," +
                                                                   "'" + msgetEmployeeGID + "', " +
                                                                   "'" + Convert.ToDateTime(lsholiday_date).ToString("yyyy-MM-dd") + "', " +
                                                                   "'" + user_gid + "', " +
                                                                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                                                        mnResult8 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    }
                                                }
                                            }

                                            msGetemployeetype = objcmnfunctions.GetMasterGID("SETD");

                                            WageType = row["WageType(Monthly or Weekly)"].ToString();

                                            if (WageType == "")
                                            {
                                                WageType = "Monthly";
                                            }

                                            msSQL = "select wagestype_gid from hrm_mst_twagestype where wagetype_name = '" + WageType + "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                            if (objOdbcDataReader.HasRows == true)
                                            {
                                                wagestype_gid = objOdbcDataReader["wagestype_gid"].ToString();
                                            }

                                            WorkerType = row["WorkerType(Staff or Worker)"].ToString();

                                            if (WorkerType == "")
                                            {
                                                WorkerType = "";
                                            }

                                            msSQL = "select workertype_gid from hrm_mst_tworkertype where workertype_name = '" + WorkerType + "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                            if (objOdbcDataReader.HasRows == true)
                                            {
                                                workertype_gid = objOdbcDataReader["workertype_gid"].ToString();
                                            }

                                            msSQL = " insert into hrm_trn_temployeetypedtl (" +
                                                    " employeetypedtl_gid," +
                                                    " employeetype_name," +
                                                    " employee_gid," +
                                                    " wagestype_gid, " +
                                                    " workertype_gid, " +
                                                    " systemtype_gid," +
                                                    " branch_gid," +
                                                    " department_gid," +
                                                    " designation_gid," +
                                                    " created_date," +
                                                    " created_by " +
                                                    " ) values " +
                                                    " ('" + msGetemployeetype + "', " +
                                                    " '" + EmployeeType + "', " +
                                                    " '" + msgetEmployeeGID + "'," +
                                                    " '" + WageType + "'," +
                                                    " '" + workertype_gid + "'," +
                                                    " '" + RollType + "'," +
                                                    " '" + msGetbranchGID + "'," +
                                                    " '" + msGetdepartmentGID + "'," +
                                                    " '" + msGetdesignationGID + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                    " '" + user_gid + "') ";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            msPermanentAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

                                            Country = row["Country"].ToString();

                                            msSQL = "select country_gid from adm_mst_tcountry where country_name ='" + Country + "'";
                                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                            if (objOdbcDataReader.HasRows == false)
                                            {
                                                msGetcountryGID = objcmnfunctions.GetMasterGID("CN");
                                                msSQL = " insert into adm_mst_tcountry " +
                                                        " (country_gid," +
                                                        " country_code," +
                                                        " country_name, " +
                                                        " cou_code " +
                                                        " )values (" +
                                                        " '" + msGetcountryGID + "'," +
                                                        " '" + Country.Substring(0, 3) + "'," +
                                                        " '" + Country + "'," +
                                                        " Null)";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                if (mnResult == 1)
                                                {
                                                    CountryGID = msGetcountryGID;
                                                }
                                            }
                                            else
                                            {
                                                CountryGID = objOdbcDataReader["country_gid"].ToString();
                                            }

                                            AddressFirstLine = row["AddressFirstLine"].ToString();
                                            AddressSecondLine = row["AddressSecondLine"].ToString();
                                            City = row["City"].ToString();
                                            PinCode = row["PinCode"].ToString();
                                            State = row["State"].ToString();

                                            msSQL = " insert into adm_mst_taddress " +
                                                    " ( address_gid ," +
                                                    " parent_gid," +
                                                    " country_gid, " +
                                                    " address1, " +
                                                    " address2, " +
                                                    " city, " +
                                                     " address_type, " +
                                                    " postal_code," +
                                                    " state) " +
                                                    " values ( " +
                                                    " '" + msPermanentAddressGetGID + "', " +
                                                    " '" + msgetEmployeeGID + "', " +
                                                    " '" + CountryGID + "', " +
                                                    " '" + AddressFirstLine + "', " +
                                                    " '" + AddressSecondLine + "', " +
                                                    " '" + City.Replace("'", "") + "', " +
                                                     " 'Permanent'," +
                                                     " '" + PinCode + "'," +
                                                    " '" + State.Replace("'", "") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            msTemporaryAddressGetGID = objcmnfunctions.GetMasterGID("SADM");

                                            msSQL = " insert into adm_mst_taddress " +
                                                    " ( address_gid ," +
                                                    " parent_gid," +
                                                    " country_gid, " +
                                                    " address1, " +
                                                    " address2, " +
                                                    " city, " +
                                                    " address_type, " +
                                                    " postal_code," +
                                                    " state) " +
                                                    " values ( " +
                                                    " '" + msTemporaryAddressGetGID + "', " +
                                                    " '" + msgetEmployeeGID + "', " +
                                                    " '" + CountryGID + "', " +
                                                    " '" + AddressFirstLine + "', " +
                                                    " '" + AddressSecondLine + "', " +
                                                    " '" + City.Replace("'", "") + "', " +
                                                     " 'Temporary'," +
                                                    " '" + PinCode + "'," +
                                                    " '" + State.Replace("'", "") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                            msEmployeedtlGid = objcmnfunctions.GetMasterGID("HEMC");
                                            msSQL = " Insert into hrm_trn_temployeedtl " +
                                                    " (employeedtl_gid," +
                                                    " permanentaddress_gid ," +
                                                    " temporaryaddress_gid," +
                                                    " employee_gid)" +
                                                    " VALUE ( " +
                                                    " '" + msEmployeedtlGid + "'," +
                                                    " '" + msPermanentAddressGetGID + "'," +
                                                    " '" + msTemporaryAddressGetGID + "' ," +
                                                    " '" + msgetEmployeeGID + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                if (mnResult == 1)
                {
                    objResult.status = true;
                    objResult.message = "Employee details imported Successfully";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error occured while importing employee details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetOnChangeRole(string role_gid, MdlHrmTrnAdmincontrol values)
        {
            try
            {
                msSQL = " select role_gid, role_name, probation_period from hrm_mst_trole " +
                        " where role_gid = '" + role_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getonchangerolelist>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getonchangerolelist
                        {
                            role_name = dt["role_name"].ToString(),
                            probation_period = dt["probation_period"].ToString(),
                        });
                        values.Getonchangerolelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Change Role!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/hrm/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public Dictionary<string, object> Dapersonaldatapdf(string employee_gid, MdlHrmTrnAdmincontrol values)
        {
            var response = new Dictionary<string, object>();
            string full_path = null;
            string lscompanycode, report_path, base_pathOF_currentFILE;

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            msSQL = " select a.bloodgroup,a.employee_qualification,a.employee_mobileno,a.ac_no,a.employee_photo," +
                                    " a.employee_gender,a.employee_sign,a.pf_no,a.esi_no,a.identity_no," +
                                    " date_format(a.employee_dob,'%d-%m-%Y') as employee_dob,f.remarks as left_reason, " +
                                    " date_format(a.employee_joiningdate,'%d-%m-%Y') as employee_joiningdate," +
                                    " concat(b.user_firstname,' ',b.user_lastname) as emp_name,c.designation_name," +
                                    " d.department_name, e.user_code,date_format(Now(),'%d-%m-%Y')as today_date,g.gross_salary as earned_gross_salary " +
                                    " from hrm_mst_temployee a" +
                                    " left join adm_mst_tuser b on a.user_gid = b.user_gid" +
                                    " left join adm_mst_tdesignation c on a.designation_gid=c.designation_gid" +
                                    " left join hrm_mst_tdepartment d on a.department_gid=d.department_gid" +
                                    " left join adm_mst_tuser e on a.user_gid = e.user_gid" +
                                    " left join hrm_trn_texitemployee f on a.employee_gid=f.employee_gid " +
                                    " left join pay_trn_temployee2salarygradetemplate g on a.employee_gid=g.employee_gid " +
                                    " where a.employee_gid= '" + employee_gid + "' ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1"); //employee

            msSQL = "SELECT a.company_name, a.company_address,a.company_logo_path as company_logo, a.company_phone, a.company_mail, a.contact_person " +
           "FROM adm_mst_tcompany a ";
            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable2.Columns.Add("company_logo", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));
                    if (System.IO.File.Exists(company_logo_path) == true)
                    {
                        //Convert  Image Path to Byte

                        company_logo = System.Drawing.Image.FromFile(company_logo_path);

                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));

                        DataTable2.Rows.Add(bytes);
                    }
                }
            }
            dt1.Dispose();
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");


            msSQL = " Select concat_ws('\n', a.address1,a.address2,a.city,a.state) as adr1" +
                                    " from adm_mst_taddress a" +
                                    " where" +
                                    " a.parent_gid = '" + employee_gid + "' and address_type = 'Permanent' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3"); //permenant

            msSQL = " Select concat_ws('\n',a.address1,a.address2,a.city,a.state) as address1" +
                                    " from adm_mst_taddress a" +
                                    " where " +
                                    "  a.parent_gid = '" + employee_gid + "' and a.address_type = 'Temporary' ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable4"); //temporary



            msSQL = " select company_name from adm_mst_tcompany ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable6"); //company

            //msSQL = "select c.employee_gid,c.ctc from hrm_mst_temployee a" +
            //                         " inner join pay_trn_temployee2salarygradetemplate c on c.employee_gid=a.employee_gid " +
            //                         " where c.employee_gid= '" +employee_gid+ "' ";
            //MyCommand.CommandText = msSQL;
            //MyCommand.CommandType = System.Data.CommandType.Text;
            //MyDA.SelectCommand = MyCommand;
            //myDS.EnforceConstraints = false;
            //MyDA.Fill(myDS, "DataTable7"); //salary


            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "hrm_rpt_personaldatapdfwithdata.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_file_path_hrm"].ToString(), "PersonalDetails_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }

        //public Dictionary<string, object> Daformapdf(string employee_gid, MdlHrmTrnAdmincontrol values)
        //{
        //    var response = new Dictionary<string, object>();
        //    string full_path = null;
        //    string lscompanycode, report_path, base_pathOF_currentFILE;

        //    OdbcConnection myConnection = new OdbcConnection();
        //    myConnection.ConnectionString = objdbconn.GetConnectionString();
        //    OdbcCommand MyCommand = new OdbcCommand();
        //    MyCommand.Connection = myConnection;
        //    DataSet myDS = new DataSet();
        //    OdbcDataAdapter MyDA = new OdbcDataAdapter();
        //}

        public string GetDateFormat(string lsdate)
        {


            DateTime Date;
            string[] formats = { "dd/MM/yyyy","dd-MM-yyyy","yyyy-MM-dd",
                                 "dd/M/yyyy","dd-M-yyyy","yyyy/MM/dd",
                                 "d/M/yyyy", "d-M-yyyy","yyyy-dd-MM",
                                 "d/MM/yyyy","d-MM-yyyy","yyyy/dd/MM",
                                 "dd/MM/yy", "dd-MM-yy",
                                 "dd/M/yy","dd-M-yy",
                                 "d/M/yy", "d-M-yy",
                                 "d/MM/yy", "d-MM-yy",
                                 "MMM/dd/yyyy","MMM-dd-yyyy",
                                 "MMM/d/yy","MMM-d-yy",
                                 "MMM/dd/yy","MMM-dd-yy",
                                 "MM/dd/yyyy","MM-dd-yyyy",
                                 "M/dd/yyyy","M-dd-yyyy",
                                 "M/d/yyyy","M-d-yyyy",
                                 "yyyy-MM-dd h:mm:ss tt","yyyy/MM/dd h:mm:ss tt",
                                 "yyyy-MM-dd hh:mm:ss tt","yyyy/MM/dd hh:mm:ss tt",
                                 "yyyy-MM-dd HH:mm:ss tt","yyyy/MM/dd HH:mm:ss tt",
                                 "yyyy-MM-ddTHH:mm: ss","yyyy/MM/ddTHH:mm: ss",
                                 "yyyy-dd-MM h:mm:ss tt","yyyy/dd/MM h:mm:ss tt",
                                 "yyyy-dd-MM hh:mm:ss tt","yyyy/dd/MM hh:mm:ss tt",
                                 "yyyy-dd-MM HH:mm:ss tt","yyyy/dd/MM HH:mm:ss tt",
                                 "yyyy-dd-MMTHH:mm: ss","yyyy/dd/MMTHH:mm: ss",
                                 "yyyy-mm-dd HH:MM:SS tt","yyyy/mm/dd HH:MM:SS tt",
                                 "yyyy-mm-dd hh:mm:ss","yyyy/mm/dd hh:mm:ss",
                                 "M/d/yyyy h:mm:ss tt","M-d-yyyy h:mm:ss tt",
                                 "M/d/yyyy hh:mm:ss tt","M-d-yyyy hh:mm:ss tt",
                                 "M/d/yyyy HH:mm:ss tt","M-d-yyyy HH:mm:ss tt",
                                 "M/d/yyyy HH:MM:ss tt","M-d-yyyy HH:MM:ss tt",
                                 "M/d/yyyy HH:MM:SS tt","M-d-yyyy HH:MM:SS tt",
                                 "M/d/yyyyTHH:mm: ss","M-d-yyyyTHH:mm: ss",
                                 "d/M/yyyy h:mm:ss tt","d-M-yyyy h:mm:ss tt",
                                 "d/M/yyyy hh:mm:ss tt","d-M-yyyy hh:mm:ss tt",
                                 "d/M/yyyy HH:mm:ss tt","d-M-yyyy HH:mm:ss tt",
                                 "d/M/yyyy HH:MM:ss tt","d-M-yyyy HH:MM:ss tt",
                                 "d/M/yyyy HH:MM:SS tt","d-M-yyyy HH:MM:SS tt",
                                 "d/M/yyyyTHH:mm: ss","d-M-yyyyTHH:mm: ss",
                                 "MM/d/yyyy h:mm:ss tt","MM-d-yyyy h:mm:ss tt",
                                 "MM/d/yyyy hh:mm:ss tt","MM-d-yyyy hh:mm:ss tt",
                                 "MM/d/yyyy HH:mm:ss tt","MM-d-yyyy HH:mm:ss tt",
                                  "MM/d/yyyy HH:MM:ss tt","MM-d-yyyy HH:MM:ss tt",
                                   "MM/d/yyyy HH:MM:SS tt","MM-d-yyyy HH:MM:SS tt",
                                 "MM/d/yyyyTHH:mm: ss","MM-d-yyyyTHH:mm: ss",
                                 "M/dd/yyyy h:mm:ss tt","M-dd-yyyy h:mm:ss tt",
                                 "M/dd/yyyy hh:mm:ss tt","M-dd-yyyy hh:mm:ss tt",
                                 "M/dd/yyyy HH:mm:ss tt","M-dd-yyyy HH:mm:ss tt",
                                 "M/dd/yyyy HH:MM:ss tt","M-dd-yyyy HH:MM:ss tt",
                                 "M/dd/yyyy HH:MM:SS tt","M-dd-yyyy HH:MM:SS tt",
                                 "M/dd/yyyyTHH:mm: ss","M-dd-yyyyTHH:mm: ss",
                                 "MM/dd/yyyy h:mm:ss tt","MM-dd-yyyy h:mm:ss tt",
                                 "MM/dd/yyyy hh:mm:ss tt","MM-dd-yyyy hh:mm:ss tt",
                                 "MM/dd/yyyy HH:mm:ss tt","MM-dd-yyyy HH:mm:ss tt",
                                 "MM/dd/yyyy HH:MM:ss tt","MM-dd-yyyy HH:MM:ss tt",
                                 "MM/dd/yyyy HH:MM:SS tt","MM-dd-yyyy HH:MM:SS tt",
                                 "MM/dd/yyyyTHH:mm: ss","MM-dd-yyyyTHH:mm: ss",
                                 "dd/MM/yyyy h:mm:ss tt","dd-MM-yyyy h:mm:ss tt",
                                 "dd/MM/yyyy hh:mm:ss tt","dd-MM-yyyy hh:mm:ss tt",
                                 "dd/MM/yyyy HH:mm:ss tt","dd-MM-yyyy HH:mm:ss tt",
                                 "dd/MM/yyyy HH:MM:ss tt","dd-MM-yyyy HH:MM:ss tt",
                                 "dd/MM/yyyy HH:MM:SS tt","dd-MM-yyyy HH:MM:SS tt",
                                 "dd/MM/yyyyTHH:mm: ss","dd-MM-yyyyTHH:mm: ss",
                                 "dd-M-yyyy h:mm:ss tt" ,"d-MM-yyyy h:mm:ss tt",
                                 "dd-M-yyyy hh:mm:ss tt" ,"d-MM-yyyy hh:mm:ss tt",
                                 "dd-M-yyyy HH:mm:ss tt" ,"d-MM-yyyy HH:mm:ss tt",
                                 "dd-M-yyyy HH:MM:ss tt" ,"d-MM-yyyy HH:MM:ss tt",
                                 "dd-M-yyyy HH:MM:SS tt" ,"d-MM-yyyy HH:MM:SS tt",
                                 "dd-M-yyyyTHH:mm: ss" ,"d-MM-yyyyTHH:mm: ss",
                                 "dd/M/yyyy h:mm:ss tt","dd-MM-yy h:mm:ss tt",
                                 "dd/M/yyyy hh:mm:ss tt","dd-MM-yy hh:mm:ss tt",
                                 "dd/M/yyyy HH:mm:ss tt","dd-MM-yy HH:mm:ss tt",
                                "dd/M/yyyy HH:MM:ss tt","dd-MM-yy HH:MM:ss tt",
                                 "dd/M/yyyy HH:MM:SS tt","dd-MM-yy HH:MM:SS tt",
                                 "dd/M/yyyyTHH:mm: ss","dd-MM-yyTHH:mm: ss",
                                 "dd/MM/yy h:mm:ss tt","d/M/yyyy h:mm:ss",
                                 "dd/MM/yy hh:mm:ss tt","d/M/yyyy hh:mm:ss",
                                 "dd/MM/yy HH:mm:ss tt","d/M/yyyy HH:mm:ss",
                                "dd/MM/yy HH:MM:ss tt","d/M/yyyy HH:MM:ss",
                                 "dd/MM/yy HH:MM:SS tt","d/M/yyyy HH:MM:SS tt",
                                 "dd/MM/yyTHH:mm: ss","d/M/yyyyTHH:mm: ss",
                                 "d-M-yyyy h:mm:ss","dd/MM/yyyy h:mm:ss",
                                 "d-M-yyyy hh:mm:ss","dd/MM/yyyy hh:mm:ss",
                                 "d-M-yyyy HH:mm:ss","dd/MM/yyyy HH:mm:ss",
                                 "d-M-yyyy HH:MM:ss tt","dd/MM/yyyy HH:mm:ss tt",
                                 "d-M-yyyy HH:MM:SS tt","dd/MM/yyyy HH:MM:SS tt",
                                 "d-M-yyyyTHH:mm: ss","dd/MM/yyyyTHH:mm: ss",
                                 "dd-MM-yyyy h:mm:ss","dd-M-yyyy h:mm:ss" ,
                                 "dd-MM-yyyy hh:mm:ss","dd-M-yyyy hh:mm:ss" ,
                                 "dd-MM-yyyy HH:mm:ss","dd-M-yyyy HH:mm:ss" ,
                                 "dd-MM-yyyy HH:MM:ss","dd-M-yyyy HH:MM:ss" ,
                                 "dd-MM-yyyyTHH:mm: ss","dd-M-yyyyTHH:mm: ss" ,
                                 "d-MM-yyyy h:mm:ss","dd/M/yyyy h:mm:ss",
                                 "d-MM-yyyy hh:mm:ss","dd/M/yyyy hh:mm:ss",
                                 "d-MM-yyyy HH:mm:ss","dd/M/yyyy HH:mm:ss",
                                 "d-MM-yyyy HH:MM:ss","dd/M/yyyy HH:MM:ss",
                                 "d-MM-yyyyTHH:mm: ss","dd/M/yyyyTHH:mm: ss",
                                 "dd-MM-yy h:mm:ss","dd/MM/yy h:mm:ss",
                                 "dd-MM-yy hh:mm:ss","dd/MM/yy hh:mm:ss",
                                 "dd-MM-yy HH:mm:ss","dd/MM/yy HH:mm:ss",
                                 "dd-MM-yy HH:MM:ss","dd/MM/yy HH:MM:ss",
                                 "dd-MM-yy HH:MM:SS","dd/MM/yy HH:MM:SS",
                                 "dd-MM-yyTHH:mm: ss","dd/MM/yyTHH:mm: ss"

            };
            DateTime.TryParseExact(lsdate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date);
            lsconverted_date = Convert.ToDateTime(Date).ToString("yyyy-MM-dd");


            if (lsconverted_date == "0001-01-01")
            {
                //double excelDateValue = Convert.ToDouble(lsdate);
                //DateTime Date = DateTime.FromOADate(excelDateValue);
                //string lsdatetime = Convert.ToString(Date);

                DateTime dateTime = DateTime.FromOADate(double.Parse(lsdate));
                string formattedDate = dateTime.ToString("MM/dd/yyyy");
                lsconverted_date = Convert.ToDateTime(formattedDate).ToString("yyyy-MM-dd");


            }
            return lsconverted_date;

        }
    }
}