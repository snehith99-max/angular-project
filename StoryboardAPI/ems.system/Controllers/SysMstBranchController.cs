using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;

namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/SysMstBranch")]
    public class SysMstBranchController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysMstBranch objDaBranch = new DaSysMstBranch();

        [ActionName("BranchSummary")]
        [HttpGet]
        public HttpResponseMessage BranchSummary()
        {
            MdlSysMstBranch values = new MdlSysMstBranch();
            objDaBranch.DaBranchSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostBranch")]
        [HttpPost]
        public HttpResponseMessage PostBranch(branch_list1 values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objDaBranch.DaPostBranch(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("getUpdatedBranch")]
        //[HttpPost]
        //public HttpResponseMessage getUpdatedBranch(string user_gid, branch_list1 values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    objDaBranch.DagetUpdatedBranch(user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("getUpdatedBranch")]
        [HttpPost]
        public HttpResponseMessage getUpdatedBranch(branch_list1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaBranch.DagetUpdatedBranch(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetbranchActive")]
        [HttpGet]
        public HttpResponseMessage GetbranchActive(string params_gid)
        {
            MdlSysMstBranch objresult = new MdlSysMstBranch();
            objDaBranch.DaBranchActive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetbranchInactive")]
        [HttpGet]
        public HttpResponseMessage GetbranchInactive(string params_gid)
        {
            MdlSysMstBranch objresult = new MdlSysMstBranch();
            objDaBranch.DaBranchInactive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("DeleteBranch")]
        [HttpGet]
        public HttpResponseMessage DeleteBranch(string params_gid)
        {
            branch_list1 objresult = new branch_list1();
            objDaBranch.DaDeleteBranch(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("BranchSummarydetail")]
        [HttpPost]
        public HttpResponseMessage BranchSummarydetail(branch_list1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaBranch.DaBranchSummarydetail(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Updatedbranchlogo")]
        [HttpPost]
        public HttpResponseMessage Updatedbranchlogo()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            httpRequest = HttpContext.Current.Request;
            result values = new result();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaBranch.DaUpdatedbranchlogo(httpRequest, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PopupBranch")]
        [HttpGet]
        public HttpResponseMessage PopupBranch(string branch_gid)
        {
            MdlSysMstBranch values = new MdlSysMstBranch();
            objDaBranch.DaPopupBranch(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}