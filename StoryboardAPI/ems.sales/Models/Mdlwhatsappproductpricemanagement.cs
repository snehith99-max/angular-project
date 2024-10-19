using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.sales.Models
{
    public class Mdlwhatsappproductpricemanagement : result
    {
       public List<branchsum_list> branchsum_list { get; set; }   
        public List<unassignedproduct_list> unassignedproduct_list{  get; set; }   
        public List<assignedproduct_list> assignedproduct_list{  get; set; }
        public string total_products { get; set; }
        public List<branchproduct_list> branchproduct_list { get; set; }
        public List<branchinsproduct_list> branchinsproduct_list { get; set; }
        public List<branchofsproduct_list> branchofsproduct_list { get; set; }
        public List<mobileconfig_list> mobileconfig_list { get; set; }


    }
    public class branchsum_list :result
    {
        public string branch_gid { get; set; }
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string branch_location { get; set; }
        public string assignedproduct { get; set; }
        public string manager_number { get; set; }
        public string msgsend_manger { get; set; }
        public string owner_number { get; set; }
        public string msgsend_owner { get; set; }
        public string cart_status { get; set; }
    }
    public class unassignedproduct_list
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_desc { get; set; }
        public string product_image { get; set; }
        public string mrp_price { get; set; }
        public string sku { get; set; }

    }
    public class waassign_list : result
    {
        public string branch_gid { get; set; }
        public waprodassign[] waprodassign;
        public List<waproduct_list> waproduct_list { get; set; }


    }
    public class waproduct_list : result
    {
        public string product_gid { get; set; }
        public string mrp_price { get; set; }
        public string mobile { get; set; }
        public string branch_gid { get; set; }
        public string whatsapp_price { get; set; }

    }
    public class waprodassign : result
    {
        public string product_gid { get; set; }
        public string branch2product_gid  { get; set; }
        public string mrp_price { get; set; }
        public string whatsapp_price { get; set; }
    }
  
    public class assignedproduct_list
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_desc { get; set; }
        public string product_image { get; set; }
        public string whatsapp_price { get; set; }
        public string sku { get; set; }
        public string branch_gid { get; set; }
        public string branch2product_gid { get; set; }

    }
    public class whatsappconfiguration2
    {
        public string wacatalouge_id { get; set; }
        public string wachannel_id { get; set; }
        public string waaccess_token { get; set; }
        public string meta_phone_id { get; set; }
        public string waphone_number { get; set; }
        public string waworkspace_id { get; set; }

    }
    public class branchproduct_list : result
    {
        public string branch_gid { get; set; }  
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
        public string branch2product_gid {  get; set; }
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
    public class paymentgatewayconfiguration
    {
        public string razorpay_accname { get; set; }
        public string razorpay_accpwd { get; set; }
        public string stripe_key { get; set; }
        public string payment_gateway { get; set; }
       
    }
    public class branchinsproduct_list
    {
        public string product_name {get; set;}
        public string whatsapp_id { get; set;}
        public string whatsappstock_status { get; set;}

    }
    public class branchofsproduct_list
    {
        public string product_name { get; set; }
        public string whatsapp_id { get; set; }
        public string whatsappstock_status { get; set; }
    }
    public class Mdlstockdetails:result
    {
        public string active_Products { get; set; }
        public string product_added { get; set; }
        public string in_stock { get; set; }
        public string out_of_stock { get; set; }
        public List<branchproduct_list> branchproduct_list { get; set; }


    }
    public class mobileconfig_list :result
    {
        public string manager_number { get; set; }
        public string msgsend_manger { get; set; }
        public string owner_number { get; set; }
        public string msgsend_owner { get; set; }
        public string branch_gid { get; set; }
    }
    public class stripePriceResponses
    {
        public string id { get; set; }

    }
}
