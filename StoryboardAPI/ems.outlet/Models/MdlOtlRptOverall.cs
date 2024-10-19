using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{
    public class MdlOtlRptOverall : result
    {
        public List<outlet_overall> outlet_overall { get; set; }

    }

    public class outlet_overall : result
    {

        public string revenue_amount { get; set; }
        public string expense_amount { get; set; }
        public string campaign_name { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string edit_status { get; set; }

    }


}