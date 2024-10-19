using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstTDSapproval : result
    {
        public List<tdsapprovalpendingsummary_list> tdsapprovalpendingsummary_list { get; set; }
        public List<tdsapprovedsummary_list> tdsapprovedsummary_list { get; set; }
    }
    public class tdsapprovalpendingsummary_list : result
    {
        public string assessment_gid { get; set; }
        public string employee_gid { get; set; }
        public string assessment_year { get; set; }
        public string emp_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string approval_status { get; set; }
    }

    public class MdlPayMstPostTDSApprove : result
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
    public class tdsapprovedsummary_list : result
    {
        public string assessment_gid { get; set; }
        public string employee_gid { get; set; }
        public string assessment_year { get; set; }
        public string emp_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string approval_status { get; set; }
    }
}