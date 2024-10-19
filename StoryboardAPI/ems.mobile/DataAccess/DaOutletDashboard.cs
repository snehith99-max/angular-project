using ems.mobile.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;

namespace ems.mobile.DataAccess
{
    public class DaOutletDashboard
    {

        dbconn objdbconn = new dbconn();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        cmnfunctions objcmnfunctions = new cmnfunctions();



        public void DaGetBudgetDetails(string user_gid, MdlOutletDashboard values, string selectedTab, string headerSelectedTab, string outletName)
        {
            if (selectedTab == "7 days" && headerSelectedTab == "Abstract")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                msSQL = "SELECT " +
                        "SUM(revenue_amount) AS total_revenue, " +
                        "'7 DAYS OVERALL OUTLET' AS outlet_name, " +
                        "SUM(expense_amount) AS total_expense, " +
                        "CASE " +
                        "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 " +
                        "ELSE SUM(revenue_amount) - SUM(expense_amount) " +
                        "END AS total_profit, " +
                        "CASE " +
                        "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) " +
                        "ELSE 0 " +
                        "END AS total_loss, " +
                        "DATE_FORMAT(MIN(trade_date), '%d-%m-%Y') AS start_date, " +
                        "DATE_FORMAT(MAX(trade_date), '%d-%m-%Y') AS end_date " +
                        "FROM sys_mst_tdaytrackerdtl " +
                        "WHERE trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallbudget_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallbudget_list
                        {
                            outlet_name = dt["outlet_name"].ToString(),
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            total_revenue = dt["total_revenue"].ToString(),
                            total_expense = dt["total_expense"].ToString(),
                            total_profit = dt["total_profit"].ToString(),
                            total_loss = dt["total_loss"].ToString()
                        });
                        values.Overallbudget_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            else if (selectedTab == "15 days" && headerSelectedTab == "Abstract")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                msSQL = "SELECT " +
                       "SUM(revenue_amount) AS total_revenue, " +
                       "'15 DAYS OVERALL OUTLET' AS outlet_name, " +
                       "SUM(expense_amount) AS total_expense, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 " +
                       "ELSE SUM(revenue_amount) - SUM(expense_amount) " +
                       "END AS total_profit, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) " +
                       "ELSE 0 " +
                       "END AS total_loss, " +
                       "DATE_FORMAT(MIN(trade_date), '%d-%m-%Y') AS start_date, " +
                       "DATE_FORMAT(MAX(trade_date), '%d-%m-%Y') AS end_date " +
                       "FROM sys_mst_tdaytrackerdtl " +
                       "WHERE trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallbudget_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallbudget_list
                        {
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            outlet_name = dt["outlet_name"].ToString(),
                            total_revenue = dt["total_revenue"].ToString(),
                            total_expense = dt["total_expense"].ToString(),
                            total_profit = dt["total_profit"].ToString(),
                            total_loss = dt["total_loss"].ToString()
                        });
                        values.Overallbudget_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            else if (selectedTab == "30 days" && headerSelectedTab == "Abstract")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                msSQL = "SELECT " +
                       "SUM(revenue_amount) AS total_revenue, " +
                        "'30 DAYS OVERALL OUTLET' AS outlet_name, " +
                       "SUM(expense_amount) AS total_expense, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 " +
                       "ELSE SUM(revenue_amount) - SUM(expense_amount) " +
                       "END AS total_profit, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) " +
                       "ELSE 0 " +
                       "END AS total_loss, " +
                       "DATE_FORMAT(MIN(trade_date), '%d-%m-%Y') AS start_date, " +
                       "DATE_FORMAT(MAX(trade_date), '%d-%m-%Y') AS end_date " +
                       "FROM sys_mst_tdaytrackerdtl " +
                       "WHERE trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallbudget_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallbudget_list
                        {
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            outlet_name = dt["outlet_name"].ToString(),
                            total_revenue = dt["total_revenue"].ToString(),
                            total_expense = dt["total_expense"].ToString(),
                            total_profit = dt["total_profit"].ToString(),
                            total_loss = dt["total_loss"].ToString()
                        });
                        values.Overallbudget_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            else if (selectedTab == "7 days" && headerSelectedTab == "Master")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "SELECT " +
                       "SUM(revenue_amount) AS total_revenue, " +
                        "'30 DAYS OVERALL OUTLET' AS outlet_name, " +
                       "SUM(expense_amount) AS total_expense, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 " +
                       "ELSE SUM(revenue_amount) - SUM(expense_amount) " +
                       "END AS total_profit, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) " +
                       "ELSE 0 " +
                       "END AS total_loss, " +
                       "DATE_FORMAT(MIN(trade_date), '%d-%m-%Y') AS start_date, " +
                       "DATE_FORMAT(MAX(trade_date), '%d-%m-%Y') AS end_date " +
                       "FROM sys_mst_tdaytrackerdtl " +
                       "WHERE trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' AND campaign_gid = '" + lscampaignGid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallbudget_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallbudget_list
                        {
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            outlet_name = dt["outlet_name"].ToString(),
                            total_revenue = dt["total_revenue"].ToString(),
                            total_expense = dt["total_expense"].ToString(),
                            total_profit = dt["total_profit"].ToString(),
                            total_loss = dt["total_loss"].ToString()
                        });
                        values.Overallbudget_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            else if (selectedTab == "15 days" && headerSelectedTab == "Master")

            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");



                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "SELECT " +
                       "SUM(revenue_amount) AS total_revenue, " +
                        "'30 DAYS OVERALL OUTLET' AS outlet_name, " +
                       "SUM(expense_amount) AS total_expense, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 " +
                       "ELSE SUM(revenue_amount) - SUM(expense_amount) " +
                       "END AS total_profit, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) " +
                       "ELSE 0 " +
                       "END AS total_loss, " +
                       "DATE_FORMAT(MIN(trade_date), '%d-%m-%Y') AS start_date, " +
                       "DATE_FORMAT(MAX(trade_date), '%d-%m-%Y') AS end_date " +
                       "FROM sys_mst_tdaytrackerdtl " +
                       "WHERE trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' AND campaign_gid = '" + lscampaignGid + "' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallbudget_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallbudget_list
                        {
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            outlet_name = dt["outlet_name"].ToString(),
                            total_revenue = dt["total_revenue"].ToString(),
                            total_expense = dt["total_expense"].ToString(),
                            total_profit = dt["total_profit"].ToString(),
                            total_loss = dt["total_loss"].ToString()
                        });
                        values.Overallbudget_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            else if (selectedTab == "30 days" && headerSelectedTab == "Master")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "SELECT " +
                       "SUM(revenue_amount) AS total_revenue, " +
                        "'30 DAYS OVERALL OUTLET' AS outlet_name, " +
                       "SUM(expense_amount) AS total_expense, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 " +
                       "ELSE SUM(revenue_amount) - SUM(expense_amount) " +
                       "END AS total_profit, " +
                       "CASE " +
                       "WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) " +
                       "ELSE 0 " +
                       "END AS total_loss, " +
                       "DATE_FORMAT(MIN(trade_date), '%d-%m-%Y') AS start_date, " +
                       "DATE_FORMAT(MAX(trade_date), '%d-%m-%Y') AS end_date " +
                       "FROM sys_mst_tdaytrackerdtl " +
                       "WHERE trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' AND campaign_gid = '" + lscampaignGid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallbudget_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallbudget_list
                        {
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            outlet_name = dt["outlet_name"].ToString(),
                            total_revenue = dt["total_revenue"].ToString(),
                            total_expense = dt["total_expense"].ToString(),
                            total_profit = dt["total_profit"].ToString(),
                            total_loss = dt["total_loss"].ToString()
                        });
                        values.Overallbudget_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
        }

        public void DaGetChartDetails(string user_gid, MdlOutletDashboard values, string selectedTab, string headerSelectedTab, string outletName)
        {

            if (selectedTab == "7 days" && headerSelectedTab == "Abstract")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");


                string msSQL = "SELECT " +
               " date_created, " +
               "'Profit' AS type, " +
               "profit_amount AS amount " +
               "FROM " +
               "(SELECT " +
               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 ELSE SUM(revenue_amount) - SUM(expense_amount) END AS profit_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS profits " +
               "UNION ALL " +
               "SELECT " +
               "date_created, " +
               "'Income' AS type, " +
               "revenue_amount AS amount " +
               "FROM " +
               "(SELECT " +
               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "SUM(revenue_amount) AS revenue_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS incomes " +
               "UNION ALL " +
               "SELECT " +
               "date_created, " +
               "'Expense' AS type, " +
               "expense_amount AS amount " +
               "FROM " +
               "(SELECT " +
              "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "SUM(expense_amount) AS expense_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  GROUP BY DATE(trade_date)) AS expenses " +
               "UNION ALL " +
               "SELECT " +
               "date_created, " +
               "'Loss' AS type, " +
               "ABS(loss_amount) AS amount " +
               "FROM " +
               "(SELECT " +
              "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) ELSE 0 END AS loss_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS losses " +
               "ORDER BY date_created, type";


                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallchart_list
                        {
                            created_date = dt["date_created"].ToString(),
                            type = dt["type"].ToString(),
                            amount = dt["amount"].ToString()
                        });
                    }
                    values.Overallchart_Lists = getModuleList;
                }
                dt_datatable.Dispose();
            }


            else if (selectedTab == "15 days" && headerSelectedTab == "Abstract")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");




                string msSQL = "SELECT " +
              " date_created, " +
              "'Profit' AS type, " +
              "profit_amount AS amount " +
              "FROM " +
              "(SELECT " +
              "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
              "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 ELSE SUM(revenue_amount) - SUM(expense_amount) END AS profit_amount " +
              "FROM " +
              "sys_mst_tdaytrackerdtl " +
              "WHERE " +
              "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS profits " +
              "UNION ALL " +
              "SELECT " +
              "date_created, " +
              "'Income' AS type, " +
              "revenue_amount AS amount " +
              "FROM " +
              "(SELECT " +
              "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
              "SUM(revenue_amount) AS revenue_amount " +
              "FROM " +
              "sys_mst_tdaytrackerdtl " +
              "WHERE " +
              "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS incomes " +
              "UNION ALL " +
              "SELECT " +
              "date_created, " +
              "'Expense' AS type, " +
              "expense_amount AS amount " +
              "FROM " +
              "(SELECT " +
              "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
              "SUM(expense_amount) AS expense_amount " +
              "FROM " +
              "sys_mst_tdaytrackerdtl " +
              "WHERE " +
              "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS expenses " +
              "UNION ALL " +
              "SELECT " +
              "date_created, " +
              "'Loss' AS type, " +
              "ABS(loss_amount) AS amount " +
              "FROM " +
              "(SELECT " +
              "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
              "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) ELSE 0 END AS loss_amount " +
              "FROM " +
              "sys_mst_tdaytrackerdtl " +
              "WHERE " +
              "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS losses " +
              "ORDER BY date_created, type";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallchart_list
                        {
                            created_date = dt["date_created"].ToString(),
                            type = dt["type"].ToString(),
                            amount = dt["amount"].ToString()
                        });
                    }
                    values.Overallchart_Lists = getModuleList;
                }
                dt_datatable.Dispose();

            }

            else if (selectedTab == "30 days" && headerSelectedTab == "Abstract")

            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL = "SELECT " +
               " date_created, " +
               "'Profit' AS type, " +
               "profit_amount AS amount " +
               "FROM " +
               "(SELECT " +
               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 ELSE SUM(revenue_amount) - SUM(expense_amount) END AS profit_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS profits " +
               "UNION ALL " +
               "SELECT " +
               "date_created, " +
               "'Income' AS type, " +
               "revenue_amount AS amount " +
               "FROM " +
               "(SELECT " +
               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "SUM(revenue_amount) AS revenue_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS incomes " +
               "UNION ALL " +
               "SELECT " +
               "date_created, " +
               "'Expense' AS type, " +
               "expense_amount AS amount " +
               "FROM " +
               "(SELECT " +
               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "SUM(expense_amount) AS expense_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS expenses " +
               "UNION ALL " +
               "SELECT " +
               "date_created, " +
               "'Loss' AS type, " +
               "ABS(loss_amount) AS amount " +
               "FROM " +
               "(SELECT " +
               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
               "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) ELSE 0 END AS loss_amount " +
               "FROM " +
               "sys_mst_tdaytrackerdtl " +
               "WHERE " +
               "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' GROUP BY DATE(trade_date)) AS losses " +
               "ORDER BY date_created, type";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallchart_list
                        {
                            created_date = dt["date_created"].ToString(),
                            type = dt["type"].ToString(),
                            amount = dt["amount"].ToString()
                        });
                    }
                    values.Overallchart_Lists = getModuleList;
                }
                dt_datatable.Dispose();

            }
            else if (selectedTab == "7 days" && headerSelectedTab == "Master")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                string msSQL = "SELECT " +
                                " date_created, " +
                                "'Profit' AS type, " +
                                "profit_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 ELSE SUM(revenue_amount) - SUM(expense_amount) END AS profit_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS profits " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Income' AS type, " +
                                "revenue_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "SUM(revenue_amount) AS revenue_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS incomes " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Expense' AS type, " +
                                "expense_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "SUM(expense_amount) AS expense_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS expenses " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Loss' AS type, " +
                                "ABS(loss_amount) AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) ELSE 0 END AS loss_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS losses " +
                                "ORDER BY date_created, type";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallchart_list
                        {
                            created_date = dt["date_created"].ToString(),
                            type = dt["type"].ToString(),
                            amount = dt["amount"].ToString()
                        });
                    }
                    values.Overallchart_Lists = getModuleList;
                }
                dt_datatable.Dispose();

            }
            else if (selectedTab == "15 days" && headerSelectedTab == "Master")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                string msSQL = "SELECT " +
                                " date_created, " +
                                "'Profit' AS type, " +
                                "profit_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                               "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 ELSE SUM(revenue_amount) - SUM(expense_amount) END AS profit_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS profits " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Income' AS type, " +
                                "revenue_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "SUM(revenue_amount) AS revenue_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS incomes " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Expense' AS type, " +
                                "expense_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "SUM(expense_amount) AS expense_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS expenses " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Loss' AS type, " +
                                "ABS(loss_amount) AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) ELSE 0 END AS loss_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS losses " +
                                "ORDER BY date_created, type";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallchart_list
                        {
                            created_date = dt["date_created"].ToString(),
                            type = dt["type"].ToString(),
                            amount = dt["amount"].ToString()
                        });
                    }
                    values.Overallchart_Lists = getModuleList;
                }
                dt_datatable.Dispose();

            }
            else if (selectedTab == "30 days" && headerSelectedTab == "Master")
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                string msSQL = "SELECT " +
                                " date_created, " +
                                "'Profit' AS type, " +
                                "profit_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN 0 ELSE SUM(revenue_amount) - SUM(expense_amount) END AS profit_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS profits " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Income' AS type, " +
                                "revenue_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "SUM(revenue_amount) AS revenue_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS incomes " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Expense' AS type, " +
                                "expense_amount AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "SUM(expense_amount) AS expense_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS expenses " +
                                "UNION ALL " +
                                "SELECT " +
                                "date_created, " +
                                "'Loss' AS type, " +
                                "ABS(loss_amount) AS amount " +
                                "FROM " +
                                "(SELECT " +
                                "date_format(trade_date,'%d-%m-%Y') AS date_created, " +
                                "CASE WHEN SUM(revenue_amount) - SUM(expense_amount) < 0 THEN ABS(SUM(revenue_amount) - SUM(expense_amount)) ELSE 0 END AS loss_amount " +
                                "FROM " +
                                "sys_mst_tdaytrackerdtl " +
                                "WHERE " +
                                "trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'  AND campaign_gid = '" + lscampaignGid + "' GROUP BY DATE(trade_date)) AS losses " +
                                "ORDER BY date_created, type";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallchart_list
                        {
                            created_date = dt["date_created"].ToString(),
                            type = dt["type"].ToString(),
                            amount = dt["amount"].ToString()
                        });
                    }
                    values.Overallchart_Lists = getModuleList;
                }
                dt_datatable.Dispose();
            }

        }

        public void DaGetSummaryDetails(string user_gid, MdlOutletDashboard values, string selectedTab, string headerSelectedTab, string outletName)
        {


            if (selectedTab == "7 days" && headerSelectedTab == "Abstract")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                msSQL = "SELECT " +
                        "a.daytrackerdtl_gid, " +
                        "SUM(a.expense_amount) AS total_expense_amount, " +
                        "SUM(a.revenue_amount) AS total_income_amount, " +
                        "date_format(a.trade_date, '%d-%m-%y') AS created_date, " +
                        "CASE " +
                        "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0   " +
                        "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount))" +
                        "END AS total_profit_amount, " +
                        "CASE " +
                        "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) " +
                        "ELSE 0 " +
                        "END AS total_loss_amount " +
                        "FROM sys_mst_tdaytrackerdtl a " +
                        "LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                        "LEFT JOIN otl_trn_touletcampaign c ON c.campaign_gid = a.campaign_gid " +
                        "WHERE " +
                        "a.trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' " +
                        "GROUP BY  DATE_FORMAT(a.trade_date, '%Y-%m-%d') " +
                        "ORDER BY a.trade_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallsummary_list
                        {
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            income_amount = dt["total_income_amount"].ToString(),
                            expense_amount = dt["total_expense_amount"].ToString(),
                            profit_amount = dt["total_profit_amount"].ToString(),
                            loss_amount = dt["total_loss_amount"].ToString()

                        });
                        values.SummaryData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }

            else if (selectedTab == "15 days" && headerSelectedTab == "Abstract")
            {


                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                msSQL = "SELECT " +
                         "a.daytrackerdtl_gid, " +
                         "SUM(a.expense_amount) AS total_expense_amount, " +
                         "SUM(a.revenue_amount) AS total_income_amount, " +
                          "date_format(a.trade_date, '%d-%m-%y') AS created_date, " +
                         "CASE " +
                         "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0   " +
                         "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount))" +
                         "END AS total_profit_amount, " +
                         "CASE " +
                         "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) " +
                         "ELSE 0 " +
                         "END AS total_loss_amount " +
                         "FROM sys_mst_tdaytrackerdtl a " +
                         "LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                         "LEFT JOIN otl_trn_touletcampaign c ON c.campaign_gid = a.campaign_gid " +
                         "WHERE " +
                         "a.trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' " +
                         "GROUP BY  DATE_FORMAT(a.trade_date, '%Y-%m-%d') " +
                         "ORDER BY a.trade_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallsummary_list
                        {
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            income_amount = dt["total_income_amount"].ToString(),
                            expense_amount = dt["total_expense_amount"].ToString(),
                            profit_amount = dt["total_profit_amount"].ToString(),
                            loss_amount = dt["total_loss_amount"].ToString()

                        });
                        values.SummaryData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }


            else if (selectedTab == "30 days" && headerSelectedTab == "Abstract")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                msSQL = "SELECT " +
                         "a.daytrackerdtl_gid, " +
                         "SUM(a.expense_amount) AS total_expense_amount, " +
                         "SUM(a.revenue_amount) AS total_income_amount, " +
                         "date_format(a.trade_date, '%d-%m-%y') AS created_date, " +
                         "CASE " +
                         "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0   " +
                         "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount))" +
                         "END AS total_profit_amount, " +
                         "CASE " +
                         "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) " +
                         "ELSE 0 " +
                         "END AS total_loss_amount " +
                         "FROM sys_mst_tdaytrackerdtl a " +
                         "LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                         "LEFT JOIN otl_trn_touletcampaign c ON c.campaign_gid = a.campaign_gid " +
                         "WHERE " +
                         "a.trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "' " +
                         "GROUP BY  DATE_FORMAT(a.trade_date, '%Y-%m-%d') " +
                         "ORDER BY a.trade_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallsummary_list
                        {
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            income_amount = dt["total_income_amount"].ToString(),
                            expense_amount = dt["total_expense_amount"].ToString(),
                            profit_amount = dt["total_profit_amount"].ToString(),
                            loss_amount = dt["total_loss_amount"].ToString()

                        });
                        values.SummaryData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }

            else if (selectedTab == "7 days" && headerSelectedTab == "Master")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

                msSQL = "SELECT " +
                          "a.daytrackerdtl_gid, " +
                          "SUM(a.expense_amount) AS total_expense_amount, " +
                          "SUM(a.revenue_amount) AS total_income_amount, " +
                         "date_format(a.trade_date, '%d-%m-%y') AS created_date, " +
                          "CASE " +
                          "WHEN (a.revenue_amount - a.expense_amount) < 0 THEN 0 " +
                          "ELSE (a.revenue_amount - a.expense_amount) " +
                          "END AS profit_amount, " +
                          "CASE " +
                          "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 " +
                          "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount))END AS total_profit_amount," +
                          "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount))" +
                          "ELSE 0 END AS total_loss_amount " +
                          "FROM sys_mst_tdaytrackerdtl a " +
                          "LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                          "LEFT JOIN otl_trn_touletcampaign c ON c.campaign_gid = a.campaign_gid " +
                          "WHERE " +
                          "a.trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'AND a.campaign_gid = '" + lscampaignGid + "' " +
                          "GROUP BY  DATE_FORMAT(a.trade_date, '%Y-%m-%d') " +
                          "ORDER BY a.trade_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallsummary_list
                        {
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            income_amount = dt["total_income_amount"].ToString(),
                            expense_amount = dt["total_expense_amount"].ToString(),
                            profit_amount = dt["total_profit_amount"].ToString(),
                            loss_amount = dt["total_loss_amount"].ToString()

                        });
                        values.SummaryData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            else if (selectedTab == "15 days" && headerSelectedTab == "Master")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);


                msSQL = "SELECT " +
                         "a.daytrackerdtl_gid, " +
                         "SUM(a.expense_amount) AS total_expense_amount, " +
                         "SUM(a.revenue_amount) AS total_income_amount, " +
                          "date_format(a.trade_date, '%d-%m-%y') AS created_date, " +
                         "CASE " +
                         "WHEN (a.revenue_amount - a.expense_amount) < 0 THEN 0 " +
                         "ELSE (a.revenue_amount - a.expense_amount) " +
                         "END AS profit_amount, " +
                         "CASE " +
                         "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 " +
                         "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount))END AS total_profit_amount," +
                         "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount))" +
                         "ELSE 0 END AS total_loss_amount " +
                         "FROM sys_mst_tdaytrackerdtl a " +
                         "LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                         "LEFT JOIN otl_trn_touletcampaign c ON c.campaign_gid = a.campaign_gid " +
                         "WHERE " +
                         "a.trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'AND a.campaign_gid = '" + lscampaignGid + "' " +
                         "GROUP BY  DATE_FORMAT(a.trade_date, '%Y-%m-%d') " +
                         "ORDER BY a.trade_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallsummary_list
                        {
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            income_amount = dt["total_income_amount"].ToString(),
                            expense_amount = dt["total_expense_amount"].ToString(),
                            profit_amount = dt["total_profit_amount"].ToString(),
                            loss_amount = dt["total_loss_amount"].ToString()

                        });
                        values.SummaryData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            else if (selectedTab == "30 days" && headerSelectedTab == "Master")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);

                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL1 = " Select campaign_gid from otl_trn_touletcampaign WHERE campaign_title='" + outletName + "' ";
                string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);


                msSQL = "SELECT " +
                        "a.daytrackerdtl_gid, " +
                        "SUM(a.expense_amount) AS total_expense_amount, " +
                        "SUM(a.revenue_amount) AS total_income_amount, " +
                         "date_format(a.trade_date, '%d-%m-%y') AS created_date, " +
                        "CASE " +
                        "WHEN (a.revenue_amount - a.expense_amount) < 0 THEN 0 " +
                        "ELSE (a.revenue_amount - a.expense_amount) " +
                        "END AS profit_amount, " +
                        "CASE " +
                        "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 " +
                        "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount))END AS total_profit_amount," +
                        "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount))" +
                        "ELSE 0 END AS total_loss_amount " +
                        "FROM sys_mst_tdaytrackerdtl a " +
                        "LEFT JOIN adm_mst_tuser b ON b.user_gid = a.created_by " +
                        "LEFT JOIN otl_trn_touletcampaign c ON c.campaign_gid = a.campaign_gid " +
                        "WHERE " +
                        "a.trade_date BETWEEN '" + startDateString + "' AND '" + endDateString + "'AND a.campaign_gid = '" + lscampaignGid + "' " +
                        "GROUP BY  DATE_FORMAT(a.trade_date, '%Y-%m-%d') " +
                        "ORDER BY a.trade_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<overallsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new overallsummary_list
                        {
                            daytrackerdtl_gid = dt["daytrackerdtl_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            income_amount = dt["total_income_amount"].ToString(),
                            expense_amount = dt["total_expense_amount"].ToString(),
                            profit_amount = dt["total_profit_amount"].ToString(),
                            loss_amount = dt["total_loss_amount"].ToString()

                        });
                        values.SummaryData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
        }

        public void DaGetOutletNameDetails(string user_gid, MdlOutletDashboard values, string selectedTab, string headerSelectedTab)
        {

            if (selectedTab == "7 days" && headerSelectedTab == "Master")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-6);
                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL = "SELECT " +
                 "a.campaign_gid, " +
                 "a.branch, " +
                 "a.campaign_description, " +
                 "a.campaign_status, " +
                 "a.campaign_title AS outlet_name " +
                 "FROM otl_trn_touletcampaign a " +
                 "ORDER BY a.created_date";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outlet_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outlet_list
                        {
                            outlet_name = dt["outlet_name"].ToString(),
                            branch = dt["branch"].ToString(),
                            campaign_description = dt["campaign_description"].ToString(),
                            campaign_status = dt["campaign_status"].ToString()

                        });
                        values.OutletData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            else if (selectedTab == "15 days" && headerSelectedTab == "Master")


            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-14);
                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL = "SELECT " +
                   "a.campaign_gid, " +
                   "a.branch, " +
                   "a.campaign_description, " +
                   "a.campaign_status, " +
                   "a.campaign_title AS outlet_name " +
                   "FROM otl_trn_touletcampaign a " +
                   "ORDER BY a.created_date";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outlet_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outlet_list
                        {
                            outlet_name = dt["outlet_name"].ToString(),
                            branch = dt["branch"].ToString(),
                            campaign_description = dt["campaign_description"].ToString(),
                            campaign_status = dt["campaign_status"].ToString()

                        });
                        values.OutletData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            else if (selectedTab == "30 days" && headerSelectedTab == "Master")
            {

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddDays(-29);
                string startDateString = startDate.ToString("yyyy-MM-dd");
                string endDateString = endDate.ToString("yyyy-MM-dd");

                string msSQL = "SELECT " +
                  "a.campaign_gid, " +
                  "a.branch, " +
                  "a.campaign_description, " +
                  "a.campaign_status, " +
                  "a.campaign_title AS outlet_name " +
                  "FROM otl_trn_touletcampaign a " +
                  "ORDER BY a.created_date";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outlet_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outlet_list
                        {
                            outlet_name = dt["outlet_name"].ToString(),
                            branch = dt["branch"].ToString(),
                            campaign_description = dt["campaign_description"].ToString(),
                            campaign_status = dt["campaign_status"].ToString()

                        });
                        values.OutletData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
        }
        public void DaGetExpenseRevenueDetials(string user_gid, MdlOutletDashboard values, string createdDate, string outletName)
        {

            DateTime parsedDate = DateTime.ParseExact(createdDate, "dd-MM-yy", CultureInfo.InvariantCulture);


            string formattedDate = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");



            string msSQL1 = "SELECT campaign_gid FROM otl_trn_touletcampaign WHERE campaign_title='" + outletName + "'";
            string lscampaignGid = objdbconn.GetExecuteScalar(msSQL1);

            if (string.IsNullOrEmpty(lscampaignGid))
            {
                string msSQL = "SELECT " +
                    "COALESCE(revenue_data.revenue_name, NULL) AS revenue_name, " +
                    "CONCAT(FORMAT(COALESCE(revenue_data.revenue_amount, NULL), 2, 'en_IN')) AS revenue_amount, " +
                    "COALESCE(expense_data.expense_name, NULL) AS expense_name, " +
                    "CONCAT(FORMAT(COALESCE(expense_data.expense_amount, NULL), 2, 'en_IN')) AS expense_amount " +
                    "FROM " +
                    "(SELECT " +
                    "revenue_name, " +
                    "revenue_amount, " +
                    "@row_num := @row_num + 1 AS row_num " +
                    "FROM " +
                    "(SELECT @row_num := 0) AS init, " +
                    "(SELECT " +
                    "revenue_name, " +
                    "revenue_amount " +
                    "FROM " +
                    "sys_mst_tdaytrackerdtl " +
                    "WHERE " +
                    "trade_date = '" + formattedDate + "' " +
                    "ORDER BY " +
                    "trade_date) AS rd) AS revenue_data " +
                    "LEFT JOIN " +
                    "(SELECT " +
                    "expense_name, " +
                    "expense_amount, " +
                    "@row_num2 := @row_num2 + 1 AS row_num " +
                    "FROM " +
                    "(SELECT @row_num2 := 0) AS init, " +
                    "(SELECT " +
                    "expense_name, " +
                    "expense_amount " +
                    "FROM " +
                    "sys_mst_tdaytrackerdtl " +
                    "WHERE " +
                    "trade_date = '" + formattedDate + "' " +
                    "ORDER BY " +
                    "trade_date) AS ed) AS expense_data " +
                    "ON " +
                    "revenue_data.row_num = expense_data.row_num " +
                    "ORDER BY " +
                    "COALESCE(revenue_data.row_num, expense_data.row_num)";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<revenue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new revenue_list
                        {
                            revenue_name = dt["revenue_name"].ToString(),
                            expense_name = dt["expense_name"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                            expense_amount = dt["expense_amount"].ToString()
                        });
                        values.RevenueData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            else
            {
                string msSQL = "SELECT " +
                    "COALESCE(revenue_data.revenue_name, NULL) AS revenue_name, " +
                    "CONCAT(FORMAT(COALESCE(revenue_data.revenue_amount, NULL), 2, 'en_IN')) AS revenue_amount, " +
                    "COALESCE(expense_data.expense_name, NULL) AS expense_name, " +
                    "CONCAT(FORMAT(COALESCE(expense_data.expense_amount, NULL), 2, 'en_IN')) AS expense_amount " +
                    "FROM " +
                    "(SELECT " +
                    "revenue_name, " +
                    "revenue_amount, " +
                    "@row_num := @row_num + 1 AS row_num " +
                    "FROM " +
                    "(SELECT @row_num := 0) AS init, " +
                    "(SELECT " +
                    "revenue_name, " +
                    "revenue_amount " +
                    "FROM " +
                    "sys_mst_tdaytrackerdtl " +
                    "WHERE " +
                    "trade_date = '" + formattedDate + "' AND campaign_gid = '" + lscampaignGid + "' " +
                    "ORDER BY " +
                    "trade_date) AS rd) AS revenue_data " +
                    "LEFT JOIN " +
                    "(SELECT " +
                    "expense_name, " +
                    "expense_amount, " +
                    "@row_num2 := @row_num2 + 1 AS row_num " +
                    "FROM " +
                    "(SELECT @row_num2 := 0) AS init, " +
                    "(SELECT " +
                    "expense_name, " +
                    "expense_amount " +
                    "FROM " +
                    "sys_mst_tdaytrackerdtl " +
                    "WHERE " +
                    "trade_date = '" + formattedDate + "' AND campaign_gid = '" + lscampaignGid + "' " +
                    "ORDER BY " +
                    "trade_date) AS ed) AS expense_data " +
                    "ON " +
                    "revenue_data.row_num = expense_data.row_num " +
                    "ORDER BY " +
                    "COALESCE(revenue_data.row_num, expense_data.row_num)";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<revenue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new revenue_list
                        {
                            revenue_name = dt["revenue_name"].ToString(),
                            expense_name = dt["expense_name"].ToString(),
                            revenue_amount = dt["revenue_amount"].ToString(),
                            expense_amount = dt["expense_amount"].ToString()
                        });
                        values.RevenueData_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
        }
        public void DaGetTodayReportDetails(string user_gid, MdlOutletDashboard values)
        {
            string currentDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string previousDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");

            string msSQL = @"
                    SELECT 
                    a.campaign_gid, 
                   'Today' AS period, 
                    DATE_FORMAT(a.trade_date, '%d-%m-%Y') AS created_date, 
                    b.campaign_description,
                    CONCAT(FORMAT(SUM(a.revenue_amount), 2, 'en_IN')) AS revenue_amount, 
                    CONCAT(FORMAT(SUM(a.expense_amount), 2, 'en_IN')) AS expense_amount,
                    CASE 
                    WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(0, 2, 'en_IN')) 
                    ELSE CONCAT(FORMAT(SUM(a.revenue_amount) - SUM(a.expense_amount), 2, 'en_IN')) 
                    END AS profit_amount, 
                    CASE 
                    WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)), 2, 'en_IN')) 
                    ELSE CONCAT(FORMAT(0, 2, 'en_IN')) 
                    END AS loss_amount
                    FROM 
                    sys_mst_tdaytrackerdtl a 
                    LEFT JOIN 
                    otl_trn_touletcampaign b ON a.campaign_gid = b.campaign_gid 
                    WHERE 
                    a.trade_date = '" + currentDate + @"'
                    GROUP BY 
        a.campaign_gid 

    UNION ALL 

    SELECT 
        a.campaign_gid, 
        'Yesterday' AS period, 
        DATE_FORMAT(a.trade_date, '%d-%m-%Y') AS created_date, 
        b.campaign_description, 
        CONCAT(FORMAT(SUM(a.revenue_amount), 2, 'en_IN')) AS revenue_amount, 
        CONCAT(FORMAT(SUM(a.expense_amount), 2, 'en_IN')) AS expense_amount, 
        CASE 
            WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(0, 2, 'en_IN')) 
            ELSE CONCAT(FORMAT(SUM(a.revenue_amount) - SUM(a.expense_amount), 2, 'en_IN')) 
        END AS profit_amount, 
        CASE 
            WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)), 2, 'en_IN')) 
            ELSE CONCAT(FORMAT(0, 2, 'en_IN')) 
        END AS loss_amount 
    FROM 
        sys_mst_tdaytrackerdtl a 
    LEFT JOIN 
        otl_trn_touletcampaign b ON a.campaign_gid = b.campaign_gid 
    WHERE 
        a.trade_date = '" + previousDate + @"' 
    GROUP BY 
        a.campaign_gid 

    UNION ALL 

    SELECT 
        NULL AS campaign_gid, 
        'Overall Today' AS period, 
        DATE_FORMAT(a.trade_date, '%d-%m-%Y') AS created_date, 
        NULL AS campaign_description,
        CONCAT(FORMAT(SUM(a.revenue_amount), 2, 'en_IN')) AS revenue_amount, 
        CONCAT(FORMAT(SUM(a.expense_amount), 2, 'en_IN')) AS expense_amount,
        CASE 
            WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(0, 2, 'en_IN')) 
            ELSE CONCAT(FORMAT(SUM(a.revenue_amount) - SUM(a.expense_amount), 2, 'en_IN')) 
        END AS profit_amount, 
        CASE 
            WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)), 2, 'en_IN')) 
            ELSE CONCAT(FORMAT(0, 2, 'en_IN')) 
        END AS loss_amount
    FROM 
        sys_mst_tdaytrackerdtl a 
    WHERE 
        a.trade_date = '" + currentDate + @"'

    UNION ALL 

    SELECT 
        NULL AS campaign_gid, 
        'Overall Yesterday' AS period, 
        DATE_FORMAT(a.trade_date, '%d-%m-%Y') AS created_date, 
        NULL AS campaign_description,
        CONCAT(FORMAT(SUM(a.revenue_amount), 2, 'en_IN')) AS revenue_amount, 
        CONCAT(FORMAT(SUM(a.expense_amount), 2, 'en_IN')) AS expense_amount,
        CASE 
            WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(0, 2, 'en_IN')) 
            ELSE CONCAT(FORMAT(SUM(a.revenue_amount) - SUM(a.expense_amount), 2, 'en_IN')) 
        END AS profit_amount, 
        CASE 
            WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN CONCAT(FORMAT(ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)), 2, 'en_IN')) 
            ELSE CONCAT(FORMAT(0, 2, 'en_IN')) 
        END AS loss_amount 
    FROM 
        sys_mst_tdaytrackerdtl a 
    WHERE 
        a.trade_date = '" + previousDate + @"'";

            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<dailytodayreport_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new dailytodayreport_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_description = dt["campaign_description"].ToString(),
                        period = dt["period"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        revenue_amount = dt["revenue_amount"].ToString(),
                        expense_amount = dt["expense_amount"].ToString(),
                        profit_amount = dt["profit_amount"].ToString(),
                        loss_amount = dt["loss_amount"].ToString()
                    });
                }
                values.TodayReportData_list = getModuleList;
            }
            dt_datatable.Dispose();
        }




        public void DaGetYesterdayReportDetails(string user_gid, MdlOutletDashboard values)
        {

            DateTime yesterday = DateTime.Now.AddDays(-1);
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");


            string msSQL = "SELECT " +
                           "SUM(a.revenue_amount) AS revenue_amount, " +
                           "SUM(a.expense_amount) AS expense_amount, " +
                           "b.campaign_description, " +
                           "CASE " +
                           "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount)) END AS profit_amount, " +
                           "CASE " +
                           "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) ELSE 0 END AS loss_amount " +
                           "FROM sys_mst_tdaytrackerdtl a " +
                           "LEFT JOIN otl_trn_touletcampaign b ON b.campaign_gid = a.campaign_gid " +
                           "WHERE a.created_date = '" + yesterdayDate + "' " +
                           "GROUP BY a.campaign_gid " +
                           "ORDER BY a.created_date";

            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<dailyyesterdayreport_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new dailyyesterdayreport_list
                    {
                        campaign_description = dt["campaign_description"].ToString(),
                        revenue_amount = dt["revenue_amount"].ToString(),
                        expense_amount = dt["expense_amount"].ToString(),
                        profit_amount = dt["profit_amount"].ToString(),
                        loss_amount = dt["loss_amount"].ToString()
                    });
                }
                values.YesterdayReportData_list = getModuleList;
            }
            dt_datatable.Dispose();
        }
        public void DaGetCustomReportDetails(string user_gid, MdlOutletDashboard values, string fromDate, string toDate)
        {
            string msSQL = "SELECT " +
                "SUM(a.revenue_amount) AS revenue_amount, " +
                "SUM(a.expense_amount) AS expense_amount, date_format(a.trade_date,'%d-%m-%Y') as created_date, " +
                "b.campaign_description, " +
                "CASE " +
                "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount)) END AS profit_amount, " +
                "CASE " +
                "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) ELSE 0 END AS loss_amount " +
                "FROM sys_mst_tdaytrackerdtl a " +
                "LEFT JOIN otl_trn_touletcampaign b ON b.campaign_gid = a.campaign_gid " +
                "WHERE a.trade_date BETWEEN '" + fromDate + "' AND '" + toDate + "' " +
                "GROUP BY a.trade_date";

            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customreport_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customreport_list
                    {
                        created_date = dt["created_date"].ToString(),
                        revenue_amount = dt["revenue_amount"].ToString(), 
                        expense_amount = dt["expense_amount"].ToString(),
                        profit_amount = dt["profit_amount"].ToString(),  
                        loss_amount = dt["loss_amount"].ToString()
                    });
                }
                values.CustomReportData_list = getModuleList;
            }
            dt_datatable.Dispose();
        }

        public void DaGetCustomDetailReportDetails(string user_gid, MdlOutletDashboard values, string created_date)
        {
          
            DateTime parsedDate = DateTime.ParseExact(created_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

           
            string formattedDate = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");


            string msSQL = "SELECT " +
                           "CONCAT(FORMAT(SUM(a.revenue_amount), 2, 'en_IN')) AS revenue_amount, " +
                           "CONCAT(FORMAT(SUM(a.expense_amount), 2, 'en_IN')) AS expense_amount, " +
                           "DATE_FORMAT(a.trade_date, '%d-%m-%Y') AS created_date, " +
                           "b.campaign_description, " +
                           "CASE " +
                           "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN " +
                           "CONCAT(FORMAT(0, 2, 'en_IN')) " +
                           "ELSE CONCAT(FORMAT(SUM(a.revenue_amount) - SUM(a.expense_amount), 2, 'en_IN')) END AS profit_amount, " +
                           "CASE " +
                           "WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN " +
                           "CONCAT(FORMAT(ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)), 2, 'en_IN')) " +
                           "ELSE CONCAT(FORMAT(0, 2, 'en_IN')) END AS loss_amount " +
                           "FROM sys_mst_tdaytrackerdtl a " +
                           "LEFT JOIN otl_trn_touletcampaign b ON b.campaign_gid = a.campaign_gid " +
                           "WHERE a.trade_date = '" + formattedDate + "' " +
                           "GROUP BY a.campaign_gid";


            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customdetailreport_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customdetailreport_list
                    {
                        campaign_description = dt["campaign_description"].ToString(),
                        revenue_amount = dt["revenue_amount"].ToString(),
                        expense_amount = dt["expense_amount"].ToString(),
                        profit_amount = dt["profit_amount"].ToString(),
                        loss_amount = dt["loss_amount"].ToString()
                    });
                }
                values.CustomReportDetail_List = getModuleList;
            }
            dt_datatable.Dispose();
        }

        public void DaGetOverallOutletdailyReport(string user_gid, MdlOutletDashboard values)
        {

            string previousDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string previousDate1 = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");


            string msSQL = "SELECT a.campaign_gid, " +
                           "'Today' AS period, date_format(a.trade_date,'%d-%m-%Y') as created_date, b.campaign_description, " +
                           "SUM(a.revenue_amount) AS revenue_amount, " +
                           "SUM(a.expense_amount) AS expense_amount, " +
                           "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 " +
                           "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount)) END AS profit_amount, " +
                           "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) " +
                           "ELSE 0 END AS loss_amount " +
                           "FROM sys_mst_tdaytrackerdtl a " +
                           "LEFT JOIN otl_trn_touletcampaign b ON a.campaign_gid = b.campaign_gid " +
                           "WHERE a.trade_date = '" + previousDate + "' " +
                           "GROUP BY a.campaign_gid " +
                           "UNION ALL " +
                           "SELECT a.campaign_gid, " +
                           "'Yesterday' AS period, date_format(a.trade_date,'%d-%m-%Y') as created_date, b.campaign_description, " +
                           "SUM(a.revenue_amount) AS revenue_amount, " +
                           "SUM(a.expense_amount) AS expense_amount, " +
                           "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN 0 " +
                           "ELSE (SUM(a.revenue_amount) - SUM(a.expense_amount)) END AS profit_amount, " +
                           "CASE WHEN (SUM(a.revenue_amount) - SUM(a.expense_amount)) < 0 THEN ABS(SUM(a.revenue_amount) - SUM(a.expense_amount)) " +
                           "ELSE 0 END AS loss_amount " +
                           "FROM sys_mst_tdaytrackerdtl a " +
                           "LEFT JOIN otl_trn_touletcampaign b ON a.campaign_gid = b.campaign_gid " +
                           "WHERE a.trade_date = '" + previousDate1 + "' " +
                           "GROUP BY a.campaign_gid";
        }


    }
}