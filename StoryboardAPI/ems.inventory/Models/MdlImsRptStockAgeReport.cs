using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.inventory.Models
{
    public class MdlImsRptStockAgeReport : result
    {
        public List<stockage_list> stockage_list { get; set; }
    }
    public class stockage_list : result
    {
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string product_price { get; set; }
        public string product_gid { get; set; }
        public string total_price { get; set; }
        public string branch_name { get; set; }
        public string productuom_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string totaldays { get; set; }
        public string display_field { get; set; }
        public string location_name { get; set; }
        public string stock_quantity { get; set; }
        public string stocktype_name { get; set; }
        public string branch_prefix { get; set; }

    }
}