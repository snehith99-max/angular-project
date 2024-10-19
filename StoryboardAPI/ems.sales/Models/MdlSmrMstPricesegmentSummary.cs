using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using StoryboardAPI.Models;
using System.Web.Management;
using System.Drawing;

namespace ems.sales.Models
{
    public class MdlSmrMstPricesegmentSummary : result
    {
        public List<campaignassign_list> campaignassign_list { get; set; }

        public List<Getpricesegment_list> pricesegment_list { get; set; }
        public List<pricesegmentheader_list> pricesegmentheader_list { get; set; }
        public List<Getpricesegmentgrid_list> pricesegmentgrid_list { get; set; }
        public List<GetProductGroupDropdown> GetSmrGroupDtl { get; set; }
        public List<GetProductNameDropdown> GetSmrProductDtl { get; set; }
        public List<GetProductUnitDropdown> GetSmrUnitDtl { get; set; }
        public List<GetProduct> productgroup_list { get; set; }
        public List<GetProductName> OnChangeProductName { get; set; }
        public List<postproductunassign_list> postproductunassign_list { get; set; }
        public List<Getproduct_list> products_list { get; set; }
        public List<GetUnassignedlists> GetUnassignedlists { get; set; }
        public List<GetAssignedlists> GetAssignedlists { get; set; }
        public List<GetUnassigned> GetUnassigned { get; set; }
        public List<unassignproduct_list> unassignproduct_list { get; set; }
        public List<Getproductonchange> Getproductonchange { get; set; }
        public List<pricesegmentcustomer_list> pricesegmentcustomer_list { get; set; }
        public List<pricesegmentproduct_list> pricesegmentproduct_list { get; set; }
        public List<pricesegmentassign> pricesegmentassign { get; set; }
        public List<pricesegmentunassign> pricesegmentunassign { get; set; }



    }
    public class pricesegmentheader_list : result
    {
        public string pricesegment_name { get; set; }
        public string pricesegment_count { get; set; }
    }
    public class GetUnassigned : result
    {

        public string pricesegment_gid { get; set; }
        public string employee_gid1 { get; set; }

        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class pricesegmentproduct_list : result
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
    }
    public class pricesegmentcustomer_list : result

    {
        public string customer_name { get; set; }
        public string customer_id { get; set; }



    }

    public class GetAssignedlists : result
    {
        public string customer_gid { get; set; }
        public string employee_gid1 { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }
    public class Getproductonchange : result
    {
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string productuom_name { get; set; }
        public string product_price { get; set; }
        public string unitprice { get; set; }
    }

    public class GetUnassignedlists : result
    {

        public string pricesegment_gid { get; set; }
        public string employee_gid1 { get; set; }
        public string employee_name { get; set; }
        public string employee_gid { get; set; }
    }

    public class Getpricesegment_list : result
    {

        public string pricesegment_gid { get; set; }
        public string productcount { get; set; }
        public string customercount { get; set; }
        public string pricesegment_prefix { get; set; }
        public string pricesegment_code { get; set; }
        public string discount_percentage { get; set; }
        public string pricesegment_name { get; set; }
        public string editpricesegment_prefix { get; set; }
        public string editdiscount_percentage { get; set; }
        public List<string> products { get; set; }
        public string pricesegmentedit_code { get; set; }
        public string pricesegmentedit_name { get; set; }
        public string price_details { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }


    }
    public class Getpricesegmentgrid_list : result
    {

        public string pricesegment_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }


    }

    public class GetProductGroupDropdown : result
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
    }

    public class GetProductNameDropdown : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }
    public class GetProductUnitDropdown : result
    {
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
    }
    public class GetProduct : result
    {
        public string pricesegment_gid { get; set; }
        public string pricesegment_name { get; set; }
        public string discount_amount { get; set; }
        public string mrp { get; set; }
        public string product_desc { get; set; }
        public string sku { get; set; }
        public string product_code { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string stock_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string product_price { get; set; }
        public string customerproduct_code { get; set; }
        public string created_date { get; set; }

    }
    public class unassignproduct_list : result
    {
        public string pricesegment_gid { get; set; }
        public string pricesegment_name { get; set; }
        public string discount_amount { get; set; }
        public string mrp { get; set; }
        public string product_desc { get; set; }
        public string sku { get; set; }
        public string product_code { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string stock_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string pricesegment2product_gid { get; set; }
        public string product_price { get; set; }
        public string customerproduct_code { get; set; }
        public string created_date { get; set; }

    }
    public class postproductunassign_list:result
    {
        public string pricesegment_gid { get; set; }
        public productunassign_list[] productunassign_list;

    }
    public class productunassign_list : result
    {
        public string pricesegment2product_gid { get; set; }
    }

    public class GetProductName : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
    

    }
    public class Getproduct_list : result
    {

        public string pricesegment_gid { get; set; }
        public string product_gid { get; set; }
        public string stock_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string pricesegment_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string stocktype_gid { get; set; }
        public string originalProductPrice { get; set; }
        public string remarks { get; set; }
        public double product_price { get; set; }
        public string stock_qty { get; set; }
        public string grn_qty { get; set; }

        public string created_by { get; set; }
        public string created_date { get; set; }
        public string branch_gid { get; set; }
        public productgroup_list[] productgroup_list;


    }
    public class productgroup_list : result
    {
        public string product_gid { get; set; }
    }
    public class campaignassign_list : result
    {
        public string pricesegment_gid { get; set; }
        public campaignunassign[] campaignunassign;
        public campaignassign[] campaignassign;


    }
    public class campaignassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }

    public class campaignunassign_list : result
    {
        public string pricesegment_gid { get; set; }
        public string team_gid { get; set; }

        public campaignunassign[] campaignunassign;
        public campaignassign[] campaignassign;
    }
    public class campaignunassign : result
    {
        public string _id { get; set; }
        public string _key1 { get; set; }
        public string _key3 { get; set; }

        public string _name { get; set; }
    }

    //public class campaignassign_list : result
    //{
    //    public string pricesegment_gid { get; set; }
    //    public string pricesegment_name { get; set; }
    //    public string customer_gid { get; set; }
    //    public string customer_name { get; set; }
    //    public string employee_gid { get; set; }

    //    public campaignassign[] campaignassign;
    //}
    //public class campaignassign : result
    //{
    //    public string _id { get; set; }
    //    public string _key1 { get; set; }
    //    public string _key3 { get; set; }
    //    public string _name { get; set; }
    //    public string _key2 { get; set; }
    //    public string pricesegment_gid { get; set; }

    //}

    public class pricesegmentassign : result
    {

        public string customer_id { get; set; }
        public string customer_gid { get; set; }
        public string pricesegment_name { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string customer_country { get; set; }
        public string customer_since { get; set; }
        public string salesperson_gid { get; set; }

    }
    public class customerpricesegmentsubmitlist : result
    {
        public List<pricesegmentassign> pricesegmentassign { get; set; }
        public string pricesegment_gid { get; set; }

    }
    public class customerpricesegmentunsubmitlist : result
    {
        public List<pricesegmentunassign> pricesegmentunassign { get; set; }
        public string pricesegment_gid { get; set; }

    }
    public class pricesegmentunassign : result
    {

        public string customer_id { get; set; }
        public string customer_gid { get; set; }
        public string pricesegment_name { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string region_name { get; set; }
        public string customer_country { get; set; }
        public string salespersonname { get; set; }
        public string customer_since { get; set; }
        public string salesperson_gid { get; set; }

    }

}