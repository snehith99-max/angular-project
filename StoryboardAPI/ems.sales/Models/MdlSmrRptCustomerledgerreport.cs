﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.sales.Models
{
   


    public class MdlSmrRptCustomerledgerreport : result
    {
        public List<customerledger_list> customerledger_list {  get; set; }
    }
   
    public class customerledger_list : result
    {
        public string customer_refno { get; set; }
        public string customer_name { get; set; }
        public string contact_details { get; set; }
        public string products { get; set; }
        public string order_type { get; set; }
        public string order_value { get; set; }
        public string customer_gid { get; set; }
    }

}