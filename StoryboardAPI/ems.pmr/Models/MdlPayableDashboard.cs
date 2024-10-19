﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPayableDashboard:result
    {

        public List<payable_tile> payable_tile { get; set; }
        public List<payablesummary_list> payablesummary_list { get; set; }

    }

    public class payable_tile : result
    {
        public string total_count { get; set; }
        public string cancel_invoice { get; set; }
        public string pending_count { get; set; }
        public string product_count { get; set; }
        public string vendor_count { get; set; }

    }
    public class payablesummary_list :result
    {
        public string so_referencenumber { get; set; }
        public string payment_date { get; set; }
        public string invoice_gid { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string overall_status { get; set; }
        public string invoice_reference { get; set; }
        public string invoice_from { get; set; }
        public string invoice_status { get; set; }
        public string mail_status { get; set; }
        public string invoice_amount { get; set; }
        public string customer_contactperson { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
    }
}