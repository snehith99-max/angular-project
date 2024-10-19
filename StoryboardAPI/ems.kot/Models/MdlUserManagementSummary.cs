using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.kot.Models
{
    public class MdlUserManagementSummary : result
    {
        public List<user_list> user_list { get; set; }
        public List<usersummary_list> usersummary_list { get; set; }
        public List<userviewsummary_list> userviewsummary_list { get; set; }
    }
    public class user_list : result
    {
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string branchname { get; set; }
        public string entityname { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public string first_name { get; set; }
        public string mobile { get; set; }
        public string user_type { get; set; }

    }


    public class usersummary_list : result
    {
        public string user_gid { get; set; }
        public string entity_name { get; set; }
        public string user_name { get; set; }
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
        public string user_type { get; set; }
        public string branch_gid { get; set; }
        public string user_code { get; set; }
    }

    public class userviewsummary_list : result
    {
        public string employee_gid { get; set; }
        public string branch_gid { get; set; }
        public string user_firstname { get; set; }
        public string entity_gid { get; set; }
        public string user_code { get; set; }
        public string branchname { get; set; }
        public string entityname { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public string user_name { get; set; }
        public string mobile { get; set; }
        public string user_type { get; set; }
        public string user_gid { get; set; }
        public string employee_mobileno { get; set; }
        public string entity_name { get; set; }


    }
}