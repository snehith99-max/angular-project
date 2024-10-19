using System;
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
    [RoutePrefix("api/SysMstUserGroupTemp")]
    [Authorize]

    public class SysMstUserGroupTempController : ApiController
    {
        DaSysMstUserGroupTemp objDaSystemUserGroupTemplate = new DaSysMstUserGroupTemp();
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();

        [ActionName("GetUserMenuList")]
        [HttpGet]
        public HttpResponseMessage GetUserMenuList(MdlSysMstUserGroupTemp values)
        {
            menu_response objresult = new menu_response();
            objDaSystemUserGroupTemplate.DaUserMenuList(values, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostUserGroupTemp")]
        [HttpPost]
        public HttpResponseMessage PostUserGroupTemp(MdlSysMstUserGroupTemp values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaSystemUserGroupTemplate.DaPostUserGroupTemp(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUserGroupTempSummary")]
        [HttpGet]
        public HttpResponseMessage getTopMenu()
        {
            MdlSysMstUserGroupTemp objresult = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaUserGroupTempSummary(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetEditUserMenuList")]
        [HttpGet]
        public HttpResponseMessage GetEditUserMenuList(string usergrouptemplate_gid)
        {
            MdlSysMstUserGroupTemp values = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaEditUserMenuList(values, usergrouptemplate_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("GetEditUserGroupTempSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditUserGroupTempSummary(string usergrouptemplate_gid)
        {
            MdlSysMstUserGroupTemp values = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaGetEditUserGroupTempSummary(usergrouptemplate_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateUserGroupTemp")]
        [HttpPost]
        public HttpResponseMessage UpdateUserGroupTemp(MdlSysMstUserGroupTemp values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaSystemUserGroupTemplate.DaUpdateUserGroupTemp(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetusergroupActive")]
        [HttpGet]
        public HttpResponseMessage GetusergroupActive(string params_gid)
        {
            MdlSysMstUserGroupTemp objvalues = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaUsergroupActive(params_gid, objvalues);
            return Request.CreateResponse(HttpStatusCode.OK, objvalues);
        }

        [ActionName("GetusergroupInactive")]
        [HttpGet]
        public HttpResponseMessage GetusergroupInactive(string params_gid)
        {
            MdlSysMstUserGroupTemp objvalues = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaUsergroupInactive(params_gid, objvalues);
            return Request.CreateResponse(HttpStatusCode.OK, objvalues);
        }

        [ActionName("Getuesrgroupdetails")]
        [HttpGet]
        public HttpResponseMessage Getuesrgroupdetails(string usergrouptemplate_gid)
        {
            MdlSysMstUserGroupTemp objvalues = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaGetuesrgroupdetails(usergrouptemplate_gid, objvalues);
            return Request.CreateResponse(HttpStatusCode.OK, objvalues);
        }


        [ActionName("DeleteUserGroupDetails")]
        [HttpGet]
        public HttpResponseMessage DeleteUserGroupDetails(string usergrouptemplate_gid)
        {
            MdlSysMstUserGroupTemp objresult = new MdlSysMstUserGroupTemp();
            objDaSystemUserGroupTemplate.DaDeleteUserGroupDetails(usergrouptemplate_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}