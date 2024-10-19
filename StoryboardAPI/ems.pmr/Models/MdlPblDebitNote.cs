using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPblDebitNote : result
    {
        public List<Getdebitnote_list> Getdebitnote_list { get; set; }
        public List<Getraisedebitnote_list> Getraisedebitnote_list { get; set; }
        public List<GetRaiseDebitNoteAdd_list> GetRaiseDebitNoteAdd_list { get; set; }
        public List<GetDebitProduct_list> GetDebitProduct_list { get; set; }
        public List<Postdebit_list> Postdebit_list { get; set; }
        public List<GetDebitdtl_list> GetDebitdtl_list { get; set; }
        public List<GetStockReturnDebit_list> GetStockReturnDebit_list { get; set; }
        public List<GetStockReturnproduct_list> GetStockReturnproduct_list { get; set; }
    }
    public class Getdebitnote_list : result
    {
        public string invoice_gid { get; set; }
        public string debitnote_gid { get; set; }
        public string vendor_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string vendor_refnodate { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string mobile { get; set; }
        public string invoice_amount { get; set; }
        public string debit_amount { get; set; }
        public string debit_date { get; set; }
        public string invoice_from { get; set; }
        public string payment_gid { get; set; }
    }
    public class Getraisedebitnote_list
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_status { get; set; }
        public string vendor_companyname { get; set; }
        public string invoice_amount { get; set; }
        public string vendor_gid { get; set; }
        public string payment_amount { get; set; }
        public string outstanding { get; set; }
        public string contact { get; set; }
        public string debit_note { get; set; }
        public string costcenter_name { get; set; }
        public string vendor_refnodate { get; set; }

    }
    public class GetRaiseDebitNoteAdd_list : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string price { get; set; }
        public string total_amount { get; set; }
        public string debit_note { get; set; }
        public string invoice_from { get; set; }
        public string invoice_reference { get; set; }
        public string tax_amount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_name { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string invoice_amount { get; set; }
        public string currency_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string email_id { get; set; }
        public string mobile { get; set; }
        public string outstanding { get; set; }
        public string freightcharges { get; set; }
        public string buybackorscrap { get; set; }
        public string payment_amount { get; set; }
        public string costcenter_name { get; set; }
        public string vendor_refnodate { get; set; }
        public string product_remarks { get; set; }
        public string invoice_remarks { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string exchange_rate { get; set; }
        public string vendor_gid { get; set; }
        public double creditinvoice_amount { get; set; }
    }
    public class GetDebitProduct_list: result
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
    }
    public class Postdebit_list : result
    {
        public string invoice_gid { get; set; }
        public double outstanding { get; set; }
        public double creditinvoice_amount { get; set; }
        public string reasondebit { get; set; }
        public string debitnote_date { get; set; }
        public double exchange_rate { get; set; }
        public string payment_mode { get; set; }
        public string branch_gid { get; set; }
        public string vendor_gid { get; set; }
        public string invoice_refno { get; set; }
    }
    public class GetDebitdtl_list : result 
    {
        public string debitnotedtl_gid { get; set; }
        public string debitnote_gid { get; set; }
    }
    public class GetStockReturnDebit_list : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string price { get; set; }
        public string total_amount { get; set; }
        public string debit_note { get; set; }
        public string invoice_from { get; set; }
        public string invoice_reference { get; set; }
        public string tax_amount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_name { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string invoice_amount { get; set; }
        public string currency_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string email_id { get; set; }
        public string mobile { get; set; }
        public string outstanding { get; set; }
        public string freightcharges { get; set; }
        public string buybackorscrap { get; set; }
        public string payment_amount { get; set; }
        public string costcenter_name { get; set; }
        public string vendor_refnodate { get; set; }
        public string product_remarks { get; set; }
        public string remarks { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string exchange_rate { get; set; }
        public string vendor_gid { get; set; }
        public double creditinvoice_amount { get; set; }
    }
    public class GetStockReturnproduct_list : result
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
        public string product_total { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string uom_gid { get; set; }
        public double stockreturn_qty { get; set; }
    }
    public class PostReturnStockDebit_list : result
    {
        public string debitnote_gid { get; set; }
        public GetStockReturnproduct_list[] GetStockReturnproduct_list { get; set; }
    }
}