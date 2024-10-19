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
    [RoutePrefix("api/SmrTrnCustomerEnquiry")]
    [Authorize]
    public class SmrTrnCustomerEnquiryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnCustomerEnquiry objsales = new DaSmrTrnCustomerEnquiry();

        //summary
        [ActionName("GetCustomerEnquirySummary")]
        [HttpGet]
        public HttpResponseMessage GetCustomerEnquirySummary()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetCustomerEnquirySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Lead DropDown
        [ActionName("GetLeadDtl")]
        [HttpGet]
        public HttpResponseMessage GetLeadDtl()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetLeadDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Update Close


        [ActionName("GetUpdatedCloseEnquiry")]
        [HttpPost]
        public HttpResponseMessage GetUpdatedCloseEnquiry(GetCusEnquiry values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetUpdatedCloseEnquiry(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Team DropDown
        [ActionName("GetTeamDtl")]
        [HttpGet]
        public HttpResponseMessage GetTeamDtl()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetTeamDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Employee DropDown
        [ActionName("GetEmployeeDtl")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDtl()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetEmployeeDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Update Reassign
        [ActionName("GetUpdatedReAssignEnquiry")]
        [HttpPost]
        public HttpResponseMessage GetUpdatedReAssignEnquiry(GetCusEnquiry values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetUpdatedReAssignEnquiry(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Product Drop down
        [ActionName("GetProduct")]
        [HttpGet]
        public HttpResponseMessage GetProduct()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetProducts(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
      
        // Customer Dropdown
        [ActionName("GetCustomer")]
        [HttpGet]
        public HttpResponseMessage GetCustomer()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetCustomer(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLead")]
        [HttpGet]
        public HttpResponseMessage GetLead()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetLead(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Direct sales customer on change
        [ActionName("GetOnChangeCustomerName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomerName(string customercontact_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetOnChangeCustomerName(customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Direct sales lead on change
        [ActionName("GetOnChangeLead")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeLead(string leadbank_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetOnChangeLead(leadbank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Edit customer name (Direct Sales)
        [ActionName("GetOnEditCustomerName")]
        [HttpGet]
        public HttpResponseMessage GetOnEditCustomerName(string customercontact_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetOnEditCustomerName(customercontact_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Direct sales on change product
        [ActionName("GetOnChangeProductsName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductsName(string product_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetOnChangeProductsName(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //  Temp Product Summary (Direct Enquiry)
        [ActionName("GetProductsSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetProductsSummary(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Product Add for Direct raise Enquiry
        [ActionName("PostOnAdds")]
        [HttpPost]
        public HttpResponseMessage PostOnAdds(productsummarys_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostOnAdds(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // Currency dropdown for Raise quotation from enquiry
        [ActionName("GetCurrencyDets")]
        [HttpGet]
        public HttpResponseMessage GetCurrencyDets()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetCurrencyDets(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Branch dropdown
        [ActionName("GetBranchDet")]
        [HttpGet]
        public HttpResponseMessage GetBranchDet()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetBranchDet(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Employee Dropdown
        [ActionName("GetEmployeePerson")]
        [HttpGet]
        public HttpResponseMessage GetEmployeePerson()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetEmployeePerson(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // delete for direct enquiry event
        [ActionName("GetDeleteEnquiryProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteEnquiryProductSummary(string tmpsalesenquiry_gid)
        {
            productsummarys_list objresult = new productsummarys_list();
            objsales.DaGetDeleteEnquiryProductSummary(tmpsalesenquiry_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Overall Submit For Customer Enquiry
        [ActionName("PostCustomerEnquiry")]
        [HttpPost]
        public HttpResponseMessage PostCustomerEnquiry(PostAll values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostCustomerEnquiry(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Raise Proposal
        [ActionName("GetSmrTrnRaiseProposal")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnRaiseProposalstring(string enquiry_gid)
        {
            MdlSmrTrnCustomerEnquiry objresult = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetSmrTrnRaiseProposal(enquiry_gid, objresult); ;
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Document Type for Raise Proposal from Customer Enquiry Summary
        [ActionName("GetDocumentType")]
        [HttpGet]
        public HttpResponseMessage GetDocumentType()
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetDocumentType(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

       // Post Proposal
        [ActionName("Postpropsal")]
        [HttpPost]
        public HttpResponseMessage Postpropsal()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objsales.DaPostproposal(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        // Get Proposal Summary
        [ActionName("GetProposalSummary")]
        [HttpGet]
        public HttpResponseMessage GetProposalSummary(string enquiry_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetProposalSummary(enquiry_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // [ActionName("Uploaddocument")]
        //  [HttpPost]
        //public HttpResponseMessage Uploaddocument(string user_gid )
        //{
        //    HttpRequest httpRequest;
        //   string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        /// getsessionvalues = Objgetgid.gettokenvalues(token);
        //httpRequest = HttpContext.Current.Request;
        //result objResult = new result();
        //objsales.DaUploaddocument(httpRequest, objResult ,getsessionvalues.user_gid);
        //return Request.CreateResponse(HttpStatusCode.OK, objResult);

        // Customer Enquiry Summary View
        [ActionName("GetViewEnquirySummary")]
        [HttpGet]
        public HttpResponseMessage GetViewEnquirySummary(string enquiry_gid)
        {
            MdlSmrTrnCustomerEnquiry objresult = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetViewEnquirySummary(enquiry_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        // DIRECT ENQUIRY PRODUCT EDIT

        [ActionName("GetDirectEnquiryEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDirectEnquiryEditProductSummary(string tmpsalesenquiry_gid)
        {
            MdlSmrTrnCustomerEnquiry objresult = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetDirectEnquiryEditProductSummary(tmpsalesenquiry_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE PRODUCT -- DIRECT ENQUIRY PRODUCT SUMMARY
        [ActionName("PostUpdateEnquiryProduct")]
        [HttpPost]
        public HttpResponseMessage PostUpdateEnquiryProduct(productsummarys_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostUpdateEnquiryProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Customer Dropdown
        [ActionName("GetCustomer360")]
        [HttpGet]
        public HttpResponseMessage GetCustomer360(string leadbank_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetCustomer360(leadbank_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Direct sales customer on change
        [ActionName("GetOnChangeCustomerName360")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomerName360(string customer_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetOnChangeCustomerName(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // PROPOSAL DELETE
        [ActionName("DeleteProposal")]
        [HttpGet]
        public HttpResponseMessage DeleteProposal(string proposal_gid)
        {
            productsummarys_list objresult = new productsummarys_list();
            objsales.DaDeleteProposal(proposal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        //Lead to Customer page
        [ActionName("GetLeadSummary")]
        [HttpGet]
        public HttpResponseMessage GetLeadSummary(string leadbank_gid)
        {
            MdlSmrTrnCustomerEnquiry values = new MdlSmrTrnCustomerEnquiry();
            objsales.DaGetLeadSummary(leadbank_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Lead to Customer Submit
        [ActionName("Postlead")]
        [HttpPost]
        public HttpResponseMessage Postcustomer(postlead_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostlead(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}

