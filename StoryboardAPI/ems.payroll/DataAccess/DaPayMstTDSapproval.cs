using System;
using System.Collections.Generic;
using System.Web;
using ems.payroll.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using File = System.IO.File;
using System.Data;
using System.Globalization;
using System.Data.Odbc;
using Org.BouncyCastle.Asn1.Ocsp;
using static OfficeOpenXml.ExcelErrorValue;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using MySqlX.XDevAPI;
using System.Diagnostics.Eventing.Reader;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ems.payroll.DataAccess
{
    public class DaPayMstTDSapproval
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string msGetGid, pdf_name, lspath2, lspath1, mail_path, final_path, lsgrosssalary;
        double lsgross_salary, lsgross_salary_new;
        public void DaTDSApprovalPendingSummary(MdlPayMstTDSapproval values)
        {
            try
            {
                msSQL = " select a.assessment_gid, a.employee_gid, cast(concat(b.financialyear_startdate,' ','to',' ', b.financialyear_enddate) as char) as assessment_year, " +
                        " concat(d.user_firstname,' ',d.user_lastname) as emp_name, e.branch_name, f.department_name, g.designation_name, a.status " +
                        " from pay_trn_ttds a  " +
                        " left join pay_mst_tassessmentyear b on b.assessment_gid = a.assessment_gid " +
                        " left join hrm_mst_temployee c on c.employee_gid = a.employee_gid " +
                        " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                        " left join hrm_mst_tbranch e on e.branch_gid = c.branch_gid " +
                        " left join hrm_mst_tdepartment f on f.department_gid = c.department_gid " +
                        " left join adm_mst_tdesignation g on g.designation_gid = c.designation_gid " +
                        " where a.status = 'Approval Pending' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tdsapprovalpendingsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tdsapprovalpendingsummary_list
                        {
                            assessment_gid = dt["assessment_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            assessment_year = dt["assessment_year"].ToString(),
                            emp_name = dt["emp_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            approval_status = dt["status"].ToString(),
                        });
                        values.tdsapprovalpendingsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assessment details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostTDSApprove(MdlPayMstPostTDSApprove values, string user_gid)
        {
            try
            {
                msSQL = " delete from pay_trn_ttdsdeductions where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsothersections where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsaggregateofdeductable where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttds where employee_gid = '" + values.employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                string msGetGid = objcmnfunctions.GetMasterGID("PADE");

                msSQL = " insert into pay_trn_ttdsdeductions ( " +
                        " tdsdeductions_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " section80c_name1, " +
                        " section80c_grossamount1, " +
                        " section80c_name2, " +
                        " section80c_grossamount2, " +
                        " section80c_name3, " +
                        " section80c_grossamount3, " +
                        " section80c_name4, " +
                        " section80c_grossamount4, " +
                        " section80c_name5, " +
                        " section80c_grossamount5, " +
                        " section80c_name6, " +
                        " section80c_grossamount6, " +
                        " section80c_name7, " +
                        " section80c_grossamount7, " +
                        " section80c_amount7, " +
                        " section80c_deductible_amount7, " +
                        " section80ccc_grossamount, " +
                        " section80ccc_deductible_amount, " +
                        " section80ccd_grossamount, " +
                        " section80ccd_deductible_amount, " +
                        " aggregate_gross, " +
                        " aggregate_deduct, " +
                        " section80ccd1b_grossamount, " +
                        " section80ccd1b_deductamount, " +
                        " created_by, " +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.assessment_gid + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + values.section80C_i_name + "', " +
                        " '" + values.section80C_i_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_ii_name + "', " +
                        " '" + values.section80C_ii_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_iii_name + "', " +
                        " '" + values.section80C_iii_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_iv_name + "', " +
                        " '" + values.section80C_iv_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_v_name + "', " +
                        " '" + values.section80C_v_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_vi_name + "', " +
                        " '" + values.section80C_vi_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_vii_name + "', " +
                        " '" + values.section80C_vii_value.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_vii_gross_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80C_vii_deductable_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80CCC_gross_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80CCC_deductable_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80CCD_gross_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80CCD_deductable_total.ToString().Replace(",", "") + "', " +
                        " '" + values.aggregate3sec_gross_total.ToString().Replace(",", "") + "', " +
                        " '" + values.aggregate3sec_deductable_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80CCD1B_gross_total.ToString().Replace(",", "") + "', " +
                        " '" + values.section80CCD1B_deductable_total.ToString().Replace(",", "") + "', " +
                        " '" + values.employee_gid + "', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAOS");

                    msSQL = " insert into pay_trn_ttdsothersections ( " +
                            " tdsothersections_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " section1_name, " +
                            " section1_grossamount, " +
                            " section1_qualifiying_amount, " +
                            " section1_deductible_amount, " +
                            " section2_name, " +
                            " section2_grossamount, " +
                            " section2_qualifiying_amount, " +
                            " section2_deductible_amount, " +
                            " section3_name, " +
                            " section3_grossamount, " +
                            " section3_qualifiying_amount, " +
                            " section3_deductible_amount, " +
                            " section4_name, " +
                            " section4_grossamount, " +
                            " section4_qualifiying_amount, " +
                            " section4_deductible_amount, " +
                            " section5_name, " +
                            " section5_grossamount, " +
                            " section5_qualifiying_amount, " +
                            " section5_deductible_amount, " +
                            " created_by, " +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.other_section1_value + "', " +
                            " '" + values.other_section1_gross_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section1_qualifying_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section1_deductable.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section2_value + "', " +
                            " '" + values.other_section2_gross_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section2_qualifying_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section2_deductable.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section3_value + "', " +
                            " '" + values.other_section3_gross_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section3_qualifying_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section3_deductable.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section4_value + "', " +
                            " '" + values.other_section4_gross_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section4_qualifying_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section4_deductable.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section5_value + "', " +
                            " '" + values.other_section5_gross_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section5_qualifying_amount.ToString().Replace(",", "") + "', " +
                            " '" + values.other_section5_deductable.ToString().Replace(",", "") + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAAD");

                    msSQL = " insert into pay_trn_ttdsaggregateofdeductable ( " +
                            " ttdsaggregateofdeductable_gid, " +
                            " aggregatedeductable_totalamount, " +
                            " employee_gid, " +
                            " assessment_gid, " +
                            " created_by, " +
                            " created_date " +
                            " ) Values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.aggregate4Asec_deductible_total.ToString().Replace(",", "") + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.assessment_gid + "'," +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PTDS");

                    msSQL = " insert into pay_trn_ttds ( " +
                            " tds_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " total_income, " +
                            " tax_total_income, " +
                            " educationcess_amount, " +
                            " tax_payable12plus13, " +
                            " less_relief89, " +
                            " tax_payable14minus15, " +
                            " less_tds, " +
                            " balance_tax, " +
                            " status, " +
                            " created_by, " +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + values.employee_gid + "', " +
                            " '" + values.total_taxable_income.ToString().Replace(",", "") + "', " +
                            " '" + values.tax_on_total_income.ToString().Replace(",", "") + "', " +
                            " '" + values.educationcess.ToString().Replace(",", "") + "', " +
                            " '" + values.tax_payable.ToString().Replace(",", "") + "', " +
                            " '" + values.less_relief.ToString().Replace(",", "") + "', " +
                            " '" + values.net_tax_payable.ToString().Replace(",", "") + "', " +
                            " '" + values.less_tax_deducted_at_source.ToString().Replace(",", "") + "', " +
                            " '" + values.balance_tax_pay_refund.ToString().Replace(",", "") + "', " +
                            " ' Approved ', " +
                            " '" + values.employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "TDS Details approved Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while approving TDS details !!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaTDSApprovedSummary(MdlPayMstTDSapproval values)
        {
            try
            {
                msSQL = " select a.assessment_gid, a.employee_gid, cast(concat(b.financialyear_startdate,' ','to',' ', b.financialyear_enddate) as char) as assessment_year, " +
                        " concat(d.user_firstname,' ',d.user_lastname) as emp_name, e.branch_name, f.department_name, g.designation_name, a.status " +
                        " from pay_trn_ttds a  " +
                        " left join pay_mst_tassessmentyear b on b.assessment_gid = a.assessment_gid " +
                        " left join hrm_mst_temployee c on c.employee_gid = a.employee_gid " +
                        " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                        " left join hrm_mst_tbranch e on e.branch_gid = c.branch_gid " +
                        " left join hrm_mst_tdepartment f on f.department_gid = c.department_gid " +
                        " left join adm_mst_tdesignation g on g.designation_gid = c.designation_gid " +
                        " where a.status = ' Approved' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tdsapprovedsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tdsapprovedsummary_list
                        { 
                            assessment_gid = dt["assessment_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            assessment_year = dt["assessment_year"].ToString(),
                            emp_name = dt["emp_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            approval_status = dt["status"].ToString(),
                        });
                        values.tdsapprovedsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assessment details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}