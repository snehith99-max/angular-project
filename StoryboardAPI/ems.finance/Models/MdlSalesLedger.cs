using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlSalesLedger
    {
        public List<GetSaleLedgerfin_list> GetSaleLedgerfin_list { get; set; }
    }

    public class GetSaleLedgerfin_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_gid { get; set; }
        public string account_gid { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string journal_gid { get; set; }
        public string customer_gid { get; set; }
        public string account_name { get; set; }
        public string transaction_amount { get; set; }
        public string tax_amount { get; set; }
        public string net_amount { get; set; }
        public string journal_refno { get; set; }
    }
}