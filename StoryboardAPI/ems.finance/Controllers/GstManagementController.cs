using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.finance.DataAccess;
using ems.finance.Models;
namespace ems.finance.Controllers
{
    [Authorize]
    [RoutePrefix("api/GstManagement")]
    public class GstManagementController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaGstManagement objdacreditcard = new DaGstManagement();

        [ActionName("GstManagementSummary")]
        [HttpGet]
        public HttpResponseMessage GstManagementSummary()
        {
            MdlGstManagement values = new MdlGstManagement();
            objdacreditcard.DaGstManagementSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("InGstManagementSummary")]
        [HttpGet]
        public HttpResponseMessage InGstManagementSummary(string month,string year)
        {
            MdlGstManagement values = new MdlGstManagement();
            objdacreditcard.DaInGstManagementSummary(values, month, year);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutGstManagementSummary")]
        [HttpGet]
        public HttpResponseMessage OutGstManagementSummary(string month, string year)
        {
            MdlGstManagement values = new MdlGstManagement();
            objdacreditcard.DaOutGstManagementSummary(values, month, year);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("InFillingGstManagementSummary")]
        [HttpGet]
        public HttpResponseMessage InFillingGstManagementSummary(string month, string year)
        {
            MdlGstManagement values = new MdlGstManagement();
            objdacreditcard.DaInFillingGstManagementSummary(values, month, year);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutFillingGstManagementSummary")]
        [HttpGet]
        public HttpResponseMessage OutFillingGstManagementSummary(string month, string year)
        {
            MdlGstManagement values = new MdlGstManagement();
            objdacreditcard.DaOutFillingGstManagementSummary(values, month, year);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}