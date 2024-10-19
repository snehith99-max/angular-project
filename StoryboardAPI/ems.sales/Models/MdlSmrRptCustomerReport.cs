using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrRptCustomerReport
    {
        public List<Getcustomerreport_List> Getcustomerreport_List { get; set; }
        public List<Getsubbankbook_list> Getsubbankbook_list { get; set; }
    }

    public class Getsubbankbook_list : result
    {
        public int s_no { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string transaction_gid { get; set; }
        public string bank_gid { get; set; }
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string account_no { get; set; }
        public string account_name { get; set; }
        public string remarks { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string journal_type { get; set; }
        public string transaction_type { get; set; }
        public string bank_name { get; set; }
        public string transaction_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }

    }
    public class Getcustomerreport_List : result
    {
        public string account_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string customer { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
        public string contact { get; set; }
        public string opening_balance { get; set; }
        public string outstanding_amount { get; set; }
        public string payment_amount { get; set; }
        public string invoice_amount { get; set; }
        public string customer_gid { get; set; }
        public string sales_type { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
    }
}