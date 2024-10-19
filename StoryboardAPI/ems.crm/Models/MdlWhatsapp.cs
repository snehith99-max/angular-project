﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlWhatsapp : result
    {
        public List<whatsappmessagescount> whatsappmessagescount { get; set; }
        public List<log> log { get; set; }
        public List<whatsapplist> whatsapplist { get; set; }
        public List<contactlist> contactlist { get; set; }

        public List<whatscontactlist> whatscontactlist { get; set; }
        public List<whatsleadlist> whatsleadlist { get; set; }

        public List<whatscontact_list> whatscontact_list { get; set; }
        public List<whatsmessagelist> whatsmessagelist { get; set; }
        public List<whatsmessagelist2> whatsmessagelist2 { get; set; }
        public List<whatsapplist3> whatsapplist3 { get; set; }

        public List<whatsappMessagetemplatelist> whatsappMessagetemplatelist { get; set; }

        public List<contactcount_list1> contactcount_list { get; set; }
        public List<whatsappCampaign> whatsappCampaign { get; set; }
        public List<Gettemplatepreviewview_list> Gettemplateview_list { get; set; }
        public List<customertype_list2> customertype_list2 { get; set; }
        public List<leadsummary> leadsummary { get; set; }
        public List<customerreport> customerreport { get; set; }
        public List<breadcrummaillist> breadcrummaillist { get; set; }

        // -----------------------------------------start excel log -----------------------------------------------------//
        public List<whatslog_list> whatslog_list { get; set; }
        public List<whatsdtllog_list> whatsdtllog_list { get; set; }
        public List<invalidlog_list> invalidlog_list { get; set; }
        public List<invalidDTLlog_list> invalidDTLlog_list { get; set; }

        // -----------------------------------------end excel log -----------------------------------------------------//
        public List<campaignsentchart_lists> campaignsentchart_lists { get; set; }
        public List<campaign_lists> campaignlists { get; set; }
        public class campaign_lists : result
        {

            public string p_name { get; set; }
            public string project_count { get; set; }


        }

    }
    public class Gettemplatepreviewview_list
    {
        public string p_name { get; set; }
        public string template_body { get; set; }
        public string media_url { get; set; }
        public string footer { get; set; }
        public string file_type { get; set; }


    }

    public class campaignsentchart_lists
    {
        public string months { get; set; }
        public string mailsent_count { get; set; }
        public string whatsappsent_count { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string days { get; set; }

    }
    public class breadcrummaillist
    {
        public string l1_menu { get; set; }
        public string l2_menu { get; set; }
        public string l3_menu { get; set; }
        public string l1_sref { get; set; }
        public string l2_sref { get; set; }
        public string l3_sref { get; set; }

    }
    public class leadsummary
    {
        public string campaign_title { get; set; }
        public string leadbank_name { get; set; }
        public string region { get; set; }
        public string source { get; set; }
        public string customer_type { get; set; }
        public string identifiervalue { get; set; }

    }
    public class contactlist
    {
        public string displayName { get; set; }
        public Identifiers[] results { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }

    }
    //public class identifiers
    //{
    //    public string key { get; set; }
    //    public string value { get; set; }
    //}

    public class whatsapplist
    {
        public string nextPageToken { get; set; }
        public Result3[] results { get; set; }
    }

    public class whatscontactlist
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
        public string read_flag { get; set; }
        public string last_seen { get; set; }
        public string customer_type { get; set; }
        public string source_name { get; set; }
        public string lead_status { get; set; }
        public string sendcampaign_flag { get; set; }
        public string customer_from { get; set; }
        public string region_name { get; set; }
        public string leadbank_gid { get; set; }





    }
    public class whatsleadlist
    {
        public string leadbank_name { get; set; }
        public string id { get; set; }
        public string wkey { get; set; }
        public string value { get; set; }
        public string displayName { get; set; }
        public string first_letter { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string leadbank_gid { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string read_flag { get; set; }
        public string mobile { get; set; }
        public string customer_type { get; set; }
        public string source_name { get; set; }
        public string lead_status { get; set; }
        public string sendcampaign_flag { get; set; }
        public string customer_from { get; set; }
        public string region_name { get; set; }




    }
    public class whatscontact_list : result
    {
        public string id { get; set; }
        public string value { get; set; }
        public string displayName { get; set; }
        public string customer_type { get; set; }
        public string lead_status { get; set; }
        public string source_name { get; set; }


    }
    public class whatsappMessagetemplatelist
    {
        public string template_gid { get; set; }
        public string id { get; set; }
        public string p_type { get; set; }
        public string p_name { get; set; }
        public string description { get; set; }
        public string created_date { get; set; }
        public string version { get; set; }
        public string template_id { get; set; }
        public string template_body { get; set; }
        public string footer { get; set; }
        public string media_url { get; set; }



    }

    public class whatsmessagelist
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

    public class Result3
    {
        public string id { get; set; }
        public string computedDisplayName { get; set; }
        public Featuredidentifier[] featuredIdentifiers { get; set; }
        public int identifierCount { get; set; }
        public Attributes attributes { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Attributes
    {
        public string avatarUrl { get; set; }
        public string country { get; set; }
        public string displayName { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
        public object[] locales { get; set; }
    }

    public class Featuredidentifier
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class whatsappmessagescount
    {
        public string Total_messages { get; set; }
        public string Received_messages { get; set; }
        public string Sent_messages { get; set; }

    }

    public class createContact
    {
        public string displayName { get; set; }
        public Identifiers identifiers { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
    }

    public class mdlCreateContactInput : result
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
        public string customer_type { get; set; }
        public getphonenumber1 phone { get; set; }
        public getleadbank_gid param { get; set; }


    }

    public class getleadbank_gid
    {
        public string leadbank_gid { get; set; }

    }

    public class getphonenumber1
    {
        public string e164Number { get; set; }

    }

    public class mdlUpdateContactInput : result
    {
        public string displayName_edit { get; set; }
        public string firstname_edit { get; set; }
        public string lastname_edit { get; set; }
        public string customertype_edit { get; set; }
        public string whatsapp_gid { get; set; }
        public string contact_id { get; set; }
        public getphonenumber12 phone_edit { get; set; }

    }
    public class getphonenumber12
    {
        public string e164Number { get; set; }

    }
    public class mdlCreateTemplateInput
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
        public string project_name { get; set; }
        public string project_description { get; set; }
        public string category { get; set; }
        public string category_change { get; set; }
        public string template_name { get; set; }
    }



    public class Identifiers
    {
        public Identifiers()
        {
            IdentifierData[] identifierslist = new IdentifierData[] { };
        }
        public IdentifierData[] identifierslist { get; set; }
    }

    public class IdentifierData
    {
        public string key { get; set; }
        public string value { get; set; }
    }


    public class Rootobject
    {
        public string id { get; set; }
        public string computedDisplayName { get; set; }
        public Featuredidentifier1[] featuredIdentifiers { get; set; }
        public int identifierCount { get; set; }
        public Attributes1 attributes { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Attributes1
    {
        public string displayName { get; set; }
    }

    public class Featuredidentifier1
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class whatsmessagelist2
    {
        public string nextPageToken { get; set; }


        public Result[] results { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string channelId { get; set; }
        public Sender sender { get; set; }
        public Receiver receiver { get; set; }
        public Body body { get; set; }
        public Meta meta { get; set; }
        public Template template { get; set; }
        public string reference { get; set; }
        public Part[] parts { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string direction { get; set; }
        public DateTime lastStatusAt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Sender
    {
        public Connector connector { get; set; }
        public Contact contact { get; set; }
    }

    public class Connector
    {
        public string id { get; set; }
        public string identifierValue { get; set; }
    }

    public class Contact
    {
        public string id { get; set; }
        public string identifierKey { get; set; }
        public string identifierValue { get; set; }
    }

    public class Receiver
    {
        public Contact1[] contacts { get; set; }
        public Connector1 connector { get; set; }
    }

    public class Connector1
    {
        public string id { get; set; }
        public string identifierValue { get; set; }
    }

    public class Contact1
    {
        public string id { get; set; }
        public string identifierKey { get; set; }
        public string identifierValue { get; set; }
        public string type { get; set; }
    }

    public class Body
    {
        public string type { get; set; }
        public Image2 image { get; set; }
        public Text text { get; set; }
        public list list { get; set; }
        public File file { get; set; }
    }

    public class Image2
    {
        public string text { get; set; }
        public Image3[] images { get; set; }
    }

    public class Image3
    {
        public string mediaUrl { get; set; }
        public string altText { get; set; }
    }

    public class list
    {
        public string text { get; set; }
        public string altText { get; set; }
    }

    public class Text
    {
        public string text { get; set; }
    }

    public class Meta
    {
        public Extrainformation extraInformation { get; set; }
    }

    public class Extrainformation
    {
        public string conversation_id { get; set; }
        public string cost_request_id { get; set; }
        public string useCase { get; set; }
        public string use_wa_platform_account_id_approach { get; set; }
        public string timestamp { get; set; }
    }

    public class Template
    {
        public string projectId { get; set; }
        public string version { get; set; }
        public string locale { get; set; }
        public object variables { get; set; }
    }

    public class Part
    {
        public string platformReference { get; set; }
    }


    public class createProject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int draftCount { get; set; }
        public int pendingCount { get; set; }
        public int activeCount { get; set; }
        public int inactiveCount { get; set; }
        public object activeResourceId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class mdlCreateTemplateInput1
    {
        public string body { get; set; }
        public string value { get; set; }
        public string footer { get; set; }
        public string value1 { get; set; }
        public string description { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
        public string projectId { get; set; }
        public string p_name { get; set; }

        public string project_id { get; set; }

        public string category_change { get; set; }
        public whatsapptemplate whatsapptemplate { get; set; }


    }
    public class whatsapptemplate
    {
        public string footer { get; set; }
        public string p_name { get; set; }
        public string project_id { get; set; }


    }
    public class createtemplate
    {
        public string id { get; set; }
        public string projectId { get; set; }
        public string defaultLocale { get; set; }
        public string status { get; set; }
        public object[] assets { get; set; }
        public Style[] styles { get; set; }
        public Deployment[] deployments { get; set; }
        public object[] variables { get; set; }
        public object[] genericContent { get; set; }
        public Platformcontent[] platformContent { get; set; }
        public string[] supportedPlatforms { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isCloneable { get; set; }
        public string editorId { get; set; }
    }

    public class Style
    {
        public string key { get; set; }
        public bool isDefault { get; set; }
        public string valueString { get; set; }
        public bool valueBoolean { get; set; }
    }

    public class Deployment
    {
        public string key { get; set; }
        public object locale { get; set; }
        public string platform { get; set; }
        public object channelIds { get; set; }
        public string value { get; set; }
    }

    public class Platformcontent
    {
        public string locale { get; set; }
        public string type { get; set; }
        public string platform { get; set; }
        public object channelIds { get; set; }
        public string[] channelGroupIds { get; set; }
        public Block[] blocks { get; set; }
    }

    public class Block
    {
        public string id { get; set; }
        public string type { get; set; }
        public string role { get; set; }
        public Texts text { get; set; }
    }

    public class Texts
    {
        public string text { get; set; }
    }
    public class File
    {
        public File1[] files { get; set; }
    }

    public class File1
    {
        public string contentType { get; set; }
        public string mediaUrl { get; set; }
        public Metadata metadata { get; set; }

    }
    public class Metadata
    {
        public bool isAnimated { get; set; }
    }

    public class sendmessage
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

    public class template_creation
    {

        public string body { get; set; }
        public string template_name { get; set; }
        public string description { get; set; }
        public string p_name { get; set; }
        public string footer { get; set; }
     

    }

    public class sendmessages
    {
        public Receiver1 receiver { get; set; }
        public Body1 text { get; set; }
        public Body1 type { get; set; }

        public Contact11 identifierValue { get; set; }
        public Contact11 identifierKey { get; set; }
    }

    public class Receiver1
    {
        public Contact[] contacts { get; set; }
    }

    public class Contact11
    {
        public string identifierValue { get; set; }
        public string identifierKey { get; set; }
    }

    public class Body1
    {
        public string type { get; set; }
        public Text text { get; set; }
    }

    public class Text1
    {
        public string text { get; set; }
    }

    public class Servicewindow
    {
        public object serviceWindowExpireAt { get; set; }
    }



    public class whatsapplist3
    {
        public string nextPageToken { get; set; }
        public Result2[] results { get; set; }
    }

    public class Result2
    {
        public string id { get; set; }
        public string computedDisplayName { get; set; }
        public Featuredidentifier[] featuredIdentifiers { get; set; }
        public int identifierCount { get; set; }
        public Attributes11 attributes { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Attributes11
    {
        public string avatarUrl { get; set; }
        public string country { get; set; }
        public string displayName { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
        public object[] locales { get; set; }
    }

    public class Featuredidentifier11
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    public class Campaignstatus
    {
        public string last_updated { get; set; }
        public string id { get; set; }

        public string updated_date { get; set; }
        public string version_id { get; set; }
        public string project_id { get; set; }
        public string publish_status { get; set; }
        public string template_gid { get; set; }
        public string p_type { get; set; }
        public string p_name { get; set; }
        public string description { get; set; }
        public string created_date { get; set; }
        public string send_campaign { get; set; }

        public bool status { get; internal set; }

        public string message { get; internal set; }
    }

    public class whatsappCampaign
    {

        public string last_updated { get; set; }
        public string id { get; set; }

        public string updated_date { get; set; }
        public string version_id { get; set; }
        public string project_id { get; set; }
        public string publish_status { get; set; }
        public string template_gid { get; set; }
        public string p_type { get; set; }
        public string p_name { get; set; }
        public string description { get; set; }
        public string created_date { get; set; }
        public string send_campaign { get; set; }
        public string campaign_flag { get; set; }
        public string project_count { get; set; }
        public string lsmtd { get; set; }
        public string lsytd { get; set; }
        public string delivered_messages { get; internal set; }

        public bool status { get; internal set; }

        public string message { get; internal set; }
    }
    public class log
    {
        public string wvalue { get; set; }
        public string displayName { get; set; }
        public string identifiervalue { get; set; }
        public string status { get; set; }
        public string direction { get; set; }
        public string created_date { get; set; }
        public string source_name { get; set; }
        public string lead_status { get; set; }
        public string customer_type { get; set; }
        public string p_name { get; set; }
        public string reason { get; set; }


    }
    public class customerreport
    {
        public string leadbank_name { get; set; }
        public string project_count { get; set; }
        public string region_name { get; set; }
        public string source_name { get; set; }
        public string wvalue { get; set; }
        public string contact_id { get; set; }

    }

    public class Templatelist
    {
        public Whatsapptemplate[] results { get; set; }
    }

    public class Whatsapptemplate
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string[] supportedPlatforms { get; set; }
        public string[] locales { get; set; }
        public int draftCount { get; set; }
        public int pendingCount { get; set; }
        public int activeCount { get; set; }
        public int inactiveCount { get; set; }
        public string activeResourceId { get; set; }
        public string[] approvedTemplateChannelsId { get; set; }
        public string[] approvedTemplateChannelGroupIds { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class mdlBulkMessageList
    {
        public string project_id { get; set; }
        public string sendtext { get; set; }
        public List<mdlBulkMessageContacts> contacts_list { get; set; }
    }

    public class mdlBulkMessageContacts
    {
        public string whatsapp_gid { get; set; }
        public string value { get; set; }
        public string display_name { get; set; }
    }
    public class mdlimportlead
    {
        public List<mdlimportleads> contacts_list { get; set; }

    }
    public class mdlimportleads
    {
        public string leadbank_gid { get; set; }
        public string value { get; set; }
        public string display_name { get; set; }
        public string leadbank_name { get; set; }
        public string mobile { get; set; }
        public string source_name { get; set; }
        public string customer_type { get; set; }
        public string region_name { get; set; }

    }
    public class mdlpublishtemplate
    {
        public string project_id { get; set; }
        public string template_id { get; set; }
    }

    public class PubliahTemplate
    {
        public string id { get; set; }
        public string projectId { get; set; }
        public string defaultLocale { get; set; }
        public string status { get; set; }
        public object[] assets { get; set; }
        public Style1[] styles { get; set; }
        public Deployment1[] deployments { get; set; }
        public object[] variables { get; set; }
        public object[] genericContent { get; set; }
        public Platformcontent1[] platformContent { get; set; }
        public string[] supportedPlatforms { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isCloneable { get; set; }
        public string editorId { get; set; }
        public string publisherId { get; set; }
    }

    public class Style1
    {
        public string key { get; set; }
        public bool isDefault { get; set; }
        public string valueString { get; set; }
        public bool valueBoolean { get; set; }
    }

    public class Deployment1
    {
        public string key { get; set; }
        public object locale { get; set; }
        public string platform { get; set; }
        public object channelIds { get; set; }
        public string value { get; set; }
    }

    public class Platformcontent1
    {
        public string locale { get; set; }
        public string type { get; set; }
        public string platform { get; set; }
        public object channelIds { get; set; }
        public string[] channelGroupIds { get; set; }
        public Block1[] blocks { get; set; }
        public Approval1[] approvals { get; set; }
    }

    public class Block1
    {
        public string id { get; set; }
        public string type { get; set; }
        public string role { get; set; }
        public Text11 text { get; set; }
    }

    public class Text11
    {
        public string text { get; set; }
    }

    public class Approval1
    {
        public string approvalReference { get; set; }
        public object channelId { get; set; }
        public string status { get; set; }
        public object reason { get; set; }
        public object platformReference { get; set; }
        public string channelGroupId { get; set; }
        public string platformAccountIdentifier { get; set; }
        public string platform { get; set; }
    }

    public class notifications
    {
        public int notification_count { get; set; }
        public List<notification_list> notification_Lists { get; set; }
    }

    public class notification_list
    {
        public string displayName { get; set; }
        public string contact_id { get; set; }
        public string count { get; set; }
        public string leadbank_gid { get; set; }
        public string ca_type { get; set; }
        public string assigned_to { get; set; }

    }

    public class MdlWaFiles : result
    {
        public List<wa_files_list> wa_files_list { get; set; }
        public List<wa_images_list> wa_images_list { get; set; }
    }

    public class wa_files_list
    {
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string content_type { get; set; }
        public string file_gid { get; set; }
        public string created_date { get; set; }


    }

    public class wa_images_list
    {
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string file_gid { get; set; }
        public string created_date { get; set; }


    }
    public class whatsappconfiguration
    {
        public string workspace_id { get; set; }
        public string channel_id { get; set; }
        public string access_token { get; set; }
        public string channelgroup_id { get; set; }

    }

    public class TemplateCreation
    {
        public string id { get; set; }
        public string projectId { get; set; }
        public string defaultLocale { get; set; }
        public string status { get; set; }
        public object[] assets { get; set; }
        public Style11[] styles { get; set; }
        public Deployment11[] deployments { get; set; }
        public object[] variables { get; set; }
        public object[] genericContent { get; set; }
        public Platformcontent11[] platformContent { get; set; }
        public string[] supportedPlatforms { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isCloneable { get; set; }
        public string editorId { get; set; }
    }

    public class Style11
    {
        public string key { get; set; }
        public bool isDefault { get; set; }
        public string valueString { get; set; }
        public bool valueBoolean { get; set; }
    }

    public class Deployment11
    {
        public string key { get; set; }
        public object locale { get; set; }
        public string platform { get; set; }
        public object channelIds { get; set; }
        public string value { get; set; }
    }

    public class Platformcontent11
    {
        public string locale { get; set; }
        public string type { get; set; }
        public string platform { get; set; }
        public object channelIds { get; set; }
        public string[] channelGroupIds { get; set; }
        public Block11[] blocks { get; set; }
    }

    public class Block11
    {
        public string id { get; set; }
        public string type { get; set; }
        public string role { get; set; }
        public Image11 image { get; set; }
        public Text111 text { get; set; }
    }

    public class Image11
    {
        public string mediaUrl { get; set; }
        public string altText { get; set; }
    }

    public class Text111
    {
        public string text { get; set; }
    }

    public class customertype_list2 : result
    {
        public string customertype_gid2 { get; set; }
        public string customer_type2 { get; set; }

    }
    public class servicewindow_list : result
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
        public string read_flag { get; set; }
        public List<servicewindow_list1> contacts_list { get; set; }

    }
    public class servicewindow_list1
    {
        public string whatsapp_gid { get; set; }
        public string value { get; set; }
    }

    // -----------------------------------------start excel log -----------------------------------------------------//
    public class whatslog_list : result
    {
        public string importmissed_count { get; set; }
        public string upload_date { get; set; }
        public string upload_by { get; set; }
        public string upload_gid { get; set; }
        public string user_name { get; set; }
    }
    public class whatsdtllog_list : result
    {
        public string whatsapplog_gid { get; set; }
        public string leadbank_name { get; set; }
        public string mobile { get; set; }
        public string customer_type { get; set; }
        public string upload_gid { get; set; }
    }

    // -----------------------------------------invalid number MDl ------------------------------//
    public class invalidlog_list : result
    {
        public string importinvalid_count { get; set; }
        public string upload_date { get; set; }
        public string upload_by { get; set; }
        public string upload_gid { get; set; }
        public string user_name { get; set; }
    }
    public class invalidDTLlog_list : result
    {
        public string invalidate_gid { get; set; }
        public string leadbank_name { get; set; }
        public string mobile { get; set; }
        public string customer_type { get; set; }
        public string upload_gid { get; set; }
        public string remarks { get; set; }
    }

    // -----------------------------------------end excel log -----------------------------------------------------//   


    public class messagestatus
    {
        public string nextPageToken { get; set; }
        public Result22[] results { get; set; }
    }

    public class Result22
    {
        public string id { get; set; }
        public string channelId { get; set; }
        public Sender33 sender { get; set; }
        public Receiver33 receiver { get; set; }
        public Body33 body { get; set; }
        public Meta33 meta { get; set; }
        public Template33 template { get; set; }
        public string reference { get; set; }
        public Part33[] parts { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string direction { get; set; }
        public Failure failure { get; set; }
        public int chargeableUnits { get; set; }
        public DateTime lastStatusAt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Sender33
    {
        public Connector33 connector { get; set; }
    }

    public class Connector33
    {
        public string id { get; set; }
        public string identifierValue { get; set; }
    }

    public class Receiver33
    {
        public Contact33[] contacts { get; set; }
    }

    public class Contact33
    {
        public string id { get; set; }
        public string identifierKey { get; set; }
        public string identifierValue { get; set; }
        public Annotations annotations { get; set; }
        public string countryCode { get; set; }
    }

    public class Annotations
    {
        public string name { get; set; }
    }

    public class Body33
    {
        public string type { get; set; }
        public Image33 image { get; set; }
    }

    public class Image33
    {
        public string text { get; set; }
        public Image133[] images { get; set; }
    }

    public class Image133
    {
        public string mediaUrl { get; set; }
        public string altText { get; set; }
    }

    public class Meta33
    {
        public Extrainformation33 extraInformation { get; set; }
    }

    public class Extrainformation33
    {
        public string use_wa_platform_account_id_approach { get; set; }
        public string conversation_id { get; set; }
        public string cost_request_id { get; set; }
    }

    public class Template33
    {
        public string projectId { get; set; }
        public string version { get; set; }
        public string locale { get; set; }
        public object variables { get; set; }
        public object shortLinks { get; set; }
        public object parameters { get; set; }
    }

    public class Failure
    {
        public int code { get; set; }
        public Source33 source { get; set; }
    }

    public class Source33
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Part33
    {
        public string platformReference { get; set; }
    }

}