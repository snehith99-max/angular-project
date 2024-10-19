using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ems.crm.Models
{
    public class MdlCampaignService: result
    {
        public List<campaignservice_list> campaignservice_list { get; set; }
        public List<shopifycampaignservice_list> shopifycampaignservice_list { get; set; }
        public List<mailcampaignservice_list> mailcampaignservice_list { get; set; }
        public List<shopifyservcie_list> shopifyservcie_list { get; set; }
        public List<emailservice_list> emailservice_list { get; set; }
        public List<facebookcampaignservice_list> facebookcampaignservice_list { get; set; }
        public List<instagramcampaignservice_list> instagramcampaignservice_list { get; set; }
        public List<linkedincampaignservice_list> linkedincampaignservice_list { get; set; }
        public List<telegramcampaignservice_list> telegramcampaignservice_list { get; set; }
        public List<customertype_list> customertype_list { get; set; }
        public List<livechatservice_list> livechatservice_list { get; set; }
        public List<country_list2> country_list2 { get; set; }
        public List<Company_list> Company_list { get; set; }
        public List<currency_list> currency_list { get; set; }
        public List<module_list> module_list { get; set; }
        public List<submodule_list> submodule_list {  get; set; }
        public List<updatemodule_list> updatemodule_list { get; set; }
        public List<modulesummary_list> modulesummary_list { get; set; }
        public List<clicktocall_list> clicktocall_list { get; set; }
        public List<gmailcampaignservice_list> gmailcampaignservice_list { get; set; }
        public List<googleanalyticsservice_list> googleanalyticsservice_list { get; set; }
        public List<smscampaignservice_list> smscampaignservice_list { get; set; }
        public List<indiamartcampaignservice_list> indiamartcampaignservice_list { get; set; }
        public List<calendarservice_list> calendarservice_list { get; set; }
        public List<mintsoft_list> mintsoft_list { get; set; }
        public List<einvoice_list> einvoice_list { get; set; }
        public List<outlookcampaignservice_list> outlookcampaignservice_list { get; set; }
        public List<taglist> taglist { get; set; }
        public List<untaglist> untaglist { get; set; }
        public List<paymentgatewayservice_list> paymentgatewayservice_list { get; set; }   


    }
    public class smscampaignservice_list : result
    {
        public string sms_user_id { get; set; }
        public string sms_password { get; set; }
        public string sms_id { get; set; }
        public string sms_status { get; set; }
    }
    public class indiamartcampaignservice_list : result
    {
        public string api_key { get; set; }
        public string indiamart_id { get; set; }
        public string indiamart_status { get; set; }
        public DateTime apikey_generateddate { get; set; }
    }
    public class telegramcampaignservice_list : result
    {

        public string bot_id { get; set; }
        public string chat_id { get; set; }
        public string telegram_id { get; set; }
        public string telegram_status { get; set; }

    }
    public class linkedincampaignservice_list : result
    {

        public string linkedin_access_token { get; set; }
        public string linkedin_id { get; set; }
        public string linkedin_status { get; set; }


    }
    public class facebookcampaignservice_list : result
    {

        public string facebook_access_token { get; set; }
        public string facebook_id { get; set; }
        public string facebook_page_id { get; set; }
        public string facebook_status { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }


    public class emailservice_list : result
    {
        public string mail_access_token { get; set; }
        public string mail_base_url { get; set; }
        public string email_id { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string email_username { get; set; }
        public string email_status { get; set; }
        public string mail_service { get; set; }

    }
    public class gmailservice_list : result
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string refresh_token { get; set; }
        public string gmail_address { get; set; }
        public string gmail_status { get; set; }
        public string email_id { get; set; }
        public string mail_service { get; set; }



    }
    public class gmailservice_lists : result
    {
        public string gclient_id { get; set; }
        public string gclient_secret { get; set; }
        public string grefresh_token { get; set; }
        public string ggmail_address { get; set; }
        public string ggmail_status { get; set; }
        public string gemail_id { get; set; }
        public string mail_service { get; set; }



    }
    public class outlookservice_list : result
    {
        public string outlook_client_id { get; set; }
        public string outlook_client_secret { get; set; }
        public string tenant_id { get; set; }
        public string gmail_status { get; set; }
        public string mail_service { get; set; }


    }

    public class shopifyservcie_list : result
    {
        public string shopify_accesstoken { get; set; }
        public string shopify_store_name { get; set; }
        public string store_month_year { get; set; }
        public string shopify_id { get; set; }

        public string shopify_status { get; set; }



    }
    public class campaignservice_list : result
    {
        public string access_token { get; set; }
        public string base_url { get; set; }
        public string workspace_id { get; set; }
        public string channel_id { get; set; }
        public string mobile_number { get; set; }
        public string channelgroup_id { get; set; }
        public string channel_name { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string access_token_edit { get; set; }
        public string whatsapp_accesstoken { get; set; }
        public string workspace_id_edit { get; set; }
        public string s_no { get; set; }
        public string whatsapp_id { get; set; }
        public string whatsapp_status { get; set; }
        public string name { get; set; }
        public string user_gid { get; set; }


    }
    public class taglist : result
    {
       
        public string name { get; set; }
        public string user_gid { get; set; }


    }
  
    public class untaglist : result
    {
      
        public string name { get; set; }
        public string user_gid { get; set; }


    }
    public class shopifycampaignservice_list : result
    {
        

        public string shopify_access_token { get; set; }
        public string shopify_store_name { get; set; }
        public string store_month_year { get; set; }
        public string shopify_created_date { get; set; }
        public string shopify_created_by { get; set; }
        public string s_no { get; set; }
        public string shopify_status { get; set; }



    }

    public class mailcampaignservice_list : result
    {


        public string mail_access_token { get; set; }
        public string mail_base_url { get; set; }
        public string mail_created_date { get; set; }
        public string mail_created_by { get; set; }
        public string s_no { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string email_username { get; set; }
        public string email_status { get; set; }
        public string mail_toggle { get; set; }


    }
    public class gmailcampaignservice_list : result
    {


        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string refresh_token { get; set; }
        public string gmail_address { get; set; }
        public string gmail_status { get; set; }
        public string s_no { get; set; }



    }
    public class instagramcampaignservice_list : result
    {

        public string instagram_access_token { get; set; }
        public string instagram_id { get; set; }
        public string instagram_account_id { get; set; }
        public string instagram_status { get; set; }



    }
    public class calendarservice_list : result
    {

        public string api_key { get; set; }
        public string active_flag { get; set; }
        public string calendar_id { get; set; }

    }

    public class einvoice_list : result
    {
        public string einvoice_id { get; set; }
        public string id { get; set; }
        public string einvoiceAutenticationURL { get; set; }
        public string einvoiceIRNGenerate { get; set; }
        public string gspappid { get; set; }
        public string gspappsecret { get; set; }
        public string einvoiceuser_name { get; set; }
        public string einvoicepwd { get; set; }
        public string einvoicegstin { get; set; }
        public string einvoice_Auth { get; set; }
        public string generateQRURL { get; set; }
        public string cancleIRN { get; set; }
        public string einvoice_flag { get; set; }
    }
    public class mintsoft_list : result
    {

        public string api_key { get; set; }
        public string mintsoft_flag { get; set; }
        public string mintsoft_id { get; set; }
        public string id { get; set; }
        public string base_url { get; set; }

    }
    public class customertype_list : result
    {
        public string customer_type { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
        public string customertype_gid { get; set; }

        public string corporate_gid { get; set; }
        public string corporate_name { get; set; }
        public string distributor_gid { get; set; }
        public string distributor_name { get; set; }
        public string retailer_gid { get; set; }
        public string retailer_name { get; set; }

    }
    public class livechatservice_list : result
    {

        public string livechat_agentid { get; set; }
        public string livechat_access_token { get; set; }
        public string livechat_id { get; set; }
        public string livechat_status { get; set; }

    }

    public class Company_list : result
    {
        public string sequence_reset { get; set; }
        public string country_name { get; set; }
        public string company_state { get; set; }
        public string contact_person_phone { get; set; }
        public string contact_person_mail { get; set; }
        public string company_address { get; set; }
        public string contact_person { get; set; }
        public string company_name { get; set; }
        public string company_code { get; set; }
        public string company_mail { get; set; }
        public string company_phone { get; set; }
        public string company_gid { get; set; }
        public string company_logo { get; set; }
        public string welcome_logo { get; set; }
        public string company_address1 { get; set; }
        public string currency_code { get; set; }
        public string currency { get; set; }
        

    }

    public class currency_list : result
    {
        public string currency_code { get; set; }
        public string currency { get; set; }
        public string symbol { get; set; }
    }

    public class country_list2 : result
    {
        public string country_name { get; set; }
        public string country_gid { get; set; }
    }

    public class module_list : result
    {
        public string module_gid { get; set; }

        public string module_name { get; set; }

    }

    public class submodule_list : result
    {
        public string Module_gid { get; set; }
        public string Module_name { get; set;}
    }

    public class updatemodule_list: result
    {
        public string Module_gid { get; set;}
        public string Module_name { get; set;}

        public string module_gid { get; set; }
        public string module_name { get; set; }
    }

    public class modulesummary_list : result
    {
        public string module_gids { get; set;}
        public string module_names { get; set;}
    }
    public class clicktocall_list : result
    {

        public string clicktocall_id { get; set; }
        public string clicktocall_status { get; set; }
        public string clicktocall_baseurl { get; set; }
        public string clicktocall_access_token { get; set; }

    }
    public class googleanalyticsservice_list : result
    {

        public string user_url { get; set; }
        public string googleanalytics_id { get; set; }
        public string page_url { get; set; }
        public string googleanalytics_status { get; set; }

    }
    public class outlookcampaignservice_list : result
    {


        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string tenant_id { get; set; }
        public string outlook_status { get; set; }
       


    }
    public class tagemployee : result
    {
        public string emailaddress { get; set; }

        public tagemployeelist[] taglist;
    }
    public class untagemployee : result
    {
        public string emailaddress { get; set; }

        public untagemployeelist[] untaglist;
    }
    public class tagemployeelist : result
    {
       
        public string user_gid { get; set; }

    }
    public class untagemployeelist : result
    {
       
        public string user_gid { get; set; }

    }
    public class paymentgatewayservice_list : result
    {

        public string key1 { get; set; }
        public string key2 { get; set; }
        public string key3 { get; set; }
        public string payment_gateway { get; set; }

    }
    public class Mdlhomepage:result
    {
        public string home_page { get; set; }

    }
    public class Mdlenablekot : result
    {
        public string enable_kot { get; set; }
    }

}