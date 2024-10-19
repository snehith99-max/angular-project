using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.pmr.DataAccess;
using ems.pmr.Models;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrVendor360")]
    [Authorize]
    public class PmrVendor360Controller : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrVendor360 objfinance = new DaPmrVendor360();

        [ActionName("GetTilesCount")]
        [HttpGet]
        public HttpResponseMessage GetTilesCount(string vendor_gid)
        {
            MdlPmrVendor360 values = new MdlPmrVendor360();
            objfinance.DaGetTilesCount(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // VENDOR CONTACT DETAILS

        [ActionName("GetVendorDetails")]
        [HttpGet]
        public HttpResponseMessage GetVendorDetails(string vendor_gid)
        {
            MdlPmrVendor360 values = new MdlPmrVendor360();
            objfinance.DaGetVendorDetails(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // purchase count
        [ActionName("GetPurchasetatus")]
        [HttpGet]
        public HttpResponseMessage GetPurchasetatus(string vendor_gid)
        {
            MdlPmrVendor360 values = new MdlPmrVendor360();
            objfinance.DaGetPurchasetatus(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // get payment count
        [ActionName("GetPaymentCount")]
        [HttpGet]
        public HttpResponseMessage GetPaymentCount(string vendor_gid)
        {
            MdlPmrVendor360 values = new MdlPmrVendor360();
            objfinance.DaGetPaymentCount(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
