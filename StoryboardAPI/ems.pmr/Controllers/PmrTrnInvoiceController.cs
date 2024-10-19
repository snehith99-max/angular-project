﻿using ems.pmr.DataAccess;
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
using Microsoft.SqlServer.Server;

namespace ems.pmr.Controllers
{

    [RoutePrefix("api/PmrTrnInvoice")]
    [Authorize]
    public class PmrTrnInvoiceController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnInvoice objDaPurchase = new DaPmrTrnInvoice();
        // Module DaGetPurchaseInvoicesummary
        [ActionName("GetPmrTrnInvoiceAddSelectSummary")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnInvoiceAddSelectSummary()
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetPmrTrnInvoiceAddSelectSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPmrTrnInvoiceServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnInvoiceServiceSummary()
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetPmrTrnInvoiceServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetInvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceSummary()
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetInvoiceSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        //Summary Add
        [ActionName("GetEditInvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditInvoiceSummary(string vendor_gid)
        {
            MdlPmrTrnInvoice objresult = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetEditInvoiceSummary(vendor_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Gettaxnamedropdown")]
        [HttpGet]
        public HttpResponseMessage Gettaxnamedropdown()
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGettaxnamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Purchasetype dropdown
        [ActionName("GetPmrPurchaseDtl")]
        [HttpGet]
        public HttpResponseMessage GetPmrPurchaseDtl()
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetPmrPurchaseDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPmrTrnInvoiceview")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnInvoiceview(string invoice_gid)
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetPmrTrnInvoiceview(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetPmrTrnInvoiceviewproduct")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnInvoiceviewproduct(string invoice_gid)
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetPmrTrnInvoiceviewproduct (invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        [ActionName("pblinvoicesubmit")]
        [HttpPost]
        public HttpResponseMessage pblinvoicesubmit(pblinvoice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.Dapblinvoicesubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getinvoiceordersummary")]
        [HttpGet]
        public HttpResponseMessage Getinvoiceordersummary()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objDaPurchase.DaGetPurchaseOrderSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        [ActionName("GetInvoiceDelete")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceDelete(string invoice_gid)
        {
            taxnamedropdown values = new taxnamedropdown();
            objDaPurchase.DaGetInvoiceDelete(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //invoice report

        [ActionName("GetpmrInvoiceForLastSixMonths")]
        [HttpGet]

        public HttpResponseMessage GetInvoiceForLastSixMonths()
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetpmrInvoiceForLastSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetpmrInvoiceReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetpmrInvoiceReportForLastSixMonthsSearch(string from_date, string to_date)
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetpmrInvoiceReportForLastSixMonthsSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetpmrInvoiceDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetpmrInvoiceDetailSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetpmrInvoiceDetailSummary(getsessionvalues.branch_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIndividualreport")]
        [HttpGet]
        public HttpResponseMessage GetIndividualreport(string purchaseorder_gid)
        {
            MdlPmrTrnInvoice values = new MdlPmrTrnInvoice();
            objDaPurchase.DaGetIndividualreport(values, purchaseorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}   