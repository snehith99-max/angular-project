using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysYearEndClose : result
    {
        public List<GetYearEndDetails_list> GetYearEndDetails_list { get; set; }

    }
    public class GetYearEndDetails_list : result
    {
        public string finyear_gid { get; set; }
        public string fyear_start { get; set; }
        public string start_year { get; set; }
        public string end_year { get; set; }
        public string fyear_end { get; set; }
        public string yearendactivity_flag { get; set; }
        public string unauditedclosing_flag { get; set; }
        public string auditedclosing_flag { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
}