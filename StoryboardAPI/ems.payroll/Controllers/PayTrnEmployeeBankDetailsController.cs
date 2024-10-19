﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;


namespace ems.payroll.Controllers
{


    [Authorize]
    [RoutePrefix("api/PayTrnEmployeeBankDetails")]
    public class PayTrnEmployeeBankDetailsController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaEmployeeBankDetails objdaemployeebankdetails = new DaEmployeeBankDetails();

        [ActionName("GetEmployeeBankDetailsSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeBankDetailsSummary()
        {
            MdlPayTrnEmployeeBankDetails values = new MdlPayTrnEmployeeBankDetails();
            objdaemployeebankdetails.DaGetEmployeeBankDetailsSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostEmployeeBankDetails")]
        [HttpPost]
        public HttpResponseMessage PostEmployeeBankDetails(employeebankdetails_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaemployeebankdetails.DaPostEmployeeBankDetails(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBankDtl")]
        [HttpGet]
        public HttpResponseMessage GetBankDtl()
        {
            MdlPayTrnEmployeeBankDetails values = new MdlPayTrnEmployeeBankDetails();
            objdaemployeebankdetails.DaGetBankDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getbankdetails")]
        [HttpGet]
        public HttpResponseMessage Getbankdetails(string employee_gid)
        {
            MdlPayTrnEmployeeBankDetails values = new MdlPayTrnEmployeeBankDetails();
            objdaemployeebankdetails.DaGetBankdetails(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("BankDtlImport")]
        [HttpPost]
        public HttpResponseMessage BankDtlImport()
        {
            HttpRequest httpRequest;
            employeebankdetails_list values = new employeebankdetails_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result1 objResult = new result1();
            objdaemployeebankdetails.DaBankDtlImport(httpRequest, getsessionvalues.user_gid, objResult, values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetDocumentDtllist")]
        [HttpGet]
        public HttpResponseMessage GetDocumentDtllist(string document_gid)
        {
            MdlPayTrnEmployeeBankDetails objresult = new MdlPayTrnEmployeeBankDetails();
            objdaemployeebankdetails.DaGetDocumentDtllist(document_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetDocumentlist")]
        [HttpGet]
        public HttpResponseMessage GetDocumentlist()
        {
            MdlPayTrnEmployeeBankDetails values = new MdlPayTrnEmployeeBankDetails();
            objdaemployeebankdetails.DaGetDocumentlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}