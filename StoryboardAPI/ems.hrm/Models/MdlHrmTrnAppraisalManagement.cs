using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmTrnAppraisalManagement : result
    {
        public List<GetEmployeeDetail> GetEmployeeDetail { get; set; }
        public List<username_list> usernamelist { get; set; }
        //public List<username360_list> username360list { get; set; }
        public List<GetPeerDetail> GetPeer_Detail { get; set; }
        public List<GetManagerDetail> GetManager_Detail { get; set; }
        public List<GetManagementDetail> GetManagement_Detail { get; set; }
        public List<review_list> reviewlist { get; set; }
        public List<appraisaldtl_list> appraisaldtllist { get; set; }
        public List<ViewReviewSummary_list> ViewReviewSummarylist { get; set; }
    }
    public class GetEmployeeDetail : result
    {
        public string user_gid { get; set; }
        public string user_code { get; set; }
    }

    public class ViewReviewSummary_list : result
    {
        public string Reviewed_by { get; set; }
    }

    public class username_list : result
    {
        public string employee_emailid { get; set; }
        public string user_name { get; set; }
        public string designation_name { get; set; }
        public string employee_gid { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string dob { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string employee_mobileno { get; set; }


    }


    public class GetPeerDetail : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }
    }

    public class GetManagerDetail : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }
    }
    public class GetManagementDetail : result
    {
        public string user_gid { get; set; }
        public string user_name { get; set; }
    }
    public class review_list : result
    {
        public string created_by { get; set; }
        public string appraisal_gid { get; set; }
        public string emp_designation { get; set; }
        public string emp_department { get; set; }
        public string emp_branch { get; set; }
        public string created_date { get; set; }
        public string user_gid { get; set; }

        public string employee_gid { get; set; }
        public string emp_firstname { get; set; }
        public string designation_name { get; set; }
        public string department_name { get; set; }
        public string branch_name { get; set; }
        public string emp_mobile { get; set; }
        public string emp_dob { get; set; }
        public string self_name { get; set; }
        public string peer_name { get; set; }
        public string manager_name { get; set; }
        public string management_name { get; set; }

    }

    public class appraisaldtl_list : result
    {
        public string appraisal2employee_gid { get; set; }
        public string work_experience { get; set; }
        public string soft_skills { get; set; }
        public string created_date { get; set; }
        public string revised_for { get; set; }
        public string created_by { get; set; }
        public string employee_gid { get; set; }
        public string experience { get; set; }
        public string softskills { get; set; }
        public string contribution { get; set; }
        public string Reviewed_by { get; set; }
        public string grade_no { get; set; }
        public string grade { get; set; }
        public string recommended_type { get; set; }
       
    }

}