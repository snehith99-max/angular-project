using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class Mdlscriptmanagement : result
    {
        public List<serverlists> serverlists { get; set; }
        public string company_code { get; set; }
        public string server_gid { get; set; }
        public string server_name { get; set; }
        public List<string> productmodule_name { get; set; }
        //public string productmodule_name { get; set; }

        public string module_name { get; set; }
        public string database_name { get; set; }

        public string dynamicdbscriptmanagement_gid { get; set; }
    }
    public class serverlists : result
    {
        public string company_code { get; set; }
        public string server_gid { get; set; }
        public string server_name { get; set; }
        public string file_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string file_path { get; set; }
        public string script_name { get; set; }
        public string execute_query { get; set; }
        public string dbscriptmanagementdocument_gid { get; set; }
    }

   
}