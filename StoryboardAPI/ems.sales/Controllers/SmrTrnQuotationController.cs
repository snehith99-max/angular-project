using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;


namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnQuotation")]
    [Authorize]
    public class SmrTrnQuotationController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnQuotation objDaSales = new DaSmrTrnQuotation();
        // Module Summary
        [ActionName("GetSmrTrnQuotation")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnQuotation()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetSmrTrnQuotation(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getquotationsixmonthschart")]
        [HttpGet]
        public HttpResponseMessage Getquotationsixmonthschart()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.daGetquotationsixmonthschart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //  sales person dropdown

        [ActionName("GetSalesDtl")]
        [HttpGet]
        public HttpResponseMessage GetSalesDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetSalesDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Currency dropdown

        [ActionName("GetCurrencyCodeDtl")]
        [HttpGet]
        public HttpResponseMessage GetCurrencyCodeDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetCurrencyCodeDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 1 dropdown

        [ActionName("GetTaxOnceDtl")]
        [HttpGet]
        public HttpResponseMessage GetTaxOnceDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTaxOnceDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMailId")]
        [HttpGet]
        public HttpResponseMessage GetMailId()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetMailId(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


       
        [ActionName("GetSendMail_MailId")]
        [HttpGet]
        public HttpResponseMessage GetSendMail_MailId(string mail_invoice_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetSendMail_MailIdquot(values, mail_invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 2 dropdown

        [ActionName("GetTaxTwiceDtl")]
        [HttpGet]
        public HttpResponseMessage GetTaxTwiceDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTaxTwiceDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 3 dropdown

        [ActionName("GetTaxThriceDtl")]
        [HttpGet]
        public HttpResponseMessage GetTaxThriceDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTaxThriceDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Product dropdown

        [ActionName("GetProductNamesDtl")]
        [HttpGet]
        public HttpResponseMessage GetProductNamesDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetProductNamesDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 4 dropdown

        [ActionName("GetTaxFourSDtl")]
        [HttpGet]
        public HttpResponseMessage GetTaxFourSDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTaxFourSDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductTaxseg")]
        [HttpGet]

        public HttpResponseMessage GetProductTaxseg(string product_gid, string customercontact_gid)

        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetProductTaxseg(product_gid, customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // On change
        [ActionName("GetOnChangeProductsName")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeProductsName(string product_gid, string customercontact_gid)

        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetOnChangeProductsName(product_gid, customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetOnChangeProductsNames")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeProductsNames(string product_gid)

        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetOnChangeProductsNames(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // Branch dropdown

        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetBranchDt(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //customer

        [ActionName("GetCustomerDtl")]
        [HttpGet]
        public HttpResponseMessage GetCustomerDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetCustomerDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }




        // Person dropdown

        [ActionName("GetPersonDtl")]
        [HttpGet]
        public HttpResponseMessage GetPersonDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetPersonDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Currency dropdown

        [ActionName("GetCurrencyDtl")]
        [HttpGet]
        public HttpResponseMessage GetCurrencyDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetCurrencyDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Product dropdown


        [ActionName("GetProductDtl")]
        [HttpGet]
        public HttpResponseMessage GetProductDtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetProductDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 1 dropdown

        [ActionName("GetTax1Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax1Dtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTax1Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 2 dropdown

        [ActionName("GetTax2Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax2Dtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTax2Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax 3 dropdown

        [ActionName("GetTax3Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax3Dtl()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTax3Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        // On change customer
        [ActionName("GetOnChangeCustomerDtls")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomerDtls(string customercontact_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetOnChangeCustomerDtls(customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Terms And Condition dropdown

        [ActionName("GetTermsandConditions")]
        [HttpGet]
        public HttpResponseMessage GetTermsandConditions()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTermsandConditions(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // On change t and c
        [ActionName("GetOnChangeTerms")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeTerms(string template_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetOnChangeTerms(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // temp product summary by pugaz 

        [ActionName("GetTempProductsSummary")]
        [HttpGet]
        public HttpResponseMessage GetTempProductsSummary()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetTempProductsSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

      
        // delete for enq to quote event

        [ActionName("GetDeleteQuotationProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteQuotationProductSummary(string tmpsalesorderdtl_gid)
        {
            summaryprod_list objresult = new summaryprod_list();
            objDaSales.DaGetDeleteQuotationProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        // Overall submit For Direct Quotation

        [ActionName("PostDirectQuotation")]
        [HttpPost]
        public HttpResponseMessage PostDirectQuotation(postQuatation values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostDirectQuotation(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetViewQuotationSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewQuotationSummary(string quotation_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetViewQuotationSummary(quotation_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetViewquotationdetails")]
        [HttpGet]
        public HttpResponseMessage GetViewquotationdetails(string quotation_gid, string customer_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetViewquotationdetails(quotation_gid, customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetOnchangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeCurrencyexhange")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrencyexhange(string currency_code)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetOnChangeCurrencyexhange(currency_code, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetProductdetails(string quotation_gid)
        {
            MdlSmrTrnQuotation objresult = new MdlSmrTrnQuotation();
            objDaSales.DaGetProductdetails(quotation_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("getDeleteQuotation")]
        [HttpGet]
        public HttpResponseMessage getDeleteQuotation(string params_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DagetDeleteQuotation(params_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getquotationhistorydata")]
        [HttpGet]
        public HttpResponseMessage Getquotationhistorydata(string quotation_gid)
        {
            quotationhistorylist values = new quotationhistorylist();
            values = objDaSales.DaGetquotationhistorydata(quotation_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getquotationhistorysummarydata")]
        [HttpGet]
        public HttpResponseMessage Getquotationhistorysummarydata(string quotation_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();

            objDaSales.DaGetquotationhistorysummarydata(values, quotation_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getquotationproductdetails")]
        [HttpGet]
        public HttpResponseMessage Getquotationproductdetails(string quotation_gid)
        {
            MdlSmrTrnQuotation objresult = new MdlSmrTrnQuotation();
            objDaSales.DaGetquotationproductdetails(quotation_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        //mail func

        [ActionName("GetTemplatelist")]
        [HttpGet]
        public HttpResponseMessage GetTemplatelist()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTemplatelist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplate")]
        [HttpGet]
        public HttpResponseMessage GetTemplate(string template_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetTemplatet(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostMail")]
        [HttpPost]
        public HttpResponseMessage PostMail()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objDaSales.DaPostMail(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        // DIRECT QUOTATION PRODUCT EDIT

        [ActionName("GetDirectQuotationEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDirectQuotationEditProductSummary(string tmpquotationdtl_gid)
        {
            MdlSmrTrnQuotation objresult = new MdlSmrTrnQuotation();
            objDaSales.DaGetDirectQuotationEditProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        public HttpResponseMessage GetRaiseQuotedetail(string product_gid)
        {
            MdlSmrTrnQuotation objresult = new MdlSmrTrnQuotation();
            objDaSales.DaGetRaiseQuotedetail(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE PRODUCT -- DIRECT QUOTATION PRODUCT SUMMARY

        [ActionName("PostUpdateDirectQuotationProduct")]
        [HttpPost]
        public HttpResponseMessage PostUpdateDirectQuotationProduct(DirecteditQuotationList values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostUpdateDirectQuotationProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //download Report files
        [ActionName("GetQuotationRpt")]
        [HttpGet]
        public HttpResponseMessage GetQuotationRpt(string quotation_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaSales.DaGetQuotationRpt(quotation_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("GetProductsearchSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary(string producttype_gid, string product_name, string customer_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetProductsearchSummary(producttype_gid, product_name, customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("PostOnAddproduct")]
        //[HttpPost]
        //public HttpResponseMessage PostOnAddproduct(summaryprod_list values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDaSales.DaPostOnAddproduct(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GetProductAdd")]
        [HttpPost]
        public HttpResponseMessage GetProductAdd(productsubmit__list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostOnAddproduct( getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetEditQuotationSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditQuotationSummary(string quotation_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            objDaSales.DaGetEditQuotationSummary(quotation_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditTempProductsSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditTempProductsSummary()
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetEditTempProductsSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditTempProdSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditTempProdSummary(string quotation_gid)
        {
            MdlSmrTrnQuotation values = new MdlSmrTrnQuotation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetEditTempProdSummary(quotation_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditQuoteProdSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditQuoteProdSummary(string quotation_gid)
        {
            MdlSmrTrnQuotation objresult = new MdlSmrTrnQuotation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaGetEditQuoteProdSummary(quotation_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostUpdateQuotation")]
        [HttpPost]
        public HttpResponseMessage PostUpdateQuotation(postQuatation values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSales.DaPostUpdateQuotation(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}

