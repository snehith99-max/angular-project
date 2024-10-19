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
    [RoutePrefix("api/OutStandingAmount")]
    [Authorize]
    public class OutStandingAmountController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOutstandingAmount objoutstandingamount = new DaOutstandingAmount();
        [ActionName("GetOutstandingAmountReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetOutstandingAmountReportSummary( string from_date , string to_date)
        {
            MdlOutstandingAmount values = new MdlOutstandingAmount();
            objoutstandingamount.DaGetOutstandingAmountReportSummary(from_date, to_date,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}