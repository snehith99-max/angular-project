using ems.einvoice.DataAccess;
using ems.einvoice.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Generic;

namespace ems.einvoice.Controllers
{
    [RoutePrefix("api/ProformaInvoice")]
    [Authorize]
    public class ProformaInvoiceController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaProformaInvoice objDaInvoice = new DaProformaInvoice();

        [ActionName("GetProformaInvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceSummary()
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoiceAddSummary")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceAddSummary()
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceAddSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoicedata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoicedata(string directorder_gid)
        {
            ProformaInvoicelist values = new ProformaInvoicelist();
            values=objDaInvoice.DaGetProformaInvoicedata(directorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoiceProductsdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceProductsdata(string directorder_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceProductsdata(values,directorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoiceProductsEditdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceProductsEditdata(string invoice_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceProductsEditdata(values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditProformaInvoicedata")]
        [HttpGet]
        public HttpResponseMessage GetEditProformaInvoicedata(string directorder_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetEditProformaInvoicedata(values, directorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ProformaInvoiceSubmit")]
        [HttpPost]
        public HttpResponseMessage ProformaInvoiceSubmit(proformainvoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInvoice.DaProformaInvoiceSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoiceEditdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceEditdata(string invoice_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceEditdata(values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateProformainvoice")]
        [HttpPost]
        public HttpResponseMessage UpdateProformainvoice(ProformaInvoiceEditlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInvoice.DaUpdateProformainvoice(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoiceAdvancedata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceAdvancedata(string invoice_gid)
        {
            ProformaInvoiceAdvancelist values = new ProformaInvoiceAdvancelist();
            values = objDaInvoice.DaGetProformaInvoiceAdvancedata(invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoiceProductdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceProductdata(string invoice_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceProductdata(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoicemodeofpayment")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoicemodeofpayment()
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoicemodeofpayment(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("AdvrptProformaInvoiceSubmit")]
        [HttpPost]
        public HttpResponseMessage AdvrptProformaInvoiceSubmit()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaInvoice.DaAdvrptProformaInvoiceSubmit(httpRequest,getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetTemplatelist")]
        [HttpGet]
        public HttpResponseMessage GetTemplatelist()
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetTemplatelist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplate")]
        [HttpGet]
        public HttpResponseMessage GetTemplate(string template_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetTemplatet(template_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMailId")]
        [HttpGet]
        public HttpResponseMessage GetMailId()
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInvoice.DaMaillId(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMail")]
        [HttpPost]
        public HttpResponseMessage PostMail()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaInvoice.DaPostMail(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetProductdetails(string invoice_gid)
        {
            MdlProformaInvoice objresult = new MdlProformaInvoice();
            objDaInvoice.DaGetProductdetails(invoice_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetaddproformaProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetaddproformaProductdetails(string directorder_gid)
        {
            MdlProformaInvoice objresult = new MdlProformaInvoice();
            objDaInvoice.DaGetaddproformaProductdetails(directorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetProformaInvoiceViewdata")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoiceViewdata(string invoice_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DaGetProformaInvoiceViewdata(values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteProformaInvoice")]
        [HttpGet]
        public HttpResponseMessage DeleteProformaInvoice(string invoice_gid, string invoice_reference, string invoice_amount)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.DeleteProformaInvoice(values, invoice_gid, invoice_reference, invoice_amount);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getdepositebankname")]
        [HttpGet]
        public HttpResponseMessage Getdepositebankname()
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            objDaInvoice.Getdepositebankname(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProformaInvoicePDF")]
        [HttpGet]
        public HttpResponseMessage GetProformaInvoicePDF(string invoice_gid)
        {
            MdlProformaInvoice values = new MdlProformaInvoice();
            var response = new Dictionary<string, object>();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            response = objDaInvoice.DaGetProformaInvoicePDF(invoice_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [ActionName("PostSubmitAdvanceReceipt")]
        [HttpPost]
        public HttpResponseMessage PostSubmitAdvanceReceipt(MdlAdvrptProformaInvoicelist values)
        {            
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);           
            objDaInvoice.DaPostSubmitAdvanceReceipt(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}