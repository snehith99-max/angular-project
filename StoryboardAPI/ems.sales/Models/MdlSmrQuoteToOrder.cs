﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrQuoteToOrder : result
    {
        public List<GetsummaryList> SO_list { get; set; }
        public List<summary_lists> summarys_lists { get; set; }
        public List<Gettemporarysummary> temp_list { get; set; }
        public List<directeditQuotationList> directeditquotation_list { get; set; }
        public List<GetOnchangeProdQTO> GetOnchangeProd_QTO { get; set; }
        public List<GetQTOTaxSegmentList> QTOTaxSegmentList { get; set; }
        public List<PostProductQuote> PostProductQuote { get; set; }
        public List<postsalesQuote_list> postsalesQuote_list { get; set; }
        public string customer_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string selling_price { get; set; }
        public string quantity { get; set; }
        public string discountamount { get; set; }
        public string discountpercentage { get; set; }
        public string totalamount { get; set; }
        public string vendor_gid { get; set; }
        public string slno { get; set; }
        public string Grandtotal { get; set; }
        public string tax_name4 { get; set; }
        public string total_price { get; set; }
        public string product_requireddate { get; set; }
        public string product_requireddateremarks { get; set; }
        public string vendor_price { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string producttotalprice { get; set; }
        public string finaltotal { get; set; }
        public string salesorder_gid { get; set; }
        public string frieght_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string branch_gid { get; set; }
        public string quotation_date { get; set; }
        public string customer_name { get; set; }
        public string customercontact_names { get; set; }
        public string branch_name { get; set; }
        public string salesorder_date { get; set; }
        public string customerbranch_gid { get; set; }
        public string customer_address { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string remarks { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string termsandconditions { get; set; }
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }
        public string exchange_rate { get; set; }
        public string shipping_to { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string vessel { get; set; }
        public string user_name { get; set; }
        public string tax_amount4 { get; set; }
        public string roundoff { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string freight_charges { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string uom_gid { get; set; }
        public string productuom_name { get; set; }
        public string quotation_gid { get; set; }
        public string tax_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string order_type { get; set; }
        public string total_amount { get; set; }
        public string quotation_referenceno1 { get; set; }
        public double ltotalamount { get; set; }
        public double grandtotal { get; set; }
        public double totalprice { get; set; }
        public double grand_total { get; set; }
        public double taxamount { get; set; }
    }
    public class PostProductQuote : result
    {
        public string producttype_name { get; set; }
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
        public string quotation_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string product_remarks { get; set; }
    }
    public class directeditQuotationList : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_gid { get; set; }
        public string selling_price { get; set; }
        public string quantity { get; set; }
        public string product_code { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string totalamount { get; set; }
        public string tmpquotationdtl_gid { get; set; }
        public string product_requireddate { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string product_price { get; set; }
        public string price { get; set; }
    }
    public class GetOnchangeProdQTO : result
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string selling_price { get; set; }
    }
    public class Gettemporarysummary : result
    {
        public double total_price1 { get; set; }
        public string tax_rate { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string salesorder_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string quantity { get; set; }
        public double discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string uom_gid { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string producttotalamount { get; set; }
        public string selling_price { get; set; }
        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string taxname3 { get; set; }
        public string slno { get; set; }
        public string product_requireddate { get; set; }
        public string productrequireddate_remarks { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string grand_total { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string margin_amount { get; set; }
        public string price { get; set; }
        public string grandtotal { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string customer_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string tax { get; set; }
        public string tax2 { get; set; }
        public string tax3 { get; set; }
        public string totalprice { get; set; }
        public string product_remarks { get; set; }
        public string taxamount { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_prefix1 { get; set; }
        public string tax_prefix2 { get; set; }
        public string tax_prefix3 { get; set; }


    }
    public class GetsummaryList : result
    {
        public string quotation_gid { get; set; }
        public string customer_gid { get; set; }
        public string branch_gid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string salesorder_date { get; set; }
        public string branch_name { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string so_referencenumber { get; set; }
        public string customercontact_names { get; set; }
        public string customer_mobile { get; set; }
        public string customer_email { get; set; }
        public string leadbank_gid { get; set; }
        public string zip_code { get; set; }
        public string city { get; set; }
        public string lead2campaign_gid { get; set; }
        public string so_remarks { get; set; }
        public string shipping_to { get; set; }
        public string customer_address { get; set; }
        public string customer_name { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string currency_gid { get; set; }
        public string user_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_name4 { get; set; }
        public string user_gid { get; set; }
        public string campaign_title { get; set; }
        public string campaign_gid { get; set; }
        public string gst_number { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string buyback_charges { get; set; }
        public string quantity { get; set; }
        public string discountamount { get; set; }
        public string discountpercentage { get; set; }
        public string product_price { get; set; }
        public string tax_amount4 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string insurance_charges { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string roundoff { get; set; }
        public string product_requireddate { get; set; }
        public string product_requireddateremarks { get; set; }
        public string selling_price { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string Grandtotal { get; set; }
        public string total_price { get; set; }
        public double total_price1 { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string customer_details { get; set; }
        public string customerenquiryref_number { get; set; }
        public string quotation_date { get; set; }
        public string contact_person { get; set; }
        public string contact_no { get; set; }
        public string contact_mail { get; set; }
        public string quotation_remarks { get; set; }
        public string user_firstname { get; set; }
        public string slno { get; set; }
        public string customerproduct_code { get; set; }
        public string display_field { get; set; }
        public string uom_name { get; set; }
        public string discount_percentage { get; set; }
        public string price { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string total_amount { get; set; }
        public string tax_amount { get; set; }
        public string uom_gid { get; set; }
        public string totalamount { get; set; }
        public string termsandconditions { get; set; }
        public string Grand_total { get; set; }


    }
    public class summary_lists : result
    {
        public string tmpsalesorderdtl_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string vendor_gid { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string product_code { get; set; }
        public string quotation_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string order_type { get; set; }
        public string qty_quoted { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string price { get; set; }
        public string selling_price { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string slno { get; set; }
        public string product_requireddate { get; set; }
        public string product_requireddateremarks { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string productgroup_name { get; set; }
        public string unitprice { get; set; }

        public string display_field { get; set; }
        public string quantity { get; set; }
        public string totalamount { get; set; }
        public string tax_gid { get; set; }
        public string tax_gid2 { get; set; }
        public string tax_gid3 { get; set; }
        public string lstmpquotationgid { get; set; }
        public string lsproductgroup_gid { get; set; }
        public string lsproductgroup { get; set; }
        public string lsproductname_gid { get; set; }
        public string lsproductname { get; set; }
        public string lsuom_gid { get; set; }
        public string lsvendor_gid { get; set; }
        public string lsuom { get; set; }
        public string lsuom_name { get; set; }
        public string lsunitprice { get; set; }
        public string lsrequired_date { get; set; }
        public string lsdiscountpercentage { get; set; }
        public string lsdiscountamount { get; set; }
        public string lstax_name1 { get; set; }
        public string lscustomerproduct_code { get; set; }
        public string lstax_name2 { get; set; }
        public string lstax_name3 { get; set; }
        public string lstaxamount_1 { get; set; }
        public string lstaxamount_2 { get; set; }
        public string lstaxamount_3 { get; set; }
        public string lstotalamount { get; set; }
        public string lssono { get; set; }
        public string lsquantity { get; set; }
        public string lsdisplay_field { get; set; }
        public string lslocalmarginpercentage { get; set; }
        public string lslocalsellingprice { get; set; }

        public string lsreqdate_remarks { get; set; }
    }
    public class GetQTOTaxSegmentList : result
    {
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }
    }
    public class postsalesQuote_list : result
    {
        public string total_amount { get; set; }
        public string customerinstruction { get; set; }
        public string branch_address { get; set; }
        public string user_gid { get; set; }
        public string campaign_title { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string currency_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string contactemailaddress { get; set; }
        public string Grandtotal { get; set; }
        public string campaign_gid { get; set; }
        public string salesperson_gid { get; set; }
        public string gst_amount { get; set; }
        public string customer_contact_person { get; set; }
        public string salesorder_gid { get; set; }
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
        public string designation { get; set; }
        public string fax { get; set; }
        public string fax_area_code { get; set; }
        public string fax_country_code { get; set; }
        public string branch_gid { get; set; }
        public string salesorder_date { get; set; }
        public string customerbranch_gid { get; set; }
        public string customercontact_gid { get; set; }
        public string exchange_rate { get; set; }
        public string customercontact_names { get; set; }
        public string addoncharge { get; set; }
        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string customer_Address { get; set; }
        public string currency_code { get; set; }
        public string so_referencenumber { get; set; }
        public string so_referenceno1 { get; set; }
        public string so_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string shipping_to { get; set; }
        public string tax_name4 { get; set; }
        public string txttaxamount_1 { get; set; }
        public string totalamount { get; set; }
        public string total_price { get; set; }
        public string delivery_days { get; set; }
        public string productgroup_name { get; set; }
        public string user_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string freight_charges { get; set; }
        public string main_branch { get; set; }
        public string created_flag { get; set; }
        public string did_number { get; set; }
        public string country_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string main_contact { get; set; }
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
        public string salesorder_refno { get; set; }
        public string vendor_gid { get; set; }
        public string slno { get; set; }
        public string product_requireddate { get; set; }
        public string product_requireddateremarks { get; set; }
        public string vendor_price { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string order_type { get; set; }
        public string mrp { get; set; }
        public string taxamount_1 { get; set; }
        public string producttotalamount { get; set; }
        public string renewal_date { get; set; }
        public string renewal_mode { get; set; }
        public string frequency_terms { get; set; }
        public string frequency_term { get; set; }
    }
}