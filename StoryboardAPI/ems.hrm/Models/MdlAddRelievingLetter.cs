using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlAddRelievingLetter : result
    {
        public List<Employeelists> Employeelists { get; set; }
        public List<GetEmployeeonchangedetails> GetEmployeeonchangedetails { get; set; }
        public List<PostEmployeeLists> PostEmployeeLists { get; set; }
        public List<GetRelievingLetterdropdown> GetRelievingLetterdropdown { get; set; }
    }
    public class Employeelists : result
    {
        public string EmployeeCode { get; set; }
        public string Employeegid { get; set; }
    }
    public class GetEmployeeonchangedetails : result
    {
        public string Name { get; set; }
        public string IDNo { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string TotalServices { get; set; }
        public string joiningdate { get; set; }
        public string exit_date { get; set; }
    }
    public class PostEmployeeLists : result
    {
        public string Name { get; set; }
        public string Settlement_Date { get; set; }
        public string template_name { get; set; }
        public string Total_Services { get; set; }
        public string EmployeeCode { get; set; }
        public string ID_No { get; set; }
        public string Date_of_Joining { get; set; }

        public string Department { get; set; }
        public string Date_of_Relieving { get; set; }
        public string Designation { get; set; }
        public string Reason_for_Settlement { get; set; }
        public string Min_Wages { get; set; }
        public string Salary { get; set; }
        public string ESIC { get; set; }
        public string Leave_Salary { get; set; }
        public string EPF { get; set; }
        public string Bonus { get; set; }
        public string LWF { get; set; }
        public string Gratuity { get; set; }
        public string Loan { get; set; }
        public string Total { get; set; }
        public string employee_gid { get; set; }
        public string user_name { get; set; }
       public string templatename { get; set; }
       public string Relievinglettertemplate_content { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
        public string qualification { get; set; }
        public string employee_salary { get; set; }




    }
    public class GetRelievingLetterdropdown : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
       
    }
}