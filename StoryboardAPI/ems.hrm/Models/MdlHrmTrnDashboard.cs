﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class mdlcompanypolicies : result
    {
        public List<CompanyPolicies> CompanyPolicies { get; set; }
        public List<salarydetails_list> salarydetails_list { get; set; }
        public List<additionsalarydetail_list> additionsalarydetail_list { get; set; }
        public List<deductionsalarydetail_list> deductionsalarydetail_list { get; set; }
        public List<othersalarydetail_list> othersalarydetail_list { get; set; }
        public List<punchinlogin> punchinlogin { get; set; }
        public string checkeventname {  get; set; }
       

    }
    public class CompanyPolicies : result
    {
        public string policy_name { get; set; }
        public string policy_desc { get; set; }
    }
    public class punchinlogin : result
    {
        public string login_time { get; set; }
        public string update_flag { get; set; }
        public string logout_time { get; set; }
    }


    public class mdliAttendance : result
    {
        //public bool status { get; set; }
        public string location { get; set; }
        public string update_flag { get; set; }
        public string login_time_audit { get; set; }
        public string iattendence_privilege { get; set; }
    }
    public class mdlloginreq : result
    {
        public DateTime loginreq_date { get; set; }
        public DateTime login_date { get; set; }
        public string loginreq_reason { get; set; }
        public DateTime logintime { get; set; }
    }
    public class mdllogoutreq : result
    {
        public DateTime logoutattendence_date { get; set; }
        public DateTime logouttime { get; set; }
        public DateTime logout_date { get; set; }
        public string logouttime_reason { get; set; }
        public string logoutreq_reason { get; set; }
    }

    public class salarydetails_list : result
    {
        public string salary_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string salary_days { get; set; }
        public string lop_days { get; set; }
        public string basic_salary { get; set; }
        public string earned_basic_salary { get; set; }
        public string gross_salary { get; set; }
        public string net_salary { get; set; }

    }

    public class additionsalarydetail_list : result
    {
        public string salary_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string earned_salarycomponent_amount { get; set; }

    }
    public class deductionsalarydetail_list : result
    {
        public string salary_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string earned_salarycomponent_amount { get; set; }
    }
    public class othersalarydetail_list:result
    {
        public string salary_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string earned_salarycomponent_amount { get; set; }
        public string othercomponent_type {  get; set; }
    }
    public class mdlcompffreq : result
    {
        public DateTime actualworking_date { get; set; }
        public DateTime compensatoryoff_applydate { get; set; }
        public string compoff_reason { get; set; }
    }
    public class eventdetail : result
    {
        public List<createevent> createevent { get; set; }
        public string event_date { get; set; }
        public DateTime event_time { get; set; }
        public TimeSpan time { get; set; }
        public string today_event { get; set; }
        public string event_title { get; set; }
        public string reminder_gid { get; set; }
    }
    public class createevent : result
    {
        public string event_date { get; set; }
        public DateTime event_time { get; set; }
        public string time { get; set; }
        public string today_event { get; set; }
        public string event_title { get; set; }
        public string reminder_gid { get; set; }
    }
    public class monthlyAttendence : result
    {
        public List<last6MonthAttendence_list> last6MonthAttendence_list { get; set; }
        public string monthyear { get; set; }
        public string totalDays { get; set; }
        public string countPresent { get; set; }
        public string countAbsent { get; set; }
        public string countLeave { get; set; }
        public string countholiday { get; set; }
        public string countWeekOff { get; set; }
    }

    public class last6MonthAttendence_list
    {
        public string monthname { get; set; }
        public string countPresentm { get; set; }
        public string countAbsentm { get; set; }
        public string monthyear { get; set; }
        public string totalDays { get; set; }
        public string countPresent { get; set; }
        public string countAbsent { get; set; }
        public string countLeave { get; set; }
        public string countholiday { get; set; }
        public string countWeekOff { get; set; }
    }

    public class mdlloginsummary
    {
        public List<loginsummary_list> loginsummary_list { get; set; }
    }

    public class holidaycalender
    {
        public bool status { get; set; }
        public string employee_gid { get; set; }


        public List<holidaycalender_list> holidaycalender_list { get; set; }
    }
    public class holidaycalender_list : result
    {
        public string holiday_date { get; set; }
        public string holiday_dayname { get; set; }
        public string holiday_name { get; set; }

    }
    public class loginsummary_list : result
    {
        public string attendancelogintmp_gid { get; set; }
        public string applyDate { get; set; }
        public string attendanceDate { get; set; }
        public string login_Time { get; set; }
        public string login_status { get; set; }
        public string remarks { get; set; }
    }

    public class mdllogoutsummary
    {
        public List<logoutsummary_list> logoutsummary_list { get; set; }
    }

    public class logoutsummary_list : result
    {
        public string attendancetmp_gid { get; set; }
        public string applyDate { get; set; }
        public string attendanceDate { get; set; }
        public string logout_Time { get; set; }
        public string logout_status { get; set; }
        public string remarks { get; set; }
    }

    public class applyondutydetails : result
    {
        public DateTime od_date { get; set; }
        public string od_fromhr { get; set; }
        public string od_tohr { get; set; }
        public string od_frommin { get; set; }
        public string od_tomin { get; set; }
        public string onduty_period { get; set; }
        public string od_session { get; set; }
        public string total_duration { get; set; }
        public string od_reason { get; set; }
        //public bool status { get; set; }
        public string ondutytracker_status { get; set; }
        public string half_day_flag { get; set; }
        public string half_session { get; set; }
        public string onduty_count { get; set; }
    }
    public class onduty_detail_list : result
    {
        public List<onduty_details> onduty_details { get; set; }
    }
    public class onduty_details : result
    {
        public string ondutytracker_gid { get; set; }
        public string apply_date { get; set; }
        public string onduty_date { get; set; }
        public string onduty_from { get; set; }
        public string onduty_to { get; set; }
        public string onduty_duration { get; set; }
        public string onduty_reason { get; set; }
        public string ondutytracker_status { get; set; }
        public string approved_by { get; set; }
        public string approved_date { get; set; }
    }
    public class permission_details : result
    {
        public DateTime permission_date { get; set; }
        public DateTime permission_applydate { get; set; }
        public string permission_fromhr { get; set; }
        public string permission_tohr { get; set; }
        public string permission_frommin { get; set; }
        public string permission_tomin { get; set; }
        public string permission_total { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string approved_by { get; set; }
        public DateTime approved_date { get; set; }
    }    
    public class compoff_list : result
    {
        public List<compoffSummary_details> compoffSummary_details { get; set; }
    }
    public class compoffSummary_details : result
    {
        public string compensatoryoff_gid { get; set; }
        public string compoff_date { get; set; }
        public string atual_working { get; set; }
        public string compoff_reason { get; set; }
        public string compoff_status { get; set; }
    }
    public class permission_details_list : result
    {
        public List<permissionSummary_details> permissionSummary_details { get; set; }
    }
    public class permissionSummary_details : result
    {
        public string permission_gid { get; set; }
        public string permissiondtl_gid { get; set; }
        public string permission_date { get; set; }
        public string permission_applydate { get; set; }
        public string permission_from { get; set; }
        public string permission_to { get; set; }
        public string permission_total { get; set; }
        public string permission_reason { get; set; }
        public string permission_status { get; set; }
        public string approved_by { get; set; }
        public string approved_date { get; set; }
    }

    public class monthlyAttendenceReport : result
    {
        public List<monthlyAttendenceReport_list> monthlyAttendenceReport_list { get; set; }

    }

    public class monthlyAttendenceReport_list
    {
        public string attendance_date { get; set; }
        public string Login_time { get; set; }
        public string logout_time { get; set; }
        public string attendance_type { get; set; }
        public string shift_name { get; set; }
        public string worked_hrs { get; set; }
        public string early { get; set; }
        public string late { get; set; }

    }


}