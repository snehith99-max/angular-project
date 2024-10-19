using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlAdovacacyManagement:result
    {
        public List<GetCustomerdropdown_list> GetCustomerdropdown_list { get; set; }
        public List<GetAdovacacysummary_list> GetAdovacacysummary_list { get; set; }
        public List<GetAdvocacyDetails_list> GetAdvocacyDetails_list { get; set; }
        public List<GetLeaddropdownadvocacy_list> GetLeaddropdownadvocacy_list { get; set; }
    }
    public class GetLeaddropdownadvocacy_list
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

    public class GetCustomerdropdown_list 
    { 
        public string leadbank_gid { get; set;} 
        public string leadbank_name { get; set;} 
    }
    public class postadovacacy_list:result
    {
        public leadtoapi[] leadtoapi;

        public string customer_gid { get; set; }
    }
    public class leadtoapi
    {
        public string leadbank_gid { get; set; } 
    }
    public class GetAdovacacysummary_list
    {
        public string source_name { get; set; } 
        public string region_name { get; set; } 
        public string lead_details { get; set; } 
        public string leadbank_name { get; set; } 
        public string adovacacy_leadbankgid { get; set; } 
    }
    public class GetAdvocacyDetails_list
    {
        public string source_name { get; set; } 
        public string region_name { get; set; } 
        public string lead_details { get; set; } 
        public string leadbank_name { get; set; } 
        public string adovacacy_leadbankgid { get; set; } 
        public string reference_leadbankgid { get; set; } 
        public string leadbank_gid { get; set; } 
    }





}