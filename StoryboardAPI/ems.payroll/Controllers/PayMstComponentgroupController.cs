using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
namespace ems.payroll.Controllers
{
    [Authorize]
    [RoutePrefix("api/PayMstComponentgroup")]

    public class PayMstComponentgroupController :ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstComponentgroup objdapay = new DaPayMstComponentgroup();

        [ActionName("GetComponentgroupSummary")]
        [HttpGet]
        public HttpResponseMessage GetComponentgroupSummary()
        {
            MdlPayMstComponentgroup values = new MdlPayMstComponentgroup();
            objdapay.DaComponentgroupSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postcomponentgroup")]
        [HttpPost]
        public HttpResponseMessage Postcomponentgroup(Componentgroup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdapay.DaPostcomponentgroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Updatecomponentgroup")]
        [HttpPost]
        public HttpResponseMessage Updatecomponentgroup(string user_gid, Componentgroup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdapay.DaUpdatecomponentgroup(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getDeleteComponent")]
        [HttpGet]
        public HttpResponseMessage getDeleteComponent(string componentgroup_gid)
        {
            MdlPayMstComponentgroup objresult = new MdlPayMstComponentgroup();
            objdapay.DagetDeleteComponent(componentgroup_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

    }
}