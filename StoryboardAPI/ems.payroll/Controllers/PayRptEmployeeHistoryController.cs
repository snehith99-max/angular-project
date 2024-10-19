using System;
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
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayRptEmployeeHistory")]
    public class PayRptEmployeeHistoryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayRptEmployeeHistory objDaPayRptEmployeeHistory = new DaPayRptEmployeeHistory();

        [ActionName("GetBranchDetail")]
        [HttpGet]
        public HttpResponseMessage GetBranchDetail()
        {
            MdlPayRptEmployeeHistory values = new MdlPayRptEmployeeHistory();
            objDaPayRptEmployeeHistory.DaGetBranchDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDepartmentDetail")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentDetail(string branch_gid)
        {
            MdlPayRptEmployeeHistory values = new MdlPayRptEmployeeHistory();
            objDaPayRptEmployeeHistory.DaGetDepartmentDetail(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeHistory")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeHistory(string branch_name, string department_name)
        {
            MdlPayRptEmployeeHistory values = new MdlPayRptEmployeeHistory();
            objDaPayRptEmployeeHistory.DaGetEmployeeHistory(branch_name, department_name, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getViewEmployeePaymentSummary")]
        [HttpGet]
        public HttpResponseMessage getViewEmployeePaymentSummary(string employee_gid)
        {
            MdlPayRptEmployeeHistory values = new MdlPayRptEmployeeHistory();
            objDaPayRptEmployeeHistory.DagetViewEmployeePaymentSummary(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getViewPromotionHistory")]
        [HttpGet]
        public HttpResponseMessage getViewPromotionHistory(string employee_gid)
        {
            MdlPayRptEmployeeHistory values = new MdlPayRptEmployeeHistory();
            objDaPayRptEmployeeHistory.DagetViewPromotionHistory(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getViewPaymentDetails")]
        [HttpGet]
        public HttpResponseMessage getViewPaymentDetails(string employee_gid)
        {
            MdlPayRptEmployeeHistory values = new MdlPayRptEmployeeHistory();
            objDaPayRptEmployeeHistory.DagetViewPaymentDetails(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}