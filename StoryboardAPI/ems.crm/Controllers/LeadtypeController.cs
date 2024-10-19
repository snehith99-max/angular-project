using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Leadtype")]
    public class LeadtypeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLeadtype objDacrm = new DaLeadtype();

        [ActionName("GetLeadtypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetLeadtypeSummary()
        {
            MdlLeadtype values = new MdlLeadtype();
            objDacrm.DaLeadtypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostLeadtype")]
        [HttpPost]
        public HttpResponseMessage PostLeadtype(leadtype_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacrm.DaPostLeadtype(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedLeadtype")]
        [HttpPost]
        public HttpResponseMessage UpdatedLeadtype(leadtype_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDacrm.DaUpdatedLeadtype(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleteLeadtypeSummary")]
        [HttpGet]
        public HttpResponseMessage deleteLeadtypeSummary(string leadtype_gid)
        {
            leadtype_lists objresult = new leadtype_lists();
            objDacrm.DadeleteLeadtypeSummary(leadtype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("OnStatusUpdateLeadtype")]
        [HttpPost]
        public HttpResponseMessage OnStatusUpdateLeadtype(mdlstatus_update values)
        {
            
            objDacrm.DaOnStatusUpdateLeadtype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}

