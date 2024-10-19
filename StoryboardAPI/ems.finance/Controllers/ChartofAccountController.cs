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
    // Code By snehith
    [Authorize]
    [RoutePrefix("api/ChartofAccount")]
    public class ChartofAccountController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaChartofAccount objdacreditcard = new DaChartofAccount();

        [ActionName("ChartofAccountSummary")]
        [HttpGet]
        public HttpResponseMessage ChartofAccountSummary()
        {
            MdlChartofAccount values = new MdlChartofAccount();
            objdacreditcard.DaChartofAccountSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }     

        [ActionName("ChartofAccountAssetSummary")]
        [HttpGet]
        public HttpResponseMessage ChartofAccountAssetSummary()
        {
            MdlChartofAccount values = new MdlChartofAccount();
            objdacreditcard.DaChartofAccountAssetSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ChartofAccountLiabilitySummary")]
        [HttpGet]
        public HttpResponseMessage ChartofAccountLiabilitySummary()
        {
            MdlChartofAccount values = new MdlChartofAccount();
            objdacreditcard.DaChartofAccountLiabilitySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ChartofAccountIncomeSummary")]
        [HttpGet]
        public HttpResponseMessage ChartofAccountIncomeSummary()
        {
            MdlChartofAccount values = new MdlChartofAccount();
            objdacreditcard.DaChartofAccountIncomeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ChartofSubAccountSummary")]
        [HttpGet]
        public HttpResponseMessage ChartofSubAccountSummary(string account_gid)
        {
            MdlChartofAccount values = new MdlChartofAccount();
            objdacreditcard.DaChartofSubAccountSummary(values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteChartofAccount")]
        [HttpGet]
        public HttpResponseMessage DeleteChartofAccount(string account_gid, string account_type)
        {
            chartstatus_lists values = new chartstatus_lists();
            objdacreditcard.DaDeleteChartofAccount(account_gid, account_type, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostAccountGroup")]
        [HttpPost]
        public HttpResponseMessage PostAccountGroup(chartaccount_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaPostAccountGroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateAccountGroup")]
        [HttpPost]
        public HttpResponseMessage UpdateAccountGroup(chartgroupupdate_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaUpdateAccountGroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostAccountSubGroup")]
        [HttpPost]
        public HttpResponseMessage PostAccountSubGroup(chartsubaccount_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaPostAccountSubGroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateAccountSubGroup")]
        [HttpPost]
        public HttpResponseMessage UpdateAccountSubGroup(chartsubaccount_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaUpdateAccountSubGroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ChartofAccountCountList")]
        [HttpGet]
        public HttpResponseMessage ChartofAccountCountList()
        {
            MdlChartofAccount values = new MdlChartofAccount();
            objdacreditcard.DaChartofAccountCountList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getFolders")]
        [HttpGet]
        public HttpResponseMessage getFolders()
        {
            MdlFinanceFolders objresult = new MdlFinanceFolders();
            objdacreditcard.DaChartofAccountFolderList(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("UpdateLedger")]
        [HttpPost]
        public HttpResponseMessage UpdateLedger(chartsubaccount_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacreditcard.DaUpdateLedger(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}