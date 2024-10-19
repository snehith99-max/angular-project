﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
//using ems.hrm.Models;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayMstEmployeesalarytemplate")]
    public class PayMstEmployeesalarytemplateController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstEmployeesalarytemplate objdapay = new DaPayMstEmployeesalarytemplate();

        [ActionName("GetEmployeesalarytemplateSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeesalarytemplateSummary()
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaGetEmployeesalarytemplateSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GettemplateName")]
        [HttpGet]
        public HttpResponseMessage GettemplateName()
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaGettemplateName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeegradeassignsummary")]
        [HttpGet]
        public HttpResponseMessage GetEmployeegradeassignsummary()
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaGetGetEmployeegradeassignsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetComponentpopup")]
        [HttpGet]
        public HttpResponseMessage GetComponentpopup(string employee2salarygradetemplate_gid,string salarygradetype)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaGetComponentpopup(employee2salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Addtictionsummary")]
        [HttpGet]
        public HttpResponseMessage Addtictionsummary(string salarygradetemplate_gid, string salarygradetype)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaAddtictionsummary(salarygradetemplate_gid,salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deductionsummary")]
        [HttpGet]
        public HttpResponseMessage deductionsummary(string salarygradetemplate_gid, string salarygradetype)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.Dadeductionsummary(salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Detailssummary")]
        [HttpGet]
        public HttpResponseMessage Detailssummary(string salarygradetemplate_gid)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaDetailssummary(salarygradetemplate_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSalaryGradetoemployee")]
        [HttpPost]
        public HttpResponseMessage PostSalaryGradetoemployee(SalaryGradetoemployee values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdapay.DaPostSalaryGradetoemployee(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("EditAddtictionsummary")]
        [HttpGet]
        public HttpResponseMessage EditAddtictionsummary(string employee2salarygradetemplate_gid, string salarygradetype)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaEditAddtictionsummary(employee2salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Editdeductionsummary")]
        [HttpGet]
        public HttpResponseMessage Editdeductionsummary(string employee2salarygradetemplate_gid, string salarygradetype)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaEditdeductionsummary(employee2salarygradetemplate_gid, salarygradetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("EditDetailssummary")]
        [HttpGet]
        public HttpResponseMessage EditDetailssummary(string employee2salarygradetemplate_gid)
        {
            MdlPayMstEmployeesalarytemplate values = new MdlPayMstEmployeesalarytemplate();
            objdapay.DaEditDetailssummary(employee2salarygradetemplate_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("UpdateSalaryGradetoemployee")]
        [HttpPost]
        public HttpResponseMessage UpdateSalaryGradetoemployee(updateSalaryGradetoemployee values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdapay.DaUpdateSalaryGradetoemployee(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}