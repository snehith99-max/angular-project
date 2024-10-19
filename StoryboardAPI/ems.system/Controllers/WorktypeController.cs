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
    [RoutePrefix("api/Worktype")]

    public class WorktypeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaWorktype ObjdaWorktype = new DaWorktype();


        [ActionName("GetWorktypeSummary")]
        [HttpGet]
        public HttpResponseMessage getWorktypeSummary()
        {

            MdlWorktype values = new MdlWorktype();
            ObjdaWorktype.DagetWorktypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostWorktype")]
        [HttpPost]
        public HttpResponseMessage PostWorktype(WorktypeLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjdaWorktype.DaPostWorktype(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getUpdatedWorktype")]
        [HttpPost]
        public HttpResponseMessage getUpdatedWorktype(WorktypeLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);

            ObjdaWorktype.DagetUpdatedWorktype(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteWorktype")]
        [HttpGet]
        public HttpResponseMessage DeleteWorktype(string Worktype_gid)
        {
            WorktypeLists objresult = new WorktypeLists();
            ObjdaWorktype.DaDeleteWorktype(Worktype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}