using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlCustomerEnquiryEdit : result
    {
        public List<editenquirylist> editenquiry_list { get; set; }
        public List<editenquiryproductsummary_list> editenquiryproductsummarylist { get; set; }
        public List<editproductsummarylist> editproductsummary_list { get; set; }
        public List<Postedit> Post_edit { get; set; }
    }

    public class editenquirylist : result
    {
        public string enquiry_gid { get; set; }
        public string customerbranch_name { get; set; }
        public string enquiry_date { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string customer_name { get; set; }
        public string contact_number { get; set; }
        public string contact_person { get; set; }
        public string assign_to { get; set; }
        public string contact_email { get; set; }
        public string enquiry_remarks { get; set; }
        public string contact_address { get; set; }
        public string customer_requirement { get; set; }
        public string landmark { get; set; }
        public string closure_date { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string potential_value { get; set; }
        public string product_requireddate { get; set; }
        public string customer_gid { get; set; }
        public string customer_rating { get; set; }
    }

    public class editenquiryproductsummary_list : result
    {
        public string enquiry_gid { get; set; }
        public string potential_value { get; set; }
        public string qty_requested { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string product_name { get; set; }
        public string product_requireddate { get; set; }
       

    }

    public class editproductsummarylist : result
    {
        public string tmpsalesenquiry_gid { get; set; }
        public string qty_requested { get; set; }
        public string product_requireddate { get; set; }
        public string productgroup_name { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productuom_name { get; set; }
        public string product_gid { get; set; }
        public string potential_value { get; set; }
        public string enquiry_gid { get; set; }
    }

    public class Postedit : result
    {
        public string enquiry_gid { get; set; }
        public string branch_name { get; set; }
        public string closure_date { get; set; }
        public string branch_gid { get; set; }
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
        public string contact_number { get; set; }
        public string contact_person { get; set; }
        public string contact_email { get; set; }
        public string customerbranch_name { get; set; }
        public string contact_address { get; set; }
        public string enquiry_remarks { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string customer_requirement { get; set; }
        public string landmark { get; set; }
        public string assign_to { get; set; }
        public string customer_rating { get; set; }
    }
}