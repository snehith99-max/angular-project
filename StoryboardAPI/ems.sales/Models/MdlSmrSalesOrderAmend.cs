using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrSalesOrderAmend : result
    {
        public List<SOAmendsummaryList> SOAmendsummary_List { get; set; }
        public List<amendtemplist> amendtemp_list { get; set; }
        public List<PostAmendSO_List> PostAmendSOList { get; set; }
        public List<editorderList> editorderList { get; set; }
        public List<Salesorders_list> Salesorders_list { get; set; }

        public double grand_total { get; set; }
    }

    public class editorderList : result
    {
        public string salesorder_gid { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string product_code { get; set; }
        public string unitprice { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string slno { get; set; }
        public string productgroup_gid { get; set; }
        public string tax_percentage { get; set; }
        public string productgroup_name { get; set; }
        public string product_price { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string margin_amount { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string margin_percentage { get; set; }
        public string unit { get; set; }
        public string tax_name { get; set; }
        public string price { get; set; }
        public string totalamount { get; set; }
        public string total_amount { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string salesperson_gid { get; set; }
        public string user_name { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string buyback_charges { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string currency_gid { get; set; }
        public string insurance_charges { get; set; }
        public string customer_email { get; set; }
        public string salesorder_refno { get; set; }
    }
        public class SOAmendsummaryList : result
    {
        public string salesorder_gid { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string roundoff { get; set; }
        public string salesorder_date { get; set; }
        public string customer_gid { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string customer_mobile { get; set; }
        public string tax_gid { get; set; }
        public string gst_amount { get; set; }
        public string customer_contact_person { get; set; }
        public string total_price { get; set; }
        public string shipping_to { get; set; }
        public string termsandconditions { get; set; }
        public string so_remarks { get; set; }
        public string so_referencenumber { get; set; }
        public string Grandtotal { get; set; }
        public string delivery_days { get; set; }
        public string payment_days { get; set; }
        public string additional_discount { get; set; }
        public string addon_charge { get; set; }
        public string branch_name { get; set; }
        public string campaign_gid { get; set; }
        public string so_referenceno1 { get; set; }
        public string total_amount { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string salesperson_gid { get; set; }
        public string user_name { get; set; }
        public string freight_charges { get; set; }
        public string packing_charges { get; set; }
        public string buyback_charges { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string currency_gid { get; set; }
        public string insurance_charges { get; set; }
        public string customer_email { get; set; }
        public string salesorder_refno { get; set; }
        public string tax_name4 { get; set; }
        public string tax_amount { get; set; }
    }

    public class amendtemplist : result
    {
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string tax_name { get; set; }
        public string salesorder_gid { get; set; }
        public string product_code { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string totalamount { get; set; }
        public string tax_amount { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string discount_amount { get; set; }
        public string tax_percentage { get; set; }
        public string price { get; set; }
        public string product_gid { get; set; }
        public string uom_gid { get; set; }
        public string uom_name { get; set; }
        public string qty_quoted { get; set; }
        public string product_price { get; set; }
        public double grand_total { get; set; }
    }

    public class PostAmendSO_List : result
    {
        public string customer_gid { get; set; }
        public string user_name { get; set; }
        public string tax_name4 { get; set; }
        public string tax_amount4 { get; set; }
        public string salesorder_gid { get; set; }
        public string currency_code { get; set; }
        public string total_amount { get; set; }
        public string exchange_rate { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string producttotalamount { get; set; }
        public string Grandtotal { get; set; }
        public string so_referencenumber { get; set; }
        public string shipping_to { get; set; }
        public string so_referenceno1 { get; set; }
        public string vessel_name { get; set; }
        public string salesorder_date { get; set; }
        public string customer_name { get; set; }
        public string customer_contact_person { get; set; }
        public string customer_address { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string so_remarks { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string termsandconditions { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string roundoff { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string customer_branch { get; set; }
        public string quotation_refno { get; set; }
        public string currency_gid { get; set; }
        public string salesperson_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup { get; set; }
    }

    public class Salesorders_list : result
    {
        public string total_amount { get; set; }
        public string unit { get; set; }
        public string grand_total { get; set; }
        public string grandtotal { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string created_by { get; set; }
        public string salesorder_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string selling_price { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }

        public string employee_gid { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string slno { get; set; }
        public string productgroup_name { get; set; }
        public string product_price { get; set; }
        public string display_field { get; set; }

        public string qty_quoted { get; set; }
        public string discount_percentage { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string discount_amount { get; set; }
        public string uom_gid { get; set; }
        public string uom_name { get; set; }

        public string price { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_gid { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }

        public string vendor_gid { get; set; }

        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }

        public string tax_percentage3 { get; set; }
        public string order_type { get; set; }
        public string product_requireddateremarks { get; set; }
        public string product_requireddate { get; set; }
        public double unitprice { get; set; }
        public double quantity { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string producttotalamount { get; set; }
        public double discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string product_type { get; set; }

    }

}