using ems.mobile.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.mobile.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.mobile.Controllers
{
    [RoutePrefix("api/MblTicketSummary")]
    [AllowAnonymous]
    public class MblTicketSummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaTicketSummary objDaMobileDasboard = new DaTicketSummary();


        [ActionName("GetTicketSummary")]   
        [HttpGet]
        public HttpResponseMessage GetTicketSummary(string user_code)
        {
            MdlTicketSummary values = new MdlTicketSummary();
            objDaMobileDasboard.DaGetTicketSummary(user_code,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTicketCount")]
        [HttpGet]
        public HttpResponseMessage GetTicketCount(string user_code)
        {
            MdlTicketSummary values = new MdlTicketSummary();
            objDaMobileDasboard.DaGetTicketCount(user_code, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTicketSummaryDetailView")]
        [HttpGet]
        public HttpResponseMessage GetTicketSummaryDetailView(string complaint_gid)
        {
            MdlTicketSummary values = new MdlTicketSummary();
            objDaMobileDasboard.DaGetTicketSummaryDetailView(complaint_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTicketSummaryManagerAssign")]
        [HttpGet]
        public HttpResponseMessage GetTicketSummaryManagerAssign(string complaint_gid)
        {
            MdlTicketSummary values = new MdlTicketSummary();
            objDaMobileDasboard.DaGetTicketSummaryManagerAssign(complaint_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



    }
}