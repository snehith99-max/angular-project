using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptProductIssueReport:result
    {
        public List<product_issuelist> product_issuelist { get; set; }
    }
    public class product_issuelist : result
    {
        public string materialissued_gid { get; set; }
        public string materialrequisition_gid { get; set; }
        public string materialissued_date { get; set; }
        public string materialissued_status { get; set; }
        public string purchasestatus { get; set; }
        public string status { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string product_name { get; set; }
        public string qty_issued { get; set; }
        public string qty_requested { get; set; }
        public string unit_price { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string purchase_requested_qty { get; set; }
        public string issued_value { get; set; }
        public string display_field { get; set; }
        public string issued_to { get; set; }
        public string branch_name { get; set; }
        public string balance_qty { get; set; }
        public string branch_prefix { get; set; }
       
    }
}