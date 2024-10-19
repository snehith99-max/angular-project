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
    [RoutePrefix("api/Instagram")]
    [Authorize]


    public class InstagramController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaInstagram objDaInstagram = new DaInstagram();

        [ActionName("Getaccountdetails")]
        [HttpGet]
        public HttpResponseMessage Getaccountdetails()
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetaccountdetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getaccountdetailsummary")]
        [HttpGet]
        public HttpResponseMessage Getaccountdetailsummary()
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetaccountdetailsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getinstapost")]
        [HttpGet]
        public HttpResponseMessage Getinstapost(string account_id)
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetinstapost(values, account_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getinstapostsummary")]
        [HttpGet]
        public HttpResponseMessage GetGetinstapostsummary(string account_id)
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetinstapostsummary(account_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcomments")]
        [HttpGet]
        public HttpResponseMessage Getcomments(string account_id)
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetcomments(values, account_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getinstacomments")]
        [HttpGet]
        public HttpResponseMessage Getinstacomments(string post_id)
        {
            mdlcomment values = new mdlcomment();
            objDaInstagram.DaGetinstacomments(post_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getviewinsights")]
        [HttpGet]
        public HttpResponseMessage Getviewinsights(string account_id)
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetviewinsights(values, account_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getinsights")]
        [HttpGet]
        public HttpResponseMessage Getinsights(string post_id)
        {
            mdlcomment values = new mdlcomment();
            objDaInstagram.DaGetinsights(values, post_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostInstaimage")]
        [HttpPost]
        public HttpResponseMessage PostInstaimage()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaInstagram.DaPostInstaimage(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("PostInstareel")]
        [HttpPost]
        public HttpResponseMessage PostInstareel()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaInstagram.DaPostInstareel(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("carouselpost")]
        [HttpPost]
        public HttpResponseMessage carouselpost()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objresult = new result();
            objDaInstagram.Dacarouselpost(httpRequest, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostInstastory")]
        [HttpPost]
        public HttpResponseMessage PostInstastory()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaInstagram.DaPostInstastory(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
}