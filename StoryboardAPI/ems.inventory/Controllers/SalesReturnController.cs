using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/SalesReturn")]
    [Authorize]
    public class SalesReturnController : ApiController
    {
        session_values objGetGID = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSalesReturn objsalesreturn = new DaSalesReturn();

        [ActionName("GetSalesReturnSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesReturnSummary()
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetSalesReturnSummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        //--------------------------------------------salesreturn view-----------------------------------------//
        [ActionName("GetSalesReturnViewSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesReturnViewSummary(string salesreturn_gid) 
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetSalesReturnViewSummary(salesreturn_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetSalesReturnViewDetails")]
        [HttpGet]
        public HttpResponseMessage GetSalesReturnViewDetails(string salesreturn_gid, string directorder_gid)
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetSalesReturnViewDetails(salesreturn_gid, directorder_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetSalesReturnAddSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesReturnAddSummary()
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetSalesReturnAddSummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetSalesReturnAddselect")]
        [HttpGet]
        public HttpResponseMessage GetSalesReturnAddselect(string directorder_gid) 
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetSalesReturnAddselect(directorder_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetSalesReturnAddDetaisls")]
        [HttpGet]
        public HttpResponseMessage GetSalesReturnAddDetaisls(string directorder_gid, string salesorder_gid)
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetSalesReturnAddDetails(directorder_gid, salesorder_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("PostsalesReturn")]
        [HttpPost]
        public HttpResponseMessage PostsalesReturn(PostSalesReturn_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objGetGID.gettokenvalues(token);
            objsalesreturn.DaPostsalesReturn(getsessionvalues.user_gid,getsessionvalues.employee_gid,values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetViewSRProduct")]
        [HttpGet]
        public HttpResponseMessage GetViewSRProduct(string salesreturn_gid)
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetViewSRProduct(salesreturn_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetViewreturnProduct")]
        [HttpGet]
        public HttpResponseMessage GetViewreturnProduct(string directorder_gid)
        {
            MdlSalesReturn values = new MdlSalesReturn();
            objsalesreturn.DaGetViewreturnProduct(directorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

    }
}