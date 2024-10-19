using ems.pmr.Models;
using ems.pmr.DataAccess;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using System.Data.OleDb;

using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace ems.pmr.DataAccess
{
    public class DaPmrRptPurchaseLegder
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lsstart_date = "", lsend_date = "";
        public void DaGetPurchaselegderSummary(MdlPmrRptPurchaseLegder values)
        {
            try
            {

                msSQL = " select a.invoice_refno,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date,c.contactperson_name,a.vendor_address,a.invoice_gid, " +
                     "  c.vendor_code,a.vendor_gid,c.contact_telephonenumber,e.account_name,  " +
                      " format(a.total_amount, 2) as total_amount, CONCAT(c.vendor_code, '/', c.vendor_companyname) AS vendor, " +
                      " b.tax_name,b.tax_percentage, format(b.tax_amount, 2) as tax_amount,b.tax_name2,  " +
                      " b.tax_percentage2,format(b.tax_amount2, 2) as tax_amount2, " +
                      " FORMAT(SUM(b.tax_amount) + SUM(b.tax_amount2), 2) AS total_tax, " +
                      " FORMAT(sum(b.product_price*b.qty_invoice), 2) AS product_price_L, FORMAT(sum(b.discount_amount_L), 2) AS discount_amount_L, " +
                      " CASE WHEN e.account_name IS NULL THEN " +
                      " CASE WHEN b.product_name LIKE '%Azure%' THEN 'Purchase - Cloud Server' " +
                      " WHEN b.product_name LIKE '%Microsoft%' THEN 'Purchase -License' " +
                      " ELSE 'Purchase -Domain & hosting' " +
                      " END " +
                      " ELSE e.account_name " +
                      " END AS purchase_type, " +
                      " format(a.invoice_amount, 2) as invoice_amount from acp_trn_tinvoice a " +
                      " left join acp_trn_tinvoicedtl b on b.invoice_gid = a.invoice_gid " +
                       " left join acp_mst_tvendor c on c.vendor_gid = a.vendor_gid " +
                       " left join pmr_trn_tpurchasetype d on a.purchase_type = d.purchasetype_gid " +
                       " left join acc_mst_tchartofaccount e on e.account_gid = d.account_gid " +
                       " GROUP BY a.invoice_gid  order by DATE(a.invoice_date) DESC,a.invoice_refno desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSaleLedger_list = new List<GetSaleLedger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSaleLedger_list.Add(new GetSaleLedger_list
                        {
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            vendor_name = dt["contactperson_name"].ToString(),
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
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            vendor = dt["vendor"].ToString(),
                            purchase_type = dt["purchase_type"].ToString(),
                            total_tax = dt["total_tax"].ToString(),
                        });
                        values.GetSaleLedger_list = GetSaleLedger_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPurchaselegderDate(string from_date, string to_date, MdlPmrRptPurchaseLegder values)
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
                msSQL = " select a.invoice_refno,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date,c.contactperson_name,a.vendor_address,a.invoice_gid, " +
                       "  c.vendor_code,a.vendor_gid,c.contact_telephonenumber,e.account_name,  " +
                        " format(a.total_amount, 2) as total_amount, CONCAT(c.vendor_code, '/', c.vendor_companyname) AS vendor, " +
                        " b.tax_name,b.tax_percentage, format(b.tax_amount, 2) as tax_amount,b.tax_name2,  " +
                        " b.tax_percentage2,format(b.tax_amount2, 2) as tax_amount2, " +
                        " FORMAT(SUM(b.tax_amount) + SUM(b.tax_amount2), 2) AS total_tax, " +
                        " FORMAT(sum(b.product_price*b.qty_invoice), 2) AS product_price_L, FORMAT(sum(b.discount_amount_L), 2) AS discount_amount_L, " +
                        " CASE WHEN e.account_name IS NULL THEN " +
                        " CASE WHEN b.product_name LIKE '%Azure%' THEN 'Purchase - Cloud Server' " +
                        " WHEN b.product_name LIKE '%Microsoft%' THEN 'Purchase -License' " +
                        " ELSE 'Purchase -Domain & hosting' " +
                        " END " +
                        " ELSE e.account_name " +
                        " END AS purchase_type, " +
                        " format(a.invoice_amount, 2) as invoice_amount from acp_trn_tinvoice a " +
                        " left join acp_trn_tinvoicedtl b on b.invoice_gid = a.invoice_gid " +
                         " left join acp_mst_tvendor c on c.vendor_gid = a.vendor_gid " +
                         " left join pmr_trn_tpurchasetype d on a.purchase_type = d.purchasetype_gid " +
                         " left join acc_mst_tchartofaccount e on e.account_gid = d.account_gid  where  a.invoice_date >= '" + lsstart_date + "' and a.invoice_date <= '" + lsend_date + "'" +
                         " GROUP BY a.invoice_gid  order by DATE(a.invoice_date) DESC,a.invoice_refno desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSaleLedger_list = new List<GetSaleLedger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSaleLedger_list.Add(new GetSaleLedger_list
                        {
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            vendor_name = dt["contactperson_name"].ToString(),
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
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            account_name = dt["account_name"].ToString(),
                            vendor = dt["vendor"].ToString(),
                            purchase_type = dt["purchase_type"].ToString(),
                            total_tax = dt["total_tax"].ToString(),
                        });
                        values.GetSaleLedger_list = GetSaleLedger_list;
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
