using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using MySqlX.XDevAPI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Web.UI.WebControls;
using Mysqlx;
using System.Web.Http.Controllers;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Math;

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnDePromote
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL2 = string.Empty;

        OdbcDataReader objMySqlDataReader, objMySqlDataReader2;


        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable dt_datatable;
        int mnResult, mnResult2, mnResult3;
        string msGetGid, msGetGid2, lsemployee_gid, experience_gid, msGetGid1, lsContent, xDate, xDate1, lsduration, lsdate, lsjoining_date, lsexit_date, lsempoyeegid, msTemporaryAddressGetGID, lsemployee2salarygradetemplate_gid, msGetemployeetype, msPermanentAddressGetGID, msgetshift, lssalarycomponent_name, lssalarycomponent_gid, msUserGid, msEmployeeGID;
        double lsday, lsyear, lsmonth;
        DataTable dt1 = new DataTable();
        DataTable DataTable2 = new DataTable();
        string company_logo_path;
        System.Drawing.Image company_logo;

        public void DaGetDePromoteSummary(MdlHrmTrnDePromote values)
        {
            try
            {

                msSQL = " SELECT DISTINCT a.promotion_gid, CONCAT(c.user_code, ' | ', c.user_firstname, ' ', c.user_lastname) AS user_firstname, " +
                        " a.promotion_effectivedate, DATE_FORMAT(a.promotedd_date, '%d-%m-%Y') AS promotedd_date, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date, " +
                        " (SELECT designation_name FROM adm_mst_tdesignation e WHERE e.designation_gid = a.current_designation) AS currentdesignation, " +
                        " (SELECT designation_name FROM adm_mst_tdesignation e WHERE e.designation_gid = a.previous_designation) AS previousdesignation, " +
                        " (SELECT branch_name FROM hrm_mst_tbranch y WHERE y.branch_gid = a.current_branch) AS currentbranch, " +
                        " (SELECT branch_name FROM hrm_mst_tbranch y WHERE y.branch_gid = a.previous_branch) AS perviousbranch, " +
                        " (SELECT department_name FROM hrm_mst_tdepartment z WHERE z.department_gid = a.current_department) AS currentdepartment, " +
                        " (SELECT department_name FROM hrm_mst_tdepartment z WHERE z.department_gid = a.previous_department) AS perviousdepartment, " +
                        " (SELECT a.user_firstname FROM adm_mst_tuser a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.user_gid = b.user_gid " +
                        " WHERE a.promotion_approvedby = b.employee_gid) AS approveby_name, " +
                        " a.employee_gid, e.promotion_flag, f.branch_name, g.department_name, c.user_firstname " +
                        " FROM hrm_trn_tpromotion a " +
                        " LEFT JOIN hrm_mst_temployee b ON a.employee_gid = b.employee_gid " +
                        " LEFT JOIN adm_mst_tuser c ON c.user_gid = b.user_gid " +
                        " LEFT JOIN adm_mst_tdesignation d ON b.designation_gid = d.designation_gid " +
                        " LEFT JOIN hrm_mst_temployeedocument e ON e.employee_gid = a.employee_gid " +
                        " LEFT JOIN hrm_mst_tbranch f ON b.branch_gid = f.branch_gid " +
                        " LEFT JOIN hrm_mst_tdepartment g ON b.department_gid = g.department_gid " +
                        " WHERE c.user_status = 'Y' and promotion_status='DePromoted'" +
                        " ORDER BY a.created_date DESC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<DePromotionsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DePromotionsummary_list
                        {
                            promotion_gid = dt["promotion_gid"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            promotion_effectivedate = dt["promotion_effectivedate"].ToString(),
                            promotedd_date = dt["promotedd_date"].ToString(),
                            currentdesignation = dt["currentdesignation"].ToString(),
                            previousdesignation = dt["previousdesignation"].ToString(),
                            currentbranch = dt["currentbranch"].ToString(),
                            perviousbranch = dt["perviousbranch"].ToString(),
                            currentdepartment = dt["currentdepartment"].ToString(),
                            perviousdepartment = dt["perviousdepartment"].ToString(),
                            approveby_name = dt["approveby_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            promotion_flag = dt["promotion_flag"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.DePromotionsummary_list = getModuleList;
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

        public void DaGetEmployeeNameDtl(MdlHrmTrnDePromote values)
        {
            try
            {
                msSQL = " SELECT a.user_gid, a.employee_gid, CONCAT(b.user_code, '/', b.user_firstname, ' ', b.user_lastname) AS employee_name " +
                        " FROM hrm_mst_temployee a " +
                        " INNER JOIN adm_mst_tuser b ON b.user_gid = a.user_gid " +
                        " INNER JOIN hrm_mst_tbranch c ON c.branch_gid = a.branch_gid " +
                        " WHERE b.user_status = 'Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeenameddtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeenameddtldropdown
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetEmployeeuser_Dtl = getModuleList;
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

        public void DaGetBranchNameDtl(MdlHrmTrnDePromote values)
        {
            try
            {
                msSQL = " Select branch_name,branch_gid  " +
                        " from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbranchnameddtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getbranchnameddtldropdown
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetBranchuser_Dtl = getModuleList;
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

        public void DaGetDepartmentNameDtl(MdlHrmTrnDePromote values)
        {
            try
            {
                msSQL = " Select department_name,department_gid  " +
                        " from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdepartmentnameddtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdepartmentnameddtldropdown
                        {
                            department_name = dt["department_name"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.GetDepartmentuser_Dtl = getModuleList;
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

        public void DaGetDesignationNameDtl(MdlHrmTrnDePromote values)
        {
            try
            {
                msSQL = " Select designation_name,designation_gid  " +
                        " from adm_mst_tdesignation ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdesignationnameddtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdesignationnameddtldropdown
                        {
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                        });
                        values.GetDesignationuser_Dtl = getModuleList;
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

        public void DaGetEmployeeDetail(string employee_gid, MdlHrmTrnDePromote values)
        {
            try
            {
                msSQL = " select e.branch_gid, e.employee_gid, b.branch_name, d.department_name, f.designation_name from hrm_mst_temployee e " +
                        " LEFT JOIN hrm_mst_tbranch b ON e.branch_gid = b.branch_gid " +
                        " LEFT JOIN hrm_mst_tdepartment d ON e.department_gid = d.department_gid " +
                        " LEFT JOIN adm_mst_tdesignation f ON e.designation_gid = f.designation_gid " +
                " where e.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeeUserDataDetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeeUserDataDetail
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                        });
                        values.GetEmployeeUserData_Detail = getModuleList;
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

        //public void DaGetEmployeeDtl(MdlHrmTrnDePromote values)
        //{
        //    try
        //    {
        //        msSQL = " SELECT a.user_gid, a.employee_gid, CONCAT(b.user_code, '/', b.user_firstname, ' ', b.user_lastname) AS employee_name " +
        //                " FROM hrm_mst_temployee a " +
        //                " INNER JOIN adm_mst_tuser b ON b.user_gid = a.user_gid " +
        //                " INNER JOIN hrm_mst_tbranch c ON c.branch_gid = a.branch_gid " +
        //                " WHERE b.user_status = 'Y' ";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<GetEmployeedetaildatadropdown>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {

        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new GetEmployeedetaildatadropdown
        //                {
        //                    employee_gid = dt["employee_gid"].ToString(),
        //                    employee_name = dt["employee_name"].ToString(),
        //                });
        //                values.GetEmployee_Dtl = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }

        //}

        public void DaPostDePromotion(string employee_gid, DePromotionsummary_list values)
        {
            try
            {
                msSQL = " SELECT employee_gid  FROM " +
                        " hrm_trn_tpromotion WHERE employee_gid = '" + values.employeegid + "'";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Already De-Promoted";
                    return;
                }
                string uiDateStr = values.effective_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqleffectivedate = uiDate.ToString("yyyy-MM-dd");

                msGetGid = objcmnfunctions.GetMasterGID("HPMP");
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = "Error Occured While Generating Gid";
                }

                msGetGid2 = objcmnfunctions.GetMasterGID("HPMP");
                if (msGetGid2 == "E")
                {
                    values.status = false;
                    values.message = "Error Occured While Generating Gid";
                }

                msSQL = " Insert into hrm_trn_tpromotion (" +
                     " promotion_gid , " +
                     " employee_gid, " +
                     " promotedd_date," +
                     " current_designation,  " +
                     " previous_designation, " +
                     " promotion_remarks, " +
                     " promotion_approvedby, " +
                     " current_branch,  " +
                     " previous_branch, " +
                     " current_department,  " +
                     " previous_department, " +
                     " promotion_status, " +
                     " created_by," +
                     " created_date) " +
                     " values (" +
                     "'" + msGetGid + "'," +
                     "'" + values.employeegid + "'," +
                     "'" + mysqleffectivedate + "'," +
                     "'" + values.designation_detail + "'," +
                     "'" + values.designation_name + "'," +
                     "'" + values.reason + "'," +
                     "'" + employee_gid + "', " +
                     "'" + values.branch_detail + "'," +
                "'" + values.branch_name + "'," +
                "'" + values.department_detail + "'," +
                "'" + values.department_name + "'," +
                "'DePromoted'," +
                "'" + employee_gid + "', " +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "De-Promotion Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding De-Promotion";
                }

                msSQL2 = "insert into hrm_trn_tpromotionhistory (" +
                       " promotionhistory_gid , " +
                       " employee_gid , " +
                       " designation_gid , " +
                       " branch_gid , " +
                       " department_gid, " +
                       " created_date , " +
                       " from_date," +
                       " created_by) " +
                       " values ( " +
             "'" + msGetGid2 + "'," +
             "'" + values.employeegid + "'," +
             "'" + values.designation_detail + "'," +
             "'" + values.branch_detail + "'," +
             "'" + values.department_detail + "'," +
             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
             "'" + mysqleffectivedate + "'," +
             "'" + employee_gid + "') ";
                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL2);

                if (mnResult2 != 0)
                {
                    msSQL = " update hrm_mst_temployee set branch_gid='" + values.branch_detail + "',designation_gid='" + values.designation_detail + "',department_gid='" + values.department_detail + "'" +
                            " where employee_gid='" + values.employeegid + "'";
                    mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult3 != 0)
                    {
                        values.status = true;
                        values.message = "De-Promotion Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating De-Promotion";
                    }
                    values.status = true;
                    values.message = "De-Promotion History Details Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding De-Promotion History";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        //public void DagetUpdatedDePromotion(string user_gid, DePromotion_list values)
        //{
        //    try
        //    {
        //        msSQL = " select a.employee_gid, concat(user_firstname, ' ', user_lastname) as employee_name " +
        //                   " from hrm_mst_temployee a " +
        //                   " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
        //                   " where a.employee_gid='" + values.employee_nameedit + "'";
        //        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
        //        if (objMySqlDataReader.HasRows)
        //        {
        //            objMySqlDataReader.Read();
        //            lsemployee_gid = objMySqlDataReader["employee_gid"].ToString();
        //        }

        //        msSQL = " update hrm_trn_tpenality set  " +
        //           " employee_gid ='" + lsemployee_gid + "'," +
        //           " project_gid ='" + values.project_nameedit + "'," +
        //           " reason ='" + values.reason_edit + "'," +
        //           " updated_by = '" + user_gid + "'," +
        //           " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
        //           " where penality_gid ='" + values.penality_gid + "' ";
        //        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //        if (mnResult != 0)
        //        {
        //            values.status = true;
        //            values.message = "De-Promotion Updated Successfully";
        //        }
        //        else
        //        {
        //            values.status = false;
        //            values.message = "Error While Updating De-Promotion";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.status = false;

        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //         "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }

        //}

        public void DaDeletedepromotion(string promotion_gid, DePromotionsummary_list values)
        {
            try
            {
                msSQL = "  delete from hrm_trn_tpromotion where promotion_gid='" + promotion_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "De-Promotion Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting De-Promotion";
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

        public void DaGetDePromotionHistorySummary(string employee_gid, MdlHrmTrnDePromote values)
        {
            try
            {
                msSQL = " select a.employee_gid,a.designation_gid,a.branch_gid,b.branch_name,d.designation_name,CONCAT(u.user_code, ' / ', u.user_firstname, ' ', u.user_lastname) AS username,date_format(a.from_date, '%d-%m-%Y') as from_date, " +
                        " case when a.to_date is null then 'Till Now' else cast(date_format(a.to_date,'%d-%m-%Y')as char) end as to_date," +
                " a.salarygrade_gid,z.department_name,y.salarygradetemplate_name,k.promotedd_date from hrm_trn_tpromotionhistory a " +
                " left join hrm_mst_temployee e on e.employee_gid=a.employee_gid " +
                " left join  hrm_mst_tbranch b on b.branch_gid=a.branch_gid " +
                " left join adm_mst_tdesignation d on d.designation_gid=a.designation_gid " +
                " left join hrm_mst_tdepartment z on z.department_gid=a.department_gid " +
                " left join pay_trn_temployee2salarygradetemplate s on s.employee_gid=e.employee_gid " +
                " left join pay_mst_tsalarygradetemplate y on y.salarygradetemplate_gid=a.salarygrade_gid " +
                " left join adm_mst_tuser u on u.user_gid=e.user_gid " +
                " left join hrm_trn_tpromotion k on a.employee_gid=k.employee_gid " +
                " where e.employee_gid='" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            
                var getModuleList = new List<DePromotionHistorysummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DePromotionHistorysummary_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            username = dt["username"].ToString(),
                            from_date = dt["from_date"].ToString(),
                            to_date = dt["to_date"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            promotedd_date = dt["promotedd_date"].ToString(),

                        });
                        values.DePromotionHistorysummary_list = getModuleList;
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
