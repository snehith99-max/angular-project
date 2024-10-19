using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysMstBranch : result
    {
        public List<branch_list1> branch_list1 { get; set; }
        //public string status { get; set; }
        public string result {  get; set; }
    }
    public class branch_list1 : result
    {
        public string Postal_code { get; set; }
        public string branch_gid { get; set; }
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string branch_prefix { get; set; }
        public string branchmanager_gid { get; set; }
        public string branch_code_edit { get; set; }
        public string user_gid { get; set; }
        public string branch_name_edit { get; set; }
        public string Branch_address { get; set; }
        public string branch_logo_path { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string postal_code { get; set; }
        public string Phone_no { get; set; }
        public string Email_address { get; set; }
        public string GST_no { get; set; }
        public string branch_prefix_edit { get; set; }
        public string Phone_no_add { get; set; }
        public string GST_no_add { get; set; }
        public string Email_address_add { get; set; }
        public string Branch_address_add { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string BranchStatus { get; set; }
        public string branch_code_add { get; set; }
        public string city { get; set; }
       }  
}