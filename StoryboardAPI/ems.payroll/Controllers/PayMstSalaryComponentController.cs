﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.payroll.DataAccess;
using System.Net.Http;
using ems.payroll.Models;
using static OfficeOpenXml.ExcelErrorValue;
using System.Web.Http.Results;


namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayMstSalaryComponent")]

    public class PayMstSalaryComponentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstSalaryComponent objdasalarycomponent = new DaPayMstSalaryComponent();


        // GET: PayMstSalaryComponent

        [ActionName("GetSalaryComponentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalaryComponentSummary()
        {
            MdlPayMstSalaryComponent values = new MdlPayMstSalaryComponent();
            objdasalarycomponent.DaGetSalaryComponentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCustomizeComponent")]
        [HttpGet]
        public HttpResponseMessage GetCustomizeComponent()
        {
            MdlPayMstSalaryComponent values = new MdlPayMstSalaryComponent();
            objdasalarycomponent.DaGetCustomizeComponent(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAddtionVariable")]
        [HttpGet]
        public HttpResponseMessage GetAddtionVariable()
        {
            MdlPayMstSalaryComponent values = new MdlPayMstSalaryComponent();
            objdasalarycomponent.DaGetAddtionVariable(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       

        [ActionName("GetComponentGroupDtl")]
        [HttpGet]
        public HttpResponseMessage GetComponentGroupDtl()
        {
            MdlPayMstSalaryComponent values = new MdlPayMstSalaryComponent();
            objdasalarycomponent.DaGetComponentGroupDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostComponent")]
        [HttpPost]
        public HttpResponseMessage PostComponent(string user_gid, salarycompoent_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdasalarycomponent.DaPostComponent(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getEditComponent")]
        [HttpGet]
        public HttpResponseMessage getEditComponent(string salarycomponent_gid)
        {
            MdlPayMstSalaryComponent values = new MdlPayMstSalaryComponent();
            objdasalarycomponent.DagetEditComponent(salarycomponent_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
      
        [ActionName("getUpdatedComponent")]
        [HttpPost]
        public HttpResponseMessage getUpdatedComponent(salarycompoent_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdasalarycomponent.DagetUpdatedComponent(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getDeleteComponent")]
        [HttpGet]
        public HttpResponseMessage getDeleteComponent(string params_gid)
        {
            salarycompoent_list objresult = new salarycompoent_list();
            objdasalarycomponent.DagetDeleteComponent(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("getViewSalaryComponentSummary")]
        [HttpGet]
        public HttpResponseMessage getViewSalaryComponentSummary(string salarycomponent_gid)
        {
            MdlPayMstSalaryComponent values = new MdlPayMstSalaryComponent();
            objdasalarycomponent.DagetViewSalaryComponentSummary(salarycomponent_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}