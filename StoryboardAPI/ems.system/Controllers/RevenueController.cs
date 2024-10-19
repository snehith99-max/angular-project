using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{

    [Authorize]
    [RoutePrefix("api/Revenue")]

    public class RevenueController: ApiController
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
            public HttpResponseMessage postrevenue(revenue_list values)
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
                revenue_list objresult = new revenue_list();
                objdaregion.DaDeleterevenue(revenue_gid, objresult);
                return Request.CreateResponse(HttpStatusCode.OK, objresult);
            }

        
    }
}