using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.payroll.Models
{
    public class MdlPayMstSalaryComponent : result
    {
        public List<salarycompoent_list> salarycompoent_list { get; set; }
        public List<getViewSalaryComponentSummary> getViewSalaryComponentSummary { get; set; }

        public List<salarycomponentedit_list> getEditComponent { get; set; }

        public List<GetComponentGroupdropdown> GetComponentGroupDtl { get; set; }
        public List<GetComponentnamedropdown> GetComponentnamedropdown { get; set; }
        public List<Getadditioncomponentvariable> Getadditioncomponentvariable { get; set; }
    }
    public class GetComponentnamedropdown : result
    {
        public string component_gid { get; set; }
        public string salarycomponent_name { get; set; }


    }
  
    public class Getadditioncomponentvariable : result
    {
        public string  component_gid { get; set; }
        public string salarycomponent_name { get; set; }


    }

    public class salarycompoent_list : result
    {
    
        public string salarycomponent_gid { get; set; }
        public string operatoraffect_in { get; set; }
        public string formulaaffect_in { get; set; }
        public List<string> customizecomponent { get; set; }
        public List<string> additionvariblecomponent { get; set; }
        public string componentgroup_gid { get; set; }
        public string component_code { get; set; }
        public string component_name { get; set; }
        public string componentgroup_name { get; set; }
        public string employee_percent { get; set; }
        public string employer_percentage { get; set; }
        public string employee_amount { get; set; }
        public double percent { get; set; }
        public string employer_amount { get; set; }
        public string component_type { get; set; }
        public string affecting_in { get; set; }
        public string affect_in { get; set; }
        public string component { get; set; }
        public string other_componenttype { get; set; }
        public string statutory_pay { get; set; }
        public string lop_deduction { get; set; }
        public string is_percent { get; set; }
        public string is_percentage { get; set; }
        public string employer_component { get; set; }
        public string contribution_type { get; set; }
        public string display_name { get; set; }
       
    }

    public class getViewSalaryComponentSummary : result
    {
        public string source_variale { get; set; }
        public string formula_operator { get; set; }
        public string formula_variable { get; set; }
        public string customizecomponent { get; set; }
        public string salarycomponent_gid { get; set; }
        public string componentgroup_gid { get; set; }
        public string component_code { get; set; }
        public string component_name { get; set; }
        public string componentgroup_name { get; set; }
        public string employee_percent { get; set; }
        public string employer_percentage { get; set; }
        public string employee_amount { get; set; }
        public double percent { get; set; }
        public string employer_amount { get; set; }
        public string component_type { get; set; }
        public string affecting_in { get; set; }
        public string affect_in { get; set; }
        public string component { get; set; }
        public string other_componenttype { get; set; }
        public string statutory_pay { get; set; }
        public string lop_deduction { get; set; }
        public string is_percent { get; set; }
        public string is_percentage { get; set; }
        public string employer_component { get; set; }
        public string contribution_type { get; set; }
        public string display_name { get; set; }

    }
    public class GetComponentGroupdropdown : result
    { 
        public string componentgroup_gid { get; set; }
        public string componentgroup_name { get; set; }

    }

    public class salarycomponentedit_list : result
    {
        public string statutory_flag { get; set; }
        public string source_variale { get; set; }
        public string formula_operator { get; set; }
        public string formula_variable { get; set; }
        public List<string> customizecomponent1 { get; set; }
        public List<string> Formulacomponent { get; set; }
        public string customizecomponent { get; set; }
        public string salarycomponent_gid { get; set; }
        public string componentgroup_gid { get; set; }
        public string componentgroup_name { get; set; }
        public string component_code { get; set; }
        public string lop_deduction { get; set; }
        public string statutory_pay { get; set; }
        public string component_name { get; set; }
        public string is_percent { get; set; }
        public string is_percentage { get; set; }
        public string employee_percent { get; set; }
        public string employee_amount { get; set; }
        public string component_type { get; set; }
        public string affect_in { get; set; }
        public string employer_percentage { get; set; }
        public string employer_amount { get; set; }
        public string contribution_type { get; set; }
        public string other_allowance { get; set; }
    


    }

}