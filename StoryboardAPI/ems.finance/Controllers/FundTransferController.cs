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
    [RoutePrefix("api/FundTransfer")]
    public class FundTransferController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaFundTransfer objdacreditcard = new DaFundTransfer();

        [ActionName("GetFundTransferSummary")]
        [HttpGet]
        public HttpResponseMessage GetFundTransferSummary()
        {
            MdlFundTransfer values = new MdlFundTransfer();
            objdacreditcard.DaGetFundTransferSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostFundTransfer")]
        [HttpPost]
        public HttpResponseMessage PostFundTransfer(FundTransfer_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdacreditcard.DaPostFundTransfer(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
         [ActionName("UpdateFundTransfer")]
        [HttpPost]
        public HttpResponseMessage UpdateFundTransfer(FundTransfer_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdacreditcard.DaUpdateFundTransfer(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteFundTransfers")]
        [HttpGet]
        public HttpResponseMessage DeleteFundTransfers(string pettycash_gid)
        {
            results values = new results();
            objdacreditcard.DaDeleteFundTransfers(pettycash_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}