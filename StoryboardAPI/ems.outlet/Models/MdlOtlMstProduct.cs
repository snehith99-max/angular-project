using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOtlMstProduct : result
    {
        public List<Products_list> Products_list { get; set; }
        public List<Product_listedit> Product_listedit { get; set; }
        public List<Product_listview> Product_listview { get; set; }
        public List<GetProducttype_list> GetProducttype_list { get; set; }
        public List<GetProductGroup_list> GetProductGroup_list { get; set; }
        public List<GetProductUnitclass_list> GetProductUnitclass_list { get; set; }
        public List<GetProductUnit_list> GetProductUnit_list { get; set; }
        public List<tax_list> tax_list { get; set; }

    }
    public class Products_list : result
    {


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
        public string symbol { get; set; }

        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_image { get; set; }
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
        public string tax { get; set; }



    }
    public class Product_listedit : result
    {

        public string id { get; set; }
        public string currency_code { get; set; }
        public string vendor_name { get; set; }
        public string product_status { get; set; }
        public string product_type { get; set; }
        public string shopify_productid { get; set; }
        public string batch_flag { get; set; }
        public string serial_flag { get; set; }
        public string purchasewarrenty_flag { get; set; }
        public string expirytracking_flag { get; set; }
        public string product_desc { get; set; }
        public string avg_lead_time { get; set; }
        public string product_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_image { get; set; }
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
    public class Product_listview : result
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
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string producttype_name { get; set; }
        public string productuomclass_name { get; set; }


    }
    public class GetProducttype_list
    {
        public string producttype_name { get; set; }
        public string producttype_gid { get; set; }

    }
    public class GetProductGroup_list
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }

    }
    public class GetProductUnitclass_list
    {
        public string productuomclass_gid { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }

    }
    public class GetProductUnit_list
    {
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }


    }
    public class tax_list : result
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
    }

}
