using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using ems.outlet.Models;

namespace ems.outlet.Dataaccess
   {
    public class Daotl_rpt_overall:ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;

        public void DaGetoutletreportsummary(string edit_status, MdlOtlRptOverall values)
        {
            try
            {
                msSQL = "select a.revenue_amount,a.expense_amount,a.campaign_name,date_format(a.created_date,'%d-%m-%Y') as created_date ," +
                        "concat(d.user_firstname,' ', d.user_lastname) as created_by from otl_mst_tdaytracker a " +
                        "left join adm_mst_tuser d on d.user_gid = a.created_by  where a.edit_status='"+ edit_status + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outlet_overall>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outlet_overall
                        {
                            revenue_amount = dt["revenue_amount"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            campaign_name = dt["campaign_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                        });
                        values.outlet_overall = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Overall Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}