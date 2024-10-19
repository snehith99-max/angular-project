using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace ems.inventory.DataAccess
{
    public class DaImsRptStockStatusReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string lsstart_date, lsend_date;
        public void DaGetImsRptStockStatusreport(MdlImsRptStockStatusReport values)
        {
            try
            {

                        msSQL = " select a.product_gid,e.product_name,a.reference_gid,date_format(b.grn_date,'%d-%m-%Y') as grn_date,d.branch_name, " +
                                " c.vendor_companyname,a.stock_qty,f.productuom_name from ims_trn_tstock a " +
                                " left join pmr_trn_tgrn b on b.grn_gid = a.reference_gid " +
                                " left join acp_mst_tvendor c on c.vendor_gid = b.vendor_gid " +
                                " left join hrm_mst_tbranch d on d.branch_gid = a.branch_gid " +
                                " left join pmr_mst_tproduct e on e.product_gid = a.product_gid " +
                                " left join pmr_mst_tproductuom f on f.productuom_gid=a.uom_gid " +
                                " where a.stocktype_gid = 'SY0905270002' and a.stock_flag = 'Y' ";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<stocklist>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new stocklist
                                {
                                    product_gid = dt["product_gid"].ToString(),
                                    product_name = dt["product_name"].ToString(),
                                    reference_gid = dt["reference_gid"].ToString(),
                                    grn_date = dt["grn_date"].ToString(),
                                    branch_name = dt["branch_name"].ToString(),
                                    vendor_companyname = dt["vendor_companyname"].ToString(),
                                    stock_qty = dt["stock_qty"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                });
                                values.stocklist = getModuleList;
                            }
                        }
                        dt_datatable.Dispose();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                public void DaGetStockStatusdate(string from_date, string to_date, MdlImsRptStockStatusReport values)
                {
                    try
                    {
                        if ((from_date == null) || (to_date == null))
                        {
                            lsstart_date = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

                            lsend_date = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            //-- from date
                            DateTime from_date1 = DateTime.ParseExact(from_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            lsstart_date = from_date1.ToString("yyyy-MM-dd");

                            //-- to date
                            DateTime lsDateto = DateTime.ParseExact(to_date.ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            lsend_date = lsDateto.ToString("yyyy-MM-dd");
                        }
                        msSQL = " select a.product_gid,e.product_name,a.reference_gid,date_format(b.grn_date,'%d-%m-%Y') as grn_date,d.branch_name, " +
                                 " c.vendor_companyname,a.stock_qty,f.productuom_name from ims_trn_tstock a " +
                                 " left join pmr_trn_tgrn b on b.grn_gid = a.reference_gid " +
                                 " left join acp_mst_tvendor c on c.vendor_gid = b.vendor_gid " +
                                 " left join hrm_mst_tbranch d on d.branch_gid = a.branch_gid " +
                                 " left join pmr_mst_tproduct e on e.product_gid = a.product_gid " +
                                 " left join pmr_mst_tproductuom f on f.productuom_gid=a.uom_gid " +
                                 " where a.stocktype_gid = 'SY0905270002' and a.stock_flag = 'Y' " +
                                 " and b.grn_date >= '" + lsstart_date + "' and  b.grn_date <= '" + lsend_date + "'";
                                

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<stocklist>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new stocklist
                                {
                                    product_gid = dt["product_gid"].ToString(),
                                    product_name = dt["product_name"].ToString(),
                                    reference_gid = dt["reference_gid"].ToString(),
                                    grn_date = dt["grn_date"].ToString(),
                                    branch_name = dt["branch_name"].ToString(),
                                    vendor_companyname = dt["vendor_companyname"].ToString(),
                                    stock_qty = dt["stock_qty"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                });
                                values.stocklist = getModuleList;
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