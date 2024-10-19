using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    
    public class MdlPmrTrnGrn : result
    {
        public List<Getgrn_lists> Getgrn_lists { get; set; }
        public List<grn_lists> grn_lists { get; set; }
        public List<addgrn_list> addgrn_list { get; set; }
        public List<addgrn_lists> addgrn_lists { get; set; }
        public List<grnproduct_lists> grnproduct_lists { get; set; }

        

    }

   
        public class Getgrn_lists : result
    {
        public string purchaseorder_date { get; set; }
        public string purchaseorder_gid { get; set; }
        public string porefno { get; set; }
        public string purchaseorder_status { get; set; }
        public string vendor_companyname { get; set; }
        public string costcenter_name { get; set; }
        public string department_name { get; set; }
        public string created_by { get; set; }
        public string branch_name { get; set; }
        public string contact_info { get; set; }
        public string purchaseorder_flag { get; set; }
        public string branch_prefix { get; set; }
       

    }

    public class grn_lists : result
    {
        public string branch_name { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string address { get; set; }
        public string purchaseorder_gid { get; set; }
        public string grn_gid { get; set; }
        public string user_firstname { get; set; }
        public string user_firstname1 { get; set; }
        public string purchaseorder_date { get; set; }
        public string expected_date { get; set; }
        public string tax_number { get; set; }
        public string mintsoft_flag { get; set; }

    }


    public class addgrn_list : result
    {
        public string productgroup_name { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_ordered { get; set; }
        public string qty_received { get; set; }
        public string qty_grnadjusted { get; set; }
        public string qty_delivered { get; set; }
        public string qty_free { get; set; }
        public string product_code { get; set; }
        public string producttype_name { get; set; }
        public string display_field_name { get; set; }

    }

    public class addgrn_lists : result
    {
        public string branch_name { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string address { get; set; }
        public string grn_gid { get; set; }
        public string dc_no { get; set; }
        public string priority_flag {  get; set; }
        public string invoiceref_no { get; set; }
        public string grn_date { get; set; }
        public string expected_date { get; set; }
        public string dc_date { get; set; }
        public string invoice_date { get; set; }
        public string purchaseorder_gid { get; set; }
        public string user_firstname { get; set; }
        public string user_firstname1 { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string grn_remarks { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string purchaseorderdtl_gid { get; set; }
        public string received_note { get; set; }
        public string no_box { get; set; }
        public string modeof_dispatch { get; set; }
        public string deliverytracking { get; set; }
        public string goodsintypes_id { get; set; }


        public List<summary_list> summary_list { get; set; }

        ///public summary_list[] summary_list;
    }
    public class summary_list : result
    {
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_ordered { get; set; }
        public double qty_received { get; set; }
        public double qtyreceivedas { get; set; }
         public double qty_delivered { get; set; }
         public double qty_grnadjusted  { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string qty_receivedAS { get; set; }
        public string qty_GRNAdjusted { get; set; }
        public string display_field_name { get; set; }

        public string bin_gid { get; set; }
        public string location_gid { get; set; }
        public string qty_adjustable { get; set; }
        public string purchaseorderdtl_gid { get; set; }

    }
    public class SummaryItem
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string display_field_name { get; set; }
        public double qty_delivered { get; set; }
        public double qty_received { get; set; }
        public double qty_grnadjusted { get; set; }
        public double qty_ordered { get; set; }
    }

    public class ASNList
    {
        public int WarehouseId { get; set; }
        public string POReference { get; set; }
        public string EstimatedDelivery { get; set; }
        public string GoodsInType { get; set; }
        public int Quantity { get; set; }
        public string Supplier { get; set; }
        public string ClientId { get; set; }
        public int ProductSupplierId { get; set; }

        public AsnItem[] Items { get; set; }
    }

    public class AsnItem
    {
        public string ProductId { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public string ExpiryDate { get; set; }
    }


    public class grnproduct_lists : result
    {
        public string grn_gid { get; set; }
        public string grndtl_gid { get; set; }
        public string product_gid { get; set; }
        public string qty_delivered { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string qty_ordered { get; set; }
        public string productuom_name { get; set; }

    }

}