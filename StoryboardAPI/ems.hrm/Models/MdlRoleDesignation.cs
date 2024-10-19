using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlRoleDesignation : result
    {
        public List<RoleDesignationLists> RoleDesignationLists { get; set; }
        public List<rolelists> rolelists { get; set; }
        public List<designationlists> designationlists {  get; set; }
    }
    public class RoleDesignationLists : result
    {
        public string Role_Name { get; set; }
        public string Designation_Code { get; set; }
        public string Designation_Name { get; set; }
        public string TotalNoofEmployee { get; set; }
        public string role_gid { get; set; }
        public string designation_gid { get; set; }
         public string Designation_code_manual { get; set; }
        public string Code_Generation { get; set; }


    }
    public class rolelists : result
    {
        public string role_name { get; set; }
        public string role_gid {  get; set; }
    }

    public class designationlists : result
    {
        public string Role_Name { get; set; }
        public string Designation_Code { get; set; }
        public string Designation_Name { get; set; }

    }
}