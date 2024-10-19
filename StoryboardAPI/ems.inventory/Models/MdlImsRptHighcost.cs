using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptHighcost:result
    {
            public List<highcost_list> highcost_list { get; set; }
       
    }

    public class highcost_list : result
    {
        public string branch_name { get; set; }
        public string location_name { get; set; }
        public string display_field { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string remarks { get; set; }
        public string stock_qty { get; set; }
        public string max_price { get; set; }
        public string min_price { get; set; }
        public string price_variance { get; set; }
    }
}