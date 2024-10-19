using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnStockTransferSummary:result
    {
        public List<stocktransfersummary> stocktransfersummary {  get; set; }
        public List<Stockproduct_list> Stockproduct_list {  get; set; }
        public List<Stockdeatails_list> Stockdeatails_list {  get; set; }
        public List<stocktransferbranchsummary> stocktransferbranchsummary {  get; set; }
        public List<stocktransferbranchview> stocktransferbranchview {  get; set; }
        public List<stocktransferlocationview> stocktransferlocationview {  get; set; }
        public List<stocktransferlocationproductview> stocktransferlocationproductview {  get; set; }
        public List<branchaddsummary> branchaddsummary {  get; set; }
        public List<branchtransfer> branchtransfer {  get; set; }
        public List<Poststocktransfer> Poststocktransfer { get; set; }
        public List<Postlocationstocktransfer> PostLocationstocktransfer { get; set; }
        public List<Location> Location { get; set; }
        public List<LocationTo> LocationTo { get; set; }
        public List<GetProductCode1> GetProductCode1 { get; set; }
        public List<GetProductgroup> GetProductgroup { get; set; }
        public List<GetProduct1> GetProduct1 { get; set; }
        public List<GetPopsummary_list> GetPopsummary_list { get; set; }
        public List<productlist1> productlist1 { get; set; }

        public List<GetPop_list> GetPop_list { get; set; }
        public List<productsummary_list1> productsummary_list1 { get; set; }
        public List<stocktransferreport_list> stocktransferreport_list { get; set; }
        public List<stocktransferapproval_list> stocktransferapproval_list { get; set; }

    }
    public class stocktransferbranchsummary : result
    {
        public string branch_from { get; set; }
        public string branch_to { get; set; }
        public string stocktransfer_gid { get; set; }
        public string transfered_by { get; set; }
        public string status { get; set; }
        public string product_gid { get; set; }
        public string transfer_date { get; set; }
        public string remarks { get; set; }
        public string si_no { get; set; }
        public string stock_gid { get; set; }
        public string user_firstname { get; set; }
    }
    public class stocktransfersummary : result
    {
        public string branch_from {  get; set; }
        public string branch_to {  get; set; }
        public string stocktransfer_gid {  get; set; }
        public string transfered_by {  get; set; }
        public string status {  get; set; }
        public string product_gid {  get; set; }
        public string transfer_date {  get; set; }
        public string remarks {  get; set; }
        public string si_no {  get; set; }
        public string stock_gid {  get; set; }
        public string user_firstname {  get; set; }
    }
    public class stocktransferbranchview : result
    {
        public string branch_from { get; set; }
        public string branch_to { get; set; }
        public string stocktransfer_gid { get; set; }
        public string transfered_by { get; set; }
        public string status { get; set; }
        public string product_gid { get; set; }
        public string transfer_date { get; set; }
        public string remarks { get; set; }
        public string si_no { get; set; }
        public string stock_gid { get; set; }
        public string user_firstname { get; set; }
        public string stock_qty { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_desc { get; set; }
    }
    public class stocktransferlocationview : result
    {
        public string branch_from { get; set; }
        public string branch_to { get; set; }
        public string stocktransfer_gid { get; set; }
        public string transfered_by { get; set; }
        public string status { get; set; }
        public string product_gid { get; set; }
        public string transfer_date { get; set; }
        public string remarks { get; set; }
        public string si_no { get; set; }
        public string stock_gid { get; set; }
    }
    public class stocktransferlocationproductview : result
    {
       
        public string stocktransferdtl_gid { get; set; }
        public string stock_qty { get; set; }
        public string product_gid { get; set; }
        public string productuom_name { get; set; }
        public string display_field { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
    }
    public class branchaddsummary : result
    {
        public string uom_gid { get; set; }
        public string created_date { get; set; }
        public string reference_gid { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string stock_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }
        public string serial_flag { get; set; }
        public string transfer_qty { get; set; }
        public string location_name { get; set; }
        public string bin_number { get; set; }
    }
    public class branchtransfer : result
    {
        public string stock_gid { get; set; }
        public string branch_name { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_desc { get; set; }
        public string unit_price { get; set; }
        public string product_gid { get; set; }
        public string stock_qty { get; set; }
        public string uom_gid { get; set; }
    }

    public class Poststocktransfer : result
    {
        public string product_gid { get; set; }
        public string branch_gid { get; set; }
        public string user_gid { get; set; }
        public string stock_gid { get; set; }
        public string remarks { get; set; }
        public string stock_qty { get; set; }
        public string stock_balance { get; set; }
        public string uom_gid { get; set; }
        public string transfer_stock { get; set; }
        public string employee_gid { get; set; }
        public string stock_price { get; set; }
        public string location_name { get; set; }
        public string location_gid { get; set; }
        public string branch_name { get; set; }
        public string branch_name1 { get; set; }
        public string product_desc { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string unit_price { get; set; }

    }
    public class Location
    {
        public string location_name { get; set; }
        public string location_gid { get; set; }
        public string branch_gid { get; set; }


    }
    public class LocationTo
    {
        public string locationto_name { get; set; }
        public string locationto_gid { get; set; }


    }

    public class GetProductCode1 : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }

    }
    public class GetProductgroup : result
    {
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }


    }
    public class GetProduct1 : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }


    }
    public class GetPopsummary_list : result
    {
        public string product_gid { get; set;}
        public string created_date { get; set;}
        public string stock_gid { get; set;}
        public string display_field { get; set;}
        public string uom_gid { get; set;}
        public string reference_gid { get; set;}
        public string stock_qty { get; set;}
        public string product_name { get; set;}
        public string productuom_name { get; set;}
        public string productuom_gid { get; set;}
        public string productgroup_name { get; set;}
        public string product_code { get; set;}
        public string issued_qty { get; set;}
        public string stock_total { get; set;}
        public string location_gid { get; set;}
        public string branch_gid { get; set;}
       
    }
    public class GetPop_list : result
    {
        public string stock_quantity { get; set; }
       
    }
    public class Postlocationstocktransfer : result
    {
        public string product_gid { get; set; }
        public string branch_gid { get; set; }
        public string user_gid { get; set; }
        public string stock_gid { get; set; }
        public string remarks { get; set; }
        public string stock_qty { get; set; }
        public string stock_balance { get; set; }
        public string uom_gid { get; set; }
        public string transfer_stock { get; set; }
        public string employee_gid { get; set; }
        public string stock_price { get; set; }
        public string location_name { get; set; }
        public string location_gid { get; set; }
        public string branch_name { get; set; }
        public string branch_name1 { get; set; }
        public string product_desc { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string unit_price { get; set; }
        public string locationto_gid { get; set; }
        public string req_remarks { get; set; }

    }
    public class productlist1 : result
    {
        public string product_gid { get; set; }
        public double qty_requested { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string display_field { get; set; }
        public double stock_quantity { get; set; }

    }
    public class productsummary_list1 : result
    {
        public string tmpstocktransfer_gid { get; set; }
        public string qty_requested { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string display_field { get; set; }
        public string stock_quantity { get; set; }

    }
    public class stocktransferreport_list : result
    {
        public string branch_name { get; set; }
        public string Transfer_qty { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string branchto_gid { get; set; }
        public string stock_quantity { get; set; }
        public string transfered_date { get; set; }
        public string transfered_by { get; set; }
        public string product_gid { get; set; }
        public string branchfrom_name { get; set; }
        public string branchto_name { get; set; }
    }
    public class stocktransferapproval_list : result
    {
        public string stocktransfer_gid { get; set; }
        public string transfered_by { get; set; }
        public string status { get; set; }
        public string branchfrom_name { get; set; }
        public string branchto_name { get; set; }
        public string transfer_date { get; set; }
        public string product_gid { get; set; }
        public string remarks { get; set; }
        public string si_no { get; set; }
        public string stock_gid { get; set; }
        public string user_firstname { get; set; }
    }

    public class Stockproduct_list
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string stock_qty { get; set; }
    }

    public class Stockdeatails_list
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string stock_qty { get; set; }
    }
}