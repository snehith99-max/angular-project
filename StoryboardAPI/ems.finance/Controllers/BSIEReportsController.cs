using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.finance.Controllers
{
    public class BSIEReports
    {

        [RoutePrefix("api/BSIEReports")]
        [Authorize]

        public class BSIEReportsController : ApiController
        {
            session_values objgetgid = new session_values();
            logintoken getsessionvalues = new logintoken();
            DaBSIEReports objDaBSIEReports = new DaBSIEReports();
            [ActionName("GetLiabilitySummary")]
            [HttpGet]
            public HttpResponseMessage GetLiabilitySummary(string entity_gid, string finyear)
            {
                MdlBSIEReports values = new MdlBSIEReports();
                objDaBSIEReports.DaGetLiabilitySummary(entity_gid, finyear, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            [ActionName("GetAssetSummary")]
            [HttpGet]
            public HttpResponseMessage GetAssetSummary(string entity_gid, string finyear)
            {
                MdlBSIEReports values = new MdlBSIEReports();
                objDaBSIEReports.DaGetAssetSummary(entity_gid, finyear, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            [ActionName("GetLedgerView")]
            [HttpGet]
            public HttpResponseMessage GetLedgerView(string account_gid, string finyear, string branch_name)
            {
                MdlBSIEReports values = new MdlBSIEReports();
                objDaBSIEReports.DaGetLedgerView(account_gid, finyear, branch_name, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

            [ActionName("GetIncomeSummary")]
            [HttpGet]
            public HttpResponseMessage GetIncomeSummary(string finyear)
            {
                MdlBSIEReports values = new MdlBSIEReports();
                objDaBSIEReports.DaGetIncomeSummary(finyear, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            [ActionName("GetExpenseSummary")]
            [HttpGet]
            public HttpResponseMessage GetExpenseSummary(string finyear)
            {
                MdlBSIEReports values = new MdlBSIEReports();
                objDaBSIEReports.DaGetExpenseSummary(finyear, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
        }   
    }
}