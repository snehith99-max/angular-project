using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnMaterialindent : result
    {
        public List<materialindent_list> materialindentsummary_list { get; set; }
        public List<materialindentview_list> materialindentview_list { get; set; }
        public List<materialindentviewproduct_list> materialindentviewproduct_list { get; set; }
        public List<issueequest_list> issueequest_list { get; set; }
        public List<productrequest_list> productrequest_list { get; set; }
        public List<productsummary_list> productsummary_list { get; set; }
        public List<Mitopo_list> Mitopo_list { get; set; }
        public List<Potopayment_list> Potopayment_list { get; set; }
        public List<MICount> MICount { get; set; }
    }

        public class materialindent_list : result
        {
            public string materialrequisition_gid { get; set; }
            public string materialrequisition_reference { get; set; }
            public string materialrequisition_remarks { get; set; }
            public string material { get; set; }
            public string materialrequisition_status { get; set; }
            public string user_firstname { get; set; }
            public string materialrequisition_date { get; set; }
            public string mrbapproval_remarks { get; set; }
            public string costcenter_name { get; set; }
            public string department_name { get; set; }
            public string created_date { get; set; }
            public string expected_date { get; set; }
            public string priority { get; set; }
            public string available { get; set; }
            public string branch_prefix { get; set; }

        }

    public class materialindentview_list : result
    {
        public string materialrequisition_gid { get; set; }
        public string materialrequisition_reference { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string material { get; set; }
        public string materialrequisition_status { get; set; }
        public string user_firstname { get; set; }
        public string requested_by { get; set; }
        public string materialrequisition_date { get; set; }
        public string mrbapproval_remarks { get; set; }
        public string costcenter_name { get; set; }
        public string budget_available { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string created_date { get; set; }
        public string approver_remarks { get; set; }
        public string approvername { get; set; }
        public string materialrequisition_type { get; set; }
        public string approved_date { get; set; }
        public string expected_date { get; set; }
        public string priority { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_issued { get; set; }
        public string qty_requested { get; set; }
        public string display_field { get; set; }

    }
    public class materialindentviewproduct_list : result
    {
        public string materialrequisition_gid { get; set; }
        
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_issued { get; set; }
        public string qty_requested { get; set; }
        public string display_field { get; set; }
        public string pending_qty { get; set; }

    }

    public class issueequest_list : result
    {

        public string materialrequisition_gid { get; set; }
        public string materialrequisition_date { get; set; }
        public string approver_remarks { get; set; }
        public string materialrequisition_type { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string materialrequisition_reference { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string costcenter_gid { get; set; }
        public string budget_available { get; set; }
        public string costcenter_name { get; set; }
        public string approvername { get; set; }
        public string approved_date { get; set; }
        public string priority { get; set; }
        public string materialrequisition_status { get; set; }
        public string expected_date { get; set; }
    }

    public class productrequest_list : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set;}
        public string req_qty { get; set;}
        public string branch_gid { get; set;}
        public string materialrequisition_gid { get; set;}
        public string productuom_gid { get; set;}
        public string productuom_name { get; set;}
        public string display_field { get; set;}
        public string materialrequisitiondtl_gid { get; set;}
        public string qty_issued { get; set;}
        public string pending_mr { get; set;}
        public string pending_pr { get; set;}
        public string stock_quantity { get; set;}
        public string Avl_qty { get; set;}
        public string issuerequestqty { get; set;}
        public string productgroup_name { get; set;}
    }
    public class productsummary_list : result
    {
        public string materialrequisition_gid { get; set; }
        public string materialrequisitiondtl_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string available_quantity { get; set; }

    }
    public class Mitopo_list : result
    {
        public string materialrequisitiondtl_gid { get; set; }
        public string materialrequisition_date { get; set; }
        public string materialrequisition_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string materialrequisition_status { get; set; }
        public string display_field { get; set; }
        public string purchaserequisition_date { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string qty_issued { get; set; }
        public string materialissued_date { get; set; }
        public string materialissued_reference { get; set; }
        public string purchaserequisitiondtl_gid { get; set; }
        public string purchaserequisition_status { get; set; }

    }
    public class Potopayment_list : result
    {
        public string purchaserequisition_gid { get; set; }
        public string purchaseorder_gid { get; set; }
        public string grn_gid { get; set; }
        public string invoice_gid { get; set; }
        public string payment_gid { get; set; }
        public string grn_flag { get; set; }
        public string overall_status { get; set; }
    }
    public class issuematerialrequest_list : result
    {
        public string remarks { get; set; }
        public List<productrequest_list> productrequest_list { get; set; }
    }
    public class MICount
    {
        public string totalcount {  get; set; }
        public string prioritycount {  get; set; }
        public string available_count {  get; set; }
    }
}
                   