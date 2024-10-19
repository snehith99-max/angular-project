﻿using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlMyvisit : result
    {
        public List<ExpiredVisit_list> ExpiredVisit_list { get; set; }
        public List<Todayvisit_list> Todayvisit_list { get; set; }
        public List<Upcomingvisit_list> Upcomingvisit_list { get; set; }
        public List<product_list1> product_list1 { get; set; }
        public List<product_group_list> product_group_list { get; set; }
        public List<myvisitcount_list> myvisitcount_list { get; set; }
        public List<schedulelogsummary_list> schedulelogsummary_list { get; set; }

    }
    public class ExpiredVisit_list : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }
        public string log_gid { get; set; }
        public string date_of_demo { get; set; }
        public string meeting_time { get; set; }
        public string location { get; set; }
        public string prosperctive_percentage { get; set; }
        public string schedule_remarks { get; set; }
        public string customer_address { get; set; }
        public string region_source { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string ScheduleRemarks { get; set; }
        public string schedule_status { get; set; }
        public string created_date { get; set; }
        public string schedulelog_gid { get; set; }
        public string date_of_demo_online { get; set; }
        public string meeting_time_online { get; set; }
        public string prosperctive_percentage_online { get; set; }
        public string technical_assist { get; set; }
        public string prospective_percentage { get; set; }
        public string product_name { get; set; }
        public string contact_person_online { get; set; }
        public string product_group { get; set; }
        public string demo_remarks { get; set; }
        public string date_of_visit_offline { get; set; }
        public string meeting_time_offline { get; set; }
        public string visited_by { get; set; }
        public string location_offline { get; set; }
        public string meeting_remarks_offline { get; set; }
        public string contact_person_offline { get; set; }
        public string altrnate_person { get; set; }
        public string contact_person { get; set; }
        public string schedule_type_offline { get; set; }

    }
    public class Todayvisit_list : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }

        public string customer_address { get; set; }
        public string region_name { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }
        public string schedulelog_gid { get; set; }

        public string ScheduleRemarks { get; set; }
        public string schedule_status { get; set; }
        public string log_gid {  get; set; }

    }
    public class Upcomingvisit_list : result
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string contact_details { get; set; }

        public string customer_address { get; set; }
        public string region_source { get; set; }
        public string schedule_type { get; set; }
        public string schedule { get; set; }

        public string ScheduleRemarks { get; set; }
        public string schedule_status { get; set; }
        
        public string postponed_date { get; set; }

        public string meeting_time_postponed { get; set; }
        public string schedulelog_gid { get; set; }
        public string log_gid { get; set; }

    }
    public class product_list1 : result
    {

        public string product_gid { get; set; }
        public string product_name { get; set; }



    }
    public class product_group_list : result
    {

        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
    }


    public class myvisitcount_list : result
    {
        public string expired { get; set;}
        public string upcoming { get; set;}
        public string todayvisit { get; set;}
        public string completed { get; set;}
    }
    public class schedulelogsummary_list : result
    {
        public string leadbank_name { get; set; }
        public string onmeet_date { get; set; }
        public string onmeet_hour { get; set; }
        public string onmeet_contactperson { get; set; }
        public string onmeet_remarks { get; set; }
        public string technical_aid { get; set; }
        public string demo_shown { get; set; }
        public string fieldvisit_date { get; set; }
        public string fieldvisit_hour { get; set; }
        public string fieldvisit_contactperson { get; set; }
        public string fieldvisit_remarks { get; set; }
        public string fieldvisit_contactperson2 { get; set; }
        


    }

}