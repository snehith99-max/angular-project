using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsMstReorderlevel:result
    {
        public List<rol_list> rol_list { get; set; }
        public List<ProductGroup> ProductGroup { get; set; }
        public List<Producttype> Producttype { get; set; }
        public List<ProductUnitclass> ProductUnitclass { get; set; }
        public List<ProductNamDropdown> ProductNamDropdown { get; set; }
        public List<BranchDropdown> BranchDropdown { get; set; }
        public List<productsCode> productsCode { get; set; }
        public List<Product_names> Product_names { get; set; }
        public List<postrollist> postrollist { get; set; }
        public List<EditRol_list> EditRol_list { get; set; }
        public List<roledit_list> roledit_list { get; set; }
    }
    public class rol_list : result
    {
        public string rol_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string branch_name { get; set; }
        public string productuom_name { get; set; }
        public string reorder_level { get; set; }
        public string display_field { get; set; }
        public string branch_prefix { get; set; }
    }
    public class ProductGroup : result
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
    }
    public class Producttype : result
    {
        public string producttype_gid { get; set; }
        public string producttype_name { get; set; }
    }
    public class ProductUnitclass : result
    {
        public string productuomclass_name { get; set; }
        public string productuomclass_gid { get; set; }
        public string productuomclass_code { get; set; }
    }
    public class ProductNamDropdown : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }

    public class BranchDropdown : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string address1 { get; set; }
    }

    public class productsCode : result
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
        public string product_desc { get; set; }
    }
    public class Product_names : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }
    public class postrollist : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string uom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string productuom_gid { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string reorder_level { get; set; }
        public string display_field { get; set; }
    }
    public class EditRol_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productuom_gid { get; set; }
        public string branch_gid { get; set; }
        public string rol_gid { get; set; }
        public string branch_name { get; set; }
        public string reorder_level { get; set; }
        public string product_desc { get; set; }
    }
    public class roledit_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string uom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string productuom_gid { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string reorder_level { get; set; }
        public string product_desc { get; set; }
        public string rol_gid { get; set; }

    }
}