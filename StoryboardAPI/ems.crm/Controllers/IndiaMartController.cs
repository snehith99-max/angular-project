using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Linq;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/IndiaMART")]  
    public class IndiaMartController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaIndiaMart objDaIndiaMart = new DaIndiaMart();

        [ActionName("Getindiamartsummary")]
        [HttpGet]
        public HttpResponseMessage Getindiamartsummary()
        {
            MdlIndiaMart values = new MdlIndiaMart();
            objDaIndiaMart.DaGetindiamartsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getsyncdetails")]
        [HttpGet]
        public HttpResponseMessage Getsyncdetails()
        {
            MdlIndiaMart values = new MdlIndiaMart();
            objDaIndiaMart.DaGetsyncdetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetindiamartviewSummary")]
        [HttpGet]
        public HttpResponseMessage GetleadbankviewSummary(string unique_query_id)
        {
            indiamartview_list objresult = new indiamartview_list();
            objDaIndiaMart.DaGetindiamartviewSummary(unique_query_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
       
        [ActionName("LoadLeadsFromIndiaMart")]
        [HttpGet]
        public HttpResponseMessage LoadLeadsFromIndiaMart()
        {
            MdlIndiamartResponse objresult = objDaIndiaMart.DaLoadLeadsFromIndiaMart();
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("SyncDetails")]
        [HttpGet]
        public HttpResponseMessage SyncDetails()
        {
            MdlSyncDetails values = new MdlSyncDetails();
            objDaIndiaMart.DaSyncDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAddtoLeadBank")]
        [HttpPost]
        public HttpResponseMessage PostAddtoLeadBank(mdlAddasleadtolead values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaIndiaMart.DaPostAddtoLeadBank(values,getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("markAsUnread")]
        [HttpGet]
        public HttpResponseMessage markAsUnread(string unique_query_id)
        {
            result objresult = new result();
            objDaIndiaMart.DaMarkAsUnread(unique_query_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}