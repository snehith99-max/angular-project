using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Web.Http;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/AccTrailBalanceReport")]
    [Authorize]
    public class AccTrailBalanceReportController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccTrailBalanceReport objfinance = new DaAccTrailBalanceReport();

        [ActionName("GetTrialBalanceTransactionDtls")]
        [HttpGet]
        public HttpResponseMessage GetTrialBalanceTransactionDtls(string account_gid)
        {
            MdlAccTrailBalanceReport values = new MdlAccTrailBalanceReport();
            objfinance.DaGetTrialBalanceTransactionDtls(account_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTrialBalanceSummary")]
        [HttpGet]
        public HttpResponseMessage GetTrialBalanceSummary(string branch, string year_gid)
        {
            MdlAccTrailBalanceReport values = new MdlAccTrailBalanceReport();
            objfinance.DaGetTrialBalanceSummary(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}

