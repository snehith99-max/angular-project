using System;
using System.Collections.Generic;

namespace ems.pmr.Models
{
  
    public class MdlPmrTrnDirectInvoice :result
    {
        public List<GetBranchnamedropdown> GetBranchnamedropdown { get; set; }
        public List<GetVendornamedropdown> GetVendornamedropdown { get; set; }
        public List<GetOnChangeVendor> GetOnChangeVendor { get; set; }
        public List<Getcurrencycodedropdown> Getcurrencycodedropdown { get; set; }
        public List<GetPurchaseTypedropDown> GetPurchaseTypedropDown { get; set; }
        public List<Gettaxnamedropdown> Gettaxnamedropdown { get; set; }
        public List<GetExtraAddondropDown> GetExtraAddondropDown { get; set; }
        public List<GetExtraDeductiondropDown> GetExtraDeductiondropDown { get; set; }
        public List<directsalesinvoicelist> directsalesinvoicelist { get; set; }
        public List<GetPblTaxSegmentList> GetPblTaxSegmentList { get; set; }
        public List<GetPblProductsearch> GetPblProductsearch { get; set; }
        public List<Pblproductsummary_list> Pblproductsummary_list { get; set; }
        public List<PblGetTaxSegmentList> PblGetTaxSegmentList { get; set; }
        public List<PblGetOnchangeCurrency> PblGetOnchangeCurrency { get; set; }
        public List<PblsubmitProducts> PblsubmitProducts { get; set; }
        public List<PblDirectInvoice> PblDirectInvoice { get; set; }
        public List<PblGetTaxFourDropdown> PblGetTaxFourDropdown { get; set; }
        public List<PblGetproducttype> PblGetproducttype { get; set; }
        public List<PblGetTax> PblGetTax { get; set; }
        public List<GetDraftinvoice> GetDraftinvoice { get; set; }
        public List<Getadditional_list> Getadditional_list { get; set; }
        public List<Getdiscount_list> Getdiscount_list { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
        public double product_gid { get; set; }
        public double vendor_gid { get; set; }
        public List<invoice_listsedit> invoice_listsedit { get; set; }
    }
    public class GetBranchnamedropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }
    public class GetVendornamedropdown : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
    }

    public class GetOnChangeVendor : result
    {
        public string address { get; set; }
        public string vendorcontact { get; set; }
        public string phone { get; set; }
        public string taxsegment_gid { get; set; }
        public string vendor_gid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string gst_no { get; set; }
        public string fax { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_companyname { get; set; }
        public string contact_telephonenumber { get; set; }
        public string country_name { get; set; }
        public string email_id { get; set; }
        public string currencyexchange_gid { get; set; }
        
    }
    public class Getcurrencycodedropdown : result
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }
        public string default_currency { get; set; }
        public string exchange_rate { get; set; }
    }
    public class GetPurchaseTypedropDown : result
    {
        public string account_gid { get; set; }
        public string purchasetype_name { get; set; }
    }
    public class Gettaxnamedropdown : result
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
    }
    public class GetExtraAddondropDown : result
    {
        public string additional_gid { get; set; }
        public string additional_name { get; set; }
    }
    public class GetExtraDeductiondropDown : result
    {
        public string discount_gid { get; set; }
        public string discount_name { get; set; }
    }
    public class directsalesinvoicelist : result
    {
        public string direct_invoice_ven_name { get; set; }
        public string direct_invoice_refno { get; set; }
        public DateTime direct_invoice_date { get; set; }
        public DateTime direct_invoice_due_date { get; set; }
        public string vendor_gid { get; set; }
        public string direct_invoice_addon_amount { get; set; }
        public string direct_invoice_discount_amount { get; set; }
        public string direct_invoice_grand_total { get; set; }
        public double direct_invoice_amount { get; set; }
        public string direct_invoice_payterm { get; set; }
        public string direct_invoice_remarks { get; set; }
        public string direct_invoice_currencycode { get; set; }
        public string direct_invoice_exchange_rate { get; set; }
        public string direct_invoice_freight_charges { get; set; }
        public string direct_invoice_extra_addon { get; set; }
        public string direct_invoice_extra_deduction { get; set; }
        public string direct_invoice_buyback_scrap_charges { get; set; }
        public string direct_invoice_ven_ref_no { get; set; }
        public string direct_invoice_branchgid { get; set; }
        public string direct_invoice_round_off { get; set; }
        public string direct_invoice_taxname1 { get; set; }
        public string direct_invoice_taxname2 { get; set; }
        public string tax_gid1 { get; set; }
        public string tax_gid2 { get; set; }
        public string direct_invoice_ven_address { get; set; }
        public string direct_invoice_ven_contact_person { get; set; }
        public string direct_invoice_description { get; set; }
        public string vendor_contact_person { get; set; }
        public string direct_invoice_type { get; set; }
    }
    public class GetPblTaxSegmentList : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }
    }
    public class GetPblProductsearch : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string producttype_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
        public int quantity { get; set; }
        public int discount_persentage { get; set; }
        public int total_amount { get; set; }
        public int discount_amount { get; set; }
    }
    public class Pblproductsummary_list : result
    {
        public string productuom_name { get; set; }
        public string taxamount1 { get; set; }
        public string unitprice { get; set; }

        public string product_name { get; set; }
        public string tmpinvoicedtl_gid { get; set; }
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
        public string tax_name2 { get; set; }
        public string taxamount2 { get; set; }

        public string tax_name3 { get; set; }
        public string tax_percentage3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_gid { get; set; }
    }
    public class PblGetTaxSegmentList : result
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
    public class PblGetOnchangeCurrency
    {

        public string exchange_rate { get; set; }
        public string currency_code { get; set; }

    }
    public class taxSegments_inv
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string taxAmount { get; set; }
    }
    public class PblsubmitProducts
    {
        public List<DIProductList1> DIProductList { get; set; }
        public string producttype_name { get; set; }
        public string Vendor { get; set; }
        public string branch_name { get; set; }
        public string productuom_name { get; set; }
        public string product_remarks { get; set; }
        public string product_gid { get; set; }
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
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string tax_name1 { get; set; }
        public string totalamount { get; set; }
        public bool status { get; internal set; }
        public string message { get; internal set; }
    }
    public class DIProductList1
    {
        public List<taxSegments_inv> taxSegments { get; set; }
        public string qty_requested { get; set; }
        public string quantity { get; set; }
        public string discount_persentage { get; set; }
        public string discount_amount { get; set; }
        public string tax1 { get; set; }
        public string tax2 { get; set; }
        public string totalTaxAmount { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount1 { get; set; }
        public string taxsegment_gid { get; set; }
        public string tax_gids_string { get; set; }
        public string total_amount { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string vendorcompanyname { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string tax3 { get; set; }
        public string display_field { get; set; }
        public string unitprice { get; set; }
   
    }

    public class PblDirectInvoice : result
    {
        public string vendor_fax { get; set; }
        public string due_date { get; set; }
        public string email_address { get; set; }
        public string productuom_gid { get; set; }
        public string overalltax { get; set; }
        public string additional_discount { get; set; }
        public string tax_percentage { get; set; }
        public string netamount { get; set; }
        public string tax_name { get; set; }
        public string product_total { get; set; }
        public string overall_tax { get; set; }
        public string invoice_date { get; set; }
        public string discountamount { get; set; }
        public string tax_amount4 { get; set; }
        public string vendorcompanyname { get; set; }
        public string invoice_ref_no { get; set; }   
        public string invoice_gid { get; set; }   
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
        public string vendor_contactnumber { get; set; }
        public string vendor_contact_person { get; set; }
        public string vendor_faxnumber { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_details { get; set; }
        public string vendor_emailid { get; set; }
        public string vendor_address { get; set; }
        public string exchange_rate { get; set; }
        public string po_no { get; set; }
        public string ship_via { get; set; }
        public string addoncharge { get; set; }
        public string payment_terms { get; set; }
        public string delivery_terms { get; set; }
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
        public string vendor_ref_no { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string taxsegment_gid { get; set; }
        public string discount_amount { get; set; }
        public string total_amount { get; set; }
        public string additional_name { get; set; }
        public string discount_name { get; set; }
        public string freightcharges { get; set; }
        public string additionalamount { get; set; }
        public string deductionamount { get; set; }
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
        public string tax_name4 { get;  set; }
        public string dispatch_mode { get;  set; }
        public string billing_mail { get;  set; }
        public string invoice_remarks { get;  set; }
        public string address1 { get;  set; }
        public string purchase_type { get;  set; }
        public string purchasetype_name { get;  set; }
    }
    public class Pdlproductlist : result
    {
        public string tax_gid3 { get; set; }
        public string tax_gid { get; set; }

        public string quantity { get; set; }
        public string productname { get; set; }
        public string productuom { get; set; }
        public string product_gid { get; set; }
        public string tax_name1 { get; set; }
        public string totalamount { get; set; }
        public string tax_name2 { get; set; }
        public string unitprice { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount3 { get; set; }

        public string tax_name3 { get; set; }
        public string totalamount1 { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string created_by { get; set; }
        public string taxpercentage3 { get; set; }

        public string needby_date { get; set; }

        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string productdescription { get; set; }
        public string tax_gid1 { get; set; }
        public string tax_gid2 { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup { get; set; }
        public string costprice { get; set; }
        public string productcode { get; set; }
        public string customerproductcode { get; set; }
        public string taxpercentage1 { get; set; }
        public string taxpercentage2 { get; set; }
        public string productgid { get; set; }

        public string vendorprice { get; set; }
    }
    public class PblGetTax
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_name1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string percentage { get; set; }



    }
    public class PblGetTaxFourDropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name4 { get; set; }
        public string percentage { get; set; }

    }
    public class PblGetproducttype :  result
    {
        public string producttype_name { get; set; }
        public string producttype_code { get; set; }
        public string producttype_gid { get; set; }
    }
    public class Getadditional_list: result
    {
        public string additional_gid { get; set;}
        public string additional_name { get; set;}
    }
    public class Getdiscount_list : result
    {
        public string discount_gid { get; set;}
        public string discount_name { get; set;}
    }

    public class invoice_listsedit : result
    {
        public string branch_name { get; set; }
        public string invoice_gid { get; set; }
        public string vendor_gid { get; set; }
        public string contact_telephonenumber { get; set; }
        public string invoice_date { get; set; }
        public string vendorinvoiceref_no { get; set; }
        public string invoice_refno { get; set; }
        public string contactperson_name { get; set; }
        public string exchange_rate { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_address { get; set; }
        public string currency_code { get; set; }
        public string payment_date { get; set; }
        public string payment_days { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_type { get; set; }
        public string email_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string product_totalprice { get; set; }
        public string discount_amount { get; set; }
        public string tax_name1 { get; set; }
        public string tax_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount3 { get; set; }
        public string product_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string additionalcharges_amount { get; set; }
        public string freightcharges { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string round_off { get; set; }
        public string invoice_amount { get; set; }
        public string termsandconditions { get; set; }
        public string price { get; set; }
        public string billing_email { get; set; }
        public string shipping_address { get; set; }
        public string payment_term { get; set; }
        public string delivery_term { get; set; }
        public string mode_despatch { get; set; }
        public string tax_name4 { get; set; }
        public string branch_gid { get; set; }
        public string tax1_gid { get; set; }
        public string currencyexchange_gid { get; set; }
        public string purchase_type { get; set; }

    }
    public class GetDraftinvoice : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string invoicedraft_gid { get; set; }
        public string invoice_amount { get; set; } 
        public string invoice_date { get; set; } 
        public string invoice_reference  { get; set; } 
    }
}

