
using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.payroll.Models
{
    public class MdlRptPayrunSummary : result
    {
        public List<GetBranchdropdown> GetBranchDtl { get; set; }
        public List<initialpayrun_list> initialpayrun_list {  get; set; }

        public List<GetDepartmentdropdown> GetDepartmentDtl { get; set; }

        public List<GetPayrunlist> payrunlist { get; set; }
        public List<addsummary> addsummary { get; set; }
        public List<dedsummary> dedsummary { get; set; }
        public List<othersummary> othersummary { get; set; }
        public List<GetMailId_list> GetMailId_list { get; set; }
    }
    public class GetBranchdropdown : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }


    }
    public class initialpayrun_list : result
    {
        public string salary_gid { get; set; }
        public string branch_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string branch_name { get; set; }
        public string department { get; set; }
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string leave_taken { get; set; }
        public string lop { get; set; }
        public string public_holidays { get; set; }
        public string basic_salary { get; set; }
        public string earned_basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string earned_gross_salary { get; set; }
        public string net_salary { get; set; }
        public string earned_net_salary { get; set; }
        public string actual_month_workingdays { get; set; }
        public string month_workingdays { get; set; }
    }

    public class GetDepartmentdropdown : result
    {
        public string department_gid { get; set; }
        public string department_name { get; set; }


    }

    public class addsummary : result
    {
        public string salary_gid { get; set; }
        public string earned_amount { get; set; }
        public string salarycomponent_name { get; set; }
    }
    public class dedsummary : result
    {
        public string salary_gid { get; set; }
        public string earned_amount { get; set; }
        public string salarycomponent_name { get; set; }
    }
    public class othersummary : result
    {
        public string salary_gid { get; set; }
        public string earned_amount { get; set; }
        public string salarycomponent_name { get; set; }
    }
    public class GetMailId_list : result
    {
        public string employee_emailid { get; set; }
        public string pop_username { get; set; }
    }
    public class GetPayrunlist : result
    {
        public string salary_gid { get; set; }
        public string branch_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string branch_name { get; set; }
        public string department { get; set; }
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string leave_taken { get; set; }
        public string to_emailid1 { get; set; }
        public string lop { get; set; }
        public string public_holidays { get; set; }
        public string basic_salary { get; set; }
        public string earned_basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string earned_gross_salary { get; set; }
        public string net_salary { get; set; }
        public string earned_net_salary { get; set; }
        public string actual_month_workingdays { get; set; }
        public string month_workingdays { get; set; }
        public string addsalarycomponent_name { get; set; }
        public string addearned_amount { get; set; }
        public string dedsalarycomponent_name { get; set; }
        public string dedearned_amount { get; set; }
        public string othersalarycomponent_name { get; set; }
        public string otherearned_amount { get; set; }



    }
    public class deletepayrunlist : result
    {
        public List<GetPayrunlist> payrunlist { get; set; }
        public string month { get; set; }
        public string year { get; set; }

    }


}

