using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrMstTaxSegment : result
    {
        public List<TaxSegmentTotalCustomer_list> TaxSegmentTotalCustomer_list { get; set; }
        public List<TaxSegmentCustomer_list> TaxSegmentCustomer_list { get; set; }
        public List<TaxSegmentVendor_list> TaxSegmentVendor_list { get; set; }
        public List<TaxSegmentSummary_list> TaxSegmentSummary_list { get; set; }
        public List<GetCustomerUnassignedlist> GetCustomerUnassignedlist { get; set; }
        public List<GetCustomerassignedlist> GetCustomerassignedlist { get; set; }
        public List<GetVendorUnassignedlist> GetVendorUnassignedlist { get; set; }
        public List<customerassign_list> customerassign_list { get; set; }
        public List<vendorassign_list> vendorassign_list { get; set; }
        public List<GetWithInState_list> GetWithInState_list { get; set; }
        public List<GetInterState_list> GetInterState_list { get; set; }
        public List<GetOverseas_list> GetOverseas_list { get; set; }
        public List<GetUnassignsummary_list> GetUnassignsummary_list { get; set; }
        public List<GetTaxSegmentDropDown_list> GetTaxSegmentDropDown_list { get; set; }
        public List<GetotherSegment_list> GetotherSegment_list { get; set; }
        public List<postunassignedcustomer> postunassignedcustomer { get; set; }
        public string product_gid { get; set; }

    }
    public class TaxSegmentTotalCustomer_list : result
    {
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class TaxSegmentVendor_list : result
    {
        public string total_customer { get; set; }
        public string within_customer { get; set; }
        public string interstate_customer { get; set; }
        public string overseas_customer { get; set; }
        public string other_customer { get; set; }
        public string unassign_customer { get; set; }
    }
    public class TaxSegmentCustomer_list : result
    {
        public string total_customer { get; set; }
        public string within_customer { get; set; }
        public string interstate_customer { get; set; }
        public string overseas_customer { get; set; }
        public string other_customer { get; set; }
        public string unassign_customer { get; set; }
        public string assigncount { get; set; }
    }
    //Vendor List
    public class vendorassign_list : result
    {
        public string taxsegment_gid { get; set; }

        public vendorunassign[] vendorcampaignunassignemp;
        public vendorassign[] vendorcampaignassign;


    }
    public class vendorunassign_list : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string taxsegment_gid { get; set; }

        public vendorunassign[] vendorcampaignunassign;
        public vendorassign[] vendorcampaignassign;
    }
    public class vendorunassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }

    public class vendorassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }
        public string _name { get; set; }

    }
    public class GetVendorUnassignedlist : result
    {
        public string vendor_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string vendor_companyname { get; set; }

    }

    //Customer List
    public class customerassign_list : result
    {
        public string taxsegment_gid { get; set; }

        public customerunassign[] campaignunassignemp;
        public customerassign[] customerassign;


    }
    public class customerunassign_list : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string taxsegment_gid { get; set; }

        public customerunassign[] campaignunassign;
        public customerassign[] campaignassign;
    }
    public class customerunassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }

    public class customerassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }
        public string _name { get; set; }

    }
    public class GetCustomerUnassignedlist : result
    {
        public string customer_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string customer_name { get; set; }

    }
    public class GetCustomerassignedlist : result
    {
        public string customer_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string customer_name { get; set; }
        public string email { get; set; }
        public string customer_country { get; set; }
        public string customer_pin { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string address { get; set; }
        public string contact_info { get; set; }
        public string user_firstname { get; set; }
        public string created_date { get; set; }
        public string customer_region { get; set; }
        public string customer_code  { get; set; }
        public string pricesegment_name { get; set; }
        public string customer_status { get; set; }

    }
    public class TaxSegmentSummary_list : result
    {
        public List<tax_list> tax_list { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment2product_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_code { get; set; }
        public string productuom_name { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_amount { get; set; }
        public string mrp_price { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string taxsegment_description { get; set; }
        public string active_flag { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string customerproduct_code { get; set; }
        public string description { get; set; }
        public string taxsegment_name_edit { get; set; }
        public string taxsegment_description_edit { get; set; }
        public string active_flag_edit { get; set; }
        public string taxsegment_count { get; set; }


    }
    public class tax_list : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
        public string product_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }

    }
    public class GetWithInState_list : result
    {
        public string customer_type { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetInterState_list : result
    {
        public string customer_type { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetOverseas_list : result
    {
        public string customer_type { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetotherSegment_list : result
    {
        public string customer_type { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetUnassignsummary_list : result
    {
        public string customer_type { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string email { get; set; }
        public string customer_pin { get; set; }
        public string contact_info { get; set; }
        public string created_date { get; set; }
        public string user_firstname { get; set; }
        public string customer_region { get; set; }
        public string customer_code { get; set; }
        public string pricesegment_name { get; set; }
        public string country_name { get; set; }
        public List<GetUnassignsummary_list1> GetUnassignsummary_list1 { get; set; }
    }
    public class GetUnassignsummary_list1 : result
    {
        public string customer_gid { get; set; }
    }
    public class GetTaxSegmentDropDown_list : result
    {
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class PostTaxsegment
    {
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string[] customer_gid1 { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public List<GetUnassignsummary_list> GetUnassignsummary_list{ get; set; }

    }

    public class customer_gid1
    {
        public string customer_gid { get; set; }
    }
    public class unassigncustomerchecklist 
    {
        public string customer_gid { get; set; }
    }
    public class postunassignedcustomer : result
    {
        public List<GetCustomerassignedlist> GetCustomerassignedlist { get; set; }
    }
}