using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlPincode : result
    {
        public List<Pincode_list> Pincode_list { get; set; }
        public List<Getpincode_list> Getpincode_list { get; set; }
    }
    public class Pincode_list : result
    {
        public string pincode { get; set; }
        public string deliverycost { get; set; }
        public string branch_gid { get; set; }
    }
    public class Getpincode_list : result
    {
        public string pincode_code { get; set; }
        public string pincode_id { get; set; }
        public string branch_gid { get; set; }
        public string deliverycost { get; set; }
        public string branch_name { get; set; }
        public string symbol { get; set; }
    }
}