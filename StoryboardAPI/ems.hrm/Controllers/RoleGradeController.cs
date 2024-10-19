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
    [RoutePrefix("api/RoleGrade")]
    public class RoleGradeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsession_values = new logintoken();
        DaRoleGrade ObjdaRoleGrade = new DaRoleGrade();

        [ActionName("GetRoleGradeSummary")]
        [HttpGet]
        public HttpResponseMessage getRoleGradeSummary()
        {
            mdlrolegrade values = new mdlrolegrade();
            ObjdaRoleGrade.DagetRoleGradeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostRoleGrade")]
        [HttpPost]
        public HttpResponseMessage PostRoleGrade(RoleGradeList values)
        
        
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);
            ObjdaRoleGrade.DaPostRoleGrade(values,getsession_values.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("getUpdatedRoleGrade")]
        [HttpPost]
        public HttpResponseMessage getUpdatedRoleGrade(RoleGradeList values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);

            ObjdaRoleGrade.DagetUpdatedRoleGrade(getsession_values.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("DeleteRoleGrade")]
        [HttpGet]
        public HttpResponseMessage DeleteRoleGrade(string gradelevel_gid)
        {
            RoleGradeList objresult = new RoleGradeList();
            ObjdaRoleGrade.DaDeleteRoleGrade(gradelevel_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }




    }
}