﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.einvoice.Models
{
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }


    }
    public class MdlInvoicedashboard : result
    {
        public List<einvoicesummary_list> einvoicesummary_list { get; set; }
        public List<TileCount> TileCount { get; set; }
    }


        public class TileCount : result
        {
            public string product_count { get; set; }
            public string customer_count { get; set; }
            public string total_raised_invoice { get; set; }
            public string invoice_cancelled { get; set; }
            public string cancelled_invoice { get; set; }
            public string irn_generated { get; set; }
            public string irn_Pending { get; set; }
            public string irn_cancelled { get; set; }
            public string credit_note { get; set; }

        }
        public class einvoicesummary_list : result
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
        }

    }



