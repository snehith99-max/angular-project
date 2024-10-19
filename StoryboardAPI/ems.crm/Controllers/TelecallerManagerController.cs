using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.utilities.Models;
using ems.crm.Models;
using System.Net.Http;
using System.Net;

namespace ems.crm.Controllers
{

    [Authorize]
    [RoutePrefix("api/TelecallerManager")]
    public class TelecallerManagerController:ApiController
    {

        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaTelecallerManager objdaMarkeingManager = new DaTelecallerManager();


        [ActionName("GetTelecallerManagerSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("GetTelecallerTeamSummary")]
        //[HttpGet]
        //public HttpResponseMessage GetTelecallerTeamSummary()
        //{
        //    MdlTelecallerManager values = new MdlTelecallerManager();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    objdaMarkeingManager.DaGetTelecallerTeamSummary(getsessionvalues.employee_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GetTelecallerTeamViewSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerTeamViewSummary(string campaign_gid)
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerTeamViewSummary(campaign_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelecallerManagerTotalSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerTotalSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerTotalSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetNewleadchartcount")]
        [HttpGet]
        public HttpResponseMessage GetNewleadchartcount()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetNewleadchartcount(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetPendingCallsleadchartcount")]
        //[HttpGet]
        //public HttpResponseMessage GetPendingCallsleadchartcount()
        //{
        //    MdlTelecallerManager values = new MdlTelecallerManager();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    objdaMarkeingManager.DaGetPendingCallsleadchartcount(getsessionvalues.employee_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        //[ActionName("GetFollowUpleadchartcount")]
        //[HttpGet]
        //public HttpResponseMessage GetFollowUpleadchartcount()
        //{
        //    MdlTelecallerManager values = new MdlTelecallerManager();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    objdaMarkeingManager.DaGetFollowUpleadchartcount(getsessionvalues.employee_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        //[ActionName("GetProspectleadchartcount")]
        //[HttpGet]
        //public HttpResponseMessage GetProspectleadchartcount()
        //{
        //    MdlTelecallerManager values = new MdlTelecallerManager();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    objdaMarkeingManager.DaGetProspectleadchartcount(getsessionvalues.employee_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

       

        [ActionName("Getteamcount")]
        [HttpGet]
        public HttpResponseMessage Getteamcount()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetteamcount(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTeamPerformencechart")]
        [HttpGet]
        public HttpResponseMessage GetTeamPerformencechart()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTeamPerformencechart(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerNewSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerNewSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerNewSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerFollowSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerFollowSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerFollowSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerProspectSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerProspectSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerProspectSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerDropSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerDropSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerDropSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelecallerManagerPendingCallsSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerPendingCallsSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerPendingCallsSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerScheduledSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerScheduledSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerScheduledSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerLaspsedLeadSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerLaspsedLeadSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerLaspsedLeadSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTelecallerManagerLongestLeadSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerManagerLongestLeadSummary()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaGetTelecallerManagerLongestLeadSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        public HttpResponseMessage PostTeleMoveToTransfer(telecampaigntransfer_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaMarkeingManager.DaPostTeleMoveToTransfer(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelecallerCallerTeamlist")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerCallerTeamlist()
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            objdaMarkeingManager.DaGetTelecallerCallerTeamlist( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelecallerCallerEmployeelist")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerCallerEmployeelist(string team_gid)
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            objdaMarkeingManager.DaGetTelecallerCallerEmployeelist(team_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("TeleLeadMoveToBin")]
        [HttpPost]
        public HttpResponseMessage TeleLeadMoveToBin(Telecallerbin_list values)
        {
            objdaMarkeingManager.DaTeleLeadMoveToBin(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelecallerDropRemarks")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerDropRemarks(string leadbank_gid)
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            objdaMarkeingManager.DaGetTelecallerDropRemarks(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLeadNoteDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadNoteDetails(string leadbank_gid)
        {
            MdlTelecallerManager values = new MdlTelecallerManager();
            objdaMarkeingManager.DaGetLeadNoteDetails(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSchedulelogsummary")]
        [HttpGet]
        public HttpResponseMessage GetSchedulelogsummary(string leadbank_gid)
        {
            MdlMarketingmanager values = new MdlMarketingmanager();
            objdaMarkeingManager.DaGetSchedulelogsummary(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}