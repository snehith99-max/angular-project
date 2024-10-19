using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayTrnRptPFandESIFormat : result
    {
        public List<PfListFormat>PFList_format { get; set; }
        public List<Pfassign_type> Pfassign_type { get; set; }
        public List<PfList> pf_listdata { get; set; }
        public List<esi_list> esi_list { get; set; }
    }

    public class PfListFormat: result
    {
        public string month { get; set; }
        public string sal_year { get; set; }
        public string no_of_employee { get; set; }
        public string earnedbasic_salary { get; set; }
        public string pf_amount { get; set; }
    }

    public class Pfassign_type : result
    {
        public string month { get; set; }
        public string sal_year { get; set; }
    }

    public class PfList : result
    {
        public string employee_gid { get; set; }
        public string uan_no { get; set; }
        public string employee_name { get; set; }
        public string Gross { get; set; }
        public string EPF { get; set; }
        public string EPS { get; set; }
        public string EDLI { get; set; }
        public string EE { get; set; }
        public string EPS1 { get; set; }
        public string ER { get; set; }
        public string Refunds { get; set; }
        public string Pension_Share { get; set; }
        public string lop_days { get; set; }
        public string ER_PF_Share { get; set; }
        public string EE_Share { get; set; }
        public string Posting_location_of_the_member { get; set; }
    }

    public class esi_list : result
    {
        public string esi_no { get; set; }
        public string employee_name { get; set; }
        public string actual_month_workingdays { get; set; }
        public string employee_esi { get; set; }
        public string employer_esi { get; set; }
        public string earned_gross_salary { get; set; }

    }


}