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
    public class DaSmrRptSalesOrderDetailedReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL1, msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, currency ,lsorder_type, lsprice, lstype1, lsproduct_price, mssalesorderGID, mssalesorderGID1, mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2, msGetGid3, lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetSmrTrnSalesorderDetailedReportsummary(MdlSmrRptSalesOrderDetailedReport values)
        {
            try
            {
               
                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

            if(objOdbcDataReader.HasRows) {
                    
                currency = objOdbcDataReader["currency_code"].ToString();
                    
            }
                objOdbcDataReader.Close();

                msSQL = " select distinct a.salesorder_gid, concat(a.so_referenceno1,'/',a.salesorder_gid) as so_referenceno1, date_format(a.salesorder_date, '%d-%b-%Y') as salesorder_date ,c.user_firstname,date_format(a.approved_date, '%d-%b-%Y') as approved_date,f.delivered_date,f.directorder_date,g.user_firstname as approved_by, " +
                    "  a.customer_contact_person, a.salesorder_status,a.currency_code,f.delivered_by, " +
                    " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                    " case when a.currency_code = '" + currency + "' then a.customer_name " +
                    "  when a.currency_code is null then a.customer_name " +
                    "  when a.currency_code is not null and a.currency_code <> '" + currency + "' then concat(a.customer_name,' / ',h.country) end as customer_name, " +
                    " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                    " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact " +
                    "  from smr_trn_tsalesorder a " +
                    " inner join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                    " inner join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " inner join adm_mst_tuser c on b.user_gid= c.user_gid " +
                    " left join smr_trn_tdeliveryorder f on a.salesorder_gid=f.salesorder_gid " +
                    " inner join adm_mst_tuser g on a.approved_by=g.user_gid " +
                    " where so_type='Sales' " +
                    " group by salesorder_gid  order by date(a.salesorder_date) desc,a.salesorder_date asc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesorderdetail_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new salesorderdetail_list
                    {
                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        salesorder_date = dt["salesorder_date"].ToString(),
                        so_referenceno1 = dt["so_referenceno1"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact = dt["contact"].ToString(),
                        Grandtotal = dt["Grandtotal"].ToString(),
                        user_firstname = dt["user_firstname"].ToString(),
                        salesorder_status = dt["salesorder_status"].ToString(),
                        approved_date = dt["approved_date"].ToString(),
                        delivered_date = dt["delivered_date"].ToString(),
                        approved_by = dt["approved_by"].ToString(),
                        customer_contact_person = dt["customer_contact_person"].ToString(),
                        currency_code= dt["currency_code"].ToString(),
                        delivered_by = dt["delivered_by"].ToString(),

                    });
                    values.salesorderdetail_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading  Sales Order Detail Report Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

    }
}