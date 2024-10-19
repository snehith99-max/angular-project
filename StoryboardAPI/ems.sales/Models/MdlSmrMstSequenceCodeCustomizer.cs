using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrMstSequenceCodeCustomizer : result
    {
        public List<SequenceCodeSummary> SequenceCodeSummary { get; set; }
    }
    public class SequenceCodeSummary : result
    {
        public string sequencecodecustomizer_gid { get; set; }
        public string sequence_curval { get; set; }
        public string sequence_code { get; set; }
        public string sequence_name { get; set; }
        public string branch_flag { get; set; }
        public string businessunit_flag { get; set; }
        public string department_flag { get; set; }
        public string year_flag { get; set; }
        public string company_code { get; set; }
        public string delimeter { get; set; }
        public string location_flag { get; set; }
        public string month_flag { get; set; }
        public string runningno_prefix { get; set; }
        public string branch_name { get; set; }
    }
}