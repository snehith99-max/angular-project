using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlotltrnMI:result
    {
        public List<materialindent_list> MIoutletsummary_list { get; set; }
        public List<materialindentview_list> materialindentview_list { get; set; }
        public List<materialindentviewproduct_list> materialindentviewproduct_list { get; set; }
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
        public string created_date { get; set; }
        public string branch_name { get; set; }
        public string approver_remarks { get; set; }
        public string approvername { get; set; }
        public string materialrequisition_type { get; set; }
        public string approved_date { get; set; }
        public string expected_date { get; set; }
        public string priority { get; set; }

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

    }
}