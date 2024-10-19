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
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.payroll.Controllers
{
    [RoutePrefix("api/PayMstEmployeeAssessmentSummary")]
    [Authorize]
    public class PayMstEmployeeAssessmentSummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstEmployeeAssessmentSummary objDaPayMstEmployeeAssessment = new DaPayMstEmployeeAssessmentSummary();

        [ActionName("GetEmployeeAssessmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeAssessmentSummary()
        {
            MdlPayMstEmployeeAssessmentSummary values = new MdlPayMstEmployeeAssessmentSummary();
            objDaPayMstEmployeeAssessment.DaEmployeeAssessmentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeePersonalData")]
        [HttpGet]
        public HttpResponseMessage GetEmployeePersonalData()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            MdlEmployeePersonalData values = new MdlEmployeePersonalData();
            values=objDaPayMstEmployeeAssessment.DaEmployeePersonalData(getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeOldTaxSlabSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeOldTaxSlabSummary()
        {
            MdlPayMstEmployeeAssessmentSummary values = new MdlPayMstEmployeeAssessmentSummary();
            objDaPayMstEmployeeAssessment.DaEmployeeOldTaxSlabSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeNewTaxSlabSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeNewTaxSlabSummary()
        {
            MdlPayMstEmployeeAssessmentSummary values = new MdlPayMstEmployeeAssessmentSummary();
            objDaPayMstEmployeeAssessment.DaEmployeeNewTaxSlabSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetFinYearDropdown")]
        [HttpGet]
        public HttpResponseMessage GetFinYearDropdown()
        {
            MdlPayMstEmployeeAssessmentSummary values = new MdlPayMstEmployeeAssessmentSummary();
            objDaPayMstEmployeeAssessment.DaFinYearDropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostEmployeeIncomeTaxDocument")]
        [HttpPost]
        public HttpResponseMessage PostEmployeeIncomeTaxDocument()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaPayMstEmployeeAssessment.DaPostEmployeeIncomeTaxDocument(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetEmployeeIncomeTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeIncomeTaxSummary()
        {
            MdlPayMstEmployeeAssessmentSummary values = new MdlPayMstEmployeeAssessmentSummary();
            objDaPayMstEmployeeAssessment.DaEmployeeIncomeTaxSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostEmployeeIncomeData")]
        [HttpPost]
        public HttpResponseMessage PostEmployeeIncomeData(MdlEmployeeIncomedata values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstEmployeeAssessment.DaPostEmployeeIncomeData(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeIncomedata")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeIncomedata(string assessment_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlEmployeeIncomedata values = new MdlEmployeeIncomedata();
            values = objDaPayMstEmployeeAssessment.DaEmployeeIncomedata(getsessionvalues.employee_gid, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        

        [ActionName("PostEmployeeTDSData")]
        [HttpGet]
        public HttpResponseMessage PostEmployeeTDSData(string tds_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlEmployeeTDSData values = new MdlEmployeeTDSData();
            objDaPayMstEmployeeAssessment.DaPostEmployeeTDSData(values, tds_gid, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ActionName("GetEmployeeTDSData")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeTDSData(string assessment_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlEmployeeTDSData values = new MdlEmployeeTDSData();
            values = objDaPayMstEmployeeAssessment.DaEmployeeTDSData(getsessionvalues.employee_gid, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}