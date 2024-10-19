﻿using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using StoryboardAPI.Models;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PblTrnPaymentRpt")]
    [Authorize]
    public class PblTrnPaymentRptController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPblTrnPaymentRpt objDaPurchase = new DaPblTrnPaymentRpt();
        
        [ActionName("GetPaymentRptSummary")]
        [HttpGet]
        public HttpResponseMessage GetPaymentRptSummary()
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetPaymentRptSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoicedetails")]
        [HttpGet]
        public HttpResponseMessage GetInvoicedetails(string invoice_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetInvoicedetails(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPaymentview")]
        [HttpGet]
        public HttpResponseMessage GetPaymentview(string payment_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetPaymentview(payment_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }



        [ActionName("GetPaymentAddproceed")]
        [HttpGet]
        public HttpResponseMessage GetPaymentAddproceed(string vendorname)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetPaymentAddproceed(values, vendorname);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMakepaymentExpand")]
        [HttpGet]
        public HttpResponseMessage GetMakepaymentExpand(string vendor_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetMakepaymentExpand(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Productdetails")]
        [HttpGet]
        public HttpResponseMessage Productdetails(string invoice_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaProductdetails(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSinglePaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSinglePaymentSummary(string vendor_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetSinglePaymentSummary(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBankDetail")]
        [HttpGet]
        public HttpResponseMessage GetBankDetail()
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetBankDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCardDetail")]
        [HttpGet]
        public HttpResponseMessage GetCardDetail()
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetCardDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getmultipleinvoice2employeedtl")]
        [HttpGet]
        public HttpResponseMessage Getmultipleinvoice2employeedtl(string vendor_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetmultipleinvoice2employeedtl(getsessionvalues.user_gid,vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetpaymentInvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetpaymentInvoiceSummary(string vendor_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetpaymentInvoiceSummary(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postmultipleinvoice2singlepayment")]
        [HttpPost]
        public HttpResponseMessage Postmultipleinvoice2singlepayment(multipleinvoice2singlepayment values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostmultipleinvoice2singlepayment(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetpaymentCancel")]
        [HttpGet]
        public HttpResponseMessage GetpaymentCancel(string payment_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetpaymentCancel(payment_gid, values,getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getPaymenamount")]
        [HttpGet]
        public HttpResponseMessage getPaymenamount(string payment_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DagetPaymenamount(payment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("paymentcancelsubmit")]
        [HttpGet]
        public HttpResponseMessage paymentcancelsubmit(string payment_gid)
        {
            MdlPblTrnPaymentRpt objresult = new MdlPblTrnPaymentRpt();
            objDaPurchase.Dapaymentcancelsubmit(payment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetPaymentrpt")]
        [HttpGet]
        public HttpResponseMessage GetPaymentrpt (string payment_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            var ls_response = new Dictionary<string, object>();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ls_response = objDaPurchase.DaGetPaymentrpt(payment_gid, values,getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("GetSingleaddPaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSingleaddPaymentSummary(string invoice_gid)
        {
            MdlPblTrnPaymentRpt values = new MdlPblTrnPaymentRpt();
            objDaPurchase.DaGetSingleaddPaymentSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Postsinglepayment")]
        [HttpPost]
        public HttpResponseMessage Postsinglepayment(multipleinvoice2singlepayment values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostsinglepayment(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}