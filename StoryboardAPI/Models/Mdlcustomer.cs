using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryboardAPI.Models
{
    public class Mdlcustomer : result
    {
        public List<Postcustomer> Postcustomer { get; set; }
        public List<customerloginResponse> customerloginResponse { get; set; }
        public List<PostCustomerForgotuser> PostCustomerForgotuser { get; set; }
        public List<GetCustomerName> GetCustomerName { get; set; }
    }
    public class Postcustomer : result
    {
        public string eportal_emailid { get; set;}
        public string eportal_password { get; set;}
        public string company_code { get; set;}
    }
    public class customerloginResponse : result
    {
        public string token { get; set;}
        public string customer_gid { get; set;}
        public string c_code { get; set;}
        public string dashboard_flag { get; set;}        
    }
    public class PostCustomerForgotuser : result
    {
        public string companyid { get; set; }   
        public string forgoteportal_emailid { get; set; }
        public string customer_gid { get; set; }
    }
    public class GetCustomerName : result
    {
        public string customer_name { get; set; }
    }
}