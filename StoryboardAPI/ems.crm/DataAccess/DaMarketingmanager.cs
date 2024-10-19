﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using RestSharp;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.ComponentModel;
using ems.system.Models;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;



namespace ems.crm.DataAccess
{
    public class DaMarketingmanager
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsentity_name, lsemployee_gid, lsuser_gid, msGetappointmentGid, msGetsheduleGid;

        // Module Master Summary
        public void DaGetMarketingManagerSummary(string user_gid, MdlMarketingmanager values)
        {

            try
            {
                 
                if (user_gid != null && user_gid != "")
                {
                    msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
                    lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
                }
                else
                {
                    lsemployee_gid = null;

                }

                msSQL = "SELECT a.campaign_gid,a.campaign_title,a.campaign_prefix,a.campaign_location,c.branch_name, " +
         " (SELECT COUNT(x.employee_gid) FROM crm_trn_tcampaign2employee x WHERE x.campaign_gid = a.campaign_gid) as employeecount, " +
         " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE x.campaign_gid = a.campaign_gid and x.campaign_gid = a.campaign_gid ) as assigned_leads, " +
         " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE x.leadstage_gid = '1' and x.campaign_gid = a.campaign_gid and x.campaign_gid = a.campaign_gid) as newleads, " +
         " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE x.leadstage_gid = '4' and x.campaign_gid = a.campaign_gid) as potential, " +
         " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE x.leadstage_gid = '3' and x.campaign_gid = a.campaign_gid) as prospect, " +
         " (SELECT COUNT(g.schedulelog_gid) FROM crm_trn_tschedulelog g LEFT JOIN crm_trn_tappointment f ON g.leadbank_gid = f.leadbank_gid WHERE g.schedule_type = 'Meeting' AND g.schedule_date >= CURDATE() AND f.campaign_gid = a.campaign_gid) as upcoming, " +
         " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE(x.leadstage_gid = '5') and x.campaign_gid = a.campaign_gid ) as drop_status, " +
         " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE(x.leadstage_gid = '6') and x.campaign_gid = a.campaign_gid) as customer, " +
         " l.lapsed_count,l.longest_count, " +
         " (SELECT COUNT(c.salesorder_gid) FROM crm_trn_tappointment a LEFT JOIN hrm_mst_temployee y ON y.employee_gid = a.assign_to " +
         " LEFT JOIN crm_trn_tleadbank b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN crm_trn_tcampaign k ON a.campaign_gid = k.campaign_gid  " +
         " LEFT JOIN cmn_trn_tmanagerprivilege m ON m.team_gid = k.campaign_gid LEFT JOIN smr_trn_tsalesorder c ON b.customer_gid = c.customer_gid " +
         " WHERE MONTH(c.salesorder_date) = MONTH(CURDATE()) AND YEAR(c.salesorder_date) = YEAR(CURDATE()) AND m.employee_gid = '" + lsemployee_gid + "' " +
         " AND k.campaign_title = a.campaign_title) as mtd_count, " +
         " (SELECT COUNT(c.salesorder_gid) FROM crm_trn_tappointment a LEFT JOIN hrm_mst_temployee y ON y.employee_gid = a.assign_to " +
         " LEFT JOIN crm_trn_tleadbank b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN crm_trn_tcampaign k ON a.campaign_gid = k.campaign_gid " +
         " LEFT JOIN cmn_trn_tmanagerprivilege m ON m.team_gid = k.campaign_gid LEFT JOIN smr_trn_tsalesorder c ON b.customer_gid = c.customer_gid " +
         " WHERE  YEAR(c.salesorder_date) = YEAR(CURDATE()) AND m.employee_gid = '" + lsemployee_gid + "' AND k.campaign_title = a.campaign_title) as ytd_count  " +
         " FROM crm_trn_tcampaign a LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid " +
         " LEFT JOIN(SELECT k.campaign_gid, SUM(CASE WHEN DATEDIFF(NOW(), x.created_date) > 10 AND x.leadstage_gid = '1' THEN 1 ELSE 0 END) as lapsed_count, " +
         " SUM(CASE WHEN DATEDIFF(NOW(), x.created_date) > 10 AND x.leadstage_gid <= 4 THEN 1 ELSE 0 END) as longest_count " +
         " FROM crm_trn_tappointment x LEFT JOIN crm_trn_tcampaign k ON x.campaign_gid = k.campaign_gid " +
         " LEFT JOIN cmn_trn_tmanagerprivilege m ON m.team_gid = k.campaign_gid WHERE m.employee_gid = '" + lsemployee_gid + "' AND x.leadbank_gid NOT IN( " +
         " SELECT leadbank_gid FROM crm_trn_tappointment GROUP BY leadbank_gid HAVING COUNT(*) > 1) " +
         " GROUP BY k.campaign_gid ) l ON a.campaign_gid = l.campaign_gid WHERE a.campaign_gid IN(SELECT team_gid FROM cmn_trn_tmanagerprivilege " +
         " WHERE employee_gid = '" + lsemployee_gid + "') GROUP BY a.campaign_gid, a.campaign_title,a.campaign_prefix, a.campaign_location, c.branch_name ORDER BY a.campaign_gid DESC";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<marketingmanager_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new marketingmanager_lists
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            campaign_location = dt["campaign_location"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            employeecount = dt["employeecount"].ToString(),
                            assigned_leads = dt["assigned_leads"].ToString(),
                            Lapsed_Leads = dt["lapsed_count"].ToString(),
                            Longest_Leads = dt["longest_count"].ToString(),
                            newleads = dt["newleads"].ToString(),
                            visit = dt["potential"].ToString(),
                            prospect = dt["prospect"].ToString(),
                            drop_status = dt["drop_status"].ToString(),
                            customer = dt["customer"].ToString(),
                            upcoming = dt["upcoming"].ToString(),
                            mtd = dt["mtd_count"].ToString(),
                            ytd = dt["ytd_count"].ToString(),

                        });
                        values.marketingmanager_lists = getModuleList;
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

        //Total tile count 
        public void DaGetTotaltilecount(string user_gid, MdlMarketingmanager values)
        {
            try
            {
                 
                if (user_gid != null && user_gid != "")
                {
                    msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
                    lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
                }
                else
                {
                    lsemployee_gid = null;

                }
                msSQL = "SELECT FORMAT(SUM(potential_value),2) as potential_value from crm_trn_tleadbank";
                string potential_value = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT COALESCE(SUM(a.assigned_leads), 0) as total_assigned_leads, COALESCE(SUM(a.newleads), 0) as total_newleads, " +
                        " COALESCE(SUM(a.potential), 0) as total_potential,COALESCE(SUM(a.prospect), 0) as total_prospect, " +
                        " COALESCE(SUM(a.upcoming), 0) as total_upcoming,COALESCE(SUM(a.drop_status), 0) as total_drop_status,COALESCE(SUM(a.customer), 0) as total_customer, " +
                        " COALESCE(SUM(a.mtd_count), 0) as total_mtd,COALESCE(SUM(a.ytd_count), 0) as total_ytd,COALESCE(SUM(a.Lapsed_lead), 0) as total_Lapsed_lead, " +
                        " COALESCE(SUM(a.Longest_lead), 0) as total_Longest_lead FROM(SELECT a.campaign_gid, a.campaign_title, a.campaign_location, c.branch_name, " +
                        " (SELECT COUNT(x.appointment_gid) as total FROM crm_trn_tappointment x WHERE x.campaign_gid = a.campaign_gid and(x.leadstage_gid != '5') and(x.leadstage_gid != '6') ) as assigned_leads, " +
                        " (SELECT COUNT(x.appointment_gid) as new FROM crm_trn_tappointment x WHERE x.leadstage_gid = '1' and x.leadstage_gid is not null  and x.campaign_gid = a.campaign_gid ) as newleads, " +
                        " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE x.leadstage_gid = '4' and x.campaign_gid = a.campaign_gid) as potential, " +
                        " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE x.leadstage_gid = '3' and x.campaign_gid = a.campaign_gid) as prospect, " +
                        " (SELECT COUNT(g.schedulelog_gid) FROM crm_trn_tschedulelog g LEFT JOIN crm_trn_tappointment f ON g.leadbank_gid = f.leadbank_gid WHERE g.schedule_type = 'Meeting' AND g.schedule_date >= CURDATE() AND f.campaign_gid = a.campaign_gid) as upcoming, " +
                        " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE(x.leadstage_gid = '5') and x.campaign_gid = a.campaign_gid ) as drop_status, " +
                        " (SELECT COUNT(x.appointment_gid) FROM crm_trn_tappointment x WHERE(x.leadstage_gid = '6') and x.campaign_gid = a.campaign_gid) as customer, " +
                        " (SELECT COUNT(c.salesorder_gid) FROM crm_trn_tappointment a LEFT JOIN hrm_mst_temployee y ON y.employee_gid = a.assign_to LEFT JOIN crm_trn_tleadbank b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN crm_trn_tcampaign k ON a.campaign_gid = k.campaign_gid  LEFT JOIN cmn_trn_tmanagerprivilege m ON m.team_gid = k.campaign_gid LEFT JOIN smr_trn_tsalesorder c ON b.leadbank_gid = c.customer_gid WHERE MONTH(c.salesorder_date) = MONTH(CURDATE()) AND YEAR(c.salesorder_date) = YEAR(CURDATE()) AND m.employee_gid = '" + lsemployee_gid + "' AND k.campaign_title = a.campaign_title) as mtd_count, " +
                        " (SELECT COUNT(c.salesorder_gid) FROM crm_trn_tappointment a LEFT JOIN hrm_mst_temployee y ON y.employee_gid = a.assign_to LEFT JOIN crm_trn_tleadbank b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN crm_trn_tcampaign k ON a.campaign_gid = k.campaign_gid LEFT JOIN cmn_trn_tmanagerprivilege m ON m.team_gid = k.campaign_gid LEFT JOIN smr_trn_tsalesorder c ON b.leadbank_gid = c.customer_gid WHERE YEAR(c.salesorder_date) = YEAR(CURDATE()) AND m.employee_gid = '" + lsemployee_gid + "' AND k.campaign_title = a.campaign_title) as ytd_count, " +
                        " (SELECT COUNT(*) FROM crm_trn_tappointment a left join crm_trn_tleadbank b on b.leadbank_gid = a.leadbank_gid LEFT JOIN crm_trn_tleadbankcontact g ON g.leadbank_gid = b.leadbank_gid WHERE DATEDIFF(NOW(), a.created_date) > 7 and a.leadstage_gid = '1'and g.status = 'Y' AND g.main_contact = 'Y') as Lapsed_lead, " +
                        " (SELECT COUNT(*) FROM crm_trn_tappointment a left join crm_trn_tleadbank b on b.leadbank_gid = a.leadbank_gid LEFT JOIN crm_trn_tleadbankcontact g ON g.leadbank_gid = b.leadbank_gid WHERE DATEDIFF(NOW(), a.created_date) > 7 and a.leadstage_gid <= 4 and g.status = 'Y' AND g.main_contact = 'Y') as Longest_lead " +
                        " FROM crm_trn_tcampaign a LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid WHERE a.campaign_gid IN(SELECT team_gid FROM cmn_trn_tmanagerprivilege WHERE employee_gid = '" + lsemployee_gid + "') " +
                        " GROUP BY a.campaign_gid ORDER BY a.campaign_gid DESC) a ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<totaltilecount_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new totaltilecount_lists
                        {
                            total_assignedleads = dt["total_assigned_leads"].ToString(),
                            total_LapsedLeads = dt["total_Lapsed_lead"].ToString(),
                            total_LongestLeads = dt["total_Longest_lead"].ToString(),
                            total_newleads = dt["total_newleads"].ToString(),
                            total_potential = dt["total_potential"].ToString(),
                            total_prospect = dt["total_prospect"].ToString(),
                            total_dropstatus = dt["total_drop_status"].ToString(),
                            total_customer = dt["total_customer"].ToString(),
                            total_upcoming = dt["total_upcoming"].ToString(),
                            total_mtd = dt["total_mtd"].ToString(),
                            total_ytd = dt["total_ytd"].ToString(),
                            potential_value = potential_value,

                        });
                        values.totaltilecount_lists = getModuleList;
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

        public void DaGetteamcount( string employee_gid,MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = " SELECT COUNT(DISTINCT campaign_title) AS team_count" +
               " FROM crm_trn_tcampaign a LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid" +
               " WHERE a.campaign_gid IN (SELECT team_gid FROM cmn_trn_tmanagerprivilege WHERE employee_gid = '" + employee_gid + "');";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<teamdetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new teamdetails
                        {
                            team_count = dt["team_count"].ToString(),
                        });
                        values.teamdetails = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Total Lapsed and Longest lead count
        public void DaTotallapsedlongest(string employee_gid, MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = " SELECT COALESCE(SUM(CASE WHEN DATEDIFF(NOW(), a.created_date) > 10 AND a.leadstage_gid = '1' THEN 1 ELSE 0 END),0) as lapsed_count," +
                   " COALESCE(SUM(CASE WHEN DATEDIFF(NOW(), a.created_date) > 10 AND a.leadstage_gid <= 4 THEN 1 ELSE 0 END),0) as longest_count" +
                   " FROM crm_trn_tlead2campaign a LEFT JOIN crm_trn_tcampaign k ON a.campaign_gid = k.campaign_gid " +
                   " LEFT JOIN cmn_trn_tmanagerprivilege m ON m.team_gid = k.campaign_gid" +
                   " WHERE m.employee_gid = '" + employee_gid + "' AND a.leadbank_gid NOT IN (" +
                   " SELECT leadbank_gid FROM crm_trn_tlead2campaign GROUP BY leadbank_gid HAVING COUNT(*) > 1)";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<totallapsedlongest>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new totallapsedlongest
                        {
                            total_lapsedcount = dt["lapsed_count"].ToString(),
                            total_longestcount = dt["longest_count"].ToString(),

                        });
                        values.totallapsedlongest = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Total Count of Lapsed and Longest Lead!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Schedulelog summary
        public void DaGetSchedulelogsummary(string appointment_gid, MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = "select log_remarks,log_date,schedule_status,postponed_date from crm_trn_topportunitylog where appointment_gid = '" + appointment_gid + "' and log_type = 'Opportunity'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<schedulesummary_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new schedulesummary_list1
                        {
                            log_details = dt["log_remarks"].ToString(),
                            log_legend = dt["log_date"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            postponed_date = dt["postponed_date"].ToString(),

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


        //Marketing Manager Total summary
        public void DaGetMarketingManagerTotalSummary(string employee_gid, MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = " Select  a.appointment_gid,b.potential_value, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,u.branch_name,concat(o.user_firstname,'/',o.user_code) as assign_to, a.drop_remarks,b.leadbank_name," +
                " k.campaign_title,concat(i.user_firstname,'/',i.user_code) as created_by,concat(k.campaign_title) as emp_team,k.campaign_prefix,b.customer_type,(y.employee_gid) as assignedto_gid," +
                " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details,concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name," +
                    " a.internal_notes,z.leadstage_name From crm_trn_tappointment a" +
                    " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                    " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                    " left join hrm_mst_temployee y on y.employee_gid = a.assign_to " +
                    " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
                    " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
                    " left join hrm_mst_temployee l on l.employee_gid = a.created_by " +
                    " left join adm_mst_tuser i on  i.user_gid = l.user_gid" +
                    " where g.status='Y' and g.main_contact='Y' and m.employee_gid = '" + employee_gid + "' and a.leadstage_gid != 5 and a.leadstage_gid != 6 ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<marketingmanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new marketingmanager_totallists
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            drop_remarks = dt["drop_remarks"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),


                        });

                        values.marketingmanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Total Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMarketingManagerteamviewSummary(string campaign_gid,string employee_gid, MdlMarketingmanager values)
        {
            try
            {
                msSQL = " SELECT FORMAT(SUM(a.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                        " left join crm_trn_tlead2campaign b on a.leadbank_gid = b.leadbank_gid " +
                        "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                        " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                        " left join cmn_trn_tmanagerprivilege d on d.team_gid = b.campaign_gid " +
                        " where g.status='Y' and g.main_contact='Y' and d.employee_gid = '" + employee_gid + "' and b.campaign_gid = '" + campaign_gid + "'";

                        string potential_value_count = objdbconn.GetExecuteScalar(msSQL);


                msSQL = "Select  a.appointment_gid,b.potential_value, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,u.branch_name,concat(o.user_firstname,'/',o.user_code) as assign_to, a.drop_remarks,b.leadbank_name, " +
                        " k.campaign_title,concat(i.user_firstname,'/',i.user_code) as created_by,concat(k.campaign_title) as emp_team,k.campaign_prefix,b.customer_type,(y.employee_gid) as assignedto_gid, " +
                        " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details,concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name, " +
                        " a.internal_notes,z.leadstage_name From crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid  " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid  " +
                        " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                        " left join crm_mst_tsource h on b.source_gid=h.source_gid  " +
                        " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid  " +
                        " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid  " +
                        " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid  " +
                        " left join hrm_mst_temployee y on y.employee_gid = a.assign_to  " +
                        " left join adm_mst_tuser o on o.user_gid = y.user_gid " +
                        " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid " +
                        " left join hrm_mst_temployee l on l.employee_gid = a.created_by  " +
                        " left join adm_mst_tuser i on  i.user_gid = l.user_gid " +
                        " where g.status='Y' and g.main_contact='Y' and m.employee_gid = '" + employee_gid + "' and a.campaign_gid = '" + campaign_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<marketingmanager_totallists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new marketingmanager_totallists
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            drop_remarks = dt["drop_remarks"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            potential_value_count = potential_value_count


                        }) ;

                        values.marketingmanager_totallists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Total Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Marketing Manager Lapsed Lead summary
        public void DaGetMarketingManagerLapsedSummary(string employee_gid, MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = " SELECT a.appointment_gid,b.potential_value,a.leadbank_gid,a.campaign_gid,g.leadbankcontact_gid,u.branch_name,concat(o.user_firstname,'/',o.user_code) as assign_to,b.leadbank_name," +
                   " DATE_FORMAT(a.created_date, '%d-%m-%y') AS created_date,y.employee_gid AS assignedto_gid,k.campaign_title,k.campaign_prefix,concat(i.user_firstname,'/',i.user_code) AS created_by," +
                   " CONCAT(k.campaign_title) AS emp_team,b.customer_type,CONCAT(DATE_FORMAT(a.created_date, '%d-%m-%y'), ' - ', DATEDIFF(NOW()," +
                   " a.created_date), ' days') AS lapsed_count,CONCAT(g.leadbankcontact_name, ' / ', g.mobile, ' / ', g.email) AS contact_details," +
                   " CONCAT(d.region_name, '/', b.leadbank_city, '/', b.leadbank_state, '/', h.source_name) AS region_name,a.internal_notes,b.remarks AS internal_notes,z.leadstage_name" +
                   " FROM crm_trn_tappointment a " +
                   " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                   " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                   " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                   " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                   " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                   " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
                   " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                   " left join hrm_mst_temployee y on y.employee_gid = a.assign_to " +
                   " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
                   " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
                   " left join hrm_mst_temployee l on l.employee_gid = a.created_by " +
                   " left join adm_mst_tuser i on  i.user_gid = l.user_gid" +
                   " where g.status='Y' and g.main_contact='Y' and  DATEDIFF(NOW(), a.created_date) > 10 and a.leadstage_gid='1' " +
                   " and m.employee_gid = '" + employee_gid + "' AND a.leadbank_gid NOT IN " +
                   " ( SELECT leadbank_gid FROM crm_trn_tlead2campaign GROUP BY leadbank_gid HAVING COUNT(*) > 1 )";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<marketingmanager_lapsedlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new marketingmanager_lapsedlists
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            lapsed_count = dt["lapsed_count"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),


                        });

                        values.marketingmanager_lapsedlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Lapsed Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Marketing Manager Longest Lead summary
        public void DaGetMarketingManagerLongestSummary(string employee_gid, MdlMarketingmanager values)
        {

            try
            {
                 
                msSQL = " Select  a.appointment_gid,b.potential_value, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,u.branch_name,concat(o.user_firstname,'/',o.user_code) as assign_to,b.leadbank_name,DATE_FORMAT(a.created_date, '%d-%m-%y') as created_date,(y.employee_gid) as assignedto_gid," +
                   " k.campaign_title,concat(i.user_firstname,'/',i.user_code)  as created_by,concat(k.campaign_title) as emp_team,k.campaign_prefix,b.customer_type,CONCAT(DATE_FORMAT(a.created_date, '%d-%m-%y'), ' - ', DATEDIFF(NOW(), a.created_date), ' days') as lapsed_count," +
                   " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details,concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name," +
                   " a.internal_notes,z.leadstage_name From crm_trn_tappointment a" +
                   " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                   " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                   " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                   " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                   " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                   " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
                   " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                   " left join hrm_mst_temployee y on y.employee_gid = a.assign_to " +
                   " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
                   " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
                   " left join hrm_mst_temployee l on l.employee_gid = a.created_by " +
                   " left join adm_mst_tuser i on  i.user_gid = l.user_gid" +
                   " where g.status='Y' and g.main_contact='Y' and  DATEDIFF(NOW(), a.created_date) > 10 and a.leadstage_gid <= 4" +
                   " and m.employee_gid = '" + employee_gid + "' AND a.leadbank_gid NOT IN " +
                   " ( SELECT leadbank_gid FROM crm_trn_tlead2campaign GROUP BY leadbank_gid HAVING COUNT(*) > 1 )";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<marketingmanager_longestlists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new marketingmanager_longestlists
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            lapsed_count = dt["lapsed_count"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),

                        });

                        values.marketingmanager_longestlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Longest Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        //Ends here


        public void DaGetManagerSummaryDetailTable(string campaign_gid, MdlMarketingmanager values)
        {

            try
            {
                 
                msSQL = "  select distinct a.campaign_gid,e.department_name," +
                       " a.employee_gid as assign_to,concat(c.user_firstname, '-', c.user_code) as user, " +
                       " ( SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                       " where x.assign_to = a.employee_gid and x.campaign_gid = a.campaign_gid) as total, " +
                       " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                       " where x.assign_to = a.employee_gid and (x.leadstage_gid ='1' or x.leadstage_gid is null) and x.campaign_gid = a.campaign_gid) as newleads, " +
                       " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                       " where x.assign_to = a.employee_gid and x.leadstage_gid ='2' and x.campaign_gid = a.campaign_gid) as followup, " +
                       " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                        " where x.assign_to = a.employee_gid and x.leadstage_gid ='4' and x.campaign_gid = a.campaign_gid) as potential, " +
                       " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                       " where x.assign_to = a.employee_gid and x.leadstage_gid ='3' and x.campaign_gid = a.campaign_gid) as prospect, " +
                       " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                       " where x.assign_to = a.employee_gid and x.leadstage_gid ='5' and x.campaign_gid = a.campaign_gid) as drop_status, " +
                       " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tlead2campaign x " +
                       " where x.assign_to = a.employee_gid and x.leadstage_gid ='6' and x.campaign_gid = a.campaign_gid) as customer " +
                       " from crm_trn_tcampaign2employee a" +
                       " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                       " left join adm_mst_tuser c on c.user_gid=b.user_gid " +
                       " left join hrm_mst_tdepartment e on b.department_gid=e.department_gid" +
                       " where a.campaign_gid= '" + campaign_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<managerDetailTable_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new managerDetailTable_lists
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            user = dt["user"].ToString(),
                            total = dt["total"].ToString(),
                            newleads = dt["newleads"].ToString(),
                            followup = dt["followup"].ToString(),
                            visit = dt["visit"].ToString(),
                            prospect = dt["prospect"].ToString(),
                            drop_status = dt["drop_status"].ToString(),
                            customer = dt["customer"].ToString(),

                        });
                        values.managerDetailTable_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Detail Table Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCampaignmanagerSummary(string campaign_gid, string assign_to, string stages, MdlMarketingmanager values)
        {

            try
            {
                 
                msSQL = " Select b.customer_type AS customer_type,b.potential_value,  b.leadbank_name,i.call_response," +
              " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
              " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name," +
              " b.remarks as internal_notes," +
              " concat(y.user_firstname,' ',y.user_lastname , '/',y.user_code)As created_by,z.leadstage_name," +
              " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid" +
              " From crm_trn_tlead2campaign a" +
              " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid        " +
              " left join crm_mst_tregion d on b.leadbank_region=d.region_gid           " +
              " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
              " left join crm_mst_tsource h on b.source_gid=h.source_gid                " +
              " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid          " +
              " left join hrm_mst_temployee x on a.created_by=x.employee_gid            " +
              " left join adm_mst_tuser y on x.user_gid=y.user_gid                      " +
              " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
              " left join crm_trn_tcalllog i on i.leadbank_gid=b.leadbank_gid " +
              " where a.assign_to = '" + assign_to + "' and g.main_contact ='Y' " +
              " a.campaign_gid = '" + campaign_gid + "'";
                if (stages == "leadiden")
                {

                    msSQL += "and a.leadstage_gid='1'";
                }
                else if (stages == "first")
                {
                    msSQL += "and a.leadstage_gid='2'";
                }
                else if (stages == "qualified")
                {
                    msSQL += "and a.leadstage_gid='3'";
                }
                else if (stages == "negotiation")
                {
                    msSQL += "and a.leadstage_gid='4'";
                }
                else if (stages == "oip")
                {
                    msSQL += "and a.leadstage_gid='6'";
                }

                msSQL += " order by b.leadbank_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCampaignmanagerSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCampaignmanagerSummary
                        {

                            leadbank_name = dt["leadbank_name"].ToString(),
                            call_response = dt["call_response"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            potential_value = dt["potential_value"].ToString(),


                        });
                        values.GetCampaignmanagerSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Campaign Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCampaignmanagerTeam(string campaign_gid, string assign_to, MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = " SELECT  (Select campaign_title from crm_trn_tcampaign where campaign_gid ='" + campaign_gid + "') AS campaign_title, " +
                   " (select concat(b.user_firstname, ' ', b.user_lastname) as user_firstname from hrm_mst_temployee a  " +
                   " inner join adm_mst_tuser b on a.user_gid = b.user_gid where employee_gid ='" + assign_to + "') AS user_firstname";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCampaignmanagerTeam>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCampaignmanagerTeam
                        {

                            campaign_title = dt["campaign_title"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),

                        });
                        values.GetCampaignmanagerTeam = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Campaign Manager Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //drop//
        public void DaGetCampaignMoveToBin(string user_gid, campaignbin_list values)
        {

            try
            {

                msSQL = "  update crm_trn_tappointment set leadstage_gid='5', drop_remarks='"+values.drop_remarks + "' where appointment_gid='" + values.appointment_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Lead moved to Drop successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Lead moved to Drop";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                        values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Moving Lead To Bin!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCampaignMoveToTransfer(string user_gid, campaigntransfer_list values)
        {
            try
            {
                 
                for (int i = 0; i < values.campaign_list.ToArray().Length; i++)
                {

                    msSQL = " update crm_trn_tlead2campaign Set " +
                            " assign_to = '" + values.team_member + "'," +
                            " campaign_gid = '" + values.team_name + "'" +
                            " where leadbank_gid = '" + values.campaign_list[i].leadbank_gid + "'and" +
                            " assign_to = '" + values.assign_user + "' and " +
                            " lead2campaign_gid='" + values.campaign_list[i].lead2campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Lead Transfered Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While  Transfering Lead";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Transfer Lead!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Marketing Manager Transfer Popup
        public void DaPostMoveToTransfer(string user_gid, campaigntransfer_list values)
        {
            try
            {
                 
                msSQL = " update crm_trn_tappointment Set " +
                 " assign_to = '" + values.team_member + "'," +
                 " campaign_gid = '" + values.team_name + "'" +
                 " where leadbank_gid = '" + values.leadbank_gid + "'and" +
                 " assign_to = '" + values.assignedto_gid + "' and " +
                 " appointment_gid='" + values.appointment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
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

        public void DaGetCampaignSchedule(string user_gid, campaignschedule_list values)
        {
            try
            {
                 
                for (int i = 0; i < values.campaign_list.ToArray().Length; i++)
                {

                    msGetGid = objcmnfunctions.GetMasterGID("BLGP");
                    msSQL = " insert into crm_trn_tlog ( " +
                            " log_gid, " +
                            " leadbank_gid, " +
                            " log_type, " +
                            " log_desc, " +
                            " log_by, " +
                            " reference_gid," +
                            " log_date ) " +
                            " values (  " +
                            " '" + msGetGid + "', " +
                            " '" + values.campaign_list[i].leadbank_gid + "', " +
                            " 'Schedule'," +
                            " '" + values.schedule_remarks + "', " +
                            " '" + user_gid + "'," +
                            " '" + values.campaign_list[i].lead2campaign_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "' )";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "' ";
                        objOdbcDataReader  = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader .HasRows)
                        {
                            lsemployee_gid = objOdbcDataReader ["employee_gid"].ToString();
                        }
                        msGetappointmentGid = objcmnfunctions.GetMasterGID("APMT");

                        string[] substrings = values.schedule_date.Split('-');
                        string schedule_date = substrings[2] + "-" + substrings[1] + "-" + substrings[0];

                        msSQL = " insert into cmn_trn_tscheduleappointments( " +
                          " appointment_gid, " +
                          " appointment_start, " +
                          " appointment_end, " +
                          " appointment_summary, " +
                          " appointment_description, " +
                          " appointment_from, " +
                          " employee_gid, " +
                          " created_date, " +
                          " created_by, " +
                          " reference_gid " +
                          " )values( " +
                          " '" + msGetappointmentGid + "', " +
                          " '" + schedule_date + " " + values.schedule_time + "', " +
                          " '" + schedule_date + " " + values.schedule_time + "', " +
                          " '" + values.schedule_type + values.schedule_remarks + "', " +
                          " '" + values.schedule_remarks + "', " +
                          " 'CRM LEAD', " +
                          " '" + lsemployee_gid + "', " +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                          " '" + user_gid + "', " +
                          " '" + msGetGid + "' ) ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msGetsheduleGid = objcmnfunctions.GetMasterGID("BSLC");
                            msSQL = " Insert into crm_trn_tschedulelog ( " +
                          " schedulelog_gid, " +
                          " leadbank_gid, " +
                          " schedule_date, " +
                          " schedule_type, " +
                          " schedule_remarks, " +
                          " schedule_status, " +
                          " schedule_time, " +
                          " status_flag," +
                          " created_by, " +
                          " reference_gid," +
                          " reference_gid," +
                          " log_gid, " +
                          " created_date ) " +
                          " Values ( " +
                          "'" + msGetsheduleGid + "'," +
                          "'" + values.campaign_list[i].leadbank_gid + "'," +
                          "'" + schedule_date + "'," +
                          "'" + values.schedule_type + "'," +
                          "'" + values.schedule_remarks + "'," +
                          " 'Pending'," +
                          " '" + values.schedule_time + "'," +
                          " 'N', " +
                          "'" + user_gid + "'," +
                          "'" + values.campaign_list[i].lead2campaign_gid + "'," +
                          "'" + values.assign_user + "'," +
                          "'" + msGetGid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Schedule";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Schedule";
                    }

                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Campaign Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        ////Marketing Manager Schedule 
        public void DaPostManagerSchedule(string user_gid, campaignschedule_list values)
        {

            try
            {
                msSQL = " insert into crm_trn_tOpportunitylog ( " +
                       " appointment_gid, " +
                       " log_type, " +
                       " log_date, " +
                       " log_remarks, " +
                       " schedule_status, " +
                       " created_by, " +
                       " created_date ) " +
                       " values (  " +
                       "'" + values.appointment_gid + "'," +
                       "'Opportunity'," +
                       "'" + values.schedule_date + "'," +
                       "'" + values.schedule_remarks + "'," +
                       "'Pending'," +
                       "'" + user_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                     mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Scheduled Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Scheduling";
                    }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Manager Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }

        public void DaGetTeamNamedropdown(string user_gid, MdlMarketingmanager values)
        {

            try
            {
                 
                msSQL = "  SELECT campaign_gid, concat(campaign_prefix,' / ',campaign_title) as campaign_title FROM crm_trn_tcampaign Order by campaign_title asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTeamNamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTeamNamedropdown
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),

                        });
                        values.GetTeamNamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Name Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }        
        }
        public void DaGetTeamEmployeedropdown(string team_gid, MdlMarketingmanager values)
        {
            try
            {
                 
                msSQL = " select a.employee_gid," +
             " concat(c.user_firstname, ' ',c.user_lastname)AS user_name" +
             " from crm_trn_tcampaign2employee a" +
             " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
             " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
             " where a.campaign_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTeamEmployeedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTeamEmployeedropdown
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                        });
                        values.GetTeamEmployeedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Employee Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }          
        }
        //upcoming//
        public void DaGetUpcomingManagerSummary(MdlMarketingmanager values, string employee_gid)
        {
            try
            {
                 
                msSQL = "SELECT f.assign_to,b.potential_value,g.campaign_title,k.branch_name,concat(l.user_firstname,'/',l.user_code) AS created_by,b.customer_type,CONCAT(g.campaign_title) AS emp_team,g.campaign_prefix, a.log_gid,a.schedulelog_gid,b.leadbank_gid,a.schedule_remarks,a.schedule_status,b.remarks," +
               " CASE WHEN DATE(a.schedule_date) = CURDATE() THEN 'Today' WHEN DATE(a.schedule_date) > CURDATE() THEN 'Upcoming'ELSE NULL END AS schedule_status1," +
               " CONCAT(DATE_FORMAT(a.schedule_date, '%d %b %Y'), ', ', TIME_FORMAT(a.schedule_time, '%h:%i%p')) AS schedule," +
               " CONCAT(c.leadbankcontact_name, ' / ', c.mobile, ' / ', c.email) AS Contact," +
               " b.leadbank_name, d.region_name,a.schedule_type,f.appointment_gid,c.leadbankcontact_gid " +
               " FROM crm_trn_tschedulelog a " +
               " INNER JOIN crm_trn_tleadbank b ON a.leadbank_gid = b.leadbank_gid" +
               " INNER JOIN crm_trn_tleadbankcontact c ON b.leadbank_gid = c.leadbank_gid" +
               " LEFT JOIN crm_mst_tregion d ON b.leadbank_region = d.region_gid" +
               " LEFT JOIN crm_mst_tsource e ON b.source_gid = e.source_gid" +
               " LEFT JOIN crm_trn_tappointment f ON b.leadbank_gid = f.leadbank_gid" +
               " LEFT JOIN crm_trn_tcampaign g ON g.campaign_gid = f.campaign_gid" +
               " left join cmn_trn_tmanagerprivilege m on m.team_gid=g.campaign_gid " +
               " LEFT JOIN hrm_mst_temployee i ON i.employee_gid = f.assign_to" +
               " LEFT JOIN hrm_mst_tbranch k ON k.branch_gid = i.branch_gid" +
               " LEFT JOIN adm_mst_tuser l ON l.user_gid = i.user_gid" +
               " WHERE  a.schedule_type = 'Meeting' AND a.schedule_date >= CURDATE()  " +
               "AND c.status = 'Y'  AND c.main_contact = 'Y' and m.employee_gid = '" + employee_gid + "' ORDER BY  b.leadbank_name ASC";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<upcoming>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new upcoming
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            assignedto_gid = dt["assign_to"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            schedule_remarks = dt["schedule_remarks"].ToString(),
                            schedule_status1 = dt["schedule_status1"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            schedule = dt["schedule"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),




                        });
                        values.upcoming = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Upcoming Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //New//
        public void DaGetNewManagerSummary(MdlMarketingmanager values, string emmployee_gid)
        {

            try
            {
                 
                msSQL = "select  a.appointment_gid,b.leadbank_name,b.potential_value, b.customer_type,k.campaign_title,k.campaign_prefix,b.customer_type, g.mobile,b.remarks,z.leadstage_name,u.branch_name," +
               " k.campaign_title,concat(i.user_firstname,'/',i.user_code)  as created_by,concat(k.campaign_title) as emp_team,concat(o.user_firstname,'/',o.user_code) as assign_to,(y.employee_gid) as assignedto_gid," +
               " concat(g.leadbankcontact_name, ' / ', g.mobile, ' / ', g.email) as Contact, a.internal_notes ," +
               " a.appointment_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid" +
               " from crm_trn_tappointment a" +
               " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
               " left join crm_mst_tregion d on b.leadbank_region = d.region_gid" +
               " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid" +
               " left join crm_mst_tsource h on b.source_gid = h.source_gid" +
               " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
               " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
               " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid" +
               " left join hrm_mst_temployee y on y.employee_gid = a.assign_to" +
               " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
               " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
               " left join hrm_mst_temployee l on l.employee_gid = a.created_by" +
               " left join adm_mst_tuser i on i.user_gid = l.user_gid" +
               " where " +
               "  (a.leadstage_gid = '1' or a.leadstage_gid is null)" +
               " and g.status = 'Y' and g.main_contact = 'Y' and m.employee_gid = '" + emmployee_gid + "'" ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<New>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new New
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),


                        });
                        values.New = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Prospect//
        public void DaGetProspectManagerSummary(MdlMarketingmanager values, string employee_gid)
        {

            try
            {
                 
                msSQL = "Select  a.appointment_gid,b.potential_value, b.leadbank_name,b.customer_type,b.remarks,z.leadstage_name,k.campaign_title,k.campaign_prefix, u.branch_name,concat(i.user_firstname,'/',i.user_code)  as created_by,concat(k.campaign_title) as emp_team," +
             " concat(g.leadbankcontact_name, ' / ', g.mobile, ' / ', g.email) as Contact,b.leadbank_gid, g.mobile,(y.employee_gid) as assignedto_gid,concat(o.user_firstname,'/',o.user_code) as assign_to," +
             " a.appointment_gid, a.leadbank_gid, a.campaign_gid,a.internal_notes, g.leadbankcontact_gid " +
             " From crm_trn_tappointment a" +
             " left join hrm_mst_temployee y on y.employee_gid = a.assign_to left join adm_mst_tuser o on o.user_gid = y.user_gid left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid left join hrm_mst_temployee l on l.employee_gid = a.created_by left join adm_mst_tuser i on i.user_gid = l.user_gid" +
             " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
             " left join crm_mst_tregion d on b.leadbank_region = d.region_gid" +
             " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid" +
             " left join crm_mst_tsource h on b.source_gid = h.source_gid" +
             " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
             " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
             " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid" +
             " where a.leadstage_gid = '3' and g.status = 'Y' and g.main_contact = 'Y'" +
             " and m.employee_gid = '" + employee_gid + "' order by b.leadbank_name asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Prospect>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Prospect
                        {

                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),


                        });
                        values.Prospect = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Prospect Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //potential//

        public void DaGetPotentialtManagerSummary(MdlMarketingmanager values, string employee_gid)
        {

            try
            {
                msSQL = " SELECT FORMAT(SUM(b.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                        " left join crm_trn_tappointment b on a.leadbank_gid = b.leadbank_gid " +
                        "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                        " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                        " left join cmn_trn_tmanagerprivilege d on d.team_gid = b.campaign_gid " +
                        " where g.status='Y' and g.main_contact='Y' and d.employee_gid = '" + employee_gid + "' and b.leadstage_gid = '4'";

                string potential_value_count = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select   a.appointment_gid,Format(a.potential_value,2)as potential_value,b.leadbank_name,b.customer_type,b.remarks,z.leadstage_name,  u.branch_name,concat(i.user_firstname,'/',i.user_code)  as created_by,concat(k.campaign_title) as emp_team," +
                   " concat(g.leadbankcontact_name, ' / ', g.mobile, ' / ', g.email) as Contact,b.leadbank_gid, g.mobile  ,z.leadstage_name,k.campaign_title,k.campaign_prefix, " +
                   " a.appointment_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid,a.internal_notes ,(y.employee_gid) as assignedto_gid,concat(o.user_firstname,'/',o.user_code) as assign_to" +
                   " From crm_trn_tappointment a " +
                   " left join hrm_mst_temployee y on y.employee_gid = a.assign_to left join adm_mst_tuser o on o.user_gid = y.user_gid left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid left join hrm_mst_temployee l on l.employee_gid = a.created_by left join adm_mst_tuser i on i.user_gid = l.user_gid " +
                   " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                   " left join crm_mst_tregion d on b.leadbank_region = d.region_gid " +
                   " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                   " left join crm_mst_tsource h on b.source_gid = h.source_gid " +
                   " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
                   " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
                   " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid" +
                   " where a.leadstage_gid = '4' and g.status = 'Y' and g.main_contact = 'Y' and m.employee_gid = '" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Potential>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Potential
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            potential_value_count = potential_value_count,


                        });
                        values.Potential = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Potential Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
        //customer//
        public void DaGetCustomerManagerSummary(MdlMarketingmanager values, string employee_gid)
        {
            try
            {
                msSQL = " SELECT FORMAT(SUM(b.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                       " left join crm_trn_tappointment b on a.leadbank_gid = b.leadbank_gid " +
                       "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                       " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                       " left join cmn_trn_tmanagerprivilege d on d.team_gid = c.campaign_gid " +
                       " where g.status='Y' and g.main_contact='Y' and d.employee_gid = '" + employee_gid + "' and b.leadstage_gid = '6'";
                string potential_value_count = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select  a.appointment_gid,Format(a.potential_value,2)as potential_value, b.leadbank_name, k.campaign_title,b.customer_type,b.leadbank_gid, g.mobile,b.remarks,u.branch_name,concat(i.user_firstname,'/',i.user_code)  as created_by,concat(k.campaign_title) as emp_team,k.campaign_prefix," +
                   " concat(g.leadbankcontact_name, ' / ', g.mobile, ' / ', g.email) as Contact,(y.employee_gid) as assignedto_gid,concat(o.user_firstname,'/',o.user_code) as assign_to," +
                   " z.leadstage_name,a.appointment_gid, a.leadbank_gid,a.internal_notes, a.campaign_gid, g.leadbankcontact_gid" +
                   " From crm_trn_tappointment a" +
                   "  left join hrm_mst_temployee y on y.employee_gid = a.assign_to" +
                   " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
                   " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
                   " left join hrm_mst_temployee l on l.employee_gid = a.created_by" +
                   " left join adm_mst_tuser i on i.user_gid = l.user_gid" +
                   " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                   " left join crm_mst_tregion d on b.leadbank_region = d.region_gid" +
                   " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid" +
                   " left join crm_mst_tsource h on b.source_gid = h.source_gid" +
                   " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
                   " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
                   " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid" +
                   " where(a.leadstage_gid = '6')" +
                   " and g.status = 'Y' and g.main_contact = 'Y'  and m.employee_gid = '" + employee_gid + "' order by b.leadbank_name asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Customer1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Customer1
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            potential_value_count = potential_value_count,


                        });
                        values.Customer1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Customer Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
        //drop//
        public void DaGetDropManagerSummary(MdlMarketingmanager values, string employee_gid)
        {

            try
            {
                 
                msSQL = "Select   a.appointment_gid,b.potential_value,b.leadbank_name,k.campaign_title,a.assign_to,b.customer_type,b.leadbank_gid, b.remarks,u.branch_name,concat(i.user_firstname,'/',i.user_code)  as created_by,concat(k.campaign_title) as emp_team,k.campaign_prefix," +
             " concat(g.leadbankcontact_name, ' / ', g.mobile, ' / ', g.email) as Contact,(y.employee_gid) as assignedto_gid,concat(o.user_firstname,'/',o.user_code) as leadassign_to," +
             " a.appointment_gid, a.leadbank_gid,z.leadstage_name,a.internal_notes, a.campaign_gid, g.leadbankcontact_gid" +
             " From crm_trn_tappointment a" +
             " left join hrm_mst_temployee y on y.employee_gid = a.assign_to" +
             " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
             " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
             " left join hrm_mst_temployee l on l.employee_gid = a.created_by" +
             " left join adm_mst_tuser i on i.user_gid = l.user_gid" +
             " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
             " left join crm_mst_tregion d on b.leadbank_region = d.region_gid" +
             " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid" +
             " left join crm_mst_tsource h on b.source_gid = h.source_gid" +
             " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
             " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
             " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid" +
             " where a.leadstage_gid = '5'" +
             " and g.status = 'Y' and g.main_contact = 'Y' and m.employee_gid = '" + employee_gid + "' order by b.leadbank_name asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Drop>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Drop
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            Contact = dt["Contact"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            assignedto_gid = dt["assignedto_gid"].ToString(),
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            leadassign_to = dt["leadassign_to"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),


                        });
                        values.Drop = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Drop Manager Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }          
        }
        //mtd//
        public void DaGetMTD(MdlMarketingmanager values , string employee_gid)
        {

            try
            {
                 
                //string currency = "INR";

                msSQL = " Select  a.appointment_gid,b.leadbank_gid,b.potential_value,c.salesorder_gid,a.campaign_gid, " +
                    " b.leadbank_name , k.campaign_title,b.customer_type,a.internal_notes,b.remarks," +
                    " concat(k.campaign_title) as emp_team,k.campaign_prefix,(y.employee_gid) as assignedto_gid,z.leadstage_name," +
                    " c.Grandtotal,c.salesorder_gid,c.so_type,c.invoice_flag,c.salesorder_status," +
                    " cast(concat(c.so_referenceno1, if(c.so_referencenumber<>'',concat(c.so_referencenumber),'') ) as char)" +
                    " as so_referenceno1, DATE_FORMAT(c.salesorder_date, '%d-%b-%Y') as salesorder_date" +
                    " From crm_trn_tappointment a" +
                    " left join hrm_mst_temployee y on y.employee_gid = a.assign_to" +
                    " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
                    " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
                    " left join hrm_mst_temployee l on l.employee_gid = a.created_by" +
                    " left join adm_mst_tuser i on i.user_gid = l.user_gid" +
                    " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
                    " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid" +
                    " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid " +
                    " left join smr_trn_tsalesorder c on b.customer_gid = c.customer_gid" +
                    " WHERE MONTH(c.salesorder_date) = MONTH(CURDATE())  AND YEAR(c.salesorder_date) = YEAR(CURDATE())" +
                    " and m.employee_gid = '" + employee_gid + "' ORDER BY  c.salesorder_date DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<M2D>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new M2D
                        {
                            customer_type = dt["customer_type"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["leadbank_name"].ToString(),
                            so_type = dt["so_type"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),

                        });
                        values.M2D = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }
        //ytd//
        public void DaGetYTD(MdlMarketingmanager values , string employee_gid)
        {

            try
            {
                 
                //string currency = "INR";

                msSQL = " Select  a.appointment_gid,b.potential_value,b.leadbank_gid,c.salesorder_gid,a.campaign_gid, " +
                    " b.leadbank_name , k.campaign_title,b.customer_type,a.internal_notes,b.remarks," +
                    " concat(k.campaign_title) as emp_team,k.campaign_prefix,(y.employee_gid) as assignedto_gid,z.leadstage_name," +
                    " c.Grandtotal,c.salesorder_gid,c.so_type,c.invoice_flag,c.salesorder_status," +
                    " cast(concat(c.so_referenceno1, if(c.so_referencenumber<>'',concat(c.so_referencenumber),'') ) as char)" +
                    " as so_referenceno1, DATE_FORMAT(c.salesorder_date, '%d-%b-%Y') as salesorder_date" +
                    " From crm_trn_tappointment a" +
                    " left join hrm_mst_temployee y on y.employee_gid = a.assign_to" +
                    " left join adm_mst_tuser o on o.user_gid = y.user_gid" +
                    " left join hrm_mst_tbranch u on u.branch_gid = y.branch_gid" +
                    " left join hrm_mst_temployee l on l.employee_gid = a.created_by" +
                    " left join adm_mst_tuser i on i.user_gid = l.user_gid" +
                    " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid" +
                    " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid" +
                    " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid" +
                    " left join crm_mst_tleadstage z on a.leadstage_gid = z.leadstage_gid " +
                    " left join smr_trn_tsalesorder c on b.customer_gid = c.customer_gid" +
                    " WHERE  YEAR(c.salesorder_date) = YEAR(CURDATE())" +
                    " and m.employee_gid = '" + employee_gid + "' ORDER BY  c.salesorder_date DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<M2D>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new M2D
                        {
                            customer_type = dt["customer_type"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["leadbank_name"].ToString(),
                            so_type = dt["so_type"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            emp_team = dt["emp_team"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),

                        });
                        values.M2D = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting YTD Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void Daleadchartcount(string user_gid,MdlMarketingmanager values)
        {
            try
            {
                if (user_gid != null && user_gid != "")
                {
                    msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
                    lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
                }
                else
                {
                    lsemployee_gid = null;

                }

                msSQL = " select date_format(a.created_date, '%b') as months,  " +
                         " sum(case when a.leadstage_gid = '1'  then 1 else 0 end) as new_leads,  " +
                         " sum(case when a.leadstage_gid = '3'  then 1 else 0 end) as prospect,  " +
                         " sum(case when a.leadstage_gid = '4'  then 1 else 0 end) as potentials,  " +
                         " sum(case when a.leadstage_gid = '6'  then 1 else 0 end) as order_count  " +
                         " from crm_trn_tappointment a " +
                         " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid " +
                         " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid  " +
                         " where a.created_date >= date_sub(curdate(), interval 6 month) and m.employee_gid = '" + lsemployee_gid + "' group by months order by a.created_date";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscount_list
                        {
                            Months = dt["Months"].ToString(),
                            new_leads = dt["new_leads"].ToString(),
                            potentialscount = dt["potentials"].ToString(),
                            prospectcount = dt["prospect"].ToString(),
                            orderscount = dt["order_count"].ToString(),
                        });
                        values.chartscount_list = getModuleList;
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

        //public void Daquotationchartcount(string user_gid, MdlMarketingmanager values)
        //{
        //    try
        //    {
        //        if (user_gid != null && user_gid != "")
        //        {
        //            msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
        //            lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
        //        }
        //        else
        //        {
        //            lsemployee_gid = null;

        //        }

        //        msSQL = " SELECT count(lead2campaign_gid) as potentials,DATE_FORMAT(created_date, '%b') AS Months" +
        //                " FROM crm_trn_tlead2campaign WHERE so_status <> 'Y' AND leadstage_gid = '4' AND assign_to = '" + lsemployee_gid + "' AND created_date >= DATE_SUB(CURRENT_DATE, INTERVAL 6 MONTH) group by months order by created_date";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<chartscount_list>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new chartscount_list
        //                {
        //                    potentialsmonth = dt["Months"].ToString(),
                            
        //                });
        //                values.chartscount_list = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting Marketing Manager Chart!";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
        //      ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }


        //}

        //public void Daenquirychartcount(string user_gid, MdlMarketingmanager values)
        //{
        //    try
        //    {
        //        if (user_gid != null && user_gid != "")
        //        {
        //            msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
        //            lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
        //        }
        //        else
        //        {
        //            lsemployee_gid = null;

        //        }

        //        msSQL = " SELECT count(lead2campaign_gid) as prospect,DATE_FORMAT(created_date, '%b') AS Months" +
        //                " FROM crm_trn_tlead2campaign WHERE so_status <> 'Y' AND leadstage_gid = '3' AND assign_to = '" + lsemployee_gid + "' AND created_date >= DATE_SUB(CURRENT_DATE, INTERVAL 6 MONTH) group by months order by created_date";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<chartscount_list>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new chartscount_list
        //                {
        //                    prospectmonth = dt["Months"].ToString(),
                            
        //                });
        //                values.chartscount_list = getModuleList;
        //            }
        //            dt_datatable.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting Marketing Manager Chart!";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
        //      ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }
        
        //}

        //public void Dasaleschartcount(string user_gid, MdlMarketingmanager values)
        //{
        //    try
        //    {
        //        if (user_gid != null && user_gid != "")
        //        {
        //            msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
        //            lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
        //        }
        //        else
        //        {
        //            lsemployee_gid = null;

        //        }

        //        msSQL = " SELECT count(lead2campaign_gid) as order_count,DATE_FORMAT(created_date, '%b') AS Months" +
        //                " FROM crm_trn_tlead2campaign WHERE so_status <> 'Y' AND leadstage_gid = '6' AND assign_to = '" + lsemployee_gid + "' AND created_date >= DATE_SUB(CURRENT_DATE, INTERVAL 6 MONTH) group by months order by created_date";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getModuleList = new List<chartscount_list>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new chartscount_list
        //                {
        //                    ordersmonth = dt["Months"].ToString(),
                            
        //                });
        //                values.chartscount_list = getModuleList;
        //            }
        //            dt_datatable.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting Marketing Manager Chart!";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
        //      ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //    }

        //}

        public void Dateamactivitysummary(string user_gid, MdlMarketingmanager values)
        {
            try
            {
                if (user_gid != null && user_gid != "")
                {
                    msSQL1 = " SELECT employee_gid FROM hrm_mst_temployee where user_gid='" + user_gid + "' ";
                    lsemployee_gid = objdbconn.GetExecuteScalar(msSQL1);
                }
                else
                {
                    lsemployee_gid = null;

                }
                msSQL = " select date_format(a.created_date, '%b') AS month_names,  " +
                        " count(case when a.leadstage_gid = '1' then a.appointment_gid end) as new_leads,  " +
                         " count(case when a.leadstage_gid = '3' then a.appointment_gid end) as prospect_lead, " +
                         " count(case when a.leadstage_gid = '4' then a.appointment_gid end) as potential_lead,  " +
                         " count(case when a.leadstage_gid = '6' then a.appointment_gid end) as order_lead " +
                         " from crm_trn_tappointment a  " +
                         " left join crm_trn_tcampaign k on a.campaign_gid = k.campaign_gid  " +
                         " left join cmn_trn_tmanagerprivilege m on m.team_gid=k.campaign_gid " +
                         " where  m.employee_gid = '" + lsemployee_gid + "' and a.created_date >= DATE_SUB(CURRENT_DATE, INTERVAL 6 MONTH)  " +
                         " GROUP BY month_names ORDER BY MIN(a.created_date) desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<chartscount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new chartscount_list
                        {
                            Months = dt["month_names"].ToString(),
                            leads_count = dt["new_leads"].ToString(),
                            enquiry_count = dt["prospect_lead"].ToString(),
                            quote_count = dt["potential_lead"].ToString(),
                            order_count = dt["order_lead"].ToString(),
                           
                        });
                        values.chartscount_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetMarketingManagerDropRemarks(string appointment_gid, MdlMarketingmanager values)
        {
            try
            {
                msSQL = "select drop_remarks from crm_trn_tappointment where appointment_gid ='" + appointment_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<campaignbin_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new campaignbin_list
                        {
                            drop_remark1 = dt["drop_remarks"].ToString(),


                        });
                        values.campaignbin_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Manager Drop Remarks!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}

