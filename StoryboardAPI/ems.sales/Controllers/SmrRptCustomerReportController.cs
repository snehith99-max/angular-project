using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrRptCustomerReport")]
    [Authorize]
    public class SmrRptCustomerReportController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptCustomerReport objsales = new DaSmrRptCustomerReport();

        [ActionName("GetCustomerReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerReportSummary()
        {
            MdlSmrRptCustomerReport values = new MdlSmrRptCustomerReport();
            objsales.DaGetCustomerReportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerDetailedReport")]
        [HttpGet]
        public HttpResponseMessage GetCustomerDetailedReport(string customer_gid, string from_date, string to_date)
        {
            MdlSmrRptCustomerReport values = new MdlSmrRptCustomerReport();
            objsales.DaGetCustomerDetailedReport(customer_gid, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}