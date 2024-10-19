using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.crm.Models;
using System.Threading.Tasks;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Google.Apis.Gmail.v1;


namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/GmailCampaign")]
    public class GmailCampaignController : ApiController
    {

        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaGmailCampaign objdagmailcampaign = new DaGmailCampaign();
        cmnfunctions objcmnfunctions = new cmnfunctions();

        //[ActionName("DaRefreshAccessTockenGenerate")]
        //[HttpGet]
        //public HttpResponseMessage RefreshAccessTockenGenerate()
        //{
        //    MdlGmailCampaign values = new MdlGmailCampaign();
        //    objdagmailcampaign.DaRefreshAccessTockenGenerate(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GmailTemplateSummary")]
        [HttpGet]
        public HttpResponseMessage GmailTemplateSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGmailTemplateSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GmailTemplateSendSummary")]
        [HttpGet]
        public HttpResponseMessage GmailTemplateSendSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGmailTemplateSendSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GmailSenditemSummary")]
        [HttpGet]
        public HttpResponseMessage GmailSenditemSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailSenditemSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GmailindividualSenditemSummary")]
        [HttpGet]
        public HttpResponseMessage GmailindividualSenditemSummary(string leadbank_gid)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailindividualSenditemSummary(values, leadbank_gid, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GmailSaveTemplate")]
        [HttpPost]
        public HttpResponseMessage GmailSaveTemplate(gmailsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailSaveTemplate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SendGmailTemplate")]
        [HttpPost]
        public HttpResponseMessage SendGmailTemplate(gmailtemplatesendsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objresult = new result();
            objresult = objdagmailcampaign.DaSendGmailTemplate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GmailTemplateView")]
        [HttpGet]
        public HttpResponseMessage GmailTemplateView(string template_gid)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGmailTemplateView(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getattachement")]
        [HttpGet]
        public HttpResponseMessage Getattachement(string mail_gid)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGetattachement(mail_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateGmailTemplateStatus")]
        [HttpPost]
        public HttpResponseMessage UpdateGmailTemplateStatus(gmailtemplatesendsummary_list values)
        {
            objdagmailcampaign.DaUpdateGmailTemplateStatus(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GmailSendStatusSummary")]
        [HttpGet]
        public HttpResponseMessage GmailSendStatusSummary(string template_gid)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGmailSendStatusSummary(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GmailinboxSummary")]
        [HttpGet]
        public HttpResponseMessage GmailinboxSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailinboxSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GmailAPIinboxSummary")]
        [HttpGet]
        public HttpResponseMessage GmailAPIinboxSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailAPIinboxSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GmailAPIinboxFolderSummary")]
        [HttpGet]
        public HttpResponseMessage GmailAPIinboxFolderSummary(string label_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailAPIinboxFolderSummary(values, label_id, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("shopifyenquiry")]
        [HttpGet]
        public HttpResponseMessage shopifyenquiry()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.Dashopifyenquiry(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("shopifyenquirysummary")]
        [HttpGet]
        public HttpResponseMessage shopifyenquirysummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.Dashopifyenquirysummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetshopifyenquiryviewSummary")]
        [HttpGet]
        public HttpResponseMessage GetshopifyenquiryviewSummary(string s_no)
        {
            gmailapiinboxsummary_list objresult = new gmailapiinboxsummary_list();
            objdagmailcampaign.DaGetshopifyenquiryviewSummary(s_no, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GmailAPIinboxTrashSummary")]
        [HttpGet]
        public HttpResponseMessage GmailAPIinboxTrashSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGmailAPIinboxTrashSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GmailAPIInboxMailLoad")]
        [HttpGet]
        public async Task<HttpResponseMessage> GmailAPIInboxMailLoad()
        {
            try
            {
                get values = new get();

                // Ensure objdagmailcampaign is initialized
                if (objdagmailcampaign != null)
                {
                    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                    getsessionvalues = objgetgid.gettokenvalues(token);
                    values = await objdagmailcampaign.DaGmailAPIInboxMailLoad(getsessionvalues.user_gid);
                    return Request.CreateResponse(HttpStatusCode.OK, values);
                }
                else
                {
                    // Log or handle the case where objdagmailcampaign is null
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "objdagmailcampaign is not initialized.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                // Handle the exception based on your application's requirements
                objcmnfunctions.LogForAudit(
                                   "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                   "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                   "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                   DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [ActionName("GmailAPIDirectly")]
        [HttpGet]
        public async Task<HttpResponseMessage> GmailAPIDirectly()
        {
            try
            {
                if (objdagmailcampaign == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Gmail campaign data access is not initialized.");
                }

                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                if (string.IsNullOrEmpty(token))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Authorization token is missing.");
                }

                var getsessionvalues = objgetgid.gettokenvalues(token);
                if (getsessionvalues == null || string.IsNullOrEmpty(getsessionvalues.user_gid))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid or expired token.");
                }

                MdlGmailCampaign values = new MdlGmailCampaign();
                await objdagmailcampaign.DaGmailAPIDirectly(values, getsessionvalues.user_gid);

                if (values.gmailapiinboxsummary_lists == null || values.gmailapiinboxsummary_lists.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Gmail data found.");
                }

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit(
                    $"*******Date*****{DateTime.Now:yyyy-MM-dd HH:mm:ss}" +
                    $"***********DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    $"***********{ex.Message}" +
                    "*******Apiref********",
                    $"SocialMedia/ErrorLog/Mail/Log{DateTime.Now:yyyy-MM-dd HH}.txt");

                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error occurred while processing your request. Please try again later.");
            }
        }


    [ActionName("GetInboxAttchement")]
        [HttpGet]
        public HttpResponseMessage GetInboxAttchement(string inbox_id)
        {
            MdlGmailCampaign objresult = new MdlGmailCampaign();
            objdagmailcampaign.DaGetInboxAttchement(inbox_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetGmailComments")]
        [HttpGet]
        public HttpResponseMessage GetGmailComments(string inbox_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGetGmailComments(inbox_id, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostGmailComments")]
        [HttpPost]
        public HttpResponseMessage PostGmailComments(gmailcomments_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaPostGmailComments(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatedGmailComments")]
        [HttpPost]
        public HttpResponseMessage UpdatedGmailComments(gmailcomments_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaUpdatedGmailComments(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("deleteGmailComments")]
        [HttpGet]
        public HttpResponseMessage deleteGmailComments(string s_no)
        {
            gmailcomments_list objresult = new gmailcomments_list();
            objdagmailcampaign.DadeleteGmailComments(s_no, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetReplyMail")]
        [HttpGet]
        public HttpResponseMessage GetReplyMail(string inbox_id)
        {
            MdlGmailCampaign objresult = new MdlGmailCampaign();
            objdagmailcampaign.DaGetReplyMail(inbox_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetForwardMail")]
        [HttpGet]
        public HttpResponseMessage GetForwardMail(string inbox_id)
        {
            MdlGmailCampaign objresult = new MdlGmailCampaign();
            objdagmailcampaign.DaGetForwardMail(inbox_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("MovetoTrash")]
        [HttpPost]
        public async Task<HttpResponseMessage> MovetoTrash(gmailmovedlist values)
        {
            try
            {
                await objdagmailcampaign.DaMovetoTrash(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("MoveToInbox")]
        [HttpPost]
        public async Task<HttpResponseMessage> MoveToInbox(gmailmovedlist values)
        {
            try
            {
                await objdagmailcampaign.DaMoveToInbox(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("ReplyInboxMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReplyInboxMail(replymail_list values)
        {
            try
            {
                await objdagmailcampaign.DaReplyInboxMail(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("ReplyOrForwardInboxMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReplyOrForwardInboxMail(forwardmail_list values)
        {
            try
            {
                await objdagmailcampaign.DaReplyOrForwardInboxMail(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("ReplyOrForwardInboxMailWithAttach")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReplyOrForwardInboxMailWithAttach(forwardmail_list values)
        {
            try
            {
                await objdagmailcampaign.DaReplyOrForwardInboxMailWithAttach(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("ForwardOfFwdMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> ForwardOfFwdMail(forwardoffwdmail_list values)
        {
            try
            {
                await objdagmailcampaign.DaForwardOfFwdMail(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("ForwardOfFwdMailWithAttach")]
        [HttpPost]
        public async Task<HttpResponseMessage> ForwardOfFwdMailWithAttach(forwardoffwdmail_list values)
        {
            try
            {
                await objdagmailcampaign.DaForwardOfFwdMailWithAttach(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = "error", message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("ReplyAllWithAttachment")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReplyAllWithAttachment()
        {
            try
            {
                // Get the authorization token from request headers
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                // Assuming you have a service or repository instance for handling Gmail operations
                var service = new GmailService(); // Initialize your Gmail service instance here

                // Get the logged-in user's session values or necessary parameters
                getsessionvalues = objgetgid.gettokenvalues(token);

                // Process the request and prepare the response
                results values = new results();
                HttpRequest httpRequest = HttpContext.Current.Request;
                await objdagmailcampaign.DaReplyAllWithAttachment(httpRequest, values, getsessionvalues.user_gid);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                objcmnfunctions.LogForAudit(
                                 "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                 "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                 "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                 DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [ActionName("GmailInboxStatusUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> GmailInboxStatusUpdate(replymail_list values)
        {
            try
            {
                // Get the authorization token from request headers
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                // Assuming you have a service or repository instance for handling Gmail operations
                var service = new GmailService(); // Initialize your Gmail service instance here

                // Get the logged-in user's session values or necessary parameters
                getsessionvalues = objgetgid.gettokenvalues(token);

                // Process the request and prepare the response
                // MdlGmailCampaign values = new MdlGmailCampaign();

                await objdagmailcampaign.DaGmailInboxStatusUpdate(values);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                objcmnfunctions.LogForAudit(
                                 "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                 "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                 "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                 DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [ActionName("GmailInboxStatusUpdateBack")]
        [HttpPost]
        public async Task<HttpResponseMessage> GmailInboxStatusUpdateBack(replymail_list values)
        {
            try
            {
                // Get the authorization token from request headers
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                // Assuming you have a service or repository instance for handling Gmail operations
                var service = new GmailService(); // Initialize your Gmail service instance here

                // Get the logged-in user's session values or necessary parameters
                getsessionvalues = objgetgid.gettokenvalues(token);

                // Process the request and prepare the response
                // MdlGmailCampaign values = new MdlGmailCampaign();

                await objdagmailcampaign.DaGmailInboxStatusUpdateBack(values, getsessionvalues.user_gid);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                objcmnfunctions.LogForAudit(
                                 "*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                 "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                 "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                                 DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [ActionName("PostcreateAppointments")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostcreateAppointments(appointmentcreations values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            await objdagmailcampaign.DaPostcreateAppointments(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmailLabelDetails")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmailLabelDetails()
        {
            try
            {
                MdlGmailCampaign values = new MdlGmailCampaign();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdagmailcampaign.DaGetEmailLabelDetails(values, getsessionvalues.user_gid);

                return Ok(values);
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return InternalServerError();
            }
        }
        [ActionName("GetMailFolderDetails")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroup()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGetMailFolderDetails(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("CreateEmailLabel")]
        [HttpPost]
        public HttpResponseMessage CreateEmailLabel(createlabel_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objdagmailcampaign.DaCreateEmailLabel(values, getsessionvalues.user_gid);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        [ActionName("UpdateEmailLabel")]
        [HttpPost]
        public HttpResponseMessage UpdateEmailLabel(updatelabel_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objdagmailcampaign.DaUpdateEmailLabel(values, getsessionvalues.user_gid);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        [ActionName("DeleteGmailLabel")]
        [HttpGet]
        public async Task<HttpResponseMessage> DeleteGmailLabel(string label_id)
        {
            try
            {
                updatelabel_list values = new updatelabel_list();
                await objdagmailcampaign.DaDeleteLabelOrFolder(values, label_id);
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit($"Error in DeleteGmailLabel: {ex.Message}", "SocialMedia/ErrorLog/Mail/Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        [ActionName("MovetoFolder")]
        [HttpPost]
        public async Task<HttpResponseMessage> MovetoFolder(gmailfolderlist values)
        {
            try
            {
                await objdagmailcampaign.DaMovetoFolder(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        [ActionName("MoveToFolderFromTrash")]
        [HttpPost]
        public async Task<HttpResponseMessage> MoveToFolderFromTrash(gmailfolderlist values)
        {
            try
            {
                await objdagmailcampaign.DaMoveToFolderFromTrash(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        //[ActionName("ReplyToMessage")]
        //[HttpPost]
        //public async Task<IHttpActionResult> ReplyToMessage()
        //{
        //    try
        //    {
        //        // Retrieve Gmail API credentials and tokens from AppSettings
        //        string clientId = ConfigurationManager.AppSettings["clientId"];
        //        string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
        //        string refreshToken = ConfigurationManager.AppSettings["refreshToken"];

        //        // Initialize Gmail API service
        //        var gmailService = await GetGmailService(clientId, clientSecret, refreshToken);

        //        // Extract form data from the request
        //        var httpRequest = HttpContext.Current.Request;
        //        string replyText = httpRequest.Form["replyText"];
        //        string inboxId = httpRequest.Form["inbox_id"];
        //        string emailBody = httpRequest.Form["emailBody"];
        //        string originalBody = httpRequest.Form["original_body"];

        //        // Handle attachments
        //        var attachments = httpRequest.Files;

        //        // Process each attachment
        //        List<string> savedFileNames = new List<string>();
        //        foreach (string fileName in httpRequest.Files.AllKeys)
        //        {
        //            var file = httpRequest.Files[fileName];
        //            if (file != null && file.ContentLength > 0)
        //            {
        //                // Save the file to specified directory
        //                string saveFilePath = ConfigurationManager.AppSettings["filepath"];
        //                string savePath = Path.Combine(saveFilePath, file.FileName);
        //                file.SaveAs(savePath);
        //                savedFileNames.Add(file.FileName);
        //            }
        //        }

        //        // Construct the reply message body
        //        string formattedReplyBody = $"{emailBody}<br><br> On {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}, wrote:<br><br>{originalBody}";

        //        // Construct the raw message content for Gmail API
        //        string rawMessageContent = $"To: {originalFrom}\r\n";
        //        rawMessageContent += $"Subject: Re: {originalSubject}\r\n";
        //        rawMessageContent += $"Content-Type: text/html; charset=UTF-8\r\n\r\n";
        //        rawMessageContent += formattedReplyBody;

        //        // Encode the raw message content using base64url encoding
        //        string encodedRawMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawMessageContent))
        //                                           .Replace('+', '-')
        //                                           .Replace('/', '_')
        //                                           .Replace("=", "");

        //        // Create the Gmail message
        //        var replyMessage = new Message
        //        {
        //            Raw = encodedRawMessageContent
        //        };

        //        // Send the reply message using Gmail API
        //        var sendMessageRequest = gmailService.Users.Messages.Send(replyMessage, "me");
        //        var sendMessageResponse = await sendMessageRequest.ExecuteAsync();

        //        // Check if message was sent successfully
        //        if (sendMessageResponse != null && sendMessageResponse.LabelIds.Contains("SENT"))
        //        {
        //            // Optionally, save information to your database
        //            SaveReplyToDatabase(inboxId, sendMessageResponse.Id, originalFrom, originalSubject, formattedReplyBody, savedFileNames);

        //            // Return success response
        //            return Ok(new { Message = "Reply sent successfully", SavedAttachments = savedFileNames });
        //        }
        //        else
        //        {
        //            // Handle failure to send reply
        //            return BadRequest("Failed to send reply.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        return InternalServerError(ex);
        //    }
        //}
        [ActionName("GmailAddaslead")]
        [HttpPost]
        public HttpResponseMessage GmailAddaslead(mdlAddaslead values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objdagmailcampaign.DaGmailAddaslead(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }


        [ActionName("GmailView")]
        [HttpGet]
        public HttpResponseMessage GmailView(string gmail_gid)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGmailView(gmail_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Get360Gmailsummary")]
        [HttpGet]
        public HttpResponseMessage Get360Gmailsummary(string leadbank_gid)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGet360Gmailsummary(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeaddropdown")]
        [HttpGet]
        public HttpResponseMessage GetLeaddropdown()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGetLeaddropdown(getsessionvalues.user_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //code by snehith
        [ActionName("GetInboxCustomerDetails")]
        [HttpGet]
        public HttpResponseMessage GetInboxCustomerDetails(string email_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGetInboxCustomerDetails(email_id, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCustomerAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetCustomerAssignedlist(string inbox_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGetCustomerAssignedlist(inbox_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCustomerUnAssignedlist")]
        [HttpGet]
        public HttpResponseMessage GetCustomerUnAssignedlist(string inbox_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaGetCustomerUnAssignedlist(inbox_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("TagCustomertoGmail")]
        [HttpPost]
        public HttpResponseMessage TagCustomertoGmail(tagcustomertogmail values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaTagCustomertoGmail(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("UnTagCustomertoGmail")]
        [HttpPost]
        public HttpResponseMessage UnTagCustomertoGmail(untagcustomertogmail values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdagmailcampaign.DaUnTagCustomertoGmail(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [ActionName("MoveoneFoldertoother")]
        [HttpPost]
        public async Task<HttpResponseMessage> MoveoneFoldertoother(gmailoneFoldertoother_list values)
        {
            try
            {
                await objdagmailcampaign.DaMoveoneFoldertoother(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        [ActionName("TrashDeleteMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> TrashDeleteMail(gmailmovedlist values)
        {
            try
            {
                await objdagmailcampaign.DaTrashDeleteMail(values);

                // Use the status and message from the data access method
                if (values.status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = values.status, message = values.message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { status = values.status, message = values.message });
                }
            }
            catch (Exception ex)
            {
                // Log error
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
        [ActionName("GetEmailId")]
        [HttpGet]
        public HttpResponseMessage GetEmailId()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objdagmailcampaign.DaGetEmailId(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }

}