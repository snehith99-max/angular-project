using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlIndiaMart : result
    {
        public List<indiamartsummary_list> indiamartsummary_list { get; set; }
        public List<indiamartview_list> indiamartview_list { get; set; }
        public string contactsync_till { get; set; }
        public string last_sync_at { get; set; }
        public string nextsync_at { get; set; }
        public string unique_query_count { get; set; }

    }
    public class indiamartsummary_list 
    {
        public string sender_name { get; set; }
        public string sender_mobile { get; set;}
        public string sender_address { get; set;}
        public string query_product_name { get; set;}
        public string query_time { get; set;}
        public string unique_query_id { get; set;}
        public string sender_city { get; set; }
        public string read_flag { get; set; }
        public string sender_state { get; set; }
    }
    public class indiamartview_list : result
    {
        public string query_type { get; set; }
        public string sender_name { get; set; }
        public string sender_email { get; set; }
        public string sender_company { get; set; }
        public string sender_city { get; set; }
        public string sender_state { get; set; }
        public string sender_pincode { get; set; }
        public string sender_country_iso { get; set; }
        public string sender_mobile_alt { get; set; }
        public string query_message { get; set; }
        public string query_mcat_name { get; set; }
        public string call_duration { get; set; }
        public string sender_mobile { get; set; }
        public string sender_address { get; set; }
        public string query_product_name { get; set; }
        public string receiver_mobile { get; set; }
        public string leadbank_gid { get; set; }
        public string read_flag { get; set; }
    }
    public class mdlAddasleadtolead : result
    {
        public string query_type { get; set; }
        public string sender_name { get; set; }
        public string sender_email { get; set; }
        public string sender_company { get; set; }
        public string sender_city { get; set; }
        public string sender_state { get; set; }
        public string sender_pincode { get; set; }
        public string sender_country_iso { get; set; }
        public string sender_mobile_alt { get; set; }
        public string query_message { get; set; }
        public string query_mcat_name { get; set; }
        public string call_duration { get; set; }
        public string sender_mobile { get; set; }
        public string sender_address { get; set; }
        public string query_product_name { get; set; }
        public string receiver_mobile { get; set; }
        public string unique_query_id { get; set; }
        public string customer_type { get; set; }
        public string appointment_timing { get; set; }
        public string bussiness_verticle { get; set; }
        public string lead_title { get; set; }
    }

    public class MdlSyncDetails : result
    {
        public string synced_till { get; set; }
        public string indiamart_status { get; set; }
    }

    public class MdlIndiaMartLeads
    {
        public int CODE { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public int APP_AUTH_FAILURE_CODE { get; set; }
        public RESPONSE[] RESPONSE { get; set; }
    }

    public class RESPONSE
    {
        public string UNIQUE_QUERY_ID { get; set; }
        public string QUERY_TYPE { get; set; }
        public string QUERY_TIME { get; set; }
        public string SENDER_NAME { get; set; }
        public string SENDER_MOBILE { get; set; }
        public string SENDER_EMAIL { get; set; }
        public string SUBJECT { get; set; }
        public string SENDER_COMPANY { get; set; }
        public string SENDER_ADDRESS { get; set; }
        public string SENDER_CITY { get; set; }
        public string SENDER_STATE { get; set; }
        public string SENDER_PINCODE { get; set; }
        public string SENDER_COUNTRY_ISO { get; set; }
        public string SENDER_MOBILE_ALT { get; set; }
        public string SENDER_PHONE { get; set; }
        public string SENDER_PHONE_ALT { get; set; }
        public string SENDER_EMAIL_ALT { get; set; }
        public string QUERY_PRODUCT_NAME { get; set; }
        public string QUERY_MESSAGE { get; set; }
        public string QUERY_MCAT_NAME { get; set; }
        public string CALL_DURATION { get; set; }
        public string RECEIVER_MOBILE { get; set; }
    }

    public class MdlIndiamartResponse
    {
        public string message { get; set; }
        public int code { get; set; }

        public int auth_code { get; set; }

        public bool status { get; set; }

    }
}