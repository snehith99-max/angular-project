using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HrmTrnLeaveManage")]
    [Authorize]
    public class HrmTrnLeaveManageController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnLeaveManage objDaHrmTrnLeaveManage = new DaHrmTrnLeaveManage();

        [ActionName("GetLeaveManageSummary")]
        [HttpGet]
        public HttpResponseMessage GetLeaveManageSummary()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetLeaveManageSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPermissionSummary")]
        [HttpGet]
        public HttpResponseMessage GetPermissionSummary()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetPermissionSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnDutySummary")]
        [HttpGet]
        public HttpResponseMessage GetOnDutySummary()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetOnDutySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDepartmentDtl")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentDtl()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetDepartmentDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeeDtl")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDtl()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetEmployeeDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDateOfJoin")]
        [HttpGet]
        public HttpResponseMessage GetDateOfJoin(string employee_gid)
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetDateOfJoin(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLeaveAvailableDtl")]
        [HttpGet]
        public HttpResponseMessage GetLeaveAvailableDtl()
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetLeaveAvailableDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLeavebalance")]
        [HttpGet]
        public HttpResponseMessage DaGetLeavebalance(string employee_gid)
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetLeavebalance(values,employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLeaveManage")]
        [HttpGet]
        public HttpResponseMessage GetLeaveManage(string branch_gid, string department_gid, string leavetype)
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaGetLeaveManage(branch_gid, department_gid, leavetype, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //[ActionName("GetLeaveManage")]
        //[HttpGet]
        //public HttpResponseMessage GetLeaveManage(string branch, string department, string fromdate, string leavetype)
        //{
        //    MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
        //    objDaHrmTrnLeaveManage.DaGetLeaveManage(branch, department, fromdate, leavetype, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("DeleteLeaveManage")]
        [HttpGet]
        public HttpResponseMessage DeleteLeaveManage(string params_gid)
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaDeleteLeaveManage(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeletePermission")]
        [HttpGet]
        public HttpResponseMessage DeletePermission(string params_gid)
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaDeletePermission(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteOnDuty")]
        [HttpGet]
        public HttpResponseMessage DeleteOnDuty(string params_gid)
        {
            MdlHrmTrnLeaveManage values = new MdlHrmTrnLeaveManage();
            objDaHrmTrnLeaveManage.DaDeleteOnDuty(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("LeaveSubmit")]
        [HttpPost]
        public HttpResponseMessage LeaveSubmit(leavemanage_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnLeaveManage.DaLeaveSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PermissionSubmit")]
        [HttpPost]
        public HttpResponseMessage PermissionSubmit(permissionname_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnLeaveManage.DaPermissionSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("OnDutySubmit")]
        [HttpPost]
        public HttpResponseMessage OnDutySubmit(ondutyname_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnLeaveManage.DaOnDutySubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PermissionImport")]
        [HttpPost]
        public HttpResponseMessage PermissionImport()
        {
            HttpRequest httpRequest;
            permissionname_list values = new permissionname_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaHrmTrnLeaveManage.DaPermissionImport(httpRequest, getsessionvalues.employee_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("LeaveImport")]
        [HttpPost]
        public HttpResponseMessage LeaveImport()
        {
            HttpRequest httpRequest;
            leavemanage_list values = new leavemanage_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaHrmTrnLeaveManage.DaLeaveImport(httpRequest, getsessionvalues.employee_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
}