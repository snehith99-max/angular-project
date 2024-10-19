

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
    [RoutePrefix("api/ImsRptGrnReport")]
    [Authorize]
    public class ImsRptGrnReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptGrnreport objDaInventory = new DaImsRptGrnreport();

        [ActionName("GetBranch")]
        [HttpGet]
        public HttpResponseMessage GetBranch()
        {
            MdlImsRptGrnReport values = new MdlImsRptGrnReport();
            objDaInventory.DaGetBranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetVendor")]
        [HttpGet]
        public HttpResponseMessage GetVendor()
        {
            MdlImsRptGrnReport values = new MdlImsRptGrnReport();
            objDaInventory.DaGetVendor(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsRptGrnreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptGrnreport()
        {
            MdlImsRptGrnReport values = new MdlImsRptGrnReport();
            objDaInventory.DaGetImsRptGrnreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImsRptGrnreportsearch")]
        [HttpGet]
        public HttpResponseMessage GetImsRptGrnreportsearch(string from_date, string to_date,string branch_gid,string vendor_gid)
        {
            MdlImsRptGrnReport values = new MdlImsRptGrnReport();
            objDaInventory.DaGetImsRptGrnreportsearch(from_date, to_date,branch_gid,vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



    }


}