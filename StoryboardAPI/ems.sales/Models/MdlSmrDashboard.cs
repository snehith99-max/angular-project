using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoryboardAPI.Models;

namespace ems.sales.Models
{

    public class MdlSmrDashboard : result
    {
        //
        public List<GetTilesDetails_list> GetTilesDetails_list { get; set; }


        //
        public List<GetSalesPerformanceChart_List> GetSalesPerformanceChart_List { get; set; }        
        public List<GetSalesOrderCount_List> GetSalesOrderCount_List { get; set; }
        public List<GetOverallSalesOrderChart_List> GetOverallSalesOrderChart_List { get; set; }
        public List<Mtd_List>Mtd_List { get; set; }
        public List<chartscounts_list2> chartscounts_list2 { get; set; }
        public List<currency_list> currency_list { get; set; }
        public List<GetSalesStatus_list> GetSalesStatus_list { get; set; }
        public List<GetSalesdashboard_list> GetSalesdashboard_list { get; set; }
        public List<GetPaymentandDeliveryChart_list> GetPaymentandDeliveryChart_list { get; set; }
        public List<Getoverallsalesbarchart_list> Getoverallsalesbarchart_list { get; set; }
        public List<Salesordersixmonthchart_list> Salesordersixmonthchart_list { get; set; }
        public string totalinvoice { get; set; }
        public string approval_pending { get; set; }
        public string payment_pending { get; set; }
        public string approvalpendinginnvoice { get; set; }
        public string potentialleadcount { get; set; }

        public string mtd_over_due_payment { get; set; }
        public string mtd_over_due_payment_amount { get; set; }
        public string mtd_over_due_invoice_amount { get; set; }
        public string mtd_over_due_invoice { get; set; }

        public string ytd_over_due_payment { get; set; }
        public string ytd_over_due_payment_amount { get; set; }
        public string ytd_over_due_invoice_amount { get; set; }
        public string ytd_over_due_invoice { get; set; }
        public string customer_count { get; set; }
    }
    //
    public class GetTilesDetails_list
    {
        public string month_invoiceamount { get; set; }
        public string mtd_invoice { get; set; }
        public string ytd_invoice { get; set; }
        public string ytd_invoiceamount { get; set; }
        public string currency_symbol { get; set; }
        public string customer_count { get; set; }
        public string month_invoiccount { get; set; }
        public string ytd_invoicecount { get; set; }
        
    }
    public class GetSalesdashboard_list
    {
        public string Today_SalesOrder { get; set; }
        public string Today_DespatchOrder { get; set; }
        public string Today_Invoice { get; set; }
        public string Today_Payment { get; set; }
        public string Today_PaymentAmount { get; set; }
        public string Today_InvoiceAmount { get; set; }
        public string Yesterday_SalesOrder { get; set; }
        public string Yesterday_DespatchOrder { get; set; }
        public string Yesterday_Invoice { get; set; }
        public string Yesterday_PaymentAmount { get; set; }
        public string Yesterday_Payment { get; set; }
        public string Yesterday_InvoiceAmount { get; set; }
        public string CurrentWeek_SalesOrder { get; set; }
        public string CurrentWeek_DespatchOrder { get; set; }
        public string CurrentWeek_Invoice { get; set; }
        public string CurrentWeek_Payment { get; set; }
        public string CurrentWeek_PaymentAmount { get; set; }
        public string CurrentWeek_InvoiceAmount { get; set; }
        public string LastWeek_SalesOrder { get; set; }
        public string LastWeek_DespatchOrder { get; set; }
        public string LastWeek_Invoice { get; set; }
        public string LastWeek_Payment { get; set; }
        public string LastWeek_PaymentAmount { get; set; }
        public string LastWeek_InvoiceAmount { get; set; }
        public string CurrentMonth_SalesOrder { get; set; }
        public string CurrentMonth_DespatchOrder { get; set; }
        public string CurrentMonth_Invoice { get; set; }
        public string CurrentMonth_Payment { get; set; }
        public string CurrentMonth_PaymentAmount { get; set; }
        public string CurrentMonth_InvoiceAmount { get; set; }
        public string PreviousMonth_SalesOrder { get; set; }
        public string PreviousMonth_DespatchOrder { get; set; }
        public string PreviousMonth_Invoice { get; set; }
        public string PreviousMonth_Payment { get; set; }
        public string PreviousMonth_PaymentAmount { get; set; }
        public string PreviousMonth_InvoiceAmount { get; set; }
        public string CurrentYear_SalesOrder { get; set; }
        public string CurrentYear_DespatchOrder { get; set; }
        public string CurrentYear_Invoice { get; set; }
        public string CurrentYear_Payment { get; set; }
        public string CurrentYear_PaymentAmount { get; set; }
        public string CurrentYear_InvoiceAmount { get; set; }
        public string PerivousYear_SalesOrder { get; set; }
        public string PerivousYear_DespatchOrder { get; set; }
        public string PerivousYear_Invoice { get; set; }
        public string PerivousYear_Payment { get; set; }
        public string PerivousYear_PaymentAmount { get; set; }
        public string PerivousYear_InvoiceAmount { get; set; }
    }

    public class GetSalesStatus_list
    {
        public string invoice_count { get; set; }
        public string order_count { get; set; }
        public string enquiry_count { get; set; }
        public string quotation_count { get; set; }
        public string customer_count { get; set; }
        public string Months { get; set; }
    }

    public class GetPaymentandDeliveryChart_list
    {
        public string d_count { get; set; }
        public string p_count { get; set; }
        public string pd_month { get; set; }
        public string delivery_pending { get; set; }
        public string delivery_completed { get; set; }
        public string delivery_month { get; set; }
        //public string payment_rejected { get; set; }
        //public string payment_pending { get; set; }
        //public string payment_done_partial { get; set; }
        //public string payment_completed { get; set; }
    }




    //
    public class chartscounts_list2
    {
        public string customermonthcount { get; set; }
        public string quotationmonthcount { get; set; }
        public string customermonth { get; set; }
        public string quotationmonth { get; set; }
        public string enquirymonthcount { get; set; }
        public string enquirymonth { get; set; }
        public string salesmonth { get; set; }
        public string salesmonthcount { get; set; }
        public string quoteorder_amount { get; set; }
        public string salesorder_amount { get; set; }
        public string order_count { get; set; }
        public string quote_count { get; set; }
        public string enquiry_count { get; set; }
        public string customer_count { get; set; }
        public string Months { get; set; }
    }
    public class currency_list : result
    {
        public string currency_code { get; set; }
        public string currency { get; set; }
        public string symbol { get; set; }
    }
    public class GetSalesPerformanceChart_List : result
    {
        public string total_customers { get; set; } 
        public string order_amount { get; set; }
        public string amount { get; set; }
        public string payment_day { get; set; }
        public string payment_amount { get; set; }
        public string invoice_amount { get; set; }
        public string orderdate { get; set; }
        public string outstanding_amount { get; set; }
        public string lsemployee_gid { get; set; }
        public string ordercount { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string salesorder_date { get; set; }
        // public string lsemployee_gid_list { get; set; }

    }
    public class GetSalesOrderCount_List : result
    {
        public string total_so { get; set; }
        public string approved_so { get; set; }
        public string pending_So { get; set; }
        public string rejected_so { get; set; }
        public string total_do { get; set; }
        public string pending_do { get; set; }
        public string completed_do { get; set; }
        public string Partial_done { get; set; }
        public string today_invoice { get; set; }
        public string totalinvoice { get; set; }
        public string so_amended { get; set; }
        public string approved_invoice { get; set; }
        public string payment_pending { get; set; }
        public string pending_invoice { get; set; }
        public string totalpayment { get; set; }
        public string total_quotation { get; set; }
        public string quotation_canceled { get; set; }
        public string quotation_completed { get; set; } 
        public string payment_completed { get; set; }
        public string payment_done_partial { get; set; } 
        public string invoice_count { get; set; }

    }
    public class GetOverallSalesOrderChart_List : result
    {
        public string count_own { get; set; }
        public string salesorder_status_own { get; set; }
        public string count_Hierarchy { get; set; }
        public string salesorder_status_Hierarchy { get; set; }
        public string do_count_own { get; set; }
        public string delivery_status_own { get; set; }
        public string do_count_Hierarchy { get; set; }
        public string delivery_status_Hierarchy { get; set; }
        public string payment_day { get; set; }
        public string amount { get; set; }

    }


    public class Mtd_List : result
    {

        public string mtd_over_due_payment { get; set; }
        public string mtd_over_due_payment_amount { get; set; }
        public string mtd_over_due_invoice_amount { get; set; }
        public string mtd_over_due_invoice { get; set; }

        public string ytd_over_due_payment { get; set; }
        public string ytd_over_due_payment_amount { get; set; }
        public string ytd_over_due_invoice_amount { get; set; }
        public string ytd_over_due_invoice { get; set; }

    }  
    public class Getoverallsalesbarchart_list
    {

        public string overall_enquiry { get; set; }
        public string overall_quotation { get; set; }
        public string overall_salesorder { get; set; }
        public string overall_invoice { get; set; }
    }
    public class Salesordersixmonthchart_list
    {
        public string salesordersixmonth { get; set; }
        public string whatsappordersixmonth { get; set; }
        public string shopifyordersixmonth { get; set; }
        public string salesorder_datesixmonth { get; set; }
    }
}
