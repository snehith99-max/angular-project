using ems.inventory.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/IndentPriceEstimation")]
    [Authorize]
    public class IndentPriceEstimationController : ApiController
    {       
        session_values objgetGID = new session_values();
        logintoken getsession_values = new logintoken();
        DaIndentPriceEstimation objdata = new DaIndentPriceEstimation();

        [HttpGet]
        [ActionName("GetIndentPriceSummary")]
        public HttpResponseMessage GetIndentPriceSummary()
        {
            MdlIndentPriceEstimation values = new MdlIndentPriceEstimation();
            objdata.DaGetIndentPriceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [HttpGet]
        [ActionName("GetIDTPREstimateProductDetails")]
        public HttpResponseMessage GetIDTPREstimateProductDetails(string materialrequisition_gid)
        {
            MdlIndentPriceEstimation values = new MdlIndentPriceEstimation();
            objdata.DaGetIDTPREstimateProductDetails(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [HttpGet]
        [ActionName("GetIDTPREstimateDetails")]
        public HttpResponseMessage GetIDTPREstimateDetails(string materialrequisition_gid)
        {
            MdlIndentPriceEstimation values = new MdlIndentPriceEstimation();
            objdata.DaGetIDTPREstimateDetails(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [HttpGet]
        [ActionName("GetIDPRProductDetailsCheckPrice")]
        public HttpResponseMessage GetIDPRProductDetailsCheckPrice(string product_gid)
        {
            MdlIndentPriceEstimation values = new MdlIndentPriceEstimation();
            objdata.DaGetIDPRProductDetailsCheckPrice(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }
        [HttpPost]
        [ActionName("UpdateProductPriceGenerate")]
        public HttpResponseMessage UpdateProductPriceGenerate(generateprice_list values)
        {
            objdata.DaUpdateProductPriceGenerate(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [HttpGet]
        [ActionName("UpdateProvisionalAmount")]
        public HttpResponseMessage UpdateProvisionalAmount(string materialrequisition_gid, string provisional_amount)
        {
            MdlIndentPriceEstimation values = new MdlIndentPriceEstimation();
            objdata.DaUpdateProvisionalAmount(materialrequisition_gid, provisional_amount, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}