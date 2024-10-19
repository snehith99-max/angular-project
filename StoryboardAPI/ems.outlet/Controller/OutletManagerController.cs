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

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("api/OutletManager")]
    public class OutletManagerController:ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOutletManager objdamanager = new DaOutletManager();

        [ActionName("GetdaymanagerSummary")]
        [HttpGet]
        public HttpResponseMessage GetdaymanagerSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOutletManager values = new MdlOutletManager();
            objdamanager.DaGetdaymanagerSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetmanagerApproval")]
        [HttpGet]
        public HttpResponseMessage GetmanagerApproval(string daytracker_gid)
        {
            MdlOutletManager values = new MdlOutletManager();
            objdamanager.DaGetmanagerApproval(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostManagerupdate")]
        [HttpPost]
        public HttpResponseMessage PostManagerupdate(managerApproval_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamanager.DaPostManagerupdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}