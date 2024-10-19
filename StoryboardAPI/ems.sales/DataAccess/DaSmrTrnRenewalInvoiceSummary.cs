using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static ems.sales.Models.MdlSmrTrnRenewalInvoiceSummary;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnRenewalInvoiceSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        int mnResult;

            public void DaGetRenewalInvoiceSummary(string employee_gid, MdlSmrTrnRenewalInvoiceSummary values)
            {

                try
                {


                string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                               " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                               " WHERE  a.renewal_to ='" + employee_gid + "'  and d.renewal_flag = 'Y' " +
                               " AND a.renewal_date >= CURRENT_DATE() AND a.renewal_status != 'Closed' AND a.renewal_status != 'Dropped' UNION " +
                               " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a" +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " + "" +
                               " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid " +
                               " WHERE a.renewal_type = 'Agreement' and a.renewal_to ='" + employee_gid + "'" +
                               " AND a.renewal_date >= CURRENT_DATE() AND a.renewal_status != 'Closed' AND a.renewal_status != 'Dropped' " +
                               " ORDER BY renewal_gid DESC";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetRenewalInvoiceSummary_lists>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetRenewalInvoiceSummary_lists
                            {
                                renewal_gid = dt["renewal_gid"].ToString(),
                                salesorder_date = dt["order_agreement_date"].ToString(),
                                grandtotal = dt["grandtotal"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                contact_details = dt["contact_details"].ToString(),
                                renewal_date = dt["renewal_date"].ToString(),
                                renewal = dt["renewal"].ToString(),
                                duration = dt["duration"].ToString()
                            });
                            values.GetRenewalInvoiceSummary_lists = getModuleList;
                        }

                    }
            
                    dt_datatable.Dispose();

                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while getting renewal invoice summary!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }


            }

        public void DaGetRenewalInvoiceSummaryless30(string employee_gid, MdlSmrTrnRenewalInvoiceSummary values)
        {

            try
            {
              

                string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                               " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                               " WHERE  a.renewal_to ='" + employee_gid + "'  and d.renewal_flag = 'Y'" +
                               " AND a.renewal_date BETWEEN CURRENT_DATE()  AND DATE_ADD(CURRENT_DATE(), INTERVAL 30 DAY) AND a.renewal_status != 'Dropped' UNION " +
                               " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a" +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " + "" +
                               " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid " +
                               " WHERE a.renewal_type = 'Agreement' and a.renewal_to ='" + employee_gid + "' " +
                               " AND a.renewal_date BETWEEN CURRENT_DATE() AND DATE_ADD(CURRENT_DATE(), INTERVAL 30 DAY) AND a.renewal_status != 'Dropped' " +
                               " ORDER BY renewal_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalInvoiceSummary_lists2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalInvoiceSummary_lists2
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            salesorder_date = dt["order_agreement_date"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            duration = dt["duration"].ToString()
                        });
                        values.GetRenewalInvoiceSummary_lists2 = getModuleList;
                    }

                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting renewal invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetRenewalInvoiceSummaryexpired(string employee_gid, MdlSmrTrnRenewalInvoiceSummary values)
        {

            try
            {


                string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                               " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                               " WHERE  a.renewal_to ='" + employee_gid + "' and a.renewal_date < CURDATE() AND a.renewal_status != 'Closed' AND a.renewal_status != 'Dropped' and d.renewal_flag = 'Y'UNION " +
                               " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a" +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " + "" +
                               " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid " +
                               " WHERE a.renewal_type = 'Agreement' and a.renewal_date < CURDATE() AND a.renewal_status != 'Closed' AND a.renewal_status != 'Dropped' and a.renewal_to ='" + employee_gid + "'" +
                               " ORDER BY renewal_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalInvoiceSummary_lists3>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalInvoiceSummary_lists3
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            salesorder_date = dt["order_agreement_date"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            duration = dt["duration"].ToString()
                        });
                        values.GetRenewalInvoiceSummary_lists3 = getModuleList;
                    }

                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting renewal invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }


        public void DaGetRenewalInvoiceSummarydrop(string employee_gid, MdlSmrTrnRenewalInvoiceSummary values)
        {

            try
            {


                string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                               " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                               " WHERE  a.renewal_to ='" + employee_gid + "'  and d.renewal_flag = 'Y' AND a.renewal_status != 'Closed' and a.renewal_status='Dropped' UNION " +
                               " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a" +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " + "" +
                               " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid " +
                               " WHERE a.renewal_type = 'Agreement' AND a.renewal_status != 'Closed' and a.renewal_status='Dropped' and a.renewal_to ='" + employee_gid + "'" +
                               " ORDER BY renewal_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalInvoiceSummary_lists4>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalInvoiceSummary_lists4
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            salesorder_date = dt["order_agreement_date"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            duration = dt["duration"].ToString()
                        });
                        values.GetRenewalInvoiceSummary_lists4 = getModuleList;
                    }

                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting renewal invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetRenewalInvoiceSummaryall(string employee_gid, MdlSmrTrnRenewalInvoiceSummary values)
        {

            try
            {


                string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                               " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                               " WHERE  a.renewal_to ='" + employee_gid + "'  and d.renewal_flag = 'Y'UNION " +
                               " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                               " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                               " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                               " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a" +
                               " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " + "" +
                               " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid " +
                               " WHERE a.renewal_type = 'Agreement' and a.renewal_to ='" + employee_gid + "'" +
                               " ORDER BY renewal_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalInvoiceSummary_lists5>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalInvoiceSummary_lists5
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            salesorder_date = dt["order_agreement_date"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            duration = dt["duration"].ToString()
                        });
                        values.GetRenewalInvoiceSummary_lists5 = getModuleList;
                    }

                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting renewal invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetCount(string employee_gid, MdlSmrTrnRenewalInvoiceSummary values)
            {
                msSQL=" SELECT COUNT(CASE WHEN renewal_status = 'Open' THEN 1 END) AS open_count," +
                "COUNT(CASE WHEN renewal_status = 'Dropped' THEN 1 END) AS dropped_count, " +
                "COUNT(CASE WHEN renewal_status = 'Closed' THEN 1 END) AS closed_count " +
                "FROM crm_trn_trenewal WHERE renewal_to ='" + employee_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRenewalInvoiceSummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRenewalInvoiceSummary_lists
                        {
                            open_count = dt["open_count"].ToString(),
                            dropped_count = dt["dropped_count"].ToString(),
                            closed_count = dt["closed_count"].ToString(),
                            
                        });
                        values.GetRenewalInvoiceSummary_lists = getModuleList;
                    }

                }

                dt_datatable.Dispose();

            }

        public void DaGetDroprenewal(string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
                msSQL = "update crm_trn_trenewal set " +
                      " renewal_status= 'Dropped' " +
                    " where renewal_gid='" + renewal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Renewal Dropped successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Dropping Renewal";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding renewal!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}