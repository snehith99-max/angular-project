using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
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
    [RoutePrefix("api/HolidayGradeManagement")]
    public class HolidayGradeManagementController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHolidayGradeManagement objdaHoliday = new DaHolidayGradeManagement();

        [ActionName("HolidayGradeSummary")]
        [HttpGet]
        public HttpResponseMessage HolidayGradeSummary()
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DaHolidayGradeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("AddHolidayGradesubmit")]
        [HttpPost]
        public HttpResponseMessage AddHolidayGradesubmit(addholidaygrade_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaHoliday.DaAddHolidayGradesubmit( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Addholidaysummary")]
        [HttpGet]
        public HttpResponseMessage Addholidaysummary()
        {
            Addholidayassign_list values = new Addholidayassign_list();
            objdaHoliday.DaAddholidaysummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("HolidayAssignSubmit")]
        [HttpPost]
        public HttpResponseMessage HolidayAssignSubmit(Addholidayassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaHoliday.DaHolidayAssignSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Deleteholiday")]
        [HttpGet]
        public HttpResponseMessage Deleteholiday(string params_gid)
        {
            MdlHolidaygradeManagement objresult = new MdlHolidaygradeManagement();
            objdaHoliday.DaDeleteholiday(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("HolidaygradeAssignemployee")]
        [HttpGet]
        public HttpResponseMessage HolidaygradeAssignemployee(string holidaygrade_gid)
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DaHolidaygradeAssignemployee(holidaygrade_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("HolidayAssignSubmitemploye")]
        [HttpPost]
        public HttpResponseMessage HolidayAssignSubmitemploye(Holidayemployeesumbit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaHoliday.DaHolidayAssignSubmitemploye(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("HolidaygradeUnAssignemployee")]
        [HttpGet]
        public HttpResponseMessage HolidaygradeUnAssignemployee(string holidaygrade_gid)
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DaHolidaygradeUnAssignemployee(holidaygrade_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("HolidayUnAssignSubmit")]
        [HttpPost]
        public HttpResponseMessage HolidayUnAssignSubmit( holidayunassignemployeesubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaHoliday.DaHolidayUnAssignSubmit( values,getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("HolidayEditAssign")]
        [HttpGet]
        public HttpResponseMessage HolidayEditAssign(string holidaygrade_gid)
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DaHolidayEditAssign(holidaygrade_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("HolidayEditUnassign")]
        [HttpGet]
        public HttpResponseMessage HolidayEditUnassign()
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DaHolidayEditUnassign(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Holidayassign")]
        [HttpGet]
        public HttpResponseMessage Holidayassign(string holidaygrade_gid)
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaHoliday.DaHolidayassign(values, holidaygrade_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("HolidayEditUnAssignsubmit")]
        [HttpPost]
        public HttpResponseMessage HolidayEditUnAssignsubmit(HolidayEditUnassignsubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaHoliday.HolidayEditUnAssignsubmit(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteEditholiday")]
        [HttpGet]
        public HttpResponseMessage DeleteEditholiday(string holiday_gid)
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DeleteEditholiday(holiday_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("HolidayGradeViewSummary")]
        [HttpGet]
        public HttpResponseMessage HolidayGradeViewSummary(string holidaygrade_gid)
        {
            MdlHolidaygradeManagement values = new MdlHolidaygradeManagement();
            objdaHoliday.DaHolidayGradeViewSummary(holidaygrade_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }

}