using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;


namespace ems.sales.DataAccess
{
    public class DaSmrDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult, mnResult1;
        string lsemployee_gid_list;

        ////new dashboard api 16.05.2024
        public void DaGetTilesDetails(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "select(select sum(invoice_amount) from rbl_trn_tinvoice where  MONTH(created_date) = MONTH(CURDATE())AND" +
                    " YEAR(created_date) = YEAR(CURDATE())) as month_invoiceamount,(select DATE_FORMAT(CURDATE(), '%M')) as mtd_invoice,(select count(invoice_gid) from rbl_trn_tinvoice where  MONTH(created_date) = MONTH(CURDATE())AND YEAR(created_date) = YEAR(CURDATE())) as month_invoiccount," +
                    "(select DATE_FORMAT(CURDATE(), '%Y')) as ytd_invoice,(select sum(invoice_amount) from rbl_trn_tinvoice" +
                    " where YEAR(created_date) = YEAR(CURDATE())) as ytd_invoiceamount,(select count(invoice_amount) from rbl_trn_tinvoice where YEAR(created_date) = YEAR(CURDATE())) as ytd_invoicecount," +
                    "(select b.symbol from adm_mst_tcompany a left join crm_trn_tcurrencyexchange b on a.currency_code=b.currencyexchange_gid)as  currency_symbol,(select count(customer_gid)from crm_mst_tcustomer) AS customer_count;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetTilesDetails_list = new List<GetTilesDetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetTilesDetails_list.Add(new GetTilesDetails_list
                        {
                            month_invoiceamount = (dt["month_invoiceamount"].ToString()),
                            mtd_invoice = (dt["mtd_invoice"].ToString()),
                            month_invoiccount = (dt["month_invoiccount"].ToString()),
                            ytd_invoice = (dt["ytd_invoice"].ToString()),
                            ytd_invoiceamount = (dt["ytd_invoiceamount"].ToString()),
                            ytd_invoicecount = (dt["ytd_invoicecount"].ToString()),
                            currency_symbol = (dt["currency_symbol"].ToString()),
                            customer_count = (dt["customer_count"].ToString()),


                        });
                        values.GetTilesDetails_list = GetTilesDetails_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetSalesdashboard(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "call salesdashboard";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSalesdashboard_list = new List<GetSalesdashboard_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSalesdashboard_list.Add(new GetSalesdashboard_list
                        {
                            Today_SalesOrder = (dt["Today_SalesOrder"].ToString()),
                            Today_DespatchOrder = (dt["Today_DespatchOrder"].ToString()),
                            Today_Invoice = (dt["Today_Invoice"].ToString()),
                            Today_Payment = (dt["Today_Payment"].ToString()),
                            Today_PaymentAmount = (dt["Today_PaymentAmount"].ToString()),
                            Today_InvoiceAmount = (dt["Today_InvoiceAmount"].ToString()),
                            Yesterday_SalesOrder = (dt["Yesterday_SalesOrder"].ToString()),
                            Yesterday_DespatchOrder = (dt["Yesterday_DespatchOrder"].ToString()),
                            Yesterday_Invoice = (dt["Yesterday_Invoice"].ToString()),
                            Yesterday_Payment = (dt["Yesterday_Payment"].ToString()),
                            Yesterday_PaymentAmount = (dt["Yesterday_PaymentAmount"].ToString()),
                            Yesterday_InvoiceAmount = (dt["Yesterday_InvoiceAmount"].ToString()),
                            CurrentWeek_SalesOrder = (dt["CurrentWeek_SalesOrder"].ToString()),
                            CurrentWeek_DespatchOrder = (dt["CurrentWeek_DespatchOrder"].ToString()),
                            CurrentWeek_Invoice = (dt["CurrentWeek_Invoice"].ToString()),
                            CurrentWeek_Payment = (dt["CurrentWeek_Payment"].ToString()),
                            CurrentWeek_PaymentAmount = (dt["CurrentWeek_PaymentAmount"].ToString()),
                            CurrentWeek_InvoiceAmount = (dt["CurrentWeek_InvoiceAmount"].ToString()),
                            LastWeek_SalesOrder = (dt["LastWeek_SalesOrder"].ToString()),
                            LastWeek_DespatchOrder = (dt["LastWeek_DespatchOrder"].ToString()),
                            LastWeek_Invoice = (dt["LastWeek_Invoice"].ToString()),
                            LastWeek_Payment = (dt["LastWeek_Payment"].ToString()),
                            LastWeek_InvoiceAmount = (dt["LastWeek_InvoiceAmount"].ToString()),
                            LastWeek_PaymentAmount = (dt["LastWeek_PaymentAmount"].ToString()),
                            CurrentMonth_SalesOrder = (dt["CurrentMonth_SalesOrder"].ToString()),
                            CurrentMonth_DespatchOrder = (dt["CurrentMonth_DespatchOrder"].ToString()),
                            CurrentMonth_Invoice = (dt["CurrentMonth_Invoice"].ToString()),
                            CurrentMonth_Payment = (dt["CurrentMonth_Payment"].ToString()),
                            CurrentMonth_PaymentAmount = (dt["CurrentMonth_PaymentAmount"].ToString()),
                            CurrentMonth_InvoiceAmount = (dt["CurrentMonth_InvoiceAmount"].ToString()),
                            PreviousMonth_SalesOrder = (dt["PreviousMonth_SalesOrder"].ToString()),
                            PreviousMonth_DespatchOrder = (dt["PreviousMonth_DespatchOrder"].ToString()),
                            PreviousMonth_Invoice = (dt["PreviousMonth_Invoice"].ToString()),
                            PreviousMonth_Payment = (dt["PreviousMonth_Payment"].ToString()),
                            PreviousMonth_PaymentAmount = (dt["PreviousMonth_PaymentAmount"].ToString()),
                            PreviousMonth_InvoiceAmount = (dt["PreviousMonth_InvoiceAmount"].ToString()),
                            CurrentYear_SalesOrder = (dt["CurrentYear_SalesOrder"].ToString()),
                            CurrentYear_DespatchOrder = (dt["CurrentYear_DespatchOrder"].ToString()),
                            CurrentYear_Invoice = (dt["CurrentYear_Invoice"].ToString()),
                            CurrentYear_Payment = (dt["CurrentYear_Payment"].ToString()),
                            CurrentYear_PaymentAmount = (dt["CurrentYear_PaymentAmount"].ToString()),
                            CurrentYear_InvoiceAmount = (dt["CurrentYear_InvoiceAmount"].ToString()),
                            PerivousYear_SalesOrder = (dt["PerivousYear_SalesOrder"].ToString()),
                            PerivousYear_DespatchOrder = (dt["PerivousYear_DespatchOrder"].ToString()),
                            PerivousYear_Invoice = (dt["PerivousYear_Invoice"].ToString()),
                            PerivousYear_Payment = (dt["PerivousYear_Payment"].ToString()),
                            PerivousYear_PaymentAmount = (dt["PerivousYear_PaymentAmount"].ToString()),
                            PerivousYear_InvoiceAmount = (dt["PerivousYear_InvoiceAmount"].ToString()),
                            


                        });
                        values.GetSalesdashboard_list = GetSalesdashboard_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetSalesStatus(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "SELECT Months, SUM(customer_count) AS customer_count, SUM(quotation_count) AS quotation_count, SUM(enquiry_count) AS enquiry_count," +
                    " SUM(order_count) AS order_count,sum(invoice_count) as invoice_count FROM (SELECT DATE_FORMAT(a.created_date, '%b') AS Months, " +
                    "COUNT(a.customer_gid) AS customer_count, 0 AS quotation_count, 0 AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth" +
                    " FROM crm_mst_tcustomer a WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b') AS Months," +
                    " 0 AS customer_count, COUNT(a.quotation_gid) AS quotation_count, 0 AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth" +
                    " FROM smr_trn_treceivequotation a WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b') AS Months," +
                    " 0 AS customer_count, 0 AS quotation_count, COUNT(a.enquiry_gid) AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth " +
                    "FROM smr_trn_tsalesenquiry a WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b') AS Months," +
                    " 0 AS customer_count, 0 AS quotation_count, 0 AS enquiry_count, COUNT(a.salesorder_gid) AS order_count,0 as invoice_count,created_date AS ordermonth " +
                    "FROM smr_trn_tsalesorder a WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b') AS Months," +
                    " 0 AS customer_count, 0 AS quotation_count, 0 AS enquiry_count,0 AS order_count,COUNT(a.invoice_gid) AS invoice_count,created_date AS ordermonth " +
                    "FROM rbl_trn_tinvoice a WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months) AS combined_data GROUP BY Months order by ordermonth;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSalesStatus_list = new List<GetSalesStatus_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSalesStatus_list.Add(new GetSalesStatus_list
                        {
                            customer_count = (dt["customer_count"].ToString()),
                            quotation_count = (dt["quotation_count"].ToString()),
                            enquiry_count = (dt["enquiry_count"].ToString()),
                            order_count = (dt["order_count"].ToString()),
                            invoice_count = (dt["invoice_count"].ToString()),
                            Months = (dt["Months"].ToString()),


                        });
                        values.GetSalesStatus_list = GetSalesStatus_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        
        public void DaGetPaymentandDeliveryChart(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "SELECT pd_month, SUM(p_count) AS p_count, SUM(d_count) AS d_count FROM (SELECT DATE_FORMAT(a.created_date, '%b') AS pd_month," +
                    " COUNT(a.payment_gid) AS p_count, 0 AS d_count,created_date AS ordermonth FROM rbl_trn_tpayment a " +
                    "WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY pd_month UNION ALL SELECT DATE_FORMAT(a.created_date,'%b') AS pd_month," +
                    " 0 AS p_count, COUNT(a.directorder_gid) AS d_count,created_date AS ordermonth FROM smr_trn_tdeliveryorder a " +
                    "WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY pd_month) AS combined_data GROUP BY pd_month order by ordermonth;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetPaymentandDeliveryChart_list = new List<GetPaymentandDeliveryChart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPaymentandDeliveryChart_list.Add(new GetPaymentandDeliveryChart_list
                        {
                            p_count = (dt["p_count"].ToString()),
                            d_count = (dt["d_count"].ToString()),
                            pd_month = (dt["pd_month"].ToString()),
                        });
                        values.GetPaymentandDeliveryChart_list = GetPaymentandDeliveryChart_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetDeliveryChart(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "SELECT delivery_month, SUM(delivery_completed) AS delivery_completed, SUM(delivery_pending) AS delivery_pending" +
                    " FROM (SELECT DATE_FORMAT(a.created_date, '%b') AS delivery_month, COUNT(a.directorder_gid) AS delivery_completed," +
                    " 0 AS delivery_pending,created_date AS ordermonth FROM smr_trn_tdeliveryorder a WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH" +
                    "  and a.delivery_status = 'Delivery Completed' GROUP BY delivery_month UNION ALL  SELECT DATE_FORMAT(a.created_date, '%b') AS delivery_month," +
                    " 0 AS delivery_completed, COUNT(a.directorder_gid)  AS delivery_pending,created_date AS ordermonth FROM smr_trn_tdeliveryorder a " +
                    "WHERE a.created_date >= CURDATE() - INTERVAL 6 MONTH  and a.delivery_status = 'Delivery Pending' GROUP BY delivery_month ) AS combined_data" +
                    " GROUP BY delivery_month order by ordermonth;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetPaymentandDeliveryChart_list = new List<GetPaymentandDeliveryChart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPaymentandDeliveryChart_list.Add(new GetPaymentandDeliveryChart_list
                        {

                            delivery_completed = (dt["delivery_completed"].ToString()),
                            delivery_pending = (dt["delivery_pending"].ToString()),
                            delivery_month = (dt["delivery_month"].ToString()),


                        });
                        values.GetPaymentandDeliveryChart_list = GetPaymentandDeliveryChart_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }




        // Recursive Looping ChildLoop Function to get Employee GID Hierarchywise

        public void DaGetChildLoop(string employee_gid, MdlSmrDashboard values)
        {
            try
            {
                
                msSQL = " select a.employee_gid, concat(b.user_firstname, '-', b.user_code) as user  from adm_mst_tmodule2employee a  " +
                 " inner join hrm_mst_temployee c on a.employee_gid = c.employee_gid   " +
                 " inner join adm_mst_tuser b on c.user_gid = b.user_gid   " +
                 " where a.module_gid = 'MKT'  " +
                 " and a.employeereporting_to = '" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var SalesPerformanceChart_List = new List<GetSalesPerformanceChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string lsemployee_gid = dt["employee_gid"].ToString();
                        msSQL = " select a.*, b.user_gid,c.employee_gid  from adm_mst_tmodule2employee a  " +
                    " inner join hrm_mst_temployee c on a.employee_gid = c.employee_gid  " +
                    " inner join adm_mst_tuser b on c.user_gid = b.user_gid  " +
                    " where a.module_gid = 'MKT' " +
                    " and a.employee_gid ='" + lsemployee_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            lsemployee_gid_list = lsemployee_gid + "," + dt["employee_gid"].ToString() + ",";
                            //SalesPerformanceChart_List.Add(new GetSalesPerformanceChart_List
                            //{
                            //    lsemployee_gid_list = lsemployee_gid + (dt["employee_gid"].ToString()) +",",
                            //});
                            //values.GetSalesPerformanceChart_List = SalesPerformanceChart_List;
                        }
                        DaGetChildLoop(lsemployee_gid, values);
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Performance Chart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // SalesPerformanceChart
        public void DaGetSalesPerformanceChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
                
                msSQL = " select sum(a.grandtotal) as order_amount,sum(a.received_amount) as payment_amount, sum(a.invoice_amount) as invoice_amount, " +
                     " cast(MONTHNAME(a.salesorder_date) as char) as orderdate, " +
                     " sum(a.invoice_amount-a.received_amount)  as outstanding_amount " +
                     " from smr_trn_tsalesorder a " +
                     " where a.salesorder_status in ('Delivery Completed','Approved') " +
                     " and created_by in ( '" + user_gid + "') " +
                     " and a.salesorder_date BETWEEN CURDATE() - INTERVAL 6 MONTH AND CURDATE() " +
                     " group by MONTH(salesorder_date) desc order by a.salesorder_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var SalesPerformanceChart_List = new List<GetSalesPerformanceChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        SalesPerformanceChart_List.Add(new GetSalesPerformanceChart_List
                        {
                            order_amount = (dt["order_amount"].ToString()),
                            payment_amount = (dt["payment_amount"].ToString()),
                            invoice_amount = (dt["invoice_amount"].ToString()),
                            orderdate = (dt["orderdate"].ToString()),
                            outstanding_amount = (dt["outstanding_amount"].ToString()),

                        });
                        values.GetSalesPerformanceChart_List = SalesPerformanceChart_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Performance Chart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        // GetSalesOrderCount
        public void DaGetSalesOrderCount(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {

                msSQL = "SELECT (SELECT COUNT(salesorder_gid) FROM smr_trn_tsalesorder WHERE salesorder_status IN ('Approved', 'SO Amended', 'Cancelled')" +
                    " AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS total_so, (SELECT COUNT(*) FROM smr_trn_tsalesorder c " +
                    "WHERE c.salesorder_status = 'Approved' AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS approved_so," +
                    " (SELECT COUNT(*) FROM smr_trn_tsalesorder d WHERE d.salesorder_status = 'SO Amended' AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) " +
                    "AS pending_so, (SELECT COUNT(*) FROM smr_trn_tsalesorder f WHERE f.salesorder_status = 'Cancelled' AND" +
                    " created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS rejected_so, (SELECT COUNT(directorder_gid) " +
                    "FROM smr_trn_tdeliveryorder WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS total_do, " +
                    "(SELECT COUNT(*) FROM smr_trn_tdeliveryorder WHERE delivery_status = 'Delivery Pending' AND " +
                    "created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS pending_do, (SELECT COUNT(*) FROM smr_trn_tdeliveryorder" +
                    " WHERE delivery_status = 'Delivery Completed' AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS completed_do," +
                    " (SELECT COUNT(*) FROM smr_trn_tdeliveryorder WHERE delivery_status = 'Delivery Partial Done' AND created_date >= DATE_SUB(CURDATE()," +
                    " INTERVAL 6 MONTH)) AS partial_done, (SELECT COUNT(quotation_gid) FROM smr_trn_treceivequotation WHERE created_date >= DATE_SUB(CURDATE()," +
                    " INTERVAL 6 MONTH)) AS total_quotation, (SELECT COUNT(*) FROM smr_trn_treceivequotation b WHERE b.quotation_status = 'Quotation Amended' " +
                    "AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS quotation_amended, (SELECT COUNT(*) FROM smr_trn_treceivequotation " +
                    "WHERE quotation_status = 'Quotation cancelled' AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS quotation_cancelled," +
                    " (SELECT COUNT(*) FROM smr_trn_treceivequotation WHERE quotation_status = 'Approved' AND " +
                    "created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS quotation_completed, " +
                    "(SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice WHERE invoice_flag IN ('Invoice Approved', 'Invoice Approval Pending') " +
                    "AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS totalinvoice, (SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice " +
                    "WHERE DATE(invoice_date) = CURDATE() AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS today_invoice," +
                    " (SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice c WHERE c.invoice_flag = 'Invoice Approved' AND " +
                    "created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS approved_invoice, (SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice c" +
                    " WHERE c.invoice_flag = 'Invoice Approval Pending' AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS pending_invoice," +
                    " (SELECT COUNT(invoice_gid) FROM rbl_trn_tpayment WHERE created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS totalpayment, " +
                    "(SELECT COUNT(*) FROM rbl_trn_tpayment c WHERE c.approval_status = 'Payment Done' AND created_date >= DATE_SUB(CURDATE()," +
                    " INTERVAL 6 MONTH)) AS payment_completed, (SELECT COUNT(*) FROM rbl_trn_tpayment c WHERE c.approval_status = 'Payment done Partial' " +
                    "AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS payment_done_partial, (SELECT COUNT(*) FROM rbl_trn_tinvoice c " +
                    "WHERE c.payment_amount = 0 AND created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) AS payment_pending FROM smr_trn_tsalesorder a" +
                    " LEFT JOIN rbl_trn_tinvoice b ON b.customer_gid = a.customer_gid WHERE a.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) AND" +
                    " a.salesorder_status <> 'SO Amended';";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSalesOrderCount_List = new List<GetSalesOrderCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSalesOrderCount_List.Add(new GetSalesOrderCount_List
                        {
                            total_so = (dt["total_so"].ToString()),
                            approved_so = (dt["approved_so"].ToString()),
                            pending_So = (dt["pending_So"].ToString()),
                            rejected_so = (dt["rejected_so"].ToString()),
                            total_do = (dt["total_do"].ToString()),
                            pending_do = (dt["pending_do"].ToString()),
                            completed_do = (dt["completed_do"].ToString()),
                            Partial_done = (dt["Partial_done"].ToString()),
                            total_quotation = (dt["total_quotation"].ToString()),
                            quotation_canceled = (dt["quotation_amended"].ToString()),
                            quotation_completed = (dt["quotation_completed"].ToString()),
                            totalinvoice = (dt["totalinvoice"].ToString()),
                            today_invoice = (dt["today_invoice"].ToString()),
                            approved_invoice = (dt["approved_invoice"].ToString()),
                            pending_invoice = (dt["pending_invoice"].ToString()),
                            totalpayment = (dt["totalpayment"].ToString()),
                            payment_completed = (dt["payment_completed"].ToString()),
                            payment_done_partial = (dt["payment_done_partial"].ToString()),
                            payment_pending = (dt["payment_pending"].ToString()),


                        });
                        values.GetSalesOrderCount_List = GetSalesOrderCount_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  SalesOrder Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        // GetSalesMoreOrderCount
        public void DaGetMoreSalesOrderCount(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
               
                msSQL = " SELECT count(invoice_gid) as totalinvoice FROM rbl_trn_tinvoice where invoice_flag <> 'Invoice Cancelled' and user_gid in ( '" + user_gid + "') ";
                values.totalinvoice = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT count(invoice_gid) as approval_pending FROM rbl_trn_tinvoice where invoice_flag = 'Invoice Approved' and user_gid in ( '" + user_gid + "') ";
                values.approval_pending = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select count(invoice_gid) as payment_pending from rbl_trn_tinvoice where  payment_amount='0.00' and user_gid in ( '" + user_gid + "') ";
                values.payment_pending = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT count(invoice_gid) as totalinvoice FROM rbl_trn_tinvoice where invoice_flag = 'Invoice Approval Pending' and user_gid in ( '" + user_gid + "') ";
                values.approvalpendinginnvoice = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select count(lead2campaign_gid) as potentialleadcount from crm_trn_tenquiry2campaign where leadstage_gid='5' and created_by in ( ( '" + employee_gid + "') ";
                values.potentialleadcount = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  SalesOrder Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        // GetOwnOverallSalesOrderChart
        public void DaGetOwnOverallSalesOrderChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
               
                msSQL = " select count(*) as count_own,salesorder_status as status from smr_trn_tsalesorder  " +
                    // " where created_by='" + employee_gid + "' and so_type='Sales' " +
                    " group by salesorder_status ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var OverallSalesOrderChart_List = new List<GetOverallSalesOrderChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        OverallSalesOrderChart_List.Add(new GetOverallSalesOrderChart_List
                        {
                            count_own = (dt["count_own"].ToString()),
                            salesorder_status_own = (dt["status"].ToString()),

                        });
                        values.GetOverallSalesOrderChart_List = OverallSalesOrderChart_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  SalesOrder Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        // GetHierarchyOverallSalesOrderChart
        public void DaGetHierarchyOverallSalesOrderChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try {
                

                msSQL = " select count(*) as count_Hierarchy,salesorder_status_Hierarchy as status from smr_trn_tsalesorder  " +
                //  " where created_by in ('" + employee_gid + "') and so_type='Sales' " +
                " group by salesorder_status ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var OverallSalesOrderChart_List = new List<GetOverallSalesOrderChart_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    OverallSalesOrderChart_List.Add(new GetOverallSalesOrderChart_List
                    {
                        count_Hierarchy = (dt["count_Hierarchy"].ToString()),
                        salesorder_status_Hierarchy = (dt["salesorder_status_Hierarchy"].ToString()),

                    });
                    values.GetOverallSalesOrderChart_List = OverallSalesOrderChart_List;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting OverallDeliveryOrder Chart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        // GetOwnOverallDeliveryOrderChart
        public void DaGetOwnOverallDeliveryOrderChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
               
                msSQL = " select count(*) as do_count_own,delivery_status as delivery_status_own from smr_trn_tdeliveryorder  " +
                    //  " where created_name='" + employee_gid + "' " +
                    " group by delivery_status ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var OverallSalesOrderChart_List = new List<GetOverallSalesOrderChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        OverallSalesOrderChart_List.Add(new GetOverallSalesOrderChart_List
                        {
                            do_count_own = (dt["do_count_own"].ToString()),
                            delivery_status_own = (dt["delivery_status_own"].ToString()),

                        });
                        values.GetOverallSalesOrderChart_List = OverallSalesOrderChart_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting OverallDeliveryOrder Chart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // GetHierarchyOverallDeliveryOrderChart
        public void DaGetHierarchyOverallDeliveryOrderChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
                
                msSQL = " select count(*) as count,delivery_status as delivery_status_own from smr_trn_tdeliveryorder where " +
                    " created_name in ('" + employee_gid + "') " +
                    " group by delivery_status ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var OverallSalesOrderChart_List = new List<GetOverallSalesOrderChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        OverallSalesOrderChart_List.Add(new GetOverallSalesOrderChart_List
                        {
                            do_count_Hierarchy = (dt["do_count_Hierarchy"].ToString()),
                            delivery_status_Hierarchy = (dt["delivery_status_own"].ToString()),

                        });
                        values.GetOverallSalesOrderChart_List = OverallSalesOrderChart_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured Overall DeliveryOrderChart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // GetMonthlySalesPipelineChart
        public void DaGetMonthlySalesPipelineChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
               
                msSQL = " SELECT cast(concat(monthname(payment_date),'-',year(payment_date)) as char) as payment_day,  " +
                    "(sum(amount)*exchange_rate) as amount " +
                    " FROM rbl_trn_tpayment group by YEAR(payment_date),MONTH(payment_date) ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var OverallSalesOrderChart_List = new List<GetOverallSalesOrderChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        OverallSalesOrderChart_List.Add(new GetOverallSalesOrderChart_List
                        {
                            payment_day = (dt["payment_day"].ToString()),
                            amount = (dt["amount"].ToString()),

                        });
                        values.GetOverallSalesOrderChart_List = OverallSalesOrderChart_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Monthly Sales Chart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        //public void DaGetMTDCounts(MdlSmrDashboard values)
        //{
        //    try
        //    {
               
        //        msSQL = "  select ifnull(format(sum(total_amount),2),'0.00') as mtd_over_due_payment from rbl_trn_tpayment  where payment_date >= DATE_FORMAT(curdate(), '%Y-%m-01') and payment_date <= DATE_FORMAT(curdate(), '%Y-%m-%d')  ";

        //        values.mtd_over_due_payment = objdbconn.GetExecuteScalar(msSQL);


        //        msSQL = "  SELECT IFNULL(COUNT(*), 0) AS mtd_over_due_payment_amount FROM rbl_trn_tpayment WHERE payment_date >= DATE_FORMAT(CURDATE(), '%Y-%m-01') AND payment_date <= CURDATE() ";
        //        values.mtd_over_due_payment_amount = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = " select ifnull(format(sum(invoice_amount),2),'0.00') as mtd_over_due_invoice_amount from rbl_trn_tinvoice  where invoice_date >= DATE_FORMAT(curdate(), '%Y-%m-01') and invoice_date <= DATE_FORMAT(curdate(), '%Y-%m-%d') ";
        //        values.mtd_over_due_invoice_amount = objdbconn.GetExecuteScalar(msSQL);
                 
        //        msSQL = " SELECT IFNULL(COUNT(*), 0) AS mtd_over_due_invoice FROM rbl_trn_tinvoice WHERE invoice_date >= DATE_FORMAT(CURDATE(), '%Y-%m-01') AND invoice_date <= CURDATE() ";
        //        string mtd_over_due_invoice = objdbconn.GetExecuteScalar(msSQL);
        //        values.mtd_over_due_invoice = mtd_over_due_invoice;
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting MTD Counts !";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
        //       $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
        //       msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
        //    }
            

        //}
       
        public void DaGetYTDCounts(MdlSmrDashboard values)
        {
            try
            {
               
                msSQL = "   SELECT IFNULL(COUNT(*), 0) AS ytd_over_due_payment FROM rbl_trn_tpayment WHERE YEAR(payment_date) = YEAR(CURDATE())";
                values.ytd_over_due_payment = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "  SELECT ifnull(format(sum(total_amount),2),'0.00')  as ytd_over_due_payment_amount  FROM rbl_trn_tpayment WHERE YEAR(payment_date) = YEAR(curdate()) ";
                values.ytd_over_due_payment_amount = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = " SELECT IFNULL(COUNT(*), 0) AS ytd_over_due_invoice FROM rbl_trn_tinvoice WHERE YEAR(invoice_date) = YEAR(CURDATE())";
                msSQL = "SELECT  IFNULL(COUNT(*), 0) AS ytd_over_due_invoice FROM rbl_trn_tinvoice WHERE invoice_date >= DATE_FORMAT(CURDATE() - INTERVAL 1 YEAR, '%Y-04-01') AND invoice_date <= DATE_FORMAT(CURDATE(), '%Y-03-31')";
                values.ytd_over_due_invoice_amount = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "  SELECT ifnull(format(sum(invoice_amount),2),'0.00')  as ytd_over_due_invoice_amount  FROM rbl_trn_tinvoice WHERE YEAR(invoice_date) = YEAR(curdate()) ";
                msSQL = " SELECT IFNULL(FORMAT(SUM(invoice_amount), 2), '0.00') AS ytd_over_due_invoice_amount  FROM rbl_trn_tinvoice WHERE invoice_date >= DATE_FORMAT(CURDATE() - INTERVAL 1 YEAR, '%Y-04-01') AND invoice_date <= DATE_FORMAT(CURDATE(), '%Y-03-31') ";
                values.ytd_over_due_invoice = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting YTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        public void DaGetMonthlySalesChart(string employee_gid, string user_gid, MdlSmrDashboard values)
        {
            try
            {
                msSQL = " select DATE_FORMAT(salesorder_date, '%b-%Y')  as salesorder_date,substring(date_format(a.salesorder_date,'%M'),1,3)as month,a.salesorder_gid,year(a.salesorder_date) as year, " +
                 " round(sum(a.grandtotal),2)as amount,count(a.salesorder_gid)as ordercount    " +
                 " from smr_trn_tsalesorder a   " +
                 " where a.salesorder_date > date_add(now(), interval-6 month) and a.salesorder_date<=date(now())   " +
                 " and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc  ";
                 dt_datatable = objdbconn.GetDataTable(msSQL);
                var SalesPerformanceChart_List = new List<GetSalesPerformanceChart_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        SalesPerformanceChart_List.Add(new GetSalesPerformanceChart_List
                        {
                           
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            ordercount = (dt["ordercount"].ToString()),

                        });
                        values.GetSalesPerformanceChart_List = SalesPerformanceChart_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Monthly SalesChart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        public void DaGetcurrency(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "select default_currency,currency_code,concat(currency_code,'/',exchange_rate)as currency,symbol " +
                        " from crm_trn_tcurrencyexchange where default_currency='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<currency_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new currency_list
                        {
                            currency = dt["currency"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            symbol = dt["symbol"].ToString(),
                        });
                        values.currency_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Cumpany Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetCustomerCounts(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "select count(customer_gid) as total_customers from crm_mst_tcustomer where status='Active'";
                values.customer_count = objdbconn.GetExecuteScalar(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Monthly SalesChart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        } 
        public void DaGetoverallsalesbarchart(MdlSmrDashboard values)
        {
            try
            {
                msSQL = "select(select count(a.enquiry_gid) from smr_trn_tsalesenquiry a  where a.created_date >= DATE_SUB(CURDATE()," +
                    " INTERVAL 6 MONTH)) as overall_enquiry,(select count(b.quotation_gid) from smr_trn_treceivequotation b  " +
                    "where b.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH))as overall_quotation," +
                    "(select count(c.salesorder_gid)from  smr_trn_tsalesorder c  where c.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH))as overall_salesorder," +
                    "(select count(invoice_gid)from  rbl_trn_tinvoice d  where d.created_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)) as overall_invoice";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<Getoverallsalesbarchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new Getoverallsalesbarchart_list
                        {
                            overall_enquiry = dt["overall_enquiry"].ToString(),
                            overall_quotation = dt["overall_quotation"].ToString(),
                            overall_salesorder = dt["overall_salesorder"].ToString(),
                            overall_invoice = dt["overall_invoice"].ToString(),
                        });
                        values.Getoverallsalesbarchart_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Monthly SalesChart !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetSalesordersixmonthchart(MdlSmrDashboard values)
        {
            try
            {

                msSQL = "SELECT COUNT(CASE WHEN source_flag = 'S' AND salesorder_gid IS NOT NULL THEN 1 ELSE NULL END) AS shopifyorder," +
                    "COUNT(CASE WHEN source_flag = 'I' AND salesorder_gid IS NOT NULL THEN 1 ELSE NULL END) AS salesorder," +
                    "COUNT(CASE WHEN source_flag = 'W' AND salesorder_gid IS NOT NULL THEN 1 ELSE NULL END) AS whatsapporder," +
                    "DATE_FORMAT(salesorder_date, '%b-%y') AS salesorder_date FROM smr_trn_tsalesorder" +
                    " GROUP BY DATE_FORMAT(salesorder_date, '%b-%m')ORDER BY salesorder_date DESC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<Salesordersixmonthchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new Salesordersixmonthchart_list
                        {
                            shopifyordersixmonth = dt["shopifyorder"].ToString(),
                            salesordersixmonth = dt["salesorder"].ToString(),
                            whatsappordersixmonth = dt["whatsapporder"].ToString(),
                            salesorder_datesixmonth = dt["salesorder_date"].ToString(),


                        });
                        values.Salesordersixmonthchart_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                var message = "Exception Occured While Getting Sales Dashboard";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + message + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


    }
}