﻿using ems.inventory.DataAccess;
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

    [RoutePrefix("api/ImsTrnDeliveryorderSummary")]
    [Authorize]

    public class ImsTrnDeliveryordersummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnDeliveryordersummary objDaInventory = new DaImsTrnDeliveryordersummary();

        [ActionName("GetImsTrnAddDeliveryorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnAddDeliveryorderSummary()
        {
            MdlImsTrnDeliveryordersummary values = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetImsTrnAddDeliveryorderSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetImsTrnDeliveryorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnDeliveryorderSummary()
        {
            MdlImsTrnDeliveryordersummary values = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetImsTrnDeliveryorderSummary(values);

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetRaiseDeliveryorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetRaiseDeliveryorderSummary(string salesorder_gid)
        {
            MdlImsTrnDeliveryordersummary objresult = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetRaiseDeliveryorderSummary(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("modeofdispatchdropdown")]
        [HttpGet]
        public HttpResponseMessage modeofdispatchdropdown()
        {
            MdlImsTrnDeliveryordersummary objresult = new MdlImsTrnDeliveryordersummary();
            objDaInventory.Damodeofdispatchdropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetProductdelivery")]
        [HttpGet]
        public HttpResponseMessage GetProductdelivery(string salesorder_gid)
        {
            MdlImsTrnDeliveryordersummary objresult = new MdlImsTrnDeliveryordersummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetProductdelivery(salesorder_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // for select 

        [ActionName("GetOutstandingQty")]
        [HttpGet]
        public HttpResponseMessage GetOutstandingQty(string salesorder_gid, string salesorderdtl_gid)
        {
            MdlImsTrnDeliveryordersummary objresult = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetOutstandingQty(salesorder_gid,salesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("IssueFromStock")]
        [HttpGet]
        public HttpResponseMessage IssueFromStock(string product_gid, string salesorder_gid,string salesorderdtl_gid)
        {
            MdlImsTrnDeliveryordersummary objresult = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaIssueFromStock(product_gid,salesorder_gid, salesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostSelectIssueQtySubmit")]
        [HttpPost]
        public HttpResponseMessage PostSelectIssueQtySubmit(IssuedQty_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostSelectIssueQtySubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDeliveryOrderSubmit")]
        [HttpPost]
        public HttpResponseMessage PostDeliveryOrderSubmit(MdlImsTrnDeliveryorder values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostDeliveryOrderSubmit( values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //orderview api

        [ActionName("GetImsTrnDeliveryorderSummaryView")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnDeliveryorderSummaryView(string directorder_gid)
        {
            MdlImsTrnDeliveryordersummary values = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetImsTrnDeliveryorderSummaryView(directorder_gid,values);

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //productvie api

        [ActionName("GetImsTrnDeliveryorderProductView")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnDeliveryorderProductView(string directorder_gid)
        {
            MdlImsTrnDeliveryordersummary values = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetImsTrnDeliveryorderProductView(directorder_gid, values);

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //download Report files
        [ActionName("GetDeliveryOrderRpt")]
        [HttpGet]
        public HttpResponseMessage GetDeliveryOrderRpt(string directorder_gid)
        {
            MdlImsTrnDeliveryordersummary values = new MdlImsTrnDeliveryordersummary();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaInventory.DaGetDeliveryOrderRpt(directorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("GetDOsixmonthschart")]
        [HttpGet]
        public HttpResponseMessage GetDOsixmonthschart()
        {
            MdlImsTrnDeliveryordersummary values = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetDOsixmonthschart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDetialViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetDetialViewProduct(string directorder_refno)
        {
            MdlImsTrnDeliveryordersummary objresult = new MdlImsTrnDeliveryordersummary();
            objDaInventory.DaGetDetialViewProduct(directorder_refno, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}