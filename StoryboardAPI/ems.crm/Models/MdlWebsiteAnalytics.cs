﻿using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlWebsiteAnalytics : result
    {
        public List<assignvisitsubmit_list6> assignvisitsubmit_list6 { get; set; }
        public List<analytics_list> analytics_list { get; set; }
        public List<analytics_summarylist> analytics_summarylist { get; set; }
        public List<analytics_summarylist1> analytics_summarylist1 { get; set; }
        public List<analytics_summarylist2> analytics_summarylist2 { get; set; }
        public List<analytics_report> analytics_report { get; set; }
        public List<analytics_summarylist3> analytics_summarylist3 { get; set; }
    }
    
    public class assignvisitsubmit_list6 : result
    {
        public analytics_list[] analytics_list;
       
    }
    public class analytics_list : result
    {
        public string firstSessionDate { get; set; }
        public string firstUserCampaignName { get; set; }
        public string country { get; set; }
        public string activeUsers { get; set; }
        public string newUsers { get; set; }
        public string totalUsers { get; set; }
        public string eventCount { get; set; }
        public string date { get; set; }
        public string hour { get; set; }
        public string minute { get; set; }
        public string deviceCategory { get; set; }
        public string operatingSystem { get; set; }
        public string pageTitle { get; set; }
        public string city { get; set; }
        public string screenPageViews { get; set; }
        public string screenPageViewsPerUser { get; set; }
    }


    public class analytics_summarylist : result
    {
        public string eventCount { get; set; }
        public string CampaignName { get; set; }
        //public string userCount { get; set; }
        public string total_user { get; set; }
        //public string referral { get; set; }
        public string newUsers { get; set; }
        //public string organic { get; set; }


    }

    public class analytics_summarylist1 : result
    {
        public string eventCount { get; set; }
        public string direct { get; set; }
        public string userCount { get; set; }
        public string total_user { get; set; }
        public string referral { get; set; }
        public string total_users { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string deviceCategory { get; set; }
        public string browser { get; set; }
        public string firstUserCampaignName { get; set; }
        public string newUsers { get; set; }
        public string pageTitle { get; set; }

    }
   public class analytics_summarylist2:result{
        public string eventCount { get; set; }
        public string direct { get; set; }
        public string userCount { get; set; }
        public string total_user { get; set; }
        public string referral { get; set; }
        public string total_users { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string deviceCategory { get; set; }
        public string browser { get; set; }
        public string firstUserCampaignName { get; set; }
        public string newUsers { get; set; }
    }

    public class analytics_report : result
    {

        public string week_date { get; set; }
        public string week_users { get; set; }
        public string daily_date { get; set; }
        public string daily_users { get; set; }
        public string Months { get; set; }
        public string users { get; set; }
        public string year_users { get; set; }
        public string year { get; set; }
    }
    public class analytics_summarylist3 : result
    {
        public string pageTitle { get; set; }
        public string totalDuration { get; set; }
    }
    public class googleanalyticsconfiguration
    {

        public string user_url { get; set; }
        public string page_url { get; set; }
    }
}