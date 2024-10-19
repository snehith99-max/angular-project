using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlQuotation360CRM : result
    {
        public List<GetBranchDropDownQuoteCRM> GetBranchQCRM { get; set; }
        public List<GetCurrencyDropdownQuoteCRM> GetCurrencyQCRM { get; set; }
        public List<GetProductDropdownQuoteCRM> GetProductDropdown_QCRM { get; set; }
        public List<GetTaxDropdownQuoteCRM> GetTaxQCRM { get; set; }
        public List<GetTaxDropdown2QuoteCRM> GetTax2QCRM { get; set; }
        public List<GetTermsDropdownQuoteCRM> GetTermsQCRM { get; set; }
        public List<GetCustomerDropDownQuoteCRM> GetCustomerQCRM { get; set; }
        public List<GetSalesDropdownQuoteCRM> GetSalesPersonQCRM { get; set; }
        public List<GetCustomerOnChangeQuoteCRM> GetCustomerOnChange_QCRM { get; set; }
        public List<GetProductOnChangeQuoteCRM> GetProductOnChange_QCRM { get; set; }
        public List<GetOnChangeTermsQuoteCRM> GetOnChangeTerms_QCRM { get; set; }
        public List<PostQuoteCRM_List> PostQCRMList { get; set; }
        public List<Quote360Product> Quotation360Product_list { get; set; }
        public double total_amount { get; set; }
        public double ltotalamount { get; set; }
    }

    public class GetBranchDropDownQuoteCRM : result
    {
        public string branch_gid { get; set;}
        public string branch_name { get; set;}
    }

    public class GetCurrencyDropdownQuoteCRM : result
    {
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }
        public string default_currency { get; set; }
        public string exchange_rate { get; set; }
    }
    public class GetProductDropdownQuoteCRM : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }

    public class GetTaxDropdownQuoteCRM : result
    {
        public string tax_gid { get; set; }
        public string tax1_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
    }

    public class GetTaxDropdown2QuoteCRM : result
    {
        public string tax_gid { get; set; }
        public string tax_name4 { get; set; }
        public string percentage { get; set; }
    }

    public class GetTermsDropdownQuoteCRM : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string termsandconditions { get; set; }
    }

    public class GetCustomerDropDownQuoteCRM : result
    {
        public string customer_gid { get; set;}
        public string customer_name { get; set;}
    }

    public class GetSalesDropdownQuoteCRM : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }
    }

    public class GetCustomerOnChangeQuoteCRM : result
    {
        public string customer_gid { get; set; }
        public string customercontact_names { get; set; }
        public string customercontact_gid { get; set; }
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
    }

    public class GetProductOnChangeQuoteCRM : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }

    }

    public class GetOnChangeTermsQuoteCRM : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string termsandconditions { get; set; }
    }

    public class Quote360Product : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }

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
        public string tax_gid { get; set; }
        public string tax_amount { get; set; }
        public string quotation_gid { get; set; }
        public string slno { get; set; }
        public string price { get; set; }
        
    }
    public class PostQuoteCRM_List : result
    {
        public string customer_name { get; set; }
        public string tax_amount4 { get; set; }
        public string tax_amount { get; set; }
        public string cuscontact_gid { get; set; }
        public string customercontact_names { get; set; }
        public string quotation_date { get; set; }
        public string branch_name { get; set; }
        public string customer_gid { get; set; }
        public string quotation_remarks { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string mobile { get; set; }
        public string address1 { get; set; }
        public string email { get; set; }
        public string addoncharge { get; set; }
        public string additional_discount { get; set; }
        public string exchange_rate { get; set; }
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }
        public string total_amount { get; set; }
        public string tax4_gid { get; set; }
        public string user_name { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string tax_name4 { get; set; }
        public string pricingsheet_gid { get; set; }
        public string pricingsheet_refno { get; set; }
        public string roundoff { get; set; }
        public string producttotalamount { get; set; }
        public string freightcharges { get; set; }
        public string buybackcharges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
    }
}