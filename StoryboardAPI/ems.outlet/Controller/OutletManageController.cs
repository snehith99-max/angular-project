using ems.outlet.Dataaccess;
using ems.outlet.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.system.Models;

namespace ems.outlet.Controller
    {[Authorize]
    [RoutePrefix("api/OutletManage")]
    public class OutletManageController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOutletManage objoutletmanage = new DaOutletManage();

        [ActionName("Getoutletsummary")]
        [HttpGet]
        public HttpResponseMessage Getoutletsummary()
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetoutletsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOtlTrnOutletCount")]
        [HttpGet]
        public HttpResponseMessage GetOtlTrnOutletCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetOtlTrnOutletCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getoutletbranch")]
        [HttpGet]
        public HttpResponseMessage Getoutletbranch()
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetoutletbranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOutlet")]
        [HttpPost]
        public HttpResponseMessage PostOutlet(campaign_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objoutletmanage.DaPostOutlet(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUpdateoutlet")]
        [HttpPost]
        public HttpResponseMessage PostUpdateoutlet(campaign_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objoutletmanage.DaPostUpdateoutlet(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetotlUnassignedManagerlist")]
        [HttpGet]
        public HttpResponseMessage GetotlUnassignedManagerlist(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetotlUnassignedManagerlist(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetotlAssignedManagerlist")]
        [HttpGet]
        public HttpResponseMessage GetAssignedManagerlist(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetotlAssignedManagerlist(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetotlUnassignedEmplist")]
        [HttpGet]
        public HttpResponseMessage GetotlUnassignedEmplist(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetotlUnassignedEmplist(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetotlAssignedEmplist")]
        [HttpGet]
        public HttpResponseMessage GetotlAssignedEmplist(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetotlAssignedEmplist(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostoutAssignedManagerlist")]
        [HttpPost]
        public HttpResponseMessage PostoutAssignedManagerlist(outletassignmanager_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objoutletmanage.DaPostoutAssignedManagerlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOutUnassignedManagerlist")]
        [HttpPost]
        public HttpResponseMessage PostOutUnassignedManagerlist(outletassignmanager_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objoutletmanage.DaPostOutUnassignedManagerlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOutAssignedEmplist")]
        [HttpPost]
        public HttpResponseMessage PostOutAssignedEmplist(outletassignemployee_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objoutletmanage.DaPostOutAssignedEmplist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOutUnassignedEmplist")]
        [HttpPost]
        public HttpResponseMessage PostOutUnassignedEmplist(outletassignemployee_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objoutletmanage.DaPostOutUnassignedEmplist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOutletInactive")]
        [HttpGet]
        public HttpResponseMessage GetOutletInactive(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetOutletInactive(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOutletActive")]
        [HttpGet]
        public HttpResponseMessage GetOutletActive(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetOutletActive(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getotlmanagergrid")]
        [HttpGet]
        public HttpResponseMessage Getotlmanagergrid(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetotlmanagergrid(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getotlemloyeegrid")]
        [HttpGet]
        public HttpResponseMessage Getotlemloyeegrid(string campaign_gid)
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetotlemloyeegrid(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getoutletbranchuser")]
        [HttpGet]
        public HttpResponseMessage Getoutletbranchuser()
        {
            MdlOutletmanage values = new MdlOutletmanage();
            objoutletmanage.DaGetoutletbranchuser(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUserSummary")]
        [HttpGet]
        public HttpResponseMessage GetUserSummary()
        {
            MdlOutletmanage values = new MdlOutletmanage();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objoutletmanage.DaGetUserSummary(getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUserEditSummary")]
        [HttpGet]
        public HttpResponseMessage GetUserEditSummary(string employee_gid)
        {
            MdlOutletmanage objresult = new MdlOutletmanage();
            objoutletmanage.DaGetUserEditSummary(employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }        
    }
}