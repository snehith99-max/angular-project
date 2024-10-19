﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlEmployeelist : result
    {
        public List<Getentitydropdown> Getentitydropdown { get; set; }
        public List<employee_list10> employee_list { get; set; }

        public List<Getemployee_lists> Getemployee_lists { get; set; }
        public List<Getbranchdropdown> Getbranchdropdown { get; set; }
        public List<Getdepartmentdropdown> Getdepartmentdropdown { get; set; }
        public List<Getdesignationdropdown> Getdesignationdropdown { get; set; }
        public List<Getcountrydropdown> Getcountrydropdown { get; set; }
        public List<Getcountry2dropdown> Getcountry2dropdown { get; set; }
        //public List<Getreportingtodropdown> Getreportingtodropdown { get; set; }
        public List<employee_lists> employee_lists { get; set; }
        public List<Getusergrouptempdropdown> Getusergrouptempdropdown { get; set; }
        public List<GetEditEmployeeSummary> GetEditEmployeeSummary { get; set; }
       

    }
    public class employee_list10 : result
    {
        public string user_gid { get; set; }
        public string useraccess { get; set; }
        public string employee_joiningdate { get; set; }
        //public string user_code { get; set; }
        public string user_name { get; set; }
        public string employee_gender { get; set; }
        public string emp_address { get; set; }
        public string designation_name { get; set; }
        public string designation_gid { get; set; }
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
        public string user_status { get; set; }
        public string department_gid { get; set; }
        public string branch_gid { get; set; }
        public string department_name { get; set; }
        public string entity_name { get; set; }
        public string employee_level { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string remarks { get; set; }
    }
    public class Getentitydropdown : result
    {
        public string entity_gid { get; set; }
        public string entity_name { get; set; }

    }
    public class GetEditEmployeeSummary : result
    {
        public string temporary_addressgid  { get; set; }
        public string permanent_addressgid { get; set; }
        public string permanent_countrygid { get; set; }
        public string temporary_countrygid { get; set; }
        public string entity_gid { get; set; }
        public string branch_gid { get; set; }
        public string permanent_address1 { get; set; }
        public string department_gid { get; set; }
        public string designation_gid { get; set; }
        public string permanent_address2 { get; set; }
        public string permanent_city { get; set; }
        public string permanent_state { get; set; }
        public string permanent_postalcode { get; set; }
        public string permanent_country { get; set; }
        public string temporary_address1 { get; set; }
        public string temporary_address2 { get; set; }
        public string temporary_city { get; set; }
        public string temporary_postalcode { get; set; }
        public string temporary_country { get; set; }
        public string temporary_state { get; set; }
        public string designation_name { get; set; }
        public int Length { get; set; }
        public string employee_photo { get; set; }
        public string employee_gid { get; set; }
        public string employee_gender { get; set; }
        public string entity_name { get; set; }
        public string identity_no { get; set; }
        public string employee_dob { get; set; }
        public string employee_sign { get; set; }
        public string bloodgroup { get; set; }
        public string employee_image { get; set; }
        public string employee_emailid { get; set; }
        public string comp_email { get; set; }
        public string employee_mobileno { get; set; }
        public string employee_documents { get; set; }
        public string employee_experience { get; set; }
        public string employee_experiencedtl { get; set; }
        public string employeereporting_to { get; set; }
        public string employment_type { get; set; }
        public string user_code { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string user_status { get; set; }
        public string usergroup_gid { get; set; }
        public string usergroup_code { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string approveby_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string approver_name { get; set; }
        public string nationality { get; set; }
        public string nric_no { get; set; }
        public string user_password { get; set; }
        public string file_name { get; set; }
        public string usergrouptemplate_name { get; set; }
        public string usergrouptemplate_gid { get; set; }


    }
    public class employee_lists : result
    {
        public string remarks { get; set; }
        public string file { get; set; }
        public string usergrouptemplate { get; set; }
        public string deactivation_date { get; set; }
        public string temporary_addressgid { get; set; }
        public string employee_gid { get; set; }
        public string permanent_addressgid { get; set; }
        public string user_code { get; set; }
        public string branchname { get; set; }
        public string entityname { get; set; }
        public string departmentname { get; set; }
        public string designationname { get; set; }
        public string active_flag { get; set; }
        public string gender { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string comp_email { get; set; }
        public string mobile { get; set; }
        public string permanent_address1 { get; set; }
        public string permanent_address2 { get; set; }
        public string permanent_city { get; set; }
        public string permanent_state { get; set; }
        public string permanent_postal { get; set; }
        public string country { get; set; }
        public string countryname { get; set; }
        public string temporary_address1 { get; set; }
        public string temporary_address2 { get; set; }
        public string temporary_state { get; set; }
        public string temporary_city { get; set; }
        public string temporary_postal { get; set; }
        public string usergrouptemplate_gid { get; set; }

    }
    public class Getemployee_lists : result
    {
        public string user_gid { get; set; }
        public string useraccess { get; set; }
        public string entity_name { get; set; }
        //public string user_code { get; set; }
        public string user_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string employee_gender { get; set; }
        public string emp_address { get; set; }
        public string designation_name { get; set; }
        public string designation_gid { get; set; }
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
        //public string reportingto { get; set; }
        public string user_status { get; set; }
        public string department_gid { get; set; }
        public string department_name { get; set; }
        public string branch_gid { get; set; }




    }
    public class Getbranchdropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }

    }
    //Other Application  List


    public class Getdepartmentdropdown : result
    {
        public string department_name { get; set; }
        public string department_gid { get; set; }

    }
    public class Getdesignationdropdown : result
    {
        public string designation_name { get; set; }
        public string designation_gid { get; set; }

    }
    public class Getcountrydropdown : result
    {
        public string country_name { get; set; }
        public string country_gid { get; set; }

    }
    public class Getcountry2dropdown : result
    {
        public string country_names { get; set; }
        public string country_gids { get; set; }

    }
    //public class Getreportingtodropdown : result
    //{
    //    public string employee_name { get; set; }
    //    public string employee_gid { get; set; }
    //}
    public class Getusergrouptempdropdown : result
    {
        public string usergrouptemplate_gid { get; set; }
        public string usergrouptemplate_code { get; set; }
        public string usergrouptemplate_name { get; set; }
    }
}