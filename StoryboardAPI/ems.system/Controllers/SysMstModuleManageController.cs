﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
using ems.system.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Web.Http.Results;

namespace ems.system.Controllers
{
    [RoutePrefix("api/SysMstModuleManage")]
    [Authorize]
    public class SysMstModuleManageController : ApiController
    {
        DaSysMstModuleManage objDaSysMstModuleManage = new DaSysMstModuleManage();
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();

        [ActionName("GetModuleListSummary")]
        [HttpGet]
        public HttpResponseMessage getTopMenu()
        {
            mdlModuleList objresult = new mdlModuleList();
            objDaSysMstModuleManage.DaGetModuleListSummary(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostManagerAssign")]
        [HttpPost]
        public HttpResponseMessage PostManagerAssign(mdlManagerAssignDtl values)
        {
            objDaSysMstModuleManage.DaPostManagerAssign(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeAssignlist")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeAssignlist(string module_gid)
        {
            mdlemployee objmaster = new mdlemployee();
            objDaSysMstModuleManage.DaGetEmployeeAssignlist(module_gid, objmaster);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }

        [ActionName("GetUserGroupTemplist")]
        [HttpGet]
        public HttpResponseMessage GetUserGroupTemplist(string module_gid)
        {
            mdlModuleList objmaster = new mdlModuleList();
            objDaSysMstModuleManage.DaUserGroupTemplist(module_gid, objmaster);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }

        [ActionName("GetModuleAssignedEmployee")]
        [HttpGet]
        public HttpResponseMessage GetModuleAssignedEmployee(string module_gid)
        {
            mdlModuleAssignedList objresult = new mdlModuleAssignedList();
            objDaSysMstModuleManage.DaGetModuleAssignedEmployee(module_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostModuleEmployeeAssign")]
        [HttpPost]
        public HttpResponseMessage PostModuleEmployeeAssign(mdlModuleemployeedtl values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaSysMstModuleManage.DaPostModuleEmployeeAssign(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUserRoleList")]
        [HttpPost]
        public HttpResponseMessage GetUserRoleList(MdlSelectedModule values)
        {
            menu_response objresult = new menu_response();
            objDaSysMstModuleManage.DaGetUserRoleList(values, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostPrivilege")]
        [HttpPost]
        public HttpResponseMessage DaPostPrivilege(MdlSelectedModule values)
        {
            objDaSysMstModuleManage.DaPostPrivilege(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("deletehierarchy")]
        [HttpPost]
        public HttpResponseMessage deletehierarchy(mdlManagerAssignDtl values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaSysMstModuleManage.Dadeletehierarchy(values,getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       

        [ActionName("ApprovalSummary")]
        [HttpGet]
        public HttpResponseMessage ApprovalSummary(string module_gid)
        {
            mdlModuleList values = new mdlModuleList();
            objDaSysMstModuleManage.DaApprovalSummary(module_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Approvalsubmit")]
        [HttpPost]
        public HttpResponseMessage Approvalsubmit(Approvalsubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaSysMstModuleManage.DaApprovalsubmit(getsessionvalues.user_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostEmployeeAssignSubmit")]
        [HttpPost]
        public HttpResponseMessage PostEmployeeAssignSubmit(submoudule_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaSysMstModuleManage.DaPostEmployeeAssignSubmit(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetApprovalAssignHierarchysummary")]
        [HttpGet]
        public HttpResponseMessage GetApprovalAssignHierarchysummary(string module_gid)
        {
            mdlModuleList values = new mdlModuleList();
            objDaSysMstModuleManage.DaGetApprovalAssignHierarchysummary(module_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
