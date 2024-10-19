using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayTrnReportPayment")]
    public class PayTrnReportPaymentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayTrnReportPayment objDaRptReportPayment = new DaPayTrnReportPayment();


        [ActionName("GetPaymentSummary")]   
        [HttpGet]
        public HttpResponseMessage GetPaymentSummary()
        {
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaPaymentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPaymentSummarybasedondate")]
        [HttpGet]
        public HttpResponseMessage GetPaymentSummarybasedondate(string fromdate , string todate,string branch_name)
        {
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaPaymentSummarybasedondate(values,fromdate,todate,branch_name);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetreportPaymentExpand")]
        [HttpGet]
        public HttpResponseMessage GetreportPaymentExpand(string month, string year)
        {
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DareportPaymentExpand(month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        [ActionName("GetPaymentmodeExpand")]
        [HttpGet]
        public HttpResponseMessage GetPaymentmodeExpand()
        {
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaGetPaymentmodeExpand(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        public HttpResponseMessage GetReportExportExcel()
        {
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaGetReportExportExcel(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLastSixMonths_List")]
        [HttpGet]

        public HttpResponseMessage GetLastSixMonths_List()
        {
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaGetLastSixMonths_List(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPaymentReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetPaymentReportForLastSixMonthsSearch(string from, string to)
        {

            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaGetPaymentReportForLastSixMonthsSearch(values, from, to);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeDetailsSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDetailsSummary(string month_wise)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPayTrnReportPayment values = new MdlPayTrnReportPayment();
            objDaRptReportPayment.DaGetEmployeeDetailsSummary(getsessionvalues.employee_gid, month_wise, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}