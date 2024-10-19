using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
namespace ems.payroll.Controllers

{
    [Authorize]
    [RoutePrefix("api/PayRptPayrunSummary")]

    public class PayRptPayrunSummaryController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaRptPayrunSummary objDaRptPayrunSummary = new DaRptPayrunSummary();


        [ActionName("GetPayrunSummary")]
        [HttpGet]
        public HttpResponseMessage GetPayrunSummary(string branch_gid, string department_gid, string year, string month)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetPayrunSummary(branch_gid, department_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getpayruninitialsummary")]
        [HttpGet]
        public HttpResponseMessage Getpayruninitialsummary(string month,string year)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetpayruninitialsummary(month, year,values);
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }

        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDepartmentDtl")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentDtl()
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetDepartmentDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPayslipRpt")]
        [HttpGet]
        public HttpResponseMessage GetPayslipRpt(string month, string year, string salary_gid)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            var ls_response = new Dictionary<string, object>();
            try { ls_response = objDaRptPayrunSummary.DaGetPayslipRpt(month, year, salary_gid, values); }
            catch (Exception ex) {                
                
                ls_response = new Dictionary<string, object>
                    {
                        {"status",false },
                        {"message",ex.Message}
                    };
            }    
            
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
        [ActionName("postpayrunmail")]
        [HttpPost]
        public HttpResponseMessage postpayrunmail()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest=HttpContext.Current.Request;
            result objResult = new result();
            objDaRptPayrunSummary.Dapayrunmail(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Deleteforpayrun")]
        [HttpPost]
        public HttpResponseMessage Deleteforpayrun(deletepayrunlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRptPayrunSummary.DaDeleteforpayrun(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMailId")]
        [HttpGet]
        public HttpResponseMessage GetMailId()
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRptPayrunSummary.DaMaillId(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetPayrunReportForLastSixMonthsSearch")]
        //[HttpGet]
        //public HttpResponseMessage GetPayrunReportForLastSixMonthsSearch(string from, string to)
        //{

        //    MdlRptPayrunSummary values = new MdlRptPayrunSummary();
        //    objDaRptPayrunSummary.DaGetPayrunReportForLastSixMonthsSearch(values, from, to);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
    }
}