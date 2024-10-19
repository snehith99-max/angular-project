using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{  
    // Code By snehith
    public class MdlChartofAccount
    {
        public List<Getchartofaccount_list> Getchartofaccount_list { get; set; }
        public List<Getchartofsubaccount_list> Getchartofsubaccount_list { get; set; }
        public List<Getchartofaccountasset_list> Getchartofaccountasset_list { get; set; }
        public List<Getchartofaccountliability_list> Getchartofaccountliability_list { get; set; }
        public List<Getchartofaccountincome_list> Getchartofaccountincome_list { get; set; }
        public List<chartstatus_lists> chartstatus_lists { get; set; }
        public List<chartaccount_list> chartaccount_list { get; set; }
        public List<chartgroupupdate_list> chartgroupupdate_list { get; set; }
        public List<chartsubaccount_list> chartsubaccount_list { get; set; }
        public List<Getchartofaccountcount_list> Getchartofaccountcount_list { get; set;  }
    }
    public class Getchartofaccountcount_list : result
    {
        public string expense_totalcount { get; set; }
        public string income_totalcount { get; set; }
        public string liability_totalcount { get; set; }
        public string asset_totalcount { get; set; }
    }
    public class chartsubaccount_list : result
    {
        public string account_gid { get; set; }
        public string accountcodes { get; set; }
        public string accountgroups { get; set; }
        public string accountsubcode { get; set; }
        public string accountsubgroup { get; set; }
        public string ledger_flag { get; set; }
        public string accountType { get; set; }
        public string displayType { get; set; }
    }
    public class chartgroupupdate_list : result
    {
        public string account_groupname { get; set; }
        public string account_groupgid { get; set; }
        public string account_groupcode { get; set; }
    }
    public class chartaccount_list : result
    {
        public string accountType { get; set; }
        public string accountcode { get; set; }
        public string accountgroup { get; set; }
        public string displayType { get; set; }
    }
    public class chartstatus_lists
    {
        public string message { get; set; }
        public bool status { get; set; }
    }
    public class Getchartofaccountincome_list : result
    {
        public string account_gid { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string gl_code { get; set; }
        public string accountgroup_name { get; set; }
        public string display_type { get; set; }
        public string has_child { get; set; }
        public string ledger_type { get; set; }
        public string group_id { get; set; }
    }
    public class Getchartofaccountliability_list : result
    {
        public string account_gid { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string gl_code { get; set; }
        public string accountgroup_name { get; set; }
        public string display_type { get; set; }
        public string has_child { get; set; }
        public string ledger_type { get; set; }
        public string group_id { get; set; }
    }
    public class Getchartofaccountasset_list : result
    {
        public string account_gid { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string gl_code { get; set; }
        public string accountgroup_name { get; set; }
        public string display_type { get; set; }
        public string has_child { get; set; }
        public string ledger_type { get; set; }
        public string group_id { get; set; }
    }
    public class Getchartofaccount_list : result
    {
        public string account_gid { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string gl_code { get; set; }
        public string accountgroup_name { get; set; }
        public string display_type { get; set; }
        public string has_child { get; set; }
        public string ledger_type { get; set; }
        public string group_id { get; set; }
    }
    public class Getchartofsubaccount_list : result
    {
        public string account_gid { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string gl_code { get; set; }
        public string accountgroup_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string display_type { get; set; }
        public string has_child { get; set; }
        public string ledger_type { get; set; }
    }
    //new code
    public class Module
    {
        public string account_gid { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string[] mainfolder_list { get; set; }
    }
    public class Getsubfolder_Lists
    {
        public string accountGroup_gid { get; set; }        
    }

    public class MdlFinanceFolders : result
    {
        public List<Folders> parentfolders { get; set; }
        public List<Folders> subfolders { get; set; }
        public List<Folders> incomefolder { get; set; }
        public List<Folders> assetfolder { get; set; }
        public List<Folders> liabilityfolder { get; set; }
    }
    public class Folders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string account_code { get; set; }
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string has_child { get ; set; }
        public string group_id { get; set; }
        public string account { get; set; }
        public List<subfolders> subfolders { get; set; } = new List<subfolders>();
    }

    public class subfolders
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string account_code { get; set; }
        public string accountgroup_gid { get; set; }
        public string account { get; set; }
        public List<subfolders> subfolderslist { get; set; }
    }
}