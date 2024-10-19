

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
    [RoutePrefix("api/ImsRptMaterialIssueReport")]
    [Authorize]
    public class ImsRptMaterialIssueReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptMaterialIssueReport objDaInventory = new DaImsRptMaterialIssueReport();

        [ActionName("GetImsRptMaterialissuereport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptMaterialissuereport()
        {
            MdlImsRptMaterialIssueReport values = new MdlImsRptMaterialIssueReport();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetImsRptMaterialissuereport(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }


}