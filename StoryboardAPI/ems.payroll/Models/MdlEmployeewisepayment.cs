using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlEmployeewisepayment : result
    {
        public List<Getbranchdropdown> Getbranchdropdown{ get; set; }
        public List<Employeewisepaymentlists> Employeewisepaymentlists { get; set; }
    }
    public class Getbranchdropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }

    }
    public class Employeewisepaymentlists : result
    {
        public string Department_Name { get; set; }
        public string User_Code { get; set; }
        public string Employee_Name { get; set; }
        public string Designation_Name { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Payment_Type { get; set; }
        public string Total_Salary { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string month { get; set; }
        public string year { get; set; }


    }
}