using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmTrnDailyAttendance : result
    {
        public List<daily_list1> daily_list1 { get; set; }
    }
    public class daily_list1 : result
    {
        public string date { get; set; }
        public string department_gid { get; set; }
        public string branch_gid { get; set; }
        public string late_hours { get; set; }
        public string permission_totalhours { get; set; }
        public string extra_hours { get; set; }
        public string attendance_status { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string department_name { get; set; }
        public string shift { get; set; }
        public string shift_in { get; set; }
        public string shift_out { get; set; }
        public string login { get; set; }
        public string logout { get; set; }



    }
}