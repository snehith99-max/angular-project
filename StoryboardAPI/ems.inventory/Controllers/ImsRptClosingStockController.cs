

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
    [RoutePrefix("api/ImsRptClosingStock")]
    [Authorize]
    public class ImsRptClosingStockController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptClosingStockReport objDaInventory = new DaImsRptClosingStockReport();
        [ActionName("GetLocation")]
        [HttpGet]
        public HttpResponseMessage GetLocation()
        {
            MdlImsRptClosingStock values = new MdlImsRptClosingStock();
            objDaInventory.DaGetLocation(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetClosingStockreport")]
        [HttpGet]
        public HttpResponseMessage GetClosingStockreport()
        {
            MdlImsRptClosingStock values = new MdlImsRptClosingStock();
            objDaInventory.DaGetClosingStockreport( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
        [ActionName("GetClosingStocklocation")]
        [HttpGet]
        public HttpResponseMessage GetClosingStocklocation(string location_gid)
        {
            MdlImsRptClosingStock values = new MdlImsRptClosingStock();
            objDaInventory.DaGetClosingStocklocation(location_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
    }


}