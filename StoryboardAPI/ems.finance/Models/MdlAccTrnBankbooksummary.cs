//using StoryboardAPI.Models;
using System.Collections.Generic;

namespace ems.finance.Models
{
    public class MdlAccTrnBankbooksummary : result

    {
        public List<Getbankbook_list> Getbankbook_list { get; set; }
        public List<Getsubbankbook_list> Getsubbankbook_list { get; set; }
        public List<Getbank_list> addbank_list { get; set; }
        public List<GetAccountGroupDropdown> GetAccTrnGroupDtl { get; set; }
        public List<GetAccountNameDropdown> GetAccTrnNameDtl { get; set; }
        public List<accountfetch_list> accountfetch_list { get; set; }
        public List<GetInputTaxSummaryList> GetInputTaxSummaryList { get; set; }
        public List<GetOutputTaxSummaryList> GetOutputTaxSummaryList { get; set; }
        public List<GetCreditNoteTaxSummaryList> GetCreditNoteTaxSummaryList { get; set; }
        public List<GetDebitNoteTaxSummaryList> GetDebitNoteTaxSummaryList { get; set; }
        public List<GetDebitorreport_List> GetDebitorreport_List { get; set; }
        public List<GetAssetLedgerReportSummary_List> GetAssetLedgerReportSummary_List { get; set; }
        public List<GetLiabilityLedgerReportSummary_List> GetLiabilityLedgerReportSummary_List { get; set; }
        public List<GetDebitorreportView_List> GetDebitorreportView_List { get; set; }
        public List<GetDebitorreportCustomerView_List> GetDebitorreportCustomerView_List { get; set; }
        public List<GetCreditorreport_List> GetCreditorreport_List { get; set; }
        public List<GetCreditorreportView_List> GetCreditorreportView_List { get; set; }        
        public List<GetCreditorreportVendorView_List> GetCreditorreportVendorView_List { get; set; }
        public List<Getledgername_List> Getledgername_List { get; set; }
        public List<GetLiabilityaccountname_List> GetLiabilityaccountname_List { get; set; }
        public List<GetAssetaccountname_List> GetAssetaccountname_List { get; set; }
        public List<GetIncomeaccountname_List> GetIncomeaccountname_List { get; set; }
        public List<GetExpenseaccountname_List> GetExpenseaccountname_List { get; set; }
        public List<GetLedgerbook_List> GetLedgerbook_List { get; set; }
        public List<GetFinancialYear_List> GetFinancialYear_List { get; set; }
        public List<bankbookdtl_list> bankbookdtl_list { get; set; }
        public List<GetBankBookEntryView_List> GetBankBookEntryView_List { get; set; }
        public List<GetAccountGroup_List> GetAccountGroup_List { get; set; }
        public List<GetAccount_List> GetAccount_List { get; set; }
        public List<GetAccountMulAdd_List> GetAccountMulAdd_List { get; set; }
        public List<acctmuladddtl_list> acctmuladddtl_list { get; set; }
        public List<bankbookadd_list> bankbookadd_list { get; set; }
        public List<GetSubbank_list> GetSubbank_list { get; set; }
        public List<GetCreditorreportOpening_List> GetCreditorreportOpening_List { get; set; }
        public List<GetDebtorreportOpening_List> GetDebtorreportOpening_List { get; set; }
        public List<getFinanceDashboardCount_List> getFinanceDashboardCount_List { get; set; }
        public List<GetJournalEntrybook_list> GetJournalEntrybook_list { get; set; }
        public List<Getassetledgerreport_list> Getassetledgerreport_list { get; set; }
        public List<GetLiabilityLedgerReportDetails_List> GetLiabilityLedgerReportDetails_List { get; set; }
        public List<GetLiabilityLedgerReportView_List> GetLiabilityLedgerReportView_List { get; set; }
        public List<IncomeLedgerReport_list> IncomeLedgerReport_list { get; set; }
        public List<ExpenseLedgerReport_list> ExpenseLedgerReport_list { get; set; }
        public List<GetIncomeLedgerReportView_List> GetIncomeLedgerReportView_List { get; set; }
        public List<GetIncomeLedgerReportDetails_List> GetIncomeLedgerReportDetails_List { get; set; }
        public List<GetIncomeReportMonthwise_List> GetIncomeReportMonthwise_List { get; set; }
        public List<GetExpenseLedgerReportView_List> GetExpenseLedgerReportView_List { get; set; }
        public List<GetExpenseLedgerReportDetails_List> GetExpenseLedgerReportDetails_List { get; set; }
        public List<GetExpenseReportMonthwise_List> GetExpenseReportMonthwise_List { get; set; }
        public List<GetAccountNameDropdown> GetAccountNameDropdown { get; set; }
    }
    public class getFinanceDashboardCount_List
    {
        public string bank_count { get; set; }
        public string creditcard_count { get; set; }
        public string bankbook_count { get; set; }
        public string cashbook_count { get; set; }
        public string journalentry_count { get; set; }
        public string tax_count { get; set; }
        public string totaldebtor_count { get; set; }
        public string totalcreditor_count { get; set; }
        public string fundtransfer_count { get; set; }
        public string fundpending_count { get; set; }
        public string fundapproved_count { get; set; }
        public string fundrejected_count { get; set; }
    }
    public class GetCreditorreportOpening_List
    {
        public string opening_balance { get; set; }
    }
    public class GetDebtorreportOpening_List
    {
        public string opening_balance { get; set; }
    }
    public class GetSubbank_list
    {
        public string journal_desc { get; set; }
        public string dr_cr { get; set; }
        public string transaction_amount { get; set; }
        public string remarks { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class bankbookadd_list
    {
        public string transaction_date { get; set; }
        public string bank_gid { get; set; }
        public string acct_refno { get; set; }
        public string direct_remarks { get; set; }
        public string dr_cr { get; set; }
        public string transaction_amount { get; set; }
        public string remarks { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class GetAccountMulAdd_List
    {
        public string session_id { get; set; }
        public string transaction_amount { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string remarks { get; set; }
        public string dr_cr { get; set; }
    }
    public class acctmuladddtl_list
    {
        public string transaction_type { get; set; }
        public string txtremarks { get; set; }
        public string transaction_amount { get; set; }
        public string account_name { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class GetAccountGroup_List
    {
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
    }
    public class GetAccount_List
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetBankBookEntryView_List
    {
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string gl_code { get; set; }
        public string account_no { get; set; }
        public string account_type { get; set; }
        public string ifsc_code { get; set; }
        public string neft_code { get; set; }
        public string swift_code { get; set; }
        public string account_gid { get; set; }
    }
    public class GetFinancialYear_List : result
    {
        public string finyear_gid { get; set; }
        public string finyear { get; set; }
    }
    public class GetLedgerbook_List : result
    {
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string account_desc { get; set; }
        public string type { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string closing_amount { get; set; }
    }
    public class Getledgername_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }    
    public class GetLiabilityaccountname_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetAssetaccountname_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetIncomeaccountname_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetExpenseaccountname_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetDebitorreportCustomerView_List : result
    {
        public string customer_name { get; set; }
    }
    public class GetCreditorreportVendorView_List : result
    {
        public string vendor_name { get; set; }
        public string vendor_code { get; set; }
    }
    public class GetCreditorreportView_List : result
    {
        public string transaction_date { get; set; }
        public string customer_name { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string account_name { get; set; }
        public string bank_name { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
    }
    public class GetCreditorreport_List : result
    {
        public string account_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
        public string contact_details { get; set; }
        public string account_name { get; set; }
        public string vendor_code { get; set; }
        public string vendor_gid { get; set; }
    }

    public class GetDebitorreport_List : result
    {
        public string account_gid { get; set; }
        public string customer_name { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
        public string customer_id { get; set; }
        public string contact { get; set; }
        public string customer_since { get; set; }
        public string customer_gid { get; set; }
        public string tds { get; set; }
    }
    public class GetAssetLedgerReportSummary_List : result
    {
        public string account_gid { get; set; }
        public string customer_name { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
        public string customer_id { get; set; }
        public string contact { get; set; }
        public string customer_since { get; set; }
        public string customer_gid { get; set; }
        public string subgroup_name { get; set; }
        public string MainGroup_name { get; set; }
    }

    public class GetLiabilityLedgerReportSummary_List : result
    {
        public string account_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
        public string contact_details { get; set; }
        public string account_name { get; set; }
        public string vendor_code { get; set; }
        public string vendor_gid { get; set; }
        public string subgroup_name { get; set; }
        public string MainGroup_name { get; set; }
    }
    public class GetDebitorreportView_List : result
    {
        public string transaction_date { get; set; }
        public string customer_name { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
    }
    public class GetInputTaxSummaryList : result
    {
        public string invoice_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string vendor_name { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_2point5 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_2point5 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string invoice_amount { get; set; }
        public string Taxable_Amount { get; set; }
        public string Non_Taxable_Amount { get; set; }
    }
    public class GetOutputTaxSummaryList : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string invoice_date { get; set; }
        public string customer_name { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_2point5 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_2point5 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string invoice_amount { get; set; }
        public string Taxable_Amount { get; set; }
        public string Non_Taxable_Amount { get; set; }
        
    }
    public class GetCreditNoteTaxSummaryList : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string credit_date { get; set; }
        public string customer_name { get; set; }
        public string invoice_amount { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_2point5 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_2point5 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string Taxable_Amount { get; set; }
    }
    public class GetDebitNoteTaxSummaryList : result
    {
        public string invoice_gid { get; set; }
        public string invoice_refno { get; set; }
        public string debit_date { get; set; }
        public string vendor_name { get; set; }
        public string invoice_amount { get; set; }
        public string gst_number { get; set; }
        public string SGST_0 { get; set; }
        public string SGST_2point5 { get; set; }
        public string SGST_6 { get; set; }
        public string SGST_9 { get; set; }
        public string SGST_14 { get; set; }
        public string CGST_0 { get; set; }
        public string CGST_2point5 { get; set; }
        public string CGST_6 { get; set; }
        public string CGST_9 { get; set; }
        public string CGST_14 { get; set; }
        public string IGST_0 { get; set; }
        public string IGST_5 { get; set; }
        public string IGST_12 { get; set; }
        public string IGST_18 { get; set; }
        public string IGST_28 { get; set; }
        public string Taxable_Amount { get; set; }
    }
    public class Getbankbook_list : result
    {
        public string account_gid { get; set; }
        public string bank_gid { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string branch_name { get; set; }
        public string account_no { get; set; }
        public string ifsc_code { get; set; }
        public string neft_code { get; set; }
        public string swift_code { get; set; }
        public string openning_balance { get; set; }
        public string closing_amount { get; set; }
    }
    public class Getsubbankbook_list : result
    {
        public int s_no { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string transaction_gid { get; set; }
        public string bank_gid { get; set; }
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string account_no { get; set; }
        public string account_name { get; set; }
        public string remarks { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string journal_type { get; set; }
        public string transaction_type { get; set; }
        public string bank_name { get; set; }
        public string transaction_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string tds { get; set; }
    }
    public class bankbookdtl_list : result
    {
        public string account_gid { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
    }
    public class Getbank_list : result
    {
        public string account_gid { get; set; }
        public string bank_gid { get; set; }
        public string bank_name { get; set; }
        public string account_no { get; set; }
        public string account_type { get; set; }
        public string ifsc_code { get; set; }
        public string neft_code { get; set; }
        public string swift_code { get; set; }
        public string gl_code { get; set; }
    }
    public class GetAccountGroupDropdown : result
    {
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
    }
    public class GetAccountNameDropdown : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class accountfetch_list : result
    {
        public string session_id { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string dr_cr { get; set; }
        public string transaction_amount { get; set; }
        public string journal_desc { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }        
    }
    public class GetJournalEntrybook_list : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
        public string account_subgroup { get; set; }
        public string opening_amount { get; set; }
    }
    public class Getassetledgerreport_list : result
    {
        public int s_no { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string transaction_gid { get; set; }
        public string bank_gid { get; set; }
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string account_no { get; set; }
        public string account_name { get; set; }
        public string remarks { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string journal_type { get; set; }
        public string transaction_type { get; set; }
        public string bank_name { get; set; }
        public string transaction_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string subgroup_name { get; set; }
        public string MainGroup_name { get; set; }
    }
    public class GetLiabilityLedgerReportDetails_List : result
    {
        public string vendor_name { get; set; }
        public string vendor_code { get; set; }
    }
    public class GetLiabilityLedgerReportView_List : result
    {
        public string transaction_date { get; set; }
        public string customer_name { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string account_name { get; set; }
        public string bank_name { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
    }

    public class IncomeLedgerReport_list : result
    {
        public string journal_gid { get; set; }
        public string transaction_date { get; set; }
        public string branch_gid { get; set; }
        public string journal_year { get; set; }
        public string transaction_gid { get; set; }
        public string transaction_amount { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
    }

    public class ExpenseLedgerReport_list : result
    {
        public string journal_gid { get; set; }
        public string transaction_date { get; set; }
        public string branch_gid { get; set; }
        public string journal_year { get; set; }
        public string transaction_gid { get; set; }
        public string transaction_amount { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string opening_amount { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string closing_amount { get; set; }
    }

    public class GetIncomeLedgerReportView_List : result
    {
        public string transaction_date { get; set; }
        public string customer_name { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string account_name { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
        public string customer_code { get; set; }
    }

    public class GetIncomeLedgerReportDetails_List : result
    {
        public string customer_code { get; set; }
        public string customer_name { get; set; }
    }
    public class GetIncomeReportMonthwise_List : result
    {
        public string transaction_date { get; set; }
        public string customer_name { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string account_name { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
        public string customer_code { get; set; }
    }
    public class GetExpenseLedgerReportView_List : result
    {
        public string transaction_date { get; set; }
        public string vendor_companyname { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string account_name { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
        public string vendor_code { get; set; }
    }
    public class GetExpenseLedgerReportDetails_List : result
    {
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
    }
    public class GetExpenseReportMonthwise_List : result
    {
        public string transaction_date { get; set; }
        public string remarks { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string account_name { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string closingbalance { get; set; }
        public string openingbalance { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
    }
}
