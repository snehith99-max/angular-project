using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlFeatures : result
    {
        public List<notesupdate_list> notesupdate_list { get; set; }

        public List<meetingschedule_list> meetingschedule_list { get; set; }

    }
    public class notesupdate_list : result
    {
        public string notes_detail { get; set; }
        public string s_no { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
    }

    public class meetingschedule_list : result
    {
        public string s_no { get; set; }
        public string meeting_title { get; set; }
        public string meeting_link { get; set; }
        public string start_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }

    }

    public class MdlCalendly : result
    {
        public List<MdlCalendlyMeetingDates> calendlyMeetingList { get; set; }
    }

    public class MdlCalendlyMeetingDates
    {
        public string meeting_date { get; set; }
        public List<MdlCalendlyMeetingDetails> meetingList { get; set; }
    }

    public class MdlCalendlyMeetingDetails
    {
        public string meeting_organiser { get; set; }
        public string meeting_url { get; set; }
        public string meeting_participants { get; set; }
        public string meeting_title { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string location { get; set; }
        public string meeting_type { get; set; }

    }


    public class MdlCalendlyUserDetails
    {
        public Resource resource { get; set; }
    }

    public class Resource
    {
        public string avatar_url { get; set; }
        public DateTime created_at { get; set; }
        public string current_organization { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string resource_type { get; set; }
        public string scheduling_url { get; set; }
        public string slug { get; set; }
        public string timezone { get; set; }
        public DateTime updated_at { get; set; }
        public string uri { get; set; }
    }

    public class MdlcalendlyAccountDetails : result
    {
        public string scheduling_url { get; set;}
    }
}