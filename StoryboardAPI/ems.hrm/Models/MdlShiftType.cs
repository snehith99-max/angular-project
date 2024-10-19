using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlShiftType : result
    {
        public List<Assign_type> Assign_type { get; set; }
        public List<shift_list> shift_list { get; set; }
        public List<Time_list> Time_list { get; set; }
        public List<Assignsubmit_list> Assignsubmit_list { get; set; }
        public List<UnAssignsubmit_list> UnAssignsubmit_list { get; set; }
        public List<GetEditShiftType> GetEditShiftType { get; set; }
        public List<GetEditlogin> GetEditlogin { get; set; }
        public List<GetEditlogout> GetEditlogout { get; set; }


    }
    public class shift_list : result
    {
        public string statuses { get; set; }
        public string shifttype_gid { get; set; }
        public string shifttype_name { get; set; }
        public string branch_name { get; set; }
        public string Status { get; set; } 
    }
    public class Assign_type : result
    {
        public string shifttype_gid { get; set; }
        public string shifttype_name { get; set; }
        public string branch_name { get; set; }
    }
    public class Assignsubmit_list : result
    {
        public string employeegid { get; set; }
        public string shifttype_gid { get; set; }
        public string shifttype_name { get; set; }
        public string branch_name { get; set; }       
        public List<Assign_list> Assign_list { get; set; }
    }
    public class UnAssignsubmit_list : result
    {
        public string employeegid { get; set; }
        public string shifttype_gid { get; set; }
        public string shifttype_name { get; set; }
        public string branch_name { get; set; }
        public List<UnAssign_list> UnAssign_list { get; set; }
    }
    public class GetEditlogin : result
    {
        public string shifttype_name { get; set; }
        public string overnight_flag { get; set; }
        public string In_overnightflag { get; set; }
        public string Out_overnightflag { get; set; }
        public string execute_in { get; set; }
        public string execute_out { get; set; }
        public string cutoff_time { get; set; }
    }
    public class GetEditlogout : result
    {
        public string shifttype_name { get; set; }
        public string overnight_flag { get; set; }
        public string In_overnightflag { get; set; }
        public string Out_overnightflag { get; set; }
        public string execute_in { get; set; }
        public string execute_out { get; set; }
        public string cutoff_time { get; set; }
    }
    public class UnAssign_list : result
    {
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
    }
    public class Assign_list : result
    {
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
    }
    public class Time_list : result
    {
        public string shifttypedtl_gid { get; set; }
        public string shifttypedtl_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }
    public class weekday_list : result
    {
        public string weekday_gid { get; set; }
        public string weekday { get; set; }
        public DateTime logintime { get; set; }
        public DateTime logouttime { get; set; }
        public DateTime Ot_cutoff { get; set; }
    }
    public class shiftedit_submit : result
    {
        public string login_scheduler { get; set; }
        public string entrycutoff_time { get; set; }
        public string overnight_flag { get; set; }
        public string inovernight_flag { get; set; }
        public string outovernight_flag { get; set; }
        public string logout_schedular { get; set; }
        public string existcutoff_time { get; set; }
        public string logout_overnight_flag { get; set; }
        public string logout_inovernight_flag { get; set; }
        public string logout_outovernight_flag { get; set; }       
        public string shift_name { get; set; }
        public string grace_time { get; set; }
        public string email_list { get; set; }
        public string shifttype_gid { get; set; }
        public List<weekday_list> weekday_list { get; set; }

    }

    public class shifttypeadd_list : result
    {
        public string login_scheduler { get; set; }
        public string entrycutoff_time { get; set; }
        public string existcutoff_time { get; set; }
        public string overnight_flag { get; set; }
        public string inovernight_flag { get; set; }
        public string outovernight_flag { get; set; }
        public string logout_overnight_flag { get; set; }
        public string logout_inovernight_flag { get; set; }
        public string logout_outovernight_flag { get; set; }
        public string logout_schedular { get; set; }
        public string shift_name { get; set; }
        public string email_list { get; set; }
        public string grace_time { get; set; }
        public string logintime { get; set; }
        public string logouttime { get; set; }
        public string Ot_cutoff { get; set; }
        public List<weekday_list> weekday_list { get; set; }
    }
    public class GetEditShiftType : result
    {
        public string shifttype_gid { get; set; }
        public string shifttypedtl_gid { get; set; }
        public string shift_name { get; set; }
        public string email_list { get; set; }
        public string grace_time { get; set; }
        public string weekday { get; set; }
        public string logintime { get; set; }
        public string logouttime { get; set; }
        public string Ot_cutoff { get; set; }
        public string login_scheduler { get; set; }
        public string entrycutoff_time { get; set; }
        public string overnight_flag { get; set; }
        public string inovernight_flag { get; set; }
        public string outovernight_flag { get; set; }
        public string logout_schedular { get; set; }
        public string existcutoff_time { get; set; }
        public string logout_overnight_flag { get; set; }
        public string logout_inovernight_flag { get; set; }
        public string logout_outovernight_flag { get; set; }

     
    }
}