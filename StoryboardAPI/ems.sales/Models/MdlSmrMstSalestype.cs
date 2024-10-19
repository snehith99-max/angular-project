using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
    public class MdlSmrMstSalestype:result
    {
        public List<GetSalesType_List> GetSalesType_List { get; set; }
    }
    public class GetSalesType_List:result
    {
        public string salestype_gid { get; set; }
        public string salestype_code { get; set; }
        public string salestype_name { get; set; }
        public string account_gid { get; set; }
        public string account_name { get; set; }
    }
}