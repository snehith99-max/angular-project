using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using static ems.pmr.Models.pmrtax_list;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrMstTax")]
    [Authorize]
    public class PmrMstTaxController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrMstTax objDaPurchase = new DaPmrMstTax();
        // Module Summary
        [ActionName("GetTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSummary()
        {
            MdlPmrMstTax values = new MdlPmrMstTax();
            objDaPurchase.DaGetTaxSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Post Tax
        [ActionName("PostTax")]
        [HttpPost]
        public HttpResponseMessage PostTax(pmrtax_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedTaxSummary")]
        [HttpPost]
        public HttpResponseMessage UpdatedTaxSummary(pmrtax_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaUpdatedTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deleteTaxSummary")]
        [HttpGet]
        public HttpResponseMessage deleteTaxSummary(string tax_gid)
        {
            pmrtax_list objresult = new pmrtax_list();
            objDaPurchase.DadeleteTaxSummary(tax_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetTaxSegmentdropdown")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentdropdown()
        {
            MdlPmrMstTax values = new MdlPmrMstTax();
            objDaPurchase.DaGetTaxSegmentdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTaxName")]
        [HttpGet]
        public HttpResponseMessage GetTaxName(string tax_gid)
        {
            MdlPmrMstTax values = new MdlPmrMstTax();
            objDaPurchase.DaGetTaxName(values, tax_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetAssignedProduct")]
        [HttpGet]
        public HttpResponseMessage GetAssignedProduct(string tax_gid)
        {
            MdlPmrMstTax values = new MdlPmrMstTax();
            objDaPurchase.DaGetAssignedProductCount(values, tax_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("GetTaxSegment2ProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegment2ProductSummary(string tax_gid, string taxsegment_gid)
        {
            MdlPmrMstTax values = new MdlPmrMstTax();
            objDaPurchase.GetTaxSegment2ProductSummary(tax_gid, taxsegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMappedProducts")]
        [HttpPost]
        public HttpResponseMessage PostMappedProducts(mapproduct_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostMappedProducts(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxUnmapping")]
        [HttpGet]
        public HttpResponseMessage GetTaxUnmapping(string tax_gid, string taxsegment_gid)
        {
            MdlPmrMstTax values = new MdlPmrMstTax();
            objDaPurchase.DaGetTaxUnmapping(tax_gid, taxsegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUnMappedProducts")]
        [HttpPost]
        public HttpResponseMessage PostUnMappedProducts(mapproduct_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostUnMappedProducts(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}



