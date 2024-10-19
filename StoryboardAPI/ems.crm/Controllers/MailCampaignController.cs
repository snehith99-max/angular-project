﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.crm.Models;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/MailCampaign")]
    public class MailCampaignController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaMailCampaign objdamailcampaign = new DaMailCampaign();


        [ActionName("GetMailSummary")]
        [HttpGet]
        public HttpResponseMessage GetMailSummary()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("MailSend")]
        [HttpPost]
        public HttpResponseMessage MailSend(mailsummary_list values)
        {
           
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdamailcampaign.DaMailSend(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ScheduledMailSend")]
        [HttpPost]
        public HttpResponseMessage ScheduledMailSend()
        {

            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            results objResult = new results();
            objdamailcampaign.DaScheduledMailSend(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetMailView")]
        [HttpGet]
        public HttpResponseMessage GetMailView(string mailmanagement_gid)
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailView(mailmanagement_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("MailUpload")]
        [HttpPost]
        public HttpResponseMessage MailUpload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            results objResult = new results();
            objdamailcampaign.DaMailUpload(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        /////////////////mail send event by template/////////////////////

        [ActionName("GetMailEventDelivery")]
        [HttpGet]
        public HttpResponseMessage GetMailEventDelivery()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailEventDelivery(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMailEventOpen")]
        [HttpGet]
        public HttpResponseMessage GetMailEventOpen()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailEventOpen(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMailEventClick")]
        [HttpGet]
        public HttpResponseMessage GetMailEventClick()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailEventClick(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMailEventCount")]
        [HttpGet]
        public HttpResponseMessage GetMailEventCount()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailEventCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("SaveTemplate")]
        [HttpPost]
        public HttpResponseMessage SaveTemplate(mailsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamailcampaign.DaSaveTemplate(values,getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("TemplateSummary")]
        [HttpGet]
        public HttpResponseMessage TemplateSummary()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaTemplateSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("SendTemplate")]
        [HttpPost]
        public HttpResponseMessage SendTemplate(mailtemplatesendsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objresult = new result();
            objresult = objdamailcampaign.DaSendTemplate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("MailTemplateSendSummary")]
        [HttpGet]
        public HttpResponseMessage MailTemplateSendSummary(string temp_mail_gid)
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaMailTemplateSendSummary(values,temp_mail_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("MailTemplateView")]
        [HttpGet]
        public HttpResponseMessage MailTemplateView(string temp_mail_gid)
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaMailTemplateView(temp_mail_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("MailSendStatusSummary")]
        [HttpGet]
        public HttpResponseMessage MailSendStatusSummary(string temp_mail_gid)
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaMailSendStatusSummary(temp_mail_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIndividualMailSummary")]
        [HttpGet]
        public HttpResponseMessage GetIndividualMailSummary(string leadbank_gid)
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaIndividualMailSummary(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Addaslead")]
        [HttpPost]
        public HttpResponseMessage Addaslead(mdlAddaslead values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objdamailcampaign.DaAddaslead(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetMailEventCountSummary")]
        [HttpGet]
        public HttpResponseMessage GetMailEventCountSummary()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetMailEventCountSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateMailTemplateStatus")]
        [HttpPost]
        public HttpResponseMessage UpdateMailTemplateStatus(mailtemplatesendsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdamailcampaign.DaUpdateMailTemplateStatus(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerTypeSummary()
        {
            MdlMailCampaign values = new MdlMailCampaign();
            objdamailcampaign.DaGetCustomerTypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
