using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Configuration;
using System.IO;
using System.Web.Http.Results;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Runtime.Remoting.Lifetime;

namespace ems.outlet.Dataaccess
{
    public class DaDayTracker : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable, dt_datatable1;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msgetdaytracker_gid,lscampaign_name, lsversion_id, lsrevenue_amount, lsproject_id, lstracker_gid, lsidentifierValue, lsexpense_amount, lsapprovalotp, lsmanager, lscampaign_gid;
        public void DaGetdaytrackerSummary(string user_gid,MdlDayTracker values)
        {
            try
            {
                msSQL = " SELECT a.daytracker_gid,FORMAT(a.revenue_amount, 2) AS revenue_amount, FORMAT(a.expense_amount, 2) AS expense_amount, " +
                        " a.campaign_name,  DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,  DATE_FORMAT(a.trade_date, '%d-%m-%Y') AS trade_date," +
                        " a.edit_status, b.tracker_gid, CONCAT(c.user_firstname, ' ', c.user_lastname) AS created_by  FROM otl_mst_tdaytracker a  " +
                        " LEFT JOIN otl_trn_tedittracker b ON a.daytracker_gid = b.daytracker_gid   " +
                        " LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by    " +
                        " INNER JOIN ( SELECT created_by, MAX(trade_date) AS max_trade_date  " +
                        " FROM otl_mst_tdaytracker GROUP BY created_by) d ON a.created_by = d.created_by " +
                        " AND a.trade_date = d.max_trade_date WHERE a.created_by = '"+ user_gid + "' group by daytracker_gid ORDER BY created_date DESC"; 
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<daytrackersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new daytrackersummary_list
                        {
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                            daytracker_gid = dt["daytracker_gid"].ToString(),
                            edit_status = dt["edit_status"].ToString(),
                            tracker_gid = dt["tracker_gid"].ToString(),
                            campaign_name = dt["campaign_name"].ToString(),
                            trade_date = dt["trade_date"].ToString(),
                        });
                        values.daytrackersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading DayTracker Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaRevenuesummary(MdlDayTracker values)
        {
            try
            {

                msSQL = " select  a.revenue_gid, a.revenue_desc,a.revenue_name, a.revenue_code" +
                    " from sys_mst_trevenuecategory a ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<revenue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new revenue_list
                        {
                            revenue_gid = dt["revenue_gid"].ToString(),
                            revenue_desc = dt["revenue_desc"].ToString(),
                            revenue_code = dt["revenue_code"].ToString(),
                            revenue_name = dt["revenue_name"].ToString(),
                            revenue_amount="0"

                        });
                        values.revenue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public void DaGetExpenseSummary(MdlDayTracker values)
        {
            try
            {

                msSQL = " select  a.expense_gid, a.expense_desc,a.expense_name, a.expense_code" +
                    " from sys_mst_texpensecategory a ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<expense_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new expense_list
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            expense_desc = dt["expense_desc"].ToString(),
                            expense_code = dt["expense_code"].ToString(),
                            expense_name = dt["expense_name"].ToString(),

                        });
                        values.expense_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostDaytrackersubmit(string user_gid, DaytrackerData values)
        {
            try
            {
                msSQL = " select a.campaign_title,a.campaign_gid from otl_trn_touletcampaign a " +
                          " left join otl_trn_tcampaign2manager b on b.campaign_gid = a.campaign_gid " +
                          " left join hrm_mst_temployee c on c.employee_gid = b.employee_gid " +
                          " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                          " where d.user_gid = '" + user_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscampaign_name = objOdbcDataReader["campaign_title"].ToString();
                    lscampaign_gid = objOdbcDataReader["campaign_gid"].ToString();
                }
                string tradeDateString = values.trade_date; 
                DateTime tradeDate = DateTime.ParseExact(tradeDateString, "dd-MMMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string lstrade_Date = tradeDate.ToString("yyyy-MM-dd");
                msSQL = "Select daytracker_gid from otl_mst_tdaytracker where created_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and trade_date='" + lstrade_Date + "' and campaign_gid='" + lscampaign_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == false)
                {
                    msgetdaytracker_gid = objcmnfunctions.GetMasterGID("DTRN");
                    msSQL = " insert into otl_mst_tdaytracker(" +
                            " daytracker_gid," +
                            " revenue_amount," +
                            " expense_amount," +
                            " campaign_name," +
                            " campaign_gid," +
                            " remarks," +
                            " created_by," +
                            " trade_date," +
                            " created_date" +
                            " )values(" +
                            " '" + msgetdaytracker_gid + "'," +
                            " '" + values.revenue_total + "'," +
                            " '" + values.expense_total + "'," +
                            " '" + lscampaign_name + "'," +
                            " '" + lscampaign_gid + "'," +
                            " '" + values.remarks + "'," +
                            " '" + user_gid + "'," +
                            " '" + lstrade_Date + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    foreach (var data in values.revenue_list)
                    {
                        if (values.leave=="Y")
                        {
                            lsrevenue_amount = "0.00";
                        }
                        else
                        {
                            lsrevenue_amount = data.revenue_amount;
                        }
                        string msgetdaytrackerdtl_gid = objcmnfunctions.GetMasterGID("DTRDT");
                        msSQL = " insert into sys_mst_tdaytrackerdtl(" +
                               " daytrackerdtl_gid," +
                               " daytracker_gid," +
                               " revenue_code," +
                               " revenue_gid," +
                               " revenue_desc," +
                               " revenue_name," +
                               " revenue_amount," +
                               " daytrackertype," +
                               " campaign_gid," +
                               " campaign_name," +
                               " created_by," +
                               " trade_date," +
                               " created_date" +
                                " )values(" +
                                 " '" + msgetdaytrackerdtl_gid + "'," +
                                 " '" + msgetdaytracker_gid + "'," +
                                 " '" + data.revenue_code + "'," +
                                 " '" + data.revenue_gid + "'," +
                                 " '" + data.revenue_desc + "'," +
                                 " '" + data.revenue_name + "'," +
                                 " '" + lsrevenue_amount + "'," +
                                 "'Revenue'," +
                                 " '" + lscampaign_gid + "'," +
                                 " '" + lscampaign_name + "'," +
                                 " '" + user_gid + "'," +
                                " '" + lstrade_Date + "'," +
                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    foreach (var data in values.expense_list)
                    {
                        if (values.leave == "Y")
                        {
                            lsexpense_amount = "0.00";
                        }
                        else
                        {
                            lsexpense_amount = data.expense_amount;
                        }
                        string msgetdaytrackerdtl_gid = objcmnfunctions.GetMasterGID("DTRDT");
                        msSQL = " insert into sys_mst_tdaytrackerdtl(" +
                                 " daytrackerdtl_gid," +
                                 " daytracker_gid," +
                                 " expense_name," +
                                 " expense_code," +
                                 " expense_gid," +
                                 " expense_desc," +
                                 " expense_amount," +
                                 " daytrackertype," +
                                 " campaign_gid," +
                                 " campaign_name," +
                                 " created_by," +
                                 " trade_date," +
                                 " created_date" +
                                 " )values(" +
                                 " '" + msgetdaytrackerdtl_gid + "'," +
                                 " '" + msgetdaytracker_gid + "'," +
                                 " '" + data.expense_name + "'," +
                                 " '" + data.expense_code + "'," +
                                 " '" + data.expense_gid + "'," +
                                 " '" + data.expense_desc + "'," +
                                 " '" + lsexpense_amount + "'," +
                                 "'Expense'," +
                                 " '" + lscampaign_gid + "'," +
                                 " '" + lscampaign_name + "'," +
                                 " '" + user_gid + "'," +
                                 " '" + lstrade_Date + "'," +
                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Day Tracker Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Day Tracker!!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Expense and Revenue Amount  Already Created today !!";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPosttradedatesubmit(string user_gid, DaytrackerData values)
        {
            try
            {
                    msSQL = " select a.campaign_title,a.campaign_gid from otl_trn_touletcampaign a " +
                           " left join otl_trn_tcampaign2manager b on b.campaign_gid = a.campaign_gid " +
                           " left join hrm_mst_temployee c on c.employee_gid = b.employee_gid " +
                           " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                           " where d.user_gid = '" + user_gid + "'";

                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        lscampaign_name = objOdbcDataReader["campaign_title"].ToString();
                        lscampaign_gid = objOdbcDataReader["campaign_gid"].ToString();
                    }
                    msgetdaytracker_gid = objcmnfunctions.GetMasterGID("DTRN");
                    //string msgetdaytracker_gid = objcmnfunctions.GetMasterGID("DTRN");
                    msSQL = " insert into otl_mst_tdaytracker(" +
                            " daytracker_gid," +
                            " revenue_amount," +
                            " expense_amount," +
                            " campaign_gid," +
                            " campaign_name," +
                            " remarks," +
                            " created_by," +
                            " trade_date," +
                            " created_date" +
                            " )values(" +
                            " '" + msgetdaytracker_gid + "'," +
                            " '" + values.revenue_total + "'," +
                            " '" + values.expense_total + "'," +
                            " '" + lscampaign_name + "'," +
                            " '" + lscampaign_gid + "'," +
                            " '" + values.remarks + "'," +
                            " '" + user_gid + "'," +
                            " '" + values.trade_date + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    foreach (var data in values.revenue_list)
                    {
                        string msgetdaytrackerdtl_gid = objcmnfunctions.GetMasterGID("DTRDT");
                        msSQL = " insert into sys_mst_tdaytrackerdtl(" +
                               " daytrackerdtl_gid," +
                               " daytracker_gid," +
                               " revenue_code," +
                               " revenue_gid," +
                               " revenue_desc," +
                               " revenue_name," +
                               " revenue_amount," +
                               " daytrackertype," +
                               " campaign_gid," +
                               " campaign_name," +
                               " created_by," +
                               " trade_date," +
                               " created_date" +
                                " )values(" +
                                 " '" + msgetdaytrackerdtl_gid + "'," +
                                 " '" + msgetdaytracker_gid + "'," +
                                 " '" + data.revenue_code + "'," +
                                 " '" + data.revenue_gid + "'," +
                                 " '" + data.revenue_desc + "'," +
                                 " '" + data.revenue_name + "'," +
                                 " '" + data.revenue_amount + "'," +
                                 "'Revenue'," +
                                 " '" + lscampaign_gid + "'," +
                                 " '" + lscampaign_name + "'," +
                                 " '" + user_gid + "'," +
                                 " '" + values.trade_date + "'," +
                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    foreach (var data in values.expense_list)
                    {

                        string msgetdaytrackerdtl_gid = objcmnfunctions.GetMasterGID("DTRDT");
                        msSQL = " insert into sys_mst_tdaytrackerdtl(" +
                                 " daytrackerdtl_gid," +
                                 " daytracker_gid," +
                                 " expense_name," +
                                 " expense_code," +
                                 " expense_gid," +
                                 " expense_desc," +
                                 " expense_amount," +
                                 " daytrackertype," +
                                 " campaign_gid," +
                                 " campaign_name," +
                                 " created_by," +
                                 " trade_date," +
                                 " created_date" +
                                 " )values(" +
                                 " '" + msgetdaytrackerdtl_gid + "'," +
                                 " '" + msgetdaytracker_gid + "'," +
                                 " '" + data.expense_name + "'," +
                                 " '" + data.expense_code + "'," +
                                 " '" + data.expense_gid + "'," +
                                 " '" + data.expense_desc + "'," +
                                 " '" + data.expense_amount + "'," +
                                 "'Expense'," +
                                 " '" + lscampaign_gid + "'," +
                                 " '" + lscampaign_name + "'," +
                                 " '" + user_gid + "'," +
                                 " '" + values.trade_date + "'," +
                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Day Tracker Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Day Tracker!!";
                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetRevenueEditsummary(string daytracker_gid,MdlDayTracker values)
        {
            try
            {

                msSQL = " select daytrackerdtl_gid,daytracker_gid,revenue_gid, revenue_desc,revenue_name, revenue_code,format(revenue_amount,2) as revenue_amount  from sys_mst_tdaytrackerdtl where  daytracker_gid='" + daytracker_gid + "' and revenue_gid is not null ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<revenue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                   
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                       
                        getModuleList.Add(new revenue_list
                        {
                            revenue_gid = dt["revenue_gid"].ToString(),
                            revenue_desc = dt["revenue_desc"].ToString(),
                            revenue_code = dt["revenue_code"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                            revenue_name = dt["revenue_name"].ToString(),
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            daytracker_gid = dt["daytracker_gid"].ToString(),
                        });
                        values.revenue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Revenue data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public void DaGetExpenseEditSummary(string daytracker_gid,MdlDayTracker values)
        {
            try
            {

                msSQL = " select daytrackerdtl_gid,daytracker_gid,expense_gid, expense_desc,expense_name, expense_code,Format(expense_amount,2) as expense_amount   from sys_mst_tdaytrackerdtl where  daytracker_gid='" + daytracker_gid + "' and expense_gid is not null ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<expense_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new expense_list
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            expense_desc = dt["expense_desc"].ToString(),
                            expense_code = dt["expense_code"].ToString(),
                            expense_name = dt["expense_name"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            daytracker_gid = dt["daytracker_gid"].ToString(),
                        });
                        values.expense_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOutletname(string user_gid, MdlDayTracker values)
        {
            try
            {

                msSQL = " select a.campaign_title from otl_trn_touletcampaign a "+
                        " left join otl_trn_tcampaign2manager b on b.campaign_gid = a.campaign_gid "+
                        " left join hrm_mst_temployee c on c.employee_gid = b.employee_gid " +
                        " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                        " where d.user_gid = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outletname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outletname_list
                        {
                            campaign_title = dt["campaign_title"].ToString(),
                        });
                        values.outletname_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading outlet Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaTradesummary(string user_gid, MdlDayTracker values)
        {
            try
            {
                msSQL = " select a.campaign_gid from otl_trn_touletcampaign a " +
                         " left join otl_trn_tcampaign2manager b on b.campaign_gid = a.campaign_gid " +
                         " left join hrm_mst_temployee c on c.employee_gid = b.employee_gid " +
                         " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                         " where d.user_gid = '" + user_gid + "'";
                lscampaign_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "SELECT MAX(trade_date) AS trade_date FROM otl_mst_tdaytracker  " +
                                "WHERE campaign_gid = '" + lscampaign_gid + "'";
                object result = objdbconn.GetExecuteScalar(msSQL);
                string tradeDateString = result?.ToString();
                var getModuleList = new List<trade_list>();
                DateTime yesterday = DateTime.Now.Date.AddDays(-1);
                if (!string.IsNullOrEmpty(tradeDateString) && DateTime.TryParse(tradeDateString, out DateTime trade_date))
                {
                    DateTime nextDate = trade_date.AddDays(1);
                    if (nextDate < DateTime.Now.Date)
                    {
                        if (yesterday != nextDate)
                        {
                            getModuleList.Add(new trade_list
                            {
                                balance_date = nextDate.ToString("dd-MMMM-yyyy"),
                                notification = "Y",
                                previous_date=trade_date.ToString("dd-MMMM-yyyy")
                            });
                        }
                        else 
                        {
                            getModuleList.Add(new trade_list
                            {
                                balance_date = DateTime.Now.AddDays(-1).ToString("dd-MMMM-yyyy"),
                                notification = "N"
                            });
                        }

                    }
                   
                }
                else
                {
                    getModuleList.Add(new trade_list
                    {
                        balance_date = DateTime.Now.AddDays(-1).ToString("dd-MMMM-yyyy"),
                        notification = "N"
                    }); 

                }
                values.trade_list = getModuleList;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading outlet Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPosteditrequest(string user_gid, outletname_list values)
        {
            try
            {
                msSQL = "SELECT * FROM otl_trn_tedittracker WHERE daytracker_gid ='"+ values.daytracker_gid + "' AND edit_status='Pending'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {
                    Random random = new Random();
                    int randomDigits = random.Next(1000, 10000);
                    string grnOtp = randomDigits.ToString();
                    msGetGid = objcmnfunctions.GetMasterGID("OET");
                    msSQL = " insert into otl_trn_tedittracker(" +
                              " tracker_gid," +
                              " daytracker_gid," +
                              " campaign_title," +
                              " edit_reason," +
                              " edit_status," +
                              " approval_otp," +
                              " created_by, " +
                              " created_date)" +
                              " values(" +
                              "'" + msGetGid + "'," +
                              "'" + values.daytracker_gid + "'," +
                              "'" + values.campaign_title + "'," +
                              "'" + values.edit_reason + "'," +
                              "'Pending'," +
                              "'" + objcmnfunctions.ConvertToAscii(grnOtp) + "'," + 
                              "'" + user_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {

                        msSQL= " select project_id, version_id from crm_smm_whatsapptemplate where p_type = 'otp approval'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                             lsproject_id = objOdbcDataReader["project_id"].ToString();
                             lsversion_id = objOdbcDataReader["version_id"].ToString();
                            msSQL = "SELECT employee_mobileno FROM hrm_mst_temployee WHERE user_gid = '" + user_gid + "'";
                            lsidentifierValue = objdbconn.GetExecuteScalar(msSQL);
                            if (!lsidentifierValue.StartsWith("+91"))
                            {
                                lsidentifierValue = "+91" + lsidentifierValue;
                            }
                            msSQL = "select concat(user_firstname,' ',user_lastname) as manager_name from adm_mst_tuser where user_gid='" + user_gid + "'";
                            lsmanager= objdbconn.GetExecuteScalar(msSQL);
                            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                            Result objsendmessage = new Result();
                            string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + lsidentifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + lsproject_id + "\",\"version\":\"" + lsversion_id + "\",\"variables\":{ \"name\":\"" + lsmanager + "\",\"outlet_name\":\"" + values.campaign_title + "\",\"date\":\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",\"Manager\":\"" + lsmanager + "\",\"password\":\"" + grnOtp + "\",\"reason\":\"" + values.edit_reason + "\"},\"locale\":\"en\"}}";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                            request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            string waresponse = response.Content;
                            objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);
                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                msSQL = " update otl_mst_tdaytracker set" +
                                          " edit_status = 'Pending'" +
                                          " where daytracker_gid = '" + values.daytracker_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                values.status = true;
                                values.message = "Your edit request has been successfully raised and is now in the approval stage. Please wait for some time.";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Sending OTP";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Sending OTP";
                        }     
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Edit Tracker";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Your Editing Request Already in Pending Stage";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Edit Tracker Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

          
        }
        public whatsappconfiguration whatsappcredentials()
        {
            whatsappconfiguration getwhatsappcredentials = new whatsappconfiguration();
            try
            {


                msSQL = " select workspace_id,channel_id,access_token,channelgroup_id from crm_smm_whatsapp_service";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    getwhatsappcredentials.workspace_id = objOdbcDataReader["workspace_id"].ToString();
                    getwhatsappcredentials.channel_id = objOdbcDataReader["channel_id"].ToString();
                    getwhatsappcredentials.access_token = objOdbcDataReader["access_token"].ToString();
                    getwhatsappcredentials.channelgroup_id = objOdbcDataReader["channelgroup_id"].ToString();
                }
                else
                {

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return getwhatsappcredentials;
        }
        public void DaGetotpverification(string daytracker_gid, MdlDayTracker values)
        {
            try
            {
                msSQL = " select date_format(a.created_date,'%d-%m-%Y') as created_date," +
                        " a.campaign_title,a.edit_reason,a.edit_status," +
                        " concat(b.user_firstname,' ' ,b.user_lastname) as rasied_by " +
                        " from otl_trn_tedittracker a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by " +
                        " where a.daytracker_gid='" + daytracker_gid + "' " +
                        " and a.edit_status='Pending'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<otpverification_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new otpverification_list
                        {
                            created_date = dt["created_date"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            edit_reason = dt["edit_reason"].ToString(),
                            edit_status = dt["edit_status"].ToString(),
                            rasied_by = dt["rasied_by"].ToString(),
                        });
                        values.otpverification_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading OTP verification!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPosteditotp(string user_gid, otpverification_list values)
        {
            try
            {
                msSQL = "select approval_otp from otl_trn_tedittracker where daytracker_gid='" + values.daytracker_gid + "' and trackedit_flag='Y'";
                lsapprovalotp=objdbconn.GetExecuteScalar(msSQL);
                if (lsapprovalotp == objcmnfunctions.ConvertToAscii(values.approval_otp))
                {
                    msSQL = "select tracker_gid from otl_trn_tedittracker where daytracker_gid='" + values.daytracker_gid + "' and trackedit_flag='Y'";
                    lstracker_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " update otl_trn_tedittracker set" +
                            " edit_status = 'Approved'," +
                            " trackedit_flag = 'N'" +
                            " where daytracker_gid = '" + values.daytracker_gid + "' and tracker_gid='" + lstracker_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                   msSQL = " update otl_mst_tdaytracker set" +
                           " edit_status = 'Approved'" +
                           " where daytracker_gid = '" + values.daytracker_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1) {
                            values.status = true;
                            values.message = "OTP Verified Successfully";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While updating OTP";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Invalid OTP";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Edit request OTP Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostDaytrackeredit(string user_gid, DaytrackerData values)
        {
            try
            {
                msSQL = "select daytracker_gid,revenue_amount,expense_amount,campaign_name from otl_mst_tdaytracker where daytracker_gid='" + values.daytracker_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Read();
                    msgetdaytracker_gid = objcmnfunctions.GetMasterGID("THN");
                    msSQL = " insert into otl_mst_tdaytrackerhistory(" +
                               " trackerhis_gid," +
                               " daytracker_gid," +
                               " revenue_amount," +
                               " expense_amount," +
                               " campaign_name," +
                               " created_by, " +
                               " created_date)" +
                               " values(" +
                               "'" + msgetdaytracker_gid + "'," +
                               "'" + values.daytracker_gid + "'," +
                               "'" + objOdbcDataReader["revenue_amount"].ToString() + "'," +
                               "'" + objOdbcDataReader["expense_amount"].ToString() + "'," +
                               "'" + objOdbcDataReader["campaign_name"].ToString() + "'," +
                               "'" + user_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    foreach (var data in values.revenue_list)
                    {
                        msSQL = " select daytrackerdtl_gid,daytracker_gid,revenue_gid,revenue_amount,revenue_code,revenue_desc," +
                                " daytrackertype,revenue_name,campaign_gid,campaign_name from sys_mst_tdaytrackerdtl" +
                                " where daytracker_gid='" + values.daytracker_gid + "' and daytrackerdtl_gid='" + data.daytrackerdtl_gid + "' and daytrackertype='Revenue'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                       if (dt_datatable.Rows.Count != 0)
                        {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            string msgetdaytrackerdtl_gid = objcmnfunctions.GetMasterGID("TDHN");
                            string daytracker_gid = dt["daytracker_gid"].ToString();
                            string daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString();
                            string revenue_code = dt["revenue_code"].ToString();
                            string revenue_gid = dt["revenue_gid"].ToString();
                            string revenue_desc = dt["revenue_desc"].ToString();
                            string revenue_name = dt["revenue_name"].ToString();
                            string daytrackertype = dt["daytrackertype"].ToString();
                            string revenue_amount = dt["revenue_amount"].ToString();
                            string campaign_gid = dt["campaign_gid"].ToString();
                            string campaign_name = dt["campaign_name"].ToString();

                             msSQL = " insert into otl_trn_tdaytrackerhis(" +
                                                 " dayhistory_gid," +
                                                 " daytrackerdtl_gid," +
                                                 " daytracker_gid," +
                                                 " trackerhis_gid," +
                                                 " revenue_code," +
                                                 " revenue_gid," +
                                                 " revenue_desc," +
                                                 " revenue_name," +
                                                 " revenue_amount," +
                                                 " daytrackertype," +
                                                 " campaign_gid," +
                                                 " campaign_name," +
                                                 " created_by," +
                                                 " created_date" +
                                                 " )values(" +
                                                 " '" + msgetdaytrackerdtl_gid + "'," +
                                                 " '" + daytrackerdtl_gid + "'," +
                                                 " '" + daytracker_gid + "'," +
                                                 " '" + msgetdaytracker_gid + "'," +
                                                 " '" + revenue_code + "'," +
                                                 " '" + revenue_gid + "'," +
                                                 " '" + revenue_desc + "'," +
                                                 " '" + revenue_name + "'," +
                                                 " '" + revenue_amount + "'," +
                                                 " '" + daytrackertype + "'," +
                                                 " '" + campaign_gid + "'," +
                                                 " '" + campaign_name + "'," +
                                                 " '" + user_gid + "'," +
                                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                        msSQL = " update sys_mst_tdaytrackerdtl set" +
                                " revenue_amount = '" + data.revenue_amount + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                " where daytrackerdtl_gid = '" + data.daytrackerdtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                      }
                        if (mnResult != 0)
                        {
                            msSQL = " update otl_trn_tedittracker set" +
                                   " edit_status = 'Completed'" +
                                   " where daytracker_gid = '" + values.daytracker_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    
                    foreach (var data in values.expense_list)
                    {
                        msSQL = " select daytrackerdtl_gid,daytracker_gid,expense_gid,expense_amount,expense_code," +
                               " expense_desc,daytrackertype,expense_name,campaign_gid,campaign_name from sys_mst_tdaytrackerdtl " +
                               " where daytracker_gid='" + values.daytracker_gid + "' and daytrackerdtl_gid='" + data.daytrackerdtl_gid + "' and daytrackertype='Expense'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                string msgetdaytrackerdtl_gid = objcmnfunctions.GetMasterGID("TDHN");
                                string daytracker_gid = dt["daytracker_gid"].ToString();
                                string daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString();
                                string expense_gid = dt["expense_gid"].ToString();
                                string expense_desc = dt["expense_desc"].ToString();
                                string expense_amount = dt["expense_amount"].ToString();
                                string expense_name = dt["expense_name"].ToString();
                                string daytrackertype = dt["daytrackertype"].ToString();
                                string expense_code = dt["expense_code"].ToString();
                                string campaign_gid = dt["campaign_gid"].ToString();
                                string campaign_name = dt["campaign_name"].ToString();
                                msSQL = " insert into otl_trn_tdaytrackerhis(" +
                                                    " dayhistory_gid," +
                                                    " daytrackerdtl_gid," +
                                                    " daytracker_gid," +
                                                    " trackerhis_gid," +
                                                    " expense_code," +
                                                    " expense_gid," +
                                                    " expense_desc," +
                                                    " expense_name," +
                                                    " expense_amount," +
                                                    " daytrackertype," +
                                                    " campaign_gid," +
                                                    " campaign_name," +
                                                    " created_by," +
                                                    " created_date" +
                                                    " )values(" +
                                                    " '" + msgetdaytrackerdtl_gid + "'," +
                                                    " '" + daytrackerdtl_gid + "'," +
                                                    " '" + daytracker_gid + "'," +
                                                    " '" + msgetdaytracker_gid + "'," +
                                                    " '" + expense_code + "'," +
                                                    " '" + expense_gid + "'," +
                                                    " '" + expense_desc + "'," +
                                                    " '" + expense_name + "'," +
                                                    " '" + expense_amount + "'," +
                                                    " '" + daytrackertype + "'," +
                                                    " '" + campaign_gid + "'," +
                                                    " '" + campaign_name + "'," +
                                                    " '" + user_gid + "'," +
                                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }

                        msSQL = " update sys_mst_tdaytrackerdtl set" +
                                " expense_amount = '" + data.expense_amount + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                " where daytrackerdtl_gid = '" + data.daytrackerdtl_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        msSQL = " update otl_trn_tedittracker set" +
                               " edit_status = 'Completed'" +
                               " where daytracker_gid = '" + values.daytracker_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update otl_mst_tdaytracker set" +
                                    " edit_status = 'Completed'," +
                                    " revenue_amount = '" + values.revenue_total + "'," +
                                    " expense_amount = '" + values.expense_total + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                    " where daytracker_gid = '" + values.daytracker_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Day Tracker Edited Successfully !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Edit Day Tracker!!";
                        }
                    }
                    
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Edit Day Tracker!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Day tracker Edit!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetviewdaytracker(string daytracker_gid, MdlDayTracker values)
        {
            try
            {
                msSQL = " select Format(revenue_amount,2) as revenue_amount,Format(expense_amount,2) as expense_amount ,campaign_name,date_format(created_date,'%d-%m-%Y') as created_date,date_format(trade_date,'%d-%m-%Y') as trade_date from otl_mst_tdaytracker where daytracker_gid='" + daytracker_gid + "'" ;
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<daytrackersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new daytrackersummary_list
                        {
                            created_date = dt["created_date"].ToString(),
                            trade_date = dt["trade_date"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            campaign_name = dt["campaign_name"].ToString(),
                        });
                        values.daytrackersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading OTP verification!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetedittrackersummaryr(string daytracker_gid, MdlDayTracker values)
        {
            try
            {
                string msSQL = "SELECT b.daytracker_gid,a.tracker_gid, a.edit_reason, a.edit_status, DATE_FORMAT(a.created_date,'%d-%m-%Y') AS created_date, CONCAT(d.user_firstname,' ', d.user_lastname) AS created_by, " +
                               "Format(b.revenue_amount,2) as revenue_amount, Format(b.expense_amount,2) as expense_amount " +
                               "FROM otl_mst_tdaytracker b " +
                               "LEFT JOIN otl_trn_tedittracker a ON a.daytracker_gid = b.daytracker_gid " +
                               "LEFT JOIN adm_mst_tuser d ON d.user_gid = a.created_by " +
                               "WHERE b.daytracker_gid = '" + daytracker_gid + "'";
                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                msSQL = "SELECT trackerhis_gid,Format(revenue_amount,2) AS pre_revenue, Format(expense_amount,2) AS pre_expense " +
                        "FROM otl_mst_tdaytrackerhistory " +
                        "WHERE daytracker_gid = '" + daytracker_gid + "'";
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<edittracker_list>();
                if (dt_datatable.Rows.Count == dt_datatable1.Rows.Count)
                {
                    for (int i = 0; i < dt_datatable.Rows.Count; i++)
                    {
                        DataRow dtRow1 = dt_datatable.Rows[i];
                        DataRow dtRow2 = dt_datatable1.Rows[i];
                        getModuleList.Add(new edittracker_list
                        {
                            tracker_gid = dtRow1["tracker_gid"].ToString(),
                            created_date = dtRow1["created_date"].ToString(),
                            revenue_amount = dtRow1["revenue_amount"].ToString(),
                            expense_amount = dtRow1["expense_amount"].ToString(),
                            edit_reason = dtRow1["edit_reason"].ToString(),
                            edit_status = dtRow1["edit_status"].ToString(),
                            created_by = dtRow1["created_by"].ToString(),
                            pre_revenue = dtRow2["pre_revenue"].ToString(),
                            pre_expense = dtRow2["pre_expense"].ToString(),
                            trackerhis_gid = dtRow2["trackerhis_gid"].ToString(),
                            daytracker_gid = dtRow1["daytracker_gid"].ToString(),
                        });
                    }
                    values.edittracker_list = getModuleList;
                }
                dt_datatable.Dispose();
                dt_datatable1.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Edit list!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetedittrackerdtl(string trackerhis_gid, MdlDayTracker values)
        {
            try
            {
                msSQL = " select b.trackerhis_gid,a.revenue_name,Format(a.revenue_amount,2) as revenue_amount ,Format(b.revenue_amount,2) as amount from sys_mst_tdaytrackerdtl a  "+
                        " left join otl_trn_tdaytrackerhis b on b.daytrackerdtl_gid = a.daytrackerdtl_gid "+
                        " where b.trackerhis_gid = '"+ trackerhis_gid + "' and b.daytrackertype = 'Revenue' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<edittrackerrevenue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new edittrackerrevenue_list
                        {
                            trackerhis_gid = dt["trackerhis_gid"].ToString(),
                            revenue_name = dt["revenue_name"].ToString(),
                            amount = dt["amount"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                        });
                        values.edittrackerrevenue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading DayTracker Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetedittrackerdtl1(string trackerhis_gid,MdlDayTracker values)
        {
            try
            {
                msSQL = " select b.trackerhis_gid,a.expense_name,Format(a.expense_amount,2) as expense_amount ,Format(b. expense_amount,2) as amount from sys_mst_tdaytrackerdtl a  "+
                        " left join otl_trn_tdaytrackerhis b on b.daytrackerdtl_gid = a.daytrackerdtl_gid "+
                        " where b.trackerhis_gid = '"+ trackerhis_gid + "' and b.daytrackertype = 'Expense'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<edittrackerexpence_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new edittrackerexpence_list
                        {
                            trackerhis_gid = dt["trackerhis_gid"].ToString(),
                            expense_name = dt["expense_name"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            amount = dt["amount"].ToString(),
                        });
                        values.edittrackerexpence_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading DayTracker Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetRevenueEditnewsummary(string daytracker_gid, MdlDayTracker values)
        {
            try
            {

                msSQL = " select daytrackerdtl_gid,daytracker_gid,revenue_gid, revenue_desc,revenue_name, revenue_code,revenue_amount from sys_mst_tdaytrackerdtl where  daytracker_gid='" + daytracker_gid + "' and revenue_gid is not null ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<revenue_list>();
                if (dt_datatable.Rows.Count != 0)
                {

                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new revenue_list
                        {
                            revenue_gid = dt["revenue_gid"].ToString(),
                            revenue_desc = dt["revenue_desc"].ToString(),
                            revenue_code = dt["revenue_code"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                            revenue_name = dt["revenue_name"].ToString(),
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            daytracker_gid = dt["daytracker_gid"].ToString(),
                        });
                        values.revenue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Revenue data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public void DaGetExpenseEditnewSummary(string daytracker_gid, MdlDayTracker values)
        {
            try
            {

                msSQL = " select daytrackerdtl_gid,daytracker_gid,expense_gid, expense_desc,expense_name, expense_code,expense_amount from sys_mst_tdaytrackerdtl where  daytracker_gid='" + daytracker_gid + "' and expense_gid is not null ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<expense_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new expense_list
                        {
                            expense_gid = dt["expense_gid"].ToString(),
                            expense_desc = dt["expense_desc"].ToString(),
                            expense_code = dt["expense_code"].ToString(),
                            expense_name = dt["expense_name"].ToString(),
                            expense_amount = dt["expense_amount"].ToString(),
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            daytracker_gid = dt["daytracker_gid"].ToString(),
                        });
                        values.expense_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Expense Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
