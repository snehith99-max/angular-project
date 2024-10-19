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
    public class DaImsTrnProductSplit
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string split_quantity, reference_gid, stock_balance, branch_gid, product_gid, productuom_name;
        public void DaGetProductSplit(String employee_gid, MdlImsTrnProductSplit values)
        {
            try
            {
                if (values.split_quantity > values.stock_balance)
                {
                    values.message = "Stock Balance exceed for the base product quantity";
                    return;
                }
                string msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");

                string msSQL = "insert into ims_trn_tstockdtl(" +
                               "stockdtl_gid," +
                               "stock_gid," +
                               "branch_gid," +
                               "product_gid," +
                               "uom_gid," +
                               "issued_qty," +
                               "amend_qty," +
                               "damaged_qty," +
                               "adjusted_qty," +
                               "transfer_qty," +
                               "return_qty," +
                               "reference_gid," +
                               "stock_type," +
                               "remarks," +
                               "created_by," +
                               "created_date," +
                               "display_field" +
                               ") values (" +
                               "'" + msstockdtlGid + "'," +
                               "'" + values.stock_gid + "'," +
                               "'" + values.branch_gid + "'," +
                               "'" + values.product_gid + "'," +
                               "'" + values.uom_gid + "'," +
                               "'" + values.split_quantity + "'," +
                               "'0.00'," +
                               "'0.00'," +
                               "'0.00'," +
                               "'0.00'," +
                               "'0.00'," +
                               "'" + values.reference_gid + "'," +
                               "'Split Product Issued'," +
                               "''," +
                               "'" + employee_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'')";
                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = "update ims_trn_tstock set " +
                            "issued_qty = issued_qty + '" + values.split_quantity + "' " +
                            "where stock_gid = '" + values.stock_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                string msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");

                msSQL = "insert into ims_trn_tstock (" +
                           "stock_gid, " +
                           "branch_gid, " +
                           "location_gid, " +
                           "bin_gid, " +
                           "product_gid, " +
                           "uom_gid, " +
                           "stock_qty, " +
                           "grn_qty, " +
                           "rejected_qty, " +
                           "stocktype_gid, " +
                           "reference_gid, " +
                           "display_field, " +
                           "stock_flag, " +
                           "remarks, " +
                           "created_by, " +
                           "created_date, " +
                           "issued_qty, " +
                           "amend_qty, " +
                           "damaged_qty" +
                           ") values (" +
                           "'" + msGetStockGID + "', " +
                           "'" + values.branch_gid + "', " +
                           "'" + "" + "', " +
                           "'" + "" + "', " +
                           "'" + values.product_gid + "', " +
                           "'" + values.uom_gid + "', " +
                           "'" + values.incoming_quantity + "', " +
                           "'" + values.incoming_quantity + "', " +
                           "'0', " +
                           "'SY0905270002', " +
                           "'" + values.reference_gid + "', " +
                           "'" + values.display_field + "', " +
                           "'Y', " +
                           "'From Purchase', " +
                           "'" + employee_gid + "', " +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                           "'0', " +
                           "'0', " +
                           "'0'" +
                           ")";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                objdbconn.CloseConn();
                if (mnResult == 1)
                {

                    values.status = true;
                    values.message = "Product Split Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while deleting the details";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating prd[oduct split !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
            


        }

        public void DaGetOnChangeUnit( MdlImsTrnProductSplit values)
        {
            try
            {


                msSQL = "  select productuom_name,productuom_gid from pmr_mst_tproductuom a" +
                    " left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid   order by a.sequence_level";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLocationstocks>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLocationstocks
                        {
                            productuom_name = dt["productuom_name"].ToString(),
                            product_uom_gid = dt["productuom_gid"].ToString(),

                        });
                        values.GetLocation = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
    }
}