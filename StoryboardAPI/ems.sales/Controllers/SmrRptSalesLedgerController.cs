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
    [RoutePrefix("api/SmrRptSalesLedger")]
    [Authorize]
    public class SmrRptSalesLedgerController :ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptSalesLedger objsalesledger = new DaSmrRptSalesLedger();


        [ActionName("GetsalesLedgersummary")]
        [HttpGet]
        public HttpResponseMessage GetsalesLedgersummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptSalesLedger values = new MdlSmrRptSalesLedger();
            objsalesledger.DaGetsalesLedgersummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetsalesLedgerdate")]
        [HttpGet]
        public HttpResponseMessage GetsalesLedgerdate(string from_date, string to_date)
        {
            MdlSmrRptSalesLedger values = new MdlSmrRptSalesLedger();
            objsalesledger.DaGetsalesLedgerdate(from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}