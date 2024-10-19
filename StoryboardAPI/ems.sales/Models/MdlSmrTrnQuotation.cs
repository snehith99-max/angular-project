﻿using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{

    public class MdlSmrTrnQuotation : result
    {
        public List<GetProductsearch> GetProductsearch { get; set; }
        public List<Directeddetailslist1> Directeddetailslist1{ get; set; }
        public List<GetTaxSegmentList> GetTaxSegmentList { get; set; }
        public List<DirecteditQuotationList> directeditquotation_list { get; set; }
        public List<Quotationlist> Quotationlist { get; set; }
        public List<GetproductsCodes> ProductsCode { get; set; }
        public string subcompanyname { get; set; }
        public string subsymbol { get; set; }
        public string amendedcount { get; set; }
        public string approvecount { get; set; }
        public string enquiry_gid { get; set; }
        public List<Productsummaryamend_list> Productsummaryamend_list { get; set; }
        public List<quotationlastsixmonths_list> quotationlastsixmonths_list { get; set; }
        public List<quotationhistorylist> quotationhistorylist { get; set; }
        public List<quotationhistorysummarylist> quotationhistorysummarylist { get; set; }
        public List<quotationproduct_list> quotationproduct_list { get; set; }
        public List<quotation_list> quotation_list { get; set; }
        public List<productlist> product_list { get; set; }
        public List<GetSalesDropdown> GetSalesDtl { get; set; }
        public List<GetCurrencyCodeDropdown> GetCurrencyCodeDtl { get; set; }
        public List<GetOnchangecurrency> GetOnchangecurrency { get; set; }
        public List<templatelist> templatelist { get; set; }
        public List<GetTaxOnceDropdown> GetTaxOnceDtl { get; set; }
        public List<GetTaxTwiceDropdown> GetTaxTwiceDtl { get; set; }
        public List<GetTaxThriceDropdown> GetTaxThriceDtl { get; set; }
        public List<GetTaxFourSDropdown> GetTaxFourSDtl { get; set; }
        public List<GetProductNamesDropdown> GetProductNamesDtl { get; set; }
        public List<GetProductsCode> GetProductsCode { get; set; }
        public List<GetBranchDropdowns> GetBranchDt { get; set; }
        public List<GetCurrencyDropdowns> GetCurrencyDt { get; set; }
        public List<GetMailId> GetMailid { get; set; }
        public List<Gettemporarysummary> Gettemporarysummary { get; set; }
        public List<GetSendMail_MailIdquot> GetSendMail_MailIdquot { get; set; }

        public List<postsalesquotation_list> SO_list { get; set; } 
        public List<postsalesquotationdetails_list> Sq_list { get; set; }
        
        public List<GetCustomerDropdowns> GetCustomerDt { get; set; }
        public List<GetPersonDropdowns> GetPersonDt { get; set; }
        public List<GetProductDropdowns> GetProductDt { get; set; }
        public List<GetSendMail_MailIdq> GetSendMail_MailIdq { get; set; }
        public List<GetTax1> GetTax1Dtl { get; set; }
        public List<GetTax2> GetTax2Dtl { get; set; }
        public List<GetTax3> GetTax3Dtl { get; set; }
   
        public List<GetCustomerNames> CustomerNames { get; set; }
        public List<GetTaxSegmentListView> GetTaxSegmentListView { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
        public double totalprice { get; set; }
        public string quotation_referenceno1 { get; set; }
        public List<GetTandCDropdown> GetTermsandConditions { get; set; }
       
        public List<GetTermDropdown> terms_list { get; set; }
        public List<GetCustomerDetl> GetCustomerdetls { get; set; }
        public List<postquotation1_list> postquotation1_list { get; set; }
        public List<tempsummary_list> prodsummary_list { get; set; }
        public List<summarys_lists> summarys_lists { get; set; }
        public List<Post_List> Qoutepost_list { get; set; }
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
        public string termsandcondition { get; set; }
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }
        public string exchange_rate { get; set; }
        public string shipping_to { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string vessel { get; set; }
        public string user_name { get; set; }
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
        public double total_amount { get; set; }
        public double ltotalamount { get; set; }   
        public string pop_mail { get; set; }

    }
    public class GetSendMail_MailIdquot : result
    {
        public string cc_contact_emails { get; set; }
        public string to_customer_email { get; set; }
        public string customer_name { get; set; }
        public string quotation_gid { get; set; }
        public string quotation_reference { get; set; }
        public string due_date { get; set; }
        public string total_amount { get; set; }


    }
    public class postquotation1_list : result
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
    public class GetSendMail_MailIdq : result
    {
        public string cc_contact_emails { get; set; }
        public string to_customer_email { get; set; }
        public string customer_name { get; set; }
        public string quotation_gid { get; set; }
        public string quotation_reference { get; set; }


    }
    public class productsubmit__list : result
    {
        public string salesorder_gid { get; set; }
        public string product_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string product_code { get; set; }
        public string tax_prefix { get; set; }
        public string producttype_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string tax_amount { get; set; }
        public string selling_price { get; set; }
        public string currency_code { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string quantity { get; set; }
        public string tax_name { get; set; }
        public string productquantity { get; set; }
        public string unitprice { get; set; }
        public string productdiscount { get; set; }
        public string producttotal_amount { get; set; }
        public string exchange_rate { get; set; }
        public string customer_gid { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
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
        public string discount_amount { get; set; }
        public string discountprecentage { get; set; }
        public string product_remarks { get; set; }
        public string quotation_gid { get; set; }

    }
    public class taxSegments
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string taxAmount { get; set; }
    }
    public class ProductList1
    {
        public List<taxSegments> taxSegments { get; set; }
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
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string tax3 { get; set; }
        public string display_field { get; set; }
        public string unitprice { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class submitProducts
    {
        public List<ProductList1> ProductList { get; set; }
        public string taxsegment_gid { get; set; }
        public string qty_requested { get; set; }
        public string quantity { get; set; }
        public string discount_persentage { get; set; }
        public string discount_amount { get; set; }
        public string tax1 { get; set; }
        public string tax2 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount1 { get; set; }
        public string total_amount { get; set; }
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
    }
    public class GetTaxSegmentList : result
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

    public class GetProdTaxSegmentList : result
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

    public class postsalesquotationdetails_list : result
    {
        public string productgroup_name { get; set; }
        public string product_remarks { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string uom_name { get; set; }
        public string quotation_gid { get; set; }
        public string quantity { get; set; }
        public string discount_amount { get; set; }
        public string productuom_name { get; set; }
        public string product_price { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_prefix1 { get; set; }
        public string tax_prefix2 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string discount_percentage { get; set; }
        public string slno { get; set; }

        public string price { get; set; }
    }
    public class Quotationlist : result
    {

        public string gst_percentage { get; set; }
        public string quotation_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string insurance_charges { get; set; }
        public string packing_charges { get; set; }
        public string buyback_charges { get; set; }
        public string freight_charges { get; set; }
        public string roundoff { get; set; }
        public string customerenquiryref_number { get; set; }
        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string pricingsheet_refno { get; set; }
        public string pricingsheet_gid { get; set; }
        public string total_price { get; set; }
        public string enquiry_refno { get; set; }
        public string vessel_name { get; set; }
        public string salesperson_gid { get; set; }
        public string tax_name { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string remarks { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string termsandcondition { get; set; }
        public string customer_address { get; set; }
        public string customer_gid { get; set; }
        public string quotation_referencenumber { get; set; }
        public string quotation_date { get; set; }
        public string contact_person { get; set; }
        public string total_amount { get; set; }
        public string tax_gid { get; set; }
        public string currency_gid { get; set; }
        public string quotation_status { get; set; }
        public string contact_no { get; set; }
        public string contact_mail { get; set; }
        public string Grandtotal { get; set; }
        public string Grandtotal_l { get; set; }
        public string addon_charge { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string additional_discount { get; set; }
        public string currency_code { get; set; }
        public string termsandconditions { get; set; }
        public string created_date { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string exchange_rate { get; set; }
        public string customer_name { get; set; }
        public string quotation_remarks { get; set; }



    }
    public class  result
    {
        public bool status { get; set; }
        public string message { get; set; }


    }
    public class quotationlastsixmonths_list : result
    {
        public string months { get; set; }
        public string quotationamount {  get; set; }
        public string quotation_date { get; set; }

        public string quotation_gid { get; set; }
    }


    public class Productsummaryamend_list : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public double grand_total { get; internal set; }
        public string quotationdtl_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string uom_name { get; set; }
        public string product_price { get; set; }
        public string discount { get; set; }
        public string selling_price { get; set; }
        public string product_total { get; set; }
        public string tax { get; set; }
        public string quotation_gid { get; set; }
        public string qty_quoted { get; set; }
        public string mrp { get; set; }
        public string discount_percentage { get; set; }


    }

    public class GetOnchangecurrency
    {

        public string exchange_rate { get; set; }
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }

    }
    public class summarys_lists : result
    {
        public string tmpsalesorderdtl_gid { get; set; }
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

 
    public class GetTemporarysummary : result
    {
        public double total_price1 { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string salesorder_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string producttotalamount { get; set; }
        public string selling_price { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
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
        public string totalprice { get; set; }
 

    }

    public class GetCustomerNames : result
    {
        public string customer_gid { get; set; }
        public string customercontact_names { get; set; }
        public string customer_address { get; set; }

    }
    public class GetTax1 : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }

    }

    public class GetTax2 : result
    {
        public string tax_gid2 { get; set; }
        public string tax_name2 { get; set; }
        public string percentage { get; set; }


    }

    public class GetTax3 : result
    {
        public string tax_gid3 { get; set; }
        public string tax_name3 { get; set; }
        public string percentage { get; set; }


    }


    public class GetMailId : result
    {
        public string pop_server { get; set; }
        public string pop_port { get; set; }
        public string pop_username { get; set; }
        public string pop_password { get; set; }
        public string company_code { get; set; }
        public string company_name { get; set; }
        public string company_gid { get; set; }


    }

    public class GetProductDropdowns : result
    {
        public string product_gid { get; set; }
        public string productname { get; set; }

    }

    public class GetCurrencyDropdowns : result
    {
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }
        public string default_currency { get; set; }    
        public string exchange_rate { get; set; }

    }
    public class GetPersonDropdowns : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }

    }
    public class GetCustomerDropdowns : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }

    }
    public class GetBranchDropdowns : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }

    }
    public class productlist : result
    {
        public string customerproduct_code { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string qty_quoted { get; set; }
    }

    public class quotation_list : result
    {
        public string quotation_gid { get; set; }
        public string assign_to { get; set; }
        public string created_by { get; set; }
        public string quotation_date { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string enquiry_gid { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string quotation_type { get; set; }
        public string customer_gid { get; set; }
        public string user_firstname { get; set; }
        public string Grandtotal { get; set; }
        public string quotation_status { get; set; }



    }

    public class GetSalesDropdown : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }
    }

    public class GetCurrencyCodeDropdown : result
    {
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }
    }


    public class GetTaxOnceDropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
    }

    public class GetTaxTwiceDropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name2 { get; set; }
        public string percentage { get; set; }
    }

    public class GetTaxThriceDropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name3 { get; set; }
        public string percentage { get; set; }
    }

    public class GetTaxFourSDropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name4 { get; set; }
        public string percentage { get; set; }
    }

    public class GetProductNamesDropdown : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }

    public class GetProductsCode : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }

    }

    public class GetproductsCodes : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string selling_price { get; set; }

    }
    public class GetProductsearch
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
        public double discount_persentage { get; set; }

    }
    public class GetTaxSegmentLis : result
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

    public class GetTaxSegmentListView : result
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
    public class postsalesquotation_list : result
    {
        public string quotation_gid { get; set; }
        public string tax_nameno { get; set; }
        public string pincode { get; set; }
        public string customer_gid { get; set; }
        public string gst_number { get; set; }
        public string branch_gid { get; set; }
        public string salesorder_date { get; set; }
        public string branch_name { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string so_referencenumber { get; set; }
        public string customercontact_names { get; set; }
        public string customer_mobile { get; set; }
        public string customer_email { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string so_remarks { get; set; }
        public string shipping_to { get; set; }
        public string customer_address { get; set; }
        public string customer_name { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
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
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string buyback_charges { get; set; }
        public string quantity { get; set; }
        public string discountamount { get; set; }
        public string discountpercentage { get; set; }
        public string product_price { get; set; }
        public string tax_amount { get; set; }
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
        public string uom_gid { get; set; }
        public string totalamount { get; set; }
        public string termsandconditions { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string region_name { get; set; }
        public string pricesegment_name { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_amount4 { get; set; }
        public string tax_name4 { get; set; }
        public string salesperson_name { get; set; }
        public string customer_pin { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }
        public string customer_city { get; set; }
        public string country_name { get; set; }


    }

    public class GetTandCDropdown : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string termsandconditions { get; set; }
    }

    public class GetTermDropdown : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string termsandconditions { get; set; }

    }

    public class GetCustomerDetl : result
    {
        public string customercontact_names { get; set; }
        public string branch_name { get; set; }
        public string country_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string zip_code { get; set; }
        public string country_gid { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address2 { get; set; }
        public string address1 { get; set; }
        public string customercontact_gid { get; set; }
        public string customer_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string customer_name { get; set; }
        public string taxsegment_name { get; set; }
        public string pricesegment_name { get; set; }
        public string region_name { get; set; }
        public string gst_number { get; set; }
    }

    public class tempsummary_list : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string qty_requested { get; set; }
        public string product_requireddate { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string product_gid { get; set; }
        public string product_requireddateremarks { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxsegment_gid { get; set; }
        public string tax_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string tax_name2 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount3 { get; set; }
        public string selling_price { get; set; }
        public string slno { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string product_price { get; set; }
        public string price { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string taxseg_taxtotal { get; set; }
        
    }

    public class summaryprod_list : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string quotation_gid { get; set; }
        public string product_price { get; set; }
        public string product_code { get; set; }
        public string customerproduct_code { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string employee_gid { get; set; }
        public string selling_price { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string tax_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string vendor_gid { get; set; }
        public string product_requireddate { get; set; }
        public string product_requireddateremarks { get; set; }
        public string quotation_type { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string product_gid { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string slno { get; set; } 
        public string taxsegmenttax_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public List<ProductList1> ProductList { get; set; }
        
        public string qty_requested { get; set; }
        
        public string discount_persentage { get; set; }
        public string discount_amount { get; set; }
        public string tax1 { get; set; }
        public string tax2 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount1 { get; set; }
        public string total_amount { get; set; }
       
        public string productuom_gid { get; set; }
       
        public string productgroup_gid { get; set; }
      
        public string tax3 { get; set; }
    

    }

    public class Post_List : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string tax_name { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string salesorder_gid { get; set; }
        public string product_code { get; set; }
        public string address2 { get; set; }
        public string tax_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string quotation_gid { get; set; }
        public string quote_type { get; set; }
        public string product_gid { get; set; }
        public string assign_to { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string tax { get; set; }
        public string tax2 { get; set; }
        public string tax3 { get; set; }
        public string product_remarks { get; set; }
        public string selling_price { get; set; }
        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string taxname3 { get; set; }
        public string slno { get; set; }
        public string cuscontact_gid { get; set; }
        public string created_by { get; set; }
        public string product_requireddate { get; set; }
        public string productrequireddate_remarks { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string grand_total { get; set; }
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string price { get; set; }
        public string productgroup_name { get; set; }
        public string display_field { get; set; }
        public string uom_gid { get; set; }
        public string uom_name { get; set; }
        public string tax1_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string vendor_gid { get; set; }
        public string approved_by { get; set; }
        public string branch_name { get; set; }
        public string quotation_referencenumber { get; set; }
        public string quotation_date { get; set; }
        public string quotation_remarks { get; set; }
        public string mobile { get; set; }
        public string address1 { get; set; }
        public string email { get; set; }
        public string addoncharge { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customerbranch_name { get; set; }
        public string customercontact_gid { get; set; }
        public string so_remarks { get; set; }
        public string customercontact_names { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string customer_mobile { get; set; }
        public string customer_address { get; set; }
        public string customer_email { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string currencyexchange_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string total_amount { get; set; }
        public string pricingsheet_gid { get; set; }
        public string tax4_gid { get; set; }
        public string user_name { get; set; }
        public string vessel_name { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string tax_name4 { get; set; }
        public string pricingsheet_refno { get; set; }
        public string roundoff { get; set; }
        public string producttotalamount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string freightcharges { get; set; }
        public string buybackcharges { get; set; }
        public string insurance_charges { get; set; }
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
        public string currency_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string campaignGid { get; set; }
        public string taxamount { get; set; }
        public string tax_rate { get; set; }

    }
    public class postQuatation : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string address2 { get; set; }
        public string tax_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string quotation_gid { get; set; }
        public string quote_type { get; set; }
        public string product_gid { get; set; }
        public string assign_to { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string selling_price { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string slno { get; set; }
        public string cuscontact_gid { get; set; }
        public string created_by { get; set; }
        public string product_requireddate { get; set; }
        public string productrequireddate_remarks { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string grand_total { get; set; }
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string price { get; set; }
        public string productgroup_name { get; set; }
        public string display_field { get; set; }
        public string uom_gid { get; set; }
        public string uom_name { get; set; }
        public string tax1_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string vendor_gid { get; set; }
        public string approved_by { get; set; }
        public string branch_name { get; set; }
        public string quotation_referencenumber { get; set; }
        public string quotation_date { get; set; }
        public string quotation_remarks { get; set; }
        public string mobile { get; set; }
        public string address1 { get; set; }
        public string email { get; set; }
        public string addoncharge { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customerbranch_name { get; set; }
        public string customercontact_gid { get; set; }
        public string so_remarks { get; set; }
        public string customercontact_names { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string customer_mobile { get; set; }
        public string customer_address { get; set; }
        public string customer_email { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string currencyexchange_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string total_amount { get; set; }
        public string pricingsheet_gid { get; set; }
        public string tax4_gid { get; set; }
        public string user_name { get; set; }
        public string vessel_name { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string tax_name4 { get; set; }
        public string zip_code { get; set; }
        public string pricingsheet_refno { get; set; }
        public string roundoff { get; set; }
        public string producttotalamount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string freightcharges { get; set; }
        public string buybackcharges { get; set; }
        public string insurance_charges { get; set; }
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
        public string currency_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string campaignGid { get; set; }

    }
    public class quotationhistorylist : result
    {
        public string quotation_gid { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string quotation_date { get; set; }
        public string customer_name { get; set; }
        public string quotation_remarks { get; set; }
    }
    public class quotationhistorysummarylist : result
    {
        public string quotation_date { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string customer_contact_person { get; set; }
        public string user_firstname { get; set; }
        public string Grandtotal { get; set; }
        public string quotation_status { get; set; }
        public string quotation_gid { get; set; }
        public string customer_gid { get; set; }

    }


    public class quotationproduct_list : result
    {
        public string product_name { get; set; }
        public string qty_quoted { get; set; }
        public string quotation_gid { get; set; }



    }
    public class templatelist : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
    }
    public class mailsend_list : result
    {
        public result results { get; set; }
        public string AutoID_Key { get; set; }
    }
    public class MailsendAttachmentbase
    {
        public string name { get; set; }
        public string type { get; set; }
        public string data { get; set; }
    }
    public class DbsendAttachmentPath
    {
        public string path { get; set; }
    }
    public class attachment : result
    {
        public string filename { get; set; }
        public string typedata { get; set; }
    }
    public class file : result
    {
        public string File { get; set; }
    }

    public class DirecteditQuotationList : result
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
        public string taxsegment_gid { get; set; }
        public string totalamount { get; set; }
        public string tmpquotationdtl_gid { get; set; }
        public string product_requireddate { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string product_price { get; set; }
        public string price { get; set; }
    }
    public class Directeddetailslist1 : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string currency_code { get; set; }
        public string product_price { get; set; }
        public string qty_quoted { get; set; }
        public string customer_name { get; set; }
        public string quotation_date { get; set; }



    }
        

}