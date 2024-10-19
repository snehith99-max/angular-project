
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
using static ems.sales.Models.MdlSalesOrder360;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrSalesOrder360")]
    [Authorize]
    public class SmrSalesOrder360Controller : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSalesOrder360 objsales = new DaSalesOrder360();

        [ActionName("GetBranchDtlSOCRM")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Customer dropdown 360

        [ActionName("GetCustomerDtlCRM")]
        [HttpGet]
        public HttpResponseMessage GetCustomerDtlCRM(string leadbank_gid)
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetCustomerDtlCRM(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // Person dropdown

        [ActionName("GetPersonDtlCRM")]
        [HttpGet]
        public HttpResponseMessage GetPersonDtl()
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetPersonDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 1 dropdown

        [ActionName("GetTax1DtlCRM")]
        [HttpGet]
        public HttpResponseMessage GetTax1Dtl()
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetTax1Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 2 dropdown

        [ActionName("GetTax4DtlSOCRM")]
        [HttpGet]
        public HttpResponseMessage GetTax4DtlSOCRM()
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetTax4DtlSOCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Product dropdown CRM

        [ActionName("GetProductNamDtlCRM")]
        [HttpGet]
        public HttpResponseMessage GetProductNamDtlCRM()
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetProductNamDtlCRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Terms And Condition dropdown

        [ActionName("GetTermsandConditionsSOCRM")]
        [HttpGet]
        public HttpResponseMessage GetTermsandConditions()
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetTermsandConditions(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Customer onchange 360

        [ActionName("GetCustomerOnchangeCRM")]
        [HttpGet]
        public HttpResponseMessage GetCustomerOnchangeCRM(string customer_gid)
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetCustomerOnchangeCRM(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //On change currency
        [ActionName("GetOnChangeCurrencySOCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetOnchangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CRM product on change
        [ActionName("GetOnChangeProductsNameCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsNameCRM(string customercontact_gid, string product_gid)
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetOnChangeProductsNameCRM(customercontact_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeProductsNamesCRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsNamesCRM(string product_gid)
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.GetOnChangeProductsNamesCRM(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // productadd///
        [ActionName("PostOnAdds")]
        [HttpPost]
        public HttpResponseMessage PostOnAdds(salesorders_listSOCRM values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostOnAdds(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //product summary/
        [ActionName("GetSalesOrdersummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesOrdersummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetSalesOrdersummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // On change t and c
        [ActionName("GetOnChangeTerms")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeTerms(string template_gid)
        {
            MdlSalesOrder360 values = new MdlSalesOrder360();
            objsales.DaGetOnChangeTerms(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("PostSalesOrder")]
        [HttpPost]
        public HttpResponseMessage PostSalesOrder(postsales_listSOCRM values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSalesOrder(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Delete product summary//
        [ActionName("GetDeleteDirectSOProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteDirectSOProductSummary(string tmpsalesorderdtl_gid)
        {
            salesorders_listSOCRM objresult = new salesorders_listSOCRM();
            objsales.GetDeleteDirectSOProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("GetSalesorderdetail")]
        [HttpGet]
        public HttpResponseMessage GetSalesorderdetail(string product_gid)
        {
            MdlSalesOrder360 objresult = new MdlSalesOrder360();
            objsales.DaGetSalesorderdetail(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        // DIRECT SALES ORDER PRODUCT EDIT

        [ActionName("GetDirectSalesOrderEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDirectSalesOrderEditProductSummary(string tmpsalesorderdtl_gid)
        {
            MdlSalesOrder360 objresult = new MdlSalesOrder360();
            objsales.DaGetDirectSalesOrderEditProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE PRODUCT -- DIRECT QUOTATION PRODUCT SUMMARY

        [ActionName("PostUpdateDirectSOProduct")]
        [HttpPost]
        public HttpResponseMessage PostUpdateDirectSOProduct(DirecteditSalesorderListsocrm values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostUpdateDirectSOProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}