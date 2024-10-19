using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlSalaryDetails : result
    {
        public List<salarydetails_branch> salarydetails_branch { get; set; }
        public List<salarydetails_department> salarydetails_department { get; set; }
        public List<salarydetails_component> salarydetails_component { get; set; }
        public List<Employeewisepaymentlists> Employeewisepaymentlists { get; set; }
        public List<GetSalaryDetailsComponent_list> GetSalaryDetailsComponent_list { get; set; }
    }

    public class salarydetails_branch : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }

    public class salarydetails_department : result
    {
        public string department_name { get; set; }
        public string department_gid { get; set; }

    }

    public class salarydetails_component : result
    {
        public string salarycomponent_gid { get; set; }
        public string component_name { get; set; }


    }

    //public class Employeewisepaymentlists : result
    //{
    //}


    public class GetSalaryDetailsComponent_list : result
    {

    }
}