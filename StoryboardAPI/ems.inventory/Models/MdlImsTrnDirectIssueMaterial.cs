using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ems.inventory.Models
{
    public class MdlImsTrnDirectIssueMaterial : result
    {
        public List<imslocation_list> imslocation_list { get; set; }
        public List<imscostenter_list> imscostenter_list { get; set; }
        public List<imsdirectissue_list> imsdirectissue_list { get; set; }
        public List<imsavailable_list> imsavailable_list { get; set; }
        public List<imsproducttype_list> imsproducttype_list { get; set; }
        public List<imsproductsummary_list> imsproductsummary_list { get; set; }
        public List<imsproductissue_list> imsproductissue_list { get; set; }
        public List<tmpproductsummary_list> tmpproductsummary_list { get; set; }

        public double available_amount { get; set; }

    }
    public class POissuemetrial : result
    {
        public List<imsproductissue_list> imsproductissue_list { get; set; }
        public string type { get; set; }
    }


        public class imslocation_list : result
    {
        public string location_gid { get; set; }
        public string location_name { get; set; }
        public string location_code { get; set; }

    }
    public class imscostenter_list : result
    {
        public string costcenter_gid { get; set; }
        public string costcenter_name { get; set; }

    }
    public class imsdirectissue_list : result
    {
        public string user_firstname { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }

    }
    public class imsavailable_list : result
    {
        public string user_firstname { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string available_amount { get; set; }

    }
    public class imsproducttype_list:result
    {
        public string producttype_name { get; set; }
        public string producttype_code { get; set; }
        public string producttype_gid { get; set; }

    }
    public class imsproductsummary_list:result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string stock_quantity { get; set; }
        public string display_field { get; set; }

    }
    public class imsproductissue_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public double qty_requested { get; set; }
        public string display_field { get; set; }
        public double stock_quantity { get; set; }

    }
    public class tmpproductsummary_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string tmpmaterialrequisition_gid { get; set; }
        public string product { get; set; }
        public string product_remarks { get; set; }
        public string display_field { get; set; }
        public string stock_quantity { get; set; }

    }
    public class issuematerial_list : result
    {
        public List<tmpproductsummary_list> tmpproductsummary_list { get; set; }
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string tmpmaterialrequisition_gid { get; set; }
        public string product { get; set; }
        public string product_remarks { get; set; }
        public string display_field { get; set; }
        public string stock_quantity { get; set; }
        public string user_firstname { get; set; }
        public string employee_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string available_amount { get; set; }
        public string location_name { get; set; }
        public string costcenter_name { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string materialrequisition_reference { get; set; }
        public string materialissued_date { get; set; }
        public string expected_date { get; set; }
        public string priority { get; set; }


    }
}


