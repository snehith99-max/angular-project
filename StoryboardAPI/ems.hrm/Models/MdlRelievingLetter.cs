using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlRelievingLetter : result
    {
        public List<RelievingLetterLists> RelievingLetterLists { get; set; }
        public List<onchangerelieving> onchangerelieving { get; set; }
    }
    public class RelievingLetterLists : result
    {
        public string Branch { get; set; }
        public string Department { get; set; }
        public string releiving_gid { get; set; }
        public string Designation { get; set; }
        public string Employee_Name { get; set; }
    }
    public class onchangerelieving : result
    {
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string employee_name { get; set; }
        public string releiving_gid { get; set; }
        public string Employee_Name { get; set; }
    }
}