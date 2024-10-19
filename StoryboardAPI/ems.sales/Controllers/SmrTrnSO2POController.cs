//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ems.sales.Controllers
//{
//    public class SmrTrnSO2POController
//    {
//    }
//}

using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;




namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnSO2PO")]
    [Authorize]
    public class SmrTrnSO2POController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnSO2PO objpurchase = new DaSmrTrnSO2PO();

        //[ActionName("GetPurchaseOrderSummaryaddselect")]
        //[HttpGet]
        //public HttpResponseMessage GetPurchaseOrderSummaryaddselect(string branch_gid)
        //{
        //    MdlSO2PO values = new MdlSO2PO();
        //    objpurchase.DaGetPurchaseOrderAddselect(branch_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);

        //}

        [ActionName("GetSO2POSummary")]
        [HttpGet]
        public HttpResponseMessage GetSO2POSummary(string salesorder_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlSO2PO values = new MdlSO2PO();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetSO2POSummary(salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetRaisePOSummary")]
        [HttpGet]
        public HttpResponseMessage GetRaisePOSummary(string purchaserequisition_gid)
        {
            MdlSO2PO objresult = new MdlSO2PO();
            objpurchase.DaGetRaisePOSummary(purchaserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostPoaddSubmit")]
        [HttpGet]
        public HttpResponseMessage PostPoaddSubmit(string purchaserequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSO2PO values = new MdlSO2PO();
            objpurchase.DaPostPoaddSubmit(getsessionvalues.user_gid, purchaserequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPurchaseOrderSelect")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSelect()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlSO2PO values = new MdlSO2PO();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetPurchaseOrderSelect(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        [ActionName("PostOverallSubmit")]
        [HttpPost]
        public HttpResponseMessage PostOverallSubmit(postpo_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostOverallSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductsearchSummary1")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary1(string producttype_gid, string product_gid, string vendor_gid)
        {
            MdlSO2PO values = new MdlSO2PO();
            objpurchase.DaGetProductsearchSummary1(producttype_gid, product_gid, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSO2POproducttax")]
        [HttpGet]
        public HttpResponseMessage GetSO2POproducttax(string salesorder_gid, string vendor_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSO2PO values = new MdlSO2PO();
            objpurchase.DaGetSO2POproducttax(getsessionvalues.user_gid, salesorder_gid, vendor_gid, values);
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