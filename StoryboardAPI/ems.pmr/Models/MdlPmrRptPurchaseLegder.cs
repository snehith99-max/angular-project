using ems.pmr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrRptPurchaseLegder :result
    {
        public List<GetSaleLedger_list> GetSaleLedger_list { get; set; }
    }
    public class GetSaleLedger_list : result
    {

        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string vendor_name { get; set; }
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
        public string vendor_code { get; set; }
        public string vendor_gid { get; set; }
        public string account_name { get; set; }
        public string purchase_type { get; set; }
        public string vendor { get; set; }
        public string total_tax { get; set; }

    }
}