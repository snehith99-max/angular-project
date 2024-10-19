using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlMyAppointment : result
    {
        public List<gettotalappointmentsummary_lists> gettotalappointmentsummary_lists { get; set; }
        public List<getmyappointmenttilescount_lists> getmyappointmenttilescount_lists { get; set; }
        public List<getmyappointmentlog_lists> getmyappointmentlog_lists { get; set; }
    }
    public class gettotalappointmentsummary_lists : result
    {
        public string leadbank_gid { get; set; }
        public string appointment_gid { get; set; }
        public string internal_notes { get; set; }
        public string potential_value { get; set; }
        public string lead_title { get; set; }
        public string Opportunitylog_gid { get; set; }
        public string business_vertical { get; set; }
        public string schedule_status { get; set; }
        public string leadbank_name { get; set; }
        public string Address_details { get; set; }
        public string Details { get; set; }
        public string region_source { get; set; }
        public string appointment_date { get; set; }
        public string lead_contact { get; set; }
        public string potential_value_count { get; set; }
    }
    public class getmyappointmenttilescount_lists : result
    {
        public string total_count { get; set; }
        public string assigned_count { get; set; }
        public string unassigned_count { get; set; }
        public string completed_count { get; set; }
        public string today_count { get; set; }
        public string upcoming_count { get; set; }
        public string expired_count { get; set; }
        public string New_count { get; set; }
        public string prospect_count { get; set; }
        public string potentials_count { get; set; }
        public string drop_count { get; set; }
        public string closed_count { get; set; }
    }    public class getmyappointmentlog_lists : result
    {
        public string appointment_date { get; set; }
        public string lead_title { get; set; }

    }
    public class Poststatuspostpone_list : result
    {
        public string schedule_remarks { get; set; }
        public string postponed_date { get; set; }
        public string Opportunitylog_gid { get; set; }
        public string appointment_gid { get; set; }
    }
    public class Poststatusclose_list : result
    {
        public string schedule_remarks { get; set; }
        public string Opportunitylog_gid { get; set; }
    }
}