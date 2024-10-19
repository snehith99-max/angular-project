using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/AttendanceDaily")]
    public class HrmTrnAttendanceDailyController : ApiController
    {
        
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        HrmTrnDaAttendanceDaily objdadaily = new HrmTrnDaAttendanceDaily();

        [ActionName("GetDailySummary")]
        [HttpGet]
        public HttpResponseMessage GetDailySummary(string date)
        {
            MdlHrmTrnAttendanceDaily values = new MdlHrmTrnAttendanceDaily();
            objdadaily.DaGetDailySummary(values, date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}