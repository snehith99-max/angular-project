using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlTelecallerManager:result
    {
        public List<telecallermanager_lists>telecallermanager_lists{ get;set; }
        public List<telecallermanager_totallists> telecallermanager_totallists { get;set; }
        public List<telechartscount_list> telechartscount_list { get;set; }
        public List<teletotaltilecount_lists> teletotaltilecount_lists { get;set; }
        public List<Performencechart_list> Performencechart_list { get;set; }
        public List<telecampaigntransfer_list> telecampaigntransfer_list { get;set; }
        public List<telecallerteam_list> telecallerteam_list { get; set; }
        public List<teleemployee_list> teleemployee_list { get; set; }
        public List<Telecallerbin_list> Telecallerbin_list { get; set; }
        public List<GetLeadNoteDetails_list> GetLeadNoteDetails_list { get; set; }
        
    } 
    public class telecallermanager_lists : result
    {
            public string campaign_gid { get; set; }
            public string campaign_title { get; set; }
            public string campaign_location { get; set; }
            public string branch_name { get; set; }
            public string employeecount { get; set; }
            public string assigned_leads { get; set; }
            public string Schedule_log { get; set; }
            public string newleads { get; set; }
            public string followup { get; set; }
            public string prospect { get; set; }
            public string NewPending { get; set; }
            public string drop_status { get; set; }
            public string closed { get; set; }
            public string lapsed_count { get; set; }
            public string longest_count { get; set; }
            public string leadbank_gid { get; set; }
            public string lead2campaign_gid { get; set; }
            public string team_prefix { get; set; }
        public string todayappointment { get; set; }



    }

    public class telecallermanager_totallists : result
    {
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string assigned_to { get; set; }
        public string department_name { get; set; }
        public string created_by { get; set; }
        public string leadstage_name { get; set; }
        public string leadbank_name { get; set; }
        public string campaign_title { get; set; }
        public string branch_name { get; set; }
        public string customer_type { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string assignedto_gid { get; set; }
        public string drop_remarks { get; set; }
        public string team_prefix { get; set; }
        public string internal_notes { get; set; }
        public string remarks { get; set; }
        public string notes_count { get; set; }
        public string appointment_gid { get; set; }
        
    }

    public class telechartscount_list : result
    {
        public string new_leads { get; set; }
        public string months { get; set; }
        public string follow_up { get; set; }
        public string pending_calls { get; set; }
        public string prospect { get; set; }
        //public string quotationmonthcount { get; set; }
        //public string salesmonthcount { get; set; }
        //public string salesmonth { get; set; }


    }

    public class teletotaltilecount_lists : result
    {
        public string total_potential {  get; set; }
        public string total_followup {  get; set; }
        public string total_newleads {  get; set; }
        public string total_prospect {  get; set; }
        public string total_dropstatus {  get; set; }
        public string assigned_leads {  get; set; }
       

    }

    public class Performencechart_list : result
    {
        public string call_response { get; set; }
        public string callcount { get; set;}
    }

    public class telecampaigntransfer_list : result
    {
        public string team_name { get; set; }
        public string team_member { get; set; }
        public string assign_user { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string assignedto_gid { get; set; }

        public campaign_list[] campaign_list;
    }
    public class telecallerteam_list : result
    {
        public string campaign_gid { get; set; }
        public string campaign_title { get; set; }
    }
    public class teleemployee_list : result
    {
        public string employee_gid { get; set; }
        public string user_name { get; set; }
    }
    public class Telecallerbin_list : result
    {

        public string campaign_gid { get; set; }
        public string assign_user { get; set; }
        public string leadbank_gid { get; set; }
        public string drop_remarks { get; set; }
        public string drop_remark1 { get; set; }
    }
    public class GetLeadNoteDetails_list : result
    {

        public string s_no { get; set; }
        public string internal_notes { get; set; }
        public string leadbank_gid { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
}