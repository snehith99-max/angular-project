using ems.mobile.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.mobile.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;


namespace ems.mobile.Controllers
{
    [Authorize]
    [RoutePrefix("api/MblOutletDashboard")]
    public class MblOutletDashboardController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOutletDashboard objDaMobileDasboard = new DaOutletDashboard();

        
        [ActionName("GetOverAllBudget")]
        [HttpGet]
        public HttpResponseMessage GetOverAllBudgetBudgetDetails(string selectedTab,string headerSelectedTab,string outletName)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetBudgetDetails(getsessionvalues.user_gid, values, selectedTab, headerSelectedTab, outletName);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOverAllChartValue")]
        [HttpGet]
        public HttpResponseMessage GetOverAllChartValueDetails(string selectedTab,string headerSelectedTab,string outletName)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetChartDetails(getsessionvalues.user_gid, values, selectedTab, headerSelectedTab,outletName);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSummaryData")]
        [HttpGet]
        public HttpResponseMessage GetSummaryDataValueDetails(string selectedTab, string headerSelectedTab, string outletName)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetSummaryDetails(getsessionvalues.user_gid, values, selectedTab, headerSelectedTab, outletName);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOutletName")]
        [HttpGet]
        public HttpResponseMessage GetOutletNameDetails(string selectedTab, string headerSelectedTab)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetOutletNameDetails(getsessionvalues.user_gid, values, selectedTab, headerSelectedTab);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseRevenueDetials")]
        [HttpGet]
        public HttpResponseMessage GetExpenseRevDetails(string createdDate, string outletName)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetExpenseRevenueDetials(getsessionvalues.user_gid, values, createdDate, outletName);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetTodayReport")]
        [HttpGet]
        public HttpResponseMessage GetTodayReportDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetTodayReportDetails(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetYesterdayReport")]
        [HttpGet]
        public HttpResponseMessage GetYesterdayReportDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetYesterdayReportDetails(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomReport")]
        [HttpGet]
        public HttpResponseMessage GetCustomReportDetails(string fromDate, string toDate)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetCustomReportDetails(getsessionvalues.user_gid, values, fromDate, toDate);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomDetailReport")]
        [HttpGet]
        public HttpResponseMessage GetCustomDetailReportDetails(string created_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetCustomDetailReportDetails(getsessionvalues.user_gid, values, created_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOverallOutletdailyReport")]
        [HttpGet]
        public HttpResponseMessage GetOverallOutletdailyReportDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlOutletDashboard values = new MdlOutletDashboard();
            objDaMobileDasboard.DaGetOverallOutletdailyReport(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }

}