using ems.outlet.Dataaccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.outlet.Models;

namespace ems.outlet.Controller
{
    [RoutePrefix("api/OtlTrnPurchaseIndent")]
    [Authorize]
    public class OtlTrnPurchaseIndentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOtlTrnPurchaseIndent objDaoutlet = new DaOtlTrnPurchaseIndent();

        // Purchase indent Summary

        [ActionName("GetOtlTrnPurchaseIndent")]
        [HttpGet]
        public HttpResponseMessage GetOtlTrnPurchaseIndent()
        {
            MdlOtlTrnPurchaseIndent values = new MdlOtlTrnPurchaseIndent();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaoutlet.DaGetOtlTrnPurchaseIndent(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // product details
        [ActionName("GetProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetProductdetails(string purchaserequisition_gid)
        {
            MdlOtlTrnPurchaseIndent objresult = new MdlOtlTrnPurchaseIndent();
            objDaoutlet.DaGetProductdetails(purchaserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Post Product
        [ActionName("PostOnAddpr")]
        [HttpPost]
        public HttpResponseMessage PostOnAddpr(productlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaoutlet.DaPostOnAdd(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // product summary 
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlOtlTrnPurchaseIndent values = new MdlOtlTrnPurchaseIndent();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaoutlet.DaProductSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Raise PI submit
        [ActionName("PostPurchaseRequisition")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseRequisition(PostAllPI values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaoutlet.DaPostPurchaseRequisition(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //View
        [ActionName("GetPurchaseRequisitionView")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseRequisitionView(string purchaserequisition_gid)
        {
            MdlOtlTrnPurchaseIndent objresult = new MdlOtlTrnPurchaseIndent();
            objDaoutlet.DaGetPurchaseRequisitionView(purchaserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetPurchaseRequisitionproduct")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseRequisitionproduct(string purchaserequisition_gid)
        {
            MdlOtlTrnPurchaseIndent objresult = new MdlOtlTrnPurchaseIndent();
            objDaoutlet.DaGetPurchaseRequisitionproduct(purchaserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}