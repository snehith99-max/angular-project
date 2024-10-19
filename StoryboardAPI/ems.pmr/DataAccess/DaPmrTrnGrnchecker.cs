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
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Text;
using System.Web;


namespace ems.pmr.DataAccess
{
    public class DaPmrTrnGrnchecker
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetPmrTrnGrnchecker(string user_gid,MdlPmrTrnGrnchecker values)
        {
            try
            {
                
                msSQL = "SELECT DISTINCT a.grn_gid, a.grn_gid AS grnrefno, a.dc_no, a.grn_status, a.vendor_gid, a.checkeruser_gid, a.grn_flag, " +
                    "CASE    WHEN a.invoice_flag<> 'IV Pending' THEN a.invoice_flag  " +
                    "ELSE a.grn_flag   END AS overall_status, " +
                    "CASE   WHEN group_concat(DISTINCT e.purchaserequisition_referencenumber) = ',' THEN ''  " +
                    " WHEN group_concat(DISTINCT e.purchaserequisition_referencenumber) <> ',' THEN group_concat(DISTINCT e.purchaserequisition_referencenumber) " +
                   " END AS reference_no, " +
                  " DATE(a.grn_date) AS grn_date, c.vendor_companyname, a.purchaseorder_gid, FORMAT(d.total_amount, 2) AS po_amount, a.created_date " +
                    " FROM pmr_trn_tgrn a " +
                    " LEFT JOIN pmr_trn_tgrndtl b ON a.grn_gid = b.grn_gid " +
                    " LEFT JOIN acp_mst_tvendor c ON a.vendor_gid = c.vendor_gid " +
                    " LEFT JOIN pmr_trn_tpurchaseorder d ON d.purchaseorder_gid = a.purchaseorder_gid " +
                   " LEFT JOIN pmr_trn_tpurchaserequisition e ON e.purchaserequisition_gid = d.purchaserequisition_gid " +
                  " where 0 = 0  and a.checkeruser_gid  = '" + user_gid + "'and a.grn_flag = 'GRN Pending QC' group by a.grn_gid order by date(a.grn_date) desc,a.grn_date asc, a.grn_gid desc ";
      
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetGrnChecker_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetGrnChecker_list
                    {

                        grn_gid = dt["grn_gid"].ToString(),
                        grn_date = dt["grn_date"].ToString(),
                        grnrefno = dt["grnrefno"].ToString(),
                        purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        reference_no = dt["reference_no"].ToString(),
                        po_amount = dt["po_amount"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        overall_status = dt["overall_status"].ToString(),
                        dc_no = dt["dc_no"].ToString(),

                    });
                    values.GetGrnChecker_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                 $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                 ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                 msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                 DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
    }
}