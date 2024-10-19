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
using ems.storage.Models;
namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTrnPurchaseagreement")]
    [Authorize]
    public class PmrTrnPurchaseagreementController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnPurchaseagreement objpurchase = new DaPmrTrnPurchaseagreement();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        [ActionName("GetPurchaseagreementOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseagreementOrderSummary()
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetPurchaseagreementOrderSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostProductAdd")]
        [HttpPost]
        public HttpResponseMessage PostProductAdd(PostProduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostProductAdd(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary(string vendor_gid, string product_gid)
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaProductSummary(getsessionvalues.user_gid, vendor_gid, values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ProductSubmit")]
        [HttpPost]
        public HttpResponseMessage ProductSubmit(GetViewagreementOrder values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaProductSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetViewagreementSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewagreementSummary(string renewal_gid)
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetViewagreementSummary(renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getpurchaseagreementdelete")]
        [HttpGet]
        public HttpResponseMessage Getpurchaseagreementdelete(string renewal_gid)
        {
            MdlPmrTrnPurchaseagreement objresult = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetpurchaseagreementdelete(renewal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetPurchaseInvoicesummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseInvoicesummary(string renewal_gid)
        {
            MdlPmrTrnPurchaseagreement objresult = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetPurchaseInvoicesummary(renewal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetPurchaseInvoiceproduct")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseInvoiceproduct(string renewal_gid)
        {
            MdlPmrTrnPurchaseagreement objresult = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetPurchaseInvoiceproduct(renewal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostOverAllSubmit")]
        [HttpPost]
        public HttpResponseMessage PostOverAllSubmit(OverallSubmit_list1 values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostOverAllSubmit(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

//Edit
        [ActionName("GetEditAgreementSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditAgreementSummary(string renewal_gid)
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetEditAgreementSummary(getsessionvalues.user_gid, renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostUpdateAgreement")]
        [HttpPost]
        public HttpResponseMessage PostUpdateAgreement(GetViewPurchaseagreement values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostUpdateAgreement(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 
        [ActionName("Getagreementtoinvoicetag")]
        [HttpGet]
        public HttpResponseMessage Getagreementtoinvoicetag(string renewal_gid)
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetagreementtoinvoicetag(values, renewal_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostMappedinvoicetag")]
        [HttpPost]
        public HttpResponseMessage PostMappedinvoicetag(mapinvoice_lists1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostMappedinvoicetag(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRenewalChart")]
        [HttpGet]
        public HttpResponseMessage GetRenewalChart()
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetRenewalChart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMonthyRenewal")]
        [HttpGet]
        public HttpResponseMessage GetMonthyRenewal()
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetMonthyRenewal(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRenewalReportForLastSixMonths")]
        [HttpGet]

        public HttpResponseMessage GetRenewalReportForLastSixMonths()
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetRenewalReportForLastSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRenewalReportForLastSixMonthsSearch")]
        [HttpGet]

        public HttpResponseMessage GetRenewalReportForLastSixMonthsSearch()
        {
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetRenewalReportForLastSixMonthsSearch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetrenewalDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetrenewalDetailSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrTrnPurchaseagreement values = new MdlPmrTrnPurchaseagreement();
            objpurchase.DaGetrenewalDetailSummary(getsessionvalues.branch_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}