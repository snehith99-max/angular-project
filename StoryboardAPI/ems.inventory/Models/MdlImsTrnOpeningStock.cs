using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class MdlImsTrnOpeningStock : result
    {
        public List<stockedit_list> stockedit_list { get; set; }
        public List<stock_list> stock_list { get; set; }
        public List<stockadd_list> stockadd_list { get; set; }
        public List<GetLocation> GetLocation { get; set; }
        public List<Postopeningstock> Postopeningstock { get; set; }

        public List<GetEditOpeningStock> GetEditOpeningStock { get; set; }
        public List<GetproductsCode> ProductsCode { get; set; }
        public List<GetProduct_name> GetProduct_name { get; set; }
        public List<branchdtl_lists> branchdtl_lists { get; set; }
        public List<GetFinancialYear_List> GetFinancialYear_List { get; set; }
        public List<GetProductNamDtl> GetProductNamDtl { get; set; }
        public List<GetProductGroup1> GetProductGroup1 { get; set; }


    }

    public class GetproductsCode : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string unitprice { get; set; }
        public string product_desc { get; set; }

    }
    public class stock_list : result
    {
        public string created_date { get; set; }
        public string branch_name { get; set; }
        public string location_name { get; set; }
        public string productgroup_name { get; set; }
        public string producttype_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string opening_stock { get; set; }

        public string stock_gid { get; set; }
        public string product_gid { get; set; }
        public string issued_qty { get; set; }
        public string display_field { get; set; }
        public string branch_prefix { get; set; }

    }
    public class stockadd_list : result
    {
        public string created_date { get; set; }
        public string branch_name { get; set; }
        public string location_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string opening_stock { get; set; }
        public string productgroup_gid { get; set; }
        public string product_desc { get; set; }
        public string productuom_gid { get; set; }
        public string product_gid { get; set; }
       


    }
    public class GetLocation : result
    {
        public string location_name { get; set; }
        public string location_gid { get; set; }
        public string branch_gid { get; set; }


    }
    public class Postopeningstock : result
    {
        public string location_name { get; set; }
        public string location_gid { get; set; }
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string branch_gid { get; set; }
        public string display_field { get; set; }
        public string stock_qty { get; set; }
        public string unit_price { get; set; }
        public string finyear { get; set; }
       



    }
    public class GetEditOpeningStock : result
    {

        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string product_desc { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }

        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string financial_year { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
      
        public string cost_price { get; set; }
        public string opening_stock { get; set; }

        public string stock_gid { get; set; }
        public string product_status { get; set; }
        public string stock_qty { get; set; }
        public string opening_qty { get; set; }
    }


    public class stockedit_list : result
    {


        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string product_desc { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }

        public string uom_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }

        public string cost_price { get; set; }
        public string opening_stock { get; set; }
        public string stock_qty { get; set; }
        public string stock_gid { get; set; }

        public string product_status { get; set; }
        public string finyear { get; set; }
    }
    public class Stockedit_list1 : result
    {


        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string product_desc { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }

        public string uom_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }

        public string cost_price { get; set; }
        public string opening_stock { get; set; }
        public string stock_gid { get; set; }

        public string product_status { get; set; }
        public string finyear { get; set; }
        public string stock_qty { get; set; }
        public string opening_qty { get; set; }
    }
    public class GetProduct_name : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }
    public class get
    {

        public bool status { get; set; }
        public string message { get; set; }
    }
    public class ProductInventory
    {
        public string ProductId { get; set; }
        public string StockLevel { get; set; }
        public int Allocated { get; set; }
        public string OnHand { get; set; }
        public string OffHand { get; set; }
        public string AwaitingReplen { get; set; }
        public string OnOrder { get; set; }
        public string RequiredByBackOrder { get; set; }
        public string InQuarantine { get; set; }
        public string InTransit { get; set; }
        public string InTransition { get; set; }
        public string Scrapped { get; set; }
        public string SKU { get; set; }
        public string WarehouseId { get; set; }
        public string ID { get; set; }
        public string LocationId { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class MintsoftProduct
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

    public class Commoditycode
    {
        public string Code { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Countryofmanufacture
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Code3 { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productincategory
    {
        public Productcategory ProductCategory { get; set; }
        public string ProductId { get; set; }
        public string ProductCategoryId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productcategory
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productsupplier
    {
        public Productsupplier1 ProductSupplier { get; set; }
        public string ProductId { get; set; }
        public string ProductSupplierId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Productsupplier1
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

    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Code3 { get; set; }
        public int ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Currency
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class branchdtl_lists
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }
    public class GetFinancialYear_List
    {
        public string finyear { get; set; }
        public string finyear_gid { get; set; }
    }

    public class GetProductNamDtl
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }
    public class GetProductGroup1
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
    }
}