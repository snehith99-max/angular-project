
using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;

namespace ems.inventory.Controllers
{

    [RoutePrefix("api/ImsMstReorderlevel")]
    [Authorize]
    public class ImsMstReorderlevelController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsMstReorderlevel objDaInventory = new DaImsMstReorderlevel();

        [ActionName("GetImsMstReorderlevelSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsMstReorderlevelSummary()
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetImsMstReorderlevelSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProducttype")]
        [HttpGet]
        public HttpResponseMessage GetProducttype()
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetProducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductUnitclass")]
        [HttpGet]
        public HttpResponseMessage GetProductUnitclass()
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetProductUnitclass(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductNamDtl")]
        [HttpGet]
        public HttpResponseMessage GetProductNamDtl()
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetProductNamDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeproductName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeproductName(string product_gid)
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetOnChangeproductName(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetonchangeProductNamDtl")]
        [HttpGet]
        public HttpResponseMessage GetonchangeProductNamDtl(string productgroup_gid)
        {
            MdlImsMstReorderlevel values = new MdlImsMstReorderlevel();
            objDaInventory.DaGetonchangeProductNamDtl(productgroup_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostROL")]
        [HttpPost]
        public HttpResponseMessage PostROL(postrollist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostROL(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditROLSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditROLSummary(string rol_gid)
        {
            MdlImsMstReorderlevel objresult = new MdlImsMstReorderlevel();
            objDaInventory.DaGetEditROLSummary(rol_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostROLUpdate")]
        [HttpPost]
        public HttpResponseMessage PostROLUpdate(roledit_list values)
        {
            MdlImsMstReorderlevel objresult = new MdlImsMstReorderlevel();
            objDaInventory.DaPostROLUpdate(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}