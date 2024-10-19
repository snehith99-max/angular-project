using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class Mdlproductreports:result
    {

        public List< GetproudctssellingDetailSummarylist> GetproudctssellingDetailSummarylist { get; set; }
        public List<GetproductssellingForLastSixMonths_List> GetproductssellingForLastSixMonths_List { get; set; }
        public List<productreport_list> productreport_list { get; set; }
        public List<ProductConsumptionReport_list> ProductConsumptionReport_list { get; set; }

        public List<ProductGroupwiseChart_list> ProductGroupwiseChart_list { get; set; }
        public List<ProductReportgrid_list> ProductReportgrid_list { get; set; }

    }
    public class productreport_list : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }
    public class GetproudctssellingDetailSummarylist : result
    {
        public string productgroup_name {  get; set; }
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string mrp_price { get; set; }


    }
    public class GetproductssellingForLastSixMonths_List : result
    {
        public string months { get; set; }
        public string productcounts { get; set; }
        public string most_used_product { get; set; }
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string unitprice { get; set; }
        public string product_name { get; set; }
        public string year { get; set; }
        

    }
    public class ProductGroupwiseChart_list : result
    {
        public string amount { get; set; }
        public string productgroup_name { get; set; }
        //public string product_gid { get; set; }

    }
    public class ProductConsumptionReport_list : result
    {
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string product_quantity { get; set; }
        public string available_quantity { get; set; }
        public string branch_gid { get; set; }
        public string product_gid { get; set; }


    }
    public class ProductReportgrid_list : result
    {
        public string customer_name { get; set; }
        public string product_qtydelivered { get; set; }
        public string product_gid { get; set; }

    }
}