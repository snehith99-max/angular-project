using System.Collections.Generic;

namespace ems.pmr.Models
{
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class MdlPmrMstProductUnit : result
    {
        public List<productunit_list> productunit_list { get; set; }
        public List<productunitgrid_list> productunitgrid_list { get; set; }
        public List<unitclass_list> unitclass_list { get; set; }
    }

    public class productunit_list : result
    {

        public string productuomclass_gid { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }
        public string productuomclassedit_code { get; set; }
        public string productuomclassedit_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_code { get; set; }


    }

    public class productunitgrid_list : result
    {

        public string productuom_gid { get; set; }
        public string productuom_code { get; set; }
        public string productuom_name { get; set; }
        public string sequence_level { get; set; }
        public string convertion_rate { get; set; }
        public string baseuom_flag { get; set; }
        public string total_count { get; set; }
        public string productuomclass_gid { get; set; }
        public string productuomclass_name { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclassedit_name1 { get; set; }
        public string conversion_rate { get; set; }
        public string productuomclassedit_code1 { get; set; }
        public string batch_flag { get; set; }



    }
    public class productunitgrid_listedit : result
    {

        public string productuom_gid { get; set; }
        public string productuomclass_gid { get; set; }
        public string productuom_code { get; set; }
        public string productuomedit_name { get; set; }
        public string sequence_leveledit { get; set; }
        public string conversion_rateedit { get; set; }
        public string batch_flagedit { get; set; }
        public string productuomclass_nameedit { get; set; }
        public string productuomedit_code  { get; set; }


    }

    public class unitclass_list : result
    {

        public string productuomclass_gid { get; set; }
        public string productuomclass_code { get; set; }
        public string productuomclass_name { get; set; }
    }

}