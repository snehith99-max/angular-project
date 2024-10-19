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
    [RoutePrefix("api/SmrTrnRenewalInvoiceSummary")]
    [Authorize]
    public class SmrTrnRenewalInvoiceSummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnRenewalInvoiceSummary objsales = new DaSmrTrnRenewalInvoiceSummary();

        [ActionName("GetRenewalInvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetRenewalInvoiceSummary()
        {
            MdlSmrTrnRenewalInvoiceSummary objresult = new MdlSmrTrnRenewalInvoiceSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetRenewalInvoiceSummary(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
            

        }
        [ActionName("GetRenewalInvoiceSummaryless30")]
        [HttpGet]
        public HttpResponseMessage GetRenewalInvoiceSummaryless30()
        {
            MdlSmrTrnRenewalInvoiceSummary objresult = new MdlSmrTrnRenewalInvoiceSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetRenewalInvoiceSummaryless30(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);


        }
        [ActionName("GetRenewalInvoiceSummaryexpired")]
        [HttpGet]
        public HttpResponseMessage GetRenewalInvoiceSummaryexpired()
        {
            MdlSmrTrnRenewalInvoiceSummary objresult = new MdlSmrTrnRenewalInvoiceSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetRenewalInvoiceSummaryexpired(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);


        }
        [ActionName("GetRenewalInvoiceSummarydrop")]
        [HttpGet]
        public HttpResponseMessage GetRenewalInvoiceSummarydrop()
        {
            MdlSmrTrnRenewalInvoiceSummary objresult = new MdlSmrTrnRenewalInvoiceSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetRenewalInvoiceSummarydrop(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);


        }
        [ActionName("GetRenewalInvoiceSummaryall")]
        [HttpGet]
        public HttpResponseMessage GetRenewalInvoiceSummaryall()
        {
            MdlSmrTrnRenewalInvoiceSummary objresult = new MdlSmrTrnRenewalInvoiceSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetRenewalInvoiceSummaryall(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);


        }

        [ActionName("GetCount")]
        [HttpGet]
        public HttpResponseMessage GetCount()
        {
            MdlSmrTrnRenewalInvoiceSummary objresult = new MdlSmrTrnRenewalInvoiceSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetCount(getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);


        }

        [ActionName("GetDroprenewal")]
        [HttpGet]
        public HttpResponseMessage GetDroprenewal(string renewal_gid)
        {
            MdlSmrTrnRenewalsummary objresult = new MdlSmrTrnRenewalsummary();
            objsales.DaGetDroprenewal(renewal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

    }
}