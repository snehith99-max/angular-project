using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysMstDesignation : result
    {
        public List<Designation_list> Designation_list { get; set; }
        public string result { get; set; }
    }
    public class Designation_list : result
    {
        public string designation_gid { get; set; }
        public string designation_code { get; set; }
        public string designation_name { get; set; }
        public string designation_status { get; set; }
        public string created_date { get; set; }
        public string designationStatus { get; set; }
        public string created_by { get; set;}
        public string designation_remarks { get; set;}
        public string Code_Generation { get; set;}
        public string designation_code_manual { get; set;}
    }
}