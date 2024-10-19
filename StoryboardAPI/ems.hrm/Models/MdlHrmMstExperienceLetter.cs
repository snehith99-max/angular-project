using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Mysqlx.Datatypes.Scalar.Types;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.Models
{
    public class MdlHrmMstExperienceLetter : result
    {
        public List<Experiencesummary_list> Experiencesummary_list { get; set; }
        public List<GetUserdropdown> GetUserDtl { get; set; }
       
    }

    public class Experiencesummary_list : result
    {
        public string experience_gid { get; set; }
        //public string user_code { get; set; }
        public string first_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string empname { get; set; }

    }

    public class GetUserdropdown : result
    {
        public string employee_gid { get; set; }
        public string employee_joiningdate { get; set; }
        public string exit_date { get; set; }
        public string employee_name { get; set; }
    }

    public class AddExperienceletter_list : result
    {
        public string template_name { get; set; }
        public string employee_gid { get; set; }
        public string experience_gid { get; set; }
        public string template_gid { get; set; }
        public string Experiencelettertemplate_content { get; set; }
    }

    }