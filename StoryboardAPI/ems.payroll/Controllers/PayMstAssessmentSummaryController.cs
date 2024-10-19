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
    [RoutePrefix("api/PayMstAssessmentSummary")]
    [Authorize]
    public class PayMstAssessmentSummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstAssessment objDaPayMstAssessment = new DaPayMstAssessment();

        [ActionName("Getassessmentsummary")]
        [HttpGet]
        public HttpResponseMessage Getassessmentsummary()
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            objDaPayMstAssessment.Daassessmentsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getassessmentyear")]
        [HttpGet]
        public HttpResponseMessage Getassessmentyear(string assessment_gid)
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            objDaPayMstAssessment.DaGetassessmentyear(values, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getassignempsummary")]
        [HttpGet]
        public HttpResponseMessage Getassignempsummary(string assessment_gid)
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            objDaPayMstAssessment.Daassignempsummary(assessment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postassignemployee")]
        [HttpPost]
        public HttpResponseMessage Postassignemployee(postassignemployeelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaPostassignemployee(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getgenerateformsummary")]
        [HttpGet]
        public HttpResponseMessage Getgenerateformsummary(string assessment_gid)
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            objDaPayMstAssessment.Dagenerateformsummary(assessment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPersonaldata")]
        [HttpGet]
        public HttpResponseMessage GetPersonaldata(string employee_gid)
        {
            MdlPersonalData values = new MdlPersonalData();
            values = objDaPayMstAssessment.DaGetPersonaldata(employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatePersonalInfo")]
        [HttpPost]
        public HttpResponseMessage UpdatePersonalInfo(updatepersonalinfolist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaUpdatePersonalInfo(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getfinyeardropdown")]
        [HttpGet]
        public HttpResponseMessage Getfinyeardropdown()
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            objDaPayMstAssessment.DaGetfinyeardropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostIncometax")]
        [HttpPost]
        public HttpResponseMessage PostIncometax()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaPayMstAssessment.DaPostIncometax(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("Getincometaxsummary")]
        [HttpGet]
        public HttpResponseMessage Getincometaxsummary()
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            objDaPayMstAssessment.DaGetincometaxsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deleteincometaxsummary")]
        [HttpGet]
        public HttpResponseMessage deleteincometaxsummary(string taxdocument_gid)
        {
            MdlPayMstAssessment objresult = new MdlPayMstAssessment();
            objDaPayMstAssessment.Dadeleteincometaxsummary(taxdocument_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostQuartersInfo")]
        [HttpPost]
        public HttpResponseMessage PostQuartersInfo(postquartersinfolist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaPostQuartersInfo(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetQuatersdata")]
        [HttpGet]
        public HttpResponseMessage GetQuatersdata(string employee_gid)
        {
            MdlQuartersData values = new MdlQuartersData();
            values = objDaPayMstAssessment.DaGetQuatersdata(employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncomedata")]
        [HttpGet]
        public HttpResponseMessage GetIncomedata(string employee_gid, string assessment_gid)
        {
            MdlPayIncomedata values = new MdlPayIncomedata();
            values = objDaPayMstAssessment.DaGetIncomedata(employee_gid, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostIncome")]
        [HttpPost]
        public HttpResponseMessage PostIncome(MdlPayIncomedata values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaPostIncome(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostTDS")]
        [HttpPost]
        public HttpResponseMessage PostTDS(MdlPayTDSdata values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaPostTDS(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTDSdata")]
        [HttpGet]
        public HttpResponseMessage GetTDSdata(string employee_gid, string assessment_gid)
        {
            MdlPayTDSdata values = new MdlPayTDSdata();
            values = objDaPayMstAssessment.DaGetTDSdata(employee_gid, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTdsPDF")]
        [HttpGet]
        public HttpResponseMessage GetTdsPDF(string assessment_gid, string employee_gid)
        {
            MdlPayMstAssessment values = new MdlPayMstAssessment();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaPayMstAssessment.DaGetTdsPDF(assessment_gid, employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("Getanx1data")]
        [HttpGet]
        public HttpResponseMessage Getanx1data(string employee_gid, string assessment_gid)
        {
            Mdlanx1data values = new Mdlanx1data();
            values = objDaPayMstAssessment.DaGetanx1data(employee_gid, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postanx1")]
        [HttpPost]
        public HttpResponseMessage Postanx1(MdlAnx1data values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaPostanx1(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getanx2data")]
        [HttpGet]
        public HttpResponseMessage Getanx2data(string employee_gid, string assessment_gid)
        {
            Mdlanx2data values = new Mdlanx2data();
            values = objDaPayMstAssessment.DaGetanx2data(employee_gid, assessment_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postanx2")]
        [HttpPost]
        public HttpResponseMessage Postanx2(MdlAnx2data values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayMstAssessment.DaPostanx2(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncometaxdata")]
        [HttpGet]
        public HttpResponseMessage GetIncometaxdata(string employee_gid)
        {
            MdlIncometaxData values = new MdlIncometaxData();
            values = objDaPayMstAssessment.DaGetIncometaxdata(employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}