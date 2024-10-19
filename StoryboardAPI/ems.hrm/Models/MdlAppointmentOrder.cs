﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlAppoinmentOrder : result
    {
        public List<appointmentorder_list> appointmentorder_list { get; set; }
        public List<Getbranchdropdown> Getbranchdropdown { get; set; }
        public List<Getdepartmentdropdown> Getdepartmentdropdown { get; set; }
        public List<Getdesignationdropdown> Getdesignationdropdown { get; set; }
        public List<GetEmployeeList> GetEmployeeList { get; set; }
        public List<getcountrydropdown> getcountrydropdown { get; set; }
        //public List<editappoinmentorder> editappoinmentorder { get; set; }
        public List<update_list> update_list { get; set; }
        public List<GetAppointmentdropdown> GetAppointmentdropdown { get; set; }



    }
    public class GetEmployeeList : result
    {
        public string employee_name { get; set; }
        public string employee_joiningdate { get; set; }
        public string user_lastname { get; set; }
        public string exit_date { get; set; }
        public string designation_name { get; set; }
        public string user_code { get; set; }
    }

    public class appointmentorder_list : result
    {
        public string user_gid { get; set; }
        public string appointmentorder_gid { get; set; }
        public string appointment_date { get; set; }
        public string mobile_number { get; set; }
        public string user_name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string designation_name { get; set; }
        public string designation_gid { get; set; }
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
        public string email_address { get; set; }
        public string qualification { get; set; }
        public string branch_gid { get; set; }
    }
    public class Getbranchdropdown : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
    }

    public class Getdepartmentdropdown : result
    {
        public string department_gid { get; set; }
        public string department_name { get; set; }
    }
    public class GetAppointmentdropdown : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }

    }

    public class Getdesignationdropdown : result
    {
        public string designation_gid { get; set; }
        public string designation_name { get; set; }
    }

    public class getcountrydropdown : result
    {
        public string country_gid { get; set; }
        public string country { get; set; }
    }

    public class editappoinmentorderlist : result
    {
        public string designation_gid { get; set;}
        public string appointmentletterdate { get; set;}
        public string designation_name { get; set;}
        public string appointmentorder_gid { get; set;}
        public string branch_gid { get; set; }
        public string employee_salary { get; set; }
        public string branch_name { get; set; }
        public string joiningdate { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
        public string qualification { get; set; }
        public string experience_detail { get; set; }
        public string document_path { get; set; }
        public string perm_address_gid { get; set; }
        public string appointmentordertemplate_content { get; set; }
        public string appointment_date { get; set; }
        public string temp_address_gid { get; set; }
        public string template { get; set; }
        public string template_gid { get; set; }
        public string appointment_type { get; set; }
        public string created_by { get; set; }
        public string department_gid { get; set; }
        public string department_name { get; set; }
        public string created_date { get; set; }
        public string employee_gid { get; set; }
        public string perm_address1 { get; set; }
        public string perm_address2 { get; set; }
        public string perm_country { get; set; }
        public string perm_state { get; set; }
        public string perm_city{ get; set; }
        public string perm_pincode { get; set; }
        public string temp_address1 { get; set; }
        public string temp_address2 { get; set; }
        public string temp_country { get; set; }
        public string temp_state { get; set; }
        public string temp_city { get; set; }
        public string temp_pincode { get; set; }
        public string template_name { get; set; }

    }
    public class update_list : result
    {
        public string designation_gid { get; set; }
        public string designation_name { get; set; }
        public string appointmentorder_gid { get; set; }
        public string branch_gid { get; set; }
        public string employee_salary { get; set; }
        public string branch_name { get; set; }
        public DateTime joiningdate { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }
        public string mobile_number { get; set; }
        public string email_address { get; set; }
        public string qualification { get; set; }
        public string experience_detail { get; set; }
        public string document_path { get; set; }
        public string perm_address_gid { get; set; }
        public string appointmentordertemplate_content { get; set; }
        public DateTime appointment_date { get; set; }
        public string temp_address_gid { get; set; }
        public string template_name { get; set; }
        public string template_gid { get; set; }
        public string appointment_type { get; set; }
        public string created_by { get; set; }
        public string department_gid { get; set; }
        public string department_name { get; set; }
        public string created_date { get; set; }
        public string employee_gid { get; set; }
        public string perm_address1 { get; set; }
        public string perm_address2 { get; set; }
        public string perm_country { get; set; }
        public string perm_state { get; set; }
        public string perm_city { get; set; }
        public string perm_pincode { get; set; }
        public string temp_address1 { get; set; }
        public string temp_address2 { get; set; }
        public string temp_country { get; set; }
        public string temp_state { get; set; }
        public string temp_city { get; set; }
        public string temp_pincode { get; set; }

    }



}