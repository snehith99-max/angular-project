using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrVendor360 : result
    {
        public List<VendorTilesCount> VendorTilesCount { get; set; }
        public List<vendordetails> vendordetails { get; set; }
        public List<purchasecount> purchasecount { get; set; }
        public List<paymentcount_list> paymentcount_list { get; set; }
    }

    public class VendorTilesCount : result
    {
        public string PO_count { get; set; }
        public string paymentCount { get; set; }
        public string paymentamount { get; set; }
        public string totalamount { get; set; }
        public string invoicecount { get; set; }
        public string invoiceamount { get; set; }
        public string purchasequotation { get; set; }
        public string quotationamount { get; set; }
    }

    public class vendordetails : result
    {
        public string vendor_code { get; set; }
        public string email_id { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string created_date { get; set; }
    }
    public class purchasecount : result
    {
        public string Months { get; set; }
        public string invoice_count { get; set; }
        public string po_count { get; set; }
    }
    public class paymentcount_list : result
    {
        public string cancelled_payment { get; set; }
        public string approved_payment { get; set; }
        public string completed_payment { get; set; }


    }
}
