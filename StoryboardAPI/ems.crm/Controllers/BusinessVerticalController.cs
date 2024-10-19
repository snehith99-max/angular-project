using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/BusinessVertical")]
    public class BusinessVerticalController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaBusinessVertical objbusiness = new DaBusinessVertical();

        [ActionName("GetBusinessSummary")]
        [HttpGet]
        public HttpResponseMessage GetBusinessSummary()
        {
            MdlBusinessVertical values = new MdlBusinessVertical();
            objbusiness.DaGetBusinessSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postbusinessvertical")]
        [HttpPost]
        public HttpResponseMessage Postbusinessvertical(businessvertical_summary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbusiness.DaPostbusinessvertical(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletebusinessvertical")]
        [HttpGet]
        public HttpResponseMessage GetDeletebusinessvertical(string businessvertical_gid)
        {
            businessvertical_summary values = new businessvertical_summary();
            objbusiness.DaGetDeletebusinessvertical(businessvertical_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUpdatebusinessvertical")]
        [HttpPost]
        public HttpResponseMessage GetUpdatebusinessvertical(businessvertical_summary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbusiness.DaGetUpdatebusinessvertical(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Activatebusinessvertical")]
        [HttpGet]
        public HttpResponseMessage Activatebusinessvertical(string businessvertical_gid)
        {
            businessvertical_summary values = new businessvertical_summary();
            objbusiness.DaActivatebusinessvertical(businessvertical_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Inactivatebusinessvertical")]
        [HttpGet]
        public HttpResponseMessage Inactivatebusinessvertical(string businessvertical_gid)
        {
            businessvertical_summary values = new businessvertical_summary();
            objbusiness.DaInactivatebusinessvertical(businessvertical_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}