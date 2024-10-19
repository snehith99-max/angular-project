using ems.pmr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPblInvoiceGrnDetails : result
    {
        public List<GetInvTaxSegmentList> GetInvTaxSegmentList { get; set; }
        public List<GetInvoicePurchaseOrderGrnDetails_list> GetInvoicePurchaseOrderGrnDetails_list { get; set; }
        public List<GetInvoiceVendorDetails_list> GetInvoiceVendorDetails_list { get; set; }
        public List<GetTax4Dropdown> GetTaxfourDtl { get; set; }
        public List<GetNetamount_list> GetNetamount_list { get; set; }
        public List<GetInvoiceGrnDetails_list> GetInvoiceGrnDetails_list { get; set; }
        public List<GetVendorUserDetails_list> GetVendorUserDetails_list { get; set; }
        public List<GetPurchaseType_list> GetPurchaseType_list { get; set; }
        public List<OverallSubmit_list> OverallSubmit_list { get; set; }
        public List<OverapprovalSubmit_list> OverapprovalSubmit_list { get; set; }
        public List<GetViewApproval> GetViewApproval { get; set; }
        public List<GetinviocePO> GetinviocePO { get; set; }
        public List<GetinvioceProduct> GetinvioceProduct { get; set; }
        public double grand_total { get; set; }
    }

    public class GetViewApproval : result
    {
        public string vendor_fax { get; set; }
        public string requested_details { get; set; }
        public string requested_by { get; set; }
        public string po_covernote { get; set; }
        public string email_address { get; set; }
        public string productuom_gid { get; set; }
        public string overalltax { get; set; }
        public string additional_discount { get; set; }
        public string tax_percentage { get; set; }
        public string netamount { get; set; }
        public string tax_name { get; set; }
        public string product_total { get; set; }
        public string overall_tax { get; set; }
        public string po_date { get; set; }
        public string discountamount { get; set; }
        public string tax_amount4 { get; set; } 
        public string invoice_amount { get; set; }
        public string total { get; set; }

        public string dispatch_name { get; set; }
        public string grandtotal { get; set; }

        public string Shipping_address { get; set; }

        public string contact_person { get; set; }

        public string pocovernote_address { get; set; }

        public string totalamount { get; set; }

        public string template_content { get; set; }
        public string contact_number { get; set; }
        public string remarks { get; set; }

        public string purchaseorder_gid { get; set; }
        public string grn_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string manualporef_no { get; set; }
        public string deliverytobranch { get; set; }
        public string branch_name { get; set; }
        public string branch_add1 { get; set; }
        public string vendor_contactnumber { get; set; }
        public string vendor_contact_person { get; set; }
        public string vendor_faxnumber { get; set; }
        public string vendor_companyname { get; set; }
        public string Requestor_details { get; set; }
        public string dispatch_mode { get; set; }

        public string address1 { get; set; }
        public string employee_name { get; set; }
        public string delivery_terms { get; set; }
        public string vendor_emailid { get; set; }
        public string vendor_address { get; set; }
        public string exchange_rate { get; set; }
        public string po_no { get; set; }

        public string ship_via { get; set; }
        public string addoncharge { get; set; }

        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string delivery_location { get; set; }
        public string currency_code { get; set; }
        public string shipping_address { get; set; }
        public string purchaseorder_reference { get; set; }
        public string purchaseorder_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string taxsegment_gid { get; set; }
        public string discount_amount { get; set; }
        public string total_amount { get; set; }
        public string freightcharges { get; set; }
        public string qyt_unit { get; set; }
        public string tax_amount { get; set; }
        public string approver_remarks { get; set; }

        public string delivery_days { get; set; }
        public string product_totalprice { get; set; }

        public string productgroup_name { get; set; }

        public string product_code { get; set; }

        public string product_name { get; set; }

        public string productuom_name { get; set; }

        public string product_price { get; set; }

        public string priority_n { get; set; }

        public string qty_ordered { get; set; }
        public string qty_Received { get; set; }

        public string qty_grnadjusted { get; set; }
        public string priority_remarks { get; set; }

        public string tax_amount2 { get; set; }

        public string tax_amount3 { get; set; }

        public string addon_amount { get; set; }
        public string product_gid { get; set; }
        public string display_field_name { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string tax_name4 { get; set; }
        public string email_id { get; set; }
        public string contact_telephonenumber { get; set; }
        public string contactperson_name { get; set; }
        public string tax_number { get; set; }
        public string address2 { get; set; }
        public string mode_despatch { get; set; }
        public string product_price_L { get; set; }
        public string discount_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string overalltaxname { get; set; }

    }
    public class GetInvTaxSegmentList : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }

    }
    public class GetInvoicePurchaseOrderGrnDetails_list : result
    {
        public string currency_code { get; set; }
        public string branch_name { get; set; }
        public string purchaseorder_date { get; set; }
        public string purchaseorder_gid { get; set; }
        public string branch_gid { get; set; }
        public string exchange_rate { get; set; }
        public string buybackorscrap { get; set; }
        public string addon_amount { get; set; }
        public string freightcharges { get; set; }
        public string discount_amount { get; set; }
        public string Grand_total { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string payment_date { get; set; }
        public string payment_term { get; set; }
        public string payment_days { get; set; }
        public string tax_amount { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }

    }

    public class GetInvoiceVendorDetails_list : result
    {
        public string vendor_address { get; set; }
        public string vendor_gid { get; set; }
        public string fax { get; set; }
        public string payment_terms { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_companyname { get; set; }
        public string email_id { get; set; }
        public string currencyexchange_gid { get; set; }
        public string contact_telephonenumber { get; set; }
        public string country_name { get; set; }
        public string currency_code { get; set; }
    }
    public class GetInvoiceGrnDetails_list : result
    {
        public string grndtl_gid { get; set; }
        public string Product_amount { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string vendor_gid { get; set; }
        public string total_product_price { get; set; }
        public string grn_gid { get; set; }
        public string product_gid { get; set; }
        public string qty_delivered { get; set; }
        public string qtyreceivedas { get; set; }
        public string qty_rejected { get; set; }
        public string qty_accepted { get; set; }
        public string qty_billed { get; set; }
        public string qty_excess { get; set; }
        public string qty_invoice { get; set; }
        public string qty_returned { get; set; }
        public string qty_grnadjusted { get; set; }
        public string purchaseorderdtl_gid { get; set; }
        public string receiveduom_gid { get; set; }
        public string split_flag { get; set; }
        public string location_gid { get; set; }
        public string display_field { get; set; }
        public string productgroup_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_code { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string product_price { get; set; }
        public string excise_percentage { get; set; }
        public string excise_amount { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string purchaseorder_gid { get; set; }
        public string tax_percentage1 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string buybackorscrap { get; set; }
        public string total_amount { get; set; }
        public string invoice_amount { get; set; }
        public string product_total { get; set; }
        public string product_price_L { get; set; }
    }
    public class GetVendorUserDetails_list : result
    {
        public string employee_name { get; set; }
        public string employee_emailid { get; set; }
        public string employee_phoneno { get; set; }
        public string employee_mobileno { get; set; }

    }

    public class GetPurchaseType_list : result
    {
        public string account_gid { get; set; }
        public string purchasetype_name { get; set; }
    }
    public class GetNetamount_list : result
    {
        public string netamount { get; set; }

    }

    public class OverallSubmit_list : result
    {
        public string inv_ref_no { get; set; }
        public string invoice_date { get; set; }
        public string priority_n { get; set; }
        public string purchasetype_name { get; set; }
        public string net_amount { get; set; }
        public string Vendor_ref_no { get; set; }
        public string branch_name { get; set; }
        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string payment_days { get; set; }
        public string payment_date { get; set; }
        public string employee_emailid { get; set; }
        public string employee_mobileno { get; set; }
        public string employee_name { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string dispatch_mode { get; set; }
        public string address1 { get; set; }
        public string contact_telephonenumber { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_delivered { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }
        public string discount_percentage { get; set; }
        public string tax_name{ get; set; }
        public string tax_name4{ get; set; }
        public string tax_amount { get; set; }
        public string tax_amount4 { get; set; }
        public string total_amount { get; set; }
        public string product_remarks { get; set; }
        public string freightcharges { get; set; }
        public string vendor_gid { get; set; }
        public string invoice_remarks { get; set; }
        public string addon_charge { get; set; }
        public string grandtotal { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string insurance_charges { get; set; }
        public string payment_terms { get; set; }
        public string template_content { get; set; }
        public string delivery_term { get; set; }
        public string purchase_type { get; set; }
        public string packing_charges { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string discount_amount1 { get; set; }

        public string totalamount { get; set; }
        public string addoncharge { get; set; }
        public string additional_discount { get; set; }
        public string shipping_address { get; set; }
        public string billing_email { get; set; }
    }

    public class GetTax4Dropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name4 { get; set; }
        public string percentage { get; set; }

    }

    public class OverapprovalSubmit_list : result
    {
        public string inv_ref_no { get; set; }
        public string invoice_date { get; set; }
        public List<GetInvoiceGrnDetails_list> GetInvoiceGrnDetails_list { get; set; }

    }
    public class GetinviocePO : result
    {
        public string vendor_fax { get; set; }
        public string requested_details { get; set; }
        public string requested_by { get; set; }
        public string po_covernote { get; set; }
        public string email_address { get; set; }
        public string productuom_gid { get; set; }
        public string overalltax { get; set; }
        public string additional_discount { get; set; }
        public string tax_percentage { get; set; }
        public string netamount { get; set; }
        public string net_amount { get; set; }
        public string tax_name { get; set; }
        public string product_total { get; set; }
        public string overall_tax { get; set; }
        public string po_date { get; set; }
        public string discountamount { get; set; }
        public string tax_amount4 { get; set; }

        public string dispatch_name { get; set; }
        public string grandtotal { get; set; }

        public string Shipping_address { get; set; }

        public string contact_person { get; set; }

        public string pocovernote_address { get; set; }

        public string totalamount { get; set; }

        public string template_content { get; set; }
        public string contact_number { get; set; }
        public string remarks { get; set; }

        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string manualporef_no { get; set; }
        public string deliverytobranch { get; set; }
        public string branch_name { get; set; }
        public string branch_add1 { get; set; }
        public string vendor_contactnumber { get; set; }
        public string vendor_contact_person { get; set; }
        public string vendor_faxnumber { get; set; }
        public string vendor_companyname { get; set; }
        public string Requestor_details { get; set; }
        public string dispatch_mode { get; set; }

        public string address1 { get; set; }
        public string employee_name { get; set; }
        public string delivery_terms { get; set; }
        public string vendor_emailid { get; set; }
        public string vendor_address { get; set; }
        public string exchange_rate { get; set; }
        public string po_no { get; set; }

        public string ship_via { get; set; }
        public string addoncharge { get; set; }

        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string delivery_location { get; set; }
        public string currency_code { get; set; }
        public string shipping_address { get; set; }
        public string bill_to { get; set; }
        public string purchaseorder_reference { get; set; }
        public string purchaseorder_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string taxsegment_gid { get; set; }
        public string discount_amount { get; set; }
        public string total_amount { get; set; }
        public string freightcharges { get; set; }
        public string qyt_unit { get; set; }
        public string tax_amount { get; set; }
        public string total_amount_L { get; set; }
        public string approver_remarks { get; set; }

        public string delivery_days { get; set; }
        public string product_totalprice { get; set; }

        public string productgroup_name { get; set; }

        public string product_code { get; set; }

        public string product_name { get; set; }

        public string productuom_name { get; set; }

        public string product_price { get; set; }
        public string tax_prefix { get; set; }

        public string priority_n { get; set; }

        public string qty_ordered { get; set; }
        public string qty_Received { get; set; }

        public string qty_grnadjusted { get; set; }
        public string priority_remarks { get; set; }

        public string tax_amount2 { get; set; }

        public string tax_amount3 { get; set; }

        public string addon_amount { get; set; }
        public string product_gid { get; set; }
        public string display_field_name { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string tax_name4 { get; set; }
        public string email_id { get; set; }
        public string contact_telephonenumber { get; set; }
        public string contactperson_name { get; set; }
        public string tax_number { get; set; }
        public string address2 { get; set; }
        public string mode_despatch { get; set; }
        public string product_price_L { get; set; }
        public string discount_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string overalltaxname { get; set; }
        public string po_type { get; set; }
        public string pototalamount { get; set; }
        public string podiscountamount { get; set; }
        public string potaxpercentage { get; set; }
        public string potaxamount { get; set; }
        public string poaddonamount { get; set; }
        public string pofreightcharges { get; set; }
        public string potaxgid { get; set; }
        public string poroundoff { get; set; }
        public string poproducttotal { get; set; }

    }

    public class GetinvioceProduct : result
    {
        public string tax_percentage { get; set; }

        public string tax_name { get; set; }
        public string product_total { get; set; }
        public string overall_tax { get; set; }
        public string po_date { get; set; }
        public string discountamount { get; set; }
        public string tax_amount4 { get; set; }
        public string totalamount { get; set; }

        public string contact_number { get; set; }
        public string remarks { get; set; }

        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string branch_name { get; set; }
        public string branch_add1 { get; set; }
        public string po_no { get; set; }

        public string ship_via { get; set; }
        public string purchaseorder_reference { get; set; }
        public string purchaseorder_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string discount_amount { get; set; }
        public string total_amount { get; set; }
        public string qyt_unit { get; set; }
        public string tax_amount { get; set; }
        public string product_totalprice { get; set; }

        public string productgroup_name { get; set; }

        public string product_code { get; set; }

        public string product_name { get; set; }

        public string productuom_name { get; set; }

        public string product_price { get; set; }

        public string priority_n { get; set; }

        public string qty_ordered { get; set; }
        public string qty_Received { get; set; }

        public string qty_grnadjusted { get; set; }
        public string priority_remarks { get; set; }

        public string tax_amount2 { get; set; }

        public string tax_amount3 { get; set; }

        public string addon_amount { get; set; }
        public string product_gid { get; set; }
        public string display_field_name { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string tax_name4 { get; set; }
        public string tax_number { get; set; }
        public string product_price_L { get; set; }
        public string discount_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string overalltaxname { get; set; }
        public string netamount { get; set; }


    }











}