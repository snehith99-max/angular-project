using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.finance.Models;

namespace ems.finance.DataAccess
{
    public class DaGstManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objODBCDatareader;
        DataTable dt_datatable;
        string msEmployeeGID, msGetDlGID2, msGetGid2, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGstManagementSummary(MdlGstManagement values)
        {
            try
            {
                msSQL = " select distinct month,year,branch_name,gst_no, '' as iptax,'' as optax,'' as tax_payable,'' as taxfilling_date,'' as filling_refno from  " +
                        " ( select distinct cast(monthname( a.invoice_date) as char) as month,year(a.invoice_date) as year, " +
                        " a.invoice_gid,b.branch_name,gst_no,a.invoice_date from rbl_trn_tinvoice a " +
                        " inner join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                        " group by monthname(a.invoice_date),year(a.invoice_date) " +
                        " union " +
                        " select distinct cast(monthname( a.invoice_date) as char) as month,year(a.invoice_date) as year, " +
                        " a.invoice_gid,b.branch_name,b.gst_no,a.invoice_date " +
                        " from acp_trn_tinvoice a " +
                        " inner join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                        " group by monthname(a.invoice_date),year(a.invoice_date) " +
                        " order by year(invoice_date) desc,month(str_to_date(substring(monthname(invoice_date),1,3),'%b')) desc)x ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetGstManagement_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetGstManagement_list
                        {
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            gst_no = dt["gst_no"].ToString(),
                            iptax = dt["iptax"].ToString(),
                            optax = dt["optax"].ToString(),
                            tax_payable = dt["tax_payable"].ToString(),
                            taxfilling_date = dt["taxfilling_date"].ToString(),
                            filling_refno = dt["filling_refno"].ToString(),
                        });
                    }
                    values.GetGstManagement_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                                             "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                                             " * **********" + ex.Message.ToString() + "***********" + msSQL +
                                             "*******Apiref********", "ErrorLog/Finance " + "Log" +
                                             DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInGstManagementSummary(MdlGstManagement values, string month, string year)
        {
            try
            {
                msSQL = " select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                         " a.invoice_refno,b.vendor_companyname ,b.gst_number, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' " +
                          " from pbl_trn_vinvoicetax x " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_0,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5'  " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_2point5, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_6,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_9,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_14," +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0'  " +
                          " and x.tax_name like 'CGST%'),2) as CGST_0, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                          " and x.tax_name like 'CGST%'),2) as CGST_2point5, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as CGST_6 " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                          " and x.tax_name like 'CGST%'),2) as CGST_6,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as CGST_9 " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9'  " +
                          " and x.tax_name like 'CGST%'),2) as CGST_9,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as CGST_14 " +
                          " from pbl_trn_vinvoicetax x                          " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                          " and x.tax_name like 'CGST%'),2) as CGST_14,         " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from pbl_trn_vinvoicetax x   " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' from pbl_trn_vinvoicetax x                        " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name like 'IGST%'),2) as IGST_5, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='12' and x.tax_name like 'IGST%'),2) as IGST_12, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from pbl_trn_vinvoicetax x                          " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as IGST_18,       " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from pbl_trn_vinvoicetax x                         " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as IGST_28,     " +
                          " format(a.invoice_amount,2) as invoice_amount, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) from pbl_trn_vinvoicetax x                                        " +
                          "  where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount,                                                   " +
                          " format((select ifnull(sum((qty_invoice * product_price)-x.discount_amount),0.00) +  " +
                          " ifnull((additionalcharges_amount+freightcharges+round_off),0.00)- ifnull((y.discount_amount+buybackorscrap),0.00) " +
                          " from acp_trn_tinvoicedtl x " +
                          " inner join acp_trn_tinvoice y on x.invoice_gid=y.invoice_gid " +
                          " where y.invoice_gid=a.invoice_gid group by y.invoice_gid),2) as Non_Taxable_Amount " +
                          " from acp_trn_tinvoice a " +
                          " inner join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid " +
                          " where monthname(a.invoice_date)='" + month + "' and year(a.invoice_date)='" + year + "' " +
                          " and a.ingst='N' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetInGstManagement_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetInGstManagement_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            SGST_0 = dt["SGST_0"].ToString(),
                            SGST_2point5 = dt["SGST_2point5"].ToString(),
                            SGST_6 = dt["SGST_6"].ToString(),
                            SGST_9 = dt["SGST_9"].ToString(),
                            SGST_14 = dt["SGST_14"].ToString(),
                            CGST_0 = dt["CGST_0"].ToString(),
                            CGST_2point5 = dt["CGST_2point5"].ToString(),
                            CGST_6 = dt["CGST_6"].ToString(),
                            CGST_9 = dt["CGST_9"].ToString(),
                            CGST_14 = dt["CGST_14"].ToString(),
                            IGST_0 = dt["IGST_0"].ToString(),
                            IGST_5 = dt["IGST_5"].ToString(),
                            IGST_12 = dt["IGST_12"].ToString(),
                            IGST_18 = dt["IGST_18"].ToString(),
                            IGST_28 = dt["IGST_28"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            Taxable_Amount = dt["Taxable_Amount"].ToString(),
                            Non_Taxable_Amount = dt["Non_Taxable_Amount"].ToString()
                        });
                        values.GetInGstManagement_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
              
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaOutGstManagementSummary(MdlGstManagement values, string month, string year)
        {
            try
            {
                msSQL = " select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                        " a.invoice_refno as invoice_refno,a.customer_name as customer_name,b.gst_number as gst_number, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' " +
                       " from rbl_trn_vinvoicetax x " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_0,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5'  " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_2point5, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_6,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_9,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_14," +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0'  " +
                       " and x.tax_name like 'CGST%'),2) as CGST_0, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                       " and x.tax_name like 'CGST%'),2) as CGST_2point5, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 6' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                       " and x.tax_name like 'CGST%'),2) as CGST_6,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 9' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9'  " +
                       " and x.tax_name like 'CGST%'),2) as CGST_9,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 14' " +
                       " from rbl_trn_vinvoicetax x                          " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                       " and x.tax_name like 'CGST%'),2) as CGST_14,         " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from rbl_trn_vinvoicetax x   " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' from rbl_trn_vinvoicetax x                        " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name like 'IGST%'),2) as IGST_5, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='12' and x.tax_name like 'IGST%'),2) as IGST_12, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from rbl_trn_vinvoicetax x                          " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as IGST_18,       " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from rbl_trn_vinvoicetax x                         " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as IGST_28,     " +
                       " format(a.invoice_amount,2) as invoice_amount, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) from rbl_trn_vinvoicetax x                                        " +
                       "  where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount,                                                   " +
                       " format((select sum(x.qty_invoice * (x.product_price-x.discount_amount))+ (ifnull((y.freight_charges+y.packing_charges " +
                       " + y.insurance_charges+y.additionalcharges_amount+roundoff),0.00))-ifnull(y.buyback_charges,0.00)-ifnull(y.discount_amount,0.00) " +
                       " from rbl_trn_tinvoicedtl x " +
                       " inner join rbl_trn_tinvoice y on x.invoice_gid=y.invoice_gid " +
                       " where a.invoice_gid=x.invoice_gid group by y.invoice_gid ),2) as Non_Taxable_Amount " +
                       " from rbl_trn_tinvoice a " +
                       " inner join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                       " where monthname(a.invoice_date)='" + month + "' and year(a.invoice_date)='" + year + "' " +
                       " and a.outgst='N' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOutGstManagement_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOutGstManagement_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            SGST_0 = dt["SGST_0"].ToString(),
                            SGST_2point5 = dt["SGST_2point5"].ToString(),
                            SGST_6 = dt["SGST_6"].ToString(),
                            SGST_9 = dt["SGST_9"].ToString(),
                            SGST_14 = dt["SGST_14"].ToString(),
                            CGST_0 = dt["CGST_0"].ToString(),
                            CGST_2point5 = dt["CGST_2point5"].ToString(),
                            CGST_6 = dt["CGST_6"].ToString(),
                            CGST_9 = dt["CGST_9"].ToString(),
                            CGST_14 = dt["CGST_14"].ToString(),
                            IGST_0 = dt["IGST_0"].ToString(),
                            IGST_5 = dt["IGST_5"].ToString(),
                            IGST_12 = dt["IGST_12"].ToString(),
                            IGST_18 = dt["IGST_18"].ToString(),
                            IGST_28 = dt["IGST_28"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            Taxable_Amount = dt["Taxable_Amount"].ToString(),
                            Non_Taxable_Amount = dt["Non_Taxable_Amount"].ToString()
                        });
                        values.GetOutGstManagement_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInFillingGstManagementSummary(MdlGstManagement values, string month, string year)
        {
            try
            {
                msSQL =   " select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                          " a.invoice_refno,b.vendor_companyname ,b.gst_number, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' " +
                          " from pbl_trn_vinvoicetax x " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_0,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5'  " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_2point5, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_6,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_9,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                          " and x.tax_name like 'SGST%'),2) as SGST_14," +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0'  " +
                          " and x.tax_name like 'CGST%'),2) as CGST_0, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                          " and x.tax_name like 'CGST%'),2) as CGST_2point5, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as CGST_6 " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                          " and x.tax_name like 'CGST%'),2) as CGST_6,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as CGST_9 " +
                          " from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9'  " +
                          " and x.tax_name like 'CGST%'),2) as CGST_9,  " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as CGST_14 " +
                          " from pbl_trn_vinvoicetax x                          " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                          " and x.tax_name like 'CGST%'),2) as CGST_14,         " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from pbl_trn_vinvoicetax x   " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' from pbl_trn_vinvoicetax x                        " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name like 'IGST%'),2) as IGST_5, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from pbl_trn_vinvoicetax x  " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='12' and x.tax_name like 'IGST%'),2) as IGST_12, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from pbl_trn_vinvoicetax x                          " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as IGST_18,       " +
                          " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from pbl_trn_vinvoicetax x                         " +
                          " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as IGST_28,     " +
                          " format(a.invoice_amount,2) as invoice_amount, " +
                          " format((select ifnull(sum(x.tax_amount),0.00) from pbl_trn_vinvoicetax x                                        " +
                          "  where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount,                                                   " +
                          " format((select ifnull(sum((qty_invoice * product_price)-x.discount_amount),0.00) +  " +
                          " ifnull((additionalcharges_amount+freightcharges+round_off),0.00)- ifnull((y.discount_amount+buybackorscrap),0.00) " +
                          " from acp_trn_tinvoicedtl x " +
                          " inner join acp_trn_tinvoice y on x.invoice_gid=y.invoice_gid " +
                          " where y.invoice_gid=a.invoice_gid group by y.invoice_gid),2) as Non_Taxable_Amount " +
                          " from acp_trn_tinvoice a " +
                          " inner join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid " +
                          " where monthname(a.invoice_date)='" + month + "' and year(a.invoice_date)='" + year + "' " +
                          " and a.ingst='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetInGstManagement_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetInGstManagement_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            SGST_0 = dt["SGST_0"].ToString(),
                            SGST_2point5 = dt["SGST_2point5"].ToString(),
                            SGST_6 = dt["SGST_6"].ToString(),
                            SGST_9 = dt["SGST_9"].ToString(),
                            SGST_14 = dt["SGST_14"].ToString(),
                            CGST_0 = dt["CGST_0"].ToString(),
                            CGST_2point5 = dt["CGST_2point5"].ToString(),
                            CGST_6 = dt["CGST_6"].ToString(),
                            CGST_9 = dt["CGST_9"].ToString(),
                            CGST_14 = dt["CGST_14"].ToString(),
                            IGST_0 = dt["IGST_0"].ToString(),
                            IGST_5 = dt["IGST_5"].ToString(),
                            IGST_12 = dt["IGST_12"].ToString(),
                            IGST_18 = dt["IGST_18"].ToString(),
                            IGST_28 = dt["IGST_28"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            Taxable_Amount = dt["Taxable_Amount"].ToString(),
                            Non_Taxable_Amount = dt["Non_Taxable_Amount"].ToString()
                        });
                        values.GetInGstManagement_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaOutFillingGstManagementSummary(MdlGstManagement values, string month, string year)
        {
            try
            {
                msSQL = " select a.invoice_gid,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, " +
                        " a.invoice_refno as invoice_refno,a.customer_name as customer_name,b.gst_number as gst_number, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST0' " +
                       " from rbl_trn_vinvoicetax x " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_0,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 2.5'  " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_2point5, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 6' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_6,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 9' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_9,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'SGST 14' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                       " and x.tax_name like 'SGST%'),2) as SGST_14," +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 0' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0'  " +
                       " and x.tax_name like 'CGST%'),2) as CGST_0, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 2.5' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='2.5' " +
                       " and x.tax_name like 'CGST%'),2) as CGST_2point5, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 6' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='6' " +
                       " and x.tax_name like 'CGST%'),2) as CGST_6,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 9' " +
                       " from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='9'  " +
                       " and x.tax_name like 'CGST%'),2) as CGST_9,  " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'CGST 14' " +
                       " from rbl_trn_vinvoicetax x                          " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='14' " +
                       " and x.tax_name like 'CGST%'),2) as CGST_14,         " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 0' from rbl_trn_vinvoicetax x   " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='0' and x.tax_name like 'IGST%'),2) as IGST_0, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 5' from rbl_trn_vinvoicetax x                        " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='5' and x.tax_name like 'IGST%'),2) as IGST_5, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 12' from rbl_trn_vinvoicetax x  " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='12' and x.tax_name like 'IGST%'),2) as IGST_12, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 18' from rbl_trn_vinvoicetax x                          " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='18' and x.tax_name like 'IGST%'),2) as IGST_18,       " +
                       " format((select ifnull(sum(x.tax_amount),0.00) as 'IGST 28' from rbl_trn_vinvoicetax x                         " +
                       " where x.invoice_gid=a.invoice_gid and x.tax_percentage='28' and x.tax_name like 'IGST%'),2) as IGST_28,     " +
                       " format(a.invoice_amount,2) as invoice_amount, " +
                       " format((select ifnull(sum(x.tax_amount),0.00) from rbl_trn_vinvoicetax x                                        " +
                       "  where x.invoice_gid=a.invoice_gid ),2) as Taxable_Amount,                                                   " +
                       " format((select sum(x.qty_invoice * (x.product_price-x.discount_amount))+ (ifnull((y.freight_charges+y.packing_charges " +
                       " + y.insurance_charges+y.additionalcharges_amount+roundoff),0.00))-ifnull(y.buyback_charges,0.00)-ifnull(y.discount_amount,0.00) " +
                       " from rbl_trn_tinvoicedtl x " +
                       " inner join rbl_trn_tinvoice y on x.invoice_gid=y.invoice_gid " +
                       " where a.invoice_gid=x.invoice_gid group by y.invoice_gid ),2) as Non_Taxable_Amount " +
                       " from rbl_trn_tinvoice a " +
                       " inner join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                       " where monthname(a.invoice_date)='" + month + "' and year(a.invoice_date)='" + year + "' " +
                       " and a.outgst='N' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOutGstManagement_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOutGstManagement_list
                        {                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            SGST_0 = dt["SGST_0"].ToString(),
                            SGST_2point5 = dt["SGST_2point5"].ToString(),
                            SGST_6 = dt["SGST_6"].ToString(),
                            SGST_9 = dt["SGST_9"].ToString(),
                            SGST_14 = dt["SGST_14"].ToString(),
                            CGST_0 = dt["CGST_0"].ToString(),
                            CGST_2point5 = dt["CGST_2point5"].ToString(),
                            CGST_6 = dt["CGST_6"].ToString(),
                            CGST_9 = dt["CGST_9"].ToString(),
                            CGST_14 = dt["CGST_14"].ToString(),
                            IGST_0 = dt["IGST_0"].ToString(),
                            IGST_5 = dt["IGST_5"].ToString(),
                            IGST_12 = dt["IGST_12"].ToString(),
                            IGST_18 = dt["IGST_18"].ToString(),
                            IGST_28 = dt["IGST_28"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            Taxable_Amount = dt["Taxable_Amount"].ToString(),
                            Non_Taxable_Amount = dt["Non_Taxable_Amount"].ToString()
                        });
                        values.GetOutGstManagement_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + msSQL + "*******Apiref********", "ErrorLog/Finance/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}