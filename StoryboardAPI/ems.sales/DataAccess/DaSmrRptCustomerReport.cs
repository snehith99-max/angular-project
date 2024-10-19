using ems.sales.Models;
using ems.utilities.Functions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Principal;

//using System.Web;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using System.Web;

namespace ems.sales.DataAccess
{
    public class DaSmrRptCustomerReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL, msSQL12, msSQL2, msSQL13, msSQL14, msSQL1, msGetDlGID = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable, dt_datatable1, dt_datatable2;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lssopening_amount, lsCode, lsuser_code;
        string msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lstax_amount, lstax_amount2, lstax_amount3, lsclosing_amount;
        string lstax_total, lstotal_tax, lsfinyear, lsfinyear_start, lsfinyear_end, lsaccountname, lsbranch_gid;
        string lstransaction_code, lsbank_name, lsdr_cr, lsdrcr_value, lsbdr_cr, lsaccount_gid, lsjournal_desc, lsaaccount_gid;
        string lstransaction_amount, customer_name, customer_id, lsstax_amount, lsstax_amount2, lsstax_amount3, lsstotal_tax, lsfyear_startyear, lsfyear_endyear;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        decimal closecredit_amount, closedebit_amount, openingcredit_amount, openingdebit_amount;
        double closingbal = 0;
        string lsstart_date = "", lsend_date = "";
        DataSet ds_dataset;

        public void DaGetCustomerReportSummary(MdlSmrRptCustomerReport values)
        {
            msSQL = " SELECT c.account_gid,a.customer_address,FORMAT(SUM(a.invoice_amount), 2) AS invoice_amount," +
                " FORMAT(SUM(a.payment_amount), 2) AS payment_amount," +
                " a.customer_gid,b.customer_id,b.customer_name," +
                " concat(b.customer_id,' / ',b.customer_name) as customer," +
                " FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance," +
                " CONCAT(COALESCE(d.customercontact_name, '')," +
                " '/', COALESCE(d.mobile, ''),'/', COALESCE(d.email, '')) AS contact," +
                " FORMAT(SUM(a.invoice_amount) - SUM(a.payment_amount), 2) AS outstanding_amount" +
                " FROM rbl_trn_tinvoice a" +
                " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid" +
                " LEFT JOIN crm_mst_tcustomercontact d ON d.customer_gid = b.customer_gid" +
                " LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid" +
                " GROUP BY a.customer_address, a.customer_gid, b.customer_id," +
                " b.customer_name, d.customercontact_name, d.mobile, d.email, c.opening_balance";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var Debitorreport_List = new List<Getcustomerreport_List>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    Debitorreport_List.Add(new Getcustomerreport_List
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer = dt["customer"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        contact = dt["contact"].ToString(),
                        opening_balance = dt["opening_balance"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        payment_amount = dt["payment_amount"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        account_gid = dt["account_gid"].ToString(),

                    });
                    values.Getcustomerreport_List = Debitorreport_List;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetCustomerDetailedReport(string customer_gid, string from_date, string to_date, MdlSmrRptCustomerReport values)
        {
            try
            {
                
                if ((from_date == null) || (to_date == null))
                {
                    msSQL = " SELECT a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,e.salestype_name,c.account_gid,a.customer_address," +
                            " FORMAT(SUM(a.invoice_amount), 2) AS invoice_amount," +
                           " FORMAT(SUM(a.payment_amount), 2) AS payment_amount," +
                           " a.customer_gid,b.customer_id,b.customer_name," +
                           " FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance," +
                           " CONCAT(COALESCE(d.customercontact_name, '')," +
                           " '/', COALESCE(d.mobile, ''),'/', COALESCE(d.email, '')) AS contact," +
                           " FORMAT(SUM(a.invoice_amount) - SUM(a.payment_amount), 2) AS outstanding_amount" +
                           " FROM rbl_trn_tinvoice a" +
                           " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid" +
                           " LEFT JOIN crm_mst_tcustomercontact d ON d.customer_gid = b.customer_gid" +
                           " LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid" +
                           " Left join smr_trn_tsalestype e on e.account_gid = c.account_gid" +
                           " LEFT JOIN rbl_trn_tpayment f on f.invoice_gid = a.invoice_gid" +
                           " where b.customer_gid = '"+customer_gid+ "' and f.payment_return='0'" +
                           " GROUP BY a.customer_address, a.customer_gid, b.customer_id," +
                           " b.customer_name, d.customercontact_name, d.mobile, d.email, c.opening_balance" +
                           " order by f.payment_date desc";
                }
                else
                {
                    //-- from date
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    //-- to date
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");

                    msSQL = " SELECT a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,e.salestype_name,c.account_gid,a.customer_address,FORMAT(SUM(a.invoice_amount), 2) AS invoice_amount," +
                           " FORMAT(SUM(a.payment_amount), 2) AS payment_amount," +
                           " a.customer_gid,b.customer_id,b.customer_name," +
                           " FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance," +
                           " CONCAT(COALESCE(d.customercontact_name, '')," +
                           " '/', COALESCE(d.mobile, ''),'/', COALESCE(d.email, '')) AS contact," +
                           " FORMAT(SUM(a.invoice_amount) - SUM(a.payment_amount), 2) AS outstanding_amount" +
                           " FROM rbl_trn_tinvoice a" +
                           " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid" +
                           " LEFT JOIN crm_mst_tcustomercontact d ON d.customer_gid = b.customer_gid" +
                           " LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid" +
                           " Left join smr_trn_tsalestype e on e.account_gid = c.account_gid" +
                           " LEFT JOIN rbl_trn_tpayment f on f.invoice_gid = a.invoice_gid" +
                           " where b.customer_gid = '" + customer_gid + "' and f.payment_return='0'" +
                           " and a.invoice_date >= '" + lsstart_date + "' and a.invoice_date <= '" + lsend_date + "'" +
                           " GROUP BY a.customer_address, a.customer_gid, b.customer_id," +
                           " b.customer_name, d.customercontact_name, d.mobile, d.email, c.opening_balance" +
                           " order by f.payment_date desc";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var Debitorreport_List = new List<Getcustomerreport_List>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        Debitorreport_List.Add(new Getcustomerreport_List
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            contact = dt["contact"].ToString(),
                            opening_balance = dt["opening_balance"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            account_gid = dt["account_gid"].ToString(),
                            sales_type = dt["salestype_name"].ToString(),
                            invoice_refno = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            customer = dt["customer_name"].ToString(),
                            customer_id = dt["customer_id"].ToString(),

                        });
                        values.Getcustomerreport_List = Debitorreport_List;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }


    }
}