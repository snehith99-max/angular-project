using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrTrnRateContract:result
    {
        public List<contract_summarylist> contract_summarylist { get; set; }
        public List<Imsvendorrate_list> Imsvendorrate_list { get; set; }
        public List<contractproduct_list> contractproduct_list { get; set; }
        public List<assignproduct_list> Assignproduct_list { get; set; }
        public List<unassignproduct_list> unassignproduct_list { get; set; }
        public List<contractvendor_list> contractvendor_list { get; set; }
        public List<contractamend_list> contractamend_list { get; set; }
        

    }

    public class contract_summarylist : result
    {

       
        public string ratecontract_gid { get; set; }
        public string vendor_gid { get; set; }
        public  List<vendorgid> vendorgid { get; set; }
        public string vendor_companyname { get; set; }
        public string agreement_date { get; set; }
        public string expairy_date { get; set; }
        public string created_date { get; set; }
        public string assigned_product { get; set; }
        public string vendor_code { get; set; }
        public string vendor_details { get; set; }
     
    }

    public class vendorgid
    {
        public string vendor_companyname { get; set; }
        public string vendor_code { get; set; }
        public string vendor_gid { get; set; }
        public string agreement_date { get; set; }
        public string expairy_date { get; set; }

        public List<producttype> producttype { get; set; }
    }


    public class producttype : result
    {
        public string producttype_gid { get; set; }
        public string producttype_code { get; set; }
        public string producttype_name { get; set; }
    }
    public class Imsvendorrate_list : result
    {
        public List<producttype> producttype { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_code { get; set; }
        public string agreement_date { get; set; }
        public string expairy_date { get; set; }
        public List<string> Producttype_name { get; set; }
        public string Producttype_gid { get; set; }

        public List<rcproducttype_list> rcproducttype_list { get; set; }
    }
    public class rcproducttype_list : result
    {
        public string producttype_gid { get; set; }
        public string producttype_name { get; set; }
    }

    public class contractproduct_list : result

    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_gid { get; set; }
        public string cost_price { get; set; }
        public string mrp_price { get; set; }
        public string product_price { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string sku { get; set; }
        public string producttype_name { get; set; }

    }

    public class assignproduct_list:result
    {
        public string ratecontract_gid { get; set; }

        public List<contractassignlist> contractassignlist { get; set; }
    }
    public class contractassignlist : result

    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_gid { get; set; }
        public string cost_price { get; set; }
        public string mrp_price { get; set; }
        public string product_price { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string sku { get; set; }
        public string remarks { get; set; }

    }
    public class unassignproduct_list : result

    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_gid { get; set; }
        public string cost_price { get; set; }
        public string mrp_price { get; set; }
        public string product_price { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string sku { get; set; }

    }
    public class contractvendor_list : result

    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string address { get; set; }
        public string agreement_date { get; set; }
        public string expairy_date { get; set; }
        public string gst_no { get; set; }
    }
    public class contractamend_list : result

    {
        public string product_price { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

    }
}