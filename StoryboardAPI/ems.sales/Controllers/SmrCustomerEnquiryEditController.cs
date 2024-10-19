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
    [RoutePrefix("api/SmrCustomerEnquiryEdit")]
    [Authorize]
    public class SmrCustomerEnquiryEditController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCustomerEnquiryEdit objsales = new DaCustomerEnquiryEdit();

        // DATA FETCHING FOR EDIT CUSTOMER ENQUIRY SUMMARY

        [ActionName("GetEditCustomerEnquirySummary")]
        [HttpGet]
        public HttpResponseMessage GetEditCustomerEnquirySummary(string enquiry_gid)
        {
            MdlCustomerEnquiryEdit values = new MdlCustomerEnquiryEdit();
            objsales.DaGetEditCustomerEnquirySummary(enquiry_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // POST PRODUCT FOR EDIT CUSTOMER ENQUIRY
        [ActionName("PostProductEnquiryEdit")]
        [HttpPost]
        public HttpResponseMessage PostProductEnquiryEdit(editenquiryproductsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostProductEnquiryEdit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // CUSTOMER EDIT ENQUIRY - PRODUCT SUMMARY
        [ActionName("EditCustomerProductSummary")]
        [HttpGet]
        public HttpResponseMessage EditCustomerProductSummary(string enquiry_gid)
        {
            MdlCustomerEnquiryEdit values = new MdlCustomerEnquiryEdit();
            objsales.DaEditCustomerProductSummary(enquiry_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        // CUSTOMER EDIT ENQUIRY - PRODUCT SUMMARY
        [ActionName("EditProductSummary")]
        [HttpGet]
        public HttpResponseMessage EditProductSummary(string tmpsalesenquiry_gid)
        {
            MdlCustomerEnquiryEdit values = new MdlCustomerEnquiryEdit();
            objsales.DaEditProductSummary(tmpsalesenquiry_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        // DELETE EVENT FOR EDIT CUSTOMER ENQUIRY PRODUCT SUMMARY

        [ActionName("DeleteProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeleteProductSummary(string tmpsalesenquiry_gid)
        {
            editproductsummarylist objresult = new editproductsummarylist();
            objsales.DaDeleteProductSummary(tmpsalesenquiry_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        // UPDATE PRODUCT --  ENQUIRY PRODUCT SUMMARY
        [ActionName("PostEnquiryUpdateProduct")]
        [HttpPost]
        public HttpResponseMessage PostEnquiryUpdateProduct(editproductsummarylist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostEnquiryUpdateProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // POST CUSTOMER ENQUIRY EDIT
        [ActionName("PostCustomerEnquiryEdit")]
        [HttpPost]
        public HttpResponseMessage PostCustomerEnquiryEdit(Postedit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostCustomerEnquiryEdit(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}