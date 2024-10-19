using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsMstLocation:result
    {
        public List<locationsummary_list> locationsummary_list { get; set; }
        public List<locationbranch_list> locationbranch_list { get; set; }
        public List<locationadd_list> locationadd_list { get; set; }
    }


    public class locationsummary_list : result
    {
        public string branch_gid { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string location_code { get; set; }
        public string branch_name { get; set; }
    }

    public class locationbranch_list : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }
    public class locationadd_list : result
    {
        public string branch_gid { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string location_code { get; set; }
        public string branch_name { get; set; }
    }
}