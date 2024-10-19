using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using System.Windows.Media.Media3D;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using CrystalDecisions.Shared.Json;
using System.Web.UI.WebControls;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnStockRegularization
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsadjustedqty, lsbranch_gid, lsproduct_gid, lsuom_gid, msGetStockGID;
        int i, mnResult;

        public void DaGetStockRegularizationSummary(MdlImsTrnStockRegularization values)
        {
            try
            {

                msSQL = " SELECT d.stock_gid,d.branch_gid,d.product_gid,d.uom_gid, a.product_code, a.product_name,  b.productgroup_name, c.productuom_name," +
                        " d.adjusted_qty as adjustment_qty,g.branch_name,g.branch_prefix," +
                        " e.producttype_name as product_type " +
                        " FROM ims_trn_tstock d " +
                        " left join pmr_mst_tproduct a on d.product_gid = a.product_gid" +
                        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                        " left join pmr_mst_tproductuom c on d.uom_gid = c.productuom_gid" +
                        " left join pmr_mst_tproducttype e on a.producttype_gid = e.producttype_gid" +
                        " left join ims_mst_tstocktype f on d.stocktype_gid = f.stocktype_gid" +
                        " left join hrm_mst_tbranch g on d.branch_gid = g.branch_gid " +
                        " WHERE d.adjusted_qty<>'0' Order by  product_name asc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockdetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockdetails_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            adjustment_qty = dt["adjustment_qty"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            product_type = dt["product_type"].ToString(),
                        });
                        values.stockdetails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while error!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostStockRegularization( string user_gid, merge_list values)
        {
            foreach (var data1 in values.ContractSummary_List)
            {
                i++;
                lsadjustedqty += Convert.ToDouble(data1.adjustment_qty);


                if (i != 0)
                {
                    lsbranch_gid = data1.branch_gid;
                    lsproduct_gid = data1.product_gid;
                    lsuom_gid = data1.uom_gid;
                }
                else
                {
                    string lsbranchnxt_gid = data1.branch_gid;
                    string lsproductnxt_gid = data1.product_gid;
                    string lsuomnxt_gid = data1.uom_gid;
                    if (lsbranch_gid != lsbranchnxt_gid)
                    {
                        values.message = "Please merge products in the same branch";
                        return;
                    }

                    if (lsproduct_gid != lsproductnxt_gid)
                    {
                        values.message = "Please merge the same products";
                        return;
                    }

                    if (lsuom_gid != lsuomnxt_gid)
                    {
                        values.message = "Please merge products with the same UOM";
                        return;
                    }
                }

                msSQL = "UPDATE ims_trn_tstock SET adjusted_qty='0.00' WHERE stock_gid='" + data1.stock_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (i!= 0 ) 
                {
                    
                    msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                    msSQL = " Insert into ims_trn_tstock (" +
                    " stock_gid," +
                    " branch_gid, " +
                    " product_gid," +
                    " uom_gid," +
                    " stock_qty," +
                    " created_by," +
                    " created_date," +
                    " stocktype_gid) " +
                    " values ( " +
                    " '" + msGetStockGID + "', " +
                    " '" + lsbranch_gid + "'," +
                    " '" + lsproduct_gid + "', " +
                    " '" + lsuom_gid + "', " +
                    " '" + lsadjustedqty + "'," +
                    " '" + user_gid + "'," +
                     " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                    " 'SY0905270008')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Some error occurred ";
                    }

                }
                lsadjustedqty = "";
            }

            if (mnResult == 1)
            {
                    values.status = true;
                    values.message = "Stock Regularization Mergd Successfully";
            }
            else
            {
                    values.status = false;
                    values.message = "Some error occurred while Stock Regularization Merege";
            }


        }
    }
}