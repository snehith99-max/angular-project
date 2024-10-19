using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using ems.law.Models;

namespace ems.law.DataAccess
{
    public class DaLglDashboard :ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid, msGETGID4, lsstatus;

        public void DaGetlgldashboardcountsummary(MdlLglDashboard values)
        {
            try
            {
                msSQL = " select (select count(casetype_gid)from lgl_mst_tcasetype) as casetype_count," +
                        " (select count(institute_gid) from law_mst_tinstitute) as institute_count," +
                        " (select count(institute_gid) from law_mst_tinstitute where active_flag='N') as active_count," +
                        " (select count(institute_gid) from law_mst_tinstitute where active_flag='Y') as Inactive_count," +
                        " (SELECT COUNT(case_gid) FROM lgl_trn_tcaseinformation "+
                        " WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 7 DAY))as case_week," +
                        "(select COUNT(case_gid) FROM lgl_trn_tcaseinformation "+
                        "WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH))as case_month, count(case_gid) as case_count from lgl_trn_tcaseinformation";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<lgldashboard_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new lgldashboard_list
                        {
                            casetype_count = dt["casetype_count"].ToString(),
                            institute_count = dt["institute_count"].ToString(),
                            active_count = dt["active_count"].ToString(),
                            Inactive_count = dt["Inactive_count"].ToString(),
                            case_week = dt["case_week"].ToString(),
                            case_month = dt["case_month"].ToString(),
                            case_count = dt["case_count"].ToString(),
                        });
                        values.lgldashboard_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Dashboard Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}