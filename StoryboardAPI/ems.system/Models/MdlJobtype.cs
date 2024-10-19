using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlJobtype : result
    {
        public List<JobtypeLists> JobtypeLists { get; set; }
    }
    public class JobtypeLists : result
    {
        public string Jobtype_gid { get; set; }
        public string JobType_Code { get; set; }
        public string JobType_Name { get; set; }

    }
}