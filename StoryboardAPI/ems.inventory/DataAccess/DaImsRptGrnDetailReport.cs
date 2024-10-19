using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaImsRptGrnDetailReport
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsstart_date, lsend_date, lsMainbranch_flag;


        public void DaGetVendor(MdlImsRptGrnDetailReport values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select a.vendor_gid,a.vendor_companyname " +
                        " from acp_mst_tvendor a " +
                        " order by a.vendor_companyname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<vendor_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new vendor_lists
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                        });
                        values.vendor_lists = getModuleList;
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


        public void DaGetImsRptGrndetailreport(MdlImsRptGrnDetailReport values)
        {
            try
            {
                msSQL = " select c.grn_gid,date_format(c.grn_date, '%d-%m-%Y') as grn_date,c.grn_status,b.purchaseorder_gid,d.product_code,d.product_name,i.productuom_name, format(a.qty_delivered,0)as qty_delivered,k.costcenter_name, c.invoice_flag,h.branch_name," +
                " format(a.qty_rejected,0) as qty_rejected ,e.vendor_gid, e.vendor_companyname,(f.user_firstname) as approved_by ,c.approved_date,c.grn_flag,format((a.qty_delivered-a.qty_rejected)* g.product_price,2) as amount, " +
                " CASE when c.invoice_flag <> 'IV Pending' then c.invoice_flag " +
                " else c.grn_flag end as 'overall_status', format(g.qty_ordered,0)as qty_ordered,(g.qty_ordered-g.qty_received) as qty_pending " +
                " from pmr_trn_tgrndtl a " +
                " inner join pmr_trn_tgrn c on c.grn_gid=a.grn_gid " +
                " inner join pmr_trn_tpurchaseorder b on b.purchaseorder_gid=c.purchaseorder_gid " +
                " inner join pmr_mst_tproduct d on d.product_gid= a.product_gid " +
                " inner join acp_mst_tvendor e on e.vendor_gid = c.vendor_gid " +
                " inner join adm_mst_tuser f on f.user_gid = c.approved_by " +
                " inner join hrm_mst_tbranch h on h.branch_gid=c.branch_gid" +
                " inner join pmr_trn_tpurchaseorderdtl g on g.purchaseorderdtl_gid = a.purchaseorderdtl_gid " +
                " left join pmr_mst_tcostcenter k on b.costcenter_gid=k.costcenter_gid" +
                " left join pmr_mst_tproductuom i on i.productuom_gid = a.uom_gid" +
                " where c.vendor_gid = b.vendor_gid Order by  c.grn_date desc, grn_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<grn_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new grn_lists
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            grn_status = dt["grn_status"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            qty_rejected = dt["qty_rejected"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            approved_by = dt["approved_by"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                            amount = dt["amount"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            qty_pending = dt["qty_pending"].ToString(),
                        });
                        values.grn_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetImsRptGrndetailreportsearch(string from_date, string to_date, string vendor_gid, MdlImsRptGrnDetailReport values)
        {
            try
            {

                if (from_date == "" || from_date == "null" && to_date== ""||to_date == "null")
                {
                    lsstart_date = from_date;
                    lsend_date = to_date;
                }
                else
                {
                    DateTime from_date1 = DateTime.ParseExact(from_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsstart_date = from_date1.ToString("yyyy-MM-dd");

                    DateTime lsDateto = DateTime.ParseExact(to_date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    lsend_date = lsDateto.ToString("yyyy-MM-dd");
                }



                msSQL = " select c.grn_gid,date(c.grn_date) as grn_date,c.grn_status,b.purchaseorder_gid,d.product_code,d.product_name,i.productuom_name, format(a.qty_delivered,0)as qty_delivered,k.costcenter_name, c.invoice_flag,h.branch_name," +
                " format(a.qty_rejected,0) as qty_rejected ,e.vendor_gid, e.vendor_companyname,(f.user_firstname) as approved_by ,c.approved_date,c.grn_flag,format((a.qty_delivered-a.qty_rejected)* g.product_price,2) as amount, " +
                " CASE when c.invoice_flag <> 'IV Pending' then c.invoice_flag " +
                " else c.grn_flag end as 'overall_status', format(g.qty_ordered,0)as qty_ordered,(g.qty_ordered-g.qty_received) as qty_pending " +
                " from pmr_trn_tgrndtl a " +
                " inner join pmr_trn_tgrn c on c.grn_gid=a.grn_gid " +
                " inner join pmr_trn_tpurchaseorder b on b.purchaseorder_gid=c.purchaseorder_gid " +
                " inner join pmr_mst_tproduct d on d.product_gid= a.product_gid " +
                " inner join acp_mst_tvendor e on e.vendor_gid = c.vendor_gid " +
                " inner join adm_mst_tuser f on f.user_gid = c.approved_by " +
                " inner join hrm_mst_tbranch h on h.branch_gid=c.branch_gid" +
                " inner join pmr_trn_tpurchaseorderdtl g on g.purchaseorderdtl_gid = a.purchaseorderdtl_gid " +
                " left join pmr_mst_tcostcenter k on b.costcenter_gid=k.costcenter_gid" +
                " left join pmr_mst_tproductuom i on i.productuom_gid = a.uom_gid" +
                " where c.vendor_gid = b.vendor_gid ";
                if (vendor_gid =="" || vendor_gid == "null") {
                    
                }

                else
                {
                    msSQL += " and e.vendor_gid ='" + vendor_gid + "'";
                }

                if ( lsstart_date =="" || lsstart_date == "null")
                {
                   
                }
                else
                {
                    msSQL += " and c.grn_date >= '" + lsstart_date + "'";
                }
                if (lsend_date == "null")
                {
                    
                }
                else
                {
                    msSQL += "AND c.grn_date <= '" + lsend_date + "'";
                }
                msSQL += " Order by  c.grn_date desc, grn_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<grn_lists>();

                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new grn_lists
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            grn_status = dt["grn_status"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            qty_rejected = dt["qty_rejected"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            approved_by = dt["approved_by"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                            amount = dt["amount"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            qty_pending = dt["qty_pending"].ToString(),
                        });
                    }
                    values.grn_lists = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

    }
}