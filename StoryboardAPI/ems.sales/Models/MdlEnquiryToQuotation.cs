//using OfficeOpenXml.FormulaParsing.FormulaExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{

    public class MdlEnquiryToQuotation : result
    {
        public List<GetEnqtoQuotation>EnquiryQuotationlist { get; set; }
        public List<GetProductETQDropDown> GetProductETQ { get; set; }
        public List<GetOnChangeProductDropdownETQ> GetOnChangeProductETQ { get; set; }
        public List<GetProdTaxDropdownETQ> GetProdTaxETQ { get; set; }
        public List<GetOverallTaxDropDownETQ> GetOverallTaxETQ { get; set; }
        public List<GetTermDropDownETQ> GetTermETQ { get; set; }
        public List<GetTermsandConditionETQ> GetTermsandCondition_ETQ { get; set; }
        public List<GetEnqtoQuoteTempSummary>EnquiryQuotationTemp { get; set; }
        public List<EnquirytoQuotationsummary_list> EnquirytoQuotationsummarylist { get; set; }
        public List<PostEnquiryToQUotation_list> PostEnquiryToQUotation_list { get; set; }
        public List<GetQuoteDtl_List> GetQuoteDtllist { get; set; }
        public List<EditQuoteProductList> EditQuoteProduct_list { get; set; }
        public List<QuoteSegmentList> QuoteSegment_List { get; set; }
        public double grand_total { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public List<TaxSegmentLists> TaxSegmentLists { get; set; }
        public List<Gettemporarysummary1> temp_list1 { get; set; }
    }
    public class Gettemporarysummary1 : result
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
    public class TaxSegmentLists : result
    {
        public string product_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }

    }
    public class GetEnqtoQuotation : result
    {
        public string enquiry_gid { get; set; }
        public string customer_gid { get; set; }
        public string quotation_date { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_contact { get; set; }
        public string customer_mobile { get; set; }
        public string customer_email { get; set; }
        public string so_remarks { get; set; }
        public string user_name { get; set; }
        public string customer_address { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string quantity { get; set; }
        public string totalamount { get; set; }
        public string producttotalamount { get; set; }
        public string grandtotal { get; set; }
        public string city { get; set; }
        public string pricesegment_name { get; set; }
        public string pricesegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string country_gid { get; set; }
        public string customer_region { get; set; }
        public string credit_days { get; set; }
        public string customer_pin { get; set; }
        public string gst_number { get; set; }
    }

    public class GetProductETQDropDown : result
    {
        public string product_gid { get; set; } 
        public string product_name { get; set; } 
    }

    public class GetOnChangeProductDropdownETQ : result
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
    }

    public class GetProdTaxDropdownETQ : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
    }

    public class GetOverallTaxDropDownETQ : result
    {
        public string tax_gid4 { get; set; }
        public string percentage { get; set; }
        public string tax_name4 { get; set; }
    }

    public class GetTermDropDownETQ : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }

    }

    public class GetTermsandConditionETQ : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
    }
    public class GetEnqtoQuoteTempSummary : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string quotation_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }    
        public string tax_name { get; set; }
        public string slno { get; set; }
        public string tax_amount { get; set; }
        public string grand_total { get; set; }
  
        
    }

    public class EnquirytoQuotationsummary_list : result

    {
        public string product_name { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string tax_name { get; set; }
        public string unitprice { get; set; }
        public string product_code { get; set; }
        public string quotation_type { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string tax_amount { get; set; }
        public string totalamount { get; set; }
        public string enquiry_gid { get; set; }
      
    }

    public class PostEnquiryToQUotation_list : result
    {
        public string enquiry_gid { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string enq { get; set; }
        public string customer_name { get; set; }
        public string tax_name4 { get; set; }
        public string tax_amount4 { get; set; }
        public string assign_to { get; set; }
        public string branch_name { get; set; }
        public string customer_contact { get; set; }
        public string quotation_date { get; set; }
        public string quotation_referenceno1 { get; set; }
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
        public string currency_code { get; set; }
        public string currency_gid { get; set; }
        public string exchange_rate { get; set; }
        public string pricingsheet_gid { get; set; }
        public string pricingsheet_refno { get; set; }
        public string customer_address { get; set; }
        public string producttotalamount { get; set; }
        public string total_amount { get; set; }
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string roundoff { get; set; }
        public string customerenquiryref_number { get; set; }
        public string additional_discount { get; set; }
        public string buyback_charges { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string salesperson_gid { get; set; }
        
        public string tmpquotationdtl_gid { get; set; }
        public string address2 { get; set; }
        
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string quotation_gid { get; set; }
        public string quote_type { get; set; }
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
      
        public string quotation_referencenumber { get; set; }
        
        public string quotation_remarks { get; set; }
        public string mobile { get; set; }
        public string address1 { get; set; }
        public string email { get; set; }
        public string addoncharge { get; set; }
        public string customer_gid { get; set; }
       
        public string customerbranch_name { get; set; }
        public string customercontact_gid { get; set; }
        public string so_remarks { get; set; }
        public string customercontact_names { get; set; }
        
        public string termsandconditions { get; set; }
       
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string currencyexchange_gid { get; set; }
        
        public string tax4_gid { get; set; }
        public string user_name { get; set; }
        public string vessel_name { get; set; }
       
        public string zip_code { get; set; }
        
        public string freightcharges { get; set; }
        public string buybackcharges { get; set; }
      
        public string template_gid { get; set; }
        public string template_name { get; set; }
       
        public string campaignGid { get; set; }

    
}

    public class GetQuoteDtl_List : result
    {
        public string enquiry_gid { get; set; }
        public string tmpquotationdtl_gid { get; set; }
        public string quotation_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string uom_gid { get; set; }
        public string uom_name { get; set; }
        public string price { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_amount { get; set; }
    }

    public class EditQuoteProductList : result
    {
        public string tmpquotationdtl_gid { get; set; } 
        public string product_gid { get; set; } 
        public string product_name { get; set; } 
        public string productuom_name { get; set; } 
        public string productgroup_name { get; set; } 
        public string productgroup_gid { get; set; } 
        public string quantity { get; set; } 
        public string product_code { get; set; } 
        public string unitprice { get; set; } 
        public string discountpercentage { get; set; } 
        public string discountamount { get; set; } 
        public string totalamount { get; set; } 
        public string tax_name { get; set; } 
        public string tax_amount { get; set; } 
        public string tax_gid { get; set; } 
        public string taxamount1 { get; set; } 
        public string quotation_date { get; set; } 
        public string customer_name { get; set; } 
        public string qty_quoted { get; set; } 
        public string product_price { get; set; } 
        public string currency_code { get; set; } 
    }

    public class QuoteSegmentList : result
    {
        public string taxsegment_gid {get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_gid { get; set; }
        public string mrp_price { get; set; }
        public string tax_amount { get; set; }
        public string cost_price { get; set; }
    }
}