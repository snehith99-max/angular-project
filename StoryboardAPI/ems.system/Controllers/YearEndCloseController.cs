using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Controllers;
using ems.utilities.Functions;
using ems.utilities.Models;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.system.Controllers
{

    [Authorize]
    [RoutePrefix("api/YearEndClose")]

    public class YearEndCloseController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysYearEndClose objyearend = new DaSysYearEndClose();


        [ActionName("GetYearendactivities")]
        [HttpGet]

        public HttpResponseMessage GetYearendactivities()
        {
            MdlSysYearEndClose values = new MdlSysYearEndClose();
            objyearend.DaGetYearendactivities(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostYearendactivities")]
        [HttpGet]

        public HttpResponseMessage PostYearendactivities(string finyear_gid, string start_year) 
        {
            MdlSysYearEndClose values = new MdlSysYearEndClose();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objyearend.DaPostYearendactivities(finyear_gid, start_year, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}