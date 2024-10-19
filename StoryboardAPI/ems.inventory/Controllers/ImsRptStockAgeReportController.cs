

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
    [RoutePrefix("api/ImsRptStockAgeReport")]
    [Authorize]
    public class ImsRptStockAgeReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnStockAgeReport objDaInventory = new DaImsTrnStockAgeReport();
        [ActionName("GetImsRptStockagereport30")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockagereport30()
        {
            MdlImsRptStockAgeReport values = new MdlImsRptStockAgeReport();
            objDaInventory.DaGetImsRptStockagereport30(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }

        [ActionName("GetImsRptStockagereport60")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockagereport60()
        {
            MdlImsRptStockAgeReport values = new MdlImsRptStockAgeReport();
            objDaInventory.DaGetImsRptStockagereport60(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
        [ActionName("GetImsRptStockagereport90")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockagereport90()
        {
            MdlImsRptStockAgeReport values = new MdlImsRptStockAgeReport();
            objDaInventory.DaGetImsRptStockagereport90(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
        [ActionName("GetImsRptStockagereport120")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockagereport120()
        {
            MdlImsRptStockAgeReport values = new MdlImsRptStockAgeReport();
            objDaInventory.DaGetImsRptStockagereport120(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetImsRptStockagereport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockagereport()
        {
            MdlImsRptStockAgeReport values = new MdlImsRptStockAgeReport();
            objDaInventory.DaGetImsRptStockagereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }

    }


}