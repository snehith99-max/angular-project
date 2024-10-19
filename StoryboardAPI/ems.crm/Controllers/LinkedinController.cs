using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Linkedin")]
    public class LinkedinController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLinkedin objDaLinkedin = new DaLinkedin();

        [ActionName("Getlinkedinaccountdetails")]
        [HttpGet]
        public HttpResponseMessage Getlinkedinaccountdetails()
        {

            MdlLinkedin values = objDaLinkedin.DaGetlinkedinaccountdetails();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getlinkedinsummary")]
        [HttpGet]
        public HttpResponseMessage Getlinkedinsummary()
        {
            MdlLinkedin values = new MdlLinkedin();
            objDaLinkedin.DaGetlinkedinsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getlinkedinaccountview")]
        [HttpGet]
        public HttpResponseMessage Getlinkedinaccountdetails(string account_id)
        {
            Mdlaccountview values = new Mdlaccountview();
            objDaLinkedin.DaGetlinkedinaccountview(values, account_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Linkedintextpost")]
        [HttpPost]
        public HttpResponseMessage Linkedintextpost(postvalues values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaLinkedin.DaLinkedintextpost(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Linkedinmediapost")]
        [HttpPost]
        public HttpResponseMessage Linkedinmediapost()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaLinkedin.DaLinkedinmediapost(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Linkedinpollpost")]
        [HttpPost]
        public HttpResponseMessage Linkedinpollpost(pollpost values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaLinkedin.DaLinkedinpollpost(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetLinkedinpost")]
        [HttpGet]
        public HttpResponseMessage GetLinkedinpost (string account_id)
        {
            MdlLinkedin values = new MdlLinkedin();
            objDaLinkedin.DaGetLinkedinpost(values, account_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLinkedinpostsummary")]
        [HttpGet]
        public HttpResponseMessage GetLinkedinpostsummary(string account_id)
        {
            Mdlpostsummary values = new Mdlpostsummary();
            objDaLinkedin.DaGetLinkedinpostsummary(account_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}