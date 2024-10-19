using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmTrnEmployee360:result   {
       
            public List<GetPaymentdetails> GetPaymentdetails { get; set; }
            public List<Getloandetails> Getloandetails { get; set; }
            public List<Getattendancedetails> Getattendancedetails { get; set; }
            public List<Getstatutorydetails> Getstatutorydetails { get; set; }
            public List<Getworkexperienedetails> Getworkexperienedetails { get; set; }
            public List<Getdocumentdetails> Getdocumentdetails { get; set; }
            public List<Getemployeebinding> Getemployeebinding { get; set; }
            public List<Getemployeeinformation> Getemployeeinformation { get; set; }
            public List<Getemployeegeneral> Getemployeegeneral { get; set; }
            public List<getaddrees> getaddrees { get; set; }
            public List<getaccount> getaccount { get; set; }
          
        
    }
    public class Getemployeebinding : result
    {
        public string user_name { get; set; }
        public string user_code { get; set; }
        public string employee_photo { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }     

    }
    public class Getemployeeinformation : result
    {
        public string employee_qualification { get; set; }
        public string employee_experience { get; set; }
        public string employee_mobileno { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }     
    }
     public class Getemployeegeneral:result
    {
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public string marital_status { get; set; }
        public string religion { get; set; }
        public string nationality { get; set; }
    }   
    public class getaccount:result
    {
        public string quarter_no { get; set; }
        public string lic_no { get; set; }
        public string pf_no { get; set; }
        public string stateinsure_no { get; set; }
        public string pf_doj { get; set; }
    }   
    public class getaddrees:result
    {
        public string employee_village { get; set; }
        public string employee_taluk { get; set; }
        public string employee_subdivision { get; set; }
        public string employee_po { get; set; }
        public string employee_district { get; set; }
        public string employee_state { get; set; }
    }

    public class GetPaymentdetails : result
    {
        public string employee_gid { get; set; }
        public string salary_gid { get; set; }
        public string earned_gross_salary { get; set; }
        public string payment_date { get; set; }
        public string payment_month { get; set; }
        public string payment_year { get; set; }
        public string net_salary { get; set; }
        public string account_no { get; set; }   
        public string bank_branch { get; set; }   

    }
    public class Getloandetails : result
    {
        public string loan_gid { get; set; }
        public string employee_gid { get; set; }
        public string loan_refno { get; set; }
        public string salary_gid { get; set; }
        public string earned_gross_salary { get; set; }
        public string loan_duration { get; set; }   
        public string loan_amount { get; set; }   
        public string loan_repayment_startfrom { get; set; }   
        public string created_date { get; set; }
        public string loan_remarks { get; set; }
        public string type { get; set; }
        public string grade_name { get; set; }
        public string net_salary { get; set; }
    }
    public class Getattendancedetails : result
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string employee_gid { get; set; }
        public string present { get; set; }
        public string absent { get; set; }
        public string weekoff { get; set; }
        public string leave_count { get; set; }   
        public string Holiday { get; set; }   
        public string late_count { get; set; }   
        public string permission_count { get; set; }
        public string earlyout_count { get; set; }
        
    }
    public class Getstatutorydetails : result
    {
        public string year { get; set; }
        public string month { get; set; }
        public string salarycomponent_name { get; set; }
        public string eligibleemployer_component { get; set; }
        public string salary_gid { get; set; }
        public string eligible_component { get; set; }        
    }
    public class Getworkexperienedetails : result
    {
        public string experience_gid { get; set; }
        public string company_name { get; set; }
        public string job_title { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string description { get; set; }        
        public string employee_gid { get; set; }        
    }
    public class Getdocumentdetails : result
    {
        public string document_gid { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string document_location { get; set; }
        public string document_name { get; set; }          
    }
}