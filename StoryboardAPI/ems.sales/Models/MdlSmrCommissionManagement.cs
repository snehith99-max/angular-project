﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{


    public class MdlSmrCommissionManagement : result
    {
        public List<GetCommissionManagement_List> GetCommissionManagement_List { get; set; }
        public List<GetCommissionPayout_List> GetCommissionPayout_List { get; set; }
        public List<invoicesummary_list> invoicesummary_list { get; set; }

        public string salestype_gid { get; set; }
        public string sales_type { get; set; }
        public string neworder_percentage { get; set; }
        public string repeatorder_percentage { get; set; }
        public string customer_gid { get; set; }

        public string invoice_refno { get; set; }
        public string invoice_gid { get; set; }
        public string payable_commission { get; set; }
        public string invoice_amount { get; set; }
        public string commission_amount { get; set; }
        public string balance_payable { get; set; }


    }
    public class GetCommissionManagement_List : result
    {
        public string sales_type { get; set; }
        public string neworder_percentage { get; set; }
        public string repeatorder_percentage { get; set; }


    }

    public class GetCommissionPayout_List : result
    {
        public string commissionpayout_gid { get; set; }
        public string invoice_refno { get; set; }
        public string generation_date { get; set; }
        public string total_invoice { get; set; }
        public string invoice_amount { get; set; }
        public string payable_commission { get; set; }
        public string commission_amount { get; set; }
        public string payout_status { get; set; }
        public string invoice_gid { get; set; }
        public string user_name { get; set; }
        public string campaign_title { get; set; }
        public string campaign_gid { get; set; }
        public string total_count { get; set; }
        public string salesperson_gid { get; set; }
        public string sales_person { get; set; }




    }
    public class invoicesummary_list : result
    {
        public string so_referencenumber { get; set; }
        public string irn { get; set; }
        public string invoice_gid { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string customer_name { get; set; }
        public string invoice_reference { get; set; }
        public string invoice_from { get; set; }
        public string invoice_status { get; set; }
        public string mail_status { get; set; }
        public string invoice_amount { get; set; }
        public string customer_contactperson { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string customer_type { get; set; }
        public string campaign_gid { get; set; }
        public string customer_gid { get; set; }
        public string user_firstname { get; set; }
        public string campaign_title { get; set; }
        public string payable_commission { get; set; }
        public string balance_payable { get; set; }
        public string commission_amount { get; set; }

    }
}