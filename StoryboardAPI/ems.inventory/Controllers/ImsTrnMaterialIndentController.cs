using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
namespace ems.inventory.Controllers
{

    [RoutePrefix("ImsTrnMaterialIndent")]
    [Authorize]
    public class ImsTrnMaterialIndentController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaIMSTrnMaterialindent objIndentmaterial = new DaIMSTrnMaterialindent();

        [ActionName("MatrialIndentsummary")]
        [HttpGet]

        public HttpResponseMessage MatrialIndentsummary()
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaMatrialIndentsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("MatrialIndentApprovalsummary")]
        [HttpGet]

        public HttpResponseMessage MatrialIndentApprovalsummary()
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaMatrialIndentApprovalsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetMaterialIndentView")]
        [HttpGet]
        public HttpResponseMessage GetMaterialIndentView(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetMaterialIndentView(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetMaterialIndentViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetMaterialIndentViewProduct(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetMaterialIndentViewProduct(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetmaterialindentRpt")]
        [HttpGet]
        public HttpResponseMessage GetmaterialindentRpt(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            var ls_response = new Dictionary<string, object>();
            ls_response = objIndentmaterial.DaGetmaterialindentRpt(getsessionvalues.branch_gid, materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("GetMIrequest")]
        [HttpGet]
        public HttpResponseMessage GetMIrequest(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetMIrequest(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetproductMIrequest")]
        [HttpGet]
        public HttpResponseMessage GetproductMIrequest(string materialrequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetproductMIrequest(getsessionvalues.branch_gid, materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("MIrequest")]
        [HttpPost]
        public HttpResponseMessage MIrequest(issuematerialrequest_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objIndentmaterial.DaMIrequest(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMIViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetMIViewProduct(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetMIViewProduct(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetMitoPO")]
        [HttpGet]
        public HttpResponseMessage GetMitoPO(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetMitoPO(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetPRtoPayment")]
        [HttpGet]
        public HttpResponseMessage GetPRtoPayment(string materialrequisition_gid)
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetPRtoPayment(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("MaterialIndentApprove")]
        [HttpGet]
        public HttpResponseMessage MaterialIndentApprove(string materialrequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaMaterialIndentApprove(materialrequisition_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("MaterialIndentReject")]
        [HttpGet]
        public HttpResponseMessage MaterialIndentReject(string materialrequisition_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaMaterialIndentReject(materialrequisition_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //GetMICount

        [ActionName("GetMICount")]
        [HttpGet]
        public HttpResponseMessage GetMICount()
        {
            MdlImsTrnMaterialindent values = new MdlImsTrnMaterialindent();
            objIndentmaterial.DaGetMICount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}