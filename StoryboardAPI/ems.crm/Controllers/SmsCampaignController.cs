using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmsCampaign")]
    public class SmsCampaignController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmsCampaign objDasmscmapaign = new DaSmsCampaign();
        // code By snehith
        [ActionName("GetSmsCampaignCount")]
        [HttpGet]
        public HttpResponseMessage GetSmsCampaignCount()
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaGetSmsCampaignCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // code By snehith
        [ActionName("GetSmsCampaign")]
        [HttpGet]
        public HttpResponseMessage GetSmsCampaign()
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaGetSmsCampaign(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSmsCampaign")]
        [HttpPost]
        public HttpResponseMessage PostSmsCampaign(smspostcampaign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasmscmapaign.DaPostSmsCampaign(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateSmsCampaign")]
        [HttpPost]
        public HttpResponseMessage UpdateSmsCampaign(smspostcampaign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasmscmapaign.DaUpdateSmsCampaign(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteSmsCampaign")]
        [HttpGet]
        public HttpResponseMessage DeleteSmsCampaign(string template_id)
        {
            smspostcampaign_list objresult = new smspostcampaign_list();
            objDasmscmapaign.DaDeleteSmsCampaign(template_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("SmsLeadCustomerDetails")]
        [HttpGet]
        public HttpResponseMessage SmsLeadCustomerDetails()
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaSmsLeadCustomerDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("smstemplatesendsummarylist")]
        [HttpPost]
        public HttpResponseMessage smstemplatesendsummarylist(smstemplatesendsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objresult = new result();
            objresult = objDasmscmapaign.Dasmstemplatesendsummarylist(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getsmscampaignlog")]
        [HttpGet]
        public HttpResponseMessage Getsmscampaignlog(string template_id)
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaGetsmscampaignlog(values, template_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getindividuallog")]
        [HttpGet]
        public HttpResponseMessage Getindividuallog(string phone_number)
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaGetindividuallog(values, phone_number);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("smstemplatepreview")]
        [HttpGet]
        public HttpResponseMessage smstemplatepreview(string template_id)
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.Dasmstemplatepreview(values, template_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}