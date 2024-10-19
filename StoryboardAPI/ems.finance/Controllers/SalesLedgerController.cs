using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.finance.DataAccess;
using ems.finance.Models;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/SalesLedger")]
    [Authorize]
    public class SalesLedgerController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSalesLedger objsalesledger = new DaSalesLedger();

        [ActionName("GetsalesLedger")]
        [HttpGet]
        public HttpResponseMessage GetsalesLedger()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSalesLedger values = new MdlSalesLedger();
            objsalesledger.DaGetsalesLedger(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetsalesLedgerdatefin")]
        [HttpGet]
        public HttpResponseMessage GetsalesLedgerdatefin(string from_date, string to_date)
        {
            MdlSalesLedger values = new MdlSalesLedger();
            objsalesledger.DaGetsalesLedgerdatefin(from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}