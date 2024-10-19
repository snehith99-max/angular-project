﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.IO;




namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Leadbank")]
    public class LeadbankController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLeadBank objDaleadbank = new DaLeadBank();

        [ActionName("GetLeadbankSummary")]
        [HttpGet]
        public HttpResponseMessage GetLeadbankSummary()
        {
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetLeadbankSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadbankSummary1")]
        [HttpGet]
        public HttpResponseMessage GetLeadbankSummary1()
        {
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetLeadbankSummary1(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadbankSummary2")]
        [HttpGet]
        public HttpResponseMessage GetLeadbankSummary2()
        {
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetLeadbankSummary2(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetleadbankeditSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbankeditSummary(string leadbank_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadbankeditSummary(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Getsourcedropdown")]
        [HttpGet]
        public HttpResponseMessage Getsourcedropdown()
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetsourcedropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Getregiondropdown")]
        [HttpGet]
        public HttpResponseMessage Getregiondropdown()
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetregiondropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Getindustrydropdown")]
        [HttpGet]
        public HttpResponseMessage Getindustrydropdown()
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetindustrydropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Getcountrynamedropdown")]
        [HttpGet]
        public HttpResponseMessage Getcountrynamedropdown()
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetcountrynamedropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Postleadbank")]
        [HttpPost]
        public HttpResponseMessage Postleadbank(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaleadbank.DaPostleadbank(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("RetailerPostleadbank")]
        [HttpPost]
        public HttpResponseMessage RetailerPostleadbank(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaleadbank.DaRetailerPostleadbank(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetleadbankcontacteditsSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbankcontacteditsSummary(string leadbankcontact_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadbankcontacteditsSummary(leadbankcontact_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Updateleadbank")]
        [HttpPost]
        public HttpResponseMessage Updateleadbank(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaleadbank.DaUpdateleadbank(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetleadbankviewSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbankviewSummary(string leadbank_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadbankviewSummary(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetleadbankviewSummary1")]
        [HttpGet]
        public HttpResponseMessage GetleadbankviewSummary1(string leadbank_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadbankviewSummary1(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetleadbankviewSummary2")]
        [HttpGet]
        public HttpResponseMessage GetleadbankviewSummary2(string leadbank_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadbankviewSummary2(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Getbranchdropdown")]
        [HttpGet]
        public HttpResponseMessage Getbranchdropdown(string leadbank_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetbranchdropdown(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("GetleadbankcontactaddSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbankcontactaddSummary(string leadbank_gid)
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadbankcontactaddSummary(leadbank_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Postleadbankcontactadd")]
        [HttpPost]
        public HttpResponseMessage Postleadbankcontactadd(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaleadbank.DaPostleadbankcontactadd(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("UpdateleadbankContactedit")]
        [HttpPost]
        public HttpResponseMessage UpdateleadbankContactedit(leadbank_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaleadbank.DaUpdateleadbankContactedit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deleteLeadbankSummary")]
        [HttpGet]
        public HttpResponseMessage deleteLeadbankSummary(string leadbank_gid)
        {
            leadbank_list values = new leadbank_list();
            objDaleadbank.DadeleteLeadbankSummary(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleteLeadbankcontact")]
        [HttpGet]
        public HttpResponseMessage deleteLeadbankcontact(string leadbankcontact_gid)
        {
            leadbank_list values = new leadbank_list();
            objDaleadbank.DadeleteLeadbankcontact(leadbankcontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        public HttpResponseMessage GetLeadReportExport()
        {
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetLeadReportExport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("LeadReportImport")]
        [HttpPost]
        public HttpResponseMessage LeadReportImport()
        {
            HttpRequest httpRequest;
            leadbank_list values = new leadbank_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaleadbank.DaLeadReportImport(httpRequest, getsessionvalues.user_gid, objResult, values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        // Lead Bank Count
        [ActionName("GetLeadBankCount")]
        [HttpGet]
        public HttpResponseMessage GetLeadBankCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetLeadBankCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerTypeSummary()
        {
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetCustomerTypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadsCount")]
        [HttpGet]
        public HttpResponseMessage GetLeadsCount()
        {
            MdlLeadBank values = new MdlLeadBank();
            objDaleadbank.DaGetLeadsCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getleadtypedropdown")]
        [HttpGet]
        public HttpResponseMessage Getleadtypedropdown()
        {
            MdlLeadBank objresult = new MdlLeadBank();
            objDaleadbank.DaGetleadtypedropdown(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}
