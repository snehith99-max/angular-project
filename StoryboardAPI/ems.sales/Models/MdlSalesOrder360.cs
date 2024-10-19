using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSalesOrder360 : result
    {
        public List<GetBranchDropdownSOCRM> GetBranchSOCRM { get; set; }
        public List<GetCustomerDropdownSOCRM> GetCustomerSOCRM { get; set; }
        public List<GetPersonDropdownSOCRM> GetPersonSOCRM { get; set; }
        public List<GetTaxoneDropdownSOCRM> GetTax1SOCRM { get; set; }
        public List<GetTaxFourDropdownSOCRM> GetTax4SOCRM { get; set; }
        public List<GetProductNamDropdownSOCRM> GetProductNameSOCRM { get; set; }
        public List<GetTandCDropdownSOCRM> GetTermsandConditionsSOCRM { get; set; }
        public List<GetCustomerDetSOCRM> GetCustomerdetSOCRM { get; set; }
        public List<GetOnchangeCurrencySOCRM> GetOnchangecurrencySOCRM { get; set; }
        public List<getproductsCodeSOCRM> ProductsCodesSOCRM { get; set; }
        public List<salesorders_listSOCRM> salesorderslistSOCRM { get; set; }
        public List<GetTermDropdownSOCRM> terms_listSOCRM { get; set; }
        public List<postsales_listSOCRM> postsaleslistSOCRM { get; set; }
        public List<Directeddetailslist2SOCRM> DirecteddetailslistSOCRM { get; set; }
        public List<DirecteditSalesorderListsocrm> directeditsalesorder_listsocrm { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }


        public class GetBranchDropdownSOCRM : result
        {
            public string branch_gid { get; set; }
            public string branch_name { get; set; }

        }
        public class GetCustomerDropdownSOCRM : result
        {
            public string customer_gid { get; set; }
            public string customer_name { get; set; }
            public string leadbank_gid { get; set; }
            public string leadbank_name { get; set; }

        }
        public class GetPersonDropdownSOCRM : result
        {
            public string user_gid { get; set; }
            public string user_name { get; set; }

        }

        public class GetTaxoneDropdownSOCRM : result
        {
            public string tax_gid { get; set; }
            public string tax1_gid { get; set; }
            public string tax_name { get; set; }
            public string percentage { get; set; }

        }
        public class GetTaxFourDropdownSOCRM : result
        {
            public string tax_gid { get; set; }
            public string tax_name4 { get; set; }
            public string percentage { get; set; }

        }
        public class GetProductNamDropdownSOCRM : result
        {
            public string product_gid { get; set; }
            public string product_name { get; set; }

        }

        public class GetTandCDropdownSOCRM : result
        {
            public string template_gid { get; set; }
            public string template_name { get; set; }
            public string termsandconditions { get; set; }
        }

        public class GetCustomerDetSOCRM : result
        {
            public string customercontact_names { get; set; }
            public string branch_name { get; set; }
            public string country_name { get; set; }
            public string customer_email { get; set; }
            public string customer_mobile { get; set; }
            public string zip_code { get; set; }
            public string country_gid { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string address2 { get; set; }
            public string customer_address { get; set; }
            public string customercontact_gid { get; set; }
            public string customer_gid { get; set; }
        }

        public class GetOnchangeCurrencySOCRM
        {

            public string exchange_rate { get; set; }
            public string currency_code { get; set; }

        }
        public class getproductsCodeSOCRM : result
        {
            public string product_gid { get; set; }
            public string product_code { get; set; }
            public string productgroup_name { get; set; }
            public string productuom_name { get; set; }
            public string productuom_gid { get; set; }
            public string productgroup_gid { get; set; }
            public string product_name { get; set; }
            public string unitprice { get; set; }

        }

        public class salesorders_listSOCRM : result
        {
            public string total_amount { get; set; }
            public string unit { get; set; }
            public string grand_total { get; set; }
            public string grandtotal { get; set; }
            public string tmpsalesenquiry_gid { get; set; }
            public string tmpsalesorderdtl_gid { get; set; }
            public string created_by { get; set; }
            public string salesorder_gid { get; set; }
            public string customerproduct_code { get; set; }
            public string selling_price { get; set; }
            public string productuom_gid { get; set; }
            public string productgroup_gid { get; set; }
            public string product_name { get; set; }

            public string employee_gid { get; set; }
            public string product_gid { get; set; }
            public string product_code { get; set; }
            public string slno { get; set; }
            public string productgroup_name { get; set; }
            public string product_price { get; set; }
            public string display_field { get; set; }

            public string qty_quoted { get; set; }
            public string discount_percentage { get; set; }
            public string margin_percentage { get; set; }
            public string margin_amount { get; set; }
            public string discount_amount { get; set; }
            public string uom_gid { get; set; }
            public string uom_name { get; set; }

            public string price { get; set; }
            public string tax_name { get; set; }
            public string tax_name2 { get; set; }
            public string tax_name3 { get; set; }
            public string tax_gid { get; set; }
            public string tax2_gid { get; set; }
            public string tax3_gid { get; set; }

            public string vendor_gid { get; set; }

            public string tax_amount { get; set; }
            public string tax_amount2 { get; set; }
            public string tax_amount3 { get; set; }
            public string tax_percentage { get; set; }
            public string tax_percentage2 { get; set; }

            public string tax_percentage3 { get; set; }
            public string order_type { get; set; }
            public string product_requireddateremarks { get; set; }
            public string product_requireddate { get; set; }
            public double unitprice { get; set; }
            public double quantity { get; set; }
            public string productuom_name { get; set; }
            public string totalamount { get; set; }
            public string producttotalamount { get; set; }
            public double discountpercentage { get; set; }
            public string discountamount { get; set; }
            public string product_type { get; set; }

        }

        public class GetTermDropdownSOCRM : result
        {
            public string template_gid { get; set; }
            public string template_name { get; set; }
            public string termsandconditions { get; set; }

        }

        public class postsales_listSOCRM : result
        {

            public string total_amount { get; set; }
            public string tax_amount4 { get; set; }
            public string currency_gid { get; set; }
            public string leadbank_gid { get; set; }
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


        }

        public class Directeddetailslist2SOCRM : result
        {
            public string product_gid { get; set; }
            public string product_name { get; set; }
            public string currency_code { get; set; }
            public string product_price { get; set; }
            public string qty_quoted { get; set; }
            public string customer_name { get; set; }
            public string salesorder_date { get; set; }



        }

        public class DirecteditSalesorderListsocrm : result
        {
            public string product_code { get; set; }
            public string product_gid { get; set; }
            public string product_name { get; set; }
            public string productuom_name { get; set; }
            public string productgroup_name { get; set; }
            public string productgroup_gid { get; set; }
            public string quantity { get; set; }
            public string unitprice { get; set; }
            public string discount_percentage { get; set; }
            public string discountamount { get; set; }
            public string totalamount { get; set; }
            public string tax_name { get; set; }
            public string tax_gid { get; set; }
            public string tax_amount { get; set; }
            public string tmpsalesorderdtl_gid { get; set; }
        }
    }
}