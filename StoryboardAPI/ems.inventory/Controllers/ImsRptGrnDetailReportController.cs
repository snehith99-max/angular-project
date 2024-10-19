

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
    [RoutePrefix("api/ImsRptGrnDetailReport")]
    [Authorize]
    public class ImsRptGrnDetailReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptGrnDetailReport objDaInventory = new DaImsRptGrnDetailReport();


        [ActionName("GetVendor")]
        [HttpGet]
        public HttpResponseMessage GetVendor()
        {
            MdlImsRptGrnDetailReport values = new MdlImsRptGrnDetailReport();
            objDaInventory.DaGetVendor(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsRptGrndetailreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptGrndetailreport()
        {
            MdlImsRptGrnDetailReport values = new MdlImsRptGrnDetailReport();
            objDaInventory.DaGetImsRptGrndetailreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImsRptGrndetailreportsearch")]
        [HttpGet]
        public HttpResponseMessage GetImsRptGrndetailreportsearch(string from_date, string to_date,string vendor_gid)
        {
            MdlImsRptGrnDetailReport values = new MdlImsRptGrnDetailReport();
            objDaInventory.DaGetImsRptGrndetailreportsearch(from_date, to_date,vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



    }


}