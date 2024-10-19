using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;





namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ImsTrnProductSplit")]
    [Authorize]
    public class ImsTrnProductSplitController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnProductSplit objpurchase = new DaImsTrnProductSplit();

        [ActionName("GetProductSplit")]
        [HttpPost]
        public HttpResponseMessage GetProductSplit(MdlImsTrnProductSplit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetProductSplit(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetOnChangeUnit")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeUnit()
        {
            MdlImsTrnProductSplit values = new MdlImsTrnProductSplit();
            objpurchase.DaGetOnChangeUnit(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }
}