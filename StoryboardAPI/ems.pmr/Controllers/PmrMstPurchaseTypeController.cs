using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.pmr.DataAccess;
using ems.pmr.Models;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrMstPurchaseType")]
    [Authorize]
    public class PmrMstPurchaseTypeController:ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrMstPurchaseType objpurchase = new DaPmrMstPurchaseType();

        [ActionName("GetpurchaseType")]
        [HttpGet]
        public HttpResponseMessage GetpurchaseType()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlPmrMstPurchasetype values = new MdlPmrMstPurchasetype();
            objpurchase.DaGetpurchaseType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPmrpurchaseType")]
        [HttpPost]
        public HttpResponseMessage PostPmrpurchaseType(GetpurchaseType_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostPmrpurchaseType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatePmrpurchaseType")]
        [HttpPost]
        public HttpResponseMessage UpdatePmrpurchaseType(GetpurchaseType_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaUpdatePmrpurchaseType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletepurchasetype")]
        [HttpGet]
        public HttpResponseMessage GetDeletepurchasetype(string purchasetype_gid)
        {
            GetpurchaseType_List objresult = new GetpurchaseType_List();
            objpurchase.DaGetDeletepurchasetype(purchasetype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


    }
}