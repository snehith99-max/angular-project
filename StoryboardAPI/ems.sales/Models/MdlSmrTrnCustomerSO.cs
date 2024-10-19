using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnCustomerSO : result
    {
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
        public List<Customersalesorder_list> Customersalesorder_list { get;set;}
        public List<GetProduct_list> GetProduct_list { get;set;}
        public List<CustomerGetproductgroup> CustomerGetproductgroup { get;set;}  
        public List<CustomerGetProductsearchs> CustomerGetProductsearchs { get; set; }
        public List<CustomerPostProduct_list> CustomerPostProduct_list { get; set; }
        public List<CustomerGetTaxSegmentListorder> CustomerGetTaxSegmentListorder { get; set; }   
        public List<CustomerSOProductList1> CustomerSOProductList1 { get; set; }    
        public List<CustomertaxSegments_list> CustomertaxSegments_list { get; set; }   
        public List<Customersalesorders_list> Customersalesorders_list { get; set; }  
        public List<Customerpostsales_list> Customerpostsales_list { get; set; }
        public List <Customerpostsalesorderdetails_list> Customerpostsalesorderdetails_list { get; set; }
        public List <Customerpostsalesorder_list> Customerpostsalesorder_list { get; set; }
        public List<orderstatuscount_list> orderstatuscount_list { get; set; }
        
    }


    public class Customerpostsalesorder_list : result
    {
        public string so_referenceno1 { get; set; }
        public string total_amount { get; set; }
        public string gst_amount { get; set; }
        public string salesorder_date { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_contact_person { get; set; }
        public string billing_email { get; set; }
        public string customer_email { get; set; }
        public string customer_mobile { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string currency_code { get; set; }
        public string salesorder_gid { get; set; }
        public string exchange_rate { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string payment_days { get; set; }
        public string so_referencenumber { get; set; }
        public string shipping_to { get; set; }
        public string delivery_days { get; set; }
        public string order_instruction { get; set; }
        public string so_remarks { get; set; }
        public string salesperson_name { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string uom_name { get; set; }
        public string qty_quoted { get; set; }
        public string selling_price { get; set; }
        public string price { get; set; }
        public string product_requireddate { get; set; }
        public string product_price { get; set; }
        public string addon_charge { get; set; }
        public string additional_discount { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string roundoff { get; set; }
        public string Grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string margin_percentage { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string totax { get; set; }
        public string tax_name4 { get; set; }
        public string total_price { get; set; }


    }
   
    public class orderstatuscount_list : result
    {
        public string deliverycount { get; set; }
        public string pendingcount { get; set; }
        public string totalcount { get; set; }
    }
    public class Customerpostsalesorderdetails_list : result
    {
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string uom_name { get; set; }
        public string salesorder_gid { get; set; }
        public string qty_quoted { get; set; }
        public string margin_amount { get; set; }
        public string margin_percentage { get; set; }
        public string product_price { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string discount_percentage { get; set; }
        public string discount_amount { get; set; }
        public string productgroup_name { get; set; }

        public string price { get; set; }
    }

    public class Customerpostsales_list : result
    {

        public string total_amount { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string currency_gid { get; set; }
        public string leadbank_gid { get; set; }
        public string Grandtotal { get; set; }
        public string campaign_gid { get; set; }
        public string salesperson_gid { get; set; }
        public string gst_amount { get; set; }
        public string customer_contact_person { get; set; }
        public string salesorder_gid { get; set; }
        public string tax_amount { get; set; }
        public string product_code { get; set; }
        public string tax_name { get; set; }
        public string customer_address2 { get; set; }
        public string customer_address { get; set; }
        public string productuom_gid { get; set; }
        public string customer_city { get; set; }
        public string additional_discount { get; set; }

        public string currencyexchange_gid { get; set; }

        public string countryname { get; set; }

        public string region_name { get; set; }
        public string customer_type { get; set; }
        public string statuses { get; set; }

        public string customer_state { get; set; }
        public string exchange { get; set; }

        public string grandtotal { get; set; }

        public string gst_number { get; set; }
        public string postal_code { get; set; }

        public string customercontact_name { get; set; }
        public string customer_email { get; set; }
        public string customermail { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string customer_mobile { get; set; }
        public string designation { get; set; }
        public string fax { get; set; }
        public string fax_area_code { get; set; }
        public string fax_country_code { get; set; }
        public string branch_gid { get; set; }
        public string salesorder_date { get; set; }
        public string customerbranch_gid { get; set; }
        public string customercontact_gid { get; set; }
        public string exchange_rate { get; set; }

        public string customercontact_names { get; set; }
        public string addoncharge { get; set; }

        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string customer_Address { get; set; }
        public string currency_code { get; set; }
        public string so_referencenumber { get; set; }
        public string so_referenceno1 { get; set; }
        public string so_remarks { get; set; }
        public string contact_person { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string shipping_to { get; set; }
        public string tax_name4 { get; set; }
        public string txttaxamount_1 { get; set; }
        public string totalamount { get; set; }
        public string total_price { get; set; }

        public string delivery_days { get; set; }

        public string productgroup_name { get; set; }

        public string user_name { get; set; }

        public string product_name { get; set; }

        public string productuom_name { get; set; }

        public string start_date { get; set; }

        public string end_date { get; set; }

        public string freight_charges { get; set; }
        public string main_branch { get; set; }
        public string created_flag { get; set; }
        public string did_number { get; set; }
        public string country_gid { get; set; }
        public string leadbankcontact_gid { get; set; }
        public string main_contact { get; set; }
        public string branch_name { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount_2 { get; set; }
        public string taxamount_3 { get; set; }
        public string taxamount_l { get; set; }
        public string selling_price_l { get; set; }
        public string totalamount_l { get; set; }
        public string discount_amount_l { get; set; }
        public string marginpercentage { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string termsandcondition { get; set; }
        public string buyback_charges { get; set; }
        public string addon_charge { get; set; }

        public string tax_amount2 { get; set; }

        public string tax_amount3 { get; set; }

        public string productgroup_gid { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string customerproduct_code { get; set; }
        public string display_field { get; set; }
        public string selling_price { get; set; }
        public string qty_quoted { get; set; }
        public string margin_percentage { get; set; }
        public string margin_amount { get; set; }
        public string addon_charge_l { get; set; }
        public string additional_discount_l { get; set; }
        public string grandtotal_l { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax2_gid { get; set; }
        public string tax3_gid { get; set; }
        public string vessel_name { get; set; }
        public string tax_amount_l { get; set; }

        public string tax_amount2_l { get; set; }
        public string tax_amount3_l { get; set; }
        public string product_discount { get; set; }
        public string price { get; set; }
        public string salesorder_refno { get; set; }
        public string vendor_gid { get; set; }

        public string slno { get; set; }
        public string product_requireddate { get; set; }

        public string product_requireddateremarks { get; set; }
        public string vendor_price { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string order_type { get; set; }
        public string mrp { get; set; }
        public string taxamount_1 { get; set; }
        public string producttotalamount { get; set; }


    }

    public class Customersalesorders_list : result
    {
        public List<CustomerSOProductList1> SOProductList { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount3 { get; set; }
        public string taxprecentage1 { get; set; }
        public string taxprecentage2 { get; set; }
        public string taxprecentage3 { get; set; }
        public string total_amount { get; set; }
        public string unit { get; set; }
        public string grand_total { get; set; }
        public string grandtotal { get; set; }
        public string totaltaxamount { get; set; }
        public string tmpsalesenquiry_gid { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string created_by { get; set; }
        public string salesorder_gid { get; set; }
        public string customerproduct_code { get; set; }
        public string selling_price { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string currency_code { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tax_prefix { get; set; }
        public string tax_prefix2 { get; set; }
        public string tax_prefix3 { get; set; }
        public string employee_gid { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string product_code { get; set; }
        public string slno { get; set; }
        public string productgroup_name { get; set; }
        public string product_price { get; set; }
        public string display_field { get; set; }
        public string taxgid2 { get; set; }
        public string taxgid1 { get; set; }
        public string taxgid3 { get; set; }
        public string taxname2 { get; set; }
        public string taxname3 { get; set; }
        public string taxname1 { get; set; }

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
        public string unitprice { get; set; }
        public string exchange_rate { get; set; }
        public string quantity { get; set; }
        public string productuom_name { get; set; }
        public string totalamount { get; set; }
        public string producttotalamount { get; set; }
        public double discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string product_type { get; set; }
        public string taxsegment_name { get; set; }

        public string mrp_price { get; set; }
        public string cost_price { get; set; }



    }
    public class CustomertaxSegments_list
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string taxsegment_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxAmount { get; set; }
    }
    public class CustomerSOProductList1
    {
        public List<CustomertaxSegments_list> taxSegments { get; set; }
        public string qty_requested { get; set; }
        public string quantity { get; set; }
        public string discount_persentage { get; set; }
        public string discount_amount { get; set; }
        public string tax1 { get; set; }
        public string tax2 { get; set; }
        public string totalTaxAmount { get; set; }
        public string tax_amount { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount1 { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tax_name { get; set; }
        public string salesorder_gid { get; set; }
        public string exchange_rate { get; set; }
        public string discount_percentage { get; set; }
        public string discountamount { get; set; }
        public string currency_code { get; set; }
        public string tax_gids_string { get; set; }
        public string total_amount { get; set; }
        public string product_name { get; set; }
        public string product_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string tax3 { get; set; }
        public string display_field { get; set; }
        public string unitprice { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string unit_price { get; set; }

    }
    public class Customersalesorder_list : result
    {
        public string contact { get; set; }
        public string customer_gid { get; set; }
        public string salesorder_gid { get; set; }
        public string salesorder_date { get; set; }
        public string so_referenceno1 { get; set; }
        public string customer_name { get; set; }
        public string branch_name { get; set; }
        public string so_type { get; set; }
        public string Grandtotal { get; set; }
        public string user_firstname { get; set; }
        public string salesorder_status { get; set; }
        public string salesperson_name { get; set; }
        public string billing_email { get; set; }
        public string so_remarks { get; set; }
        public string customer_contact_person { get; set; }
        public string customer_email { get; set; }
        public string invoice_gid { get; set; }
    }

    public class CustomerGetproductgroup : result
    {
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
    }
    public class CustomerGetProductsearchs
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string unitprice { get; set; }
        public string producttype_gid { get; set; }
        public double taxamount1 { get; set; }
        public double taxamount2 { get; set; }
        public double quantity { get; set; }
        public double total_amount { get; set; }
        public double discount_amount { get; set; }
        public double discount_percentage { get; set; }
        public double discount_persentage { get; set; }

    }

    public class CustomerPostProduct_list : result
    {
        public string producttype_name { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productquantity { get; set; }
        public string unitprice { get; set; }
        public string productdiscount { get; set; }
        public string discountprecentage { get; set; }
        public string producttotal_amount { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount3 { get; set; }
        public string taxgid1 { get; set; }
        public string taxgid2 { get; set; }
        public string taxgid3 { get; set; }
        public string taxprecentage1 { get; set; }
        public string taxprecentage2 { get; set; }
        public string taxprecentage3 { get; set; }
        public string exchange_rate { get; set; }
        public string customer_gid { get; set; }
        public string taxname3 { get; set; }
        public string taxname2 { get; set; }
        public string taxname1 { get; set; }
        public string discount_amount { get; set; }
        public string salesorder_gid { get; set; }
        public string taxsegment_gid { get; set; }
    }

    public class CustomerGetTaxSegmentListorder : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string producttype_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
        public double total_amount { get; set; }
        public double quantity { get; set; }
        public double discount_percentage { get; set; }
        public double discount_amount { get; set; }
        public string tax_prefix { get; set; }
        public string exchange_rate { get; set; }
        public string currencyexchange_gid { get; set; }

    }

    public class GetProduct_list :  result
    {
        public string product_name { get; set; }
        public string product { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string pricesegment_name { get; set; }
        public string pricesegment_code { get; set; }
        public string product_gid { get; set; }
        public string productuomclass_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuomclass_gid { get; set; }
        public string productgroup_gid { get; set; }        
        public string producttype_gid { get; set; }        
        public string pricesegment_gid { get; set; }        
        public string unit_price { get; set; }        
        public string tax_amount { get; set; }        
        public string tax_percentage { get; set; }        
        public string tax_name { get; set; }        
        public string tax_gid { get; set; }        
        public string taxsegment_name { get; set; }        
        public string taxsegment_gid { get; set; }                      
        public double quantity { get; set; }        
    }

}