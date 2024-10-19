using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.pmr.Controllers
{
    [Authorize] 
    [RoutePrefix("api/PmrRptPurchaseOrder")]
    public class PmrRptPurchaseOrderController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrRptPurchaseOrder objDaPmrRptPurchaseOrder = new DaPmrRptPurchaseOrder();

        // GetOrderForLastSixMonths

        [ActionName("GetPurchaseOrderReportForLastSixMonth")]
        [HttpGet]

        public HttpResponseMessage GetPurchaseOrderReportForLastSixMonth()
        {
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetPurchaseOrderReportForLastSixMonth(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPurchaseOrderDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderDetailSummary(string month, string year, string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetPurchaseOrderDetailSummary(getsessionvalues.employee_gid, month, year, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //individualreport details
        [ActionName("GetIndividualreport")]
        [HttpGet]
        public HttpResponseMessage GetIndividualreport(string purchaseorder_gid)
        {
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetIndividualreport(values, purchaseorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GetOrderSummary

        [ActionName("GetPurchaseOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSummary(string purchaseorder_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetPurchaseOrderSummary(getsessionvalues.employee_gid, purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // GetMonthwiseOrderReport

        [ActionName("GetMonthwiseOrderReport")]
        [HttpGet]
        public HttpResponseMessage GetMonthwiseOrderReport()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetMonthwiseOrderReport(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerData")]
        [HttpGet]
        public HttpResponseMessage GetCustomerData(string month_wise)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetCustomerData(getsessionvalues.employee_gid, month_wise, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("GetOrderReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetOrderReportForLastSixMonthsSearch(string from_date, string to_date)
        {
            //string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            //getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrRptPurchaseOrder values = new MdlPmrRptPurchaseOrder();
            objDaPmrRptPurchaseOrder.DaGetOrderForLastSixMonthsSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        

    }
}