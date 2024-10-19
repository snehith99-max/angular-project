﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayTrnLoanSummary")]

    public class PayTrnLoanSummaryController : ApiController
    {
    
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLoan objDaLoan = new DaLoan();


        [ActionName("GetLoanSummary")]
        [HttpGet]
        public HttpResponseMessage GetLoanSummary()
        {
            MdlPayTrnLoanSummary values = new MdlPayTrnLoanSummary();
            objDaLoan.DaGetLoanSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeDtl")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDtl()
        {
            MdlPayTrnLoanSummary values = new MdlPayTrnLoanSummary();
            objDaLoan.DaGetEmployeeDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        [ActionName("GetBankDetail")]
        [HttpGet]
        public HttpResponseMessage GetBankDetail()
        {
            MdlPayTrnLoanSummary values = new MdlPayTrnLoanSummary();
            objDaLoan.DaGetBankDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostLoan")]
        [HttpPost]
        public HttpResponseMessage PostLoan( loan_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLoan.DaPostLoan(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getUpdatedLoan")]
        [HttpPost]
        public HttpResponseMessage getUpdatedLoan( loan_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLoan.DagetUpdatedLoan(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getEditLoan")]
        [HttpGet]
        public HttpResponseMessage getEditLoan(string loan_gid)
        {
            MdlPayTrnLoanSummary values = new MdlPayTrnLoanSummary();
            objDaLoan.DagetEditLoan(loan_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getViewLoanSummary")]
        [HttpGet]
        public HttpResponseMessage getViewLoanSummary(string loan_gid)
        {
            MdlPayTrnLoanSummary values = new MdlPayTrnLoanSummary();
            objDaLoan.DagetViewLoanSummary(loan_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getLoanrepaySummary")]
        [HttpGet]
        public HttpResponseMessage getLoanrepaySummary(string loan_gid)
        {
            MdlPayTrnLoanSummary values = new MdlPayTrnLoanSummary();
            objDaLoan.DagetLoanrepaySummary(loan_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getUpdatedmonth")]
        [HttpPost]
        public HttpResponseMessage getUpdatedmonth(month_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLoan.DagetUpdatedmonth(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}