﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlProductReport : result
    {
        public List<ProductConsumptionReport_list> ProductConsumptionReport_list { get; set; }
        public List<ProductReportgrid_list> ProductReportgrid_list { get; set; }
        public List<ProductGroupwiseChart_list> ProductGroupwiseChart_list { get; set; }
        public List<productdropdown_list> productdropdown_list { get; set; }
        public List<productpurchaseorder_list> productpurchaseorder_list { get; set; }
        public List<productSaleseorder_list> productSaleseorder_list { get; set; }
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
    public class ProductGroupwiseChart_list : result
    {
        public string amount { get; set; }
        public string productgroup_name { get; set; }
        //public string product_gid { get; set; }

    }
    public class productdropdown_list : result
    {

        public string product_gid { get; set; }
        public string product_name { get; set; }

    }

    public class productpurchaseorder_list: result
    {
        public string purchaseorder_date { get; set; }
        public string vendor_companyname { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string product_price { get; set; }

    }

    public class productSaleseorder_list: result
    {
        public string salesorder_date { get; set; }
        public string customer_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string product_price { get; set; }


    }
}