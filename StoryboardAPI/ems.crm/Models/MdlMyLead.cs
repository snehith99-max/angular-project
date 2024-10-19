﻿using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlMyLead : result
    {

        public List<Upcomingvisit_list> Upcomingvisit_list1 { get; set; }
        public List<ExpiredVisit_list> ExpiredVisit_list1 { get; set; }
        public List<myleads_list> myleadslist { get; set; }
        public List<currency_codelist> currencycodelist { get; set; }
        public List<inprogress_list> inprogresslist { get; set; }
        public List<potential_list> potential_list { get; set; }
        public List<customer_list> customerlist { get; set; }
        public List<drop_list1> droplist { get; set; }
        public List<all_list> alllist { get; set; }

        //public List<leadbank_list> leadbank_list { get; set; }
        public List<Source_list> Source_list { get; set; }
        public List<regionname_list> regionname_list { get; set; }
        public List<industryname_list> industryname_list { get; set; }
        public List<company_list> company_list { get; set; }
        public List<country_list1> country_list { get; set; }
        public List<leadbankedit_list> leadbankedit_list { get; set; }
        public List<product_list32> product_list32 { get; set; }
        public List<callresponse_list> callresponse_list { get; set; }
        public List<product_group_list12> product_group_list12 { get; set; }
        public List<Todayvisit_list1> Todayvisit_list1 { get; set; }
        //public List<leadbankbranch_list> leadbankbranch_list { get; set; }
        public List<leadbankcontact_list> leadbankcontact_list { get; set; }
        public List<newSummary_list> newSummary_list { get; internal set; }
        //public List<breadcrumb_list> breadcrumb_list { get; set; }
        public List<call_list> call_list { get; internal set; }
        public List<getMyLeadsCount_List> getMyLeadsCount_List { get; internal set; }
        public List<schedulesummary_list> schedulesummary_list { get; set; }
        public List<Opportunityschedulesummary_list> Opportunityschedulesummary_list { get; set; }
        public List<myleadsassignedteamdropdown_list> myleadsassignedteamdropdown_list { get; set; }
        public List<postmyleadsleadbank_list> postmyleadsleadbank_list { get; set; }
    }



    public class myleads_list : result
    {

        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string lead2campaign_gid { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string contact_details { get; set; }
        public string internal_notes { get; set; }
        public string schedule_remarks { get; set; }
        public string region_name { get; set; }
        public string customer_type { get; set; }
        public string schedule_status { get; set; }
        public string potential_value { get; set; }

        public string schedulelog_gid { get; set; }

    }
    public class currency_codelist : result
    {
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }



    }
    public class newSummary_list : result
    {
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string customer_type { get; set; }
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
        public string internal_notes { get; set; }
        public string potential_value { get; set; }




    }
    public class Todayvisit_list1 : result
    {
        public string leadbank_gid1 { get; set; }
        public string lead2campaign_gid1 { get; set; }
        public string leadbank_name1 { get; set; }
        public string contact_details1 { get; set; }

        public string customer_address1 { get; set; }
        public string region_name1 { get; set; }
        public string schedule_type1 { get; set; }
        public string schedule1 { get; set; }
        public string schedulelog_gid1 { get; set; }
        public string potential_value { get; set; }


        public string schedule_remarks { get; set; }
        public string schedule_status { get; set; }

        //public string schedule_remarks { get; set; }
        public string customer_type { get; set; }

    }
    public class product_list32 : result
    {

        public string product_gid1 { get; set; }
        public string product_name1 { get; set; }



    }

    public class call_list : result
    {
        public string call_remarks { get; set; }
        public string call_count { get; set; }
        public string leadbank_gid { get; set; }
        public string created_date { get; set; }

    }
    public class product_group_list12 : result
    {

        public string productgroup_gid1 { get; set; }
        public string productgroup_name1 { get; set; }
    }
    public class inprogress_list : result
    {

        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string customer_type { get; set; }
        public string lead2campaign_gid { get; set; }
        public string internal_notes { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string call_count { get; set; }
        public string dialed_number { get; set; }
        public string potential_value { get; set; }


    }
    public class potential_list : result
    {

        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string customer_type { get; set; }
        public string lead2campaign_gid { get; set; }
        public string internal_notes { get; set; }
        public string potential_value_count { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string call_count { get; set; }
        public string dialed_number { get; set; }
        public string potential_value { get; set; }


    }
    public class customer_list : result
    {

        public string leadbank_gid { get; set; }
        public string potential_value_count { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string customer_type { get; set; }
        public string internal_notes { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string call_count { get; set; }
        public string dialed_number { get; set; }
        public string potential_value { get; set; }


    }

    public class drop_list1 : result
    {

        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string customer_type { get; set; }
        public string campaign_title { get; set; }
        public string internal_notes { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string leadstage_name { get; set; }
        public string drop_stage { get; set; }
        public string call_count { get; set; }
        public string drop_remarks { get; set; }
        public string potential_value { get; set; }



    }

    public class all_list : result
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
        public string potential_value { get; set; }


    }

    public class customeradd_list : result
    {
        public string leadbank_gid { get; set; }
        public string customername { get; set; }


        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string leadbank_name { get; set; }
        public string contactpersonname { get; set; }
        public string leadbankcontact_name { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string company_website { get; set; }
        public string area_code1 { get; set; }
        public string fax_country_code { get; set; }


        public string fax { get; set; }
        public string credit_days { get; set; }
        public string credit_limit { get; set; }
        public string currency_code { get; set; }
        public string country_code1 { get; set; }
        public string phone2 { get; set; }
        public string customer_address { get; set; }
        public string pin { get; set; }
        public string city { get; set; }
        public string region_name { get; set; }



        public string customer_address1 { get; set; }

        public string customer_city { get; set; }
        public string countrycode { get; set; }
        public string areacode { get; set; }
        public string countrycode1 { get; set; }
        public string areacode1 { get; set; }
        public string country_name { get; set; }
        public string customer_pin { get; set; }
        public string approval_flag { get; set; }
        public string customer_state { get; set; }
        public string phone1 { get; set; }
        public string country_code2 { get; set; }
        public string fax_area_code { get; set; }
        public string area_code2 { get; set; }
        public string country_gid { get; set; }


        public string customer_region { get; set; }

        public string customerbranch_name { get; set; }
        public string customercontact_name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string main_contact { get; set; }
        public string country_code { get; set; }
        public string area_code { get; set; }

        public string state { get; set; }


    }

    public class Source_list : result
    {
        public string source_gid { get; set; }
        public string source_name { get; set; }
    }


    public class company_list : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
    }

    public class country_list : result
    {
        public string country_name { get; set; }
        public string country_gid { get; set; }
    }

    public class regionname_list1 : result
    {
        public string region_name { get; set; }
        public string region_gid { get; set; }
    }

    public class callresponse_list : result
    {
        public string call_response { get; set; }
        public string callresponse_gid { get; set; }
        public string moving_stage { get; set; }
        public string callresponse_code { get; set; }

    }

    public class leadbankedit_list1 : result
    {
        public string leadbank_gid { get; set; }
        public string source_name { get; set; }
        public string region_name { get; set; }
        public string categoryindustry_name { get; set; }
        public string referred_by { get; set; }
        public string leadbankbranch_name { get; set; }

        public string remarks { get; set; }
        public string leadbank_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string designation { get; set; }
        public string email { get; set; }

        public string company_website { get; set; }
        public string mobile { get; set; }

        public string fax_area_code { get; set; }
        public string fax_country_code { get; set; }
        public string fax { get; set; }

        public string leadbank_country { get; set; }

        public string country_code1 { get; set; }
        public string area_code1 { get; set; }
        public string phone1 { get; set; }
        public string country_code2 { get; set; }
        public string area_code2 { get; set; }

        public string phone2 { get; set; }
        public string leadbank_code { get; set; }
        public string leadbank_address1 { get; set; }
        public string leadbank_address2 { get; set; }
        public string leadbank_city { get; set; }
        public string leadbank_state { get; set; }

        public string country_name { get; set; }
        public string approval_flag { get; set; }
        public string categoryindustry_gid { get; set; }
        public string leadbank_pin { get; set; }

    }




    //public class breadcrumb_list3 : result
    //{

    //    public string module_name1 { get; set; }
    //    public string sref1 { get; set; }
    //    public string module_name2 { get; set; }
    //    public string sref2 { get; set; }
    //    public string module_name3 { get; set; }
    //    public string sref3 { get; set; }


    //}

    public class getMyLeadsCount_List : result
    {
        public string todaytask_count { get; set; }
        public string upcoming_counts { get; set; }
        public string newlead_count { get; set; }
        public string prospects_count { get; set; }
        public string potential_count { get; set; }
        public string drop_count { get; set; }
        public string completed_count { get; set; }
        public string allleads_count { get; set; }
        public string potential_value { get; set; }
    }
    public class ExpiredVisit_list1 : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string log_gid { get; set; }
        public string date_of_demo { get; set; }
        public string meeting_time { get; set; }
        public string location { get; set; }
        public string prosperctive_percentage { get; set; }
        public string schedule_remarks { get; set; }
        public string customer_address { get; set; }
        public string region_name { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string ScheduleRemarks { get; set; }
        public string schedule_status { get; set; }
        public string created_date { get; set; }
        public string schedulelog_gid { get; set; }
        public string date_of_demo_online { get; set; }
        public string meeting_time_online { get; set; }
        public string prosperctive_percentage_online { get; set; }
        public string technical_assist { get; set; }
        public string prospective_percentage { get; set; }
        public string product_name { get; set; }
        public string contact_person_online { get; set; }
        public string product_group { get; set; }
        public string demo_remarks { get; set; }
        public string date_of_visit_offline { get; set; }
        public string meeting_time_offline { get; set; }
        public string visited_by { get; set; }
        public string location_offline { get; set; }
        public string meeting_remarks_offline { get; set; }
        public string contact_person_offline { get; set; }
        public string altrnate_person { get; set; }
        public string contact_person { get; set; }

    }

    public class Upcomingvisit_list1 : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }

        public string customer_address { get; set; }
        public string region_name { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }

        public string ScheduleRemarks { get; set; }
        public string schedule_status { get; set; }

        public string postponed_date { get; set; }

        public string meeting_time_postponed { get; set; }
        public string schedulelog_gid { get; set; }

    }

    public class schedulesummary_list : result
    {
        public string log_details { get; set; }
        public string log_legend { get; set; }
        public string leadbank_gid { get; set; }
    }
    public class Opportunityschedulesummary_list : result
    {
        public string log_details { get; set; }
        public string log_legend { get; set; }
        public string leadbank_gid { get; set; }
        public string schedule_status { get; set; }
        public string postponed_date { get; set; }
    }
    public class myleadsassignedteamdropdown_list : result
    {
        public string campaign_title { get; set; }
        public string campaign_gid { get; set; }
    }  
    public class postmyleadsleadbank_list : result
    {
        public string leadbank_gid { get; set; }

        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
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


}
