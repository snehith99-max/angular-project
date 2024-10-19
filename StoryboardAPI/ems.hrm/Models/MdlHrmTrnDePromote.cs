using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Mysqlx.Datatypes.Scalar.Types;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.Models
{
    public class MdlHrmTrnDePromote : result
    {
        public List<DePromotionsummary_list> DePromotionsummary_list { get; set; }
        public List<DePromotionHistorysummary_list> DePromotionHistorysummary_list { get; set; }
        //public List<GetEmployeedetaildatadropdown> GetEmployee_Dtl { get; set; }
        public List<GetEmployeenameddtldropdown> GetEmployeeuser_Dtl { get; set; }
        public List<Getbranchnameddtldropdown> GetBranchuser_Dtl { get; set; }
        public List<Getdepartmentnameddtldropdown> GetDepartmentuser_Dtl { get; set; }
        public List<Getdesignationnameddtldropdown> GetDesignationuser_Dtl { get; set; }
        public List<GetEmployeeUserDataDetail> GetEmployeeUserData_Detail { get; set; }
    }
    public class DePromotionsummary_list : result 
    {
        public string DePromotionStatus { get; set; }
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

    public class DePromotionHistorysummary_list : result 
    {
        public string employee_gid { get; set; }
        public string designation_gid { get; set; }
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string designation_name { get; set; }
        public string employee_name { get; set; }
        public string username { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string department_name { get; set; }
        public string promotedd_date { get; set; }

    }

    //public class GetEmployeedetaildatadropdown : result
    //{
    //    public string employee_gid { get; set; }
    //    public string employee_name { get; set; }

    //}

    public class GetEmployeenameddtldropdown : result  
    {
        public string employee_gid { get; set; }
        public string employee_name { get; set; }

    }

    public class Getbranchnameddtldropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }

    }

    public class Getdepartmentnameddtldropdown : result
    {
        public string department_name { get; set; }
        public string department_gid { get; set; }

    }

    public class Getdesignationnameddtldropdown : result
    {
        public string designation_name { get; set; }
        public string designation_gid { get; set; }

    }

    public class GetEmployeeUserDataDetail : result
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string employee_gid { get; set; }

    }
}