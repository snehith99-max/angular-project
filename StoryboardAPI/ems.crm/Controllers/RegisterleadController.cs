﻿using ems.crm.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ems.crm.Models;

namespace ems.crm.Controllers
{
    [RoutePrefix("api/registerlead")]
    [Authorize]
    public class RegisterLeadController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaRegisterLead objDaRegisterLead = new DaRegisterLead();

        // Module Summary
        [ActionName("GetRegisterLeadSummary")]
        [HttpGet]
        public HttpResponseMessage GetRegisterLeadSummary()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRegisterLead.DaGetRegisterLeadSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRegisterLeadSummary1")]
        [HttpGet]
        public HttpResponseMessage GetRegisterLeadSummary1()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRegisterLead.DaGetRegisterLeadSummary1(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRegisterLeadSummary2")]
        [HttpGet]
        public HttpResponseMessage GetRegisterLeadSummary2()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRegisterLead.DaGetRegisterLeadSummary2(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Post  Terms and conditions
        [ActionName("Postregisterlead")]
        [HttpPost]
        public HttpResponseMessage Postregisterlead(Registerlead_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRegisterLead.DaPostregisterlead(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("postbranchlead")]
        //[HttpPost]
        //public HttpResponseMessage postbranchlead(leadaddbranch_list values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDaRegisterLead.Dapostbranchlead(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("Addbranchlead")]
        [HttpPost]
        public HttpResponseMessage Addbranchlead(leadaddbranch_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRegisterLead.DaAddbranchlead(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Getcountrynamedropdown")]
        [HttpGet]
        public HttpResponseMessage Getcountrynamedropdown()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            objDaRegisterLead.DaGetcountrynamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getregiondropdown1")]
        [HttpGet]
        public HttpResponseMessage Getregiondropdown1()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            objDaRegisterLead.DaGetregiondropdown1(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getindustrydropdown")]
        [HttpGet]
        public HttpResponseMessage Getindustrydropdown()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            objDaRegisterLead.DaGetindustrydropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSourcetypedropdown")]
        [HttpGet]
        public HttpResponseMessage GetSourcetypedropdown()
        {
            MdlRegisterLead values = new MdlRegisterLead();
            objDaRegisterLead.DaGetSourcetypedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getbranchdropdown")]
        [HttpGet]
        public HttpResponseMessage Getbranchdropdown(string leadbank_gid)
        {
            MdlRegisterLead objresult = new MdlRegisterLead();
            objDaRegisterLead.DaGetbranchdropdown(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("deleteregisterleadSummary")]
        [HttpGet]
        public HttpResponseMessage deleteregisterleadSummary(string leadbank_gid)
        {
            GetRegisterLeadSummary_list values = new GetRegisterLeadSummary_list();
            objDaRegisterLead.DadeleteregisterleadSummary(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetleadbranchaddSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbranchaddSummary(string leadbank_gid)
        {
            MdlRegisterLead objresult = new MdlRegisterLead();
            objDaRegisterLead.DaGetleadbranchaddSummary(leadbank_gid , objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }



        [ActionName("GetleadbrancheditSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbrancheditSummary(string leadbankcontact_gid)
        {
            MdlRegisterLead objresult = new MdlRegisterLead();
            objDaRegisterLead.DaGetleadbankbrancheditSummary(leadbankcontact_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Updateleadbranchedit")]
        [HttpPost]
        public HttpResponseMessage Updateleadbranchedit(leadaddbranch_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaRegisterLead.DaUpdateleadbranchedit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Register Lead Count
        [ActionName("GetRegisterLeadCount")]
        [HttpGet]
        public HttpResponseMessage GetRegisterLeadCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlRegisterLead values = new MdlRegisterLead();
            objDaRegisterLead.DaGetRegisterLeadCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCustomerTypeCount")]
        [HttpGet]
        public HttpResponseMessage GetCustomerTypeCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlRegisterLead values = new MdlRegisterLead();
            objDaRegisterLead.DaGetCustomerTypeCount(getsessionvalues.employee_gid,  values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
