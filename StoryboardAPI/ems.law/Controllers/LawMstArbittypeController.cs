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
    [RoutePrefix("api/LawMstArbittype")]
    public class LawMstArbittypeController:ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLawMstArbittype objarbittype = new DaLawMstArbittype();

        [ActionName("Getarbittypesummary")]
        [HttpGet]
        public HttpResponseMessage Getarbittypesummary()
        {
            MdlLawMstArbittype values = new MdlLawMstArbittype();
            objarbittype.DaGetarbittypesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostArbittypeAdd")]
        [HttpPost]
        public HttpResponseMessage PostArbittypeAdd(arbit_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objarbittype.DaPostArbittypeAdd(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostUpdateArbittype")]
        [HttpPost]
        public HttpResponseMessage PostUpdateArbittype(arbit_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objarbittype.DaPostUpdateArbittype(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletearbittype")]
        [HttpGet]
        public HttpResponseMessage GetDeletearbittype(string arbit_gid)
        {
            arbit_list objresult = new arbit_list();
            objarbittype.DaGetDeletearbittype(arbit_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}