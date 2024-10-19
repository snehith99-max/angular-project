using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ems.finance.Models
{
    public class MdlAccTrailBalanceReport : result
    {
        public List<GetTrialBalance_list> GetTrialBalance_list { get; set; }
        public List<TrialBalanceFolders> parent_folders { get; set; }
        public List<TrialBalanceFolders> sub_folders1 { get; set; }
    }
    public class GetTrialBalance_list
    {
        public string journal_gid { get; set; }
        public string transaction_date { get; set; }
        public string journal_refno { get; set; }
        public string remarks { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
    }
    public class TrialBalanceFolders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string accountgroup_gid { get; set; }
        public List<sub_folders1> sub_folders1 { get; set; } = new List<sub_folders1>();

    }

    public class sub_folders1
    {
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
    }

}
