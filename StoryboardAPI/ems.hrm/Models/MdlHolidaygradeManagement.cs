using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHolidaygradeManagement : result
    {
        public List<holidaysummary_list> holidaysummary_list { get; set; }
        public List<addholidaygrade_list> addholidaygrade_list { get; set; }
        public List<holidaygrade1_list> holidaygrade1_list { get; set; }
        public List<Addholidayassign_list> Addholidayassign_list { get; set; }
        public List<holidayassignemployee> holidayassignemployee { get; set; }
        public List<holidayunassign> holidayunassign { get; set; }
        public List<holidayunassignemployeesubmit> holidayunassignemployeesubmit { get; set; }
        public List<holidayeditassign> holidayeditassign { get; set; }
        public List<holidayeditunassign> holidayeditunassign { get; set; }
        public List<HolidayEditUnassignsubmit> HolidayEditUnassignsubmit { get; set; }
        public List<deleteedit> deleteedit { get; set; }
        public List<holidayview_list> holidayview_list { get; set; }
        public List<Holidayassign_type> Holidayassign_type { get; set; }

    }
    public class deleteedit : result
    {

        public string holidaygrade_gid { get; set; }

    }
    public class Addholidayassign_list : result
    {
        public string holidaygrade_gid { get; set; }
        public string holidaygrade_code { get; set; }
        public string holidaygrade_name { get; set; }
        public List<holidaygrade1_list> holidaygrade1_list { get; set; }

    }
    public class holidayeditassign : result
    {
        public string holidaygrade_gid { get; set; }
        public string holiday_gid { get; set; }
        public string holiday_date { get; set; }
        public string holiday_name { get; set; }

    }

    public class Holidayassign_type : result
    {
        public string holidaygrade_gid { get; set; }
        public string holidaygrade_code { get; set; }
        public string holidaygrade_name { get; set; }

    }

    public class holidayview_list : result
    {
        public string holiday_gid { get; set; }
        public string holidaygrade_gid { get; set; }
        public string holiday_date { get; set; }
        public string holiday_name { get; set; }

    }
    public class holidayeditunassign : result
    {
        public string holidaygrade_gid { get; set; }
        public string holiday_gid { get; set; }
        public DateTime holiday_date { get; set; }
        public string holiday_name { get; set; }

    }
    public class holidaysummary_list : result
    {
        public string holidaygrade_gid { get; set; }
        public string holidaygrade_code { get; set; }
        public string holidaygrade_name { get; set; }
        public string holiday_name { get; set; }
        public string holiday_type { get; set; }
        public DateTime holiday_date { get; set; }
        public string holiday_remarks { get; set; }
        public string holidayremarks { get; set; }
        public string holiday_gid { get; set; }
    }
    public class holidaygrade1_list : result
    {
        public string holidaygrade_gid { get; set; }
        public string holidaygrade_code { get; set; }
        public string holidaygrade_name { get; set; }
        public string holiday_name { get; set; }
        public string holiday_type { get; set; }
        public DateTime holiday_date { get; set; }
        public string holiday_remarks { get; set; }
        public string holidayremarks { get; set; }
        public string holiday_gid { get; set; }


    }

    public class addholidaygrade_list : result
    {
        public string holiday_name { get; set; }
        public string holiday_type { get; set; }
        public string holiday_date { get; set; }
        public string holiday_remarks { get; set; }
        public string holiday_gid { get; set; }
    }
    public class holidayassignemployee : result
    {
        public string employee_gid { get; set; }
        public string designation_name { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string empname { get; set; }
        public string holidaygrade_gid { get; set; }
    }
    public class Holidayemployeesumbit: result
    {
        public string employee_gid { get; set; }
        public string designation_name { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string empname { get; set; }
        public string holidaygrade_gid { get; set; }
        public DateTime holiday_date { get; set; }
        public string holiday_gid { get; set; }
        public List<holidayassignemployee> holidayassignemployee { get; set; }

    }    public class HolidayEditUnassignsubmit: result
    {
        
        public string holidaygrade_gid { get; set; }
        public DateTime holiday_date { get; set; }
        public string holiday_gid { get; set; }
        public string holiday_name { get; set; }
        public List<holidayeditunassign> holidayeditunassign { get; set; }

    }
    public class holidayunassign : result
    {
        public string employee_gid { get; set; }
        public string designation_name { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string empname { get; set; }
        public string holidaygrade_gid { get; set; }
    }
    public class holidayunassignemployeesubmit : result
    {
        public string employee_gid { get; set; }
        public string designation_name { get; set; }
        public string user_code { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string empname { get; set; }
        public string holidaygrade_gid { get; set; }
        public DateTime holiday_date { get; set; }
        public string holiday_gid { get; set; }
        public List<holidayunassign> holidayunassign { get; set; }

    }
    public class holiday
    {
        public string holiday_gid { get; set; } 
        public string holiday_date { get; set; }
    }
}