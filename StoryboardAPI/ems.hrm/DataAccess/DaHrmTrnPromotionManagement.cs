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

namespace ems.hrm.DataAccess
{
    public class DaHrmTrnPromotionManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL2 = string.Empty;

        OdbcDataReader objMySqlDataReader, objMySqlDataReader2;


        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        DataTable dt_datatable;
        int mnResult, mnResult2, mnResult3;
        string msGetGid, msGetGid2, experience_gid, msGetGid1, lsContent, xDate, xDate1, lsduration, lsdate, lsjoining_date, lsexit_date, lsempoyeegid, msTemporaryAddressGetGID, lsemployee2salarygradetemplate_gid, msGetemployeetype, msPermanentAddressGetGID, msgetshift, lssalarycomponent_name, lssalarycomponent_gid, msUserGid, msEmployeeGID;
        double lsday, lsyear, lsmonth;
        DataTable dt1 = new DataTable();
        DataTable DataTable2 = new DataTable();
        string company_logo_path;
        System.Drawing.Image company_logo;

        public void DaGetPromotionSummary(MdlHrmTrnPromotionManagement values)
        {
            try
            {

                msSQL = " Select distinct a.promotion_gid,concat(c.user_code,' | ',' ',c.user_firstname,' ',c.user_lastname) as user_firstname,a.promotion_effectivedate, date_format(a.promotedd_date, '%d-%m-%Y') as promotedd_date, date_format(a.created_date, '%d-%m-%Y') as created_date," +
                        " (Select designation_name from adm_mst_tdesignation e where e.designation_gid = a.current_designation) as currentdesignation, " +
                        " (Select designation_name from adm_mst_tdesignation e where e.designation_gid = a.previous_designation) as previousdesignation, " +
                        " (Select branch_name from hrm_mst_tbranch y where y.branch_gid = a.current_branch) as currentbranch, " +
                        " (Select branch_name from hrm_mst_tbranch y where y.branch_gid = a.previous_branch) as perviousbranch, " +
                        " (Select department_name from hrm_mst_tdepartment z where z.department_gid = a.current_department) as currentdepartment, " +
                        " (Select department_name from hrm_mst_tdepartment z where z.department_gid = a.previous_department) as perviousdepartment, " +
                        " (select a.user_firstname from adm_mst_tuser a " +
                        " left join  hrm_mst_temployee b on a.user_gid = b.user_gid where a.promotion_approvedby = b.employee_gid) as approveby_name,a.employee_gid, " +
                        " e.promotion_flag,f.branch_name, g.department_name , c.user_firstname " +
                        " from hrm_trn_tpromotion a left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " left join adm_mst_tdesignation d on  b.designation_gid = d.designation_gid " +
                        " left join hrm_mst_temployeedocument e on e.employee_gid = a.employee_gid " +
                        " left join hrm_mst_tbranch f on b.branch_gid = f.branch_gid " +
                        " left join hrm_mst_tdepartment g on b.department_gid = g.department_gid " +
                        " where c.user_status = 'Y' and promotion_status='Promoted' order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Promotionsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Promotionsummary_list
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
                        values.Promotionsummarylist = getModuleList;
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

        public void DaGetEmployeeNameDtl(MdlHrmTrnPromotionManagement values)
        {
            try
            {
                msSQL = " SELECT a.user_gid, a.employee_gid, CONCAT(b.user_code, '/', b.user_firstname, ' ', b.user_lastname) AS employee_name " +
                        " FROM hrm_mst_temployee a " +
                        " INNER JOIN adm_mst_tuser b ON b.user_gid = a.user_gid " +
                        " INNER JOIN hrm_mst_tbranch c ON c.branch_gid = a.branch_gid " +
                        " WHERE b.user_status = 'Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeedtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeedtldropdown
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetEmployee_Dtl = getModuleList;
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

        public void DaGetBranchNameDtl(MdlHrmTrnPromotionManagement values)
        {
            try
            {
                msSQL = " Select branch_name,branch_gid  " +
                        " from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getbranchdtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getbranchdtldropdown
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetBranch_Dtl = getModuleList;
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

        public void DaGetDepartmentNameDtl(MdlHrmTrnPromotionManagement values)
        {
            try
            {
                msSQL = " Select department_name,department_gid  " +
                        " from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdepartmentdtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdepartmentdtldropdown
                        {
                            department_name = dt["department_name"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                        });
                        values.GetDepartment_Dtl = getModuleList;
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

        public void DaGetDesignationNameDtl(MdlHrmTrnPromotionManagement values)
        {
            try
            {
                msSQL = " Select designation_name,designation_gid  " +
                        " from adm_mst_tdesignation ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Getdesignationdtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getdesignationdtldropdown
                        {
                            designation_name = dt["designation_name"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                        });
                        values.GetDesignation_Dtl = getModuleList;
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

        public void DaGetEmployeeDetail(string employee_gid, MdlHrmTrnPromotionManagement values)
        {
            try
            {
                msSQL = " select e.branch_gid, e.employee_gid, b.branch_name, d.department_name, f.designation_name from hrm_mst_temployee e " +
                        " LEFT JOIN hrm_mst_tbranch b ON e.branch_gid = b.branch_gid " +
                        " LEFT JOIN hrm_mst_tdepartment d ON e.department_gid = d.department_gid " +
                        " LEFT JOIN adm_mst_tdesignation f ON e.designation_gid = f.designation_gid " +
                " where e.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeeDataDetail>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeeDataDetail
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                        });
                        values.GetEmployeeData_Detail = getModuleList;
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

        public void DaPostPromotion(string employee_gid, Promotionsummary_list values)
        {
            try
            {

                msSQL = " SELECT employee_gid  FROM " +
                        " hrm_trn_tpromotion WHERE employee_gid = '" + values.employeegid + "'";
                DataTable dt_datatable2 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable2.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Already Promoted";
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
                "'Promoted'," +
                "'" + employee_gid + "', " +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Promotion Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Promotion";
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
                        values.message = "Promotion Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Promotion";
                    }
                    values.status = true;
                    values.message = "Promotion History Details Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Promotion History";
                }
            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaDeletePromotion(string promotion_gid, Promotionsummary_list values)
        {
            try
            {
                msSQL = "  delete from hrm_trn_tpromotion  where promotion_gid ='" + promotion_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Promotion Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Promotion";
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

        public void DaGetPromotionHistorySummary(string employee_gid, MdlHrmTrnPromotionManagement values)
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

                var getModuleList = new List<PromotionHistorysummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PromotionHistorysummary_list
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
                        values.PromotionHistorysummary_list = getModuleList;
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