using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class mdlrolegrade : result
    {
        public List<RoleGradeLists> RoleGradeLists { get; set; }

    }
    public class RoleGradeLists : result
    {
        public string gradelevel_gid { get; set; }
        public string gradelevel_code { get; set; }
        public string gradelevel_name { get; set; }
    }
    public class RoleGradeList : result
    {
        public string rolegradecode { get; set; }
        public string RoleGradename { get; set; }
        public string gradelevel_gid { get; set; }
        public string role_code_manual { get;set; } 
        public string Code_Generation { get;set; }
    }
}