using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ems.finance.Models
{
    public class MdlAccMstCreditCard
    {
        public List<Getacctgroupname_List> Getacctgroupname_List { get; set; }
        public List<Getcreditcard_List> Getcreditcard_List { get; set; }
    }
    public class Getcreditcard_List
    {
        public string bank_gid { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string cardholder_name { get; set; }
        public string creditcard_no { get; set; }
        public string openning_balance { get; set; }
        public string default_flag { get; set; }
        public string status_flag { get; set; }
    }
    public class creditcard_list
    {
        public string date_value { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string card_number { get; set; }
        public string cardholder_name { get; set; }
        public string opening_balance { get; set; }
        public string account_group { get; set; }
        public string remarks { get; set; }
        public string transaction_type { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
    }
    public class Getacctgroupname_List
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class creditcarddtledit_list
    {
        public string editbank_name { get; set; }
        public string editcard_number { get; set; }
        public string editcardholder_name { get; set; }
        public string default_account { get; set; }
        public string bank_gid { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
}