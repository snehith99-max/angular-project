using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlDeliveryCost : result
    {
        public List<GetDeliverycost_list> GetDeliverycost_list { get; set; }
        public List<PostDeliverycost_list> PostDeliverycost_list { get; set; }
        public List<GetPincodeSummaryAssign> GetPincodeSummaryAssign { get; set; }
        public List<PostAssignPincodedelivery_list> PostAssignPincodedelivery_list { get; set; }
    }
    public class GetDeliverycost_list : result
    {
        public string deliverybase_cost { get; set; }
        public string deliverycost_id { get; set; }
        public string created_by { get; set; }
    }
    public class PostDeliverycost_list : result
    {
        public string deliverybase_cost { get; set; }
        public string deliverycost_id { get; set; }
    }
    public class GetPincodeSummaryAssign : result
    {
        public string pincode_code { get; set; }
        public string pincode_id { get; set; }
        public string created_date { get; set; }
        public string branch_gid { get; set; }
        public string created_by { get; set; }
    }
    public class PostAssignPincodedelivery_list : result
    {
        public string deliverycost_id { get; set; }
        public deliverypincodeassing[] deliverypincodeassing { get; set; }
    }
    public class deliverypincodeassing 
    {
        public string pincode_id { get; set; }
    }
}
