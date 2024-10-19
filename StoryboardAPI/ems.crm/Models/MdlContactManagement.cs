using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ems.crm.Models.MdlContactManagement.contactIndividualsummary;

namespace ems.crm.Models
{
    public class MdlContactManagement : result

    {

        public class contact_list : contact
        {
            public List<contact> contact { get; set; }
            public List<contactsummary> contactsummary { get; set; }
            public List<contactIndividualsummary> contactIndividualsummary { get; set; }
            public List<contactCorporatesummary> contactCorporatesummary { get; set; }
            public List<Constitutiondropdown_list> Constitutiondropdown_list { get; set; }
        }
        public class contactIndividualsummary : result
        {
            public string contact_gid { get; set; } = string.Empty;
            public string contact_ref_no { get; set; } = string.Empty;
            public string contact_type { get; set; } = string.Empty;
            public string contact_name { get; set; } = string.Empty;
            public string created_by { get; set; } = string.Empty;
            public string pan_no { get; set; } = string.Empty;
            public string created_date { get; set; } = string.Empty;
            public string source_name { get; set; } = string.Empty;
            public string region_name { get; set; } = string.Empty;
            public string Individual_count { get; set; } = string.Empty;
            public string remarks { get; set; } = string.Empty;
            public string lead_status { get; set; } = string.Empty;
            public string assign_to { get; set; } = string.Empty;


        }
        public class contactCorporatesummary : result
        {
            public string contact_gid { get; set; } = string.Empty;
            public string contact_ref_no { get; set; } = string.Empty;
            public string contact_type { get; set; } = string.Empty;
            public string lgltrade_name { get; set; } = string.Empty;
            public string created_by { get; set; } = string.Empty;
            public string corporate_pan_no { get; set; } = string.Empty;
            public string created_date { get; set; } = string.Empty;
            public string Corporate_count { get; set; } = string.Empty;
            public string source_name { get; set; } = string.Empty;
            public string region_name { get; set; } = string.Empty;
            public string remarks { get; set; } = string.Empty;
            public string lead_status { get; set; } = string.Empty;
            public string assign_to { get; set; } = string.Empty;

        }

        public class contactsummary : result
        {
            public string contact_gid { get; set; } = string.Empty;
            public string contact_ref_no { get; set; } = string.Empty;
            public string contact_type { get; set; } = string.Empty;
            public string contact_name { get; set; } = string.Empty;
            public string pan_no { get; set; } = string.Empty;
            public string created_by { get; set; } = string.Empty;
            public string total_count { get; set; } = string.Empty;
            public string created_date { get; set; } = string.Empty;
            public string source_name { get; set; } = string.Empty;
            public string region_name { get; set; } = string.Empty;
            public string remarks { get; set; } = string.Empty;
            public string lead_status { get; set; } = string.Empty;
            public string assign_to { get; set; } = string.Empty;

        }

        public class contact : result
        {
            public string referred_by { get; set; } = string.Empty;
            public string leadbank_region { get; set; } = string.Empty;
            public string spouse_lastname { get; set; } = string.Empty;
            public string business_start_date { get; set; } = string.Empty;
            public string maritalstatus_gid { get; set; } = string.Empty;
            public string incometype_gid { get; set; } = string.Empty;
            public string spouse_firstname { get; set; } = string.Empty;
            public string mother_lastname { get; set; } = string.Empty;
            public string region_gid { get; set; } = string.Empty;
            public string source_gid { get; set; } = string.Empty;
            public string mother_firstname { get; set; } = string.Empty;
            public string father_lastname { get; set; } = string.Empty;
            public string father_firstname { get; set; } = string.Empty;
            public string individual_dob { get; set; } = string.Empty;
            public string gender_name { get; set; } = string.Empty;
            public string maritalstatus_name { get; set; } = string.Empty;
            public string father_name { get; set; } = string.Empty;
            public string fathercontact_no { get; set; } = string.Empty;
            public string mothercontact_no { get; set; } = string.Empty;
            public string mother_name { get; set; } = string.Empty;
            public string spouse_name { get; set; } = string.Empty;
            public string spousecontact_no { get; set; } = string.Empty;
            public string educationalqualification_name { get; set; } = string.Empty;
            public string incometype_name { get; set; } = string.Empty;
            public string contact_ref_no { get; set; } = string.Empty;
            public string corporate_pan_no { get; set; } = string.Empty;
            public string contact_gid { get; set; }
            public string businesscategory_gid { get; set; } = string.Empty;
            public string amlcategory_gid { get; set; }
            public string salutation { get; set; } = string.Empty;
            public string salutation_gid { get; set; } = string.Empty;
            public string mobile { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            public string designation { get; set; }
            public string first_name { get; set; } = string.Empty;
            public string last_name { get; set; } = string.Empty;
            public string mobile_no { get; set; } = string.Empty;
            public string email_address { get; set; } = string.Empty;
            public string address1 { get; set; } = string.Empty;
            public string address2 { get; set; } = string.Empty;
            public string source_name { get; set; } = string.Empty;
            public string region_name { get; set; } = string.Empty;

            public string contact_type { get; set; } = string.Empty;
            public int age { get; set; }
            public string dob_date { get; set; } = string.Empty;
            public string gender { get; set; } = string.Empty;
            public string gender_gid { get; set; } = string.Empty;
            public string aadhaar_no { get; set; } = string.Empty;
            public string individual_pan_no { get; set; } = string.Empty;
            public string marital_status { get; set; } = string.Empty;
            public string designation_gid { get; set; } = string.Empty;
            public string designation_name { get; set; } = string.Empty;
            public string father_first_name { get; set; } = string.Empty;
            public string father_last_name { get; set; } = string.Empty;
            public string father_contact_refno { get; set; } = string.Empty;
            public string mother_first_name { get; set; } = string.Empty;
            public string mother_last_name { get; set; } = string.Empty;
            public string mother_contact_refno { get; set; } = string.Empty;
            public string spouse_last_name { get; set; } = string.Empty;
            public string spouse_first_name { get; set; } = string.Empty;
            public string spouse_contact_refno { get; set; } = string.Empty;
            public string education_qualification { get; set; } = string.Empty;
            public string main_occupation { get; set; } = string.Empty;
            public string annual_income { get; set; } = string.Empty;
            public string monthly_income { get; set; } = string.Empty;
            public string income_type { get; set; } = string.Empty;

            public string created_by { get; set; } = string.Empty;
            public string created_date { get; set; } = string.Empty;
            public string location { get; set; } = string.Empty;
            public string contact_name { get; set; } = string.Empty;
            public string constitution_gid { get; set; } = string.Empty;
            public string constitution_name { get; set; } = string.Empty;
            public string marital_status_gid { get; set; } = string.Empty;
            public string income_type_gid { get; set; } = string.Empty;


            public string pan_no { get; set; } = string.Empty;
            public string aadhar_no { get; set; } = string.Empty;
            public string templongitude { get; set; } = string.Empty;
            public string templatitude { get; set; } = string.Empty;
            public string tempcountry_gid { get; set; } = string.Empty;
            public string tempcountry_name { get; set; } = string.Empty;
            public string temppostal_code { get; set; } = string.Empty;
            public string tempstate { get; set; } = string.Empty;
            public string tempcity { get; set; } = string.Empty;
            public string tempaddress2 { get; set; } = string.Empty;
            public string tempaddress1 { get; set; } = string.Empty;
            public string longitude { get; set; } = string.Empty;
            public String latitude { get; set; } = string.Empty;
            public String country_gid { get; set; } = string.Empty;
            public string country_name { get; set; } = string.Empty;
            public string postal_code { get; set; } = string.Empty;
            public string state { get; set; } = string.Empty;
            public string city { get; set; } = string.Empty;
            public string physicalstatus_name { get; set; } = string.Empty;
            public string physicalstatus_gid { get; set; } = string.Empty;
            public string middle_name { get; set; } = string.Empty;
            public string last_year_turnover { get; set; } = string.Empty;
            public string category_business { get; set; } = string.Empty;
            public string category_aml { get; set; } = string.Empty;
            public string udhayam_registration { get; set; } = string.Empty;
            public String kin { get; set; } = string.Empty;
            public String tan_state { get; set; } = string.Empty;
            public string tan { get; set; } = string.Empty;
            public string businessstart_date { get; set; } = string.Empty;
            public string businesss_vintage { get; set; } = string.Empty;
            public string constitution { get; set; } = string.Empty;
            public string cin_date { get; set; } = string.Empty;
            public string cin { get; set; } = string.Empty;
            public string lei { get; set; } = string.Empty;
            public string lgltrade_name { get; set; } = string.Empty;

            public List<gst> gst_list { get; set; }
            public List<mobile> mobile_list { get; set; }
            public List<email> email_list { get; set; }
            public List<address> address_list { get; set; }
            public List<promoter> promoter_list { get; set; }
            public List<director> director_list { get; set; }

            public List<GeneralDocumentList> DocumentList { get; set; }
        }
        public class promoter
        {
            public string pan_no { get; set; } = string.Empty;
            public string aadhar_no { get; set; } = string.Empty;
            public string mobile { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            public string designation { get; set; } = string.Empty;
            public string last_name { get; set; } = string.Empty;
            public string middle_name { get; set; } = string.Empty;
            public string first_name { get; set; } = string.Empty;
            public string salutation { get; set; } = string.Empty;
            public string salutation_gid { get; set; } = string.Empty;
            public string promoter_name { get; set; } = string.Empty;
        }
        public class director
        {
            public string pan_no { get; set; } = string.Empty;
            public string aadhar_no { get; set; } = string.Empty;
            public string mobile { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            public string designation { get; set; } = string.Empty;
            public string last_name { get; set; } = string.Empty;
            public string middle_name { get; set; } = string.Empty;
            public string first_name { get; set; } = string.Empty;
            public string salutation { get; set; } = string.Empty;
            public string salutation_gid { get; set; } = string.Empty;
            public string director_name { get; set; } = string.Empty;
        }
        public class mobile
        {
            public string contact2mobileno_gid { get; set; } = string.Empty;
            public string contact_gid { get; set; } = string.Empty;
            public string mobile_no { get; set; } = string.Empty;
            public string primary_status { get; set; } = string.Empty;
        }
        public class email
        {
            public string contact2email_gid { get; set; } = string.Empty;
            public string email_address { get; set; } = string.Empty;
            public string primary_status { get; set; } = string.Empty;
            public string contact_gid { get; set; } = string.Empty;
        }
        public class gst
        {
            public string contact2gst_gid { get; set; } = string.Empty;
            public string gst_location { get; set; } = string.Empty;
            public string gst_no { get; set; } = string.Empty;
            public string gst_state { get; set; } = string.Empty;
            public string contact_gid { get; set; } = string.Empty;
        }


        public class address
        {
            public string contact2address_gid { get; set; } = string.Empty;
            public string address1 { get; set; } = string.Empty;
            public string address2 { get; set; } = string.Empty;
            public string primary_status { get; set; } = string.Empty;
            public string addresstype_gid { get; set; } = string.Empty;
            public string addresstype { get; set; } = string.Empty;
            public string mobile_no { get; set; } = string.Empty;
            public string email_address { get; set; } = string.Empty;
            public string longitude { get; set; } = string.Empty;
            public string latitude { get; set; } = string.Empty;
            public string country_gid { get; set; } = string.Empty;
            public string country_name { get; set; } = string.Empty;
            public string postal_code { get; set; } = string.Empty;
            public string state { get; set; } = string.Empty;
            public string city { get; set; } = string.Empty;

        }
        public class tag_list : result
        {
            public string state_name { get; set; }
            public string district_name { get; set; }
            public string tag_type_gid { get; set; }
            public string tag_type { get; set; }
            public string business_head_gid { get; set; }
            public string business_head { get; set; }
            public string zonal_head { get; set; }
            public string zonal_head_gid { get; set; }
            public string cluster_head { get; set; }
            public string cluster_head_gid { get; set; }
            public string regional_head { get; set; }
            public string regional_head_gid { get; set; }
            public string cluster_manager_gid { get; set; }
            public string cluster_manager { get; set; }
            public string relationship_manager_gid { get; set; }
            public string relationship_manager { get; set; }
            public string credit_manager_gid { get; set; }
            public string credit_manager { get; set; }
            public string zonal_risk_manager { get; set; }
            public string zonal_risk_manager_gid { get; set; }
            public string risk_manager { get; set; }
            public string risk_manager_gid { get; set; }
            public string head_risk_monitoring { get; set; }

            public string head_risk_monitoring_gid { get; set; }
            public string user_gid { get; set; }
            public string contact_gid { get; set; }
        }

        public class contactedittag : result
        {
            public string contact_gid { get; set; }
            public string state_gid { get; set; }
            public string state_name { get; set; }
            public string district_gid { get; set; }

            public string district_name { get; set; }
            public string zrm_name { get; set; }
            public string zrm_gid { get; set; }
            public string rm_gid { get; set; }
            public string rm_name { get; set; }

        }


        public class zonalList : result
        {
            public List<selectedzonaldtl> selectedzonaldtl { get; set; }
        }

        public class selectedzonaldtl : result
        {
            public string zonal_name { get; set; }

            public string zonal_gid { get; set; }
        }

        public class locationList : result
        {
            public List<selectedlocationdtl> selectedlocationdtl { get; set; }
        }

        public class selectedlocationdtl : result
        {
            public string location_name { get; set; }

            public string location_gid { get; set; }
        }

        public class GeneralDocumentname : result
        {
            public List<GeneralDocumentList> DocumentList { get; set; }

            public string document_name { get; set; }
            public string customer2sanction_gid { get; set; }

        }
        public class GeneralDocumentList : result
        {
            public string document_name { get; set; }
            public string employee_gid { get; set; }
            public string contact_gid { get; set; }
            public string file_name { get; set; }
            public string FileName { get; set; }
            public string path { get; set; }
            public string created_date { get; set; }
            public string created_by { get; set; }
            public string updated_by { get; set; }
            public string updated_date { get; set; }

        }
        public class mdlcontdoc
        {
            public string document_name { get; set; }
            public string file_name { get; set; }
            public string AutoID_Key { get; set; }
        }
        public class Constitutiondropdown_list: result
        {
            public string constitution_gid { get; set; }
            public string constitution_name { get; set; }
        }
    }
}