using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using StoryboardAPI.Models;

namespace ems.mobile.Models
{

    public class MdlOutletDashboard : result
    {
        public List<overallbudget_list> Overallbudget_list { get; set; }
        public List<overallchart_list> Overallchart_Lists { get; set; }
        public List<overallsummary_list> SummaryData_list { get; set; }
        public List<revenue_list> RevenueData_list { get; set; }
        public List<outlet_list> OutletData_list { get; set; }
        public List<dailytodayreport_list> TodayReportData_list { get; set; }
        public List<dailyyesterdayreport_list> YesterdayReportData_list { get; set; }
        public List<customreport_list> CustomReportData_list { get; set; }

        public List<customdetailreport_list> CustomReportDetail_List { get; set; }

    }


    public class customdetailreport_list : result
    {
        public string campaign_description { get; set; }
        public string profit_amount { get; set; }
        public string expense_amount { get; set; }
        public string revenue_amount { get; set; }
        public string loss_amount { get; set; }

    }


    public class customreport_list : result
    {
        public string created_date { get; set; }
        public string profit_amount { get; set; }
        public string expense_amount { get; set; }
        public string revenue_amount { get; set; }
        public string loss_amount { get; set; }

    }

    public class dailytodayreport_list : result
    {
        public string campaign_gid { get; set; }
        public string campaign_description { get; set; }
        public string period { get; set; }
        public string created_date { get; set; }
        public string revenue_amount { get; set; }
        public string expense_amount { get; set; }

        public string profit_amount { get; set; }
        public string loss_amount { get; set; }
     

    }

    public class dailyyesterdayreport_list : result
    {
        public string campaign_description { get; set; }
        public string profit_amount { get; set; }
        public string expense_amount { get; set; }
        public string revenue_amount { get; set; }
        public string loss_amount { get; set; }

    }

    public class overallbudget_list : result
        {
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string outlet_name { get; set; }
            public string total_revenue { get; set; }
            public string total_expense { get; set; }
            public string total_profit { get; set; }
            public string total_loss { get; set; }

        }


        public class overallchart_list : result
        {
            public string created_date { get; set; }
            public string type { get; set; }
            public string amount { get; set; }

        }


    public class overallsummary_list : result
    {
        public string daytrackerdtl_gid { get; set; }
        public string created_date { get; set; }
        public string income_amount { get; set; }
        public string expense_amount { get; set; }
        public string profit_amount { get; set; }
        public string loss_amount { get; set; }
        public string user_firstname { get; set; }
        public string revenue_desc { get; set; }
        public string revenue_code { get; set; }
        public string expense_code { get; set; }
        public string expense_desc { get; set; }
        public string expense_name { get; set; }
        public string campaign_title { get; set; }



    }


    public class outlet_list : result
    {
        public string outlet_name { get; set; }
        public string branch { get; set; }
        public string campaign_description { get; set; }
        public string campaign_status { get; set; }
    }

    public class revenue_list : result
    {

        public string revenue_name { get; set; }
        public string expense_name { get; set; }
        public string revenue_amount { get; set; }
        public string expense_amount { get; set; }

    }

}