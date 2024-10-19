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
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HrmMstAssetList")]
    [Authorize]
    public class HrmMstAssetListController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmMstAssetList objDaHrmMstAssetList = new DaHrmMstAssetList();

        [ActionName("GetAssetListSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssetListSummary()
        {
            MdlHrmMstAssetList values = new MdlHrmMstAssetList();
            objDaHrmMstAssetList.DaGetAssetListSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostAssetList")]
        [HttpPost]
        public HttpResponseMessage PostAssetList(asset_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objDaHrmMstAssetList.DaPostAssetList(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdatedAssetList")]
        [HttpPost]
        public HttpResponseMessage UpdatedAssetList(asset_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmMstAssetList.DaUpdatedAssetList(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("DeleteAssetList")]
        //[HttpGet]
        //public HttpResponseMessage DeleteAssetList(string params_gid)
        //{
        //    asset_list objresult = new asset_list();
        //    objDaHrmMstAssetList.DaDeleteAssetList(params_gid, objresult);
        //    return Request.CreateResponse(HttpStatusCode.OK, objresult);
        //}
    }
}