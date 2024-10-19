using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlOutlookCampaign : result
    {
        public List<outlooktemplatesummary_list> outlooktemplatesummary_list { get; set; }
        public List<ComposeOutlookMail_list> ComposeOutlookMail_list { get; set; }
        public List<outlooksentMail_list> outlooksentMail_list { get; set; }
        public List<outlooksentcampaign_list> outlooksentcampaign_list { get; set; }
        public List<RecipientMailList_list> RecipientMailList_list { get; set; }
        public List<GetOutlookFolder_list> GetOutlookFolder_list { get; set; }
        public List<outlookapiinboxsummary_list> outlookapiinboxsummary_list { get; set; }
        


    }
    public class outlooktemplate_list : result
    {
        public string template_body { get; set; }
        public string template_name { get; set; }
        public string template_subject { get; set; }

    }
    public class mdlgraph_list 
    {
        public string clientID { get; set; }
        public string client_secret { get; set; }
        public string tenantID { get; set; }

    }
    public class outlooktemplatesummary_list : result
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
    public class appointmentcreation : result
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
    public class ComposeOutlookMail_list : result
    {
        public string employee_emailid { get; set; }
    }
    public class MdlGraphMailContent
    {
        public Message1 message { get; set; }
        public bool saveToSentItems { get; set; }
    }

    public class Message1
    {
        public string subject { get; set; }
        public Body2 body { get; set; }
        public Torecipient[] toRecipients { get; set; } = new Torecipient[0];
        public Torecipient[] ccRecipients { get; set; } = new Torecipient[0];
        public Torecipient[] bccRecipients { get; set; } = new Torecipient[0];
        public attachments[] attachments { get; set; } = new attachments[0];
    }

    public class Body2
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class Torecipient
    {
        public Emailaddress emailAddress { get; set; }
    }

    public class Emailaddress
    {
        public string address { get; set; }
    }

    public class attachments
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string name { get; set; }
        public string contentBytes { get; set; }
    }

    public class MdlSendMail: result
    {
        public string from_mail { get; set; }
        public string to_mail { get; set; }

        public string[] cc { get; set; }

        public string[] bcc { get; set; }

        public files[] file { get; set; }

        public string mail_body { get; set; }
        public string mail_sub { get; set; }
        public string leadbank_gid { get; set; }
        public string cc_mail { get; set; }
        public string bcc_mail { get; set; }
    }

    public class files
    {
        public string file_name { get; set; }
        public string content_bytes { get; set; }
    }
    public class graphtoken
    {

        public string message { get; set; }
        public string access_token { get; set; }
        public bool status { get; set; }
    }
    public class graphLoginSuccessResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
    public class outlooksentMail_list : result
    {
        public string to_mailaddress { get; set; }
        public string sent_flag { get; set; }
        public string sent_by { get; set; }
        public string sent_date { get; set; }
        public string sent_time { get; set; }
        public string leadbank_gid { get; set; }
        public string mail_gid { get; set; }
        public string mail_body { get; set; }
        public string mail_subject { get; set; }
        public string from_mailaddress { get; set; }
    }

    public class outlookmailtemplatesendsummary_list : result
    {
        public string template_gid { get; set; }
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
        public List<outlookmailsendchecklist> outlookmailsendchecklist { get; set; }


    }
    public class outlookmailsendchecklist : result
    {
        public string from_mail { get; set; }
        public string to_mail { get; set; }
        public string email { get; set; }
        public string mailmanagement_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string file { get; set; }


    }
    public class outlooksentcampaign_list : result
    {
        public string source_name { get; set; }
        public string leadbank_name { get; set; }
        public string sent_date { get; set; }
        public string email { get; set; }
        public string leadbank_region { get; set; }
        public string leadbank_gid { get; set; }


    }
    public class RecipientMailList_list : result
    {
        public string name { get; set; }
        public string contact_name { get; set; }
        public string email { get; set; }



    }

    //code by snehith 
    public class GetOutlookFolder_list : result
    {
        public string s_no { get; set; }
        public string label_id { get; set; }
        public string label_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string company_code { get; set; }
        public string integrated_gmail { get; set; }


    }
    public class outlookapiinboxsummary_list : result
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
        public string template_body { get; set; }
        public string leadbank_gid { get; set; }
        public string read_flag { get; set; }
        public string integrated_gmail { get; set; }




    }
}