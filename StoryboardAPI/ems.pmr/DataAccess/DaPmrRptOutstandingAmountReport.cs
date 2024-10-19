﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;
using System.Globalization;


namespace ems.pmr.DataAccess
{
    public class DaPmrRptOutstandingAmountReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetOutstandingAmountReportSummary(MdlPmrRptOutstandingAmountReport values)
        {
            try
            {


                msSQL = "select invoice_gid,invoice_refno,replace (invoice_status,'IV','Inovice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days," +
               " due_date,vendor_gid,invoice_amount,invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, CASE WHEN a.invoice_refno IS NULL OR a.invoice_refno = '' THEN a.invoice_gid ELSE a.invoice_refno END AS invoice_refno, a.invoice_status, a.invoice_flag, " +
                " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                " else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, " +
                " date_format(a.payment_date,'%d-%m-%Y') as due_date, " +
                " a.vendor_gid, ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, " +
                " a.payment_flag,c.branch_name, " +
                " concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails, a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount " +
                " from acp_trn_tinvoice a " +
                " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                " left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                " where a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc,invoice_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getoutstandingamountreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getoutstandingamountreport_list
                        {

                            purchaseorder_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_refnodate = dt["vendor_refnodate"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            companydetails = dt["companydetails"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            pending_days = dt["pending_days"].ToString(),
                            invoice_status = dt["overall_status"].ToString(),
                        });
                        values.Getoutstandingamountreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting outstanding report summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
          ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
          msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
          DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetOutstandingAmountSearch(MdlPmrRptOutstandingAmountReport values, string from_date, string to_date)
        {
            try
            {
                string inputDate = from_date;
                DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string f_date = uiDate.ToString("yyyy-MM-dd");

                string Date = to_date;
                DateTime uiDate1 = DateTime.ParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string t_date = uiDate1.ToString("yyyy-MM-dd");

                if (f_date == null && t_date == null)
                {
                    msSQL = "select invoice_gid,invoice_refno,invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days," +
                            " due_date,vendor_gid,invoice_amount,invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag, " +
                             " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                             " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                             " else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                             " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, " +
                             " date_format(a.payment_date,'%d-%m-%Y') as due_date, " +
                             " a.vendor_gid, ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, " +
                             " a.payment_flag,c.branch_name, " +
                             " concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails, a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                             " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount " +
                             " from acp_trn_tinvoice a " +
                             " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                             " left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                             " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                             " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                             " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                             " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                             " where a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by invoice_date desc";

                }
                else
                {
                    msSQL = "select invoice_gid,invoice_refno,invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days," +
                            " due_date,vendor_gid,invoice_amount,invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag, " +
                             " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                             " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                             " else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                             " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, " +
                             " date_format(a.payment_date,'%d-%m-%Y') as due_date, " +
                             " a.vendor_gid, ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, " +
                             " a.payment_flag,c.branch_name, " +
                             " concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails, a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                             " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount " +
                             " from acp_trn_tinvoice a " +
                             " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                             " left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                             " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                             " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                             " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                             " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                             " where a.invoice_status <> 'IV Canceled')x where invoice_date between '" + f_date + "' and '" + t_date + "'" +
                             " and  outstanding_amount <> '0.00' order by invoice_date desc";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var Getoutstandingamountreportsearch = new List<Getoutstandingamountreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        Getoutstandingamountreportsearch.Add(new Getoutstandingamountreport_list
                        {
                            purchaseorder_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_refnodate = dt["vendor_refnodate"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            companydetails = dt["companydetails"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            pending_days = dt["pending_days"].ToString(),
                            invoice_status = dt["overall_status"].ToString(),
                        });
                        values.Getoutstandingamountreport_list = Getoutstandingamountreportsearch;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

    }
}