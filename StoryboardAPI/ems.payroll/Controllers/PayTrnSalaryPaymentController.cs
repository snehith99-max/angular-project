using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayTrnSalaryPayment")]
    public class PayTrnSalaryPaymentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayTrnSalaryPayment objDaPayTrnSalaryPayment = new DaPayTrnSalaryPayment();


        [ActionName("GetSalaryPaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalaryPaymentSummary()
        {
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.DaGetSalaryPaymentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getonchangebankname")]
        [HttpGet]
        public HttpResponseMessage getonchangebankname(string month, string year , string bankname)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.Dagetonchangebankname( month, year, bankname, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeBankDtl")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeBankDtl()
        {
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.DaGetEmployeeBankDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeePayFromBank")]
        [HttpGet]
        public HttpResponseMessage GetEmployeePayFromBank()
        {
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.DaGetEmployeePayFromBank(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetSalaryPaymentExpand")]
        [HttpGet]
        public HttpResponseMessage GetSalaryPaymentExpand(string month,string year)
        {
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.DaGetSalaryPaymentExpand(month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
      
        [ActionName("GetSalaryPaymentExpand2")]
        [HttpGet]
        public HttpResponseMessage GetSalaryPaymentExpand2(string month, string year, string payment_date, string modeof_payment)
        {
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.DaGetSalaryPaymentExpand2(month, year, payment_date, modeof_payment, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMakePaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetMakePaymentSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.DaGetMakePaymentSummary(month, year, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMakePayment")]
        [HttpPost]
        public HttpResponseMessage PostMakePayment(payment_listdtl values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayTrnSalaryPayment.DaPostMakePayment(getsessionvalues.user_gid, getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("salarypaymentupdate")]
        [HttpPost]
        public HttpResponseMessage salarypaymentupdate(salaryedit_listdtl values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPayTrnSalaryPayment.Dasalarypaymentupdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getsalarypaymentedit")]
        [HttpGet]
        public HttpResponseMessage getsalarypaymentedit(string payment_date, string payment_type,string payment_month,string payment_year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.Dagetsalarypaymentedit( payment_type, payment_date, payment_month, payment_year,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getDeletePayment")]
        [HttpGet]
        public HttpResponseMessage getDeletePayment(string payment_date, string payment_type, string payment_month, string payment_year ,string paid_bank)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            objDaPayTrnSalaryPayment.Dagetdeletepayment(payment_type, payment_date, payment_month, payment_year,paid_bank, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ExportExcel")]
        [HttpGet]
        public HttpResponseMessage ExportExcel(string payment_date, string paidbybank, string payment_type)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            var ls_response = new Dictionary<string, object>();
            MdlPayTrnSalaryPayment values = new MdlPayTrnSalaryPayment();
            try { ls_response = objDaPayTrnSalaryPayment.DaExportexcelbankdetails(getsessionvalues.user_gid, payment_date, paidbybank, payment_type, values); }
            catch (Exception ex)
            {

                ls_response = new Dictionary<string, object>
                    {
                        {"status",false },
                        {"message",ex.Message}
                    };
            }

            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
    }
}