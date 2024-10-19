using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrRptVendorledgerreport : result
    {
        public List<vendorledger_list> vendorledger_list { get; set; }
    }
    public class vendorledger_list : result
    {
        public string vendor { get; set; }
        public string vendor_refno { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_code { get; set; }
        public string vendor_address { get; set; }
        public string contact_details { get; set; }
        public string products { get; set; }
        public string order_value { get; set; }        
        public string invoice_amount { get; set; }
        public string vendor_companyname { get; set; }
        public string payment_amount { get; set; }
        public string opening_balance { get; set; }
        public string outstanding_amount { get; set; }
        public string contact { get; set; }
        public string invoice_reference { get; set; }
        public string invoice_date { get; set; }
        public string purchasetype_name { get; set; }

    }
}