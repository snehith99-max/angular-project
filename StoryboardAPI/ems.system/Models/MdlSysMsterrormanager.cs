using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysMsterrormanager:result
    {
        public List<errorcount> errorcount { get; set; }
        public List<errorsubmit> errorsubmit{ get; set; }
    }

    public class errorcount : result
    {
        public string error_gid { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }
        public string error_type { get; set; }
   
    }
    public class errorsubmit : result
    {
        public string error_gid { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }
        public string error_type { get; set; }
        public string ref_no { get; set; }
   
    }

}