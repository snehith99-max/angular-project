using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/SysRptScreenPrivilege")]

    public class SysRptScreenPrivilegeController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysRptScreenPrivilege objDaSysRptScreenPrivilege = new DaSysRptScreenPrivilege();

        [ActionName("GetScreenPrivilegeSummary")]
        [HttpGet]
        public HttpResponseMessage GetScreenPrivilegeSummary()
        {
            MdlSysRptScreenPrivilege values = new MdlSysRptScreenPrivilege();
            objDaSysRptScreenPrivilege.DaGetScreenPrivilegeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLevel1Menu")]
        [HttpGet]
        public HttpResponseMessage GetLevel1Menu()
        {
            MdlSysRptScreenPrivilege values = new MdlSysRptScreenPrivilege();
            objDaSysRptScreenPrivilege.DaGetLevel1Menu(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetLevel2Menu")]
        [HttpGet]
        public HttpResponseMessage GetLevel2Menu()
        {
            MdlSysRptScreenPrivilege values = new MdlSysRptScreenPrivilege();
            objDaSysRptScreenPrivilege.DaGetLevel2Menu(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLevel3Menu")]
        [HttpGet]
        public HttpResponseMessage GetLevel3Menu()
        {
            MdlSysRptScreenPrivilege values = new MdlSysRptScreenPrivilege();
            objDaSysRptScreenPrivilege.DaGetLevel3Menu(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeLevel1Detail")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeLevel1Detail(string module_gid)
        {
            MdlSysRptScreenPrivilege values = new MdlSysRptScreenPrivilege();
            objDaSysRptScreenPrivilege.DaGetEmployeeLevel1Detail(module_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}