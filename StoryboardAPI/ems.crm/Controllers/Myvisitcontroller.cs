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

namespace ems.crm.Controllers
{
    [RoutePrefix("api/Myvisit")]
    [Authorize]
    public class MyVisitController :ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaMyvisit objDaMyvisit = new DaMyvisit();

        [ActionName("GetExpiredSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpiredSummary()
        {
            MdlMyvisit values = new MdlMyvisit();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaGetExpiredSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCompletedSummary")]
        [HttpGet]
        public HttpResponseMessage GetCompletedSummary()
        {
            MdlMyvisit values = new MdlMyvisit();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaGetCompletedSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTodaySummary")]
        [HttpGet]
        public HttpResponseMessage GetTodaySummary()
        {
            MdlMyvisit values = new MdlMyvisit();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaGetTodaySummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUpcomingSummary")]
        [HttpGet]
        public HttpResponseMessage GetUpcomingSummary()
        {
            MdlMyvisit values = new MdlMyvisit();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaGetUpcomingSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postcloselog")]
        [HttpPost]
        public HttpResponseMessage Postcloselog(ExpiredVisit_list values)
         {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaPostcloselog( getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postpostonedlog")]
        [HttpPost]
        public HttpResponseMessage Postpostonedlog(Upcomingvisit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaPostpostonedlog(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postdroplog")]
        [HttpPost]
        public HttpResponseMessage Postdroplog(ExpiredVisit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaPostdroplog(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetProductdropdown")]
        [HttpGet]

        public HttpResponseMessage GetProductdropdown()
        {
            MdlMyvisit values = new MdlMyvisit();
            objDaMyvisit.DaGetProductdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroupdropdown")]
        [HttpGet]

        public HttpResponseMessage GetProductGroupdropdown()
        {
            MdlMyvisit values = new MdlMyvisit();
            objDaMyvisit.DaGetProductGroupdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postonline")]
        [HttpPost]
        public HttpResponseMessage Postonline(ExpiredVisit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaPostonline(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Postoffline")]
        [HttpPost]
        public HttpResponseMessage Postoffline(ExpiredVisit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaPostoffline(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetMyVisitCount")]
        [HttpGet]
        public HttpResponseMessage GetMyVisitCount()
        {
            MdlMyvisit values = new MdlMyvisit();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaMyvisit.DaGetMyVisitCount(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnlineScheduleLogSummary")]
        [HttpGet]
        public HttpResponseMessage GetOnlineScheduleLogSummary(string log_gid)
        {
            MdlMyvisit values = new MdlMyvisit();
            objDaMyvisit.DaGetOnlineScheduleLogSummary(log_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOfflineScheduleLogSummary")]
        [HttpGet]
        public HttpResponseMessage GetOfflineScheduleLogSummary(string log_gid)
        {
            MdlMyvisit values = new MdlMyvisit();
            objDaMyvisit.DaGetOfflineScheduleLogSummary(log_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}