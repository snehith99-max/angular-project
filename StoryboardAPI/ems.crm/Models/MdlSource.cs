﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlSource : result
    {
        public List<source_lists> source_lists { get; set; }
    }

    public class source_lists
    {
        public string source_gid { get; set; }
        public string params_gid { get; set; }
        public string source_name { get; set; }
        public string source_description { get; set; }
        public string source_code { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string sourceedit_name { get; set; }
        public string sourceedit_description { get; set; }
        public string message { get; set; }
        public string Status { get; set; }
        public bool status {  get; set; }
        

    }
    public class mdmregionstatus : result
    {
        public string source_gid { get; set; }

        public string status_flag { get; set; }

    }
}