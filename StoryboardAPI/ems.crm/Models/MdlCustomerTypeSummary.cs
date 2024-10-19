using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlCustomerTypeSummary : result
    {
        public List<customertypesummary_lists> customertypesummary_lists { get; set; }
    }
    public class customertypesummary_lists : result
    {
        public string customertype_gid { get; set; }
        public string display_name { get; set; }
        public string customer_type { get; set; }
        public string customertype_description { get; set; }
        public string customertype_desc { get; set; }
        public string customertype_gidedit { get; set; }
        public string customer_typeedit { get; set; }
        public string customertype_descriptionedit { get; set; }
        public string status_flag { get; set; }
    }
}