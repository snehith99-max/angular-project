using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmMstConfig
    {
        public class hrmconfiglist : result
        {
            public string totalshifthours { get; set; }
            public string halfmineligiblehours { get; set; }
            public string halfmaxeligiblehours { get; set; }
            public string weekoff_salary { get; set; }
            public string holiday_salary { get; set; }
            public string totalpermissionallowed { get; set; }
            public string otavailed { get; set; }
            public string otminhours { get; set; }
            public string otmaxhours { get; set; }
            public string active_flag { get; set; }
            public string allowed_leave { get; set; }
            public string attendance_allowance_flag { get;set; }
            public string allowance_amount { get; set; }
        }
    }
}