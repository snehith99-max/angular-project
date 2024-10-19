using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class mdlcompany : result
    {
        public List<countrylists> countrylists { get; set; }
        public List<currencylists> currencylists { get; set; }
        public List<companylists> companylists { get; set; }

    }

    public class countrylists : result
    {
        public string country_name{ get; set; }
        public string country_gid { get; set; }
    }
    
    public class currencylists : result
    {
        public string currencyexchange_gid { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
    }
    public class companylists : result
    {
        public string file_name { get; set; } 
        public string company_gid { get; set; }
        public string company_code { get; set;}
        public string company_name { get; set; }
        public string company_address { get; set; }
        public string salary_startdate { get; set; }
        public string company_phone { get; set; }
        public string company_website { get; set; }
        public string company_mail { get;set; }
        public string contact_person { get; set;}
        public string manufacturer_licence { get; set;}
        public string auth_code { get; set; }
        public string fax { get; set;}   
        public string pop_server { get; set;}
        public string pop_port { get; set;}
        public string pop_username { get; set;}
        public string pop_password {  get; set;}
        public string currency_code { get; set;}
        public string company_logo_path { get; set;}
        public string welcome_logo { get; set; }
        public string authorised_sign { get; set;}
        public string sequence_reset { get; set;}
        public string country_name { get; set;}
        public string country_gid {  get; set;} 
    }


}