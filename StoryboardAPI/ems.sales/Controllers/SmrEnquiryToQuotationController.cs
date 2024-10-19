using ems.sales.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrEnquiryToQuotation")]
    [Authorize]

    public class SmrEnquiryToQuotationController : ApiController
    {
   
            session_values Objgetgid = new session_values();
            logintoken getsessionvalues = new logintoken();
            DaEnquiryToQuotation objsales = new DaEnquiryToQuotation();

            // DATA FETCHING FROM ENQUIRY SUMMARY 

            [ActionName("GetEnquiryQuotationSummary")]
            [HttpGet]
            public HttpResponseMessage GetEnquiryQuotationSummary(string enquiry_gid)
            {
                MdlEnquiryToQuotation objresult = new MdlEnquiryToQuotation();
                objsales.DaGetEnquiryQuotationSummary(enquiry_gid, getsessionvalues.employee_gid, objresult);
                return Request.CreateResponse(HttpStatusCode.OK, objresult);
            }

          // PRODUCT DROP DOWN FOR CUSTOMER ENQUIRY FROM CRM 360

            [ActionName("GetProductETQ")]
            [HttpGet]
            public HttpResponseMessage GetProductETQ()
            {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
               objsales.DaGetProductETQ(values);
               return Request.CreateResponse(HttpStatusCode.OK, values);
            }

        // PRODUCT ON CHANGE EVENT FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetOnChangeProductETQ")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductETQ(string customercontact_gid,string product_gid)
        {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
            objsales.DaGetOnChangeProductETQ(customercontact_gid,product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // PRODUCT SUMMARY TAX DROP DOWN FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetProdTaxETQ")]
        [HttpGet]
        public HttpResponseMessage GetProdTaxETQ()
        {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
            objsales.DaGetProdTaxETQ(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // OVERALL TAX FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetOverallTaxETQ")]
        [HttpGet]
        public HttpResponseMessage GetOverallTaxETQ()
        {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
            objsales.DaGetOverallTaxETQ(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // TERMS AND CONDITIONS DROP DOWN FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetTermSForETQ")]
        [HttpGet]
        public HttpResponseMessage GetTermSForETQ()
        {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
            objsales.DaGetTermSForETQ(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ON CHANGE EVENT FOR TERMS AND CONDITIONS FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetOnChangeTermsAndConditionsETQ")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeTermsAndConditionsETQ(string template_gid)
        {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
            objsales.DaGetOnChangeTermsAndConditionsETQ(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // PRODUCT TEMPORARY SUMMARY

        [ActionName("GetEnquirytoQuotationTempSummary")]
            [HttpGet]
            public HttpResponseMessage GetEnquirytoQuotationTempSummary(string enquiry_gid)
            {
                MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
                objsales.DaGetEnquirytoQuotationTempSummary(enquiry_gid, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);

            }

            // PRODUCT ADD FOR ENQUIRY TO QUOTATION

            [ActionName("GetEnquirytoQuotationProductAdd")]
            [HttpPost]
            public HttpResponseMessage GetEnquirytoQuotationProductAdd(EnquirytoQuotationsummary_list values)
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = Objgetgid.gettokenvalues(token);
                objsales.DaGetEnquirytoQuotationProductAdd(getsessionvalues.employee_gid, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

            // SUBMIT EVENT FOR ENQUIRY TO QUOTATION

            [ActionName("PostEnquirytoQuotation")]
            [HttpPost]
            public HttpResponseMessage PostEnquirytoQuotation(PostEnquiryToQUotation_list values)
            {
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = Objgetgid.gettokenvalues(token);
                objsales.DaPostEnquirytoQuotation(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

        // PRODUCT EDIT EVENT FOR ENQUIRY TO QUOTATION

        [ActionName("GetEnqtoQuoteEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEnqtoQuoteEditProductSummary(string tmpquotationdtl_gid)
        {
            MdlEnquiryToQuotation objresult = new MdlEnquiryToQuotation();
            objsales.DaGetEnqtoQuoteEditProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE PRODUCT --  ENQUIRY TO QUOTATION PRODUCT SUMMARY
        [ActionName("PostUpdateEnquirytoQuotationProduct")]
        [HttpPost]
        public HttpResponseMessage PostUpdateEnquirytoQuotationProduct(EditQuoteProductList values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostUpdateEnquirytoQuotationProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ENQUIRY TO QUOTATION DELETE EVENT

        [ActionName("GetDeleteQuoteProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteQuoteProductSummary(string tmpquotationdtl_gid)
        {
            EditQuoteProductList objresult = new EditQuoteProductList();
            objsales.DaGetDeleteQuoteProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // ENQUIRY TO QUOTATION PRODUCT SUMMARY DETAIL BUTTON

        [ActionName("GetRaiseQuotedetail")]
        [HttpGet]
        public HttpResponseMessage GetRaiseQuotedetail(string product_gid)
        {
            MdlEnquiryToQuotation objresult = new MdlEnquiryToQuotation();
            objsales.DaGetRaiseQuotedetail(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        //tax segment popup

        [ActionName("GetProductTaxdetails")]
        [HttpGet]

        public HttpResponseMessage GetProductTaxdetails(string product_gid, string customercontact_gid)

        {
            MdlEnquiryToQuotation values = new MdlEnquiryToQuotation();
            objsales.DaGetProductTaxdetails(product_gid, customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
    }
}
