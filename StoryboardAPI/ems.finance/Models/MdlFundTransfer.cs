using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlFundTransfer
    {
        public List<GetFundTransfer_list> GetFundTransfer_list { get; set; }
        public List<FundTransfer_list> FundTransfer_list { get; set; }
    }
    public class FundTransfer_list : result
    {

        public string created_date { get; set; }
        public string frombranch { get; set; }
        public string tobranch { get; set; }
        public string transfer_amount { get; set; }
        public string remarks { get; set; }
        public string pettycash_gid { get; set; }


    }
    public class GetFundTransfer_list : result
    {
        public string pettycash_gid { get; set; }
        public string transaction_amount { get; set; }
        public string user_firstname { get; set; }
        public string transaction_date { get; set; }
        public string approval_flag { get; set; }
        public string from_branch { get; set; }
        public string from_branch_gid { get; set; }
        public string to_branch { get; set; }
        public string to_branch_gid { get; set; }
        public string remarks { get; set; }
        public string reason { get; set; }


    }
}