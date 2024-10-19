using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptClosingStock:result
    {
        public List<location> location {  get; set; }
        public List<closingstock_list> closingstock_list {  get; set; }
    }
    public class location : result
    {
        public string location_name { get; set; }
        public string location_gid { get; set; }
    }
    public class closingstock_list : result
    {
        public string stock_gid { get; set; }
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string branch_gid { get; set; }
        public string product_name { get; set; }
        public string branch_name { get; set; }
        public string productuom_name { get; set; }
        public string total_Stock_Quantity { get; set; }
        public string issued_Quantity { get; set; }
        public string amended_Quantity { get; set; }
        public string damaged_qty { get; set; }
        public string adjusted_qty { get; set; }
        public string available_quantity { get; set; }
        public string transfer_qty { get; set; }
        public string location_name { get; set; }
    }
}