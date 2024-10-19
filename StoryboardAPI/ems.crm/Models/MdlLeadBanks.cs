﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ems.crm.Models.leadbank_list;


namespace ems.crm.Models
{
    public class MdlLeadBank : result
    {
        public List<leadbank_list1> leadbank_list1 { get; set; }
        public List<leadbank_list2> leadbank_list2 { get; set; }
        public List<leadbank_list> leadbank_list { get; set; }
        public List<source_list> source_list { get; set; }
        public List<regionname_list> regionname_list { get; set; }
        public List<industryname_list> industryname_list { get; set; }
        //public List<company_list> company_list { get; set; }
        public List<country_list1> country_list { get; set; }
        public List<leadbankedit_list> leadbankedit_list { get; set; }

        public List<leadexport_list> leadexport_list { get; set; }
        public List<leadbankcontact_list> leadbankcontact_list { get; set; }

        //public List<leadbankbranch_list> leadbankbranch_list { get; set; }
        //public List<leadbankcontact_list> leadbankcontact_list { get; set; }
        //public List<breadcrumb_list1> breadcrumb_list1 { get; set; }
        public List<LeadBankCount_List> LeadBankCount_List { get; set; }
        public List<customertype_list1> customertype_list1 { get; set; }
        public List<leadtype_list> leadtype_list { get; set; }

    }

    public class leadtype_list 
    {
        public string leadtype_name { get; set; }
        public string leadtype_gid { get; set; }

    }

     public class leadbankcontact_list : result
    {
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string leadbankbranch_name { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country_name { get; set; }
        public string pincode { get; set; }

    }



    public class leadexport_list : result
    {
        public string lspath1 { get; set; }
        public string lsname2 { get; set; }
    }

    public class leadimport_list : result
    {
        public string lsCompanyID { get; set; }
        public string lsCompanyName { get; set; }
        public string lsContactPerson { get; set; }
        public string lsDesignation { get; set; }
        public string lsAddress1 { get; set; }
        public string lsAddress2 { get; set; }
        public string lsCity { get; set; }
        public string lsPincode { get; set; }
        public string lsState { get; set; }
        public string lsMobile { get; set; }
        public string lsphone1 { get; set; }
        public string lsphone2 { get; set; }
        public string lsfax { get; set; }
        public string lsEmail { get; set; }
        public string lsWebsite { get; set; }
        public string lsareacode1 { get; set; }
        public string lsareacode2 { get; set; }
        public string lsfaxcountrycode { get; set; }
        public string lsfaxareacode { get; set; }
        public string lsCountry { get; set; }
        public string lsRegion { get; set; }
        public string lsIndustry { get; set; }
        public string lsCategoryIndustry { get; set; }
    }

    public class leadbank_list : result
    {
        public string leadbank_gid { get; set; }

        public string company_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string contact_details { get; set; }
        public string interests { get; set; }
        public string birthday { get; set; }
        public string weddingday { get; set; }
        public string birth_day { get; set; }
        public string wedding_day { get; set; }
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
        public string leadbankcontact_nameedit { get; set; }
        public string designation_edit { get; set; }
        public getphonenumber2 phone_edit { get; set; }
        public string email_edit { get; set; }
        public string phone1_edit { get; set; }
        public string phone2_edit { get; set; }
        public bool Status { get; set; }
        public string created_flag { get; set; }
        public string lead_type { get; set; }
        //public string message { get; set; }

    }

    public class getphonenumber2
    {
        public string e164Number { get; set; }

    }
    public class leadbank_list1 : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_id { get; set; }
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

        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string status { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
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
        //public string message { get; set; }

    }
    public class leadbank_list2 : result
    {
        public string leadbank_gid { get; set; }
        public string company_name { get; set; }
        public string leadbank_id { get; set; }

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

        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string leadbank_name { get; set; }
        public string status { get; set; }
        public bool addtocustomer { get; set; }
        public string designation { get; set; }
        public string mobile { get; set; }
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
        public string lead_type { get; set; }
        //public string message { get; set; }

    }

    public class source_list : result
    {
        public string source_gid { get; set; }
        public string source_name { get; set; }
    }


    public class industryname_list : result
    {
        public string categoryindustry_gid { get; set; }
        public string categoryindustry_name { get; set; }
    }

    //public class company_list : result
    //{
    //    public string leadbank_gid { get; set; }
    //    public string leadbank_name { get; set; }
    //}
    public class country_list1 : result
    {
        public string country_name { get; set; }
        public string country_gid { get; set; }
    }

    public class regionname_list : result
    {
        public string region_name { get; set; }
        public string region_gid { get; set; }
    }

    public class leadbankedit_list : result
    {
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string categoryindustry_gid { get; set; }
        public string region_gid { get; set; }
        public string country_gid { get; set; }

        public string customer_type { get; set; }
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
        public string interests { get; set; }
        public string birth_day { get; set; }
        public string wedding_day { get; set; }

        public string company_website { get; set; }
        public string mobile { get; set; }

        public string fax_area_code { get; set; }
        public string fax_country_code { get; set; }
        public string fax { get; set; }

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
        public string leadbank_pin { get; set; }
        public string customertype_gid { get; set; }
        public string lead_type { get; set; }

    }
    public class leadbankedit_list11 : result
    {
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string categoryindustry_gid { get; set; }
        public string region_gid { get; set; }
        public string country_gid { get; set; }

        public string customer_type { get; set; }
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
        public string leadbank_pin { get; set; }

    }
    public class leadbankedit_list2 : result
    {
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string source_gid { get; set; }
        public string categoryindustry_gid { get; set; }
        public string region_gid { get; set; }
        public string country_gid { get; set; }

        public string customer_type { get; set; }
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
        public string leadbank_pin { get; set; }

    }

    //public class leadbankbranch_list : result
    //{
    //    public string leadbankbranch_name { get; set; }
    //    public string Address { get; set; }

    //    public string leadbankcontact_name { get; set; }

    //    public string leadbank_name { get; set; }
    //    public string city { get; set; }

    //    public string mobile { get; set; }
    //    public string pincode { get; set; }
    //    public string country_name { get; set; }
    //    public string state { get; set; }
    //    public string email { get; set; }

    //    public string designation { get; set; }


    //}

    //public class leadbankcontact_list : result
    //{
    //    public string leadbank_gid { get; set; }
    //    public string leadbankbranch_name { get; set; }
    //    public string Address { get; set; }
    //    public string city { get; set; }
    //    public string state { get; set; }
    //    public string country_name { get; set; }
    //    public string pincode { get; set; }

    //}

    //public class breadcrumb_list1 : result
    //{
    //    public string module_name1 { get; set; }
    //    public string sref1 { get; set; }
    //    public string module_name2 { get; set; }
    //    public string sref2 { get; set; }
    //    public string module_name3 { get; set; }
    //    public string sref3 { get; set; }

    //}
    public class LeadBankCount_List : result
    {
        public string distributor_count { get; set; }
        public string retailer_counts { get; set; }
        public string corporate_count { get; set; }
        public string total_count { get; set; }
        public string corporate_label { get; set; }
        public string distributor_label { get; set; }
        public string retailer_label { get; set; }
        public string display_name { get; set; }
        public string lead_count { get; set; }
    }

    public class customertype_list1 : result
    {
        public string customertype_gid1 { get; set; }
        public string customer_type1 { get; set; }

    }
}

