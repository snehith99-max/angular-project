using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.kot.Models
{
    public class MdlWhatsApporderSummary : result

    {
        public List<etdupdate_List> etdupdate_List { get; set; }
        public List<readyorder_list> readyorder_list { get; set; }
        public List<delivered_List> delivered_List { get; set; }
        public List<shop_details> shop_details { get; set; }
        public List<woscontactsummary_List> woscontactsummary_List { get; set; }
        public string contact_count { get; set; }
        public List<wosmsgtemplate_list> wosmsgtemplate_list { get; set; }
        public List<wostotalcontact_list> wostotalcontact_list { get; set; }


    }
    public class mdloveralsummary : result
    {

        public List<Folders1> mainsummary { get; set; }
        public List<Folders1> subsummary { get; set; }
    }
    public class Folders1
    {

        public string order_id { get; set; }
        public string kot_gid { get; set; }
        public string kot_tot_price { get; set; }
        public string customer_phone { get; set; }
        public string kot_delivery_charges { get; set; }
        public string source { get; set; }
        public string message_id { get; set; }
        public string order_type { get; set; }
        public string created_date { get; set; }
        public string payment_status { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string eta_time { get; set; }
        public string total_quantity { get; set; }
        public string total_product { get; set; }
        public string grand_total { get; set; }
        public string orderstatus { get; set; }
        public string preparing_status { get; set; }
        public string delivery_status { get; set; }
        public string kotdtl_gid { get; set; }
        public string kot_product_price { get; set; }
        public string product_quantity { get; set; }
        public string product_amount { get; set; }
        public string currency { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public string productgroup_name { get; set; }
        public string order_instructions { get; set; }
        public string symbol { get; set; }
        public string payment_method { get; set; }
        public List<subsummary> subsummary { get; set; } = new List<subsummary>();
    }
    public class subsummary
    {
        public string order_id { get; set; }
        public string kot_gid { get; set; }
        public string kot_tot_price { get; set; }
        public string customer_phone { get; set; }
        public string kot_delivery_charges { get; set; }
        public string source { get; set; }
        public string message_id { get; set; }
        public string order_type { get; set; }
        public string created_date { get; set; }
        public string payment_status { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string total_quantity { get; set; }
        public string total_product { get; set; }
        public string grand_total { get; set; }
        public string orderstatus { get; set; }
        public string preparing_status { get; set; }
        public string delivery_status { get; set; }
        public string kotdtl_gid { get; set; }
        public string kot_product_price { get; set; }
        public string product_quantity { get; set; }
        public string product_amount { get; set; }
        public string currency { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public string productgroup_name { get; set; }

    }
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }


    }
    public class etdupdate_List : result
    {
        public string kot_gid { get; set; }
        public string order_duration { get; set; }
        public string order_eta { get; set; }
        public string customer_phone { get; set; }
    }
    public class readyorder_list
    {
        public string kot_gid { get; }
    }
    public class delivered_List : result
    {
        public string kot_gid { get; set; }
        public string order_id { get; set; }
        public string customer_phone { get; set; }
        public string created_date { get; set; }
        public string total_quantity { get; set; }
        public string total_product { get; set; }
        public string kot_tot_price { get; set; }
    }
    public class shop_details : result
    {
        public string shop_status { get; set; }
        public string cart_status { get; set; }
    }
    public class whatsappconfiguration
    {
        public string wacatalouge_id { get; set; }
        public string wachannel_id { get; set; }
        public string waaccess_token { get; set; }
        public string meta_phone_id { get; set; }
        public string waphone_number { get; set; }
        public string waworkspace_id { get; set; }
        public string bird_token { get; set; }
        public string manager_number { get; set; }

    }


    public class mdlwhatsapporderdashboard : result
    {
        public string total_ordercount { get; set; }
        public string kot_totalprice { get; set; }
        public string max_date { get; set; }
        public string min_date { get; set; }

    }

    public class mdlwhatsapporderprdtdtl : result
    {
        public string active_Products { get; set; }
        public string product_added { get; set; }
        public string in_stock { get; set; }
        public string out_of_stock { get; set; }

        public string branch_name { get; set; }

        public string branch_gid { get; set; }
        public string msgsend_owner { get; set; }
        public string owner_number { get; set; }
        public string msgsend_manger { get; set; }
        public string manager_number { get; set; }

    }


    public class mdlorderreject :result
    {
        public string branch_gid { get; set; }
        public string customer_phone { get; set; }
        public string kot_gid { get; set; }
        public string reject_reason { get; set; }
        public string contact_id { get; set; }
        public string order_id { get; set; }

    }
    public class CheckServicewindow
    {
        public object serviceWindowExpireAt { get; set; }
    }
    public class mdlwosshopenable : result
    {
        public string shopstatus { get; set; }
        public string branch_gid { get; set; }
    }
    public class mdlorderupdates: result
    {
        public string branch_gid { get; set; }
        public string customer_phone { get; set; }
        public string kot_gid { get; set; }
        public string order_update { get; set; }
        public string contact_id { get; set; }
        public string order_id { get; set; }

    }
    public class woscontactsummary_List
    {
        public string whatsapp_gid { get; set; }
        public string value { get; set; }
        public string displayName { get; set; }
        public string first_letter { get; set; }
        public string read_flag { get; set; }
        public string customer_from { get; set; }
        public string last_seen { get; set; }
        public string customer_type { get; set; }
        public string lead_status { get; set; }
        public string last_message { get; set; }

    }
    public class Mdlwhatsappchat_list : result
    {
        public string displayName { get; set; }
        public string first_letter { get; set; }
        public string identifierValue { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contact_id { get; set; }
        public string leadbank_gid { get; set; }
        public string customertype_gid { get; set; }
        public List<wosmsgchatsummary_list> wosmsgchatsummary_list { get; set; }


    }
    public class wosmsgchatsummary_list
    {

        public string message_text { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string time { get; set; }
        public string direction { get; set; }
        public string created_date { get; set; }
        public string document_name { get; set; }
        public string document_path { get; set; }
        public string template_body { get; set; }
        public string footer { get; set; }
        public string media_url { get; set; }
        public string project_id { get; set; }
        public string message_id { get; set; }
        public string template_image { get; set; }
        public string contact_id { get; set; }
        public string identifierValue { get; set; }

    }
    public class mdlwoscontactupdate : result
    {
        public string displayName_edit { get; set; }
        public string firstname_edit { get; set; }
        public string lastname_edit { get; set; }
        public string customertype_edit { get; set; }
        public string whatsapp_gid { get; set; }
        public string contact_id { get; set; }
        public string phone_edit { get; set; }

    }
    public class individualmessagesend
    {

        public string identifierValue { get; set; }
        public string identifierKey { get; set; }
        public string type { get; set; }
        public string sendtext { get; set; }
        public string project_id { get; set; }
        public string contact_id { get; set; }
        public string projectId { get; set; }
        public string version { get; set; }
        public string media_url { get; set; }
        public string template_body { get; set; }
        public string footer { get; set; }

    }
    public class Results
    {
        public string id { get; set; }
        public string channelId { get; set; }
        public wosSender sender { get; set; }
        public wosReceiver receiver { get; set; }
        public wosBody body { get; set; }
        public wosMeta meta { get; set; }
        public wosTemplate template { get; set; }
        public string reference { get; set; }
        public wosPart[] parts { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string direction { get; set; }
        public DateTime lastStatusAt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class wosSender
    {
        public wosConnector connector { get; set; }
        public wosContact contact { get; set; }
    }
    public class wosPart
    {
        public string platformReference { get; set; }
    }
    public class wosConnector
    {
        public string id { get; set; }
        public string identifierValue { get; set; }
    }

    public class wosContact
    {
        public string id { get; set; }
        public string identifierKey { get; set; }
        public string identifierValue { get; set; }
    }

    public class wosReceiver
    {
        public wosContact1[] contacts { get; set; }
        public wosConnector1 connector { get; set; }
    }

    public class wosConnector1
    {
        public string id { get; set; }
        public string identifierValue { get; set; }
    }

    public class wosContact1
    {
        public string id { get; set; }
        public string identifierKey { get; set; }
        public string identifierValue { get; set; }
        public string type { get; set; }
    }

    public class wosBody
    {
        public string type { get; set; }
        public wosImage2 image { get; set; }
        public wosText text { get; set; }
        public woslist list { get; set; }
        public wosFile file { get; set; }
    }

    public class wosImage2
    {
        public string text { get; set; }
        public wosImage3[] images { get; set; }
    }
    public class wosFile
    {
        public wosFile1[] files { get; set; }
    }

    public class wosFile1
    {
        public string contentType { get; set; }
        public string mediaUrl { get; set; }
        public wosMetadata metadata { get; set; }

    }
    public class wosMetadata
    {
        public bool isAnimated { get; set; }
    }
    public class wosImage3
    {
        public string mediaUrl { get; set; }
        public string altText { get; set; }
    }

    public class woslist
    {
        public string text { get; set; }
        public string altText { get; set; }
    }

    public class wosText
    {
        public string text { get; set; }
    }

    public class wosMeta
    {
        public wosExtrainformation extraInformation { get; set; }
    }

    public class wosExtrainformation
    {
        public string conversation_id { get; set; }
        public string cost_request_id { get; set; }
        public string useCase { get; set; }
        public string use_wa_platform_account_id_approach { get; set; }
        public string timestamp { get; set; }
    }

    public class wosTemplate
    {
        public string projectId { get; set; }
        public string version { get; set; }
        public string locale { get; set; }
        public object variables { get; set; }
    }
   
    public class Servicewindows
    {
        public object serviceWindowExpireAt { get; set; }
    }
    public class Mdlwosfile_list : result
    {
        public List<wosfiles_list> wosfiles_list { get; set; }
        public List<wosimages_list> wosimages_list { get; set; }
    }

    public class wosfiles_list
    {
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string content_type { get; set; }
        public string file_gid { get; set; }
        public string created_date { get; set; }


    }

    public class wosimages_list
    {
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string file_gid { get; set; }
        public string created_date { get; set; }


    }
    public class contact_infolist : result
    {
        public string company_name { get; set; }
        public string contact_person_name { get; set; }
        public string mobilenumber { get; set; }
        public string customertype { get; set; }
    }
    public class wosRootobject
    {
        public string id { get; set; }
        public string computedDisplayName { get; set; }
        public wosFeaturedidentifier[] featuredIdentifiers { get; set; }
        public int identifierCount { get; set; }
        public wosAttributes1 attributes { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class wosAttributes1
    {
        public string displayName { get; set; }
    }

    public class wosFeaturedidentifier
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    public class wosmsgtemplate_list
    {
        public string id { get; set; }
        public string p_name { get; set; }
        public string version { get; set; }
    }
    public class wostotalcontact_list
    {
        public string whatsapp_gid { get; set; }
        public string displayName { get; set; }
        public string value { get; set; }
    }
    public class mdlwosbulkremplates : result
    {
        public List<wosbulktemplate_send> wosbulktemplatesend;

    }
    public class wosbulktemplate_send : result
    {
        public string value { get; set; }
        public string whatsapp_gid { get; set; }
    }
    public class mdlwoscontactcreate : result
    {
        public string displayName { get; set; }
        public string CompanyName { get; set; }
        public string ContactpersonName { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
        public string leadbank_gid { get; set; }
        public wosphonenumber phone { get; set; }



    }
    public class wosphonenumber
    {
        public string e164Number { get; set; }

    }
}