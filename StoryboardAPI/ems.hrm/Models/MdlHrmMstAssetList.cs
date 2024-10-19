using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmMstAssetList : result
    {
        public List<asset_list> asset_list { get; set; }
        public List<asset_list> asset_name { get; set; }
    }

    public class asset_list : result
    {
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string assigned_count { get; set; }
        public string available_count { get; set; }
        public string asset_countedit { get; set; }
        public string asset_count { get; set; }
        public string assetref_no { get; set; }
        public string active_flag { get; set; }
        public string asset_name { get; set; }
        public string asset_gid { get; set; }
        public string assetref_noedit { get; set; }
        public string asset_nameedit { get; set; }

    }
}