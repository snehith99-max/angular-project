using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlReceipt:result
    {
        public List<receiptsummary_list> receiptsummary_list { get; set; }
        public List<receiptaddsummary_list> receiptaddsummary_list { get; set; }
        public List<Getmodeofpaymentlist> Getmodeofpaymentlist { get; set; }
        public List<makereceipt_list> makereceipt_list { get; set; }
        public List<updatereceipt_list> updatereceipt_list { get; set; }
        public List<invoice_list> invoice_list { get; set; }
        public List<receiptapprovallist> receiptapprovallist { get; set; }
        public List<invoicereceipt_list> invoicereceipt_list { get; set; }
        public List<invoicereceiptsummary_list> invoicereceiptsummary_list { get; set; }
        public List<GetBankdtldropdown> GetBankNameVle { get; set; }
        public List<GetCarddtldropdown> GetCardNameVle { get; set; }
        public List<receiptapprove_list> receiptapprove_list { get; set; }
        public string receipt_date { get; set; }
        public string defaultexchangerate {  get; set; }    
        public string payment_date { get; set; }
        public string payment_mode { get; set; }
        public string invoice_from { get; set; }
        public string cheque_date { get; set; }
        public string cheque_no { get; set; }
        public string bank_name { get; set; }
        public string branch_name { get; set; }
        public string payment_remarks { get; set; }
        public string cusbank_name { get; set; }
        public string neft_date { get; set; }
        public string trancsaction_no { get; set; }
        public string directorder_gid { get; set; }
        public string modepayemnt { get; set; }
        public string defaultcurrency { get; set; }
        public string salesorder_gid { get; set; }
        public List<ReceiptView_list> ReceiptView_list { get; set; }
    }
    public class ReceiptView_list : result
    {
        public string payment_date { get; set; }
        public string customer_gid { get; set; }
        public string currency_code { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string payment_remarks { get; set; }
        public string exchange_rate { get; set; }
        public string branch_name {  get; set; }
        public string customer_address {  get; set; }
        public string neft_transcationid {  get; set; }
        public string cash_date {  get; set; }
        public string dbank_gid {  get; set; }
        public string cheque_date {  get; set; }
        public string branch {  get; set; }
        public string bank_name {  get; set; }
        public string payment_mode {  get; set; }
        public string cheque_number {  get; set; }
    }
    public class Getmodeofpaymentlist : result
    {
        public string modeofpayment_gid { get; set; }
        public string payment_type { get; set; }
    }
    public class receiptapprovallist : result
    {
        public string payment_gid { get; set; }
        public string payment_mode { get; set; }
        public string customercontact_name { get; set; }
        public string mobile { get; set; }
        public string payment_type { get; set; }
        public string amount { get; set; }
        public string total_pay { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string approval_status { get; set; }
        public string invoice_gid { get; set; }
        public string outstanding_amount { get; set; }
        public string grand_total { get; set; }
        public string payment_date { get; set; }
        public string tds_amount { get; set; }
        public string adjust_amount { get; set; }
        public string bank_charge { get; set; }
        public string exchange_loss { get; set; }
        public string exchange_gain { get; set; }
        public string total_amount { get; set; }
        public string payment_remarks { get; set; }
        public string dbank_gid { get; set; }
        public string invoice_refno { get; set; }
        public string advance_received { get; set; }
        public string invoice_status { get; set; }
        public string invoice_amount { get; set; }
        public string invoiceamount_basecurrency { get; set; }
        public string receivedamount_bank { get; set; }
        public string paymentamont_basecur { get; set; }
        

    }
    public class receiptapprove_list : result
    {
        public string adjust_amount { get; set; }
        public string payment_gid { get; set; }
        public string tds_amount { get; set; }
        public string paid_amount { get; set; }
        public string bank_charge { get; set; }
        public string exchange_loss { get; set; }
        public string exchange_gain { get; set; }
        public string invoice_gid { get; set; }
        public string total_amount { get; set; }
        public string approval_status { get; set; }
        public string payment_date { get; set; }
        public string branch_gid { get; set; }
        public string account_gid { get; set; }
        public string customer_gid { get; set; }
        public string dbank_gid { get; set; }
        public string payment_remarks { get; set; }
        public string payment_type { get; set; }
        public string payment_mode { get; set; }

    }
    public class receiptsummary_list : result
    {
        public string payment_gid { get; set; }
        public string payment_mode { get; set; }
        public string invoice_refno { get; set; }
        public string payment_date { get; set; }
        public string payment_type { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string total_amount { get; set; }
        public string approval_status { get; set; }
        public string invoice_gid { get; set; }

    }

    public class receiptaddsummary_list : result 
    {
        public string invoice_from { get; set; }
        public string customer_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_status { get; set; }
        public string invoice_amount { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string payment_amount { get; set; }
        public string invoice_count { get; set; }
        public string outstanding { get; set; }
    }

    public class makereceipt_list : result
    {
        public string customer_address { get; set; }
        public string invoice_gid { get; set; }

        public string customer_contactnumber { get; set; }
        public string customer_email { get; set; }
        public string payment_date { get; set; }
        public string branch_name { get; set; }

        public string customer_name { get; set; }
        public string invoice_id { get; set; }
        public string currency_code { get; set; }
        public string invoice_amount { get; set; }
        public string advance_amount { get; set; }
        public string os_amount { get; set; }
        public string received_amount { get; set; }
        public string tds_receivable { get; set; }
        public string adjust_amount { get; set; }
        public string payment_amount { get; set; }
        public string total_amount { get; set; }
    }

    public class updatereceipt_list : result
    {
        public string receipt_payment_amount { get; set; }
        public string invoice_gid { get; set; }
        public string total_amount { get; set; }
        public string tds_receivable { get; set; }
        public string adjust_amount { get; set; }
        public string bank_name { get; set; }
        public string directorder_gid { get; set; }
        public string cheque_number { get; set; }
        public string receipt_branch_name { get; set; }
        public DateTime cheque_date { get; set; }
        public DateTime neft_date { get; set; }
        public DateTime cash_date { get; set; }
        public string currency_code { get; set; }
        public string payment_remarks { get; set; }
        public string invoice_id { get; set; }
        public string exchange_rate { get; set; }
        public string invoice_amount { get; set; }
        public string basecurrencyinvoiceamt { get; set; }
        public string advance_amount { get; set; }
        public string modepayemnt { get; set; }
        public string os_amount { get; set; }
        public string tdsreceivale_amount { get; set; }
        public string receive_amount { get; set; }
        public string exchange_loss { get; set; }
        public string exchange_gain { get; set; }
        public string payment_date { get; set; }
        public string creditcard_number { get; set; }
        public string bank_charges { get; set; }
        public string received_in_bank { get; set; }

    }

    public class invoice_list : result
    {
        public string invoice_refno { get; set; }
        public string invoice_amount { get; set; }
        public string invoice_date { get; set; }
        public string payment_amount { get; set; }
        public string total_amount { get; set; }
        public string tds { get; set; }
        public string payment_mode { get; set; }



    }
    public class invoicereceipt_list : result
    {
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_contactnumber { get; set; }
        public string customer_email { get; set; }
        public string customer_gid { get; set; }
        public string branch_name { get; set; }
        public string salesorder_gid { get; set; }


    }
    public class invoicereceiptsummary_list : result
    {
        public string invoice_id { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string invoice_amount { get; set; }
        public string advance_amount { get; set; }
        public string basecurrencyinvoiceamt { get; set; }
        public string payment_remarks { get; set; }
        public string os_amount { get; set; }
        public string tdsreceivale_amount { get; set; }
        public string received_in_bank { get; set; }
        public string payment_amount { get; set; }
        public string adjust_amount { get; set; }
        public string receive_amount { get; set; }
        public string bank_charges { get; set; }
        public string exchange_loss { get; set; }
        public string exchange_gain { get; set; }
    }
    public class GetBankdtldropdown : result
    {
        public string bank_name { get; set; }
    }

    public class GetCarddtldropdown : result
    {
        public string bank_gid { get; set; }
        public string bank_name { get; set; }
    }




}