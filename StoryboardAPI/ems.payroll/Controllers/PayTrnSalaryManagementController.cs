using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.utilities.Models;


namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayTrnSalaryManagement")]
    public class PayTrnSalaryManagementController :ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayTrnSalaryManagement objdaemployeesalary = new DaPayTrnSalaryManagement();


        [ActionName("GetEmployeeSalaryManagement")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeSalaryManagement()
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaEmployeeSalaryManagement(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeeSelect")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeSelect(string month,string year)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetEmployeeSelect(month,year,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getpayrunsummary")]
        [HttpGet]
        public HttpResponseMessage Getpayrunsummary(string month, string year)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetpayrunsummary(month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postforpayrun")]
        [HttpPost]
        public HttpResponseMessage Postforpayrun(GetEmployeelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaemployeesalary.DaPostforpayrun(values);
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }

        [ActionName("Updatemonthlypayrun")]
        [HttpPost]
        public HttpResponseMessage Updatemonthlypayrun(Getmonthlypayrun values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaemployeesalary.DaUpdatemonthlypayrun(getsessionvalues.user_gid,getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetManageLeave")]
        [HttpGet]
        public HttpResponseMessage GetManageLeave(string month, string year)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetManageLeave(month,year,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetManageLeaveDate")]
        [HttpGet]
        public HttpResponseMessage GetManageLeaveDate(string fromDate, string toDate)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetManageLeaveDate(fromDate, toDate, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Additionalsubsummary")]
        [HttpGet]
        public HttpResponseMessage Additionalsubsummary(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaAdditionalsubsummary(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Deductsubsummary")]
        [HttpGet]
        public HttpResponseMessage Deductsubsummary(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaDeductsubsummary(salary_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("othersummary")]
        [HttpGet]
        public HttpResponseMessage othersummary(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.Daothersummary(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getpayrundetails")]
        [HttpGet]
        public HttpResponseMessage Getpayrundetails(string salary_gid)
        {
            Payrun_list values = new Payrun_list();
            values = objdaemployeesalary.DaGetpayrundetails(salary_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getpayruneditsummary")]
        [HttpGet]
        public HttpResponseMessage Getpayruneditsummary(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetpayruneditsummary(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetadditionEdit")]
        [HttpGet]
        public HttpResponseMessage GetadditionEdit(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetadditionEdit(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetdeductionEdit")]
        [HttpGet]
        public HttpResponseMessage GetdeductionEdit(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetdeductionEdit(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetothersEdit")]
        [HttpGet]
        public HttpResponseMessage GetothersEdit(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetothersEdit(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetSalaryEdit")]
        [HttpGet]
        public HttpResponseMessage GetSalaryEdit(string salary_gid)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetSalaryEdit(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Updatepayrunedit")]
        [HttpPost]
        public HttpResponseMessage Updatepayrunedit(UpdatePayrun values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaemployeesalary.DaUpdatepayrunedit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("GetLeaveReport")]
        [HttpGet]
        public HttpResponseMessage GetLeaveReport(string month, string year)
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetLeaveReport(month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Deleteleavereport")]
        [HttpPost]
        public HttpResponseMessage Deleteleavereport(deleteleavereportlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaemployeesalary.DaDeleteleavereport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBasicSalary")]
        [HttpGet]
        public HttpResponseMessage GetBasicSalary()
        {
            MdlPayTrnSalaryManagement values = new MdlPayTrnSalaryManagement();
            objdaemployeesalary.DaGetBasicSalary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



    }
}
   
