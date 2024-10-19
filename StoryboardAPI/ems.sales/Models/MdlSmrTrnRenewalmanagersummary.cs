using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnRenewalmanagersummary : result
    {
        public List<GetRenewalSummary_lists> GetRenewalSummary_lists { get; set; }
        public List<GetRenewalTeam_list> GetRenewalTeam_list {  get; set; }
        public List<GetRenewalTeamEmployee_list> GetRenewalTeamEmployee_list { get; set; }
        public List<GetRenewalTeamRenewals_list> GetRenewalTeamRenewals_list {  get; set; }
        public List<GetMonthlyRenewal_lists> GetMonthlyRenewal_lists {  get; set; }
        public List<GetRenewalForLastSixMonths_List> GetRenewalForLastSixMonths_List { get; set; }

        public List<GetRenewalDetailSummarylist> GetRenewalDetailSummarylist { get; set; }

    }

    public class GetMonthlyRenewal_lists
    {
        public string renewal_month_name { get; set; }
        public string renewal_year { get; set; }
        public string renewal_count { get; set; }
    }

    public class GetRenewalTeamEmployee_list
    {
        public string totalemployee { get; set; }
    }
    public class GetRenewalTeamRenewals_list
    {
        public string total_renewal_status_count { get; set; }
    }

    public class GetRenewalSummary_lists
    {
        public string campaign_gid { get; set; }
        public string team_name { get; set; }
        public string team_branch { get; set; }
        public string renewals_assigned {  get; set; }  
        public string renewals_new { get; set;}
        public string completed_renewals { get; set;}
        public string renewals_follow { get; set; } 
        public string dropped_renewals { get; set; }
        public string assigned_employee { get; set; }
        public string open_count { get;set; }
        public string closed_count { get; set;}
        public string dropped_count { get;set;}
        
    }
    public class GetRenewalTeam_list
    {
        public string renewal_gid { get; set; }
        public string duration { get; set; }
        public string customer_gid { get; set; }
        public string renewal_description { get; set; }
        public string created_by { get; set; }
        public string renewal { get; set; }
        public string renewal_status { get; set; }
        public string renewal_to { get; set; }
        public string user_name { get; set; }
        public string customer_name { get; set; }
        public string salesorder_date { get; set; }
        public string renewal_date { get; set; }
        public string grandtotal { get; set; }
        public string contact_details { get; set; }
        

    }
    public class GetRenewalForLastSixMonths_List : result
    {
        public string months { get; set; }

        public string year { get; set; }
        public string amount { get; set; }
        public string renewalcount { get; set; }
        public string renewal_date { get; set; }
        public string renewal_gid { get; set; }
        public string renewal_status { get; set; }

        public string customer_name { get; set; }


    }
    public class GetRenewalDetailSummarylist : result
    {
        public string renewal_gid { get; set; }
        public string order_agreement_gid { get; set; }
        public string order_agreement_date { get; set; }
        public string duration { get; set; }
        public string customer_gid { get; set; }
        public string renewal_description { get; set; }
        public string created_by { get; set; }
        public string renewal { get; set; }
        public string renewal_status { get; set; }
        public string renewal_to { get; set; }
        public string user_name { get; set; }
        public string customer_name { get; set; }
        public string salesorder_date { get; set; }
        public string renewal_date { get; set; }
        public string Grandtotal { get; set; }
        public string contact_details { get; set; }
    }



}