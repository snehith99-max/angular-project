﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSystemRptEmployeeReport
    {
        public List<employeereport_list> employeereport_list { get; set; }
        public List<GetBranchnamedropdown> GetBranchDtl { get; set; }
        public List<GetreportExportExcel> getreport_exportexcel { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class employeereport_list : result
    {
        public string user_gid { get; set; }
        public string employee_gid { get; set; }
        //public string user_code { get; set; }
        public string user_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string employee_gender { get; set; }
        public string employee_joiningdate { get; set; }
        public string workpermit_no { get; set; }
        public string workpermit_expiredate { get; set; }
        public string passport_no { get; set; }
        public string passport_expiredate { get; set; }
        public string fin_no { get; set; }
        public string salary { get; set; }
        public string permonth_rate { get; set; }
       
    }
    public class GetBranchnamedropdown : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }

    public class GetreportExportExcel : result
    {
        public string lspath1 { get; set; }
        public string lsname { get; set; }
    }

}