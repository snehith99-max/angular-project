using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using static ems.inventory.Models.MdlImsTrnRaiseMI;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ImsTrnStoreRequisition")]
    [Authorize]
    public class ImsTrnStoreRequisitionController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnStoreRequisition objDaInventory = new DaImsTrnStoreRequisition();

        [ActionName("GetSRSummary")]
        [HttpGet]
        public HttpResponseMessage GetSRSummary()
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetSRSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductrolGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductrolGroup()
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetProductrolGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetOnrolProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetOnrolProductGroup(string productgroup_gid)
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetOnrolProductGroup(productgroup_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetrolProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetrolProductSummary()
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetrolProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOnSRproduct")]
        [HttpPost]
        public HttpResponseMessage PostOnSRproduct(srproductsingle_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostOnSRproduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("tmprolProductSummary")]
        [HttpGet]
        public HttpResponseMessage tmprolProductSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DatmprolProductSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostSR")]
        [HttpPost]
        public HttpResponseMessage PostSR(storeRequisition_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostSR(getsessionvalues.user_gid, getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeletetmpProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeletetmpProductSummary(string tmpsr_gid)
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaDeletetmpProductSummary(tmpsr_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSRViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetSRViewProduct(string storerequisition_gid)
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetSRViewProduct(storerequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetStoreView")]
        [HttpGet]
        public HttpResponseMessage GetStoreView(string storerequisition_gid)
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetStoreView(storerequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetstoreViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetstoreViewProduct(string storerequisition_gid)
        {
            MdlImsTrnStoreRequisition values = new MdlImsTrnStoreRequisition();
            objDaInventory.DaGetstoreViewProduct(storerequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

    }
}