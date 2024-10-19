using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;


namespace ems.finance.Controllers
{
    [RoutePrefix("api/AccMstCreditCard")]
    [Authorize]
    public class AccMstCreditCardController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccMstCreditCard objDaCreditcard = new DaAccMstCreditCard();

        [ActionName("GetAccountGroupName")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroupName()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccMstCreditCard values = new MdlAccMstCreditCard();
            objDaCreditcard.DaGetAccountGroupName(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCreditCardSummary")]
        [HttpGet]
        public HttpResponseMessage GetCreditCardSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccMstCreditCard values = new MdlAccMstCreditCard();
            objDaCreditcard.DaGetCreditCardSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostCreditCardDetails")]
        [HttpPost]
        public HttpResponseMessage PostCreditCardDetails(creditcard_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaCreditcard.DaPostCreditCardDetails(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getupdatecreditcarddtls")]
        [HttpPost]
        public HttpResponseMessage Getupdatecreditcarddtls(creditcarddtledit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaCreditcard.DaGetupdatecreditcarddtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostCreditCardStatus")]
        [HttpGet]
        public HttpResponseMessage PostCreditCardStatus(string status_flag, string bank_gid)
        {
            result values = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaCreditcard.DaPostCreditCardStatus(getsessionvalues.user_gid,status_flag, bank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

    }
}