using ems.outlet.Dataaccess;
using ems.outlet.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("api/otl_rpt_overallreport")]
    public class otl_rpt_overallreportController:ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        Daotl_rpt_overall objoutletoverall = new Daotl_rpt_overall();
        [ActionName("Getoutletreportsummary")]
        [HttpGet]
        public HttpResponseMessage Getoutletreportsummary(string edit_status)
        {
            MdlOtlRptOverall values = new MdlOtlRptOverall();
            objoutletoverall.DaGetoutletreportsummary(edit_status, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}