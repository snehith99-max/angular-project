using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlAccTrnRecordExpenseSummary:result
    {
        public List<record_expense_list> record_expense_list { get; set; }
        public List<record_expense_list> Make_Payment_Group_list { get; set; }
        public List<expensemuladd_list> Make_Payment_list { get; set; }
        public List<vendor_list> vendor_list { get; set; }
        public List<accountgroupname_list> accountgroupname_list { get; set; }
        public List<vendordetails_list> vendordetails_list { get; set; }
        public List<accountgroup_lists> accountgroup_lists { get; set; }
        public List<expensemuladd_list> expensemuladd_list { get; set; }
        public List<expensedetailadd_list> expensedetailadd_list { get; set; }
    }
    public class record_expense_list : result
    {
        public string expenserequisition_gid { get; set; }
        public string expenserequisition_date { get; set; }
        public string expenserequisition_flag { get; set; }
        public string overall_status { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string total_amount { get; set; }
        public string editbank_name { get; set; }
        public string payment_mode { get; set; }
        public string payment_date { get; set; }
        public string transaction_number { get; set; }
        public string vendor { get; set; }
        public string vendor_gst { get; set; }
        public string contactperson_name { get; set; }
        public string due_date { get; set; }
        public string vendor_address { get; set; }
    }
    public class vendor_list : result
    {
        public string vendor_gid { get; set; }
        public string vendor_name { get; set; }
    }
    public class accountgroupname_list : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class vendordetails_list : result
    {
        public string vendor_address { get; set; }
        public string vendor_gst { get; set; }
    }
    public class accountgroup_lists : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class expensedetailadd_list
    {
        public string expenserequisition_date { get; set; }
        public string contactperson_name { get; set; }
        public string due_date { get; set; }
        public string vendor { get; set; }
        public string vendor_gst { get; set; }
        public string vendor_address { get; set; }
        public string document_attachments { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class expensemuladd_list
    {
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string expense_amount { get; set; }
        public string remarks { get; set; }
        public string claim_date { get; set; }
        public string transaction_amount { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class expensesubmitlist : result
    {
        public List<expensemuladd_list> expensemuladd_list { get; set; }
    }
}
