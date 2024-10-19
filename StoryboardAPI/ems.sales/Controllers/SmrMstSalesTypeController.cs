using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;
namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrMstSalesType")]
    [Authorize]
    public class SmrMstSalesTypeController:ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrMstSalesType objsales = new DaSmrMstSalesType();

        [ActionName("GetsalesType")]
        [HttpGet]
        public HttpResponseMessage GetsalesType()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrMstSalestype values = new MdlSmrMstSalestype();
            objsales.DaGetsalesType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSmrSalesType")]
        [HttpPost]
        public HttpResponseMessage PostSmrSalesType(GetSalesType_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSmrSalesType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateSmrSalesType")]
        [HttpPost]
        public HttpResponseMessage UpdateSmrSalesType(GetSalesType_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaUpdateSmrSalesType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeleteSalestype")]
        [HttpGet]
        public HttpResponseMessage GetDeleteSalestype(string salestype_gid)
        {
            GetSalesType_List objresult = new GetSalesType_List();
            objsales.DaGetDeleteSalestype(salestype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}