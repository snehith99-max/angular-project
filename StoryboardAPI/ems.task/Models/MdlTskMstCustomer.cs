using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.task.Models
{
    public class MdlTskMstCustomer : result
    {
        public List<projectlist> project_list { get; set; }
        public List<customerlist> customer_list { get; set; }
        public List<GetUnassignedmodulelist> GetUnassignedmodule_list { get; set; }
        public List<GetAssignedmodulelist> GetAssignedmodule_list { get; set; }
    }
    public class customerlist : result
    {
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
    }
    public class GetUnassignedmodulelist : result {
        public string project_gid { get; set; }
        public string team_name { get; set; }
        public string team_gid { get; set; }
        public string teamname_gid { get; set; }
    }
    public class GetAssignedmodulelist : result
    {
        public string project_gid { get; set; }
        public string team_name { get; set; }
        public string team_gid { get; set; }
        public string teamname_gid { get; set; }
    }
    public class projectlist : result
    {
        public List<listteam> listteam { get; set; }
        public List<customer_list> customer_list { get; set; }
        public List<client_list> client_list { get; set; }
        public string project_name { get; set; }
        public string project_code { get; set; }
        public string project_gid { get; set; }
        public string team_gid { get; set; }
        public string task_gid { get; set; }
        public string status_log { get; set; }
        public string created_date { get; set; }
        public string projectname_gid { get; set; }
        public string total_module_count { get; set; }
        public string created_by { get; set; }
    }
    public class listteam : result
    {
        public string team_name { get; set; }
        public string team_gid { get; set; }
    }
    public class client_list : result
    {
        public string project_gid { get; set; }
        public string project_name { get; set; }
    }
    public class customer_list : result
    {
        public string project_name { get; set; }
        public string project_gid { get; set; }
    }
    public class mdltaskteam : result
    {
        public List<taskteamlist> team_list { get; set; }
        public List<module_list> module_list { get; set; }
        public string task_total_count { get; set; }
        public int team_count { get; set; }
        public string assigned { get; set; }
    }
    public class module_list : result
    {
        public string team_gid { get; set; }
        public string team_name { get; set; }
        public string team_code { get; set; }
    }
    public class assignman_list : result
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
    }
    public class GetAssignedlist : result
    {
        public string team_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
        public string user_gid { get; set; }
    }
    public class GetUnassignedlist : result
    {
        public string team_gid { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
        public string user_gid { get; set; }
    }
    public class assignmemlist
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
    }
    public class menulevel
    {
        public string module_name { get; set; }
        public string module_gid { get; set; }
    }
    public class chart_count : result
    {
        public string nice_to_have_count { get; set; }
        public string critical_non_mandatory_count { get; set; }
        public string critical_mandatory_count { get; set; }
        public string show_stopper_count { get; set; }
        public string month_name { get; set; }
    }
    public class taskdetail_list
    {
        public string task_name { get; set; }
        public string task_gid { get; set; }
    }
    public class graph_count
    {
        public string created_year { get; set; }
        public string total_count { get; set; }
    }
    public class taskteamlist : result
    {
        public List<assignmemlist> assignmem_list { get; set; }
        public List<menulevel> menulevel { get; set; }
        public List<graph_count> graph_count { get; set; }
        public List<chart_count> chart_count { get; set; }
        public List<taskdetail_list> taskdetail_list { get; set; }
        public List<assignman_list> assignman_list { get; set; }
        public List<GetUnassignedclientlist> GetUnassignedclientlist { get; set; }
        public List<GetAssignedclient_list> GetAssignedclient_list { get; set; }
        public List<GetAssignedlist> GetAssignedlist { get; set; }
        public List<GetUnassignedlist> GetUnassignedlist { get; set; }
        public string team_name { get; set; }
        public string team_code { get; set; }
        public string team_gid { get; set; }
        public string deployment_date { get; set; }
        public string module_gid { get; set; }
        public string total_hrs { get; set; }
        public string assignmanager_name { get; set; }
        public string teamname_gid { get; set; }
        public string process { get; set; }
        public string remarks { get; set; }
        public string estimated_hrs { get; set; }
        public string assigned_member { get; set; }
        public string assigned_member_gid { get; set; }
        public string total_member_count { get; set; }
        public string status_log { get; set; }
        public string employee_status { get; set; }
        public string created_date { get; set; }
        public string task_gid { get; set; }
        public string created_by { get; set; }
        public string total_manager_count { get; set; }
        public string actualcompleted_hrs { get; set; }
        public string actualdevelopment_hrs { get; set; }
    }
    public class result
    {
        public string message { get; set; }
        public bool status { get; set; }
    }
    public class GetUnassignedclientlist
    {
        public string project_name { get; set; }
        public string project_gid { get; set; }
        public string team_gid { get; set; }
    }
    public class GetAssignedclient_list
    {
        public string project_name { get; set; }
        public string project_gid { get; set; }
        public string team_gid { get; set; }
        public string task_gid { get; set; }
    }
}