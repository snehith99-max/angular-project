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
    [RoutePrefix("api/LawMstCasestage")]
    public class LawMstCasestageController : ApiController
    {

        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLawMstCasestage objcasestage = new DaLawMstCasestage();

        [ActionName("Getcasestagesummary")]
        [HttpGet]
        public HttpResponseMessage Getcasestagesummary()
        {
            MdlLawMstCasestage values = new MdlLawMstCasestage();
            objcasestage.DaGetcasestagesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostcasestageAdd")]
        [HttpPost]
        public HttpResponseMessage PostcasestageAdd(casestage_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objcasestage.DaPostcasestageAdd(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUpdatecasestage")]
        [HttpPost]
        public HttpResponseMessage PostUpdatecasestage(casestage_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objcasestage.DaPostUpdatecasestage(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletecasestage")]
        [HttpGet]
        public HttpResponseMessage GetDeletecasestage(casestage_list values, string casestage_gid)
        {
            casestage_list objresult = new casestage_list();
            objcasestage.DaGetDeletecasestage(casestage_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}