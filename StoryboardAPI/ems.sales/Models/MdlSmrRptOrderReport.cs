using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using DocumentFormat.OpenXml.Presentation;
using StoryboardAPI.Models;

namespace ems.sales.Models
{
    public class MdlSmrRptOrderReport : result
    {
        public List<GetOrderForLastSixMonths_List> GetOrderForLastSixMonths_List { get; set; }
        public List<GetOrders> GetOrders { get; set; }
        public List<GetOrderDetailSummary> GetOrderDetailSummary { get; set; }
        public List<salesperson_list> salesperson_list { get; set; }
        public List<GetOrdersummary> GetOrdersummary { get; set; }
        public List<GetMonthwiseOrderReport_List> GetMonthwiseOrderReport_List { get; set; }
        public List<GetOrderwiseOrderReport_List> GetOrderwiseOrderReport_List { get; set; }
        public List<customerdata_list> customerdata_list { get; set; }
        public List<Product_list> Product_list { get; set; }
        public List<Delivery_list> Delivery_list { get; set; }
        public List<individualreport_list> individualreport_list { get; set; }
        public string month_wise { get; set; }
        public string total_invoice { get; set; }
        public string total_payment { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        
    }

    public class customerdata_list : result
    {
        public string salesorder_gid { get; set; }
        public string salesorder_date { get; set; }
        public string so_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string salesorder_status { get; set; }
        public string created_by { get; set; }
        public string Grandtotal { get; set; }
    }
    public class salesperson_list : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }
    }
    public class GetOrderForLastSixMonths_List : result
    {
        public string month { get; set; }
        public string month_wise { get; set; }
        public string orderamount { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public string amount1 { get; set; }
        public string ordercount { get; set; }
        public string salesorder_date { get; set; }        
        public string contact_details { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string grandtotal_l { get; set; }
        public string customer_name { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }

    }
    public class GetOrdersummary : result
    {
        public string year { get; set; }
        public string amount { get; set; }
        public string so_total { get; set; }
        public string month_wise { get; set; }
        public string month { get; set; }
        public string salesorder_date { get; set; }
        public string salesorder_gid { get; set; }
    }
    public class GetOrders : result
    {
        public string month { get; set; }
        public string orderamount { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public string ordercount { get; set; }
        public string salesorder_date { get; set; }        
        public string contact_details { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string grandtotal_l { get; set; }
        public string customer_name { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string month_wise { get; set; }
        public string so_total { get; set; }

    }
    public class GetOrderDetailSummary : result
    {
        public string salesorder_date { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string so_referenceno1 { get; set; }
        public string grandtotal_l { get; set; }
        public string salesorder_gid { get; set; }

    }
    public class GetMonthwiseOrderReport_List : result
    {
        public string month_wise { get; set; }
        public string so_total { get; set; }
        public string total { get; set; }
        public string total_invoice { get; set; }
        public string total_payment { get; set; }
        public string year { get; set; }
        public string salesorder_gid { get; set; }
    }
    public class Product_list : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string currency_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_quoted { get; set; }
        public string customer_name { get; set; }
        public string quotation_date { get; set; }



    }
    public class Delivery_list : result
    {
        public string directorder_refno { get; set; }
        public string directorder_date { get; set; }
        public string delivered_date { get; set; }
        public string delivered_by { get; set; }
        public string product_qtydelivered { get; set; }
        
    }

    public class GetOrderwiseOrderReport_List : result
    {
        public string month_wise { get; set; }
        public string salesorder_gid { get; set; }
        public string so_total { get; set; }
        public string total { get; set; }
        public string total_invoice { get; set; }
        public string total_payment { get; set; }
    }
    public class individualreport_list
    {
        public string salesorder_date { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string pending_invoice_amount { get; set; }
        public string invoice_amount { get; set; }
        public string advance_amount { get; set; }
        public string grand_total { get; set; }
        public string invoice_date { get; set; }
        public string so_referenceno1 { get; set; }
        public string invoice_refno { get; set; }
        public string branch_name { get; set; }
        public string customer_contact { get; set; }
        public string customer_address { get; set; }
        public string customer_details { get; set; }
        public string so_type { get; set; }
        public string salesorder_status { get; set; }
    }


}