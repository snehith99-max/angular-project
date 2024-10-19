using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmTrnForm25 : result
    {
        public List<month_list> month_list { get; set; }
        public List<form253mployee_list> form253mployee_list { get; set; }

    }
    public class month_list : result
        {
            public string month { get; set; }
            public string year { get; set; }
        }
    public class form253mployee_list : result
    {
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string branch_gid { get; set; }
        public string department_gid { get; set; }
    }
    public class formdetailslist : result
    {
        public string branch_gid { get; set; }
        public string department_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }

}