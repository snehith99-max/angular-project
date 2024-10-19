using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using ems.utilities.Functions;
using ems.hrm.Models;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Globalization;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnAppraisalManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        HttpPostedFile httpPostedFile;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1;
        string msEmployeeGID, lsempgid, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsdepartment_code, lsdepartment_name, lsdepartment_prefix, lsdepartment_name_edit;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetEmployeeDetail(MdlHrmTrnAppraisalManagement values)
        {
            try
            {


                msSQL = " select a.user_gid, a.user_firstname, a.user_lastname, concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_code from adm_mst_tuser a ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeeDetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeeDetail
                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                        });
                        values.GetEmployeeDetail = getModuleList;

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

        public void DaGetAppraisalSummary(MdlHrmTrnAppraisalManagement values)
        {
            try
            {
                msSQL = " select a.appraisal_gid,b.employee_gid,b.user_gid, a.user_name, a.emp_designation, a.emp_department, a.emp_branch," +
                        " date_format(a.created_date, '%d-%m-%Y') as created_date, a.created_by from hrm_trn_tappraisalmanage a" +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                        " order by appraisal_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<review_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new review_list
                        {
                            appraisal_gid = dt["appraisal_gid"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            emp_firstname = dt["user_name"].ToString(),
                            emp_designation = dt["emp_designation"].ToString(),
                            emp_department = dt["emp_department"].ToString(),
                            emp_branch = dt["emp_branch"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),


                        });
                        values.reviewlist = getModuleList;
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

        public bool DaUserList(string user_gid, MdlHrmTrnAppraisalManagement values)
        {
            try
            {

                msSQL = " SELECT CONCAT(a.user_firstname,' ',a.user_lastname) AS user_name,b.employee_gid,a.user_firstname,a.user_lastname,b.employee_photo,b.employee_mobileno,b.employee_emailid,date_format(b.employee_dob,'%d-%m-%Y') as dob," +
                " c.branch_name,d.department_name,e.designation_name FROM adm_mst_tuser a " +
                " LEFT JOIN hrm_mst_temployee b on a.user_gid=b.user_gid " +
                " LEFT JOIN hrm_mst_tbranch c on c.branch_gid=b.branch_gid " +
                " LEFT JOIN hrm_mst_tdepartment d on d.department_gid=b.department_gid " +
                " LEFT JOIN adm_mst_tdesignation e on e.designation_gid=b.designation_gid WHERE a.user_gid = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<username_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new username_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_lastname = dt["user_lastname"].ToString(),
                            dob = dt["dob"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),

                        });
                    }
                    values.usernamelist = getModuleList;
                }
                dt_datatable.Dispose();
                values.status = true;
                return true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }

       
        public void DaGetPeerDetail(string user_gid, MdlHrmTrnAppraisalManagement values)
        {
            try
            {
                msSQL = " select distinct a.employee_gid,b.user_gid,concat(b.user_firstname,' / ',d.designation_name,' / ',e.department_name) as user_name, c.module2employee_gid, c.hierarchy_level  from hrm_mst_temployee a " +
                    " left join adm_mst_tuser b on a.user_gid = b.user_gid" +
                    " left join adm_mst_tmodule2employee c on a.employee_gid=c.employee_gid " +
                     " left join adm_mst_tdesignation d on a.designation_gid = d.designation_gid" +
                    " left join hrm_mst_tdepartment e on a.department_gid = e.department_gid" +
                    " where c.hierarchy_level = '6' and   not a.employee_gid='" + user_gid + "'group by  employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPeerDetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPeerDetail
                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                        });

                    }
                    values.GetPeer_Detail = getModuleList;
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

        public void DaGetManagerDetail(string user_gid, MdlHrmTrnAppraisalManagement values)
        {
            try
            {
                msSQL = " select a.employee_gid,b.user_gid,concat(b.user_firstname,' / ',d.designation_name,' / ',e.department_name) as user_name from hrm_mst_temployee a " +
                    " left join adm_mst_tuser b on a.user_gid = b.user_gid" +
                    " left join adm_mst_tmodule2employee c on a.employee_gid=c.employee_gid " +
                    " left join adm_mst_tdesignation d on a.designation_gid = d.designation_gid" +
                    " left join hrm_mst_tdepartment e on a.department_gid = e.department_gid" +
                    " where c.hierarchy_level = '4' and   not a.employee_gid='" + user_gid + "' group by  employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagerDetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagerDetail
                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                        });

                    }
                    values.GetManager_Detail = getModuleList;
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

        public void DaGetManagementDetail(string user_gid, MdlHrmTrnAppraisalManagement values)
        {
            try
            {

                msSQL = " select a.employee_gid,b.user_gid,concat(b.user_firstname,' / ',d.designation_name,' / ',e.department_name) as user_name, c.module2employee_gid, c.hierarchy_level  from hrm_mst_temployee a " +
                    " left join adm_mst_tuser b on a.user_gid = b.user_gid" +
                    " left join adm_mst_tmodule2employee c on a.employee_gid=c.employee_gid " +
                    " left join adm_mst_tdesignation d on a.designation_gid = d.designation_gid" +
                    " left join hrm_mst_tdepartment e on a.department_gid = e.department_gid" +
                    " where c.hierarchy_level = '2' and   not a.employee_gid='" + user_gid + "' group by  employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagementDetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagementDetail
                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                        });

                    }
                    values.GetManagement_Detail = getModuleList;
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

        public void DaPostReview(string user_gid, review_list values)
        {
            try
            {
                msSQL = " SELECT user_name  FROM " +
                        " hrm_trn_tappraisalmanage WHERE user_name = '" + values.emp_firstname + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "User Name already Exist";

                }
                else
                {


                    string uiDateStr = values.emp_dob;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string mysqlremp_dob = uiDate.ToString("yyyy-MM-dd");

                    msGetGid = objcmnfunctions.GetMasterGID("HRAM");
                    if (msGetGid == "E")
                    {
                        values.status = false;
                        values.message = " Error Occured While Generating Gid ";
                    }

                    msSQL = " insert into hrm_trn_tappraisalmanage (" +
                        " appraisal_gid, " +
                        " employee_gid," +
                        " user_name, " +
                        " emp_designation, " +
                        " emp_department," +
                        " emp_branch," +
                        " emp_mob, " +
                        " emp_dob, " +
                        " self_review, " +
                        " peer_review, " +
                        " manager_review, " +
                        " management_review, " +
                        " created_by, " +
                        " created_date ) " +
                        " values (" +
                        "'" + msGetGid + "'," +
                        "'" + values.employee_gid + "'," +
                        "'" + values.emp_firstname + "'," +
                        "'" + values.emp_designation + "'," +
                        "'" + values.emp_department + "'," +
                        "'" + values.emp_branch + "'," +
                        "'" + values.emp_mobile + "'," +
                        "'" + mysqlremp_dob + "'," +
                        "'" + values.self_name + "'," +
                        "'" + values.peer_name + "'," +
                        "'" + values.manager_name + "'," +
                        "'" + values.management_name + "'," +
                         "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Appraisal Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Appraisal";
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

        public void DagetViewReviewSummary(string user_gid, MdlHrmTrnAppraisalManagement values)
        {
            try
            {
                msSQL = " select b.employee_gid, b.user_gid, a.appraisal_gid, concat(a.self_review,'  /  ' '', '' , a.peer_review,'  /  ' '', '', a.manager_review,'  /  ' '', '', a.management_review) as Reviewed_by from hrm_trn_tappraisalmanage a " +
                        " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                        " where b.user_gid =  '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ViewReviewSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ViewReviewSummary_list
                        {
                            Reviewed_by = dt["Reviewed_by"].ToString(),
                        });
                    }
                    values.ViewReviewSummarylist = getModuleList;

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

        public void DaPostAppraisalDtl(string user_gid, appraisaldtl_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("HREM");
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = " Error Occured While Generating Gid ";
                }
                msSQL = " insert into hrm_trn_tappraisal2employee (" +
                    " appraisal2employee_gid, " +
                    " employee_gid," +
                    " work_experience," +
                    " soft_skills, " +
                    " contribution, " +
                    " Reviewed_by, " +
                    " grade, " +
                    " revised_for, " +
                    " created_by, " +
                    " created_date) " +
                    " values (" +
                    "'" + msGetGid + "'," +
                    "'" + values.employee_gid + "'," +
                    "'" + values.experience + "'," +
                    "'" + values.softskills + "'," +
                    "'" + values.contribution + "'," +
                    "'" + values.Reviewed_by + "'," +
                    "'" + values.grade_no + "'," +
                    "'" + values.recommended_type + "'," +
                     "'" + user_gid + "', " +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Appraisal Details Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Appraisal Details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetAppraisalDetailSummary(string user_gid, MdlHrmTrnAppraisalManagement values)
        {
            try
            {
                msSQL = " select d.appraisal2employee_gid, b.employee_gid,b.user_gid, d.work_experience, d.soft_skills, d.contribution, d.Reviewed_by, " +
                        " date_format(d.created_date, '%d-%m-%Y') as created_date, d.grade,d.revised_for, d.created_by from hrm_trn_tappraisal2employee d " +
                        " left join hrm_mst_temployee b on d.employee_gid = b.employee_gid " +
                        " where b.user_gid = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<appraisaldtl_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new appraisaldtl_list
                        {
                            appraisal2employee_gid = dt["appraisal2employee_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            work_experience = dt["work_experience"].ToString(),
                            soft_skills = dt["soft_skills"].ToString(),
                            contribution = dt["contribution"].ToString(),
                            Reviewed_by = dt["Reviewed_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            grade = dt["grade"].ToString(),
                            revised_for = dt["revised_for"].ToString(),
                            created_by = dt["created_by"].ToString(),

                        });
                        values.appraisaldtllist = getModuleList;

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

        public void DaDeleteAppraisalDetail(string appraisal2employee_gid, appraisaldtl_list values)
        {
            try
            {

                msSQL = "  delete from hrm_trn_tappraisal2employee where appraisal2employee_gid='" + appraisal2employee_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Appraisal Detail Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Appraisal Detail";
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