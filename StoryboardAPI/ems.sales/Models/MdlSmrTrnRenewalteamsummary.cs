using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnRenewalteamsummary : result
    {
        public List<renewalteam_list> renewalteam_list { get; set; }
        public List<renewlassign_list> renewlassign_list { get; set; }
        public List<renewalunassign_list> renewalunassign_list { get; set; }
        public List<GetAssignedlist> GetAssignedlist { get; set; }
        public List<GetUnassignedlist1> GetUnassignedlist1 { get; set; }
        public List<GetManagerUnassignedlist> GetManagerUnassignedlist { get; set; }
        public List<GetManagerAssignedlist> GetManagerAssignedlist { get; set; }
        public List<popup_list> popup_list { get; set; }
        public List<unmappedrenewal_list> unmappedrenewal_list { get; set; }
        public List<renewalemployee_list> renewalemployee_list { get; set; }

    }
    public class mapproduct_lists1 : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string renewal_gid { get; set; }
        public salesproduct_list[] salesproduct_list { get; set; }
        public List<unmappedrenewal_list> unmappedrenewal_list { get; set; }
    }



    public class renewalteam_list : result
        {

            public string TeamCount { get; set; }
            public string team_gid { get; set; }
            public string records_retreived { get; set; }
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
    public class renewlassign_list : result
    {
        public string campaign_gid { get; set; }
        public campaignunassign[] campaignunassign;
        public campaignassign[] campaignassign;


    }

    public class renewalunassign_list : result
    {
        public string campaign_gid { get; set; }
        public string team_gid { get; set; }

        public campaignunassign[] campaignunassign;
        public campaignassign[] campaignassign;
    }
    public class GetAssignedlist : result
    {
        public string campaign_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class GetUnassignedlist1 : result
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
    public class popup_list : result
    {
        public string assign_manager { get; set; }
        public string assign_employee { get; set; }
        public string assign_lead { get; set; }
    }
    public class unmappedrenewal_list : result

    {
        public string salesorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string renewal_gid { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string renewal_date { get; set; }
        public string renewal { get; set; }
        public string Grandtotal { get; set; }
        public string renewal_description { get; set; }
    }

    public class renewalemployee_list : result
    {
        public string user { get; set; }
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string totalrenewal { get; set; }
        public string upcomming { get; set; }
        public string completed { get; set; }
        public string dropped { get; set; }



    }
}
