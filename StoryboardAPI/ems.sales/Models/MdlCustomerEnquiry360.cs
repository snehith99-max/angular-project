using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlCustomerEnquiry360 : result
    {
        public List<GetProductDropDownECRM> GetProductECRM { get; set; }
        public List<GetCustomerDropDownECRM> GetCustomerECRM { get; set; }
        public List<GetBranchDropDownECRM> GetBranchECRM { get; set; }
        public List<GetSalesPersonDropDownECRM> GetSalesPersonECRM { get; set; }
        public List<GetOnchangeCustomerECRM> GetOnchangeCustomer_ECRM { get; set; }
        public List<GetOnChangeProductECRM> GetOnChangeProduct_ECRM { get; set; }
        public List<ECRMProductSummary_list> ECRMProductSummarylist { get; set; }
        public List<PostECRM_list> PostECRMlist { get; set; }
    }

    public class GetProductDropDownECRM : result
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }
    }

    public class GetCustomerDropDownECRM : result
    {
        public string customer_gid { get; set; }
        public string customer_name { get; set; }
    }

    public class GetBranchDropDownECRM : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }
    public class GetSalesPersonDropDownECRM : result
    {
        public string employee_gid { get; set; }
        public string campaign_gid { get; set; }
        public string user_firstname { get; set; }
        public string campaign_title { get; set; }
    }

    public class GetOnchangeCustomerECRM : result
    {
        public string customer_gid { get; set; }
        public string customercontact_name { get; set; }
        public string customerbranch_name { get; set; }
        public string country_name { get; set; }
        public string contact_email { get; set; }
        public string contact_number { get; set; }
        public string zip_code { get; set; }
        public string country_gid { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address2 { get; set; }
        public string contact_address { get; set; }
    }

    public class GetOnChangeProductECRM : result
    {
        public string product_gid { get; set;}
        public string product_name { get; set;}
        public string product_code { get; set;}
        public string productuom_name { get; set;}
        public string productgroup_name { get; set;}
        public string productuom_gid { get; set;}
        public string productgroup_gid { get; set;}
    }

    public class ECRMProductSummary_list : result
    {
        public string potential_value { get; set;}
        public string qty_requested { get; set;}
        public string product_requireddate { get; set;}
        public string product_name { get; set;}
        public string productuom_name { get; set;}
        public string productgroup_name { get; set;}
        public string tmpsalesenquiry_gid { get; set;}
        public string product_code { get; set;}
        public string product_gid { get; set;}
        public string productgroup_gid { get; set;}
    }

    public class PostECRM_list : result
    {
        public string customer_gid { get; set;}
        public string leadbank_gid { get; set;}
        public string enquiry_date { get; set;}
        public string closure_date { get; set;}
        public string branch_name { get; set;}
        public string customer_name { get; set;}
        public string contact_number { get; set;}
        public string customercontact_name { get; set;}
        public string contact_email { get; set;}
        public string customerbranch_name { get; set;}
        public string contact_address { get; set;}
        public string enquiry_remarks { get; set;}
        public string enquiry_referencenumber { get; set;}
        public string customer_requirement { get; set;}
        public string landmark { get; set;}
        public string customer_rating { get; set;}
        public string user_firstname { get; set;}
    }
}