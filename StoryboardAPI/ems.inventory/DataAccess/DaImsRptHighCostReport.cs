using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;

namespace ems.inventory.DataAccess
{
    public class DaImsRptHighCostReport
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsstart_date, lsend_date;

        public void DaGetHighcostreport(MdlImsRptHighcost values)
        {
            try
            {
                msSQL = " select branch_name,location_name,display_field,productgroup_name,product_name,product_code,productuom_name,remarks,stock_qty,format(max_price,2) as max_price,format(min_price,2) as min_price, "+
                         " if (format(((max_price - min_price) / max_price) * 100, 1) like '%.0' ,format(((max_price - min_price) / max_price) * 100, 0), "+
                         " format(((max_price - min_price) / max_price) * 100, 1)) as price_variance from(select b.branch_name, f.location_name, " +
                         " a.display_field, d.productgroup_name, c.product_name, c.product_code, e.productuom_name, a.remarks, " +
                         " sum(a.stock_qty) as stock_qty, max(a.unit_price) as max_price, min(a.unit_price) as min_price from ims_trn_tstock a " +
                         " left join hrm_mst_tbranch b on b.branch_gid = a.branch_gid " +
                         " left join pmr_mst_tproduct c on c.product_gid = a.product_gid " +
                         " left join pmr_mst_tproductgroup d on d.productgroup_gid = c.productgroup_gid " +
                         " left join pmr_mst_tproductuom e on e.productuom_gid = a.uom_gid " +
                         " left join ims_mst_tlocation f on f.location_gid = a.location_gid " +
                         " where a.stock_flag = 'Y' and a.unit_price<> '0.00' group by a.product_gid," +
                         " a.display_field, a.uom_gid, a.location_gid, a.branch_gid)x where(max_price-min_price)/ max_price * 100 <> 0 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<highcost_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new highcost_list
                        {
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            max_price = dt["max_price"].ToString(),
                            min_price = dt["min_price"].ToString(),
                            price_variance = dt["price_variance"].ToString()
                        });
                        values.highcost_list = getModuleList;
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