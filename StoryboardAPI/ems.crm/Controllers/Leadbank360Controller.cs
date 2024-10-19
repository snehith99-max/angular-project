using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using static OfficeOpenXml.ExcelErrorValue;
using static ems.crm.Models.MdlLeadbank360;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Leadbank360")]
    public class Leadbank360Controller : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLeadbank360 objDaLeadbank360 = new DaLeadbank360();
        [ActionName("GetWhatsappLeadContact")]
        [HttpGet]
        public HttpResponseMessage GetWhatsappLeadContact(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetWhatsappLeadContact(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("CreateContact")]
        [HttpPost]
        public HttpResponseMessage CreateContact(mdlCreateContactInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaLeadbank360.dacreatecontact(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetWhatsappLeadMessage")]
        [HttpGet]
        public HttpResponseMessage GetWhatsappLeadMessage(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaGetWhatsappLeadMessage(values, leadbank_gid, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostLeadWhatsappMessage")]
        [HttpPost]
        public HttpResponseMessage PostLeadWhatsappMessage(leadwhatsappsendmessage values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaLeadbank360.DaPostLeadWhatsappMessage(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("LeadMailSend")]
        [HttpPost]
        public HttpResponseMessage LeadMailSend(leadmailsummary_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objDaLeadbank360.DaLeadMailSend(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetEmailSendDetails")]
        [HttpGet]
        public HttpResponseMessage GetEmailSendDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetEmailSendDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("LeadMailUpload")]
        [HttpPost]
        public HttpResponseMessage LeadMailUpload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaLeadbank360.DaLeadMailUpload(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("GetLeadOrderDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadOrderDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadOrderDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadQuotationDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadQuotationDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadQuotationDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadInvoiceDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadInvoiceDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadInvoiceDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadCountDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadCountDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadCountDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Document Summary
        [ActionName("GetLeadDocumentDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadDocumentDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadDocumentDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Document upload
        [ActionName("LeadDocumentUpload")]
        [HttpPost]
        public HttpResponseMessage LeadDocumentUpload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            results objResult = new results();
            objDaLeadbank360.DaLeadDocumentUpload(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        //Document Download

        [ActionName("LeadDocumentdownload")]
        [HttpGet]
        public HttpResponseMessage LeadDocumentdownload(string document_gid)
        {
            MdlLeadbank360 objresult = new MdlLeadbank360();
            objDaLeadbank360.DaLeadDocumentdownload(document_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        // Notes summary
        [ActionName("GetNotesSummary")]
        [HttpGet]
        public HttpResponseMessage GetNotesSummary(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetNotesSummary(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Notes upload
        [ActionName("LeadNotesUpload")]
        [HttpPost]
        public HttpResponseMessage LeadNotesUpload(notes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objDaLeadbank360.DaLeadNotesUpload(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Lead Basic Details
        [ActionName("GetLeadBasicDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadBasicDetails(string appointment_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaGetLeadBasicDetails(values, appointment_gid, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        // Lead Basictele Details
        [ActionName("GetLeadBasicTeleDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeadBasicTeleDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaGetLeadBasicTeleDetails(values, leadbank_gid, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //editcontact//
        [ActionName("Getupdatecontactdetails")]
        [HttpPost]
        public HttpResponseMessage Getupdatecontactdetails(contactedit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            // getsessionvalues = objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaGetupdatecontactdetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Get edit contact details
        [ActionName("GetEditContactdetails")]
        [HttpGet]
        public HttpResponseMessage GetEditContactdetails(string leadbank_gid)
        {
            MdlLeadbank360 objresult = new MdlLeadbank360();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaGetEditContactdetails(leadbank_gid, objresult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        //Get sales Enquiry details
        [ActionName("GetEnquiryDetails")]
        [HttpGet]
        public HttpResponseMessage GetEnquiryDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetEnquiryDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //GetGidDetails
        [ActionName("GetGidDetails")]
        [HttpGet]
        public HttpResponseMessage GetGidDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetGidDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //GetTeleGidDetails
        [ActionName("GetTeleGidDetails")]
        [HttpGet]
        public HttpResponseMessage GetTeleGidDetails(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetTeleGidDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Add to customer
        [ActionName("Addtocustomer")]
        [HttpGet]
        public HttpResponseMessage Addtocustomer(string leadbank_gid,string displayName,string mobile,string email,string address1,string taxsegment_name,string address2,string customer_city,string currency,string postal_code, string countryname)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaAddtocustomer(values, leadbank_gid, displayName, mobile, email, address1, taxsegment_name, address2, customer_city, currency, postal_code, countryname, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //gmailsend event
        [ActionName("Gmailupload")]
        [HttpPost]
        public HttpResponseMessage Gmailupload()
        {

            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            results objResult = new results();
            objDaLeadbank360.DaGmailupload(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Gmailtext")]
        [HttpPost]
        public HttpResponseMessage Gmailtext(responselist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaGmailtext(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PotentialValue")]
        [HttpPost]
        public HttpResponseMessage PotentialValue(responselist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaPotentialValue(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLeadStage")]
        [HttpGet]
        public HttpResponseMessage GetLeadStage(string leadstage_name, string leadbank_gid)
        {
            MdlLeadbank360 objresult = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadStage(objresult, leadstage_name, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetTeleLeadStage")]
        [HttpGet]
        public HttpResponseMessage GetTeleLeadStage(string leadstage_name, string leadbank_gid)
        {
            MdlLeadbank360 objresult = new MdlLeadbank360();
            objDaLeadbank360.DaGetTeleLeadStage(objresult, leadstage_name, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostLeadStage")]
        [HttpPost]
        public HttpResponseMessage PostLeadStage(new_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaPostLeadStage(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostteleLeadStage")]
        [HttpPost]
        public HttpResponseMessage PostteleLeadStage(new_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaPostteleLeadStage(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Getcalllogreport")]
        [HttpGet]
        public HttpResponseMessage Getcalllogreport(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetcalllogreport(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getdeletedocuments")]
        [HttpGet]
        public HttpResponseMessage Getdeletedocuments(string document_gid)
        {
            MdlLeadbank360 objresult = new MdlLeadbank360();
            objDaLeadbank360.DaGetdeletedocuments(document_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        //Notes upload
        [ActionName("Notesadd")]
        [HttpPost]
        public HttpResponseMessage Notesadd(notes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objDaLeadbank360.DaNotesadd(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Noteupdate")]
        [HttpPost]
        public HttpResponseMessage Noteupdate(notes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objDaLeadbank360.DaNoteupdate(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Notedelete")]
        [HttpPost]
        public HttpResponseMessage Notedelete(notes values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objDaLeadbank360.DaNotedelete(values, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Tele Lead Notes summary
        [ActionName("GetTeleLeadNotesSummary")]
        [HttpGet]
        public HttpResponseMessage GetTeleLeadNotesSummary(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetTeleLeadNotesSummary(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadAppointmentLog")]
        [HttpGet]
        public HttpResponseMessage GetLeadAppointmentLog(string leadbank_gid)
        {
            MdlLeadbank360 values = new MdlLeadbank360();
            objDaLeadbank360.DaGetLeadAppointmentLog(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Updatepricesegement")]
        [HttpPost]
        public HttpResponseMessage Updatepricesegement(pricesegement_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaLeadbank360.DaUpdatepricesegement(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getpricesegement")]
        [HttpGet]
        public HttpResponseMessage Getpricesegement(string customer_gid)
        {
            pricesegement_list values = new pricesegement_list();
            objDaLeadbank360.DaGetpricesegement(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}