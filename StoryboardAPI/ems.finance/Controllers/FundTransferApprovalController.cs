using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.finance.DataAccess;
using ems.finance.Models;

namespace ems.finance.Controllers
{
    [Authorize]
    [RoutePrefix("api/FundTransferApproval")]
    public class FundTransferApprovalController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaFundTransferApproval objdacreditcard = new DaFundTransferApproval();

        [ActionName("GetFundTransferApprovalSummary")]
        [HttpGet]
        public HttpResponseMessage GetFundTransferApprovalSummary()
        {
            MdlFundTransferApproval values = new MdlFundTransferApproval();
            objdacreditcard.DaGetFundTransferApprovalSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("FundTransferApprovalStatus")]
        [HttpPost]
        public HttpResponseMessage FundTransferApprovalStatus(FundTransferApproval_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdacreditcard.DaFundTransferApprovalStatus(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}