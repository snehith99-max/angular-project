using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class MdlSubManagement : result
    {
        public List<subscription_list> subscription_list { get; set; }
        public List<server_list> server_list { get; set; }
        public List<consumer_list> consumer_list { get; set; }
        public List<consumerinactive> consumerinactive { get; set; }
        public string server_name { get; set; }
        public string hosting_details { get; set; }
        public string token_number { get; set; }
        public string server_ipaddress { get; set; }
        public string server_gid { get; set; }
        public string consumer_gid { get; set; }
        public string company_code { get; set; }
        public string consumer_url { get; set; }
        public string subscription_details { get; set; }
        public string server_status { get; set; }
        public string active_status { get; set; }
        public List<country> country { get; set; }
        public string country_name { get; set; }
        public string cbopermanent_country { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string company_name { get; set; }
        
    }
    public class country : result
    {
        public string country_gid { get; set; }
        public string country_name { get; set; }
    }
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string payment_link { get; set; }
    }

    public class subscription_list : result
    {

            public string company_code { get; set; }
            public string company_name { get; set; }
            public string server_name { get; set; }
            public string db_name { get; set; }
            public string created_date { get; set; }
            public string user_name { get; set; }
        
    }
    public class server_list : result
    {

        public string server_name { get; set; }
        public string hosting_details { get; set; }
        public string token_number { get; set; }
        public string server_ipaddress { get; set; }
        public string server_gid { get; set; }
        public string consumer_gid { get; set; }
        public string company_code { get; set; }
        public string consumer_url { get; set; }
        public string subscription_details { get; set; }
        public string status { get; set; }

        public string server_status { get; set; }

        public string total_count { get; set; }

        public string country_name { get; set; }

    }
    public class consumer_list : result
    {
        public string server_gid { get; set; }
        public string server_name { get; set; }
        public string consumer_gid { get; set; }
        public string company_code { get; set; }
        public string consumer_url { get; set; }
        public string subscription_details { get; set; }
        public string status { get; set; }
        public string active_status { get; set; }
        public string end_date { get; set; }
        public string start_date { get; set; }
    }

    public class consumerinactive : result
    {
        public string rbo_status { get; set; }
        public string remarks { get; set; }
        public string consumer_gid { get; set; }
    }

    public class consumerhistory : result
    {
        public List<consumerhistory_list> consumerhistory_list { get; set; }

    }

    public class consumerhistory_list : result
    {
        public string statuses { get; set; }
        public string remarks { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }

    }
    public class scriptuploaddocument : result
    {
        public string institution2form60documentupload_gid { get; set; }
        public string institution2documentupload_gid { get; set; }
        public string institution_gid { get; set; }
        public string[] filename { get; set; }
        public string filepath { get; set; }
        public string[] compfilename { get; set; }
        public string compfilepath { get; set; }
        public string[] forwardfilename { get; set; }
        public string forwardfilepath { get; set; }

        public string[] doufilename { get; set; }
        public string doufilepath { get; set; }
    }
    public class UploadDocument
    {
        public string AutoID_Key { get; set; }
        public string document_name { get; set; }
        public string document_gid { get; set; }
        public string file_name { get; set; }
    }
}
