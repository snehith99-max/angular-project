using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmTrnDailyAttendance")]
    public class HrmTrnDailyAttendanceController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnDailyAttendance objdadaily = new DaHrmTrnDailyAttendance();


       [ActionName("GetDailySummary")]
        [HttpGet]
        public HttpResponseMessage GetDailySummary(string date , string department_name , string branch_name)
        {
            MdlHrmTrnDailyAttendance values = new MdlHrmTrnDailyAttendance();
            objdadaily.DaGetDailySummary(values, date, department_name, branch_name);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}