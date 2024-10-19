using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;

namespace ems.sales.DataAccess
{
    public class DaSmrRptSalesReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader, objODBCDatareader1;
        DataTable dt_datatable, dt_datatable1;
        string msGetGid, msGetGid1, msGetGid2, mscusconGetGID, msleadbankGetGID, msleadbankconGetGID, msGetGidfil, lsmodule_name, lssalestype_gid, lssalestype_name, lsasset_status, lsproduct_name, lsassetdtl_gid, lssasset_status;
        int mnResult;
        string lsstart_date = "", lsend_date = "";



        public void DaGetsalesReportsummary(string user_gid, MdlSmrRptSalesReport values)
        {
            try
            {
                msSQL = " SELECT concat(e.customercontact_name,' / ',e.email,' / ',e.mobile) as contact,a.invoice_refno, DATE_FORMAT(a.invoice_date, '%d-%m-%Y') AS invoice_date, a.customer_name, " +
                    " a.customer_address, a.invoice_gid, c.customer_code, a.customer_gid, a.customer_contactnumber, " +
                    " d.salestype_name, CONCAT(c.customer_id, '/', a.customer_name) AS customer, FORMAT(a.total_amount, 2) AS total_amount, " +
                    " b.tax_name, b.tax_percentage, FORMAT(b.tax_amount, 2) AS tax_amount, " +
                    " b.tax_name2, b.tax_percentage2, FORMAT(b.tax_amount2, 2) AS tax_amount2, FORMAT(a.invoice_amount, 2) AS invoice_amount, " +
                    " FORMAT(SUM(b.tax_amount) + SUM(b.tax_amount2), 2) AS total_tax, " +
                    " FORMAT(sum(CASE WHEN b.vendor_price IS NOT NULL THEN b.vendor_price * b.qty_invoice ELSE b.product_price * b.qty_invoice END), 2) AS product_price_L, FORMAT(sum(b.discount_amount), 2) AS discount_amount_L, " +
                    " CASE WHEN d.salestype_name IS NULL THEN " +
                    " CASE WHEN b.product_name LIKE '%Azure%' THEN 'Sales - Cloud Server' " +
                    " WHEN b.product_name LIKE '%License%' THEN 'Sales-License' " +
                    " ELSE 'Sales-Professional Services' END " +
                    " ELSE d.salestype_name END AS sales_type " +
                    " FROM rbl_trn_tinvoice a " +
                    " LEFT JOIN rbl_trn_tinvoicedtl b ON b.invoice_gid = a.invoice_gid " +
                    " LEFT JOIN crm_mst_tcustomer c ON c.customer_gid = a.customer_gid " +
                    " LEFT JOIN smr_trn_tsalestype d ON a.sales_type = d.salestype_gid " +
                    " LEFT JOIN crm_mst_tcustomercontact e ON e.customer_gid = a.customer_gid " +                    
                    " where a.invoice_status NOT IN ('Invoice Cancelled') GROUP BY a.invoice_gid  order by DATE(a.invoice_date) DESC,a.invoice_refno desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSaleLedger_list = new List<GetSalesReport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSaleLedger_list.Add(new GetSalesReport_list
                        {
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            total_amount = dt["product_price_L"].ToString(),
                            discount_amount_L = dt["discount_amount_L"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            total_tax = dt["total_tax"].ToString(),
                            customer = dt["customer"].ToString(),
                            sales_type = dt["sales_type"].ToString(),
                        });
                        values.GetSalesReport_list = GetSaleLedger_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetsalesReportdate(string from_date, string to_date, MdlSmrRptSalesReport values)
        {
            try
            {
                if ((from_date == null) || (to_date == null))
                {
                    lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                    lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    //-- from date
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    //-- to date
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }
                msSQL = " SELECT concat(e.customercontact_name,' / ',e.email,' / ',e.mobile) as contact,a.invoice_refno, DATE_FORMAT(a.invoice_date, '%d-%m-%Y') AS invoice_date, a.customer_name, " +
                        " a.customer_address, a.invoice_gid, c.customer_code, a.customer_gid, a.customer_contactnumber, " +
                        " d.salestype_name, CONCAT(c.customer_id, '/', a.customer_name) AS customer, FORMAT(a.total_amount, 2) AS total_amount, " +
                        " b.tax_name, b.tax_percentage, FORMAT(b.tax_amount, 2) AS tax_amount, " +
                        " b.tax_name2, b.tax_percentage2, FORMAT(b.tax_amount2, 2) AS tax_amount2, FORMAT(a.invoice_amount, 2) AS invoice_amount, " +
                        " FORMAT(SUM(b.tax_amount) + SUM(b.tax_amount2), 2) AS total_tax, " +
                        " FORMAT(sum(CASE WHEN b.vendor_price IS NOT NULL THEN b.vendor_price * b.qty_invoice ELSE b.product_price * b.qty_invoice END), 2) AS product_price_L, FORMAT(sum(b.discount_amount), 2) AS discount_amount_L, " +
                        " CASE WHEN d.salestype_name IS NULL THEN " +
                        " CASE WHEN b.product_name LIKE '%Azure%' THEN 'Sales - Cloud Server' " +
                        " WHEN b.product_name LIKE '%License%' THEN 'Sales-License' " +
                        " ELSE 'Sales-Professional Services' END " +
                        " ELSE d.salestype_name END AS sales_type " +
                        " FROM rbl_trn_tinvoice a " +
                        " LEFT JOIN rbl_trn_tinvoicedtl b ON b.invoice_gid = a.invoice_gid " +
                        " LEFT JOIN crm_mst_tcustomer c ON c.customer_gid = a.customer_gid " +
                        " LEFT JOIN smr_trn_tsalestype d ON a.sales_type = d.salestype_gid " +
                        " LEFT JOIN crm_mst_tcustomercontact e ON e.customer_gid = a.customer_gid " +
                        " where  a.invoice_date >= '" + lsstart_date + "' and a.invoice_date <= '" + lsend_date + "'" +
                        " and a.invoice_status NOT IN ('Invoice Cancelled') GROUP BY a.invoice_gid  order by DATE(a.invoice_date) DESC,a.invoice_refno desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSaleLedger_list = new List<GetSalesReport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSaleLedger_list.Add(new GetSalesReport_list
                        {
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            total_amount = dt["product_price_L"].ToString(),
                            discount_amount_L = dt["discount_amount_L"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            total_tax = dt["total_tax"].ToString(),
                            customer = dt["customer"].ToString(),
                            sales_type = dt["sales_type"].ToString(),
                        });
                        values.GetSalesReport_list = GetSaleLedger_list;
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