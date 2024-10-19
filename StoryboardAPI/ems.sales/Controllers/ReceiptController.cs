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
using System.Web.Http.Results;

namespace ems.sales.Controllers
{

    [RoutePrefix("api/Receipt")]
    [Authorize]
    public class ReceiptController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaReceipt objDaReceipt = new DaReceipt();

        [ActionName("GetReceiptSummary")]
        [HttpGet]
        public HttpResponseMessage GetReceiptSummary()
        {
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetReceiptSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getmodeofpayment")]
        [HttpGet]
        public HttpResponseMessage Getmodeofpayment()
        {
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetmodeofpayment(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAddReceiptSummary")]
        [HttpGet]
        public HttpResponseMessage GetAddReceiptSummary()
        {
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetAddReceiptSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMakeReceiptdata")]
        [HttpGet]
        public HttpResponseMessage GetMakeReceiptdata(string customer_gid)
        {
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetMakeReceiptdata(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedMakeReceipt")]
        [HttpPost]
        public HttpResponseMessage UpdatedMakeReceipt(MdlReceipt values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaReceipt.DaUpdatedMakeReceipt(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deleteReceiptSummary")]
        [HttpGet]
        public HttpResponseMessage deleteReceiptSummary(string params_gid)
        {
            MdlReceipt objresult = new MdlReceipt();
            objDaReceipt.DadeleteReceiptSummary(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }       

        [ActionName("Getreceiptdetails")]
        [HttpGet]
        public HttpResponseMessage Getreceiptdetails(string invoice_gid)
        {
            MdlReceipt objresult = new MdlReceipt();
            objDaReceipt.DaGetreceiptdetails(invoice_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetReceiptPDF")]
        [HttpGet]
        public HttpResponseMessage GetReceiptPDF(string payment_gid, string payment_type)
        {
            MdlReceipt values = new MdlReceipt();
            var response = new Dictionary<string, object>();
            response = objDaReceipt.DaGetReceiptPDF(payment_gid, payment_type, values);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [ActionName("Getinvoicereceipt")]
        [HttpGet]
        public HttpResponseMessage Getinvoicereceipt(string customer_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetinvoicereceipt( customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Getinvoicereceiptsummary")]
        [HttpGet]
        public HttpResponseMessage Getinvoicereceiptsummary(string customer_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlReceipt values = new MdlReceipt();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaReceipt.DaGetinvoicereceiptsummary(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetBankDetail")]
        [HttpGet]
        public HttpResponseMessage GetBankDetail()
        {
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetBankDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCardDetail")]
        [HttpGet]
        public HttpResponseMessage GetCardDetail()
        {
            MdlReceipt values = new MdlReceipt();
            objDaReceipt.DaGetCardDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
     

    }
}