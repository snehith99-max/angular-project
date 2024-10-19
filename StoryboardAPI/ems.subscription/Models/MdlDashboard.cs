using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class MdlDashboard:result
    {
        public List<portalchart_list> portalchart_list { get; set; }
        public List<subscriptiontilescount_list> subscriptiontilescount_list { get; set; }
        public List<monthwisedbchart_list> monthwisedbchart_list { get; set; }
    }

    public class portalchart_list
    {
        public string server_name { get; set; }
        public string company_code { get; set; }     
        public string server_gid { get; set; }     
        public string mtd_month { get; set; }     

    }
    public class subscriptiontilescount_list
    {
        public string total_count { get; set; }
        public string company_code { get; set; }
        public string server_name { get; set; }
        
    }
    public class monthwisedbchart_list
    {
        public string db_count { get; set; }
        public string month { get; set; }
        public string month_wise { get; set; }
        public string year { get; set; }
        public string server_count { get; set; }
    }
}