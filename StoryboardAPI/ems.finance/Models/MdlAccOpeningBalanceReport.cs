using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ems.finance.Models
{
    public class MdlAccOpeningBalanceReport
    {
        public List<openingbalanceFolders> openingbalanceFolders { get; set; }

        public List<openingbalanceFolders> openingbalanceSubFolders { get; set; }
        public List<openingbalanceFolders> openingbalanceAssetFolders { get; set; }
        public List<openingbalanceFolders> openingbalanceAssetSubFolders { get; set; }


    }

  

    public class openingbalanceFolders : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string sum_debit { get; set; }
        public string sum_credit { get; set; }
        public string accountgroup_gid { get; set; }
        public List<openingbalancesubfolders> subfolders { get; set; } = new List<openingbalancesubfolders>();

    }

    public class openingbalancesubfolders :result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }

        public string sum_debit { get; set; }
        public string sum_credit { get; set; }
        public List<openingbalancesubfolders> subfolderslist { get; set; }

    }
}
