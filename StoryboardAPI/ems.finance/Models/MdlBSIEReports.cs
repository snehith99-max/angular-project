using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlBSIEReports
    {
        public List<Liability_list> Liability_list { get; set; }
        public List<Asset_list> Asset_list { get; set; }
        public List<LedgerView_list> LedgerView_list { get; set; }
        public List<Income_list> Income_list { get; set; }
        public List<Expense_list> Expense_list { get; set; }
    }
    public class Liability_list : result
    {
        public string accountgroup_name { get; set; }
        public string openningbalance_gid { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string credit_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string remarks { get; set; }
        public string openingfinancial_year { get; set; }
        public string entity_name { get; set; }
        public string opening_balance_gid { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string account_code { get; set; }
        public string account_gid { get; set; }
        public string sum_of_maingroup { get; set; }

    }
    public class Asset_list : result
    {
        public string openningbalance_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string debit_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string remarks { get; set; }
        public string openingfinancial_year { get; set; }
        public string entity_name { get; set; }
        public string opening_balance_gid { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string account_code { get; set; }
        public string sum_of_maingroup { get; set; }
    }
    public class LedgerView_list : result
    {
        public string opening_balance_gid { get; set; }
        public string account_gid { get; set; }
        public string opening_balance { get; set; }
        public string account_ref_no { get; set; }
        public string created_date { get; set; }
        public string account_name { get; set; }
    }
    public class Income_list : result
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
        public string Sum_of_maingroup { get; set; }
    }
    public class Expense_list : result
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
    }
}