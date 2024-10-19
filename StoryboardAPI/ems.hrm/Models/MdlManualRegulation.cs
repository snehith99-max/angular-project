
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.hrm.Models
{
    public class MdlManualRegulation : result
    {
        public List<manuallist> manuallist { get; set; }
        public List<daylist> dayslist { get; set; }
        public List<daydatalist> daydatalist { get; set; }
        public List<updateday_list> updateday_list { get; set; }

    }
    public class mdlshiftdetails
    {
        public string employee_gid { get; set; }
        public string scheduled_intime { get; set; }
        public string scheduled_outtime { get; set; }
        public string shifttypedtl_name { get; set; }
        public string shifttype_gid { get; set; }
       
     }

    
    public class manuallist : result
    {
        public string employee_gid { get; set; }
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string branch_gid { get; set; }
        public string status { get; set; }
        public string branch_name { get; set; }
        public List<daylist> dayslist { get; set; }
        public List<daydatalist> daydatalist { get; set; }

    }

    public class daylist : result
    {
        public string days { get; set; }

        public List<manuallist> manuallist { get; set; }
    }
    public class updateday_list : result
    {
        public string attendance_gid { get; set; }
        public string shift_date { get; set; }
        public List<manuallist> manuallist { get; set; }
        public List<daydatalist> daydatalist { get; set; }

        public string fromdate { get; set; }
        public DateTime todate { get; set; }
        public string employee_gid { get; set; }





    }
    public class daydatalist : result
    {
        public string dayi { get; set; }
        public string i { get; set; }
        public string attendance { get; set; }
        

    }

    }