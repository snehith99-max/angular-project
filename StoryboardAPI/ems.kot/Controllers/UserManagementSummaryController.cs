using ems.kot.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.kot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;



namespace ems.kot.Controllers
{
    [RoutePrefix("api/UserManagementSummary")]
    [Authorize]

    public class UserManagementSummaryController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaUserManagementSummary objusersummary = new DaUserManagementSummary();

        [ActionName("Postuserdetails")]
        [HttpPost]
        public HttpResponseMessage Postuserdetails(user_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objusersummary.DaPostuserdetails(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Updateuserdetails")]
        [HttpPost]
        public HttpResponseMessage Updateuserdetails(user_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objusersummary.DaUpdateuserdetails(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetUserSummary")]
        [HttpGet]
        public HttpResponseMessage GetUserSummary()
        {
            MdlUserManagementSummary values = new MdlUserManagementSummary();
            objusersummary.DaGetUserSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetViewuserSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewuserSummary(string employee_gid)
        {
            MdlUserManagementSummary objresult = new MdlUserManagementSummary();
            objusersummary.DaGetViewuserSummary(employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getdeleteuser")]
        [HttpGet]
        public HttpResponseMessage Getdeleteuser(string employee_gid)
        {
            MdlUserManagementSummary objresult = new MdlUserManagementSummary();
            objusersummary.DaGetdeleteuser(employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


    }
}