using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrRptInvoiceReport : result
    {
        /// ///////////////////////
        public List<salesinvoicesummary_list> salesinvoicesummary_list { get; set; }
        public List<Getproductnamedropdown> Getproductnamedropdown { get; set; }
        public List<Getcurrencycodedropdown> Getcurrencycodedropdown { get; set; }
        public List<GetCustomernamedropdown> GetCustomernamedropdown { get; set; }
        public List<GetBranchnamedropdown> GetBranchnamedropdown { get; set; }
        public List<Gettomailidlist> Gettomailidlist { get; set; }
        public List<CancelinvoiceList> CancelinvoiceList {  get; set; }
        public List<Gettaxnamedropdown> Gettaxnamedropdown { get; set; }
        public List<GetTermsandconditionsDropdown> terms_lists { get; set; }
        public List<invoiceproductsummary_list> invoiceproductsummarylist { get; set; }
        public List<Getcustomeronchangedetails> Getcustomeronchangedetails { get; set; }
        public List<Getproductonchangedetails> Getproductonchangedetails { get; set; }
        public List<editinvoiceproductsummary_list> editinvoiceproductsummarylist { get; set; }
        public List<GetProductsearchlist> GetProductsearchlist { get; set; }
        public List<GetTaxSegmentListDetails> GetTaxSegmentListDetails { get; set; }
        public List<directinvoiceproductsummary_list> directinvoiceproductsummary_list { get; set; }
        public List<InvoiceEdit_list> InvoiceEdit_list { get; set; }
        public List<GetInvoiceForLastSixMonths_List> GetInvoiceForLastSixMonths_List { get; set; }
        public List<GetReceiptForLastSixMonths_List> GetReceiptForLastSixMonths_List { get; set; }

        public List<GetInvoiceDetailSummary> GetInvoiceDetailSummary { get; set; }
        public List<GetReceiptDetailSummary> GetReceiptDetailSummary { get; set; }

        public string frommailid { get; set; }
        public string subcompanyname { get; set; }
        public string subsymbol { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
        public double product_gid { get; set; }




        //////////////


        //public List<invoicesummary_list> invoicesummary_list { get; set; }

        public List<InvoiceData> invoiceData { get; set; }
        public List<EinvoiceAddField> EinvoiceAddField { get; set; }
        public List<GetOnChangeCurrency> GetOnChangeCurrency { get; set; }
        public List<GetOnChangeBranch> GetOnChangeBranch { get; set; }
        //public List<SalesInvoicelist> SalesInvoicelist { get; set; }
        public List<salesinvoiceproduct_list> salesinvoiceproduct_list { get; set; }
        public List<salesinvoice_list> salesinvoice_list { get; set; }
        public List<viewinvoice_list> viewinvoice_list { get; set; }
        public List<salesproduct_list> salesproduct_list { get; set; }
        public List<GetSendMail_MailId> GetSendMail_MailId { get; set; }
        public List<Salesviewinvoice_list> Salesviewinvoice_list { get; set; }
        public List<Getsalestype> Getsalestype { get; set; }
    }


    public class Getsalestype : result
    {
        public string salestype_gid { get; set; }
        public string salestype_code { get; set; }
        public string salestype_name { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }

    }

    public class GetSendMail_MailId : result
    {
        public string cc_contact_emails { get; set; }
        public string to_customer_email { get; set; }
        public string customer_name { get; set; }
        public string invoice_gid { get; set; }
        public string invoice_reference { get; set; }
        public string due_date { get; set; }
        public string total_amount { get; set; }


    }
    public class Gettomailidlist : result
    {
        public List<string> emails { get; set; }
       
       
        public string customer_name { get; set; }
       


    }
    public class graphtoken
    {

        public string message { get; set; }
        public string access_token { get; set; }
        public bool status { get; set; }
    }
    public class graphLoginSuccessResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
    public class mdlgraph_list
    {
        public string clientID { get; set; }
        public string client_secret { get; set; }
        public string tenantID { get; set; }

    }
    public class MdlGraphMailContent
    {
        public Message1 message { get; set; }
        public bool saveToSentItems { get; set; }
    }
    public class Message1
    {
        public string subject { get; set; }
        public Body2 body { get; set; }
        public Torecipient[] toRecipients { get; set; } = new Torecipient[0];
        public Torecipient[] ccRecipients { get; set; } = new Torecipient[0];
        public Torecipient[] bccRecipients { get; set; } = new Torecipient[0];
        public attachments[] attachments { get; set; } = new attachments[0];
    }
    public class Body2
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class Torecipient
    {
        public Emailaddress emailAddress { get; set; }
    }

    public class Emailaddress
    {
        public string address { get; set; }
    }

    public class attachments
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string name { get; set; }
        public string contentBytes { get; set; }
    }

    public class sendmail_list
    {
        public Results results { get; set; }
        public string AutoID_Key { get; set; }
    }
    public class Results
    {
        public int total_rejected_recipients { get; set; }
        public int total_accepted_recipients { get; set; }
        public string id { get; set; }
    }

    public class MailAttachmentbase64
    {
        public string name { get; set; }
        public string type { get; set; }
        public string data { get; set; }
    }
    public class DbAttachmentPath
    {
        public string path { get; set; }
    }
    public class responselist : result
    {
        public string id { get; set; }
        public string threadId { get; set; }
        public string[] labelIds { get; set; }
        public string gmail_mail_from { get; set; }
        public string gmail_to_mail { get; set; }
        public string base64EncodedText { get; set; }
        public string gmail_body { get; set; }
        public string gmail_sub { get; set; }
        public string leadbank_gid { get; set; }
        public string potential_value { get; set; }
        public string appointment_gid { get; set; }

    }
    public class GetReceiptDetailSummary : result
    {
        public string payment_date { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string approval_status { get; set; }
        public string total_amount { get; set; }
        public string contact { get; set; }
        public string invoice_gid { get; set; }
    }
    public class GetInvoiceDetailSummary : result
    {
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string salesinvoice_status { get; set; }
        public string invoice_refno { get; set; }
        public string invoiceamount { get; set; }
        public string branch_name { get; set; }
        public string invoice_gid { get; set; }
        public string created_by { get; set; }

    }
    public class refreshtokenlist
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }
    public class gmailconfiguration
    {
        public string s_no { get; set; }
        public string access_token { get; set; }
        public string gmail_address { get; set; }
        public string refresh_token { get; set; }
        public string client_secret { get; set; }
        public string client_id { get; set; }
        public string default_template { get; set; }


    }

    public class SalesOrderDetails
    {
        public List<shopify_id> shopify_id { get; set; }
        public string salesorder_gid { get; set; }
        public string shopify_ids { get; set; }
        public string branch_gid { get; set; }
        public string account_gid { get; set; }
        public string tax_name { get; set; }
        public DateTime salesorder_date { get; set; }
        public string customer_gid { get; set; }
        public string customer_contact_person { get; set; }
        public string customer_address { get; set; }
        public string so_referencenumber { get; set; }
        public string so_remarks { get; set; }
        public string total_amount { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string salesorder_status { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string termsandconditions { get; set; }
        public string so_referenceno1 { get; set; }
        public double Grandtotal { get; set; }
        public string customer_name { get; set; }
        public string salesorder_advance { get; set; }
        public string so_type { get; set; }
        public string executeorder_flag { get; set; }
        public string invoice_flag { get; set; }
        public string salesorder_remarks { get; set; }
        public string salesorder_flag { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string despatch_status { get; set; }
        public string updated_advance { get; set; }
        public string received_amount { get; set; }
        public string invoice_amount { get; set; }
        public string updated_addon_charge { get; set; }
        public string updated_additional_discount { get; set; }
        public string cst_number { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string currency_gid { get; set; }
        public string approved_by { get; set; }
        public string approved_date { get; set; }
        public string renewal_date { get; set; }
        public string renewal_description { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string renewal_gid { get; set; }
        public string reference_gid { get; set; }
        public string customer_contact_gid { get; set; }
        public string renewal_status { get; set; }
        public string pinvoice_amount { get; set; }
        public string approval_remarks { get; set; }
        public string shipping_to { get; set; }
        public string billing_to { get; set; }
        public string fulfillment_status { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string customerbranch_gid { get; set; }
        public string commit_advance { get; set; }
        public string otp_password { get; set; }
        public string advance_wht { get; set; }
        public string updated_advancewht { get; set; }
        public string advanceapproval_flag { get; set; }
        public string campaign_gid { get; set; }
        public string agreement_flag { get; set; }
        public string marketingexecutive_gid { get; set; }
        public string marketingexecutiveteam_gid { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string order_note { get; set; }
        public string gst_amount { get; set; }
        public string tax_gid { get; set; }
        public string total_price { get; set; }
        public string vessel_gid { get; set; }
        public string vessel_name { get; set; }
        public string salesperson_gid { get; set; }
        public string costcenter_gid { get; set; }
        public string quotation_gid { get; set; }
        public string roundoff { get; set; }
        public string progressive_flag { get; set; }
        public string advanceamount_bank { get; set; }
        public string advancewhtbase_currency { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string mawb_no { get; set; }
        public string hawb_no { get; set; }
        public string invoice_no { get; set; }
        public string sb_no { get; set; }
        public string flight_no { get; set; }
        public string pkgs { get; set; }
        public string gr_wt { get; set; }
        public string ch_wt { get; set; }
        public string igm_no { get; set; }
        public string igm_date { get; set; }
        public string assesable_amount { get; set; }
        public string cbm { get; set; }
        public string lfrom { get; set; }
        public string lto { get; set; }
        public string be_date { get; set; }
        public string dutyamount { get; set; }
        public string mawb_date { get; set; }
        public string hawb_date { get; set; }
        public string buyer_name { get; set; }
        public string buyer_address { get; set; }
        public string buy_email { get; set; }
        public string buy_mobile { get; set; }
        public string airline_charges { get; set; }
        public string direct_shipment { get; set; }
        public string branch_name { get; set; }
        public string businessunit_gid { get; set; }
        public string description { get; set; }
        public string others { get; set; }
        public string flight_date { get; set; }
        public string businessunit_name { get; set; }
        public string consignee_to { get; set; }
        public string customerref_no { get; set; }
        public string supplier_name { get; set; }
        public string supplier_address { get; set; }
        public string buyer_gid { get; set; }
        public string supplier_gid { get; set; }
        public string agent_gid { get; set; }
        public string shipper_address { get; set; }
        public string consignee_address { get; set; }
        public string shopify_orderid { get; set; }
        public string tax_name4 { get; set; }
        public string shopifyorder_number { get; set; }
        public string shopifycustomer_id { get; set; }
        public string customer_contactperson { get; set; }
        public string mobile { get; set; }
        public string tax_amount { get; set; }
        public string customer_instruction { get; set; }
        public string billing_email { get; set; }
        public string mintsoftid { get; set; }
        public string order_instruction { get; set; }
        public string message_id { get; set; }
        public string source_flag { get; set; }
    }
    public class SalesOrderDetailsdtl
    {
        public string salesorderdtl_gid { get; set; }
        public string salesorder_gid { get; set; }
        public string product_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string qty_quoted { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string taxable { get; set; }
        public string tax_percentage { get; set; }
        public string tax_amount { get; set; }
        public string product_remarks { get; set; }
        public string uom_gid { get; set; }
        public string uom_name { get; set; }
        public string payment_days { get; set; }
        public string delivery_period { get; set; }
        public string price { get; set; }
        public string display_field { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string salesorder_refno { get; set; }
        public string product_delivered { get; set; }
        public string qty_executed { get; set; }
        public string tax_amount_l { get; set; }
        public string tax_amount2_l { get; set; }
        public string tax_amount3_l { get; set; }
        public string discount_amount_l { get; set; }
        public string product_price_l { get; set; }
        public string price_l { get; set; }
        public string tax1_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string margin_percentage { get; set; }
        public string margin_price { get; set; }
        public string margin_amount { get; set; }
        public string selling_price { get; set; }
        public string vendor_gid { get; set; }
        public string slno { get; set; }
        public string againstpo_flag { get; set; }
        public string vendor_price { get; set; }
        public string product_requireddate { get; set; }
        public string type { get; set; }
        public string delivery_quantity { get; set; }
        public string customerproduct_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string product_requireddateremarks { get; set; }
        public string mrp { get; set; }
        public string foreign_currency { get; set; }
        public string foreign_amount { get; set; }
        public string exchange_rate { get; set; }
        public string product_code { get; set; }
        public string currency_gid { get; set; }
        public string shopify_lineitemid { get; set; }
        public string shopify_orderid { get; set; }
        public string shopify_productid { get; set; }
        public string order_type { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
    }

    public class shopify_id : result
    {
        public string shopify_ids { get; set; }
    }
    public class CancelinvoiceList : result
    {
        public string invoice_gid { get; set; }
        public string salesorder_gid { get; set; }
        public string invoice_amount { get; set; }
    }
    public class salesinvoicesummary_list : result
    {
        public DateTime directorder_date { get; set; }
        public string directorder_gid { get; set; }
        public string so_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string mobile { get; set; }
        public string company_code {  get; set; }
        public string order_type { get; set; }
        public string currency_code { get; set; }
        public string grandtotal { get; set; }
        public string invoice_amout { get; set; }
        public string outstanding_amount { get; set; }
        public string status { get; set; }
        public string customer_code { get; set; }
        public string leadbank_gid { get; set; }
        public string lead2campaign_gid { get; set; }
        public string customer_gid { get; set; }
        public string progressive_invoice { get; set; }
        public string cancelirn_limit { get; set; }        
        public string so_referencenumber { get; set; }
        public string irn { get; set; }
        public string einvoice_flag { get; set; }
        public string invoice_gid { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_reference { get; set; }
        public string invoice_from { get; set; }
        public string invoice_status { get; set; }
        public string mail_status { get; set; }
        public string invoice_amount { get; set; }
        public string customer_contactperson { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string salesorder_status { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string payment_flag { get; set; }
        public string initialinvoice_amount { get; set; }
        public string customer_contactnumber { get; set; }        
        public string salesorder_gid { get; set; }

    }

    public class Getproductnamedropdown : result
    {
        public string productname { get; set; }
        public string productcode { get; set; }
        public string productgid { get; set; }
    }

    public class Getcurrencycodedropdown : result
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }
    }

    public class GetCustomernamedropdown : result
    {
        public string customergid { get; set; }
        public string customername { get; set; }
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
    }

    public class GetBranchnamedropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }

    public class Gettaxnamedropdown : result
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
    }

    public class GetTermsandconditionsDropdown 
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }

    }

    public class invoiceproductsummary_list : result
    {
        public string product_name { get; set; }
        public string invoicedtl_gid { get; set; }
        public string discount { get; set; }
        public string tax_amount { get; set; }
        public string hsn { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string uom_name { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }
        public string selling_price { get; set; }
        public string product_total { get; set; }
        public string tax_name { get; set; }
        public string productgroup_gid { get; set; }
        public string uom_gid { get; set; }
        public string discount_percentage { get; set; }
        public string product_gid { get; set; }
        public string tax1_gid { get; set; }
    }    
   
    public class salesinvoicelist : result
    {     
        public string customer_gid { get; set; }
        public string invoice_gid { get; set; }
        public string grandtotal { get; set; }
        public string currencygid { get; set; }
        public string invoiceref_no { get; set; }
        public string invoice_date { get; set; }
        public string payment_days { get; set; }
        public string due_date { get; set; }
        public string order_refno { get; set; }
        public string sales_type { get; set; }
        public string customercontactperson { get; set; }
        public string customercontactnumber { get; set; }
        public string customeraddress { get; set; }
        public string customeremailaddress { get; set; }
        public string invoicetotalamount { get; set; }
        public string GrandTotalAmount { get; set; }
        public string invoicediscountamount { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string remarks { get; set; }
        public string termsandconditions { get; set; }
        public string exchange_rate { get; set; }
        public string branch_name { get; set; }
        public string roundoff { get; set; }
        public string tax_gid { get; set; }
        public string tax_name4 { get; set; }
        public string tax_amount4 { get; set; }
        public string taxsegment_gid { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string buybackcharges { get; set; }
        public string forwardingCharges { get; set; }
        public string insurance_charges { get; set; }
        public string producttotalamount { get; set; }
        public string salestype { get; set; }
        public string dispatch_mode { get; set; }
        public string totalamount { get; set; }
        public string delivery_days { get; set; }
        public string payment { get; set; }
        public string deliveryperiod { get; set; }
        public string currency_code { get; set; }
        public string shipping_to { get; set; }
        public string bill_email { get; set; }
        
    }
    public class GetReceiptForLastSixMonths_List : result
    {
        public string month_year { get; set; }
        public string paymentamount { get; set; }
        public string invoicecount { get; set; }
        public string month { get; set; }
        public string year { get; set; }

    }
    public class GetInvoiceForLastSixMonths_List : result
    {
        public string months { get; set; }
        public string invoiceamount { get; set; }
        public string invoiceamount1 { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public string Tax_amount { get; set; }
        public string net_amount { get; set; }
        public string invoicecount { get; set; }
        public string invoice_date { get; set; }
        public string invoice_gid { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string grandtotal_l { get; set; }
        public string customer_name { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }

    }
    public class Getcustomeronchangedetails : result
    {
        public string customerbranchname { get; set; }
        public string customercontactname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string taxsegment_gid { get; set; }
    }

    public class Getproductonchangedetails : result
    {
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string productuom_name { get; set; }
        public string product_price { get; set; }
        public string unitprice { get; set; }
    }

    public class invoiceproductlist : result
    {
        public double quantity { get; set; }
        public string mrp { get; set; }
        public string productgid { get; set; }
        public string hsncode { get; set; }
        public string totalamount { get; set; }
        public string hsndescription { get; set; }
        public double unitprice { get; set; }
        public double discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount1 { get; set; }
        public string totalamount1 { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string taxamount2 { get; set; }
        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string productdescription { get; set; }
        public string tax_gid1 { get; set; }
        public string tax_gid2 { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string display_field { get; set; }
        public string productgroup_name { get; set; }
        public string costprice { get; set; }
        public string productcode { get; set; }
        public string customerproductcode { get; set; }
        public string taxpercentage1 { get; set; }
        public string taxpercentage2 { get; set; }
        public string vendorprice { get; set; }
        public string invoicedtl_gid { get; set; }
        public double exchangerate { get; set; }
    }

    public class editinvoiceproductsummary_list : result
    {
        public string product_name { get; set; }
        public string invoicedtl_gid { get; set; }
        public string discount { get; set; }
        public string tax { get; set; }
        public string taxamount2 { get; set; }
        public string taxname2 { get; set; }
        public string hsn { get; set; }
        public string tax_amount { get; set; }
        public string product_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string vendor_price { get; set; }
        public string uom_name { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }
        public string selling_price { get; set; }
        public string product_total { get; set; }
        public string tax_name { get; set; }
    }

    public class invoicelist : result
    {
        public string customer_gid { get; set; }
        public string invoice_gid { get; set; }
        public string branch_name { get; set; }
        public string sales_type { get; set; }
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customercontactnumber { get; set; }
        public string customeremailaddress { get; set; }
        public string customer_details { get; set; }
        public string shipping_to { get; set; }
        public string dispatch_mode { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string template_name { get; set; }
        public string template_gid { get; set; }
        public string due_date { get; set; }
        public string bill_email { get; set; }
        public string grandtotal { get; set; }
        public string roundoff { get; set; }
        public string insurance_charges { get; set; }
        public string freight_charges { get; set; }
        public string additional_discount { get; set; }
        public string addon_charge { get; set; }
        public string tax_amount4 { get; set; }
        public string tax_name4 { get; set; }
        public string totalamount { get; set; }
        public string delivery_days { get; set; }
        public string insurancecharges { get; set; }
        public string invoice_type { get; set; }
        public string mode_of_despatch { get; set; }
        public string deliverydate { get; set; }
        public string payment { get; set; }
        public string deliveryperiod { get; set; }


    }

    public class InvoiceEdit_list : result
    {
        public string invoice_gid { get; set; }
        public string customer_name { get; set; }
        public string customercontact_name { get; set; }
        public string customer_contactnumber { get; set; }
        public string customer_email { get; set; }
        public string customerbranch_name { get; set; }
        public string customer_address { get; set; }
        public string customer_gid { get; set; }
        public string mode_of_despatch { get; set; }
        public string invoice_type { get; set; }
        public string branch_gid { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string delivery_date { get; set; }
        public string delivery_days { get; set; }
        public string payment_days { get; set; }
        public string payment_term { get; set; }
        public string invoice_amount { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string total_amount { get; set; }
        public string termsandconditions { get; set; }
        public string shipping_to { get; set; }
        public string currencyexchange_gid { get; set; }
        public string bill_email { get; set; }
        public string payment_date { get; set; }
        public string tax_gid { get; set; }
        public string tax_amount { get; set; }
        public string sales_type { get; set; }
        public string salestype_name { get; set; }

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

    public class directinvoiceproductsummary_list : result
    {
        public string productuom_name { get; set; }
        public string taxamount1 { get; set; }
        public string unitprice { get; set; }
        public double price { get; set; }
        public double vendor_price { get; set; }
        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string taxname3 { get; set; }
        public string tax_prefix1 { get; set; }
        public string tax_prefix2 { get; set; }
        public string product_name { get; set; }
        public string tmppurchaseorderdtl_gid { get; set; }
        public string discount_percentage { get; set; }
        public string tax { get; set; }
        public string tax2 { get; set; }
        public string tax3 { get; set; }
        public string totalamount { get; set; }
        public string hsn { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string uom_name { get; set; }
        public string qty { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }
        public string product_total { get; set; }
        public string taxsegment_gid { get; set; }
        public string display_field { get; set; }
        public string invoicedtl_gid { get; set; }
        public string invoice_gid { get; set; }
        public string tax_percentage3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_gid { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string productuom_gid { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount3 { get; set; }
        public string taxamount { get; set; }
        public string product_remarks { get; set; }
    }

    public class directinvoiceproductsubmit_list
    {        
        public string taxsegment_gid { get; set; }
        public string taxseg_taxtotal { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string qty_requested { get; set; }
        public double productquantity { get; set; }
        public string discountprecentage { get; set; }
        public double discount_amount { get; set; }
        public string tax1 { get; set; }
        public string tax2 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount1 { get; set; }
        public string producttotal_amount { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public double unitprice { get; set; }
        public string display_field { get; set; }
        public string tax3 { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string cost_price { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount3 { get; set; }
        public string taxgid1 { get; set; }
        public string taxgid2 { get; set; }
        public string taxgid3 { get; set; }
        public string taxprecentage1 { get; set; }
        public string taxprecentage2 { get; set; }
        public string taxprecentage3 { get; set; }
        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string taxname3 { get; set; }
        public string product_desc { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string tax_prefix3 { get; set; }
    }
    public class invoicerefno_list : result
    {
        public string invoice_gid { get; set; }
        public string Old_invoicerefno {  get; set; }
        public string new_invoicerefno { get; set; }
    }
   
    public class viewinvoice_list : result
    {
        public string invoice_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_branch { get; set; }
        public string customer_contactperson { get; set; }
        public string customer_contactnumber { get; set; }
        public string customer_email { get; set; }
        public string customer_address { get; set; }
        public string branch_name { get; set; }
        public string gst_no { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string hsn_details { get; set; }
        public string total_amount { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string invoice_amount { get; set; }
        public string termsandconditions { get; set; }
        public string product_code { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string productuom_name { get; set; }
        public string vendor_price { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string discount_percentage { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string price { get; set; }
        public string price_total { get; set; }

    }
   public class GetOnChangeCurrency : result
    {
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
    }
    public class GetOnChangeBranch : result
    {
        public string GST { get; set; }
    }
    public class EinvoiceAddField : result
    {
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string invoicerefno { get; set; }
        public string paymentterms { get; set; }
        public string customeremail { get; set; }
        public string paymentduedate { get; set; }
        public string invoice_gid { get; set; }
        public string documentDetails_payment_term { get; set; }
        public string documentDetails_due_date { get; set; }
        public string customer_contactperson { get; set; }
        public string sellercontact_no { get; set; }
        public string selleremail_address { get; set; }
        public string sellerDetails_Gstin { get; set; }
        public string sellerDetails_LglNm { get; set; }
        public string sellerDetails_TrdNm { get; set; }
        public string sellerDetails_Address { get; set; }
        public string sellerDetails_Loc { get; set; }
        public int sellerDetails_Pin { get; set; }
        public string sellerDetails_cont_num { get; set; }
        public string roundoff { get; set; }
        public string frieghtcharges { get; set; }
        public string buybackcharges { get; set; }
        public string forwardingCharges { get; set; }
        public string insurancecharges { get; set; }
        public string sellerDetails_Stcd { get; set; }
        public string sellerDetails_email { get; set; }
        public string transactionDetails_TaxSch { get; set; }
        public string transactionDetails_ecomm_gstin { get; set; }
        public string transactionDetails_SupTyp { get; set; }
        public string transactionDetails_RegRev { get; set; }
        public string transactionDetails_IgstOnIntra { get; set; }
        public string transactionDetails_trans_category { get; set; }
        public string transactionDetails_reverse_charge { get; set; }
        public string transactionDetails_tranaction_type { get; set; }
        public string documentDetails_Typ { get; set; }
        public string documentDetails_No { get; set; }
        public string documentDetails_Dt { get; set; }
        public string buyerDetails_Gstin { get; set; }
        public string buyerDetails_comp_name { get; set; }
        public string buyerDetails_cont_num { get; set; }
        public string buyerDetails_LglNm { get; set; }
        public string buyerDetails_TrdNm { get; set; }
        public string buyerDetails_Pos { get; set; }
        public string buyerDetails_cont_person { get; set; }
        public string buyerDetails_email { get; set; }
        public string buyerDetails_Address { get; set; }
        public string buyerDetails_Loc { get; set; }
        public int buyerDetails_Pin { get; set; }
        public int buyerDetails_remarks { get; set; }
        public string buyerDetails_Stcd { get; set; }
        public string dispatchDetails_Nm { get; set; }
        public string dispatchDetails_Address { get; set; }
        public string dispatchDetails_Loc { get; set; }
        public int dispatchDetails_Pin { get; set; }
        public string dispatchDetails_Stcd { get; set; }
        public string shipDetails_Gstin { get; set; }
        public string shipDetails_LglNm { get; set; }
        public string shipDetails_TrdNm { get; set; }
        public string shipDetails_Address { get; set; }
        public string shipDetails_Loc { get; set; }
        public int shipDetails_Pin { get; set; }
        public string shipDetails_Stcd { get; set; }
        public string item_SlNo { get; set; }
        public string item_PrdDesc { get; set; }
        public string item_IsServc { get; set; }
        public string item_HsnCd { get; set; }
        public double item_Qty { get; set; }
        public string item_Unit { get; set; }
        public double item_UnitPrice { get; set; }
        public double item_TotAmt { get; set; }
        public double item_Discount { get; set; }
        public double item_AssAmt { get; set; }
        public double item_GstRt { get; set; }
        public double item_IgstAmt { get; set; }
        public double item_SgstAmt { get; set; }
        public double item_CgstAmt { get; set; }
        public double item_TotItemVal { get; set; }
        public string lblroundoff { get; set; }
        public string remarks { get; set; }
        public string vendorEmail { get; set; }
        public string vendorPhoneNo { get; set; }
        public string addoncharges { get; set; }
        public string additionaldiscount { get; set; }
        public string discount_amount { get; set; }
        public string extracharges { get; set; }
        public string extradiscount { get; set; }
        public double grandtotal_amountvalue { get; set; }
        public string currencycode_additionaldiscount { get; set; }
        public string currencycode_addon { get; set; }
        public string currencycode_total { get; set; }
        public string currencycode_grandtotal { get; set; }
        public string tax_code { get; set; }
        public string finaltotal_code { get; set; }
        public string extraadditional { get; set; }
        public string cbodiscountamount { get; set; }
        public string additionalamountvalue { get; set; }
        public string extradiscountamountvalue { get; set; }
        public string overalltaxt { get; set; }
        public string finaltotal { get; set; }
        public string cbo_overallTax { get; set; }
        public string producttotalprice { get; set; }
        public string hdnexchange_rate { get; set; }
        public string freightcharges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string freight_currency_code { get; set; }
        public string buyback_currency_code { get; set; }
        public string packing_currency_code { get; set; }
        public string insurance_currency_code { get; set; }
        public string extracharges_currencycode { get; set; }
        public string extradiscount_currencycode { get; set; }
    }
    public class InvoiceData
    {
        public string Version { get; set; }
        public TransactionDetails TranDtls { get; set; }
        public DocumentDetails DocDtls { get; set; }
        public SellerDetails SellerDtls { get; set; }
        public BuyerDetails BuyerDtls { get; set; }
        public DispatchDetails DispDtls { get; set; }
        public ShipDetails ShipDtls { get; set; }
        public List<ItemDetails> ItemList { get; set; }
        public ValueDetails ValDtls { get; set; }
    }
    public class TransactionDetails
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string RegRev { get; set; }
        public string IgstOnIntra { get; set; }
    }
    public class ValueDetails
    {
        public double AssVal { get; set; }
        public double IgstVal { get; set; }
        public double SgstVal { get; set; }
        public double CgstVal { get; set; }
        public double Discount { get; set; }
        public double OthChrg { get; set; }
        public double RndOffAmt { get; set; }
        public double TotInvVal { get; set; }
    }
    public class DocumentDetails
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }
    }
    public class SellerDetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }
    }
    public class BuyerDetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Pos { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }
    }
    public class DispatchDetails
    {
        public string Nm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }
    public class ShipDetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }
    public class ItemDetails
    {
        public string SlNo { get; set; }
        public string PrdDesc { get; set; }
        public string IsServc { get; set; }
        public string HsnCd { get; set; }
        public double Qty { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double TotAmt { get; set; }
        public double Discount { get; set; }
        public double AssAmt { get; set; }
        public double GstRt { get; set; }
        public double IgstAmt { get; set; }
        public double SgstAmt { get; set; }
        public double CgstAmt { get; set; }
        public double TotItemVal { get; set; }
    }
    public class ApiResponse
    {
        public bool success { get; set; }
        public string qr_code { get; set; }
        public string request_id { get; set; }
        public string errorMessage { get; set; }
    }
    public class ResponseData
    {
        public Boolean success { get; set; }
        public string message { get; set; }
        public ResultItem result { get; set; }
    }
    public class qrDownloadResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string jti { get; set; }
    }
    public class ResultItem
    {
        public string AckNo { get; set; }
        public DateTime AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string Status { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string Remarks { get; set; }
    }
    public class qr_request
    {
        public string data { get; set; }
    }
   //public class invoicesummary_list : result
    //{
    //    public string so_referencenumber { get; set; }
    //    public string irn { get; set; }
    //    public string invoice_gid { get; set; }
    //    public DateTime invoice_date { get; set; }
    //    public string invoice_refno { get; set; }
    //    public string customer_name { get; set; }
    //    public string invoice_reference { get; set; }
    //    public string invoice_from { get; set; }
    //    public string invoice_status { get; set; }
    //    public string mail_status { get; set; }
    //    public string invoice_amount { get; set; }
    //    public string customer_contactperson { get; set; }
    //    public string created_date { get; set; }
    //    public string created_by { get; set; }
    //}

    public class einvoicelist : result
    {
        public string invoice_gid { get; set; }
        public string customer_email { get; set; }
        public string company_name { get; set; }
        public string postalcode { get; set; }
        public string gst_no { get; set; }
        public string buyerDetails_Pos { get; set; }
        public string sellercontact_number { get; set; }
        public string dispatchDetails_Nm { get; set; }
        public string dispatchDetails_Address { get; set; }
        public string dispatchDetails_Loc { get; set; }
        public string dispatchDetails_Pin { get; set; }
        public string dispatchDetails_Stcd { get; set; }
        public string shipDetails_Gstin { get; set; }
        public string shipDetails_LglNm { get; set; }
        public string shipDetails_TrdNm { get; set; }
        public string shipDetails_Address { get; set; }
        public string shipDetails_Loc { get; set; }
        public string shipDetails_Pin { get; set; }
        public string shipDetails_Stcd { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string customer_address { get; set; }
        public string customer_name { get; set; }
        public string mobile { get; set; }
        public string gst_number { get; set; }
        public string customer_contactperson { get; set; }
        public string customer_pin { get; set; }
        public string customer_state { get; set; }
        public string customer_city { get; set; }
        public string company_address { get; set; }
        public string state { get; set; }
        public string company_mail { get; set; }
        public string city { get; set; }
        public string remarks { get; set; }
        public string termsandconditions { get; set; }
        public string exchangerate { get; set; }
        public string branch { get; set; }
        public double netamount { get; set; }
        public string addoncharges { get; set; }
        public string invoicediscountamount { get; set; }
        public string frieghtcharges { get; set; }
        public string buybackcharges { get; set; }
        public double packing_charges { get; set; }
        public string forwardingCharges { get; set; }
        public string insurancecharges { get; set; }
        public string roundoff { get; set; }
        public string invoice_amount { get; set; }
    }
   
    public class GetIRNDetails : result
    {
        public string invoice_refno { get; set; }
        public string customer_name { get; set; }
        public string invoice_amount { get; set; }
        public string invoice_date { get; set; }
        public string irn { get; set; }
    }
    public class creditnote_list : result
    {
        public string invoice_refno_creditnote { get; set; }
        public string invoice_date_creditnote { get; set; }
        public string invoice_custname_creditnote { get; set; }
        public string invoice_amount_creditnote { get; set; }
        public string irn_no_creditnote { get; set; }
        public string invoice_gid { get; set; }
    }
    public class cancelIrn_list : result
    {
        public string invoice_refno_cancel { get; set; }
        public string invoice_date_cancel { get; set; }
        public string invoice_custname_cancel { get; set; }
        public string invoice_amount_cancel { get; set; }
        public string irn_no_cancel { get; set; }
        public string invoice_gid { get; set; }
    }
    public class Cancelirn_ResultItem
    {
        public string Irn { get; set; }
        public DateTime CancelDate { get; set; }
    }
    public class ResponseDatacredit
    {
        public bool success { get; set; }
        public string message { get; set; }
        public ResultItem result { get; set; }
    }
    public class CancelIRN_ResponseData
    {
        public Boolean success { get; set; }
        public string message { get; set; }
        public Cancelirn_ResultItem result { get; set; }
    }
    public class CancelIRN
    {
        public string irn { get; set; }
        public string cnlrsn { get; set; }
        public string cnlrem { get; set; }
    }


    public class SalesInvoicelist : result
    {
        public string serviceorder_gid { get; set; }
        public string total_amount1 { get; set; }
        public string product_name { get; set; }
        public string discount_percentage { get; set; }
        public string discountamount { get; set; }
        public string unit { get; set; }
        public string qty_quoted { get; set; }
        public string tax { get; set; }
        public string tax_name { get; set; }
        public string tax1_gid { get; set; }
        public string tax_amount1 { get; set; }
        public string product_price { get; set; }
        public string product_code { get; set; }
        public string total_price { get; set; }
        public string tax_amount { get; set; }
        public string so_reference { get; set; }
        public string serviceorder_date { get; set; }
        public string addon_amount { get; set; }
        public string total_amount { get; set; }
        public string grand_total { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_mobile { get; set; }
        public string branch_name { get; set; }
        public string email { get; set; }
        public string taxamount { get; set; }
        public string roundoff { get; set; }
        public string addoncharge { get; set; }
        public string additionaldiscount { get; set; }
        public string customercontact_name { get; set; }
        public string discount_amount { get; set; }
        public string customer_gid { get; set; }
        public string customer_code { get; set; }
        public string termsandconditions { get; set; }
        public string gst_amount { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string buyback_charges { get; set; }
        public string insurance_charges { get; set; }
        public string invoice_no { get; set; }
        public string description { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string invoice_amount { get; set; }
        public string invoiceaccounting_branch { get; set; }
    }

    public class salesinvoiceproduct_list : result
    {
        public string customerproduct_code { get; set; }
        public string product_name { get; set; }
        public string discount_percentage { get; set; }
        public string total_amount { get; set; }
        public string description { get; set; }
        public string qty_quoted { get; set; }
        public string vendoramount { get; set; }
        public string discount_amount { get; set; }
        public string amount1 { get; set; }
        public string product_price { get; set; }
        public string final_amount { get; set; }
        public string tax { get; set; }
        public string tax_amount1 { get; set; }
        public string product_code { get; set; }
        public string tax1_gid { get; set; }

        public string unit { get; set; }


    }

    public class salesinvoice_list : result
    {
        public string invoice_date { get; set; }
        public string invoiceaccounting_ordertotal_withtax { get; set; }
        public string invoiceaccounting_branch { get; set; }
        public string branch_name { get; set; }
        public string invoiceaccounting_payterm { get; set; }
        public DateTime invoiceaccounting_orderdate { get; set; }
        public string invoiceaccounting_duedate { get; set; }
        public string invoiceaccounting_orderrefno { get; set; }
        public string invoiceaccounting_salestype { get; set; }
        public string invoiceaccounting_contactperson { get; set; }
        public string invoiceaccounting_customername { get; set; }
        public string invoiceaccounting_contactnumber { get; set; }
        public string invoiceaccounting_customeraddress { get; set; }
        public string invoiceaccounting_email { get; set; }
        public string invoiceaccounting_grandtotal { get; set; }
        public string invoice_refno { get; set; }
        public string invoiceaccounting_refno { get; set; }
        public string employee_gid { get; set; }
        public string customer_gid { get; set; }
        public string invoiceaccounting_currency { get; set; }
        public string discount_amount { get; set; }
        public string invoiceaccounting_addonamount { get; set; }
        public string invoiceaccounting_remarks { get; set; }
        public string invoiceaccounting_termsandconditions { get; set; }
        public string invoiceaccounting_exchangerate { get; set; }
        public string invoiceaccounting_roundoff { get; set; }
        public string invoiceaccounting_insurancecharges { get; set; }
        public string invoiceaccounting_freightcharges { get; set; }
        public string invoiceaccounting_packingcharges { get; set; }
        public string currency_gid { get; set; }
        public string invoiceaccounting_salesorder_gid { get; set; }
    }
    public class Salesviewinvoice_list : result
    {
        public string invoice_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_branch { get; set; }
        public string customer_contactperson { get; set; }
        public string customer_contactnumber { get; set; }
        public string customer_email { get; set; }
        public string customer_address { get; set; }
        public string branch_name { get; set; }
        public string gst_no { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string invoice_remarks { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string payment_term { get; set; }
        public string payment_date { get; set; }
        public string bill_email { get; set; }
        public string salestype_name { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string hsn_details { get; set; }
        public string total_amount { get; set; }
        public string additionalcharges_amount { get; set; }
        public string discount_amount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string invoice_amount { get; set; }
        public string termsandconditions { get; set; }
        public string product_code { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string productuom_name { get; set; }
        public string vendor_price { get; set; }
        public string qty_invoice { get; set; }
        public string product_price { get; set; }
        public string discount_percentage { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string price { get; set; }
        public string price_total { get; set; }
        public string gst_number { get; set; }
        public string mode_of_despatch { get; set; }
        public string delivery_days { get; set; }
        public string payment_days { get; set; }
        public string branch_gid { get; set; }
        public string shipping_to { get; set; }
        public string discount_amount_L { get; set; }

    }







}