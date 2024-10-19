using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ems.crm.Models
{
    public class Mdlclicktocall : result
    {
        public List<calllog_report> calllog_report { get; set; }
        public List<agent_report> agent_report { get; set; }
        public List<agent_barchartreport> agent_barchartreport { get; set; }
        public List<inboundanalytics_report> inboundanalytics_report { get; set; }
        public List<dateanalytics_report> dateanalytics_report { get; set; }
        public List<outboundanalytics_report> outboundanalytics_report { get; set; }
        public List<leadupdate> leadupdate { get; set; }

        public string user_name { get; set; }
        public string phone_number { get; set; }
        public string agent_missed { get; set; }
        public string answered { get; set; }
        public string total_count { get; set; }
        public string customer_missed { get; set; }
    }

    public class calllog_report : result
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
        public string lead_flag { get; set; }
    }
    public class calling
    {
        public string user_name { get; set; }
        public string phone_number { get; set; }
        public string didnumber { get; set; }
        public string remarks { get; set; }
        public string individual_gid { get; set; }

    }
    public class addleadvalue
    {

        public string customertype_edit { get; set; }
        public string phone_number { get; set; }
        public string user_name { get; set; }
        public string inline_id { get; set; }

    }
    public class click2callresponse
    {
        public int statusCode { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public string uid { get; set; }
    }

    public class click2callerrorresponse
    {
        public int statusCode { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }
    public class agent_report : result
    {
        public string agent_missed { get; set; }
        public string answered { get; set; }
        public string total_count { get; set; }
        public string customer_missed { get; set; }
        public string agent_name { get; set; }
        public string agent_number { get; set; }
        public string agent_mailid { get; set; }
        public string agent_gid { get; set; }
    }
    public class agent_barchartreport : result
    {
        public string agent_missed { get; set; }
        public string answered { get; set; }
        public string total_count { get; set; }
        public string customer_missed { get; set; }
    }
    public class inboundanalytics_report : result
    {

        public string weekly_user { get; set; }
        public string daily_users { get; set; }
    }
    public class outboundanalytics_report: result
    {

        public string weekly_user { get; set; }
        public string daily_users { get; set; }
    }
    public class dateanalytics_report : result
    {
        public string daily_date { get; set; }
        public string week_date { get; set; }

    }
    public class clicktocallconfiguration
    {
        public string base_url { get; set; }
        public string access_token { get; set; }

    }
    public class leadupdate : result
    {
        public string phone_number { get; set; }
       
    }

}
