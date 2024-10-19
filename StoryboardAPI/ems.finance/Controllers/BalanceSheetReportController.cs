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
using System.Web.Http.Results;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/BalanceSheetReport")]
    [Authorize]
    public class BalanceSheetReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaBalanceSheetReport objdacreditcard = new DaBalanceSheetReport();

        [ActionName("GetBalanceSheetLiability")]
        [HttpGet]
        public HttpResponseMessage GetBalanceSheetLiability(string branch, string year_gid)
        {
            MdlBalanceSheetReport values = new MdlBalanceSheetReport();
            objdacreditcard.DaGetBalanceSheetLiability(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBalanceSheetAsset")]
        [HttpGet]
        public HttpResponseMessage GetBalanceSheetAsset(string branch, string year_gid)
        {
            MdlBalanceSheetReport values = new MdlBalanceSheetReport();
            objdacreditcard.DaGetBalanceSheetAsset(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("GetProfilelossfinyear")]
        //[HttpGet]
        //public HttpResponseMessage GetProfilelossfinyear()
        //{
        //    MdlBalanceSheetReport values = new MdlBalanceSheetReport();
        //    objdacreditcard.DaGetProfilelossfinyear(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        //[ActionName("GetProfitandlosslDetails")]
        //[HttpGet]
        //public HttpResponseMessage GetProfitandlosslDetails(string account_gid)
        //{
        //    MdlBalanceSheetReport values = new MdlBalanceSheetReport();
        //    objdacreditcard.DaGetProfitandlosslDetails(account_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GetSummaryLiability")]
        [HttpGet]
        public HttpResponseMessage GetSummaryLiability(string branch, string year_gid)
        {
            MdlFinanceLiabilityFolders values = new MdlFinanceLiabilityFolders();
            objdacreditcard.DaGetSummaryLiability(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSummaryAsset")]
        [HttpGet]
        public HttpResponseMessage GetSummaryAsset(string branch, string year_gid)
        {
            MdlFinanceLiabilityFolders values = new MdlFinanceLiabilityFolders();
            objdacreditcard.DaGetSummaryAsset(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetNetAmountDetails")]
        [HttpGet]
        public HttpResponseMessage GetNetAmountDetails(string branch, string year_gid)
        {
            MdlBalanceSheetReport values = new MdlBalanceSheetReport();
            objdacreditcard.DaGetNetAmountDetails(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}