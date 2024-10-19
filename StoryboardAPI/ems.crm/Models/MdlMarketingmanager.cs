﻿using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.crm.Models
{
    public class MdlMarketingmanager : result
    {
        public List<Drop> Drop { get; set; }
        public List<teamdetails> teamdetails { get; set; }
        public List<Customer1> Customer1 { get; set; }
        public List<Potential> Potential { get; set; }
        public List<Prospect> Prospect { get; set; }
        public List<New> New { get; set; }
        public List<upcoming> upcoming { get; set; }
        public List<marketingmanager_lists> marketingmanager_lists { get; set; }
        public List<marketingmanager_lapsedlists> marketingmanager_lapsedlists { get; set; }
        public List<marketingmanager_longestlists> marketingmanager_longestlists { get; set; }
        public List<marketingmanager_totallists> marketingmanager_totallists { get; set; }
        public List<managerDetailTable_lists> managerDetailTable_lists { get; set; }
        public List<GetCampaignmanagerSummary> GetCampaignmanagerSummary { get; set; }
        public List<GetCampaignmanagerTeam> GetCampaignmanagerTeam { get; set; }
        public List<campaignbin_list> campaignbin_list { get; set; }
        public List<schedulesummary_list1> schedulesummary_list1 { get; set; }
        public List<GetTeamNamedropdown> GetTeamNamedropdown { get; set; }
        public List<GetTeamEmployeedropdown> GetTeamEmployeedropdown { get; set; }
        public List<campaigntransfer_list> campaigntransfer_list { get; set; }
        public List<campaignschedule_list> campaignschedule_list { get; set; }
        public List<totaltilecount_lists> totaltilecount_lists { get; set; }
        public List<totallapsedlongest> totallapsedlongest { get; set; }
        public List<M2D> M2D { get; set; }
        public List<chartscount_list> chartscount_list { get; set; }
    }
    public class campaignschedule_list : result
    {
        public string schedule_remarks { get; set; }
        public string schedule_date { get; set; }
        public string schedule_time { get; set; }
        public string schedule_type { get; set; }
        public string assign_user { get; set; }
        public string appointment_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string assignedto_gid { get; set; }
        public campaign_list[] campaign_list;
    }
    public class GetTeamEmployeedropdown : result
    {

        public string employee_gid { get; set; }
        public string user_name { get; set; }
    }
    public class GetTeamNamedropdown : result
    {

        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
    } 
    public class campaigntransfer_list : result
    {
        public string team_name { get; set; }
        public string team_member { get; set; }
        public string assign_user { get; set; }
        public string leadbank_gid { get; set; }
        public string appointment_gid { get; set; }
        public string assignedto_gid { get; set; }

        public campaign_list[] campaign_list;
    }
    public class campaignbin_list : result
    {

        public string campaign_gid { get; set; }
        public string assign_user { get; set; }
        public string leadbank_gid { get; set; }
        public string appointment_gid { get; set; }
        public string drop_remarks { get; set; }
        public string drop_remark1 { get; set; }

        public campaign_list[] campaign_list;
    }
    public class campaign_list : result
    {
        public string call_response { get; set; }
        public string campaign_gid { get; set; }
        public string contact_details { get; set; }
        public string created_by { get; set; }
        public string message { get; set; }
        public string internal_notes { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string leadstage_name { get; set; }
        public string region_name { get; set; }
        public string status { get; set; }


    }
    public class GetCampaignmanagerTeam : result
    {

        public string campaign_title { get; set; }
        public string user_firstname { get; set; }
    }
    public class GetCampaignmanagerSummary : result
    {
        public string leadbank_name { get; set; }
        public string customer_type { get; set; }
        public string call_response { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string internal_notes { get; set; }
        public string created_by { get; set; }
        public string leadstage_name { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string campaign_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string potential_value { get; set; }

    }

    public class managerDetailTable_lists : result
    {
        public string campaign_gid { get; set; }
        public string department_name { get; set; }
        public string assign_to { get; set; }
        public string user { get; set; }
        public string total { get; set; }
        public string newleads { get; set; }
        public string followup { get; set; }
        public string visit { get; set; }
        public string prospect { get; set; }
        public string drop_status { get; set; }
        public string customer { get; set; }
    


    }

    public class marketingmanager_totallists : result
    {

        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string customer_type { get; set; }
        public string internal_notes { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string leadstage_name { get; set; }
        public string call_count { get; set; }
        public string created_by { get; set; }
        public string assign_to { get; set; }
        public string branch_name { get; set; }
        public string emp_team { get; set; }
        public string  assignedto_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string potential_value { get; set; }
        public string drop_remarks { get; set; }
        public string team_prefix { get; set; }
        public string potential_value_count { get; set; }
        public string appointment_gid { get; set; }

    }

    public class marketingmanager_lapsedlists : result
    {

        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set;}
        public string assignedto_gid { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string customer_type { get; set; }
        public string internal_notes { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string leadstage_name { get; set; }
        public string call_count { get; set; }
        public string created_by { get; set; }
        public string assign_to { get; set; }
        public string branch_name { get; set; }
        public string emp_team { get; set; }
        public string created_date { get; set; }
        public string lapsed_count { get; set; }
        public string team_prefix { get; set; }
        public string potential_value { get; set; }
        public string appointment_gid { get; set; }


    }
    public class marketingmanager_longestlists : result
    {

        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string assignedto_gid { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string customer_type { get; set; }
        public string internal_notes { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string leadstage_name { get; set; }
        public string call_count { get; set; }
        public string created_by { get; set; }
        public string assign_to { get; set; }
        public string branch_name { get; set; }
        public string emp_team { get; set; }
        public string created_date { get; set; }
        public string lapsed_count { get; set; }
        public string team_prefix { get; set; }
        public string potential_value { get; set; }
        public string appointment_gid { get; set; }

    }
    public class teamdetails : result
    {
      

        public string team_count { get; set; }
    }

    public class totallapsedlongest : result
    {
        public string total_lapsedcount {get; set;}
        public string total_longestcount { get; set; }
    }

    public class marketingmanager_lists : result
    {
        public string campaign_gid { get; set; }

       public string team_prefix { get; set; }
        public string campaign_title { get; set; }
        public string campaign_location { get; set; }
        public string branch_name { get; set; }
        public string employeecount { get; set; }
        public string assigned_leads { get; set; }
        public string Lapsed_Leads { get; set; }
        public string Longest_Leads { get; set; }
        public string newleads { get; set; }
        public string followup { get; set; }
        public string visit { get; set; }
        public string prospect { get; set; }
        public string drop_status { get; set; }
        public string customer { get; set; }
        public string mtd { get; set; }
        public string ytd { get; set; }
        public string upcoming { get; set; }

    }
    //Total tile count

    public class totaltilecount_lists : result
    {
        public string campaign_gid { get; set; }

        //  public string team_count { get; set; }
        public string campaign_title { get; set; }
        public string campaign_location { get; set; }
        public string branch_name { get; set; }
        public string potential_value { get; set; }
        public string total_employeecount { get; set; }
        public string total_assignedleads { get; set; }
        public string total_LapsedLeads { get; set; }
        public string total_LongestLeads { get; set; }
        public string total_newleads { get; set; }
        public string total_followup { get; set; }
        public string total_potential { get; set; }
        public string total_prospect { get; set; }
        public string total_dropstatus { get; set; }
        public string total_customer { get; set; }
        public string total_mtd { get; set; }
        public string total_ytd { get; set; }
        public string total_upcoming { get; set; }

    }


    public class M2D : result
    {
        public string customer_type { get; set; }
        public string contact { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_date { get; set; }
        public string so_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string branch_name { get; set; }
        public string so_type { get; set; }
        public string Grandtotal { get; set; }
        public string user_firstname { get; set; }
        public string salesorder_status { get; set; }
        public string campaign_title { get; set;}
        public string remarks { get; set; }
        public string leadstage_name { get; set; }
        public string emp_team { get; set; }
        public string team_prefix { get; set; }
        public string potential_value { get; set; }
        public string internal_notes { get; set; }
    }

    public class upcoming : result
    {
        public string schedule_remarks { get; set; }

        public string schedule_status { get; set; }
            public string schedule_status1 { get; set; }
            public string schedule { get; set; }
            public string emp_team { get; set; }
            public string branch_name { get; set; }
            public string user_firstname { get; set; }
            public string leadbank_gid { get; set; }
            public string company_name { get; set; }
            public string leadbankcontact_name { get; set; }
            public string contact_details { get; set; }
            public string customer_type { get; set; }
            public string leadstage_name { get; set; }
            public string campaign_title { get; set; }
            public string source_name { get; set; }
            public string created_by1 { get; set; }
            public string remarks { get; set; }
            public string lead_status { get; set; }
            public string assignedto_gid { get; set; }
            public string lead2campaign_gid { get; set; }
            public string leadbankcontact_gid { get; set; }
            public string source_gid { get; set; }
            public string leadbank_name { get; set; }
            public string Contact { get; set; }
            public bool addtocustomer { get; set; }
            public string designation { get; set; }
            public string mobile { get; set; }
            public string email { get; set; }
            public string created_by { get; set; }
            public string country_code2 { get; set; }
        public string team_prefix { get; set; }
        public string potential_value { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }



    }

    //new//
    public class New : result
    {
        public string emp_team { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string leadbank_gid { get; set; }
        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_title { get; set; }
        public string source_name { get; set; }
        public string created_by1 { get; set; }
        public string remarks { get; set; }
        public string lead_status { get; set; }
        public string assign_to { get; set; }
        public string potential_value { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string Contact { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string created_by { get; set; }
        public string country_code2 { get; set; }
        public string assignedto_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string team_prefix { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }

    }
    //prospect//
    public class Prospect : result
    {
        public string emp_team { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string leadbank_gid { get; set; }
        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_title { get; set; }
        public string source_name { get; set; }
        public string created_by { get; set; }
        public string remarks { get; set; }
        public string lead_status { get; set; }
        public string assign_to { get; set; }
        public string potential_value { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string Contact { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string country_code1 { get; set; }
        public string country_code2 { get; set; }
        public string assignedto_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string team_prefix { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }

    }

    //potential
    public class Potential : result
    {
        public string emp_team { get; set; }
        public string branch_name { get; set; }
        public string potential_value_count { get; set; }
        public string user_firstname { get; set; }
        public string leadbank_gid { get; set; }
        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_title { get; set; }
        public string source_name { get; set; }
        public string created_by { get; set; }
        public string remarks { get; set; }
        public string lead_status { get; set; }
        public string assign_to { get; set; }
        public string potential_value { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string Contact { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string country_code1 { get; set; }
        public string country_code2 { get; set; }
        public string assignedto_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string team_prefix { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }


    }
    //customer//
    public class Customer1 : result
    {
        public string emp_team { get; set; }
        public string potential_value_count { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string leadbank_gid { get; set; }
        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_title { get; set; }
        public string source_name { get; set; }
        public string created_by { get; set; }
        public string remarks { get; set; }
        public string lead_status { get; set; }
        public string assign_to { get; set; }
        public string potential_value { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string Contact { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string country_code1 { get; set; }
        public string country_code2 { get; set; }
        public string assignedto_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string team_prefix { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }

    }
    //drop//
    public class Drop : result
    {
        public string emp_team { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string leadbank_gid { get; set; }
        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_title { get; set; }
        public string source_name { get; set; }
        public string created_by { get; set; }
        public string remarks { get; set; }
        public string lead_status { get; set; }
        public string assign_to { get; set; }
        public string potential_value { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string Contact { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string country_code1 { get; set; }
        public string country_code2 { get; set; }
        public string assignedto_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadassign_to { get; set; }
        public string team_prefix { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }

    }
    public class schedulesummary_list1 : result
    {
        public string log_details { get; set; }
        public string log_legend { get; set; }
        public string leadbank_gid { get; set; }
        public string schedule_status { get; set; }
        public string postponed_date { get; set; }
    }

    public  class chartscount_list
    {
        public  string new_leads { get; set; }
        public  string potentialscount { get; set; }
        public  string newleadsmonth { get; set; }
        public  string potentialsmonth { get; set; }
        public string prospectcount { get; set; }
        public string prospectmonth { get; set; }
        public string ordersmonth { get; set; }
        public string orderscount { get; set; }
        public string quoteorder_amount { get; set; }
        public string salesorder_amount { get; set; }
        public string order_count { get; set; }
        public string quote_count { get; set; }
        public string enquiry_count { get; set; }
        public string leads_count { get; set; }
        public string Months { get; set; }
    }
}