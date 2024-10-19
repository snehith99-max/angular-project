using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using ems.outlet.Models;
using System.Diagnostics.Eventing.Reader;

namespace ems.outlet.Dataaccess
{
    public class DaOutletManager : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable, dt_datatable1;
        int mnResult;
        string lstracker_gid;
        public void DaGetdaymanagerSummary(string user_gid, MdlOutletManager values)
        {
            try
            {
                var getModuleList = new List<daymanagersummary_list>(); 
                msSQL = " select c.campaign_title from otl_trn_touletcampaign c " +
                        " left join otl_trn_tcampaign2manager a on c.campaign_gid = a.campaign_gid " +
                        " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                        " where b.user_gid ='" + user_gid + " '";
                dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count != 0)
                {
                    foreach (DataRow dt1 in dt_datatable1.Rows)
                    {
                        string campaign_title = dt1["campaign_title"].ToString();

                        msSQL = " select a.daytracker_gid,Format(a.revenue_amount,2) as revenue_amount,Format(a.expense_amount,2) as expense_amount,a.campaign_name,date_format(a.created_date,'%d-%m-%Y') as created_date,a.edit_status," +
                                " b.tracker_gid,date_format(a.trade_date,'%d-%m-%Y') as trade_date,concat(c.user_firstname,' ', c.user_lastname) as created_by " +
                                " from otl_mst_tdaytracker a " +
                                " left join otl_trn_tedittracker b on a.daytracker_gid = b.daytracker_gid " +
                                " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                                " where a.campaign_name='" + campaign_title + "' group by a.daytracker_gid  order by created_date desc";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new daymanagersummary_list
                                {
                                    created_date = dt["created_date"].ToString(),
                                    trade_date = dt["trade_date"].ToString(),
                                    created_by = dt["created_by"].ToString(),
                                    expense_amount = dt["expense_amount"].ToString(),
                                    revenue_amount = dt["revenue_amount"].ToString(),
                                    daytracker_gid = dt["daytracker_gid"].ToString(),
                                    edit_status = dt["edit_status"].ToString(),
                                    tracker_gid = dt["tracker_gid"].ToString(),
                                    campaign_name = dt["campaign_name"].ToString(),
                                });
                            }
                        }
                        dt_datatable.Dispose();
                    }

                    values.daymanagersummary_list = getModuleList;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while loading DayTracker Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetmanagerApproval(string daytracker_gid, MdlOutletManager values)
        {
            try
            {
                msSQL = " select date_format(a.created_date,'%d-%m-%Y') as created_date,a.daytracker_gid," +
                        " a.campaign_title,a.edit_reason,a.edit_status," +
                        " concat(b.user_firstname,' ' ,b.user_lastname) as rasied_by " +
                        " from otl_trn_tedittracker a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by " +
                        " where a.daytracker_gid='" + daytracker_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<managerApproval_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new managerApproval_list
                        {
                            created_date = dt["created_date"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            edit_reason = dt["edit_reason"].ToString(),
                            edit_status = dt["edit_status"].ToString(),
                            rasied_by = dt["rasied_by"].ToString(),
                            daytracker_gid = dt["daytracker_gid"].ToString(),
                        });
                        values.managerApproval_list = getModuleList;
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

        public void DaPostManagerupdate(string user_gid, managerApproval_list values)
        {
            try
            {
                 msSQL = "select tracker_gid from otl_trn_tedittracker where daytracker_gid='" + values.daytracker_gid + "' and trackedit_flag='Y'";
                    lstracker_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " update otl_trn_tedittracker set" +
                            " edit_status = '"+values.edit_status+"'," +
                            " trackedit_flag = 'N'" +
                            " where daytracker_gid = '" + values.daytracker_gid + "' and tracker_gid='" + lstracker_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update otl_mst_tdaytracker set" +
                                " edit_status = '"+values.edit_status+"'" +
                                " where daytracker_gid = '" + values.daytracker_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Edit Request " + values.edit_status + "!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While "+values.edit_status+"";
                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Edit "+values.edit_status+"!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


    }
}