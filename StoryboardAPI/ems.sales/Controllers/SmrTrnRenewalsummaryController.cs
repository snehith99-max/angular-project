using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.sales.Models;


namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnRenewalsummary")]
    [Authorize]
    public class SmrTrnRenewalsummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnRenewalsummary objrenewalSummary = new DaSmrTrnRenewalsummary();


        [ActionName("GetRenewalSummary")]
        [HttpGet]
        public HttpResponseMessage GetRenewalSummary()
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetRenewalSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
      
        [ActionName("GetSmrTrnRenewalall")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnRenewalall()
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetSmrTrnRenewalall(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEditrenewalSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditrenewalSummary(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetEditrenewalSummary(renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetViewrenewalSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewrenewalSummary(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetViewrenewalSummary(renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetViewrenewaldetails")]
        [HttpGet]
        public HttpResponseMessage GetViewrenewaldetails(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetViewrenewaldetails(renewal_gid,  values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetViewsalesorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderSummary(string salesorder_gid)
        {
            MdlSmrTrnRenewalsummary objresult = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetViewsalesorderSummary(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetViewsalesorderdetails")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderdetails(string salesorder_gid)
        {
            MdlSmrTrnRenewalsummary objresult = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetViewsalesorderdetails(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Getassignrenewal")]
        [HttpGet]
        public HttpResponseMessage Getassignrenewal(string campaign_gid, string employee_gid,string renewal_gid)
        {
            MdlSmrTrnRenewalsummary objresult = new MdlSmrTrnRenewalsummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaGetassignrenewal(campaign_gid, employee_gid,renewal_gid, getsessionvalues.user_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getdeleterenewal")]
        [HttpGet]
        public HttpResponseMessage Getdeleterenewal(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary objresult = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetdeleterenewal(renewal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetDeleteDirectSOProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteDirectSOProductSummary(string invoicedtl_gid)
        {
            salesorders_list objresult = new salesorders_list();
            objrenewalSummary.GetDeleteDirectSOProductSummary(invoicedtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetProductsearchSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary()
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetProductsearchSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PosttempProductAdd")]
        [HttpPost]
        public HttpResponseMessage PosttempProductAdd(PosttempProduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPosttempProductAdd(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAgreementOrder")]
        [HttpPost]
        public HttpResponseMessage PostAgreementOrder(postagree_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostAgreementOrder(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetassignTeamSummary")]
        [HttpGet]
        public HttpResponseMessage GetassignTeamSummary()
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetassignTeamSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetrenewalToinvoiceDetailsSummary")]
        [HttpGet]
        public HttpResponseMessage GetrenewalToinvoiceDetailsSummary(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetrenewalToinvoiceDetailsSummary(renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetrenewalToInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetrenewalToInvoiceProductSummary(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaGetrenewalToInvoiceProductSummary(getsessionvalues.employee_gid, renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetrenewalToInvoiceProductDetails")]
        [HttpGet]
        public HttpResponseMessage GetrenewalToInvoiceProductDetails(string renewal_gid)
        {
            MdlSmrTrnSalesorder values = new MdlSmrTrnSalesorder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaGetrenewalToInvoiceProductDetails(getsessionvalues.employee_gid, renewal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostrenewalToInvoiceProductAdd")]
        [HttpPost]
        public HttpResponseMessage PostrenewalToInvoiceProductAdd(ordertoinvoiceproductsubmit_list1 values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostrenewalToInvoiceProductAdd(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostOnsubmitrenewaltoInvoice")]
        [HttpPost]
        public HttpResponseMessage PostOnsubmitrenewaltoInvoice(renewaltoinvoicesubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostOnsubmitrenewaltoInvoice(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ProductAddRenewalEdit")]
        [HttpPost]
        public HttpResponseMessage ProductAddRenewalEdit(PosttempProduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaProductAddRenewalEdit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductSummaryRenewalEdit")]
        [HttpGet]
        public HttpResponseMessage GetProductSummaryRenewalEdit(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaGetProductSummaryRenewalEdit(renewal_gid,getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GettmpProductSummaryRenewalEdit")]
        [HttpGet]
        public HttpResponseMessage GettmpProductSummaryRenewalEdit()
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaGettmpProductSummaryRenewalEdit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 
        [ActionName("PostRenewalEdit")]
        [HttpPost]
        public HttpResponseMessage PostRenewalEdit(PostRenewalEdit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostRenewalEdit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getagreementtoinvoicetag")]
        [HttpGet]
        public HttpResponseMessage Getagreementtoinvoicetag(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary values = new MdlSmrTrnRenewalsummary();
            objrenewalSummary.DaGetagreementtoinvoicetag(values, renewal_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMappedinvoicetag")]
        [HttpPost]
        public HttpResponseMessage PostMappedinvoicetag(mapinvoice_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostMappedinvoicetag(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
