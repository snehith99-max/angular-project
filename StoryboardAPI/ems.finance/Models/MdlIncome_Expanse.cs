using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlIncome_Expanse
    {
        public List<IncomeSummary_list> IncomeSummary_list { get; set; }
        public List<ExpenseSummary_list> ExpenseSummary_list { get; set; }
        public List<GetIncome_Expense_list> GetIncome_Expense_list { get; set; }
    }
    public class GetIncome_Expense_list : result
    {
        public string transaction_date { get; set; }
        public string transaction_amount { get; set; }
        public string source { get; set; }
    }
    public class ExpenseSummary_list : result
    {
        public string openningbalance_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string debit_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string remarks { get; set; }
        public string accountgroup_gid { get; set; }
        public string entity_name { get; set; }
        public string account_gid { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string journal_from { get; set; }
        public string account_code { get; set; }
        public string salestype_name { get; set; }
        public string sum_of_maingroup { get; set; }
        public string credit_august { get; set; }
        public string credit_july { get; set; }
        public string credit_june { get; set; }
        public string credit_may { get; set; }
        public string credit_march { get; set; }
        public string credit_april { get; set; }
        public string purchasetype_name { get; set; }
        public string transaction_year { get; set; }
        public string transaction_amount { get; set; }
    } public class IncomeSummary_list : result
    {
        public string openningbalance_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string debit_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string remarks { get; set; }
        public string accountgroup_gid { get; set; }
        public string entity_name { get; set; }
        public string account_gid { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string journal_from { get; set; }
        public string account_code { get; set; }
        public string salestype_name { get; set; }
        public string sum_of_maingroup { get; set; }
        public string credit_august { get; set; }
        public string credit_july { get; set; }
        public string credit_june { get; set; }
        public string credit_may { get; set; }
        public string credit_march { get; set; }
        public string credit_april { get; set; }
        public string transaction_year { get; set; }
        public string transaction_amount { get; set; }
    }
}