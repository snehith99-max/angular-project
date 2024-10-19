using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrQuotationAmend : result
    {
        public List<quotationamendlist> quotationamend_list { get; set; }
        public List<GetProductChangeQOAmend> GetProductChange_QOAmend { get; set; }
        public List<SegmentList> SegmentList { get; set; }
        public List<quoteamendproductlist> quoteamend_productlist { get; set; }
       public double grandtotal { get; set; } 
        public List<PostQuoteAmend_List> PostQuoteAmendList { get; set; }
        public List<AmendproductList> Amendproduct_List { get; set; }

        public string customer_gid { get; set; }
        public string product_gid { get; set; }
    }


    public class AmendproductList : result
    {
        public string tmpquotationdtl_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_quoted { get; set; }
        public string price { get; set; }
        public string tax_name { get; set; }
        public string productgroup_name { get; set; }
        public string productgroup_gid { get; set; }
        public string tax_amount { get; set; }
        public string tax_gid { get; set; }
        public string discount_amount { get; set; }
        public string discount_percentage { get; set; }
        public string product_price { get; set; }

    }
    public class quotationamendlist : result
    {
        public string quotation_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string quotation_date { get; set; }
        public string quotation_referencenumber { get; set; }
        public string customer_gid { get; set; }
        public string quotation_remarks { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string contact_person { get; set; }
        public string total_amount { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string contact_no { get; set; }
        public string contact_mail { get; set; }
        public string Grandtotal { get; set; }
        public string addon_charge { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string additional_discount { get; set; }
        public string currency_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string termsandconditions { get; set; }
        public string created_date { get; set; }
        public string branch_gid { get; set; }
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string salesperson_gid { get; set; }
        public string total_price { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string customerenquiryref_number { get; set; }
        public string roundoff { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string branch_name { get; set; }

    }

    public class GetProductChangeQOAmend : result
    {
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string unitprice { get; set; }
    }
    public class quoteamendproductlist : result
    {
        public string quotation_gid { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string taxsegment_gid { get; set; }
        public string taxsegmenttax_gid { get; set; }
        public string tmpquotationdtl_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string uom_name { get; set; }
        public string product_price { get; set; }
        public string discount { get; set; }
        public string product_total { get; set; }
        public string tax_name { get; set; }
        public string tax_amount { get; set; }
        public string qty_quoted { get; set; }
        public string discount_percentage { get; set; }
        public string grandtotal { get; set; }
        public string productuom_name { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string unitprice { get; set; }
        public string quantity { get; set; }
        public string totalamount { get; set; }
        public string tmpsalesorderdtl_gid { get; set; }
        public string unit { get; set; }
        public string total_amount { get; set; }
    }

    public class PostQuoteAmend_List : result
    {
        public string quotation_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string total_amount { get; set; }
        public string customercontact_names { get; set; }
        public string quotation_date { get; set; }
        public string quotation_referencenumber { get; set; }
        public string branch_name { get; set; }
        public string customer_name { get; set; }
        public string quotation_remarks { get; set; }
        public string quotation_referenceno1 { get; set; }
        public string payment_days { get; set; }
        public string delivery_days { get; set; }
        public string grandtotal { get; set; }
        public string termsandconditions { get; set; }
        public string customer_mobile { get; set; }
        public string customer_address { get; set; }
        public string customer_email { get; set; }
        public string addon_charge { get; set; }
        public string tax_name4 { get; set; }
        public string Grandtotal { get; set; }
        public string additional_discount { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string producttotalamount { get; set; }
        public string user_name { get; set; }
        public string freight_terms { get; set; }
        public string payment_terms { get; set; }
        public string roundoff { get; set; }
        public string freight_charges { get; set; }
        public string buyback_charges { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
    }
    public class SegmentList : result
    {
        public string taxsegment_gid { get; set; }
        public string product_gid { get; set; }
        public string customer_gid { get; set; }
        public string taxsegment_name { get; set; }
        public string tax_name { get; set; }
        public string tax_percentage { get; set; }
        public string tax_gid { get; set; }
        public string mrp_price { get; set; }
        public string cost_price { get; set; }
        public string tax_amount { get; set; }
    }
}