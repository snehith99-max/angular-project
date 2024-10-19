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


namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/CampaignService")]
    public class CampaignServiceController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCampaignService objdacampaignservice = new DaCampaignService();

        [ActionName("GetWhatsappSummary")]
        [HttpGet]
        public HttpResponseMessage GetWhatsappSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetWhatsappSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateWhatsappService")]
        [HttpPost]
        public HttpResponseMessage UpdateWhatsappService(campaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateWhatsappService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetShopifySummary")]
        [HttpGet]
        public HttpResponseMessage GetShopifySummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetShopifySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetMailSummary")]
        [HttpGet]
        public HttpResponseMessage GetMailSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetMailSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetGMailSummary")]
        [HttpGet]
        public HttpResponseMessage GetGMailSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetGMailSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateShopifyService")]
        [HttpPost]
        public HttpResponseMessage UpdateShopifyService(shopifyservcie_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateShopifyService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateEmailService")]
        [HttpPost]
        public HttpResponseMessage UpdateEmailService(emailservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateEmailService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdategmailService")]
        [HttpPost]
        public HttpResponseMessage UpdategmailService(gmailservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdategmailService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetFacebookServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetFacebookServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetFacebookServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostFacebookkeys")]
        [HttpPost]
        public HttpResponseMessage PostFacebookkeys(facebookcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaPostFacebookkeys(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postmailmanagementkeys")]
        [HttpPost]
        public HttpResponseMessage Postmailmanagementkeys(gmailservice_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaPostmailmanagementkeys(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updatefacebookkeys")]
        [HttpPost]
        public HttpResponseMessage updatefacebookkeys(facebookcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.Daupdatefacebookkeys(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleteaccesstoken")]
        [HttpGet]
        public HttpResponseMessage deleteaccesstoken(string page_id)
        {
            facebookcampaignservice_list objresult = new facebookcampaignservice_list();
            objdacampaignservice.Dadeleteaccesstoken(page_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("deletemailkey")]
        [HttpGet]
        public HttpResponseMessage deletemailkey(string s_no)
        {
            gmailservice_lists objresult = new gmailservice_lists();
            objdacampaignservice.Dadeletemailkey(s_no, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetLinkedinServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetLinkedinServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetLinkedinServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateLinkedinService")]
        [HttpPost]
        public HttpResponseMessage UpdateLinkedinService(linkedincampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateLinkedinService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelegramServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelegramServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetTelegramServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateTelegramService")]
        [HttpPost]
        public HttpResponseMessage UpdateTelegramService(telegramcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateTelegramService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
      
        [ActionName("GetCustomerTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerTypeSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetCustomerTypeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateCustomerType")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerType(customertype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateCustomerType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLivechatServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetLivechatServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetLivechatServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        public HttpResponseMessage UpdateLivechatService(livechatservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateLivechatService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCompanySummary")]
        [HttpGet]
        public HttpResponseMessage GetCompanySummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetCompanySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCountry")]
        [HttpGet]
        public HttpResponseMessage GetCountry()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetCountry(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostCompanyDetails")]
        [HttpPost]
        public HttpResponseMessage PostCompanyDetails()

        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objdacampaignservice.DaPostCompanyDetails(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);

        }
        [ActionName("PostCompanyDetailsForm")]
        [HttpPost]
        public HttpResponseMessage PostCompanyDetailsForm(Company_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdacampaignservice.DaPostCompanyDetailsForm(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getcurrency")]
        [HttpGet]
        public HttpResponseMessage Getcurrency()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetcurrency(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetModuleNameSummary")]
        [HttpGet]
        public HttpResponseMessage GetModuleNameSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaGetModuleNameSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetScreenNameSummary")]
        [HttpGet]
        public HttpResponseMessage GetScreenNameSummary(string module_gid)
        {
            MdlCampaignService values = new MdlCampaignService();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaGetScreenNameSummary(getsessionvalues.user_gid, values, module_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("updatemodulename")]
        [HttpPost]
        public HttpResponseMessage updatemodulename(updatemodule_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.Daupdatemodulename(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetModuleSummery")]
        [HttpGet]
        public HttpResponseMessage GetModuleSummery()
        {
            MdlCampaignService values = new MdlCampaignService();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaGetModuleSummery(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        public HttpResponseMessage UpdateCicktocallService(clicktocall_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateCicktocallService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetClicktocallSummary")]
        [HttpGet]
        public HttpResponseMessage GetClicktocallSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetClicktocallSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetGoogleanalyticsserviceSummary")]
        [HttpGet]
        public HttpResponseMessage GetGoogleanalyticsserviceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetGoogleanalyticsserviceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updategoogleanalyticsservice")]
        [HttpPost]
        public HttpResponseMessage updategoogleanalyticsservice(googleanalyticsservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.Daupdategoogleanalyticsservice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSmsServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetSmsServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetSmsServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateSmsService")]
        [HttpPost]
        public HttpResponseMessage UpdateSmsService(smscampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateSmsService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetIndiaMARTServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetIndiaMARTServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetIndiaMARTServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateIndiaMARTService")]
        [HttpPost]
        public HttpResponseMessage UpdateIndiaMARTService(indiamartcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateIndiaMARTService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCalendarSummary")]
        [HttpGet]
        public HttpResponseMessage GetCalendarSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetCalendarSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateCalendarService")]
        [HttpPost]
        public HttpResponseMessage UpdateCalendarService(calendarservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateCalendarService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEinvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetEinvoiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetEinvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateEinvoice")]
        [HttpPost]
        public HttpResponseMessage UpdateEinvoice(einvoice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateEinvoice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMintSoftSummary")]
        [HttpGet]
        public HttpResponseMessage GetMintSoftSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetMintSoftSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateMintSoft")]
        [HttpPost]
        public HttpResponseMessage UpdateMintSoft(mintsoft_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateMintSoft(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInstaServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetInstaServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetInstaServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostInstakeys")]
        [HttpPost]
        public HttpResponseMessage PostInstakeys(instagramcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaPostInstakeys(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updateinstakeys")]
        [HttpPost]
        public HttpResponseMessage updateinstakeys(instagramcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.Daupdateinstakeys(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleteinstaaccesstoken")]
        [HttpGet]
        public HttpResponseMessage deleteinstaaccesstoken(string instagram_account_id)
        {
            instagramcampaignservice_list objresult = new instagramcampaignservice_list();
            objdacampaignservice.Dadeleteinstaaccesstoken(instagram_account_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("UpdateoutlookService")]
        [HttpPost]
        public HttpResponseMessage UpdateoutlookService(outlookservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateoutlookService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOutlookSummary")]
        [HttpGet]
        public HttpResponseMessage GetOutlookSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetOutlookSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMailmanagementSummary")]
        [HttpGet]
        public HttpResponseMessage GetMailmanagementSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetMailmanagementSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeeMailsTag")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeMailsTag(string emailaddress)
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetEmployeeMailsTag(values, emailaddress);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeeMailsUnTag")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeMailsUnTag(string emailaddress)
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetEmployeeMailsUnTag(values, emailaddress);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("TagemptoGmail")]
        [HttpPost]
        public HttpResponseMessage TagemptoGmail(tagemployee values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdacampaignservice.DaTagemptoGmail( values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("UnTagempGmail")]
        [HttpPost]
        public HttpResponseMessage UnTagempGmail(untagemployee values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdacampaignservice.DaUnTagempGmail(values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("updatepaymentgatewayservice")]
        [HttpPost]
        public HttpResponseMessage updatepaymentgatewayservice(paymentgatewayservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.Daupdatepaymentgatewayservice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetpaymentgatewaySummary")]
        [HttpGet]
        public HttpResponseMessage GetpaymentgatewaySummary()
        {
            paymentgatewayservice_list values = new paymentgatewayservice_list();
            objdacampaignservice.DaGetpaymentgatewaySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getuserhomepage")]
        [HttpGet]
        public HttpResponseMessage Getuserhomepage()
        {
            Mdlhomepage values = new Mdlhomepage();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaGetuserhomepage(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("enablekotscreen")]
        [HttpGet]
        public HttpResponseMessage enablekotscreen(string selectedOption)
        {
            result objresult = new result();
            objdacampaignservice.Daenablekotscreen(selectedOption, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getkotscreensum")]
        [HttpGet]
        public HttpResponseMessage Getkotscreensum()
        {
            Mdlenablekot values = new Mdlenablekot();
            objdacampaignservice.DaGetkotscreensum(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}