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
    [RoutePrefix("api/CrmTeleCallerTeamSummary")]
    [Authorize]
    public class CrmTeleCallerTeamSummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaTeleCallerTeamSummary objMarketing = new DaTeleCallerTeamSummary();

        [ActionName("GetTeamSummary")]
        [HttpGet]
        public HttpResponseMessage GetTeamSummary()
        {
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetTeamSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostTelecallerteam")]
        [HttpPost]
        public HttpResponseMessage PostTelecallerteam(Telecallerteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaPostTelecallerteam(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteTeam")]
        [HttpGet]
        public HttpResponseMessage DeleteTeam(string campaign_gid)
        {
            Telecallerteamlist values = new Telecallerteamlist();
            objMarketing.DaDeleteTeam(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedTelecallerteam")]
        [HttpPost]
        public HttpResponseMessage UpdatedTelecallerteam(Telecallerteamlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaUpdatedTelecallerteam(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelecallerTeamCount")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerTeamCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetTelecallerTeamCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //assign employee
        [ActionName("GetUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedlist(string campaign_gid, string campaign_location)
        {
            MdlTeleCallerTeamSummary objresult = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetUnassignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetUnassigned")]
        [HttpGet]
        public HttpResponseMessage GetUnassigned(string campaign_gid)
        {
            MdlTeleCallerTeamSummary objresult = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetUnassigned(campaign_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedlist(string campaign_gid, string campaign_location)
        {
            MdlTeleCallerTeamSummary objresult = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetAssignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostAssignedlist")]
        [HttpPost]
        public HttpResponseMessage PostAssignedlist(campaignassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaPostAssignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("PostUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostUnassignedlist(campaignassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaPostUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("GetTelecallerTeamDetailTable")]
        [HttpGet]
        public HttpResponseMessage GetTelecallerTeamDetailTable(string campaign_gid)
        {

            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetTelecallerTeamDetailTable(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUserSummary")]
        [HttpPost]
        public HttpResponseMessage GetUserSummary(detailtelacalllerteam_list1 values)
        {
            MdlTeleCallerTeamSummary objMdlCampaignSummary1 = new MdlTeleCallerTeamSummary();
            objMdlCampaignSummary1 = objMarketing.DaGetUserSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, objMdlCampaignSummary1);
        }
        [ActionName("GetAssignSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssignSummary()
        {
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetAssignSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssignLead")]
        [HttpPost]
        public HttpResponseMessage GetAssignLead(assignteam_list2 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaGetAssignLead(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //assign manager

        [ActionName("GetManagerUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetManagerUnassignedlist(string campaign_gid, string campaign_location)
        {
            MdlTeleCallerTeamSummary objresult = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetManagerUnassignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetManagerAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetManagerAssignedlist(string campaign_gid, string campaign_location)
        {
            MdlTeleCallerTeamSummary objresult = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetManagerAssignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetManagerUnassigned")]
        [HttpGet]
        public HttpResponseMessage GetManagerUnassigned(string campaign_gid)
        {
            MdlTeleCallerTeamSummary objresult = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetManagerUnassigned(campaign_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostManagerAssignedlist")]
        [HttpPost]
        public HttpResponseMessage PostManagerAssignedlist(campaignassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaPostManagerAssignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("PostManagerUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostManagerUnassignedlist(campaignassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objMarketing.DaPostManagerUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("GetManager")]
        [HttpGet]
        public HttpResponseMessage GetManager(string campaign_gid)
        {
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetManager(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getemployee")]
        [HttpGet]
        public HttpResponseMessage Getemployee(string campaign_gid)
        {
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetemployee(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getassignlead")]
        [HttpGet]
        public HttpResponseMessage Getassignlead(string campaign_gid)
        {
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetassignlead(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSearchedLeads")]
        [HttpGet]
        public HttpResponseMessage GetSearchedLeads(string region_name, string source, string customer_type)
        {
            MdlTeleCallerTeamSummary values = new MdlTeleCallerTeamSummary();
            objMarketing.DaGetSearchedLeads(region_name, source, customer_type, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}