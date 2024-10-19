using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrRptSalesLedger
    {
        public List<GetSaleLedger_list> GetSaleLedger_list { get; set; }

        public string message {  get; set; }
    }
    public class GetSaleLedger_list:result
     {

        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string invoice_gid { get; set; }
        public string total_amount { get; set; }
        public string discount_amount_L { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_name2 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_amount2 { get; set; }
        public string invoice_amount { get; set; }
        public string customer_code { get; set; }
        public string customer_gid { get; set; }
        public string account_name { get; set; }
        public string customer { get; set; }
        public string total_tax { get; set; }
        public string sales_type { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }

    }
}