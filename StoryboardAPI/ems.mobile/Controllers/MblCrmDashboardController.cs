using ems.mobile.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.mobile.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;


namespace ems.mobile.Controllers
{
    [Authorize]
    [RoutePrefix("api/MblCrmDashboard")]

    public class MblCrmDashboardController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        MblCrmDashboard objDaCrmDashboard = new MblCrmDashboard();

        [ActionName("GetAppointmentHeader")]
        [HttpGet]
        public HttpResponseMessage GetAppointmentHeaderDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetAppointmentHeader(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetAppointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetAppointmentSummaryDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetAppointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetUpcomingAppointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetUpcomingAppointmentAppointmentSummaryDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetUpcomingAppointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetthreesixtyCardViewDetails")]
        [HttpGet]
        public HttpResponseMessage Get360CardViewDetails(string appointment_gid, string leadbank_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGet360CardViewDetails(getsessionvalues.employee_gid, values, appointment_gid, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOverallCount")]
        [HttpGet]
        public HttpResponseMessage GetOverallCountDetails(string leadbank_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetOverallCountDetails(getsessionvalues.employee_gid, values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getleaddropdown")]
        [HttpGet]

        public HttpResponseMessage GetleaddropdownDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetleaddropdownDetails(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPostMblLeadStage")]
        [HttpGet]
        public HttpResponseMessage GetPostMblLeadStageDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetPostMblLeadStageDetails(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }


}
