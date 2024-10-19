using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.finance.Models;
using ems.finance.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/Vendor360")]
    [Authorize]
    public class Vendor360Controller : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaVendor360 objfinance = new DaVendor360(); 

        [ActionName("GetTilesCount")]
        [HttpGet]
        public HttpResponseMessage GetTilesCount(string vendor_gid)
        {
            MdlVendor360 values = new MdlVendor360();
            objfinance.DaGetTilesCount(vendor_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }
        // VENDOR CONTACT DETAILS

        [ActionName("GetVendorDetails")]
        [HttpGet]
        public HttpResponseMessage GetVendorDetails(string vendor_gid)
        {
            MdlVendor360 values = new MdlVendor360();
            objfinance.DaGetVendorDetails(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // purchase count
        [ActionName("GetPurchasetatus")]
        [HttpGet]
        public HttpResponseMessage GetPurchasetatus(string vendor_gid)
        {
            MdlVendor360 values = new MdlVendor360();
            objfinance.DaGetPurchasetatus(vendor_gid , values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // get payment count
        [ActionName("GetPaymentCount")]
        [HttpGet]
        public HttpResponseMessage GetPaymentCount(string vendor_gid) 
        { 
            MdlVendor360 values = new MdlVendor360();
            objfinance.DaGetPaymentCount(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}