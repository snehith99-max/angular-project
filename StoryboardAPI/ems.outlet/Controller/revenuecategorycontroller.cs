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
    [RoutePrefix("api/Revenue")]
    public class Revenuecontroller : ApiController
    {

        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        Darevenuecategory objdaregion = new Darevenuecategory();

        [ActionName("GetRevenueSummary")]
        [HttpGet]
        public HttpResponseMessage GetRevenueSummary()
        {
            Mdlrevenuecategory values = new Mdlrevenuecategory();
            objdaregion.Dagetrevenuesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("postrevenue")]
        [HttpPost]
        public HttpResponseMessage postrevenue(revenue_list1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaregion.Dapostrevenue(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleterevenue")]
        [HttpGet]
        public HttpResponseMessage deleterevenue(string revenue_gid)
        {
            revenue_list1 objresult = new revenue_list1();
            objdaregion.DaDeleterevenue(revenue_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


    }
}



    
