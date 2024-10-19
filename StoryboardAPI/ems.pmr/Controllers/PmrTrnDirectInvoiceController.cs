using ems.pmr.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.pmr.Models;
using System.Linq;

namespace ems.pmr.Controllers
{
   
    [RoutePrefix("api/PmrTrnDirectInvoice")]
    [Authorize]
    public class PmrTrnDirectInvoiceController : ApiController
    {
        string msSQL;
        int mnResult;
        dbconn objdbconn = new dbconn();

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnDirectInvoice objDainvoice = new DaPmrTrnDirectInvoice();

        [ActionName("LoadingaddSummary")]
        [HttpGet]
        public HttpResponseMessage LoadingaddSummary()
        {           
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            msSQL = "  delete from acp_tmp_tinvoice where created_by='" + getsessionvalues.user_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ActionName("GetBranchName")]
        [HttpGet]
        public HttpResponseMessage GetBranchName()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetBranchName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetVendorNamedropDown")]
        [HttpGet]
        public HttpResponseMessage GetVendorNamedropDown()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetVendornamedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeVendor")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeVendor(string vendor_gid)
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetOnChangeVendor(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        [ActionName("GetcurrencyCodedropdown")]
        [HttpGet]
        public HttpResponseMessage GetcurrencyCodedropdown()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetcurrencyCodedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPurchaseTypedropDown")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseTypedropDown()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetPurchaseTypedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Gettaxnamedropdown")]
        [HttpGet]
        public HttpResponseMessage Gettaxnamedropdown()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGettaxnamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExtraAddondropDown")]
        [HttpGet]
        public HttpResponseMessage GetExtraAddondropDown()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetExtraAddondropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExtraDeductiondropDown")]
        [HttpGet]
        public HttpResponseMessage GetExtraDeductiondropDown()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetExtraDeductiondropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("directinvoicesubmit")]
        [HttpPost]
        public HttpResponseMessage directinvoicesubmit(directsalesinvoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.Dadirectinvoicesubmit(getsessionvalues.employee_gid,getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPblProductsearchSummary")]
        [HttpGet]
        public HttpResponseMessage GetPblProductsearchSummary(string producttype_gid, string product_name, string vendor_gid)
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetPblProductsearchSummary(producttype_gid, product_name, vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPblProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetPblProductSummary(string vendor_gid, string product_gid)
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaGetPblProductSummary(getsessionvalues.user_gid, vendor_gid, values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PblGetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage PblGetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaPblGetOnchangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PblPostOnAddproduct")]
        [HttpPost]
        public HttpResponseMessage PblPostOnAddproduct(PblsubmitProducts values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaPblPostOnAddproduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PblGetAllChargesConfig")]
        [HttpGet]
        public HttpResponseMessage PblGetAllChargesConfig()
        {
            MdlPmrMstPurchaseConfig values = new MdlPmrMstPurchaseConfig();
            objDainvoice.DaPblGetAllChargesConfig(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PblDeleteProductSummary")]
        [HttpGet]
        public HttpResponseMessage PblDeleteProductSummary(string tmpinvoicedtl_gid)
        {
            Pdlproductlist values = new Pdlproductlist();
            objDainvoice.DaPblDeleteProductSummary(tmpinvoicedtl_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PblProductSubmit")]
        [HttpPost]
        public HttpResponseMessage PblProductSubmit(PblDirectInvoice values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaPblProductSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PblGetTax")]
        [HttpGet]
        public HttpResponseMessage PblGetTax()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaPblGetTax(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PblGetTax4Dtl")]
        [HttpGet]
        public HttpResponseMessage PblGetTax4Dtl()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaPblGetTax4Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PblGetproducttype")]
        [HttpGet]
        public HttpResponseMessage PblGetproducttype()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaPblGetproducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Additional")]
        [HttpGet]
        public HttpResponseMessage Additional()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaAdditional(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Discount")]
        [HttpGet]
        public HttpResponseMessage Discount()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaDiscount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //Edit
        //[ActionName("GetPmrTrnInvoiceedit")]
        //[HttpGet]
        //public HttpResponseMessage GetPmrTrnInvoiceedit(string invoice_gid)
        //{
        //    MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDainvoice.DaGetPmrTrnInvoiceedit(getsessionvalues.user_gid,invoice_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetPmrTrnInvoiceedit")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnInvoiceedit(string invoice_gid)
        {
            MdlPmrTrnDirectInvoice objresult = new MdlPmrTrnDirectInvoice();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaGetPmrTrnInvoiceedit(getsessionvalues.user_gid, invoice_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("DaPblProductUpdate")]
        [HttpPost]
        public HttpResponseMessage DaPblProductUpdate(PblDirectInvoice values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaPblProductUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoiceDraftsSummary")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceDraftsSummary()
        {
            MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
            objDainvoice.DaGetInvoiceDraftsSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PblDraftOverallSubmit")]
        [HttpPost]
        public HttpResponseMessage PblDraftOverallSubmit(PblDirectInvoice values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaPblDraftOverallSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //[ActionName("GetPmrTrnInvoiceDraft")]
        //[HttpGet]
        //public HttpResponseMessage GetPmrTrnInvoiceDraft(string invoice_gid)
        //{
        //    MdlPmrTrnDirectInvoice values = new MdlPmrTrnDirectInvoice();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDainvoice.DaGetPmrTrnInvoiceDraft(getsessionvalues.user_gid, invoice_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);

        //}


        [ActionName("GetPmrTrnInvoiceDraft")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnInvoiceDraft(string invoice_gid)
        {
            MdlPmrTrnDirectInvoice objresult = new MdlPmrTrnDirectInvoice();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDainvoice.DaGetPmrTrnInvoiceDraft(getsessionvalues.user_gid, invoice_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

    }
}