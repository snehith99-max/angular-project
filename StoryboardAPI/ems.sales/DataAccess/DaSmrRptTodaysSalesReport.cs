﻿using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Remoting;
using System.Drawing;
using System.Diagnostics;


namespace ems.sales.DataAccess
{

    public class DaSmrRptTodaysSalesReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        DataTable dt_datatable1;
        string msGetGid;
        int mnResult, mnResult1;

        public void DaGetDaySalesReportCount(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
               
                msSQL = " Select Now() as date ";
            values.today_date = objdbconn.GetExecuteScalar(msSQL);

            //Today's Sales Report Count

            msSQL = " SELECT count(salesorder_gid) as total_so FROM smr_trn_tsalesorder " +
                    " where salesorder_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and so_type='Sales' ";
            values.today_total_so = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(directorder_gid) as total_do FROM smr_trn_tdeliveryorder " +
                    " where directorder_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            values.today_total_do = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(invoice_gid) as total_invoice FROM rbl_trn_tinvoice " +
                    " where invoice_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  and invoice_from='Sales'";
            values.today_total_invoice = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  SELECT count(payment_gid) as total_payment FROM rbl_trn_tpayment " +
                   " where payment_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
            values.today_total_payment = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  select ifnull(format(sum(invoice_amount*exchange_rate),2),'0.00') as invoice_amount from rbl_trn_tinvoice " +
                   " where invoice_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and invoice_from='Sales'";
            values.today_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format(sum(amount*exchange_rate),2),'0.00') as payment_amount from rbl_trn_tpayment " +
                 " where payment_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            values.today_payment_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format((sum(invoice_amount*exchange_rate)-sum(payment_amount*exchange_rate)),2),'0.00') as outstanding_amount from rbl_trn_tinvoice " +
                 " where invoice_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and invoice_from='Sales'";
            values.today_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);

            //Yesterday's Sales Report Count


            DateTime todayDate = DateTime.Now;
            DateTime yesDate = todayDate.AddDays(-1);

            msSQL = " SELECT count(salesorder_gid) as total_so FROM smr_trn_tsalesorder " +
                    " where salesorder_date='" + yesDate.ToString("yyyy-MM-dd") + "' and so_type='Sales' ";
            values.yest_total_so = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(directorder_gid) as total_do FROM smr_trn_tdeliveryorder " +
                    " where directorder_date='" + yesDate.ToString("yyyy-MM-dd") + "' ";
            values.yest_total_do = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(invoice_gid) as total_invoice FROM rbl_trn_tinvoice " +
                    " where invoice_date='" + yesDate.ToString("yyyy-MM-dd") + "'  and invoice_from='Sales'";
            values.yest_total_invoice = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  SELECT count(payment_gid) as total_payment FROM rbl_trn_tpayment " +
                   " where payment_date='" + yesDate.ToString("yyyy-MM-dd") + "' ";
            values.yest_total_payment = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  select ifnull(format(sum(invoice_amount*exchange_rate),2),'0.00') as invoice_amount from rbl_trn_tinvoice " +
                   " where invoice_date='" + yesDate.ToString("yyyy-MM-dd") + "' and invoice_from='Sales'";
            values.yest_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format(sum(amount*exchange_rate),2),'0.00') as payment_amount from rbl_trn_tpayment " +
                 " where payment_date='" + yesDate.ToString("yyyy-MM-dd") + "'";
            values.yest_payment_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format((sum(invoice_amount*exchange_rate)-sum(payment_amount*exchange_rate)),2),'0.00') as outstanding_amount from rbl_trn_tinvoice " +
                 " where invoice_date='" + yesDate.ToString("yyyy-MM-dd") + "' and invoice_from='Sales'";
            values.yest_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Sales Order Report Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //Current Week's Sales Report Count

       public void DaGetWeekSalesReportCount(string employee_gid, MdlSmrRptTodaysSalesReport values)
{
    try
    {
        DateTime today = DateTime.Today;
        int currentYear = today.Year;

        // Current Week
        DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        msSQL = "SELECT COUNT(salesorder_gid) AS total_so FROM smr_trn_tsalesorder " +
                "WHERE YEAR(salesorder_date) = " + currentYear + " " +
                "AND salesorder_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND salesorder_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND so_type = 'Sales'";
        values.cw_total_so = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT COUNT(directorder_gid) AS total_do FROM smr_trn_tdeliveryorder " +
                "WHERE YEAR(directorder_date) = " + currentYear + " " +
                "AND directorder_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "and directorder_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "'";
        values.cw_total_do = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT COUNT(invoice_gid) AS total_invoice FROM rbl_trn_tinvoice " +
                "WHERE YEAR(invoice_date) = " + currentYear + " " +
                "AND invoice_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_from = 'Sales'";
        values.cw_total_invoice = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT COUNT(payment_gid) AS total_payment FROM rbl_trn_tpayment " +
                "WHERE YEAR(payment_date) = " + currentYear + " " +
                "AND payment_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "and payment_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "'";
        values.cw_total_payment = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT IFNULL(FORMAT(SUM(invoice_amount * exchange_rate), 2), '0.00') AS invoice_amount FROM rbl_trn_tinvoice " +
                "WHERE YEAR(invoice_date) = " + currentYear + " " +
                "AND invoice_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_from = 'Sales'";
        values.cw_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT IFNULL(FORMAT(SUM(amount * exchange_rate), 2), '0.00') AS payment_amount FROM rbl_trn_tpayment " +
                "WHERE YEAR(payment_date) = " + currentYear + " " +
                "AND payment_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "and payment_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "'";
        values.cw_payment_amount = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT IFNULL(FORMAT((SUM(invoice_amount * exchange_rate) - SUM(payment_amount * exchange_rate)), 2), '0.00') AS outstanding_amount FROM rbl_trn_tinvoice " +
                "WHERE YEAR(invoice_date) = " + currentYear + " " +
                "AND invoice_date >= '" + startOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_date <= '" + endOfWeek.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_from = 'Sales'";
        values.cw_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);

        // Last Week's Sales Report Count
        DateTime lastWeekStart = startOfWeek.AddDays(-7);
        DateTime lastWeekEnd = endOfWeek.AddDays(-7);

        msSQL = "SELECT COUNT(salesorder_gid) AS total_so FROM smr_trn_tsalesorder " +
                "WHERE YEAR(salesorder_date) = " + currentYear + " " +
                "AND salesorder_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "AND salesorder_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "' " +
                "AND so_type = 'Sales'";
        values.lw_total_so = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT COUNT(directorder_gid) AS total_do FROM smr_trn_tdeliveryorder " +
                "WHERE YEAR(directorder_date) = " + currentYear + " " +
                "AND directorder_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "and directorder_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "'";
        values.lw_total_do = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT COUNT(invoice_gid) AS total_invoice FROM rbl_trn_tinvoice " +
                "WHERE YEAR(invoice_date) = " + currentYear + " " +
                "AND invoice_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_from = 'Sales'";
        values.lw_total_invoice = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT COUNT(payment_gid) AS total_payment FROM rbl_trn_tpayment " +
                "WHERE YEAR(payment_date) = " + currentYear + " " +
                "AND payment_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "and payment_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "'";
        values.lw_total_payment = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT IFNULL(FORMAT(SUM(invoice_amount * exchange_rate), 2), '0.00') AS invoice_amount FROM rbl_trn_tinvoice " +
                "WHERE YEAR(invoice_date) = " + currentYear + " " +
                "AND invoice_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_from = 'Sales'";
        values.lw_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT IFNULL(FORMAT(SUM(amount * exchange_rate), 2), '0.00') AS payment_amount FROM rbl_trn_tpayment " +
                "WHERE YEAR(payment_date) = " + currentYear + " " +
                "AND payment_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "and payment_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "'";
        values.lw_payment_amount = objdbconn.GetExecuteScalar(msSQL);

        msSQL = "SELECT IFNULL(FORMAT((SUM(invoice_amount * exchange_rate) - SUM(payment_amount * exchange_rate)), 2), '0.00') AS outstanding_amount FROM rbl_trn_tinvoice " +
                "WHERE YEAR(invoice_date) = " + currentYear + " " +
                "AND invoice_date >= '" + lastWeekStart.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_date <= '" + lastWeekEnd.ToString("yyyy-MM-dd") + "' " +
                "AND invoice_from = 'Sales'";
        values.lw_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);
    }
    catch (Exception ex)
    {
        values.message = "Exception occurred while loading Week Sales Report Count!";
        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
            $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
            values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        //Current Month's Sales Report Count
        static DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        static DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
        public void DaGetMonthSalesReportCount(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
                
                DateTime now = DateTime.Now;
                int currentYear = now.Year;

                DateTime currMonthStart = GetFirstDayOfMonth(now);
            DateTime currMonthEnd = GetLastDayOfMonth(now);

            msSQL = " SELECT count(salesorder_gid) as total_so FROM smr_trn_tsalesorder " +
                     "WHERE YEAR(salesorder_date) = " + currentYear + " " +
                  " and salesorder_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "'" +
                " and salesorder_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "'  " +
                "and so_type='Sales' ";
            values.cm_total_so = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(directorder_gid) as total_do FROM smr_trn_tdeliveryorder " +
                     "WHERE YEAR(directorder_date) = " + currentYear + " " +
                    " and directorder_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "'" +
                  " and directorder_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "' ";
            values.cm_total_do = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(invoice_gid) as total_invoice FROM rbl_trn_tinvoice " +
                    "WHERE YEAR(invoice_date) = " + currentYear + " " +
                    " and (invoice_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "')" +
                  " and (invoice_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "')  " +
                  "and invoice_from='Sales'";
            values.cm_total_invoice = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  SELECT count(payment_gid) as total_payment FROM rbl_trn_tpayment " +
                     "WHERE YEAR(payment_date) = " + currentYear + " " +
                   " and payment_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "'" +
                  " and payment_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "'  ";
            values.cm_total_payment = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  select ifnull(format(sum(invoice_amount*exchange_rate),2),'0.00') as invoice_amount from rbl_trn_tinvoice " +
                      "WHERE YEAR(invoice_date) = " + currentYear + " " +
                    " and (invoice_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "')" +
                  " and (invoice_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "') and invoice_from='Sales'";
            values.cm_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format(sum(amount*exchange_rate),2),'0.00') as payment_amount from rbl_trn_tpayment " +
                    "WHERE YEAR(payment_date) = " + currentYear + " " +
                    " and payment_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "'" +
                  " and payment_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "' ";
            values.cm_payment_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format((sum(invoice_amount*exchange_rate)-sum(payment_amount*exchange_rate)),2),'0.00') as outstanding_amount from rbl_trn_tinvoice " +
                  "WHERE YEAR(invoice_date) = " + currentYear + " " +
                    " and invoice_date>='" + currMonthStart.ToString("yyyy-MM-dd") + "'" +
                  " and invoice_date<='" + currMonthEnd.ToString("yyyy-MM-dd") + "'  and invoice_from='Sales'";
            values.cm_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);

                //Previous Month's Sales Report Count

                DateTime thisMonth = new DateTime(currentYear, now.Month, 1);
                DateTime firstDayLastMonth = thisMonth.AddMonths(-1);
                DateTime lastDayLastMonth = thisMonth.AddDays(-1);

            msSQL = " SELECT count(salesorder_gid) as total_so FROM smr_trn_tsalesorder " +
                     "WHERE YEAR(salesorder_date) = " + currentYear + " " +
                  " and salesorder_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "'" +
                " and salesorder_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "'  and so_type='Sales' ";
            values.lm_total_so = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(directorder_gid) as total_do FROM smr_trn_tdeliveryorder " +
                     "WHERE YEAR(directorder_date) = " + currentYear + " " +
                    " and directorder_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "'" +
                  " and directorder_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "' ";
            values.lm_total_do = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT count(invoice_gid) as total_invoice FROM rbl_trn_tinvoice " +
                     "WHERE YEAR(invoice_date) = " + currentYear + " " +
                    " and (invoice_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "')" +
                  " and (invoice_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "')  and invoice_from='Sales'";
            values.lm_total_invoice = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  SELECT count(payment_gid) as total_payment FROM rbl_trn_tpayment " +
                     "WHERE YEAR(payment_date) = " + currentYear + " " +
                   " and payment_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "'" +
                  " and payment_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "'  ";
            values.lm_total_payment = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "  select ifnull(format(sum(invoice_amount*exchange_rate),2),'0.00') as invoice_amount from rbl_trn_tinvoice " +
                   "WHERE YEAR(invoice_date) = " + currentYear + " " +
                    " and (invoice_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "')" +
                  " and (invoice_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "')  and invoice_from='Sales'";
            values.lm_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format(sum(amount*exchange_rate),2),'0.00') as payment_amount from rbl_trn_tpayment " +
                 "WHERE YEAR(payment_date) = " + currentYear + " " +
                    " and payment_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "'" +
                  " and payment_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "' ";
            values.lm_payment_amount = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select ifnull(format((sum(invoice_amount*exchange_rate)-sum(payment_amount*exchange_rate)),2),'0.00') as outstanding_amount from rbl_trn_tinvoice " +
                  "WHERE YEAR(invoice_date) = " + currentYear + " " +
                    " and invoice_date>='" + firstDayLastMonth.ToString("yyyy-MM-dd") + "'" +
                  " and invoice_date<='" + lastDayLastMonth.ToString("yyyy-MM-dd") + "'  and invoice_from='Sales'";
            values.lm_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Monthly Sales Report Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetYearSalesReportCount(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                // Get the start date of the current year
                DateTime startDate = new DateTime(currentDate.Year, 1, 1);

                // Get the end date of the current year
                DateTime endDate = new DateTime(currentDate.Year, 12, 31);

                string msSQL = "SELECT COUNT(salesorder_gid) AS total_so FROM smr_trn_tsalesorder " +
                               "WHERE salesorder_date >= '" + startDate.ToString("yyyy-MM-dd") + "' " +
                               "and salesorder_date <= '" + endDate.ToString("yyyy-MM-dd") + "' AND so_type = 'Sales'";
                values.cy_total_so = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT COUNT(directorder_gid) AS total_do FROM smr_trn_tdeliveryorder " +
                        "WHERE directorder_date >= '" + startDate.ToString("yyyy-MM-dd") + "' " +
                        "and directorder_date <= '" + endDate.ToString("yyyy-MM-dd") + "'";
                values.cy_total_do = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT COUNT(invoice_gid) AS total_invoice FROM rbl_trn_tinvoice " +
                        "WHERE (invoice_date >= '" + startDate.ToString("yyyy-MM-dd") + "') " +
                        "and (invoice_date <= '" + endDate.ToString("yyyy-MM-dd") + "') AND invoice_from = 'Sales'";
                values.cy_total_invoice = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT COUNT(payment_gid) AS total_payment FROM rbl_trn_tpayment " +
                        "WHERE payment_date >= '" + startDate.ToString("yyyy-MM-dd") + "' " +
                        "and payment_date <= '" + endDate.ToString("yyyy-MM-dd") + "'";
                values.cy_total_payment = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT IFNULL(FORMAT(SUM(invoice_amount*exchange_rate), 2), '0.00') AS invoice_amount FROM rbl_trn_tinvoice " +
                        "WHERE (invoice_date >= '" + startDate.ToString("yyyy-MM-dd") + "') " +
                        "and (invoice_date <= '" + endDate.ToString("yyyy-MM-dd") + "') AND invoice_from = 'Sales'";
                values.cy_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT IFNULL(FORMAT(SUM(amount*exchange_rate), 2), '0.00') AS payment_amount FROM rbl_trn_tpayment " +
                        "WHERE payment_date >= '" + startDate.ToString("yyyy-MM-dd") + "' " +
                        "and payment_date <= '" + endDate.ToString("yyyy-MM-dd") + "'";
                values.cy_payment_amount = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT IFNULL(FORMAT((SUM(invoice_amount*exchange_rate)-SUM(payment_amount*exchange_rate)), 2), '0.00') AS outstanding_amount FROM rbl_trn_tinvoice " +
                        "WHERE invoice_date >= '" + startDate.ToString("yyyy-MM-dd") + "' " +
                        "and invoice_date <= '" + endDate.ToString("yyyy-MM-dd") + "' AND invoice_from = 'Sales'";
                values.cy_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);

                // Previous Year's Sales Report Count
                // Get the start date of the previous year
                DateTime previousYearStartDate = startDate.AddYears(-1);

                // Get the end date of the previous year
                DateTime previousYearEndDate = endDate.AddYears(-1);

                msSQL = "SELECT COUNT(salesorder_gid) AS total_so FROM smr_trn_tsalesorder " +
                        "WHERE salesorder_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "' " +
                        "and salesorder_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "' AND so_type = 'Sales'";
                values.ly_total_so = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT COUNT(directorder_gid) AS total_do FROM smr_trn_tdeliveryorder " +
                        "WHERE directorder_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "' " +
                        "and directorder_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "'";
                values.ly_total_do = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT COUNT(invoice_gid) AS total_invoice FROM rbl_trn_tinvoice " +
                        "WHERE (invoice_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "') " +
                        "and (invoice_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "') AND invoice_from = 'Sales'";
                values.ly_total_invoice = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT COUNT(payment_gid) AS total_payment FROM rbl_trn_tpayment " +
                        "WHERE payment_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "' " +
                        "and payment_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "'";
                values.ly_total_payment = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT IFNULL(FORMAT(SUM(invoice_amount*exchange_rate), 2), '0.00') AS invoice_amount FROM rbl_trn_tinvoice " +
                        "WHERE (invoice_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "') " +
                        "and (invoice_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "') AND invoice_from = 'Sales'";
                values.ly_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT IFNULL(FORMAT(SUM(amount*exchange_rate), 2), '0.00') AS payment_amount FROM rbl_trn_tpayment " +
                        "WHERE payment_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "' " +
                        "and payment_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "'";
                values.ly_payment_amount = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT IFNULL(FORMAT((SUM(invoice_amount*exchange_rate)-SUM(payment_amount*exchange_rate)), 2), '0.00') AS outstanding_amount FROM rbl_trn_tinvoice " +
                        "WHERE invoice_date >= '" + previousYearStartDate.ToString("yyyy-MM-dd") + "' " +
                        "and invoice_date <= '" + previousYearEndDate.ToString("yyyy-MM-dd") + "' AND invoice_from = 'Sales'";
                values.ly_outstanding_amount = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Yearly Sales Order Report Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                    values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetTodaySalesReport(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
               
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
            string currency = objdbconn.GetExecuteScalar(msSQL);

        

            msSQL = "  select distinct a.salesorder_gid, a.so_referenceno1, a.salesorder_date,m.branch_name,c.user_firstname,    " +
                 "  a.customer_contact_person, a.salesorder_status,a.currency_code,format(a.addon_charge, 2) as addon_charge,   " +
                " format(a.additional_discount, 2) as additional_discount,  " +
                "  case when a.grandtotal_l = '0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l, 2) end as Grandtotal,format(a.Grandtotal, 2) as grandtotal_dtl, " +
               "  if (a.payment_days = '','Nil',a.payment_days)  as payment_days,if (a.delivery_days = '','Nil',a.delivery_days)  as delivery_days, " +
                " case when a.currency_code = '  currency  ' then a.customer_name " +
                  "  when a.currency_code is null then a.customer_name" +
                 " when a.currency_code is not null and a.currency_code<> '  currency  ' then concat(a.customer_name,' / ',h.country) end as customer_name,  " +
               "  case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email)   " +
               "  when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact , " +
               "  s.salesorderdtl_gid,s.salesorder_gid,s.product_gid,s.productgroup_gid,s.productgroup_name,s.product_name,format(s.product_price, 2) as product_price,s.qty_quoted, " +
                       "  format(s.discount_percentage, 2) as discount_percentage,format(s.discount_amount, 2) as discount_amount ,format(s.tax_percentage, 2) as tax_percentage,  " +
                        " format(s.tax_amount, 2) as tax_amount , s.product_remarks,  s.uom_gid, s.uom_name, s.payment_days, s.delivery_period, format(s.price, 2) as price , " + 
                       "  s.display_field,s.tax_name,s.tax_name2,s.tax_name3,format(s.tax_percentage2, 2) as tax_percentage2 ,format(s.tax_percentage3, 2) as tax_percentage3  , " +
                       "  format(s.tax_amount2, 2) as tax_amount2 , format(s.tax_amount3, 2) as tax_amount3 , s.salesorder_refno, s.product_delivered, s.qty_executed " +
                        "  from smr_trn_tsalesorder a " +
                        " left join crm_mst_tcustomer d on a.customer_gid = d.customer_gid " +
                        "  left join smr_trn_tsalesorderdtl s on s.salesorder_gid = a.salesorder_gid " +
                       "  left join crm_mst_tcustomercontact e on d.customer_gid = e.customer_gid " +
                       "  left join hrm_mst_temployee b on b.employee_gid = a.created_by " +
                       "  left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                       "  left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                       "  left join hrm_mst_tbranch m on a.branch_gid = m.branch_gid " +


                        " where salesorder_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";


            //" where salesorder_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetTodaySalesReport_List = new List<GetTodaySalesReport_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetTodaySalesReport_List.Add(new GetTodaySalesReport_List
                    {
                        so_referenceno1 = (dt["so_referenceno1"].ToString()),
                        salesorder_date = (dt["salesorder_date"].ToString()),
                        branch_name = (dt["branch_name"].ToString()),
                        user_firstname = (dt["user_firstname"].ToString()),
                        customer_contact_person = (dt["customer_contact_person"].ToString()),
                        salesorder_status = (dt["salesorder_status"].ToString()),
                        currency_code = (dt["currency_code"].ToString()),
                        addon_charge = (dt["addon_charge"].ToString()),
                        additional_discount = (dt["additional_discount"].ToString()),
                        Grandtotal = (dt["Grandtotal"].ToString()),
                        grandtotal_dtl = (dt["grandtotal_dtl"].ToString()),
                        payment_days = (dt["payment_days"].ToString()),
                        delivery_days = (dt["delivery_days"].ToString()),
                        customer_name = (dt["customer_name"].ToString()),
                        contact = (dt["contact"].ToString()),
                        productgroup_name = (dt["productgroup_name"].ToString()),
                        product_name = (dt["product_name"].ToString()),
                        uom_name = (dt["uom_name"].ToString()),
                        qty_quoted = (dt["qty_quoted"].ToString()),
                        price = (dt["price"].ToString()),
                        product_price = (dt["product_price"].ToString()),


                    });
                    values.GetTodaySalesReport_List = GetTodaySalesReport_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Today Sales Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetTodayDeliveryOrderReport(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
               
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
            string currency = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select a.directorder_gid, a.directorder_refno, a.directorder_date,d.branch_name, " +
                " a.customer_name, a.customer_branchname, a.customer_contactperson, a.directorder_status,a.delivery_status, " +
                " concat(CAST(date_format(a.delivered_date,'%d-%m-%Y') as CHAR),'/',a.delivered_to) as delivery_details, " +
                " case when a.customer_contactnumber is null then  concat(c.customercontact_name,' / ',c.mobile,' / ',c.email) " +
                " when a.customer_contactnumber is not null then concat(a.customer_contactperson,' / ',a.customer_contactnumber,' / ',a.customer_emailid) end as contact " +
                " from smr_trn_tdeliveryorder a " +
                " left join smr_trn_tsalesorder b on b.salesorder_gid=a.salesorder_gid" +
                " left join hrm_mst_tbranch d on b.branch_gid=d.branch_gid" +
                " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid " +
                " where a.directorder_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetTodayDeliveryOrderReport_List = new List<GetTodayDeliveryOrderReport_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetTodayDeliveryOrderReport_List.Add(new GetTodayDeliveryOrderReport_List
                    {
                        directorder_gid = (dt["directorder_gid"].ToString()),
                        directorder_refno = (dt["directorder_refno"].ToString()),
                        directorder_date = (dt["directorder_date"].ToString()),
                        branch_name = (dt["branch_name"].ToString()),
                        customer_branchname = (dt["customer_branchname"].ToString()),                       
                        customer_contact_person = (dt["customer_contactperson"].ToString()),
                        directorder_status = (dt["directorder_status"].ToString()),
                        delivery_status = (dt["delivery_status"].ToString()),
                        delivery_details = (dt["delivery_details"].ToString()),                       
                        customer_name = (dt["customer_name"].ToString()),
                        contact = (dt["contact"].ToString()),

                    });
                    values.GetTodayDeliveryOrderReport_List = GetTodayDeliveryOrderReport_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetTodayInvoiceReport(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
                
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
            string currency = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag,a.mail_status,i.branch_name,a.customer_gid,a.invoice_date, " +
                " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', " +
                " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                "  format(a.invoice_amount,2) as invoice_amount, d.customer_contactperson, " +
                " case when a.currency_code = '" + currency + "' then c.customer_name when a.currency_code is null then c.customer_name " +
                "  when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(c.customer_name,' / ',h.country) end as customer_name, " +
                " case when d.customer_emailid is null then concat(e.customercontact_name,' / ',e.mobile,' / ',e.email) " +
                " when d.customer_emailid is not null then concat(d.customer_contactperson,' / ',d.customer_contactnumber,' / ',d.customer_emailid) end as contact " +
                " from rbl_trn_tinvoice a " +
                " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " left join rbl_trn_tso2invoice f on f.invoice_gid=a.invoice_gid " +
                " left join smr_trn_tdeliveryorder d on d.directorder_gid = f.directorder_gid " +
                " left join smr_trn_tsalesorder g on g.salesorder_gid=d.salesorder_gid" +
                " left join hrm_mst_tbranch i on i.branch_gid=g.branch_gid" +
                " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                " left join crm_mst_tcustomercontact e on e.customer_gid=c.customer_gid " +
                " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                " where a.invoice_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetTodayInvoiceReport_List = new List<GetTodayInvoiceReport_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetTodayInvoiceReport_List.Add(new GetTodayInvoiceReport_List
                    {
                        invoice_gid = (dt["invoice_gid"].ToString()),
                        invoice_refno = (dt["invoice_refno"].ToString()),
                        invoice_status = (dt["invoice_status"].ToString()),
                        invoice_amount = (dt["invoice_amount"].ToString()),
                        invoice_date = (dt["invoice_date"].ToString()),
                        branch_name = (dt["branch_name"].ToString()),                                         
                        overall_status = (dt["overall_status"].ToString()),
                        customer_name = (dt["customer_name"].ToString()),
                        contact = (dt["contact"].ToString()),

                    });
                    values.GetTodayInvoiceReport_List = GetTodayInvoiceReport_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Today Invoice Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetTodayPaymentReport(string employee_gid, MdlSmrRptTodaysSalesReport values)
        {
            try
            {
               
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
            string currency = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select a.payment_gid,b.customer_gid, a.directorder_gid, format(a.amount,2) as amount, a.payment_mode, " +
               " a.payment_date ,a.approval_status,b.customer_gid," +
               " case when a.currency_code = '" + currency + "' then b.customer_name " +
             "  when a.currency_code is null then b.customer_name " +
             "  when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(b.customer_name,' / ',h.country) end as customer_name, " +
              " case when b.customer_contactnumber is null then  concat(d.customercontact_name,' / ',d.mobile,' / ',d.email) " +
                " when b.customer_contactnumber is not null then concat(b.customer_contactperson,' / ',b.customer_contactnumber,' / ',d.email) end as contact " +
               " from rbl_trn_tpayment a" +
                " left join rbl_trn_tinvoice b on b.invoice_gid=a.invoice_gid " +
               " left join crm_mst_tcustomer c on c.customer_gid=b.customer_gid " +
               " left join crm_mst_tcustomercontact d on d.customer_gid=b.customer_gid " +
               " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
               " where  a.payment_return='0' and a.payment_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetTodayPaymentReport_List = new List<GetTodayPaymentReport_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetTodayPaymentReport_List.Add(new GetTodayPaymentReport_List
                    {
                        payment_date = (dt["payment_date"].ToString()),
                        payment_gid = (dt["payment_gid"].ToString()),
                        customer_name = (dt["customer_name"].ToString()),
                        contact = (dt["contact"].ToString()),
                        amount = (dt["amount"].ToString()),
                        payment_mode = (dt["payment_mode"].ToString()),
                        approval_status = (dt["approval_status"].ToString()),                      

                    });
                    values.GetTodayPaymentReport_List = GetTodayPaymentReport_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Today Payment Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
    }
}