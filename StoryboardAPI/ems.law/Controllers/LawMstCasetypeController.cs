using ems.law.DataAccess;
using ems.law.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.law.Controllers
{
    [Authorize]
    [RoutePrefix("api/LawMstCasetype")]
    public class LawMstCasetypeController : ApiController
    {

        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLawMstCasetype objcasetype = new DaLawMstCasetype();

        [ActionName("Getcasetypesummary")]
        [HttpGet]
        public HttpResponseMessage Getcasetypesummary()
        {
            MdlLawMstCasetype values = new MdlLawMstCasetype();
            objcasetype.DaGetcasetypesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostcasetypeAdd")]
        [HttpPost]
        public HttpResponseMessage PostcasetypeAdd(case_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objcasetype.DaPostcasetypeAdd(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUpdatecasetype")]
        [HttpPost]
        public HttpResponseMessage PostUpdatecasetype(case_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objcasetype.DaPostUpdatecasetype(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletecasetype")]
        [HttpGet]
        public HttpResponseMessage GetDeletecasetype(case_list values, string casetype_gid)
        {
            case_list objresult = new case_list();
            objcasetype.DaGetDeletecasetype(casetype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}