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
using static OfficeOpenXml.ExcelErrorValue;
namespace ems.hrm.Controllers
{

    [Authorize]
    [RoutePrefix("api/WeekOff")]
    public class WeekoffController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsession_values = new logintoken();
        DaWeekoff ObjdaweekOff = new DaWeekoff();

        [ActionName("GetWeekOffSummary")]
        [HttpGet]
        public HttpResponseMessage GetWeekOffSummary()
        {

            MdlWeekoff values = new MdlWeekoff();
            ObjdaweekOff.DagetWeekOffSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchdropdownlist")]
        [HttpGet]
        public HttpResponseMessage GetBranchdropdownlist()
        {
            MdlWeekoff values = new MdlWeekoff();
            ObjdaweekOff.DaGetbranchdropdownlist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getdepartmentdropdownlist")]
        [HttpGet]
        public HttpResponseMessage Getdepartmentdropdownlist()
        {
            MdlWeekoff values = new MdlWeekoff();
            ObjdaweekOff.DaGetdepartmentdropdownlists(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetWeekOffSummarySearch")]
        [HttpGet]
        public HttpResponseMessage GetWeekOffSummarySearch(string branch_name, string department_name)
        {

            MdlWeekoff values = new MdlWeekoff();
            ObjdaweekOff.DagetWeekOffSummarySearch(branch_name,department_name,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updateweekoffemployee")]
        [HttpPost]
        public HttpResponseMessage updateweekoffemployee(weekoff_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);
            ObjdaweekOff.daupdateweekoffemployee(getsession_values.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getViewWeekoffSummary")]
        [HttpGet]
        public HttpResponseMessage getViewWeekoffSummary(string employee_gid)
        {
            MdlWeekoff values = new MdlWeekoff();
            ObjdaweekOff.DagetViewWeekoffSummary(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeename")]
        [HttpGet]
        public HttpResponseMessage GetEmployeename(string employee_gid)
        {
            MdlWeekoff objresult = new MdlWeekoff();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);
            ObjdaweekOff.DaGetEmployeename(objresult, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}