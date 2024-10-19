using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ems.crm.Models
{
    public class MdlSmsCampaign:result
    {
        public List<smscampaign_list> smscampaign_list { get; set; }
        public List<smspostcampaign_list> smspostcampaign_list { get; set; }
        public List<smsleadcustomerdetails_list> smsleadcustomerdetails_list { get; set; }
        public List<smscampaigncount_list> smscampaigncount_list { get; set; }
        public List<smstemplatesendsummary_list> smstemplatesendsummary_list { get; set; }
        public List<smscampaignlog> smscampaignlog { get; set; }
        public List<individualsmslog> individualsmslog { get; set; }
        public string campaign_count { get; set; }
        public string totalcampaign_send { get; set; }
        public string current_month { get; set; }
        public string current_year { get; set; }
        public string crt_mth { get; set; }
        public string crt_yr { get; set; }
        public List<template_previewlist> template_previewlist { get; set; }

    }
    public class smscampaigncount_list : result
    {
        public string campaign_count { get; set; }
        public string totalcampaign_send { get; set; }
        public string current_month { get; set; }
        public string current_year { get; set; }
    }
    public class smstemplatesendsummary_list :result
    {
        public string template_id { get; set; }
        public List<mdlBulksmssend> customerdetailslist { get; set; }
    }
    public class mdlBulksmssend
    {
        public string default_phone { get; set; }
        public string names { get; set; }
        public string email { get; set; }
    }
    public class smsleadcustomerdetails_list : result
    {
        public string template_id { get; set; }
       public string leadbank_gid { get; set; }
        public string email { get; set; }
        public string created_date { get; set; }
        public string default_phone { get; set; }
        public string customer_type { get; set; }
        public string names { get; set; }
        public string address1 { get; set; }
        public string date { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }
   
    public class smspostcampaign_list : result
    {
        public string campaign_title { get; set; }
        public string campaign_message { get; set; }
        public string campaign_titleedit { get; set; }
        public string campaign_messageedit { get; set; }
        public string template_id { get; set; }
    }
    public class smscampaign_list : result
    {
        public string template_id { get; set; }
        public string created_by { get; set; }
        public string campagin_title { get; set; }
        public string created_date { get; set; }
        public string campagin_message { get; set; }
        public string send_count { get; set; }
    }
    public class smscampaignlog : result
    {
        public string smscampaigndtl_gid { get; set; }
        public string phone_number { get; set; }
        public string template_id { get; set; }
        public string status { get; set; }
        public string campagin_title { get; set; }
        public string contact_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
    public class individualsmslog : result
    {
       
        public string template_id { get; set; }
        public string campagin_message { get; set; }
        public string created_by { get; set; }
        public string phone_number { get; set; }
        public string created_date { get; set; }
    }
    public class template_previewlist
    {
        public string campagin_title { get; set; }
        public string campagin_message { get; set; }
    }
    public class smsconfiguration
    {
        public string sms_user_id { get; set; }
        public string sms_password { get; set; }

    }
}