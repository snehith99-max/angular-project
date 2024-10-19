using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnStoreRequisition : result
    {

        public List<srsummary_list> srsummary_list { get; set; }
        public List<getrolproductgroup_list> getrolproductgroup_list { get; set; }
        public List<getrolproduct_list> getrolproduct_list { get; set; }
        public List<getrolproduct1_list> getrolproduct1_list { get; set; }
        public List<srproductsingle_list> srproductsingle_list { get; set; }
        public List<tmpsrproduct_list> tmpsrproduct_list { get; set; }
        public List<storeRequisition_list> storeRequisition_list { get; set; }
        public List<storeRequisitionproduct_list> storeRequisitionproduct_list { get; set; }
        public List<GetStoreView_list> GetStoreView_list { get; set; }
        public List<GetstoreViewProduct_list> GetstoreViewProduct_list { get; set; }


    }

    public class srsummary_list : result
    {
        public string storerequisition_gid { get; set; }
        public string store { get; set; }
        public string storerequisition_status { get; set; }
        public string branch_gid { get; set; }
        public string storerequisition_date { get; set; }
        public string branch_name { get; set; }
        public string branch_prefix { get; set; }
        public string created_by { get; set; }
        public string user_firstname { get; set; }
        public string created_date { get; set; }
    }


    public class getrolproductgroup_list : result
    {

        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }

    }
    public class getrolproduct_list : result
    {

        public string product_name { get; set; }
        public string product_gid { get; set; }

    }
    public class getrolproduct1_list : result
    {

        public string rol_gid { get; set; }
        public string reorder_level { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string available_quantity { get; set; }
        public string productgroup_gid { get; set; }
        public string product_desc { get; set; }

    }
    public class srproductsingle_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public double qty_requested { get; set; }
        public string display_field { get; set; }
        public double stock_quantity { get; set; }
        public string reorder_level { get; set; }
        public string product_desc { get; set; }

    }
    public class tmpsrproduct_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string tmpmaterialrequisition_gid { get; set; }
        public string product { get; set; }
        public string product_remarks { get; set; }
        public string display_field { get; set; }
        public string stock_quantity { get; set; }
        public string tmpsr_gid { get; set; }
        public string reorder_level { get; set; }
        public string available_quantity { get; set; }

    }
    public class storeRequisition_list : result
    {
        public List<tmpsrproduct_list> tmpsrproduct_list { get; set; }
        public string storerequisition_date { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string user_firstname { get; set; }
        public string storerequisition_remarks { get; set; }
    }
    public class storeRequisitionproduct_list : result
    {
        public string storerequisition_gid { get; set; }
        public string storerequisitiondtl_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string qty_requested { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
    }
    public class GetStoreView_list : result
    {
        public string storerequisition_gid { get; set; }
        public string branch_gid { get; set; }
        public string storerequisition_date { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string created_by { get; set; }
        public string user_firstname { get; set; }
        public string priority_remarks { get; set; }

    }
    public class GetstoreViewProduct_list : result
    {
        public string storerequisitiondtl_gid { get; set; }
        public string storerequisition_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string display_field { get; set; }
    }
}