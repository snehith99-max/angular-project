using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlIndentPriceEstimation: result
    {
        public List<GetIndentPriceEstimate_list> GetIndentPriceEstimate_list { get; set; }
        public List<GetIDPREstimationProductDetails_list> GetIDPREstimationProductDetails_list { get; set; }
        public List<GetIDPREstimationDetails_list> GetIDPREstimationDetails_list { get; set; }
        public List<GetIDPRProductDetailsCheckPrice_list> GetIDPRProductDetailsCheckPrice_list { get; set; }
        public List<generateprice_list> generateprice_list { get; set; }
    }
    public class GetIndentPriceEstimate_list: result
    {
        public string materialrequisition_date { get; set; }
        public string materialrequisition_gid { get; set; }
        public string material { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string user_firstname { get; set; }
        public string materialrequisition_reference { get; set; }
        public string materialrequisition_status { get; set; }
        public string provisional_amount { get; set; }
        public string branch_prefix { get; set; }
        public string created_date { get; set; }
    }
    public class GetIDPREstimationProductDetails_list : result
    {
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string productuom_name { get; set; }
        public double qty_requested { get; set; }
        public string product_gid { get; set; }
        public double unit_price { get; set; }
    }
    public class GetIDPREstimationDetails_list: result
    {
        public string user_firstname { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string materialrequisition_reference { get; set; }
        public string materialrequisition_date { get; set; }
        public string materialrequisition_gid { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string costcenter_name { get; set; }
        public string materialrequisition_status { get; set; }
        public string approver_remarks { get; set; }
        public double costcenter_amount { get; set; }
    }
    public class GetIDPRProductDetailsCheckPrice_list : result 
    {
        public string purchaseorder_date { get; set; }  
        public string vendor_companyname { get; set; }  
        public string product_name { get; set; }  
        public string productuom_name { get; set; }  
        public string price { get; set; }  
        public string product_gid { get; set; }  
    }
    public class generateprice_list: result
    {
        public string materialrequisition_gid { get; set; }
        public GetIDPREstimationProductDetails_list[] GetIDPREstimationProductDetails_list { get; set; }
        public double provisional { get; set; }
    }
}