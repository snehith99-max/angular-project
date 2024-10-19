using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.sales.Models;
using System.Threading.Tasks;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrRptInvoiceReport")]
    [Authorize]
    public class SmrRptInvoiceReportController:ApiController
    {

        string msSQL= string.Empty;
        int mnResult;
        dbconn dbconn = new dbconn();

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptInvoiceReport objDaSmrRptInvoiceReport = new DaSmrRptInvoiceReport();


        /////Taxsegment

        [ActionName("GetProductsearchSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary(string producttype_gid, string product_name, string customer_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetProductsearchSummary(producttype_gid, product_name, customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

       
        [ActionName("GetSendMail_MailId")]
        [HttpGet]
        public HttpResponseMessage GetSendMail_MailId(string mail_invoice_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetSendMail_MailId(values, mail_invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getfrommailid")]
        [HttpGet]
        public HttpResponseMessage Getfrommailid()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetfrommailid(values);
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
            objDaSmrRptInvoiceReport.DaPostMail(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        // sales type drop down
        [ActionName("GetSalesType")]
        [HttpGet]
        public HttpResponseMessage GetSalesType()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetSalesType(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }












        /////




        [ActionName("GetProductNamedropDown")]
        [HttpGet]
        public HttpResponseMessage GetProductNamedropDown()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetProductNamedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetcurrencyCodedropdown")]
        [HttpGet]
        public HttpResponseMessage GetcurrencyCodedropdown()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetcurrencyCodedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerNamedropDown")]
        [HttpGet]
        public HttpResponseMessage GetCustomerNamedropDown()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetCustomerNamedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchName")]
        [HttpGet]
        public HttpResponseMessage GetBranchName()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetBranchName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Gettaxnamedropdown")]
        [HttpGet]
        public HttpResponseMessage Gettaxnamedropdown()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGettaxnamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTermsandConditions")]
        [HttpGet]
        public HttpResponseMessage GetTermsandConditions()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetTermsandConditions(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceProductSummary()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DainvoiceProductSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        

        [ActionName("GetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetOnChangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeTerm")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeTerms(string template_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetOnChangeTerms(template_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeCustomer")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCustomer(string customer_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetOnChangeCustomer(values, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetOnChangeProduct")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProduct(string product_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetOnChangeProduct(values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("InvoicePostProduct")]
        [HttpPost]
        public HttpResponseMessage InvoicePostProduct(invoiceproductlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaInvoicePostProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetEditInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditInvoiceProductSummary(string invoice_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaGetEditInvoiceProductSummary(getsessionvalues.employee_gid, values, invoice_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEditInvoice")]
        [HttpGet]
        public HttpResponseMessage GetEditInvoice(string invoice_gid)
        {
            MdlSmrRptInvoiceReport objresult = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetEditInvoice(invoice_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("UpdatedInvoice")]
        [HttpPost]
        public HttpResponseMessage UpdatedInvoice(invoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaUpdatedInvoice(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDirectInvoiceEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDirectInvoiceEditProductSummary(string invoicedtl_gid)
        {
            MdlSmrRptInvoiceReport objresult = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetDirectInvoiceEditProductSummary(invoicedtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("UpdateDirectInvoiceEditProductSummary")]
        [HttpPost]
        public HttpResponseMessage UpdateDirectInvoiceEditProductSummary(invoiceproductlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaUpdateDirectInvoiceEditProductSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("SalesinvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage SalesinvoiceSummary()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaSalesinvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getproductsummarydata")]
        [HttpGet]
        public HttpResponseMessage Getproductsummarydata(string directorder_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetproductsummarydata(values, directorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("salesinvoiceOnsubmit")]
        [HttpPost]
        public HttpResponseMessage salesinvoiceOnsubmit(salesinvoice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DasalesinvoiceOnsubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoicePDF")]
        [HttpGet]
        public HttpResponseMessage GetInvoicePDF(string invoice_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaSmrRptInvoiceReport.DaGetInvoicePDF(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        [ActionName("viewinvoice")]
        [HttpGet]
        public HttpResponseMessage viewinvoice(string invoice_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.Daviewinvoice(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("viewinvoiceproduct")]
        [HttpGet]
        public HttpResponseMessage viewinvoiceproduct(string invoice_gid)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.Daviewinvoiceproduct(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        ///---------------------------------------------new design asset-sm-----------------------------------------------///
        [ActionName("LoadPage")]
        [HttpGet]
        public HttpResponseMessage LoadPage()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            msSQL = " delete from rbl_tmp_tinvoicedtl where employee_gid='" + getsessionvalues.employee_gid + "'";
            mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [ActionName("GetShopifyInvoice")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetShopifyInvoice()
        {
            result values = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);

            values = await objDaSmrRptInvoiceReport.DaGetShopifyInvoice(getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("SaleinvoiceSummary")]
        [HttpGet]
        public HttpResponseMessage einvoiceSummary()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaSaleinvoiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaProductSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostproductDirectinvoice")]
        [HttpPost]
        public HttpResponseMessage PostproductDirectinvoice(directinvoiceproductsubmit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaPostproductDirectinvoice(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("changeinvoicerefno")]
        [HttpPost]
        public HttpResponseMessage changeinvoicerefno(invoicerefno_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            
            objDaSmrRptInvoiceReport.Dachangeinvoicerefno(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("cancelinvoice")]
        [HttpPost]
        public HttpResponseMessage cancelinvoice(CancelinvoiceList values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.Dacancelinvoice(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletemainInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeletemainInvoiceProductSummary(string invoicedtl_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetDeletemainInvoiceProductSummary(invoicedtl_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDeleteInvoiceProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteInvoiceProductSummary(string invoice_gid)
        {
            invoiceproductlist values = new invoiceproductlist();
            objDaSmrRptInvoiceReport.DaGetDeleteInvoiceProductSummary(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("SalesInvoiceSubmit")]
        [HttpPost]
        public HttpResponseMessage SalesInvoiceSubmit(salesinvoicelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSmrRptInvoiceReport.DaSalesInvoiceSubmit(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoiceForLastSixMonths")]
        [HttpGet]

        public HttpResponseMessage GetInvoiceForLastSixMonths()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetInvoiceForLastSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoiceReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceReportForLastSixMonthsSearch(string from_date, string to_date)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetInvoiceReportForLastSixMonthsSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoiceDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceDetailSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetInvoiceDetailSummary(getsessionvalues.branch_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInvoiceIndividualreport")]
        [HttpGet]
        public HttpResponseMessage GetInvoiceIndividualreport(string salesorder_gid)
        {
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptInvoiceReport.DaGetInvoivceIndividualreport(values, salesorder_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetReceiptForLastSixMonths")]
        [HttpGet]

        public HttpResponseMessage GetReceiptForLastSixMonths()
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetReceiptForLastSixMonths(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetReceiptDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetReceiptDetailSummary(string month, string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetReceiptDetailSummary(getsessionvalues.branch_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetReceiptReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetReceiptReportForLastSixMonthsSearch(string from_date, string to_date)
        {
            MdlSmrRptInvoiceReport values = new MdlSmrRptInvoiceReport();
            objDaSmrRptInvoiceReport.DaGetReceiptReportForLastSixMonthsSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}