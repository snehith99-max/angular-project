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
    [RoutePrefix("api/SysMstDesignation")]

    public class SysMstDesignationController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysMstDesignation objDasys = new DaSysMstDesignation();


        [ActionName("GetDesignationtSummary")]
        [HttpGet]
        public HttpResponseMessage GetDesignationtSummary()
        {
            MdlSysMstDesignation values = new MdlSysMstDesignation();
            objDasys.DaGetDesignationtSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDesignationAdd")]
        [HttpPost]
        public HttpResponseMessage PostDesignationAdd(Designation_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasys.DaPostDesignationAdd(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDesignationActive")]
        [HttpGet] 
        public HttpResponseMessage GetDesignationActive(string params_gid)
        {
            MdlSysMstDesignation values = new MdlSysMstDesignation();
            objDasys.DaDesignationActivate(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDesignationInactive")]
        [HttpGet]
        public HttpResponseMessage GetDesignationInactive(string params_gid)
        {
            MdlSysMstDesignation values = new MdlSysMstDesignation();
            objDasys.DaDesignationInactivate(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteDesignation")]
        [HttpGet]
        public HttpResponseMessage DeleteDesignation(string params_gid)
        {
            Designation_list objresult = new Designation_list();
            objDasys.DaDeleteDesignation(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostUpdateDesignation")]
        [HttpPost]
        public HttpResponseMessage PostUpdateDesignation(string user_gid, Designation_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasys.DaPostUpdateDesignation(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDesignationStatus")]
        [HttpPost]
        public HttpResponseMessage PostDesignationStatus(string user_gid, Designation_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasys.DaPostDesignationStatus(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("PostDesignationImport")]
        [HttpPost]
        public HttpResponseMessage PostDesignationImport()
        {
            HttpRequest httpRequest;
            Designation_list values = new Designation_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDasys.DaPostDesignationImport(httpRequest, getsessionvalues.user_gid, objResult, values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetDesignationtErrorSummary")]
        [HttpGet]
        public HttpResponseMessage GetDesignationtErrorSummary()
        {
            MdlSysMstDesignation values = new MdlSysMstDesignation();
            objDasys.DaGetDesignationtErrorSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}