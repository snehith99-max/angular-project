using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.finance.Models
{
    public class MdlGLCode : result
    {
        public List<GetDebtorSummary_List> GetDebtorSummary_List { get; set; }
        public List<GetCreditorSummary_List> GetCreditorSummary_List { get; set; }
        public List<GetTaxPayableSummary_List> GetTaxPayableSummary_List { get; set; }
        public List<GetAssetDtlsSummary_List> GetAssetDtlsSummary_List { get; set; }
        public List<GetSalesTypeSummary_List> GetSalesTypeSummary_List { get; set; }
        public List<GetPurchaseTypeSummary_List> GetPurchaseTypeSummary_List { get; set; }
        public List<GetDepartmentSummary_List> GetDepartmentSummary_List { get; set; }
        public List<GetAcctMappingSummary_List> GetAcctMappingSummary_List { get; set; }
        public List<GetExpenseGroupSummary_List> GetExpenseGroupSummary_List { get; set; }
        public List<GetMappingAcctTo_List> GetMappingAcctTo_List { get; set; }
        public List<GetPurchaseMappingAcctTo_List> GetPurchaseMappingAcctTo_List { get; set; }
        public List<GetExpenseMappingAcct_List> GetExpenseMappingAcct_List { get; set; }
        public List<GetAccountMapping_List> GetAccountMapping_List { get; set; }
        public List<GetRegionName_List> GetRegionName_List { get; set; }
        public List<GetCountryName_List> GetCountryName_List { get; set; }
        public List<taxchart_list> taxchart_list { get; set; }
        public List<taxsegmentmapping_list> taxsegmentmapping_list { get; set; }
        public List<GetTaxSegmentAccountMappingTo_List> GetTaxSegmentAccountMappingTo_List { get; set; }
        public List<GetTaxAccountMappingTo_List> GetTaxAccountMappingTo_List { get; set; }
        public List<GetDepartmentAccountDropDown_list> GetDepartmentAccountDropDown_list {  get; set; }
        public List<GetSalaryCompSummary_list> GetSalaryCompSummary_list {  get; set; }
        public List<GetSalaryCompDropdown_list> GetSalaryCompDropdown_list {  get; set; }
    }
    public class taxsegmentmapping_list : result
    {
        public string taxsegment_gid { get; set; }
        public string account_name { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_code { get; set; }
        public string reference_type { get; set; }
        public string active_flag { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }


    public class taxchart_list
    {
        public string tax_amount { get; set; }
        public string month_name { get; set; }
    }
    public class GetDebtorSummary_List
    {
        public string customerref_no { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string website { get; set; }
        public string external_gl_code { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string gl_code { get; set; }
        public string gl_code_flag { get; set; }
        public string customer_code { get; set; }
    }
    public class GetCreditorSummary_List
    {
        public string vendor_gid { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string external_gl_code { get; set; }
        public string gl_code { get; set; }
        public string gl_code_flag { get; set; }
    }
    public class GetTaxPayableSummary_List : result
    {
        public string tax_gid { get; set; }
        public string reference_type { get; set; }
        public string tax_name { get; set; }
        public string external_gl_code { get; set; }
        public string percentage { get; set; }
        public string gl_code { get; set; }
        public string gl_code_flag { get; set; }
    }
    public class GetAssetDtlsSummary_List : result
    {
        public string assetdtl_gid { get; set; }
        public string assetgl_code { get; set; }
        public string product_name { get; set; }
        public string asset_status { get; set; }
        public string external_gl_code { get; set; }
        public string gl_code_flag { get; set; }
    }
    public class GetSalesTypeSummary_List
    {
        public string salestype_gid { get; set; }
        public string salestype_code { get; set; }
        public string salestype_name { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class GetPurchaseTypeSummary_List
    {
        public string purchasetype_gid { get; set; }
        public string purchasetype_code { get; set; }
        public string purchasetype_name { get; set; }
        public string account_name { get; set; }
        public string account_gid { get; set; }
    }
    public class GetDepartmentSummary_List
    {
        public string department_gid { get; set; }
        public string department_code { get; set; }
        public string department_prefix { get; set; }
        public string department_name { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }   
    public class GetAcctMappingSummary_List
    {
        public string accountmapping_gid { get; set; }
        public string module { get; set; }
        public string screen { get; set; }
        public string field { get; set; }
        public string account_name { get; set; }
        public string account_gid { get; set; }
    }
    public class GetExpenseGroupSummary_List
    {
        public string producttype_gid { get; set; }
        public string producttype_code { get; set; }
        public string producttype_name { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class salestype_list : result
    {
        public string salestype_gid { get; set; }
        public string salestype_name { get; set; }
        public string salestype_code { get; set; }
        public string editsalestype_name { get; set; }
        public string account_name { get; set; }
    }
    public class GetMappingAcctTo_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class purchasetype_list : result
    {
        public string purchasetype_gid { get; set; }
        public string purchasetype_name { get; set; }
        public string purchasetype_code { get; set; }
        public string editpurchasetype_name { get; set; }
        public string account_name { get; set; }
    }
    public class GetPurchaseMappingAcctTo_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class taxpayable_list : result
    {
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string edittax_name { get; set; }
        public string edittax_percentage { get; set; }
        public string tax_gid { get; set; }
        public string taxgl_code { get; set; }
        public string externalacct_name { get; set; }
        public string externalgl_code { get; set; }
    }
    public class empglcode_list : result
    {
        public string empexternalgl_code { get; set; }
        public string employee_gid { get; set; }
    }
    public class assetgl_list : result
    {
        public string assetexternalgl_code { get; set; }
        public string assetdtl_gid { get; set; }
    }
    public class GetExpenseMappingAcct_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class expensegroup_list : result
    {
        public string producttype_gid { get; set; }
        public string exaccount_name { get; set; }
    }
    public class GetAccountMapping_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class accountmapping_list : result
    {
        public string account_gid { get; set; }
        public string screen_name { get; set; }
        public string module_name { get; set; }
        public string field_name { get; set; }
        public string mapping_account { get; set; }
    }
    public class mapaccount_list : result
    {
        public string editmapping_account { get; set; }
        public string accountmapping_gid { get; set; }
    }
    public class debtor_list : result
    {
        public string debtorrgl_code { get; set; }
        public string debtorexternalgl_code { get; set; }
        public string customer_gid { get; set; }
    }
    public class GetRegionName_List : result
    {
        public string region_gid { get; set; }
        public string region_name { get; set; }
    }   
    public class GetCountryName_List : result
    {
        public string country_gid { get; set; }
        public string country_name { get; set; }
    }
    public class debtorglcode_list : result
    {
        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string company_website { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string state_name { get; set; }
        public string country_name { get; set; }
        public string city_name { get; set; }
        public string pincode { get; set; }
        public string region { get; set; }
        public string contactperson_name { get; set; }
        public string email_address { get; set; }
        public string mobile_number { get; set; }
        public string designation { get; set; }
        public string countrycontact_number { get; set; }
        public string countryarea_code { get; set; }
        public string contactcountry_code { get; set; }
        public string fax_number { get; set; }
        public string faxarea_code { get; set; }
        public string faxcountry_code { get; set; }
        public string region_name { get; set; }
    }
    public class creditor_list : result
    {
        public string vendor_gid { get; set; }
        public string creditorexternalgl_code { get; set; }
        public string vendor_code { get; set; }
        public string vendorcompany_name { get; set; }
        public string contactperson_name { get; set; }
        public string contact_number { get; set; }
        public string email_address { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city_name { get; set; }
        public string state_name { get; set; }
        public string pincode { get; set; }
        public string country_name { get; set; }
        public string fax_number { get; set; }
    }
    public class GetTaxSegmentAccountMappingTo_List : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
    public class TaxSegmentAccountMapping_list : result
    {
        public string taxsegment_gid { get; set; }
      
        public string taxaccount_name { get; set; }
    }
    public class GetTaxAccountMappingTo_List : result
    {
        public string account_gid { get; set; }
      
        public string account_name { get; set; }
        public string tax_gid { get; set; }
    }
    public class GetDepartmentAccountDropDown_list : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string department_gid { get; set; }
        public string Dept_mapping_account { get; set; }
    }
    public class GetSalaryCompSummary_list : result
    {
        public string salarycomponent_gid { get; set; }
        public string componentgroup_gid { get; set; }
        public string component_code { get; set; }
        public string component_name { get; set; }
        public string componentgroup_name { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
        public string salarycomponent_gid1 { get; set; }
        public string salarycomponent_ledger_gid { get; set; }
    }
    public class GetSalaryCompDropdown_list : result
    {
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
}