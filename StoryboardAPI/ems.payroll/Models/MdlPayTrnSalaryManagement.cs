using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayTrnSalaryManagement : result
    {
        public List<employeesalary_list> employeesalarylist { get; set; }
        public List<GetEmployeeSelect> GetEmployeeSelect { get; set; }
        public List<GetEmployeelist> GetEmployeelist { get; set; }
        public List<employeeleave_list> employeeleavelist { get; set; }
        public List<GetPayrunlist1> payrunlist { get; set; }
        public List<payrunviewlist> payrunviewlist { get; set; }
        public List<addsummary1> addsummary1 { get; set; }
        public List<deductsummary1> deductsummary1 { get; set; }
        public List<otherssummary1> otherssummary1 { get; set; }
        public List<leavereport_list> leavereport_list { get; set; }
        public List<Payrun_list> Payrun_list { get; set; }
        public List<Editaddition_list> Editaddition_list { get; set; }
        public List<Editdeduction_list> Editdeduction_list { get; set; }
        public List<Editothers_list> Editothers_list { get; set; }
        public List<Editsalary_list> Editsalary_list { get; set; }
        public List<basicsalarylist> basicsalarylist { get; set; }

    }

    public class basicsalarylist : result
    {
        public string salary_mode { get; set; }
    }
    public class Editaddition_list : result
    {
        public string salary_gid { get; set; }
        public string salarydtl_gid { get; set; }
        public string addsalarycomponent_name { get; set; }
        public string addsalarycomponent_amount { get; set; }
        public string addemployer_salarycomponentamount { get; set; }
        public string addcomponentgroup_name { get; set; }
        public string gross_salary { get; set; }
        public string gross_salaryemployer { get; set; }

    }

    public class Editdeduction_list : result
    {
        public string salary_gid { get; set; }
        public string salarydtl_gid { get; set; }
        public string dedsalarycomponent_name { get; set; }
        public string dedsalarycomponent_amount { get; set; }
        public string dedemployer_salarycomponentamount { get; set; }
        public string dedcomponentgroup_name { get; set; }

    }

    public class Editothers_list : result
    {
        public string salary_gid { get; set; }
        public string salarydtl_gid { get; set; }
        public string otherssalarycomponent_name { get; set; }
        public string othersemployer_salarycomponentamount { get; set; }
        public string otherssalarycomponent_amount { get; set; }
        public string otherscomponentgroup_name { get; set; }
    }

    public class Editsalary_list : result
    {
        public string salary_gid { get; set; }
        public string basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string net_salary { get; set; }
        public string ctc { get; set; }
    }
    public class addsummary1 : result
    {
        public string salary_gid { get; set; }
        public string earned_amount { get; set; }
        public string salarycomponent_name { get; set; }
    }
    public class otherssummary1 : result
    {
        public string salary_gid { get; set; }
        public string earned_amount { get; set; }
        public string salarycomponent_name { get; set; }
    }

    public class deductsummary1 : result
    {
        public string salary_gid { get; set; }
        public string earned_amount { get; set; }
        public string salarycomponent_name { get; set; }
    }
    public class Getmonthlypayrun : result
    {
        public string month { get; set; }
        public string year { get; set; }
        public DateTime payrundate { get; set; }
        public string employee_name { get; set; }
        public List<employeeleave_list> employeeleave_list { get; set; }

    }

    public class payrunviewlist : result
    {                   
        public string salary_gid { get; set; }
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string branch_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string branch_name { get; set; }
        public string department { get; set; }
        public string leave_taken { get; set; }
        public string lop { get; set; }
        public string department_gid { get; set; }
        public string leave_wage { get; set; }
        public string ot_hours { get; set; }
        public string ot_rate { get; set; }
        public string designation_name { get; set; }
        public string user_gid { get; set; }
        public string earned_basic_salary { get; set; }
        public string basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string earned_gross_salary { get; set; }
        public string public_holidays { get; set; }
        public string permission_wage { get; set; }
        public string net_salary { get; set; }
        public string earned_net_salary { get; set; }
        public string actual_month_workingdays { get; set; }
        public string month_workingdays { get; set; }
        public string loanadvance_amount { get; set; }
        public string attendance_allowance { get; set; }
    }
    public class GetEmployeelist : result
    {
        public List<detailsdtl_list> detailsdtl_list { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
    public class detailsdtl_list : result
    { 
        public string employee_gid { get; set; }

    }
    public class GetEmployeeSelect : result
    {
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string designation_name { get; set; }
        public string branch_name { get; set; }
        public string joiningmonth_number { get; set; }
        public string department_gid { get; set; }
        public string branch_gid { get; set; }
        public string department_name { get; set; }


    }

    public class employeesalary_list : result
    {
        public string salary_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string Workingdays { get; set; }
        public string generated_by { get; set; }
        public string totalemployee { get; set; }
        public string net_salary { get; set; }
        public string earned_net_salary { get; set; }
       
      }

    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class employeeleave_list : result
    {
        public string totaldays { get; set; }
        public string lspayrun_flag { get; set; }
        public string weekoff_days { get; set; }
        public string salary_days { get; set; }
        public string absent { get; set; }
        public string adjusted_lop { get; set; }
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string username { get; set; }
        public string leavecount { get; set; }
        public string month_workingdays { get; set; }
        public string lop { get; set; }
        public string actual_lop { get; set; }
        public string user_gid { get; set; }
        public string holidaycount { get; set; }
        public string actualworkingdays { get; set; }
        public string weekoffcount { get; set; }
        public string salary_gid { get; set; }
        public string year { get; set; }
        public string employee_workeddays { get; set; }
        public string basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string net_salary { get; set; }


    }
    public class GetPayrunlist1 : result
    {
        public string salary_gid { get; set; }
        public string branch_gid { get; set; }
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

    public class payrunedit_list1 : result
    {
        public string employee_name { get; set; }
        public string payrun_date { get; set; }
        public string basic_salary { get; set; }

    }

    public class payrunedit_list2 : result
    {
        public string salarycomponent_name { get; set; }
        public string componentgroup_name { get; set; }
        public string salarycomponent_amount { get; set; }
        public string earnedemployer_salarycomponentamount { get; set; }
    }

    public class payrunedit_list3 : result
    {
        public string salarycomponent_name { get; set; }
        public string componentgroup_name { get; set; }
        public string salarycomponent_amount { get; set; }
        public string earnedemployer_salarycomponentamount { get; set; }

    }

    public class leavereport_list : result
    {
        public string totaldays { get; set; }
        public string weekoff_days { get; set; }
        public string salary_days { get; set; }
        public string absent { get; set; }
        public string adjusted_lop { get; set; }
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string username { get; set; }
        public string leavecount { get; set; }
        public string month_workingdays { get; set; }
        public string lop { get; set; }
        public string actual_lop { get; set; }
        public string user_gid { get; set; }
        public string holidaycount { get; set; }
        public string actualworkingdays { get; set; }
        public string weekoffcount { get; set; }

    }

    public class deleteleavereportlist : result
    {
        public List<employeeleave_list> employeeleavelist { get; set; }
        public string month { get; set; }
        public string year { get; set; }

    }


    public class Payrun_list : result
    {
        public string employee_name { get; set; }
        public string basic_salary { get; set; }
        public string payrun_date { get; set; }
        public string salary_mode { get; set; }

    }
    public class UpdatePayrun : result
    {
        public List<employeeleave_list> employeeleavelist { get; set; }
        public List<Editaddition_list> Editaddition_list { get; set; }
        public List<Editdeduction_list> Editdeduction_list { get; set; }
        public List<Editothers_list> Editothers_list { get; set; }
        public string payrun_date { get; set; }
        public string salary_gid { get; set; }
        public double gross_salary { get; set; }
        public double net_salary { get; set; }
        public double ctc { get; set; }
        public double basic_salary { get; set; }
    }


    public class mdlsalarycomponentlist
    {
        public string employee2salarygradetemplate_gid { get; set; }
        public string basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string net_salary { get; set; }
        public string ctc { get; set; }
        public string employee_gid { get; set; }
        public string employee2salarygradetemplatedtl_gid { get; set; }
        public string lop_flag { get; set; }
        public string statutory_flag { get; set; }
        public string salary_mode { get; set; }
        public string salarycomponent_gid { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarycomponent_amount_employer { get; set; }
        public string salarycomponent_amount { get; set; }
        public string othercomponent_type { get; set; }
        public string salarygradetype { get; set; }
        public string componentgroup_name { get; set; }
        public string salarycomponent_name { get; set; }
        public string affect_in { get; set; }
        public string salarycomponent_percentage { get; set; }


    }

    public class mdlemployeedetailslist
    {
        public string designation_gid { get; set; }
        public string designation_name { get; set; }
        public string department_gid { get; set; }
        public string department_name { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string employee_gid { get; set; }
        public string salarygradetype { get; set; }
    }

    public class mdlsalaryid
    {
        public string salary_gid { get; set; }
        public string employee_gid { get; set; }


    }

    public class mdlloanlist
    {
        public string loan_gid { get; set; }
        public string employee_gid { get; set; }
        public string repayment_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string repayment_amount { get; set; }
    }
}