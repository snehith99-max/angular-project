using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOutletManager : result
    {
        public List<daymanagersummary_list> daymanagersummary_list { get; set; }
        public List<managerApproval_list> managerApproval_list { get; set; }

    }
    public class daymanagersummary_list : result
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
        public string campaign_title { get; set; }
    }
    public class managerApproval_list : result
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
}