using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlExpiryTracker : result
    {
        public List<PostExpiryDays_list> PostExpiryDays_list { get; set; }
        public List<Getupdaysproduct_list> Getupdaysproduct_list { get; set; }
        public List<Getexpiryproduct_list> Getexpiryproduct_list { get; set; }
    }
    public class PostExpiryDays_list: result
    {
        public string expirydate { get; set; }
        public string product_gid { get; set; }
    }
    public class Getupdaysproduct_list: result
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
        public string tax { get; set; }
    }
    public class Getexpiryproduct_list : result
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
        public string tax { get; set; }
    }
}