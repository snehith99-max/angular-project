using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnRenewalAssign : result
    {
        public List<renewalsreport_list> renewalsreport_list { get; set; }
        public List<renewalscount_list> renewalscount_list {  get; set; }
    }

    public class renewalsreport_list : result
    {

        public string salesorder_date1 { get; set; }
        public string renewal_date1 { get; set; }
        public string customer_names { get; set; }
        public string contact_detail { get; set; }
        public string renewal_descriptions { get; set; }
        public string Grandtotals { get; set; }
        public string renewal_status1 { get; set; }
    }
    public class renewalscount_list : result
    {
        public string upcoming { get; set; }
        public string renewed { get; set; }
        public string dropped { get; set; }
    }
}