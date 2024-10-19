using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlUserprivilege : result
    {
        public List<employeelists> employeelists { get; set; }
    }
    public class employeelists : result
    {
        public string employee_name { get; set; }
        public string user_gid { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string user_name { get; set; }
    }
}