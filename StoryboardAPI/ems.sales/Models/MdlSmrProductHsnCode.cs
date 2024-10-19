using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrProductHsnCode : result
    {
        public List<ProductHsnCode_summary> ProductHsnCode_summary { get; set; }
        public List<UpdateHsn_Code> UpdateHsn_Code { get; set; }
    }
    public class ProductHsnCode_summary : result
    {
        public string statuses { get; set; }
        public string producttype_name { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string product_price { get; set; }
        public string branch_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string productuomclass_code { get; set; }
        public string productuom_code { get; set; }
        public string productuomclass_name { get; set; }
        public string stockable { get; set; }
        public string productuom_name { get; set; }
        public string product_type { get; set; }
        public string Status { get; set; }
        public string tax { get; set; }
        public string productuomclass_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string sku { get; set; }
        public string product_desc { get; set; }
        public string productgroupname { get; set; }
        public string producttypename { get; set; }
        public string productuomclassname { get; set; }
        public string productuomname { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
        public string producttype_gid { get; set; }
        public string productuom_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string hsn_number { get; set; }
        public string hsn_desc { get; set; }
        public string gstproducttax_percentage { get; set; }

    }

    public class UpdateHsn_Code : result
    {
        public string product_gid { get; set; }
        public string product_hsngst { get; set; }
        public string product_hsncode_desc { get; set; }
        public string product_hsncode { get; set; }

        public List<hsncodelist> hsncodelist { get; set; }
    }
    public class hsncodelist : result
    {
        public string product_hsngst { get; set; }
        public string product_hsncode_desc { get; set; }
        public string product_hsncode { get; set; }
    }
}