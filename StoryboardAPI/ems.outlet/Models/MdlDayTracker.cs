using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlDayTracker : result
    {
        public List<revenue_list> revenue_list { get; set; }
        public List<expense_list> expense_list { get; set; }
        public List<daytrackersummary_list> daytrackersummary_list { get; set; }
        public List<DaytrackerData> DaytrackerData { get; set; }
        public List<outletname_list> outletname_list { get; set; }
        public List<otpverification_list> otpverification_list { get; set; }
        public List<edittracker_list> edittracker_list { get; set; }
        public List<edittrackerrevenue_list> edittrackerrevenue_list { get; set; }
        public List<edittrackerexpence_list> edittrackerexpence_list { get; set; }
        public List<trade_list> trade_list { get; set; }


    }
    public class daytrackersummary_list : result
    {

        public string revenue_code { get; set; }
        public string revenue_desc { get; set; }
        public string revenue_gid { get; set; }
        public string revenue_amount { get; set; }
        public string created_date { get; set; }
        public string trade_date { get; set; }
        public string expense_amount { get; set; }
        public string daytracker_gid { get; set; }
        public string edit_status { get; set; }
        public string tracker_gid { get; set; }
        public string campaign_name { get; set; }
        public string created_by { get; set; }
        
    }

    public class revenue_list : result
    {

        public string revenue_code { get; set; }
        public string revenue_desc { get; set; }
        public string revenue_gid { get; set; }
        public string revenue_amount { get; set; }
        public string revenue_name { get; set; }
        public string daytrackerdtl_gid { get; set; }
        public string daytracker_gid { get; set; }


    }
    public class otpverification_list : result
    {

        public string created_date { get; set; }
        public string campaign_title { get; set; }
        public string edit_reason { get; set; }
        public string edit_status { get; set; }
        public string rasied_by { get; set; }
        public string daytracker_gid { get; set; }
        public string approval_otp { get; set; }
        public string tracker_gid { get; set; }


    }
    public class expense_list : result
    {

        public string expense_gid { get; set; }
        public string expense_code { get; set; }
        public string expense_desc { get; set; }
        public string expense_amount { get; set; }
        public string expense_name { get; set; }
        public string daytrackerdtl_gid { get; set; }
        public string daytracker_gid { get; set; }


    }
    public class DaytrackerData : result
    {

        public List<revenue_list> revenue_list { get; set; }
        public List<expense_list> expense_list { get; set; }
        public List<combinedlist> combined_list { get; set; }
        public string total_amount { get; set; }
        public string expense_total { get; set; }
        public string revenue_total { get; set; }
        public string daytracker_gid { get; set; }
        public string remarks { get; set; }
        public string trade_date { get; set; }
        public string leave { get; set; }
    }
    public class combinedlist : result

    {
        public List<revenue_list> revenue_list { get; set; }
        public List<expense_list> expense_list { get; set; }

    }
    public class outletname_list : result

    {
        public string campaign_title { get; set; }
        public string edit_reason { get; set; }
        public string daytracker_gid { get; set; }

    }
    public class trade_list : result

    {
        public string balance_date { get; set; }
        public string notification { get; set; }
        public string previous_date { get; set; }

    }
    public class whatsappconfiguration
    {
        public string workspace_id { get; set; }
        public string channel_id { get; set; }
        public string access_token { get; set; }
        public string channelgroup_id { get; set; }

    }
    public class Result
    {
        public string id { get; set; }
        public string channelId { get; set; }
        public Sender sender { get; set; }
        public Receiver receiver { get; set; }
        public string reference { get; set; }
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
    public class edittracker_list : result
    {

        public string edit_reason { get; set; }
        public string tracker_gid { get; set; }
        public string edit_status { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string revenue_amount { get; set; }
        public string expense_amount { get; set; }
        public string pre_revenue { get; set; }
        public string pre_expense { get; set; }
        public string trackerhis_gid { get; set; }
        public string daytracker_gid { get; set; }
    }
    public class edittrackerrevenue_list : result
    {
        public string revenue_name { get; set; }
        public string amount { get; set; }
        public string trackerhis_gid { get; set; }
        public string revenue_amount { get; set; }
    }
    public class edittrackerexpence_list : result
    {
        public string expense_amount { get; set; }
        public string amount { get; set; }
        public string trackerhis_gid { get; set; }
        public string expense_name { get; set; }
    }
}