using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysMstOriganisationHierarchy : result
    {
        public List<MdlSysMstOriganisationHierarchy> MdlSysMstOriganisationHierarchylist { get; set; }

        public string level_list { get; set; }
    }
}