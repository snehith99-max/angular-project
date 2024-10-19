﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ems.crm.Models
{
    public class MdlSocialMedia : result
    {
        public List<shopifyproductcount> shopifyproductcount { get; set; }
        public List<shopifycustomercount> shopifycustomercount { get; set; }
        public List<shopifyordercount> shopifyordercount { get; set; }
        public List<shopifystorename> shopifystorename { get; set; }
        public List<contactcount_list1> contactcount_list { get; set; }
        public List<messagecount_list> messagecount_list { get; set; }
        public List<messageoutgoing_list> messageoutgoing_list { get; set; }
        public List<messageincoming_list> messageincoming_list { get; set; }
        public List<emailstatus_list> emailstatus_list { get; set; }
        public List<sentmailcount_list> sentmailcount_list { get; set; }
        public List<shopifyproducts_list> shopifyproducts_list { get; set; }
    }
    public class shopifyproducts_list : result
    {
        public string shopifyproduct_count { get; set; }
    }
    public class emailstatus_list : result
    {
        public string deliverytotal_count { get; set; }
        public string opentotal_count { get; set; }
        public string clicktotal_count { get; set; }
    }
    public class sentmailcount_list : result
    {
        public string mail_sent { get; set; }
    }
    public class messageoutgoing_list : result
    {
        public string sent_count { get; set; }
    }
    public class messageincoming_list : result
    {
        public string incoming_count { get; set; }
    }
    public class contactcount_list1 : result
    {
        public string  contact_count1 { get; set; }
    }
    public class messagecount_list : result
    {
        public string message_count { get; set; }
    }
    public class shopifyproductcount : result
    {
        public int count { get; set; }
    }
    public class shopifycustomercount : result
    {
        public int count { get; set; }
    }
    public class shopifyordercount : result
    {
        public int count { get; set; }
    }

    public class shopifystorename
    {
        public Shop shop { get; set; }
    }

    public class Shop
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string domain { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string address1 { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public object source { get; set; }
        public string phone { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string primary_locale { get; set; }
        public string address2 { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string currency { get; set; }
        public string customer_email { get; set; }
        public string timezone { get; set; }
        public string iana_timezone { get; set; }
        public string shop_owner { get; set; }
        public string money_format { get; set; }
        public string money_with_currency_format { get; set; }
        public string weight_unit { get; set; }
        public string province_code { get; set; }
        public bool taxes_included { get; set; }
        public bool auto_configure_tax_inclusivity { get; set; }
        public bool tax_shipping { get; set; }
        public bool county_taxes { get; set; }
        public string plan_display_name { get; set; }
        public string plan_name { get; set; }
        public bool has_discounts { get; set; }
        public bool has_gift_cards { get; set; }
        public string myshopify_domain { get; set; }
        public object google_apps_domain { get; set; }
        public object google_apps_login_enabled { get; set; }
        public string money_in_emails_format { get; set; }
        public string money_with_currency_in_emails_format { get; set; }
        public bool eligible_for_payments { get; set; }
        public bool requires_extra_payments_agreement { get; set; }
        public bool password_enabled { get; set; }
        public bool has_storefront { get; set; }
        public bool finances { get; set; }
        public long primary_location_id { get; set; }
        public bool checkout_api_supported { get; set; }
        public bool multi_location_enabled { get; set; }
        public bool setup_required { get; set; }
        public bool pre_launch_enabled { get; set; }
        public string[] enabled_presentment_currencies { get; set; }
        public bool transactional_sms_disabled { get; set; }
        public bool marketing_sms_consent_enabled_at_checkout { get; set; }
    }


}