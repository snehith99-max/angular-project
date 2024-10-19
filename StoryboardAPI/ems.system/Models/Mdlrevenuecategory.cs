using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class Mdlrevenuecategory : result
    {
        public List<revenue_list> revenue_list {  get; set; }
    }
    public class revenue_list : result
    {
        public string revenue_gid { get; set; }
        public string revenue_code { get; set; }
        public string revenue_desc { get; set; }
    }
}