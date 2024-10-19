﻿using ems.einvoice.DataAccess;
using ems.einvoice.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Collections.Generic;

namespace ems.einvoice.Controllers
{
    [RoutePrefix("api/Einvoice")]
    [Authorize]
    public class EinvoiceController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaEinvoice objDaEinvoice = new DaEinvoice();
        // Module Summary
        [ActionName("einvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage einvoiceSummary()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaeinvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("InvoicePostProduct")]
        [HttpPost]
        public HttpResponseMessage InvoicePostProduct(invoiceproductlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaInvoicePostProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("InvoiceSubmit")]
        [HttpPost]
        public HttpResponseMessage InvoiceSubmit(invoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaInvoiceSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductNamedropDown")]
        [HttpGet]
        public HttpResponseMessage GetProductNamedropDown()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetProductNamedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerNamedropDown")]
        [HttpGet]
        public HttpResponseMessage GetCustomerNamedropDown()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetCustomerNamedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetCustomer360dropdowm")]
        //[HttpGet]
        //public HttpResponseMessage GetCustomer360dropdowm(string leadbank_gid)
        //{
        //    Mdlinvoicesummary values = new Mdlinvoicesummary();
        //    objDaEinvoice.DaGetCustomer360dropdowm(values, leadbank_gid);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetBranchName")]
        [HttpGet]
        public HttpResponseMessage GetBranchName()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetBranchName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetcurrencyCodedropdown")]
        [HttpGet]
        public HttpResponseMessage GetcurrencyCodedropdown()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetcurrencyCodedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceProductSummary()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DainvoiceProductSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("EinvoiceGeneration")]
        [HttpPost]
        public HttpResponseMessage GenerateIRN(EinvoiceAddField values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaEInvoiceGeneration(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Gettaxnamedropdown")]
        [HttpGet]
        public HttpResponseMessage Gettaxnamedropdown()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGettaxnamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeCustomer")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomer(string customer_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetOnChangeCustomer(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeProduct")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProduct(string product_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetOnChangeProduct(values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetOnChangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeBranch")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeBranch(string branch_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetOnChangeBranch(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDeleteInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteInvoiceProductSummary(string invoicedtl_gid)
        {
            invoiceproductlist values = new invoiceproductlist();
            objDaEinvoice.DaGetDeleteInvoiceProductSummary(invoicedtl_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDeleteInvoicebackProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteInvoicebackProductSummary()
        {
            invoiceproductlist values = new invoiceproductlist();
            objDaEinvoice.DaGetDeleteInvoicebackProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEinvoiceAddField")]
        [HttpGet]
        public HttpResponseMessage GetEinvoiceAddField(string invoice_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetEinvoiceData(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditInvoice")]
        [HttpGet]
        public HttpResponseMessage GetEditInvoice(string invoice_gid)
        {
            invoicelist values = new invoicelist();
            values = objDaEinvoice.DaGetEditInvoice(invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEInvoicedata")]
        [HttpGet]
        public HttpResponseMessage GetEInvoicedata(string invoice_gid)
        {
            einvoicelist values = new einvoicelist();
            values = objDaEinvoice.DaGetEInvoicedata(invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        
        [ActionName("GetEditInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditInvoiceProductSummary(string invoice_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaGetEditInvoiceProductSummary(getsessionvalues.employee_gid, values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEInvoiceProductSummary(string invoice_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaGetEInvoiceProductSummary(getsessionvalues.employee_gid, values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("UpdatedInvoice")]
        [HttpPost]
        public HttpResponseMessage UpdatedInvoice(invoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaUpdatedInvoice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [ActionName("GetIRNDetails")]
        [HttpGet]
        public HttpResponseMessage GetIRNDetails(string invoice_gid)
        {
            GetIRNDetails values = new GetIRNDetails();
            objDaEinvoice.DaGetIRNDetails(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostCancelIRN")]
        [HttpPost]
        public HttpResponseMessage PostCancelIRN(cancelIrn_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaCancelIRN(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [ActionName("PostCreditNote")]
        [HttpPost]
        public HttpResponseMessage PostCreditNote(creditnote_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.DaCreditNote(values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [ActionName("SalesinvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage SalesinvoiceSummary()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaSalesinvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSalesinvoicedata")]
        [HttpGet]
        public HttpResponseMessage GetSalesinvoicedata(string directorder_gid)
        {
            SalesInvoicelist values = new SalesInvoicelist();
            values = objDaEinvoice.DaGetSalesinvoicedata(directorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getproductsummarydata")]
        [HttpGet]
        public HttpResponseMessage Getproductsummarydata(string directorder_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetproductsummarydata(values, directorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("salesinvoicesubmit")]
        [HttpPost]
        public HttpResponseMessage salesinvoicesubmit(salesinvoice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaEinvoice.Dasalesinvoicesubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("viewinvoice")]
        [HttpGet]
        public HttpResponseMessage viewinvoice(string invoice_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.Daviewinvoice(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetProductdetails(string directorder_gid)
        {
            Mdlinvoicesummary objresult = new Mdlinvoicesummary();
            objDaEinvoice.DaGetProductdetails(directorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetTermsandConditions")]
        [HttpGet]
        public HttpResponseMessage GetTermsandConditions()
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetTermsandConditions(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeTerms")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeTerms(string template_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            objDaEinvoice.DaGetOnChangeTerms(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInvoicePDF")]
        [HttpGet]
        public HttpResponseMessage GetInvoicePDF(string invoice_gid)
        {
            Mdlinvoicesummary values = new Mdlinvoicesummary();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaEinvoice.DaGetInvoicePDF(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
    }
}
