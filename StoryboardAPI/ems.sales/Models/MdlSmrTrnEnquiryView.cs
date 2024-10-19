using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrTrnEnquiryView : result
    {
        public List<GetEnquiryview_list> GetEnquiryView {  get; set; }
        public List<GetEnquiryViewProduct_list> GetEnquiryViewProduct {  get; set; }
    }

    public class GetEnquiryview_list : result
    {
        public string enquiry_gid { get; set; }
        public string enquiry_date { get; set; }
        public string customer_name { get; set; }
        public string contact_email { get; set; }
        public string contact_address { get; set; }
        public string contact_person { get; set; }
        public string contact_number { get; set; }
        public string contact_mail { get; set; }
        public string closure_date { get; set; }
        public string customer_requirement { get; set; }
        public string branch_name { get; set; }
        public string enquiry_remarks { get; set; }
        public string landmark { get; set; }
        public string customer_gid { get; set; }
        public string enquiry_referencenumber { get; set; }
        public string user { get; set; }
        public string customer_rating { get; set; }
       
    }

    public class GetEnquiryViewProduct_list : result
    {
        public string enquirydtl_gid { get; set; }
        public string enquiry_gid { get; set; }
        public string product_gid { get; set; }
        public string product_name { get; set; }
        public string productgroup_name { get; set; }
        public string uom_name { get; set; }
        public string product_code { get; set; }
        public string qty_enquired { get; set; }
        public string potential_value { get; set; }
        public string uom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_requireddate { get; set; }
    }

}