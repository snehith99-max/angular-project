using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.subscription.DataAccess;
using ems.subscription.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.subscription.Controllers
{
    [Authorize]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaDashboard objDaDashboard = new DaDashboard();
        [ActionName("Getportalchart")]
        [HttpGet]
        public HttpResponseMessage Getportalchart()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGetportalchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Gettotaldatabasecount")]
        [HttpGet]
        public HttpResponseMessage Gettotaldatabasecount()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGettotaldatabasecount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Gettotalservercount")]
        [HttpGet]
        public HttpResponseMessage Gettotalservercount()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGettotalservercount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getsubscriptiontilescount")]
        [HttpGet]
        public HttpResponseMessage Getsubscriptiontilescount()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGetsubscriptiontilescount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getservertilescount")]
        [HttpGet]
        public HttpResponseMessage Getservertilescount()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGetservertilescount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getmonthwisedbchart")]
        [HttpGet]
        public HttpResponseMessage Getmonthwisedbchart()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGetmonthwisedbchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getmonthwiseservercount")]
        [HttpGet]
        public HttpResponseMessage Getmonthwiseservercount()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGetmonthwiseservercount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getmonthwiseservertilescount")]
        [HttpGet]
        public HttpResponseMessage Getmonthwiseservertilescount()
        {
            MdlDashboard values = new MdlDashboard();
            objDaDashboard.DaGetmonthwiseservertilescount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}