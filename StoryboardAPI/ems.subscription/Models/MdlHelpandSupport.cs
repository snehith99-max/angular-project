using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class MdlHelpandSupport:result
    {
        public List<SupportLists> SupportLists { get; set; }
    }
    public class SupportLists : result
    {
        public string module_name { get; set; }
        public string Module_name { get; set; }
        public string company_name { get; set; }
        public string contact_number { get; set; }
        public string mail_id { get; set; }
        public string screen_name { get; set; }
        public string description { get; set; }
        public string Description { get; set; }
    }
    public class Posthelpandsupport: result
    { 
    }
    }