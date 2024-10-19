using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptStockStatusReport :result
    {
        public List<stocklist> stocklist {  get; set; }
    }
    public class stocklist : result 
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string reference_gid { get; set; }
        public string grn_date { get; set; }
        public string branch_name { get; set; }
        public string vendor_companyname { get; set; }
        public string stock_qty { get; set; }
        public string productuom_name { get; set; }
    }
}