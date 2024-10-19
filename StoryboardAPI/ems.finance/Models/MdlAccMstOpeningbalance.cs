using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ems.finance.Models
{
    public class MdlAccMstOpeningbalance: result
    {
        public List<Openingbalance_list> Openingbalance_list { get; set; }
        public List<Openingbalance_lists> Openingbalance_lists { get; set; }
        public List<GetParentName_List> GetParentName_List { get; set; }
        public List<GetParentNameasset_List> GetParentNameasset_List { get; set; }
        public List<Getliabilityacct_List> Getliabilityacct_List { get; set; }
        public List<Getassetacct_List> Getassetacct_List { get; set; }
        public List<entitydtl_lists> entitydtl_lists { get; set; }
        public List<LiabilityGroupName_lists> LiabilityGroupName_lists { get; set; }
        public List<LiabilityGroupName_lists> Liabilitysum_GroupName_lists { get; set; }
        public List<LiabilitySubGroupName_lists> LiabilitySubGroupName_lists { get; set; }
        public List<LiabilityLedgerName_lists> LiabilityLedgerName_lists { get; set; }
        public List<LiabilitySubGroupName_lists> AccountList { get; set; }
        public List<LiabilityLedgerName_lists> LedgerAccountList { get; set; }
        public List<AssetGroupName_lists> AssetGroupName_lists { get; set; }
        public List<AssetGroupName_lists> Assetsum_GroupName_lists { get; set; }
        public List<AssetSubGroupName_lists> AssetSubGroupName_lists { get; set; }
        public List<AssetSubGroupName_lists> AssetAccountList { get; set; }
        public List<AssetLedgerName_lists> AssetLedgerName_lists { get; set; }
        public List<AssetLedgerName_lists> AssetLedgerAccountList { get; set; }
        public List<branchdtl_lists> branchdtl_lists { get; set; }
        public List<liabilitySummary_lists> liabilitySummary_lists { get; set; }
        public List<liabilitysubled_Summary_lists> liabilitysubled_Summary_lists { get; set; }
        public List<AssetSummary_lists> AssetSummary_lists { get; set; }
        public List<Assetsubled_Summary_lists> Assetsubled_Summary_lists { get; set; }
    }
    public class entitydtl_lists
    {
        public string entity_gid { get; set; }
        public string entity_name { get; set; }
    }
    public class openingbalanceedit_list
    {
        public string editamount_value { get; set; }
        public string editremarks { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public string journaldtl_gid { get; set; }
        public string opening_balance_gid { get; set; }
    }
    public class assetopeningbalanceedit_list
    {
        public string editassetamount_value { get; set; }
        public string editassetremarks { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public string journaldtl_gid { get; set; }
        public string opening_balance_gid { get; set; }
    }
    public class openingbalance_list
    {
        public string date_value { get; set; }
        public string balance_type { get; set; }
        public string parent_name { get; set; }
        public string acctref_no { get; set; }
        public string remarks { get; set; }
        public string parent_gid { get; set; }
        public string branch_name { get; set; }
        public string liabilityaact_name { get; set; }
        public string amount_value { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public string finyear { get; set; }
        public string entity_name { get; set; }
    }
    public class Getassetacct_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }

    }
    public class Getliabilityacct_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }

    }
    public class GetParentName_List : result
    {
        public string parent_gid { get; set; }
        public string parent_name { get; set; }

    }
    public class GetParentNameasset_List : result
    {
        public string parent_gid { get; set; }
        public string parent_name { get; set; }

    }
    public class Openingbalance_list : result
    {
        public string accountgroup_name { get; set; }
        public string openningbalance_gid { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string credit_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string remarks { get; set; }
        public string openingfinancial_year { get; set; }
        public string entity_name { get; set; }
        public string opening_balance_gid { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string account_code { get; set; }
        public string sum_of_maingroup { get; set; }

    }
    public class Openingbalance_lists : result
    {
        public string openningbalance_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string account_name { get; set; }
        public string branch_name { get; set; }
        public string debit_amount { get; set; }
        public string journal_gid { get; set; }
        public string journaldtl_gid { get; set; }
        public string remarks { get; set; }
        public string openingfinancial_year { get; set; }
        public string entity_name { get; set; }
        public string opening_balance_gid { get; set; }
        public string MainGroup_name { get; set; }
        public string subgroup_name { get; set; }
        public string account_code { get; set; }
        public string sum_of_maingroup { get; set; }
    }
    public class LiabilityGroupName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string ledger_type { get; set; }
        public string account_gid { get; set; }
        public string sum_of_group { get; set; }
    }
    public class AssetGroupName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string ledger_type { get; set; }
        public string account_gid { get; set; }
        public string sum_of_group { get; set; }
    }
    public class LiabilitySubGroupName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string sum_of_subgroup { get; set; }
    }
    public class LiabilityLedgerName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string credit_amount { get; set; }
        public string openingfinancial_year { get; set; }
        public string opening_balance_gid { get; set; }
    }
    public class AssetSubGroupName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string sum_of_subgroup { get; set; }
    }
    public class AssetLedgerName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string debit_amount { get; set; }
        public string openingfinancial_year { get; set; }
        public string opening_balance_gid { get; set; }
    }
    public class branchdtl_lists
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }
    public class Liabilitysum_GroupName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string gl_code { get; set; }
        public string sum_of_subgroup { get; set; }
    }
    public class Assetsum_GroupName_lists : result
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string accountgroup_name { get; set; }
        public string account_gid { get; set; }
        public string gl_code { get; set; }
        public string sum_of_subgroup { get; set; }
    }
    public class liabilitySummary_lists : result
    {
        public string opening_balance_gid { get; set; }
        public string account_code { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
    }
    public class AssetSummary_lists : result
    {
        public string opening_balance_gid { get; set; }
        public string account_code { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
    }
    public class liabilitysubled_Summary_lists : result
    {
        public string opening_balance_gid { get; set; }
        public string subgroup_account_gid { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string opening_balance { get; set; }
        public string account_ref_no { get; set; }
        public string Subgroup_name { get; set; }
    }
    public class Assetsubled_Summary_lists : result
    {
        public string opening_balance_gid { get; set; }
        public string subgroup_account_gid { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string accountgroup_gid { get; set; }
        public string accountgroup_name { get; set; }
        public string opening_balance { get; set; }
        public string account_ref_no { get; set; }
        public string Subgroup_name { get; set; }
    }
}