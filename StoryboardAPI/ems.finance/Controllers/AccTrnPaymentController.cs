//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ems.finance.Controllers
//{
//    public class AccTrnPaymentController
//    {
//    }
//}


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
    [RoutePrefix("api/AccTrnPayment")]
    [Authorize]
    public class AccTrnPaymentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccTrnPayment objDaPurchase = new DaAccTrnPayment();

        [ActionName("GetPaymentRptSummary")]
        [HttpGet]
        public HttpResponseMessage GetPaymentRptSummary()
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetPaymentRptSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getexpensedetails")]
        [HttpGet]
        public HttpResponseMessage Getexpensedetails(string expense_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetexpensedetails(expense_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPaymentview")]
        [HttpGet]
        public HttpResponseMessage GetPaymentview(string payment_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetPaymentview(payment_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }



        [ActionName("GetPaymentAddproceed")]
        [HttpGet]
        public HttpResponseMessage GetPaymentAddproceed(string vendorname)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetPaymentAddproceed(values, vendorname);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMakepaymentExpand")]
        [HttpGet]
        public HttpResponseMessage GetMakepaymentExpand(string vendor_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetMakepaymentExpand(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Productdetails")]
        [HttpGet]
        public HttpResponseMessage Productdetails(string expense_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaProductdetails(expense_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSinglePaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSinglePaymentSummary(string vendor_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetSinglePaymentSummary(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBankDetail")]
        [HttpGet]
        public HttpResponseMessage GetBankDetail()
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetBankDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCardDetail")]
        [HttpGet]
        public HttpResponseMessage GetCardDetail()
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetCardDetail(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getmultipleexpense2employeedtl")]
        [HttpGet]
        public HttpResponseMessage Getmultipleexpense2employeedtl(string vendor_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetmultipleexpense2employeedtl(getsessionvalues.user_gid, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetpaymentexpenseSummary")]
        [HttpGet]
        public HttpResponseMessage GetpaymentexpenseSummary(string vendor_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetpaymentexpenseSummary(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Postmultipleexpense2singlepayment")]
        [HttpPost]
        public HttpResponseMessage Postmultipleexpense2singlepayment(multipleexpense2singlepayment values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostmultipleexpense2singlepayment(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetpaymentCancel")]
        [HttpGet]
        public HttpResponseMessage GetpaymentCancel(string payment_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaGetpaymentCancel(payment_gid, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getPaymenamount")]
        [HttpGet]
        public HttpResponseMessage getPaymenamount(string payment_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DagetPaymenamount(payment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("paymentcancelsubmit")]
        [HttpGet]
        public HttpResponseMessage paymentcancelsubmit(string payment_gid)
        {
            MdlAccTrnPayment objresult = new MdlAccTrnPayment();
            objDaPurchase.Dapaymentcancelsubmit(payment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        //[ActionName("GetPaymentrpt")]
        //[HttpGet]
        //public HttpResponseMessage GetPaymentrpt(string payment_gid)
        //{
        //    MdlAccTrnPayment values = new MdlAccTrnPayment();
        //    var ls_response = new Dictionary<string, object>();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    ls_response = objDaPurchase.DaGetPaymentrpt(payment_gid, values, getsessionvalues.branch_gid);
        //    return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        //}

        [ActionName("GetSingleaddPaymentSummary")]
        [HttpGet]
        public HttpResponseMessage GetSingleaddPaymentSummary(string expense_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetSingleaddPaymentSummary(expense_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Postsinglepayment")]
        [HttpPost]
        public HttpResponseMessage Postsinglepayment(multipleexpense2singlepayment values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaPurchase.DaPostsinglepayment(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSundryPaymentView")]
        [HttpGet]
        public HttpResponseMessage GetSundryPaymentView(string payment_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetSundryPaymentView(payment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSundryPaymentViewDetails")]
        [HttpGet]
        public HttpResponseMessage GetSundryPaymentViewDetails(string payment_gid)
        {
            MdlAccTrnPayment values = new MdlAccTrnPayment();
            objDaPurchase.DaGetSundryPaymentViewDetails(payment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}