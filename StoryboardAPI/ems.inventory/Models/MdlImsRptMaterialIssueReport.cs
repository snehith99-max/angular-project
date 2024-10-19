using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptMaterialIssueReport : result
    {
        public List<materialissue_list> materialissue_list { get; set; }
    }
    public class materialissue_list : result
    {
        public string materialissued_gid { get; set; }
        public string materialrequisition_gid { get; set; }
        public string materialissued_date { get; set; }
        public string materialissued_status { get; set; }
        public string issued_to { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string materialrequisition_reference { get; set; }
       
    }
}