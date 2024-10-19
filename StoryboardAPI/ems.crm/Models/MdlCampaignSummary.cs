﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlCampaignSummary : result
    {

        public List<marketingteam_list> marketingteam_list { get; set; }

        public List<team_list> team_list { get; set; }

        public List<branch_list1> branch_list1 { get; set; }
        public List<marketingteam_list1> marketingteam_list1 { get; set; }
        public List<GetUnassignedlist> GetUnassignedlist { get; set; }
        public List<GetUnassigned> GetUnassigned { get; set; }

       public List<GetAssignedlist> GetAssignedlist { get; set; }
        public List<GetManagerUnassigned> GetManagerUnassigned { get; set; }
        public List<campaignassign_list> campaignassign_list { get; set; }
        public List<campaignunassign_list> campaignunassign_list { get; set; }
        public List<GetManagerUnassignedlist> GetManagerUnassignedlist { get; set; }
        public List<dropdown_list1> dropdown_list1 { get; set; }
        public List<assign_list> assign_list { get; set; }

        public List<GetManagerAssignedlist> GetManagerAssignedlist { get; set; }
        public List<MarketingTeamCount_List> MarketingTeamCount_List { get; set; }
        public List<popup_list> popup_list { get; set; }
        public int RowCount { get; internal set; }

        public string campaign_name { get; set; }
    }
    public class campaignassign_list : result
    {
        public string campaign_gid { get; set; }
        public campaignunassign[] campaignunassign;
        public campaignassign[] campaignassign;


    }
    public class campaignassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }

    public class campaignunassign_list : result
    {
        public string campaign_gid { get; set; }
        public string team_gid { get; set; }

        public campaignunassign[] campaignunassign;
        public campaignassign[] campaignassign;
    }
    public class campaignunassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }
    public class GetUnassignedlist : result
    {
        
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetUnassigned : result
    {

        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    
    public class GetAssignedlist : result
    {
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetManagerUnassigned : result
    {
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetManagerUnassignedlist : result
    {

        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetManagerAssignedlist : result
    {
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }

    public class marketingteam_list : result
        {

            public string TeamCount { get; set; }
            public string team_gid { get; set; }
            public string records_retreived{ get; set; }
            public string campaign_location { get; set; }
            public string employee_gid { get; set; }
            public string campaign_gid { get; set; }
            public string user_name { get; set; }
            public string campaign_name { get; set; }
            public string branch_gid { get; set; }
            public string team_name { get; set; }
            public string description { get; set; }
            public string branch { get; set; }
            public string campaign_team { get; set; }
            public string branch_name { get; set; }
            public string user_firstname { get; set; }  
            public string mail_id { get; set; }
            public string team_manager { get; set; }
            public string team_name_edit { get; set; }
            public string description_edit { get; set; }
            public string branch_edit { get; set; }
            public string mail_id_edit { get; set; }
            public string assigned_total { get; set; }
            public string created_by { get; set; }
            public string created_date { get; set; }
            public string customer_type { get; set; }
            public int Rowcount { get; set; }
            public string source_name { get; set; }
            public string region_name { get; set; }
            public string contact_details { get; set; }
            public string remarks { get; set; }
            public string leadbank_name { get; set; }
            public string leadbank_gid { get; set; }
            public string total_managers { get; set; }
            public string total_employees { get; set; }
            public string team_prefix_edit { get; set; }
            public string team_prefix { get; set; }






    }
    public class branch_list1 : result
        {
            public string branch_gid { get; set; }
            public string branch_name { get; set; }
            public string campaign_location { get; set; }

        }
        public class team_list : result
        {

            public string employee_gid { get; set; }
            public string team_manager { get; set; }
            public string campaign_location { get; set; }

        }
        public class marketingteam_list1 : result
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



    }
        public class assignteam_list1 : result
        {

            public string leadbank_gid { get; set; }
            public string campaign_gid { get; set; }
            public string schedule_remarks { get; set; }
            public string lead2campaign_gid { get; set; }

            public string employee_gid { get; set; }
            public string created_by { get; set; }
            public string created_date { get; set; }

            public summary_list1[] summary_list1;



        }
        public class summary_list1 : result
        {
            public string schedule_remarks { get; set; }
            public string leadbank_gid { get; set; }
             public string campaign_gid { get; set; }
         public string employee_gid { get; set; }
    }

        public class dropdown_list1 : result
        {

            public string source_gid { get; set; }
            public string source_name { get; set; }
            public string region_gid { get; set; }
            public string region_name { get; set; }
            public string categoryindustry_name { get; set; }
            public string categoryindustry_gid { get; set; }



        }
        public class assign_list : result
        {

            public string campaign_location { get; set; }
            public string employee_gid { get; set; }
            public string campaign_gid { get; set; }
            public string branch_gid { get; set; }
            public string team_name { get; set; }
            public string description { get; set; }
            public string branch { get; set; }
            public string campaign_team { get; set; }
            public string branch_name { get; set; }
            public string user_firstname { get; set; }
            public string mail_id { get; set; }
            public string team_manager { get; set; }
            public string team_name_edit { get; set; }
            public string description_edit { get; set; }
            public string branch_edit { get; set; }
            public string mail_id_edit { get; set; }
            public string assigned_total { get; set; }
            public string created_by { get; set; }
            public string created_date { get; set; }

            public string source_name { get; set; }
            public string region_name { get; set; }
            public string contact_details { get; set; }
            public string remarks { get; set; }
            public string leadbank_name { get; set; }
            public string leadbank_gid { get; set; }



        }

    public class MdlSearchParamters : result
    {
        public string campaign_gid   { get; set; }
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
        public string customer_type { get; set; }
        public string source { get; set; }
        public string region_name { get; set; }
        public string message { get; set; }
        public bool status { get; set; }

    }

    public class MarketingTeamCount_List : result
    {
        public string unassigned_count { get; set; }
        public string assigned_count { get; set; }
        public string team_count { get; set; }
        public string total_count { get; set; }
        public string total_employee { get; set; }
    }
    public class popup_list : result
    {
        public string assign_manager { get; set; }
        public string assign_employee { get; set; }
        public string assign_lead { get; set; }
    }

}