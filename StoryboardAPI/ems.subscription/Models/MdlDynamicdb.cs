using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class MdlDynamicdb : result
    {
        public List<Modulelists> Modulelists { get; set; }
        public List<dynamicdblists> dynamicdblists { get; set; }
    }
    public class Modulelists : result
    {
        public string module_name { get; set; }
        public string module_gid { get; set; }
    }
    public class dynamicdblists : result
    {
        public string module_name { get; set; }
        public string company_code { get; set; }
        public string server_name { get; set; }
        public string dynamicdbscriptmanagement_gid { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
}