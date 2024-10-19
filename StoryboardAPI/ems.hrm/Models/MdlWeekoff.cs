using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlWeekoff : result
    {
        public List<WeekOffLists> WeekOffLists { get; set; }
        public List<Getbranchdropdownlists> Getbranchdropdownlists { get; set; }
        public List<Getdepartmentdropdownlists> Getdepartmentdropdownlists { get; set; }
        public List<weekoff_list> weekoff_list { get; set; }
        public List<weekoffview_list> weekoffview_list { get; set; }
        public List<Employee_type> Employee_type { get; set; }
    }

    public class WeekOffLists : result
    {
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string designation_name { get; set; }
        public string branch_gid { get; set; }
        public string department_gid { get; set; }
        public string employee_gid {  get; set; }

    }
    public class Getbranchdropdownlists : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }
    public class Employee_type : result
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
    }
    public class Getdepartmentdropdownlists : result
    {
        public string department_name { get; set; }
        public string department_gid { get; set; }
    }
    public class weekoff_list : result
    {
        public string employee_gid { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        public string saturday { get; set; }
        public string sunday { get; set; }
        public List<string> employee_gid1 { get; set; }
    }
    public class weekoffview_list : result
    {
        public string weekoffemployee_gid { get; set; }
        public string employee_gid { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        public string saturday { get; set; }
        public string sunday { get; set; }
    }

    }
