using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnRenewalsummary : result
    {
        public List<renewalsummary_list> renewalsummary_list { get; set; }
        public List<renewalsummary_list2> renewalsummary_list2 { get; set; }
        public List<renewalview_list> renewalview_list { get; set; }
        public List<Viewrenewaldetail_list> Viewrenewaldetail_list { get; set; }
        public List<renewalsalesorder_list> renewalsalesorder_list { get; set; }
        public List<renewalsalesorderdetails_list> renewalsalesorderdetails_list { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
        public List<GetProductsearchs1> GetProductsearchs1 { get; set; }
        public List<PosttempProduct_list> PosttempProduct_list { get; set; }
        public List<postagree_list> postagree_list { get; set; }
        public List<GetTaxSegmentList1> GetTaxSegmentList1 { get; set; }
        public List<renewalassignteam_list> renewalassignteam_list { get; set; }
        public List<Getrenewaltoinvoice_list> Getrenewaltoinvoice_list { get; set; }
        public List<ordertoinvoiceproductsubmit_list1> ordertoinvoiceproductsubmit_list1 { get; set; }
        public List<renewaltoinvoicesubmit> renewaltoinvoicesubmit { get; set; }
        public List<ProductSummaryRenewal_list> ProductSummaryRenewal_list { get; set; }
        public List<PostRenewalEdit_list> PostRenewalEdit_list { get; set; }
        public List<invoicetagsummary_list> invoicetagsummary_list { get; set; }
        public List<unmappedinvoice_list1> unmappedinvoice_list1 { get; set; }

    }
    public class mapinvoice_lists : result
    {
        public string renewal_gid { get; set; }
        public List<invoicetagsummary_list> invoicetagsummary_list { get; set; }
    }
    public class PostRenewalEdit_list : result
    {

        public string total_amount { get; set; }
        public string renewal_date { get; set; }
        public string renewal_gid { get; set; }
        public string agreement_date { get; set; }
        public string address1 { get; set; }
        public string slno { get; set; }
        public string salesorder_refno { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string agreement_remarks { get; set; }
        public string product_remarks { get; set; }
        public string shipping_address { get; set; }
        public string branch_address { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string currency_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string contactemailaddress { get; set; }
        public string Grandtotal { get; set; }
        public string salesperson_gid { get; set; }
        public string gst_amount { get; set; }
        public string customer_contact_person { get; set; }
        public string tax_amount { get; set; }
        public string product_code { get; set; }
        public string tax_name { get; set; }
        public string customer_address2 { get; set; }
        public string customer_address { get; set; }
        public string productuom_gid { get; set; }
        public string customer_city { get; set; }
        public string additional_discount { get; set; }
        public string currencyexchange_gid { get; set; }
        public string countryname { get; set; }
        public string region_name { get; set; }
        public string customer_state { get; set; }
        public string grandtotal { get; set; }
        public string gst_number { get; set; }
        public string postal_code { get; set; }
        public string customercontact_name { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string branch_gid { get; set; }
        public string salesorder_date { get; set; }
        public string customercontact_gid { get; set; }
        public string exchange_rate { get; set; }
        public string customercontact_names { get; set; }
        public string addoncharge { get; set; }
        public string customer_Address { get; set; }
        public string currency_code { get; set; }
        public string so_referencenumber { get; set; }
        public string so_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string shipping_to { get; set; }
        public string tax_name4 { get; set; }
        public string salesorder_gid { get; set; }
        public string txttaxamount_1 { get; set; }
        public string totalamount { get; set; }
        public string total_price { get; set; }
        public string productgroup_name { get; set; }
        public string user_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string freight_charges { get; set; }
        public string created_flag { get; set; }
        public string country_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string branch_name { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount_2 { get; set; }
        public string taxamount_3 { get; set; }
        public string taxamount_l { get; set; }
        public string selling_price_l { get; set; }
        public string totalamount_l { get; set; }
        public string discount_amount_l { get; set; }
        public string marginpercentage { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string termsandcondition { get; set; }
        public string buyback_charges { get; set; }
        public string addon_charge { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string productgroup_gid { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customerproduct_code { get; set; }
        public string display_field { get; set; }
        public string selling_price { get; set; }
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string vessel_name { get; set; }
        public string tax_amount_l { get; set; }
        public string tax_amount2_l { get; set; }
        public string tax_amount3_l { get; set; }
        public string product_discount { get; set; }
        public string price { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string order_type { get; set; }
        public string mrp { get; set; }
        public string taxamount_1 { get; set; }
        public string producttotalamount { get; set; }
        public string agreement_referencenumber { get; set; }


    }
    public class GetProductsearchs1
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string unitprice { get; set; }
        public string producttype_gid { get; set; }
        public double taxamount1 { get; set; }
        public double taxamount2 { get; set; }
        public double quantity { get; set; }
        public double total_amount { get; set; }
        public double discount_amount { get; set; }
        public double discount_percentage { get; set; }
        public double discount_persentage { get; set; }
        public string product_desc { get; set; }

    }
    public class renewalassignteam_list : result
    {
        public string campaign_gid { get; set; }
        public string employee_gid { get; set; }
        public string renewal_gid { get; set; }
        public string branch { get; set; }
        public string description { get; set; }
        public string mail_id { get; set; }
        public string team_name { get; set; }
        public string team_prefix { get; set; }
        public string branch_gid { get; set; }
        public string total_managers { get; set; }
        public string total_employees { get; set; }
    }
    public class renewalsummary_list : result
    {
        public string renewal_gid { get; set; }
        public string duration { get; set; }
        public string salesorder_gid { get; set; }
        public string customer_gid { get; set; }
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
    public class renewalsummary_list2 : result
    {
        public string renewal_gid { get; set; }
        public string duration { get; set; }
        public string customer_gid { get; set; }
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
    public class GetSalesOrder_list : result
    {

        public string salesorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string so_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string branch_name { get; set; }
        public string contact { get; set; }
        public string so_type { get; set; }
        public string Grandtotal { get; set; }
        public string salesorder_status { get; set; }
    }
    public class invoicetagsummary_list : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_amount { get; set; }
        public string customer_name { get; set; }
        public string invoice_date { get; set; }
        public string contact { get; set; }
        public string renewal_gid { get; set; }
      
    }
    public class unmappedinvoice_list1 : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_amount { get; set; }
        public string customer_name { get; set; }
        public string invoice_date { get; set; }
        public string contact { get; set; }

    }
    public class renewalview_list : result
    {
        public string so_referenceno1 { get; set; }
        public string salesperson_gid { get; set; }
        public string renewal_status { get; set; }
        public string currency_gid { get; set; }
        public string customer_gid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string total_amount { get; set; }
        public string renewal_date { get; set; }
        public string frequency_term { get; set; }
        public string renewal_mode { get; set; }

        public string gst_amount { get; set; }
        public string gst_number { get; set; }
        public string salesorder_date { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_contact_person { get; set; }
        public string user_gid { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string currency_code { get; set; }
        public string salesorder_gid { get; set; }
        public string customer_instruction { get; set; }
        public string exchange_rate { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string payment_days { get; set; }
        public string so_referencenumber { get; set; }
        public string shipping_to { get; set; }
        public string delivery_days { get; set; }
        public string so_remarks { get; set; }
        public string salesperson_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string uom_name { get; set; }
        public string qty_quoted { get; set; }
        public string selling_price { get; set; }
        public string price { get; set; }
        public string product_requireddate { get; set; }
        public string product_price { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string Grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string margin_percentage { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string order_instruction { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string totax { get; set; }
        public string tax_name4 { get; set; }
        public string total_price { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country_name { get; set; }

    }
    public class Viewrenewaldetail_list : result
    {

        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string product_remarks { get; set; }
        public string uom_name { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string salesorder_gid { get; set; }
        public string qty_quoted { get; set; }
        public string margin_amount { get; set; }
        public string margin_percentage { get; set; }
        public string product_price { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string price { get; set; }
        public string salesorderdtl_gid { get; set; }
        public string additional_discount { get; set; }

    }
    public class ProductSummaryRenewal_list : result
    {

        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string product_remarks { get; set; }
        public string uom_name { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string salesorder_gid { get; set; }
        public string qty_quoted { get; set; }
        public string margin_amount { get; set; }
        public string margin_percentage { get; set; }
        public string product_price { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string price { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }

    }
    public class GetTaxSegmentList1 : result
    {
        public string product_gid { get; set; }
        public string tax_prefix { get; set; }
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
        public double discount_percentage { get; set; }
        public double discount_amount { get; set; }

    }
    public class renewalsalesorder_list : result
    {
        public string so_referenceno1 { get; set; }
        public string salesperson_gid { get; set; }
        public string currency_gid { get; set; }
        public string customer_gid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string total_amount { get; set; }

        public string gst_amount { get; set; }
        public string gst_number { get; set; }
        public string salesorder_date { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_contact_person { get; set; }
        public string user_gid { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string currency_code { get; set; }
        public string salesorder_gid { get; set; }
        public string customer_instruction { get; set; }
        public string exchange_rate { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string payment_days { get; set; }
        public string so_referencenumber { get; set; }
        public string shipping_to { get; set; }
        public string delivery_days { get; set; }
        public string so_remarks { get; set; }
        public string salesperson_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string uom_name { get; set; }
        public string qty_quoted { get; set; }
        public string selling_price { get; set; }
        public string price { get; set; }
        public string product_requireddate { get; set; }
        public string product_price { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string Grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string margin_percentage { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string order_instruction { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string totax { get; set; }
        public string tax_name4 { get; set; }
        public string total_price { get; set; }


    }

    public class renewalsalesorderdetails_list : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string product_remarks { get; set; }
        public string uom_name { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string salesorder_gid { get; set; }
        public string qty_quoted { get; set; }
        public string margin_amount { get; set; }
        public string margin_percentage { get; set; }
        public string product_price { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }

        public string price { get; set; }
    }
    public class PosttempProduct_list : result
    {
        public string producttype_name { get; set; }
        public string productdiscount_amountvalue { get; set; }
        public string product_remarks { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
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
        public string customer_gid { get; set; }
        public string taxname3 { get; set; }
        public string taxname2 { get; set; }
        public string taxname1 { get; set; }
        public string discount_amount { get; set; }
        public string salesorder_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string renewal_gid { get; set; }
    }
    public class postagree_list : result
    {

        public string total_amount { get; set; }
        public string renewal_date { get; set; }
        public string agreement_date { get; set; }
        public string address1 { get; set; }
        public string slno { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string agreement_remarks { get; set; }
        public string product_remarks { get; set; }
        public string shipping_address { get; set; }
        public string branch_address { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string currency_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string contactemailaddress { get; set; }
        public string Grandtotal { get; set; }
        public string salesperson_gid { get; set; }
        public string gst_amount { get; set; }
        public string customer_contact_person { get; set; }
        public string tax_amount { get; set; }
        public string product_code { get; set; }
        public string tax_name { get; set; }
        public string customer_address2 { get; set; }
        public string customer_address { get; set; }
        public string productuom_gid { get; set; }
        public string customer_city { get; set; }
        public string additional_discount { get; set; }
        public string currencyexchange_gid { get; set; }
        public string countryname { get; set; }
        public string region_name { get; set; }
        public string customer_state { get; set; }
        public string grandtotal { get; set; }
        public string gst_number { get; set; }
        public string postal_code { get; set; }
        public string customercontact_name { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string branch_gid { get; set; }
        public string salesorder_date { get; set; }
        public string customercontact_gid { get; set; }
        public string exchange_rate { get; set; }
        public string customercontact_names { get; set; }
        public string addoncharge { get; set; }
        public string customer_Address { get; set; }
        public string currency_code { get; set; }
        public string so_referencenumber { get; set; }
        public string so_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string shipping_to { get; set; }
        public string tax_name4 { get; set; }
        public string txttaxamount_1 { get; set; }
        public string totalamount { get; set; }
        public string total_price { get; set; }
        public string productgroup_name { get; set; }
        public string user_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string freight_charges { get; set; }
        public string created_flag { get; set; }
        public string country_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string branch_name { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount_2 { get; set; }
        public string taxamount_3 { get; set; }
        public string taxamount_l { get; set; }
        public string selling_price_l { get; set; }
        public string totalamount_l { get; set; }
        public string discount_amount_l { get; set; }
        public string marginpercentage { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string termsandcondition { get; set; }
        public string buyback_charges { get; set; }
        public string addon_charge { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string productgroup_gid { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customerproduct_code { get; set; }
        public string display_field { get; set; }
        public string selling_price { get; set; }
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string vessel_name { get; set; }
        public string tax_amount_l { get; set; }
        public string tax_amount2_l { get; set; }
        public string tax_amount3_l { get; set; }
        public string product_discount { get; set; }
        public string price { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string order_type { get; set; }
        public string mrp { get; set; }
        public string taxamount_1 { get; set; }
        public string producttotalamount { get; set; }
    }
    public class Getrenewaltoinvoice_list : result
    {
        public string salesorder_gid { get; set; }
        public string renewal_gid { get; set; }
        public string shipping_to { get; set; }
        public string so_reference { get; set; }
        public string serviceorder_date { get; set; }
        public string customer_name { get; set; }
        public string grand_total { get; set; }
        public string customer_gid { get; set; }
        public string customercontact_name { get; set; }
        public string email { get; set; }
        public string customer_code { get; set; }
        public string termsandconditions { get; set; }
        public string customer_mobile { get; set; }
        public string addon_amount { get; set; }
        public string discount_amount { get; set; }
        public string customer_address { get; set; }
        public string order_total { get; set; }
        public string insurance_charges { get; set; }
        public string packing_charges { get; set; }
        public string buyback_charges { get; set; }
        public string freight_charges { get; set; }
        public string currencyexchange_gid { get; set; }
        public string customer_id { get; set; }
        public string currency_code { get; set; }
        public string currency_gid { get; set; }
        public string exchange_rate { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country_gid { get; set; }
        public string zip_code { get; set; }
        public string country_name { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string delivery_days { get; set; }
        public string payment_days { get; set; }
        public string gst_number { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string roundoff { get; set; }
        public string tax_amount { get; set; }
        public string tax_name4 { get; set; }
    }

    public class ordertoinvoiceproductsubmit_list1
    {
        public string taxsegment_gid { get; set; }
        public string taxseg_taxtotal { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string qty_requested { get; set; }
        public string productquantity { get; set; }
        public string salesorder_gid { get; set; }
        public string discountprecentage { get; set; }
        public string discount_amount { get; set; }
        public string tax1 { get; set; }
        public string tax2 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount1 { get; set; }
        public string producttotal_amount { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string unitprice { get; set; }
        public string display_field { get; set; }
        public string tax3 { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string cost_price { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount3 { get; set; }
        public string taxgid1 { get; set; }
        public string taxgid2 { get; set; }
        public string taxgid3 { get; set; }
        public string taxprecentage1 { get; set; }
        public string taxprecentage2 { get; set; }
        public string taxprecentage3 { get; set; }
        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string taxname3 { get; set; }
        public string product_desc { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string tax_prefix3 { get; set; }
    }
    public class renewaltoinvoicesubmit : result
    {
        public string customer_gid { get; set; }
        public string renewal_gid { get; set; }
        public string invoice_gid { get; set; }
        public string grandtotal { get; set; }
        public string currencygid { get; set; }
        public string invoiceref_no { get; set; }
        public string invoice_date { get; set; }
        public string payment_days { get; set; }
        public string salesorder_gid { get; set; }
        public string due_date { get; set; }
        public string customercontactperson { get; set; }
        public string customercontactnumber { get; set; }
        public string customeraddress { get; set; }
        public string customeremailaddress { get; set; }
        public string invoicetotalamount { get; set; }
        public string GrandTotalAmount { get; set; }
        public string invoicediscountamount { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string remarks { get; set; }
        public string termsandconditions { get; set; }
        public string exchange_rate { get; set; }
        public string branch_gid { get; set; }
        public string roundoff { get; set; }
        public string tax_gid { get; set; }
        public string tax_name4 { get; set; }
        public string tax_amount4 { get; set; }
        public string taxsegment_gid { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string buybackcharges { get; set; }
        public string forwardingCharges { get; set; }
        public string insurance_charges { get; set; }
        public string producttotalamount { get; set; }
        public string salestype { get; set; }
        public string dispatch_mode { get; set; }
        public string delivery_days { get; set; }
        public string payment { get; set; }
        public string deliveryperiod { get; set; }
        public string currency_code { get; set; }
        public string shipping_to { get; set; }
        public string sales_type { get; set; }
        public string bill_email { get; set; }
        public string customer_address { get; set; }
        public string  renewal_date { get; set; }
        public string frequency_terms { get; set; }
    }

}