using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnProformaInvoice")]
    [Authorize]
    public class SmrTrnProformaInvoiceController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnProformaInvoice objSales = new DaSmrTrnProformaInvoice();

        [ActionName("GetProformaInvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceSummary()
        {
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            objSales.DaGetProformaInvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ProformaInvoiceSubmit")]
        [HttpPost]
        public HttpResponseMessage ProformaInvoiceSubmit(proformainvoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objSales.DaProformaInvoiceSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Edit
        [ActionName("GetProformaInvoiceEditdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceEditdata(string invoice_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            objSales.DaGetProformaInvoiceEditdata(getsessionvalues.employee_gid,values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //EditProduct
        [ActionName("GetOrderToProformaInvoiceProductDetails")]
        [HttpGet]
        public HttpResponseMessage GetOrderToProformaInvoiceProductDetails(string invoice_gid)
        {
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objSales.DaGetOrderToProformaInvoiceProductDetails(getsessionvalues.employee_gid, invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostproductDirectinvoice")]
        [HttpPost]
        public HttpResponseMessage PostproductDirectinvoice(Proformainvoiceproductsubmit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objSales.DaPostproductDirectinvoice(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateProformainvoice")]
        [HttpPost]
        public HttpResponseMessage UpdateProformainvoice(ProformaInvoiceEditlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objSales.DaUpdateProformainvoice(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //view
        [ActionName("GetProformaInvoiceViewdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceViewdata(string invoice_gid)
        {
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            objSales.DaGetProformaInvoiceViewdata(values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProformaInvoiceProductsEditdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceProductsEditdata(string invoice_gid)
        {
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            objSales.DaGetProformaInvoiceProductsEditdata(values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //pdf
        [ActionName("GetProformaInvoicePDF")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoicePDF(string invoice_gid)
        {
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            var response = new Dictionary<string, object>();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            response = objSales.DaGetProformaInvoicePDF(invoice_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // Delete
        [ActionName("DeleteProformaInvoice")]
        [HttpGet]
        public HttpResponseMessage DeleteProformaInvoice(string invoice_gid, string invoice_reference, string invoice_amount)
        {
            MdlSmrTrnProformaInvoice values = new MdlSmrTrnProformaInvoice();
            objSales.DeleteProformaInvoice(values, invoice_gid, invoice_reference, invoice_amount);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeleteInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteInvoiceProductSummary(string invoice_gid)
        {
            ProformaInvoiceEditlist values = new ProformaInvoiceEditlist();
            objSales.DaGetDeleteInvoiceProductSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}