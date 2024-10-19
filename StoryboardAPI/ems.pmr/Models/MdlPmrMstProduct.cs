using ems.pmr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrMstProduct : result
    {
        public List<product_list> product_list { get; set; }
        public List<GetProducttype> GetProducttype { get; set; }
        public List<GetProductGroup> GetProductGroup { get; set; }
        public List<GetProductUnitclass> GetProductUnitclass { get; set; }
        public List<GetProductUnit> GetProductUnit { get; set; }
        public List<TaxSegmentSummary_list> TaxSegmentSummary_list { get; set; }
        public List<GetEditProductSummary> GetEditProductSummary { get; set; }
        public List<GetViewProductSummary> GetViewProductSummary { get; set; }
        public List<taxdtl_list> taxdtl_list { get; set; }
        public List<productexport_list> productexport_list { get; set; }

    }
    public class productexport_list : result
    {
        public string lspath1 { get; set; }
        public string lsname2 { get; set; }
    }
    public class product_list : result
    {
        public string document_id { get; set; }
        public string log_id { get; set; }
        public string producttype_name { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string product_price { get; set; }

        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string productuomclass_code { get; set; }
        public string productuom_code { get; set; }
        public string productuomclass_name { get; set; }
        public string stockable { get; set; }
        public string productuom_name { get; set; }
        public string product_type { get; set; }
        public string Status { get; set; }
        public string serial_flag { get; set; }
        public string lead_time { get; set; }

        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

        public string product_desc { get; set; }
        public string currency_code { get; set; }
        public string avg_lead_time { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string batch_flag { get; set; }
        public string productgroupname { get; set; }
        public string producttypename { get; set; }
        public string productuomclassname { get; set; }
        public string productuomname { get; set; }
        public string sku { get; set; }
        //public string tax { get; set; }
        public List<string> tax { get; set; }
        public string tax1 { get; set; }

    }
    public class taxlist
    {
        public string tax { get; set; }
    }
    public class GetProducttype
    {
        public string producttype_name { get; set; }
        public string producttype_gid { get; set; }

    }
    public class GetProductGroup
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }

    }
    public class GetProductUnitclass
    {
        public string productuomclass_gid { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }

    }
    public class GetProductUnit
    {
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }


    }
    public class GetViewProductSummary : result
    {

        public string currency_code { get; set; }
        public string batch_flag { get; set; }
        public string serial_flag { get; set; }
        public string serialtracking_flag { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string product_desc { get; set; }
        public string avg_lead_time { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string producttype_name { get; set; }
        public string productuomclass_name { get; set; }
        public string sku { get; set; }
        public string tax { get; set; }
        public string tax1 { get; set; }


    }
    public class GetEditProductSummary : result
    {

        public string currency_code { get; set; }
        public string batch_flag { get; set; }
        public string serial_flag { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string product_desc { get; set; }
        public string avg_lead_time { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }

        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string producttype_name { get; set; }

        public string producttype_gid { get; set; }
        public string productuomclass_name { get; set; }

        public string productuomclass_gid { get; set; }
        public string sku { get; set; }
        public string tax { get; set; }
        public string tax1 { get; set; }
        public string tax_gid1  { get; set; }
        public string tax_gid { get; set; }


    }
    public class TaxSegmentSummary_list : result
    {
        public List<tax_list> tax_list { get; set; }
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
        public string taxsegment_description_edit { get; set; }
        public string active_flag_edit { get; set; }

    }

    public class tax_list : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string percentage { get; set; }
        public string product_gid { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }

    }
    public class taxdtl_list : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
    }

  
}