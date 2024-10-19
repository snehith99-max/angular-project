
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


    public class MdlImsTrnStockSummary : result
    {

        public List<stocksummary> stocksummary { get; set; }
        public List<GetProducttype> GetProducttype { get; set; }
        public List<GetProductGroup> GetProductGroup { get; set; }
        public List<GetProductUnitclass> GetProductUnitclass { get; set; }
        public List<GetProductNamDropdown> GetProductNamDtl { get; set; }
        public List<GetBranchDropdown> GetBranchDtl { get; set; }
        public List<stockaddnew_list> stockaddnew_list1 { get; set; }
        public List<GetproductsCodestock> ProductsCode { get; set; }
        public List<GetLocationstock> GetLocation { get; set; }
        public List<Poststock> Poststock { get; set; }
        public List<Getamendstock> Getamendstock { get; set; }
        public List<Getamendstocklist> Getamendstocklist { get; set; }
        public List<postamendstock> postamendstock { get; set; }

        public List<Getdamagestock> Getdamagestock { get; set; }
        public List<Getdamagedstocklist> Getdamagedstocklist { get; set; }

        public List<Getproductsplit> Getproductsplit { get; set; }
        public List<PostDamagedstock> PostDamagedstock { get; set; }
        public List<GetUomList> GetUomList { get; set; }
        public List<PostSplitstock> PostSplitstock { get; set; }
        public List<GetFinancialYear> GetFinancialYear { get; set; }

    }

    public class stocksummary : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string sku { get; set; }
        public string reference_gid { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }
        public string serial_flag { get; set; }
        public string transfer_qty { get; set; }
        public string location_name { get; set; }
        public string bin_number { get; set; }
        public string location_gid  { get; set; }
        public string branch_prefix { get; set; }

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

    public class GetProductNamDropdown : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }

    }
    public class GetBranchDropdown : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }

    }


    public class stockaddnew_list : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string reference_gid { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }
        public string serial_flag { get; set; }
        public string transfer_qty { get; set; }
        public string location_name { get; set; }
        public string bin_number { get; set; }
        public string productgroup_gid { get; set; }
        public string productuom_gid { get; set; }
        public string product_desc { get; set; }



    }

    public class GetproductsCodestock : result
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
    public class GetLocationstock : result
    {
        public string location_name { get; set; }
        public string location_gid { get; set; }
        public string branch_gid { get; set; }


    }

    public class Poststock : result
    {
        public string product_gid { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name  { get; set; }
        public string location_name { get; set; }
        public string unit_price { get; set; }
        public string display_field { get; set; }
        public string stock_qty { get; set; }
        public string uom_gid { get; set; }
        public string location_gid{ get; set; }
        public string finyear { get; set; }


    }


    public class Getamendstock : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string amend_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string branch_prefix { get; set; }
        public string producttype_name { get; set; }
        


    }

    public class Getamendstocklist  : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string amend_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }



    }

    public class Getproductsplit : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string amend_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }



    }
    public class postamendstock  : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string amend_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }
        public string remarks { get; set; }

        public string amend_type  { get; set; }

    }


    public class Getdamagestock : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string damaged_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string branch_prefix { get; set; }
        public string producttype_name { get; set; }



    }

    public class Getdamagedstocklist : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string amend_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }



    }
    public class PostDamagedstock : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string damaged_qty { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }
        public string remarks { get; set; }



    }

    public class GetUomList : result
    {
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
       
        public string productuom_name { get; set; }
        



    }
    public class PostSplitstock  : result
    {
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string stock_gid { get; set; }
        public string created_date { get; set; }
        public string stock_balance { get; set; }
        public string branch_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string split_qty  { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string branch_name { get; set; }
        public string producttype_name { get; set; }
        public string remarks { get; set; }
        public string income_qty { get; set; }
        public string location_gid  { get; set; }
        public string bin_gid { get; set; }
        public string grn_gid { get; set; }



    }

    public class GetFinancialYear : result
    {
        public string finyear { get; set; }
        public string finyear_gid { get; set; }

    }
}