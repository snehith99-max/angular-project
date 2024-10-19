

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
    [RoutePrefix("api/ImsRptMaterialTracker")]
    [Authorize]
    public class ImsRptMaterialTrackerController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptMaterialTrackerReport objDaInventory = new DaImsRptMaterialTrackerReport();
        [ActionName("GetBranch")]
        [HttpGet]
        public HttpResponseMessage GetBranch()
        {
            MdlImsRptMaterialTracker values = new MdlImsRptMaterialTracker();
            objDaInventory.DaGetBranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsRptmaterialreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptmaterialreport(string branch_gid)
        {
            MdlImsRptMaterialTracker values = new MdlImsRptMaterialTracker();
            objDaInventory.DaGetImsRptmaterialreport(branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
        [ActionName("GetImsRptmaterialporeport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptmaterialporeport(string branch_gid)
        {
            MdlImsRptMaterialTracker values = new MdlImsRptMaterialTracker();
            objDaInventory.DaGetImsRptmaterialporeport(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
    }


}