using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.pmr.Controllers
{
    [Authorize]
    [RoutePrefix("api/PmrRptPaymentReport")]
    public class PmrRptPaymentReportController : ApiController
    {
        session_values objgetgid =  new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrRptPaymentReport objPaymentReport = new DaPmrRptPaymentReport();

        [ActionName("GetPaymentReportforSixMonths")]
        [HttpGet]
        public HttpResponseMessage GetPaymentReportforSixMonths()
        {
            MdlPmrRptPaymentReport values = new MdlPmrRptPaymentReport();
            objPaymentReport.DaGetPaymentReportforSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPaymentReportDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetPaymentReportDetailSummary(string month, string year, string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlPmrRptPaymentReport values = new MdlPmrRptPaymentReport();
            objPaymentReport.DaGetPaymentReportDetailSummary(getsessionvalues.employee_gid, month, year, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPaymentReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetPaymentReportForLastSixMonthsSearch(string from_date, string to_date)
        {
            MdlPmrRptPaymentReport values = new MdlPmrRptPaymentReport();
            objPaymentReport.DaGetPaymentReportForLastSixMonthsSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}