using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmTrnAttendanceDaily:result
    {
        public List<daily_list> daily_list { get; set; }
    }
    public class daily_list : result
    {
        public string late_hours { get; set; }
        public string permission_totalhours { get; set; }
        public string extra_hours { get; set; }
        public string attendance_status { get; set; }
        public string status { get; set; }

    }
}