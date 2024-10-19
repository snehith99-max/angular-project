using ems.finance.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.finance.DataAccess
{
    public class DaVendor360
    {
        dbconn objconn = new dbconn();
        cmnfunctions objcmnfunc = new cmnfunctions();
        DataTable dt_table;
        OdbcDataReader objOdbcDataReader;
        string msSQL = string.Empty;

        public void DaGetTilesCount(string vendor_gid, MdlVendor360 values)
        {
            try
            {
                msSQL = "select(select count(a.purchaseorder_gid) from pmr_trn_tpurchaseorder a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " where a.vendor_gid ='"+vendor_gid+"') as PO_count , " +
                    " (select count(a.payment_gid) from acp_trn_tpayment a left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid" +
                    " where a.vendor_gid='"+vendor_gid+"') as paymentCount," +
                    " (select sum(a.payment_total) from acp_trn_tpayment a  left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " where a.vendor_gid='"+vendor_gid+"') as paymentamount," +
                    " (select sum(a.total_amount) as total from pmr_trn_tpurchaseorder a   left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " where a.vendor_gid='"+vendor_gid+"') as totalamount," +
                    " (select count(a.invoice_gid) from acp_trn_tinvoice a  " +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid" +
                    " where a.vendor_gid='"+vendor_gid+"') as invoicecount," +
                    " (select sum(a.invoice_amount) from acp_trn_tinvoice a  left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid" +
                    " where a.vendor_gid='"+vendor_gid+ "') as invoiceamount," +
                    " (select count(a.quotation_gid) from pmr_trn_tquotation a " +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " where a.vendor_gid='"+vendor_gid+ "') as purchasequotation," +
                    " (select sum(a.total_amount) from pmr_trn_tquotation a " +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    "  where a.vendor_gid='"+vendor_gid+ "') as quotationamount";

                dt_table = objconn.GetDataTable(msSQL);
                var vendorcount = new List<VendorTilesCount>();
                if (dt_table.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt_table.Rows)
                    {
                        vendorcount.Add(new VendorTilesCount
                        {
                            PO_count = dr["PO_count"].ToString(),
                            paymentCount = dr["paymentCount"].ToString(),
                            paymentamount = dr["paymentamount"].ToString(),
                            totalamount = dr["totalamount"].ToString(),
                            invoicecount = dr["invoicecount"].ToString(),
                            invoiceamount = dr["invoiceamount"].ToString(),
                            purchasequotation = dr["purchasequotation"].ToString(),
                            quotationamount = dr["quotationamount"].ToString()
                        });
                        values.VendorTilesCount = vendorcount;
                    }
                }
                dt_table.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Vendor Count !";
                objcmnfunc.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetVendorDetails(string vendor_gid, MdlVendor360 values)
        {

            try
            {
                    msSQL = " select a.vendor_code,a.email_id,a.vendor_companyname,a.contactperson_name,a.contact_telephonenumber," +
                            " a.currencyexchange_gid, date_format(a.created_date, '%d-%m-%Y') as created_date, " +
                            " concat(b.address1,',',b.address2,',',b.city,',',b.state,',',b.postal_code) as vendor_address  " +
                            " from acp_mst_tvendor a   left join adm_mst_taddress  b on b.address_gid=a.address_gid " +
                            " where a.vendor_gid='"+ vendor_gid + "'";

                    dt_table = objconn.GetDataTable(msSQL);
                    var getModuleList = new List<vendordetails>();
                    if (dt_table.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_table.Rows)
                        {
                            getModuleList.Add(new vendordetails
                            {
                                vendor_code = dt["vendor_code"].ToString(),
                                email_id = dt["email_id"].ToString(),
                                vendor_companyname = dt["vendor_companyname"].ToString(),
                                contactperson_name = dt["contactperson_name"].ToString(),
                                contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                                created_date = dt["created_date"].ToString()
                            });
                            values.vendordetails = getModuleList;
                        }
                    }
                    dt_table.Dispose();
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Summary !";
                objcmnfunc.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetPurchasetatus(string vendor_gid,MdlVendor360 values)
        {
            try
            {
                msSQL = " SELECT Months, SUM(po_count) AS po_count, SUM(invoice_count) AS invoice_count" +
                        " FROM (SELECT DATE_FORMAT(a.created_date, '%b') AS Months, COUNT(a.purchaseorder_gid) AS po_count, " +
                        " 0 AS invoice_count, 0 AS enquiry_count, 0 AS order_count,created_date AS ordermonth FROM pmr_trn_tpurchaseorder a " +
                        " WHERE a.vendor_gid='" + vendor_gid + "' " +
                        " and a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months UNION ALL  " +
                        " SELECT DATE_FORMAT(a.created_date,'%b') AS Months, 0 AS customer_count, " +
                        " COUNT(a.invoice_gid) AS invoice_count, 0 AS enquiry_count, 0 AS order_count,created_date AS ordermonth FROM acp_trn_tinvoice a  " +
                        " WHERE a.vendor_gid='" + vendor_gid + "' " +
                        " and a.created_date >= CURDATE() - INTERVAL 6 MONTH GROUP BY Months )AS combined_data " +
                        " GROUP BY Months  order by ordermonth";
                       

                dt_table = objconn.GetDataTable(msSQL);
                var GetSalesStatus_list = new List<purchasecount>();
                if (dt_table.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_table.Rows)
                    {
                        GetSalesStatus_list.Add(new purchasecount
                        {
                            po_count = (dt["po_count"].ToString()),
                            invoice_count = (dt["invoice_count"].ToString()),
                            Months = (dt["Months"].ToString()),
                        });
                        values.purchasecount = GetSalesStatus_list;
                    }
                }
                dt_table.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunc.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetPaymentCount(string vendor_gid, MdlVendor360 values) 
        {
            try 
            {
                msSQL = "select(select count(payment_status) as cancelled_payment  from acp_trn_tpayment" +
                    " where vendor_gid='" + vendor_gid + "' and payment_status = 'Payment Canceled' or payment_status = 'PY Canceled') as cancelled_payment ,  " +
                    " (select count(payment_status) as pending_payment  from acp_trn_tpayment" +
                    " where  vendor_gid='" + vendor_gid + " ' and payment_status = 'PY Approved') as approved_payment," +
                    " (select count(payment_status) as completed_payment " +
                    " from acp_trn_tpayment" +
                    " where  vendor_gid='" + vendor_gid + " ' and payment_status = 'Payment Done') as completed_payment  ";

                dt_table = objconn.GetDataTable(msSQL);
                var getdata = new List<paymentcount_list>();
                if (dt_table.Rows.Count != 0) 
                {
                    foreach(DataRow row in dt_table.Rows) 
                    {
                        getdata.Add(new paymentcount_list
                        {
                            cancelled_payment = row["cancelled_payment"].ToString(),
                            approved_payment = row["approved_payment"].ToString(),
                            completed_payment = row["completed_payment"].ToString()
                        });
                        values.paymentcount_list = getdata;
                    }
                }
                dt_table.Dispose();
            }
            catch(Exception ex) 
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunc.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}