//using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;

namespace ems.finance.Models
{
    public class MdlAccTrnSundryExpenses : result
    {
        public List<expense_list> expense_list { get; set; }
        public List<GetAccGroup> GetAccGroup { get; set; }
        public List<GetBranchname> GetBranchname { get; set; }
        public List<Getproducttype> Getproducttype { get; set; }
        public List<GetVendorname> GetVendorname { get; set; }
        public List<Getcurrencycode> Getcurrencycode { get; set; }
        public List<GetVendorChange> GetVendorChange { get; set; }
        public List<GetOnchangeCurrency> GetOnchangeCurrency { get; set; }
        public List<sundryexpenses_list> sundryexpenses_list { get; set; }
        public List<Gettemporarysummary> Gettemporarysummary { get; set; }
        public List<GetProductsearchlist> GetProductsearchlist { get; set; }
        public List<GetTaxSegmentListDetails> GetTaxSegmentListDetails { get; set; }
        public List<posttempledger_list> posttempledger_list { get; set; }
        public List<sundryledgerview_list> sundryledgerview_list { get; set; }
        public double grand_total { get; set; } 
    }

    public class expense_list   
    {
        public string expense_gid { get; set; }
        public string expensedtl_gid { get; set; }
        public string expense_date { get; set; }
        public string expense_reference { get; set; }
        public string due_date { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string product_total { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string ship_to { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string invoice_remarks { get; set; }
        public string currency_code { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string payment_term { get; set; }
        public string exchange_rate { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string tax_name2 { get; set; }
        public string discount_amount { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string display_field { get; set; }
        public string product_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string tmpexpensedtl_gid { get; set; }
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string Amount { get; set; }
        public string branch_gid { get; set; }
    }
    public class Gettemporarysummary : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string account_group { get; set; }
        public string maingroup_gid { get; set; }
        public string maingroup_name { get; set; }
        public string subgroup_gid { get; set; }
        public string subgroup_name { get; set; }
        public string Amount { get; set; }
        public string remarks { get; set; }
        public string tax_name2 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string product_total { get; set; }
        public string expensedtl_gid { get; set; }
        public string tmpexpensedtl_gid { get; set; }
    }
    public class sundryexpenses_list : result
    {
        public string expense_refno { get; set; }
        public string expensedtl_gid { get; set; }
        public string branch_name { get; set; }
        public string expense_ref_no { get; set; }
        public string expense_date { get; set; }
        public string vendor_companyname { get; set; }
        public string address1 { get; set; }
        public string shipping_address { get; set; }
        public string expense_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string invoice_remarks { get; set; }
        public string due_date { get; set; }
        public string payment_term { get; set; }
        public string tmpexpensedtl_gid { get; set; }
        public string grandtotal { get; set; }

        public string Account_grp { get; set; }
        public string Account_name { get; set; }
        public string product_name { get; set; }
        public string unitprice { get; set; }
        public string discount { get; set; }
        public string tax { get; set; }
        public string taxamount2 { get; set; }
        public string taxname2 { get; set; }
        public string taxgid1 { get; set; }
        public string taxgid2 { get; set; }
        public string hsn { get; set; }
        public string tax_amount { get; set; }
        public string taxamount1 { get; set; }
        public string product_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string uom_name { get; set; }
        public string productquantity { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }
        public string selling_price { get; set; }
        public string product_total { get; set; }
        public string tax_name { get; set; }
        public string taxprecentage1 { get; set; }
        public string taxprecentage2 { get; set; }
        public string producttotal_amount { get; set; }
        public ledgersEdit_list[] ledgersEdit_list {  get; set; }

    }
    public class ledgersEdit_list : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string expensedtl_gid { get; set; }
        public string expense_gid { get; set; }
        public string subgroup_gid { get; set; }
        public string Amount { get; set; }
        public string remarks { get; set; }
    }
    public class GetAccGroup : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetBranchname : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }
    public class Getproducttype : result
    {
        public string producttype_name { get; set; }
        public string producttype_code { get; set; }
        public string producttype_gid { get; set; }
    }
    public class GetVendorname : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
    }
    public class Getcurrencycode : result
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }
        public string default_currency { get; set; }
        public string exchange_rate { get; set; }
    }
    public class GetVendorChange : result
    {
        public string address { get; set; }
        public string vendorcontact { get; set; }
        public string phone { get; set; }
        public string taxsegment_gid { get; set; }
        public string vendor_gid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string gst_no { get; set; }
        public string fax { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_companyname { get; set; }
        public string contact_telephonenumber { get; set; }
        public string country_name { get; set; }
        public string email_id { get; set; }
        public string currencyexchange_gid { get; set; }

    }
    public class GetOnchangeCurrency
    {

        public string exchange_rate { get; set; }
        public string currency_code { get; set; }

    }

    public class GetProductsearchlist
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string unitprice { get; set; }
        public string producttype_gid { get; set; }
        public double taxamount1 { get; set; }
        public double taxamount2 { get; set; }
        public double quantity { get; set; }
        public double total_amount { get; set; }
        public double discount_amount { get; set; }
        public double discount_persentage { get; set; }

    }

    public class GetTaxSegmentListDetails : result
    {
        public string product_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }
        public string product_name { get; set; }

    }
    public class posttempledger_list : result
    {
        public string Account_grp { get; set; }
        public string Account_name { get; set; }
        public string remarks { get; set; }
        public string total_amount { get; set; }
        public string expense_date { get; set; }
        public string expense_ref_no { get; set; }
        public string branch_gid { get; set; }
        public string account_gid { get; set; }
    }
    public class sundryledgerview_list : result
    {
        public string expensedtl_gid { get; set; }
        public string expense_gid { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string maingroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string Amount { get; set; }
        public string remarks { get; set; }
        public string subgroup_gid { get; set; }
    }
}