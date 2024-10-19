using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysRptAuditReport : result
    {
        public List<audit_list> auditreport_list { get; set; }
        public List<audithistory_list> audithistoryreport_list { get; set; }
    }

    public class audit_list : result
    {
        public string audit_gid { get; set; }
        public string session_id { get; set; }
        public string user_gid { get; set; }
        public string ipaddress { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string companycode { get; set; }
        public string department_name { get; set; }
        public string logout { get; set; }
        public string full_name { get; set; }
    }
    public class audithistory_list : result
    {
        public string audit_gid { get; set; }
        public string session_id { get; set; }
        public string user_gid { get; set; }
        public string ipaddress { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string companycode { get; set; }
        public string department_name { get; set; }
        public string logout { get; set; }
        public string full_name { get; set; }
    }
}