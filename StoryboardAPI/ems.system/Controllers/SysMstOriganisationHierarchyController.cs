using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/SysMstOriganisationHierarchy")]
    public class SysMstOriganisationHierarchyController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysMstOriganisationHierarchy objdaOriganisationHierarchy = new DaSysMstOriganisationHierarchy();

        [ActionName("GetOriganisationHierarchysummary")]
        [HttpGet]
        public HttpResponseMessage GetOriganisationHierarchysummary(string module_gid)
        {
            MdlSysMstOriganisationHierarchy values = new MdlSysMstOriganisationHierarchy();
            objdaOriganisationHierarchy.DaOriganisationHierarchysummary(module_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ClearHierarchy")]
        [HttpPost]
        public HttpResponseMessage ClearHierarchy(MdlSysMstOriganisationHierarchy values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaOriganisationHierarchy.DaClearHierarchy(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}