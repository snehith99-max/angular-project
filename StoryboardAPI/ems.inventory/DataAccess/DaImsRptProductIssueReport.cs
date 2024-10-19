using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaImsRptProductIssueReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsMainbranch_flag, lsbranch_gid;

        public void DaGetImsRptProductissuereport( MdlImsRptProductIssueReport values)
        {
            try
            {
                msSQL = " SELECT /*+ MAX_EXECUTION_TIME(300000) */ a.materialissued_gid,a.materialrequisition_gid,date_format(a.materialissued_date, '%d-%m-%Y') as materialissued_date ,a.materialissued_status,m.purchaserequisition_status as purchasestatus, " +
                         " case materialissued_status when 'Issued Accept Pending' then 'Issued' end as status ," +
                         " b.product_gid, c.product_code, c.product_name, d.productuom_name,b.qty_issued,b.qty_requested,g.unit_price,m.purchaserequisition_gid,ifnull(n.qty_requested,0) as purchase_requested_qty,f.branch_prefix," +
                         " format((g.unit_price * b.qty_issued),2) as Issued_value,g.display_field," +
                         " concat(e.user_firstname,'/',j.department_name)as issued_to,f.branch_name,(b.qty_requested-b.qty_issued) as balance_qty " +
                         " FROM ims_trn_tmaterialissued a " +
                         " left join ims_trn_tmaterialrequisitiondtl b on a.materialrequisition_gid = b.materialrequisition_gid " +
                         " left join pmr_mst_tproduct c on c.product_gid = b.product_gid" +
                         " left join pmr_mst_tproductgroup h on c.productgroup_gid = h.productgroup_gid" +
                         " left join pmr_mst_tproductuom d on d.productuom_gid = c.productuom_gid " +
                         " left join adm_mst_tuser e on b.user_gid = e.user_gid " +
                         " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid " +
                         " left join ims_trn_tstock g on b.product_gid = g.product_gid " +
                         " left join pmr_trn_tpurchaserequisition m on a.materialrequisition_gid = m.materialrequisition_gid " +
                         " left join pmr_trn_tpurchaserequisitiondtl n on m.purchaserequisition_gid = n.purchaserequisition_gid " +
                         " left join hrm_mst_temployee i on i.user_gid = e.user_gid " +
                         " left join hrm_mst_tdepartment j on j.department_gid = i.department_gid " +
                         " WHERE 1 = 1   group by product_gid, qty_issued,user_firstname,a.materialissued_date  order by materialissued_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_issuelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_issuelist
                        {
                            materialissued_gid = dt["materialissued_gid"].ToString(),
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            materialissued_date = dt["materialissued_date"].ToString(),
                            materialissued_status = dt["materialissued_status"].ToString(),
                            purchasestatus = dt["purchasestatus"].ToString(),
                            status = dt["status"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_issued = dt["qty_issued"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            unit_price = dt["unit_price"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchase_requested_qty = dt["purchase_requested_qty"].ToString(),
                            issued_value = dt["Issued_value"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            issued_to = dt["issued_to"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            balance_qty = dt["balance_qty"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.product_issuelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Product Issued Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

    }
}