﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayRptPaymentSummary : result
    {
        public List<GetPaymentReportlist> paymentreportlist { get; set; }
    }

    public class GetPaymentReportlist : result
    {
        public string employee_count { get; set; }
        public string payment_month { get; set; }
        public string payment_year { get; set; }
        public string payment_amount { get; set; }
    }

}