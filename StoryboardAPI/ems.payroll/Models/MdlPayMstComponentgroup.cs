﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstComponentgroup : result
    {
        public List<Componentgroup_list> Componentgroup_list { get; set; }
    }
    

        public class Componentgroup_list : result
    {
        public string componentgroup_gid { get; set; }
        public string componentgroup_code { get; set; }
        public string componentgroup_name { get; set; }
        public string group_belongsto { get; set; }
        public string display_name { get; set; }
        public string statutory { get; set; }
        public string Code_Generation { get; set; }
        public string componentgroup_code_manual { get; set; }
       
    }
  }