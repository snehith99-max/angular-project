using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Web.Http.Results;
using Org.BouncyCastle.Asn1.Ocsp;


namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employeewisepayment")]
    public class EmployeewisepaymentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaEmployeewisepayment objDaEmployeewisepayment = new DaEmployeewisepayment();

        [ActionName("GetBranchdropdown")]
        [HttpGet]
        public HttpResponseMessage GetBranchdropdown()
        {
            MdlEmployeewisepayment values = new MdlEmployeewisepayment();
            objDaEmployeewisepayment.DaGetbranchdropdownlist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeewisepaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeewisepaymentSummary()
        {

            MdlEmployeewisepayment values = new MdlEmployeewisepayment();
            objDaEmployeewisepayment.DagetemployeewiseSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeewiseSummarySearch")]
        [HttpGet]
        public HttpResponseMessage GetEmployeewiseSummarySearch(string branch_gid, string month,string year)
        {

            MdlEmployeewisepayment values = new MdlEmployeewisepayment();
            objDaEmployeewisepayment.DagetemployeewiseSummarySearch(branch_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}