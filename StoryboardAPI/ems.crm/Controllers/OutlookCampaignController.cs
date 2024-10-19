using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Web;
using System.Web.Http.Results;
using System.Threading.Tasks;
using System;
using Google.Apis.Gmail.v1;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/OutlookCampaign")]
    public class OutlookCampaignController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOutlookCampaign objdaoutlookcampaign = new DaOutlookCampaign();
        cmnfunctions objcmnfunctions = new cmnfunctions();

        [ActionName("PostOutlookTemplate")]
        [HttpPost]
        public HttpResponseMessage PostOutlookTemplate(outlooktemplate_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaPostOutlookTemplate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutlookTemplateSummary")]
        [HttpGet]
        public HttpResponseMessage OutlookTemplateSummary()
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            objdaoutlookcampaign.DaOutlookTemplateSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostCampaignStatus")]
        [HttpPost]
        public HttpResponseMessage PostCampaignStatus(outlooktemplatesummary_list values)
        {
            objdaoutlookcampaign.DaPostCampaignStatus(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ComposeOutlookMail")]
        [HttpGet]
        public HttpResponseMessage ComposeOutlookMail()
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaComposeOutlookMail(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SendOutlookMail")]
        [HttpPost]
        public HttpResponseMessage SendOutlookMail(MdlSendMail values)
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objresult = objdaoutlookcampaign.DaSendOutlookMail(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("SendOutlookMailwithfiles")]
        [HttpPost]
        public HttpResponseMessage SendOutlookMailwithfiles()
        {

            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objresult = new result();
            objresult = objdaoutlookcampaign.DaSendOutlookMailwithfiles(httpRequest, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("OutlookMailSentSummary")]
        [HttpGet]
        public HttpResponseMessage OutlookMailSentSummary()
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            objdaoutlookcampaign.DaOutlookMailSentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SendOutlookTemplate")]
        [HttpPost]
        public HttpResponseMessage SendOutlookTemplate(outlookmailtemplatesendsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objresult = new result();
            objresult = objdaoutlookcampaign.DaSendOutlookTemplate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("SendTemplatePreview")]
        [HttpGet]
        public HttpResponseMessage SendTemplatePreview(string template_gid)
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            objdaoutlookcampaign.DaSendTemplatePreview(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutlookCampaignSentSummary")]
        [HttpGet]
        public HttpResponseMessage OutlookCampaignSentSummary(string template_gid)
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            objdaoutlookcampaign.DaOutlookCampaignSentSummary(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("RecipientMailList")]
        [HttpGet]
        public HttpResponseMessage RecipientMailList()
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            objdaoutlookcampaign.DaRecipientMailList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        ///code by snehith for outlook inbox
        [ActionName("ReadEmailsOutlookmail")]
        [HttpGet]
        public async Task<HttpResponseMessage> ReadEmailsOutlookmail()
        {
            try
            {
                get values = new get();

                // Ensure objdagmailcampaign is initialized
                if (objdaoutlookcampaign != null)
                {
                    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                    getsessionvalues = objgetgid.gettokenvalues(token);
                    values = await objdaoutlookcampaign.DaReadEmailsOutlookmail(getsessionvalues.user_gid);
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
        [ActionName("GmailAPIOutlookinboxSummary")]
        [HttpGet]
        public HttpResponseMessage GmailAPIOutlookinboxSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaGmailAPIOutlookinboxSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOutlookInboxAttchement")]
        [HttpGet]
        public HttpResponseMessage GetOutlookInboxAttchement(string inbox_id)
        {
            MdlGmailCampaign objresult = new MdlGmailCampaign();
            objdaoutlookcampaign.DaGetOutlookInboxAttchement(inbox_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("OutlookGetReplyMail")]
        [HttpGet]
        public HttpResponseMessage OutlookGetReplyMail(string inbox_id)
        {
            MdlGmailCampaign objresult = new MdlGmailCampaign();
            objdaoutlookcampaign.DaOutlookGetReplyMail(inbox_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetOutlookForwardMail")]
        [HttpGet]
        public HttpResponseMessage GetOutlookForwardMail(string inbox_id)
        {
            MdlGmailCampaign objresult = new MdlGmailCampaign();
            objdaoutlookcampaign.DaGetOutlookForwardMail(inbox_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetOutlookInboxCustomerDetails")]
        [HttpGet]
        public HttpResponseMessage GetOutlookInboxCustomerDetails(string email_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaGetOutlookInboxCustomerDetails(email_id, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("OutlookInboxStatusUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookInboxStatusUpdate(replymail_list values)
        {
            try
            {

                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookInboxStatusUpdate(values, getsessionvalues.user_gid);

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

        [ActionName("OutlookInboxStatusUpdateBack")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookInboxStatusUpdateBack(replymail_list values)
        {
            try
            {

                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookInboxStatusUpdateBack(values, getsessionvalues.user_gid);

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

        [ActionName("OutlookReplyInboxMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookReplyInboxMail(replymail_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookReplyInboxMail(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookReplyAllWithAttachment")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookReplyAllWithAttachment()
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
                await objdaoutlookcampaign.DaOutlookReplyAllWithAttachment(httpRequest, values, getsessionvalues.user_gid);

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
        [ActionName("OutlookReplyOrForwardInboxMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookReplyOrForwardInboxMail(forwardmail_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookReplyOrForwardInboxMail(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookReplyOrForwardInboxMailWithAttach")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookReplyOrForwardInboxMailWithAttach(forwardmail_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookReplyOrForwardInboxMailWithAttach(values, getsessionvalues.user_gid);

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

        [ActionName("OutlookForwardOfFwdMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookForwardOfFwdMail(forwardoffwdmail_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookForwardOfFwdMail(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookForwardOfFwdMailWithAttach")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookForwardOfFwdMailWithAttach(forwardoffwdmail_list values)
        {
            try
            {

                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();

                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookForwardOfFwdMailWithAttach(values, getsessionvalues.user_gid);

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
        [ActionName("GetOutlookFolderDetails")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOutlookFolderDetails()
        {
            try
            {
                MdlOutlookCampaign values = new MdlOutlookCampaign();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaGetOutlookFolderDetails(values, getsessionvalues.user_gid);

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
        [ActionName("GetOutlookMailFolder")]
        [HttpGet]
        public HttpResponseMessage GetOutlookMailFolder()
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaGetOutlookMailFolder(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutlookAPIinboxFolderSummary")]
        [HttpGet]
        public HttpResponseMessage OutlookAPIinboxFolderSummary(string label_id)
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaOutlookAPIinboxFolderSummary(values, label_id, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("OutlookAPIinboxTrashSummary")]
        [HttpGet]
        public HttpResponseMessage OutlookAPIinboxTrashSummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaOutlookAPIinboxTrashSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("OutlookMovetoFolder")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookMovetoFolder(gmailfolderlist values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookMovetoFolder(values, getsessionvalues.user_gid);

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


        [ActionName("OutlookMovetoTrash")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookMovetoTrash(gmailmovedlist values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookMovetoTrash(values, getsessionvalues.user_gid);

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
        [ActionName("OutlooMoveToInbox")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlooMoveToInbox(gmailmovedlist values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlooMoveToInbox(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookMoveToFolderFromTrash")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookMoveToFolderFromTrash(gmailfolderlist values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookMoveToFolderFromTrash(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookTrashDeleteMail")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookTrashDeleteMail(gmailmovedlist values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookTrashDeleteMail(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookCreateEmailLabel")]
        [HttpPost]
        public async Task<HttpResponseMessage> OutlookCreateEmailLabel(createlabel_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookCreateEmailLabel(values, getsessionvalues.user_gid);

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
        [ActionName("OutlookUpdateEmailLabel")]
        [HttpPost]
        public HttpResponseMessage OutlookUpdateEmailLabel(updatelabel_list values)
        {
            try
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                 objdaoutlookcampaign.DaOutlookUpdateEmailLabel(values, getsessionvalues.user_gid);
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
        [ActionName("OutlookDeleteLabelOrFolder")]
        [HttpGet]
        public async Task<HttpResponseMessage> OutlookDeleteLabelOrFolder(string label_id)
        {
            try
            {
                updatelabel_list values = new updatelabel_list();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                await objdaoutlookcampaign.DaOutlookDeleteLabelOrFolder(values, label_id, getsessionvalues.user_gid);
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
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                    "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [ActionName("OutlooklSenditemSummary")]
        [HttpGet]
        public HttpResponseMessage OutlooklSenditemSummary()
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaOutlooklSenditemSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutlooklindividualSenditemSummary")]
        [HttpGet]
        public HttpResponseMessage OutlooklindividualSenditemSummary(string leadbank_gid)
        {
            MdlOutlookCampaign values = new MdlOutlookCampaign();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaOutlooklindividualSenditemSummary(values, leadbank_gid, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostcreateAppointment")]
        [HttpPost]
        public HttpResponseMessage PostcreateAppointment(appointmentcreation values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaoutlookcampaign.DaPostcreateAppointment(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OutlookAPIDirectly")]
        [HttpGet]
        public async Task<HttpResponseMessage> GmailAPIDirectly()
        {
            try
            {
                if (objdaoutlookcampaign == null)
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
                await objdaoutlookcampaign.DaOutlookAPIDirectly(values, getsessionvalues.user_gid);

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
        [ActionName("PostOutlookcreateAppointments")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostOutlookcreateAppointments(appointmentcreations values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            await objdaoutlookcampaign.DaPostOutlookcreateAppointments(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}