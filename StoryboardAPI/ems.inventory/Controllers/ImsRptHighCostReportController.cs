using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ImsRptHighCostReport")]
    [Authorize]
    public class ImsRptHighCostReportController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptHighCostReport objDahighcost = new DaImsRptHighCostReport();


        [ActionName("GetHighcostreport")]   
        [HttpGet]
        public HttpResponseMessage GetHighcostreport()
        {
            MdlImsRptHighcost values = new MdlImsRptHighcost();
            objDahighcost.DaGetHighcostreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



    }
}