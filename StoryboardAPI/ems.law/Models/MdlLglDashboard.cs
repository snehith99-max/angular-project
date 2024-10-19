using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlLglDashboard:result
    {

        public List<lgldashboard_list> lgldashboard_list { get; set; }
    }


    public class lgldashboard_list : result
    {
        public string casetype_count { get; set; }
        public string institute_count { get; set; }
        public string active_count { get; set; }
        public string Inactive_count { get; set; }
        public string case_week { get; set; }
        public string case_month { get; set; }
        public string case_count { get; set; }
    }
}