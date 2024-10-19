﻿using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnCustomerEnquiry : result
    {
        public List<postlead_list> postlead_list { get; set; }
        public List<GetCusEnquiry> cusenquiry_list { get; set; }
        public List<prodenquirylist> prodenquiry_list { get; set; }
        public List<GetLeadDropdown> GetLeadDtl { get; set; }
        public List<GetTeamDropdown> GetTeamDtl { get; set; }
        public List<GetEmployeeDropdown> GetEmployeeDtl { get; set; }
        public List<GetProductGrp> GetProductGrp { get; set; }
        public List<GetProducts> GetProducts { get; set; }
        public List<GetProductUnits> GetProductUnits { get; set; }
        public List<GetCustomername> GetCustomername { get; set; }
        public List<GetLeadname> GetLeadname { get; set; }
        public List<GetCustomer> GetCustomer { get; set; }
        public List<GetLead> GetLead { get; set; }
        public List<GetProductsName> GetProductsName { get; set; }
        public List<productsummarys_list> productsummarys_list { get; set; }
        public List<productslist> productslist { get; set; }
        public List<GetCurrencyDetsDropdown> GetCurrencyDets { get; set; }
        public List<GetProductDetDropdown> GetProductDets { get; set; }
        public List<GetProductNameDetsDropdown> GetProductNameDets { get; set; }
        public List<GetFirsttaxDropdown> GetFirstTax { get; set; }
        public List<GetSecondtaxDropdown> GetSecondTax { get; set; }
        public List<GetThirdtaxDropdown> GetThirdTax { get; set; }
        public List<GetFourthtaxDropdown> GetFourthTax { get; set; }
        public List<GetQOSummaryList> Quote_list { get; set; }
        public List<GetTempsummary> EnqtoQuote_list { get; set; }
        public List<Productsummarys_lists> Productsummarys_lists { get; set; }
        public List<GetTermsDropdown> terms_lists { get; set; }
        public List<postquotation_list> postquotation_list { get; set; }
        public List<GetAssignDropdown> GetEmployeePerson { get; set; }
        public List<GetReceiveQuoteDtl_List> GetReceiveQuoteDtlList { get; set; }
        public List<PostAll> post_list { get; set; }
        public string enquiry_gid { get; set; }
        public List<GetBranchDetsDropdown> GetBranchDet { get; set; }
        public List<document_list> document_list { get; set; }
        public List<proposal_list> proposal_list { get; set; }
        public List<uploaddocument> uploaddocument { get; set; }
        public List<postproposal_list> postproposal_list { get; set; }
        public List<proposalsummary_list> proposalsummary_list { get; set; }
        public List<enquirylist> enquiry_list { get; set; }
        public List<EditEtoQ_list> editenquirytoquote_list { get; set; }

        public double producttotalamount { get; set; }
        public double grandtotal { get; set; }
        public double grand_total { get; set; }

        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string qty_requested { get; set; }
        public string potential_value { get; set; }
        public string enq { get; set; }

       public List <DirecteditenquiryList> directeditenquiry_list { get; set; }
       public List <Directeddetailslist> Directeddetailslist { get; set; }
        public string leadstage_name { get; set; }
        public string internal_notes { get; set; }

    }
    public class postquotation_list : result
    {

        public double enquiry_refno { get; set; }
        public string employee_gid { get; set; }
        public string enq { get; set; }
        public string branch_gid { get; set; }
        public string quotation_date { get; set; }

        public string enquiry_gid { get; set; }

        public string quotationdate { get; set; }

        public string customercontactperson { get; set; }

        public string remarks { get; set; }
        public string grandtotal { get; set; }
        public string template_content { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string addon_charge { get; set; }
        public string customer_mobile { get; set; }
        public string customer_email { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string pricingsheet_gid { get; set; }
        public string pricingsheet_refno { get; set; }
        public string customer_address { get; set; }
        public string producttotalamount { get; set; }
        public string total_amount { get; set; }
        public string gst_percentage { get; set; }
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string salesperson_gid { get; set; }
        public string roundoff { get; set; }
        public string customerenquiryref_number { get; set; }
        public string additional_discount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string customer_gid { get; set; }
        public string branch_name { get; set; }
        public string customer_contact { get; set; }
        public string currency_gid { get; set; }
        public string customerbranch_name { get; set; }
        public string customer_name { get; set; }
        public string quotation_referenceno1 { get; set; }




    }
    public class productslist : result
    {
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string qty_requested { get; set; }
        public string product_requireddate { get; set; }
        public string potential_value { get; set; }
        public string product_requireddateremarks { get; set; }
        public string product_gid { get; set; }


    }

    public class productsummarys_list : result
    {
        public string enquiry_type { get; set; }
        public string display_field { get; set; }

        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string qty_requested { get; set; }
        public string product_requireddate { get; set; }
        public string potential_value { get; set; }
        public string product_requireddateremarks { get; set; }
        public string product_gid { get; set; }
        public string stockable { get; set; }
        public string BOM { get; set; }
        public string product_type { get; set; }
        public string tmpquotationdtl_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string totalamount { get; set; }
        public string taxname1 { get; set; }
        public string taxamount1 { get; set; }





    }
    public class GetProductsName : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }


    }

    public class GetCustomer : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customercontact_name { get; set; }
        public string country_name { get; set; }
        public string contact_email { get; set; }
        public string mobile { get; set; }
        public string zip_code { get; set; }
        public string country_gid { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address2 { get; set; }
        public string contact_address { get; set; }
        public string branch_name { get; set; }
        public string customercontact_gid { get; set; }
        public string customerbranch_name { get; set; }
        public string contact_number { get; set; }
        public string Address { get; set; }

    }

    public class GetLead : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string country_name { get; set; }
        public string contact_email { get; set; }
        public string mobile { get; set; }
        public string pincode { get; set; }
        public string country_gid { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address2 { get; set; }
        public string contact_address { get; set; }
        public string branch_name { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string leadbankbranch_name { get; set; }
        public string contact_number { get; set; }
        public string Address { get; set; }

    }

    public class GetCustomername : result
    {
        public string customer_name { get; set; }
        public string customer_gid { get; set; }

    }
    public class GetLeadname : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }

    }

    public class GetTermsDropdown : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }

    }

    public class GetProductGrp : result
    {
        public string productgroup_name { get; set; }
        public string productgroup_gid { get; set; }

    }
    public class GetProducts : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }

    }
    public class GetProductUnits : result
    {
        public string productuom_name { get; set; }
        public string uom_gid { get; set; }

    }


    public class GetCusEnquiry : result
    {
        public string enquiry_gid { get; set; }
        public string enquiry_date { get; set; }
        public string enquiry_refno { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string created_date { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string contact_details { get; set; }
        public string campaign { get; set; }
        public string potorder_value { get; set; }
        public string lead_status { get; set; }
        public string enquiry_status { get; set; }
        public string customer_rating { get; set; }
        public string user_firstname { get; set; }
        public string leadstage_gid { get; set; }
        public string internal_notes { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }
        public string product_requireddateremarks { get; set; }
        public string product_requireddate { get; set; }
        public string customerproduct_code { get; set; }
        public string qty_requested { get; set; }
        public string potential_value { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer { get; set; }
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string branch_gid { get; set; }
        public string contact_number { get; set; }
        public string leadbank_state { get; set; }
        public string leadbank_city { get; set; }
        public string leadbank_pin { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string customercontact_name { get; set; }
        public string contact_email { get; set; }
        public string customerbranch_name { get; set; }
        public string contact_address { get; set; }
        public string enquiry_remarks { get; set; }
        public string customer_requirement { get; set; }
        public string landmark { get; set; }
        public string closure_date { get; set; }
        public string contact_person { get; set; }
        public string i { get; set; }
        public string currency_gid { get; set; }
        public string created_by { get; set; }
        public string quotationdate { get; set; }
        public string assign_to { get; set; }
        public string branch_prefix { get; set; }

        public string customercontactperson { get; set; }

        public string remarks { get; set; }
        public string grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string addon_charge { get; set; }
        public string contact_no { get; set; }
        public string contact_mail { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string pricingsheet_gid { get; set; }
        public string pricingsheet_refno { get; set; }
        public string customer_address { get; set; }
        public string total_price { get; set; }
        public string total_amount { get; set; }
        public string gst_percentage { get; set; }
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string salesperson_gid { get; set; }
        public string roundoff { get; set; }
        public string customerenquiryref_number { get; set; }
        public string additional_discount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string customercontact_gid { get; set; }

    }

    public class GetLeadDropdown : result
    {
        public string leadstage_gid { get; set; }
        public string leadstage_name { get; set; }


    }

    public class GetTeamDropdown : result
    {
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }


    }

    public class GetEmployeeDropdown : result
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }

    }

    public class GetCurrencyDetsDropdown : result
    {
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string default_currency { get; set; }
    }

    public class GetProductDetDropdown : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }

    public class GetProductNameDetsDropdown : result
    {
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }
        public string unitprice { get; set; }
    }

    public class GetFirsttaxDropdown : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
    }

    public class GetSecondtaxDropdown : result
    {
        public string tax_gid2 { get; set; }
        public string tax_name2 { get; set; }
        public string percentage { get; set; }

    }
    public class GetThirdtaxDropdown : result
    {
        public string tax_gid3 { get; set; }
        public string tax_name3 { get; set; }
        public string percentage { get; set; }

    }

    public class GetFourthtaxDropdown : result
    {
        public string tax_gid4 { get; set; }
        public string tax_name4 { get; set; }
        public string percentage { get; set; }
    }

    public class GetQOSummaryList : result
    {
        public string enquiry_gid { get; set; }
        public string tmpquotationdtl_gid { get; set; }
        public string quotation_gid { get; set; }
        public string product_gid { get; set; }
        public string slno { get; set; }
        public string grand_total { get; set; }
        public string quotation_date { get; set; }
        public string branch_name { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string so_referencenumber { get; set; }
        public string customer_contact { get; set; }
        public string customer_mobile { get; set; }
        public string customer_email { get; set; }
        public string so_remarks { get; set; }
        public string customer_address { get; set; }
        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string product_price { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount4 { get; set; }
        public string tax_amount3 { get; set; }
        public string product_requireddate { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_name4 { get; set; }
        public string product_requireddateremarks { get; set; }
        public string totalamount { get; set; }
        public string selling_price { get; set; }
        public string producttotalamount { get; set; }
        public string grandtotal { get; set; }

    }

    public class GetReceiveQuoteDtl_List : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string quotation_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
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
        public string quotation_date { get; set; }
        public string approved_by { get; set; }

    }
    public class GetTempsummary : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string product_code { get; set; }
       
        public string enq { get; set; }
        public string productgroup_name { get; set; }
        public string quotation_gid { get; set; }
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
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }

        public string price { get; set; }
      
    }

    public class Productsummarys_lists : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string quotation_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string quotation_type { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_price { get; set; }
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

    }

    public class GetAssignDropdown : result
    {
        public string campaign_gid { get; set; }
        public string user_firstname { get; set; }
        public string campaign_title { get; set; }
        public string employee_gid { get; set; }
    }

    public class PostAll : result
    {
        public string leadbank_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string enquiry_date { get; set; }
        public string enquiry_refno { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string contact_details { get; set; }
        public string enquiry_status { get; set; }
        public string customer_rating { get; set; }
        public string user_firstname { get; set; }
        public string customer_gid { get; set; }
        public string branch_gid { get; set; }
        public string contact_number { get; set; }
        public string customercontact_name { get; set; }
        public string contact_email { get; set; }
        public string customerbranch_name { get; set; }
        public string contact_address { get; set; }
        public string enquiry_remarks { get; set; }
        public string customer_requirement { get; set; }
        public string landmark { get; set; }
        public string closure_date { get; set; }
        public string contact_person { get; set; }
        public string employee_gid { get; set; }
        public string i { get; set; }
        public string campaign_gid { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_code { get; set; }
        public string product_requireddateremarks { get; set; }
        public string product_requireddate { get; set; }
        public string customerproduct_code { get; set; }
        public string qty_requested { get; set; }
        public string potential_value { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string lspage { get; set; }

        public string leadbank_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string leadbankbranch_name { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
    }


    public class GetBranchDetsDropdown : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }


    public class proposal_list : result
    {
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string proposal_gid { get; set; }
        public string created_date { get; set; }
        public string template_name { get; set; }
        public string created_by { get; set; }





    }

    public class document_list : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }





    }
    public class postproposal_list : result
    {
        public string customer_name { get; set; }

        public string document_path { get; set; }

        public string enquiry_gid { get; set; }
        public string proposal_name { get; set; }
        public string template_content { get; set; }

        public string proposal_from { get; set; }








    }
    public class proposalsummary_list : result
    {
        public string user_firstname { get; set; }
        public string created_date { get; set; }
        public string proposal_gid { get; set; }
        public string proposal_from { get; set; }
        public string document_path { get; set; }
        public string proposal_name { get; set; }
        public string leadbank_gid { get; set; }
        public string quotation_gid { get; set; }

        public string template_name { get; set; }
        public string enquiry_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string created_by { get; set; }






    }

    public class uploaddocument : result
    {
        public string doc_gid { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }






    }

    public class enquirylist : result
    {
        public string enquiry_gid { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
        public string enquiry_date { get; set; }
        public string product_gid { get; set; }
        public string branch_name { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string contact_person { get; set; }
        public string assign_to { get; set; }
        public string contact_email { get; set; }
        public string enquiry_remarks { get; set; }
        public string contact_address { get; set; }
        public string customer_requirement { get; set; }
        public string landmark { get; set; }
        public string assign_person { get; set; }

        public string closure_date { get; set; }
        public string contact_number { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string potential_value { get; set; }
        public string product_requireddate { get; set; }
        public string product_requireddateremarks { get; set; }
        public string branch_gid { get; set; }
        public string potorder_value { get; set; }
        public string enquirydtl_gid { get; set; }

    }
    public class EditEtoQ_list : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string totalamount { get; set; }
        public string unitprice { get; set; }
        public string price { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_gid { get; set; }
        public string tmpquotationdtl_gid { get; set; }
    }

    // DIRECT ENQUIRY PRODUCT EDIT
    public class DirecteditenquiryList : result
    {
        public string tmpsalesenquiry_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string potential_value { get; set; }
        public string product_requireddate { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_gid { get; set; }

    }

    public class Directeddetailslist : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string currency_code { get; set; }
        public string product_price { get; set; }
        public string qty_quoted { get; set; }
        public string customer_name { get; set; }
        public string quotation_date { get; set; }


    }

    public class prodenquirylist : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string totalamount { get; set; }
        public string product_requireddate { get; set; }
        public string potential_value { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_gid { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
    }

    public class postlead_list : result
    {
        public string customer_code { get; set; }
        public string customer_gid { get; set; }
        public string customer_id { get; set; }
        public string company_website { get; set; }
        public string customer_name { get; set; }
        public string leadbank_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string customer_address { get; set; }
        public string customer_address2 { get; set; }
        public string customer_city { get; set; }
        public string gst_number { get; set; }
        public string countryname { get; set; }
        public string region { get; set; }
        public string customer_state { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string customercontact_name { get; set; }
        public string taxname { get; set; }
        public string designation { get; set; }
        public string email { get; set; }
        public string postal_code { get; set; }
        public string fax { get; set; }
        public string customer_region { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string tax_no { get; set; }
        public string sales_person { get; set; }
        public string currency { get; set; }
        public string credit_days { get; set; }
        public string billemail { get; set; }
        public string fax_country_code { get; set; }
        public string country_code { get; set; }
        public string address1 { get; set; }
        public string phone1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string currency_code { get; set; }
        public string region_name { get; set; }
        public string customerbranch_name { get; set; }
        public string customer_type { get; set; }
        public string taxsegment_name { get; set; }
        public string pricesegment_name { get; set; }
        public string country_gid { get; set; }
        public string country_name { get; set; }
        public string currencyexchange_gid { get; set; }
        public string customer_pin { get; set; }
        public string mobile { get; set; }

        public getmobile mobiles { get; set; }



    }
}



