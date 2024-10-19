using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Mysqlx.Datatypes.Scalar.Types;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.Models
{
    public class MdlHrmTrnPromotionManagement : result
    {
        public List<Promotionsummary_list> Promotionsummarylist { get; set; }
        public List<PromotionHistorysummary_list> PromotionHistorysummary_list { get; set; }
        public List<GetEmployeedtldropdown> GetEmployee_Dtl { get; set; }
        public List<Getbranchdtldropdown> GetBranch_Dtl { get; set; }
        public List<Getdepartmentdtldropdown> GetDepartment_Dtl { get; set; }
        public List<Getdesignationdtldropdown> GetDesignation_Dtl { get; set; }
        public List<GetEmployeeDataDetail> GetEmployeeData_Detail { get; set; }
    }

    public class Promotionsummary_list : result 
    {
        public string PromotionStatus { get; set; }
        public string user_status { get; set; }
        public string designation_gid { get; set; }
        public string branch_gid { get; set; }
        public string department_gid { get; set; }
        public string from_date { get; set; }
        public string created_date { get; set; }
        public string promotion_gid { get; set; }
        public string user_firstname { get; set; }
        public string promotion_effectivedate { get; set; }
        public string promotedd_date { get; set; }
        public string currentdesignation { get; set; }
        public string previousdesignation { get; set; }
        public string currentbranch { get; set; }
        public string perviousbranch { get; set; }
        public string currentdepartment { get; set; }
        public string perviousdepartment { get; set; }
        public string approveby_name { get; set; }
        public string employee_gid { get; set; }
        public string promotion_flag { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string employeegid { get; set; }
        public string effective_date { get; set; }
        public string designation_detail { get; set; }
        public string designation_name { get; set; }
        public string reason { get; set; }
        public string branch_detail { get; set; }
        public string department_detail { get; set; }


    }

    public class PromotionHistorysummary_list : result
    {
        public string employee_gid { get; set; }
        public string designation_gid { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string designation_name { get; set; }
        public string username { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string department_name { get; set; }
        public string promotedd_date { get; set; }

    }
    public class GetEmployeeDataDetail : result
    {
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
        public string employee_gid { get; set; }
    }

    public class GetEmployeedtldropdown : result 
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
    }

    public class Getbranchdtldropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }

    public class Getdepartmentdtldropdown : result
    {
        public string department_name { get; set; }
        public string department_gid { get; set; }
    }
    public class Getdesignationdtldropdown : result
    {
        public string designation_name { get; set; }
        public string designation_gid { get; set; }
    }


}