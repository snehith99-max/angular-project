using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnStockAgeReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        public void DaGetImsRptStockagereport30(MdlImsRptStockAgeReport values)
        {
            try
            {
                msSQL = " select a.stock_gid,date_format(a.created_date, '%d-%m-%Y') as created_date,a.display_field,f.branch_name,c.productgroup_name,b.product_code,f.branch_prefix, " +
                " b.product_name,e.productuom_name,d.stocktype_name," +
                " sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)as stock_quantity,format(a.unit_price,2)as product_price,datediff(now(),a.created_date)as totaldays, " +
                " h.location_name from ims_trn_tstock a" +
                " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid" +
                " left join pmr_mst_tproductuom e on e.productuom_gid=b.productuom_gid" +
                " left join ims_mst_tstocktype d on d.stocktype_gid=a.stocktype_gid" +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " left join ims_mst_tlocation h on h.location_gid=a.location_gid" +
                " where  a.created_date > date_add(now(), interval - 30 day) and a.created_date <= now() " +
                " group by a.product_gid,a.display_field,b.productuom_gid,a.location_gid,a.branch_gid order by date(a.created_date) desc,a.created_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockage_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stocktype_name = dt["stocktype_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            totaldays = dt["totaldays"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockage_list = getModuleList;
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

        public void DaGetImsRptStockagereport60(MdlImsRptStockAgeReport values)
        {
            try
            {
                msSQL = " select a.stock_gid,date_format(a.created_date, '%d-%m-%Y') as created_date,a.display_field,f.branch_name,c.productgroup_name,b.product_code,f.branch_prefix, " +
                " b.product_name,e.productuom_name,d.stocktype_name," +
                " sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)as stock_quantity,format(a.unit_price,2)as product_price,datediff(now(),a.created_date)as totaldays, " +
                " h.location_name from ims_trn_tstock a" +
                " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid" +
                " left join pmr_mst_tproductuom e on e.productuom_gid=b.productuom_gid" +
                " left join ims_mst_tstocktype d on d.stocktype_gid=a.stocktype_gid" +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " left join ims_mst_tlocation h on h.location_gid=a.location_gid" +
                " where  datediff(current_date,date(a.created_date)) BETWEEN 31 AND 60 " +
                " group by a.product_gid,a.display_field,b.productuom_gid,a.location_gid,a.branch_gid order by date(a.created_date) desc,a.created_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockage_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stocktype_name = dt["stocktype_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            totaldays = dt["totaldays"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockage_list = getModuleList;
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

        public void DaGetImsRptStockagereport90(MdlImsRptStockAgeReport values)
        {
            try
            {
                msSQL = " select a.stock_gid,date_format(a.created_date, '%d-%m-%Y') as created_date,a.display_field,f.branch_name,c.productgroup_name,b.product_code,f.branch_prefix, " +
                " b.product_name,e.productuom_name,d.stocktype_name," +
                " sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)as stock_quantity,format(a.unit_price,2)as product_price,datediff(now(),a.created_date)as totaldays, " +
                " h.location_name from ims_trn_tstock a" +
                " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid" +
                " left join pmr_mst_tproductuom e on e.productuom_gid=b.productuom_gid" +
                " left join ims_mst_tstocktype d on d.stocktype_gid=a.stocktype_gid" +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " left join ims_mst_tlocation h on h.location_gid=a.location_gid" +
                " where datediff(current_date,date(a.created_date)) BETWEEN 61 AND 90 " +
                " group by a.product_gid,a.display_field,b.productuom_gid,a.location_gid,a.branch_gid order by date(a.created_date) desc,a.created_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockage_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stocktype_name = dt["stocktype_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            totaldays = dt["totaldays"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockage_list = getModuleList;
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

        public void DaGetImsRptStockagereport120(MdlImsRptStockAgeReport values)
        {
            try
            {
                msSQL = " select a.stock_gid,date_format(a.created_date, '%d-%m-%Y') as created_date,a.display_field,f.branch_name,c.productgroup_name,b.product_code,f.branch_prefix, " +
                " b.product_name,e.productuom_name,d.stocktype_name," +
                " sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)as stock_quantity,format(a.unit_price,2)as product_price,datediff(now(),a.created_date)as totaldays, " +
                " h.location_name from ims_trn_tstock a" +
                " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid" +
                " left join pmr_mst_tproductuom e on e.productuom_gid=b.productuom_gid" +
                " left join ims_mst_tstocktype d on d.stocktype_gid=a.stocktype_gid" +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " left join ims_mst_tlocation h on h.location_gid=a.location_gid" +
                " where  datediff(current_date,date(a.created_date)) > 120 " +
                " group by a.product_gid,a.display_field,b.productuom_gid,a.location_gid,a.branch_gid order by date(a.created_date) desc,a.created_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockage_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stocktype_name = dt["stocktype_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            totaldays = dt["totaldays"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockage_list = getModuleList;
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
        public void DaGetImsRptStockagereport(MdlImsRptStockAgeReport values)
        {
            try
            {
                msSQL = " select a.stock_gid,date_format(a.created_date, '%d-%m-%Y') as created_date,a.display_field,f.branch_name,c.productgroup_name,b.product_code,f.branch_prefix, " +
                " b.product_name,e.productuom_name,d.stocktype_name," +
                " sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)as stock_quantity,format(a.unit_price,2)as product_price,datediff(now(),a.created_date)as totaldays, " +
                " h.location_name from ims_trn_tstock a" +
                " left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid" +
                " left join pmr_mst_tproductuom e on e.productuom_gid=b.productuom_gid" +
                " left join ims_mst_tstocktype d on d.stocktype_gid=a.stocktype_gid" +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " left join ims_mst_tlocation h on h.location_gid=a.location_gid" +
                " where 0=0 group by a.product_gid,a.display_field,b.productuom_gid,a.location_gid,a.branch_gid order by date(a.created_date) desc,a.created_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockage_list
                        {
                            stock_gid = dt["stock_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            stocktype_name = dt["stocktype_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            totaldays = dt["totaldays"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockage_list = getModuleList;
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