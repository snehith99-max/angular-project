using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOtlMstBranch : result
    {
        public List<Prod_list> Prod_list { get; set; }
        public List<assign_list> assign_list { get; set; }
        public List<PostPincode_list> PostPincode_list { get; set; }
        public List<Getassignpincode_code> Getassignpincode_code { get; set; }
        public List<outlet_list> outlet_list { get; set; }        
        public List<GetOtlPincodeSummaryAssign> GetOtlPincodeSummaryAssign { get; set; }
        public List<PostRemovePincode_list> PostRemovePincode_list { get; set; }
        public List<GetAmendProduct_list> GetAmendProduct_list { get; set; }
    }

    public class outlet_list : result
    {

        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string branch { get; set; }
        public string branch_gid { get; set; }
        public string campaign_description { get; set; }
        public string Assigned_product { get; set; }
        public string Assigned_pincode { get; set; }
        public string managercount { get; set; }
        public string outlet_status { get; set; }
        public string mobile_number { get; set; }

    }
    public class Prod_list : result
    {


        public string producttype_name { get; set; }
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
        public string Assigned_product { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_image { get; set; }
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
        public string sku { get; set; }
        public string tax { get; set; }



    }
    public class assign_list : result
    {
        public string campaign_gid { get; set; }
        public prodassign[] prodassign;
        public produnassign[] produnassign;

        public List<product_list> product_list { get; set; }


    }
    public class product_list : result
    {
        public string product_gid { get; set; }
        public string cost_price { get; set; }
        public string mobile { get; set; }
        public string campaign_gid { get; set; }
       
    }
    public class PostPincode_list : result
    {
        public string branch_gid { get; set; }
        public pincodeassing[] pincodeassing;
    }
    public  class pincodeassing : result
    {
        public string pincode_code { get; set; }
        public string pincode_id { get; set; }
    }
    public class prodassign : result
    {
        public string _id { get; set; }
        public string product_gid { get; set; }
        public string cost_price { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }
    public class produnassign : result
    {
        public string _id { get; set; }
        public string product_gid { get; set; }
        public string cost_price { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }
    public class Getassignpincode_code : result
    {
        public string pincode_code { get; set; }
        public string pincode_id { get; set; }
        public string deliverycost { get; set; }
    }
    public class PostRemovePincode_list : result
    {
        public string branch_gid { get; set; }

        public branchunassign[] branchunassign { get; set; }
    }
    public class branchunassign
    {
        public string pincode_id { get; set; }
    }
    public class GetOtlPincodeSummaryAssign : result
    {
        public string pincode_code { get; set; }
        public string pincode_id { get; set; }
        public string created_date { get; set; }
        public string branch_gid { get; set; }
        public string created_by { get; set; }
        public string branch_name { get; set; }
        public string deliverycost { get; set; }
    }
    public class GetAmendProduct_list: result
    {
        public string branch_gid { get; set; } 
        public string product_gid { get; set; } 
        public string branch2product_gid { get; set; } 
        public string product_image { get; set; } 
        public string cost_price { get; set; } 
        public string customerproduct_code { get; set; } 
        public string product_code { get; set; } 
        public string product_name { get; set; } 
        public string product_desc { get; set; } 
    }
}