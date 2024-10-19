using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlOutstandingAmount : result
    {
        public List<GetOutsrandingAmountResult> GetOutsrandingAmountResult { get; set; }
    }

        public class GetOutsrandingAmountResult:result {
            public string invoice_date {  get; set; }   
            public string invoice_refno { get; set; }
            public string branch { get; set; }
            public string customer_details { get; set; }
            public string invoicetype { get; set; }
            public string invoice_amount { get; set; }
            public string paid_amount { get; set; }
            public string outstanding_amount { get; set; }
            public string due_date { get; set; }
            public string expiry_days { get; set; }
            public string status { get; set; }
            
        }
    }
