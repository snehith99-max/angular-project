using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.task.Models
{
    public class MdlTskTrnTaskManagement : result
    {
        public List<mdlheir1> mdlheir1 { get; set; }
        public List<memberdropdown_list> memberdropdown_list { get; set; }
        public List<allmember_list> allmember_list { get; set; }
        public List<clientview> clientview { get; set; }
        public List<subfolders> subfolders { get; set; } = new List<subfolders>();
        public string nice_to_have_count { get; set; }
        public string non_mandatory_count { get; set; }
        public string mandatory_count { get; set; }
        public int testmanager_count { get; set; }
        public int member { get; set; }
        public string show_stopper_count { get; set; }
        public int nice_to_count { get; set; }
        public int completed { get; set; }
        public int show_stopper { get; set; }
        public int non_mandatory { get; set; }
        public string row_count { get; set; }
        public int completed_count { get; set; }
        public int holdmanager_count { get; set; }
        public int mandatory { get; set; }
        public int assigned_count { get; set; }
        public int pending_count { get; set; }
        public int progress_count { get; set; }
        public int testing_count { get; set; }
        public int live_count { get; set; }
        public int hold_count { get; set; }
        public int complete_count { get; set; }
        public List<taskpendinglist> taskpending_list { get; set; }
        public List<tasksheet_list> tasksheet_list { get; set; }
        public List<moduledropdown_list> moduledropdown_list { get; set; }

    }
    public class memberdropdown_list
    {
        public string assigned_member { get; set; }
        public string task_gid { get; set; }
        public string assigned_member_gid { get; set; }
    }
    public class clientview
    {
        public string project_gid { get; set; }
        public string project_name { get; set; }
    }
    public class allmember_list
    {
        public string task_count { get; set; }
        public string progress_count { get; set; }
        public string new_count { get; set; }
        public string testing_count { get; set; }
        public string live_count { get; set; }
        public string hold_count { get; set; }
        public string module_name { get; set; }
    }
    public class subfolders : result
    {
        public string hrs_taken { get; set; }
        public string task_type_name { get; set; }
        public string task_gid { get; set; }
        public string module_name { get; set; }
        public string taskstatus { get; set; }
        public string sub_task { get; set; }
        public string task_name { get; set; }
        public string tasksheet_gid { get; set; }
        public string created_by { get; set; }
        public List<subfolders> subfolderslist { get; set; }
    }
    public class mdlheir1 : result
    {
        public string task_date { get; set; }
        public string created_by { get; set; }
        public string task_gid { get; set; }
        public string tasksheet_gid { get; set; }


    }
    public class moduledropdown_list : result
    {
        public string team_name { get; set; }
        public string team_gid { get; set; }
        public string teamname_gid { get; set; }
    }
    public class taskaddlist : result
    {
        public List<subtask> subtask { get; set; }
        public List<documentdata_list> documentdata_list { get; set; }
        public List<assigned_list> assigned_list { get; set; }
        public List<transfer_list> transfer_list { get; set; }
        public List<completedlive_list> completedlive_list { get; set; }
        public List<statuslog_list> statuslog_list { get; set; }
        public string estimated_hours { get; set; }
        public string retesting_flag { get; set; }
        public string tasksheet_gid { get; set; }
        public string task_detail { get; set; }
        public string task_typename { get; set; }
        public string remark { get; set; }
        public string total_member_count { get; set; }
        public string total_manager_count { get; set; }
        public string module_name_gid { get; set; }
        public string assigned_Date { get; set; }
        public string sub_task { get; set; }
        public string severity_gid { get; set; }
        public string hold_flag { get; set; }
        public string actualcompleted_hrs { get; set; }
        public string actualdevelopment_hrs { get; set; }
        public string employee_status { get; set; }
        public string severity_name { get; set; }
        public string task_typegid { get; set; }
        public string client_name { get; set; }
        public string remarks { get; set; }
        public string sheetstatus { get; set; }
        public string hrs_taken { get; set; }
        public string functionality_gid { get; set; }
        public string functionality_name { get; set; }
        public string client_gid { get; set; }
        public string task_date { get; set; }
        public string module_name { get; set; }
        public string module_gid { get; set; }
        public string task_name { get; set; }
        public string task_gid { get; set; }
        public string task_status { get; set; }
        public string task_code { get; set; }
        public string txtremarks { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string completed_date { get; set; }
        public string hold_date { get; set; }
        public string hold_by { get; set; }
        public string completed_by { get; set; }
        public string taskupdated_by { get; set; }
        public string taskupdated_date { get; set; }
        public string completedlive_date { get; set; }
        public string completedlive_by { get; set; }
    }
    public class module_list3: result
    {
        public string team_gid { get; set; }
        public string team_name { get; set; }
    }
    public class depmodule_list : result
    {
        public string team_gid { get; set; }
        public string team_name { get; set; }
    }
    public class depmodule_summary : result
    {
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string routes_status { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string version_number { get; set; }
        public string total_module_count { get; set; }
        public string deployment_trackergid { get; set; }
        public string script_status { get; set; }
        public string file_name { get; set; }
        public string file_gid { get; set; }
        public string dependency_status { get; set; }
    }
    public class scriptattach_file
    {
        public string docupload_type { get; set; }
        public string file_path { get; set; }
        public string script_gid { get; set; }
    }
    public class deploy_module
    {
        public string team_gid { get; set; }
        public string team_name { get; set; }
    }
    public class deploydependcy_module
    {
        public string team_gid { get; set; }
        public string team_name { get; set; }
    }
    public class tracker_list : result
    {
        public List<module_list3> module_list3 { get; set; }
        public List<deploy_module> deploy_module { get; set; }
        public List<deploydependcy_module> deploydependcy_module { get; set; }
        public List<depmodule_list> depmodule_list { get; set; }
        public List<scriptattach_file> scriptattach_file { get; set; }
        public List<depmodule_summary> depmodule_summary { get; set; }
        public string module_name { get; set; }
        public string module_gid { get; set; }
        public string file_gid { get; set; }
        public string version_number { get; set; }
        public string approval_name { get; set; }
        public string dependency_module_gid { get; set; }
        public string version { get; set; }
        public string script_status { get; set; }
        public string dependency_module { get; set; }
        public string routes_status { get; set; }
        public string description { get; set; }
        public string dependency_status { get; set; }
        public string file_name { get; set; }
        public string deployment_trackergid { get; set; }
        public string dll_status { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }

    public class completedlive_list
    {
        public string deployment_date { get; set; }
        public string total_hrs { get; set; }
        public string actualdevelopment_hrs { get; set; }
        public string actualcompleted_hrs { get; set; }
    }
    public class assigned_list
    {
        public string assigned_date { get; set; }
        public string assigned_by { get; set; }
        public string assigned_to { get; set; }
    }
    public class transfer_list
    {
        public string previous_member { get; set; }
        public string reassigned_member { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
    }
    public class statuslog_list
    {
        public string actualdevelopment_hrs { get; set; }
        public string actualcompleted_hrs { get; set; }
        public string update_by { get; set; }
        public string updated_date { get; set; }
        public string remarks { get; set; }
        public string task_status { get; set; }
    }
    public class subtask
    {
        public string subtask_name { get; set; }
        public string hrs_taken { get; set; }
    }
    public class documentdata_list
    {
        public string docupload_name { get; set; }
        public string taskdocumentupload_gid { get; set; }
        public string docupload_type { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string document_name { get; set; }
    }
    public class taskpendinglist
    {
        public string task_name { get; set; }
        public string functionality_name { get; set; }
        public string assigned_member_gid { get; set; }
        public string assigned_date { get; set; }
        public string completedlive_by { get; set; }
        public string time_taken_to_complete { get; set; }
        public string task_gid { get; set; }
        public string update_by { get; set; }
        public string update_date { get; set; }
        public string module_name { get; set; }
        public string actualdevelopment_hrs { get; set; }
        public string actualcompleted_hrs { get; set; }
        public string assigned_Date { get; set; }
        public string employee_status { get; set; }
        public string estimated_hours { get; set; }
        public string client_count { get; set; }
        public string module_gid { get; set; }
        public string severity_name { get; set; }
        public string hold_date { get; set; }
        public string client_name { get; set; }
        public string created_date { get; set; }
        public string days_since_creation { get; set; }
        public string created_by { get; set; }
        public string task_typename { get; set; }
        public string task_code { get; set; }
        public string task_status { get; set; }
        public string assignmanager_name { get; set; }
        public string assigned_member { get; set; }
    }
    public class tasksheet_list
    {
        public string task_name { get; set; }
        public string task_gid { get; set; }
        public string update_by { get; set; }
        public string update_date { get; set; }
        public string task_detail { get; set; }
        public string module_name { get; set; }
        public string status { get; set; }
        public string task_typegid { get; set; }
        public string sub_task { get; set; }
        public string hrs_taken { get; set; }
        public string task_date { get; set; }
        public string module_gid { get; set; }
        public string module_name_gid { get; set; }
        public string created_date { get; set; }
        public string tasksheet_gid { get; set; }
        public string created_by { get; set; }
        public string task_typename { get; set; }
    }
}