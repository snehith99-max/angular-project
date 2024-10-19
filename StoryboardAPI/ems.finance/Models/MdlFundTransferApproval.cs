using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlFundTransferApproval
    {
        public List<GetFundTransferApproval_list> GetFundTransferApproval_list { get; set; }
        public List<FundTransferApproval_list> FundTransferApproval_list { get; set; }
    }
    public class GetFundTransferApproval_list : result
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
    public class FundTransferApproval_list : result
    {

        public string status_flag { get; set; }
        public string reason { get; set; }
        public string remarks { get; set; }
        public string pettycashgid { get; set; }


    }
}