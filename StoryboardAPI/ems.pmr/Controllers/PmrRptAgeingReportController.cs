using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.pmr.DataAccess;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrRptAgeingReport")]
    [Authorize]
    public class PmrRptAgeingReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrRptAgeingReport objpurchase  = new DaPmrRptAgeingReport();

        [ActionName("Get30purchaseinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Get30purchaseinvoicereport()
        {
            MdlPmrRptAgeingReport values = new MdlPmrRptAgeingReport();
            objpurchase.DaGet30purchaseinvoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Get30to60purchaseinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Get30to60invoicereport()
        {
            MdlPmrRptAgeingReport values = new MdlPmrRptAgeingReport();
            objpurchase.DaGet30to60invoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Get60to90purchaseinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Get60to90invoicereport()
        {
            MdlPmrRptAgeingReport values = new MdlPmrRptAgeingReport();
            objpurchase.DaGet60to90invoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Get90to120purchaseinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Get90to120invoicereport()
        {
            MdlPmrRptAgeingReport values = new MdlPmrRptAgeingReport();
            objpurchase.DaGet90to120invoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Get120to180purchaseinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Get120to180invoicereport()
        {
            MdlPmrRptAgeingReport values = new MdlPmrRptAgeingReport();
            objpurchase.DaGet120to180invoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getallpurchaseinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Getallinvoicereport()
        {
            MdlPmrRptAgeingReport values = new MdlPmrRptAgeingReport();
            objpurchase.DaGetallinvoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}