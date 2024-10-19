using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ems.finance.Models.BalanceSheetexcel_list;

namespace ems.finance.Models
{
    public class MdlBalanceSheetReport
    {
        public List<BalanceSheetexcel_list> BalanceSheetexcel_list { get; set; }
        public List<BalanceSheetpdf_list> BalanceSheetpdf_list { get; set; }
        public List<BalanceSheetliability_list> BalanceSheetliability_list { get; set; }
        public List<BalanceSheetasset_list> BalanceSheetasset_list { get; set; }
        public List<GetBalanceSheetfinyear_list> GetBalanceSheetfinyear_list { get; set; }
        public List<Balancesheetpdf_list> Balancesheetpdf_list { get; set; }
        public List<Balancesheetoverallnetvalue> Balancesheetoverallnetvalue { get; set; }
        

    }
    public class Balancesheetoverallnetvalue : result
    {
        public string lblpandl { get; set; }
        public string lblpandlvalue { get; set; }


    }
  
    public class MdlFinanceLiabilityFolders : result
    {
        public List<LiabilityFolders> parentfoldersliability { get; set; }
        public List<LiabilityFolders> subfolders3 { get; set; }

        //public List<Folders> incomefolder { get; set; }

        public List<AssetFolders> parentfoldersasset { get; set; }
        public List<AssetFolders> subfolders4 { get; set; }

    }
    public class AssetFolders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string accountgroup_gid { get; set; }
        public List<subfolders4> subfolders4 { get; set; } = new List<subfolders4>();

    }

    public class subfolders4
    {
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public List<subfolders4> subfolderslist { get; set; }

    }
    public class LiabilityFolders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public string accountgroup_gid { get; set; }
        public List<subfolders3> subfolders3 { get; set; } = new List<subfolders3>();

    }

    public class subfolders3
    {
        public string account_gid { get; set; }
        public string accountgroup_gid { get; set; }
        public string account_name { get; set; }
        public string credit_amount { get; set; }
        public string debit_amount { get; set; }
        public string opening_balance { get; set; }
        public string closing_balance { get; set; }
        public List<subfolders3> subfolderslist { get; set; }

    }
    public class GetBalanceSheetfinyear_list : result
    {
        public string finyear { get; set; }
        public string finyear_gid { get; set; }
    }
    public class Balancesheetpdf_list : result
    {
        public string html_content { get; set; }
    }

        public class BalanceSheetexcel_list : result
    {
        public string html_content { get; set; }
    }
    public class BalanceSheetpdf_list : result
    {
        public string html_content { get; set; }
    }
    public class BalanceSheetliability_list : result
    {
        public string html_content { get; set; }
        public string expense_closebal { get; set; }

    }
    public class BalanceSheetasset_list : result
    {
        public string html_content { get; set; }
        public string income_closebal { get; set; }
        

    }
}