﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace ems.crm.Models
{
    public class MdlShopifyCustomer : result
    {
        public List<shopifycustomerlist> shopifycustomerlist { get; set; }
        public List<shopifycustomers_list> shopifycustomers_list { get; set; }
        public List<shopifycustomersassigned_list> shopifycustomersassigned_list { get; set; }
        public List<customerassignedcount_list> customerassignedcount_list { get; set; }
        public List<customertotalcount_list> customertotalcount_list { get; set; }
        public List<shopifycustomersunassigned_list> shopifycustomersunassigned_list { get; set; }
        public List<customerunassignedcount_list> customerunassignedcount_list { get; set; }

        public List<shopifyordersummary_list> shopifyordersummary_list { get; set; }
        public List<shopifypaymentsummary_list> shopifypaymentsummary_list { get; set; }
        public List<shopifyordercountsummary_list> shopifyordercountsummary_list { get; set; }
        public List<shopifyorderdtl> shopifyorderdtl { get; set; }
        public List<shopifyproductordersummary_list> shopifyproductordersummary_list { get; set; }

    }
    //code by snehith///////////////
    public class shopifyproductordersummary_list 
    {
        public string customerproduct_code { get; set; }
        public string product_price { get; set; }
        public string qty_quoted { get; set; }
        public string product_name { get; set; }
        public string tax_amount { get; set; }
        public string taxable { get; set; }
        public string tax_name { get; set; }
        public string discount_amount { get; set; }

    }
    public class shopifyorderdtl : result
    {
        public string salesorderdtl_gid { get; set; }
    public string salesorder_gid { get; set; }
    public string product_gid { get; set; }
    public string shopify_lineitemid { get; set; }
    public string shopify_orderid { get; set; }
    public string product_name { get; set; }
    public string product_price { get; set; }
    public string qty_quoted { get; set; }
    public string product_price_l { get; set; }
    public string selling_price { get; set; }
    public string price_l { get; set; }

    }
    public class shopifyordercountsummary_list : result
    {
        public string order_paidcount { get; set; }
        public string order_penidngcount { get; set; }
        public string order_refundedcount { get; set; }
        public string order_partiallycount { get; set; }
        public string total_order { get; set; }
        public string product_count { get; set; }
    }
    public class shopifypaymentsummary_list : result
    {
        
             public string status_flag { get; set; }
        public string shopifyorder_number { get; set; }
        public string shopify_orderid { get; set; }
        public string customer_contact_person { get; set; }
        public string Grandtotal { get; set; }
        public string salesorder_status { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_date { get; set; }
        public string item_count { get; set; }
        public string customer_address { get; set; }
        


    }
    public class shopifyordersummary_list : result
    {
        public string status_flag { get; set; }
        public string customer_address { get; set; }
        public string shopifyorder_number { get; set; }
        public string shopify_orderid { get; set; }
        public string customer_contact_person { get; set; }
        public string Grandtotal { get; set; }
        public string salesorder_status { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_date { get; set; }
        public string item_count { get; set; }
        

    }
    public class customerassignedcount_list : result
    {
        public string customer_assigncount { get; set; }
    }
    public class getorders
    {

        public bool status { get; set; }
    }
    public class customertotalcount_list : result
    {
        public string customer_totalcount { get; set; }
    }
    public class customerunassignedcount_list : result
    {
        public string unassign_count { get; set; }
    }
    public class get
    {

        public bool status { get; set; }
        public string message { get; set; }
    }
    public class shopifyinventorystocksend : result
    {



        public shopifyinventorystocksendlist[] shopifyinventorystocksendlist;
    }
    public class shopifyinventorystocksendlist
    {
        public string Status { get; set; }
        public string avg_lead_time { get; set; }
        public string batch_flag { get; set; }
        public string cost_price { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string currency_code { get; set; }
        public string expirytracking_flag { get; set; }
        public string inventory_item_id { get; set; }
        public string inventory_quantity { get; set; }
        public string lead_time { get; set; }
        public string message { get; set; }
        public string mrp_price { get; set; }
        public string old_inventory_quantity { get; set; }
        public string product_code { get; set; }
        public string product_desc { get; set; }
        public string product_gid { get; set; }
        public string product_image { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string product_type { get; set; }
        public string productgroup_code { get; set; }
        public string productgroup_name { get; set; }
        public string productgroupname { get; set; }
        public string producttype_name { get; set; }
        public string producttypename { get; set; }
        public string productuom_code { get; set; }
        public string productuom_name { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }
        public string productuomclassname { get; set; }
        public string productuomname { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string serial_flag { get; set; }
        public string shopify_productid { get; set; }
        public string status { get; set; }
        public string status_flag { get; set; }
        public string stockable { get; set; }
        public string variant_id { get; set; }
        public string vendor_name { get; set; }

    }
    public class shopifyordermovingtopayment : result
    {



        public shopifypaymentlists[] shopifypaymentlists;
    }
    public class shopifypaymentlists
    {
        public string customer_address { get; set; }
        public string Grandtotal { get; set; }
        public string customer_contact_person { get; set; }
        public string item_count { get; set; }
        public string message { get; set; }
        public string salesorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_status { get; set; }
        public string shopify_orderid { get; set; }
        public string shopifyorder_number { get; set; }


    }
    public class shopifyordermovingtoorder : result
    {



        public shopifyorderlists[] shopifyorderlists;
    }
    public class shopifyorderlists : result
    {
        public string customer_address { get; set; }
        public string Grandtotal { get; set; }
        public string customer_contact_person { get; set; }
        public string item_count { get; set; }
        public string message { get; set; }
        public string salesorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_status { get; set; }
        public string shopify_orderid { get; set; }
        public string shopifyorder_number { get; set; }


    }
    public class shopifyorderlists1 : result
    {
        public string customer_address { get; set; }
        public string Grandtotal { get; set; }
        public string customer_contact_person { get; set; }
        public string item_count { get; set; }
        public string message { get; set; }
        public string salesorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_status { get; set; }
        public string shopify_orderid { get; set; }
        public string shopifyorder_number { get; set; }


    }
    public class Status

    {

        public string message { get; set; }

        public bool status { get; set; }

    }

    public class mdlBulkcustomerList : result

    {

        public string project_id { get; set; }

        public string sendtext { get; set; }

        //public shopifycustomermovingtolead[] shopifycustomermovingtolead { get; set; }

    }

    public class shopifycustomermovingtolead

    {

        public string source_name { get; set; }

        public string customer_type { get; set; }

        public bool addtocustomer1 { get; set; }


        public shopifycustomers_lists[] shopifycustomers_lists;

    }
    //public class shopifycustomermovingtolead : result
    //{

    //    public string source_name { get; set; }
    //    public string customer_type { get; set; }
    //    public bool addtocustomer1 { get; set; }


    //    public shopifycustomers_lists[] shopifycustomers_lists;
    //}
    public class shopifycustomers_lists
    {
        public string shopify_id { get; set; }
        public string first_name { get; set; }
        public string id { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string orders_count { get; set; }
        public string last_order_id { get; set; }
        public string total_spent { get; set; }
        public string email_state { get; set; }
        public string default_company { get; set; }
        public string default_address1 { get; set; }
        public string default_address2 { get; set; }
        public string default_city { get; set; }
        public string default_country { get; set; }
        public string default_countrycode { get; set; }
        public string default_zip { get; set; }
        public string default_phone { get; set; }
        public string status_flag { get; set; }

    }

    public class shopifycustomersunassigned_list
    {
        public string shopify_id { get; set; }
        public string first_name { get; set; }
        public string id { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string orders_count { get; set; }
        public string last_order_id { get; set; }
        public string total_spent { get; set; }
        public string email_state { get; set; }
        public string default_company { get; set; }
        public string default_address1 { get; set; }
        public string default_address2 { get; set; }
        public string default_city { get; set; }
        public string default_country { get; set; }
        public string default_countrycode { get; set; }
        public string default_zip { get; set; }
        public string default_phone { get; set; }
        public string status_flag { get; set; }
        public string order_status { get; set; }

    }

    public class shopifycustomersassigned_list : result
    {
        public string shopify_id { get; set; }
        public string first_name { get; set; }
        public string id { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string orders_count { get; set; }
        public string last_order_id { get; set; }
        public string total_spent { get; set; }
        public string email_state { get; set; }
        public string default_company { get; set; }
        public string default_address1 { get; set; }
        public string default_address2 { get; set; }
        public string default_city { get; set; }
        public string default_country { get; set; }
        public string default_countrycode { get; set; }
        public string default_zip { get; set; }
        public string default_phone { get; set; }
        public string status_flag { get; set; }
        public string order_status { get; set; }


    }
    public class shopifycustomers_list : result
    {
        public string shopify_id { get; set; }
        public string first_name { get; set; }
        public string id { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string orders_count { get; set; }
        public string last_order_id { get; set; }
        public string total_spent { get; set; }
        public string email_state { get; set; }
        public string default_company { get; set; }
        public string default_address1 { get; set; }
        public string default_address2 { get; set; }
        public string default_city { get; set; }
        public string default_country { get; set; }
        public string default_countrycode { get; set; }
        public string default_zip { get; set; }
        public string default_phone { get; set; }
        public string status_flag { get; set; }
        public string order_status { get; set; }


    }
    public class shopifycustomermovetolead : result
    {

        public string source_name { get; set; }
        public string customer_type { get; set; }
        public bool addtocustomer1 { get; set; }


        public shopifycustomermovetolead_list[] shopifycustomermovetolead_list;
    }
    public class shopifycustomermovetolead_list
    {
        public long id { get; set; }
        public string email { get; set; }
        public bool accepts_marketing { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int orders_count { get; set; }
        public string state { get; set; }
        public string total_spent { get; set; }
        public long? last_order_id { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public string tags { get; set; }
        public string last_order_name { get; set; }
        public string currency { get; set; }
        public string phone { get; set; }
        public Address[] addresses { get; set; }
        public DateTime accepts_marketing_updated_at { get; set; }
        public object marketing_opt_in_level { get; set; }
        public object[] tax_exemptions { get; set; }
        public Email_Marketing_Consent email_marketing_consent { get; set; }
        public Sms_Marketing_Consent sms_marketing_consent { get; set; }
        public string admin_graphql_api_id { get; set; }
        public Default_Address default_address { get; set; }
    }

    public class shopifycustomerlist
    {
        public Customer[] customers { get; set; }
    }

    public class Customer
    {
        public long id { get; set; }
        public string email { get; set; }
        public bool accepts_marketing { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public int orders_count { get; set; }
        public string state { get; set; }
        public string total_spent { get; set; }
        public long? last_order_id { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public string tags { get; set; }
        public string last_order_name { get; set; }
        public string currency { get; set; }
        public string phone { get; set; }
        public Address[] addresses { get; set; }
        public DateTime? accepts_marketing_updated_at { get; set; }
        public object marketing_opt_in_level { get; set; }
        public object[] tax_exemptions { get; set; }
        public Email_Marketing_Consent email_marketing_consent { get; set; }
        public Sms_Marketing_Consent sms_marketing_consent { get; set; }
        public string admin_graphql_api_id { get; set; }
        public Default_Address default_address { get; set; }
    }

    public class Email_Marketing_Consent
    {
        public string state { get; set; }
        public string opt_in_level { get; set; }
        public object consent_updated_at { get; set; }
    }

    public class Sms_Marketing_Consent
    {
        public string state { get; set; }
        public string opt_in_level { get; set; }
        public object consent_updated_at { get; set; }
        public string consent_collected_from { get; set; }
    }

    public class Default_Address
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public object company { get; set; }
        public string address1 { get; set; } = string.Empty;
        public object address2 { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public object province { get; set; }
        public string country { get; set; } = string.Empty;
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public object province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool _default { get; set; }
    }

    public class Address
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public object company { get; set; }
        public string address1 { get; set; }
        public object address2 { get; set; }
        public string city { get; set; }
        public object province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public object province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool _default { get; set; }
    }


    public class shopifyorder_lists
    {
        public Order[] orders { get; set; }
    }

    public class Order
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public int app_id { get; set; }
        public string browser_ip { get; set; }
        public bool buyer_accepts_marketing { get; set; }
        public object cancel_reason { get; set; }
        public object cancelled_at { get; set; }
        public string cart_token { get; set; }
        public string checkout_id { get; set; }
        public string checkout_token { get; set; }
        public Client_Details client_details { get; set; }
        public object closed_at { get; set; }
        public string confirmation_number { get; set; }
        public bool confirmed { get; set; }
        public string contact_email { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public string current_subtotal_price { get; set; }
        public Current_Subtotal_Price_Set current_subtotal_price_set { get; set; }
        public object current_total_additional_fees_set { get; set; }
        public string current_total_discounts { get; set; }
        public Current_Total_Discounts_Set current_total_discounts_set { get; set; }
        public object current_total_duties_set { get; set; }
        public string current_total_price { get; set; }
        public Current_Total_Price_Set current_total_price_set { get; set; }
        public string current_total_tax { get; set; }
        public Current_Total_Tax_Set current_total_tax_set { get; set; }
        public string customer_locale { get; set; }
        public object device_id { get; set; }
        public Discount_Codes[] discount_codes { get; set; }
        public string email { get; set; }
        public bool estimated_taxes { get; set; }
        public string financial_status { get; set; }
        public string fulfillment_status { get; set; }
        public string landing_site { get; set; }
        public object landing_site_ref { get; set; }
        public object location_id { get; set; }
        public object merchant_of_record_app_id { get; set; }
        public string name { get; set; }
        public object note { get; set; }
        public object[] note_attributes { get; set; }
        public int number { get; set; }
        public int order_number { get; set; }
        public string order_status_url { get; set; }
        public object original_total_additional_fees_set { get; set; }
        public object original_total_duties_set { get; set; }
        public string[] payment_gateway_names { get; set; }
        public string phone { get; set; }
        public object po_number { get; set; }
        public string presentment_currency { get; set; }
        public DateTime processed_at { get; set; }
        public string reference { get; set; }
        public string referring_site { get; set; }
        public string source_identifier { get; set; }
        public string source_name { get; set; }
        public object source_url { get; set; }
        public string subtotal_price { get; set; }
        public Subtotal_Price_Set subtotal_price_set { get; set; }
        public string tags { get; set; }
        public bool tax_exempt { get; set; }
        public Tax_Lines[] tax_lines { get; set; }
        public bool taxes_included { get; set; }
        public bool test { get; set; }
        public string token { get; set; }
        public string total_discounts { get; set; }
        public Total_Discounts_Set total_discounts_set { get; set; }
        public string total_line_items_price { get; set; }
        public Total_Line_Items_Price_Set total_line_items_price_set { get; set; }
        public string total_outstanding { get; set; }
        public string total_price { get; set; }
        public Total_Price_Set total_price_set { get; set; }
        public Total_Shipping_Price_Set total_shipping_price_set { get; set; }
        public string total_tax { get; set; }
        public Total_Tax_Set total_tax_set { get; set; }
        public string total_tip_received { get; set; }
        public int total_weight { get; set; }
        public DateTime updated_at { get; set; }
        public long? user_id { get; set; }
        public Billing_Address billing_address { get; set; }
        public Customer customer { get; set; }
        public Discount_Applications[] discount_applications { get; set; }
        public Fulfillment[] fulfillments { get; set; }
        public Line_Items1[] line_items { get; set; }
        public Payment_Terms payment_terms { get; set; }
        public Refund[] refunds { get; set; }
        public Shipping_Address shipping_address { get; set; }
        public Shipping_Lines[] shipping_lines { get; set; }
    }

    public class Client_Details
    {
        public string accept_language { get; set; }
        public object browser_height { get; set; }
        public string browser_ip { get; set; }
        public object browser_width { get; set; }
        public object session_hash { get; set; }
        public string user_agent { get; set; }
    }

    public class Current_Subtotal_Price_Set
    {
        public Shop_Money shop_money { get; set; }
        public Presentment_Money presentment_money { get; set; }
    }

    public class Shop_Money
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Current_Total_Discounts_Set
    {
        public Shop_Money1 shop_money { get; set; }
        public Presentment_Money1 presentment_money { get; set; }
    }

    public class Shop_Money1
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money1
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Current_Total_Price_Set
    {
        public Shop_Money2 shop_money { get; set; }
        public Presentment_Money2 presentment_money { get; set; }
    }

    public class Shop_Money2
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money2
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Current_Total_Tax_Set
    {
        public Shop_Money3 shop_money { get; set; }
        public Presentment_Money3 presentment_money { get; set; }
    }

    public class Shop_Money3
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money3
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Subtotal_Price_Set
    {
        public Shop_Money4 shop_money { get; set; }
        public Presentment_Money4 presentment_money { get; set; }
    }

    public class Shop_Money4
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money4
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Discounts_Set
    {
        public Shop_Money5 shop_money { get; set; }
        public Presentment_Money5 presentment_money { get; set; }
    }

    public class Shop_Money5
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money5
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Line_Items_Price_Set
    {
        public Shop_Money6 shop_money { get; set; }
        public Presentment_Money6 presentment_money { get; set; }
    }

    public class Shop_Money6
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money6
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Price_Set
    {
        public Shop_Money7 shop_money { get; set; }
        public Presentment_Money7 presentment_money { get; set; }
    }

    public class Shop_Money7
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money7
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Shipping_Price_Set
    {
        public Shop_Money8 shop_money { get; set; }
        public Presentment_Money8 presentment_money { get; set; }
    }

    public class Shop_Money8
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money8
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Tax_Set
    {
        public Shop_Money9 shop_money { get; set; }
        public Presentment_Money9 presentment_money { get; set; }
    }

    public class Shop_Money9
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money9
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Billing_Address
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public string address2 { get; set; }
        public string company { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }


    public class Payment_Terms
    {
        public long id { get; set; }
        public DateTime created_at { get; set; }
        public int? due_in_days { get; set; }
        public Payment_Schedules[] payment_schedules { get; set; }
        public string payment_terms_name { get; set; }
        public string payment_terms_type { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Payment_Schedules
    {
        public long id { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public DateTime? issued_at { get; set; }
        public DateTime? due_at { get; set; }
        public DateTime? completed_at { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

    public class Shipping_Address
    {
        public string first_name { get; set; } = string.Empty;
        public string address1 { get; set; } = string.Empty;
        public string phone { get; set; }
        public string city { get; set; } = string.Empty;
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; } = string.Empty;
        public string last_name { get; set; }
        public string address2 { get; set; } = string.Empty;
        public string company { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class Discount_Codes
    {
        public string code { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
    }

    public class Tax_Lines
    {
        public string price { get; set; }
        public float rate { get; set; }
        public string title { get; set; }
        public Price_Set price_set { get; set; }
        public bool channel_liable { get; set; }
    }

    public class Price_Set
    {
        public Shop_Money10 shop_money { get; set; }
        public Presentment_Money10 presentment_money { get; set; }
    }

    public class Shop_Money10
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money10
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Discount_Applications
    {
        public string target_type { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
        public string allocation_method { get; set; }
        public string target_selection { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Fulfillment
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public DateTime created_at { get; set; }
        public long location_id { get; set; }
        public string name { get; set; }
        public long order_id { get; set; }
        public Origin_Address origin_address { get; set; }
        public Receipt receipt { get; set; }
        public string service { get; set; }
        public object shipment_status { get; set; }
        public string status { get; set; }
        public string tracking_company { get; set; }
        public string tracking_number { get; set; }
        public string[] tracking_numbers { get; set; }
        public string tracking_url { get; set; }
        public string[] tracking_urls { get; set; }
        public DateTime updated_at { get; set; }
        public Line_Items[] line_items { get; set; }
    }

    public class Origin_Address
    {
    }

    public class Receipt
    {
    }

    public class Line_Items
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public object[] attributed_staffs { get; set; }
        public int fulfillable_quantity { get; set; }
        public string fulfillment_service { get; set; }
        public string fulfillment_status { get; set; }
        public bool gift_card { get; set; }
        public int grams { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Price_Set1 price_set { get; set; }
        public bool product_exists { get; set; }
        public long? product_id { get; set; }
        public object[] properties { get; set; }
        public int quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string sku { get; set; }
        public bool taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public Total_Discount_Set total_discount_set { get; set; }
        public long? variant_id { get; set; }
        public string variant_inventory_management { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public Tax_Lines1[] tax_lines { get; set; }
        public object[] duties { get; set; }
        public Discount_Allocations[] discount_allocations { get; set; }
    }

    public class Price_Set1
    {
        public Shop_Money11 shop_money { get; set; }
        public Presentment_Money11 presentment_money { get; set; }
    }

    public class Shop_Money11
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money11
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Discount_Set
    {
        public Shop_Money12 shop_money { get; set; }
        public Presentment_Money12 presentment_money { get; set; }
    }

    public class Shop_Money12
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money12
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Tax_Lines1
    {
        public bool channel_liable { get; set; }
        public string price { get; set; }
        public Price_Set2 price_set { get; set; }
        public float rate { get; set; }
        public string title { get; set; }
    }

    public class Price_Set2
    {
        public Shop_Money13 shop_money { get; set; }
        public Presentment_Money13 presentment_money { get; set; }
    }

    public class Shop_Money13
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money13
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Discount_Allocations
    {
        public string amount { get; set; }
        public Amount_Set amount_set { get; set; }
        public int discount_application_index { get; set; }
    }

    public class Amount_Set
    {
        public Shop_Money14 shop_money { get; set; }
        public Presentment_Money14 presentment_money { get; set; }
    }

    public class Shop_Money14
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money14
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Line_Items1
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public object[] attributed_staffs { get; set; }
        public int fulfillable_quantity { get; set; }
        public string fulfillment_service { get; set; }
        public string fulfillment_status { get; set; }
        public bool gift_card { get; set; }
        public int grams { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Price_Set3 price_set { get; set; }
        public bool product_exists { get; set; }
        public long? product_id { get; set; }
        public object[] properties { get; set; }
        public int quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string sku { get; set; }
        public bool taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public Total_Discount_Set1 total_discount_set { get; set; }
        public long? variant_id { get; set; }
        public string variant_inventory_management { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public Tax_Lines2[] tax_lines { get; set; }
        public object[] duties { get; set; }
        public Discount_Allocations1[] discount_allocations { get; set; }
    }

    public class Price_Set3
    {
        public Shop_Money15 shop_money { get; set; }
        public Presentment_Money15 presentment_money { get; set; }
    }

    public class Shop_Money15
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money15
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Discount_Set1
    {
        public Shop_Money16 shop_money { get; set; }
        public Presentment_Money16 presentment_money { get; set; }
    }

    public class Shop_Money16
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money16
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Tax_Lines2
    {
        public bool channel_liable { get; set; }
        public string price { get; set; }
        public Price_Set4 price_set { get; set; }
        public float rate { get; set; }
        public string title { get; set; }
    }

    public class Price_Set4
    {
        public Shop_Money17 shop_money { get; set; }
        public Presentment_Money17 presentment_money { get; set; }
    }

    public class Shop_Money17
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money17
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Discount_Allocations1
    {
        public string amount { get; set; }
        public Amount_Set1 amount_set { get; set; }
        public int discount_application_index { get; set; }
    }

    public class Amount_Set1
    {
        public Shop_Money18 shop_money { get; set; }
        public Presentment_Money18 presentment_money { get; set; }
    }

    public class Shop_Money18
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money18
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Refund
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public DateTime created_at { get; set; }
        public string note { get; set; }
        public long order_id { get; set; }
        public DateTime processed_at { get; set; }
        public bool restock { get; set; }
        public Total_Duties_Set total_duties_set { get; set; }
        public long user_id { get; set; }
        public Order_Adjustments[] order_adjustments { get; set; }
        public Transaction[] transactions { get; set; }
        public Refund_Line_Items[] refund_line_items { get; set; }
        public object[] duties { get; set; }
    }

    public class Total_Duties_Set
    {
        public Shop_Money19 shop_money { get; set; }
        public Presentment_Money19 presentment_money { get; set; }
    }

    public class Shop_Money19
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money19
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Order_Adjustments
    {
        public long id { get; set; }
        public string amount { get; set; }
        public Amount_Set2 amount_set { get; set; }
        public string kind { get; set; }
        public long order_id { get; set; }
        public string reason { get; set; }
        public long refund_id { get; set; }
        public string tax_amount { get; set; }
        public Tax_Amount_Set tax_amount_set { get; set; }
    }

    public class Amount_Set2
    {
        public Shop_Money20 shop_money { get; set; }
        public Presentment_Money20 presentment_money { get; set; }
    }

    public class Shop_Money20
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money20
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Tax_Amount_Set
    {
        public Shop_Money21 shop_money { get; set; }
        public Presentment_Money21 presentment_money { get; set; }
    }

    public class Shop_Money21
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money21
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Transaction
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string amount { get; set; }
        public string authorization { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public object device_id { get; set; }
        public object error_code { get; set; }
        public string gateway { get; set; }
        public string kind { get; set; }
        public object location_id { get; set; }
        public string message { get; set; }
        public long order_id { get; set; }
        public long parent_id { get; set; }
        public string payment_id { get; set; }
        public Payments_Refund_Attributes payments_refund_attributes { get; set; }
        public DateTime processed_at { get; set; }
        public Receipt1 receipt { get; set; }
        public string source_name { get; set; }
        public string status { get; set; }
        public bool test { get; set; }
        public long user_id { get; set; }
        public Payment_Details payment_details { get; set; }
    }

    public class Payments_Refund_Attributes
    {
        public string status { get; set; }
        public string acquirer_reference_number { get; set; }
    }

    public class Receipt1
    {
        public string id { get; set; }
        public int amount { get; set; }
        public Balance_Transaction balance_transaction { get; set; }
        public Charge charge { get; set; }
        public string _object { get; set; }
        public object reason { get; set; }
        public string status { get; set; }
        public int created { get; set; }
        public string currency { get; set; }
        public Metadata1 metadata { get; set; }
        public Payment_Method_Details1 payment_method_details { get; set; }
        public Mit_Params1 mit_params { get; set; }
    }

    public class Balance_Transaction
    {
        public string id { get; set; }
        public string _object { get; set; }
        public object exchange_rate { get; set; }
    }

    public class Charge
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int amount { get; set; }
        public string application_fee { get; set; }
        //public string balance_transaction { get; set; }
        public bool captured { get; set; }
        public int created { get; set; }
        public string currency { get; set; }
        public object failure_code { get; set; }
        public object failure_message { get; set; }
        public Fraud_Details fraud_details { get; set; }
        public bool livemode { get; set; }
        public Metadata2 metadata { get; set; }
        public Outcome outcome { get; set; }
        public bool paid { get; set; }
        public string payment_intent { get; set; }
        public string payment_method { get; set; }
        public Payment_Method_Details payment_method_details { get; set; }
        public bool refunded { get; set; }
        public object source { get; set; }
        public string status { get; set; }
        public Mit_Params mit_params { get; set; }
    }

    public class Fraud_Details
    {
    }

    public class Metadata2
    {
        public string email { get; set; }
        public string manual_entry { get; set; }
        public string order_id { get; set; }
        public string order_transaction_id { get; set; }
        public string payments_charge_id { get; set; }
        public string shop_id { get; set; }
        public string shop_name { get; set; }
        public string transaction_fee_tax_amount { get; set; }
        public string transaction_fee_total_amount { get; set; }
    }

    public class Outcome
    {
        public string network_status { get; set; }
        public object reason { get; set; }
        public string risk_level { get; set; }
        public string seller_message { get; set; }
        public string type { get; set; }
    }

    public class Payment_Method_Details
    {
        public Card card { get; set; }
        public string type { get; set; }
    }

    public class Card
    {
        public int amount_authorized { get; set; }
        public string brand { get; set; }
        public Checks checks { get; set; }
        public string country { get; set; }
        public string description { get; set; }
        public object ds_transaction_id { get; set; }
        public int exp_month { get; set; }
        public int exp_year { get; set; }
        public Extended_Authorization extended_authorization { get; set; }
        public string fingerprint { get; set; }
        public string funding { get; set; }
        public string iin { get; set; }
        public Incremental_Authorization incremental_authorization { get; set; }
        public bool incremental_authorization_supported { get; set; }
        public object installments { get; set; }
        public string issuer { get; set; }
        public string last4 { get; set; }
        public object mandate { get; set; }
        public object moto { get; set; }
        public Multicapture multicapture { get; set; }
        public string network { get; set; }
        public Network_Token network_token { get; set; }
        public string network_transaction_id { get; set; }
        public Overcapture overcapture { get; set; }
        public string payment_account_reference { get; set; }
        public Three_D_Secure three_d_secure { get; set; }
        public object wallet { get; set; }
    }

    public class Checks
    {
        public string address_line1_check { get; set; }
        public string address_postal_code_check { get; set; }
        public string cvc_check { get; set; }
    }

    public class Extended_Authorization
    {
        public string status { get; set; }
    }

    public class Incremental_Authorization
    {
        public string status { get; set; }
    }

    public class Multicapture
    {
        public string status { get; set; }
    }

    public class Network_Token
    {
        public bool used { get; set; }
    }

    public class Overcapture
    {
        public int maximum_amount_capturable { get; set; }
        public string status { get; set; }
    }

    public class Three_D_Secure
    {
        public bool authenticated { get; set; }
        public string authentication_flow { get; set; }
        public string result { get; set; }
        public object result_reason { get; set; }
        public bool succeeded { get; set; }
        public string version { get; set; }
    }

    public class Mit_Params
    {
        public string network_transaction_id { get; set; }
    }

    public class Metadata1
    {
        public string order_transaction_id { get; set; }
        public string payments_refund_id { get; set; }
    }

    public class Payment_Method_Details1
    {
        public Card1 card { get; set; }
        public string type { get; set; }
    }

    public class Card1
    {
        public object acquirer_reference_number { get; set; }
        public string acquirer_reference_number_status { get; set; }
    }

    public class Mit_Params1
    {
    }

    public class Payment_Details
    {
        public string credit_card_bin { get; set; }
        public string avs_result_code { get; set; }
        public string cvv_result_code { get; set; }
        public string credit_card_number { get; set; }
        public string credit_card_company { get; set; }
        public object buyer_action_info { get; set; }
        public string credit_card_name { get; set; }
        public string credit_card_wallet { get; set; }
        public int? credit_card_expiration_month { get; set; }
        public int? credit_card_expiration_year { get; set; }
        public string payment_method_name { get; set; }
    }

    public class Refund_Line_Items
    {
        public long id { get; set; }
        public long line_item_id { get; set; }
        public long? location_id { get; set; }
        public int quantity { get; set; }
        public string restock_type { get; set; }
        public float subtotal { get; set; }
        public Subtotal_Set subtotal_set { get; set; }
        public float total_tax { get; set; }
        public Total_Tax_Set1 total_tax_set { get; set; }
        public Line_Item line_item { get; set; }
    }

    public class Subtotal_Set
    {
        public Shop_Money22 shop_money { get; set; }
        public Presentment_Money22 presentment_money { get; set; }
    }

    public class Shop_Money22
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money22
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Tax_Set1
    {
        public Shop_Money23 shop_money { get; set; }
        public Presentment_Money23 presentment_money { get; set; }
    }

    public class Shop_Money23
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money23
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Line_Item
    {
        public long id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public object[] attributed_staffs { get; set; }
        public int fulfillable_quantity { get; set; }
        public string fulfillment_service { get; set; }
        public string fulfillment_status { get; set; }
        public bool gift_card { get; set; }
        public int grams { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Price_Set5 price_set { get; set; }
        public bool product_exists { get; set; }
        public long? product_id { get; set; }
        public object[] properties { get; set; }
        public int quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string sku { get; set; }
        public bool taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public Total_Discount_Set2 total_discount_set { get; set; }
        public long? variant_id { get; set; }
        public string variant_inventory_management { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public Tax_Lines3[] tax_lines { get; set; }
        public object[] duties { get; set; }
        public Discount_Allocations2[] discount_allocations { get; set; }
    }

    public class Price_Set5
    {
        public Shop_Money24 shop_money { get; set; }
        public Presentment_Money24 presentment_money { get; set; }
    }

    public class Shop_Money24
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money24
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Total_Discount_Set2
    {
        public Shop_Money25 shop_money { get; set; }
        public Presentment_Money25 presentment_money { get; set; }
    }

    public class Shop_Money25
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money25
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Tax_Lines3
    {
        public bool channel_liable { get; set; }
        public string price { get; set; }
        public Price_Set6 price_set { get; set; }
        public float rate { get; set; }
        public string title { get; set; }
    }

    public class Price_Set6
    {
        public Shop_Money26 shop_money { get; set; }
        public Presentment_Money26 presentment_money { get; set; }
    }

    public class Shop_Money26
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money26
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Discount_Allocations2
    {
        public string amount { get; set; }
        public Amount_Set3 amount_set { get; set; }
        public int discount_application_index { get; set; }
    }

    public class Amount_Set3
    {
        public Shop_Money27 shop_money { get; set; }
        public Presentment_Money27 presentment_money { get; set; }
    }

    public class Shop_Money27
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money27
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Shipping_Lines
    {
        public long id { get; set; }
        public string carrier_identifier { get; set; }
        public string code { get; set; }
        public string discounted_price { get; set; }
        public Discounted_Price_Set discounted_price_set { get; set; }
        public object phone { get; set; }
        public string price { get; set; }
        public Price_Set7 price_set { get; set; }
        public object requested_fulfillment_service_id { get; set; }
        public string source { get; set; }
        public string title { get; set; }
        public Tax_Lines4[] tax_lines { get; set; }
        public object[] discount_allocations { get; set; }
    }

    public class Discounted_Price_Set
    {
        public Shop_Money28 shop_money { get; set; }
        public Presentment_Money28 presentment_money { get; set; }
    }

    public class Shop_Money28
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money28
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Price_Set7
    {
        public Shop_Money29 shop_money { get; set; }
        public Presentment_Money29 presentment_money { get; set; }
    }

    public class Shop_Money29
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money29
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Tax_Lines4
    {
        public bool channel_liable { get; set; }
        public string price { get; set; }
        public Price_Set8 price_set { get; set; }
        public float rate { get; set; }
        public string title { get; set; }
    }

    public class Price_Set8
    {
        public Shop_Money30 shop_money { get; set; }
        public Presentment_Money30 presentment_money { get; set; }
    }

    public class Shop_Money30
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class Presentment_Money30
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

}