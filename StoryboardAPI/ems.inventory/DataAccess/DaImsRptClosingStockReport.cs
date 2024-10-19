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
    public class DaImsRptClosingStockReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        public void DaGetLocation(MdlImsRptClosingStock values)
        {
            try
            {
                msSQL = " select location_gid,location_name,location_code from  ims_mst_tlocation " ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<location>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new location
                        {
                            location_name = dt["location_name"].ToString(),
                            location_gid = dt["location_gid"].ToString(),

                        });
                        values.location = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location Dropdown !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetClosingStockreport(MdlImsRptClosingStock values)
        {
            try
            {

                msSQL = " select a.stock_gid,a.product_gid,a.uom_gid,a.branch_gid,c.product_name,b.branch_name,d.productuom_name,sum(a.stock_qty) as Total_Stock_Quantity, " +
                      " sum(a.issued_qty) as Issued_Quantity,sum(a.amend_qty) as Amended_Quantity," +
                      " sum(a.damaged_qty) as damaged_qty,sum(a.adjusted_qty) as adjusted_qty, " +
                      " ifnull(sum(a.stock_qty)+sum(a.amend_qty)-sum(a.damaged_qty)-sum(a.issued_qty)-sum(a.transfer_qty),0) as available_quantity, " +
                      " sum(a.transfer_qty) as transfer_qty,e.location_name from ims_trn_tstock a " +
                      " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                      " left join pmr_mst_tproduct c on a.product_gid=c.product_gid " +
                      " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid " +
                      " left join ims_mst_tlocation e on a.location_gid=e.location_gid" +
                      " where 1=1 and a.product_gid<>'' group by a.product_gid,a.uom_gid,a.branch_gid order by a.created_date desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<closingstock_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new closingstock_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            total_Stock_Quantity = dt["Total_Stock_Quantity"].ToString(),
                            issued_Quantity = dt["Issued_Quantity"].ToString(),
                            amended_Quantity = dt["Amended_Quantity"].ToString(),
                            damaged_qty = dt["damaged_qty"].ToString(),
                            adjusted_qty = dt["adjusted_qty"].ToString(),
                            available_quantity = dt["available_quantity"].ToString(),
                            transfer_qty = dt["transfer_qty"].ToString(),
                            location_name = dt["location_name"].ToString(),
                        });
                        values.closingstock_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetClosingStocklocation(string location_gid, MdlImsRptClosingStock values)
        {
            try
            {
                msSQL = " select a.stock_gid,a.product_gid,a.uom_gid,a.branch_gid,c.product_name,b.branch_name,d.productuom_name,sum(a.stock_qty) as Total_Stock_Quantity, " +
                      " sum(a.issued_qty) as Issued_Quantity,sum(a.amend_qty) as Amended_Quantity," +
                      " sum(a.damaged_qty) as damaged_qty,sum(a.adjusted_qty) as adjusted_qty, " +
                      " ifnull(sum(a.stock_qty)+sum(a.amend_qty)-sum(a.damaged_qty)-sum(a.issued_qty)-sum(a.transfer_qty),0) as available_quantity, " +
                      " sum(a.transfer_qty) as transfer_qty,e.location_name from ims_trn_tstock a " +
                      " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                      " left join pmr_mst_tproduct c on a.product_gid=c.product_gid " +
                      " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid " +
                      " left join ims_mst_tlocation e on a.location_gid=e.location_gid" +
                      " where 1=1 and a.product_gid<>'' and a.location_gid='" + location_gid + "' group by a.product_gid,a.uom_gid,a.branch_gid order by a.created_date desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<closingstock_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new closingstock_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            total_Stock_Quantity = dt["Total_Stock_Quantity"].ToString(),
                            issued_Quantity = dt["Issued_Quantity"].ToString(),
                            amended_Quantity = dt["Amended_Quantity"].ToString(),
                            damaged_qty = dt["damaged_qty"].ToString(),
                            adjusted_qty = dt["adjusted_qty"].ToString(),
                            available_quantity = dt["available_quantity"].ToString(),
                            transfer_qty = dt["transfer_qty"].ToString(),
                            location_name = dt["location_name"].ToString(),
                        });
                        values.closingstock_list = getModuleList;
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