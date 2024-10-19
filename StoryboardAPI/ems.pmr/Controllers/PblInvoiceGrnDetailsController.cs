using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ems.pmr.DataAccess;
using System.Net.Http;
using ems.pmr.Models;
using System.Net;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PblInvoiceGrnDetails")]
    [Authorize]
    public class PblInvoiceGrnDetailsController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPblInvoiceGrnDetails objpayableinvoice = new DaPblInvoiceGrnDetails();

        [ActionName("GetGrnAmountDetails")]
        [HttpGet]
        public HttpResponseMessage GetGrnAmountDetails(string purchaseorder_gid)
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetGrnAmountDetails(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetVendorDetails")]
        [HttpGet]
        public HttpResponseMessage GetVendorDetails(string vendor_gid)
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetVendorDetails(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseOrderDetails")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderDetails(string grn_gid)
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetPurchaseOrderDetails(grn_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetViewApprovalAddSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewPurchaseOrderSummary(string purchaseorder_gid)
        {
            MdlPblInvoiceGrnDetails objresult = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetViewApprovalAddSummary(purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetVendorUserDetails")]
        [HttpGet]
        public HttpResponseMessage GetVendorUserDetails()
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpayableinvoice.DaGetVendorUserDetails(getsessionvalues.user_gid, values);            
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseTyepDetails")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseTyepDetails()
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();            
            objpayableinvoice.DaGetPurchaseTyepDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOverAllSubmit")]
        [HttpPost]
        public HttpResponseMessage PostOverAllSubmit(OverallSubmit_list values)
        {
            
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpayableinvoice.DaPostOverAllSubmit(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postpricerequest")]
        [HttpPost]
        public HttpResponseMessage Postpricerequest(OverapprovalSubmit_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpayableinvoice.DaPostpricerequest(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoiceNetAmount")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceNetAmount(string grn_gid)
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetInvoiceNetAmount (grn_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTax4Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax4Dtl()
        {
            MdlPblInvoiceGrnDetails values = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetTax4Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSubmit")]
        [HttpPost]
        public HttpResponseMessage PostSubmit(OverallSubmit_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpayableinvoice.DaPostSubmit(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseInvoicesummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseInvoicesummary(string purchaseorder_gid, string grn_gid)
        {
            MdlPblInvoiceGrnDetails objresult = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetPurchaseInvoicesummary(purchaseorder_gid, grn_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetPurchaseInvoiceproduct")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseInvoiceproduct(string purchaseorder_gid)
        {
            MdlPblInvoiceGrnDetails objresult = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetPurchaseInvoiceproduct(purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetPurchaseServiceInvoicesummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseServiceInvoicesummary(string purchaseorder_gid)
        {
            MdlPblInvoiceGrnDetails objresult = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetPurchaseServiceInvoicesummary(purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetPurchaseServiceInvoiceproduct")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseServiceInvoiceproduct(string purchaseorder_gid)
        {
            MdlPblInvoiceGrnDetails objresult = new MdlPblInvoiceGrnDetails();
            objpayableinvoice.DaGetPurchaseServiceInvoiceproduct(purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        public HttpResponseMessage PostOverAllServiceSubmit(OverallSubmit_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpayableinvoice.DaPostOverAllServiceSubmit(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}