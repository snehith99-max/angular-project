

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
    [RoutePrefix("api/ImsRptStockStatusReport")]
    [Authorize]
    public class ImsRptStockStatusReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptStockStatusReport objDaInventory = new DaImsRptStockStatusReport();

        [ActionName("GetImsRptStockStatusreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockStatusreport()
        {
            MdlImsRptStockStatusReport values = new MdlImsRptStockStatusReport();
            objDaInventory.DaGetImsRptStockStatusreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetStockStatusdate")]
        [HttpGet]
        public HttpResponseMessage GetStockStatusdate(string from_date, string to_date)
        {
            MdlImsRptStockStatusReport values = new MdlImsRptStockStatusReport();
            objDaInventory.DaGetStockStatusdate(from_date,to_date,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



    }


}