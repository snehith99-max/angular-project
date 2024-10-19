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
    [RoutePrefix("api/SmrTrnSales360")]
    [Authorize]
    public class SmrTrnSales360Controller : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnSales360 ObjSales = new DaSmrTrnSales360();

        // CUSTOMER CONTACT DETAILS

        [ActionName("GetCustomerDetails")]
        [HttpGet]
        public HttpResponseMessage GetCustomerDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetCustomerDetails(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ENQUIRY DETAILS
        [ActionName("Get360EnquiryDetails")]
        [HttpGet]
        public HttpResponseMessage Get360EnquiryDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGet360EnquiryDetails(customer_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // QUOTATION DETAILS
        [ActionName("GetQuotationDetails")]
        [HttpGet]
        public HttpResponseMessage GetQuotationDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetQuotationDetails(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ORDER DETAILS
        [ActionName("GetSalesOrderDetails")]
        [HttpGet]
        public HttpResponseMessage GetSalesOrderDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetSalesOrderDetails(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //TILE COUNTS
       
        [ActionName("GetCountandAmount")]
        [HttpGet]
        public HttpResponseMessage GetCountandAmount(string customer_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetCountandAmount(getsessionvalues.employee_gid, customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CUSTOMER UPDATE

        [ActionName("UpdateCustomer")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomer(overall_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjSales.DaUpdateCustomer(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // INVOICE DETAILS
        [ActionName("GetInvoiceDetails")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetInvoiceDetails(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // INACTIVE CUSTOMER

        [ActionName("InactiveCustomer")]
        [HttpPost]
        public HttpResponseMessage InactiveCustomer(MdlSmrTrnSales360 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjSales.DaInactiveCustomer(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerCountDetails")]
        [HttpGet]
        public HttpResponseMessage GetCustomerCountDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetLeadCountDetails(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Notesadd")]
        [HttpPost]
        public HttpResponseMessage Notesadd(SmrNotes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            ObjSales.DaSmrNotesAdd(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SmrGetNotesSummary")]
        [HttpGet]
        public HttpResponseMessage SmrGetNotesSummary(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaSmrGetNotesSummary(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SmrNoteupdate")]
        [HttpPost]
        public HttpResponseMessage SmrNoteupdate(SmrNotes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            ObjSales.DaNoteEditupdate(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SmrNotedelete")]
        [HttpPost]
        public HttpResponseMessage SmrNotedelete(SmrNotes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            ObjSales.DaSmrNotedelete(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEnquiryDetails")]
        [HttpGet]
        public HttpResponseMessage GetEnquiryDetails(string leadbank_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetEnquiryDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SmrGetDocumentDetails")]
        [HttpGet]
        public HttpResponseMessage SmrGetDocumentDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaSmrGetDocumentDetails(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmrQuotationDetails")]
        [HttpGet]
        public HttpResponseMessage GetSmrQuotationDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetSmrQuotationDetails(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmrOrderDetails")]
        [HttpGet]
        public HttpResponseMessage GetSmrOrderDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetSmrOrderDetails(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadInvoiceDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadInvoiceDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetSmrInvoiceDetails(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmrInvoiceDetails")]
        [HttpGet]
        public HttpResponseMessage GetSmrInvoiceDetails(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetSmrInvoiceDetails(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // Sales count
        [ActionName("GetSalestatus")]
        [HttpGet]
        public HttpResponseMessage GetSalestatus(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetSalestatus(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // get payment count
        [ActionName("GetPaymentCount")]
        [HttpGet]
        public HttpResponseMessage GetPaymentCount(string customer_gid)
        {
            MdlSmrTrnSales360 values = new MdlSmrTrnSales360();
            ObjSales.DaGetPaymentCount(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}