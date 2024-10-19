using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.kot.DataAccess;
using ems.kot.Models;
using System.Net.Http;


namespace ems.kot.Controllers
{
    [RoutePrefix("api/WhatsApporderSummary")]
    [Authorize]
    public class WhatsApporderSummaryController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaWhatsApporderSummary objWhatsAppordersummary = new DaWhatsApporderSummary();

        [ActionName("overallsummary")]
        [HttpGet]
        public HttpResponseMessage overallsummary()
        {
            mdloveralsummary values = new mdloveralsummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Daoverallsummary(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatedETD")]
        [HttpPost]
        public HttpResponseMessage UpdatedETD(etdupdate_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.DaUpdatedETD(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Orderstatusupdate")]
        [HttpGet]
        public HttpResponseMessage Orderstatusupdate(string orderstatus, string kot_gid, string customer_phone)
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.DaOrderstatusupdate(orderstatus, kot_gid, getsessionvalues.branch_gid, customer_phone, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("adminreadyordersummary")]
        [HttpGet]
        public HttpResponseMessage adminreadyordersummary()
        {
            mdloveralsummary values = new mdloveralsummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Daadminreadyordersummary(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("admindeliveredordersummary")]
        [HttpGet]
        public HttpResponseMessage admindeliveredordersummary()
        {
            MdlWhatsApporderSummary values = new MdlWhatsApporderSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Daadmindeliveredordersummary(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("shopenable")]
        [HttpGet]
        public HttpResponseMessage shopenable(string status)
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Dashopenable(status, getsessionvalues.branch_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("getshopdetails")]
        [HttpGet]
        public HttpResponseMessage getshopdetails()
        {
            shop_details objresult = new shop_details();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Dagetshopdetails(getsessionvalues.branch_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("payAndUpdateOrder")]
        [HttpPost]
        public HttpResponseMessage payAndUpdateOrder(etdupdate_List values)
        {
            objWhatsAppordersummary.daUpdateAndPayOrder(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Updateorderrejectreason")]
        [HttpPost]
        public HttpResponseMessage Updateorderrejectreason(mdlorderreject values)
        {

            objWhatsAppordersummary.DaUpdateorderrejectreason(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("wosshopenable")]
        [HttpPost]
        public HttpResponseMessage wosshopenable(mdlwosshopenable values)
        {
            objWhatsAppordersummary.Dawosshopenable(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("orderupdates")]
        [HttpPost]
        public HttpResponseMessage orderupdates(mdlorderupdates values)
        {

            objWhatsAppordersummary.Daorderupdates(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("whatsappdashboard")]
        [HttpGet]
        public HttpResponseMessage whatsappdashboard()
        {
            mdlwhatsapporderdashboard values = new mdlwhatsapporderdashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Dawhatsappdashboard(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("whatsappproductdtl")]
        [HttpGet]
        public HttpResponseMessage whatsappproductdtl()
        {
            mdlwhatsapporderprdtdtl values = new mdlwhatsapporderprdtdtl();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Dawhatsappproductdtl(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getwoscontctsummary")]
        [HttpGet]
        public HttpResponseMessage Getwoscontctsummary()
        {
            MdlWhatsApporderSummary values = new MdlWhatsApporderSummary();
            objWhatsAppordersummary.DaGetwoscontctsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetWhatsappchatSummary")]
        [HttpGet]
        public HttpResponseMessage GetMessage(string whatsapp_gid)
        {
            Mdlwhatsappchat_list values = new Mdlwhatsappchat_list();
            objWhatsAppordersummary.DaGetWhatsappchatSummary(values, whatsapp_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updatewoscontact")]
        [HttpPost]
        public HttpResponseMessage updatewoscontact(mdlwoscontactupdate values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.Daupdatewoscontact(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("wosmsgsend")]
        [HttpPost]
        public HttpResponseMessage wosmsgsend(individualmessagesend values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objWhatsAppordersummary.Dawosmsgsend(values, getsessionvalues.user_gid,getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("wosdocumentssend")]
        [HttpPost]
        public HttpResponseMessage wosdocumentssend()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objWhatsAppordersummary.dawosdocumentssend(httpRequest,getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("wosgetfilesummary")]
        [HttpGet]
        public HttpResponseMessage wosgetfilesummary(string contact_id)
        {
            Mdlwosfile_list obj = new Mdlwosfile_list();
            obj = objWhatsAppordersummary.dawosgetfilesummary(contact_id);
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [ActionName("wosContactImport")]
        [HttpPost]
        public HttpResponseMessage ContactImport()
        {
            HttpRequest httpRequest;
            contact_infolist values = new contact_infolist();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objWhatsAppordersummary.DawosContactImport(httpRequest, getsessionvalues.user_gid,getsessionvalues.branch_gid, objResult, values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Getwosmsgtemplatesummary")]
        [HttpGet]
        public HttpResponseMessage Getwosmsgtemplatesummary()
        {
            MdlWhatsApporderSummary values = new MdlWhatsApporderSummary();
            objWhatsAppordersummary.DaGetwosmsgtemplatesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Gettotalcontactlist")]
        [HttpGet]
        public HttpResponseMessage Gettotalcontactlist()
        {
            MdlWhatsApporderSummary values = new MdlWhatsApporderSummary();
            objWhatsAppordersummary.DaGettotalcontactlist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postbulktemplatesend")]
        [HttpPost]
        public HttpResponseMessage Postbulktemplatesend(mdlwosbulkremplates values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objWhatsAppordersummary.DaPostbulktemplatesend(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("woscontactcreate")]
        [HttpPost]
        public HttpResponseMessage woscontactcreate(mdlwoscontactcreate values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objWhatsAppordersummary.dawoscontactcreate(values, getsessionvalues.user_gid, getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
}