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
    [RoutePrefix("api/Facebook")]
    public class FacebookController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaFacebook objDaFacebook = new DaFacebook();

        [ActionName("GetPagedetails")]
        [HttpGet]
        public HttpResponseMessage GetPagedetails()
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaGetPagedetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPagedetailssummary")]
        [HttpGet]
        public HttpResponseMessage GetPagedetailssummary()
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaGetPagedetailssummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getpagepost")]
        [HttpGet]
        public HttpResponseMessage Getpagepost(string page_id)
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaGetpagepost(values,page_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPostsummarydetails")]
        [HttpGet]
        public HttpResponseMessage GetPostsummarydetails(string facebook_page_id)
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaPostsummarydetails(facebook_page_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetViewpostcomment")]
        [HttpGet]
        public HttpResponseMessage GetViewpostcomment(string facebookmain_gid)
        {
            mdlFbPostView objresult = new mdlFbPostView();
            objDaFacebook.DaGetViewpostcomment(facebookmain_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("UploadImage")]
        [HttpPost]
        public HttpResponseMessage UploadImage()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaFacebook.DaUploadImage(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("ScheduleUploadImage")]
        [HttpPost]
        public HttpResponseMessage ScheduleUploadImage()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaFacebook.DaScheduleUploadImage(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Getschedulelogdetails")]
        [HttpGet]
        public HttpResponseMessage Getschedulelogdetails(string page_id)
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaGetschedulelogdetails(values, page_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getpagelist")]
        [HttpGet]
        public HttpResponseMessage Getpagelist()
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaGetpagelist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Multiplepagepost")]
        [HttpPost]
        public HttpResponseMessage Multiplepagepost()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaFacebook.DaMultiplepagepost(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Getinstagramaccountlist")]
        [HttpGet]
        public HttpResponseMessage Getinstagramaccountlist()
        {
            MdlFacebook values = new MdlFacebook();
            objDaFacebook.DaGetinstagramaccountlist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postinstaplatformpost")]
        [HttpPost]
        public HttpResponseMessage Postinstaplatformpost()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaFacebook.DaPostinstaplatformpost(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }

    }
