using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptGrnReport:result
    {
        public List<grn_list> grn_list { get; set; }
        public List<branch> branch { get; set; }
        public List<vendor_list> vendor_list { get; set; }
    }
    public class branch : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }

    }
    public class vendor_list : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }

    }
    public class grn_list : result
    {
        public string grn_gid { get; set; }
        public string purchaseorder_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string grn_status { get; set; }
        public string vendor_gid { get; set; }
        public string grn_flag { get; set; }
        public string invoice_flag { get; set; }
        public string branch_name { get; set; }
        public string costcenter_name { get; set; }
        public string grn_date { get; set; }
        public string branch_gid { get; set; }
        public string overall_status { get; set; }
        public string approved_date { get; set; }
        public string approved_by { get; set; }
        public string branch_prefix { get; set; }
    }
}