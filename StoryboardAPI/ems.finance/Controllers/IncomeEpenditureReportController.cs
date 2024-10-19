using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.finance.DataAccess;
using ems.finance.Models;

namespace ems.finance.Controllers
{
    [Authorize]
    [RoutePrefix("api/IncomeEpenditureReport")]
    public class IncomeEpenditureReportController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaIncomeEpenditureReport objdacreditcard = new DaIncomeEpenditureReport();
        [ActionName("GVcreditNeedDataSource")]
        [HttpGet]
        public HttpResponseMessage GVcreditNeedDataSource(string branch, string from_date, string to_date)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGVcreditNeedDataSource(branch, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GVcreditDetailTable")]
        [HttpGet]
        public HttpResponseMessage GVcreditDetailTable(string branch, string from_date, string to_date, string month, string year)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGVcreditDetailTable(branch, from_date, to_date, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetIncomeReport")]
        [HttpGet]
        public HttpResponseMessage GetIncomeReport(string branch, string from_date, string to_date)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGetIncomeReport(branch, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GVdebitNeedDataSource")]
        [HttpGet]
        public HttpResponseMessage GVdebitNeedDataSource(string branch, string from_date, string to_date)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGVdebitNeedDataSource(branch, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GVdebitDetailTable")]
        [HttpGet]
        public HttpResponseMessage GVdebitDetailTable(string branch, string from_date, string to_date, string month, string year)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGVdebitDetailTable(branch, from_date, to_date, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetExpenseReport")]
        [HttpGet]
        public HttpResponseMessage GetExpenseReport(string branch, string from_date, string to_date)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGetExpenseReport(branch, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBarChartIncomeexpene")]
        [HttpGet]
        public HttpResponseMessage GetBarChartIncomeexpene(string branch, string from_date, string to_date)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGetBarChartIncomeexpene(branch, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GVPopTransaction")]
        [HttpGet]
        public HttpResponseMessage GVPopTransaction(string branch, string from_date, string to_date)
        {
            MdlIncomeEpenditureReport values = new MdlIncomeEpenditureReport();
            objdacreditcard.DaGVPopTransaction(branch, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}