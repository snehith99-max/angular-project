using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrMstPurchasetype
    {
        public List<GetpurchaseType_List> GetpurchaseType_List { get; set; }
    }
        public class GetpurchaseType_List : result
        {
            public string purchasetype_gid { get; set; }
            public string purchasetype_code { get; set; }
            public string purchasetype_name { get; set; }
            public string account_gid { get; set; }
            public string account_name { get; set; }
        }
    
}