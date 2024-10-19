using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlCaseManagement : result
    {
        public List<GetCaseManagementSummart_list> GetCaseManagementSummart_list { get; set; }
        public List<Getcasetype_list> Getcasetype_list { get; set; }
        public List<Getcaseinstitute_list> Getcaseinstitute_list { get; set; }
        public List<attachements> attachements { get; set; }
        public List<GetViewSummaryCase_list> GetViewSummaryCase_list { get; set; }
        public Dictionary<string, List<GetViewDoc_list>> GetViewDoc_list { get; set; }
        public List<getcasestage_list> getcasestage_list { get; set; }
        public List<docprovider_list> docprovider_list { get; set; }
        public List<GetViewDocument_list> GetViewDocument_list { get; set; }
    }

    public class GetCaseManagementSummart_list : result 
    {
        public string case_gid {  get; set; }   
        public string casetype_gid {  get; set; }   
        public string institute_gid {  get; set; }   
        public string institute_name {  get; set; }   
        public string casetype_name {  get; set; }   
        public string case_date {  get; set; }   
        public string case_no {  get; set; }   
        public string client_name {  get; set; }   
        public string case_remarks {  get; set; }   
        public string created_date {  get; set; }   
        public string created_by {  get; set; }   
    }

    public class Getcasetype_list : result
    {
        public string casetype_code { get; set; }  
        public string casetype_gid { get; set; }  
        public string casetype_name { get; set; }          
    }

    public class Getcaseinstitute_list: result
    {
        public string institute_code { get; set; }
        public string institute_gid { get; set; }
        public string institute_name { get; set; }
    }
    public class attachements : result
    {
        public string lspath { get; set; }
        public string doc { get; set; }
        public string file_extension { get; set; }
        public string lspath1 { get; set; }
        public string lspath2 { get; set; }
    }
    public class GetViewSummaryCase_list : result
    {
        public string case_gid { get; set; }
        public string casetype_gid { get; set; }
        public string institute_gid { get; set; }
        public string institute_name { get; set; }
        public string casetype_name { get; set; }
        public string case_date { get; set; }
        public string case_no { get; set; }
        public string client_name { get; set; }
        public string case_remarks { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
    }
    public class GetViewDoc_list : result
    {
        public string case_gid { get; set; }
        public string doc_gid { get; set; }
        public string doc_extension { get; set; }
        public string doc_name { get; set; }
        public string doc_filepath { get; set; }
        public string doc_attpath { get; set; }
        public string doc_path { get; set; }
        public string casestage_name { get; set; }
        public string doc_provider { get; set; }
        public string uploaded_by { get; set; }
        public string remarks { get; set; }
        public string created_date { get; set; }
    }
    public class GetViewDocument_list : result
    {
        public string case_gid { get; set; }
        public string doc_gid { get; set; }
        public string doc_extension { get; set; }
        public string doc_name { get; set; }
        public string doc_filepath { get; set; }
        public string doc_attpath { get; set; }
        public string doc_path { get; set; }
        public string casestage_name { get; set; }
        public string doc_provider { get; set; }
        public string uploaded_by { get; set; }
        public string remarks { get; set; }
        public string created_date { get; set; }
    }
    public class docprovider_list : result
    {
        public string provider_gid { get; set; }
        public string doc_provider { get; set; }
    }
    public class getcasestage_list : result
    {
        public string casestage_gid { get; set; }
        public string casestage_name { get; set; }
    }
}