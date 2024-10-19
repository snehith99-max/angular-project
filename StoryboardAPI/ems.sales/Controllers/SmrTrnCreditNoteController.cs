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
    [RoutePrefix("api/SmrTrnCreditNote")]
    [Authorize]
    public class SmrTrnCreditNoteController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnCreditNote objsales = new DaSmrTrnCreditNote();


        // Credit Note  Summary
        [ActionName("GetCreditNoteSummary")]
        [HttpGet]
        public HttpResponseMessage GetCreditNoteSummary()
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetCreditNoteSummary(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // CN Add Select 
        [ActionName("GetCreditNoteAddSelectSummary")]
        [HttpGet]
        public HttpResponseMessage GetCreditNoteAddSelectSummary()
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetCreditNoteAddSelectSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //  Add Select Confirm
        [ActionName("GetAddSelectSummary")]
        [HttpGet]
        public HttpResponseMessage GetAddSelectSummary(string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetAddSelectSummary(invoice_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //  Add Select Product Summary
        [ActionName("GetAddSelectProdSummary")]
        [HttpGet]
        public HttpResponseMessage GetAddSelectProdSummary(string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetAddSelectProdSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // post credit note
        [ActionName("PostCreditNote")]
        [HttpPost]
        public HttpResponseMessage PostCreditNote(postcreditnote_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostCreditNote(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //  View Add Select Confirm
        [ActionName("GetViewAddSelectSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewAddSelectSummary(string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetViewAddSelectSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //  View Select Product Summary
        [ActionName("GetViewAddSelectProdSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewAddSelectProdSummary(string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetViewAddSelectProdSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // Delete Credit Note
        [ActionName("GetDeleteCreditNote")]
        [HttpGet]
        public HttpResponseMessage GetDeleteCreditNote(string creditnote_gid,string receipt_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetDeleteCreditNote(creditnote_gid, receipt_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //  Stock Return Summary
        [ActionName("GetStockReturnSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockReturnSummary(string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetStockReturnSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //  Stock Return Product Summary
        [ActionName("GetStockReturnProdSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockReturnProdSummary(string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            objsales.DaGetStockReturnProdSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // post stock return
        [ActionName("PostStockReturn")]
        [HttpPost]
        public HttpResponseMessage PostStockReturn(postcreditnote_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostStockReturn(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //pdf
        [ActionName("CreditPDF")]
        [HttpGet]
        public HttpResponseMessage DebitPDF(string creditnote_gid, string invoice_gid)
        {
            MdlSmrTrnCreditNote values = new MdlSmrTrnCreditNote();
            var ls_response = new Dictionary<string, object>();
            ls_response = objsales.DaCreditPDF(creditnote_gid, invoice_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, ls_response);
        }
        
    }
}