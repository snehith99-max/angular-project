using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsRptStockreport : result
    {
        public List<stockreport_list> stockreport_list { get; set; }
        public List<branch_list> branch_list { get; set; }
        public List<GetStockState_list> GetStockState_list { get; set; }
        public List<StockcustomerList> StockcustomerList { get; set; }
        public List<StockpurchasecustomerList> StockpurchasecustomerList { get; set; }
        public List<GetStockpurchaseState_list> GetStockpurchaseState_list { get; set; }        
        public List<GetStockvendor_list> GetStockvendor_list { get; set; }        
        public List<Getpurchaseorder_history> Getpurchaseorder_history { get; set; }
        public List<GetStockStatement_list> GetStockStatement_list { get; set; }
        public List<GetStockcustomer_list> GetStockcustomer_list { get; set; }
        public List<Getsaleshistory_list> Getsaleshistory_list { get; set; }
        public List<Getstock_list> Getstock_list { get; set; }
        public List<Getlastsixmonthstock_list> Getlastsixmonthstock_list { get; set; }
        public string branch_name { get; set; }

    }

    public class stockreport_list : result
    {
        public string bin_number { get; set; }
        public string branch_name { get; set; }
        public string location_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string stock_balance { get; set; }
        public string stock_value { get; set; }
        public string product_price { get; set; }
        public string display_field { get; set; }
        public string product_gid { get; set; }
        public string branch_gid { get; set; }
        public string productgroup_name { get; set; }
        public double product_qtydelivered { get; set; }
        public double qty_received { get; set; }
        public string opening_stock { get; set; }
        public string branch_prefix { get; set; }
        public string created_date { get; set; }
        public string reference_gid { get; set; }
        public string remarks { get; set; }
    }
    public class branch_list : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }
    public class GetStockState_list : result
    {
        public string product_name { get; set; }
        public string qty_quoted { get; set; }
        public string price { get; set; }
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string product_code { get; set; }
        public string salesorder_date { get; set; }
    }
    public class StockcustomerList : result
    {
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string total_amount { get; set; }
    }
    public class StockpurchasecustomerList : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string total_amount { get; set; }
    }
    public class GetStockpurchaseState_list: result
    {
        public string qty_ordered { get; set; }
        public string product_price { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string branch_gid { get; set; }
        public string purchaseorder_gid { get; set; }
        public string vendor_code { get; set; }
        public string stock_value { get; set; }

        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string product_gid { get; set; }
    }

    public class GetStockvendor_list : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string email_id { get; set; }
        public string gst_no { get; set; }
        public string address { get; set; }
    }
    public class Getpurchaseorder_history : result
    {

        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string ExpectedPODeliveryDate { get; set; }
        public string porefno { get; set; }
        public string Vendor { get; set; }
        public string poref_no { get; set; }
        public string branch_name { get; set; }
        public string costcenter_name { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string Contact { get; set; }
        public string flag { get; set; }

        public string total_amount { get; set; }
        public string remarks { get; set; }
        public string purchaseorder_status { get; set; }
        public string vendor_status { get; set; }
        public string paymentamount { get; set; }
        public string currency_code { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string grn_gid { get; set; }
        public string grn_flag { get; set; }

    }

    public class GetStockStatement_list : result
    {

        public string qty_quoted { get; set; }
        public string price { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string contact { get; set; }
        public string product_gid { get; set; }
    }
    public class GetStockcustomer_list : result
    {

        public string customer_address { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_gid { get; set; }
        public string contact { get; set; }
    }
    public class Getsaleshistory_list : result
    {
        public DateTime directorder_date { get; set; }
        public string directorder_gid { get; set; }
        public string so_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string mobile { get; set; }
        public string company_code { get; set; }
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
    public class Getstock_list : result
    {
        public string month { get; set; }
        public string grn_count { get; set; }
        public string delivery_count { get; set; }

    }
    public class Getlastsixmonthstock_list : result
    {
        public string month { get; set; }
        public string grn_count { get; set; }
        public string delivery_count { get; set; }
        public string grn_date { get; set; }

    }

}
