using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/PurchaseReturn")]
    [Authorize]
    public class PurchaseReturnController : ApiController
    {
        session_values objGetGid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPurchaseReturn objpurchasereturn = new DaPurchaseReturn();

        [ActionName("GetPurchaseReturnSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseReturnSummary()
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            objpurchasereturn.DaGetPurchaseReturnSummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        //---------------------------------------purchase return add select----------------------------------//
        [ActionName("GetBranchPurchaseReturn")]
        [HttpGet]
        public HttpResponseMessage GetBranchPurchaseReturn()
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objGetGid.gettokenvalues(token);
            objpurchasereturn.DaGetBranchPurchaseReturn(getsessionvalues.user_gid,values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseReturnGRN")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseReturnGRN(string branch_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            objpurchasereturn.DaGetPurchaseReturnGRN(branch_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
//---------------------------------------- add purchase return --------------------------------------------//
        [ActionName("GetPurchaseReturnDetailsSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseReturnDetailsSummary(string vendor_gid, string grn_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objGetGid.gettokenvalues(token);
            objpurchasereturn.DaGetPurchaseReturnDetailsSummary(vendor_gid, grn_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("PostPurchaseReturn")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseReturn(PostPurchaseReturn_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objGetGid.gettokenvalues(token);
            objpurchasereturn.DaPostPurchaseReturn(getsessionvalues.user_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseReturnView")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseReturnView(string purchasereturn_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            objpurchasereturn.DaGetPurchaseReturnView(purchasereturn_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseReturnViewDetails")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseReturnViewDetails(string purchasereturn_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            objpurchasereturn.DaGetPurchaseReturnViewDetails(purchasereturn_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("PurchaseReturnCancel")]
        [HttpGet]
        public HttpResponseMessage PurchaseReturnCancel(string purchasereturn_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            objpurchasereturn.DaPurchaseReturnCancel(purchasereturn_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        [ActionName("GetViewSRProduct")]
        [HttpGet]
        public HttpResponseMessage GetViewSRProduct(string purchasereturn_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            objpurchasereturn.DaGetViewSRProduct(purchasereturn_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetPurchaseReturnRpt")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseReturnRpt(string purchasereturn_gid)
        {
            MdlPurchaseReturn values = new MdlPurchaseReturn();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objGetGid.gettokenvalues(token);
            var ls_response = new Dictionary<string, object>();
            ls_response = objpurchasereturn.DaGetPurchaseReturnRpt(purchasereturn_gid, getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);

        }


    }
}