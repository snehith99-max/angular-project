using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Web.Http.Results;

namespace ems.payroll.Controllers
{
    [RoutePrefix("api/PayMstBankMaster")]
    [Authorize]
    public class PayMstBankMasterController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstBankMaster objDaPayMstBankMaster = new DaPayMstBankMaster();

        [ActionName("GetBankMasterSummary")]
        [HttpGet]
        public HttpResponseMessage GetBankMasterSummary()
        {
            MdlPayMstBankMaster values = new MdlPayMstBankMaster();
            objDaPayMstBankMaster.DaGetBankMasterSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetBankName")]
        [HttpGet]
        public HttpResponseMessage GetBankName()
        {
            MdlPayMstBankMaster values = new MdlPayMstBankMaster();
            objDaPayMstBankMaster.DaGetBankName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccountType")]
        [HttpGet]
        public HttpResponseMessage GetAccountType()
        {
            MdlPayMstBankMaster values = new MdlPayMstBankMaster();
            objDaPayMstBankMaster.DaGetAccountType(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAccountGroup")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroup()
        {
            MdlPayMstBankMaster values = new MdlPayMstBankMaster();
            objDaPayMstBankMaster.DaGetAccountGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchName")]
        [HttpGet]
        public HttpResponseMessage GetBranchName()
        {
            MdlPayMstBankMaster values = new MdlPayMstBankMaster();
            objDaPayMstBankMaster.DaGetBranchName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostBankMaster")]
        [HttpPost]
        public HttpResponseMessage PostBankMaster(GetBankMaster_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstBankMaster.DaPostBankMaster(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBankMasterDetail")]
        [HttpGet]
        public HttpResponseMessage GetBankMasterDetail(string bank_gid)
        {
            MdlPayMstBankMaster objresult = new MdlPayMstBankMaster();
            objDaPayMstBankMaster.DaGetBankMasterDetail(bank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostBankMasterUpdate")]
        [HttpPost]
        public HttpResponseMessage PostBankMasterUpdate(GetEditBankMaster_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstBankMaster.DaPostBankMasterUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}