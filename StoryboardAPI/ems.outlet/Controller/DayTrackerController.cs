using ems.outlet.Dataaccess;
using ems.outlet.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("api/DayTracker")]

    public class DayTrackerController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaDayTracker objdatracker = new DaDayTracker();

        [ActionName("GetdaytrackerSummary")]
        [HttpGet]
        public HttpResponseMessage GetdaytrackerSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetdaytrackerSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Tradesummary")]
        [HttpGet]
        public HttpResponseMessage Tradesummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaTradesummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetExpenseSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseSummary()
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetExpenseSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Revenuesummary")]
        [HttpGet]
        public HttpResponseMessage Revenuesummary()
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaRevenuesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostDaytrackersubmit")]
        [HttpPost]
        public HttpResponseMessage PostDaytrackersubmit(DaytrackerData values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdatracker.DaPostDaytrackersubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Posttradedatesubmit")]
        [HttpPost]
        public HttpResponseMessage Posttradedatesubmit(DaytrackerData values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdatracker.DaPosttradedatesubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseEditSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseEditSummary(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetExpenseEditSummary(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRevenueEditsummary")]
        [HttpGet]
        public HttpResponseMessage GetRevenueEditsummary(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetRevenueEditsummary(daytracker_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOutletname")]
        [HttpGet]
        public HttpResponseMessage GetOtlTrnOutletCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetOutletname(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Posteditrequest")]
        [HttpPost]
        public HttpResponseMessage Posteditrequest(outletname_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdatracker.DaPosteditrequest(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getotpverification")]
        [HttpGet]
        public HttpResponseMessage Getotpverification(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetotpverification(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Posteditotp")]
        [HttpPost]
        public HttpResponseMessage Posteditotp(otpverification_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdatracker.DaPosteditotp(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDaytrackeredit")]
        [HttpPost]
        public HttpResponseMessage PostDaytrackeredit(DaytrackerData values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdatracker.DaPostDaytrackeredit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getviewdaytracker")]
        [HttpGet]
        public HttpResponseMessage Getviewdaytracker(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetviewdaytracker(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getedittrackersummary")]
        [HttpGet]
        public HttpResponseMessage Getedittrackersummary(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetedittrackersummaryr(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getedittrackerdtl")]
        [HttpGet]
        public HttpResponseMessage Getedittrackerdtl(string trackerhis_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetedittrackerdtl(trackerhis_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getedittrackerdtl1")]
        [HttpGet]
        public HttpResponseMessage Getedittrackerdtl1(string trackerhis_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetedittrackerdtl1(trackerhis_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseEditnewSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseEditnewSummary(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetExpenseEditnewSummary(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRevenueEditnewsummary")]
        [HttpGet]
        public HttpResponseMessage GetRevenueEditnewsummary(string daytracker_gid)
        {
            MdlDayTracker values = new MdlDayTracker();
            objdatracker.DaGetRevenueEditnewsummary(daytracker_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}   