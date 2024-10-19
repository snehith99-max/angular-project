using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.storage.Models;




namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTrnPurchaseOrder")]
    [Authorize]
    public class PmrTrnPurchaseOrderController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnPurchaseOrder objpurchase = new DaPmrTrnPurchaseOrder();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        [ActionName("GetPurchaseOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderSummary()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetPurchaseOrderSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //contract summary

        [ActionName("GetContractPO")]
        [HttpGet]
        public HttpResponseMessage GetContractPO(string vendor_gid)
        {
            MdlPmrTrnPurchaseOrder objresult = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetContractPO(vendor_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("GetBranch")]
        [HttpGet]
        public HttpResponseMessage GetBranch()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetBranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getproducttype")]
        [HttpGet]
        public HttpResponseMessage Getproducttype()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetproducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTax")]
        [HttpGet]
        public HttpResponseMessage GetTax()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetTax(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTax4Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax4Dtl()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetTax4Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetVendor")]
        [HttpGet]
        public HttpResponseMessage GetVendor()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetVendor(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetVendorContract")]
        [HttpGet]
        public HttpResponseMessage GetVendorContract()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetVendorContract(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetDispatchToBranch")]
        [HttpGet]
        public HttpResponseMessage GetDispatchToBranch()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetDispatchToBranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetCurrency")]
        [HttpGet]
        public HttpResponseMessage GetCurrency()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetCurrency(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductCode")]
        [HttpGet]
        public HttpResponseMessage GetProductCode()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetProductCode(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetViewPurchaseOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewPurchaseOrderSummary(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder objresult = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetViewPurchaseOrderSummary(purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetOnChangeBranch")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeBranch(string branch_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetOnChangeBranch(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeProductCode")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductCode(string product_code)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetOnChangeProductCode(product_code, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeProductName")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductName(string product_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetOnChangeProductName(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("GetProductsearchSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary(string producttype_gid, string product_name, string vendor_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetProductsearchSummary(producttype_gid, product_name, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeVendor")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeVendor(string vendor_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetOnChangeVendor(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProduct")]
        [HttpGet]
        public HttpResponseMessage GetProduct()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetProduct(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetOnchangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("GetOnAdd")]
        //[HttpPost]
        //public HttpResponseMessage GetOnAdd(productlist values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objpurchase.DaGetOnAdd(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("PostOnAddproduct")]
        [HttpPost]
        public HttpResponseMessage PostOnAddproduct(submitProducts values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostOnAddproduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeleteProductSummary(string tmppurchaseorderdtl_gid)
        {
            productlist values = new productlist();
            objpurchase.DaDeleteProductSummary(tmppurchaseorderdtl_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary(string vendor_gid, string product_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaProductSummary(getsessionvalues.user_gid, vendor_gid, values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Submit

        [ActionName("ProductSubmit")]
        [HttpPost]
        public HttpResponseMessage ProductSubmit(GetViewPurchaseOrder values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaProductSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //mailfunction

        [ActionName("GetTemplatelist")]
        [HttpGet]
        public HttpResponseMessage GetTemplatelist()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetTemplatelist(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplate")]
        [HttpGet]
        public HttpResponseMessage GetTemplate(string template_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetTemplatet(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMailId")]
        [HttpGet]
        public HttpResponseMessage GetMailId()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaMaillId(getsessionvalues.employee_gid, values);
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
            objpurchase.DaPostMail(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        //download Report files
        [ActionName("GetPurchaseOrderRpt")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderRpt(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            var ls_response = new Dictionary<string, object>();
            ls_response = objpurchase.DaGetPurchaseOrderRpt(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("GetPurchaserwithoutpricepdf")]
        [HttpGet]
        public HttpResponseMessage GetPurchaserwithoutpricepdf(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            var ls_response = new Dictionary<string, object>();
            ls_response = objpurchase.DaGetPurchaserwithoutpricepdf(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
        //[ActionName("GetFromMailDropdown")]
        //[HttpGet]
        //public HttpResponseMessage GetFromMailDropdown()
        //{
        //    MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
        //    objpurchase.DaGetFromMailDropdown(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        //[ActionName("GetProductsearchSummaryPurchase")]
        //[HttpGet]
        //public HttpResponseMessage GetProductsearchSummaryPurchase(string producttype_gid, string product_name, string vendor_gid)
        //{
        //    MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
        //    objpurchase.DaGetProductsearchSummaryPurchase(producttype_gid, product_name, vendor_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("PostProductAdd")]
        [HttpPost]
        public HttpResponseMessage PostProductAdd(PostPOProduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostProductAdd(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getuser")]
        [HttpGet]
        public HttpResponseMessage Getuser()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetuser(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductWithTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductWithTaxSummary(string product_gid, string vendor_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetProductWithTaxSummary(product_gid, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductsearchSummaryPurchase")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummaryPurchase()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetProductsearchSummaryPurchase(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetEditPurchaseOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditPurchaseOrderSummary(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder objresult = new MdlPmrTrnPurchaseOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetEditPurchaseOrderSummary(getsessionvalues.user_gid, purchaseorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("PoSubmit")]
        [HttpPost]
        public HttpResponseMessage PoSubmit(GetViewPurchaseOrder values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPoSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Contract PO Submit

        [ActionName("Postcontractpo")]
        [HttpPost]
        public HttpResponseMessage Postcontractpo(contractposubmit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostcontractpo(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OperationalApprovalPO")]
        [HttpGet]
        public HttpResponseMessage OperationalApprovalPO(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaOperationalApprovalPO(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("OperationalRejectPO")]
        [HttpGet]
        public HttpResponseMessage OperationalRejectPO(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaOperationalRejectPO(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("FinanceApprovalPO")]
        [HttpGet]
        public HttpResponseMessage FinanceApprovalPO(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaFinanceApprovalPO(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("FinanceRejectPO")]
        [HttpGet]
        public HttpResponseMessage FinanceRejectPO(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaFinanceRejectPO(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("FinanceApprovalSummary")]
        [HttpGet]
        public HttpResponseMessage FinanceApprovalSummary()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaFinanceApprovalSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOpertionalApprovalSummary")]
        [HttpGet]
        public HttpResponseMessage GetOpertionalApprovalSummary()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetOpertionalApprovalSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getproductdeletetemp")]
        [HttpGet]
        public HttpResponseMessage Getproductdeletetemp()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetproductdeletetemp(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetVendoremail")]
        [HttpGet]
        public HttpResponseMessage GetVendoremail(string purchaseorder_gid)
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetVendoremail(purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("CreateASN")]
        [HttpPost]
        public HttpResponseMessage CreateOrder(string purchaseorder_gid)
        {
            result objresult = new result();
            objresult = objpurchase.DaCreateASN(purchaseorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        [ActionName("Getpurchaseordersixmonthschart")]
        [HttpGet]
        public HttpResponseMessage Getpurchaseordersixmonthschart()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.daGetpurchaseordersixmonthschart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // submit with file upload
        [ActionName("PostPurchaseOrderfileupload")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseOrderfileupload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objpurchase.DaPostPurchaseOrderfileupload(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("DownloadDocument")]
        [HttpPost]
        public HttpResponseMessage download_Collateraldoc(MdlTelecallingDownload values)
        {
            var ls_response = new Dictionary<string, object>();
            //values.file_path = objFnazurestorage.DecryptData(values.file_path);
            ls_response = objFnazurestorage.FnDownloadDocument(values.file_path, values.file_name);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("PostPurchaseOrderfileuploaddraft")]
        [HttpPost]
        public HttpResponseMessage PostPurchaseOrderfileuploaddraft()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objpurchase.DaPostPurchaseOrderfileuploaddraft(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("ProductSubmitdraft")]
        [HttpPost]
        public HttpResponseMessage ProductSubmitdraft(GetViewPurchaseOrder values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaProductSubmitdraft(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPoDraftsSummary")]
        [HttpGet]
        public HttpResponseMessage GetPoDraftsSummary()
        {
            MdlPmrTrnPurchaseOrder values = new MdlPmrTrnPurchaseOrder();
            objpurchase.DaGetPoDraftsSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDraftPurchaseOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetDraftPurchaseOrderSummary(string purchaseorderdraft_gid)
        {
            MdlPmrTrnPurchaseOrder objresult = new MdlPmrTrnPurchaseOrder();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetDraftPurchaseOrderSummary(getsessionvalues.user_gid, purchaseorderdraft_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // submit with file upload
        [ActionName("UpdatePurchaseOrderfileupload")]
        [HttpPost]
        public HttpResponseMessage UpdatePurchaseOrderfileupload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objpurchase.DaUpdatePurchaseOrderfileupload(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
}