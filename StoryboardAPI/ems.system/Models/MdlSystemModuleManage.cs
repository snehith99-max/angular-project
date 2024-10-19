using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{

    public class mdlModuleList : result
    {
        public List<mdlModuleDtl> mdlModuleDtl { get; set; }
        public List<Approvalsummary> Approvalsummary { get; set; }
        public List<Approvalsubmit> Approvalsubmit { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public string level_list { get; set; }
        public List<usergrouptemplist> usergrouptemplist { get; set; }

    }
    //public class Hierarchylevel
    //{
    //    public string level_list { get; set; }
      
    //}
    public class mdlModuleDtl
    {
        public string module_gid { get; set; }
        public string module_name { get; set; }
        public string module_code { get; set; }
        public string module_manager { get; set; }
        public string employee_total { get; set; }
        public string modulemanager_gid { get; set; }
    }
    public class Approvalsubmit
    {
        public string module_gid { get; set; }
        public string approval_type { get; set; }
        public string module_name { get; set; }
        public string approval_limit { get; set; }
        public string modulemanager_gid { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
    }
    public class submoudule_list : result
    {

        public string module_gid { get; set; }
        public string module_name { get;set; }
        public string approval_type { get; set; }
        public List<employeelist> employeelist { get; set; }

    }

    public class mdlManagerAssignDtl : result
    {
        public string module_gid { get; set; }
        public string modulemanager_gid { get; set; }
    }
    public class Approvalsummary 
    {
        public string module_gid { get; set; }
        public string module_name { get; set; }
        public string approval_type { get; set; }
        public string approval_limit { get; set; }
    }

    public class mdlModuleAssignedList : result
    {
        public List<mdlModuleAssigneddtl> mdlModuleAssigneddtl { get; set; }
        public List<mdlModuleHierarchy> mdlModuleHierarchy { get; set; }
    }
    public class mdlModuleAssigneddtl
    {
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string user_status { get; set; }
        public string employee_gid { get; set; }
        public string user_gid { get; set; }
        public string menuaccess { get; set; }
    }

    public class mdlModuleHierarchy
    {
        public string user_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class mdlModuleemployeedtl : result
    {
        public string module_gid { get; set; }
        public string assign_hierarchy { get; set; }
        public List<Mdlassignemployeelist> Mdlassignemployeelist { get; set; }
        public string employee_gid { get; set; }
        public string usergrouptemplate_gid { get; set; }
        public string user_gid { get; set; }
    }
    public class Mdlassignemployeelist
    {
        public string employee_gid { get; set; }
    }
    public class MdlSelectedModule : result
    {
        public string module_gid { get; set; }
        public string module_parentgid { get; set; }
        public string user_gid { get; set; }
    }
    public class usergrouptemplist : result
    {
        public string usergrouptemplate_gid { get; set; }
        public string usergrouptemplate_code { get; set; }
        public string usergrouptemplate_name { get; set; }
    }
}
