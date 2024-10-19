using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrQuotation360CRM")]
    [Authorize]
    public class SmrQuotation360CRMController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrQuotation360CRM objsales = new DaSmrQuotation360CRM();

        // BRANCH DROPDOWM FOR QUOTATION 360

        [ActionName("GetBranchQCRM")]
        [HttpGet]
        public HttpResponseMessage GetBranchQCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetBranchQCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CURRENCY DROP DOWN FOR QUOTATION 360

        [ActionName("GetCurrencyQCRM")]
        [HttpGet]
        public HttpResponseMessage GetCurrencyQCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetCurrencyQCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT DROPDOWM FOR 360 QUOTATION

        [ActionName("GetProductNameQCRM")]
        [HttpGet]
        public HttpResponseMessage GetProductNameQCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetProductNameQCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT SUMMARY TAX DROPDOWN

        [ActionName("GetTax1QCRM")]
        [HttpGet]
        public HttpResponseMessage GetTax1QCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetTax1QCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // OVERALL SUMMARY TAX DROPDOWN

        [ActionName("GetTax2QCRM")]
        [HttpGet]
        public HttpResponseMessage GetTax2QCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetTax2QCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // TERMS AND CONDITIONS FOR QUOTATION CRM 360

        [ActionName("GetTermsandConditionsQCRM")]
        [HttpGet]
        public HttpResponseMessage GetTermsandConditionsQCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetTermsandConditionsQCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CUSTOMER DROPDOWN FOR QUOTATION 360

        [ActionName("GetCustomerQCRM")]
        [HttpGet]
        public HttpResponseMessage GetCustomerQCRM(string leadbank_gid)
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetCustomerQCRM(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // SALES PERSON DROP DOWN FOR QUOTATION 360

        [ActionName("GetSalesPersonQCRM")]
        [HttpGet]
        public HttpResponseMessage GetSalesPersonQCRM()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetSalesPersonQCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ON CHANGE CUSTOMER NAME FOR QUOTATION 360

        [ActionName("GetOnChangeCustomerQCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomerQCRM(string customercontact_gid)
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetOnChangeCustomerQCRM(customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ON CHANGE PRODUCT NAME FOR QUOTATION 360

        [ActionName("GetOnChangeProductNameQCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductNameQCRM(string product_gid, string customercontact_gid)
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetOnChangeProductNameQCRM(product_gid, customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeProductNamesQCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductNamesQCRM(string product_gid)
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetOnChangeProductNamesQCRM(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // TERMS AND CONDITIONS ON CHANGE EVENT FOR QUOTATION 360

        [ActionName("GetOnChangeTermsQCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeTermsQCRM(string template_gid)
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            objsales.DaGetOnChangeTermsQCRM(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // PRODUCT ADD FOR QUOTATION 360

        [ActionName("PostAddProductQCRM")]
        [HttpPost]
        public HttpResponseMessage PostAddProductQCRM(Quote360Product values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostAddProductQCRM(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT SUMMARY FOR QUOTATION 360

        [ActionName("GetQCRMTempProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetQCRMTempProductSummary()
        {
            MdlQuotation360CRM values = new MdlQuotation360CRM();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetQCRMTempProductSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT SUMMARY EDIT FOR QUOTATION 360

        [ActionName("GetQuotation360EditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetQuotation360EditProductSummary(string tmpquotationdtl_gid)
        {
            MdlQuotation360CRM objresult = new MdlQuotation360CRM();
            objsales.DaGetQuotation360EditProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE PRODUCT  QUOTATION 360

        [ActionName("UpdateQuotationProductCRM")]
        [HttpPost]
        public HttpResponseMessage UpdateQuotationProductCRM(Quote360Product values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaUpdateQuotationProductCRM(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT DELETE EVENT FOR QUOTATION 360

        [ActionName("DeleteQCRMProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeleteQCRMProductSummary(string tmpquotationdtl_gid)
        {
            Quote360Product objresult = new Quote360Product();
            objsales.DaDeleteQCRMProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // POST DIRECT QUOTATION FOR 360

        [ActionName("PostDirectQuotationCRM")]
        [HttpPost]
        public HttpResponseMessage PostDirectQuotationCRM(PostQuoteCRM_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostDirectQuotationCRM(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}