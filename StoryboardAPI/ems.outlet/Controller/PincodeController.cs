using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using ems.outlet.Dataaccess;
using ems.outlet.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("api/Pincode")]
    public class PincodeController : ApiController
    {
        
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPincode objpincode = new DaPincode();

        [ActionName("GetPincodeSummary")]
        [HttpGet]
        public HttpResponseMessage GetPincodeSummary()
        {
            MdlPincode values = new MdlPincode();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpincode.DaPincodeSummary(getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPincode")]
        [HttpPost]
        public HttpResponseMessage PostPincode(Pincode_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault(); 
            getsessionvalues = objgetgid.gettokenvalues(token);
            objpincode.DaPostpincode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeletePincode")]
        [HttpGet]
        public HttpResponseMessage DeletePincode(string pincode_id)
        {
            MdlPincode values = new MdlPincode();
            objpincode.Deletepincode(pincode_id,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("BranchDetailsOutlet")]
        [HttpGet]
        public HttpResponseMessage BranchDetailsOutlet()
        {
            MdlPincode values = new MdlPincode();
            objpincode.DaGetbranchdetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}