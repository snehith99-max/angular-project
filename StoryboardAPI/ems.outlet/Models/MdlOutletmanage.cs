using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOutletmanage : result
    {
        public List<campaign_list> campaign_list { get; set; }
        public List<branch_list> branch_list { get; set; }
        public List<GetOTLUnassignedManagerlist> GetOTLUnassignedManagerlist { get; set; }
        public List<GetOTLAssignedManagerlist> GetOTLAssignedManagerlist { get; set; }
        public List<GetOTLAssignedEmplist> GetOTLAssignedEmplist { get; set; }
        public List<GetOTLUnassignedEmplist> GetOTLUnassignedEmplist { get; set; }
        public List<outletassignmanager_list> outletassignmanager_list { get; set; }
        public List<outletassignemployee_list> outletassignemployee_list { get; set; }
        public List<outletCountList> outletCountList { get; set; }
        public List<ouletmanagergrid_list> ouletmanagergrid_list { get; set; }
        public List<outletemployeegrid_list> outletemployeegrid_list { get; set; }
        public List<Getoutletbranchuser_list> Getoutletbranchuser_list { get; set; }
        public List<Outletuser_list> Outletuser_list { get; set; }
        public List<GetOtlEdituserSummary> GetOtlEdituserSummary { get; set; }
    }
    public class GetOtlEdituserSummary : result
    {
        public string temporary_addressgid { get; set; }
        public string permanent_addressgid { get; set; }
        public string permanent_countrygid { get; set; }
        public string temporary_countrygid { get; set; }
        public string entity_gid { get; set; }
        public string branch_gid { get; set; }
        public string permanent_address1 { get; set; }
        public string department_gid { get; set; }
        public string designation_gid { get; set; }
        public string permanent_address2 { get; set; }
        public string permanent_city { get; set; }
        public string permanent_state { get; set; }
        public string permanent_postalcode { get; set; }
        public string permanent_country { get; set; }
        public string temporary_address1 { get; set; }
        public string temporary_address2 { get; set; }
        public string temporary_city { get; set; }
        public string temporary_postalcode { get; set; }
        public string temporary_country { get; set; }
        public string temporary_state { get; set; }
        public string designation_name { get; set; }
        public string user_password { get; set; }
        public string employee_photo { get; set; }
        public string employee_gid { get; set; }
        public string employee_gender { get; set; }
        public string entity_name { get; set; }
        public string identity_no { get; set; }
        public string employee_dob { get; set; }
        public string employee_sign { get; set; }
        public string bloodgroup { get; set; }
        public string employee_image { get; set; }
        public string employee_emailid { get; set; }
        public string employee_mobileno { get; set; }
        public string employee_documents { get; set; }
        public string employee_experience { get; set; }
        public string employee_experiencedtl { get; set; }
        public string employeereporting_to { get; set; }
        public string employment_type { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string user_status { get; set; }
        public string usergroup_gid { get; set; }
        public string usergroup_code { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string approveby_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string approver_name { get; set; }
        public string nationality { get; set; }
        public string nric_no { get; set; }


    }

    public class campaign_list : result
    {   

        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string branch { get; set; }
        public string branch_gid { get; set; }
        public string campaign_description { get; set; }
        public string employeecount { get; set; }
        public string managercount { get; set; }
        public string outlet_status { get; set; }

    }
    public class outletCountList : result
    {
        public string employeecount { get; set; }
        public string outletcount { get; set; }
        public string managercount { get; set; }
    }

    public class branch_list : result {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }

    }
    public class GetOTLUnassignedManagerlist : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }

    }
    public class GetOTLAssignedManagerlist : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }

    }
    public class GetOTLUnassignedEmplist : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }
        public string employee_name { get; set; }

    }
    public class GetOTLAssignedEmplist : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; } 
        public string user_code { get; set; } 
        public string department_name { get; set; } 
        public string employee_name { get; set; }

    }
    public class outletassignmanager_list : result
    {
        public string campaign_gid { get; set; }

        public List<GetOTLUnassignedManagerlist> GetOTLUnassignedManagerlist { get; set; }
        public List<GetOTLAssignedManagerlist> GetOTLAssignedManagerlist { get; set; }
        public string type { get; set; }
    }
    public class assignmanager_list : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string _key3 { get; set; }
    }

    public class outletassignemployee_list : result
    {
        public string campaign_gid { get; set; }

        public List<GetOTLAssignedEmplist> GetOTLAssignedEmplist { get; set; }
        public List<GetOTLUnassignedEmplist> GetOTLUnassignedEmplist { get; set; }

        public outcampaignassignemp[] outcampaignassignemp;
    }
    public class outcampaignassignemp : result
    {
        public string _id { get; set; }
        public string _name { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }
    }

    public class ouletmanagergrid_list : result
    {
        public string employee_gid { get; set; }
        public string manager { get; set; }
    } 
    public class outletemployeegrid_list : result
    {
        public string employee_gid { get; set; }
        public string employee { get; set; }
    }

    public class Getoutletbranchuser_list: result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }
    public class Outletuser_list : result
    {
        public string user_gid { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string user_password { get; set; }
        public string user_status { get; set; }
        public string employee_gid { get; set; }
        public string designation_gid { get; set; }
        public string employee_mobileno { get; set; }
        public string employee_dob { get; set; }
        public string employee_emailid { get; set; }
        public string employee_gender { get; set; }
        public string department_gid { get; set; }
        public string employee_photo { get; set; }
        public string useraccess { get; set; }
        public string engagement_type { get; set; }
        public string biometric_id { get; set; }
        public string attendance_flag { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string user_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string emp_address { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string employee_name { get; set; }
        public string company_name { get; set; }
        public string gender { get; set; }
        public string entity_gid { get; set; }
        public string entity_name { get; set; }
        public string entity_flag { get; set; }
        public string per_address1 { get; set; }
        public string per_address2 { get; set; }
        public string per_country_gid { get; set; }
        public string per_country_name { get; set; }
        public string per_state { get; set; }
        public string per_city { get; set; }
        public string per_postal_code { get; set; }
        public string temp_address1 { get; set; }
        public string temp_address2 { get; set; }
        public string temp_country_gid { get; set; }
        public string temp_country_name { get; set; }
        public string temp_state { get; set; }
        public string temp_city { get; set; }
        public string temp_postal_code { get; set; }
        public string role_name { get; set; }
        public string role_gid { get; set; }
        public string employee_reportingto_name { get; set; }
        public string employee_reportingto { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string baselocation_gid { get; set; }
        public string baselocation_name { get; set; }
        public string user_access { get; set; }
        public string employeereporting_to { get; set; }
        public string marital_status { get; set; }
        public string marital_status_gid { get; set; }
        public string bloodgroup_name { get; set; }
        public string bloodgroup_gid { get; set; }
        public string joining_date { get; set; }
        public DateTime joiningdate { get; set; }
        public string personal_phone_no { get; set; }
        public string personal_emailid { get; set; }
        public string remarks { get; set; }
        public string relive_date { get; set; }
        public string employee_status { get; set; }
        public string hierarchy_level { get; set; }
        public string reportingto { get; set; }
        public string erp_id { get; set; }
        public string employee_externalid { get; set; }
        public string employee_entitychange_flag { get; set; }
        public string subfunction_gid { get; set; }
        public string subfunction_name { get; set; }
        public string api_code { get; set; }

    }
}