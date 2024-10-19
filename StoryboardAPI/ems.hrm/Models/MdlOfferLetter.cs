using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Mysqlx.Datatypes.Scalar.Types;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.Models
{
    public class MdlOfferLetter
    {
        public List<Offersummary_list> Offersummary_list { get; set; }
        public List<Getgradetemplatedropdown> Getgradetemplatedropdown { get; set; }
        public List<SummaryAddition_list> SummaryAddition_list { get; set; }
        public List<Deduction_list> Deduction_list { get; set; }
        public List<others_list> others_list { get; set; }
        public List<GetAdditionalcomponentname> GetAdditionalcomponentname { get; set; }

        public List<Getdeductioncomponentname> Getdeductioncomponentname { get; set; }
        public List<Summarybind_list> Summarybind_list { get; set; }
        public List<othersSummarybind_list> othersSummarybind_list { get; set; }
        public List<deductionSummarybind_list> deductionSummarybind_list { get; set; }
        public List<Getemployeebind> Getemployeebind { get; set; }
        public List<getexistemployee> getexistemployee { get; set; }

        public string message { get; set; }
        public bool status { get; set; }
        public double netsalary { get; set; }
        public double overallnetsalary { get; set; }
        public double ctc { get; set; }
    }
    public class Offersummary_list : result
    {
        public string offer_gid { get; set; }
        public string emp_name { get; set; }
        public string designation_name { get; set; }
        public string branch_name { get; set; }
        public string offer_date { get; set; }
        public string department_name { get; set; }
        public string joining_date { get; set; }

    }

    public class getexistemployee : result
    {
        public string designation_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string country_gid { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address2 { get; set; }
        public string address1 { get; set; }
        public string employee_experience { get; set; }
        public string employee_qualification { get; set; }
        public string employee_emailid { get; set; } 
        public string branch_gid { get; set; }
        public string designation_gid { get; set; }
        public string department_gid { get; set; }
        public string employee_dob { get; set; }
        public string employee_mobileno { get; set; }
        public string employee_joiningdate { get; set; }
        public string employee_gender { get; set; }
        public string user_lastname { get; set; }
        public string user_firstname { get; set; }
        public string employee_gid { get; set; }


    }
    public class Getemployeebind : result
    {
        public string employee_gid { get; set; }
        public string emp_name { get; set; }

    }
    public class EmployeedataConfirmation : result
    {
        public List<Summarybind_list> Summarybind_list { get; set; }
        public List<deductionSummarybind_list> deductionSummarybind_list { get; set; }
        public List<othersSummarybind_list> othersSummarybind_list { get; set; }

        public string offer_gid { get; set; }
        public string first_name { get; set; }
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string user_code { get; set; }
        public string jobtype { get; set; }
        public string user_password { get; set; }
        public string confirmpassword { get; set; }
        public string active_flag { get; set; }
        public string user_status { get; set; }
        public string template_name { get; set; }
        public string gross_salary { get; set; }
        public string ctc { get; set; }
        public string net_salary { get; set; }
        public string BasicSalary { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string salarygradetype { get; set; }
        public string employee2salarygradetemplate_gid { get; set; }
    }
    public class AddOfferletter_list : result
    {
        public string offerletter_type { get; set; }
        public string branch_gid { get; set; }
        public string department_name { get; set; }
        public string department_gid { get; set; }
        public string country_gid { get; set; }
        public string designation_name { get; set; }
        public string joiningdate { get; set; }
        public string designation_gid { get; set; }
        public string employee_gid { get; set; }
        public string letter_flag { get; set; }
        public string template_gid { get; set; }
        public string temp_pincode { get; set; }
        public string temp_city { get; set; }
        public string temp_state { get; set; }
        public string temp_country { get; set; }
        public string temp_address2 { get; set; }
        public string temp_address1 { get; set; }
        public string perm_pincode { get; set; }
        public string perm_city { get; set; }
        public string perm_state { get; set; }
        public string offer_date { get; set; }
        public string offer_gid { get; set; }
        public string first_name { get; set; }
        public string branch_name { get; set; }
        public string offer_no { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
        public string qualification { get; set; }
        public string experience_detail { get; set; }
        public string document_path { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string employee_salary { get; set; }
        public string perm_address1 { get; set; }
        public string perm_address2 { get; set; }
        public string permanent_country { get; set; }
        public string offertemplate_content { get; set; }
        public string lsdesignation { get; set; }
    }

    public class Getgradetemplatedropdown : result
    {
        public string salarygradetemplate_name { get; set; }
        public string salarygradetemplate_gid { get; set; }
    }

    public class SummaryAddition_list : result
    {
        public string salarygradetemplatedtl_gid { get; set; }
        public string employer_contribution { get; set; }
        public string salarycomponent_percentage_employer { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string affect_in { get; set; }
        public string employee_contribution { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public string salarygradetype { get; set; }
    }

    public class Deduction_list : result
    {
        public string salarygradetemplatedtl_gid { get; set; }
        public string demployer_contribution { get; set; }
        public string salarycomponent_percentage_employer { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string affect_in { get; set; }
        public string demployee_contribution { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public string salarygradetype { get; set; }
    }

    public class others_list : result
    {
        public string salarygradetemplatedtl_gid { get; set; }
        public string demployer_contribution { get; set; }
        public string salarycomponent_percentage_employer { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string affect_in { get; set; }
        public string demployee_contribution { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public string salarygradetype { get; set; }

    }

    public class GetAdditionalcomponentname : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public string componentgroup_name { get; set; }
    }

    public class Getdeductioncomponentname : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
      
    }

    public class Summarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double basicsalay { get; set; }
        public double salarygradetype { get; set; }
    }

    public class deductionSummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public double net_salary { get; set; }
        public double ctc { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarygradetype { get; set; }

    }

    public class othersSummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double basicsalay { get; set; }
        public double salarygradetype { get; set; }

    }
}