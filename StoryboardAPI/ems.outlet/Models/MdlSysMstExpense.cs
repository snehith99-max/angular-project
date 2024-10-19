using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.outlet.Models
{

    public class MdlSysMstExpense : result
    {
        public List<expensecategory_listdata> expensecategory_listdata { get; set; }
    }

    public class expensecategory_listdata : result
    {
        public string expense_gid { get; set; }
        public string expense_code { get; set; }
        public string expense_data { get; set; }
        public string expense_desc { get; set; }

    }
}