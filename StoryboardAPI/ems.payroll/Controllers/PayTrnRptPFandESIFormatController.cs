using ems.payroll.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.payroll.Models;

namespace ems.payroll.Controllers
{
    [RoutePrefix("api/PayTrnRptPFandESIFormat")]
    [Authorize]

    public class PayTrnRptPFandESIFormatController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayTrnRptPFandESIFormat ObjPayroll = new DaPayTrnRptPFandESIFormat();

        [ActionName("GetPFandESISummary")]
        [HttpGet]
        public HttpResponseMessage GetPFandESISummary()
        {
            MdlPayTrnRptPFandESIFormat values = new MdlPayTrnRptPFandESIFormat();
            ObjPayroll.DaGetPFandESISummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Pfassign")]
        [HttpGet]
        public HttpResponseMessage Pfassign(string month, string sal_year)
        {
            MdlPayTrnRptPFandESIFormat values = new MdlPayTrnRptPFandESIFormat();
            ObjPayroll.DaPfassign(month, sal_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPFSummary")]
        [HttpGet]
        public HttpResponseMessage GetPFSummary(string month, string year)
        {
            MdlPayTrnRptPFandESIFormat values = new MdlPayTrnRptPFandESIFormat();
            ObjPayroll.DaGetPFSummary(month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetESISummary")]
        [HttpGet]
        public HttpResponseMessage GetESISummary(string month, string year)
        {
            MdlPayTrnRptPFandESIFormat values = new MdlPayTrnRptPFandESIFormat();
            ObjPayroll.DaGetESISummary(month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}