using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Net.NetworkInformation;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Configuration;
using System.Globalization;
using static ems.sales.Models.MdlOutstandingAmount;

namespace ems.sales.DataAccess
{
    public class DaOutstandingAmount
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;

        DataTable dt_datatable;
        public void DaGetOutstandingAmountReportSummary( string from_date , string to_date ,MdlOutstandingAmount values)
        {
            try
            {

                msSQL = "SELECT DISTINCT a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag, date_format(invoice_date,'%d-%m-%Y') as invoice_date, a.invoice_type, c.branch_name, " +
                            " FORMAT(a.advance_amount, 2) AS advance_amount, a.invoice_from, " +
                            " format((payment_amount * exchange_rate),2) AS payment_amount, a.payment_date, " +
                            " a.invoice_status AS overall_status, " +
                            "  CASE WHEN a.payment_date < NOW() THEN DATEDIFF(NOW(), a.payment_date) ELSE NULL END AS expiry_days, "+
                            " date_format(a.payment_date, '%d-%m-%Y') AS due_date, " +
                            " format((a.invoice_amount * a.exchange_rate),2) AS invoice_amount, a.invoice_date, " +
                            " CONCAT(z.customer_id, '/', z.customer_name) AS companydetails, " +
                            " a.invoice_from, format(((a.invoice_amount * a.exchange_rate) - (a.payment_amount * a.exchange_rate)),2) AS outstanding_amount " +
                            " FROM rbl_trn_tinvoice a " +
                            " LEFT JOIN rbl_trn_tinvoicedtl b ON a.invoice_gid = b.invoice_gid " +
                            " LEFT JOIN hrm_mst_tbranch c ON a.branch_gid = c.branch_gid " +
                              " LEFT JOIN crm_mst_tcustomer z ON a.customer_gid = z.customer_gid " +
                            " WHERE a.invoice_amount <> a.payment_amount " +
                            " AND a.invoice_status <> 'Invoice Cancelled' " +
                            " AND a.invoice_flag <> 'Invoice Cancelled'";
                if (from_date != null && to_date != null)
                {
                    //string fromDateFormatted = from_date.Value.ToString("yyyy-MM-dd");
                    //string toDateFormatted = to_date.Value.ToString("yyyy-MM-dd");

                    msSQL += $" AND a.invoice_date BETWEEN '{from_date}' AND '{to_date}'";
                }
                msSQL += " GROUP BY a.invoice_gid " +
                    " ORDER BY DATE(a.invoice_date) DESC, a.invoice_date ASC, a.invoice_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOutsrandingAmountResult>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOutsrandingAmountResult
                        {
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_details = dt["companydetails"].ToString(),
                            invoicetype = dt["invoice_type"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            branch = dt["branch_name"].ToString(),
                            paid_amount = dt["payment_amount"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            expiry_days = dt["expiry_days"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            status = dt["overall_status"].ToString()
                          

                        });
                        values.GetOutsrandingAmountResult = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Customer Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
    }
}