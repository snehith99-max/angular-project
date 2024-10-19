using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlConstitution : result
    {
        public List<constitution_list> constitutionlist { get; set; }

    }

    public class constitution_list : result
    {
        public string Status { get; set; }
        public string status_log { get; set; }
        public string constitution_gid { get; set; }
        public string constitution_name { get; set; }
        public string constitution_code { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
    }

    public class Constitutionstatus : result
    {
        public char rbo_status { get; set; }
        public string constitution_gid { get; set; }
        public string constitution_code { get; set; }
        public string constitution_name { get; set; }
        public string remarks { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
    }
    public class ConstitutionInactiveHistory : result
    {
        public List<ConstitutionInactiveHistory_list> ConstitutionInactiveHistorylist { get; set; }
    }
    public class ConstitutionInactiveHistory_list
    {
        public string status { get; set; }
        public string status_log { get; set; }
        public string remarks { get; set; }
        public string updated_by { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string constitution_name { get; set; }
        public string updated_date { get; set; }
    }
    public class mdConstitutionstatus : result
    {
        public string constitution_gid { get; set; }
        public string status_flag { get; set; }



    }
}
