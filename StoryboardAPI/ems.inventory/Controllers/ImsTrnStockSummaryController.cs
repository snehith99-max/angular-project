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


namespace ems.inventory.Controllers
{

    [RoutePrefix("api/ImsTrnStockSummary")]
    [Authorize]
    public class ImsTrnStockSummaryController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnStockSummary objDaInventory = new DaImsTrnStockSummary();
        // Module Summary
        [ActionName("GetStockSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockSummary(string branch_gid,string finyear)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetStockSummary(branch_gid, finyear,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductSplitSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSplitSummary(string product_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetProductSplitSummary(product_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProducttype")]
        [HttpGet]
        public HttpResponseMessage GetProducttype()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetProducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductUnitclass")]
        [HttpGet]
        public HttpResponseMessage GetProductUnitclass()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetProductUnitclass(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductNamDtl")]
        [HttpGet]
        public HttpResponseMessage GetProductNamDtl()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetProductNamDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsTrnOpeningstockAdd")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnOpeningstockAdd()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetImsTrnOpeningstockAdd(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeproductName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeproductName(string product_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetOnChangeproductName(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeLocation")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeLocation(string branch_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetOnChangeLocation(branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostStock")]
        [HttpPost]
        public HttpResponseMessage PostStock (Poststock values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPoststock(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAmendStockSummary")]
        [HttpGet]
        public HttpResponseMessage GetAmendStockSummary (string product_gid, string uom_gid, string branch_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetAmendStockSummary(product_gid, uom_gid, branch_gid,  values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAmendStock")]
        [HttpGet]
        public HttpResponseMessage GetAmendStock(string stock_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetAmendStock(stock_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postamendstock")]
        [HttpPost]
        public HttpResponseMessage Postamendstock(postamendstock values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostamendstock(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDamageStockSummary")]
        [HttpGet]
        public HttpResponseMessage GetDamageStockSummary (string product_gid, string uom_gid, string branch_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetDamageStockSummary(product_gid, uom_gid, branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("GetDamagedStock")]
        [HttpGet]
        public HttpResponseMessage GetDamagedStock(string stock_gid)
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetDamagedStock(stock_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostDamagedstock")]
        [HttpPost]
        public HttpResponseMessage PostDamagedstock(PostDamagedstock values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostDamagedstock (getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetproductUomName")]
        [HttpGet]
        public HttpResponseMessage GetproductUomName()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetproductUomName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostSplitstock")]
        [HttpPost]
        public HttpResponseMessage PostSplitstock(PostSplitstock values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostSplitstock(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetFinancialYear")]
        [HttpGet]
        public HttpResponseMessage GetFinancialYear()
        {
            MdlImsTrnStockSummary values = new MdlImsTrnStockSummary();
            objDaInventory.DaGetFinancialYear(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}