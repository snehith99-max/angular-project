using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.sales.Models;
using ems.sales.DataAccess;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrMstTaxSummary")]
    [Authorize]
    public class SmrMstTaxSummaryController : ApiController
    {
        string msSQL = string.Empty;
        int mnResult;
        dbconn dbconn = new dbconn();

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrMstTaxSummary objDaSales = new DaSmrMstTaxSummary();
        // Module Summary
        [ActionName("GetTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSummary()
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetTaxSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Post Tax
        [ActionName("PostTax")]
        [HttpPost]
        public HttpResponseMessage PostTax(smrtax_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedTaxSummary")]
        [HttpPost]
        public HttpResponseMessage UpdatedTaxSummary(smrtax_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaUpdatedTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deleteTaxSummary")]
        [HttpGet]
        public HttpResponseMessage deleteTaxSummary(string tax_gid)
        {
            smrtax_list objresult = new smrtax_list();
            objDaSales.DadeleteTaxSummary(tax_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetTaxName")]
        [HttpGet]
        public HttpResponseMessage GetTaxName(string tax_gid)
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetTaxName(values, tax_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUnMappedProducts")]
        [HttpGet]
        public HttpResponseMessage GetUnMappedProducts(string tax_gid,string taxsegment_gid)
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetUnMappedProducts(values, tax_gid, taxsegment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("PostMappedProducts")]
        //[HttpPost]
        ////public HttpResponseMessage PostMappedProducts(mapproduct_lists values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDaSales.DaPostMappedProducts(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetAssignedProduct")]
        [HttpGet]
        public HttpResponseMessage GetAssignedProduct(string tax_gid)
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetAssignedProductCount(values, tax_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductCounts")]
        [HttpGet]
        public HttpResponseMessage GetProductCounts()
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetProductCounts(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTaxSegmentdropdown")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentdropdown()
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetTaxSegmentdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UnAssignProduct")]
        [HttpPost]
        public HttpResponseMessage UnAssignProduct(PostUnassignedProduct values)
        {
            for (int i = 0; i < values.unassignproductchecklist.ToArray().Length ; i++)
            {
                msSQL = "delete from acp_mst_ttaxsegment2product where " +
                    "taxsegment2product_gid='" + values.unassignproductchecklist[i].taxsegment2product_gid + "'";
                mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Product Unassigned successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while unassigning product";
                }
            }
          
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTaxSegment2ProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegment2ProductSummary(string tax_gid, string taxsegment_gid)
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.GetTaxSegment2ProductSummary(tax_gid, taxsegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMappedProducts")]
        [HttpPost]
        public HttpResponseMessage PostMappedProducts(mapproduct_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostMappedProducts(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxUnmapping")]
        [HttpGet]
        public HttpResponseMessage GetTaxUnmapping (string tax_gid, string taxsegment_gid)
        {
            MdlSmrMstTaxSummary values = new MdlSmrMstTaxSummary();
            objDaSales.DaGetTaxUnmapping (tax_gid, taxsegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUnMappedProducts")]
        [HttpPost]
        public HttpResponseMessage PostUnMappedProducts(mapproduct_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostUnMappedProducts(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}
