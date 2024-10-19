using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlHrmRptEmployeeFormA : result
    {
        public List<employee_listform> employee_listform { get; set; }
    }

    public class employee_listform : result
    {
        public string category_address { get; set; }
        public string employment_type { get; set; }
        public string LWF { get; set; }
        public string Aadhar { get; set; }
        public string servicebookno { get; set; }
        public string reason_exit { get; set; }
        public string mark_identification { get; set; }
        public string employee_sign { get; set; }
        public string employee_photo { get; set; }
        public string bank { get; set; }
        public string bank_code { get; set; }
        public string esi_no { get; set; }
        public string uan_no { get; set; }
        public string pan_no { get; set; }
        public string ac_no { get; set; } 
        public string employee_mobileno { get; set; }
        public string employee_qualification { get; set; }
        public string nationality { get; set; }
        public string employee_dob { get; set; }
        public string father_name { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string user_gid { get; set; }
        public string useraccess { get; set; }
        public string employee_joiningdate { get; set; }
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string employee_gender { get; set; }
        public string emp_address { get; set; }
        public string designation_name { get; set; }
        public string designation_gid { get; set; }
        public string employee_gid { get; set; }
        public string branch_name { get; set; }
        public string user_status { get; set; }
        public string department_gid { get; set; }
        public string branch_gid { get; set; }
        public string department_name { get; set; }
        public string entity_name { get; set; }
        public string employee_level { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string remarks { get; set; }
        public string exit_date { get; set; }
    }
}