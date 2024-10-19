using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models

{
    public class MdlCallResponse : result
    {
        public List<call_lists> call_lists { get; set; }
        public List<leadstagedropdown_list> leadstagedropdown_list { get; set; }
    }
    public class call_lists : result
    {
        public string callresponse_gid { get; set; }
        public string call_response { get; set; }
        public string moving_stage { get; set; }
        public string moving_stagename { get; set; }
        public string callresponse_code { get; set; }
        public string active_flag { get; set; }
        public string leadstage_gid { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string callresponseedit_name { get; set; }
        public string movingstage_edit { get; set; }
        public string message { get; set; }
        public bool status { get; set; }

    }
     public class leadstagedropdown_list 
    {
        public string leadstage_gid { get; set; }
        public string leadstage_name { get; set; }

    }

}