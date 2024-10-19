using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmtrnExitRequisition : result
    {
        public List<GetExitEmployee> GetExitEmployee { get; set; }
        public List<exitrequisition_list> exitrequisition_list { get; set; }
    }

    public class GetExitEmployee : result
    {
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string designation_name { get; set; }
    }

    public class exitrequisition_list : result
    {
        public string employee_gid { get; set; }
        public string created_date { get; set; }
        public string relieving_date { get; set; }
        public string reason { get; set; }
        public string exitemployee_gid { get; set; }
        public string employee_name { get; set; }
        public string user_code { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string overall_status { get; set; }
        public string designation_name { get; set; }
        public string remarks { get; set; }
      
    }

}