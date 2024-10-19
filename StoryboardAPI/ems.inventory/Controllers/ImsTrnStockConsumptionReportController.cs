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
    [RoutePrefix("api/ImsTrnStockConsumptionReport")]
    [Authorize]
    public class ImsTrnStockConsumptionReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnStockConsumptionReport objDaInventory = new DaImsTrnStockConsumptionReport();
        [ActionName("GetImsRptStockconsumptionreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockconsumptionreport()
        {
            MdlImsTrnStockConsumptionReport values = new MdlImsTrnStockConsumptionReport();
            objDaInventory.DaGetImsRptStockconsumptionreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
    }


}