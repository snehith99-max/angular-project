using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysRptScreenPrivilege : result
    {
        public List<screenprivilegedata_list> screenprivilegedatalist { get; set; }
        public List<GetModuledropdown> GetLevel1Menu { get; set; }
        public List<GetModule2dropdown> GetLevel2Menu { get; set; }
        public List<GetModule3dropdown> GetLevel3Menu { get; set; }
        public string module_name2 { get; set; }
        
    }

    public class screenprivilegedata_list : result 
    {
        public string designation_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string user_code { get; set; }
        public string Employee_name { get; set; }
    }

    public class GetModuledropdown : result
    { 
        public string module_name { get; set; }
        public string module_gid { get; set; }

    }
    public class GetModule2dropdown : result
    {
        public string module_name { get; set; }
        public string module_gid { get; set; }

    }
    public class GetModule3dropdown : result
    {
        public string module_name { get; set; }
        public string module_gid { get; set; }

    }
}