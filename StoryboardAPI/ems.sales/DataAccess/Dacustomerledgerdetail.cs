﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;

//using Google.Protobuf.WellKnownTypes;
using System.Xml.Linq;

namespace ems.sales.DataAccess
{
    public class Dacustomerledgerdetail
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL1, msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsorder_type,
            lsdiscountpercentage, lsdiscountamount, lsprice, lstype1, lsproduct_price, mssalesorderGID,
            mssalesorderGID1, mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2,
            msGetGid3, lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetcustomerledgersalesorder(string customer_gid, Mdlcustomerledgerdetail values)
        {

            try
            {
                
                string currency = "INR";

                msSQL = " select distinct a.salesorder_gid,a.so_type, a.so_referenceno1,a.customer_gid, a.salesorder_date,c.user_firstname, " +
                    "  a.customer_contact_person, a.salesorder_status,a.currency_code, " +
                     " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                 " case when a.currency_code = '" + currency + "' then a.customer_name " +
                 "  when a.currency_code is null then a.customer_name " +
                 "  when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(a.customer_name,' / ',h.country) end as customer_name, " +
             " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                 " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact " +
                    "  from smr_trn_tsalesorder a " +
                    " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                      " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                      " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                    " left join smr_trn_tsalesorderdtl p on p.salesorder_gid=a.salesorder_gid" +
                    " where a.customer_gid='" + customer_gid + "'";
                msSQL += " order by  a.salesorder_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<customerledgersalesorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new customerledgersalesorder_list
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            so_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact"].ToString(),
                            type = dt["so_type"].ToString(),
                            amount = dt["Grandtotal"].ToString(),
                            created_by = dt["user_firstname"].ToString(),
                            so_status = dt["salesorder_status"].ToString(),

                        });
                        values.customerledgersalesorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading customerledger Salesorder Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
                $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

    }
        public void DaGetcustomerledgersalesorderdetail(string salesorder_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
               
                msSQL = "select a.qty_quoted,b.product_name from smr_trn_tsalesorderdtl a " +
                "left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                "where salesorder_gid='" + salesorder_gid + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledgersalesorderdetail_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledgersalesorderdetail_list
                    {
                       
                        product = dt["product_name"].ToString(),
                        qty_quoted = dt["qty_quoted"].ToString(),

                    });
                    values.customerledgersalesorderdetail_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customerledgersalesorder !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetcustomerledgerinvoice(string customer_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
               
                string currency = "INR";

            msSQL = "select distinct a.invoice_gid, a.invoice_refno,a.invoice_type, a.invoice_status,a.invoice_from,a.invoice_flag,a.mail_status,a.customer_gid,a.invoice_date, " +
                " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', " +
                " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                "  format(a.invoice_amount*a.exchange_rate,2) as invoice_amount, d.customer_contactperson, " +
                " case when a.currency_code = '" + currency + "' then c.customer_name when a.currency_code is null then c.customer_name " +
                "  when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(c.customer_name,' / ',h.country) end as customer_name, " +
                " case when d.customer_contactnumber is null then concat(a.customer_contactperson,'/', a.customer_contactnumber)   when a.customer_contactnumber is not null then concat(a.customer_contactperson,'/',a.customer_contactnumber)  end as contact" +
                " from rbl_trn_tinvoice a " +
                " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                  " left join rbl_trn_tso2invoice f on f.invoice_gid=a.invoice_gid " +
                      " left join smr_trn_tdeliveryorder d on d.directorder_gid = f.directorder_gid " +
                       " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                  " left join crm_mst_tcustomercontact e on e.customer_gid=c.customer_gid " +
                   " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                   " where a.customer_gid= '" + customer_gid + "'";

            msSQL += " order by a.invoice_date desc, a.invoice_gid desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledgerinvoice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledgerinvoice_list
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_referenceno1 = dt["invoice_refno"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact_details = dt["contact"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        amount = dt["invoice_amount"].ToString(),
                        invoice_status = dt["invoice_status"].ToString(),

                    });
                    values.customerledgerinvoice_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customerledger Invoice Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetcustomerledgerinvoicedetail(string invoice_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
               

                msSQL = "SELECT a.invoice_gid,j.invoicedtl_gid, j.product_gid,j.qty_invoice, b.product_name " +
                " FROM rbl_trn_tinvoice a " +
                " left join rbl_trn_tinvoicedtl j on j.invoice_gid = a.invoice_gid  " +
                " left join pmr_mst_tproduct b on b.product_gid = j.product_gid  " +
                " where a.invoice_gid = '" + invoice_gid + "' order by j.invoicedtl_gid desc  ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledgerinvoicedetail_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledgerinvoicedetail_list
                    {

                        product = dt["product_name"].ToString(),
                        qty_invoice = dt["qty_invoice"].ToString(),

                    });
                    values.customerledgerinvoicedetail_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customerledger Invoicedetail Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetcustomerledgerpayment(string customer_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
              

                msSQL = " select a.payment_gid,b.customer_gid,a.payment_type,b.invoice_refno, a.directorder_gid, format(a.amount_L,2) as amount, a.payment_mode, " +
                " a.payment_date ,a.approval_status, " +
                 " case when a.currency_code = 'INR' then b.customer_name " +
                 " when a.currency_code is null then b.customer_name " +
                 " when a.currency_code is not null and a.currency_code <> 'INR' then concat(b.customer_name,' / ',a.currency_code) end as customer_name, " +
                 " concat(b.customer_contactperson,' / ',b.customer_contactnumber,' / ',b.customer_email) as contact," +
                 " b.customer_gid, a.payment_type " +
                 " from rbl_trn_tpayment a " +
                 " left join rbl_trn_tinvoice b on b.invoice_gid=a.invoice_gid " +
                 " left join crm_mst_tcustomer c on c.customer_gid=b.customer_gid " +
                 " left join crm_mst_tcustomercontact d on d.customer_gid=b.customer_gid " +
                 " where b.customer_gid= '" + customer_gid + "'";
            msSQL += " order by a.payment_gid desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledgerpayment_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledgerpayment_list
                    {
                        payment_gid = dt["payment_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        payment_date = dt["payment_date"].ToString(),
                        payment_referenceno1 = dt["invoice_refno"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact_details = dt["contact"].ToString(),
                        type = dt["payment_mode"].ToString(),
                        amount = dt["amount"].ToString(),
                        payment_status = dt["approval_status"].ToString(),

                    });
                    values.customerledgerpayment_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customerledger Payment Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetcustomerledgerpaymentdetail(string payment_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
               
                msSQL = " select a.payment_gid,a.directorder_gid,a.invoice_gid,format(a.amount,2)as amount ,format(a.total_amount,2) as total_amount," +
                " format(b.invoice_amount,2)as invoice_amount ,c.qty_invoice,c.product_gid,d.product_name,format(a.tds_amount,2) as tds,a.payment_mode " +
                " from rbl_trn_tpayment a" +
                " left join rbl_trn_tinvoice b on b.invoice_gid=a.invoice_gid" +
                " left join rbl_trn_tinvoicedtl c on c.invoice_gid=a.invoice_gid" +
                " left join pmr_mst_tproduct d on d.product_gid=c.product_gid " +
                " left join smr_trn_tdeliveryorder f on f.directorder_gid=a.directorder_gid " +
                " left join smr_trn_tsalesorder e on e.salesorder_gid=f.salesorder_gid " +
                " where a.payment_gid='" + payment_gid + "' group by invoice_gid  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<customerledgerpaymentdetail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new customerledgerpaymentdetail_list
                        {

                            product = dt["product_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),

                        });
                        values.customerledgerpaymentdetail_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading customerledger Paymentdetail Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetcustomerledgeroutstanding(string customer_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
                

                msSQL = " select distinct a.invoice_gid,a.invoice_date,a.customer_gid,a.invoice_refno, a.invoice_status, a.invoice_flag,e.directorder_gid, " +
                " format(a.advance_amount,2) as advance_amount,ifnull(format(a.payment_amount*exchange_rate,2),'0') as payment_amount," +
                " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                " else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, " +
                " date_format(a.payment_date,'%d-%m-%Y') as due_date, " +
                " a.customer_gid, ifnull(format(a.invoice_amount*exchange_rate,2),'0') as invoice_amount, " +
                "  a.invoice_date, a.payment_flag, " +
                " c.customer_gid, concat(c.customer_name,'/',d.customercontact_name,'/',d.mobile) as companydetails, a.invoice_from, " +
                " ifnull(format(((a.invoice_amount*exchange_rate)-(a.payment_amount*exchange_rate)),2),'0') as outstanding_amount " +
                " from rbl_trn_tinvoice a " +
                " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                " left join crm_mst_tcustomercontact d on a.customer_gid = d.customer_gid " +
                " left join rbl_trn_tso2invoice e on e.invoice_gid=a.invoice_gid " +
                 " where a.customer_gid= '" + customer_gid + "' and a.invoice_amount  <> a.payment_amount";


            msSQL += " order by a.invoice_date desc, a.invoice_gid desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledgeroutstanding_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledgeroutstanding_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        invoice_referenceno1 = dt["invoice_refno"].ToString(),
                        customer_details = dt["companydetails"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        days_expired = dt["pending_days"].ToString(),
                        outstanding_status = dt["overall_status"].ToString()

                    });
                    values.customerledgeroutstanding_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customerledger Outstanding Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetcustomerledgercount(string customer_gid, Mdlcustomerledgerdetail values)
        {
            try
            {
               
                msSQL = "SELECT " +
                        "(SELECT COUNT(salesorder_gid) FROM smr_trn_tsalesorder WHERE customer_gid = '"+customer_gid+"' AND salesorder_status<> 'SO Amended') as total_so,"+
                        "(SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice WHERE customer_gid = '"+customer_gid+"' AND invoice_status <> 'Payment done') as total_invoice," +
                        "(SELECT COUNT(a.payment_gid) FROM rbl_trn_tpayment a LEFT JOIN rbl_trn_tinvoice b ON a.invoice_gid = b.invoice_gid WHERE b.customer_gid = '"+customer_gid+"') as Payment," +
                        "IFNULL(FORMAT((SELECT SUM(invoice_amount) - SUM(payment_amount) FROM rbl_trn_tinvoice WHERE customer_gid = '"+customer_gid+"'), 2), '0') as outstanding_amount";



            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledgercount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledgercount_list
                    {
                        salesorder_count = dt["total_so"].ToString(),
                        invoice_count = dt["total_invoice"].ToString(),
                        payment_count = dt["Payment"].ToString(),
                        outstanding_count = dt["outstanding_amount"].ToString()

                    });
                    values.customerledgercount_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading customerledgercount !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}


