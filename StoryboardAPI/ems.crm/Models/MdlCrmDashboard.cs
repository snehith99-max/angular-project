using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlCrmDashboard : result
    {
        public List<getDashboardCount_List> getDashboardCount_List { get; set; }
        public List<getDashboardQuotationAmt_List> getDashboardQuotationAmt_List { get; set; }
        public List<getleadbasedonemployee_List> getleadbasedonemployee_List { get; set; }
        public List<socialmedialeadcount> socialmedialeadcount { get; set; }
        public List<crmtilescount_list> crmtilescount_list { get; set; }
        public List<crmleadchart_list> crmleadchart_list { get; set; }
        public List<campaignsentchart_list> campaignsentchart_list { get; set; }
        public List<leadstagechart_list> leadstagechart_list { get; set; }
        public List<crmregionchart_list> crmregionchart_list { get; set; }
        public List<crmsourcechart_list> crmsourcechart_list { get; set; }
        public List<appointmentcount_list> appointmentcount_list { get; set; }

    }
    public class getDashboardCount_List : result
    {
        public string mycalls_count { get; set; }
        public string myleads_count { get; set; }
        public string myappointments_count { get; set; }
        public string assignvisit_count { get; set; }
        public string completedvisit_count { get; set; }
        public string shared_proposal { get; set; }
        public string completedorder_count { get; set; }
        public string totalorder_count { get; set; }
        public string total_count { get; set; }
        public string completedorder_count1 { get; set; }
        public string cancel_count { get; set; }
        public string product_count { get; set; }
        public string rejected_count { get; set; }
        public string total_count1 { get; set; }
    }
    public class getDashboardQuotationAmt_List : result
    {
        public string year { get; set; }
        public string month_name { get; set; }
        public string total_amount { get; set; }
    }
    public class appointmentcount_list : result
    {
        public string appointment_month { get; set; }
        public string appointment_leadcount { get; set; }
    }
    public class getleadbasedonemployee_List : result
    {
        public string lead_year { get; set; }
        public string lead_monthname { get; set; }
        public string lead_count { get; set; }
    }
    public class socialmedialeadcount
    {
        public string whatsapp_count { get; set; }
        public string mail_count { get; set; }
        public string shopify_count { get; set; }
        public string source_name { get; set; }
        public string source_count { get; set; }
    }

    public class crmtilescount_list
    {
        public string total_count { get; set; }
        public string mtd_count { get; set; }
        public string mtd_month { get; set; }
        public string ytd_count { get; set; }
        public string customer_count { get; set; }
        public string ytd_year { get; set; }
    }

    public class crmleadchart_list
    {
        public string lead_count { get; set; }
        public string months { get; set; }
        public string customer_count { get; set; }

    }

    public class campaignsentchart_list
    {
        public string months { get; set; }
        public string mailsent_count { get; set; }
        public string whatsappsent_count { get; set; }

    }
    public class leadstagechart_list
    {
        public string months_detail { get; set; }
        public string prospects { get; set; }
        public string potential { get; set; }
        public string drop_leads { get; set; }
        public string customer { get; set; }

    }
    public class crmregionchart_list
    {
        public string region_name { get; set; }
        public string region_count { get; set; }

    }
    public class crmsourcechart_list
    {
        public string source_count { get; set; }
        public string source_name { get; set; }
     

    }


}