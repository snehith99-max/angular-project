using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/ConstitutionMaster")]
    public class ConstitutionMasterController : ApiController
    {
        DaConstitution objdaconst = new DaConstitution();
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();

        [ActionName("GetConstitutionSummary")]

        [HttpGet]
        public HttpResponseMessage GetConstitutionSummary()
        {
            MdlConstitution values = new MdlConstitution();
            objdaconst.DaConstitutionSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ConstitutionAdd")]
        [HttpPost]
        public HttpResponseMessage ConstitutionAdd(constitution_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdaconst.DaConstitutionAdd(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ConstitutionDelete")]
        [HttpGet]
        public HttpResponseMessage ConstitutionDelete(string constitution_gid)
        {
            constitution_list values = new constitution_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdaconst.DaConstitutionDelete(constitution_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetConstitutionEdit")]
        [HttpGet]
        public HttpResponseMessage GetConstitutionEdit(string constitution_gid)
        {
            constitution_list values = new constitution_list();
            objdaconst.DaGetConstitutionEdit(constitution_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateGetConstitution")]
        [HttpPost]
        public HttpResponseMessage UpdateGetConstitution(constitution_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdaconst.DaUpdateConstitution(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("InactiveConstitution")]
        [HttpPost]
        public HttpResponseMessage InactiveConstitution(Constitutionstatus values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdaconst.DaInactiveConstitution(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ConstitutionInactiveHistory")]
        [HttpGet]
        public HttpResponseMessage ConstitutionInactiveHistory(string constitution_gid)
        {
            ConstitutionInactiveHistory objapplicationhistory = new ConstitutionInactiveHistory();
            objdaconst.DaConstitutionInactiveHistory(objapplicationhistory, constitution_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objapplicationhistory);
        }
        [ActionName("constitutionstatusupdate")]
        [HttpPost]
        public HttpResponseMessage constitutionstatusupdate(mdConstitutionstatus values)
        {

            objdaconst.Daconstitutionstatusupdate(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
