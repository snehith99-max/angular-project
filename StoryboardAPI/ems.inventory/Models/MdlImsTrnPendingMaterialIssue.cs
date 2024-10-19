using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnPendingMaterialIssue:result
    {
        public List<pendingmaterialissue_list> pendingmaterialissue_list {  get; set; }
        public List<raisematerialindent_list> raisematerialindent_list {  get; set; }
        public List<materialindentproduct_list> materialindentproduct_list {  get; set; }
        public List<raisepr_list> raisepr_list {  get; set; }
        public List<detialspop_list> detialspop_list {  get; set; }
    }
    public class pendingmaterialissue_list :result
    {
        public string materialrequisition_gid { get; set; }
        public string materialrequisition_reference { get; set; }
        public string materialrequisition_status { get; set; }
        public string material_status { get; set; }
        public string materialrequisition_date { get; set; }
        public string user_firstname { get; set; }
        public string costcenter_name { get; set; }
        public string department_name { get; set; }
        public string created_date { get; set; }
        public string expected_date { get; set; }
        public string branch_prefix { get; set; }
    }
    public class raisematerialindent_list : result
    {
        public string materialrequisition_gid { get; set; }
        public string materialrequisition_date { get; set; }
        public string provisional_amount { get; set; }
        public string materialrequisition_type { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string materialrequisition_reference { get; set; }
        public string user_firstname { get; set; }
        public string costcenter_name { get; set; }
        public string department_name { get; set; }
        public string expected_date { get; set; }
        public string priority_remarks { get; set; }
        public string priority { get; set; }

    }
    public class materialindentproduct_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string req_qty { get; set; }
        public string branch_gid { get; set; }
        public string materialrequisition_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string display_field { get; set; }
        public string materialrequisitiondtl_gid { get; set; }
        public string productgroup_name { get; set; }
        public string qty_issued { get; set; }
        public string pending_mr { get; set; }
        public string pending_pr { get; set; }
        public string stock_quantity { get; set; }
        public string net_avalible { get; set; }
    }
    public class raisepr_list : result
    {
        public string priority { get; set; }
        public string priority_remarks { get; set; }
        public string qty_requested { get; set; }
        public string expected_date { get; set; }
        public string materialrequisition_gid { get; set; }
        public List<product_requestlist> product_requestlist { get; set; }
    }
    public class product_requestlist : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string req_qty { get; set; }
        public string branch_gid { get; set; }
        public string materialrequisition_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string display_field { get; set; }
        public string materialrequisitiondtl_gid { get; set; }
        public string productgroup_name { get; set; }
        public string qty_issued { get; set; }
        public string pending_mr { get; set; }
        public string pending_pr { get; set; }
        public string stock_quantity { get; set; }
        public string net_avalible { get; set; }
    }
    public class detialspop_list : result
    {
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string qty_issued { get; set; }
        public string branch_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string stockqty { get; set; }
    }

}