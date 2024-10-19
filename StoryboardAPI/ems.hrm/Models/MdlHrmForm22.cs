using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmForm22 : result
    {
        public List<company_list> company_list { get; set; }
        public List<GetProduct> GetProduct { get; set; }
        public List<GetEditConcession> GetEditConcession { get; set; }
        public List<yearreturn_list> yearreturn_list { get; set; }
        public List<Getformdropdown> Getformdropdown { get; set; }
        public List<form_list> form_list { get; set; }
        public List<product_list> product_list { get; set; }
        public List<leavewage_list> leavewage_list { get; set; }
        public List<concession_list> concession_list { get; set; }
        public List<employeement_list> employeement_list { get; set; }
        public List<GetEditForm22SubRule3> GetEditForm22SubRule3 { get; set; }
        public List<GetEditForm22SubRule4> GetEditForm22SubRule4 { get; set; }
        public List<GetEditForm22SubRule1> GetEditForm22SubRule1 { get; set; }
        public List<GetEditCompany> GetEditCompany { get; set; }
        public List<GetEditLeaveWage> GetEditLeaveWage { get; set; }
    }

    public class company_list : result
    {
        public string company_gid { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
        public string contact_person { get; set; }
        public string contactperson_address { get; set; }
        public string occupier_address { get; set; }
        public string occupier_name { get; set; }
        public string natureof_industry { get; set; }
        public string companyregistrtion_number { get; set; }
    }

    public class employeement_list : result
    {
        public string form_gid { get; set; }
        public string processed_year { get; set; }
        public string menrollstartdate { get; set; }
        public string womenrollstartdate { get; set; }
        public string adloscentmenrollstartdate { get; set; }
        public string adloscentwomenrollstartdate { get; set; }
        public string menrollenddate { get; set; }
        public string womenrollenddate { get; set; }
        public string adloscentmenrollenddate { get; set; }
        public string adloscentwomenrollenddate { get; set; }
        public string menfactoryworked { get; set; }
        public string womenfactoryworked { get; set; }
        public string adloscentmenfactoryworked { get; set; }
        public string adloscentwomenfactoryworked { get; set; }
        public string menworkedyearnormal { get; set; }
        public string womenworkedyearnormal { get; set; }
        public string adloscentmenworkedyearnormal { get; set; }
        public string adloscentwomenworkedyearnormal { get; set; }
        public string menworkedyearot { get; set; }
        public string womenworkedyearot { get; set; }



        public string adloscentmenworkedyearot { get; set; }
        public string adloscentwomenworkedyearot { get; set; }
        public string menworkedyeartotal { get; set; }
        public string womenworkedyeartotal { get; set; }
        public string adloscentmenworkedyeartotal { get; set; }
        public string adloscentwomenworkedyeartotal { get; set; }
        public string menworkperweek { get; set; }
        public string womenworkperweek { get; set; }
        public string adloscentmenworkperweek { get; set; }
        public string adloscentwomenworkperweek { get; set; }


    }


    public class concession_list : result
    {
        public string empbonus_number { get; set; }
        public string bonus_declared { get; set; }
        public string bonus_amount { get; set; }
        public string exgratia_amount { get; set; }
        public string incentive_amount { get; set; }
        public string bonus_date { get; set; }
        public string exgratia_date { get; set; }
        public string incentive_date { get; set; }
        public string form_gid { get; set; }
        public string processed_year { get; set; }
    }

    public class GetProduct : result
    {
        public string productdetails_gid { get; set; }
        public string product_name { get; set; }
        public string capacity { get; set; }
        public string quantity { get; set; }
        public string product_value { get; set; }

    }
    public class GetEditConcession : result
    {
        public string bonus_gid { get; set; }
        public string numberofemployeeseligible_bonus { get; set; }
        public string bonus_percentage { get; set; }
        public string amountof_bonus { get; set; }
        public string amountof_exgratia { get; set; }
        public string amountof_incentive { get; set; }
        public string paymentof_bonus { get; set; }
        public string paymentof_exgratia { get; set; }
        public string paymentof_incentive { get; set; }

    }

    public class GetEditLeaveWage : result
    {
        public string earnleavewage_gid { get; set; }
        public string total_employeemen { get; set; }
        public string employee_eligiblemen { get; set; }
        public string no_ofemployeeavailedmen { get; set; }
        public string no_ofemployeedischargemen { get; set; }
        public string employee_lieuearnmen { get; set; }
        public string total_employeewomen { get; set; }
        public string employee_eligiblewomen { get; set; }
        public string no_ofemployeeavailedwomen { get; set; }
        public string no_employeedischargewomen { get; set; }
        public string employee_lieuearnwomen { get; set; }
        public string total_employeeado { get; set; }
        public string employee_eligibleado { get; set; }
        public string no_ofemployeeavailedado { get; set; }
        public string no_employeedischargeado { get; set; }
        public string employee_lieuearnado { get; set; }

    }

    public class yearreturn_list : result
    {
        public string form_gid { get; set; }
        public string form_name { get; set; }
        public string processed_year { get; set; }

    }

    public class Getformdropdown : result
    {
        public string sanctuaryform_gid { get; set; }
        public string form_name { get; set; }

    }

    public class form_list : result
    {
        public string form_gid { get; set; }
        public string form_name { get; set; }
        public string processed_year { get; set; }

    }

    public class product_list : result
    {
        public string productdetails_gid { get; set; }
        public string product_name { get; set; }
        public string capacity { get; set; }
        public string quantity { get; set; }
        public string product_value { get; set; }
        public string form_gid { get; set; }
        public string processed_year { get; set; }

    }

    public class leavewage_list : result
    {
        public string mentotalnoofemp { get; set; }
        public string menearnedleave { get; set; }
        public string mengrantedleave { get; set; }
        public string mendischarged { get; set; }
        public string mennoofempwages { get; set; }
        public string womentotalnoofemp { get; set; }
        public string womenearnedleave { get; set; }
        public string womengrantedleave { get; set; }
        public string womendischarged { get; set; }
        public string woemennoofempwages { get; set; }
        public string adolescentstotalnoofemp { get; set; }
        public string adloscentsearnedleave { get; set; }
        public string adloscentsgrantedleave { get; set; }
        public string adloescentsdischarged { get; set; }
        public string adolescentsnoofempwages { get; set; }
        public string form_gid { get; set; }
        public string processed_year { get; set; }
    }

    public class GetEditForm22SubRule3 : result
    {
        public string numberof_workmen { get; set; }
        public string numberofworkmen_form1 { get; set; }
        public string totalworkdays_year { get; set; }
        public string nonpermanent_count { get; set; }
        public string permanent_count { get; set; }
        public string permanentcount_firstjuly { get; set; }
        public string reasons { get; set; }
        public string remarks { get; set; }
        public string company_address { get; set; }
        public string typeof_industry { get; set; }
        public string form_gid { get; set; }
        public string processed_year { get; set; }
    }

    public class GetEditForm22SubRule4 : result
    {
        public string serial_number { get; set; }
        public string nameaddressofemployee_suspension { get; set; }
        public string wagespaid_monthlyemployees { get; set; }
        public string departmentanddesignation_last { get; set; }
        public string natureof_offence { get; set; }
        public string suspension_date { get; set; }
        public string commencementenquiry_date { get; set; }
        public string completionenquiry_date { get; set; }
        public string revocationsuspension_date { get; set; }
        public string subsistenceallowence_rate { get; set; }
        public string subsistenceallowence_paid { get; set; }
        public string dateofissue_finalorder { get; set; }
        public string employees_punishment { get; set; }
        public string remarks { get; set; }
        public string postal_address { get; set; }
        public string managingpartner_address { get; set; }

    }

    public class GetEditForm22SubRule1 : result
    {
        public string averageofworkersemployeed_daily { get; set; }
        public string numberofdaysworked_halfyear { get; set; }
        public string adultsmale_count { get; set; }
        public string adultsfemale_count { get; set; }
        public string adolescentsmale_count { get; set; }
        public string adolescentsfemale_count { get; set; }
        public string childrenmale_count { get; set; }
        public string childrenfemale_count { get; set; }
        public string dispatch_date { get; set; }
        public string registration_number { get; set; }
        public string occupier_address { get; set; }
        public string managingpartner_address { get; set; }
        public string managingpartner_address1 { get; set; }
        public string natureof_industry { get; set; }
    
    }



    public class form2subrule3 : result
    {
        public string processed_year { get; set; }
        public string numberof_workmen { get; set; }
        public string form_gid { get; set; }
        public string numberofworkmen_form1 { get; set; }
        public string totalworkdays_year { get; set; }
        public string nonpermanent_count { get; set; }
        public string permanent_count { get; set; }
        public string permanentcount_firstjuly { get; set; }
        public string reason_delay { get; set; }
        public string remarks { get; set; }
        public string company_address { get; set; }
        public string typeof_industry { get; set; }
        
    }

    public class form2subrule4 : result
    {
        public string form_gid { get; set; }
        public string postal_address { get; set; }
        public string nameaddress_employers { get; set; }
        public string serial_number { get; set; }
        public string nameaddressofemployee_suspension { get; set; }
        public string wagespaid_monthlyemployees { get; set; }
        public string departmentanddesignation_last { get; set; }
        public string natureof_offence { get; set; }
        public string suspension_date { get; set; }
        public string commencementenquiry_date { get; set; }
        public string completionenquiry_date { get; set; }
        public string revocationsuspension_date { get; set; }
        public string subsistenceallowence_rate { get; set; }
        public string subsistenceallowence_paid { get; set; }
        public string dateofissue_finalorder { get; set; }
        public string employees_punishment { get; set; }
        public string remarks_data { get; set; }
    }

    public class form21subrule1 : result
    {
        public string form_gid { get; set; }
        public string numberofdaysworked_halfyear { get; set; }
        public string dispatch_date { get; set; }
        public string averageofworkersemployeed_daily { get; set; }
        public string adultsmale_count { get; set; }
        public string adultsfemale_count { get; set; }
        public string adolescentsmale_count { get; set; }
        public string adolescentsfemale_count { get; set; }
        public string childrenmale_count { get; set; }
        public string childrenfemale_count { get; set; }
        public string registration_number { get; set; }
        public string occupier_address { get; set; }
        public string managingpartner_address { get; set; }
        public string managingpartner_address1 { get; set; }
        public string natureof_industry { get; set; }
        public string employees_punishment { get; set; }
        public string remarks_data { get; set; }

    }

    public class companydetails : result
    {
        public string companyregistrtion_number { get; set; }
        public string company_name { get; set; }
        public string occupier_name { get; set; }
        public string occupier_address { get; set; }
        public string company_address { get; set; }
        public string company_phone { get; set; }
        public string natureof_industry { get; set; }
    }
    public class GetEditCompany : result
    {
        public string company_name { get; set; }
        public string contact_person { get; set; }
        public string occupier_name { get; set; }
        public string contactperson_address { get; set; }
        public string companyregistrtion_number { get; set; }
        public string company_address { get; set; }
        public string occupier_address { get; set; }
        public string company_phone { get; set; }
        public string natureof_industry { get; set; }
    }
    }