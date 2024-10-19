using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.storage.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTrnGrnInward")]
    [Authorize]
    public class PmrTrnGrnInwardController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnGrnInward objpurchase = new DaPmrTrnGrnInward();

        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        [ActionName("GetGrnInwardSummary")]
        [HttpGet]
        public HttpResponseMessage GetGrnInwardSummary()
        {
            MdlPmrTrnGrnInward values = new MdlPmrTrnGrnInward();
            objpurchase.DaGetGrnInwardSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditGrnInward")]
        [HttpGet]
        public HttpResponseMessage GetEditGrnInward( string grn_gid)
        {
            MdlPmrTrnGrnInward objresult = new MdlPmrTrnGrnInward();
            objpurchase.DaGetEditGrnInward(grn_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);      
        }
        [ActionName("GetEditGrnInwardproduct")]
        [HttpGet]
        public HttpResponseMessage GetEditGrnInwardproduct(string grn_gid)
        {
            MdlPmrTrnGrnInward objresult = new MdlPmrTrnGrnInward();
            objpurchase.DaGetEditGrnInwardproduct(grn_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("GetPurchaseOrderDetails")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderDetails(string purchaseorder_gid)
        {
            MdlPmrTrnGrnInward objresult = new MdlPmrTrnGrnInward();
            objpurchase.DaGetPurchaseOrderDetails(purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        //[ActionName("CreateASN")]
        //[HttpPost]
        //public HttpResponseMessage CreateOrder(MdlASNPost_list values)
        //{
        //    result objresult = new result();
        //    objresult = objpurchase.DaCreateASN(values.grn_gid, values.goodsintypes_id, values.supplier);
        //    return Request.CreateResponse(HttpStatusCode.OK, objresult);
        //}
        [ActionName("GetGoodInTypesMintSoft")]
        [HttpGet]
        public HttpResponseMessage GetGoodInTypesMintSoft()
        {
            MdlPmrTrnGrnInward values = new MdlPmrTrnGrnInward();
            objpurchase.DaGetGoodInTypesMintSoft(values);
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }

        [ActionName("DownloadDocument")]
        [HttpPost]
        public HttpResponseMessage download_Collateraldoc(MdlTelecallingDownload values)
        {
            var ls_response = new Dictionary<string, object>();
            //values.file_path = objFnazurestorage.DecryptData(values.file_path);
            ls_response = objFnazurestorage.FnDownloadDocument(values.file_path, values.file_name);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
        [ActionName("GetGRNsixmonthschart")]
        [HttpGet]
        public HttpResponseMessage GetGRNsixmonthschart()
        {
            MdlPmrTrnGrnInward values = new MdlPmrTrnGrnInward();
            objpurchase.DaGetGRNsixmonthschart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}