using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.outlet.Dataaccess;
using ems.outlet.Models;
using System.Linq;


namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("OtlMstBranch")]
    public class OtlMstBranchController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOtlMstBranch objbranch = new DaOtlMstBranch();

        // Outlet Summary
        [ActionName("Getoutletsummary")]
        [HttpGet]
        public HttpResponseMessage Getoutletsummary()
        {
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.Daoutletsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Summary
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.DaGetProductSummary(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Assign Product Summary
        [ActionName("GetAssignProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssignProductSummary(string branch_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.DaGetAssignProductSummary(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Assign Product
        [ActionName("postassignedlist")]
        [HttpPost]
        public HttpResponseMessage postassignedlist(assign_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaPostAssignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // UnAssign Product Summary
        [ActionName("GetUnAssignProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetUnAssignProductSummary(string campaign_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.DaGetUnAssignProductSummary(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        
        [ActionName("PostUnAssignedlist")]
        [HttpPost]
        public HttpResponseMessage postunassignedlist(assign_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaPostUnAssignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostChangePrice")]
        [HttpPost]
        public HttpResponseMessage PostChangePrice(assign_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaPostChangePrice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        
        [ActionName("GetBranchDetails")]
        [HttpGet]
        public HttpResponseMessage GetBranchDetails(string campaign_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.DaGetBranchDetails(campaign_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Update Number
        [ActionName("PostNumberUpdate")]
        [HttpPost]
        public HttpResponseMessage PostNumberUpdate(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaPostNumberUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssignedPincodeSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssignedPincodeSummary(string branch_gid)
        {
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.DaGetAssignedPincodeSummary(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPincodeSummaryAssign")]
        [HttpGet]
        public HttpResponseMessage GetPincodeSummaryAssign(string branch_gid)
        {
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaGetPincodeSummaryAssign(branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPincodeassing")]
        [HttpPost]
        public HttpResponseMessage PostPincodeassing(PostPincode_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaPostPincodeassing(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("RemovePincode")]
        [HttpPost]
        public HttpResponseMessage RemovePincode(PostRemovePincode_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objbranch.DaRemovePincode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAmendProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetAmendProductSummary(string branch_gid)
        {
            MdlOtlMstBranch values = new MdlOtlMstBranch();
            objbranch.DaGetAmendProductSummary(branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}