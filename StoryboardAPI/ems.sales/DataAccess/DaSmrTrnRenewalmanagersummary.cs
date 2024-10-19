using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows;
using static ems.sales.Models.MdlSmrTrnRenewalmanagersummary;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnRenewalmanagersummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        

        public void DaGetRenewalManagerSummary(MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {
                msSQL = " SELECT t1.total, t1.campaign_gid, t1.campaign_title, t1.campaign_location, t1.branch_name, t1.campaign_startdate,t1.created_date, t1.campaign_enddate, t1.product_name, " +
                    "t2.complete, t3.dropped,t4.total_managers,t5.total_employees " +
                    "FROM(SELECT COUNT(d.lead2campaign_gid) AS total, a.campaign_gid, a.campaign_title, a.campaign_location, c.branch_name, " +
                    "DATE_FORMAT(a.campaign_startdate, '%d-%m-%Y') AS campaign_startdate, CASE WHEN a.campaign_enddate = '0000-00-00' THEN 'Infinite'" +
                    " ELSE DATE_FORMAT(a.campaign_enddate, '%d-%m-%Y') END AS campaign_enddate, b.product_name,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date FROM crm_trn_trenewalteam a " +
                    "LEFT JOIN crm_mst_tproduct b ON a.product_gid = b.product_gid " +
                    "LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid " +
                    "LEFT JOIN crm_trn_trenewal2campaign d ON a.campaign_gid = d.campaign_gid " +
                    "WHERE 1 = 1 GROUP BY a.campaign_gid, a.campaign_title, a.campaign_location,a.created_date, c.branch_name, a.campaign_startdate, a.campaign_enddate,b.product_name) t1" +
                    " LEFT JOIN(SELECT COUNT(lead2campaign_gid) AS complete, campaign_gid" +
                    " FROM crm_trn_trenewal2campaign WHERE leadstage_gid = '5' GROUP BY campaign_gid) t2 ON t1.campaign_gid = t2.campaign_gid " +
                    "LEFT JOIN(SELECT COUNT(lead2campaign_gid) AS dropped, campaign_gid " +
                    "FROM crm_trn_trenewal2campaign WHERE lead_status = 'Closed' AND leadstage_gid = '6' GROUP BY campaign_gid) t3 ON t1.campaign_gid = t3.campaign_gid " +
                    "LEFT JOIN(SELECT a.campaign_gid, COUNT(b.employee_gid) AS total_managers " +
                    "FROM crm_trn_trenewalteam a LEFT JOIN cmn_trn_tmanagerprivilege b ON a.campaign_gid = b.team_gid GROUP BY a.campaign_gid) t4 ON t1.campaign_gid = t4.campaign_gid " +
                    "LEFT JOIN(SELECT a.campaign_gid, COUNT(b.employee_gid) AS total_employees FROM crm_trn_trenewalteam a " +
                    "LEFT JOIN crm_trn_trenewal2employee b ON a.campaign_gid = b.campaign_gid GROUP BY a.campaign_gid) t5 ON t1.campaign_gid = t5.campaign_gid " +
                    "ORDER BY t1.campaign_gid DESC LIMIT 0, 1000 ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalSummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalSummary_lists
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            team_name = dt["campaign_title"].ToString(),
                            team_branch = dt["branch_name"].ToString(),
                            assigned_employee = dt["total_employees"].ToString(),
                            completed_renewals = dt["complete"].ToString(),
                            dropped_renewals = dt["dropped"].ToString(),
                            renewals_assigned = dt["total"].ToString()
                        });
                        values.GetRenewalSummary_lists = getModuleList;
                    }
               
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while getting renewal manager summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetRenewalChart(MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {
                msSQL="SELECT COUNT(CASE WHEN renewal_status = 'Open' THEN 1 END) AS open_count, " +
                    "COUNT(CASE WHEN renewal_status = 'Closed' THEN 1 END) AS closed_count," +
                    "COUNT(CASE WHEN renewal_status = 'Dropped' THEN 1 END) AS dropped_count " +
                    "FROM crm_trn_trenewal";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalSummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalSummary_lists
                        {
                            open_count = dt["open_count"].ToString(),
                            closed_count = dt["closed_count"].ToString(),
                            dropped_count = dt["dropped_count"].ToString(),
                        });
                        values.GetRenewalSummary_lists = getModuleList;
                    }

                }
                dt_datatable.Dispose();

            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while Getting Renewal Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetRenewalTeamSummary(MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {
          string msSQL = " SELECT  a.renewal_gid,a.renewal_status,d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal, DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date, " +
                         "b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details, FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date, " +
                         " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a " +
                         " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                         " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid   WHERE renewal_type='sales' and  d.renewal_flag = 'Y' UNION " +
                         " SELECT a.renewal_gid,a.renewal_status,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal, DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date," +
                         " b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details, FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date, " +
                         " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                         " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                         " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid" +
                         " WHERE a.renewal_type = 'Agreement' ORDER BY renewal_gid DESC";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetRenewalTeam_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalTeam_list
                        {
                           

                            renewal_gid = dt["renewal_gid"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            renewal_status = dt["renewal_status"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            salesorder_date = dt["order_agreement_date"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                        });
                    }
                    values.GetRenewalTeam_list = getModuleList;
                }

                dt_datatable.Dispose();

            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Getting Renewal Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            
        }

        public void DaGetSmrTrnEmployeeCount(MdlSmrTrnRenewalmanagersummary values)
        {
            string msSQL = "SELECT sum(t5.total_employees)as totalemployee FROM (SELECT COUNT(d.lead2campaign_gid) AS total, a.campaign_gid, a.campaign_title, " +
                 "a.campaign_location, c.branch_name, DATE_FORMAT(a.campaign_startdate, '%d-%m-%Y') AS campaign_startdate, CASE WHEN a.campaign_enddate = '0000-00-00' " +
                 "THEN 'Infinite' ELSE DATE_FORMAT(a.campaign_enddate, '%d-%m-%Y') END AS campaign_enddate, b.product_name, " +
                 "DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date FROM crm_trn_trenewalteam a " +
                 "LEFT JOIN crm_mst_tproduct b ON a.product_gid = b.product_gid " +
                 "LEFT JOIN hrm_mst_tbranch c ON a.campaign_location = c.branch_gid " +
                 "LEFT JOIN crm_trn_trenewal2campaign d ON a.campaign_gid = d.campaign_gid WHERE 1 = 1 GROUP BY a.campaign_gid, a.campaign_title, a.campaign_location, " +
                 "a.created_date, c.branch_name, a.campaign_startdate, a.campaign_enddate, b.product_name) t1 LEFT JOIN (SELECT COUNT(lead2campaign_gid) AS complete, " +
                 "campaign_gid FROM crm_trn_trenewal2campaign WHERE leadstage_gid = '5' GROUP BY campaign_gid) t2 ON t1.campaign_gid = t2.campaign_gid " +
                 "LEFT JOIN (SELECT COUNT(lead2campaign_gid) AS dropped, campaign_gid FROM crm_trn_trenewal2campaign " +
                 "WHERE lead_status = 'Closed' AND leadstage_gid = '6' GROUP BY campaign_gid) t3 ON t1.campaign_gid = t3.campaign_gid " +
                 "LEFT JOIN (SELECT a.campaign_gid, COUNT(b.employee_gid) AS total_managers FROM crm_trn_trenewalteam a " +
                 "LEFT JOIN cmn_trn_tmanagerprivilege b ON a.campaign_gid = b.team_gid GROUP BY a.campaign_gid) t4 ON t1.campaign_gid = t4.campaign_gid " +
                 "LEFT JOIN (SELECT a.campaign_gid, COUNT(b.employee_gid) AS total_employees FROM crm_trn_trenewalteam a " +
                 "LEFT JOIN crm_trn_trenewal2employee b ON a.campaign_gid = b.campaign_gid GROUP BY a.campaign_gid) t5 ON t1.campaign_gid = t5.campaign_gid " +
                 "ORDER BY t1.campaign_gid DESC LIMIT 0, 1000";

            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetRenewalTeamEmployee_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetRenewalTeamEmployee_list
                    {
                        totalemployee = dt["totalemployee"].ToString(),
                        
                    });
                }
                values.GetRenewalTeamEmployee_list = getModuleList;
            }

            dt_datatable.Dispose();
        }
        public void DaGetSmrTrnRenewalsCount(MdlSmrTrnRenewalmanagersummary values)
        {
            string msSQL = "SELECT COUNT(renewal_status) AS total_renewal_status_count FROM(SELECT DISTINCT a.renewal_gid, a.customer_gid, a.renewal_type AS renewal, " +
                "DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date, a.renewal_to, a.renewal_status, CONCAT(c.user_firstname, '-', c.user_lastname) AS user_name, " +
                " d.customer_name, CONCAT(e.customercontact_name, ' / ', e.mobile, ' / ', e.email) AS contact_details, g.salesorder_gid, " +
                "DATE_FORMAT(g.salesorder_date, '%d-%m-%Y') AS salesorder_date, FORMAT(g.Grandtotal, 2) AS Grandtotal, a.renewal_description, a.created_by, a.created_date, " +
                "CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a " +
                "LEFT JOIN smr_trn_tsalesorder g ON a.salesorder_gid = g.salesorder_gid " +
                "LEFT JOIN hrm_mst_temployee b ON a.renewal_to = b.employee_gid " +
                "LEFT JOIN adm_mst_tuser c ON b.user_gid = c.user_gid " +
                "LEFT JOIN crm_mst_tcustomer d ON a.customer_gid = d.customer_gid " +
                "LEFT JOIN crm_mst_tcustomercontact e ON a.customer_gid = e.customer_gid  WHERE a.renewal_type<> 'Agreement' UNION " +
                "SELECT DISTINCT a.renewal_gid, a.customer_gid, a.renewal_type AS renewal, DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date, a.renewal_to, " +
                "a.renewal_status, CONCAT(c.user_firstname, '-', c.user_lastname) AS user_name, d.customer_name, " +
                "CONCAT(e.customercontact_name, ' / ', e.mobile, ' / ', e.email) AS contact_details, g.agreement_gid, DATE_FORMAT(g.agreement_date, '%d-%m-%Y') AS " +
                "salesorder_date, FORMAT(g.Grandtotal, 2) AS Grandtotal, a.renewal_description, a.created_by, a.created_date, CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), " +
                "' days') AS duration FROM crm_trn_trenewal a LEFT JOIN crm_trn_tagreement g ON a.salesorder_gid = g.agreement_gid " +
                "LEFT JOIN hrm_mst_temployee b ON a.renewal_to = b.employee_gid " +
                "LEFT JOIN adm_mst_tuser c ON b.user_gid = c.user_gid " +
                "LEFT JOIN crm_mst_tcustomer d ON a.customer_gid = d.customer_gid " +
                "LEFT JOIN crm_mst_tcustomercontact e ON a.customer_gid = e.customer_gid WHERE a.renewal_type = 'Agreement') AS combined";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalTeamRenewals_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalTeamRenewals_list
                        {
                            total_renewal_status_count = dt["total_renewal_status_count"].ToString(),
                        });
                    }
                    values.GetRenewalTeamRenewals_list = getModuleList;
                }
                dt_datatable.Dispose();
        }

        public void DaGetMonthyRenewal(MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {
                msSQL = "SELECT MONTHNAME(renewal_date) AS renewal_month_name,YEAR(renewal_date) AS renewal_year,COUNT(*) " +
                    "AS renewal_count FROM crm_trn_trenewal WHERE renewal_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) GROUP BY " +
                    "YEAR(renewal_date), MONTH(renewal_date) ORDER BY YEAR(renewal_date), MONTH(renewal_date)";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMonthlyRenewal_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMonthlyRenewal_lists
                        {
                            renewal_month_name = dt["renewal_month_name"].ToString(),
                            renewal_year = dt["renewal_year"].ToString(),
                            renewal_count = dt["renewal_count"].ToString(),
                        });
                        values.GetMonthlyRenewal_lists = getModuleList;
                    }

                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Renewal Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetRenewalReportForLastSixMonths(MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {

                msSQL = " select a.renewal_gid, YEAR(a.renewal_date) AS year, a.renewal_date, SUBSTRING(DATE_FORMAT(a.renewal_date, '%M'),1, 3) AS months , " +
                    " COUNT(a.renewal_gid) AS renewalcount, a.renewal_status " +
                    " from crm_trn_trenewal a  WHERE  a.renewal_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH)  AND a.renewal_date <= DATE(NOW()) " +
                    " GROUP BY DATE_FORMAT(a.renewal_date, '%M') ORDER BY a.renewal_date DESC ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetRenewalForLastSixMonths_List = new List<GetRenewalForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetRenewalForLastSixMonths_List.Add(new GetRenewalForLastSixMonths_List
                        {
                            renewal_gid = (dt["renewal_gid"].ToString()),
                            renewal_date = (dt["renewal_date"].ToString()),
                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            renewalcount = (dt["renewalcount"].ToString()),
                            renewal_status = (dt["renewal_status"].ToString()),
                        });
                        values.GetRenewalForLastSixMonths_List = GetRenewalForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetRenewalReportForLastSixMonthsSearch(MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {

                msSQL = " select a.renewal_gid, YEAR(a.renewal_date) AS year, a.renewal_date, SUBSTRING(DATE_FORMAT(a.renewal_date, '%M'),1, 3) AS months , " +
                    "  COUNT(a.renewal_gid) AS renewalcount, a.renewal_status from crm_trn_trenewal a " +
                    "  WHERE  a.renewal_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH)  AND a.renewal_date <= DATE(NOW()) " +
                    "  GROUP BY DATE_FORMAT(a.renewal_date, '%M') ORDER BY a.renewal_date DESC  ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetRenewalForLastSixMonths_List = new List<GetRenewalForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetRenewalForLastSixMonths_List.Add(new GetRenewalForLastSixMonths_List
                        {
                            renewal_gid = (dt["renewal_gid"].ToString()),
                            renewal_date = (dt["renewal_date"].ToString()),
                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            renewalcount = (dt["renewalcount"].ToString()),
                            renewal_status = (dt["renewal_status"].ToString()),
                        });
                        values.GetRenewalForLastSixMonths_List = GetRenewalForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetrenewalDetailSummary(string branch_gid, string month, string year, MdlSmrTrnRenewalmanagersummary values)
        {
            try
            {

                string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                             " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                             " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                             " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration,a.renewal_status  FROM crm_trn_trenewal a  " +
                             " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                             " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                             " where substring(date_format(a.renewal_date,'%M'),1,3)='" + month + "' and year(a.renewal_date)='" + year + "' and  a.renewal_flag='Y' and a.renewal_gid UNION " +
                             " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                             " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                             " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                             " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration,a.renewal_status FROM crm_trn_trenewal a" +
                             " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " + "" +
                             " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid " +
                             " where substring(date_format(a.renewal_date,'%M'),1,3)='" + month + "' and year(a.renewal_date)='" + year + "'  ORDER BY renewal_gid DESC";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetRenewalDetailSummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalDetailSummarylist
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            renewal_status = dt["renewal_status"].ToString(),
                            duration = dt["duration"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                        });
                    }
                    values.GetRenewalDetailSummarylist = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
    
}