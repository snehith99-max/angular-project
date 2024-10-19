using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.inventory.Models
{
    public class MdlImsTrnStockConsumptionReport : result
    {
        public List<stockconsumptionreport_list> stockconsumptionreport_list { get; set; }
    }
    public class stockconsumptionreport_list : result
    {
        public string quantity { get; set; }
        public string grn_gid { get; set; }
        public string product_price { get; set; }
        public string product_gid { get; set; }
        public string total_price { get; set; }
        public string branch_name { get; set; }
        public string productuom_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_gid { get; set; }
        public string display_field { get; set; }
        
    }
}