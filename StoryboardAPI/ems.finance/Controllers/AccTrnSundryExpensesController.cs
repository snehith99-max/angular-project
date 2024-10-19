using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/AccTrnSundryExpenses")]
    [Authorize]
    public class AccTrnSundryExpensesController  : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccTrnSundryExpenses objfinance = new DaAccTrnSundryExpenses();


        //Summary
        [ActionName("GetSundryExpenseSummary")]
        [HttpGet]
        public HttpResponseMessage GetSundryExpenseSummary()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetSundryExpenseSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetAccountGroup")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroup()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetAccountGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("onchangeaccountgroup")]
        [HttpGet]
        public HttpResponseMessage onchangeaccountgroup(string account_gid)
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.daonchangeaccountgroup(account_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchName")]
        [HttpGet]
        public HttpResponseMessage GetBranchName()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetBranchName(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getproducttype")]
        [HttpGet]
        public HttpResponseMessage Getproducttype()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetproducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetVendorNamedropDown")]
        [HttpGet]
        public HttpResponseMessage GetVendorNamedropDown()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetVendornamedropDown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeVendor")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeVendor(string vendor_gid)
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetOnChangeVendor(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetcurrencyCodedropdown")]
        [HttpGet]
        public HttpResponseMessage GetcurrencyCodedropdown()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetcurrencyCodedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeCurrency")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeCurrency(string currencyexchange_gid)
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetOnchangeCurrency(currencyexchange_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // post sundry expenses

        [ActionName("PostDirectsundryexpenses")]
        [HttpPost]
        public HttpResponseMessage PostDirectsundryexpenses(sundryexpenses_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostDirectsundryexpenses(getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //post product submit

        [ActionName("PostProductsundryexpenses")]
        [HttpPost]
        public HttpResponseMessage PostProductsundryexpenses(posttempledger_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostProductsundryexpenses(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // product summary
        [ActionName("GetTempProductsSummary")]
        [HttpGet]
        public HttpResponseMessage GetTempProductsSummary()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetTempProductsSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // View 
        [ActionName("GetSundryExpenseView")]
        [HttpGet]
        public HttpResponseMessage GetSundryExpenseView(string expense_gid)
        {
           
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetSundryExpenseView(expense_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Temp Products
        [ActionName("GetSundryExpenseViewProducts")]
        [HttpGet]
        public HttpResponseMessage GetSundryExpenseViewProducts(string expense_gid)
        {

            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetSundryExpenseViewProducts(expense_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // Edit
        [ActionName("GetSundryExpenseEdit")]
        [HttpGet]
        public HttpResponseMessage GetSundryExpenseEdit(string expense_gid)
        {

            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetSundryExpenseEdit(expense_gid,getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Edit Products
        [ActionName("GetSundryExpenseEditProducts")]
        [HttpGet]
        public HttpResponseMessage GetSundryExpenseEditProducts(string expense_gid)
        {

            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetSundryExpenseEditProducts(expense_gid,getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // UPDATE 
        [ActionName("UpdateSundryExpenses")]
        [HttpPost]
        public HttpResponseMessage UpdateSundryExpenses(sundryexpenses_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaUpdateSundryExpenses(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostProductUpdatesundryexpenses")]
        [HttpPost]
        public HttpResponseMessage PostProductUpdatesundryexpenses(sundryexpenses_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaPostProductUpdatesundryexpenses(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditTempProductsSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditTempProductsSummary()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objfinance.DaGetEditTempProductsSummary(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("SummaryProductDelete")]
        [HttpPost]
        public HttpResponseMessage SummaryProductDelete(sundryexpenses_list values)
        {
            objfinance.DaSummaryProductDelete(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("TempProductDelete")]
        [HttpPost]
        public HttpResponseMessage TempProductDelete(sundryexpenses_list values)
        {
            objfinance.DaTempProductDelete(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("EditTempProductDelete")]
        [HttpPost]
        public HttpResponseMessage EditTempProductDelete(sundryexpenses_list values)
        {
            objfinance.DaEditTempProductDelete(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // productSearch
        [ActionName("GetProductsearchSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummary(string producttype_gid, string product_name, string customer_gid)
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetProductsearchSummary(producttype_gid, product_name, customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAllLedgerList")]
        [HttpGet]
        public HttpResponseMessage GetAllLedgerList()
        {
            MdlAccTrnSundryExpenses values = new MdlAccTrnSundryExpenses();
            objfinance.DaGetAllLedgerList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}