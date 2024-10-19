using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrRptPaymentReport : result
    {
        public List<GetPaymentOrderForLastSixMonths_List> GetPaymentOrderForLastSixMonths_List { get; set; }
        public List<GetPaymentDetailSummary> GetPaymentDetailSummary { get; set; }
        public List<GetPaymentForLastSixMonths_List> GetPaymentForLastSixMonths_List { get; set; }
    }
    public class GetPaymentOrderForLastSixMonths_List : result
    {
        public string payment_amount { get; set; }
        public string year { get; set; }
        public string payment_date { get; set; }
        public string month_na { get; set; }
        public string payment_gid { get; set; }
        public string payment_count { get; set; }
    }

    public class GetPaymentDetailSummary : result
    {
        public string purchaseorder_date { get; set; }
        public string porefno { get; set; }
        public string vendor_companyname { get; set; }
        public string contact_details { get; set; }
        public string purchaseorder_status { get; set; }
        public string contactperson_name { get; set; }
        public string grandtotal_l { get; set; }
        public string purchaseorder_gid { get; set; }
        public string tds_amount { get; set; }
        public string payment_mode { get; set; }
        public string payment_gid { get; set; }
        public string payment_status { get; set; }
        public string payment_amount { get; set; }
        public string payment_date { get; set; }

    }
    public class GetPaymentForLastSixMonths_List : result
    {
        public string month { get; set; }
        public string orderamount { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public string ordercount { get; set; }
        public string month_na { get; set; }
        public string payment_amount { get; set; }
        public string payment_count { get; set; }
        public string payment_gid { get; set; }
        public string payment_date { get; set; }
        public string contact_details { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string grandtotal_l { get; set; }
        public string customer_name { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }

    }
}