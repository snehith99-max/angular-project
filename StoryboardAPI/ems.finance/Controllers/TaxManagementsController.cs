using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/TaxManagements")]
    [Authorize]
    public class TaxManagementsController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaTaxManagements objfinance = new DaTaxManagements();

        [ActionName("GetTaxManagementSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxManagementSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetTaxManagementSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOutputTaxView")]
        [HttpGet]
        public HttpResponseMessage GetOutputTaxView(string taxfiling_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetOutputTaxView(getsessionvalues.user_gid, values, taxfiling_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInputTaxView")]
        [HttpGet]
        public HttpResponseMessage GetInputTaxView(string taxfiling_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetInputTaxView(getsessionvalues.user_gid, values, taxfiling_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCreditNoteTaxView")]
        [HttpGet]
        public HttpResponseMessage GetCreditNoteTaxView(string taxfiling_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetCreditNoteTaxView(getsessionvalues.user_gid, values, taxfiling_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTotalTaxView")]
        [HttpGet]
        public HttpResponseMessage GetTotalTaxView(string taxfiling_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetTotalTaxView(getsessionvalues.user_gid, values, taxfiling_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTotalCreditTaxView")]
        [HttpGet]
        public HttpResponseMessage GetTotalCreditTaxView(string taxfiling_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetTotalCreditTaxView(getsessionvalues.user_gid, values, taxfiling_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("InputTaxSubmit")]
        [HttpPost]
        public HttpResponseMessage InputTaxSubmit(InputTaxSummary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaInputTaxSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("GetCompileInputTax")]
        [HttpGet]
        public HttpResponseMessage GetCompileInputTax()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetCompileInputTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCompileOutputTax")]
        [HttpGet]
        public HttpResponseMessage GetCompileOutputTax()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetCompileOutputTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCompileCreditTax")]
        [HttpGet]
        public HttpResponseMessage GetCompileCreditTax()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetCompileCreditTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCompileDebitTax")]
        [HttpGet]
        public HttpResponseMessage GetCompileDebitTax()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetCompileDebitTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutputTaxSubmit")]
        [HttpPost]
        public HttpResponseMessage OutputTaxSubmit(OutputTaxSummary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaOutputTaxSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("CreditNoteTaxSubmit")]
        [HttpPost]
        public HttpResponseMessage CreditNoteTaxSubmit(CreditNoteTaxSummary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaCreditNoteTaxSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("DebitNoteTaxSubmit")]
        [HttpPost]
        public HttpResponseMessage DebitNoteTaxSubmit(DebitNoteTaxSummary values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaDebitNoteTaxSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("GetTaxCalculation")]
        [HttpGet]
        public HttpResponseMessage GetTaxCalculation()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaGetTaxCalculation(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }       
        // Tax Overall Submit
        [ActionName("PostTaxOverallSubmit")]
        [HttpPost]
        public HttpResponseMessage PostTaxOverallSubmit(TaxOverallSubmitDtls values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostTaxOverallSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("TempTableDataDeletions")]
        [HttpGet]
        public HttpResponseMessage TempTableDataDeletions()
        {
            MdlTaxManagements values = new MdlTaxManagements();
            objfinance.DaTempTableDataDeletions(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}