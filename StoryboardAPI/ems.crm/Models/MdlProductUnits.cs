using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.crm.Models
{
    public class MdlProductUnits : result
    {
        public List<Mktproductunits_list> Mktproductunits_list { get; set; }

        public List<Mktproductunitgrid_list> Mktproductunitgrid_list { get; set; }

        public List<Mktproductunits_list1> Mktproductunits_list1 { get; set; }
    }
        public class Mktproductunits_list : result
        {

            public string productuomclass_gid { get; set; }
            public string productuomclass_code { get; set; }
            public string productuomclass_name { get; set; }
            public string productuomclassedit_code { get; set; }
            public string productuomclassedit_name { get; set; }
            public string created_by { get; set; }
            public string created_date { get; set; }


        }

        public class Mktproductunitgrid_list : result
        {

            public string productuom_gid { get; set; }
            public string productuom_code { get; set; }
            public string productuom_name { get; set; }
            public string sequence_level { get; set; }
            public string productuomclassadd_name { get; set; }
            public string productuomclassadd_code { get; set; }
            public string conversion_rate { get; set; }
            public string convertion_rate { get; set; }
            public string baseuom_flag { get; set; }
            public string total_count { get; set; }
            public string productuomclass_gid { get; set; }
            public string productuomclass_name { get; set; }
            public string productuomclass_code { get; set; }
            public string created_date { get; set; }
            public string batch_flag { get; set; }


        }

        public class Mktproductunits_list1 : result
        {
            public string total { get; set; }
            public string drop_status { get; set; }
            public string prospect { get; set; }
            public string campaign_gid { get; set; }
            public string completed { get; set; }
            public string potential { get; set; }
            public string user { get; set; }
            public string employee_gid { get; set; }
            public string team_name { get; set; }
            public string description { get; set; }
            public string branch_name { get; set; }
            public string branch { get; set; }
            public string team_manager { get; set; }
            public string employee_name { get; set; }
            public string mail_id { get; set; }



        
        }
    public class Mktproductunitgrid_listedit : result
    {

        public string productuom_gid { get; set; }
        public string productuom_code { get; set; }
        public string productuomedit_name { get; set; }
        public string sequence_leveledit { get; set; }
        public string conversion_rateedit { get; set; }
        public string batch_flagedit { get; set; }


    }
}