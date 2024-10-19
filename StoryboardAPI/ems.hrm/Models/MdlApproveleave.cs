﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{

    public class approvalcountdetails : result
    {
        public string count_approvalpending { get; set; }
        public string count_approval { get; set; }
        public string count_rejected { get; set; }
        public string count_history { get; set; }

        public string approved_leave { get; set; }
        public string approved_login { get; set; }
        public string approved_logout { get; set; }
        public string approved_onduty { get; set; }
        public string approved_compoff { get; set; }
        public string approved_permission { get; set; }

        public string rejected_leave { get; set; }
        public string rejected_login { get; set; }
        public string rejected_logout { get; set; }
        public string rejected_onduty { get; set; }
        public string rejected_compoff { get; set; }
        public string rejected_permission { get; set; }

        public string pending_leave { get; set; }
        public string pending_login { get; set; }
        public string pending_logout { get; set; }
        public string pending_onduty { get; set; }
        public string pending_compoff { get; set; }
        public string pending_permission { get; set; }
    }

    public class approvelogin : result
    {
        public string loginattendence_date { get; set; }
        public string attendancelogintmp_gid { get; set; }
        public string apply_employeegid { get; set; }
    }

    public class uploaddocuments : result
    {
        public List<uploaddocumentlist> uploaddocumentlist { get; set; }

    }

    public class uploaddocumentlist
    {
        public string documentname { get; set; }
        public string path { get; set; }
        public string tmpdocument_gid { get; set; }
       
    }
    
    public class getcompoffdetails : result
    {
        public List<compoffpending_list> compoffpending_list { get; set; }

    }

    public class compoffpending_list
    {
        public string compensatoryoff_gid { get; set; }
        public string Compoff_from { get; set; }
        public string Compoff_to { get; set; }
        public string Compoff_duration { get; set; }
        public string Compoff_reason { get; set; }
        public string Compoff_status { get; set; }
        public string employee_name { get; set; }
    }

    public class approvecompoff : result
    {
        public string compensatoryoff_gid { get; set; }

    }


    public class approveOD : result
    {
        public string ondutytracker_gid { get; set; }
       
    }

    public class getpermissiondetails : result
    {
        public List<permissionpending_list> permissionpending_list { get; set; }

    }

    public class permissionpending_list
    {
        public string permission_gid { get; set; }
        public string permissiondtl_gid { get; set; }
        public string permission_date { get; set; }
        public string permission_from { get; set; }
        public string permission_to { get; set; }
        public string permission_duration { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string employee_name { get; set; }
        public string permission_createddate { get; set; }

    }


    public class getlogoutdetails : result
    {
        public List<logoutpending_list> logoutpending_list { get; set; }
        public List<logoutrejected_list> logoutrejected_list { get; set; }
    }

    public class getODdetails : result
    {
        public List<ODpending_list> ODpending_list { get; set; }
    }

    public class ODpending_list
    {
        public string employee_gid { get; set; }
        public string createddate { get; set; }
        public string ondutydate { get; set; }
        public string onduty_fromtime { get; set; }
        public string onduty_totime { get; set; }
        public string ondutytracker_gid { get; set; }
        public string created_date { get; set; }
        public string onduty_date { get; set; }
        public string onduty_from { get; set; }
        public string onduty_to { get; set; }
        public string onduty_duration { get; set; }
        public string onduty_reason { get; set; }
        public string ondutytracker_status { get; set; }
        public string employee_name { get; set; }
        public string apply_employeegid { get; set; }
    }
    
    public class approvelogout : result
    {
        public string attendancelogouttmp_gid { get; set; }
        public string logoutattendence_date { get; set; }
        public string apply_employeegid { get; set; }
    }

    public class logoutpending_list
    {
        public string attendancelogouttmp_gid { get; set; }
        public string logoutapply_date { get; set; }
        public string employee_name { get; set; }
        public string logoutattendence_date { get; set; }
        public string logout_time { get; set; }
        public string logout_reason { get; set; }
        public string logout_status { get; set; }
        public string apply_employeegid { get; set; }
    }

    public class logoutrejected_list
    {
        public string attendancelogouttmp_gid { get; set; }
        public string logoutapply_date { get; set; }
        public string employee_name { get; set; }
        public string logoutattendence_date { get; set; }
        public string logout_time { get; set; }
        public string logout_reason { get; set; }
        public string logout_status { get; set; }
        public string apply_employeegid { get; set; }
    }



    public class approveleavedetails : result
    {
        public string leave_gid { get; set; }
        public string approval_remarks { get; set; }
    }

    public class getlogindetails : result
    {
        public List<loginpending_list> loginpending_list { get; set; }
        public List<loginrejected_list> loginrejected_list { get; set; }
    }

    public class loginpending_list
    {
        public string employee_gid { get; set; }
        public string applydate { get; set; }
        public string employeename { get; set; }
        public string attendancelogintmp_gid { get; set; }
        public string attendence_date { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }
        public string loginapply_date { get; set; }
        public string employee_name { get; set; }
        public string loginattendence_date { get; set; }
        public string login_time { get; set; }
        public string login_reason { get; set; }
        public string login_status { get; set; }
        public string apply_employeegid { get; set; }
    }

    public class loginrejected_list
    {
        public string attendancelogintmp_gid { get; set; }
        public string loginapply_date { get; set; }
        public string employee_name { get; set; }
        public string loginattendence_date { get; set; }
        public string login_time { get; set; }
        public string login_reason { get; set; }
        public string login_status { get; set; }
        public string apply_employeegid { get; set; }
    }




    public class GetapproveLeavedetails : result
    {
        public string leave_gid { get; set; }
        public string leavetype_name { get; set; }
        public string leave_fromdate { get; set; }
        public string leave_todate { get; set; }
        public string leave_noofdays { get; set; }
        public string leave_reason { get; set; }
        public string leave_status { get; set; }
        public string applied_by { get; set; }

    }


    public class getleavesummarylogindetails : result
    {
        public List<login_list> login_list { get; set; }
        public List<loginleavereject_list> loginleavereject_list { get; set; }

    }

    public class getleavesummarylogoutdetails : result
    {
        public List<logout_list> logout_list { get; set; }
        public List<logoutleaveapprove_list> logoutleaveapprove_list { get; set; }
    }
    public class getleavesummarypermissiondetails : result
    {
        public List<permission_list> permission_list { get; set; }
        public List<permissionreject_list> permissionreject_list { get; set; }
    }
    public class getleavesummaryoddetails : result
    {
        public List<od_list> od_list { get; set; }
        public List<odreject_list> odreject_list { get; set; }
    }
    public class getleavesummarycompoffdetails : result
    {
        public List<compoffdtl_list> compoffdtl_list { get; set; }
        public List<compoffdtlreject_list> compoffdtlreject_list { get; set; }
    }

    public class permissionreject_list : result 
    {
        public string permission_date { get; set; }
        public string permission_from { get; set; }
        public string permission_to { get; set; }
        public string permission_duration { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string employee_name { get; set; }

    }

    public class compoffdtlreject_list : result 
    {
        public string Compoff_from { get; set; }
        public string Compoff_to { get; set; }
        public string Compoff_duration { get; set; }
        public string Compoff_reason { get; set; }
        public string employee_name { get; set; }
        public string Compoff_status { get; set; } 
    }

    public class getleavedetails : result
    {
        public List<applyleave_list> applyleave_list { get; set; }
        public List<applyleaveapproved_list> applyleaveapproved_list { get; set; }
        public List<applyleavereject_list> applyleavereject_list { get; set; } 

    }

    public class odreject_list 
    {
        public string onduty_date { get; set; }
        public string onduty_from { get; set; }
        public string onduty_to { get; set; }
        public string onduty_duration { get; set; }
        public string onduty_reason { get; set; }
        public string employee_name { get; set; }
        public string ondutytracker_status { get; set; }
    }
    public class applyleavereject_list 
    {
        public string leave_gid { get; set; }
        public string leavetype_name { get; set; }
        public string leave_from { get; set; }
        public string leave_to { get; set; }
        public string noofdays_leave { get; set; }
        public string leave_reason { get; set; }
        public string approval_status { get; set; }
        public string applied_by { get; set; }
        public string document_name { get; set; }
    }

    public class logoutleaveapprove_list 
    {
        public string logoutapply_date { get; set; }
        public string logoutattendence_date { get; set; }
        public string logout_time { get; set; }
        public string logout_reason { get; set; }
        public string logout_status { get; set; }
        public string employee_name { get; set; }
    }

    public class applyleave_list 
    {

        public string leave_status { get; set; }
        public string leave_gid { get; set; }
        public string leavetype_name { get; set; }
        public string leave_from { get; set; }
        public string leave_to { get; set; }
        public string noofdays_leave { get; set; }
        public string leave_reason { get; set; }
        public string approval_status { get; set; }
        public string applied_by { get; set; }
        public string document_name { get; set; }
        public string employee_name { get; set; }


    }

    public class applyleaveapproved_list 
    {
        public string leave_gid { get; set; }
        public string leavetype_name { get; set; }
        public string leave_from { get; set; }
        public string leave_to { get; set; }
        public string noofdays_leave { get; set; }
        public string leave_reason { get; set; }
        public string approval_status { get; set; }
        public string applied_by { get; set; }
        public string document_name { get; set; }
    }

    public class login_list
    {

        public string created_date { get; set; }
        public string attendance_date { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }
        public string employee_gid { get; set; }
        public string loginapply_date { get; set; }
        public string loginattendence_date { get; set; }
        public string login_time { get; set; }
        public string login_reason { get; set; }
        public string login_status { get; set; }
        public string employee_name { get; set; }
    }

    public class loginleavereject_list
    {
        public string employee_gid { get; set; }
        public string created_date { get; set; }
        public string attendance_date { get; set; }
        public string remarks { get; set; }
        public string login_time { get; set; }
        public string status { get; set; }
    }
    public class logout_list
    {
        public string logoutapply_date { get; set; }
        public string logoutattendence_date { get; set; }
        public string logout_time { get; set; }
        public string logout_reason { get; set; }
        public string employee_name { get; set; }
        public string logout_status { get; set; }
    }

    public class od_list
    {
        public string onduty_date { get; set; }
        public string onduty_from { get; set; }
        public string onduty_to { get; set; }
        public string onduty_duration { get; set; }
        public string employee_name { get; set; }
        public string onduty_reason { get; set; }
        public string ondutytracker_status { get; set; }
    }

    public class compoffdtl_list
    {
        public string Compoff_from { get; set; }
        public string Compoff_to { get; set; }
        public string Compoff_duration { get; set; }
        public string Compoff_reason { get; set; }
        public string employee_name { get; set; }
        public string Compoff_status { get; set; }

    }

    public class permission_list
    {
        public string permission_date { get; set; }
        public string permission_from { get; set; }
        public string permission_to { get; set; }
        public string permission_duration { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string employee_name { get; set; }
    }
    public class approvepermission : result
    {
        public string permission_gid { get; set; }
        public List<approvepermission_list> approvepermission_list { get; set; }

    }
    public class approvepermission_list : approvepermission
    {

        public string employee_gid { get; set; }
        public string attendance_date { get; set; }
    }
    public class rejectpermission : result
    {
        public string permission_gid { get; set; }
        public List<rejectpermission_list> rejectpermission_list { get; set; }

    }
    public class rejectpermission_list : rejectpermission
    {

        public string employee_gid { get; set; }
        public string attendance_date { get; set; }
    }
    public class rejectcompoff : result
    {
        public string permission_gid { get; set; }
        public List<rejectcompoff_list> rejectcompoff_list { get; set; }

    }
    public class rejectcompoff_list : rejectcompoff
    {

        public string employee_gid { get; set; }
        public string attendance_date { get; set; }
    }
}