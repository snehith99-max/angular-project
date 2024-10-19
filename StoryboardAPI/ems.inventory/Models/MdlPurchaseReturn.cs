using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlPurchaseReturn : result
    {
        public List<GetPurchaseReturn_list> GetPurchaseReturn_list { get; set; }
        public List<GetPurchaseReturnbranch_list> GetPurchaseReturnbranch_list { get; set; }
        public List<GetGRNPurchaseReturn_list> GetGRNPurchaseReturn_list { get; set; }
        public List<GetGRNDetailsSummary_list> GetGRNDetailsSummary_list { get; set; }
        public List<PostPurchaseReturn_list> PostPurchaseReturn_list { get; set; }
        public List<GetPurchaseReturnView_list> GetPurchaseReturnView_list { get; set; }
        public List<GetPurchaseReturnViewDetails_list> GetPurchaseReturnViewDetails_list { get; set; }
        public List<getProduct_list> getProduct_list { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string vendor_companyname { get; set; }

    }
    public class GetPurchaseReturn_list: result
    {
        public string purchasereturn_gid { get; set; }
        public string branch_name { get; set; }
        public string branch_prefix { get; set; }
        public string purchasereturn_date { get; set; }
        public string purchasereturn_reference { get; set; }
        public string grn_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string contact { get; set; }
    }
    public class GetPurchaseReturnbranch_list : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }
    public class GetGRNPurchaseReturn_list : result
    {
        public string grn_gid { get; set; }
        public string grn_status { get; set; }
        public string vendor_gid { get; set; }
        public string payment_flag { get; set; }
        public string invoice_flag { get; set; }
        public string overall_status { get; set; }
        public string grn_date { get; set; }
        public string vendor_companyname { get; set; }
    }
    public class GetGRNDetailsSummary_list : result
    {
        public string grn_gid { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string purchaseorderdtl_gid { get; set; }
        public string qty_delivered { get; set; }
        public string qty_rejected { get; set; }
        public string qty_returned { get; set; }
        public string grndtl_gid { get; set; }
        public double qty_purchasereturn { get; set; }
        public string product_remarks { get; set; }
    }
    public class PostPurchaseReturn_list: result
    {
        public  purchaseQTY[] purchaseQTY { get; set; }
        public string branch_gid { get; set; }
        public string vendor_gid { get; set; }
        public string grn_gid { get; set; }
        public string purchasereturn_date { get; set; }
        public string remarks { get; set; }
    }
    public class purchaseQTY 
    {
        public string grn_gid { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string purchaseorderdtl_gid { get; set; }
        public string qty_delivered { get; set; }
        public string qty_rejected { get; set; }
        public string qty_returned { get; set; }
        public string qty_purchasereturn { get; set; }
        public string product_remarks { get; set; }
        public string grndtl_gid { get; set; }
    }
    public class GetPurchaseReturnView_list: result
    {
        public string branch_name { get; set; }
        public string vendor_companyname { get; set; }
        public string purchasereturn_date { get; set; }
        public string purchasereturn_reference { get; set; }
        public string purchasereturn_remarks { get; set; }
    }
    public class GetPurchaseReturnViewDetails_list : result
    {
        public string grn_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string qty_delivered { get; set; }
        public string qty_rejected { get; set; }
        public string qty_returned { get; set; }
        public string productuom_name { get; set; }
    }
    public class getProduct_list
    {
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string qty_returned { get; set; }
        public string qty_rejected { get; set; }
        public string qty_delivered { get; set; }
        public string product_remarks { get; set; }
    }
}