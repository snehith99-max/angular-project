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
                                                                          //code by a snehith
    [RoutePrefix("api/ProfitLossReport")]
    [Authorize]
    public class ProfitLossReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaProfitLossReport objdacreditcard = new DaProfitLossReport();

      
        [ActionName("GetProfitLossIncome")]
        [HttpGet]
        public HttpResponseMessage GetProfitLossIncome(string branch, string year_gid)
        {
            MdlProfitLossReport values = new MdlProfitLossReport();
            objdacreditcard.DaGetProfitLossIncome(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProfitLossExpense")]
        [HttpGet]
        public HttpResponseMessage GetProfitLossExpense(string branch, string year_gid)
        {
            MdlProfitLossReport values = new MdlProfitLossReport();
            objdacreditcard.DaGetProfitLossExpense(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProfilelossfinyear")]
        [HttpGet]
        public HttpResponseMessage GetProfilelossfinyear()
        {
            MdlProfitLossReport values = new MdlProfitLossReport();
            objdacreditcard.DaGetProfilelossfinyear(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProfitandlosslDetails")]
        [HttpGet]
        public HttpResponseMessage GetProfitandlosslDetails(string account_gid)
        {
            MdlProfitLossReport values = new MdlProfitLossReport();
            objdacreditcard.DaGetProfitandlosslDetails(account_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        [ActionName("GetSummaryExpense")]
        [HttpGet]
        public HttpResponseMessage GetSummaryExpense(string branch, string year_gid)
        {
            MdlFinanceExpenseFolders values = new MdlFinanceExpenseFolders();
            objdacreditcard.DaGetSummaryExpense(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSummaryIncome")]
        [HttpGet]
        public HttpResponseMessage GetSummaryIncome(string branch, string year_gid)
        {
            MdlFinanceExpenseFolders values = new MdlFinanceExpenseFolders();
            objdacreditcard.DaGetSummaryIncome(branch, year_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}