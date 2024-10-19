using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptMaterialTracker :result
    {
        public List<branchlist> branchlist { get; set; }
        public List<materialtrackerlist> materialtrackerlist { get; set; }
        public List<materialtracker_list1> materialtracker_list1 { get; set; }
    }
    public class branchlist : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }
    public class materialtrackerlist : result
    {
        public string materialrequisitiondtl_gid { get; set; }
        public string materialrequisition_date { get; set; }
        public string materialrequisition_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string materialrequisition_status { get; set; }
        public string purchaserequisition_date { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string qty_issued { get; set; }
        public string branch_gid { get; set; }
    }
    public class materialtracker_list1 : result
    {
        public string purchaseorder_gid { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string grn_gid { get; set; }
        public string invoice_gid { get; set; }
        public string payment_gid { get; set; }
        public string overall_status { get; set; }
    }
}