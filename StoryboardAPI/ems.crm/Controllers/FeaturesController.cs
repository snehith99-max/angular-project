using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Features")]
    public class FeaturesController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaFeatures objDaFeatures = new DaFeatures();


        [ActionName("GetnotesSummary")]
        [HttpGet]
        public HttpResponseMessage GetnotesSummary()
        {
            MdlFeatures values = new MdlFeatures();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaFeatures.DaGetnotesSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postnotesupdate")]
        [HttpPost]
        public HttpResponseMessage Postnotesupdate(notesupdate_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaFeatures.DaPostnotesupdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Updatednotes")]
        [HttpPost]
        public HttpResponseMessage UpdatedProductgroup(notesupdate_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaFeatures.DaUpdatednotes(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deletenotes")]
        [HttpGet]
        public HttpResponseMessage deletenotes(string s_no)
        {
            notesupdate_list objresult = new notesupdate_list();
            objDaFeatures.Dadeletenotes(s_no, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("notesdeleteevent")]
        [HttpGet]
        public HttpResponseMessage notesdeleteevent(string s_no)
        {
            notesupdate_list objresult = new notesupdate_list();
            objDaFeatures.Danotesdeleteevent(s_no, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("meetingSummary")]
        [HttpGet]
        public HttpResponseMessage meetingSummary()
        {
            MdlFeatures values = new MdlFeatures();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaFeatures.DameetingSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("addmeetingschedule")]
        [HttpPost]
        public HttpResponseMessage postmeetingschedule(meetingschedule_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaFeatures.Daaddmeetingschedule(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deletmeetingschedule")]
        [HttpGet]
        public HttpResponseMessage deletmeetingschedule(string s_no)
        {
            meetingschedule_list objresult = new meetingschedule_list();
            objDaFeatures.Dadeletmeetingschedule(s_no, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("calendlyMeeting")]
        [HttpGet]
        public HttpResponseMessage calendlyMeeting()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCalendly objMdlCalendly = new MdlCalendly();
            objDaFeatures.DaCalendlyMeeting(objMdlCalendly, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objMdlCalendly);
        }

        [ActionName("calendlyUserDetails")]
        [HttpGet]
        public HttpResponseMessage calendlyUserDetails()
        {
            MdlcalendlyAccountDetails objMdlcalendlyAccountDetails = new MdlcalendlyAccountDetails();
            objDaFeatures.DaCalendlyUserDetails(objMdlcalendlyAccountDetails);
            return Request.CreateResponse(HttpStatusCode.OK, objMdlcalendlyAccountDetails);
        }

        [ActionName("calendlyCheckIfActive")]
        [HttpGet]
        public HttpResponseMessage calendlyCheckIfActive()
        {
            result objresult = new result();
            objDaFeatures.DaCalendlyCheckIfActive(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}