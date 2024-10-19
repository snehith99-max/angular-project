using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnSales360 : result
    {
        public List<overall_list> overalllist { get; set; }
        public List<Getenquirydetails_list> Getenquirydetailslist { get; set; }
        public List<getquote_list> getquotelist { get; set; }
        public List<orderdetail_list> orderdetaillist { get; set; }
        public List<tilescount_list> tilescountlist { get; set; }
        public List<invoicedetail_list> invoicedetaillist { get; set; }
        public List<customerchart> customerchart { get; set; }
        public List<SmrNotes> SmrNotes { get; set; }
        public List<SmrEnquiry_list> SmrEnquiry_list { get; set; }
        public List<SmrDocumentList> SmrDocumentList { get; set; }
        public List<SmrQuotationList> SmrQuotationList { get; set; }
        public List<SmrOrderList> SmrOrderList { get; set; }
        public List<SmrInvoiceList> SmrInvoiceList { get; set; }
        public List<salescount> salescount { get; set; }
        public List<paymentcounts_list> paymentcounts_list { get; set; }

        public string customer_gid { get; set; }
    }

    public class paymentcounts_list : result
    {
        public string cancelled_payment { get; set; }
        public string approved_payment { get; set; }
        public string completed_payment { get; set; }


    }
    public class salescount : result
    {
        public string Months { get; set; }
        public string quotation_count { get; set; }
        public string so_count { get; set; }
        public string invoice_count { get; set; }
    }
    public class SmrInvoiceList
    {
        public string invoice_gid { get; set; }
        public string so_referencenumber { get; set; }
        public string invoice_refno { get; set; }
        public string mail_status { get; set; }
        public string customer_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_reference { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string overall_status { get; set; }
        public string payment_flag { get; set; }
        public string initialinvoice_amount { get; set; }
        public string invoice_status { get; set; }
        public string invoice_flag { get; set; }
        public string invoice_amount { get; set; }
        public string customer_name { get; set; }
        public string currency_code { get; set; }
        public string mobile { get; set; }
        public string invoice_from { get; set; }
        public string directorder_gid { get; set; }
        public string progressive_invoice { get; set; }



    }
    public class SmrOrderList
    {
        public string salesorder_gid { get; set; }
        public string so_referenceno1 { get; set; }
        public string salesorder_date { get; set; }
        public string user_firstname { get; set; }
        public string so_typecurrency_code { get; set; }
        public string customer_contact_person { get; set; }
        public string salesorder_status { get; set; }
        public string currency_code { get; set; }
        public string Grandtotal { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string invoice_flag { get; set; }
        public string customer_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadbankcontact_gid { get; set; }

    }
    public class SmrQuotationList : result
    {
        public string quotation_gid { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string quotation_date { get; set; }
        public string user_firstname { get; set; }
        public string quotation_type { get; set; }
        public string currency_code { get; set; }
        public string Grandtotal { get; set; }
        public string customer_name { get; set; }
        public string customer_contact_person { get; set; }
        public string quotation_status { get; set; }
        public string enquiry_gid { get; set; }
        public string contact { get; set; }
        public string customer_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string leadbankcontact_gid { get; set; }

    }
    public class SmrDocumentList
    {
        public string document_gid { get; set; }
        public string document_title { get; set; }
        public string document_upload { get; set; }
        public string leadbank_gid { get; set; }
        public string remarks { get; set; }
        public string created_date { get; set; }
        public string document_type { get; set; }

        //public string document_upload { get; set; }
    }
    public class SmrEnquiry_list
    {
        public string enquiry_refno { get; set; }
        public string enquiry_gid { get; set; }
        public string customer_gid { get; set; }
        public string customercontact_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string enquiry_date { get; set; }
        public string enquiry_status { get; set; }
        public string enquiry_type { get; set; }
        public string quotation_date { get; set; }
        public string contact_details { get; set; }
        public string user_firstname { get; set; }

    }

    public class SmrNotes : result
    {
        public string internal_notes { get; set; }
        public string customer_gid { get; set; }
        public string internalnotestext_area { get; set; }
        public string leadgig { get; set; }
        public string s_no { get; set; }
        public string source { get; set; }
    }
    public class customerchart
    {
        public string quotation_count { get; set; }
        public string  enquiry_count { get; set; }
        public string order_count { get; set; }
        public string invoice_count { get; set; }
        public string Months { get; set; }


    }
    public class overall_list : result
    {
        public string customer_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string customercontact_name { get; set; }
        public string customercontact_gid { get; set; }
        public string emailid { get; set; }
        public string mobile { get; set; }
        public string customer_type { get; set; }
        public string source { get; set; }
        public string created_date { get; set; }
        public string region { get; set; }
        public string designation { get; set; }
    }

    public class Getenquirydetails_list : result
    {
        public string enquiry_gid { get; set; }
        public string customer_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string enquiry_date { get; set; }
        public string contact_details { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string created_date { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string created_by { get; set; }
        public string potorder_value { get; set; }
        public string lead_status { get; set; }
        public string enquiry_status { get; set; }
        public string customer_rating { get; set; }
        public string assign_to { get; set; }
    }

    public class getquote_list : result
    {
        public string quotation_gid { get; set; }
        public string quotation_date { get; set; }
        public string customer_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string enquiry_gid { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string quotation_type { get; set; }
        public string created_by { get; set; }
        public string quotation_status { get; set; }
        public string Grandtotal { get; set; }
        public string assign_to { get; set; }
    }

    public class orderdetail_list : result
    {
       public string customer_gid { get; set; }
       public string leadbank_gid { get; set; }
       public string salesorder_gid { get; set; }
       public string salesorder_date { get; set; }
       public string so_referenceno1 { get; set; }
       public string branch_name { get; set; }
       public string contact { get; set; }
       public string so_type { get; set; }
       public string Grandtotal { get; set; }
       public string user_firstname { get; set; }
       public string salesorder_status { get; set; }
       public string salesperson_name { get; set; }

    }

    public class tilescount_list : result
    {
        public string quotataioncount { get; set; }
        public string salesordercount { get; set; }
        public string quoteamount { get; set; }
        public string salesorderamount { get; set; }
        public string invoicecount { get; set; }
        public string invoiceamount { get; set; }
        public string proposalcount { get; set; }

    }

    public class invoicedetail_list : result
    {
        public string directorder_gid { get; set; }
        public string directorder_date { get; set; }
        public string leadbank_gid { get; set; }
        public string customer_gid { get; set; }
        public string directorder_refno { get; set; }
        public string Grandtotal { get; set; }
        public string so_referenceno1 { get; set; }
        public string invoice_type { get; set; }
    }

}