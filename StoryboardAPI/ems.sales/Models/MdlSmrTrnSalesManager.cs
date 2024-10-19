using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoryboardAPI.Models;

namespace ems.sales.Models
{
    public class MdlSmrTrnSalesManager : result
    {
        public List<saleschart_list> saleschart_list { get; set; }
        public List<totalall_list> totalalllist { get; set; }
        public List<complete_list> completelist { get; set; }
        public List<prospects_list> prospectslist { get; set; }
        public List<potentials_list> potentialslist { get; set; }
        public List<drops_list> dropstatuslist { get; set; }
        public List<teammanagercount_list> teamcount_list { get; set; }
        public List<chartscounts_list1> chartscounts_list1 { get; set; }
        public List<salesteam_list1> Salesteam_list1 { get; set; }

    }

    //overall api for chart

    public class saleschart_list : result
    {
        public string customer_count { get; set; }
        public string months { get; set; }
        public string quotation_count { get; set; }
        public string enquiry_count { get; set; }
        public string order_count { get; set; }

    }

    public class salesteam_list1 : result
    {
        public string campaign_gid { get; set; }

        //  public string team_count { get; set; }
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
        public string potential { get; set; }
        public string ytd { get; set; }
        public string upcoming { get; set; }

    }
    public class chartscounts_list1
    {
        public string customermonthcount { get; set; }
        public string quotationmonthcount { get; set; }
        public string customermonth { get; set; }
        public string quotationmonth { get; set; }
        public string enquirymonthcount { get; set; }
        public string enquirymonth { get; set; }
        public string salesmonth { get; set; }
        public string salesmonthcount { get; set; }
        public string quoteorder_amount { get; set; }
        public string salesorder_amount { get; set; }
        public string order_count { get; set; }
        public string quote_count { get; set; }
        public string enquiry_count { get; set; }
        public string customer_count { get; set; }
        public string Months { get; set; }
    }

    public class totalall_list : result
    {
        public string campaign_gid { get; set; }
        public string customercontact_gid { get; set; }
        public string campaign_title { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string assigned_to { get; set; }
        public string leadbank_name { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string created_by { get; set; }
        public string customer_gid { get; set; }
        public string lead2campaign_gid {  get; set; }
        public string customergid {  get; set; }
    }

    public class complete_list : result
    {
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string assigned_to { get; set; }
        public string leadbank_name { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string created_by { get; set; }
    }


    public class prospects_list : result
    {
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string assigned_to { get; set; }
        public string leadbank_name { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string created_by { get; set; }
    }

    public class potentials_list : result
    {
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string assigned_to { get; set; }
        public string leadbank_name { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string created_by { get; set; }
    }

    public class drops_list : result
    {
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string assigned_to { get; set; }
        public string leadbank_name { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string created_by { get; set; }
    }

    public class teammanagercount_list : result
    {
        public string drop_status { get; set; }
        public string employeecount { get; set; }
        public string prospect { get; set; }
        public string potential { get; set; }
        public string completed { get; set; }
    }
}