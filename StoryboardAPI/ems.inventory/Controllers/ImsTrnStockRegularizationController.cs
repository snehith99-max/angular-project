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
    [RoutePrefix("api/ImsTrnStockRegularization")]
    [Authorize]
    public class ImsTrnStockRegularizationController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnStockRegularization objDaInventory = new DaImsTrnStockRegularization();
        [ActionName("GetStockRegularizationSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockRegularizationSummary()
        {
            MdlImsTrnStockRegularization values = new MdlImsTrnStockRegularization();
            objDaInventory.DaGetStockRegularizationSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostStockRegularization")]
        [HttpPost]
        public HttpResponseMessage PostStockRegularization(merge_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostStockRegularization(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}