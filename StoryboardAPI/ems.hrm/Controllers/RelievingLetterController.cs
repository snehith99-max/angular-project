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
    [RoutePrefix("api/RelievingLetter")]
    public class RelievingLetterController: ApiController
    {     
        
    session_values Objgetgid = new session_values();
    logintoken getsession_values = new logintoken();
    DaRelievingLetter ObjdaRelievingLetter = new DaRelievingLetter();


    [ActionName("GetRelievingLetterSummary")]
    [HttpGet]
    public HttpResponseMessage getRoleDesignationSummary()
    {
         MdlRelievingLetter values = new MdlRelievingLetter();
         ObjdaRelievingLetter.DagetRelievingLetterSummary(values);
         return Request.CreateResponse(HttpStatusCode.OK, values);
    }

    [ActionName("DeleteRelievingLetter")]
    [HttpGet]
        public HttpResponseMessage DeleteRelievingLetter(string params_gid)
        {
            MdlRelievingLetter objresult = new MdlRelievingLetter();
            ObjdaRelievingLetter.DaDeleteRelievingLetter(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}
