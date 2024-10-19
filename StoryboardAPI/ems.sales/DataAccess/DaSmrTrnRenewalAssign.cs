using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnRenewalAssign
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;

        public void DaGetRenewalReportSummary(MdlSmrTrnRenewalAssign values)
        {
            try
            {
                string msSQL = " SELECT distinct a.renewal_gid,a.customer_gid,a.renewal_type as renewal,(date_format(a.renewal_date,'%d-%m-%Y')) as renewal_date ,a.renewal_to,a.renewal_status," +
                    "  concat(c.user_firstname, '-' ,c.user_lastname) as user_name ,d.customer_name,  concat(e.customercontact_name,' / ',e.mobile,' / ',e.email) as contact_details,  " +
                    "g.salesorder_gid, (date_format(g.salesorder_date,'%d-%m-%Y')) as salesorder_date, format(g.Grandtotal,2) as Grandtotal, a.renewal_description, a.created_by," +
                    " a.created_date  from crm_trn_trenewal a  left join smr_trn_tsalesorder g on a.salesorder_gid = g.salesorder_gid   " +
                    "left join hrm_mst_temployee b on a.renewal_to=b.employee_gid  left join adm_mst_tuser c on b.user_gid=c.user_gid  " +
                    "left join crm_mst_tcustomer d on a.customer_gid =d.customer_gid " +
                    "left join crm_mst_tcustomercontact e on a.customer_gid = e.customer_gid " +
                    "where  a.renewal_type <> 'Agreement'   Union  " +
                    "SELECT distinct a.renewal_gid,a.customer_gid,a.renewal_type as renewal,(date_format(a.renewal_date,'%d-%m-%Y')) as renewal_date ,a.renewal_to,a.renewal_status,  " +
                    "concat(c.user_firstname, '-' ,c.user_lastname) as user_name ,d.customer_name,  concat(e.customercontact_name,' / ',e.mobile,' / ',e.email) as contact_details,  " +
                    "g.agreement_gid, (date_format(g.agreement_date,'%d-%m-%Y')) as salesorder_date, format(g.Grandtotal,2) as Grandtotal, a.renewal_description, a.created_by, " +
                    "a.created_date  from crm_trn_trenewal a  left join crm_trn_tagreement g on a.salesorder_gid = g.agreement_gid left join hrm_mst_temployee b on a.renewal_to=b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid  left join crm_mst_tcustomer d on a.customer_gid =d.customer_gid  " +
                    "left join crm_mst_tcustomercontact e on a.customer_gid = e.customer_gid " +
                    "where a.renewal_type = 'Agreement' order by renewal_date asc;";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalsreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalsreport_list
                        {
                            salesorder_date1 = dt["salesorder_date"].ToString(),
                            renewal_date1 = dt["renewal_date"].ToString(),
                            customer_names = dt["customer_name"].ToString(), // Adjust these fields according to your actual column names
                            contact_detail = dt["contact_details"].ToString(),
                            renewal_descriptions = dt["renewal_description"].ToString(),
                            Grandtotals = dt["Grandtotal"].ToString(),
                            renewal_status1 = dt["renewal_status"].ToString(),

                        });
                    }
                    values.renewalsreport_list = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Renewal Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetRenewalReportCount(MdlSmrTrnRenewalAssign values)
        {
                string msSQL = " SELECT COUNT(CASE WHEN renewal_status = 'Dropped' THEN 1 END) AS dropped," +
                    "COUNT(CASE WHEN renewal_status = 'Closed' THEN 1 END) AS renewed," +
                    "COUNT(CASE WHEN renewal_status = 'Open' THEN 1 END) AS upcoming FROM crm_trn_trenewal";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalscount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalscount_list
                        {
                            upcoming = dt["upcoming"].ToString(),
                            renewed = dt["renewed"].ToString(),
                            dropped = dt["dropped"].ToString(), 
                            
                        });
                    }
                    values.renewalscount_list = getModuleList;
                }

                dt_datatable.Dispose();
           
        }
    }
}