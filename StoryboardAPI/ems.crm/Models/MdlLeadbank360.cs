﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web;

namespace ems.crm.Models
{
    public class MdlLeadbank360 : result
    {
        public List<leadwhatscontactlist> leadwhatscontactlist { get; set; }
        public List<leadwhatsmessagelist> leadwhatsmessagelist { get; set; }
        public List<leadmailsummary_list> leadmailsummary_list { get; set; }
        public List<leademailsendlist> leademailsendlist { get; set; }
        public List<leadorderdetailslist> leadorderdetailslist { get; set; }
        public List<leadquotationdetailslist> leadquotationdetailslist { get; set; }
        public List<leadinvoicedetailslist> leadinvoicedetailslist { get; set; }
        public List<leadcountdetails> leadcountdetails { get; set; }
        public List<leadquotationcount> leadquotationcount { get; set; }
        public List<leadinvoicecount> leadinvoicecount { get; set; }
        public List<leaddocumentdetails> leaddocumentdetails { get; set; }
        public List<leaduploaddocument> leaduploaddocument { get; set; }
        public List<notes> notes { get; set; }
        public List<document_download> document_download { get; set; }
        public List<leadbasicdetails_list> leadbasicdetails_list { get; set; }
        public List<Enquiry_list> Enquiry_list { get; set; }
        public List<gid_list> gid_list { get; set; }
        public List<contactedit_list> contactedit_list { get; set; }
        public List<addtocustomer> addtocustomer { get; set; }
        public List<leadstage_list> leadstage_list { get; set; }
        public List<call_logreport> call_logreport { get; set; }
        public List<myappointmentlog_list> myappointmentlog_list { get; set; }
        public List<leadsaleschart> leadsaleschart { get; set; }
        public List<pricesegement_list> pricesegement_list { get; set; }
        public class refreshtokenlist
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }

        public class responselist : result
        {
            public string id { get; set; }
            public string threadId { get; set; }
            public string[] labelIds { get; set; }
            public string gmail_mail_from { get; set; }
            public string gmail_to_mail { get; set; }
            public string base64EncodedText { get; set; }
            public string gmail_body { get; set; }
            public string gmail_sub { get; set; }
            public string leadbank_gid { get; set; }
            public string potential_value { get; set; }
            public string appointment_gid { get; set; }
            public string tomailaddress_list { get; set; }

        }
        public string gmail_address { get; set; }
    }
    public class pricesegement_list:result
    {
        public string pricesegment_gid { get; set; }
        public string pricesegment_name { get; set; }
        public string customer_gid { get; set; }
    }
    public class gmailconfiguration
    {
        public string s_no { get; set; }
        public string access_token { get; set; }
        public string gmail_address { get; set; }
        public string refresh_token { get; set; }
        public string client_secret { get; set; }
        public string client_id { get; set; }
        public string default_template { get; set; }


    }

    public class leadsaleschart
    {

        public string invoice_count { get; set; }
        public string order_count { get; set; }
        public string enquiry_count { get; set; }
        public string quotation_count { get; set; }
        public string Months { get; set; }


    }


    public class Enquiry_list
    {
        public string enquiry_refno { get; set; }
        public string enquiry_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string enquiry_date { get; set; }
        public string enquiry_status { get; set; }
        public string enquiry_type { get; set; }
        public string quotation_date { get; set; }
        public string contact_details { get; set; }
        public string user_firstname { get; set; }

    }
    public class leadbasicdetails_list
    {
        public string leadbank_name { get; set; }
        public string leadbank_gid { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string customer_gid { get; set; }
        public string potential_value { get; set; }
        public string mail_service { get; set; }

        public string leadbankcontact_name { get; set; }
        public string created_date { get; set; }
        public string address1 { get; set; }
        public string customer_type { get; set; }
        public string gmail_address { get; set; }
        public string lsmodule_ref { get; set; }
        public string lsshopify_flag { get; set; }
    }
    public class contactedit_list
    {
        public string leadbank_gid { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string potential_value { get; set; }
        public string mobile1 { get; set; }
        public string message { get; internal set; }
        public bool status { get; internal set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string state { get; set; }
        public string country_name { get; set; }
        public string country_gid { get; set; }
        public string leadbank_address1 { get; set; }
        public string leadbank_address2 { get; set; }
        public string leadbank_state { get; set; }
        public string leadbank_city { get; set; }
        public string leadbank_pin { get; set; }
        public getphonenumber43 mobile { get; set; }

    }
    public class getphonenumber43
    {
        public string e164Number { get; set; }

    }

    public class leadinvoicecount
    {
        public string invoice_count { get; set; }
    }
    public class leadquotationcount
    {
        public string quotation_count { get; set; }
    }
    public class leadcountdetails
    {
        public string whatsappcampaign_count { get; set; }
        public string mail_count { get; set; }
        public string sms_count { get; set; }
        public string totalquotation_count { get; set; }
        public string totalquotation_amount { get; set; }
        public string quotationaccepted_count { get; set; }
        public string quotationaccepted_amount { get; set; }
        public string quotationdropped_count { get; set; }
        public string quotationdropped_amount { get; set; }
        public string totalorder_count { get; set; }
        public string totalorder_amount { get; set; }
        public string delevery_count { get; set; }
        public string delevery_amount { get; set; }
        public string orderpending_count { get; set; }
        public string orderpending_amount { get; set; }
        public string totalinvoice_count { get; set; }
        public string totalinvoice_amount { get; set; }
        public string paymentreceived_count { get; set; }
        public string paymentreceived_amount { get; set; }
        public string paymentpending_count { get; set; }
        public string paymentpending_amount { get; set; }
    }
    public class leaddocumentdetails
    {
        public string document_gid { get; set; }
        public string document_title { get; set; }
        public string document_upload { get; set; }
        public string leadbank_gid { get; set; }
        public string remarks { get; set; }
        public string created_date { get; set; }
        public string document_type { get; set; }

        //public string document_upload { get; set; }
    }
    public class leaduploaddocument
    {
        public string document_gid { get; set; }
        public string document_title { get; set; }
        public string leadbank_gid { get; set; }
        public string remarks { get; set; }
        public string created_date { get; set; }
    }
    public class document_download : result
    {
        public string document_gid { get; set; }
        public string document_upload { get; set; }
        public string document_title { get; set; }
    }

    public class leadinvoicedetailslist
    {
        public string invoice_gid { get; set; }
        public string so_referencenumber { get; set; }
        public string invoice_refno { get; set; }
        public string mail_status { get; set; }
        public string customer_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_reference { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string overall_status { get; set; }
        public string payment_flag { get; set; }
        public string initialinvoice_amount { get; set; }
        public string invoice_status { get; set; }
        public string invoice_flag { get; set; }
        public string invoice_amount { get; set; }
        public string customer_name { get; set; }
        public string currency_code { get; set; }
        public string mobile { get; set; }
        public string invoice_from { get; set; }
        public string directorder_gid { get; set; }
        public string progressive_invoice { get; set; }



    }

    public class leadquotationdetailslist
    {
        public string quotation_gid { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string quotation_date { get; set; }
        public string user_firstname { get; set; }
        public string quotation_type { get; set; }
        public string currency_code { get; set; }
        public string Grandtotal { get; set; }
        public string customer_name { get; set; }
        public string customer_contact_person { get; set; }
        public string quotation_status { get; set; }
        public string enquiry_gid { get; set; }
        public string contact { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadbankcontact_gid { get; set; }

    }
    public class leadorderdetailslist
    {
        public string salesorder_gid { get; set; }
        public string so_referenceno1 { get; set; }
        public string salesorder_date { get; set; }
        public string user_firstname { get; set; }
        public string so_typecurrency_code { get; set; }
        public string customer_contact_person { get; set; }
        public string salesorder_status { get; set; }
        public string currency_code { get; set; }
        public string Grandtotal { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string invoice_flag { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadbankcontact_gid { get; set; }

    }
    public class leademailsendlist
    {
        public string mailmanagement_gid { get; set; }
        public string sub { get; set; }
        public string body { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string image_path { get; set; }
        public string from_mail { get; set; }
        public string to_mail { get; set; }
        public string transmission_id { get; set; }
        public string bcc { get; set; }
        public string cc { get; set; }
        public string reply_to { get; set; }
        public string status_delivery { get; set; }
        public string status_open { get; set; }
        public string status_click { get; set; }
        public string scheduled_time { get; set; }
        public string temp_mail_gid { get; set; }
        public string schedule_id { get; set; }

    }
    public class leadwhatscontactlist
    {
        public string whatsapp_gid { get; set; }
        public string id { get; set; }
        public string wkey { get; set; }
        public string value { get; set; }
        public string displayName { get; set; }
        public string first_letter { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }

    }
    public class leadwhatsmessagelist
    {
        public string id { get; set; }
        public string contact_id { get; set; }
        public string message_id { get; set; }
        public string displayName { get; set; }
        public string first_letter { get; set; }
        public string message_text { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string time { get; set; }
        public string direction { get; set; }
        public string created_date { get; set; }
        public string identifierValue { get; set; }
        public string document_name { get; set; }
        public string document_path { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string customertype_gid { get; set; }
        public string template_body { get; set; }
        public string footer { get; set; }
        public string media_url { get; set; }
        public string project_id { get; set; }
        public string leadbank_gid { get; set; }
        public string template_image { get; set; }

    }

    public class leadwhatsappsendmessage : result
    {

        public string identifierValue { get; set; }
        public string identifierKey { get; set; }
        public string type { get; set; }
        public string sendtext { get; set; }
        public string project_id { get; set; }
        public string contact_id { get; set; }
        public string projectId { get; set; }
        public string version { get; set; }

    }
    public class leadmailsummary_list
    {
        public string mail { get; set; }
        public string company_gid { get; set; }
        public string sub { get; set; }
        public string to { get; set; }
        public string body { get; set; }
        public string mail_from { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string mailmanagement_gid { get; set; }
        public string bcc { get; set; }
        public string cc { get; set; }
        public string body_content { get; set; }
        public string reply_to { get; set; }
        public string status_date { get; set; }
        public string transmission_id { get; set; }
        public string status_open { get; set; }
        public string schedule_time { get; set; }
        public string temp_mail_gid { get; set; }
        public string image_path { get; set; }
        public List<mailtemplate_list> mailtemplate_list { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string basecode { get; set; }
        public Boolean status { get; set; }
        public string message { get; set; }
        public string template_subject { get; set; }
        public string template_body { get; set; }
        public string template_name { get; set; }
        public string gmail_address { get; set; }



    }
    public class notes : result
    {
        public string internal_notes { get; set; }
        public string leadbank_gid { get; set; }
        public string internalnotestext_area { get; set; }
        public string leadgig { get; set; }
        public string s_no { get; set; }
        public string source { get; set; }
    }

    public class gid_list
    {
        public string lead2campaign_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string leadstage_name { get; set; }
        public string campaign_gid { get; set; }

    }

    public class addtocustomer : result
    {
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer_code { get; set; }
        public string customer_id { get; set; }
        public string company_website { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_address2 { get; set; }
        public string customer_city { get; set; }
        public string gst_number { get; set; }
        public string countryname_gid { get; set; }
        public string region { get; set; }
        public string customer_state { get; set; }
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
        public string fax_area_code { get; set; }
        public string area_code { get; set; }
        public string fax_country_code { get; set; }
        public string country_code { get; set; }
        public string address1 { get; set; }
        public string phone1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string mobile { get; set; }
        public string currencyexchange_gid { get; set; }
        public string region_name { get; set; }
        public string customerbranch_name { get; set; }
        public string customer_type { get; set; }
        public string country_gid { get; set; }
        public string country_name { get; set; }
        public string customer_pin { get; set; }
        public string phone { get; set; }
        public string main_contact { get; set; }
        public string taxsegment_name { get; set; }

    }
    public class leadstage_list : result
    {

        public string leadstage_name { get; set; }
        public string leadstage_gid { get; set; }
    }
    public class myappointmentlog_list : result
    {

        public string lead_title { get; set; }
        public string appointment_date { get; set; }
        public string business_vertical { get; set; }
        public string assign_to { get; set; }
    }

    public class call_logreport : result
    {

        public string individual_gid { get; set; }
        public string station { get; set; }
        public string user_name { get; set; }
        public string phone_number { get; set; }
        public string didnumber { get; set; }
        public string status { get; set; }
        public string direction { get; set; }
        public string uniqueid { get; set; }
        public string duration { get; set; }
        public string start_time { get; set; }
        public string answertime { get; set; }
        public string endtime { get; set; }
        public string recording_path { get; set; }
        public string call_status { get; set; }
        public string agent { get; set; }
        public string remarks { get; set; }
        public string leadbank_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
    }
}