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
    [RoutePrefix("api/ImsTrnIssueMaterial")]
    [Authorize]
    public class ImsTrnIssueMaterialController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnIssueMaterial objInventory = new DaImsTrnIssueMaterial();

        [ActionName("GetIssueMaterialSummary")]
        [HttpGet]
        public HttpResponseMessage GetIssueMaterialSummary()
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            objInventory.DaGetImsTrnIssueMaterial(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetmaterialissueRpt")]
        [HttpGet]
        public HttpResponseMessage GetmaterialissueRpt(string materialissued_gid)
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            var ls_response = new Dictionary<string, object>();
            ls_response = objInventory.DaGetmaterialissueRpt(getsessionvalues.branch_gid, materialissued_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("GetViewIssueMaterial")]
        [HttpGet]
        public HttpResponseMessage GetViewIssueMaterial(string reference_gid)
        {
            MdlImsTrnIssueMaterial objresult = new MdlImsTrnIssueMaterial();
            objInventory.DaGetViewIssueMaterial(reference_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetViewIssueMaterialSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewIssueMaterialSummary(string reference_gid)
        {
            MdlImsTrnIssueMaterial objresult = new MdlImsTrnIssueMaterial();
            objInventory.DaGetViewIssueMaterialSummary(reference_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetIssueMaterialselect")]
        [HttpGet]
        public HttpResponseMessage GetIssueMaterialselect()
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objInventory.DaGetIssueMaterialselect(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetIssueViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetIssueViewProduct(string materialissued_gid)
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            objInventory.DaGetIssueViewProduct(materialissued_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetDetialViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetDetialViewProduct(string materialrequisition_gid)
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            objInventory.DaGetDetialViewProduct(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        //---------------------------  matrerial issue -----------------------------------//

        [ActionName("GetMIissuedetails")]
        [HttpGet]
        public HttpResponseMessage GetMIissuedetails(string materialrequisition_gid)
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            objInventory.DaGetMIissuedetails(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetMIissuedetailproduct")]
        [HttpGet]
        public HttpResponseMessage GetMIissuedetailproduct(string materialrequisition_gid)
        {
            MdlImsTrnIssueMaterial values = new MdlImsTrnIssueMaterial();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objInventory.DaGetMIissuedetailproduct(getsessionvalues.branch_gid, materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postissue")]
        [HttpPost]
        public HttpResponseMessage Postissue(Postissuemetrial values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objInventory.DaPostissue(getsessionvalues.user_gid, getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
    }