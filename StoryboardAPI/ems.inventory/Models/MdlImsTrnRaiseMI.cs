using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnRaiseMI:result
    {
        public List<imsraiseMi_list> imsraiseMilist { get; set; }
        public List<imsmiproducttypelist> imsmiproducttype_list { get; set; }
        public List<imsMIproductsummary_list> imsMIproductsummarylist { get; set; }
        public List<imsproductsingle_list> imsproductsinglelist { get; set; }
        public List<tmpproduct_list> tmpproductlist { get; set; }
        public List<tmpmiproductsummary_list> tmpMIproductsummary_list { get; set; }


        public class imsraiseMi_list : result
        {
            public string user_firstname { get; set; }
            public string branch_name { get; set; }
            public string department_name { get; set; }

        }
        public class imsmiproducttypelist : result
        {
            public string producttype_name { get; set; }
            public string producttype_code { get; set; }
            public string producttype_gid { get; set; }

        }


        public class imsMIproductsummary_list : result
        {
            public string product_gid { get; set; }
            public string product_code { get; set; }
            public string product_name { get; set; }
            public string productgroup_gid { get; set; }
            public string productgroup_name { get; set; }
            public string productuom_gid { get; set; }
            public string productuom_name { get; set; }
            public double qty_requested { get; set; }
            public double discount_amount { get; set; }
            public double discount_percentage { get; set; }
            public double total_amount { get; set; }
            public string display_field { get; set; }
            public double quantity { get; set; }
            public string unitprice { get; set; }
            public string producttype_gid { get; set; }

        }
        public class imsproductsingle_list : result
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


        public class tmpproduct_list : result
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



        public class materialIndent_list : result
        {
            public List<tmpmiproductsummary_list> tmpmiproductsummary_list { get; set; }
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
            public string branch_name { get; set; }
            public string department_name { get; set; }
            public string available_amount { get; set; }
            public string location_name { get; set; }
            public string costcenter_name { get; set; }
            public string materialrequisition_remarks { get; set; }
            public string materialrequisition_reference { get; set; }
            public string materialissued_date { get; set; }

        }


        public class tmpmiproductsummary_list : result
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


        public class MIproductbulk : result
        {
            public List<imsproductMI_list> imsproductMI_list { get; set; }
            public string type { get; set; }
        }
        public class imsproductMI_list : result
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

        }
    }


}