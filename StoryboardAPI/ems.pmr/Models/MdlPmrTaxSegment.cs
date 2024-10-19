using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrTaxSegment : result
    {
        public List<PmrTaxSegment_list> PmrTaxSegment_list { get; set; }   
        public List<PmrTaxSegmentSummary_list> PmrTaxSegmentSummary_list { get; set; }   
        public List<GetPmrWithInState_list> GetPmrWithInState_list { get; set; }   
        public List<GetPmrTaxSegmentDropDown_list> GetPmrTaxSegmentDropDown_list { get; set; }   
        public List<GetPmrinterstate_list> GetPmrinterstate_list { get; set; }   
        public List<GetPmrOverSeas_list> GetPmrOverSeas_list { get; set; }   
        public List<GetPmrothers_list> GetPmrothers_list { get; set; }   
        public List<GetPmrInassignlist> GetPmrInassignlist { get; set; }   
        public List<GetPmrtotal_list> GetPmrtotal_list { get; set; }   
        public List<TaxSegment_Vendorlist> TaxSegment_Vendorlist { get; set; }   
        public List<Taxsegment2assignvendorList> Taxsegment2assignvendorList { get; set; }   
        public List<Taxsegment2unassignvendorList> Taxsegment2unassignvendorList { get; set; }   
    }
    public class PostPmrTaxSegment2Vendor_list : result
    {
        public List<Taxsegment2unassignvendorList> Taxsegment2unassignvendorList { get; set; }
        public string taxsegment_gid { get; set; }  
    }
    public class PostPmrTaxSegment2unassignVendor_list : result
    {
        public List<Taxsegment2assignvendorList> Taxsegment2assignvendorList { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class TaxSegment_Vendorlist : result
    {
        public double total_vendor { get; set; }
        public string within_vendor { get; set; }
        public string interstate_vendor { get; set; }
        public string overseas_vendor { get; set; }
        public double unassign_vendor { get; set; }
        public double assign_vendor { get; set; }
        public string other_vendor { get; set; }

    }
    public class Taxsegment2assignvendorList : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_code { get; set; }
        public string Contactdetails { get; set; }
        public string country { get; set; }
        public string region_name { get; set; }
    }
    public class Taxsegment2unassignvendorList : result
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_code { get; set; }
        public string Contactdetails { get; set; }
        public string country { get; set; }
        public string region_name { get; set; }
    }
    public class PmrTaxSegment_list : result
    {
         public string taxsegment_gid { get; set; }
         public string taxsegment_name { get; set; }
         public string taxsegment_code { get; set; } 
         public string taxsegment_description { get; set; } 
         public string active_flag { get; set; } 
         public string created_by { get; set; } 
         public string assignvendor { get; set; } 
         public string created_date { get; set; } 
         public string message { get; set; } 
    }
    public class PmrTaxSegmentSummary_list : result
    {
        public List<pmrTax_list> tax_list { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment2product_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_code { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_amount { get; set; }
        public string mrp_price { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string taxsegment_description { get; set; }
        public string active_flag { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string taxsegment_name_edit { get; set; }
        public string taxsegment_code_edit { get; set; }
        public string taxsegment_description_edit { get; set; }
        public string active_flag_edit { get; set; }
    }
    public class pmrTax_list : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
        public string product_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }

    }
    public class GetPmrOverSeas_list : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string vendor_code { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    } 
    public class GetPmrtotal_list : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string vendor_code { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    } 
    public class GetPmrInassignlist : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string vendor_code { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    } 
    public class GetPmrWithInState_list : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string vendor_code { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetPmrothers_list : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string vendor_code { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetPmrinterstate_list : result
    {
        public string vendor_companyname { get; set; }
        public string vendor_gid { get; set; }
        public string contactperson_name { get; set; }
        public string vendor_address { get; set; }
        public string vendor_code { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }
    public class GetPmrTaxSegmentDropDown_list : result
    {
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
    }

    public class PmrPostTaxsegment
    {
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string[] customer_gid1 { get; set; }
        public bool status { get; set; }
        public string message { get; set; }

    }
}