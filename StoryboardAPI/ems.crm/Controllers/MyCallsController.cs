using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.system.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using static OfficeOpenXml.ExcelErrorValue;



namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Mycalls")]
    public class MyCallsController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaMyCalls objdamycalls = new DaMyCalls();

        [ActionName("Getcallschedulesummary")]
        [HttpGet]
        public HttpResponseMessage Getcallschedulesummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetcallschedulesummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getupcomingcallschedulesummary")]
        [HttpGet]
        public HttpResponseMessage Getupcomingcallschedulesummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetupcomingcallschedulesummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcallnewsummary")]
        [HttpGet]
        public HttpResponseMessage Getcallnewsummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetcallnewsummary(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcallfollowupsummary")]
        [HttpGet]
        public HttpResponseMessage Getcallfollowupsummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetcallfollowupsummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcallprospectsummary")]
        [HttpGet]
        public HttpResponseMessage Getcallprospectsummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetcallprospectsummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcalldropsummary")]
        [HttpGet]
        public HttpResponseMessage Getcalldropsummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetcalldropsummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcallallummary")]
        [HttpGet]
        public HttpResponseMessage Getcallallummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetcallallummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postcalls")]//need to check click to call button is working /not
        [HttpPost]
        public HttpResponseMessage Postcalls(callinginput values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objdamycalls.DaPostcalls(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetPendingSummary")]
        [HttpGet]
        public HttpResponseMessage GetPendingSummary()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetPendingSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        [ActionName("GetProductdropdown")]
        [HttpGet]

        public HttpResponseMessage GetProductdropdown()
        {
            MdlMyCalls values = new MdlMyCalls();
            objdamycalls.DaGetProductdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroupdropdown")]
        [HttpGet]
        public HttpResponseMessage GetProductGroupdropdown(string product_gid)
        {
            MdlMyCalls values = new MdlMyCalls();
            objdamycalls.DaGetProductGroupdropdown(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostFollowschedulelog")]
        [HttpPost]
        public HttpResponseMessage PostFollowschedulelog(followup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostFollowschedulelog(values, getsessionvalues.user_gid);
            //return Request.CreateResponse(HttpStatusCode.OK, true);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
        [ActionName("PostNewlog")]
        [HttpPost]
        public HttpResponseMessage PostNewlog(new_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostNewlog( getsessionvalues.user_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
     
        [ActionName("PostFollowuplog")]
        [HttpPost]
        public HttpResponseMessage PostFollowuplog(followup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostFollowuplog(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetMycallstilescount")]
        [HttpGet]

        public HttpResponseMessage GetMycallstilescount()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetMycallstilescount(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMycallsresponsedropdown")]
        [HttpGet]
        public HttpResponseMessage GetMycallsresponsedropdown()
        {
            MdlMyCalls values = new MdlMyCalls();
            objdamycalls.DaGetMycallsresponsedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMycallsresponsefollowupdropdown")]
        [HttpGet]
        public HttpResponseMessage GetMycallsresponsefollowupdropdown()
        {
            MdlMyCalls values = new MdlMyCalls();
            objdamycalls.DaGetMycallsresponsefollowupdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssignedteamdropdown")]
        [HttpGet]
        public HttpResponseMessage GetAssignedteamdropdown()
        {
            MdlMyCalls values = new MdlMyCalls();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaGetAssignedteamdropdown(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postmycallleadbank")]
        [HttpPost]
        public HttpResponseMessage Postmycallleadbank(postleadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostmycallleadbank(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostAppointmentmycalls")]
        [HttpPost]
        public HttpResponseMessage PostAppointmentmycalls(postappointmentmycalls_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostAppointmentmycalls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Postaddtooportunity")]//for shopify enquiry to convert opportunity
        [HttpPost]
        public HttpResponseMessage Postaddtooportunity(postappointmentmycalls_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostaddtooportunity(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Postscheduleclose")]
        [HttpPost]
        public HttpResponseMessage Postscheduleclose(Postscheduleclose_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostscheduleclose(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Postschedulepostpone")]
        [HttpPost]
        public HttpResponseMessage Postschedulepostpone(Postschedulepostpone_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostschedulepostpone(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Postscheduledrop")]
        [HttpPost]
        public HttpResponseMessage Postscheduledrop(Postscheduledrop_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamycalls.DaPostscheduledrop(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetCallLogLead")]
        [HttpGet]
        public HttpResponseMessage GetCallLogLead(string leadbank_gid)
        {
            MdlMyCalls values = new MdlMyCalls();
            objdamycalls.DaGetCallLogLead(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}