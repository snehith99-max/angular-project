using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using static ems.payroll.Models.MdlPayTrnPfManagement;

namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayTrnPfManagement")]
    public class PayTrnPfManagementController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayTrnPfManagement objdapay = new DaPayTrnPfManagement();

        [ActionName("GetPfManagementSummary")]
        [HttpGet]
        public HttpResponseMessage GetPfManagementSummary()
        {
            MdlPayTrnPfManagement values = new MdlPayTrnPfManagement();
            objdapay.DaGetPfManagementSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPfEmployeeSummary")]
        [HttpGet]
        public HttpResponseMessage GetPfEmployeeSummary()
        {
            MdlPayTrnPfManagement values = new MdlPayTrnPfManagement();
            objdapay.DaGetPfEmployeeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("EmployeeAssignSubmit")]
        [HttpPost]
        public HttpResponseMessage EmployeeAssignSubmit(GetEmployeesubmit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdapay.DaEmployeeAssignSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeePfSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeePfSummary(string employee_gid)
        {
            MdlPayTrnPfManagement values = new MdlPayTrnPfManagement();
            objdapay.DaGetEmployeePfSummary(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostemployeeAssignSubmit")]
        [HttpPost]
        public HttpResponseMessage PostemployeeAssignSubmit(GetEmployeesubmit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdapay.DaPostemployeeAssignSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}