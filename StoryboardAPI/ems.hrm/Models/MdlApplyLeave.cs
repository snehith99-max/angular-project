﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{

    public class Mdlapplyleave : result
    {
        public List<leave_type_list> leave_type_list { get; set; }
        public List<applyleavecountlist> Applyleavecountlist { get; set; }

       


    }

    public class leave_type_list : result
    {
        public string leavetype_name { get; set; }
        public string leavetype_gid { get; set; }

    }
    public class leavecountdetails : result
    {
        public List<leavetype_list> leavetype_list { get; set; }
    }
    public class leavetype_list
    {
        public Double count_leavetaken { get; set; }
        public Double count_leaveavailable { get; set; }
        public string leavetype_gid { get; set; }
        public string leavetype_name { get; set; }
        public string lsapply_leave { get; set; }
    }
    public class applyleavedetails : result
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string leavetype_gid { get; set; }
        public string leave_period { get; set; }
        public string leave_reason { get; set; }
        public string leavetype_name { get; set; }
        public DateTime leave_from { get; set; }
        public DateTime leave_to { get; set; }
        public string leavetypegidfrodocupload { get; set; }
        public Double leave_days { get; set; }
        public string approval_status { get; set; }
        public string approved_by { get; set; }
        public string leave_count { get; set; }
        public string leave_session { get; set; }
        public string leave_gid { get; set; }
        public string approval_remarks { get; set; }
        public string approve_flag { get; set; }
        public List<applyleavegetleavedetails> filename_list { get; set; }
    }
    public class applyleavegetleavedetails : result
    {
        public List<leave_list1> leave_list { get; set; }

    }
    public class leave_list1 : result
    {
        public string leave_gid { get; set; }
        public string leavetype_gid { get; set; }
        public string leave_applydate { get; set; }
        public string leavetype_name { get; set; }
        public string leave_from { get; set; }
        public string leave_to { get; set; }
        public string noofdays_leave { get; set; }
        public string leave_reason { get; set; }
        public string approval_status { get; set; }
        public string approved_by { get; set; }
        public string applied_by { get; set; }
        public string document_name { get; set; }
    }
    // Login Approval....//
    public class applyleavegetlogindetails : result
    {
        public List<loginpending_list> loginpending_list { get; set; }
        public List<loginrejected_list> loginrejected_list { get; set; }
    }
    public class applyleaveloginpending_list : result
    {
        public string employee_name { get; set; }
        public string attendancelogintmp_gid { get; set; }
        public string loginapply_date { get; set; }
        public string loginattendence_date { get; set; }
        public string login_time { get; set; }
        public string login_reason { get; set; }
        public string login_status { get; set; }
        public string apply_employeegid { get; set; }
    }
    public class applyleaveloginrejected_list : result
    {
        public string employee_name { get; set; }
        public string attendancelogintmp_gid { get; set; }
        public string loginapply_date { get; set; }
        public string loginattendence_date { get; set; }
        public string login_time { get; set; }
        public string login_reason { get; set; }
        public string login_status { get; set; }
    }
    public class applyleaveapprovelogin : result
    {
        public string attendancelogintmp_gid { get; set; }
        public string loginattendence_date { get; set; }
        public string apply_employeegid { get; set; }
    }
    // Logout Approval....//
    public class applyleavegetlogoutdetails : result
    {
        public List<logoutpending_list> logoutpending_list { get; set; }
        public List<logoutrejected_list> logoutrejected_list { get; set; }
    }
    public class applyleavelogoutpending_list : result
    {
        public string employee_name { get; set; }
        public string attendancelogouttmp_gid { get; set; }
        public string logoutapply_date { get; set; }
        public string logoutattendence_date { get; set; }
        public string logout_time { get; set; }
        public string logout_reason { get; set; }
        public string logout_status { get; set; }
        public string apply_employeegid { get; set; }
    }
    public class applyleavelogoutrejected_list : result
    {
        public string employee_name { get; set; }
        public string attendancelogouttmp_gid { get; set; }
        public string logoutapply_date { get; set; }
        public string logoutattendence_date { get; set; }
        public string logout_time { get; set; }
        public string logout_reason { get; set; }
        public string logout_status { get; set; }
    }
    public class applyleaveapprovelogout : result
    {
        public string attendancelogouttmp_gid { get; set; }
        public string logoutattendence_date { get; set; }
        public string apply_employeegid { get; set; }
    }
    // OD Approval......//
    public class applyleavegetODdetails : result
    {
        public List<ODpending_list> ODpending_list { get; set; }
        public List<ODrejected_list> ODrejected_list { get; set; }
    }
    public class applyleaveODpending_list : result
    {
        public string employee_name { get; set; }
        public string ondutytracker_gid { get; set; }
        public string onduty_date { get; set; }
        public string onduty_from { get; set; }
        public string onduty_to { get; set; }
        public string onduty_duration { get; set; }
        public string created_date { get; set; }
        public string onduty_reason { get; set; }
        public string ondutytracker_status { get; set; }
        public string apply_employeegid { get; set; }
    }
    public class ODrejected_list : result
    {
        public string employee_name { get; set; }
        public string ondutytracker_gid { get; set; }
        public string onduty_date { get; set; }
        public string onduty_from { get; set; }
        public string onduty_to { get; set; }
        public string onduty_duration { get; set; }
        public string created_date { get; set; }
        public string onduty_reason { get; set; }
        public string ondutytracker_status { get; set; }
    }
    public class applyleaveapproveOD : result
    {
        public string ondutytracker_gid { get; set; }

    }
    // Permission Approval......//
    public class applyleavegetpermissiondetails : result
    {
        public List<permissionpending_list> permissionpending_list { get; set; }
        public List<permissionrejected_list> permissionrejected_list { get; set; }
    }
    public class applyleavepermissionpending_list : result
    {
        public string employee_name { get; set; }
        public string permission_gid { get; set; }
        public string permissiondtl_gid { get; set; }
        public string permission_date { get; set; }
        public string permission_from { get; set; }
        public string permission_to { get; set; }
        public string permission_duration { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string permission_createddate { get; set; }
    }
    public class permissionrejected_list : result
    {
        public string employee_name { get; set; }
        public string permission_gid { get; set; }
        public string permission_date { get; set; }
        public string permission_from { get; set; }
        public string permission_to { get; set; }
        public string permission_duration { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string permission_createddate { get; set; }
    }
    // CompOff Approval......//
    public class applyleavegetcompoffdetails : result
    {
        public List<compoffpending_list> compoffpending_list { get; set; }
        public List<compoffrejected_list> compoffrejected_list { get; set; }
    }
    public class applyleavecompoffpending_list : result
    {
        public string employee_name { get; set; }
        public string compensatoryoff_gid { get; set; }
        public string Compoff_from { get; set; }
        public string Compoff_to { get; set; }
        public string Compoff_duration { get; set; }
        public string Compoff_reason { get; set; }
        public string Compoff_status { get; set; }
    }
    public class compoffrejected_list : result
    {
        public string employee_name { get; set; }
        public string compensatoryoff_gid { get; set; }
        public string Compoff_from { get; set; }
        public string Compoff_to { get; set; }
        public string Compoff_duration { get; set; }
        public string Compoff_reason { get; set; }
        public string Compoff_status { get; set; }
    }
    public class applyleaveapprovecompoff : result
    {
        public string compensatoryoff_gid { get; set; }
    }

    public class monthwise_leavereport : result
    {
        public string response { get; set; }
     
        //public List<leavereport_list> leavereport_list { get; set; }
    }
    public class leavereport_list : result
    {
        public string duration { get; set; }
        public string leavetype_name { get; set; }
        public string leavetype_count { get; set; }
    }
    public class leaecheecklist : result
    {
        public string leave_to { get; set; }
        public string leave_from { get; set; }
        public string radioSelected { get; set; }
        public string leavetype_gid { get; set; }

    }


    public class applyleaveuploaddocument : result
    {
        public string leavetype_gid { get; set; }
        public string leave_reason { get; set; }
        public DateTime leave_from { get; set; }
        public DateTime leave_to { get; set; }
        public Double noofdays_leave { get; set; }
        public List<applyleaveuploaddocumentlist> filename_list { get; set; }
    }
    public class applyleaveuploaddocumentlist : result
    {
        public string tmpdocument_gid { get; set; }
        public string documentname { get; set; }
        public string filename { get; set; }
        public string path { get; set; }
        public string created_date { get; set; }
        public string uploaded_by { get; set; }
    }
    public class mdlleavevalidate : result
    {
        public DateTime leave_from { get; set; }
        public DateTime leave_to { get; set; }
        public double leave_days { get; set; }
        public string leave_gid { get; set; }
        public string leave_session { get; set; }
    }

    public class applyleavecountlist : result
    {
        
        public string leavetype_name { get; set; }
        public double total_leave { get; set; }
        public double available_leavecount { get; set; }
        public double leave_taken { get; set; }
        public string lsleavetype_gid { get; set; }
       
    }
    public class Leavedelete : result
    {

        public string leave_gid { get; set; }
        

    }
}
