using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlBusinessVertical : result
    {
        public List<businessvertical_summary> businessvertical_summary { get; set; }
    }
    public class businessvertical_summary : result
    {
        public string businessvertical_gid { get; set; }
        public string business_vertical { get; set; }
        public string businessvertical_desc { get; set; }
        public string businessvertical_gidedit { get; set; }
        public string businessvertiacal_edit { get; set; }
        public string businessvertical_descedit { get; set; }
        public string status_flag { get; set; }
    }
}