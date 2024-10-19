using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.sales.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmrRptOrderReport")]
    public class SmrRptOrderReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptOrderReport objDaSmrRptOrderReport = new DaSmrRptOrderReport();

// GetOrderForLastSixMonths

        [ActionName("GetOrderForLastSixMonths")]
        [HttpGet]

        public HttpResponseMessage GetOrderForLastSixMonths()
        {
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderForLastSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("salespersondropdown")]
        [HttpGet]

        public HttpResponseMessage salespersondropdown()
        {
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.Dasalespersondropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOrderReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetOrderReportForLastSixMonthsSearch(string from_date, string to_date , string sales_person)
        {
            //string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            //getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderForLastSixMonthsSearch(values, from_date, to_date , sales_person);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // GetOrderDetailSummary
        [ActionName("GetOrderDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetOrderDetailSummary(string month_wise,string sales_person)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderDetailSummary(getsessionvalues.employee_gid, month_wise, sales_person, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //individualreport details
        [ActionName("GetIndividualreport")]
        [HttpGet]
        public HttpResponseMessage GetIndividualreport(string salesorder_gid)
        {
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetIndividualreport(values, salesorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // GetOrderSummary

        [ActionName("GetOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetOrderSummary(string salesorder_gid )
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderSummary(getsessionvalues.employee_gid, salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // GetMonthwiseOrderReport

        [ActionName("GetMonthwiseOrderReport")]
        [HttpGet]
        public HttpResponseMessage GetMonthwiseOrderReport()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetMonthwiseOrderReport(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // get customer data 

        [ActionName("GetCustomerData")]
        [HttpGet]
        public HttpResponseMessage GetCustomerData(string month, string year, string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetCustomerData(getsessionvalues.employee_gid, month,year,from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GetOrderwiseOrderReport

        [ActionName("GetOrderwiseOrderReport")]
        [HttpGet]
        public HttpResponseMessage GetOrderwiseOrderReport()
       {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderWiseOrderReport(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOrdersCharts")]
        [HttpGet]
        public HttpResponseMessage GetOrdersCharts()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrdersCharts(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOrdersChartsummary")]
        [HttpGet]
        public HttpResponseMessage GetOrdersChartsummary()
        {
           
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrdersChartsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // product detail 

        [ActionName("GetProductdetail")]
        [HttpGet]
        public HttpResponseMessage GetProductdetail(string salesorder_gid)
        {
            MdlSmrRptOrderReport objresult = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetProductdetail(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }



        [ActionName("GetDeliverydetail")]
        [HttpGet]
        public HttpResponseMessage GetDeliverydetail(string salesorder_gid)
        {
            MdlSmrRptOrderReport objresult = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetDeliverydetail(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("GetOrderSearch")]
        [HttpGet]
        public HttpResponseMessage GetOrderSearch(string from_date, string to_date)
        {
            //string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            //getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}