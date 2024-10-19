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

    [RoutePrefix("api/AccTrnCashBookSummary")]
    [Authorize]
    public class AccTrnCashBookSummaryController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccTrnCashBookSummary objDafinance = new DaAccTrnCashBookSummary();
        // Module Summary
        [ActionName("GetAccTrnCashbooksummary")]
        [HttpGet]
        public HttpResponseMessage GetAccTrnCashbooksummary()
        {
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetAccTrnCashbooksummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("GetAccTrnCashbookSelect")]
        //[HttpGet]
        //public HttpResponseMessage GetAccTrnCashbookSelect(string branch_gid, string finyear_gid)
        //{
        //    MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
        //    objDafinance.DaGetAccTrnCashbookSelect(values,branch_gid, finyear_gid);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetAccTrnCashbookSelect")]
        [HttpGet]
        public HttpResponseMessage GetAccTrnCashbookSelect(string branch_gid, string from_date, string to_date)
        {
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetAccTrnCashbookSelect(values, branch_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCashBookDtlEdit")]
        [HttpGet]
        public HttpResponseMessage GetCashBookDtlEdit(string branch_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetCashBookDtlEdit(getsessionvalues.user_gid, values, branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateCashBookDtls")]
        [HttpPost]
        public HttpResponseMessage UpdateCashBookDtls(cashbookedit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDafinance.DaUpdateCashBookDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeleteCashBookDtls")]
        [HttpGet]
        public HttpResponseMessage GetDeleteCashBookDtls(string journal_gid, string journaldtl_gid, string account_gid)
        {
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetDeleteCashBookDtls(values, journal_gid, journaldtl_gid, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCashBookEntryView")]
        [HttpGet]
        public HttpResponseMessage GetCashBookEntryView(string branch_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetCashBookEntryView(getsessionvalues.user_gid, values, branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCashBookEnterBy")]
        [HttpGet]
        public HttpResponseMessage GetCashBookEnterBy()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetCashBookEnterBy(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAccountMulAddDtls")]
        [HttpPost]
        public HttpResponseMessage PostAccountMulAddDtls(acctmuladd_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDafinance.DaPostAccountMulAddDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCashAccountMulAddDtl")]
        [HttpGet]
        public HttpResponseMessage GetCashAccountMulAddDtl()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetCashAccountMulAddDtl(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeleteMulBankDtls")]
        [HttpGet]
        public HttpResponseMessage GetDeleteMulBankDtls(string session_id)
        {
            MdlAccTrnCashBookSummary values = new MdlAccTrnCashBookSummary();
            objDafinance.DaGetDeleteMulBankDtls(values, session_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDirectCashBookEntry")]
        [HttpPost]
        public HttpResponseMessage PostDirectCashBookEntry(cashbookadd_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDafinance.DaPostDirectCashBookEntry(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}