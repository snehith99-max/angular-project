using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{


    public class MdlPmrTrnInvoice : result
    {
        public List<invoice_list> invoice_list { get; set; }
        public List<invoice_list> Serviceinvoice_list { get; set; }
        public List<GetIvTaxSegmentList> GetIvTaxSegmentList { get; set; }
        public List<invoice_lista> invoice_lista { get; set; }
        public List<breadcrumb_list> breadcrumb_list { get; set; }
        public List<GetPurchaseTypeDropdown> GetPmrPurchaseDtl { get; set; }

        public List<GetEditInvList> invoiceaddcomfirm_list { get; set; }
        public List<invoice_lists> invoice_lists { get; set; }
        public List<invoiceProduct_list> invoiceProduct_list { get; set; }
        public List<pblinvoice_list> pblinvoice_list { get; set; }
        public List<taxnamedropdown> taxnamedropdown { get; set; }
        public List<GetpmrInvoiceForLastSixMonths_List> GetpmrInvoiceForLastSixMonths_List { get; set; }
        public List<GetpmrInvoiceDetailSummary> GetpmrInvoiceDetailSummary { get; set; }
        public List<pmrindividualreport_list> pmrindividualreport_list { get; set; }
        public double grand_total { get; set; }


    }
    public class GetIvTaxSegmentList : result
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
    public class invoice_list : result
    {
        public string vendor_gid { get; set; }
        public string invoice_gid { get; set; }
        public string serviceorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string purchaseorder_gid { get; set; }
        public string branch_name { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_contact_person { get; set; }
        public string costcenter_name { get; set; }
        public string invoice_amount { get; set; }
        public string total_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string purchaseorder_from { get; set; }
        public string grn_gid { get; set; }
        public string vendor_address { get; set; }
        public string purchaseorder_status { get; set; }
        public string porefno { get; set; }


    }

    public class invoice_lista : result
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string payment_date { get; set; }
        public string invoice_refno { get; set; }
        public string vendor_gid { get; set; }
        public string vendorinvoiceref_no { get; set; }
        public string vendor_companyname { get; set; }
        public string Vendor { get; set; }
        public string costcenter_name { get; set; }
        public string invoice_amount { get; set; }
        public string invoice_type { get; set; }
        public string invoice_status { get; set; }
        public string overall_status { get; set; }
        public string contact { get; set; }
        public string vendor_code { get; set; }




    }
    public class GetPurchaseTypeDropdown : result
    {
        public string account_gid { get; set; }
        public string purchasetype_name { get; set; }
    }
    public class GetEditInvList : result
    {
        public string branch_name { get; set; }
        public string serviceorder_gid { get; set; }
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string invoice_gid { get; set; }
        public string serviceorder_date { get; set; }
        public string vendor_companyname { get; set; }
        public string email_id { get; set; }
        public string contactperson_name { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_date { get; set; }
        public string addon_amount { get; set; }
        public string discount_amount { get; set; }
        public string invoice_refno { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public string purchasetype_name { get; set; }
        public string tax_amount1 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string tax_name1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string vendor_gid { get; set; }
        public string tax_amount { get; set; }
        public string outstanding_amount { get; set; }
    }

    public class invoice_lists : result
    {
        public string branch_name { get; set; }
        public string invoice_gid { get; set; }
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
        public string mode_despatch { get; set; }
        public string tax_name4 { get; set; }
        public string delivery_term { get; set; }

    }

    public class invoiceProduct_list  : result
    {
       
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string display_field_name { get; set; }
        public string productgroup_name { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string product_total { get; set; }
        public string invoice_amount { get; set; }
        public string product_totalprice { get; set; }
        public string discount_amount { get; set; }
        public string tax_name1 { get; set; }
        public string tax_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount3 { get; set; }
        public string product_remarks { get; set; }
        public string termsandconditions { get; set; }
        public string additional_name { get; set; }
        public string extradiscount_amount { get; set; }
        public string extraadditional_amount { get; set; }
        public string discount_name { get; set; }
        public string round_off { get; set; }
        public string addon_amount { get; set; }
        public string buybackorscrap { get; set; }
        public string freightcharges { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string product_description { get; set; }
        public string discount_percentage { get; set; }
    }
    public class pblinvoice_list : result
    {
        public DateTime invoice_date { get; set; }
        public DateTime payment_date { get; set; }
        public string invoice_refno { get; set; }
        public string branch_name { get; set; }
        public string invoice_gid { get; set; }
        public string vendor_gid { get; set; }
        public string grand_total { get; set; }
        public string invoice_amount { get; set; }
        public string payment_term { get; set; }
        public string discount_amount { get; set; }
        public string addon_amount { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string branch_gid { get; set; }
        public string invoice_remarks { get; set; }
        public string serviceorder_gid { get; set; }
        public string invoice_from { get; set; }
    }

    public class taxnamedropdown : result
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
    }

    //invoice report

    public class GetpmrInvoiceForLastSixMonths_List : result
    {
        public string months { get; set; }
        public string invoiceamount { get; set; }
        public string invoiceamount1 { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public string Tax_amount { get; set; }
        public string net_amount { get; set; }
        public string invoicecount { get; set; }
        public string invoice_date { get; set; }
        public string invoice_gid { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string grandtotal_l { get; set; }
        public string customer_name { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }

    }

    public class GetpmrInvoiceDetailSummary : result
    {
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string salesinvoice_status { get; set; }
        public string invoice_refno { get; set; }
        public string invoiceamount { get; set; }
        public string branch_name { get; set; }
        public string invoice_gid { get; set; }
        public string created_by { get; set; }

    }
    public class pmrindividualreport_list
    {
        public string salesorder_date { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string pending_invoice_amount { get; set; }
        public string invoice_amount { get; set; }
        public string advance_amount { get; set; }
        public string grand_total { get; set; }
        public string invoice_date { get; set; }
        public string so_referenceno1 { get; set; }
        public string invoice_refno { get; set; }
        public string branch_name { get; set; }
        public string customer_contact { get; set; }
        public string customer_address { get; set; }
        public string customer_details { get; set; }
        public string so_type { get; set; }
        public string salesorder_status { get; set; }
    }

}