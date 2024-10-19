using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.law.Models
{
    public class MdlLawMstInstitute :result
    {
        public List<institute_list> institute_List { get; set; }
        public List<countryList> countryList { get; set; }
        public List<Institutioninactivelog_data> Institutioninactivelog_data { get; set; }
    }
    public class institute_list : result
    {   

        public string institute_gid { get; set; }
        public string institute_code { get; set; }
        public string institute_location { get; set; }
        public string institutemail_id { get; set; }
        public string mobile { get; set; }
        public string contact_person { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string password { get; set; }
        public string user_code { get; set; }
        public string institute_name { get; set; }
        public string Institute_status { get; set; }
        public string ins_address1 { get; set; }
        public string ins_address2 { get; set; }
        public string ins_pincode { get; set; }
        public string ins_city { get; set; }
        public string ins_state { get; set; }
        public string ins_country { get; set; }
        public string country_name { get; set; }
        public string institute_prefix { get; set; }


    }
    public class countryList : result
    {
        public string country_name { get; set; }
        public string country_gid { get; set; }

    }
    public class Institutioninactivelog_data : result
    {
        public string log_gid { get; set; }
        public string Status { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string remarks { get; set; }
        public string institute_gid { get; set; }
        public string institute_name { get; set; }

    }

}

