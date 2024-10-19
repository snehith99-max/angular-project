using ems.sales.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrQuoteToOrder")]
    [Authorize]
    public class SmrQuoteToOrderController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrQuoteToOrder objDaSales = new DaSmrQuoteToOrder();

        // Summary bind

        [ActionName("GetRaiseSOSummary")]
        [HttpGet]
        public HttpResponseMessage GetRaiseSOSummary(string quotation_gid)
        {
            MdlSmrQuoteToOrder objresult = new MdlSmrQuoteToOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetRaiseSOSummary(quotation_gid, getsessionvalues.employee_gid,objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetDeleteQuotetoOrderProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteQuotetoOrderProductSummary(string tmpsalesorderdtl_gid)
        {
            GetsummaryList objresult = new GetsummaryList();
            objDaSales.DaGetDeleteQuotetoOrderProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostQuotationToOrder")]
        [HttpPost]
        public HttpResponseMessage PostQuotationToOrder(postsalesQuote_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostQuotationToOrder(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetOnChangeProductsNameQTO")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsNameQTO(string customercontact_gid, string product_gid)
        {
            MdlSmrQuoteToOrder values = new MdlSmrQuoteToOrder();
            objDaSales.GetOnChangeProductsNameQTO(customercontact_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetProductAdd")]
        [HttpPost]
        public HttpResponseMessage GetProductAdd(PostProductQuote values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetProductAdd(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTemporarySummary")]
        [HttpGet]

        public HttpResponseMessage GetTemporarySummary(string quotation_gid)
        {
            MdlSmrQuoteToOrder values = new MdlSmrQuoteToOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetTemporarySummary(getsessionvalues.employee_gid, quotation_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //  QUOTATION TO ORDER PRODUCT EDIT

        [ActionName("GetQuotetoOrderProductEditSummary")]
        [HttpGet]
        public HttpResponseMessage GetQuotetoOrderProductEditSummary(string tmpsalesorderdtl_gid)
        {
            MdlSmrQuoteToOrder objresult = new MdlSmrQuoteToOrder();
            objDaSales.DaGetQuotetoOrderProductEditSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE PRODUCT --  QUOTATION TO ORDER PRODUCT SUMMARY

        [ActionName("PostUpdateQuotationtoOrderProductSummary")]
        [HttpPost]
        public HttpResponseMessage PostUpdateQuotationtoOrderProductSummary(directeditQuotationList values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostUpdateQuotationtoOrderProductSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetQuotationtoSOProductSummary")]
        [HttpGet]

        public HttpResponseMessage GetQuotationtoSOProductSummary(string quotation_gid)
        {
            MdlSmrQuoteToOrder values = new MdlSmrQuoteToOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetQuotationtoSOProductSummary(getsessionvalues.employee_gid, quotation_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}