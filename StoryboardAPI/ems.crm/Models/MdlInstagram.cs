using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.crm.Models
{
    public class MdlInstagram : result
    {
        public List<list_instaccesstoken> list_instaccesstoken { get; set; }
        public List<instagramlist> instagramlist { get; set; }
        public List<instapostsummary_List> instapostsummary_List { get; set; }
        public List<instagramaccount_summarylist> instagramaccount_summarylist { get; set; }
        public List<instagramcommentlist> instagramcommentlist { get; set; }
        public string access_token { get; set; }

    }
    public class list_instaccesstoken
    {
        public string account_id { get; set; }
        public string access_token { get; set; }
    }

    public class instapostsummary_List
    {
        public string post_id { get; set; }
        public string caption { get; set; }
        public string post_type { get; set; }
        public string post_url { get; set; }
        public string like_count { get; set; }
        public string comments_count { get; set; }
        public string username { get; set; }
    }




    public class instagramlist : result
    {
        public string id { get; set; }
        public string ig_id { get; set; }
        public string username { get; set; }
        public string media_count { get; set; }
        public string follows_count { get; set; }
        public string followers_count { get; set; }
        public string biography { get; set; }
        public instagrampostlist[] data { get; set; }

    }
    public class instagramaccount_summarylist
    {
        public string instagram_gid { get; set; }
        public string account_id { get; set; }
        public string username { get; set; }
        public string media_count { get; set; }
        public string follows_count { get; set; }
        public string followers_count { get; set; }
        public string biography { get; set; }
    }
    public class instagrampostlist
    {
        public string caption { get; set; }
        public string comments_count { get; set; }
        public string like_count { get; set; }
        public string media_type { get; set; }
        public string media_url { get; set; }
        public string id { get; set; }

    }
    public class instagramcommentlist
    {
        public string post_id { get; set; }
        public string post_type { get; set; }
        public commentlist[] data { get; set; }

    }
    public class commentlist
    {
        public userdetails from { get; set; }
        public string text { get; set; }
        public string hidden { get; set; }
        public string id { get; set; }
        public string like_count { get; set; }
        public DateTime timestamp { get; set; }

    }
    public class userdetails
    {
        public string id { get; set; }
        public string username { get; set; }
    }
    public class mdlcomment : result
    {
        public string caption { get; set; }
        public string post_type { get; set; }
        public string post_url { get; set; }
        public string like_count { get; set; }
        public string comments_count { get; set; }
        public string username { get; set; }
        public string media_count { get; set; }
        public string follows_count { get; set; }
        public string followers_count { get; set; }
        public List<instacomment_list> instacomment_list { get; set; }
        public List<instainsights_list> instainsights_list { get; set; }

    }
    public class instacomment_list
    {
        public string user_name { get; set; }
        public string commentlike_count { get; set; }
        public string comment_message { get; set; }
        public string comment_time { get; set; }
        public string comment_days { get; set; }
    }

    public class instaview_insights
    {
        public insights[] data { get; set; }
    }

    public class insights
    {
        public string name { get; set; }
        public string period { get; set; }
        public values[] values { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string id { get; set; }
    }

    public class values
    {
        public int value { get; set; }
    }

    public class instainsights_list
    {
        public string post_type { get; set; }
        public string engagement { get; set; }
        public string comments { get; set; }
        public string likes { get; set; }
        public string shares { get; set; }
        public string saved { get; set; }
        public string reach { get; set; }
        public string impressions { get; set; }
    }
    public class Testas
    {
        public string id { get; set; }
    }
    public class HrDocument
    {
        public string AutoID_Key { get; set; }
        public string document_name { get; set; }
        public string document_gid { get; set; }
        public string file_name { get; set; }
    }
    public class ErrorResponse
    {
        public ErrorDetails error { get; set; }
    }

    public class ErrorDetails
    {
        public string message { get; set; }
        public string error_user_msg { get; set; }
    }
    public class UserTag
    {
        public string username { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }

    public class taggedUsers
    {
        public UserTag[] user_tags { get; set; } = new UserTag[0];
    }
    public class UserTags
    {
        public string username { get; set; }
    
    }

    public class taggedUsers1
    {
        public UserTags[] user_tags { get; set; } = new UserTags[0];
    }
}

