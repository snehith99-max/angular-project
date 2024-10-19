using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnStockConsumptionReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        public void DaGetImsRptStockconsumptionreport(MdlImsTrnStockConsumptionReport values)
        {
            try
            {
                msSQL = " SELECT case when sum(a.qty_issued) is null then 0.00 when sum(a.qty_issued) is not null then sum(a.qty_issued) end as quantity, " +
                " a.reference_gid as grn_gid, format(i.unit_price, 2) as product_price, a.product_gid," +
                " format((i.unit_price * a.qty_issued),2) as total_price,h.branch_name, " +
                " a.productuom_gid, c.product_code, c.product_name, d.productgroup_name, g.productuom_name,a.branch_gid,i.display_field " +
                " FROM ims_trn_tstocktracker a " +
                " left join ims_trn_tstock i on a.stock_gid=i.stock_gid " +
                " left join pmr_mst_tproduct c on a.product_gid = c.product_gid " +
                " left join pmr_mst_tproductgroup d on d.productgroup_gid = c.productgroup_gid " +
                " left join pmr_mst_tproductuom g on a.productuom_gid = g.productuom_gid " +
                " left join hrm_mst_tbranch h on a.branch_gid = h.branch_gid " +
                " where 0=0  group by  a.product_gid  order by d.productgroup_name asc " ;
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockconsumptionreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockconsumptionreport_list
                        {
                            quantity = dt["quantity"].ToString(),
                            grn_gid = dt["grn_gid"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            total_price = dt["total_price"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.stockconsumptionreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Stock Consumption Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}