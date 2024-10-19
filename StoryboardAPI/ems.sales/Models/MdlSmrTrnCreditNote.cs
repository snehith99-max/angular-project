using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnCreditNote : result
    {
        public List<creditnotesummary_list> creditnotesummary_list { get; set; }
        public List<addselectCNsummary_list> addselectCNsummary_list { get; set; }
        public List<addselectProdsummary_list> addselectProdsummary_list { get; set; }
        public List<postcreditnote_list> postcreditnote_list { get; set; }
        public List<GetCreditdtl_list> GetCreditdtl_list { get; set; }
    }
    public class creditnotesummary_list : result
    {
        public string credit_date { get; set; }
        public string creditnote_gid { get; set; }
        public string creditnotedtl_gid { get; set; }
        public string creditnote_no { get; set; }
        public string invoice_date { get; set; }
        public string outstanding { get; set; }
        public string invoiceref_no { get; set; }
        public string customer_name { get; set; }
        public string credit_amount { get; set; }
        public string payment_amount { get; set; }
        public string invoice_amount { get; set; }
        public string credit_note { get; set; }
        public string invoice_gid { get; set; }
        public string receipt_gid { get; set; }
        public string contact { get; set; }
        public string invoice_status { get; set; }
    }
    public class GetCreditdtl_list : result
    {
        public string creditnotedtl_gid { get; set; }
        public string creditnote_gid { get; set; }
    }
    public class addselectCNsummary_list : result
    {
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string invoice_date { get; set; }
        public string total_amount { get; set; }
        public string invoiceref_no { get; set; }
        public string customer_name { get; set; }
        public string credit_amount { get; set; }
        public string creditnote_gid { get; set; }
        public string payment_amount { get; set; }
        public string invoice_amount { get; set; }
        public string credit_note { get; set; }
        public string currency_code { get; set; }
        public string invoice_from { get; set; }
        public string invoice_reference { get; set; }
        public string price { get; set; }
        public string Tax_amount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_name { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string customer_contactperson { get; set; }
        public string invoice_remarks { get; set; }
        public string product_remarks { get; set; }
        public string outstanding { get; set; }
        public string mobile { get; set; }
        public string customer_email { get; set; }
        public string customer_address { get; set; }
        public string credit_date { get; set; }
        public string price_total { get; set; }
    }

    public class addselectProdsummary_list : result
    {
        public string invoicedtl_gid { get; set; }
        public string invoice_gid { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string price { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string uom_gid { get; set; }
        public string price_total { get; set; }
        public string product_total { get; set; }
    }

    public class postcreditnote_list : result
    {
        public string outstanding_amount { get; set; }
        public string creditnote_amount { get; set; }
        public string creditnote_date { get; set; }
        public string inv_remarks { get; set; }
        public string invoice_gid { get; set; }
        public string creditnote_gid { get; set; }
        public string stock_return { get; set; }
        public string product_price { get; set; }
        public string invoicedtl_gid { get; set; }
    }
}