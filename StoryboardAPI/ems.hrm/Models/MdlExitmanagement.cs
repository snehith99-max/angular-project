using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlExitmanagement : result
    {
        public List<GetExitmanagament_list> GetExitmanagament_list { get; set; }
        public List<GetEmployee_list> GetEmployee_list { get; set; }
        public List<GetLeaveDetails_list> GetLeaveDetails_list { get; set; }
        public List<Initiateapproval_list> Initiateapproval_list { get; set; }
        public List<GetInitiateApproval_list> GetInitiateApproval_list { get; set; }
        public List<GetSalaryDetails_list> GetSalaryDetails_list { get; set; }
        public List<GetAddition_list> GetAddition_list { get; set; }
        public List<GetDeduction_list> GetDeduction_list { get; set; }
        public List<GetOther_list> GetOther_list { get; set; }
        public List<PostParma_list> PostParma_list { get; set; }
        public List<GetAddCustodian_list> GetAddCustodian_list { get; set; }
        public List<GetEmployeename_list> GetEmployeename_list { get; set; }
        public MdlExitmanagement()
        {
            GetSalaryDetails_list = new List<GetSalaryDetails_list>();
            GetAddition_list = new List<GetAddition_list>();
            GetDeduction_list = new List<GetDeduction_list>();
            GetOther_list = new List<GetOther_list>();
            GetEmployeename_list = new List<GetEmployeename_list>();
        }
        
    }
    
    public class GetExitmanagament_list : result
    {
        public string exitemployee_gid { get; set; }    
        public string created_date { get; set; }    
        public string employee_name { get; set; }    
        public string user_code { get; set; }    
        public string branch_name { get; set; }    
        public string department_name { get; set; }    
        public string joining_date { get; set; }    
        public string overall_status { get; set; }    
        public string designation_name { get; set; }    
        public string remarks { get; set; }    
    }

    public class GetEmployee_list : result
    {
        public string employee_name { get; set; }
        public string details_employee { get; set; }
    }

    public class GetLeaveDetails_list : result
    {
        public string Duration { get; set; }
        public string LOP { get; set; }
        public string OD { get; set; }
        public string Permission { get; set; }
        public string CompOff { get; set; }
    }
    public class Initiateapproval_list : result
    {
        public string manageremployee_gid { get; set; }
        public string department_name { get; set; }
        public string department_gid { get; set; }
        public string manager_name { get; set; }
    }
    public class GetInitiateApproval_list: result
    {
        public string exitemployeedtl_gid { get; set; }
        public string department_gid { get; set; }
        public string department_manager { get; set; }
        public string department_name { get; set; }
        public string exit_status { get; set; }
    }
    public class GetSalaryDetails_list : result
    {
        public string payment_gid { get; set; }
        public string salary_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string basic_salary { get; set; }
        public string earnedbasic_salary { get; set; }
        public string gross_salary { get; set; }
        public string net_salary { get; set; }
    }
    public class GetAddition_list : result
    {
        public string salary_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string earned_salarycomponent_amount { get; set; }
    }
    public class GetDeduction_list : result
    {
        public string salary_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string earned_salarycomponent_amount { get; set; }
    }
    public class GetOther_list : result
    {
        public string salary_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string earned_salarycomponent_amount { get; set; }
    }
    public class PostParma_list : result
    {
        public string exitemployee_gid { get; set; }
        public string manager_name { get; set; }
        public string editor_content { get; set; }
    }
    public class GetAddCustodian_list : result
    {
        public string asset_gid { get; set;}
        public string assetref_no { get; set;}
        public string asset_name { get; set;}
        public string asset_status { get; set;}
    }
    public  class GetEmployeename_list : result
    {
        public string employee_gid { get; set; }
        public string template_content { get; set; }

    }
}

