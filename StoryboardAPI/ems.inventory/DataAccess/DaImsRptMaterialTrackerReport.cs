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
    public class DaImsRptMaterialTrackerReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsMainbranch_flag, lsbranch_gid;
        public void DaGetBranch(MdlImsRptMaterialTracker values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select branch_name,branch_gid " +
                        " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branchlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branchlist
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.branchlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImsRptmaterialreport(string branch_gid, MdlImsRptMaterialTracker values)
        {
            try
            {
                msSQL = " Select mainbranch_flag from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
                lsMainbranch_flag = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct f.materialrequisitiondtl_gid,date_format(a.materialrequisition_date, '%d-%m-%Y') as materialrequisition_date,a.materialrequisition_gid,d.product_gid," +
                " d.product_name,d.product_code,e.productuom_name,f.qty_requested,a.materialrequisition_status," +
                " date_format(b.purchaserequisition_date, '%d-%m-%Y') as purchaserequisition_date," +
                " case when b.purchaserequisition_gid is null then '--' else b.purchaserequisition_gid end as purchaserequisition_gid ," +
                " f.qty_issued " +
                " from ims_trn_tmaterialrequisition a" +
                " left join ims_trn_tmaterialrequisitiondtl f on f.materialrequisition_gid=a.materialrequisition_gid " +
                " inner join pmr_mst_tproduct d on d.product_gid = f.product_gid " +
                " left join pmr_mst_tproductuom e on e.productuom_gid = f.productuom_gid " +
                " left join pmr_trn_tpurchaserequisition b on a.materialrequisition_gid = b.materialrequisition_gid " +
                " left join pmr_trn_tpurchaserequisitiondtl g on b.purchaserequisition_gid = g.purchaserequisition_gid";

                if (lsMainbranch_flag != "Y" && branch_gid =="" ) { 
                    
                    msSQL += " where a.branch_gid ='" + branch_gid + "' order by date(a.materialrequisition_date) desc, a.materialrequisition_date asc, f.materialrequisitiondtl_gid desc ";
                }
                else if (lsMainbranch_flag != "Y"){
                    msSQL += " where a.branch_gid ='" + branch_gid + "' order by date(a.materialrequisition_date) desc, a.materialrequisition_date asc, f.materialrequisitiondtl_gid desc";
                }
                else{
                    msSQL += " where 0=0 order by date(a.materialrequisition_date) desc,a.materialrequisition_date asc,f.materialrequisitiondtl_gid desc";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialtrackerlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new materialtrackerlist
                        {
                            materialrequisitiondtl_gid = dt["materialrequisitiondtl_gid"].ToString(),
                            materialrequisition_date = dt["materialrequisition_date"].ToString(),
                            materialrequisition_gid = dt["materialrequisition_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            materialrequisition_status = dt["materialrequisition_status"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            qty_issued = dt["qty_issued"].ToString(),
                        });
                        values.materialtrackerlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Material Tracker Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetImsRptmaterialporeport(string branch_gid, MdlImsRptMaterialTracker values)
        {
            try
            {
                msSQL = " Select mainbranch_flag from hrm_mst_tbranch where branch_gid='" + branch_gid + "'";
                lsMainbranch_flag = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select a.purchaserequisition_gid," +
                " case when b.purchaseorder_gid is null then '--' else b.purchaseorder_gid end as purchaseorder_gid ," +
                " case when c.grn_gid is null then '--' else c.grn_gid end as grn_gid ," +
                " case when d.invoice_gid is null then '--' else d.invoice_gid end as invoice_gid ," +
                " case when e.payment_gid is null then '--' else e.payment_gid end as payment_gid ," +
                " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag " +
                " when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                " else a.purchaserequisition_flag end as 'overall_status' " +
                " from pmr_trn_tpurchaserequisition a " +
                " left join pmr_trn_tpurchaseorder b on a.purchaserequisition_gid = b.purchaserequisition_gid " +
                " left join pmr_trn_tgrn c on b.purchaseorder_gid = c.purchaseorder_gid " +
                " left join acp_trn_tpo2invoice d on b.purchaseorder_gid = d.purchaseorder_gid " +
                " left join acp_trn_tinvoice2payment e on d.invoice_gid = e.invoice_gid";
                if (lsMainbranch_flag != "Y" && branch_gid == "") { 
                    msSQL += " where a.branch_gid ='" + branch_gid + "' order by a.purchaserequisition_gid desc";
                }
                else if (lsMainbranch_flag != "Y") {
                    msSQL += " where a.branch_gid ='" + branch_gid + "' order by a.purchaserequisition_gid desc";
                }
                else {
                    msSQL += " where 0=0 order by a.purchaserequisition_gid desc";
                }



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<materialtracker_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new materialtracker_list1
                        {
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            grn_gid = dt["grn_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                        });
                        values.materialtracker_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Material Tracker Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}