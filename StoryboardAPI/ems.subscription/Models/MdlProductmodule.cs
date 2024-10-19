using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class MdlProductmodule : result
    {
         public List<productlists> productlists { get; set; }

    }
    public class productlists : result
    {

        public string productmodule_name { get; set; }
        public string module_gid { get; set; }
        public string productmodule_gid { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
    public class productmodulelists : result
    {
        public string productmodule_name { get; set; }
    }
}