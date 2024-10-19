﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
//using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;

using System.Web.UI.WebControls;
using System.Text;


namespace ems.pmr.DataAccess
{
    public class DaPayableDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetPayablesummary(payable_tile values)
        {
            try
            {
               
                msSQL = "select count(invoice_gid) as total_count, " +
                "(select count(invoice_status)as cancel_invoice  from acp_trn_tinvoice where invoice_status='IV Canceled') as cancel_invoice ," +
                    "(select count(invoice_status) as pending_count  from acp_trn_tinvoice where invoice_status = 'IV Work In Progress') as pending_count " +
                    " from acp_trn_tinvoice";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Read();

                    values.total_count = objOdbcDataReader["total_count"].ToString();
                    values.cancel_invoice = objOdbcDataReader["cancel_invoice"].ToString();
                    values.pending_count = objOdbcDataReader["pending_count"].ToString();


                    objOdbcDataReader.Close();
                }
                msSQL = "select count(*) as 'Product_count' from pmr_mst_tproduct where 1=1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Read();
                    values.product_count = objOdbcDataReader["Product_count"].ToString();
                    objOdbcDataReader.Close();
                }
                msSQL = "select count(*) as 'vendor_count' from acp_mst_tvendor where 1=1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Read();
                    values.vendor_count = objOdbcDataReader["vendor_count"].ToString();
                    objOdbcDataReader.Close();
                }

                return;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting payable summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaPayabledashboardsummary(MdlPayableDashboard values)
        {
            try
            {
              
                msSQL = "select invoice_gid,invoice_refno,format(invoice_amount,2) as invoice_amount, invoice_reference, CASE when payment_flag <> 'PY Pending' then payment_flag " +
         "  else invoice_flag end as 'overall_status', invoice_date, payment_date, " +
           "  case when replace(invoice_status,'IV','Invoice') = 'IV Approved' then 'IV Completed' else replace(invoice_status, 'IV', 'Invoice') end as invoice_status " +
           " from acp_trn_tinvoice a group by invoice_refno order by date(invoice_date) desc,invoice_date asc, invoice_gid desc,invoice_gid desc limit 5 ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<payablesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new payablesummary_list
                        {

                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = Convert.ToDateTime(dt["invoice_date"].ToString()),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),

                            invoice_amount = dt["invoice_amount"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                        });
                        values.payablesummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting dashboard summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

    }
}