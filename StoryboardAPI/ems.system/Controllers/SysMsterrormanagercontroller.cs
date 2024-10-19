using System;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/SysMsterrormanager")]
    public class SysMsterrormanagercontroller : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysMsterrormanager objDaerror = new DaSysMsterrormanager();

        [ActionName("GeterrorSummary")]
        [HttpGet]
        public HttpResponseMessage GeterrorSummary()
        {
            MdlSysMsterrormanager values = new MdlSysMsterrormanager();
            objDaerror.DaGeterrorSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Adderrorsubmit")]
        [HttpPost]
        public HttpResponseMessage Adderrorsubmit(errorsubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaerror.DaAdderrorsubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getUpdatederrormanage")]
        [HttpPost]
        public HttpResponseMessage getUpdatederrormanage(errorsubmit values)
         {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);

            objDaerror.DagetUpdatederrormanage(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}