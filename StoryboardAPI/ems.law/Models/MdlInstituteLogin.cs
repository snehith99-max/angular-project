using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlInstituteLogin :  result
    {
        public List<Institutename_list> Institutename_list { get; set; }
    }
        public class PostInstituteLogin
        {
            public string institute_code { get; set;}
            public string company_code { get; set;}
            public string user_password { get; set;}
        }
        public class InstituteLoginResponse
        {
        public string dashboard_flag { get; set; }
        public string token { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string institute_gid { get; set; }
        public string username { get; set; }
        public string institute_code { get; set; }
        public string c_code { get; set; }
        public string sref { get; set; }
        }
        public class Institutename_list : result    
        {
            public string name { get; set; }
        }
        public class result
        {
            public bool status { get; set; }
            public string message { get; set; }


        }
    
}