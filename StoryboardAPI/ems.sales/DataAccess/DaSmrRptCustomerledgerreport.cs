﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;


namespace ems.sales.DataAccess
{
    public class DaSmrRptCustomerledgerreport
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

        public void DaGetCustomerledgerreportSummary(MdlSmrRptCustomerledgerreport values)
        {
            try
            {
                
                msSQL = "select a.so_type,a.salesorder_gid,a.customer_gid, c.customer_name,concat(d.customercontact_name,'/',d.mobile,'/',d.email) as details,group_concat(distinct e.product_name,' ') as product_name,sum(b.price) as amount, " +
                 " (select case when x.grandtotal_l='0.00' then format(sum(x.Grandtotal),2) " +
                 " when x.grandtotal_l<>'0.00' then format(sum(x.grandtotal_l),2) end as total from smr_trn_tsalesorder x where c.customer_gid=x.customer_gid) as total " +
                 " from smr_trn_tsalesorder a " +
                 " left join smr_trn_tsalesorderdtl b on a.salesorder_gid=b.salesorder_gid " +
                 " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                 " left join crm_mst_tcustomercontact d on c.customer_gid=d.customer_gid " +
                 " left join pmr_mst_tproduct e on b.product_gid=e.product_gid " +
                 " where a.salesorder_status not in ('Approve Pending','SO Amended','Rejected')";
            msSQL += "group by a.customer_gid";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customerledger_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customerledger_list
                    {

                        customer_gid = dt["customer_gid"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        order_type = dt["so_type"].ToString(),
                        contact_details = dt["details"].ToString(),
                        products = dt["product_name"].ToString(),
                        order_value = dt["amount"].ToString()



                    });
                    values.customerledger_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer Ledger Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }
    }
}