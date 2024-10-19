using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlLawMstCasetype : result
    {
        public List<case_list> case_list { get; set; }
    }
    public class case_list : result
    {

        public string casetype_gid { get; set; }
        public string casetype_code { get; set; }
        public string casetype_name { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }


    }
}