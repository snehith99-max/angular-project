using ems.inventory.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.inventory.Models;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ExpiryTracker")]
    [Authorize]
    public class ExpiryTrackerController : ApiController
    {
        session_values objget = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaExpiryTracker objexpiry = new DaExpiryTracker();

        [ActionName("PostSetProductdays")]
        [HttpPost]
        public HttpResponseMessage PostSetProductdays(PostExpiryDays_list values) 
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objget.gettokenvalues(token);
            objexpiry.DaPostSetProductdays(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK,values);
        }
        [ActionName("GetUpdaysSummary")]
        [HttpGet]
        public HttpResponseMessage GetUpdaysSummary() 
        {
            MdlExpiryTracker values = new MdlExpiryTracker();
            objexpiry.DaGetUpcomingdays(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetExpirySummary")]
        [HttpGet]
        public HttpResponseMessage GetExpirySummary() 
        {
            MdlExpiryTracker values = new MdlExpiryTracker();
            objexpiry.DaGetExpirySummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
    }
}