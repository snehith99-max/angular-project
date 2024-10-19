using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/MyAppointment")]
    public class MyAppointmentController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaMyAppointment objdamyapmnt = new DaMyAppointment();

        [ActionName("GetTotalappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetTotalappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetTotalappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 

        [ActionName("GetCompletedappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetCompletedappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetCompletedappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 
        [ActionName("GetTodayappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetTodayappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetTodayappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 
        [ActionName("GetUpcomingappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetUpcomingappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetUpcomingappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 
        [ActionName("GetExpiredappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpiredappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetExpiredappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }     
        [ActionName("GetNewappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetNewappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetNewappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }     
        [ActionName("GetprospectappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetprospectappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetprospectappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }     
        [ActionName("GetpotentialsappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetpotentialsappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetpotentialsappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }     
        [ActionName("GetclosedappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetclosedappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetclosedappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }     
        [ActionName("GetdropappointmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetdropappointmentSummary()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetdropappointmentSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMyAppointmentTilesCount")]
        [HttpGet]
        public HttpResponseMessage GetMyAppointmentTilesCount()
        {
            MdlMyAppointment values = new MdlMyAppointment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaGetMyAppointmentTilesCount(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMyAppointmentlogsummary")]
        [HttpGet]
        public HttpResponseMessage GetMyAppointmentlogsummary(string leadbank_gid)
        {
            MdlMyAppointment values = new MdlMyAppointment();
            objdamyapmnt.DaGetMyAppointmentlogsummary(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postsstatusclose")]
        [HttpPost]
        public HttpResponseMessage Postsstatusclose(Poststatusclose_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaPostsstatusclose(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Poststatuspostpone")]
        [HttpPost]
        public HttpResponseMessage Poststatuspostpone(Poststatuspostpone_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamyapmnt.DaPoststatuspostpone(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }
}