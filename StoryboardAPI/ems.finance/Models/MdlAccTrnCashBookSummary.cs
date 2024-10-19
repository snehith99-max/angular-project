using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class MdlAccTrnCashBookSummary : result
    {
        public List<CashBook_list> CashBook_list { get; set; }
        public List<CashBookSelect_list> CashBookSelect_list { get; set; }
        public List<Getcashbookdtl_List> Getcashbookdtl_List { get; set; }
        public List<GetCashBookEntryView_List> GetCashBookEntryView_List { get; set; }
        public List<GetCashBookEnterBy_List> GetCashBookEnterBy_List { get; set; }
        public List<GetCashAccountMulAdd_List> GetCashAccountMulAdd_List { get; set; }
        public List<GetSubCash_list> GetSubCash_list { get; set; }
    }
    public class GetSubCash_list
    {
        public string journal_desc { get; set; }
        public string dr_cr { get; set; }
        public string transaction_amount { get; set; }
        public string remarks { get; set; }
        public string account_gid { get; set; }
        public string account_desc { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class cashbookadd_list
    {
        public string transaction_date { get; set; }
        public string bank_gid { get; set; }
        public string acct_refno { get; set; }
        public string direct_remarks { get; set; }
        public string dr_cr { get; set; }
        public string transaction_amount { get; set; }
        public string remarks { get; set; }
        public string branch_gid { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class GetCashAccountMulAdd_List
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
    public class acctmuladd_list
    {
        public string transaction_type { get; set; }
        public string txtremarks { get; set; }
        public string transaction_amount { get; set; }
        public string account_name { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class GetCashBookEnterBy_List : result
    {
        public string employee_name { get; set; }
    }
    public class GetCashBookEntryView_List : result
    {
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string gl_code { get; set; }
        public string account_type { get; set; }
        public string account_gid { get; set; }
    }
    public class cashbookedit_list
    {
        public string branch_gid { get; set; }
        public string parent_name { get; set; }
        public string branch_name { get; set; }
        public string gl_code { get; set; }
        public string externalgl_code { get; set; }
        public string openning_balance { get; set; }
        public string branch_code { get; set; }
        public string remarks { get; set; }
        public string transaction_date { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class Getcashbookdtl_List : result
    {
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string gl_code { get; set; }
        public string openning_balance { get; set; }
        public string externalgl_code { get; set; }
        public string parent_name { get; set; }
        public string transaction_date { get; set; }
        public string remarks { get; set; }
    }
    public class CashBook_list : result
    {
        public string externalgl_code { get; set; }
        public string branch_name { get; set; }
        public string openning_balance { get; set; }
        public string gl_code { get; set; }
        public string branch_code { get; set; }
        public string branch_gid { get; set; }
        public string closing_amount { get; set; }
    }

    public class CashBookSelect_list : result
    {
        public int s_no { get; set; }
        public string transaction_gid { get; set; }
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string branch_name { get; set; }
        public string account_desc { get; set; }
        public string remarks { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string closing_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string account_gid { get; set; }
        public string branch_gid { get; set; }
        public string account_name { get; set; }
        public string journal_type { get; set; }
        public string transaction_type { get; set; }
        public string transaction_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }

    }
}