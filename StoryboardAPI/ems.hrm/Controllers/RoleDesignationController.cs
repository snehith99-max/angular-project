using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/RoleDesignation")]
    public class RoleDesignationController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsession_values = new logintoken();
        DaRoleDesignation ObjdaRoleDesignation = new DaRoleDesignation();

        [ActionName("GetRoleDesignationSummary")]
        [HttpGet]
        public HttpResponseMessage getRoleDesignationSummary()
        {

            MdlRoleDesignation values = new MdlRoleDesignation();
            ObjdaRoleDesignation.DagetRoleDesignationSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostRoleDesignation")]
        [HttpPost]
        public HttpResponseMessage PostRoleDesignation(RoleDesignationLists values) 
        { 
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);
            ObjdaRoleDesignation.DaPostRoleDesignation(values, getsession_values.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetRoleDesignationdropdown")]
        [HttpGet]
        public HttpResponseMessage GetRoleDesignationdropdown()
        {
            MdlRoleDesignation values = new MdlRoleDesignation();
            ObjdaRoleDesignation.DaGetRoleDesignationdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("getUpdatedRoleDesignation")]
        [HttpPost]
        public HttpResponseMessage getUpdatedRoleDesignation(RoleDesignationLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);

            ObjdaRoleDesignation.DagetUpdatedRoleDesignation(getsession_values.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteRoleDesignation")]
        [HttpGet]
        public HttpResponseMessage DeleteRoleDesignation(string designation_gid)
        {
             RoleDesignationLists objresult = new RoleDesignationLists();
            ObjdaRoleDesignation.DaDeleteRoleDesignation(designation_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}   
