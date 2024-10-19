﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayTrnEmployeeBankDetails : result
    {
        public List<employeebankdetails_list> employeebankdetails_list { get; set; }
        public List<GetBankdropdown> GetBankDtl { get; set; }

        public List<GetBank> GetBank { get; set; }
        public string employee_gid { get; internal set; }

        public List<documentdtl_list> documentdtl_list { get; set; }
        public List<document_list> document_list { get; set; }

    }

    public class employeebankdetails_list : result
    {
        public string user_gid { get; set; }
        public string user_code { get; set; }
        public string employee_gid { get; set; }
        public string empname { get; set; }
        public string designation_gid { get; set; }
        public string designation_name { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string department_gid { get; set; }
        public string department_name { get; set; }
        public string bank_name { get; set; }
        public string bank_code { get; set; }
        public string ac_no { get; set; }
        public string pf_no { get; set; }
        public string esi_no { get; set; }
        public string pan_no { get; set; }
        public string uan_no { get; set; }



    }
    public class GetBankdropdown : result
    {
        public string bank_gid { get; set; }
        public string bank_name { get; set; }



    }

    public class GetBank : result
    {
        public string bank_gid { get; set; }
        public string bank { get; set; }
        public string pf_no { get; set; }
        public string esi_no { get; set; }
        public string ac_no { get; set; }
        public string pan_no { get; set; }
        public string uan_no { get; set; }
    }
    public class result1
    {
        public bool status { get; set; }
        public string message { get; set; }


    }
    public class documentdtl_list : result
    {
        public string user_code { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string remarks { get; set; }
    }

    public class document_list : result
    {
        public string user_code { get; set; }
        public string remarks { get; set; }
        public string document_name { get; set; }
        public string updated_by { get; set; }
        public string uploaded_date { get; set; }
    }
}
