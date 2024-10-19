﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlMailCampaign : results
    {
        public List<Mail_list> Mail_list { get; set; }
        public List<sendmail_list> sendmail_list { get; set; }
        public List<mailsummary_list> mailsummary_list { get; set; }
        public List<mailevent_list> mailevent_list { get; set; }
        public List<mailtemplate_list> mailtemplate_list { get; set; }
        public List<mailcount_list> mailcount_list { get; set; }
        public List<mailstatus_list> mailstatus_list { get; set; }
        public List<mailtemplatesendsummary_list> mailtemplatesendsummary_list { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string email_username { get; set; }

        public List<customertype_list3> customertype_list3 { get; set; }
    }
    public class MailAttachmentbase64
    {
        public string name { get;set; }
        public string type { get;set; }
        public string data { get;set; }
    }
    public class DbAttachmentPath
    {
        public string path { get; set; }
    }
    public class results
    {
        public string message { get; set; } 
        public bool status { get; set; }
    }

    public class mailstatus_list
    {
        public string status_delivery { get; set; }
        public string status_open { get; set; }
        public string status_click { get; set; }
    }
    public class mailtemplate_list
    {
        public string temp_mail_gid { get; set; }
        public string sub { get; set; }
        public string to { get; set; }
        public string body { get; set; }
        public string mail_from { get; set; }
        public string reply_to { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string bcc { get; set; }
        public string cc { get; set; }
        public string template_name { get; set; }
        public string email { get; set; }
        public string shopify_id { get; set; }
        public string name { get; set; }
        public string read_mail { get; set; }
        public string send_mail { get; set; }
        public string click_mail { get; set; }
        public string sent_mail { get; set; }
        public string template_flag { get; set; }


    }

    public class mailcount_list
    {
        public string deliverytotal_count { get; set; }
        public string opentotal_count { get; set; }
        public string clicktotal_count { get; set; }
        public string mailsendtotal_count { get; set; }
        public string template_count { get; set; }
    }
    public class Mail_list
    {
        public Result4[] results { get; set; }
        public int total_count { get; set; }
        public Links links { get; set; }
    }

    public class Links
    {
    }




    public class Result4
    {
        public string mailbox_provider { get; set; }
        public string template_version { get; set; }
        public string friendly_from { get; set; }
        public string subject { get; set; }
        public string ip_pool { get; set; }
        public string sending_domain { get; set; }
        public object[] rcpt_tags { get; set; }
        public string type { get; set; }
        public string num_retries { get; set; }
        public string mailbox_provider_region { get; set; }
        public string raw_rcpt_to { get; set; }
        public string msg_from { get; set; }
        public string recv_method { get; set; }
        public string rcpt_to { get; set; }
        public string transmission_id { get; set; }
        public DateTime timestamp { get; set; }
        public bool click_tracking { get; set; }
        public string outbound_tls { get; set; }
        public Rcpt_Meta rcpt_meta { get; set; }
        public string message_id { get; set; }
        public string ip_address { get; set; }
        public bool initial_pixel { get; set; }
        public string queue_time { get; set; }
        public string recipient_domain { get; set; }
        public string event_id { get; set; }
        public string routing_domain { get; set; }
        public string sending_ip { get; set; }
        public string template_id { get; set; }
        public string delv_method { get; set; }
        public int customer_id { get; set; }
        public bool open_tracking { get; set; }
        public DateTime injection_time { get; set; }
        public string msg_size { get; set; }
        public User_Agent_Parsed user_agent_parsed { get; set; }
        public Geo_Ip geo_ip { get; set; }
        public string user_agent { get; set; }
        public string target_link_url { get; set; }
        public string reason { get; set; }
        public string bounce_class { get; set; }
        public string raw_reason { get; set; }
        public string error_code { get; set; }
        public string transactional { get; set; }
        public string campaign_id { get; set; }
    }

    public class Rcpt_Meta
    {
    }

    public class User_Agent_Parsed
    {
        public string os_family { get; set; }
        public bool is_mobile { get; set; }
        public string device_brand { get; set; }
        public string os_version { get; set; }
        public string device_family { get; set; }
        public bool is_prefetched { get; set; }
        public string agent_family { get; set; }
        public bool is_proxy { get; set; }
    }

    public class Geo_Ip
    {
        public int zip { get; set; }
        public string continent { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public float latitude { get; set; }
        public string region { get; set; }
        public string postal_code { get; set; }
        public float longitude { get; set; }
    }
    public class mailsummary_list
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
        public string template_name { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string basecode { get; set; }
        public Boolean status { get; set; }
        public string message { get; set; }
        public string shopify_id { get; set; }
        public string direction { get; set; }
        public string created_time { get; set; }
        public string temp_body { get; set; }
        public string temp_sub { get; set; }
        public string leadbank_gid { get; set; }
        public string document_path { get; set; }
        public string document_name { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string to_mail { get; set; }
        public string email_username { get; set; }



    }
    public class mailtemplatesendsummary_list : result
    {
        public string temp_mail_gid { get; set; }
        public string email { get; set; }
        public string created_date { get; set; }
        public string default_phone { get; set; }
        public string customer_type { get; set; }
        public string names { get; set; }
        public string address1 { get; set; }
        public string date { get; set; }
        public string to_mail { get; set; }
        public string status_delivery { get; set; }
        public string status_open { get; set; }
        public string status_click { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string sub { get; set; }
        public string to { get; set; }
        public string body { get; set; }
        public string direction { get; set; }
        public string source_gid { get; set; }
        public string source_name { get; set; }
        public string leadbank_gid { get; set; }
        public string lead_status { get; set; }
        public string template_name { get; set; }
        public string region { get; set; }
        public string mailmanagement_gid { get; set; }
        public string sent_flag { get; set; }
        public string lead_assignedto { get; set; }
        public List<mailsendchecklist> mailsendchecklist { get; set; }

       
    }
    public class mailsendchecklist : result
    {
        public string shopify_id { get; set; }
        public string email { get; set; }
        public string mailmanagement_gid { get; set; }
        public string leadbank_gid { get; set; }



    }

    public class sendmail_list
    {
        public Results results { get; set; }
        public string AutoID_Key { get; set; }
    }

    public class Results
    {
        public int total_rejected_recipients { get; set; }
        public int total_accepted_recipients { get; set; }
        public string id { get; set; }
    }


    public class mailevent_list
    {
        public mailevent_listResult[] results { get; set; }
        public string total_count { get; set; }
        public mailevent_listLinks links { get; set; }
        public List<mailsummary_list> mailsummary_list { get; set; }
    }

    public class mailevent_listLinks
    {
    }

    public class mailevent_listResult
    {
      
        public string mailbox_provider { get; set; }
        public string template_version { get; set; }
        public string friendly_from { get; set; }
        public string subject { get; set; }
        public string ip_pool { get; set; }
        public string sending_domain { get; set; }
        public object[] rcpt_tags { get; set; }
        public string type { get; set; }
        public string num_retries { get; set; }
        public string mailbox_provider_region { get; set; }
        public string raw_rcpt_to { get; set; }
        public string msg_from { get; set; }
        public string recv_method { get; set; }
        public string rcpt_to { get; set; }
        public string transmission_id { get; set; }
        public DateTime timestamp { get; set; }
        public bool click_tracking { get; set; }
        public string outbound_tls { get; set; }
        public Rcpt_Meta rcpt_meta { get; set; }
        public string message_id { get; set; }
        public string ip_address { get; set; }
        public bool initial_pixel { get; set; }
        public string queue_time { get; set; }
        public string recipient_domain { get; set; }
        public string event_id { get; set; }
        public string routing_domain { get; set; }
        public string sending_ip { get; set; }
        public string template_id { get; set; }
        public string delv_method { get; set; }
        public int customer_id { get; set; }
        public bool open_tracking { get; set; }
        public DateTime injection_time { get; set; }
        public string msg_size { get; set; }
    }

    public class mailevent_listRcpt_Meta
    {
    }

   public class mailconfiguration
    {
        public string base_url { get; set; }
        public string access_token { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string email_username { get; set; }

    }

    public class mdlAddaslead
    {
        public string displayName { get; set; }
        public string customer_type { get; set; }
        public string email_address { get; set; }

    }

    public class customertype_list3 : result
    {
        public string customertype_gid3 { get; set; }
        public string customer_type3 { get; set; }

    }

}