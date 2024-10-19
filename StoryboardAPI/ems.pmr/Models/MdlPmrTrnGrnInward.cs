using System.Collections.Generic;

namespace ems.pmr.Models
{
    public class MdlPmrTrnGrnInward : result
    {
        public List<GetGrnInward_lists> GetGrnInward_lists { get; set; }
        public List<GetEditGrnInward_lists> GetEditGrnInward_lists { get; set; }
        public List<GetEditGrnInwardproduct_lists> GetEditGrnInwardproduct_lists { get; set; }
        public List<Getpurchaseorder_list> Getpurchaseorder_list { get; set; }
        public List<GetGoodInType_list> GetGoodInType_list { get; set; }
        public List<MdlASNPost_list> MdlASNPost_list { get; set; }
        public List<GRNlastsixmonths_list> GRNlastsixmonths_list { get; set; }
        public string ordertogrncount { get; set; }
        public string ordercount { get; set; }


    }
    public class PMRASNSTOCK_list
    {
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }

    }
    public class  PMRASN_list
    {
       
    }
    public class Class1
    {
        public int ID { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object WarningMessage { get; set; }
        public bool AllocatedFromReplen { get; set; }
    }

    public class StockToMintSoft
    {
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }
    }
    public class MdlASNPost_list: result
    {
        public string grn_gid { get; set; }
        public string goodsintypes_id { get; set; }
        public string supplier { get; set; }
    }
    public class GetGrnInward_lists : result
    {
        public string grn_gid { get; set; }
        public string branch_name { get; set; }
        public string grn_date { get; set; }
        public string porefno { get; set; }
        public string po_date { get; set; }
        public string purchaseorder_gid { get; set; }
        public string grnrefno { get; set; }
        public string refrence_no { get; set; }
        public string vendor_companyname { get; set; }
        public string costcenter_name { get; set; }
        public string po_amount { get; set; }
        public string created_date { get; set; }
        public string invoice_flag { get; set; }
        public string dc_no { get; set; }
        public string vendor_code { get; set; }
        public string contact { get; set; }
        public string vendor { get; set; }
        public string vendor_gid { get; set; }
        public string despatch_mode { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string grn_flag { get; set; }
        public string mintsoft_flag { get; set; }
        public string mintsoftasn_id { get; set; }
        public string grn_status { get; set; }
        public string branch_prefix { get; set; }
    }
    public class GetEditGrnInward_lists : result
    {
        public string grn_gid {get; set; }
        public string branch_name { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_address { get; set; }
        public string grn_date { get; set; }
        public string expected_date { get; set; }
        public string vendor_contact_person { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string address { get; set; }
        public string purchaseorder_list { get; set; }
        public string reject_reason { get; set; }
        public string grn_remarks { get; set; }
        public string dc_date { get; set; }
        public string grn_reference { get; set; }
        public string dc_no { get; set; }
        public string gst_number { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_remarks { get; set; }
        public string qc_remarks { get; set; }
        public string productuom_gid { get; set; }
        public string qty_ordered { get; set; }
        public string qty_received { get; set; }
        public string qty_grnadjusted { get; set; }
        public string qty_rejected { get; set; }
        public string location_name { get; set; }
        public string bin_number { get; set; }
        public string user_checkername { get; set; }
        public string user_approvedby { get; set; }
        public string priority_n { get; set; }
        public string purchaseorder_gid { get; set; }
        public string deliverytracking_number { get; set; }
        public string dispatch_mode { get; set; }
        public string no_of_boxs { get; set; }
        public string received_note { get; set; }
    }
    public class GetEditGrnInwardproduct_lists : result
    {
        public string grn_gid { get; set; }
       
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_remarks { get; set; }
        public string qc_remarks { get; set; }
        public string display_field { get; set; }
        public string productuom_name { get; set; }
        public string qty_ordered { get; set; }
        public string qtyreceivedas { get; set; }
        public string qty_received { get; set; }
        public string qty_grnadjusted { get; set; }
        public string qty_rejected { get; set; }
        public string qty_delivered { get; set; }
        public string location_name { get; set; }
        public string bin_number { get; set; }
        public string user_checkername { get; set; }
        public string user_approvedby { get; set; }
        public string priority_n { get; set; }
       
    }
    public class Getpurchaseorder_list : result
    {
        public string purchaseorderdtl_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_ordered { get; set; }
        public string product_price { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string tax { get; set; }
        public string tax_percentage { get; set; }
        public string tax_amount { get; set; }
        public string tax_name2 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_amount2 { get; set; }
        public string product_total { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string total_amount { get; set; }
        public string total_tax { get; set; }
        public string total_discount_amount { get; set; }
        public string addon_amount { get; set; }
        public string freight_charges { get; set; }
        public string buybackorscrap { get; set; }
        public string grand_total { get; set; }
        public string insurance_charges { get; set; }
        public string packing_charges { get; set; }
        public string roundoff { get; set; }
        public string currency_code { get; set; }
    }
    // code by snehith for mintosft asn creation
    //public class ASNList1
    //{
    //    public int WarehouseId { get; set; }
    //    public string POReference { get; set; }
    //    public string EstimatedDelivery { get; set; }
    //    public string GoodsInType { get; set; }
    //    public int Quantity { get; set; }
    //    public string Supplier { get; set; }
    //    public int ProductSupplierId { get; set; }
        
    //    public AsnItem[] Items { get; set; }
    //}

    //public class AsnItem1
    //{
    //    public string ProductId { get; set; }
    //    public string SKU { get; set; }
    //    public int Quantity { get; set; }
    //    public string ExpiryDate { get; set; }
    //}
    public class GetGoodInType_list : result
    {
        public string goodsintypes_name { get; set; }
        public string goodsintypes_id { get; set; }
    }

    public class GRNlastsixmonths_list : result
    {
        public string grn_date { get; set; }
        public string months { get; set; }
        public string ordercount { get; set; }
    }
}
