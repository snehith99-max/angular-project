using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class Mdlrevenuecategory:result
    {
        public List<revenue_list1> revenue_list1 { get; set; }
    }
    public class revenue_list1 : result
    {
        public string revenue_gid { get; set; }
        public string revenue_code { get; set; }
        public string revenue_desc { get; set; }
        public string revenue_name { get; set; }
    }

    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }


    }
}