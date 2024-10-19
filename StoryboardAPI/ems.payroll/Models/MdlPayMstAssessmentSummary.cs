using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstAssessment : result
    {
        public List<assessmentsummary_list> assessmentsummary_list { get; set; }
        public List<assignempsummary_list> assignempsummary_list { get; set; }
        public List<generateformsummary_list> generateformsummary_list { get; set; }
        public List<postassignemployeelist> postassignemployeelist { get; set; }       
        public List<Getfinyeardropdown> Getfinyeardropdown { get; set; }
        public List<incometax_lists> incometax_lists { get; set; }
        public List<incometaxsummary_lists> incometaxsummary_lists { get; set; }
        public List<MdlPayIncomedata> MdlPayIncomedata { get; set; }
        public List<MdlTDSData> MdlTDSData { get; set; }
        public List<MdlPayTDSdata> MdlPayTDSdata { get; set; }
        public List<Mdlanx1data> Mdlanx1data { get; set; }
        public List<MdlAnx1data> MdlAnx1data { get; set; }
        public List<Mdlanx2data> Mdlanx2data { get; set; }
        public List<MdlAnx2data> MdlAnx2data { get; set; }
        public List<download_lists> download_lists { get; set; }

        public List<product_images> product_images { get; set; }
    }

    public class download_lists : result
    {
        public string lsfilename { get; set; }
        public string lsdocument { get; set; }

    }

    public class product_images : result
    {
        public string product_gid { get; set; }
        public string product_image { get; set; }
        public string name { get; set; }
    }
    public class assessmentsummary_list : result
    {
        public string assessment_gid { get; set; }
        public string duration { get; set; }        
        public string fin_duration { get; set; }
    }
    public class assignempsummary_list : result
    {
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
    }
    public class postassignemployeelist : result
    {
        public string assessment_gid { get; set; }
        public List<assignempsummary_list> assignempsummary_list { get; set; }
    }
    public class generateformsummary_list : result
    {
        public string branch_name { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
        public string assessment_gid { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
    }
    public class MdlPersonalData : result
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
    public class updatepersonalinfolist : result
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string dob { get; set; }
        public string email_id { get; set; }
        public string active_flag { get; set; }
        public string pan_number { get; set; }
        public string uan_number { get; set; }
        public string blood_group { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
    }
    public class Getfinyeardropdown : result
    {
        public string finyear_gid { get; set; }
        public string finyear_range { get; set; }
    }
    public class incometax_lists : result
    {
        public string finyear_range { get; set; }
        public string document_type { get; set; }
        public string document_title { get; set; }
        public string remarks { get; set; }
    }
    public class incometaxsummary_lists : result
    {
        public string taxdocument_gid { get; set; }
        public string finyear_range { get; set; }
        public string documenttype_gid { get; set; }
        public string document_title { get; set; }
        public DateTime created_date { get; set; }
        public string user_gid { get; set; }
        public string document_upload { get; set; }

    }
    public class postquartersinfolist : result
    {
        public string q1_rpt_original_statement { get; set; }
        public string assessment_gid { get; set; }
        public string employee_gid { get; set; }
        public string q1_amt_paid_credited { get; set; }
        public string q1_amt_tax_deducted { get; set; }
        public string q1_amt_tax_deposited { get; set; }
        public string q2_rpt_original_statement { get; set; }
        public string q2_amt_paid_credited { get; set; }
        public string q2_amt_tax_deducted { get; set; }
        public string q2_amt_tax_deposited { get; set; }
        public string q3_rpt_original_statement { get; set; }
        public string q3_amt_paid_credited { get; set; }
        public string q3_amt_tax_deducted { get; set; }
        public string q3_amt_tax_deposited { get; set; }
        public string q4_rpt_original_statement { get; set; }
        public string q4_amt_paid_credited { get; set; }
        public string q4_amt_tax_deducted { get; set; }
        public string q4_amt_tax_deposited { get; set; }
        public string total_amt_paid_credited { get; set; }
        public string total_amt_tax_deducted { get; set; }
        public string total_amt_tax_deposited { get; set; }
    }
    public class MdlQuartersData : result
    {
        public string tdsquarter1_receiptno { get; set; }
        public string tdsquarter1_paidcredited { get; set; }
        public string tdsquarter1_amount_deposited { get; set; }
        public string tdsquarter1_amount_deducted { get; set; }
        public string tdsquarter2_receiptno { get; set; }
        public string tdsquarter2_paidcredited { get; set; }
        public string tdsquarter2_amount_deposited { get; set; }
        public string tdsquarter2_amount_deducted { get; set; }
        public string tdsquarter3_receiptno { get; set; }
        public string tdsquarter3_paidcredited { get; set; }
        public string tdsquarter3_amount_deposited { get; set; }
        public string tdsquarter3_amount_deducted { get; set; }
        public string tdsquarter4_receiptno { get; set; }
        public string tdsquarter4_paidcredited { get; set; }
        public string tdsquarter4_amount_deposited { get; set; }
        public string tdsquarter4_amount_deducted { get; set; }
        public string totalamount_paidcredited { get; set; }
        public string tdsquarter_totalamount_deposited { get; set; }
        public string tdsquarter_totalamount_deducted { get; set; }
    }
    public class MdlPayIncomedata : result
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
    public class MdlPayTDSdata : result
    {
        public string assessment_gid { get; set; }
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
        public string taxpercentold { get; set; }
        public string taxpercentnew { get; set; }
        public string tax_on_total_income { get; set; }
        public string educationcess { get; set; }
        public string tax_payable { get; set; }
        public string less_relief { get; set; }
        public string net_tax_payable { get; set; }
        public string less_tax_deducted_at_source { get; set; }
        public string balance_tax_pay_refund { get; set; }
    }
    public class MdlTDSData : result
    {
        public string section80c_name1 { get; set; }
        public string section80c_grossamount1 { get; set; }
        public string section80c_name2 { get; set; }
        public string section80c_grossamount2 { get; set; }
        public string section80c_name3 { get; set; }
        public string section80c_grossamount3 { get; set; }
        public string section80c_name4 { get; set; }
        public string section80c_grossamount4 { get; set; }
        public string section80c_name5 { get; set; }
        public string section80c_grossamount5 { get; set; }
        public string section80c_name6 { get; set; }
        public string section80c_grossamount6 { get; set; }
        public string section80c_name7 { get; set; }
        public string section80c_grossamount7 { get; set; }
        public string section80c_amount7 { get; set; }
        public string section80c_deductible_amount7 { get; set; }
        public string section80ccc_grossamount { get; set; }
        public string section80ccc_deductible_amount { get; set; }
        public string section80ccd_grossamount { get; set; }
        public string section80ccd_deductible_amount { get; set; }
        public string aggregate_gross { get; set; }
        public string aggregate_deduct { get; set; }
        public string section80ccd1b_grossamount { get; set; }
        public string section80ccd1b_deductamount { get; set; }
        public string section1_name { get; set; }
        public string section1_grossamount { get; set; }
        public string section1_qualifiying_amount { get; set; }
        public string section1_deductible_amount { get; set; }
        public string section2_name { get; set; }
        public string section2_grossamount { get; set; }
        public string section2_qualifiying_amount { get; set; }
        public string section2_deductible_amount { get; set; }
        public string section3_name { get; set; }
        public string section3_grossamount { get; set; }
        public string section3_qualifiying_amount { get; set; }
        public string section3_deductible_amount { get; set; }
        public string section4_name { get; set; }
        public string section4_grossamount { get; set; }
        public string section4_qualifiying_amount { get; set; }
        public string section4_deductible_amount { get; set; }
        public string section5_name { get; set; }
        public string section5_grossamount { get; set; }
        public string section5_qualifiying_amount { get; set; }
        public string section5_deductible_amount { get; set; }
        public string aggregatedeductable_totalamount { get; set; }
        public string total_income { get; set; }
        public string tax_total_income { get; set; }
        public string educationcess_amount { get; set; }
        public string tax_payable12plus13 { get; set; }
        public string less_relief89 { get; set; }
        public string tax_payable14minus15 { get; set; }
        public string less_tds { get; set; }
        public string balance_tax { get; set; }
    }

    public class Mdlanx1data : result
    {
        public string tax_deposit1 { get; set; }
        public string receipt_no1 { get; set; }
        public string ddo_no1 { get; set; }
        public string date_transfer1 { get; set; }
        public string status1 { get; set; }
        public string tax_deposit2 { get; set; }
        public string receipt_no2 { get; set; }
        public string ddo_no2 { get; set; }
        public string date_transfer2 { get; set; }
        public string status2 { get; set; }
        public string tax_deposit3 { get; set; }
        public string receipt_no3 { get; set; }
        public string ddo_no3 { get; set; }
        public string date_transfer3 { get; set; }
        public string status3 { get; set; }
        public string tax_deposit4 { get; set; }
        public string receipt_no4 { get; set; }
        public string ddo_no4 { get; set; }
        public string date_transfer4 { get; set; }
        public string status4 { get; set; }
        public string tax_deposit5 { get; set; }
        public string receipt_no5 { get; set; }
        public string ddo_no5 { get; set; }
        public string date_transfer5 { get; set; }
        public string status5 { get; set; }
        public string tax_deposit6 { get; set; }
        public string receipt_no6 { get; set; }
        public string ddo_no6 { get; set; }
        public string date_transfer6 { get; set; }
        public string status6 { get; set; }
        public string tax_deposit7 { get; set; }
        public string receipt_no7 { get; set; }
        public string ddo_no7 { get; set; }
        public string date_transfer7 { get; set; }
        public string status7 { get; set; }
        public string tax_deposit8 { get; set; }
        public string receipt_no8 { get; set; }
        public string ddo_no8 { get; set; }
        public string date_transfer8 { get; set; }
        public string status8 { get; set; }
        public string tax_deposit9 { get; set; }
        public string receipt_no9 { get; set; }
        public string ddo_no9 { get; set; }
        public string date_transfer9 { get; set; }
        public string status9 { get; set; }
        public string tax_deposit10 { get; set; }
        public string receipt_no10 { get; set; }
        public string ddo_no10 { get; set; }
        public string date_transfer10 { get; set; }
        public string status10 { get; set; }
        public string tax_deposit11 { get; set; }
        public string receipt_no11 { get; set; }
        public string ddo_no11 { get; set; }
        public string date_transfer11 { get; set; }
        public string status11 { get; set; }
        public string tax_deposit12 { get; set; }
        public string receipt_no12 { get; set; }
        public string ddo_no12 { get; set; }
        public string date_transfer12 { get; set; }
        public string status12 { get; set; }
        public string total_taxdeposit { get; set; }

    }
    public class MdlAnx1data : result
    {
        public string assessment_gid { get; set; }
        public string total_tax_deposited_anx1 { get; set; }
        public string employee_gid { get; set; }
        public string totaltax_dep1_anx1 { get; set; }
        public string totaltax_dep2_anx1 { get; set; }
        public string totaltax_dep3_anx1 { get; set; }
        public string totaltax_dep4_anx1 { get; set; }
        public string totaltax_dep5_anx1 { get; set; }
        public string totaltax_dep6_anx1 { get; set; }
        public string totaltax_dep7_anx1 { get; set; }
        public string totaltax_dep8_anx1 { get; set; }
        public string totaltax_dep9_anx1 { get; set; }
        public string totaltax_dep10_anx1 { get; set; }
        public string totaltax_dep11_anx1 { get; set; }
        public string totaltax_dep12_anx1 { get; set; }
        public string receiptnum1_anx1 { get; set; }
        public string receiptnum2_anx1 { get; set; }
        public string receiptnum3_anx1 { get; set; }
        public string receiptnum4_anx1 { get; set; }
        public string receiptnum5_anx1 { get; set; }
        public string receiptnum6_anx1 { get; set; }
        public string receiptnum7_anx1 { get; set; }
        public string receiptnum8_anx1 { get; set; }
        public string receiptnum9_anx1 { get; set; }
        public string receiptnum10_anx1 { get; set; }
        public string receiptnum11_anx1 { get; set; }
        public string receiptnum12_anx1 { get; set; }
        public string ddonum1_anx1 { get; set; }
        public string ddonum2_anx1 { get; set; }
        public string ddonum3_anx1 { get; set; }
        public string ddonum4_anx1 { get; set; }
        public string ddonum5_anx1 { get; set; }
        public string ddonum6_anx1 { get; set; }
        public string ddonum7_anx1 { get; set; }
        public string ddonum8_anx1 { get; set; }
        public string ddonum9_anx1 { get; set; }
        public string ddonum10_anx1 { get; set; }
        public string ddonum11_anx1 { get; set; }
        public string ddonum12_anx1 { get; set; }
        public string date1_anx1 { get; set; }
        public string date2_anx1 { get; set; }
        public string date3_anx1 { get; set; }
        public string date4_anx1 { get; set; }
        public string date5_anx1 { get; set; }
        public string date6_anx1 { get; set; }
        public string date7_anx1 { get; set; }
        public string date8_anx1 { get; set; }
        public string date9_anx1 { get; set; }
        public string date10_anx1 { get; set; }
        public string date11_anx1 { get; set; }
        public string date12_anx1 { get; set; }
        public string status1_anx1 { get; set; }
        public string status2_anx1 { get; set; }
        public string status3_anx1 { get; set; }
        public string status4_anx1 { get; set; }
        public string status5_anx1 { get; set; }
        public string status6_anx1 { get; set; }
        public string status7_anx1 { get; set; }
        public string status8_anx1 { get; set; }
        public string status9_anx1 { get; set; }
        public string status10_anx1 { get; set; }
        public string status11_anx1 { get; set; }
        public string status12_anx1 { get; set; }
    }


    public class Mdlanx2data : result
    {
        public string total_taxdeposited { get; set; }
        public string bsrcode1 { get; set; }
        public string date_tax1 { get; set; }
        public string challanno_tax1 { get; set; }
        public string status1 { get; set; }
        public string totaltax_deposited2 { get; set; }
        public string bsrcode2 { get; set; }
        public string date_tax2 { get; set; }
        public string challanno_tax2 { get; set; }
        public string status2 { get; set; }
        public string totaltax_deposited3 { get; set; }
        public string bsrcode3 { get; set; }
        public string date_tax3 { get; set; }
        public string challanno_tax3 { get; set; }
        public string status3 { get; set; }
        public string totaltax_deposited4 { get; set; }
        public string bsrcode4 { get; set; }
        public string date_tax4 { get; set; }
        public string challanno_tax4 { get; set; }
        public string status4 { get; set; }
        public string totaltax_deposited5 { get; set; }
        public string bsrcode5 { get; set; }
        public string date_tax5 { get; set; }
        public string challanno_tax5 { get; set; }
        public string status5 { get; set; }
        public string totaltax_deposited6 { get; set; }
        public string bsrcode6 { get; set; }
        public string date_tax6 { get; set; }
        public string challanno_tax6 { get; set; }
        public string status6 { get; set; }
        public string totaltax_deposited7 { get; set; }
        public string bsrcode7 { get; set; }
        public string date_tax7 { get; set; }
        public string challanno_tax7 { get; set; }
        public string status7 { get; set; }
        public string totaltax_deposited8 { get; set; }
        public string bsrcode8 { get; set; }
        public string date_tax8 { get; set; }
        public string challanno_tax8 { get; set; }
        public string status8 { get; set; }
        public string totaltax_deposited9 { get; set; }
        public string bsrcode9 { get; set; }
        public string date_tax9 { get; set; }
        public string challanno_tax9 { get; set; }
        public string status9 { get; set; }
        public string totaltax_deposited10 { get; set; }
        public string bsrcode10 { get; set; }
        public string date_tax10 { get; set; }
        public string challanno_tax10 { get; set; }
        public string status10 { get; set; }
        public string totaltax_deposited11 { get; set; }
        public string bsrcode11 { get; set; }
        public string challanno_tax11 { get; set; }
        public string date_tax11 { get; set; }
        public string status11 { get; set; }
        public string totaltax_deposited12 { get; set; }
        public string bsrcode12 { get; set; }
        public string date_tax12 { get; set; }
        public string challanno_tax12 { get; set; }
        public string status12 { get; set; }
        public string total_taxdeposit { get; set; }

    }

    public class MdlAnx2data : result
    {
        public string assessment_gid { get; set; }
        public string employee_gid { get; set; }
        public string totaltax_dep1_anx2 { get; set; }
        public string totaltax_dep2_anx2 { get; set; }
        public string totaltax_dep3_anx2 { get; set; }
        public string totaltax_dep4_anx2 { get; set; }
        public string totaltax_dep5_anx2 { get; set; }
        public string totaltax_dep6_anx2 { get; set; }
        public string totaltax_dep7_anx2 { get; set; }
        public string totaltax_dep8_anx2 { get; set; }
        public string totaltax_dep9_anx2 { get; set; }
        public string totaltax_dep10_anx2 { get; set; }
        public string totaltax_dep11_anx2 { get; set; }
        public string totaltax_dep12_anx2 { get; set; }
        public string bsrcode1_anx2 { get; set; }
        public string bsrcode2_anx2 { get; set; }
        public string bsrcode3_anx2 { get; set; }
        public string bsrcode4_anx2 { get; set; }
        public string bsrcode5_anx2 { get; set; }
        public string bsrcode6_anx2 { get; set; }
        public string bsrcode7_anx2 { get; set; }
        public string bsrcode8_anx2 { get; set; }
        public string bsrcode9_anx2 { get; set; }
        public string bsrcode10_anx2 { get; set; }
        public string bsrcode11_anx2 { get; set; }
        public string bsrcode12_anx2 { get; set; }
        public string challan1_anx2 { get; set; }
        public string challan2_anx2 { get; set; }
        public string challan3_anx2 { get; set; }
        public string challan4_anx2 { get; set; }
        public string challan5_anx2 { get; set; }
        public string challan6_anx2 { get; set; }
        public string challan7_anx2 { get; set; }
        public string challan8_anx2 { get; set; }
        public string challan9_anx2 { get; set; }
        public string challan10_anx2 { get; set; }
        public string challan11_anx2 { get; set; }
        public string challan12_anx2 { get; set; }
        public string date1_anx2 { get; set; }
        public string date2_anx2 { get; set; }
        public string date3_anx2 { get; set; }
        public string date4_anx2 { get; set; }
        public string date5_anx2 { get; set; }
        public string date6_anx2 { get; set; }
        public string date7_anx2 { get; set; }
        public string date8_anx2 { get; set; }
        public string date9_anx2 { get; set; }
        public string date10_anx2 { get; set; }
        public string date11_anx2 { get; set; }
        public string date12_anx2 { get; set; }
        public string status1_anx2 { get; set; }
        public string status2_anx2 { get; set; }
        public string status3_anx2 { get; set; }
        public string status4_anx2 { get; set; }
        public string status5_anx2 { get; set; }
        public string status6_anx2 { get; set; }
        public string status7_anx2 { get; set; }
        public string status8_anx2 { get; set; }
        public string status9_anx2 { get; set; }
        public string status10_anx2 { get; set; }
        public string status11_anx2 { get; set; }
        public string status12_anx2 { get; set; }
        public string total_tax_deposited_anx2 { get; set; }
    }
    public class MdlIncometaxData : result
    {
        public string fin_year { get; set; }
        public string documenttype_gid { get; set; }
        public string document_title { get; set; }
        public string remarks { get; set; }
    }
}