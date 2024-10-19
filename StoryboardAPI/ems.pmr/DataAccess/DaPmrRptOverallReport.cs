﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;


namespace ems.pmr.DataAccess
{
    public class DaPmrRptOverallReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objODBCDatareader;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetOverallReportSummary(MdlPmrRptOverallReport values)
        {
            try
            {
                 

                msSQL = "select  b.purchaseorder_gid, c.grn_gid, d.invoice_gid, e.payment_gid,h.branch_name, a.purchaseorder_status,"+
                 " a.grn_flag,case when a.invoice_flag='IV Pending' then 'Invoice Pending' "+
                  "else a.invoice_flag end as 'invoice_flag',"+
                  "case when a.payment_flag='PY Pending'  then 'Payment Pending'   else a.payment_flag end as 'payment_flag'  "+    
                 "from pmr_trn_tpurchaseorder a "+
               " left join pmr_trn_tpurchaseorder b on a.purchaseorder_gid = b.purchaseorder_gid " +
                 "left join pmr_trn_tgrn c on b.purchaseorder_gid = c.purchaseorder_gid " +
                " left join acp_trn_tpo2invoice d on b.purchaseorder_gid = d.purchaseorder_gid " +
                 "left join acp_trn_tinvoice2payment e on d.invoice_gid = e.invoice_gid " +
                 "left join hrm_mst_tbranch h on h.branch_gid=a.branch_gid";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<overallreport_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new overallreport_list
                    {

                        purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                        grn_gid = dt["grn_gid"].ToString(),
                        invoice_gid = dt["invoice_gid"].ToString(),
                        payment_gid = dt["payment_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        purchaseorder_status = dt["purchaseorder_status"].ToString(),
                        grn_flag = dt["grn_flag"].ToString(),
                        payment_flag = dt["payment_flag"].ToString(),
                        invoice_flag = dt["invoice_flag"].ToString()



                    });
                    values.overallreport_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting OverallReportSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
         ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
         msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
         DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
    }
}