
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.inventory.Models
{
    public class MdlImsTrnIssueMaterial:result
    {
        public List<Getissuematerial_list> Getissuematerial_list { get; set; }
        public List<Viewissuematerial_list> Viewissuematerial_list { get; set; }
        public List<Viewissuematerialsummary_list> Viewissuematerialsummary_list { get; set; }
        public List<issuematerialselect_list> issuematerialselect_list { get; set; }
        public List<issuematerialproduct_list> issuematerialproduct_list { get; set; }
        public List<materialproduct_list> materialproduct_list { get; set; }
        public List<GetMIissuedetails_list> GetMIissuedetails_list { get; set; }
        public List<GetMIissueproduct_list> GetMIissueproduct_list { get; set; }
        public List<Postissuemetrial> Postissuemetrial { get; set; }
    }
    public class Getissuematerial_list : result
    {
        public string productuom_gid { get; set; }
        public string materialissued_date { get; set; }
        public string materialissued_gid { get; set; }
        public string costcenter_name { get; set; }
        public string department_name { get; set; }
        public string materialrequisition_gid { get; set; }

        public string materialrequisition_reference { get; set; }
        public string user_firstname { get; set; }
        public string issued_to { get; set; }
        public string branch_prefix { get; set; }

    }
    public class Postissuemetrial : result
    {
        public List<GetMIissueproduct_list> GetMIissueproduct_list { get; set; }
        public string issue_remarks { get; set; }
        public string materialrequisition_gid { get; set; }
        public string department_name { get; set; }
    }

    public class Viewissuematerial_list : result
    {
        public string materialissued_gid { get; set; }
        public string stock_gid { get; set; }
        public string product_gid { get; set; }
        public string issued_quantity { get; set; }
        public string product_name { get; set; }
        public string display_field { get; set; }

        public string product_code { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string materialissued_date { get; set; }
        public string raised_by { get; set; }
        public string requested_by { get; set; }
        public string stock_quantity { get; set; }
        public string materialissued_reference { get; set; }
        public string materialissued_remarks { get; set; }
        public string location_name { get; set; }
        public string qty_requested { get; set; }

    }


    public class Viewissuematerialsummary_list : result
    {
        public string materialissued_gid { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string materialissued_date { get; set; }
        public string raised_by { get; set; }
        public string requested_by { get; set; }
        public string materialissued_reference { get; set; }
        public string materialissued_remarks { get; set; }
        public string location_name { get; set; }

    }

    public class issuematerialselect_list : result
    {
        public string materialrequisition_gid { get; set; }
        public string materialrequisition_reference { get; set; }
        public string materialrequisition_status { get; set; }
        public string material_status { get; set; }
        public string materialrequisition_date { get; set; }
        public string user_firstname { get; set; }
        public string department_name { get; set; }
        public string created_date { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string expected_date { get; set; }
        public string costcenter_name { get; set; }
    }
    public class issuematerialproduct_list : result
    {
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string qty_issued { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string issuerequestqty { get; set; }
    }
    public class materialproduct_list : result
    {
        public string product_name { get; set; }
        public string display_field { get; set; }
        public string qty_issued { get; set; }
        public string product_code { get; set; }
        public string productuom_name { get; set; }
        public string qty_requested { get; set; }
        public string issuerequestqty { get; set; }
    }
    public class GetMIissuedetails_list : result
    {
        public string materialrequisition_gid { get; set; }
        public string materialrequisition_date { get; set; }
        public string department_name { get; set; }
        public string reference_gid { get; set; }
        public string branch_name { get; set; }
        public string user_firstname { get; set; }
        public string employee_mobileno { get; set; }
        public string department_gid { get; set; }
        public string materialrequisition_remarks { get; set; }
        public string approver_remarks { get; set; }
        public string expected_date { get; set; }

    }
    public class GetMIissueproduct_list : result
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string req_qty { get; set; }
        public string issuerequestqty { get; set; }
        public string branch_gid { get; set; }
        public string display_field { get; set; }
        public string materialrequisition_gid { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string materialrequisitiondtl_gid { get; set; }
        public string stock_quantity { get; set; }
        public string qty_issued { get; set; }
        public string productgroup_name { get; set; }



    }

}

