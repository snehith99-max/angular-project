using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstIncomeTax : result
    {
        public List<GetIncomeMaster_list> GetIncomeMaster_list { get; set; }
        public List<GetIncomeMasterNew_list> GetIncomeMasterNew_list { get; set; }
    }

    public class GetIncomeMaster_list : result
    {
        //public string tax_name_new { get; set; }
        public string tax_regime_gid { get; set; }
        public string remarks_old { get; set; }
        //public string remarks_new { get; set; }
        //public string created_by { get; set; }
        //public string created_date { get; set; }
        //public string updated_by { get; set; }
        //public string updated_date { get; set; }
        //public string tax_regime_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_slab { get; set; }
        //public string tax_slabs_toold { get; set; }
        //public string tax_slabs_fromnew { get; set; }
        //public string tax_slabs_tonew { get; set; }
        public string individuals { get; set; }
        public string resident_senior_citizens { get; set; }
        public string resident_super_senior_citizens { get; set; }
        public string tax_slabs_fromold { get; set; }
        public string tax_slabs_toold { get; set; }
        //public string income_tax_rates { get; set; }


    }

    public class GetIncomeMasterNew_list : result
    {
        public string tax_regime_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_slabnew { get; set; }
        public string income_tax_rates { get; set; }
        public string remarks_new { get; set; }
        public string tax_slabs_fromnew { get; set; }
        public string tax_slabs_tonew { get; set; }
        public string income_tax_rates1 { get; set; }
    }
    public class incometax_list : result
    {
        public string tax_nameedit { get; set; }
        public string tax_slabs_fromoldedit { get; set; }
        public string tax_slabs_tooldedit { get; set; }
        public string tax_slabs_fromnewedit { get; set; }
        public string tax_slabs_tonewedit { get; set; }
        public string individuals_edit { get; set; }
        public string resident_senior_citizensedit { get; set; }
        public string resident_super_senior_citizensedit { get; set; }
        public string income_tax_ratesedit { get; set; }
        public string remarksold_edit { get; set; }
        public string remarksnew_edit { get; set; }
        public string tax_name { get; set; }
        public string remarks_old { get; set; }
        public string remarks_new { get; set; }
        public string tax_regime_gid { get; set; }
        public string tax_slabs_fromold { get; set; }
        public string tax_slabs_toold { get; set; }
        public string tax_slabs_fromnew { get; set; }
        public string tax_slabs_tonew { get; set; }
        public string individuals { get; set; }
        public string resident_senior_citizens { get; set; }
        public string resident_super_senior_citizens { get; set; }
        public string income_tax_rates { get; set; }
    }
}
  
