using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ems.crm.Models.MdlGmailCampaign;

namespace ems.crm.Models
{
    public class MdlGmailCampaign : result
    {

        public List<gmailtemplatesendsummary_list> gmailtemplatesendsummary_list { get; set; }
        public List<gmailtemplate_list> gmailtemplate_list { get; set; }
        public List<gmailsendchecklist> gmailsendchecklist { get; set; }
        public List<gmailapiinboxsummary_list> gmailapiinboxsummary_list { get; set; }
        public List<gmailapiinboxatatchement_list> gmailapiinboxatatchement_list { get; set; }
        public List<gmailapireply_list> gmailapireply_list { get; set; }
        public List<gmailsenditemsummary_list> gmailsenditemsummary_list { get; set; }

        public List<replymail_list> replymail_list { get; set; }

        public List<forwardmail_list> forwardmail_list { get; set; }
        public List<forwardoffwdmail_list> forwardoffwdmail_list { get; set; }
        public List<gmailapireplyinboxatatchement_list> gmailapireplyinboxatatchement_list { get; set; }
        public List<gmailcomments_list> gmailcomments_list { get; set; }
        public List<gmailapiforward_list> gmailapiforward_list { get; set; }
        public List<GetMailFolderDetails_list> GetMailFolderDetails_list { get; set; }
  		public List<GetLeaddropdown_lists> GetLeaddropdown_lists { get; set; }
        public List<allattchement_list> allattchement_list { get; set; }
        public List<createlabel_list> createlabel_list { get; set; }
        public List<updatelabel_list> updatelabel_list { get; set; }
        public List<inboxcustomer_list> inboxcustomer_list { get; set; }
        public List<Getgmailcustomerassignedlist> Getgmailcustomerassignedlist { get; set; }
        public List<Getgmailcustomerunassignedlist> Getgmailcustomerunassignedlist { get; set; }
        public List<tagcustomertogmail> tagcustomertogmail { get; set; }
        public List<GetEmailId_lists> GetEmailId_lists { get; set; }
        public List<gmailapiinboxsummary_lists> gmailapiinboxsummary_lists { get; set; }
        public class Rootobject
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }
        public class Rootobject1
        {
            public string id { get; set; }
            public string threadId { get; set; }
            public string[] labelIds { get; set; }
        }

        public string gmail_address { get; set; }
    }
    
     public class GetEmailId_lists : result
    {
        public string gmail_address { get; set; }
    }
    public class createlabel_list : result
    {
        public string labelName { get; set; }
    }
    public class updatelabel_list : result
    {
        public string labelNameEdit { get; set; }
        public string label_id { get; set; }
    }
    public class gmailcomments_list : result
    {
        public string comments { get; set; }
        public string s_no { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string inbox_id { get; set; }

    }
    public class tagcustomertogmail : result
    {


        public string inbox_id { get; set; }
        public Getgmailcustomerassignedlist[] Getgmailcustomerassignedlist;
    }
    public class untagcustomertogmail : result
    {


        public string inbox_id { get; set; }
        public Getgmailcustomerunassignedlist[] Getgmailcustomerunassignedlist;
    }
    public class Getgmailcustomerassignedlist : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string customercontact_name { get; set; }
        public string city { get; set; }
        public string country_name { get; set; }
        


    }
    public class Getgmailcustomerunassignedlist : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string customercontact_name { get; set; }
        public string city { get; set; }
        public string country_name { get; set; }



    }
    public class inboxcustomer_list : result
    {
        public string sales_order_count { get; set; }
        public string invoice_count { get; set; }
        public string customercontact_name { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string customer_name { get; set; }

    }
    
    public class GetMailFolderDetails_list : result
    {
        public string s_no { get; set; }
        public string label_id { get; set; }
        public string label_name { get; set; }
        public string label_type { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string company_code { get; set; }
        public string integrated_gmail { get; set; }
        

    }
    public class gmailmultiple_credentials :result
    {
        public string s_no { get; set; }
        public string access_token { get; set; }
        public string gmail_address { get; set; }
        public string refresh_token { get; set; }
        public string client_secret { get; set; }
        public string client_id { get; set; }
        public string default_template { get; set; }


    }
    public class forwardmail_list : result
    {
        public string inbox_id { get; set; }
        public string reply_id { get; set; }
        public string emailBody { get; set; }
        public string forwardto { get; set; }
        public string attachement_flag { get; set; }


    }
    public class forwardoffwdmail_list : result
    {
        public string forward_id { get; set; }
        public string inbox_id { get; set; }
        public string reply_id { get; set; }
        public string emailBody { get; set; }
        public string forwardto { get; set; }
        public string attachement_flag { get; set; }


    }

    public class gmailapiforward_list : result
    {
        public string s_no { get; set; }
        public string reply_id { get; set; }
        public string inbox_id { get; set; }
        public string to_id { get; set; }
        public string cc { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string bcc { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string attachement_flag { get; set; }
        public string both_body { get; set; }
        public string forward_id { get; set; }
        public List<gmailapireplyinboxatatchement_list> attachments { get; set; }

    }
    public class gmailapireply_list : result
    {
        public string s_no { get; set; }
        public string reply_id { get; set; }
        public string inbox_id { get; set; }
        public string from_id { get; set; }
        public string cc { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string bcc { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string attachement_flag { get; set; }
        public string both_body { get; set; }
        public List<gmailapireplyinboxatatchement_list> attachments { get; set; }

    }
    public class gmailapireplyinboxatatchement_list : result
    {
        public string s_no { get; set; }
        public string inbox_id { get; set; }
        public string reply_id { get; set; }

        public string original_filename { get; set; }
        public string modified_filename { get; set; }
        public string file_path { get; set; }

    }
    public class replymail_list : result
    {
        public string inbox_id { get; set; }
        public string emailBody { get; set; }
        public string orginal_body { get; set; }
        public string replytoid { get; set; }
        public string replyccid { get; set; }
        public string replybccid { get; set; }

    }
    public class gmailapiinboxsummary_list : result
    {
        public string s_no { get; set; }
        public string inbox_id { get; set; }
        public string from_id { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string attachement_flag { get; set; }
        public string template_body { get; set; }
        public string leadbank_gid { get; set; }
        public string read_flag { get; set; }
        public string integrated_gmail { get; set; }
        



    }
    public class gmailoneFoldertoother_list : result
    {


        public string label_id { get; set; }

        public gmailmovelist[] gmailmovelist;
    }
    public class gmailfolderlist : result
    {


        public string label_id { get; set; }

        public gmailmovelist[] gmailmovelist;
    }
    public class gmailmovedlist : result
    {



        public gmailmovelist[] gmailmovelist;
    }
    public class gmailmovelist
    {
        public string s_no { get; set; }
        public string inbox_id { get; set; }
        public string from_id { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string cc { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string attachement_flag { get; set; }

    }
    public class gmailapiinboxatatchement_list : result
    {
        public string s_no { get; set; }
        public string inbox_id { get; set; }
        public string original_filename { get; set; }
        public string modified_filename { get; set; }
        public string file_path { get; set; }

    }

    public class gmailtemplate_list
    {
        public string template_body { get; set; }
        public string template_subject { get; set; }
        public string template_gid { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string template_name { get; set; }
        public string template_flag { get; set; }
        public string template_count { get; set; }


    }


    public class gmailsummary_list
    {
        public string template_name { get; set; }
        public string template_subject { get; set; }
        public string template_body { get; set; }
        //

        public List<mailtemplate_list> mailtemplate_list { get; set; }
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



    //public class gmailconfiguration
    //{
    //    public string s_no { get; set; }
    //    public string access_token { get; set; }
    //    public string gmail_address { get; set; }
    //    public string refresh_token { get; set; }
    //    public string client_secret { get; set; }
    //    public string client_id { get; set; }


    //}

    public class gmailtemplatesendsummary_list : result
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
        public string dates { get; set; }
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
        //
        public string template_gid { get; set; }
        public string gmail_gid { get; set; }
        public string to_mailaddress { get; set; }
        public string created_by { get; set; }
        public string base64EncodedText { get; set; }

        public string gmail_address { get; set; }
        public string created_time { get; set; }
        public string file_path { get; set; }
        public string subject { get; set; }
        public string region_name { get; set; }
        public List<gmailsendchecklist> gmailsendchecklist { get; set; }
        public List<gmailapiinboxsummary_lists> gmailapiinboxsummary_lists { get; set; }
        public List<gmailapiinboxatatchement_lists> gmailapiinboxatatchement_lists { get; set; }
        

    }
    public class gmailsendchecklist : result
    {

        public string template_gid { get; set; }
        //
        //
        public string shopify_id { get; set; }
        public string email { get; set; }
        public string mailmanagement_gid { get; set; }
        public string leadbank_gid { get; set; }



    }
  public class gmailsenditemsummary_list
    {
        public string leadbank_gid { get; set; }
        public string document_path { get; set; }
        public string document_name { get; set; }
        public string template_gid { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string mail_body { get; set; }
        public string mail_subject { get; set; }
        public string from_mailaddress { get; set; }
        public string to_mailaddress { get; set; }
        public string gmail_gid { get; set; }

    }
    public class GetLeaddropdown_lists
    {
        public string leadbank_gid { get; set; }
        public string name { get; set; }
        public string lead_details { get; set; }
        public string leadbankbranch_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string source_name { get; set; }
        public string region_name { get; set; }
        public string gmail_address { get; set; }
        public string default_template { get; set; }
    }
    public class allattchement_list
    {
        public string document_path { get; set; }
        public string document_name { get; set; }
    }
    public class gmailapiinboxsummary_lists : result
    {
        public string inbox_id { get; set; }
        public string from_id { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool read_flag { get; set; }
        public string integrated_gmail { get; set; }
        
        public List<gmailapiinboxatatchement_lists> Attachments { get; set; }  // Sublist for attachments
    }


    public class gmailapiinboxatatchement_lists
    {
        public string inbox_id { get; set; }
        public string original_filename { get; set; }
        public string file_path { get; set; }
    }
    public class appointmentcreations : result
    {
        public string leadname_gid { get; set; }
        public string bussiness_verticle { get; set; }
        public string appointment_timing { get; set; }
        public string lead_title { get; set; }
        public string campaign_gid { get; set; }
        public string Employee_gid { get; set; }
        public string email_id { get; set; }
        public string inbox_id { get; set; }

    }
}