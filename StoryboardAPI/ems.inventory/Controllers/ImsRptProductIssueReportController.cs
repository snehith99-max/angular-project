

using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ImsRptProductIssueReport")]
    [Authorize]
    public class ImsRptProductIssueReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptProductIssueReport objDaInventory = new DaImsRptProductIssueReport();

        [ActionName("GetImsRptProductissuereport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptProductissuereport()
        {
            MdlImsRptProductIssueReport values = new MdlImsRptProductIssueReport();
            objDaInventory.DaGetImsRptProductissuereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

       
    }


}