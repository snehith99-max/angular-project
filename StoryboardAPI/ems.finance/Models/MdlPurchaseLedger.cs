using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlPurchaseLedger : result
    {

        public List<GetPurchaseLedgerfin_list> GetPurchaseLedgerfin_list { get; set; }
        public List<GetPurchaseLedgerView_list> GetPurchaseLedgerView_list { get; set; }
        public List<GetPurchaseLedgerProductView_list> GetPurchaseLedgerProductView_list { get; set; }
        public double grand_total { get; set; }
    }

    public class GetPurchaseLedgerfin_list : result
    {
        public string invoice_date { get; set; }
        public string invoice_gid { get; set; }
        public string account_gid { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string journal_gid { get; set; }
        public string vendor_gid { get; set; }
        public string account_name { get; set; }
        public string transaction_amount { get; set; }
        public string tax_amount { get; set; }
        public string net_amount { get; set; }
        public string journal_refno { get; set; }
    }
    public class GetPurchaseLedgerView_list : result
    {

        public string branch_name { get; set; }
        public string invoice_gid { get; set; }
        public string contact_telephonenumber { get; set; }
        public string invoice_date { get; set; }
        public string vendorinvoiceref_no { get; set; }
        public string invoice_refno { get; set; }
        public string contactperson_name { get; set; }
        public string exchange_rate { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_address { get; set; }
        public string currency_code { get; set; }
        public string payment_date { get; set; }
        public string payment_days { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_type { get; set; }
        public string email_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string product_totalprice { get; set; }
        public string discount_amount { get; set; }
        public string tax_name1 { get; set; }
        public string tax_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount3 { get; set; }
        public string product_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string additionalcharges_amount { get; set; }
        public string freightcharges { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string round_off { get; set; }
        public string invoice_amount { get; set; }
        public string termsandconditions { get; set; }
        public string price { get; set; }
        public string billing_email { get; set; }
        public string shipping_address { get; set; }
        public string payment_term { get; set; }
        public string mode_despatch { get; set; }
        public string tax_name4 { get; set; }
        public string delivery_term { get; set; }
    }
    public class GetPurchaseLedgerProductView_list : result
    {
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string display_field_name { get; set; }
        public string productgroup_name { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string product_total { get; set; }
        public string invoice_amount { get; set; }
        public string product_totalprice { get; set; }
        public string discount_amount { get; set; }
        public string tax_name1 { get; set; }
        public string tax_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount3 { get; set; }
        public string product_remarks { get; set; }
        public string termsandconditions { get; set; }
        public string additional_name { get; set; }
        public string extradiscount_amount { get; set; }
        public string extraadditional_amount { get; set; }
        public string discount_name { get; set; }
        public string round_off { get; set; }
        public string addon_amount { get; set; }
        public string buybackorscrap { get; set; }
        public string freightcharges { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string product_description { get; set; }
        public string discount_percentage { get; set; }

    }
}