using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using static OfficeOpenXml.ExcelErrorValue;
namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserPrivilege")]
    public class UserprivilegeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaUserprivilege ObjdaUserprivilege = new DaUserprivilege();

        [ActionName("GetEmployeedropdown")]
        [HttpGet]
        public HttpResponseMessage GetEmployeedropdown()
        {
            MdlUserprivilege values = new MdlUserprivilege();
            ObjdaUserprivilege.DaGetEmployeedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeEmployee")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeEmployee(string user_gid)
        {
            MdlUserprivilege values = new MdlUserprivilege();
            ObjdaUserprivilege.DaGetOnChangeEmployee(values, user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        [ActionName("GetUserPrivilegeList")]
        [HttpGet]
        public HttpResponseMessage GetUserPrivilegeList(MdlUserprivilege values, string user_gid)
        {
            menu_response objresult = new menu_response();
            ObjdaUserprivilege.DaUserPrivilegeList(values, objresult, user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult) ;
        }

        [ActionName("GetUserPrivilegeSummary")]
        [HttpGet]
        public HttpResponseMessage GetUserPrivilegeSummary()
        {
            MdlUserprivilege values = new MdlUserprivilege();
            ObjdaUserprivilege.DaGetUserPrivilegeSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}