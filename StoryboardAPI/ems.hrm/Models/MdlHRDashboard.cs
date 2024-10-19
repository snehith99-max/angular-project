using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHRDashboard : result
    {
        public List<Leave_HistoryList> Leave_HistoryList { get; set; }
        public List<Login_HistoryList> Login_HistoryList { get; set; }
        public List<Logout_HistoryList> Logout_HistoryList { get; set; }
        public List<od_HistoryList> od_HistoryList { get; set; }
        public List<permission_HistoryList> permission_HistoryList { get; set; }
        public List<Compoff_HistoryList> Compoff_HistoryList { get; set; }  
        public List<TotalActiveEmployeeCount> TotalActiveEmployeeCount { get; set; }
        public List<ToDoCompOffList> ToDoCompOffList { get; set; } 
        public List<ToDoLogoutList> ToDoLogoutList { get; set; } 
        public List<ToDoPermissionList> ToDoPermissionList { get; set; } 
        public List<TotalActiveEmployeeList> TotalActiveEmployeeList { get; set; }
        public List<TodayBirthdayCount> TodayBirthdayCount { get; set; }
        public List<TodayBirthdayList> TodayBirthdayList { get; set; }
        public List<UpcomingBirthdayCount> UpcomingBirthdayCount { get; set; }
        public List<UpcomingBirthdayList> UpcomingBirthdayList { get; set; }
        public List<WorkAnniversaryCount> WorkAnniversaryCount { get; set; }
        public List<WorkAnniversaryList> WorkAnniversaryList { get; set; }
        public List<OnProbationCount> OnProbationCount { get; set; }
        public List<OnProbationList> OnProbationList { get; set; }
        public List<EmpCountbyLocation> EmpCountbyLocation { get; set; }        
        public List<TotalActiveEmployees> TotalActiveEmployees { get; set; }
        public List<ToDoListCount> ToDoListCount { get; set; }
        public List<ToDoList> ToDoList { get; set; }
        public List<ToDoODList> ToDoODList { get; set; } 
        public List<ToDoLoginList> ToDoLoginList { get; set; }
        public List<empStatistics_list> empStatistics_list { get; set; }
        public List<empActivecount_list> empActivecount_list { get; set; }
    }

    public class Leave_HistoryList : result  
    {
        public string leave_status { get; set; } 
        public string employee_details { get; set; } 
        public string leave_gid { get; set; }
    }

    public class Login_HistoryList : result 
    {
        public string employee_name { get; set; }
        public string StatusLogin { get; set; } 
        public string attendancelogintmp_gid { get; set; }
    }

    public class Logout_HistoryList : result 
    {
        public string employee_logoutdetails { get; set; } 
        public string StatusLogout { get; set; } 
        public string attendancetmp_gid { get; set; } 
    }

    public class od_HistoryList : result 
    {
        public string username { get; set; } 
        public string StatusOnduty { get; set; }
        public string ondutytracker_gid { get; set; }
    }

    public class Compoff_HistoryList : result  
    {
        public string employee_details { get; set; } 
        public string StatusCompoff { get; set; }
        public string compensatoryoff_gid { get; set; }
    }

    public class permission_HistoryList : result
    {
        public string employee_name { get; set; }
        public string StatusPermission { get; set; }
        public string permissiondtl_gid { get; set; }
    }
    public class TotalActiveEmployeeCount : result
    {
        public string present_count { get; set; }
        public string absent_count { get; set; }
        public string leave_count { get; set; }
        public string employee_count { get; set; }
    }

    public class ToDoCompOffList : result  
    {
        public string employee_details { get; set; }  
        public string compensatoryoff_gid { get; set; } 
    }

    public class ToDoLogoutList : result  
    {
        public string employee_logoutdetails { get; set; }  
        public string attendancetmp_gid { get; set; }
    }

    public class ToDoODList : result  
    {
        public string username { get; set; } 
        public string ondutytracker_gid { get; set; }
    }
    public class permissionapprove_list : result
    {
        public string permissiondtl_gid { get; set; }
    }

    public class ToDoPermissionList : result  
    {
        public string employee_name { get; set; } 
        public string permissiondtl_gid { get; set; }
    }
    public class approveleave : result
    {
        public string leave_gid { get; set; }
    }
    public class approveOD_list : result
    {
        public string ondutytracker_gid { get; set; }
    }
    public class approvesubmit: result
    {
        public string leave_gid { get; set; }
        public string remarks { get; set; }
    } 
    public class loginapprove: result
    {
        public string attendancelogintmp_gid { get; set; }
       
    }
    public class logoutapprove: result
    {
        public string attendancetmp_gid { get; set; }
       
    }

    public class compoffapprove : result   
    {
        public string compensatoryoff_gid { get; set; } 

    }

    public class TotalActiveEmployeeList : result
    {
        public string employee { get; set; }
        public string login_time { get; set; }
        public string logout_time { get; set; }
        public string employee_attendance { get; set; }
    }
    public class TodayBirthdayCount : result
    {
        public string today_birthdaycount { get; set; }
    }
    public class TodayBirthdayList : result
    {
        public string employee { get; set; }
        public string employee_photo { get; set; }
    }
    public class UpcomingBirthdayCount : result
    {
        public string upcoming_birthdaycount { get; set; }
    }
    public class UpcomingBirthdayList : result
    {
        public string employee { get; set; }
        public string employee_photo { get; set; }
        public string employee_dob { get; set; }
    }
    public class WorkAnniversaryCount : result
    {
        public string workanniversarycount { get; set; }
    }
    public class WorkAnniversaryList : result
    {
        public string employee { get; set; }
        public string employee_photo { get; set; }
        public string designation_name { get; set; }
        public string employee_gender { get; set; }
        public string total_experience { get; set; }
    }
    public class OnProbationCount : result
    {
        public string total_probationemployee { get; set; }
    }
    public class OnProbationList : result
    {
        public string employee { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string probationary_until { get; set; }
    }
    public class EmpCountbyLocation : result
    {
        public string branch_name { get; set; }
        public string employee_count { get; set; }
    }
    public class TotalActiveEmployees : result
    {
        public string employee { get; set; }
        public string designation_name { get; set; }
    }
    public class ToDoListCount : result
    {
        public string pending_leaves { get; set; }
        public string leave_applydate { get; set; }
        public string leave_fromdate { get; set; }
        public string leave_todate { get; set; }
        public string leave_reason { get; set; }
        public string leave_remarks { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string leavetype_name { get; set; }
        public string leave_noofdays { get; set; }
    }
    public class ToDoList : result
    {
        public string leave_details { get; set; }
        public string leave_gid { get; set; }
    
    }

    public class ToDoLoginList : result 
    {
        public string employee_name { get; set; }
        public string attendancelogintmp_gid { get; set; } 
        public string employee_gid { get; set; }
        public string status { get; set; }
        public string attendance_date { get; set; }
        public string login_time { get; set; }
        public string remarks { get; set; }

    }
    public class empStatistics_list
    {
        public string months { get; set; }
        public string emp_joining_count { get; set; }
        public string emp_exit_count { get; set; }
    }
    public class empActivecount_list
    {
        public string months { get; set; }
        public string employee_count { get; set; }
    }
}