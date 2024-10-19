using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlLeaveTypeGrade
    {
        public List<Leavetype_list> Leavetype_list { get; set; }
        public List<Addleave_list> Addleave_list { get; set; }

        public string message { get; set; }
        public bool status { get; set; }
    }
    public class Leavetype_list : result
    {
        public string leavetype_gid { get; set; }
        public string user_name { get; set; }
        public string leavetype_code { get; set; }
        public string leavetype_count { get; set; }
        public string consider_as { get; set; }
        public string leavetypestatus { get; set; }
        public string leavetype_name { get; set; }

    }
    public class Addleave_list : result
    {
        public string leavetype_gid { get; set; }
        public string leavetype_code { get; set; }
        public string leave_name { get; set; }
        public string Status_flag { get; set; }
        public string Consider_as { get; set; }
        public string weekoff_consider { get; set; }
        public string holiday_consider { get; set; }
        public string carry_forward { get; set; }
        public string Accured_type { get; set; }
        public string negative_leave { get; set; }
        public string Leave_Days { get; set; }
        public string Code_Generation { get; set; }
        public string leave_code_manual { get; set; }

    }


}