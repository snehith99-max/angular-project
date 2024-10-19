using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{

    [RoutePrefix("api/CustomerPortalOrders")]
    [Authorize]
    public class CustomerPortalOrdersController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCustomerPortalOrders objsales = new DaCustomerPortalOrders();

        [ActionName("GetCustomerPortalSalesordersummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerPortalSalesordersummary()
        {
            MdlCustomerPortalOrders values = new MdlCustomerPortalOrders();
            objsales.DaGetCustomerPortalSalesordersummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPortalCustomerViewsalesorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetPortalCustomerViewsalesorderSummary(string salesorder_gid)
        {
            MdlCustomerPortalOrders objresult = new MdlCustomerPortalOrders();
            objsales.DaGetPortalCustomerViewsalesorderSummary(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetPortalCustomerViewsalesorderDetails")]
        [HttpGet]
        public HttpResponseMessage GetPortalCustomerViewsalesorderDetails(string salesorder_gid)
        {
            MdlCustomerPortalOrders objresult = new MdlCustomerPortalOrders();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetPortalCustomerViewsalesorderDetails(salesorder_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetTempProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetTempProductSummary(string salesorder_gid)
        {
            MdlCustomerPortalOrders objresult = new MdlCustomerPortalOrders();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetTempProductSummary(salesorder_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostCustomerOrderTOSalesOrder")]
        [HttpGet]
        public HttpResponseMessage PostCustomerOrderTOSalesOrder(string salesorder_gid, string customer_gid)
        {
            MdlCustomerPortalOrders values = new MdlCustomerPortalOrders();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.PostCustomerOrderTOSalesOrder(getsessionvalues.employee_gid, getsessionvalues.user_gid,salesorder_gid, customer_gid ,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}