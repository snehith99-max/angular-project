using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/CustomerTypeSummary")]
    public class CustomerTypeSummaryController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCustomerTypeSummary objdacustype = new DaCustomerTypeSummary();

        [ActionName("GetCustomerTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerTypeSummary()
        {
            MdlCustomerTypeSummary values = new MdlCustomerTypeSummary();
            objdacustype.DaGetCustomerTypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostCustomerType")]
        [HttpPost]
        public HttpResponseMessage PostCustomerType(customertypesummary_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacustype.DaPostCustomerType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeleteCustomerType")]
        [HttpGet]
        public HttpResponseMessage GetDeleteCustomerType(string customertype_gid)
        {
            customertypesummary_lists values = new customertypesummary_lists();
            objdacustype.DaGetDeleteCustomerType(customertype_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUpdateCustomerType")]
        [HttpPost]
        public HttpResponseMessage GetUpdateCustomerType(customertypesummary_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacustype.DaGetUpdateCustomerType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ActivateCustomerType")]
        [HttpGet]
        public HttpResponseMessage ActivateCustomerType(string customertype_gid)
        {
            customertypesummary_lists values = new customertypesummary_lists();
            objdacustype.DaActivateCustomerType(customertype_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("InactivateCustomerType")]
        [HttpGet]
        public HttpResponseMessage InactivateCustomerType(string customertype_gid)
        {
            customertypesummary_lists values = new customertypesummary_lists();
            objdacustype.DaInactivateCustomerType(customertype_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}