using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrMstProduct : result
    {
        public List<product_list> product_list { get; set; }
        public List<GetProducttype> GetProducttype { get; set; }
        public List<GetProductGroup> GetProductGroup { get; set; }
        public List<GetProductUnitclass> GetProductUnitclass { get; set; }
        public List<GetProductUnit> GetProductUnit { get; set; }
        public List<taxdtl_list> taxdtl_list { get; set; }

        // public List<productlists> products { get; set; }

        public List<string> pricesegments { get; set; }
        public List<GetEditProductSummary> GetEditProductSummary { get; set; }
        public List<GetViewProductSummary> GetViewProductSummary { get; set; }
        public List<productexport_list> productexport_list { get; set; }
    }


    //public class productlists
    //{
    //    public product_list[] products { get; set; }  
    //}
    public class taxdtl_list : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
    }

    public class product_list :result
    {

        public string statuses { get; set; }
        public string producttype_name { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_code { get; set; }
        public string product_price { get; set; }
        public string branch_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string productuomclass_code { get; set; }
        public string productuom_code { get; set; }
        public string productuomclass_name { get; set; }
        public string stockable { get; set; }
        public string productuom_name { get; set; }
        public string product_type { get; set; }
        public string Status { get; set; }
        public List<string> pricesegments { get; set; }
        public string tax { get; set; }
        public string serial_flag { get; set; }
        public string lead_time { get; set; }
        public string productuomclass_gid { get; set; }
        public string product_image { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string sku { get; set; }
        public string log_id { get; set; }
        public string document_id { get; set; }

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
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
        public string producttype_gid { get; set; }
        public string productuom_gid { get; set; }
        public string customerproduct_code { get; set; }



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
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string product_desc { get; set; }
        public string avg_lead_time { get; set; }
        public string product_gid { get; set; }
        public string tax { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string producttype_name { get; set; }
        public string productuomclass_name { get; set; }
        public string customerproduct_code { get; set; }


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
        public string customerproduct_code { get; set; }
        public string tax_name { get; set; }
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


    }
    public class productexport_list : result
    {
        public string lspath1 { get; set; }
        public string lsname2 { get; set; }
    }
    //Minsoft Api code by snehith
    public class MintsoftProductdetails
    {
        public Commoditycode CommodityCode { get; set; }
        public Countryofmanufacture CountryOfManufacture { get; set; }
        public Productincategory[] ProductInCategories { get; set; }
        public object[] ProductPrices { get; set; }
        public Productsupplier[] ProductSuppliers { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string PalletSizes { get; set; }
        public string PackingInstructions { get; set; }
        public string Description { get; set; }
        public string CustomsDescription { get; set; }
        public string CountryOfManufactureId { get; set; }
        public string EAN { get; set; }
        public string UPC { get; set; }
        public string LowStockAlertLevel { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Depth { get; set; }
        public string Volume { get; set; }
        public string BackOrder { get; set; }
        public string Bundle { get; set; }
        public string DisCont { get; set; }
        public string Price { get; set; }
        public string CostPrice { get; set; }
        public string VatExempt { get; set; }
        public string AdditionalParcelsRequired { get; set; }
        public string UnitsPerParcel { get; set; }
        public bool HasBatchNumber { get; set; }
        public object LogBatchInbound { get; set; }
        public object LogBatchOutbound { get; set; }
        public bool HasSerialNumber { get; set; }
        public object LogSerialInbound { get; set; }
        public object LogSerialOutbound { get; set; }
        public bool HasExpiryDate { get; set; }
        public object LogExpiryDateInbound { get; set; }
        public object LogExpiryDateOutbound { get; set; }
        public string BestBeforeDateWarningPeriodDays { get; set; }
        public string HandlingTime { get; set; }
        public string UnNumber { get; set; }
        public string ImageURL { get; set; }
        public object ProductHazardousGoods { get; set; }
        public object ProductPurchasingSettings { get; set; }
        public object ProductGrowthRates { get; set; }
        public object ExternalFulFilmentProduct { get; set; }
        public object ExternalFulFilmentProductInventory { get; set; }
        public object OrderItems { get; set; }
        public bool Subscription { get; set; }
        public string SubscriptionLength { get; set; }
        public object SubscriptionFrequency { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    

    public class Commoditycode1
    {
        public string Code { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Countryofmanufacture1
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Code3 { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productincategory1
    {
        public Productcategory ProductCategory { get; set; }
        public string ProductId { get; set; }
        public string ProductCategoryId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productcategory1
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productsupplier3
    {
        public Productsupplier1 ProductSupplier { get; set; }
        public string ProductId { get; set; }
        public string ProductSupplierId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productsupplier2
    {
        public Country Country { get; set; }
        public Currency Currency { get; set; }
        public string Name { get; set; }
        public object ContactName { get; set; }
        public object ContactNumber { get; set; }
        public object AddressLine1 { get; set; }
        public object AddressLine2 { get; set; }
        public object AddressLine3 { get; set; }
        public object Town { get; set; }
        public object County { get; set; }
        public object Postcode { get; set; }
        public string CountryId { get; set; }
        public object ContactEmail { get; set; }
        public bool Active { get; set; }
        public object Code { get; set; }
        public string CurrencyId { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Country1
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Code3 { get; set; }
        public int ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Currency1
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
 
}
