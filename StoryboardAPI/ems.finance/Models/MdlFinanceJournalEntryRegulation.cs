using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlFinanceJournalEntryRegulation
    {
        // public List<Receivableinvoicedtl> Rblinvoicedtl { get; set; } 
    }
    public class invoicedtl : results
    {
        public string invoice_gid { get; set; }
        public string tax_gid { get; set; }
        public string tax_gid2 { get; set; }
        public string tax_gid3 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }

        public string invoice_date { get; set; }
    }
    public class Customer : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_account_gid { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string customer_code { get; set; }
    }
    public class Vendor : results
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_code { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string vendor_name { get;set; }

    }
    public class Tax : results
    {
        public string tax_account_gid { get; set; }
        public string account_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
    }
    public class AccMapping : results
    {
        public string field_name { get; set; }
        public string accmap_gid { get; set; }
    }
  public class dateform:results
    {
        public string todate {  get; set; }
    }
    public class salarydtl:results
    {
        public string statutory_amount_employee { get; set; }
        public string statutory_amount_employer { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string account_gid { get; set; }
        public string account_gid_employer { get; set; }
        public string employee_gid { get; set; }

    }

}