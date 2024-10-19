using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptGrnDetailReport : result
    {
        public List<grn_lists> grn_lists { get; set; }
        public List<branch_lists> branch_lists { get; set; }
        public List<vendor_lists> vendor_lists { get; set; }
    }
    public class branch_lists : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }

    }
    public class vendor_lists : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }

    }
    public class grn_lists : result
    {
        public string grn_gid { get; set; }
        public string grn_date { get; set; }
        public string grn_status { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_delivered { get; set; }
        public string costcenter_name { get; set; }
        public string invoice_flag { get; set; }
      
        public string branch_name { get; set; }
        public string qty_rejected { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string approved_by { get; set; }
        public string approved_date { get; set; }
        public string grn_flag { get; set; }
        public string amount { get; set; }
        public string overall_status { get; set; }
        public string qty_ordered { get; set; }
        public string qty_pending { get; set; }
    }
}