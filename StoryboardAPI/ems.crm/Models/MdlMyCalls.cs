using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlMyCalls : result
    {
        public List<new_list> new_list { get; set; }
        public List<new_pending_list> new_pending_list { get; set; }
        public List<followup_list> followup_list { get; set; }
        public List<closed_list> closed_list { get; set; }
        public List<drop_list> drop_list { get; set; }
        public List<product_list3> product_list3 { get; set; }
        public List<product_group_list1> product_group_list1 { get; set; }
        public List<schedule_list> schedule_list { get; set; }
        public List<mycallstilescount_list> mycallstilescount_list { get; set; }
        public List<mycallsresponse_list> mycallsresponse_list { get; set; }
        public List<assignedteamdropdown_list> assignedteamdropdown_list { get; set; }
        public List<postleadbank_list> postleadbank_list { get; set; }
        public List<postappointmentmycalls_list> postappointmentmycalls_list { get; set; }
        public List<Postscheduleclose_list> Postscheduleclose_list { get; set; }
        public List<Postschedulepostpone_list> Postschedulepostpone_list { get; set; }
        public List<Postscheduledrop_list> Postscheduledrop_list { get; set; }
        public List<calllog_list> calllog_list { get; set; }
        public List<GetCallLogLead_list> GetCallLogLead_list { get; set; }
    }
    public class GetCallLogLead_list : result
    {
        public string leadbank_gid { get; set; }
        public string calllog_gid { get; set; }
        public string mobile_number { get; set; }
        public string call_response { get; set; }
        public string prospective_percentage { get; set; }
        public string remarks { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string created_date { get; set; }

        

    }
    public class schedule_list : result
    {
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string schedule_type { get; set; }
        public string schedule_time { get; set; }
        public string schedulelog_gid { get; set; }
        public string source_name { get; set; }
        public string schedule_remarks { get; set; }
        public string customer_type { get; set; }
        public string schedule_date { get; set; }
        public string internal_notes { get; set; }
        public string campaign_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string mobile { get; set; }
        public string dialed_number { get; set; }
        public string schedule_status { get; set; }
        public string remarks { get; set; }
        public string appointment_gid { get; set; }
        public string notes_count { get; set; }
        public string leadstage_gid { get; set; }
        
    }


    public class callinginput
    {
        public string user_name { get; set; }
        public string phone_number { get; set; }
        public string didnumber { get; set; }
        public string remarks { get; set; }
        public string individual_gid { get; set; }

    }
    public class calllog_list
    {
        public string leadbank_name { get; set; }
        public string mobile_number { get; set; }
        public string call_response { get; set; }
        public string prospective_percentage { get; set; }
        public string remarks { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }

    }

    //
    public class new_list : result
    {
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string regionname { get; set; }
        public string call_response { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string lead_base { get; set; }
        public string user_gid { get; set; }
        public string prosperctive_percentage { get; set; }
        public string schedule_remarks { get; set; }
        public string call_feedback { get; set; }
        public string dialed_number { get; set; }
        public string customer_type { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string team_gid { get; set; }
        public string appointment_gid { get; set; }
        public string product_name { get; set; }




    }
  
    public class new_pending_list : result
    {

        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string regionname { get; set; }

        public string call_response { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string lead_base { get; set; }
        public string user_gid { get; set; }
      
        public string schedule_remarks { get; set; }
        public string prosperctive_percentage { get; set; }
        public string call_feedback { get; set; }
        public string dialed_number { get; set; }
        public string mobile { get; set; }
        public string internal_notes { get; set; }
        public string appointment_gid { get; set; }
        public string remarks { get; set; }
        public string notes_count { get; set; }

    }
    public class followup_list : result
    {

        public string leadbank_gid { get; set; }
        public string appointment_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string regionname { get; set; }
        public string call_response { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string lead_base { get; set; }
        public string user_gid { get; set; }
       
        public string schedule_remarks { get; set; }
        public string schedule_status { get; set; }
        public string schedule_time { get; set; }
        public string schedule_date { get; set; }
        public string prosperctive_percentage { get; set; }
       
        public string call_feedback { get; set; }
        public string dialed_number { get; set; }




    }
    public class closed_list : result
    {

        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string regionname { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string call_response { get; set; }
        public string lead_base { get; set; }
        public string user_gid { get; set; }
        public string prospective_percentage { get; set; }
        public string schedule_remarks { get; set; }



    }
    public class drop_list : result
    {

        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string regionname { get; set; }
        public string lead_base { get; set; }
        public string call_response { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string user_gid { get; set; }
        public string prospective_percentage { get; set; }
        public string schedule_remarks { get; set; }



    }

    public class product_list3 : result
    {

        public string product_gid { get; set; }
        public string product_name { get; set; }



    }
    public class product_group_list1 : result
    {

        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
    }
    public class mycallstilescount_list : result
    {

        public string schedule_count { get; set; }
        public string newleads_count { get; set; }
        public string followup_count { get; set; }
        public string prospect_count { get; set; }
        public string drop_count { get; set; }
        public string pending_count { get; set; }
        public string alllead_count { get;set; }
        public string upcomingschedule_count { get; set; }


    }
    public class mycallsresponse_list : result
    {

        public string call_response { get; set; }
        public string callresponse_gid { get; set; }
        public string moving_stage { get; set; }
        public string callresponse_code { get; set; }
    }
    public class assignedteamdropdown_list : result
    {

        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
    }
    public class postleadbank_list : result
    {
        public string leadbank_gid { get; set; }
        public string lead_type { get; set; }

        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string schedule_remarks { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
        public string leadbank_state { get; set; }
        public string region_name { get; set; }
        public string source_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string lead_status { get; set; }
        public string assign_to { get; set; }
        public string leadbank_id { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string status { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public getphonenumber2 phone { get; set; }
        public string email { get; set; }
        public string country_code1 { get; set; }
        public string country_code2 { get; set; }

        public string area_code1 { get; set; }
        public string area_code2 { get; set; }
        public string company_website { get; set; }
        public string fax_country_code { get; set; }
        public string fax_area_code { get; set; }
        public string fax { get; set; }
        public string approval_flag { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string leadbank_address2 { get; set; }
        public string leadbank_address1 { get; set; }
        public string leadbank_city { get; set; }
        public string leadbank_region { get; set; }
        public string leadbank_country { get; set; }
        public string leadbank_pin { get; set; }
        public string categoryindustry_gid { get; set; }
        public string referred_by { get; set; }
        public string remarks { get; set; }

        public string region_gid { get; set; }
        public string leadbank_code { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string state { get; set; }

        public string Address { get; set; }

        public string country_name { get; set; }
        public string country_gid { get; set; }
        public string leadbankbranch_name { get; set; }
        public string did_number { get; set; }
        public string zip_code { get; set; }


        public string branch_name { get; set; }
        public string contact_name { get; set; }
        public string desig { get; set; }
        public string mobileno { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
        public string pin { get; set; }


        public string categoryindustry_code { get; set; }
        public string category_desc { get; set; }
        public string categoryindustry_name { get; set; }
        public bool Status { get; set; }
        public string created_flag { get; set; }
        public string team_name { get; set; }
    }


    public class postappointmentmycalls_list : result
    {
        public string leadbank_gid { get; set; }
        public string bussiness_verticle { get; set; }
        public string appointment_timing { get; set; }
        public string lead_title { get; set; }
        public string teamname_gid { get; set; }
        public string employee_gid { get; set; }
        public post_list post_list { get; set; }
    }
    public class post_list : result
    {
        public string name { get; set; }
        public string s_no { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string comment { get; set; }
    }
    public class Postscheduleclose_list : result
    {
        public string schedule_remarks { get; set; }
        public string schedulelog_gid { get; set; }
    }
    public class Postschedulepostpone_list : result
    {
        public string schedule_remarks { get; set; }
        public string postponed_date { get; set; }
        public string meeting_time_postponed { get; set; }
        public string schedulelog_gid { get; set; }
    }
    public class Postscheduledrop_list : result
    {
        public string drop_reason { get; set; }
        public string leadbank_gid { get; set; }
        public string schedulelog_gid { get; set; }
    }
}