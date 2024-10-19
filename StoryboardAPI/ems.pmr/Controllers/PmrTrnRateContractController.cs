using ems.pmr.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.pmr.Models;

namespace ems.pmr.Controllers
{
    [RoutePrefix("PmrTrnRateContract")]
    [Authorize]
    public class PmrTrnRateContractController: ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnRateContract objPmrRateContract = new DaPmrTrnRateContract();

        [ActionName("RateContractsummary")]
        [HttpGet]
        public HttpResponseMessage RateContractsummary()
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.DaRateContractsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Imsvendorcontract")]
        [HttpGet]

        public HttpResponseMessage Imsvendorcontract()
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.DaImsvendorcontract(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("Postratecontract")]
        [HttpPost]
        public HttpResponseMessage Postratecontract(contract_summarylist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objPmrRateContract.DaPostratecontract(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcontractvendor")]
        [HttpGet]
        public HttpResponseMessage Getcontractvendor(string ratecontract_gid)
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.DaGetcontractvendor(ratecontract_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetcontractProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetcontractProductSummary(string ratecontract_gid)
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.DaGetcontractProductSummary(ratecontract_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMapProduct")]
        [HttpPost]
        public HttpResponseMessage PostMapProduct(assignproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objPmrRateContract.DaPostMapProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductunAssignSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductunAssignSummary(string ratecontract_gid)
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.DaGetProductunAssignSummary(ratecontract_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UnAssignProduct")]
        [HttpPost]
        public HttpResponseMessage UnAssignProduct(assignproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objPmrRateContract.DaUnAssignProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAmendProduct")]
        [HttpPost]
        public HttpResponseMessage PostAmendProduct(assignproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objPmrRateContract.DaPostAmendProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcontractamend")]
        [HttpGet]
        public HttpResponseMessage Getcontractamend(string ratecontract_gid,string product_gid)
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.DaGetcontractamend(ratecontract_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("vendorposummary")]
        [HttpGet]
        public HttpResponseMessage vendorposummary()
        {
            MdlPmrTrnRateContract values = new MdlPmrTrnRateContract();
            objPmrRateContract.Davendorposummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }
}