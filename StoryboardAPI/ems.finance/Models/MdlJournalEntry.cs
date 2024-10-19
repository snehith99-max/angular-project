using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlJournalEntry
    {
        public List<GetJournalEntry_list> GetJournalEntry_list { get; set; }
        public List<GetJournalEntry_lists> GetJournalEntry_lists { get; set; }
        public List<GetJournalTransaction_list> GetJournalTransaction_list { get; set; }
        public List<GetJournalTransactions_list> GetJournalTransactions_list { get; set; }
        public List<GetAccountGroupDetails> GetAccountGroupDetails { get; set; }
        public List<GetAccountNameDetails> GetAccountNameDetails { get; set; }
        public List<postjournal_list> postjournal_list { get; set; }
        public List<GetJournalEntryedit_lists> GetJournalEntryedit_lists { get; set; }
        public List<GetJournalEntryeditdtl_lists> GetJournalEntryeditdtl_lists { get; set; }
        public List<updatejournal_list> updatejournal_list { get; set; }
        public List<Jounralsummary_list> Jounralsummary_list { get; set; }
    }
    public class Jounralsummary_list : result
    {
        public string html_content { get; set; }
    }
    public class GetJournalEntryedit_lists : result
    {
        public string journal_gid { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string transaction_date { get; set; }
        public string remarks { get; set; }
        public string document_path { get; set; }
        public string branch_gid { get; set; }

    }
    public class updatejournal_list : result
    {
        public string branch_name { get; set; }
        public string created_date { get; set; }
        public string remarks { get; set; }
        public string journal_gid { get; set; }
        public string journal_refno { get; set; }
        public detailsedit[] detailsedit { get; set; }
    }
    public class detailsedit : result
    {
        public string accountGroup { get; set; }
        public string accountName { get; set; }
        public string creditAmount { get; set; }
        public string debitAmount { get; set; }
        public string particulars { get; set; }
        public string journaldtl_gid { get; set; }


    }
    public class postjournal_list : result
    {
        public string branch_name { get; set; }
        public string created_date { get; set; }
        public string remarks { get; set; }
        public details[] details { get; set; }
    }
    public class details : result
    {
        public string accountGroup { get; set; }
        public string accountName { get; set; }
        public string creditAmount { get; set; }
        public string debitAmount { get; set; }
        public string particulars { get; set; }
    }
    public class GetJournalEntryeditdtl_lists : result
    {
        public string accountGroup { get; set; }
        public string accountName { get; set; }
        public string creditAmount { get; set; }
        public string debitAmount { get; set; }
        public string particulars { get; set; }
        public string journaldtl_gid { get; set; }

    }
    public class results
    {
        public string message { get; set; }
        public bool status { get; set; }
    }
    public class GetAccountNameDetails : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetAccountGroupDetails : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetJournalTransactions_list : result
    {
        public string voucher_type { get; set; }
        public string remarks { get; set; }
        public string journaldtl_gid { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string total_credit_amount { get; set; }
        public string total_debit_amount { get; set; }
        public string journal_gid { get; set; }
        public string account_gid { get; set; }


    }
    public class GetJournalEntry_lists : result
    {
        public string journal_gid { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string transaction_date { get; set; }
        public string remarks { get; set; }
        public string document_path { get; set; }
        public string subgroup_name { get; set; }
        public string MainGroup_name { get; set; }
        public List<GetJournalTransactions_list> GetJournalTransactions_list { get; set; } // Use List instead of array



    }
    public class GetJournalEntry_list : result
    {
        public string journal_gid { get; set; }
        public string transaction_type { get; set; }
        public string journal_refno { get; set; }
        public string transaction_date { get; set; }
        public string remarks { get; set; }

    }
    public class GetJournalTransaction_list : result
    {
        public string journal_gid { get; set; }
        public string voucher_type { get; set; }
        public string journaldtl_gid { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string account_gid { get; set; }
        public string remarks { get; set; }

    }
}