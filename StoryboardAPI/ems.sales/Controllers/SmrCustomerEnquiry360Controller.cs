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
    [RoutePrefix("api/SmrCustomerEnquiry360")]
    [Authorize]
    public class SmrCustomerEnquiry360Controller : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCustomerEnquiry360 objsales = new DaCustomerEnquiry360();

        // PRODUCT DROP DOWN FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetProductECRM")]
        [HttpGet]
        public HttpResponseMessage GetProductECRM()
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            objsales.DaGetProductECRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // BRANCH DROP DOWN FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetBranchECRM")]
        [HttpGet]
        public HttpResponseMessage GetBranchECRM()
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            objsales.DaGetBranchECRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CUSTOMER DROPDOWN FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetCustomerECRM")]
        [HttpGet]
        public HttpResponseMessage GetCustomerECRM(string leadbank_gid)
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            objsales.DaGetCustomerECRM(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // SALES PERSON DROP DOWN FOR CUSTOMER ENQUIRY FROM  CRM 360

        [ActionName("GetSalesPersonECRM")]
        [HttpGet]
        public HttpResponseMessage GetSalesPersonECRM()
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            objsales.DaGetSalesPersonECRM(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CUSTOMER ON CHANGE FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetOnChangeCustomerECRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomerECRM(string customercontact_gid)
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            objsales.DaGetOnChangeCustomerECRM(customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT ON CHANGE FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetOnChangeProductECRM")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductECRM(string product_gid)
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            objsales.DaGetOnChangeProductECRM(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT ADD FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("PostProductECRM")]
        [HttpPost]
        public HttpResponseMessage PostProductECRM(ECRMProductSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostProductECRM(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT SUMMARY FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("GetProductSummaryECRM")]
        [HttpGet]
        public HttpResponseMessage GetProductSummaryECRM()
        {
            MdlCustomerEnquiry360 values = new MdlCustomerEnquiry360();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetProductSummaryECRM(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT EDIT FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("EditProductSummaryECRM")]
        [HttpGet]
        public HttpResponseMessage EditProductSummaryECRM(string tmpsalesenquiry_gid)
        {
            MdlCustomerEnquiry360 objresult = new MdlCustomerEnquiry360();
            objsales.DaEditProductSummaryECRM(tmpsalesenquiry_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // PRODUCT UPDATE FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("UpdateEnquiryProductECRM")]
        [HttpPost]
        public HttpResponseMessage UpdateEnquiryProductECRM(ECRMProductSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaUpdateEnquiryProductECRM(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // DELETE PRODUCT SUMMARY FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("DeleteProductSummaryECRM")]
        [HttpGet]
        public HttpResponseMessage DeleteProductSummaryECRM(string tmpsalesenquiry_gid)
        {
            ECRMProductSummary_list objresult = new ECRMProductSummary_list();
            objsales.DaDeleteProductSummaryECRM(tmpsalesenquiry_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // OVERALL SUBMIT FOR CUSTOMER ENQUIRY FROM CRM 360

        [ActionName("PostCustomerEnquiryECRM")]
        [HttpPost]
        public HttpResponseMessage PostCustomerEnquiryECRM(PostECRM_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostCustomerEnquiryECRM(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}