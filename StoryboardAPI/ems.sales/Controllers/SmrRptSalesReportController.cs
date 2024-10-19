using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("/api/SmrRptSalesReport")]
    [Authorize]
    public class SmrRptSalesReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptSalesReport objsalesreport = new DaSmrRptSalesReport();


        [ActionName("GetsalesReportsummary")]
        [HttpGet]
        public HttpResponseMessage GetsalesReportsummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptSalesReport values = new MdlSmrRptSalesReport();
            objsalesreport.DaGetsalesReportsummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetsalesReportdate")]
        [HttpGet]
        public HttpResponseMessage GetsalesReportdate(string from_date, string to_date)
        {
            MdlSmrRptSalesReport values = new MdlSmrRptSalesReport();
            objsalesreport.DaGetsalesReportdate(from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}