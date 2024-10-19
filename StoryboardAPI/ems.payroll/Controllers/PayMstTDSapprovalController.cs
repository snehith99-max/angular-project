using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.payroll.Controllers
{
    [RoutePrefix("api/PayMstTDSapproval")]
    [Authorize]
    public class PayMstTDSapprovalController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstTDSapproval objDaPayMstTDSApproval = new DaPayMstTDSapproval();

        [ActionName("GetTDSApprovalPendingSummary")]
        [HttpGet]
        public HttpResponseMessage GetTDSApprovalPendingSummary()
        {
            MdlPayMstTDSapproval values = new MdlPayMstTDSapproval();
            objDaPayMstTDSApproval.DaTDSApprovalPendingSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostTDSApprove")]
        [HttpPost]
        public HttpResponseMessage PostTDSApprove(MdlPayMstPostTDSApprove values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstTDSApproval.DaPostTDSApprove(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTDSApprovedSummary")]
        [HttpGet]
        public HttpResponseMessage GetTDSApprovedSummary()
        {
            MdlPayMstTDSapproval values = new MdlPayMstTDSapproval();
            objDaPayMstTDSApproval.DaTDSApprovedSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}