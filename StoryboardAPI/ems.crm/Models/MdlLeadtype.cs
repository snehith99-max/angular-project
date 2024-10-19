using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlLeadtype :result
    {
        public List<leadtype_lists> leadtype_list { get; set; }
    }
    public class leadtype_lists :result
    {
        public string leadtype_gid { get; set; }
        public string leadtype_name { get; set; }
        public string leadtype_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string leadtype_nameedit { get; set; }
        public string leadtype_codeedit { get; set; }
        public string status_flag { get; set; }
    }
    public class mdlstatus_update : result
    {
        public string leadtype_gid { get; set; }
        public string status_flag { get; set; }
    }
}
