using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysMstUserGroupTemp : result
    {
        public string module_gid { get; set; }
        public string module_parentgid { get; set; }
        public string menu_level { get; set; }
        public string user_group_temp_code { get; set; }
        public string usergrouptemplate_gid { get; set; }
        public string user_group_temp_name { get; set; }
        public string usergrouptemplate_name { get; set; }
        public string usergrouptemplate_code { get; set; }

        public List<MdlSysMstUserGroupList> MdlSysMstUserGroupList { get; set; }
        public List<sys_menu> menu_list { get; set; }
        public List<MdlEditUserGroupTempListNew> MdlEditUserGroupTempListNew { get; set; } 
        public List<MdlSysMstUpdateUserGroupList> MdlSysMstUpdateUserGroupList { get; set; } 



        public string result { get; set; }
    }

    public class MdlSysMstUserGroupList
    {

        public string usergrouptemplate_gid { get; set; }
        public string created_date { get; set; }
        public string usergrouptemplate_code { get; set; }
        public string usergrouptemplate_name { get; set; }
        public string created_by { get; set; }
        public string UsergroupStatus { get; set; }
        public string module_gid { get; set; }
        public string module_name { get; set; }
        public string menu_level { get; set; }
        public string module_gid_parent { get; set; }
        public string display_order { get; set; }
        public string menu_access { get; set; }
        public string menu_list { get; set; }
    }

    public class MdlEditUserGroupTempListNew : result
    {
        public string usergrouptemplate_gid { get; set; }
        public string usergrouptemplate_code { get; set; }
        public string usergrouptemplate_name { get; set; }
        public string menu_list { get; set; }
    }

    public class MdlSysMstUpdateUserGroupList
    {

        public string usergrouptemplate_gid { get; set; }
        public string created_date { get; set; }
        public string usergrouptemplate_code { get; set; }
        public string usergrouptemplate_name { get; set; }
        public string created_by { get; set; }
        public string UsergroupStatus { get; set; }
        public string module_gid { get; set; }
        public string module_name { get; set; }
        public string menu_level { get; set; }
        public string module_gid_parent { get; set; }
        public string display_order { get; set; }
        public string menu_access { get; set; }
        public string menu_list { get; set; }
    }
}