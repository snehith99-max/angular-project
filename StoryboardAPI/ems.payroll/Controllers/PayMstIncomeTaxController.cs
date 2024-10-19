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
using ems.system.Models;

namespace ems.payroll.Controllers
{
    [RoutePrefix("api/PayMstIncomeTax")]
    [Authorize]
    public class PayMstIncomeTaxController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstIncomeTax objDaPayMstIncomeTax = new DaPayMstIncomeTax();

        [ActionName("GetIncomeTaxMasterSummary")]
        [HttpGet]
        public HttpResponseMessage GetIncomeTaxMasterSummary()
        {
            MdlPayMstIncomeTax values = new MdlPayMstIncomeTax();
            objDaPayMstIncomeTax.DaGetIncomeTaxMasterSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetIncomeTaxMasterNew")]
        [HttpGet]
        public HttpResponseMessage GetIncomeTaxMasterNew()
        {
            MdlPayMstIncomeTax values = new MdlPayMstIncomeTax();
            objDaPayMstIncomeTax.DaGetIncomeTaxMasterNew(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("PostIncomeTaxRates")]
        [HttpPost]
        public HttpResponseMessage PostIncomeTaxRates(incometax_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objDaPayMstIncomeTax.DaPostIncomeTaxRates(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getUpdatedIncomeTaxRates")]
        [HttpPost]
        public HttpResponseMessage getUpdatedIncomeTaxRates(incometax_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstIncomeTax.DagetUpdatedIncomeTaxRates(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}