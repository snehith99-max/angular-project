using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOtlTrnPurchaseIndent : result
    {
        public List<Summary_list> Summary_list { get; set; }
        public List<productlistdetail> productlistdetail { get; set; }
        public List<productlist> productlist { get; set; }
        public List<productsummary_list> productsummary_list { get; set; }
        public List<PostAllPI> PostAllPI { get; set; }
        public List<purchaserequestitionview> purchaserequestitionview { get; set; }
    }
    public class Summary_list : result
    {
        public string purchaserequisition_gid { get; set; }
        public string purchaserequisition_date { get; set; }
        public string costcenter_name { get; set; }
        public string purchaserequisition_referencenumber { get; set; }
        public string user_firstname { get; set; }
        public string purchaserequisition_remarks { get; set; }
        public string overall_status { get; set; }
        public string branch_name { get; set; }
    }
    public class productlistdetail : result
    {
        public string customerproduct_code { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string qty_requested { get; set; }
        public string pr_originalqty { get; set; }
    }
    public class productlist : result
    {

        public string qty_requested { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string display_field { get; set; }

    }
    public class productsummary_list : result
    {
        public string tmppurchaserequisition_gid { get; set; }
        public string qty_requested { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string display_field { get; set; }

    }
    public class PostAllPI : result
    {
        public string tmppurchaserequisition_gid { get; set; }
        public string qty_requested { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string purchaserequisition_date { get; set; }
        public string costcenter_name { get; set; }
        public string purchaserequisition_referencenumber { get; set; }
        public string user_firstname { get; set; }
        public string purchaserequisition_remarks { get; set; }
        public string priority_remarks { get; set; }
        public string employee_name { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string costcenter_gid { get; set; }
        public string available_amount { get; set; }



    }
    public class purchaserequestitionview : result
    {
        public string purchaserequisition_gid { get; set; }
        public string product_name { get; set; }
        public string qty_requested { get; set; }
        public string product_code { get; set; }
        public string purchaserequisition_date { get; set; }
        public string purchaserequisition_remarks { get; set; }
        public string purchaserequisition_referencenumber { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string requested_by { get; set; }
        public string department_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string display_field { get; set; }
        public string priority_remarks { get; set; }
    }
}