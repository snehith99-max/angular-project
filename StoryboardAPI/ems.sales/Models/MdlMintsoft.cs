using System;
using System.Collections.Generic;

namespace ems.sales.Models
{
    public class MdlSalesOrder
    {
        public string salesorder_gid { get; set; }
        public int CourierServiceId { get; set; }    
    }


    public class MdlMintsoftJSON
    {
        public Orderitem[] OrderItems { get; set; }
        public object[] OrderNameValues { get; set; }
        public string OrderNumber { get; set; }
        public string ExternalOrderReference { get; set; }
        public string FirstName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int ClientId { get; set; }
        public int CourierServiceId { get; set; }
        public string CourierService { get; set; }
        public int WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public double OrderValue { get; set; }
        public int TotalVat { get; set; }
        public string CompanyName { get; set; }
    }
    public class getordersfrommintsoft
    {
        public string ClientId { get; set; }
        public string OrderNumber { get; set; }
        public string ExternalOrderReference { get; set; }
        public string OrderDate { get; set; }
        public string DespatchDate { get; set; }
        public string RequiredDespatchDate { get; set; }
        public string RequiredDeliveryDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string DeliveryNotes { get; set; }
        public string NumberOfParcels { get; set; }
        public string TotalItems { get; set; }
        public string Part { get; set; }
        public string NumberOfParts { get; set; }
        public string CourierServiceId { get; set; }
        public string CourierServiceName { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingURL { get; set; }
        public string WarehouseId { get; set; }



    }

    public class Orderitem
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int UnitPriceVat { get; set; }
        public double Discount { get; set; }
        public object[] OrderItemNameValues { get; set; }
    }


    public class MdlMintSoftResponse
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public int OrderId { get; set; }
        public int DropShipOrderId { get; set; }
        public string OrderNumber { get; set; }
        public bool Success { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public string Message { get; set; }
        public Orderitems[] OrderItems { get; set; }
    }

    public class Orderitems
    {
        public string SKU { get; set; }
        public int ID { get; set; }
    }
    //Minsoft Api code by snehith
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
        public string LocationId { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class Warehouse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }
        public bool AllowTransfersIn { get; set; }
        public bool AllowAllocatedTransfers { get; set; }
        public bool AllowUnassignedLocations { get; set; }
        public bool AllocateBasedOnLocationTypePriority { get; set; }
        public bool IncludeAllocatedStockInReplenPoint { get; set; }
        public bool VerifyLocationsWhenPicking { get; set; }
        public bool Active { get; set; }
        public string Type { get; set; }
        public bool PrependWarehouseName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string CountryId { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
        public List<string> WarehouseReferenceFields { get; set; }
    }
    public class Location
    {
        public string Name { get; set; }
        public int LocationTypeId { get; set; }
        public string SimpleLocationName { get; set; }
        public string PickSequence { get; set; }
        public string LocationName { get; set; }
        public string WarehouseId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }


    public class CourierSerivce
    {
        public string CourierServiceTypeId { get; set; }
        public string Name { get; set; }
        public string TrackingURL { get; set; }
        public bool ActiveB { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }


    public class CourierSerivceType
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class Orderchannel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Logo { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class OrderStatuses
    {
        public string Name { get; set; }
        public string ExternalName { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class OrderDetails
    {
        public Channel Channel { get; set; }
        public Country Country { get; set; }
        public Currency Currency { get; set; }
        public Ordernamevalue[] OrderNameValues { get; set; }
        public object RecipientType { get; set; }
        public int ClientId { get; set; }
        public object CLIENT_CODE { get; set; }
        public string OrderNumber { get; set; }
        public string ExternalOrderReference { get; set; }
        public string OrderDate { get; set; }
        public string DespatchDate { get; set; }
        public string RequiredDespatchDate { get; set; }
        public string RequiredDeliveryDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CountryId { get; set; }
        public string Source { get; set; }
        public string Comments { get; set; }
        public object GiftMessages { get; set; }
        public object DeliveryNotes { get; set; }
        public object VATNumber { get; set; }
        public object EORINumber { get; set; }
        public object PIDNumber { get; set; }
        public string OrderStatusId { get; set; }
        public string NumberOfParcels { get; set; }
        public string TotalItems { get; set; }
        public float TotalWeight { get; set; }
        public float OrderValue { get; set; }
        public string Part { get; set; }
        public string NumberOfParts { get; set; }
        public string CourierServiceTypeId { get; set; }
        public string CourierServiceId { get; set; }
        public string CourierServiceName { get; set; }
        public object TrackingNumber { get; set; }
        public string TrackingURL { get; set; }
        public string ShippingTotalExVat { get; set; }
        public string ShippingTotalVat { get; set; }
        public string DiscountTotalExVat { get; set; }
        public string DiscountTotalVat { get; set; }
        public string TotalVat { get; set; }
        public bool PIIRemoved { get; set; }
        public string ShippingNet { get; set; }
        public string ShippingTax { get; set; }
        public string ShippingGross { get; set; }
        public string DiscountNet { get; set; }
        public string DiscountTax { get; set; }
        public object TotalOrderNet { get; set; }
        public string TotalOrderTax { get; set; }
        public float TotalOrderGross { get; set; }
        public string DiscountGross { get; set; }
        public string WarehouseId { get; set; }
        public object WAREHOUSE_CODE { get; set; }
        public string ChannelId { get; set; }
        public string CurrencyId { get; set; }
        public string DespatchedByUser { get; set; }
        public object OrderItems { get; set; }
        public bool OrderLock { get; set; }
        public object Tags { get; set; }
        public object SourceOrderDate { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Channel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Logo { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Ordernamevalue
    {
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ID { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }



    public class SalesOrderDetaildtl
    {
        public Orderitemnamevalue[] OrderItemNameValues { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string Quantity { get; set; }
        public string Allocated { get; set; }
        public string Commited { get; set; }
        public string OnBackOrder { get; set; }
        public string SourceLineSubTotal { get; set; }
        public string SourceLineTotalTax { get; set; }
        public string SourceLineTotalDiscount { get; set; }
        public string SourceLineTotal { get; set; }
        public string Price { get; set; }
        public string Vat { get; set; }
        public string Discount { get; set; }
        public string PriceNet { get; set; }
        public string Tax { get; set; }
        public string DiscountGross { get; set; }
        public string TaxRate { get; set; }
        public string DiscountNet { get; set; }
        public string DiscountTax { get; set; }
        public string NetPaid { get; set; }
        public string TaxPaid { get; set; }
        public string TotalTax { get; set; }
        public string Details { get; set; }
        public string SKU { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Orderitemnamevalue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Internal { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class ASNStatuses
    {
        public string Name { get; set; }
        public string Colour { get; set; }
        public string TextColour { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class ASNGoodsintype
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class ASNSupplier
    {
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string ContactEmail { get; set; }
        public string Active { get; set; }
        public string Code { get; set; }
        public string CurrencyId { get; set; }
        public string Currency { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class ClientDetails
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string BrandName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string PPINumber { get; set; }
        public string VATNumber { get; set; }
        public string EORINumber { get; set; }
        public string VatExempt { get; set; }
        public string NIREORINumber { get; set; }
        public string IOSSNumber { get; set; }
        public string CountryId { get; set; }
        public string ContactEmail { get; set; }
        public string PackagingInstructions { get; set; }
        public string CurrencyId { get; set; }
        public string Active { get; set; }
        public string OnStop { get; set; }
        public string AccountingIntegrationType { get; set; }
        public string CustomerRegistrationNumber { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class ASNPODetails
    {
        public string CLIENTSHORTNAME { get; set; }
        public string POReference { get; set; }
        public string Supplier { get; set; }
        public string ProductSupplierId { get; set; }
        public string EstimatedDelivery { get; set; }
        public string EstimatedTimeToDock { get; set; }
        public string WarehouseBookedDate { get; set; }
        public string BookedInDate { get; set; }
        public string Comments { get; set; }
        public string GoodsInType { get; set; }
        public string Quantity { get; set; }
        public Asnstatus ASNStatus { get; set; }
        public string ASNStatusId { get; set; }
        public string Shipped { get; set; }
        public string HoursLogged { get; set; }
        public string Items { get; set; }
        public string WarehouseId { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Asnstatus
    {
        public string Name { get; set; }
        public string Colour { get; set; }
        public string TextColour { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }



    public class ASNPOItemDetails
    {
        public Asnstatus ASNStatus { get; set; }
        public Item[] Items { get; set; }
        public string CLIENTSHORTNAME { get; set; }
        public string POReference { get; set; }
        public string Supplier { get; set; }
        public string ProductSupplierId { get; set; }
        public string EstimatedDelivery { get; set; }
        public string EstimatedTimeToDock { get; set; }
        public string WarehouseBookedDate { get; set; }
        public string BookedInDate { get; set; }
        public string Comments { get; set; }
        public string GoodsInType { get; set; }
        public string Quantity { get; set; }
        public string ASNStatusId { get; set; }
        public string Shipped { get; set; }
        public string HoursLogged { get; set; }
        public string WarehouseId { get; set; }
        public string ClientId { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

    public class Item
    {
        public object[] ASNItemAllocations { get; set; }
        public object[] ASNItemNameValues { get; set; }
        public string ASNId { get; set; }
        public string ProductId { get; set; }
        public string QuantityExpected { get; set; }
        public string QuantityReceieved { get; set; }
        public string QuantityBooked { get; set; }
        public string OnOrder { get; set; }
        public string SSCCNumber { get; set; }
        public string Complete { get; set; }
        public string Comments { get; set; }
        public string SourceLineId { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string ExpiryDate { get; set; }
        public string SKU { get; set; }
        public string EAN { get; set; }
        public string UPC { get; set; }
        public string NAME { get; set; }
        public string HasSerialNumber { get; set; }
        public string HasExpiryDate { get; set; }
        public string HasBatchNumber { get; set; }
        public string ProductImageURL { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }


    public class MintsoftCountries
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Code3 { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }
    public class MintsoftCurrencies
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string ID { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedByUser { get; set; }
    }

}
