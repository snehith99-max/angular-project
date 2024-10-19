﻿using System;
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
    [RoutePrefix("api/CrmDashboard")]
    public class CrmDashboardController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCrmDashboard objDaCrmDashboard = new DaCrmDashboard();

        // Dashboard Count
        [ActionName("GetDashboardCount")]
        [HttpGet]
        public HttpResponseMessage GetDashboardCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetDashboardCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Dashboard Count
        [ActionName("GetDashboardQuotationAmount")]
        [HttpGet]
        public HttpResponseMessage GetDashboardQuotationAmount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetDashboardQuotationAmount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBarchartMonthlyLead")]
        [HttpGet]
        public HttpResponseMessage GetBarchartMonthlyLead()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetBarchartMonthlyLead(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        ////Social Media Lead count
        //[ActionName("Getsocialmedialeadcount")]
        //[HttpGet]
        //public HttpResponseMessage Getsocialmedialeadcount()
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    MdlCrmDashboard values = new MdlCrmDashboard();
        //    objDaCrmDashboard.DaGetsocialmedialeadcount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("Getsocialmedialeadcount")]
        [HttpGet]
        public HttpResponseMessage Getsocialmedialeadcount()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetsocialmedialeadcount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetappointmentCount")]
        [HttpGet]
        public HttpResponseMessage GetappointmentCount()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetappointmentCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //21.03.2024 changes
        [ActionName("Getcrmtilescount")]
        [HttpGet]
        public HttpResponseMessage Getcrmtilescount()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetcrmtilescount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcrmleadchart")]
        [HttpGet]
        public HttpResponseMessage Getcrmleadchart()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetcrmleadchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getsentcampaignsentchart")]
        [HttpGet]
        public HttpResponseMessage Getsentcampaignsentchart()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetsentcampaignsentchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getoverallpropectchart")]
        [HttpGet]
        public HttpResponseMessage Getoverallpropectchart()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetoverallpropectchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcrmregionchart")]
        [HttpGet]
        public HttpResponseMessage Getcrmregionchart()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetcrmregionchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcrmsourcechart")]
        [HttpGet]
        public HttpResponseMessage Getcrmsourcechart()
        {
            MdlCrmDashboard values = new MdlCrmDashboard();
            objDaCrmDashboard.DaGetcrmsourcechart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }

 }
