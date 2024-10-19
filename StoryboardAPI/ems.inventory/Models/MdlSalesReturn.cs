using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlSalesReturn : result
    {
        public List<GetSalesReturn_list> GetSalesReturn_list { get; set; }
        public List<GetSalesReturnView_list> GetSalesReturnView_list { get; set; }
        public List<GetSalesReturnViewDetails_list> GetSalesReturnViewDetails_list { get; set; }
        public List<GetSalesReturnAdd_list> GetSalesReturnAdd_list { get; set; }
        public List<GetSalesReturnAddSelect_list> GetSalesReturnAddSelect_list { get; set; }
        public List<GetSalesReturnAddDetails_list> GetSalesReturnAddDetails_list { get; set; }
        public List<PostSalesReturn_list> PostSalesReturn_list { get; set; }
        public List<getGetViewSRProduct_list> getGetViewSRProduct_list { get; set; }
        public List<GetViewreturnProduct_list> GetViewreturnProduct_list { get; set; }
    }
    public class GetSalesReturn_list: result
    {
        public string salesreturn_gid { get; set; }
        public string salesreturn_date { get; set; }
        public string deliveryorder_gid { get; set; }
        public string customer_name { get; set; }
        public string delivery_status { get; set; }
        public string contact { get; set; }
        public string branch_prefix { get; set; }
    }   
    public class GetSalesReturnView_list: result
    {
        public string customer_name { get; set; }
        public string directorder_gid { get; set; }
        public string deliveryorder_date { get; set; }
        public string salesreturn_gid { get; set; }
        public string salesreturn_date { get; set; }
        public string mobile { get; set; }
        public string customer_address { get; set; }
        public string customer_contactperson { get; set; }
        public string return_type { get; set; }
        public string remarks { get; set; }
    }
    public class GetSalesReturnViewDetails_list : result 
    {
        public string salesreturndtl_gid { get; set; }
        public string salesreturn_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string qty_returned { get; set; }
        public string product_qtydelivered { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string product_desc { get; set; }
        public string productgroup_name { get; set; }
    }
    public class GetSalesReturnAdd_list: result
    {
        public string directorder_date { get; set; }
        public string directorder_gid { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string delivery_status { get; set; }
    }
    public class GetSalesReturnAddSelect_list: result
    {
        public string customer_name { get; set; }
        public string directorder_gid { get; set; }
        public string directorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string customer_contactperson { get; set; }
        public string mobile { get; set; }
        public string customer_emailid { get; set; }
        public string customer_address { get; set; }
    }
    public class GetSalesReturnAddDetails_list : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_quoted { get; set; }
        public string product_qtydelivered { get; set; }
        public string qty_returned { get; set; }
        public string product_uom_gid { get; set; }
        public string product_description { get; set; }
        public string directorderdtl_gid { get; set; }
        public string salesorderdtl_gid { get; set; }        
        public double qty_returnsales { get; set; }        
        public string product_code { get; set; }        
        public string product_desc { get; set; }        
        public string productgroup_name { get; set; }
    }
    public class PostSalesReturn_list: result
    {
        public Productlist[] Productlist { get; set; }
        public string customer_name { get; set; }
        public string directorder_gid { get; set; }
        public string directorder_date { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorderdtl_gid { get; set; }
        public string customer_contactperson { get; set; }
        public string mobile { get; set; }
        public string customer_emailid { get; set; }
        public string customer_address { get; set; }
        public string Remarks { get; set; }
        public string product_description { get; set; }
        public string return_type { get; set; }
    }
    public class Productlist : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_quoted { get; set; }
        public string product_qtydelivered { get; set; }
        public string qty_returned { get; set; }
        public string product_uom_gid { get; set; }
        public string product_description { get; set; }
        public string directorderdtl_gid { get; set; }
        public string salesorderdtl_gid { get; set; }
        public string qty_returnsales { get; set; }
    }
    public class getGetViewSRProduct_list : result
    {
        public string product_name { get; set; }
        public string qty_returned { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string product_remarks { get; set; }
        public string productgroup_name { get; set; }
        public string product_desc { get; set; }

    }
    public class GetViewreturnProduct_list : result
    {
        public string product_name { get; set; }
        public string qty_returned { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string actual { get; set; }
        public string product_qtydelivered { get; set; }
        public string product_qty { get; set; }
        public string productgroup_name { get; set; }
        public string product_description { get; set; }
        public string product_desc { get; set; }

    }

}