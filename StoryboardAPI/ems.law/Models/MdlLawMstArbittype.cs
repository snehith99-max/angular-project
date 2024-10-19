using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlLawMstArbittype : result
    {
        public List<arbit_list> arbit_list { get; set; }
    }
        public class arbit_list : result
        {

            public string arbit_gid { get; set; }
            public string arbit_code { get; set; }
            public string arbit_type { get; set; }
            public string created_date { get; set; }
            public string created_by { get; set; }


        }
    
}