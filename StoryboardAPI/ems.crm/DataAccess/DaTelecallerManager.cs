using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;
using System.Text.RegularExpressions;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.crm.DataAccess
{
    public class DaTelecallerManager
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, lsemployee_gid;


        public void DaGetTelecallerManagerSummary(string employee_gid, MdlTelecallerManager values)
        {

            try
            {

                //if (employee_gid != null && employee_gid != "")
                //{
                //    msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
                //    lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
                //}
                //else
                //{
                //    lsemployee_gid = null;

                //}

                msSQL = " SELECT  a.campaign_gid, a.campaign_title,a.campaign_prefix,a.campaign_location,c.branch_name,(select count(x.employee_gid) as employeecount  " +
                    " from crm_trn_tteleteam2employee x  where x.campaign_gid= a.campaign_gid) as employeecount, (SELECT count(x.lead2campaign_gid)" +
                    " FROM  crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid where x.campaign_gid = a.campaign_gid " +
                    "     and (x.leadstage_gid='1' or x.leadstage_gid='2'or x.leadstage_gid='5' or x.leadstage_gid='7')and b.leadbank_name is not null) as assigned_leads, " +
                    "(SELECT count(x.lead2campaign_gid) FROM   crm_trn_ttelelead2campaign x left join crm_trn_tlog l on x.leadbank_gid = l.leadbank_gid" +
                    "  where x.so_status <>'Y' and (x.leadstage_gid ='1' or x.leadstage_gid='2') and x.leadstage_gid != '5'and  " +
                    " CAST(l.log_date AS DATE)=CURDATE()   and l.log_type = 'Schedule' and x.campaign_gid = a.campaign_gid ) as todayappointment," +
                    "(select count(x.lead2campaign_gid) from crm_trn_ttelelead2campaign x where x.leadbank_gid in(select leadbank_gid from crm_trn_tschedulelog where schedule_date = CURDATE()) and x.leadstage_gid != '5'  and x.campaign_gid = a.campaign_gid) as Schedule_log, "+
                    "  (SELECT count(x.lead2campaign_gid)  FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.so_status <>'Y' and (x.leadstage_gid ='1') and x.campaign_gid = a.campaign_gid and b.leadbank_name is not null and x.leadbank_gid  " +
                    "not in ( select leadbank_gid from crm_trn_tcalllog)) as newleads,(SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x  " +
                    "left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid where x.so_status !='Y' and (x.leadstage_gid ='2') and x.campaign_gid = a.campaign_gid " +
                    "and b.leadbank_name is not null ) as followup,(SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x  left join crm_trn_tleadbank b on " +
                    "x.leadbank_gid = b.leadbank_gid where x.so_status !='Y' and (x.leadstage_gid ='7') and x.campaign_gid = a.campaign_gid and b.leadbank_name is not null ) as prospect," +
                    "(SELECT count(DISTINCT x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tcalllog i " +
                    "on i.leadbank_gid = x.leadbank_gid where x.so_status !='Y' and i.call_response is not null and  (x.leadstage_gid ='1') and" +
                    " x.campaign_gid = a.campaign_gid and x.leadbank_gid in (select leadbank_gid from crm_trn_tcalllog)) as NewPending,(SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b " +
                    "on x.leadbank_gid = b.leadbank_gid where   (x.leadstage_gid ='5') and b.leadbank_name is not null and x.campaign_gid = a.campaign_gid ) as drop_status," +
                    "(SELECT  SUM(CASE WHEN DATEDIFF(NOW(), x.created_date) > 10 AND x.leadstage_gid = '1'  and x.campaign_gid = a.campaign_gid THEN 1 ELSE 0 END)FROM crm_trn_ttelelead2campaign x  ) as lapsed_count," +
                    " (SELECT SUM(CASE WHEN DATEDIFF(NOW(), x.created_date) > 10 AND x.leadstage_gid <= 2  and x.campaign_gid = a.campaign_gid THEN 1 ELSE 0 END)FROM crm_trn_ttelelead2campaign x) as longest_count ," +
                    "  (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x  left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    "  where x.campaign_gid = a.campaign_gid and b.leadbank_name is not null and x.leadbank_gid  in ( select DISTINCT leadbank_gid from crm_trn_tschedulelog)) as closed " +
                    " FROM crm_trn_tteleteam a left join hrm_mst_tbranch c on a.campaign_location = c.branch_gid  where a.campaign_gid in  (select team_gid  from cmn_trn_tmanagerprivilege" +
                    " where employee_gid = '"+ employee_gid + "' ) group by a.campaign_gid desc ";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new telecallermanager_lists
                        {
                            
                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            campaign_location = dt["campaign_location"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            employeecount = dt["employeecount"].ToString(),
                            assigned_leads = dt["assigned_leads"].ToString(),
                            Schedule_log = dt["Schedule_log"].ToString(),
                            newleads = dt["newleads"].ToString(),
                            followup = dt["followup"].ToString(),
                            prospect = dt["prospect"].ToString(),
                            NewPending = dt["NewPending"].ToString(),
                            drop_status = dt["drop_status"].ToString(),
                            closed = dt["closed"].ToString(),
                            lapsed_count = dt["lapsed_count"].ToString(),
                            longest_count = dt["longest_count"].ToString(),

                        });
                        values.telecallermanager_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }
        public void DaGetTelecallerTeamViewSummary(string campaign_gid ,string employee_gid, MdlTelecallerManager values)
        {

            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, a.drop_remarks, b.customer_type," +
                    " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
                    "concat_ws(' / ',d.region_name,b.leadbank_city,h.source_name) as" +
                    " region_name, Case when a.internal_notes is not null then a.internal_notes" +
                    " when a.internal_notes is null then b.remarks  end as internal_notes," +
                    "concat_ws(' / ',f.user_firstname,F.user_code) AS assigned_to , i.department_name, " +
                    " concat_ws('  ',y.user_firstname,y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid ,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  (a.leadstage_gid='1' or a.leadstage_gid='2'or a.leadstage_gid='7' or a.leadstage_gid='5') and a.campaign_gid = '" + campaign_gid + "' and g.main_contact ='Y' and " +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_name order by b.leadbank_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            drop_remarks = dt["drop_remarks"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetTelecallerManagerTotalSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, a.drop_remarks, b.customer_type," +
                    " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
                    " concat_ws(' / ',d.region_name,b.leadbank_city,h.source_name) as" +
                    " region_name,a.internal_notes ," +
                    "concat_ws(' / ',f.user_firstname,F.user_code) AS assigned_to , i.department_name, " +
                    " concat_ws(' ',y.user_firstname,y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  (a.leadstage_gid='1' or a.leadstage_gid='2'or a.leadstage_gid='5' or a.leadstage_gid='7') and g.main_contact ='Y' and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            drop_remarks = dt["drop_remarks"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            notes_count = lsCode,
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetNewleadchartcount(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = "SELECT date_format(x.created_date, '%b') AS months, " +
                        " SUM(CASE WHEN x.leadstage_gid = '1' and x.leadbank_gid not in ( select leadbank_gid from crm_trn_tcalllog) THEN 1 ELSE 0 END) AS new_leads, " +
                        " sum(case when (x.leadstage_gid = '1' ) and x.leadbank_gid in ( select leadbank_gid from crm_trn_tcalllog) then 1 else 0 end) as pending_calls, " +
                        " SUM(CASE WHEN x.leadstage_gid = '2' THEN 1 ELSE 0 END) AS follow_up, " +
                        " SUM(CASE WHEN leadstage_gid = '7' THEN 1 ELSE 0 END) AS prospect FROM crm_trn_ttelelead2campaign x " +
                        " left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                        " WHERE x.created_date >= date_sub(curdate(), interval 6 month) and x.campaign_gid in  " +
                        " (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) GROUP BY months ORDER BY x.created_date";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telechartscount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new telechartscount_list
                        {
                            months = dt["months"].ToString(),
                            new_leads = dt["new_leads"].ToString(),
                            pending_calls = dt["pending_calls"].ToString(),
                            follow_up = dt["follow_up"].ToString(),
                            prospect = dt["prospect"].ToString(),
                        });
                        values.telechartscount_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

        public void DaGetteamcount(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " SELECT SUM(employeecount) AS total_employees, SUM(assigned_leads) AS total_assigned_leads, SUM(todayappointment) AS total_today_appointments, " +
                        "  SUM(newleads) AS total_new_leads, SUM(followup) AS total_follow_ups, SUM(prospect) AS total_prospects, SUM(NewPending) AS total_new_pending, " +
                         " SUM(drop_status) AS total_drop_status FROM (SELECT (SELECT COUNT(x.employee_gid) FROM crm_trn_tteleteam2employee x WHERE a.campaign_gid = x.campaign_gid) " +
                         " AS employeecount, (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tleadbank b ON x.leadbank_gid = b.leadbank_gid " +
                         " WHERE(x.leadstage_gid = '1' OR x.leadstage_gid = '2' OR x.leadstage_gid = '5' OR x.leadstage_gid = '7') AND b.leadbank_name IS NOT NULL AND a.campaign_gid = x.campaign_gid) AS assigned_leads, " +  
                         " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tlog l ON x.leadbank_gid = l.leadbank_gid " +
                         " WHERE x.so_status<> 'Y' AND(x.leadstage_gid = '1' OR x.leadstage_gid = '2') AND x.leadstage_gid != '5' AND CAST(l.log_date AS DATE) = CURDATE() " +
                         " AND l.log_type = 'Schedule' AND a.campaign_gid = x.campaign_gid) AS todayappointment, " +
                         " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tleadbank b ON x.leadbank_gid = b.leadbank_gid " +
                         " WHERE x.so_status<> 'Y' AND(x.leadstage_gid = '1') AND b.leadbank_name IS NOT NULL " +
                         " AND a.campaign_gid = x.campaign_gid and x.leadbank_gid  not in ( select leadbank_gid from crm_trn_tcalllog)) AS newleads, " +
                         " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tleadbank b ON x.leadbank_gid = b.leadbank_gid " +
                         " WHERE x.so_status != 'Y' AND(x.leadstage_gid = '2') AND b.leadbank_name IS NOT NULL AND a.campaign_gid = x.campaign_gid) AS followup, " +
                         " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tleadbank b ON x.leadbank_gid = b.leadbank_gid " +
                         " WHERE x.so_status != 'Y' AND(x.leadstage_gid = '7') AND b.leadbank_name IS NOT NULL AND a.campaign_gid = x.campaign_gid) AS prospect, " +
                         " (SELECT COUNT(DISTINCT x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tcalllog i ON i.leadbank_gid = x.leadbank_gid " +
                         " WHERE x.leadstage_gid = '1' and x.leadbank_gid in(select leadbank_gid from crm_trn_tcalllog) and x.campaign_gid = a.campaign_gid) AS NewPending, " +
                         " (SELECT COUNT(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x LEFT JOIN crm_trn_tleadbank b ON x.leadbank_gid = b.leadbank_gid " +
                         " WHERE(x.leadstage_gid = '5') AND b.leadbank_name IS NOT NULL AND a.campaign_gid = x.campaign_gid) AS drop_status " +
                         " FROM crm_trn_tteleteam a  LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid " +
                         " WHERE a.campaign_gid IN(SELECT team_gid FROM cmn_trn_tmanagerprivilege WHERE employee_gid = '"+ employee_gid + "') " +
                         " GROUP BY  a.campaign_gid DESC) AS subquery";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<teletotaltilecount_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new teletotaltilecount_lists
                        {
                            assigned_leads = dt["total_assigned_leads"].ToString(),
                            //total_LapsedLeads = dt["newleads"].ToString(),
                            //total_LongestLeads = dt["total_Longest_lead"].ToString(),
                            total_newleads = dt["total_new_leads"].ToString(),
                            total_followup = dt["total_follow_ups"].ToString(),
                            total_potential = dt["total_prospects"].ToString(),
                            total_prospect = dt["total_new_pending"].ToString(),
                            total_dropstatus = dt["total_drop_status"].ToString(),
                            //total_customer = dt["total_customer"].ToString(),
                            //total_upcoming = dt["total_upcoming"].ToString(),
                            //total_mtd = dt["total_mtd"].ToString(),
                            //total_ytd = dt["total_ytd"].ToString(),

                        });
                        values.teletotaltilecount_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Total Tile Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTeamPerformencechart(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " select count(a.leadbank_gid) as callcount, DATE_FORMAT(a.call_date, '%b') AS Months,e.call_response" +
                    " from crm_trn_tcalllog a left join crm_trn_ttelelead2campaign b on a.leadbank_gid = b.leadbank_gid " +
                    "  left join cmn_trn_tmanagerprivilege c on b.campaign_gid = c.team_gid  left join crm_mst_callresponse e on e.callresponse_gid=a.call_response  where c.employee_gid='" + employee_gid + "' " +
                    "   group by call_response";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Performencechart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Performencechart_list
                        {
                            callcount = dt["callcount"].ToString(),
                            call_response = dt["call_response"].ToString(),
                        });
                        values.Performencechart_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerNewSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
                    "concat_ws(' / ',d.region_name,b.leadbank_city,h.source_name) as" +
                    " region_name, a.internal_notes," +
                    " concat_ws(' / ',f.user_firstname,F.user_code) AS assigned_to , i.department_name, " +
                    "concat_ws(' ',y.user_firstname,y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  a.leadstage_gid='1' and a.leadbank_gid not in ( select leadbank_gid from crm_trn_tcalllog) and g.main_contact ='Y' and " +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {

                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            notes_count = lsCode,
                            appointment_gid = dt["appointment_gid"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerFollowSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat_ws('/',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
                    " concat_ws('/',d.region_name,b.leadbank_city,h.source_name) as" +
                    " region_name, a.internal_notes," +
                    " concat_ws(' / ',f.user_firstname,F.user_code) AS assigned_to , i.department_name, " +
                    " concat_ws(' ',y.user_firstname,y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid" +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  a.leadstage_gid='2' and g.main_contact ='Y' and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            notes_count = lsCode,
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerProspectSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
                    " concat_ws(' / ',d.region_name,b.leadbank_city,h.source_name) as" +
                    " region_name, a.internal_notes," +
                    "concat_ws(' / ',f.user_firstname,F.user_code) AS assigned_to , i.department_name, " +
                    " concat_ws(' ',y.user_firstname,y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  a.leadstage_gid='7' and g.main_contact ='Y' and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            notes_count = lsCode,
                            appointment_gid = dt["appointment_gid"].ToString(),
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerDropSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
                    " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as" +
                    " region_name, a.internal_notes," +
                    " CONCAT(f.user_firstname,'/',F.user_code) AS assigned_to , i.department_name, " +
                    " concat(y.user_firstname,' ',y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks" +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  a.leadstage_gid='5' and g.main_contact ='Y' and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            notes_count = lsCode,
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerPendingCallsSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
                    " concat_ws(' / ',d.region_name,b.leadbank_city,h.source_name) as" +
                    " region_name, a.internal_notes," +
                    "concat_ws(' / ',f.user_firstname,F.user_code) AS assigned_to , i.department_name, " +
                    " concat_ws('  ',y.user_firstname,y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  a.leadstage_gid='1' and a.leadbank_gid in(select leadbank_gid from crm_trn_tcalllog) and g.main_contact ='Y' and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            notes_count = lsCode,
                            appointment_gid = dt["appointment_gid"].ToString(),
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerScheduledSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
                    " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as" +
                    " region_name,a.internal_notes," +
                    " CONCAT(f.user_firstname,'/',F.user_code) AS assigned_to , i.department_name, " +
                    " concat(y.user_firstname,' ',y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  a.leadbank_gid in( select leadbank_gid from crm_trn_tschedulelog where schedule_date=CURDATE()) and g.main_contact ='Y' and " +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null and a.leadstage_gid != '5' group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            notes_count = lsCode,
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerLaspsedLeadSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
                    " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as" +
                    " region_name, a.internal_notes," +
                    " CONCAT(f.user_firstname,'/',F.user_code) AS assigned_to , i.department_name, " +
                    " concat(y.user_firstname,' ',y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  DATEDIFF(NOW(), a.created_date) > 10 AND a.leadstage_gid = '1'  and g.main_contact ='Y' and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            notes_count = lsCode,
                            appointment_gid = dt["appointment_gid"].ToString(),
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetTelecallerManagerLongestLeadSummary(string employee_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = " Select a.leadbank_gid,a.lead2campaign_gid,a.assign_to, j.branch_name,CONCAT(k.campaign_title) as campaign_title,k.campaign_prefix,b.leadbank_name, b.customer_type," +
                    " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
                    " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as" +
                    " region_name, a.internal_notes," +
                    " CONCAT(f.user_firstname,'/',F.user_code) AS assigned_to , i.department_name, " +
                    " concat(y.user_firstname,' ',y.user_lastname)As created_by,z.leadstage_name," +
                    " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,b.remarks,CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                    " From crm_trn_ttelelead2campaign a left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where  DATEDIFF(NOW(), a.created_date) > 10 AND a.leadstage_gid <= 2   and" +
                    " a.campaign_gid in (select team_gid  from cmn_trn_tmanagerprivilege where employee_gid = '" + employee_gid + "' ) " +
                    " and b.leadbank_name is not null and g.main_contact ='Y' group by b.leadbank_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallermanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new telecallermanager_totallists
                        {
                            assignedto_gid = dt["assign_to"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            assigned_to = dt["assigned_to"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            //followup = dt["followup"].ToString(),
                            //NewPending = dt["NewPending"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                            //closed = dt["closed"].ToString(),

                        });
                        values.telecallermanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostTeleMoveToTransfer(string user_gid, telecampaigntransfer_list values)
        {
            try
            {

                msSQL = " update crm_trn_ttelelead2campaign Set " +
                 " assign_to = '" + values.team_member + "'," +
                 " campaign_gid = '" + values.team_name + "'" +
                 " where leadbank_gid = '" + values.leadbank_gid + "'and" +
                 " assign_to = '" + values.assignedto_gid + "' and " +
                 " lead2campaign_gid='" + values.lead2campaign_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msSQL = "update crm_trn_tappointment set assign_to ='" + values.team_member + "' where  leadbank_gid = '" + values.leadbank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    values.status = true;
                    values.message = "Lead Transfer Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Lead Transfer";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Transfer Lead!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTelecallerCallerTeamlist( MdlTelecallerManager values)
        {
            try
            {
                msSQL = " SELECT campaign_gid, concat(campaign_prefix,' / ',campaign_title) as campaign_title FROM crm_trn_tteleteam Order by campaign_title asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<telecallerteam_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new telecallerteam_list
                        {
                            campaign_title = dt["campaign_title"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                          


                        });
                        values.telecallerteam_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTelecallerCallerEmployeelist(string team_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = "select a.employee_gid, concat(c.user_firstname, ' ',c.user_lastname)AS user_name from crm_trn_tteleteam2employee a " +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                        " where a.campaign_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<teleemployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new teleemployee_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),



                        });
                        values.teleemployee_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaTeleLeadMoveToBin(Telecallerbin_list values)
        {

            try
            {
                msSQL = "  update crm_trn_ttelelead2campaign set leadstage_gid='5',drop_remarks = '" + values.drop_remarks + "'  where leadbank_gid='" + values.leadbank_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " DELETE FROM crm_trn_tschedulelog WHERE leadbank_gid ='" + values.leadbank_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Lead moved to Drop successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Lead moved to Drop";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********"
                            + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Lead moved to Drop";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" 
                        + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Moving Lead To Bin!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTelecallerDropRemarks(string leadbank_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = "select drop_remarks from crm_trn_ttelelead2campaign where leadbank_gid='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Telecallerbin_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Telecallerbin_list
                        {
                            drop_remark1 = dt["drop_remarks"].ToString(),
                            

                        });
                        values.Telecallerbin_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Campaign Manager Drop Remarks!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLeadNoteDetails(string leadbank_gid, MdlTelecallerManager values)
        {
            try
            {
                msSQL = "select s_no,internal_notes,leadbank_gid,CONCAT(b.user_firstname,'/',b.user_code) AS created_by," +
                    "date_format(a.created_date,'%d-%m-%Y')  as created_date from crm_trn_tleadnotes a" +
                    " left join adm_mst_tuser b on a.created_by=b.user_gid where a.leadbank_gid='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLeadNoteDetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLeadNoteDetails_list
                        {
                            s_no = dt["s_no"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),


                        });
                        values.GetLeadNoteDetails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Campaign Manager Drop Remarks!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetSchedulelogsummary(string leadbank_gid, MdlMarketingmanager values)
        {
            try
            {

                msSQL = "SELECT CASE WHEN a.log_type = 'Schedule' THEN REPLACE(" +
                " CONCAT('Schedule Remark: ', b.schedule_remarks, '<br />'),'<br />','') END AS log_details," +
                " CASE WHEN a.log_type = 'Schedule' THEN CASE " +
                " WHEN e.schedule_type = 'Call' THEN CONCAT('Call Scheduled On', ' ', DATE_FORMAT(e.schedule_date, '%d-%m-%Y'),',',TIME_FORMAT(b.schedule_time, '%h:%i %p'))" +
                " WHEN e.schedule_type = 'Meeting' THEN CONCAT('Meeting Scheduled On', ' ', DATE_FORMAT(e.schedule_date, '%d-%m-%Y'),',',TIME_FORMAT(b.schedule_time, '%h:%i %p'))" +
                " WHEN e.schedule_type = 'Mail Log' THEN CONCAT('Mail Scheduled On', ' ', DATE_FORMAT(e.schedule_date, '%d-%m-%Y'),',',TIME_FORMAT(b.schedule_time, '%h:%i %p'))" +
                " END END AS log_legend, a.leadbank_gid FROM crm_trn_tlog a" +
                " LEFT JOIN crm_trn_tschedulelog b ON b.log_gid = a.log_gid" +
                " LEFT JOIN crm_trn_tschedulelog e ON e.log_gid = a.log_gid" +
                " LEFT JOIN crm_trn_tcalllog c ON c.log_gid = a.log_gid" +
                " LEFT JOIN crm_trn_tfieldlog d ON d.log_gid = a.log_gid" +
                " WHERE a.leadbank_gid = '" + leadbank_gid + "' AND c.log_gid IS NULL AND d.log_gid IS NULL ORDER BY a.log_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<schedulesummary_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new schedulesummary_list1
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            log_details = dt["log_details"].ToString(),
                            log_legend = dt["log_legend"].ToString(),

                        });
                        values.schedulesummary_list1 = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Schedule Log Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

    }
}