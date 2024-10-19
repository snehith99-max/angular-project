using ems.crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlIndustry : result
    {
        public List<industry_list> industry_list { get; set; }

      


    }
   
    public class industry_list : result
    {
        public string industry_gid { get; set; }
        public string industry_code { get; set; }
        public string industry_name { get; set; }
        public string category_desc { get; set; }
        public string industry_description { get; set; }

        public string created_date { get; set; }
        public string created_by { get; set; }
        public string industryedit_name { get; set; }
        public string industryedit_code { get; set; }
        public string industryedit_description { get; set; }




    }
}