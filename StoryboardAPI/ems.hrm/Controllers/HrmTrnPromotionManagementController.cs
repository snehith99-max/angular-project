using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmTrnPromotionManagement")]
    public class HrmTrnPromotionManagementController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnPromotionManagement objDaDaHrmTrnPromotionManagement = new DaHrmTrnPromotionManagement();

        [ActionName("GetPromotionSummary")]
        [HttpGet]
        public HttpResponseMessage GetPromotionSummary()
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetPromotionSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeNameDtl()
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetEmployeeNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchNameDtl()
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetBranchNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDepartmentNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentNameDtl()
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetDepartmentNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDesignationNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetDesignationNameDtl()
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetDesignationNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeDetail")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDetail(string employee_gid)
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetEmployeeDetail(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostPromotion")]
        [HttpPost]
        public HttpResponseMessage PostPromotion(Promotionsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaDaHrmTrnPromotionManagement.DaPostPromotion(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeletePromotion")]
        [HttpGet]
        public HttpResponseMessage DeletePromotion(string params_gid)
        {
            Promotionsummary_list values = new Promotionsummary_list();
            objDaDaHrmTrnPromotionManagement.DaDeletePromotion(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPromotionHistorySummary")]
        [HttpGet]
        public HttpResponseMessage GetPromotionHistorySummary(string employee_gid)
        {
            MdlHrmTrnPromotionManagement values = new MdlHrmTrnPromotionManagement();
            objDaDaHrmTrnPromotionManagement.DaGetPromotionHistorySummary(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}