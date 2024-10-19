using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmMstExperienceLetter")]
    public class HrmMstExperienceLetterController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmMstExperienceLetter objDaHrmMstExperienceLetter = new DaHrmMstExperienceLetter();

        [ActionName("ExperienceLetterSummary")]
        [HttpGet]
        public HttpResponseMessage ExperienceLetterSummary()
        {
            MdlHrmMstExperienceLetter values = new MdlHrmMstExperienceLetter();
            objDaHrmMstExperienceLetter.DaExperienceLetterSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUserDetail")]
        [HttpGet]
        public HttpResponseMessage GetUserDetail()
        {
            MdlHrmMstExperienceLetter values = new MdlHrmMstExperienceLetter();
            objDaHrmMstExperienceLetter.DaGetUserDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Addexperienceletter")]
        [HttpPost]
        public HttpResponseMessage Addexperienceletter(AddExperienceletter_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaHrmMstExperienceLetter.DaAddexperienceletter(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteExperience")]
        [HttpGet]
        public HttpResponseMessage DeleteExperience(string params_gid)
        {
            AddExperienceletter_list values = new AddExperienceletter_list();
            objDaHrmMstExperienceLetter.DaDeleteExperience(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeEmployee")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeEmployee(string employee_gid)
        {
            MdlAppoinmentOrder values = new MdlAppoinmentOrder();
            objDaHrmMstExperienceLetter.DaGetOnChangeEmployee(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getexperienceletterpdf")]
        [HttpGet]
        public HttpResponseMessage Getexperienceletterpdf(string experience_gid)
        {
            MdlHrmMstExperienceLetter values = new MdlHrmMstExperienceLetter();
            var ls_response = new Dictionary<string, object>();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            ls_response = objDaHrmMstExperienceLetter.DaGetexperienceletterpdf(experience_gid, values, getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
    }
}