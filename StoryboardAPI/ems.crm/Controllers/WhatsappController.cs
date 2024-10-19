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
using System.Web.Http.Results;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Whatsapp")]
    public class WhatsappController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaWhatsapp objDaWhatsapp = new DaWhatsapp();

        [ActionName("CreateContact")]
        [HttpPost]
        public HttpResponseMessage CreateContact(mdlCreateContactInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.dacreatecontact(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("UpdateContact")]
        [HttpPost]
        public HttpResponseMessage UpdateContact(mdlUpdateContactInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.daUpdateContact(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("CreateProject")]
        [HttpPost]
        public HttpResponseMessage CreateProject(mdlCreateTemplateInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.daCreateProject(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("PostTemplateCreation")]
        [HttpPost]
        public HttpResponseMessage PostTemplateCreation()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objResult = objDaWhatsapp.daPostTemplateCreation(httpRequest);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("PostTextTemplateCreation")]
        [HttpPost]
        public HttpResponseMessage PostTextTemplateCreation(template_creation values)
        {
            result objResult = new result();
            objResult = objDaWhatsapp.daPostTextTemplateCreation(values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
  
        [ActionName("WhatsappSend")]
        [HttpPost]
        public HttpResponseMessage WhatsappSend(sendmessage values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.DaWhatsappSend(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }


        [ActionName("GetContact")]
        [HttpGet]
        public HttpResponseMessage GetContact()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetContact(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetContacts")]
        [HttpGet]
        public HttpResponseMessage GetContacts()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetContacts(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMessage")]
        [HttpGet]
        public HttpResponseMessage GetMessage(string whatsapp_gid)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetMessage(values, whatsapp_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplate")]
        [HttpGet]
        public HttpResponseMessage GetTemplate()
        {
            result values = new result();
            values = objDaWhatsapp.DaGetTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageTemplatesummary")]
        [HttpGet]
        public HttpResponseMessage GetMessageTemplatesummary()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetMessageTemplatesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageTemplateview")]
        [HttpGet]
        public HttpResponseMessage GetMessageTemplateview(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetMessageTemplateview(values, project_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcampaign")]
        [HttpGet]
        public HttpResponseMessage Getcampaign()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetcampaign(values);

            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteCampaign")]
        [HttpGet]
        public HttpResponseMessage DeleteCampaign(string project_id)
        {
            whatsappCampaign objresult = new whatsappCampaign();
            objDaWhatsapp.DaDeleteCampaign(project_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getlog")]
        [HttpGet]
        public HttpResponseMessage Getlog(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetlog(values, project_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplatepreview")]
        [HttpGet]
        public HttpResponseMessage GetTemplatepreview(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetTemplatepreview(values, project_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetContactCount")]
        [HttpGet]
        public HttpResponseMessage GetContactCount()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetContactCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetWhatsappMessageCount")]
        [HttpGet]
        public HttpResponseMessage GetWhatsappMessageCount()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetWhatsappMessageCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("bulkMessageSend")]
        [HttpPost]
        public HttpResponseMessage bulkMessageSend(mdlBulkMessageList values)
        {
            result objresult = new result();
            objresult = objDaWhatsapp.dabulkMessageSend(values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("bulkcustomizeMessageSend")]
        [HttpPost]
        public HttpResponseMessage bulkcustomizeMessageSend(mdlBulkMessageList values)
        {
            result objresult = new result();
            objresult = objDaWhatsapp.dabulkcustomizeMessageSend(values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("waNotifications")]
        [HttpGet]
        public HttpResponseMessage waNotifications()
        {
            notifications objNotifications = new notifications();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objNotifications = objDaWhatsapp.daNotifications(getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objNotifications);
        }
        [ActionName("Getbreadcrumbmail")]
        [HttpGet]
        public HttpResponseMessage Getbreadcrumbmail(string sref)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetbreadcrumbmail(sref, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("waSendDocuments")]
        [HttpPost]
        public HttpResponseMessage waSendDocuments()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objResult = objDaWhatsapp.daSendDocuments(httpRequest);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("waGetDocuments")]
        [HttpGet]
        public HttpResponseMessage waGetDocuments(string contact_id)
        {
            MdlWaFiles obj = new MdlWaFiles();
            obj = objDaWhatsapp.daGetDocuments(contact_id);
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [ActionName("UpdateWhatsappCampaignStatus")]
        [HttpPost]
        public HttpResponseMessage UpdateWhatsappCampaignStatus(Campaignstatus values)
        {
            objDaWhatsapp.DaUpdateWhatsappCampaignStatus(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCampaignContactsent")]
        [HttpGet]
        public HttpResponseMessage GetCampaignContactsent(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetCampaignContactsent(project_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCampaignContactunsent")]
        [HttpGet]
        public HttpResponseMessage GetCampaignContactunsent()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetCampaignContactunsent(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetCustomerTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerTypeSummary()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetCustomerTypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getservicewindowcontact")]
        [HttpGet]
        public HttpResponseMessage Getservicewindowcontact()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetservicewindowcontact(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostImportExcel")]
        [HttpPost]
        public HttpResponseMessage PostImportExcel(mdlimportlead values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objresult = new result();
            objresult = objDaWhatsapp.daPostImportExcel(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetContactist")]
        [HttpGet]
        public HttpResponseMessage GetContactist(string region_name,string source_name,string customer_type)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetContactist(region_name, source_name, customer_type ,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeadContact")]
        [HttpGet]
        public HttpResponseMessage GetLeadContact()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetLeadContact(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("whatsappcontactsImport")]
        [HttpPost]
        public HttpResponseMessage whatsappcontactsImport()
        {
            HttpRequest httpRequest;
            leadbank_list values = new leadbank_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objresult = new result();
            objresult = objDaWhatsapp.DawhatsappcontactsImport(httpRequest, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("whatsappcontactsleadImport")]
        [HttpPost]
        public HttpResponseMessage whatsappcontactsleadImport()
        {
            HttpRequest httpRequest;
            leadbank_list values = new leadbank_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objresult = new result();
            objresult = objDaWhatsapp.DawhatsappcontactsleadsImports(httpRequest, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getimportcontact")]
        [HttpGet]
        public HttpResponseMessage Getimportcontact(string region_name, string source_name, string customer_type)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetimportcontact(region_name, source_name, customer_type, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updatewhatsappcontact")]
        [HttpPost]
        public HttpResponseMessage updatewhatsappcontact(mdlUpdateContactInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaWhatsapp.Daupdatewhatsappcontact(values,getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

//------------------------------------------for excel log start ------------------------------------------------//

        [ActionName("GetlogWhatsapplist")]
        [HttpGet]
        public HttpResponseMessage GetlogWhatsapplist()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetlogWhatsapplist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetlogWhatsappdtllist")]
        [HttpGet]
        public HttpResponseMessage GetlogWhatsappdtllist(string document_gid)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetlogWhatsappdtllist(document_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

// -----------------------------------invalid number log ------------------------------//

        [ActionName("GetlogInvalidnumber")]
        [HttpGet]
        public HttpResponseMessage GetlogInvalidnumber()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetlogInvalidnumber( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetloginvalidDTLlist")]
        [HttpGet]
        public HttpResponseMessage GetloginvalidDTLlist(string document_gid)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetloginvalidDTLlist(document_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //------------------------------------------for excel log end ------------------------------------------------//
        [ActionName("Getsentcampaignsentchart")]
        [HttpGet]
        public HttpResponseMessage Getsentcampaignsentchart()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetsentcampaignsentchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetsentDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetsentDetailSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetsentDetailSummary(getsessionvalues.employee_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetcampaignSearch")]
        [HttpGet]
        public HttpResponseMessage GetcampaignSearch(string from_date, string to_date)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetcampaignSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcurrentchart")]
        [HttpGet]
        public HttpResponseMessage Getcurrentchart(string status)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetcurrentchart(status,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getmessagestatus")]
        [HttpGet]
        public HttpResponseMessage Getmessagestatus()
        {
            result values = new result();
            values = objDaWhatsapp.DaGetmessagestatus();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcustomerreport")]
        [HttpGet]
        public HttpResponseMessage Getcustomerreport()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetcustomerreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcampaignlist")]
        [HttpGet]
        public HttpResponseMessage Getcampaignlist(string contact_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetcampaignlist(values, contact_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}