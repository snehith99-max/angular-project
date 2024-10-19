using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlLawTrnArbitration : result
    {
        public List<arbitration_list> arbitration_list { get; set; }
    }
    public class arbitration_list:result{
        public string arbit_gid { get; set; }
        public string arbitration_gid { get; set; }
        public string arbit_type { get; set; }
        public string arbitration_no { get; set; }
        public string arbitration_date { get; set; }
        public string arbitrator { get; set; }
        public string title { get; set; }
        public string arbit_status { get; set; }
        public string institute_gid { get; set; }
        public string created_date { get; set; }

    }
}