﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlRegisterLead : result
    {

        public List<Getcompanylistdropdown> Getcompanylistdropdown { get; set; }
        public List<Getcountrynamedropdown> Getcountrynamedropdown { get; set; }
        public List<Getregiondropdown1>  Getregiondropdown1 { get; set; }

        public List<Getindustrydropdown> Getindustrydropdown { get; set; }

        public List<GetSourcetypedropdown> GetSourcetypedropdown { get; set; }

        public List<GetRegisterLeadSummary_list> GetRegisterLeadSummary_list { get; set; }
        public List<GetRegisterLeadSummary_list1> GetRegisterLeadSummary_list1 { get; set; }
        public List<GetRegisterLeadSummary_list2> GetRegisterLeadSummary_list2 { get; set; }

        public List<Registerlead_list> Registerlead_list { get; set; }

       
        public List<Registereditlead_list> Registereditlead_list { get; set; }

        public List<Registereditlead_list> Registerviewlead_list { get; set; }

        public List<leadcontact_list> leadcontact_list { get; set; }

        public List<leadbranch_list> leadbranch_list { get; set; }

        public List<leadaddbranch_list> leadaddbranch_list { get; set; }

        public List<branch_list> branch_list { get; set; }
        public List<RegisterLeadCount_List> RegisterLeadCount_List { get; set; }

        //public List<breadcrumb_list> breadcrumb_list { get; set; }


    }

    public class GetRegisterLeadSummary_list : result
    {
        public string leadbank_gid { get; set; }

        public string customer_type { get; set; }
        public string approval_flag { get; set; }
        public string remarks { get; set; }
        public string leadbank_name { get; set; }
        public string agent_name { get; set; }
        public string lead_status { get; set; }
        public string source_gid { get; set; }
        public string source_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string assign_to { get; set; }
        public string assignedto { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string Status { get; set; }
        public string leadbankcontact_name { get; set; }


    }
    public class GetRegisterLeadSummary_list1 : result
    {
        public string leadbank_gid { get; set; }

        public string customer_type { get; set; }
        public string approval_flag { get; set; }
        public string remarks { get; set; }
        public string leadbank_name { get; set; }
        public string agent_name { get; set; }
        public string lead_status { get; set; }
        public string source_gid { get; set; }
        public string source_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string assign_to { get; set; }
        public string assignedto { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string Status { get; set; }
        public string leadbankcontact_name { get; set; }


    }
    public class GetRegisterLeadSummary_list2 : result
    {
        public string leadbank_gid { get; set; }

        public string customer_type { get; set; }
        public string approval_flag { get; set; }
        public string remarks { get; set; }
        public string leadbank_name { get; set; }
        public string agent_name { get; set; }
        public string lead_status { get; set; }
        public string source_gid { get; set; }
        public string source_name { get; set; }
        public string region_name { get; set; }
        public string contact_details { get; set; }
        public string assign_to { get; set; }
        public string assignedto { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string Status { get; set; }
        public string leadbankcontact_name { get; set; }


    }
    public class Registerlead_list : result
    {
        public string source_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string region_gid { get; set; }
        public string country_gid { get; set; }
        public string categoryindustry_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string categoryindustry_name { get; set; }
        public string region_name { get; set; }
        public string referred_by { get; set; }
        public string source_name { get; set; }
        public string  customer_type { get; set; }

        public string remarks { get; set; }
        public string leadbank_state { get; set; }
        public string company_website { get; set; }

        public string leadbank_address1 { get; set; }

        public string leadbank_address2 { get; set; }

        public string leadbank_pin { get; set; }

        public string created_by { get; set; }
        public string created_date { get; set; }
        public string country { get; set; }
        public string country_name { get; set; }

        public string leadbank_name { get; set; }

        public string leadbank_city { get; set; }
        public string leadbankcontact_name { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public getphonenumber3 phone { get; set; }
        public string designation { get; set; }
        public string phone1 { get; set; }
        public string country_code1 { get; set; }
        public string area_code1 { get; set; }
        public string phone2 { get; set; }
        public string country_code2 { get; set; }
        public string area_code2 { get; set; }
        public string fax_country_code { get; set; }
        public string fax_area_code { get; set; }
        public string fax { get; set; }
        public string leadbank_code { get; set; }

        public string leadbankbranch_name { get; set; }




    }

    public class getphonenumber3
    {
        public string e164Number { get; set; }

    }

    public class leadbranch_list : result

    {
        public string leadbank_gid { get; set; }
        public string leadbankbranch_name { get; set; }
        public string Address { get; set; }
        public string leadbank_city { get; set; }
        public string state { get; set; }
        public string leadbank_country { get; set; }
        public string pincode { get; set; }
    }
    public class branch_list : result
    {
        public string leadbankbranch_name { get; set; }
    }

    public class leadaddbranch_list : result
    {
        public string leadbankbranch_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string designation { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string Address { get; set; }
        public string leadbank_gid { get; set; }
        public string region_name { get; set; }

        public string country_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
    }
    public class leadcontact_list : result
    {
        public string leadbankbranch_name { get; set; }
        public string leadbank_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string designation { get; set; }



    }

    public class Registereditlead_list : result
    {
        public string source_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string region_gid { get; set; }
        public string country_gid { get; set; }
        public string categoryindustry_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string categoryindustry_name { get; set; }
        public string region_name { get; set; }
        public string referred_by { get; set; }
        public string source_name { get; set; }

        public string status { get; set; }
        public string remarks { get; set; }
        public string leadbank_state { get; set; }
        public string company_website { get; set; }

        public string leadbank_address1 { get; set; }

        public string leadbank_address2 { get; set; }

        public string leadbank_pin { get; set; }

        public string created_by { get; set; }
        public string created_date { get; set; }
        public string country { get; set; }
        public string leadbank_country { get; set; }

        public string leadbank_name { get; set; }

        public string leadbank_city { get; set; }
        public string leadbankcontact_name { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string designation { get; set; }
        public string phone1 { get; set; }
        public string country_code1 { get; set; }
        public string area_code1 { get; set; }
        public string phone2 { get; set; }
        public string country_code2 { get; set; }
        public string area_code2 { get; set; }
        public string fax_country_code { get; set; }
        public string fax_area_code { get; set; }
        public string fax { get; set; }





    }




    public class Getcountrynamedropdown : result
    {
        public string country_gid { get; set; }
        public string country_name { get; set; }

    }
    public class Getregiondropdown1 : result
    {
        public string region_gid { get; set; }
        public string region_name { get; set; }

    }
    public class Getindustrydropdown : result
    {
        public string categoryindustry_gid { get; set; }
        public string categoryindustry_name { get; set; }

    }

    public class GetSourcetypedropdown : result
    {
        public string source_gid { get; set; }
        public string source_name { get; set; }

    }

    public class Getcompanylistdropdown : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }

    }
    public class RegisterLeadCount_List : result
    {
        public string distributor_count { get; set; }
        public string retailer_counts { get; set; }
        public string corporate_count { get; set; }
        public string total_count { get; set; }
        public string display_name { get; set; }
        public string lead_count { get; set; }

    }


}


