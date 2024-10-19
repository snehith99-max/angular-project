using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlIncomeEpenditureReport
    {
        public List<GVcreditNeedDataSource_list> GVcreditNeedDataSource_list { get; set; }
        public List<GVcreditDetailTable_list> GVcreditDetailTable_list { get; set; }
        public List<GetIncomeExcel_list> GetIncomeExcel_list { get; set; }
        public List<GVdebitNeedDataSource_list> GVdebitNeedDataSource_list { get; set; }
        public List<GVdebitDetailTable_list> GVdebitDetailTable_list { get; set; }
        public List<GetExpenseExcel_list> GetExpenseExcel_list { get; set; }
        public List<GetBarChartIncomeexpene_list> GetBarChartIncomeexpene_list { get; set; }
        public List<GVPoptransaction_list> GVPoptransaction_list { get; set; }
        

    }
    public class GVPoptransaction_list : result
    {
        public string expense_amount { get; set; }
        public string income_amount { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string net_amount { get; set; }
    }
    public class GetBarChartIncomeexpene_list : result
    {
        public string expense_amount { get; set; }
        public string income_amount { get; set; }
        public string month_name { get; set; }
    }
    public class GetExpenseExcel_list : result
    {
        public string html_content { get; set; }
    }
    public class GVdebitNeedDataSource_list : result
    {
        public string month { get; set; }
        public string year { get; set; }
        public string debit_amount { get; set; }
    }
    public class GVdebitDetailTable_list : result
    {
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string reference_type { get; set; }
        public string account_name { get; set; }
        public string remarks { get; set; }
        public string transaction_amount { get; set; }
        public string journal_type { get; set; }
    }
    public class GetIncomeExcel_list : result
    {
        public string html_content { get; set; }
    }
    public class GVcreditNeedDataSource_list : result
    {
        public string month { get; set; }
        public string year { get; set; }
        public string debit_amount { get; set; }
    }
    public class GVcreditDetailTable_list : result
    {
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string reference_type { get; set; }
        public string account_name { get; set; }
        public string remarks { get; set; }
        public string transaction_amount { get; set; }
        public string journal_type { get; set; }
    }
}