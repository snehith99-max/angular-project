﻿using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{

    public class MdlProduct : result
    {
        public List<product_list> product_list { get; set; }
        public List<Getproducttypedropdown> Getproducttypedropdown { get; set; }
        public List<Getproductgroupdropdown> Getproductgroupdropdown { get; set; }
        public List<Getproductunitclassdropdown> Getproductunitclassdropdown { get; set; }
        public List<Getproductunitdropdown> Getproductunitdropdown { get; set; }
        public List<Getcurrencydropdown> Getcurrencydropdown { get; set; }
        public List<Getcountrydropdown> Getcountrydropdown { get; set; }
        public List<GetEditProductSummary> GetEditProductSummary { get; set; }
        public List<productexport_list> productexport_list { get; set; }
        public List<GetProductUnit> GetProductUnit { get; set; }

        public List<product_images> product_images { get; set; }
        public List<GetViewProductSummary> GetViewProductSummary { get; set; }
        public List<shopifyproductlist> shopifyproductlist { get; set; }

        public List<productpost_list> productpost_list { get; set; }
        public List<productinventory_list> productinventory_list { get; set; }

        public List<shopifyproductmove_list> shopifyproductmove_list { get; set; }
        public List<productprice_list> productprice_list { get; set; }
        public List<productquantity_list> productquantity_list { get; set; }
        public List<updateshopifyimage_list> updateshopifyimage_list { get; set; }
        public string active_Products {  get; set; }
        public string product_added {  get; set; }
        public string in_stock {  get; set; }
        public string out_of_stock {  get; set; }


    }

    public class shopifyproductImage_list
    {
        public Image image { get; set; }
    }

    

    public class updateshopifyimage_list : result
    {

        public string path { get; set; }
        public string shopify_productid { get; set; }
    }

    public class productpost_list
    {
        public Product product { get; set; }
        public string whatsapp_id { get; set; }
    }

    public class locationlist
    {
        public Location1[] locations { get; set; }
    }

    public class Location1
    {
        public long id { get; set; }
        public string name { get; set; }
        public object address1 { get; set; }
        public object address2 { get; set; }
        public object city { get; set; }
        public object zip { get; set; }
        public object province { get; set; }
        public string country { get; set; }
        public object phone { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public object province_code { get; set; }
        public bool legacy { get; set; }
        public bool active { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string localized_country_name { get; set; }
        public object localized_province_name { get; set; }
    }

    public class productquantity_list : result
    {

        public string inventory_item_id { get; set; }
        public string inventory_quantity { get; set; }
        public string shopify_productid { get; set; }



    }

    public class productprice_list : result
    {
        public string shopify_productid { get; set; }
        public string variant_id { get; set; }
        public string price { get; set; }
        public string attribute { get; set; }


    }
    public class shopifyproductmove_list : result 
    {
        public shopifyproduct_lists[] shopifyproduct_lists { get; set; }
    }

    public class shopifyproduct_lists
    {

        public string inventory_item_id { get; set; }
        public string variant_id { get; set; }
        public string old_inventory_quantity { get; set; }
        public string inventory_quantity { get; set; }
        public string Status { get; set; }
        public string avg_lead_time { get; set; }
        public string sku { get; set; }
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

    public class getproduct
    {

        public bool status { get; set; }
    }

    public class shopifyproductlist
    {
        public Product[] products { get; set; }
    }

    public class Product
    {
        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string vendor { get; set; }
        public string product_type { get; set; }
        public DateTime created_at { get; set; }
        public string handle { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? published_at { get; set; }
        public string template_suffix { get; set; }
        public string published_scope { get; set; }
        public string tags { get; set; }
        public string status { get; set; }
        public string admin_graphql_api_id { get; set; }
        public Variant[] variants { get; set; }
        public Option[] options { get; set; }
        public Image1[] images { get; set; }
        public Image image { get; set; }

    }

    public class Image
    {
        public long id { get; set; }
        public object alt { get; set; }
        public int position { get; set; }
        public long product_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string admin_graphql_api_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string src { get; set; }
        public long?[] variant_ids { get; set; }
    }

    public class Variant
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public int position { get; set; }
        public string inventory_policy { get; set; }
        public string compare_at_price { get; set; }
        public string fulfillment_service { get; set; }
        public string inventory_management { get; set; }
        public string option1 { get; set; }
        public object option2 { get; set; }
        public object option3 { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool taxable { get; set; }
        public string barcode { get; set; }
        public int grams { get; set; }
        public long? image_id { get; set; }
        public float weight { get; set; }
        public string weight_unit { get; set; }
        public long inventory_item_id { get; set; }
        public int inventory_quantity { get; set; }
        public int old_inventory_quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string admin_graphql_api_id { get; set; }

    }

    public class Option
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string name { get; set; }
        public int position { get; set; }
        public string[] values { get; set; }

    }

    public class Image1
    {
        public long id { get; set; }
        public object alt { get; set; }
        public int position { get; set; }
        public long product_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string admin_graphql_api_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string src { get; set; }
        public long?[] variant_ids { get; set; }
    }


    public class productexport_list : result
    {


        public string lspath1 { get; set; }


        public string lsname { get; set; }






    }
    public class GetProductUnit
    {
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }


    }
    public class product_images : result
    {
        public string product_gid { get; set; }
        public string product_image { get; set; }
        public string name { get; set; }
    }

    public class shopifyproduct_list : result
    {

        public string shopify_productid { get; set; }

        public string product_name { get; set; }

        public string product_desc { get; set; }
        public string vendor { get; set; }
        public string producttype_name { get; set; }
        public string product_status { get; set; }




    }
    public class product_list : result
    {

        public string whatsapp_id { get; set; }
        public string whatsappstock_status { get; set; }
        public string option1 { get; set; }
        public string inventory_item_id { get; set; }
        public string product_count { get; set; }
        public string variant_id { get; set; }
        public string old_inventory_quantity { get; set; }
        public string inventory_quantity { get; set; }
        public string weight { get; set; }
        public string sku { get; set; }
        public string weight_unit { get; set; }
        public string grams { get; set; }
        public string price { get; set; }
        public string status_flag { get; set; }
        public string vendor_name { get; set; }
        public string product_image { get; set; }
        public string producttype_name { get; set; }
        public string shopify_productid { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string product_price { get; set; }
        public string whatsapp_price { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string productuomclass_code { get; set; }
        public string productuom_code { get; set; }
        public string productuomclass_name { get; set; }
        public string stockable { get; set; }
        public string productuom_name { get; set; }
        public string product_type { get; set; }
        public string Status { get; set; }
        public string serial_flag { get; set; }
        public string lead_time { get; set; }

        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

        public string product_desc { get; set; }
        public string currency_code { get; set; }
        public string avg_lead_time { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string batch_flag { get; set; }
        public string productgroupname { get; set; }
        public string producttypename { get; set; }
        public string productuomclassname { get; set; }
        public string productuomname { get; set; }



    }
    public class productinventory_list : result
    {


        public string inventory_item_id { get; set; }
        public string variant_id { get; set; }
        public string inventory_quantity { get; set; }
        public string old_inventory_quantity { get; set; }
        public string status_flag { get; set; }
        public string vendor_name { get; set; }
        public string product_image { get; set; }
        public string producttype_name { get; set; }
        public string shopify_productid { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string product_price { get; set; }

        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string productuomclass_code { get; set; }
        public string productuom_code { get; set; }
        public string productuomclass_name { get; set; }
        public string stockable { get; set; }
        public string productuom_name { get; set; }
        public string product_type { get; set; }
        public string Status { get; set; }
        public string serial_flag { get; set; }
        public string lead_time { get; set; }

        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

        public string product_desc { get; set; }
        public string currency_code { get; set; }
        public string avg_lead_time { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string batch_flag { get; set; }
        public string productgroupname { get; set; }
        public string producttypename { get; set; }
        public string productuomclassname { get; set; }
        public string productuomname { get; set; }



    }
    public class GetViewProductSummary : result
    {

        public string currency_code { get; set; }
        public string batch_flag { get; set; }
        public string serial_flag { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string product_desc { get; set; }
        public string avg_lead_time { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string producttype_name { get; set; }
        public string productuomclass_name { get; set; }
        public string product_image { get; set; }


    }
    public class GetEditProductSummary : result
    {

        public string id { get; set; }
        public string currency_code { get; set; }
        public string vendor_name { get; set; }
        public string product_status { get; set; }
        public string product_type { get; set; }
        public string shopify_productid { get; set; }
        public string batch_flag { get; set; }
        public string serial_flag { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string product_desc { get; set; }
        public string avg_lead_time { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }

        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string producttype_name { get; set; }

        public string producttype_gid { get; set; }
        public string productuomclass_name { get; set; }

        public string productuomclass_gid { get; set; }


    }
    public class Getcurrencydropdown : result
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }


    }

    public class Getcountrydropdown : result
    {
        public string country_code { get; set; }
        public string country_gid { get; set; }


    }
    public class Getproductunitdropdown : result
    {
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }


    }
    public class Getproductunitclassdropdown : result
    {
        public string productuomclass_gid { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }

    }
    public class Getproducttypedropdown : result
    {
        public string producttype_name { get; set; }
        public string producttype_gid { get; set; }

    }
    public class Getregiondropdown : result
    {
        public string region_name { get; set; }
        public string region_gid { get; set; }

    }
    public class Getproductgroupdropdown : result
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }

    }
    public class whatsappconfiguration1
    {
        public string wacatalouge_id { get; set; }
        public string wachannel_id { get; set; }
        public string waaccess_token { get; set; }
        public string meta_phone_id { get; set; }
        public string waphone_number { get; set; }
        public string waworkspace_id { get; set; }

    }

}