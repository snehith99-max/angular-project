using Microsoft.SqlServer.Server;
using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace ems.mobile.Models
{
  
    public class MdlCrmDashboard : result
    {
        public List<appointmentvalue_list> Appointmentvalue_list { get; set; }
        public List<appointmetsummary_list> Appointmetsummary_list { get; set; }
        public List<upcomingappointmetsummary_list> UpcomingAppointmetsummary_list { get; set; }
        public List<threesixtycardviewdetails_list> ThreeSixtyData_list { get; set; }
        public List<threesixtycountdetails_list> Threesixtycountdetails_list { get; set; }
        public List<leadstage_list> LeadStage_list { get; set; }

    }

    public class appointmentvalue_list : result
    {
        public string today_count { get; set; }
        public string upcoming_count { get; set; }
        public string expired_count { get; set; }
        public string completed_today_count { get; set; }


    }


    public class appointmetsummary_list : result
    {
        public string appointment_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string lead_title { get; set; }
        public string leadbank_name { get; set; }
        public string lead_contact { get; set; }
        public string appointment_date { get; set; }
        public string appointment_time { get; set; }
        public string region_source { get; set; }
        public string business_vertical { get; set; }


    }


    public class upcomingappointmetsummary_list : result
    {
        public string appointment_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string lead_title { get; set; }
        public string leadbank_name { get; set; }
        public string lead_contact { get; set; }
        public string appointment_date { get; set; }
        public string appointment_time { get; set; }
        public string region_source { get; set; }
        public string business_vertical { get; set; }
    }

    public class threesixtycardviewdetails_list : result
    {
        public string leadbank_gid { get; set; }
        public string customer_gid { get; set; }
        public string leadstage_name { get; set; }
        public string potential_value { get; set; }
        public string leadbank_name { get; set; }
        public string lead_title { get; set; }
        public string appointment_date { get; set; }
        public string created_date { get; set; }
        public string customer_type { get; set; }
        public string leadbankcontact_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string business_vertical { get; set; }
        public string assign_to { get; set; }
        public string lead_contact { get; set; }

    }

    public class threesixtycountdetails_list : result
    {
        public string whatsappcampaign_count { get; set; }
        public string mail_count { get; set; }
        public string sms_count { get; set; }
        public string totalquotation_count { get; set; }
        public string totalquotation_amount { get; set; }
        public string quotationaccepted_count { get; set; }
        public string quotationaccepted_amount { get; set; }
        public string quotationdropped_count { get; set; }
        public string quotationdropped_amount { get; set; }
        public string totalorder_count { get; set; }
        public string totalorder_amount { get; set; }
        public string delevery_count { get; set; }
        public string delevery_amount { get; set; }
        public string orderpending_count { get; set; }
        public string orderpending_amount { get; set; }
        public string totalinvoice_count { get; set; }
        public string totalinvoice_amount { get; set; }
        public string paymentreceived_count { get; set; }
        public string paymentreceived_amount { get; set; }
        public string paymentpending_count { get; set; }
        public string paymentpending_amount { get; set; }

    }


    public class leadstage_list: result
    {
        public string leadstage_name { get; set; }
        public string leadstage_gid { get; set; }
    }

    }


