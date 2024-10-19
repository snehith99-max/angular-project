using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ImsTrnPendingMaterialIssue")]
    [Authorize]
    public class ImsTrnPendingMaterialIssueController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnPendingMaterialIssue objDaInventory = new DaImsTrnPendingMaterialIssue();
        [ActionName("GetPendingMaterialIssueSummary")]
        [HttpGet]
        public HttpResponseMessage GetPendingMaterialIssueSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlImsTrnPendingMaterialIssue values = new MdlImsTrnPendingMaterialIssue();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetPendingMaterialIssueSummary(getsessionvalues.user_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetRaiseMaterialIndent")]
        [HttpGet]
        public HttpResponseMessage GetRaiseMaterialIndent(string materialrequisition_gid)
        {
            MdlImsTrnPendingMaterialIssue values = new MdlImsTrnPendingMaterialIssue();
            objDaInventory.DaGetRaiseMaterialIndent(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRaiseMaterialIndentProduct")]
        [HttpGet]
        public HttpResponseMessage GetRaiseMaterialIndentProduct(string materialrequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlImsTrnPendingMaterialIssue values = new MdlImsTrnPendingMaterialIssue();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetRaiseMaterialIndentProduct(getsessionvalues.branch_gid, materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostRaisePRSubmit")]
        [HttpPost]
        public HttpResponseMessage PostRaisePRSubmit( raisepr_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostRaisePRSubmit(getsessionvalues.user_gid ,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDetailPopup")]
        [HttpGet]
        public HttpResponseMessage GetDetailPopup(string materialrequisition_gid)
        {   
            MdlImsTrnPendingMaterialIssue values = new MdlImsTrnPendingMaterialIssue();
            objDaInventory.DaGetDetailPopup( materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}
