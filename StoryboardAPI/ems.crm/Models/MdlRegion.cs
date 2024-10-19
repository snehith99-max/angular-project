using ems.system.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlRegion : result
    {
        public List<region_lists1> region_lists1 { get; set; }

    }
    public class region_lists1 : result
    {
        public string region_gid { get; set; }
        public string region_name { get; set; }
        public string region_code { get; set; }
        public string city_name { get; set; }
        public String Status { get; set; }

        public string region_name_edit { get; set; }
        public string region_code_edit { get; set; }
        public string city_name_edit { get; set; }
        public String created_by { get; set; }
        public String created_date { get; set; }
       

    }
    public class mdregionstatus : result
    {
        public string region_gid { get; set; }

        public string status_flag { get; set; }
    }
}