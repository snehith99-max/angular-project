using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.system.Models
{
    public class MdlSysMstExpense : result
    {
        public List<expensecategory_list> expensecategory_listdata { get; set; }
    }

    public class expensecategory_list : result
    {
        public string expense_gid { get; set; }
        public string expense_code { get; set; }
        public string expense_data { get; set; }
        public string expense_desc { get; set; }
      
    }
}