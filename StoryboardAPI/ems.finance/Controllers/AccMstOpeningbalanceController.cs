using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/AccMstOpeningbalance")]
    [Authorize]
    public class AccMstOpeningbalanceController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccMstOpeningbalance objDaPurchase = new DaAccMstOpeningbalance();       

        // Module Summary

        [ActionName("GetOpeningbalance")]
        [HttpGet]
        public HttpResponseMessage GetOpeningbalance(string entity_gid,string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetOpeningbalance(entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccMstOpeningbalance")]
        [HttpGet]
        public HttpResponseMessage GetAccMstOpeningbalance(string entity_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetAccMstOpeningbalance(entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetParentName")]
        [HttpGet]
        public HttpResponseMessage GetParentName()
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetParentName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetParentNameAsset")]
        [HttpGet]
        public HttpResponseMessage GetParentNameAsset()
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetParentNameAsset(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLiabilityAccountName")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityAccountName()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetLiabilityAccountName(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssetAccountName")]
        [HttpGet]
        public HttpResponseMessage GetAssetAccountName()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetAssetAccountName(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOpeningBalance")]
        [HttpPost]
        public HttpResponseMessage PostOpeningBalance(openingbalance_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostOpeningBalance(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getupdateopeningbalance")]
        [HttpPost]
        public HttpResponseMessage Getupdateopeningbalance(openingbalanceedit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetupdateopeningbalance(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getassetupdateopeningbalance")]
        [HttpPost]
        public HttpResponseMessage Getassetupdateopeningbalance(assetopeningbalanceedit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetassetupdateopeningbalance(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEntityDetails")]
        [HttpGet]
        public HttpResponseMessage GetEntityDetails()
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetEntityDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLiabilityGroupNameList")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityGroupNameList(string entity_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetLiabilityGroupNameList(entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssetGroupNameList")]
        [HttpGet]
        public HttpResponseMessage GetAssetGroupNameList(string entity_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetAssetGroupNameList(entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLiabilitySubGroupNameList")]
        [HttpGet]
        public HttpResponseMessage GetLiabilitySubGroupNameList(string account_gid, string entity_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetLiabilitySubGroupNameList(account_gid, entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssetSubGroupNameList")]
        [HttpGet]
        public HttpResponseMessage GetAssetSubGroupNameList(string account_gid, string entity_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetAssetSubGroupNameList(account_gid, entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLiabilityLedgerNameList")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityLedgerNameList(string account_gid,string entity_gid,string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetLiabilityLedgerNameList(account_gid,entity_gid,finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssetLedgerNameList")]
        [HttpGet]
        public HttpResponseMessage GetAssetLedgerNameList(string account_gid, string entity_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetAssetLedgerNameList(account_gid, entity_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchDetails")]
        [HttpGet]
        public HttpResponseMessage GetBranchDetails()
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetBranchDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLiabilitySummary")]
        [HttpGet]
        public HttpResponseMessage GetLiabilitySummary(string branch_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetLiabilitySummary(branch_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAssetSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssetSummary(string branch_gid, string finyear)
        {
            MdlAccMstOpeningbalance values = new MdlAccMstOpeningbalance();
            objDaPurchase.DaGetAssetSummary(branch_gid, finyear, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}