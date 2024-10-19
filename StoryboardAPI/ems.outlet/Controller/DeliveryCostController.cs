using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.outlet.Dataaccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.outlet.Models;

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("api/DeliveryCost")]
    public class DeliveryCostController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaDeliveryCost objdelivery = new DaDeliveryCost();

        [ActionName("GetDeliveryCostSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeliveryCostSummary()
        {
            MdlDeliveryCost values = new MdlDeliveryCost();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdelivery.DaGetDeliveryCostSummary(getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDeliveryCost")]
        [HttpPost]
        public HttpResponseMessage PostDeliveryCost(PostDeliverycost_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdelivery.DaPostDeliveryCost(getsessionvalues.user_gid,getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPincodeSummaryAssign")]
        [HttpGet]
        public HttpResponseMessage GetPincodeSummaryAssign()
        {
            MdlDeliveryCost values = new MdlDeliveryCost();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdelivery.DaGetPincodeSummaryAssign(getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAssignPincode2deliverycost")]
        [HttpPost]
        public HttpResponseMessage PostAssignPincode2deliverycost(PostAssignPincodedelivery_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdelivery.PostAssignPincode2deliverycost(getsessionvalues.branch_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}