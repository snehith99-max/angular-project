using ems.hrm.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.hrm.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using static ems.hrm.Models.MdlHrmMstConfig;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HrmMstConfiguration")]
    [Authorize]
    public class HrmMstConfigurationController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmMstConfig objDaHrmMstConfig = new DaHrmMstConfig();

        [ActionName("PostHrmconfig")]
        [HttpPost]
        public HttpResponseMessage PostHrmconfig(hrmconfiglist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmMstConfig.DaPostHrmconfig(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAttendanceConfiguration")]
        [HttpGet]
        public HttpResponseMessage GetAttendanceConfiguration()
        {
            hrmconfiglist values = new hrmconfiglist();
            values = objDaHrmMstConfig.DaGetAttendanceConfiguration();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}