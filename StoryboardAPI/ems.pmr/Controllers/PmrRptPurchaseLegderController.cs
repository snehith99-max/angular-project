using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrRptPurchaseLegder")]
    [Authorize]
    public class PmrRptPurchaseLegderController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrRptPurchaseLegder objpurchase = new DaPmrRptPurchaseLegder();

        [ActionName("GetPurchaselegderSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaselegderSummary()
        {
            MdlPmrRptPurchaseLegder values = new MdlPmrRptPurchaseLegder();
            objpurchase.DaGetPurchaselegderSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaselegderDate")]
        [HttpGet]
        public HttpResponseMessage GetPurchaselegderDate(string from_date, string to_date)
        {
            MdlPmrRptPurchaseLegder values = new MdlPmrRptPurchaseLegder();
            objpurchase.DaGetPurchaselegderDate(from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}