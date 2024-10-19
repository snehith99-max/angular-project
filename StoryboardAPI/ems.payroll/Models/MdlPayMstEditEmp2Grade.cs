using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstEditEmp2Grade : result
    {
        public List<Emp2Gradeedit_list> Emp2Gradeedit_list { get; set; }
        public List<Editaditional_list> Editaditional_list { get; set; }
        public List<Editdeductional_list> Editdeductional_list { get; set; }
        public List<Editotherslist> Editotherslist { get; set; }

    }

    public class Emp2Gradeedit_list : result
    {
        public string salarygradetemplate_gid { get; set; }
        public string gross_salary { get; set; }
        public string salary_mode { get; set; }
        public string net_salary { get; set; }
        public string ctc { get; set; }
    }

    public class Editaditional_list : result
    {
        public string salarycomponent_percentage_employer { get; set; }
        public string othercomponent_type { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetype { get; set; }
        public string salarycomponent_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string component_percentage_employer { get; set; }
        public string salarycomponent_amount_employer { get; set; }
        public string component_flag_employer { get; set; }
        public string salarycomponent_amount { get; set; }
        public string component_percentage { get; set; }
        public string component_flag { get; set; }
        public string affect_in { get; set; }
        public string affecting_in { get; set; }
        public string component_type { get; set; }
        public string componentgroup_gid { get; set; }
        public string component_code { get; set; }
        public string component_name { get; set; }
        public string componentgroup_name { get; set; }
    }

    public class Editotherslist : result
    {
        public string salarycomponent_percentage_employer { get; set; }
        public string othercomponent_type { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetype { get; set; }
        public string salarycomponent_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string component_percentage_employer { get; set; }
        public string salarycomponent_amount_employer { get; set; }
        public string component_flag_employer { get; set; }
        public string salarycomponent_amount { get; set; }
        public string component_percentage { get; set; }
        public string component_flag { get; set; }
        public string affect_in { get; set; }
        public string affecting_in { get; set; }
        public string component_type { get; set; }
        public string componentgroup_gid { get; set; }
        public string component_code { get; set; }
        public string component_name { get; set; }
        public string componentgroup_name { get; set; }

    }

    public class Editdeductional_list : result
    {
        public string salarycomponent_percentage_employer { get; set; }
        public string othercomponent_type { get; set; }
        public string salarycomponent_name { get; set; }
        public string salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetype { get; set; }
        public string salarycomponent_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string component_percentage_employer { get; set; }
        public string salarycomponent_amount_employer { get; set; }
        public string component_flag_employer { get; set; }
        public string salarycomponent_amount { get; set; }
        public string component_percentage { get; set; }
        public string component_flag { get; set; }
        public string affect_in { get; set; }
        public string affecting_in { get; set; }
        public string component_type { get; set; }
        public string componentgroup_gid { get; set; }
        public string component_code { get; set; }
        public string component_name { get; set; }
        public string componentgroup_name { get; set; }

    }

    public class employee2editgradelist : result
    {
        public List<Addsummarybind_list> Addsummarybind_list { get; set; }
        public List<DedSummarybind_list> DedSummarybind_list { get; set; }
        public List<OthersSummarybind_list> OthersSummarybind_list { get; set; }

        public string template_name { get; set; }
        public string gross_salary { get; set; }
        public string BasicSalary { get; set; }
        public string net_salary { get; set; }
        public string ctc { get; set; }
        public string salary_mode { get; set; }
        public string grosssalary { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string employee_gid { get; set; }
        public string employee2salarygrade_gid { get; set; }
    }
}