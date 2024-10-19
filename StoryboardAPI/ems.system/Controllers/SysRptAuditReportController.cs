using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/SysRptAuditReport")]
    public class SysRptAuditReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysRptAuditReport objDaSysRptAuditReport = new DaSysRptAuditReport();

        [ActionName("GetAuditReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetAuditReportSummary()
        {
            MdlSysRptAuditReport values = new MdlSysRptAuditReport();
            objDaSysRptAuditReport.DaGetAuditReportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAuditReportHistorySummary")]
        [HttpGet]
        public HttpResponseMessage GetAuditReportHistorySummary(string user_gid)
        {
            MdlSysRptAuditReport values = new MdlSysRptAuditReport();
            objDaSysRptAuditReport.DaGetAuditReportHistorySummary(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}