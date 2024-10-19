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
    [RoutePrefix("api/SmrRptAgeingreport")]
    [Authorize]
    public class SmrRptAgeingreportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptAgeingreport objsales = new DaSmrRptAgeingreport();

        [ActionName("Get30invoicereport")]
        [HttpGet]
        public HttpResponseMessage Get30invoicereport()
        {
            MdlSmrRptAgeingreport values = new MdlSmrRptAgeingreport();
            objsales.DaGet30invoicereport( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Get30to60invoicereport")]
        [HttpGet]
        public HttpResponseMessage Get30to60invoicereport()
        {
            MdlSmrRptAgeingreport values = new MdlSmrRptAgeingreport();
            objsales.DaGet30to60invoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Get60to90invoicereport")]
        [HttpGet]
        public HttpResponseMessage Get60to90invoicereport()
        {
            MdlSmrRptAgeingreport values = new MdlSmrRptAgeingreport();
            objsales.DaGet60to90invoicereport( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Get90to120invoicereport")]
        [HttpGet]
        public HttpResponseMessage Get90to120invoicereport()
        {
            MdlSmrRptAgeingreport values = new MdlSmrRptAgeingreport();
            objsales.DaGet90to120invoicereport( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Get120to180invoicereport")]
        [HttpGet]
        public HttpResponseMessage Get120to180invoicereport()
        {
            MdlSmrRptAgeingreport values = new MdlSmrRptAgeingreport();
            objsales.DaGet120to180invoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getallinvoicereport")]
        [HttpGet]
        public HttpResponseMessage Getallinvoicereport()
        {
            MdlSmrRptAgeingreport values = new MdlSmrRptAgeingreport();
            objsales.DaGetallinvoicereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

       


    }
}