using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayTrnPfManagement
    {
        public List<GetPfManagement> GetPfManagement_list { get; set; }
        public List<GetPfEmployee> GetPfEmployee_list { get; set; }
        public List<GetEmployeesubmit_list> Employeesubmit_list { get; set; }
        public List<GetAddpfDetails> GetAddpfDetails_list { get;set;}



        public class GetPfManagement : result
        {
            public string employee_gid { get;set;}
            public string user_code { get;set;}
            public string employee_name { get;set;}
            public string branch_name { get;set;}
            public string department_name { get;set;}
            public string designation_name { get;set;}
        }

        public class GetPfEmployee : result
        {
            public string employee_gid { get; set; }
            public string user_code { get; set; }
            public string employee_name { get; set; }
            public string branch_name { get; set; }
            public string department_name { get; set; }
            public string designation_name { get; set; }
            public string department_gid { get; set; }
            public string branch_gid { get; set; }
            public string designation_gid { get; set; }
        }

        public class GetEmployeesubmit_list : result {
            public List<GetPfEmployee> GetPfEmployee_list { get; set; }

            public string pf_no { get; set; }
            public string experience { get; set; }
            public string remarks { get; set; }
            public string pf_doj { get; set; }
            public string employee_gid { get; set; }
            public string totalperoidofpreservice { get; set; }
        }

        public class GetAddpfDetails :result
        {
            public string accountdtl_gid { get; set; }
            public string pf_no { get; set; }
            public string pf_doj { get; set; }
            public string experience { get; set; }
            public string remarks { get; set; }
            public string totalperiod_preservice { get; set; }
            public string department_gid { get; set; }
            public string branch_gid { get; set; }
            public string designation_gid { get; set; }
        }
      
    }
}