using ems.hrm.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.hrm.Models;
using static OfficeOpenXml.ExcelErrorValue;
using RestSharp;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HrmTrnAppraisalManagement")]
    [Authorize]
    public class HrmTrnAppraisalManagementController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnAppraisalManagement objDaHrmTrnAppraisalManagement = new DaHrmTrnAppraisalManagement();

        [ActionName("GetEmployeeDetail")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDetail()
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            objDaHrmTrnAppraisalManagement.DaGetEmployeeDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAppraisalSummary")]
        [HttpGet]
        public HttpResponseMessage GetAppraisalSummary()
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            objDaHrmTrnAppraisalManagement.DaGetAppraisalSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UserList")]
        [HttpGet]
        public HttpResponseMessage UserList(string user_gid)
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
          
            objDaHrmTrnAppraisalManagement.DaUserList(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetPeerDetail")]
        [HttpGet]
        public HttpResponseMessage GetPeerDetail()
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnAppraisalManagement.DaGetPeerDetail(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetManagerDetail")]
        [HttpGet]
        public HttpResponseMessage GetManagerDetail()
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnAppraisalManagement.DaGetManagerDetail(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetManagementDetail")]
        [HttpGet]
        public HttpResponseMessage GetManagementDetail()
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnAppraisalManagement.DaGetManagementDetail(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostReview")]
        [HttpPost]
        public HttpResponseMessage PostReview(review_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnAppraisalManagement.DaPostReview(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

      

        [ActionName("getViewReviewSummary")]
        [HttpGet]
        public HttpResponseMessage getViewReviewSummary(string user_gid)
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            objDaHrmTrnAppraisalManagement.DagetViewReviewSummary(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostAppraisalDtl")]
        [HttpPost]
        public HttpResponseMessage PostAppraisalDtl(appraisaldtl_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnAppraisalManagement.DaPostAppraisalDtl(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAppraisalDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetAppraisalDetailSummary(string user_gid)
        {
            MdlHrmTrnAppraisalManagement values = new MdlHrmTrnAppraisalManagement();
            objDaHrmTrnAppraisalManagement.DaGetAppraisalDetailSummary(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteAppraisalDetail")]
        [HttpGet]
        public HttpResponseMessage DeleteAppraisalDetail(string params_gid)
        {
            appraisaldtl_list values = new appraisaldtl_list();
            objDaHrmTrnAppraisalManagement.DaDeleteAppraisalDetail(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}