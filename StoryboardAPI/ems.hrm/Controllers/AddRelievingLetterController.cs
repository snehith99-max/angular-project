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
    [RoutePrefix("api/AddRelievingLetter")]
    public class AddRelievingLetterController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsession_values = new logintoken();
        DaAddRelievingLetter ObjdaAddRelievingLetter = new DaAddRelievingLetter();

        [ActionName("GetEmployeedropdown")]
        [HttpGet]
        public HttpResponseMessage GetEmployeedropdown()
        {
            MdlAddRelievingLetter values = new MdlAddRelievingLetter();
            ObjdaAddRelievingLetter.DaGetEmployeedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeEmployee")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeEmployee(string Employeegid)
        {
            MdlAddRelievingLetter values = new MdlAddRelievingLetter();
            ObjdaAddRelievingLetter.DaGetOnChangeEmployee(values, Employeegid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("RelivingLetterTemplate")]
        [HttpGet]
        public HttpResponseMessage RelivingLetterTemplate()
        {
            MdlAddRelievingLetter values = new MdlAddRelievingLetter();
            ObjdaAddRelievingLetter.DaRelivingLetterTemplate(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostRelievingLetter")]
        [HttpPost]
        public HttpResponseMessage PostRelievingLetter(PostEmployeeLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);
            ObjdaAddRelievingLetter.DaPostRelievingLetter(values, getsession_values.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getrelievingletterpdf")]
        [HttpGet]
        public HttpResponseMessage Getrelievingletterpdf(string releiving_gid)
        {
            MdlAddRelievingLetter values = new MdlAddRelievingLetter();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = Objgetgid.gettokenvalues(token);
            var ls_response = new Dictionary<string, object>();
            ls_response = ObjdaAddRelievingLetter.DaGetrelievingletterpdf(releiving_gid, values,getsession_values.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
    }
}