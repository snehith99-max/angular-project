using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnRenewalmanagersummary")]
    [Authorize]
    public class SmrTrnRenewalmanagersummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnRenewalmanagersummary objpurchase = new DaSmrTrnRenewalmanagersummary();

        [ActionName("GetRenewalManagerSummary")]
        [HttpGet]
        public HttpResponseMessage GetRenewalManagerSummary()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetRenewalManagerSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetRenewalChart")]
        [HttpGet]
        public HttpResponseMessage GetRenewalChart()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetRenewalChart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRenewalTeamSummary")]
        [HttpGet]
        public HttpResponseMessage GetRenewalTeamSummary()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetRenewalTeamSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSmrTrnEmployeeCount")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnEmployeeCount()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetSmrTrnEmployeeCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSmrTrnRenewalsCount")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnRenewalsCount()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetSmrTrnRenewalsCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMonthyRenewal")]
        [HttpGet]
        public HttpResponseMessage GetMonthyRenewal()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetMonthyRenewal(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRenewalReportForLastSixMonths")]
        [HttpGet]

        public HttpResponseMessage GetRenewalReportForLastSixMonths()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetRenewalReportForLastSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRenewalReportForLastSixMonthsSearch")]
        [HttpGet]

        public HttpResponseMessage GetRenewalReportForLastSixMonthsSearch()
        {
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetRenewalReportForLastSixMonthsSearch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetrenewalDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetrenewalDetailSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnRenewalmanagersummary values = new MdlSmrTrnRenewalmanagersummary();
            objpurchase.DaGetrenewalDetailSummary(getsessionvalues.branch_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}