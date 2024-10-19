using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrMstTaxSummary : result
    {

        public List<assignedProductCount> assignedProductCount { get; set; }
        public List<smrtax_list> smrtax_list { get; set; }
        public List<unmappedproduct_list> unmappedproduct_list { get; set; }
        public List<assignedProductCount_list> assignedProductCount_list { get; set; }
        public List<PostUnassignedProduct> PostUnassignedProduct { get; set; }
        public List<TaxSegmentdtl_list> TaxSegmentdtl_list { get; set; }
    }
    public class mapproduct_lists : result
    {
        public string taxsegment_gid { get; set; }
        public string tax_gid { get; set; }
        public salesproduct_list[] salesproduct_list { get; set; }
        //public List<GetUnassignproduct_list1> GetUnassignproduct_list1 { get; set; }
        public List<unmappedproduct_list> unmappedproduct_list { get; set; }
    }
    public class GetUnassignproduct_list1 : result
    {
        public string product_gid { get; set; }
        public string status { get; set; }
        public string result { get; set; }
    }
    public class assignedProductCount_list : result
    {
        public string tax_name { get; set; }
        public string countproduct_assigned { get; set; }
        public string countproduct { get; set; }
        public string countproduct_unassigned { get; set; }
        public string taxsegment_name { get; set; }
        public string product_name { get; set; }

    }

    public class salesproduct_list
    {

        public string taxsegment_gid { get; set; }
        public string tax_gid { get; set; }
        public string old_inventory_quantity { get; set; }
        public string inventory_quantity { get; set; }
        public string Status { get; set; }
        public string avg_lead_time { get; set; }
        public string batch_flag { get; set; }
        public string cost_price { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string currency_code { get; set; }
        public string expirytracking_flag { get; set; }
        public string grams { get; set; }
        public string lead_time { get; set; }
        public string message { get; set; }
        public string mrp_price { get; set; }
        public string price { get; set; }
        public string product_code { get; set; }
        public string product_desc { get; set; }
        public string product_gid { get; set; }
        public string product_image { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string product_type { get; set; }
        public string productgroup_code { get; set; }
        public string productgroup_name { get; set; }
        public string productgroupname { get; set; }
        public string producttype_name { get; set; }
        public string producttypename { get; set; }
        public string productuom_code { get; set; }
        public string productuom_name { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }
        public string productuomclassname { get; set; }
        public string productuomname { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string serial_flag { get; set; }
        public string shopify_productid { get; set; }
        public string status { get; set; }
        public string status_flag { get; set; }
        public string stockable { get; set; }
        public string vendor_name { get; set; }
        public string weight { get; set; }
        public string weight_unit { get; set; }
        public string vendor { get; set; }
        public string product_status { get; set; }

    }


    public class unmappedproduct_list : result

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
        public string sku { get; set; }

    }
    public class TaxSegmentdtl_list : result
    {
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class assignedProductCount : result

    {
        public string tax_name { get; set; }
        public string taxsegment_name { get; set; }
        public string product_name { get; set; }
    }
    public class smrtax_list : result

    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_prefix { get; set; }
        public string count { get; set; }
        public string percentage { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string tax_segment { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmentedit { get; set; }
        public string editpercentage { get; set; }
        public string split_flag { get; set; }
        public string active_flag { get; set; }
        public string edittax_prefix { get; set; }
        public string splitedit_flag { get; set; }
        public string activeedit_flag { get; set; }

    }
    public class PostUnassignedProduct : result
    {
        public List<unassignproductchecklist> unassignproductchecklist { get; set; }
    }
    public class unassignproductchecklist
    {
        public string taxsegment2product_gid { get; set; }
    }
}