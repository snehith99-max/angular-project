using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlTeleCallerTeamSummary : result
    {
        public List<managerlist> manager_list { get; set; }
        public List<employeelist> employee_list { get; set; }
        public List<Telecallerlist> Telecaller_list { get; set; }
        public List<Telecallerteamlist> Telecallerteam_list { get; set; }
        public List<TelecallerTeamCount_List> TelecallerTeamCount_List { get; set; }
        public List<GetAssignedlist> GetAssignedlist { get; set; }
        public List<GetAssignedlist1> GetAssignedlist1 { get; internal set; }
        public List<GetUnassignedlist> GetUnassignedlist { get; internal set; }
        public List<GetUnassigned> GetUnassigned { get; internal set; }
        public List<detailtelacalllerteam_list1> detailtelacalllerteam_list1 { get; set; }
        public List<GetManagerAssignedlist> GetManagerAssignedlist { get; internal set; }
        //public List<GetManagerUnassignedlist> GetManagerUnassignedlist { get; internal set; }
        public List<GetManagerUnassigned> GetManagerUnassigned { get; internal set; }
        public List<GetManagerUnassignedlist1> GetManagerUnassignedlist1 { get; set; }
        public List<GetManagerAssignedlist1> GetManagerAssignedlist1 { get; set; }
        public List<GetUnassignedlist1> GetUnassignedlist1 { get; set; }
        public string campaign_name { get; set; }
        public int RowCount { get; internal set; }
    }

    public class managerlist : result
    {
        public string assign_manager { get; set; }
    }
    public class employeelist : result
    {
        public string assign_employee { get; set; }
        public string assign_lead { get; set; }

    }
    public class Telecallerlist : result
    {
        public string campaign_gid { get; set; }
        public string campaign_description { get; set; }
        public string branch { get; set; }
        public string mail_id { get; set; }
        public string description { get; set; }
        public string team_name { get; set; }
        public string assigned_total { get; set; }
        public string total_employees { get; set; }
        public string total_managers { get; set; }
        public string campaign_location { get; set; }
        public string campaign_title { get; set; }
        public string branch_gid { get; set; }
        public string campaign_mailid { get; set; }
        public string campaign_prefix { get; set; }

    }
    public class Telecallerteamlist : result
    {
        public string campaign_gid { get; set; }
        public string campaign_description { get; set; }
        public string branch { get; set; }
        public string mail_id { get; set; }
        public string description { get; set; }
        public string team_name { get; set; }
        public string assigned_total { get; set; }
        public string campaign_location { get; set; }
        public string campaign_title { get; set; }
        public string branch_gid { get; set; }
        public string team_manager { get; set; }
        public string campaign_mailid { get; set; }
        public string team_name_edit { get; set; }
        public string description_edit { get; set; }
        public string branch_edit { get; set; }
        public string mail_id_edit { get; set; }
        public string user_name { get; set; }
        public string campaign_name { get; set; }
        public string team_prefix { get; set; }
        public string team_prefix_edit { get; set; }


    }
    public class GetUnassignedlist1 : result
    {

        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetAssignedlist1 : result
    {
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class TelecallerTeamCount_List : result
    {
        public string unassigned_count { get; set; }
        public string assigned_count { get; set; }
        public string team_count { get; set; }
        public string total_count { get; set; }
        public string total_employee { get; set; }
    }
    public class detailtelacalllerteam_list1 : result
    {

        public string employee_gid { get; set; }
        public string campaign_title { get; set; }
        public string campaign_location { get; set; }
        public string employeecount { get; set; }
        public string user { get; set; }
        public string total { get; set; }
        public string newleads { get; set; }
        public string followup { get; set; }
        public string visit { get; set; }
        public string potential { get; set; }
        public string prospect { get; set; }
        public string drop_status { get; set; }
        public string customer { get; set; }
        public string campaign_gid { get; set; }
        public string branch_name { get; set; }
        //public string branch_edit { get; set; }
        public string assign_to { get; set; }

        public string created_by { get; set; }
        public string created_date { get; set; }
        public string campaign_name { get; set; }
        public string user_name { get; set; }
        public string leadbank_name { get; set; }
        public string remarks { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string region_name { get; set; }
        public string source_name { get; set; }
        public string leadbank_gid { get; set; }
        public int Rowcount { get; set; }
    }

    public class MdlSearchParamters1 : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string schedule_remarks { get; set; }
        public string source_name { get; set; }
        public string company_name { get; set; }
        public string industry { get; set; }
        public string region { get; set; }
        public string company { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string existing_customer { get; set; }
        public string message { get; set; }
        public bool status { get; set; }

    }
    public class assignteam_list2 : result
    {

        public string leadbank_gid { get; set; }
        public string campaign_gid { get; set; }
        public string schedule_remarks { get; set; }
        public string lead2campaign_gid { get; set; }

        public string employee_gid { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

        public summary_list2[] summary_list2;



    }
    public class summary_list2 : result
    {
        public string schedule_remarks { get; set; }
        public string leadbank_gid { get; set; }
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetManagerUnassignedlist1 : result
    {

        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetManagerAssignedlist1 : result
    {
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
}