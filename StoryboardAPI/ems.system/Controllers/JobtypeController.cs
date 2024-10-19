using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{

    [Authorize]
    [RoutePrefix("api/Jobtype")]

    public class JobtypeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaJobtype ObjdaJobtype = new DaJobtype();

        [ActionName("GetJobtypeSummary")]
        [HttpGet]
        public HttpResponseMessage getJobtypeSummary()
        {

            MdlJobtype values = new MdlJobtype();
            ObjdaJobtype.DagetJobtypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostJobtype")]
        [HttpPost]
        public HttpResponseMessage PostJobtype(JobtypeLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjdaJobtype.DaPostJobtype(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getUpdatedJobtype")]
        [HttpPost]
        public HttpResponseMessage getUpdatedJobtype(JobtypeLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);

            ObjdaJobtype.DagetUpdatedJobtype(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteJobtype")]
        [HttpGet]
        public HttpResponseMessage DeleteJobtype(string Jobtype_gid)
        {
            JobtypeLists objresult = new JobtypeLists();
            ObjdaJobtype.DaDeleteJobtype(Jobtype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}