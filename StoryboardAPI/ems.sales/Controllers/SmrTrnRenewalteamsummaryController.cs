using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.sales.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnRenewalteamsummary")]
    [Authorize]
    public class SmrTrnRenewalteamsummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnRenewalteamsummary objrenewalSummary = new DaSmrTrnRenewalteamsummary();


        [ActionName("GetTeamSummary")]
        [HttpGet]
        public HttpResponseMessage GetTeamSummary()
        {
            MdlSmrTrnRenewalteamsummary values = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetTeamSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Post renewal team
        [ActionName("PostRenewalteam")]
        [HttpPost]
        public HttpResponseMessage PostRenewalteam(renewalteam_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostRenewalteam(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostUnassignedlist(renewlassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("PostManagerUnassignedlist")]
        [HttpPost]
        public HttpResponseMessage PostManagerUnassignedlist(renewalunassign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostManagerUnassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("GetUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetUnassignedlist(string campaign_gid, string campaign_location)
        {
            MdlSmrTrnRenewalteamsummary objresult = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetUnassignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedlist(string campaign_gid, string campaign_location)
        {
            MdlSmrTrnRenewalteamsummary objresult = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetAssignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetManagerUnassignedlist")]
        [HttpGet]
        public HttpResponseMessage GetManagerUnassignedlist(string campaign_gid, string campaign_location)
        {
            MdlSmrTrnRenewalteamsummary objresult = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetManagerUnassignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetManagerAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetManagerAssignedlist(string campaign_gid, string campaign_location)
        {
            MdlSmrTrnRenewalteamsummary objresult = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetManagerAssignedlist(campaign_gid, campaign_location, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Updatedrenewalteam")]
        [HttpPost]
        public HttpResponseMessage Updatedmarketingteam(renewalteam_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaUpdatedrenewalteam(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteTeam")]
        [HttpGet]
        public HttpResponseMessage DeleteTeam(string campaign_gid)
        {
            renewalteam_list values = new renewalteam_list();
            objrenewalSummary.DaDeleteTeam(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetManagers")]
        [HttpGet]
        public HttpResponseMessage GetManagers(string campaign_gid)
        {
            MdlSmrTrnRenewalteamsummary values = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetManagers(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       

        [ActionName("Getrenewalemployee")]
        [HttpGet]
        public HttpResponseMessage Getrenewalemployee(string campaign_gid)
        {
            MdlSmrTrnRenewalteamsummary values = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetrenewalemployee(campaign_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployees")]
        [HttpGet]
        public HttpResponseMessage GetEmployees(string campaign_gid)
        {
            MdlSmrTrnRenewalteamsummary values = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetEmployees(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmrRenewalassignlist")]
        [HttpGet]
        public HttpResponseMessage GetSmrRenewalassignlist()
        {
            MdlSmrTrnRenewalteamsummary values = new MdlSmrTrnRenewalteamsummary();
            objrenewalSummary.DaGetSmrRenewalassignlist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostMappedrenewalassign")]
        [HttpPost]
        public HttpResponseMessage PostMappedrenewalassign(mapproduct_lists1 values)
        
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objrenewalSummary.DaPostMappedrenewalassign(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}