using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlAppointmentManagement
    {
        public List<GetLeaddropdown_list> GetLeaddropdown_list { get; set; }
        public List<Getbussinessverticledropdown_list> Getbussinessverticledropdown_list { get; set; }
        public List<GetAppointmentsummary_list> GetAppointmentsummary_list { get; set; }
        public List<GetAppointmentTiles_list> GetAppointmentTiles_list { get; set; }
        public List<GetTeamdropdown_list> GetTeamdropdown_list { get; set; }
    }

    public class GetLeaddropdown_list
    {
        public string leadbank_gid { get; set; }
        public string leadbank_name { get; set; }
        public string lead_details { get; set; }
        public string leadbankbranch_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string source_name { get; set; }
        public string region_name { get; set; }
    }
    public class Getbussinessverticledropdown_list
    {
        public string business_vertical { get; set; }
        public string businessvertical_gid { get; set; }
    }
    public class Postappointment_list : result
    {
        public string leadname_gid { get; set; }
        public string bussiness_verticle { get; set; }
        public string appointment_timing { get; set; }
        public string lead_title { get; set; }
        public string campaign_gid { get; set; }
        public string Employee_gid   { get; set; }

    }
    public class GetAppointmentsummary_list
    {
        public string leadbank_gid { get; set; }
        public string assign_to { get; set; }
        public string appointment_gid { get; set; }
        public string appointment_code { get; set; }
        public string internal_notes { get; set; }
        public string potential_value { get; set; }
        public string business_vertical { get; set; }
        public string leadbank_name { get; set; }
        public string Address_details { get; set; }
        public string Details { get; set; }
        public string region_source { get; set; }
        public string appointment_date { get; set; }
        public string bussiness_name { get; set; }
        public string assigned_employee { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string leadbankbranch_name { get; set; }
        public string leadbankcontact_name { get; set; }
        public string lead_title { get; set; }
        public string fullformat_date { get; set; }
    }
    public class GetAppointmentTiles_list
    {
        public string total_unassigned { get; set; }
        public string total_assigned { get; set; }
        public string total_team { get; set; }
        public string total_appointment { get; set; }
    } 
    public class GetTeamdropdown_list
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string campaign_prefix { get; set; }
        public string campaign_gid { get; set; }
    }
       public class PostAssignedEmployee_list:result
    {
        public string employee_gid { get; set; }
        public string teamname_gid { get; set; }
        public string appointment_gid { get; set; }
    }
    public class Posteditappointment_list : result
    {
        public string editleadname_gid { get; set; }
        public string editbussiness_verticle { get; set; }
        public string editappointment_timing { get; set; }
        public string editlead_title { get; set; }
        public string appointment_gid { get; set; }

    }


}