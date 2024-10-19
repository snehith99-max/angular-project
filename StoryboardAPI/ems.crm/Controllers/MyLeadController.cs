﻿using ems.crm.DataAccess;
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
using System.Web.Http.Results;
using static OfficeOpenXml.ExcelErrorValue;


namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/MyLead")]
    public class MyLeadController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaMyLead objDaMyleads = new DaMyLead();

        [ActionName("GetMyleadsSummary")]  //Today Task
        [HttpGet]
        public HttpResponseMessage GetMyleadsSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetMyleadsSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTodaySummary")]  // Upcoming
        [HttpGet]
        public HttpResponseMessage GetTodaySummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetTodaySummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetNewSummary")]
        [HttpGet]
        public HttpResponseMessage GetNewSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetNewSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInprogressSummary")] //prospect
        [HttpGet]

        public HttpResponseMessage GetInprogressSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetInprogressSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPotentialSummary")]
        [HttpGet]

        public HttpResponseMessage GetPotentialSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetPotentialSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCustomerSummary")]
        [HttpGet]

        public HttpResponseMessage GetCustomerSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetCustomerSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDropSummary")]
        [HttpGet]

        public HttpResponseMessage GetDropSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetDropSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAllSummary")]
        [HttpGet]
        public HttpResponseMessage GetAllSummary()
        {
            MdlMyLead values = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetAllSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetleadbankeditSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbankeditSummary(string leadbank_gid)
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetleadbankeditSummary(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Getregiondropdown")]
        [HttpGet]
        public HttpResponseMessage Getregiondropdown()
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetregiondropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Getcallresponsedropdown")]
        [HttpGet]
        public HttpResponseMessage Getcallresponsedropdown()
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetcallresponsedropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Getcountrydropdown")]
        [HttpGet]
        public HttpResponseMessage Getcountrydropdown()
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetcountrydropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getcurrencydropdown")]
        [HttpGet]
        public HttpResponseMessage Getcurrencydropdown()
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetcurrencydropdown( objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Postleadbank")]
        [HttpPost]
        public HttpResponseMessage Postleadbank(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostleadbank(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Movetodrop")]
        [HttpPost]
        public HttpResponseMessage Movetodrop(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaMovetodrop(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductdropdown")]
        [HttpGet]

        public HttpResponseMessage GetProductdropdown(string productgroup_gid)
        {
            MdlMyLead values = new MdlMyLead();
            objDaMyleads.DaGetProductdropdown(productgroup_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroupdropdown")]
        [HttpGet]
        public HttpResponseMessage GetProductGroupdropdown()
        {
            MdlMyLead values = new MdlMyLead();
            objDaMyleads.DaGetProductGroupdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetCallResponse")]
        //[HttpPost]
        //public HttpResponseMessage GetCallResponse(call_list values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDaMyleads.DaGetCallResponse(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);

        //}

        [ActionName("GetCallResponse")]
        [HttpGet]
        public HttpResponseMessage GetCallResponse(string leadbank_gid)
        {
            MdlMyLead objresult = new MdlMyLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaGetCallResponse(leadbank_gid, objresult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostNewschedulelog")]
        [HttpPost]
        public HttpResponseMessage PostFollowschedulelog(followup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostNewschedulelog(values, getsessionvalues.user_gid, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("PostTeleschedulelog")]
        [HttpPost]
        public HttpResponseMessage PostTeleschedulelog(followup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostTeleschedulelog(values, getsessionvalues.user_gid, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostNewlog")]
        [HttpPost]
        public HttpResponseMessage PostNewlog(new_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostNewlog(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostProspectlog")]
        [HttpPost]
        public HttpResponseMessage PostPendinglog(new_pending_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostProspectlog(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostPotentiallog")]
        [HttpPost]
        public HttpResponseMessage PostFollowuplog(followup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostPotentiallog(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        // My Leads Count
        [ActionName("GetMyLeadsCount")]
        [HttpGet]
        public HttpResponseMessage GetMyLeadsCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlMyLead values = new MdlMyLead();
            objDaMyleads.DaGetMyLeadsCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postcloselog")]
        [HttpPost]
        public HttpResponseMessage Postcloselog(ExpiredVisit_list1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostcloselog(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postpostonedlog")]
        [HttpPost]
        public HttpResponseMessage Postpostonedlog(Upcomingvisit_list1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostpostonedlog(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postdroplog")]
        [HttpPost]
        public HttpResponseMessage Postdroplog(ExpiredVisit_list1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostdroplog(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetSchedulelogsummary")]
        [HttpGet]
        public HttpResponseMessage GetSchedulelogsummary(string leadbank_gid)
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetSchedulelogsummary(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetOpportunitylogsummary")]
        [HttpGet]
        public HttpResponseMessage GetOpportunitylogsummary(string appointment_gid)
        {
            MdlMyLead objresult = new MdlMyLead();
            objDaMyleads.DaGetOpportunitylogsummary(appointment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetMarketingAssignedTeam")]
        [HttpGet]
        public HttpResponseMessage GetMarketingAssignedTeam()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlMyLead values = new MdlMyLead();
            objDaMyleads.DaGetMarketingAssignedTeam(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postmyleadsleadbank")]
        [HttpPost]
        public HttpResponseMessage Postmyleadsleadbank(postmyleadsleadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyleads.DaPostmyleadsleadbank(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }

}