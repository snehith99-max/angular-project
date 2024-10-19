using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.subscription.DataAccess;
using ems.subscription.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.subscription.Controllers
{
    [Authorize]
    [RoutePrefix("api/HelpandSupport")]

    public class HelpandSupportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHelpandSupport objDaHelpandSupport = new DaHelpandSupport();
        [ActionName("PostHelpandSupport")]
        [HttpPost]
        public HttpResponseMessage PostHelpandSupport(SupportLists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHelpandSupport.DaPostHelpandSupport(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetHelpandSupportSummary")]
        [HttpGet]
        public HttpResponseMessage GetHelpandSupportSummary()
        {
            MdlHelpandSupport values = new MdlHelpandSupport();
            objDaHelpandSupport.DaGetHelpandSupportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("GetEditSupportSummary")]
        //[HttpGet]
        //public HttpResponseMessage GetEditSupportSummary()
        //{
        //    MdlHelpandSupport values = new MdlHelpandSupport();
        //    objDaHelpandSupport.DaGetEditSupportSummary(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
    }
}