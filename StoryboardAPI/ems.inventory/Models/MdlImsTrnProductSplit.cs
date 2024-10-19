using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnProductSplit : result
    {
        public string product_gid { get; set; }
        public string stock_gid { get; set; }
        public string product_group { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string sku { get; set; }
        public int stock_balance { get; set; }
        public int incoming_quantity { get; set; }
        public int split_quantity { get; set; }
        public string branch_gid { get; set; }
        public string product_unit_text { get; set; }
        public string display_field { get; set; }
        public string reference_gid { get; set; }
        public List<GetLocationstocks> GetLocation { get; set; }
        public string uom_gid { get; set; }
    }

    public class GetLocationstocks : result
    { 
        public string productuom_name { get; set; }
        public string product_uom_gid { get; set; }
    }

}