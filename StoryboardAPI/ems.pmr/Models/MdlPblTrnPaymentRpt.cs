﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPblTrnPaymentRpt : result
    {
        public List<paymentrpt_list> paymentrptlist { get; set; }
        public List<GetInvoice> getinvoice { get; set; }
        public List<paymentadd> paymentadd { get; set; }
        public List<payment_lists> paymentlists { get; set; }
        public List<paymentamount_list> paymentamount_list { get; set; }
        public List<paymentExpand> paymentExpand { get; set; }
        public List<productdetail_list> productdetail_list { get; set; }
        public List<singleinvoicelist> singleinvoicelist { get; set; }
        public List<singlepayment_list> singlepaymentlist { get; set; }
        public List<GetBankdtldropdown> GetBankNameVle { get; set; }
        public List<GetCarddtldropdown> GetCardNameVle { get; set; }
        public List<multipleinvoice2singlepayment> multipleinvoice2singlepayment { get; set; }

        public List<GetMultipleInvoiceSummary> GetMultipleInvoiceSummary { get; set; }
        public List<Getmultipleinvoice2employeedtl> Getmultipleinvoice2employeedtl { get; set; }
        public List<paymentcancel> paymentcancel { get; set; }


    }
    public class paymentcancel : result
    {
        public string city_name { get; set; }
        public string payment_reference { get; set; }
        public string payment_total { get; set; }
        public string payment_mode { get; set; }
        public string payment_remarks { get; set; }
        public string vendor_gid { get; set; }
        public string payment_date { get; set; }
        public string payment_gid { get; set; }
        public string fax { get; set; }
        public string email_id { get; set; }
        public string vendoraddress { get; set; }
        public string contactpersonname { get; set; }
        public string contact_telephonenumber { get; set; }
        public string vendor_companyname { get; set; }
        public string country { get; set; }
        public string address1 { get; set; }
        public string name { get; set; }

    }
    public class multipleinvoice2singlepayment : result
    {
        public List<GetMultipleInvoiceSummary> GetMultipleInvoiceSummary { get; set; }
        public List<Getmultipleinvoice2employeedtl> Getmultipleinvoice2employeedtl { get; set; }
        public List<paymentdtls> paymentdtls { get; set; }
        public string payment_mode { get; set; }
        public string payment_date1 { get; set; }
        public string bankname { get; set; }
        public string branch_name { get; set; }
        public string payment_remarks { get; set; }
        public string payment_note { get; set; }
        public string cheque_no { get; set; }
        public string dd_no { get; set; }
        public string priority { get; set; }
        public string vendor_gid { get; set; } 
        public string textbox { get; set; }
    }
        public class paymentdtls : result
        {
        public string payment_mode { get; set; }
        public string payment_date1 { get; set; }
        public string bankname { get; set; }
        public string branch_name { get; set; }
        public string cheque_no { get; set; }
        public string dd_no { get; set; }
        public string priority { get; set; }
        public string textbox { get; set; }

    }
        public class Getmultipleinvoice2employeedtl : result
    {
        public string email_id { get; set; }
        public string vendor_gid { get; set; }
        public string payment_remarks { get; set; }
        public string paymentnotes { get; set; }

        public string name { get; set; }
        public string currency  { get; set; }
        public string vendor_companyname { get; set; }
        public string contact_telephonenumber { get; set; }
        public string vendorcontactdetails { get; set; }
        public string vendoraddress { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string fax { get; set; }
        public string city { get; set; }
        public string payment_date { get; set; }

    }

    public class GetMultipleInvoiceSummary : result
    {
        public string invoice_gid { get; set; }
        public double advance { get; set; }
        public string invoice_refno { get; set; }
        public string purchaseorder_gid { get; set; }
        public string invoice_date { get; set; }
        public double invoice_amount { get; set; }
        public string invoice_status { get; set; }
        public double outstanding { get; set; }
        public double payed_amount { get; set; }
        public string payment_amount { get; set; }
        public double balancepo_advance { get; set; }
        public double grand_total { get; set; }
        public string tds_amount { get; set; }
        public string exchangeloss { get; set; }
        public string bankcharges { get; set; }
        public string exchangegain { get; set; }
        public string final_amount { get; set; }
        public double totalpo_advance { get; set; }
        public string remark { get; set; }

    }
    public class paymentrpt_list : result
    {
        public string payment_gid { get; set; }
        public string invoice_gid { get; set; }
        public string payment_date { get; set; }
        public string vendor_gid { get; set; }
        public string payment_remarks { get; set; }
        public string payment_total { get; set; }
        public string payment_status { get; set; }
        public string user_gid { get; set; }
        public string created_date { get; set; }
        public string payment_reference { get; set; }
        public string purchaseorder_gid { get; set; }
        public string advance_total { get; set; }
        public string payment_mode { get; set; }
        public string bank_name { get; set; }
        public string branch_name { get; set; }
        public string cheque_no { get; set; }
        public string city_name { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string tds_amount { get; set; }
        public string tdscalculated_finalamount { get; set; }
        public string priority { get; set; }
        public string priority_remarks { get; set; }
        public string approved_by { get; set; }
        public string approved_date { get; set; }
        public string reject_reason { get; set; }
        public string bank_gid { get; set; }
        public string addon_amount { get; set; }
        public string additional_discount { get; set; }
        public string additional_gid { get; set; }
        public string discount_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string payment_amount { get; set; }      
        public string invoice_amount { get; set; }      
         

    }

    public class GetInvoice : result
    {
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string invoice_amount { get; set; }
    }

    public class paymentadd : result
    {
        public string invoice_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string invoice_status { get; set; }
        public string invoice_amount { get; set; }
        public string payment_amount { get; set; }
        public string outstanding { get; set; }
        public string invoice_from { get; set; }
        public string contact { get; set; }
        public string purchaseorder_gid { get; set; }
    }

    public class payment_lists : result
    {
        public string payment_gid { get; set; }
        public string payment_date { get; set; }
        public string vendor_gid { get; set; }
        public string payment_remarks { get; set; }
        public string payment_mode { get; set; }
        public string payment_status { get; set; }
        public string tds_amount { get; set; }
        public string payment_total { get; set; }
        public string payment_reference { get; set; }
        public string city_name { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string address1 { get; set; }
        public string country { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string fax { get; set; }
        public string vendoraddress { get; set; }
        public string email_id { get; set; }
        public string contact_telephonenumber { get; set; }
        public string payment_note { get; set; }
        public string employee_name { get; set; }


    }
    public class paymentamount_list : result
    {
        public string payment_reference { get; set; }
        public string invoice_remarks { get; set; }
        public string payment_amount { get; set; }
        public string po_advance { get; set; }
        public string invoice_gid { get; set; }
        public string invoice_amount { get; set; }
        public string paymentdtl_gid { get; set; }
        public string payment_gid { get; set; }
        public string invoice2payment_gid { get; set; }
        public string tds_amount { get; set; }
        public string exchangegain { get; set; }
        public string bankcharges { get; set; }
        public string exchangeloss { get; set; }

    }
    public class paymentExpand : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string invoice_from { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_contact_person { get; set; }
        public string vendor_address { get; set; }
        public string invoice_remarks { get; set; }
        public string overall_status { get; set; }
        public string payed_amount { get; set; }
        public string outstanding { get; set; }
        public string due_date { get; set; }
        public string invoice_amount { get; set; }
    }

    public class productdetail_list : result
    {
        public string invoice_gid { get; set; }
        public string invoicedtl_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string invoice_refno { get; set; }
        public string product_total { get; set; }
        public string qty_invoice { get; set; }
        public string product_name { get; set; }
    }

    public class singlepayment_list : result
    {
        public string invoice_gid { get; set; }
        public string invoice_status { get; set; }
        public string invoice_amount { get; set; }
        public string vendor_gid { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_from { get; set; }
        public string purchaseorder_gid { get; set; }
        public string invoice_date { get; set; }
        public string payed_amount { get; set; }
        public string vendor_companyname { get; set; }
        public string outstanding { get; set; }
    }
    public class GetBankdtldropdown : result
    {
        public string bank_name { get; set; }
        public string bank_gid { get; set; }
    }
    public class GetCarddtldropdown : result
    {
        public string bank_gid { get; set; }
        public string bank_name { get; set; }
    }


    public class singleinvoicelist : result
    {
        public string invoice_gid { get; set; }
        public string vendor_gid { get; set; }
        public string invoice_status { get; set; }
        public string invoice_amount { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_from { get; set; }
        public string invoice_date { get; set; }
        public string payed_amount { get; set; }
        public string vendor_companyname { get; set; }
        public string outstanding { get; set; }
        public string due_date { get; set; }
        public string invoice_refno { get; set; }
        public string advance_amount { get; set; }
        public string tds_amount { get; set; }
        public string exchangeloss { get; set; }
        public string exchangegain { get; set; }
        public string bankcharges { get; set; }
    }


}