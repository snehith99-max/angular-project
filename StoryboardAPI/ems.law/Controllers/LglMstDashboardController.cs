using ems.law.DataAccess;
using ems.law.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.law.Controllers
{
    [Authorize]
    [RoutePrefix("api/LglMstDashboard")]
    public class LglMstDashboardController: ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLglDashboard objlgldashboard = new DaLglDashboard();

        [ActionName("Getlgldashboardcountsummary")]
        [HttpGet]
        public HttpResponseMessage Getlgldashboardcountsummary()
        {
            MdlLglDashboard values = new MdlLglDashboard();
            objlgldashboard.DaGetlgldashboardcountsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}