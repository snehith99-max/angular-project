using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    //code by a snehith
    public class MdlProfitLossReport
    {
        public List<profitlossexcel_list> profitlossexcel_list { get; set; }
        public List<profitlosspdf_list> profitlosspdf_list { get; set; }
        public List<profitlossincome_list> profitlossincome_list { get; set; }
        public List<profitlossExpense_list> profitlossExpense_list { get; set; }
        public List<GetProfilelossfinyear_list> GetProfilelossfinyear_list { get; set; }
        public List<GetPlDetails_list> GetPlDetails_list { get; set; }
        public List<GetPlDetailspdf_list> GetPlDetailspdf_list { get; set; }
        public List<GetSummaryExpenseparent> GetSummaryExpenseparent { get; set; }
        public List<GetSummaryExpensechild> GetSummaryExpensechild { get; set; }


    }
    public class MdlFinanceExpenseFolders : result
    {
        public List<ExpenseFolders> parentfolders { get; set; }
        public List<ExpenseFolders> subfolders1 { get; set; }

        //public List<Folders> incomefolder { get; set; }

        public List<IncomeFolders> parentfoldersincome { get; set; }
        public List<IncomeFolders> subfolders2 { get; set; }

    }
    public class IncomeFolders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string accountgroup_gid { get; set; }
        public List<subfolders2> subfolders2 { get; set; } = new List<subfolders2>();

    }

    public class subfolders2
    {
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public List<subfolders2> subfolderslist { get; set; }

    }
    public class ExpenseFolders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string accountgroup_gid { get; set; }
        public List<subfolders1> subfolders1 { get; set; } = new List<subfolders1>();

    }

    public class subfolders1
    {
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public List<subfolders1> subfolderslist { get; set; }

    }
    public class GetSummaryExpenseparent : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
    }
    public class GetSummaryExpensechild : result
    {
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
    }
    public class GetPlDetailspdf_list : result
    {
        public string html_content { get; set; }
    }
    public class GetPlDetails_list : result
    {
        public string journal_gid { get; set; }
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string remarks { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string account_name { get; set; }
        public string overal_debit { get; set; }
        public string overal_credit { get; set; }
        
    }
    public class GetProfilelossfinyear_list : result
    {
        public string finyear { get; set; }
        public string finyear_gid { get; set; }
    }
    public class profitlossexcel_list : result
    {
        public string html_content { get; set; }
    }
    public class profitlosspdf_list : result
    {
        public string html_content { get; set; }
    }
    public class profitlossincome_list : result
    {
        public string html_content { get; set; }
        public string income_closebal { get; set; }
        
    }
    public class profitlossExpense_list : result
    {
        public string html_content { get; set; }
        public string expense_closebal { get; set; }
        
    }
    
}
    