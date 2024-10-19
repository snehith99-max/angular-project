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
    [RoutePrefix("api/GLCode")]
    [Authorize]
    public class GLCodeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaGLCode objfinance = new DaGLCode();

        [ActionName("GetDebtorSummary")]
        [HttpGet]
        public HttpResponseMessage GetDebtorSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetDebtorSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxSegmentMapping")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentMapping()
        {
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetTaxSegmentMapping(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCreditorSummary")]
        [HttpGet]
        public HttpResponseMessage GetCreditorSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetCreditorSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxPayableSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxPayableSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetTaxPayableSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAssetDtlsSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssetDtlsSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetAssetDtlsSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSalesTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesTypeSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetSalesTypeSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseTypeSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseTypeSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetPurchaseTypeSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDepartmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetDepartmentSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAcctMappingSummary")]
        [HttpGet]
        public HttpResponseMessage GetAcctMappingSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetAcctMappingSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetExpenseGroupSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseGroupSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetExpenseGroupSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSalesType")]
        [HttpPost]
        public HttpResponseMessage PostSalesType(salestype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostSalesType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetupdateSalesTypeDtls")]
        [HttpPost]
        public HttpResponseMessage GetupdateSalesTypeDtls(salestype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetupdateSalesTypeDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMappingAccountTo")]
        [HttpGet]
        public HttpResponseMessage GetMappingAccountTo()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetMappingAccountTo(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateSalesTypeMapping")]
        [HttpPost]
        public HttpResponseMessage UpdateSalesTypeMapping(salestype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateSalesTypeMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPurchaseType")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseType(purchasetype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostPurchaseType(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetupdatePurchaseTypeDtls")]
        [HttpPost]
        public HttpResponseMessage GetupdatePurchaseTypeDtls(purchasetype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetupdatePurchaseTypeDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseMappingAccountTo")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseMappingAccountTo()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetPurchaseMappingAccountTo(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatePurchaseTypeMapping")]
        [HttpPost]
        public HttpResponseMessage UpdatePurchaseTypeMapping(purchasetype_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdatePurchaseTypeMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostTaxPayables")]
        [HttpPost]
        public HttpResponseMessage PostTaxPayables(taxpayable_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostTaxPayables(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetupdateTaxDtls")]
        [HttpPost]
        public HttpResponseMessage GetupdateTaxDtls(taxpayable_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetupdateTaxDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetupdateTaxGLCode")]
        [HttpPost]
        public HttpResponseMessage GetupdateTaxGLCode(taxpayable_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetupdateTaxGLCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetupdateAssetGLCode")]
        [HttpPost]
        public HttpResponseMessage GetupdateAssetGLCode(assetgl_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetupdateAssetGLCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetupdateEmployeeGLCode")]
        [HttpPost]
        public HttpResponseMessage GetupdateEmployeeGLCode(empglcode_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetupdateEmployeeGLCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetExpenseMappingAcct")]
        [HttpGet]
        public HttpResponseMessage GetExpenseMappingAcct()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetExpenseMappingAcct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateExpenseGroupMapping")]
        [HttpPost]
        public HttpResponseMessage UpdateExpenseGroupMapping(expensegroup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateExpenseGroupMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAccountMappingDtl")]
        [HttpGet]
        public HttpResponseMessage GetAccountMappingDtl()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetAccountMappingDtl(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAccountMapping")]
        [HttpPost]
        public HttpResponseMessage PostAccountMapping(accountmapping_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostAccountMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateMapAccount")]
        [HttpPost]
        public HttpResponseMessage UpdateMapAccount(mapaccount_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateMapAccount(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateDebtorExternalCode")]
        [HttpPost]
        public HttpResponseMessage UpdateDebtorExternalCode(debtor_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateDebtorExternalCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetRegionDtls")]
        [HttpGet]
        public HttpResponseMessage GetRegionDtls()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetRegionDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCountryDtls")]
        [HttpGet]
        public HttpResponseMessage GetCountryDtls()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetCountryDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostDebitorGLCode")]
        [HttpPost]
        public HttpResponseMessage PostDebitorGLCode(debtorglcode_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostDebitorGLCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateCreditorExternalCode")]
        [HttpPost]
        public HttpResponseMessage UpdateCreditorExternalCode(creditor_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateCreditorExternalCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostCreditorGLCode")]
        [HttpPost]
        public HttpResponseMessage PostCreditorGLCode(creditor_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostCreditorGLCode(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxChartDetails")]
        [HttpGet]
        public HttpResponseMessage GetTaxChartDetails()
        {
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetTaxChartDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTaxSegmentAccountMappingTo")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentAccountMappingTo()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlGLCode values = new MdlGLCode();
            objfinance.DaTaxSegmentAccountMappingTo(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateTaxSegmentMapping")]
        [HttpPost]
        public HttpResponseMessage UpdateTaxSegmentMapping(TaxSegmentAccountMapping_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateTaxSegmentMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxAccountMappingTo")]
        [HttpGet]
        public HttpResponseMessage GetTaxAccountMappingTo()
        {
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetTaxAccountMappingTo( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateTaxAccountMapping")]
        [HttpPost]
        public HttpResponseMessage UpdateTaxAccountMapping(GetTaxAccountMappingTo_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateTaxAccountMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeptAcctMappingDropdown")]
        [HttpGet]
        public HttpResponseMessage GetDeptAcctMappingDropdown()
        {
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetDeptAcctMappingDropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateDepartmentAccountMapping")]
        [HttpPost]
        public HttpResponseMessage UpdateDepartmentAccountMapping(GetDepartmentAccountDropDown_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateDepartmentAccountMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSalaryCompSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalaryCompSummary()
        {
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetSalaryCompSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSalaryCompMappingDropdown")]
        [HttpGet]
        public HttpResponseMessage GetSalaryCompMappingDropdown()
        {
            MdlGLCode values = new MdlGLCode();
            objfinance.DaGetSalaryCompMappingDropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateSalaryComponentMapping")]
        [HttpPost]
        public HttpResponseMessage UpdateSalaryComponentMapping(GetSalaryCompSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateSalaryComponentMapping(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}