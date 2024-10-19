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
    [RoutePrefix("api/SmrTrnRenewalAssign")]
    [Authorize]
    public class SmrTrnRenewalAssignController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnRenewalAssign objpurchase = new DaSmrTrnRenewalAssign();

        [ActionName("GetRenewalReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetRenewalReportSummary()
        {
            MdlSmrTrnRenewalAssign values = new MdlSmrTrnRenewalAssign();
            objpurchase.DaGetRenewalReportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetRenewalReportCount")]
        [HttpGet]
        public HttpResponseMessage GetRenewalReportCount()
        {
            MdlSmrTrnRenewalAssign values = new MdlSmrTrnRenewalAssign();
            objpurchase.DaGetRenewalReportCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }
}