using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.pmr.Models
{
    public class MdlPmrTrnPurchaseagreement : result
    {
        public List<Getpurchaseagreementorder_list> Getpurchaseagreementorder_list { get; set; }
        public List<PostProduct_list> PostProduct_list { get; set; }
        public List<productsummarylist> productsummarylist { get; set; }
        public List<GetTaxSegmentList1> GetTaxSegmentList1 { get; set; }
        public List<GetTaxSegmentList1> GetViewagreementOrder { get; set; }
        public List<GetViewPurchaseagreement> GetViewPurchaseagreement { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
        public List<GetinviocePO1> GetinviocePO1 { get; set; }
        public List<GetinvioceProduct1> GetinvioceProduct1 { get; set; }
        public List<OverallSubmit_list1> OverallSubmit_list1 { get; set; }
        public List<invoicetagsummary_list1> invoicetagsummary_list1 { get; set; }
        public List<GetEditAgreementSummary> GetEditAgreementSummary { get; set; }
        public List<GetMonthlyRenewal_lists> GetMonthlyRenewal_lists { get; set; }
        public List<GetRenewalForLastSixMonths_List> GetRenewalForLastSixMonths_List { get; set; }
        public List<GetRenewalSummary_lists> GetRenewalSummary_lists { get; set; }
        public List<GetRenewalDetailSummarylist> GetRenewalDetailSummarylist { get; set; }
    }
 public class mapinvoice_lists1 : result
    {
        public string renewal_gid { get; set; }
        public List<invoicetagsummary_list1> invoicetagsummary_list1 { get; set; }
    }
    public class invoicetagsummary_list1 : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_amount { get; set; }
        public string customer_name { get; set; }
        public string invoice_date { get; set; }
        public string contact { get; set; }
        public string renewal_gid { get; set; }

    }public class GetEditAgreementSummary : result
    {
        public string vendor_companyname { get; set; }
        public string address_gid { get; set; }
        public string branch_name { get; set; }
        public string tax_name { get; set; }
        public string purchaseorder_gid { get; set; }
        public string branch_gid { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_address { get; set; }
        public string mode_despatch { get; set; }
        public string termsandconditions { get; set; }
        public string discount_amount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_amount { get; set; }
        public string addon_amount { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string freightcharges { get; set; }
        public string tax_gid { get; set; }
        public string roundoff { get; set; }
        public string freight_terms { get; set; }
        public string netamount { get; set; }
        public string requested_by { get; set; }
        public string requested_details { get; set; }
        public string po_covernote { get; set; }
        public string total_amount { get; set; }
        public string po_date { get; set; }
        public string expected_date { get; set; }
        public string payment_terms { get; set; }
        public string address2 { get; set; }
        public string gst_no { get; set; }
        public string contact_telephonenumber { get; set; }
        public string shipping_address { get; set; }
        public string poref_no { get; set; }
        public string email_id { get; set; }
    }    public class OverallSubmit_list1 : result
    {
        public string inv_ref_no { get; set; }
        public string invoice_date { get; set; }
        public string renewal_gid { get; set; }
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
        public string tax_name { get; set; }
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
        public string renewal_date { get; set; }
        public string frequency_terms { get; set; }
    }
    public class GetinvioceProduct1 : result
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

    }
    public class GetinviocePO1 : result
    {
        public string vendor_fax { get; set; }
        public string requested_details { get; set; }
        public string requested_by { get; set; }
        public string renewal_gid { get; set; }
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

    }
    public class Getpurchaseagreementorder_list : result
    {

        public string agreement_gid { get; set; }
        public string agreement_date { get; set; }
        public string created_date { get; set; }
        public string agreement_referenceno1 { get; set; }
        public string agreement_type { get; set; }
        public string Grandtotal { get; set; }
        public string customer_name { get; set; }
        public string renewal_gid { get; set; }
        public string contact { get; set; }
        public string renewal_type { get; set; }
        public string order_agreement_gid { get; set; }
        public string renewal_date { get; set; }
    }
    public class PostProduct_list : result
    {
        public string producttype_name { get; set; }
        public string Vendor { get; set; }
        public string productuom_name { get; set; }
        public string product_remarks { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productquantity { get; set; }
        public string unitprice { get; set; }
        public string productdiscount { get; set; }
        public string discountprecentage { get; set; }
        public string producttotal_amount { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount3 { get; set; }
        public string taxgid1 { get; set; }
        public string taxgid2 { get; set; }
        public string taxgid3 { get; set; }
        public string taxprecentage1 { get; set; }
        public string taxprecentage2 { get; set; }
        public string taxprecentage3 { get; set; }
        public string exchange_rate { get; set; }
        public string vendor_gid { get; set; }
        public string taxname3 { get; set; }
        public string taxname2 { get; set; }
        public string taxname1 { get; set; }
        public string discount_amount { get; set; }
        public string salesorder_gid { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class productsummarylist : result
    {
        public string productuom_name { get; set; }
        public string taxamount1 { get; set; }
        public string unitprice { get; set; }

        public string product_name { get; set; }
        public string tmppurchaseorderdtl_gid { get; set; }
        public string discount_percentage { get; set; }

        public string tax { get; set; }
        public string totalamount { get; set; }
        public string hsn { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string uom_name { get; set; }
        public string qty { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }
        public string product_total { get; set; }
        public string taxsegment_gid { get; set; }
        public string display_field { get; set; }
        public string tax_name1 { get; set; }
        public string taxamount2 { get; set; }

        public string tax_name2 { get; set; }

        public string tax_name3 { get; set; }
        public string tax_percentage3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_gid { get; set; }
    }
    public class GetTaxSegmentList1 : result
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
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string producttype_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
        public double total_amount { get; set; }
        public double quantity { get; set; }
        public double discount_persentage { get; set; }
        public double discount_amount { get; set; }

    }
    public class GetViewagreementOrder : result
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
        public string agreement_date { get; set; }
        public string renewal_date { get; set; }
        public string expected_date { get; set; }
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
        public string producttotal_amount { get; set; }

        public string ship_via { get; set; }
        public string addoncharge { get; set; }

        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string delivery_location { get; set; }
        public string currency_code { get; set; }
        public string currencycode { get; set; }
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

    public class GetViewPurchaseagreement : result
    {
        public string vendor_fax { get; set; }
        public string requested_details { get; set; }
        public string requested_by { get; set; }
        public string po_covernote { get; set; }
        public string renewal_gid { get; set; }
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
        public string expected_date { get; set; }
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
        public string branch_gid { get; set; }
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
        public string producttotal_amount { get; set; }
        public string Created_date { get; set; }
        public string renewal_description { get; set; }

        public string ship_via { get; set; }
        public string addoncharge { get; set; }
        public string currency_gid { get; set; }
        public string agreement_remarks { get; set; }
        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string delivery_location { get; set; }
        public string currency_code { get; set; }
        public string currencycode { get; set; }
        public string shipping_address { get; set; }
        public string bill_to { get; set; }
        public string purchaseorder_reference { get; set; }
        public string purchaseorder_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string customer_gid { get; set; }
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
        public string renewal_date { get; set; }
        public string agreement_date { get; set; }
        public string renewal_mode { get; set; }
        public string frequency_terms { get; set; }
        public string frequency_term { get; set; }
        public string agreement_gid { get; set; }
        public string customer_address { get; set; }

    }

    //reports

    public class GetMonthlyRenewal_lists
    {
        public string renewal_month_name { get; set; }
        public string renewal_year { get; set; }
        public string renewal_count { get; set; }
    }
    public class GetRenewalForLastSixMonths_List : result
    {
        public string months { get; set; }

        public string year { get; set; }
        public string amount { get; set; }
        public string renewalcount { get; set; }
        public string renewal_date { get; set; }
        public string renewal_gid { get; set; }
        public string renewal_status { get; set; }

        public string customer_name { get; set; }


    }

    public class GetRenewalDetailSummarylist : result
    {
        public string renewal_gid { get; set; }
        public string order_agreement_gid { get; set; }
        public string order_agreement_date { get; set; }
        public string duration { get; set; }
        public string vendor_gid { get; set; }
        public string renewal_description { get; set; }
        public string created_by { get; set; }
        public string renewal { get; set; }
        public string renewal_status { get; set; }
        public string renewal_to { get; set; }
        public string user_name { get; set; }
        public string customer_name { get; set; }
        public string salesorder_date { get; set; }
        public string renewal_date { get; set; }
        public string Grandtotal { get; set; }
        public string contact_details { get; set; }
    }

    public class GetRenewalSummary_lists
    {
        public string campaign_gid { get; set; }
        public string team_name { get; set; }
        public string team_branch { get; set; }
        public string renewals_assigned { get; set; }
        public string renewals_new { get; set; }
        public string completed_renewals { get; set; }
        public string renewals_follow { get; set; }
        public string dropped_renewals { get; set; }
        public string assigned_employee { get; set; }
        public string open_count { get; set; }
        public string closed_count { get; set; }
        public string dropped_count { get; set; }

    }

}