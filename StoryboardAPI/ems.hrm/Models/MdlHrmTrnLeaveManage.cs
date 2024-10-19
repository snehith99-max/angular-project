using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmTrnLeaveManage : result
    {
        public List<leavemanage_list> leavemanagelist { get; set; }
        public List<permissionname_list> permissionnamelist { get; set; }
        public List<ondutyname_list> ondutynamelist { get; set; }
        public List<Getbranchdetail> Getbranch_detail { get; set; }
        public List<Getdepartmentdetail> Getdepartment_detail { get; set; }
        public List<GetEmployeedtl> GetEmployee_dtl { get; set; }
        public List<GetDateJoin> GetDate_Join { get; set; }
        public List<GetLeaveAvailable> GetLeave_Available { get; set; }
        public List<GetLeaveBalance> GetLeaveBalance { get; set; }

    }
    public class leavemanage_list : result
    {
        public string leave_date { get; set; }
        public string created_date { get; set; }
        public string employeegid { get; set; }
        public string days_name { get; set; }
        public string session_leave { get; set; }
        public string user_fullname { get; set; }
        public string branch_prefix { get; set; }
        public DateTime leave_datefrom { get; set; }
        public DateTime leave_dateto { get; set; }
        public string leave_days { get; set; }
        public string reason { get; set; }
        public string leave_reason { get; set; }
        public string department_name { get; set; }
        public string leave_gid { get; set; }
        public string leave_noofdays { get; set; }
        public string leave_applydate { get; set; }
        public string leave_fromdate { get; set; }
        public string leave_todate { get; set; }
        public string leave_approveddate { get; set; }
        public string leavedtl_date { get; set; }
        public string leavetype_name { get; set; }
        public string user_gid { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string leave_status { get; set; }
        public string designation_name { get; set; }
        public string leavetype_gid { get; set; }
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
    }

    public class GetDateJoin : result
    {
        public string employee_gid { get; set; }
        public string employee_joiningdate { get; set; }
    }
    public class GetLeaveBalance : result
    {
        public string employee_gid { get; set; }
        public string leavetype_name { get; set; }
        public string availableleave_count { get; set; }
    }

    public class GetLeaveAvailable : result
    {
        public string leavetype_gid { get; set; }
        public string leavetype_name { get; set; }
      

    }

        public class GetEmployeedtl : result
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
    }

    public class permissionname_list : result
    {
        public string branch_prefix { get; set; }
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public string permission_reason { get; set; }
        public string created_date { get; set; }
        public string permission_fromhours { get; set; }
        public string permission_tohours { get; set; }
        public string permission_applydate { get; set; }
        public string permission_gid { get; set; }
        public string employeenamegid { get; set; }
        public string total_duration { get; set; }
        public string reason_permission { get; set; }
        public string from_hrs { get; set; }
        public string to_hrs { get; set; }
        public string employee_gid { get; set; }
        public string permission_date { get; set; }
        public string from_hours { get; set; }
        public string to_hours { get; set; }
        public string permission_totalhours { get; set; }
        public string permission_status { get; set; }
        public string user_code { get; set; }
        public string employee_name { get; set; }
        public string branch_name { get; set; }
    }

    public class ondutyname_list : result
    {
        public string branch_prefix { get; set; }
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public string onduty_reason { get; set; }
        public string employeedetailgid { get; set; }
        public string onduty_fromtime { get; set; }
        public string onduty_totime { get; set; }
        public string reason_onduty { get; set; }
        public string half_day { get; set; }
        public string half_session { get; set; }
        public string onduty_count { get; set; }
        public string ondutytracker_gid { get; set; }
        public string employee_gid { get; set; }
        public string from_hrsod { get; set; }
        public string to_hrsod { get; set; }
        public string onduty_date { get; set; }
        public string total_durationod { get; set; }
        public string onduty_duration { get; set; }
        public string created_date { get; set; }
        public string ondutytracker_status { get; set; }
        public string ondutytracker_date { get; set; }
        public string user_firstname { get; set; }
        public string user_code { get; set; }
        public string branch_name { get; set; }
    }

    public class Getbranchdetail : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }

    }
    public class Getdepartmentdetail : result
    {
        public string department_name { get; set; }
        public string department_gid { get; set; }

    }
}