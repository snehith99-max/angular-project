using ems.finance.DataAccess;
using ems.finance.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ems.finance.Controllers
{
    [RoutePrefix("api/AccTrnBankbooksummary")]
    [Authorize]
    public class AccTrnBankbooksummaryController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccTrnBankbooksummary objfinance = new DaAccTrnBankbooksummary();

        [ActionName("GetBankBookSummary")]
        [HttpGet]   
        public HttpResponseMessage GetBankBookSummary()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetBankBookSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetAllBankBookSummary")]
        //[HttpGet]
        //public HttpResponseMessage GetAllBankBookSummary()
        //{
        //    MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
        //    objfinance.DaGetAllBankBookSummary(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);

        //}

        [ActionName("GetAllBankBookSummary")]
        [HttpGet]
        public HttpResponseMessage GetAllBankBookSummary()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAllBankBookSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetSubBankBook")]
        [HttpGet]
        public HttpResponseMessage GetSubBankBook(string bank_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetSubBankBook(values, bank_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBankBookAddSummary")]
        [HttpGet]
        public HttpResponseMessage GetBankBookAddSummary(string bank_gid)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetBankBookAddSummary(bank_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccTrnGroupDtl")]
        [HttpGet]
        public HttpResponseMessage GetAccTrnGroupDtl()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAccTrnGroupDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccTrnNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetAccTrnNameDtl()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAccTrnNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostProductGroupSummary")]
        [HttpPost]
        public HttpResponseMessage PostProductGroupSummary(accountfetch_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objfinance.DaPostProductGroupSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInputTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetInputTaxSummary(string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetInputTaxSummary(getsessionvalues.user_gid, values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOutputTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetOutputTaxSummary(string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetOutputTaxSummary(getsessionvalues.user_gid, values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCreditNoteTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetCreditNoteTaxSummary(string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetCreditNoteTaxSummary(getsessionvalues.user_gid, values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDebitNoteTaxSummary")]
        [HttpGet]
        public HttpResponseMessage GetDebitNoteTaxSummary(string from_date, string to_date)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDebitNoteTaxSummary(getsessionvalues.user_gid, values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDebitorReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetDebitorReportSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDebitorReportSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDebitorReportView")]
        [HttpGet]
        public HttpResponseMessage GetDebitorReportView(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDebitorReportView(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDebitorReportCustomerView")]
        [HttpGet]
        public HttpResponseMessage GetDebitorReportCustomerView(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDebitorReportCustomerView(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCreditorReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetCreditorReportSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetCreditorReportSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCreditorReportView1")]
        [HttpGet]
        public HttpResponseMessage GetCreditorReportView1(string account_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetCreditorReportView1(values, account_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCreditorReportView")]
        [HttpGet]
        public HttpResponseMessage GetCreditorReportView(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetCreditorReportView(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCreditorReportVendorView")]
        [HttpGet]
        public HttpResponseMessage GetCreditorReportVendorView(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetCreditorReportVendorView(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetAccountNameDropdown")]
        //[HttpGet]
        //public HttpResponseMessage GetAccountNameDropdown()
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
        //    objfinance.DaGetAccountNameDropdown(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}


        [ActionName("GetLiabilityLedgerNameDropdown")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityLedgerNameDropdown()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaLiabilityLedgerNameDropdown(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAssetLedgerNameDropdown")]
        [HttpGet]
        public HttpResponseMessage GetAssetLedgerNameDropdown()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaAssetLedgerNameDropdown(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncomeLedgerNameDropdown")]
        [HttpGet]
        public HttpResponseMessage GetIncomeLedgerNameDropdown()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaIncomeLedgerNameDropdown(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseLedgerNameDropdown")]
        [HttpGet]
        public HttpResponseMessage GetExpenseLedgerNameDropdown()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaExpenseLedgerNameDropdown(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //[ActionName("GetLedgerNameDropdown")]
        //[HttpGet]
        //public HttpResponseMessage GetLedgerNameDropdown()
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
        //    objfinance.DaLedgerNameDropdown(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}



        [ActionName("GetLedgerBookReport")]
        [HttpGet]
        public HttpResponseMessage GetLedgerBookReport(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetLedgerBookReport(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getdeletebankbookdtls")]
        [HttpGet]
        public HttpResponseMessage Getdeletebankbookdtls(string journal_gid, string journaldtl_gid, string account_gid)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetdeletebankbookdtls(values, journal_gid, journaldtl_gid, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetFinancialYear")]
        [HttpGet]
        public HttpResponseMessage GetFinancialYear()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetFinancialYear(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBankBookEntryView")]
        [HttpGet]
        public HttpResponseMessage GetBankBookEntryView(string bank_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetBankBookEntryView(getsessionvalues.user_gid, values, bank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccountGroupList")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroupList()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAccountGroupList(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccountNameList")]
        [HttpGet]
        public HttpResponseMessage GetAccountNameList()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAccountNameList(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAccountMulAddDtl")]
        [HttpGet]
        public HttpResponseMessage GetAccountMulAddDtl()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAccountMulAddDtl(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostAccountMulAddDtls")]
        [HttpPost]
        public HttpResponseMessage PostAccountMulAddDtls(acctmuladddtl_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objfinance.DaPostAccountMulAddDtls(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDeleteMulBankDtls")]
        [HttpGet]
        public HttpResponseMessage GetDeleteMulBankDtls(string session_id)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDeleteMulBankDtls(values, session_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostDirectBankBookEntry")]
        [HttpPost]
        public HttpResponseMessage PostDirectBankBookEntry(bankbookadd_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objfinance.DaPostDirectBankBookEntry(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCreditorReportOpeningBlnc")]
        [HttpGet]
        public HttpResponseMessage GetCreditorReportOpeningBlnc(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetCreditorReportOpeningBlnc(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDebtorReportOpeningBlnc")]
        [HttpGet]
        public HttpResponseMessage GetDebtorReportOpeningBlnc(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDebtorReportOpeningBlnc(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        
        
        [ActionName("GetFinanceDashboardCount")]
        [HttpGet]
        public HttpResponseMessage GetFinanceDashboardCount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetFinanceDashboardCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDebtorDetailedReport")]
        [HttpGet]
        public HttpResponseMessage GetDebtorDetailedReport(string account_gid, string customer_gid,string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetDebtorDetailedReport(account_gid, customer_gid,from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("JournalEntryBookReport")]
        [HttpGet]
        public HttpResponseMessage JournalEntryBookReport()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaJournalEntryBookReport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncomeLedgerReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetIncomeSummary()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaIncomeLedgerReportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseLedgerReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseLedgerReportSummary()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaExpenseLedgerReportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAssetLedgerReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssetLedgerReportSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaAssetLedgerReportSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAssetLedgerReportDetails")]
        [HttpGet]
        public HttpResponseMessage GetAssetLedgerReportDetails(string account_gid, string customer_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaAssetLedgerReportDetails(account_gid, customer_gid, from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLiabilityLedgerReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityLedgerReportSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaLiabilityLedgerReportSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLiabilityLedgerReportDetails")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityLedgerReportDetails(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaLiabilityLedgerReportDetails(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLiabilityLedgerReportDetailsList")]
        [HttpGet]
        public HttpResponseMessage GetLiabilityLedgerReportDetailsList(string account_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaLiabilityLedgerReportDetailsList(values, account_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncomeLedgerReportDetailsList")]
        [HttpGet]
        public HttpResponseMessage GetIncomeLedgerReportDetailsList(string account_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaIncomeLedgerReportDetailsList(values, account_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncomeLedgerReportDetails")]
        [HttpGet]
        public HttpResponseMessage GetIncomeLedgerReportDetails(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaIncomeLedgerReportDetails(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetIncomeReportMonthwise")]
        [HttpGet]
        public HttpResponseMessage GetIncomeReportMonthwise(string account_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaIncomeReportMonthwise(values, account_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseLedgerReportDetailsList")]
        [HttpGet]
        public HttpResponseMessage GetExpenseLedgerReportDetailsList(string account_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaExpenseLedgerReportDetailsList(values, account_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseLedgerReportDetails")]
        [HttpGet]
        public HttpResponseMessage GetExpenseLedgerReportDetails(string account_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaExpenseLedgerReportDetails(getsessionvalues.user_gid, values, account_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetExpenseReportMonthwise")]
        [HttpGet]
        public HttpResponseMessage GetExpenseReportMonthwise(string account_gid, string from_date, string to_date)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaExpenseReportMonthwise(values, account_gid, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetAccountGroupNameDropdown")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroupNameDropdown()
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetAccountGroupNameDropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLedgerNameDropDownList")]
        [HttpGet]
        public HttpResponseMessage GetLedgerNameDropDownList(string Subgroup_gid)
        {
            MdlAccTrnBankbooksummary values = new MdlAccTrnBankbooksummary();
            objfinance.DaGetLedgerNameDropDownList(values, Subgroup_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
