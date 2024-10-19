using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstEmployeeAssessmentSummary : result
    {
        public List<employeeassessmentsummary_list> employeeassessmentsummary_list { get; set; }
        public List<employeeoldtaxslabsummary_list> employeeoldtaxslabsummary_list { get; set; }
        public List<employeenewtaxslabsummary_list> employeenewtaxslabsummary_list { get; set; }
        public List<getfinyeardropdownlist> getfinyeardropdownlist { get; set; }
        public List<employeeincometaxsummary_lists> employeeincometaxsummary_lists { get; set; }        
    }
    public class employeeassessmentsummary_list : result
    {
        public string assessment_gid { get; set; }
        public string duration { get; set; }
        public string fin_duration { get; set; }
        public string status { get; set; }
    }
    public class MdlEmployeePersonalData : result
    {
        public string address_gid { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string employee_dob { get; set; }
        public string employee_emailid { get; set; }
        public string employee_gender { get; set; }
        public string pan_no { get; set; }
        public string uan_no { get; set; }
        public string bloodgroup { get; set; }
        public string employee_mobileno { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country_name { get; set; }
    }
    public class employeeoldtaxslabsummary_list : result
    {
        public string tax_regime_gid { get; set; }
        public string tax_slab { get; set; }
        public string individuals { get; set; }
        public string resident_senior_citizens { get; set; }
        public string resident_super_senior_citizens { get; set; }
    }
    public class employeenewtaxslabsummary_list : result
    {
        public string tax_regime_gid { get; set; }
        public string tax_slabnew { get; set; }
        public string income_tax_rates1 { get; set; }
    }
    public class getfinyeardropdownlist : result
    {
        public string finyear_gid { get; set; }
        public string finyear_range { get; set; }
    }
    public class employeeincometaxsummary_lists : result
    {
        public string taxdocument_gid { get; set; }
        public string finyear_range { get; set; }
        public string documenttype_gid { get; set; }
        public string document_title { get; set; }
        public DateTime created_date { get; set; }
        public string user_gid { get; set; }
        public string document_upload { get; set; }
    }
    public class MdlEmployeeIncomedata : result
    {
        public double grosssalary_amount { get; set; }
        public string perquisites_amount { get; set; }
        public string profitof_salary { get; set; }
        public string totalamount { get; set; }
        public string component1 { get; set; }
        public string pfamount1 { get; set; }
        public string component2 { get; set; }
        public string pfamount2 { get; set; }
        public string component3 { get; set; }
        public string pfamount3 { get; set; }
        public string balanceamount { get; set; }
        public string entertainment_allowance { get; set; }
        public string taxon_emp { get; set; }
        public string aggreagateofab { get; set; }
        public string incomecharge_headsalaries { get; set; }
        public string employee_income1 { get; set; }
        public string employeeincome_rs1 { get; set; }
        public string employee_income2 { get; set; }
        public string employeeincome_rs2 { get; set; }
        public string employee_income3 { get; set; }
        public string employeeincome_rs3 { get; set; }
        public string employeeincome_total { get; set; }
        public string grosstotal_income { get; set; }
        public string assessment_gid { get; set; }
        public string employee_gid { get; set; }
        public string lessallowancetotal { get; set; }
    }
    public class MdlEmployeeTDSData : result
    {
        public string assessment_gid { get; set; }
        public string tds_gid { get; set; }
        public string employee_gid { get; set; }
        public string section80C_i_name { get; set; }
        public string section80C_i_value { get; set; }
        public string section80C_ii_name { get; set; }
        public string section80C_ii_value { get; set; }
        public string section80C_iii_name { get; set; }
        public string section80C_iii_value { get; set; }
        public string section80C_iv_name { get; set; }
        public string section80C_iv_value { get; set; }
        public string section80C_v_name { get; set; }
        public string section80C_v_value { get; set; }
        public string section80C_vi_name { get; set; }
        public string section80C_vi_value { get; set; }
        public string section80C_vii_name { get; set; }
        public string section80C_vii_value { get; set; }
        public string section80C_vii_gross_total { get; set; }
        public string section80C_vii_deductable_total { get; set; }
        public string section80CCC_gross_total { get; set; }
        public string section80CCC_deductable_total { get; set; }
        public string section80CCD_gross_total { get; set; }
        public string section80CCD_deductable_total { get; set; }
        public string aggregate3sec_gross_total { get; set; }
        public string aggregate3sec_deductable_total { get; set; }
        public string section80CCD1B_gross_total { get; set; }
        public string section80CCD1B_deductable_total { get; set; }
        public string other_section1_value { get; set; }
        public string other_section1_gross_amount { get; set; }
        public string other_section1_qualifying_amount { get; set; }
        public string other_section1_deductable { get; set; }
        public string other_section2_value { get; set; }
        public string other_section2_gross_amount { get; set; }
        public string other_section2_qualifying_amount { get; set; }
        public string other_section2_deductable { get; set; }
        public string other_section3_value { get; set; }
        public string other_section3_gross_amount { get; set; }
        public string other_section3_qualifying_amount { get; set; }
        public string other_section3_deductable { get; set; }
        public string other_section4_value { get; set; }
        public string other_section4_gross_amount { get; set; }
        public string other_section4_qualifying_amount { get; set; }
        public string other_section4_deductable { get; set; }
        public string other_section5_value { get; set; }
        public string other_section5_gross_amount { get; set; }
        public string other_section5_qualifying_amount { get; set; }
        public string other_section5_deductable { get; set; }
        public string aggregate4Asec_deductible_total { get; set; }
        public string total_taxable_income { get; set; }
        public string tax_on_total_income { get; set; }
        public string educationcess { get; set; }
        public string tax_payable { get; set; }
        public string less_relief { get; set; }
        public string net_tax_payable { get; set; }
        public string less_tax_deducted_at_source { get; set; }
        public string balance_tax_pay_refund { get; set; }
    }
}