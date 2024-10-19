using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstGradeConfirm : result
    {
        public List<Addsummarybind_list> Addsummarybind_list { get; set; }
        public List<DedSummarybind_list> DedSummarybind_list { get; set; }
        public List<OthersSummarybind_list> OthersSummarybind_list { get; set; }
        public List<gradetemplatedropdown> gradetemplatedropdown { get; set; }
        public List<componentlist> componentlist { get; set; }
        public List<Updateempgradelist> Updateempgradelist { get; set; }
        public List<onchangecomponentlist> onchangecomponentlist { get; set; }
        public double netsalary { get; set; }
        public double overallnetsalary { get; set; }
        public double ctc { get; set; }
        public double overallctc { get; set; }
        public List<Addeditsummarybind_list> Addeditsummarybind_list { get; set; }
        public List<DededitSummarybind_list> DededitSummarybind_list { get; set; }
        public List<OtherseditSummarybind_list> OtherseditSummarybind_list { get; set; }
        public List<componenttypelist> componenttypelist { get; set; }
        public List<componentgrouplist> componentgrouplist { get; set; }
        public List<componentnamelist> componentnamelist { get; set; }
        public List<componentamountlist> componentamountlist { get; set; }
    }

    public class Addsummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double basicsalay { get; set; }
        public double salarygradetype { get; set; }
        public string salarygradetmpdtl_gid { get; set; }

    }
    

    public class DedSummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public double net_salary { get; set; }
        public double ctc { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarygradetype { get; set; }
        public string salarygradetmpdtl_gid { get; set; }


    }

    public class OthersSummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double basicsalay { get; set; }
        public double salarygradetype { get; set; }
        public string salarygradetmpdtl_gid { get; set; }
        public string othercomponent_type { get; set; }

    }

    public class gradetemplatedropdown : result
    {
        public string salarygradetemplate_name { get; set; }
        public string salarygradetemplate_gid { get; set; }

    }

    public class componentlist : result
    {
        public string salarycomponent_gid { get; set; }
        public string component_name { get; set; }

    }

    public class Updateempgradelist : result
    {
        public string editcomponentgroup_name { get; set; }
        public string editcomponent_name { get; set; }
        public double editcomponent_amount { get; set; }
        public double editcomponent_amount_employer { get; set; }
        public string salarygradetmpdtl_gid { get; set; }

    }
    public class componentupdatedlist : result
    {
        public string salarycomponent_gid { get; set; }
        public string grosssalary { get; set; }
        public double salarycomponent_amount { get; set; }

    }

    public class onchangecomponentlist : result
    {
        public string salarycomponent_gid { get; set; }
        public string salarycomponent_name { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double salarycomponent_amount_employer { get; set; }
        public double salarycomponent_amount { get; set; }

    }

    public class Addeditsummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double basicsalay { get; set; }
        public double salarygradetype { get; set; }
        public string salarygradetmpdtl_gid { get; set; }

    }

    public class DededitSummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public double net_salary { get; set; }
        public double ctc { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public string salarygradetype { get; set; }
        public string salarygradetmpdtl_gid { get; set; }

    }

    public class OtherseditSummarybind_list : result
    {
        public string salarycomponent_name { get; set; }
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
        public Double salarycomponent_amount { get; set; }
        public Double salarycomponent_percentage_employer { get; set; }
        public Double salarycomponent_amount_employer { get; set; }
        public Double salarycomponent_percentage { get; set; }
        public string salarygradetemplatedtl_gid { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string componentgroup_gid { get; set; }
        public double basicsalay { get; set; }
        public double salarygradetype { get; set; }
        public string salarygradetmpdtl_gid { get; set; }

    }

    public class componenttypelist : result
    {
        public string component_type { get; set; }
    }
    public class componentgrouplist : result
    {
        public string componentgroup_name { get; set;}
        public string componentgroup_gid { get; set;}
    }

    public class componentnamelist: result
    {
        public string component_name { get; set; }
        public string salarycomponent_gid { get; set; }
    }

    public class componentamountlist : result
    {
        public string component_percentage { get; set; }
        public string component_percentage_employer { get; set; }
        public string component_amount { get; set; }
        public string component_amount_employer { get; set; }

    }


    public class employee2gradelist : result
    {
        public List<Addsummarybind_list> Addsummarybind_list { get; set; }
        public List<DedSummarybind_list> DedSummarybind_list { get; set; }
        public List<OthersSummarybind_list> OthersSummarybind_list { get; set; }
        public List<employee_lists> employee_lists { get; set; }

        public string template_name { get; set; }
        public string gross_salary { get; set; }
        public string BasicSalary { get; set; }
        public string net_salary { get; set; }
        public string ctc { get; set; }
        public string salary_mode { get; set; }
        public string grosssalary { get; set; }
        public string salarygradetemplate_gid { get; set; }
        public string employee_gid { get; set; }




    }




}