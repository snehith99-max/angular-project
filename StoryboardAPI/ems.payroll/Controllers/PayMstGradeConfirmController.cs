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
using ems.system.Models;

namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayMstGradeConfirm")]
    public class PayMstGradeConfirmController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstGradeConfirm objdagradeconfirm = new DaPayMstGradeConfirm();

        [ActionName("additionsummarybind")]
        [HttpGet]
        public HttpResponseMessage additionsummarybind(string salarygradetemplategid, string gross_salary)
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.Daadditionsummarybind(salarygradetemplategid, gross_salary, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Deductionsummarybind")]
        [HttpGet]
        public HttpResponseMessage Deductionsummarybind(string salarygradetemplategid, string gross_salary)
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaDeductionsummarybind(salarygradetemplategid, gross_salary, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //[ActionName("Otherssummarybind")]
        //[HttpGet]
        //public HttpResponseMessage Otherssummarybind(string salarygradetemplategid, string gross_salary)
        //{
        //    MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
        //    objdagradeconfirm.DaOtherssummarybind(salarygradetemplategid, gross_salary,values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}



        [ActionName("Getsalarygradetemplatedropdown")]
        [HttpGet]

        public HttpResponseMessage Getgradetemplatedropdown()
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaGetsalarygradetemplatedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getDeleteComponent")]
        [HttpGet]
        public HttpResponseMessage getDeleteComponent(string salarygradetmpdtl_gid,string salarygradetemplate_gid)
        {
            MdlPayMstGradeConfirm objresult = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DagetDeleteComponent(salarygradetmpdtl_gid, salarygradetemplate_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Getcomponentlistdropdown")]
        [HttpGet]

        public HttpResponseMessage Getcomponentlistdropdown()
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaGetcomponentlistdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Updateempgrade")]
        [HttpPost]
        public HttpResponseMessage Updateempgrade(Updateempgradelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagradeconfirm.DaUpdateempgrade(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getcomponenttypedropdown")]
        [HttpGet]

        public HttpResponseMessage Getcomponenttypedropdown()
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaGetcomponenttypedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeComponentgroup")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeComponentgroup(string component_type)
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaGetOnChangeComponentgroup(values, component_type);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeComponent")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeComponent(string componentgroup_name)
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaGetOnChangeComponent(values, componentgroup_name);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeComponentamount")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeComponentamount(string component_name)
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objdagradeconfirm.DaGetOnChangeComponentamount(values,component_name);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Postemployee2grade")]
        [HttpPost]
        public HttpResponseMessage Postemployeeconfirmation(employee2gradelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagradeconfirm.DaPostemployee2grade(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }























    }
}