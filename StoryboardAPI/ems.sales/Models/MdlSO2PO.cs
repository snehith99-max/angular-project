//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ems.sales.Models
//{
//    public class MdlSO2PO
//    {
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace ems.sales.Models
{
    public class MdlSO2PO : result
    {

        public List<GetPurchaseOrder_lists1> GetPurchaseOrder_lists1 { get; set; }
        public List<GetPurchaseOrder1> GetPurchaseOrder1 { get; set; }


        public List<Getemployeelist> Getemployeelist { get; set; }
        public List<postpo_list> postpo_list { get; set; }
        public List<GetProductseg> GetProductseg { get; set; }
        public List<GetRaisePO> GetRaisePO { get; set; }

        public string file_name { get; set; }
        public string file_path { get; set; }

    }
    public class GetRaisePO : result
    {
        public string vendor_fax { get; set; }
        public string requested_details { get; set; }
        public string purchaserequisition_gid { get; set; }
        public string tmppurchaseorderdtl_gid { get; set; }
        public string requested_by { get; set; }
        public string purchaserequisition_date { get; set; }
        public string po_covernote { get; set; }
        public string email_address { get; set; }
        public string productuom_gid { get; set; }
        public string overalltax { get; set; }
        public string additional_discount { get; set; }
        public string tax_percentage { get; set; }
        public string netamount { get; set; }
        public string tax_name { get; set; }
        public string product_total { get; set; }
        public string overall_tax { get; set; }
        public string po_date { get; set; }
        public string discountamount { get; set; }
        public string tax_amount4 { get; set; }

        public string dispatch_name { get; set; }
        public string grandtotal { get; set; }

        public string Shipping_address { get; set; }

        public string contact_person { get; set; }

        public string pocovernote_address { get; set; }

        public string totalamount { get; set; }

        public string template_content { get; set; }
        public string contact_number { get; set; }
        public string remarks { get; set; }

        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string manualporef_no { get; set; }
        public string deliverytobranch { get; set; }
        public string branch_name { get; set; }
        public string branch_add1 { get; set; }
        public string vendor_contactnumber { get; set; }
        public string vendor_contact_person { get; set; }
        public string vendor_faxnumber { get; set; }
        public string vendor_companyname { get; set; }
        public string Requestor_details { get; set; }
        public string dispatch_mode { get; set; }

        public string address1 { get; set; }
        public string employee_name { get; set; }
        public string delivery_terms { get; set; }
        public string vendor_emailid { get; set; }
        public string vendor_address { get; set; }
        public string exchange_rate { get; set; }
        public string po_no { get; set; }

        public string ship_via { get; set; }
        public string addoncharge { get; set; }

        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string delivery_location { get; set; }
        public string currency_code { get; set; }
        public string shipping_address { get; set; }
        public string purchaseorder_reference { get; set; }
        public string purchaseorder_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string taxsegment_gid { get; set; }
        public string discount_amount { get; set; }
        public string total_amount { get; set; }
        public string freightcharges { get; set; }
        public string qyt_unit { get; set; }
        public string tax_amount { get; set; }
        public string approver_remarks { get; set; }

        public string delivery_days { get; set; }
        public string product_totalprice { get; set; }

        public string productgroup_name { get; set; }

        public string product_code { get; set; }

        public string product_name { get; set; }

        public string productuom_name { get; set; }

        public string product_price { get; set; }

        public string priority_n { get; set; }

        public string qty_ordered { get; set; }
        public string qty_Received { get; set; }

        public string qty_grnadjusted { get; set; }
        public string priority_remarks { get; set; }

        public string tax_amount2 { get; set; }

        public string tax_amount3 { get; set; }

        public string addon_amount { get; set; }
        public string product_gid { get; set; }
        public string display_field_name { get; set; }
        public string taxseg_taxname1 { get; set; }
        public string taxseg_taxpercent1 { get; set; }
        public string taxseg_taxamount1 { get; set; }
        public string taxseg_taxname2 { get; set; }
        public string taxseg_taxpercent2 { get; set; }
        public string taxseg_taxamount2 { get; set; }
        public string taxseg_taxname3 { get; set; }
        public string taxseg_taxpercent3 { get; set; }
        public string taxseg_taxamount3 { get; set; }
        public string tax_name4 { get; set; }
        public string email_id { get; set; }
        public string contact_telephonenumber { get; set; }
        public string contactperson_name { get; set; }
        public string tax_number { get; set; }
        public string address2 { get; set; }
        public string mode_despatch { get; set; }
        public string product_price_L { get; set; }
        public string discount_amount1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string overalltaxname { get; set; }

    }

    public class GetPurchaseOrder_lists1 : result
    {

        public string purchaserequisition_gid { get; set; }
        public string approved_date { get; set; }
        public string created_by { get; set; }
        public string purchaserequisition_date { get; set; }
        public string purchaserequisition_status { get; set; }
        public string costcenter_name { get; set; }
        public string overall_status { get; set; }

        public string purchaserequisition_remarks { get; set; }
        public string pr_raisedby { get; set; }
        public string mr_raisedby { get; set; }
        public string purchaserequisition_referencenumber { get; set; }

    }


    public class addselectpo_lists : result
    {
        public string branch_name { get; set; }
        public string purchaserequisition_gid { get; set; }
        //public string selectedItems { get; set; }

        public List<selectedItems> selectedItems { get; set; }




    }
    public class selectedItems : result
    {
        public string purchaserequisition_gid { get; set; }
    }

    public class GetPurchaseOrder1 : result
    {
        public string tmppurchaseorderdtl_gid { get; set; }
        public string tmppurchaseorder_gid { get; set; }
        public string tax1_gid { get; set; }
        public string tax2_gid { get; set; }
        public string uom_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string product_price { get; set; }
        public string qty_ordered { get; set; }
        public string qty_requested { get; set; }
        public string discount_amount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage3 { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount2 { get; set; }
        public string tax_amount3 { get; set; }
        public string product_totalprice { get; set; }
        public string excise_percentage { get; set; }
        public string excise_amount { get; set; }
        public string product_total { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string producttype_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string display_field { get; set; }
        public string purchaserequisitiondtl_gid { get; set; }
        public string salesorderdtl_gid { get; set; }
        public string user_name { get; set; }

        public string discount_percentage { get; set; }



    }
    public class Getemployeelist : result
    {

        public string employee_mobileno { get; set; }
        public string department_name { get; set; }
        public string employee_phoneno { get; set; }
        public string employee_emailid { get; set; }
        public string employee_name { get; set; }



    }


    public class postpo_list : result
    {

        public string purchaseorder_gid { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string poref_no { get; set; }
        public string po_date { get; set; }
        public string expected_date { get; set; }
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string contact_telephonenumber { get; set; }
        public string vendor_address { get; set; }
        public string shipping_address { get; set; }
        public string address1 { get; set; }
        public string employee_name { get; set; }
        public string delivery_terms { get; set; }
        public string Requestor_details { get; set; }
        public string despatch_mode { get; set; }
        public string currency_gid { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string po_covernote { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
        public string template_gid { get; set; }
        public string totalamount { get; set; }
        public string addoncharge { get; set; }
        public string additional_discount { get; set; }
        public string freightcharges { get; set; }
        public string roundoff { get; set; }
        public string grandtotal { get; set; }
        public string purchaserequisitiondtl_gid { get; set; }
        public string tax_gid { get; set; }
        public string payment_terms { get; set; }

        // ------------------------------------
        public string quotation_gid { get; set; }
        public string contactperson_name { get; set; }
        public string email_id { get; set; }
        public string costcenter_gid { get; set; }
        public string priority_flag { get; set; }
        public string taxsegment_gid { get; set; }
        public string tax_amount4 { get; set; }
        public string tax_name4 { get; set; }



        public List<Posummary_list> Posummary_list { get; set; }
    }

    public class Posummary_list : result
    {
        public string tmppurchaseorderdtl_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_ordered { get; set; }
        public string product_price { get; set; }
        public string discount_percentage { get; set; }
        public string productdiscount_amountvalue { get; set; }
        public string tax_name { get; set; }
        public string tax_name2 { get; set; }
        public string tax_percentage { get; set; }
        public string tax_percentage2 { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount3 { get; set; }
        public string producttotal_amount { get; set; }

        public string productuom_gid { get; set; }
        public string producttype_gid { get; set; }
        public string tax_name3 { get; set; }
        public string tax_percentage3 { get; set; }
        public string display_field { get; set; }
        public string display_field_old { get; set; }
        public string needby_date { get; set; }
        public string purchaserequisitiondtl_gid { get; set; }


    }

    public class GetProductseg : result
    {
        public string tsproduct_gid { get; set; }
        public string tsproduct_code { get; set; }
        public string tstaxsegment_gid { get; set; }
        public string tsproduct_name { get; set; }
        public string tax_prefix { get; set; }
        public string tstaxsegment_name { get; set; }
        public string tstax_name { get; set; }
        public string tstax_gid { get; set; }
        public string tstax_percentage { get; set; }
        public string tstax_amount { get; set; }
        public string tsmrp_price { get; set; }
        public string tscost_price { get; set; }


    }

}