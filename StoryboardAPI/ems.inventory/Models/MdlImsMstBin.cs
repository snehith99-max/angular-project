using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsMstBin:result
    {
        public List<imsbin_summarylist> imsbin_summarylist { get; set; }
        public List<imsbin_addlist> imsbin_addlist { get; set; }
        public List<imsbinadd_list> imsbinadd_list { get; set; }
        public List<assignedbin_list> assignedbin_list { get; set; }
    }
    public class imsbin_summarylist : result
    {
        public string branch_gid { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string location_code { get; set; }
        public string branch_name { get; set; }
        public string branch_prefix { get; set; }
        public string branch_code { get; set; }
    }
    public class imsbin_addlist : result
    {
        public string branch_gid { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string location_code { get; set; }
        public string branch_name { get; set; }
        public string branch_code { get; set; }
        public string bin_number { get; set; }
    }
    public class imsbinadd_list : result
    {
        public string branch_gid { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string location_code { get; set; }
        public string branch_name { get; set; }
        public string branch_code { get; set; }
        public string bin_number { get; set; }
    }
    public class assignedbin_list : result
    {
        public string bin_gid { get; set; }
        public string location_gid { get; set; }
        public string bin_number { get; set; }
    }
}