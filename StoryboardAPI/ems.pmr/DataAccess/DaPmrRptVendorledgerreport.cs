using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;


namespace ems.pmr.DataAccess
{
    public class DaPmrRptVendorledgerreport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code,
            lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid,
            maGetGID, lsvendor_code, msUserGid, lsend_date, lsstart_date;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetVendorledgerReportSummary(MdlPmrRptVendorledgerreport values)
        {
            try
            {
                 
                msSQL = " SELECT a.vendor_address,FORMAT(SUM(a.invoice_amount), 2) AS invoice_amount,FORMAT(SUM(a.payment_amount), 2) AS payment_amount," +
                        " a.vendor_gid,b.vendor_code,b.vendor_companyname,FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance," +
                        " CONCAT(COALESCE(b.contactperson_name, ''),'/', COALESCE(b.contact_telephonenumber, ''), '/', COALESCE(b.email_id, '')) AS contact," +
                        " FORMAT(SUM(a.invoice_amount) - SUM(a.payment_amount), 2) AS outstanding_amount FROM acp_trn_tinvoice a LEFT JOIN " +
                        " acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid" +
                        " GROUP BY  a.vendor_gid, b.vendor_code ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<vendorledger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new vendorledger_list
                        {

                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            opening_balance = dt["opening_balance"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            contact = dt["contact"].ToString()



                        });
                        values.vendorledger_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Purchaseorder detailed report Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        public void DaGetCreditorReportViewdate (string vendor_gid,string from_date, string to_date, MdlPmrRptVendorledgerreport values)
        {
            try
            {
                if ((from_date == null) || (to_date == null))
                {
                    msSQL = " SELECT date_format(a.invoice_date,'%d-%m-%Y') as invoice_date ,CASE WHEN a.invoice_reference IS NULL OR a.invoice_reference = '' " +
                    "  THEN a.invoice_gid ELSE a.invoice_reference  END AS invoice_reference ,a.vendor_address,FORMAT(a.invoice_amount, 2) AS invoice_amount,FORMAT(a.payment_amount, 2) AS payment_amount," +
                    " a.vendor_gid,FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance, d.purchasetype_name," +
                    " FORMAT((a.invoice_amount) - (a.payment_amount), 2) AS outstanding_amount FROM acp_trn_tinvoice a LEFT JOIN " +
                    " acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid LEFT JOIN pmr_trn_tpurchasetype d ON d.purchasetype_gid = a.purchase_type" +
                    " where a.vendor_gid='" + vendor_gid + "' GROUP BY  a.invoice_gid,a.invoice_date order by invoice_gid and invoice_date desc ";

                }
                else
                {
                    //-- from date
                    DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    //-- to date
                    DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");

                    //msSQL = "select  a.purchaseorder_date,a.purchaseorder_gid,a.vendor_gid,c.vendor_companyname, c.contactperson_name,c.vendor_code,concat(f.address1,f.address2) as vendor_address, " +
                    //" concat(c.contactperson_name,'/',c.contact_telephonenumber,'/',c.email_id) as details, " +
                    //" group_concat(distinct e.product_name,' ') as product_name, " +
                    //" sum(b.product_price) as amount, " +
                    //" (select case when x.total_amount_L='0.00' then format(sum(x.total_amount),2) " +
                    //" when x.total_amount_L<>'0.00' then format(sum(x.total_amount_L),2) end as total " +
                    //" from pmr_trn_tpurchaseorder x " +
                    //" where c.vendor_gid=x.vendor_gid) as total  from pmr_trn_tpurchaseorder a " +
                    //" left join pmr_trn_tpurchaseorderdtl b on a.purchaseorder_gid=b.purchaseorder_gid " +
                    //" left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid  " +
                    //" left join pmr_mst_tproduct e on b.product_gid=e.product_gid " +
                    //" left join adm_mst_taddress f on c.address_gid = f.address_gid " +
                    //" where a.purchaseorder_date >= '" + lsstart_date + "' and a.purchaseorder_date <= '" + lsend_date + "' and a.purchaseorder_status not in ('Approve Pending','PO Amended','Rejected') " +
                    //" group by a.vendor_gid";
                    msSQL = " SELECT date_format(a.invoice_date,'%d-%m-%Y') as invoice_date ,CASE WHEN a.invoice_reference IS NULL OR a.invoice_reference = '' " +
                        "  THEN a.invoice_gid ELSE a.invoice_reference  END AS invoice_reference ,a.vendor_address,FORMAT(a.invoice_amount, 2) AS invoice_amount,FORMAT(a.payment_amount, 2) AS payment_amount," +
                        " a.vendor_gid,FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance, d.purchasetype_name," +
                        " FORMAT((a.invoice_amount) - (a.payment_amount), 2) AS outstanding_amount FROM acp_trn_tinvoice a LEFT JOIN " +
                        " acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid LEFT JOIN pmr_trn_tpurchasetype d ON d.purchasetype_gid = a.purchase_type" +
                        " where a.vendor_gid='" + vendor_gid + "' and a.invoice_date >= '" + lsstart_date + "' and a.invoice_date <= '" + lsend_date + "'  GROUP BY  a.invoice_gid,a.invoice_date order by invoice_gid and invoice_date desc ";


                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var vendorledger_list = new List<vendorledger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        vendorledger_list.Add(new vendorledger_list
                        {
                            invoice_reference = dt["invoice_reference"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),


                        });
                        values.vendorledger_list = vendorledger_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetVendorReportView ( string vendor_gid, MdlPmrRptVendorledgerreport values)
        {
            try
            {
                
                msSQL = " select vendor_gid,vendor_code,vendor_companyname from acp_mst_tvendor where vendor_gid= '" + vendor_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var vendorledger_list = new List<vendorledger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        vendorledger_list.Add(new vendorledger_list
                        {
                            
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor = dt["vendor_companyname"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                           

                        });
                        values.vendorledger_list = vendorledger_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetVendorPaymentReportView(string vendor_gid, MdlPmrRptVendorledgerreport values)
        {
            try
            {

                msSQL = " SELECT date_format(a.invoice_date,'%d-%m-%Y') as invoice_date ,CASE WHEN a.invoice_reference IS NULL OR a.invoice_reference = '' " +
                    "  THEN a.invoice_gid ELSE a.invoice_reference  END AS invoice_reference ,a.vendor_address,FORMAT(a.invoice_amount, 2) AS invoice_amount,FORMAT(a.payment_amount, 2) AS payment_amount," +
                    " a.vendor_gid,FORMAT(COALESCE(c.opening_balance, 0.00), 2) AS opening_balance, d.purchasetype_name," +
                    " FORMAT((a.invoice_amount) - (a.payment_amount), 2) AS outstanding_amount FROM acp_trn_tinvoice a LEFT JOIN " +
                    " acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid LEFT JOIN acc_trn_topeningbalance c ON c.account_gid = b.account_gid LEFT JOIN pmr_trn_tpurchasetype d ON d.purchasetype_gid = a.purchase_type" +
                    " where a.vendor_gid='" + vendor_gid + "' GROUP BY  a.invoice_gid,a.invoice_date order by invoice_gid and invoice_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var vendorledger_list = new List<vendorledger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        vendorledger_list.Add(new vendorledger_list
                        {


                            invoice_reference = dt["invoice_reference"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),

                        });
                        values.vendorledger_list = vendorledger_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }

}