using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using ems.subscription.Models;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;



namespace ems.subscription.DataAccess
{
    public class DaDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult, mnResult1;
        
        public void DaGetportalchart(MdlDashboard values)
        {
            try
            {
                msSQL = "select a.server_name,count(b.company_code)as company_code from adm_mst_tserver a left join adm_mst_tconsumer b on b.server_gid=a.server_gid group by a.server_name order by a.server_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<portalchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new portalchart_list
                        {
                            server_name = dt["server_name"].ToString(),
                            company_code = dt["company_code"].ToString(),                

                        });
                        values.portalchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGettotaldatabasecount(MdlDashboard values)
        {
            try
            {
                msSQL = "select count(company_code)as company_code from adm_mst_tconsumer";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<portalchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new portalchart_list
                        {
                            company_code = dt["company_code"].ToString(),
                        });
                        values.portalchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGettotalservercount(MdlDashboard values)
        {
            try
            {
                msSQL = "select count(server_name)as server_name from adm_mst_tserver";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<portalchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new portalchart_list
                        {
                            server_name = dt["server_name"].ToString(),
                        });
                        values.portalchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetsubscriptiontilescount(MdlDashboard values)
        {
            try
            {
                msSQL = "select count(company_code) as  company_code from adm_mst_tconsumer";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<subscriptiontilescount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new subscriptiontilescount_list
                        {
                            company_code = dt["company_code"].ToString(),                         

                        });
                        values.subscriptiontilescount_list = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetservertilescount(MdlDashboard values)
        {
            try
            {
                msSQL = "select count(server_name) as  server_name from adm_mst_tserver";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<subscriptiontilescount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new subscriptiontilescount_list
                        {
                            server_name = dt["server_name"].ToString(),
                        });
                        values.subscriptiontilescount_list = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetmonthwisedbchart(MdlDashboard values)
        {
            try
            {
                msSQL = "SELECT a.server_gid, COUNT(a.company_code) AS db_count, COUNT(DISTINCT b.server_gid) AS server_count, DATE_FORMAT(a.created_date, '%b-%Y') AS created_date, "+
                         "SUBSTRING(DATE_FORMAT(a.created_date, '%M'), 1, 3) AS month, YEAR(a.created_date) AS year, DATE_FORMAT(a.created_date, '%M/%Y') AS month_wise FROM "+
                         "adm_mst_tconsumer a LEFT JOIN adm_mst_tserver b ON a.server_gid = b.server_gid GROUP BY YEAR(a.created_date), MONTH(a.created_date), a.server_gid "+
                         "ORDER BY a.created_date ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<monthwisedbchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new monthwisedbchart_list
                        {
                            db_count = dt["db_count"].ToString(),
                            month_wise = dt["month_wise"].ToString(),
                            year = dt["year"].ToString(),
                            server_count = dt["server_count"].ToString(),
                            month = dt["month"].ToString(),
                        });
                        values.monthwisedbchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetmonthwiseservercount(MdlDashboard values)
        {
            try
            {
                msSQL = "SELECT DATE_FORMAT(created_date, '%Y-%m') AS month_year, COUNT(server_name) AS server_count FROM adm_mst_tserver GROUP BY DATE_FORMAT(created_date, '%Y-%m') ORDER BY DATE_FORMAT(created_date, '%Y-%m') DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<subscriptiontilescount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new subscriptiontilescount_list
                        {
                            server_name = dt["server_name"].ToString(),
                        });
                        values.subscriptiontilescount_list = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetmonthwiseservertilescount(MdlDashboard values)
        {
            try
            {
                msSQL = "SELECT COUNT(server_name) AS server_name,DATE_FORMAT(CURDATE(), '%M') AS mtd_month FROM adm_mst_tserver WHERE MONTH(created_date) = MONTH(CURDATE()) AND YEAR(created_date) = YEAR(CURDATE())";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardchartList = new List<portalchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardchartList.Add(new portalchart_list
                        {
                            server_name = dt["server_name"].ToString(),
                            mtd_month = dt["mtd_month"].ToString(),
                        });
                        values.portalchart_list = getDashboardchartList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}