using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;




namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTrnPurchaseorderAddselect")]
    [Authorize]
    public class PmrTrnPurchaseorderAddselectController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnPurchaseorderAddselect objpurchase = new DaPmrTrnPurchaseorderAddselect();

        [ActionName("GetPurchaseOrderSummaryaddselect")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSummaryaddselect(string branch_gid)
        {
            MdlPmrTrnPurchaseorderAddselect values = new MdlPmrTrnPurchaseorderAddselect();
            objpurchase.DaGetPurchaseOrderAddselect(branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
     
        [ActionName("GetPurchaseOrderSelectSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSelectSummary(string purchaserequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlPmrTrnPurchaseorderAddselect values = new MdlPmrTrnPurchaseorderAddselect();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetPurchaseOrderSelectSummary(purchaserequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetRaisePOSummary")]
        [HttpGet]
        public HttpResponseMessage GetRaisePOSummary(string purchaserequisition_gid)
        {
            MdlPmrTrnPurchaseorderAddselect objresult = new MdlPmrTrnPurchaseorderAddselect();
            objpurchase.DaGetRaisePOSummary(purchaserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostPoaddSubmit")]
        [HttpGet]
        public HttpResponseMessage PostPoaddSubmit(string purchaserequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrTrnPurchaseorderAddselect values = new MdlPmrTrnPurchaseorderAddselect();
            objpurchase.DaPostPoaddSubmit(getsessionvalues.user_gid, purchaserequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPurchaseOrderSelect")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSelect()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlPmrTrnPurchaseorderAddselect values = new MdlPmrTrnPurchaseorderAddselect();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetPurchaseOrderSelect(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        [ActionName("PostOverallSubmit")]
        [HttpPost]
        public HttpResponseMessage PostOverallSubmit (postpo_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostOverallSubmit (getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductsearchSummary1")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary1(string producttype_gid, string product_gid, string vendor_gid)
        {
            MdlPmrTrnPurchaseorderAddselect values = new MdlPmrTrnPurchaseorderAddselect();
            objpurchase.DaGetProductsearchSummary1(producttype_gid, product_gid, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseOrderSelectSummarytax")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSelectSummarytax( string purchaserequisition_gid , string vendor_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                       getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrTrnPurchaseorderAddselect values = new MdlPmrTrnPurchaseorderAddselect();
            objpurchase.DaGetPurchaseOrderSelectSummarytax(getsessionvalues.user_gid,purchaserequisition_gid, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

         // submit with file upload
        [ActionName("PostPurchaseOrderfileupload")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseOrderfileupload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objpurchase.DaPostPurchaseOrderfileupload(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
}