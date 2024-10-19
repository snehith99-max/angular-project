using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using StoryboardAPI.Models;

namespace ems.mobile.Models
{
    public class MdlTicketSummary:result
    {
        public List<ticket_summary> ticket_summary {  get; set; }
        public List<ticket_count> ticket_count {  get; set; }
        public List<ticketsummary_detailview> ticketsummary_detailview {  get; set; }
        public List<ticketsummary_managerassign> ticketsummary_managerassign {  get; set; }
    }
    public class ticket_summary
    {
        public string complaint_date { get; set; }
        public string raised_by { get; set; }
        public string image_path { get; set; }
        public string video_path { get; set; }
        public string location_remarks { get; set; }
        public string complaint_gid { get; set; }
        public string complaint_refno { get; set; }
        public string complaint_title { get; set; }
        public string customer_contactno { get; set; }
        public string complaint_remarks { get; set; }
        public string assign_status { get; set; }
        public string campaign_gid { get; set; }
        public string complaint2campaign_gid { get; set; }


    }
    public class ticket_count
    {
        public string tickets_count { get; set; }
        public string workbin { get; set; }
        public string completed { get; set; }
        public string droped { get; set; }
    }

    public class ticketsummary_detailview
    {
        public string complaint_date { get; set; }
        public string raised_by { get; set; }
        public string image_path { get; set; }
        public string video_path { get; set; }
        public string location_remarks { get; set; }
        public string complaint_gid { get; set; }
        public string complaint_refno { get; set; }
        public string complaint_title { get; set; }
        public string customer_contactno { get; set; }
        public string category_type { get; set; }
        public string complaint_remarks { get; set; }
        public string assign_status { get; set; }
        public string campaign_gid { get; set; }
        public string complaint2campaign_gid { get; set; }


    }

    public class ticketsummary_managerassign
    {
        public string complaint_date { get; set; }
        public string complaint_refno { get; set; }
        public string complaint_title { get; set; }
        public string customer_contactno { get; set; }
        public string assign_status { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string location_name { get; set; }
        public string raised_by { get; set; }
        public string campaign_gid { get; set; }
        public string complaint_remarks { get; set; }
        public string complaint_gid { get; set; }
        public string category_type { get; set; }
        public string imagePath { get; set; }
        public string videopath { get; set; }
        public string contact_name { get; set; }


    }
}