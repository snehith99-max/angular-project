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
    [RoutePrefix("api/SmrQuotationAmend")]
    [Authorize]
    public class SmrQuotationAmendController : ApiController

    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrQuotationAmend ObjDaSales = new DaSmrQuotationAmend();

        // AMEND DATA FETCHING FROM QUOTATION SUMARY 

        [ActionName("GetQuotationamend")]
        [HttpGet]

        public HttpResponseMessage GetQuotationamend(string quotation_gid)
        {
            MdlSmrQuotationAmend objresult = new MdlSmrQuotationAmend();
            ObjDaSales.DaGetQuotationamend(quotation_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // PRODUCT SUMMARY FOR QUOTATION AMEND

        [ActionName("QuotationAmendProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetQuotationproductSummary(string quotation_gid)
        {
            MdlSmrQuotationAmend values = new MdlSmrQuotationAmend();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjDaSales.DaGetQuotationproductSummary(values, quotation_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // QUOTATION AMEND - PRODUCT ADD

        [ActionName("PostAmendProduct")]
        [HttpPost]
        public HttpResponseMessage PostAmendProduct(quoteamendproductlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjDaSales.DaPostAmendProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // ON CHANGE PRODUCT EVENT

        [ActionName("GetOnChangeProductNameQOAmend")]
        [HttpGet]

        public HttpResponseMessage GetOnChangeProductNameQOAmend(string customercontact_gid,string product_gid)

        {
            MdlSmrQuotationAmend values = new MdlSmrQuotationAmend();
            ObjDaSales.DaGetOnChangeProductNameQOAmend(customercontact_gid,product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        // QUOTATION AMEND SUBMIT EVENT

        [ActionName("PostQuotationAmend")]
        [HttpPost]
        public HttpResponseMessage PostQuotationAmend(PostQuoteAmend_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjDaSales.DaPostQuotationAmend(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT DELETE EVENT

        [ActionName("DeleteAmendProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeleteAmendProductSummary(string tmpquotationdtl_gid)
        {
            quoteamendproductlist objresult = new quoteamendproductlist();
            ObjDaSales.DaDeleteAmendProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // EDIT QUOTATION AMEND PRODUCT SUMMARY

        [ActionName("GetQuotationAmendEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetQuotationAmendEditProductSummary(string tmpquotationdtl_gid)
        {
            MdlSmrQuotationAmend objresult = new MdlSmrQuotationAmend();
            ObjDaSales.DaGetQuotationAmendEditProductSummary(tmpquotationdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // UPDATE QUOTATION AMEND PRODUCT SUMMARY

        [ActionName("PostUpdateQuotationAmendProduct")]
        [HttpPost]
        public HttpResponseMessage PostUpdateQuotationAmendProduct(quoteamendproductlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjDaSales.DaPostUpdateQuotationAmendProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}