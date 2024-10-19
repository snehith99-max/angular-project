using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmtrnExitRequisition")]

    public class HrmtrnExitRequisitionController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnExitRequisition objDaHrmTrnExitRequisition = new DaHrmTrnExitRequisition();


       [ActionName("GetExitRequisitionSummary")]
        [HttpGet]
       public HttpResponseMessage GetExitRequisitionSummary()
       {
            MdlHrmtrnExitRequisition values = new MdlHrmtrnExitRequisition();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnExitRequisition.DaGetExitRequisitionSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
       }


        [ActionName("GetExitEmployee")]
        [HttpGet]
        public HttpResponseMessage GetExitEmployee()
        {
            MdlHrmtrnExitRequisition values = new MdlHrmtrnExitRequisition();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnExitRequisition.DaGetExitEmployee(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostExitRequisition")]
        [HttpPost]
        public HttpResponseMessage PostExitRequisition(exitrequisition_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnExitRequisition.DaPostExitRequisition(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}