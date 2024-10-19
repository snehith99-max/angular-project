using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOtlWhatsAppOrder : result
    {
        public List<whatsappordersummary_list> whatsappordersummary_list { get; set; }
    }
    public class whatsappordersummary_list: result
    {
        public string order_id { get; set; }
        public string kot_gid { get; set; }
        public string kot_tot_price { get; set; }
        public string customer_phone { get; set; }
        public string kot_delivery_charges { get; set; }
        public string source { get; set; }
        public string message_id { get; set; }
        public string order_type { get; set; }
        public string created_date { get; set; }
        public string payment_status { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string payment_method { get; set; }
        public string total_quantity { get; set; }
        public string branch_gid {  get; set; }
        public string total_product { get; set; }
        public string contact_id { get; set; }
        public string reject_reason { get; set; }
        public string line_items_total { get; set; }
        public string order_instructions { get; set; }
        public string order_status { get; set; }
        public string kitchen_status { get; set; }

        public List<Viewwhatsappsummary_list> Viewwhatsappsummary_list { get; set; }
    }
    public class Viewwhatsappsummary_list
    {
        public string kotdtl_gid { get; set; }
        public string kot_product_price { get; set; }
        public string product_quantity { get; set; }
        public string product_amount { get; set; }
        public string currency { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public string productgroup_name { get; set; }
    }
 
}