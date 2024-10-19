using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlWorktype :result
    {
        public List<WorktypeLists> WorktypeLists { get; set; }
    }
    public class WorktypeLists : result
    {
        public string Worktype_gid { get; set; }
        public string WorkType_Code { get; set; }
        public string WorkType_Name { get; set; }

    }
}