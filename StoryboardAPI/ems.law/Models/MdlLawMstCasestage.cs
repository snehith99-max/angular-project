using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlLawMstCasestage : result
    {
        public List<casestage_list> casestage_list { get; set; }
    }
    public class casestage_list : result
    {

        public string casestage_gid { get; set; }
        public string casestage_code { get; set; }
        public string casestage_name { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }


    }
}