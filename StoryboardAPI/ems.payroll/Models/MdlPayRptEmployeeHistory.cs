using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayRptEmployeeHistory : result
    {
        public List<GetBranchdata> GetBranchDetail { get; set; }
        public List<GetDepartmentdata> GetDepartmentDtl { get; set; }
        public List<employeehistory_list> employeehistory_list { get; set; }
        public List<getViewEmployeeReportSummary> getViewEmployeeReportSummary { get; set; }
        public List<getViewPromotionHistory> getViewPromotionHistory { get; set; }
        public List<getViewPaymentDetails> getViewPaymentDetails { get; set; }
    }
    public class GetBranchdata : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }


    }
    public class GetDepartmentdata : result
    {
        public string department_gid { get; set; }
        public string department_name { get; set; }


    }
    public class employeehistory_list : result
    {
        public string department_gid { get; set; }
        public string branch_gid { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string employee_gid { get; set; }
        public string total_salary { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }


    }

    public class getViewEmployeeReportSummary : result
    {
        public string employee_experience { get; set; }
        public string employee_qualification { get; set; }
        public string employeetype_name { get; set; }
        public string wagetype_name { get; set; }
        public string workertype_name { get; set; }
        public string roll_name { get; set; }
        public string employee_diffabled { get; set; }
        public string branch_name { get; set; }
        public string bloodgroup { get; set; }
        public string employee_joiningdate { get; set; }
        public string identity_no { get; set; }
        public string department_name { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string designation_name { get; set; }
        public string user_status { get; set; }
        public string employee_gender { get; set; }
        public string employee_dob { get; set; }

        public string employee_emailid { get; set; }
        public string employee_mobileno { get; set; }
        public string jobtype_name { get; set; }
        public string nationality { get; set; }
        public string daysalary_rate { get; set; }
        public string employeepreviouscompany_name { get; set; }
    }

    public class getViewPromotionHistory : result
    {
        public string employee_gid { get; set; }
        public string designation_gid { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string designation_name { get; set; }
        public string username { get; set; }
        public string salarygrade_gid { get; set; }
        public string department_name { get; set; }
        public string salarygradetemplate_name { get; set; }
    }

    public class getViewPaymentDetails : result
    {
        public string net_salary { get; set; }
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string user_code { get; set; }
        public string payment_month { get; set; }
        public string payment_year { get; set; }
    }
    }