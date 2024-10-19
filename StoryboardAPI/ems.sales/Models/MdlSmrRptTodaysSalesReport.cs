﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoryboardAPI.Models;

namespace ems.sales.Models
{
    public class MdlSmrRptTodaysSalesReport : result
    {
        public List<GetOrderForLastSixMonths_List> GetOrderForLastSixMonths_List { get; set; }
        public List<GetMonthwiseOrderReport_List> GetMonthwiseOrderReport_List { get; set; }
        public List<GetTodayDeliveryOrderReport_List> GetTodayDeliveryOrderReport_List { get; set; }
        public List<GetTodaySalesReport_List> GetTodaySalesReport_List { get; set; }
        public List<GetTodayInvoiceReport_List> GetTodayInvoiceReport_List { get; set; }
        public List<GetTodayPaymentReport_List> GetTodayPaymentReport_List { get; set; }
        
        public string today_date { get; set; }
        public string today_total_so { get; set; }
        public string today_total_do { get; set; }
        public string today_total_invoice { get; set; }
        public string today_total_payment { get; set; }
        public string today_invoice_amount { get; set; }
        public string today_payment_amount { get; set; }
        public string today_outstanding_amount { get; set; }
        public string yest_total_so { get; set; }
        public string yest_total_do { get; set; }
        public string yest_total_invoice { get; set; }
        public string yest_total_payment { get; set; }
        public string yest_invoice_amount { get; set; }
        public string yest_payment_amount { get; set; }
        public string yest_outstanding_amount { get; set; }

        public string cw_total_so { get; set; }
        public string cw_total_do { get; set; }
        public string cw_total_invoice { get; set; }
        public string cw_total_payment { get; set; }
        public string cw_invoice_amount { get; set; }
        public string cw_payment_amount { get; set; }
        public string cw_outstanding_amount { get; set; }
        public string lw_total_so { get; set; }
        public string lw_total_do { get; set; }
        public string lw_total_invoice { get; set; }
        public string lw_total_payment { get; set; }
        public string lw_invoice_amount { get; set; }
        public string lw_payment_amount { get; set; }
        public string lw_outstanding_amount { get; set; }

        public string cm_total_so { get; set; }
        public string cm_total_do { get; set; }
        public string cm_total_invoice { get; set; }
        public string cm_total_payment { get; set; }
        public string cm_invoice_amount { get; set; }
        public string cm_payment_amount { get; set; }
        public string cm_outstanding_amount { get; set; }
        public string lm_total_so { get; set; }
        public string lm_total_do { get; set; }
        public string lm_total_invoice { get; set; }
        public string lm_total_payment { get; set; }
        public string lm_invoice_amount { get; set; }
        public string lm_payment_amount { get; set; }
        public string lm_outstanding_amount { get; set; }

        public string cy_total_so { get; set; }
        public string cy_total_do { get; set; }
        public string cy_total_invoice { get; set; }
        public string cy_total_payment { get; set; }
        public string cy_invoice_amount { get; set; }
        public string cy_payment_amount { get; set; }
        public string cy_outstanding_amount { get; set; }
        public string ly_total_so { get; set; }
        public string ly_total_do { get; set; }
        public string ly_total_invoice { get; set; }
        public string ly_total_payment { get; set; }
        public string ly_invoice_amount { get; set; }
        public string ly_payment_amount { get; set; }
        public string ly_outstanding_amount { get; set; }
    }
    public class GetTodayDeliveryOrderReport_List : result
    {
       
        public string branch_name { get; set; }
       
        public string customer_contact_person { get; set; }
       
        public string customer_name { get; set; }
        public string contact { get; set; }

        public string directorder_gid { get; set; }
        public string directorder_refno { get; set; }
        public string directorder_date { get; set; }
        public string customer_branchname { get; set; }
        public string directorder_status { get; set; }
        public string delivery_status { get; set; }
        public string delivery_details { get; set; }


    }
    public class GetTodaySalesReport_List : result
    {
        public string so_referenceno1 { get; set; }
        public string salesorder_date { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string customer_contact_person { get; set; }
        public string salesorder_status { get; set; }
        public string currency_code { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string Grandtotal { get; set; }
        public string grandtotal_dtl { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }


        public string product_name { get; set; }
        public string uom_name { get; set; }
        public string qty_quoted { get; set; }

        public string product_price { get; set; }
        public string product_totalprice { get; set; }
        public string productgroup_name { get; set; }
        public string price { get; set; }
    
       
        

    }

    public class GetTodayInvoiceReport_List : result
    {
      
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string branch_name { get; set; }
        public string invoice_amount { get; set; }
        public string contact { get; set; }
        public string overall_status { get; set; }
        public string invoice_status { get; set; }
        public string customer_name { get; set; }
       


    } 
    public class GetTodayPaymentReport_List : result
    {             
        public string payment_date { get; set; }
        public string payment_gid { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string amount { get; set; }
        public string payment_mode { get; set; }
        public string approval_status { get; set; }
    }
}