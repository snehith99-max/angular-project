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
    [RoutePrefix("api/AccTrnRecordExpenseSummary")]
    public class AccTrnRecordExpenseSummaryController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccTrnRecordExpenseSummary objdacreditcard = new DaAccTrnRecordExpenseSummary();

        [ActionName("RecordExpenseSummary")]
        [HttpGet]
        public HttpResponseMessage RecordExpenseSummary()
        {
            MdlAccTrnRecordExpenseSummary values = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.DaRecordExpenseSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("onchangevendordetails")]
        [HttpGet]
        public HttpResponseMessage onchangevendordetails( string vendor_gid)
        {
            MdlAccTrnRecordExpenseSummary values = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.daonchangevendordetails( vendor_gid ,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getvendordropdown")]
        [HttpGet]
        public HttpResponseMessage getvendordropdown() 
        {
            MdlAccTrnRecordExpenseSummary values = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.dagetvendordropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getaccountgroupnamedropdown")]
        [HttpGet]
        public HttpResponseMessage Getaccountgroupnamedropdown()
        {
            MdlAccTrnRecordExpenseSummary values = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.DaGetaccountgroupnamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("onchangeaccountgroup")]
        [HttpGet]
        public HttpResponseMessage onchangeaccountgroup(string account_gid)
        {
            MdlAccTrnRecordExpenseSummary values = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.daonchangeaccountgroup(account_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateMakePayment")]
        [HttpPost]
        public HttpResponseMessage UpdateMakePayment(record_expense_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaMakePaymentUpdate(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostExpenseMulAddDtls")]
        [HttpPost]
        public HttpResponseMessage PostExpenseMulAddDtls(expensesubmitlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaPostExpenseMulAddDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostExpenseDetailsAdd")]
        [HttpPost]
        public HttpResponseMessage PostExpenseDetailsAdd(expensedetailadd_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaPostExpenseDetailsAdd(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deleteexpensesummary")]
        [HttpGet]
        public HttpResponseMessage deleteexpensesummary(string expenserequisition_gid)
        {
            MdlAccTrnRecordExpenseSummary objresult = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.DaDeleteExpense(expenserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("MakePaymentdetails")]
        [HttpGet]
        public HttpResponseMessage MakePaymentdetails(string expenserequisition_gid)
        {
            MdlAccTrnRecordExpenseSummary objresult = new MdlAccTrnRecordExpenseSummary();
            objdacreditcard.DaMakePaymentdetails(expenserequisition_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}
