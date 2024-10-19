using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Remoting;
using System.Drawing;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
namespace ems.sales.DataAccess
{
    public class Daproductreports
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL,msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        DataTable dt_datatable1;
        string msGetGid;
        string lsbranch_gid;
        int mnResult, mnResult1;

        public void daproductdropdown(string user_gid, Mdlproductreports values)
        {
            msSQL = "select product_gid,product_name from pmr_mst_tproduct";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var productreport_list = new List<productreport_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    productreport_list.Add(new productreport_list
                    {

                        product_gid = (dt["product_gid"].ToString()),
                        product_name = (dt["product_name"].ToString()),
                    });
                    values.productreport_list = productreport_list;
                }

            }

        }
        public void dagetproductsellingforsixmonths(string user_gid, Mdlproductreports values)
        {
            try
            {
                msSQL = "SELECT YEAR(a.invoice_date) AS year,SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'), 1, 3) AS months, " +
                       " COUNT(b.qty_invoice) AS productcount " +
                       " FROM rbl_trn_tinvoice a LEFT JOIN rbl_trn_tinvoicedtl b ON b.invoice_gid = a.invoice_gid " +
                       " WHERE a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) AND a.invoice_date <= DATE(NOW()) " +
                       " AND a.invoice_status NOT IN('Invoice Cancelled', 'Rejected') " +
                       " AND a.invoice_flag NOT IN('Invoice Cancelled', 'Rejected') " +
                       " GROUP BY YEAR(a.invoice_date), SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'), 1, 3) " +
                       " ORDER BY a.invoice_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetproductssellingForLastSixMonths_List = new List<GetproductssellingForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetproductssellingForLastSixMonths_List.Add(new GetproductssellingForLastSixMonths_List
                        {

                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            productcounts = (dt["productcount"].ToString()),

                        });
                        values.GetproductssellingForLastSixMonths_List = GetproductssellingForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void dagetproductsellingforsixmonthssearch( string product, Mdlproductreports values)
        {
            try
            {
                if (product != "undefined")
                {
                    msSQL = "SELECT YEAR(a.invoice_date) AS year, SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'), 1, 3) AS months," +
                       " sum(b.qty_invoice) as productcount FROM rbl_trn_tinvoice a " +
                       " LEFT JOIN rbl_trn_tinvoicedtl b ON b.invoice_gid = a.invoice_gid " +
                       " WHERE a.invoice_date > DATE_ADD(Now(), INTERVAL - 6 MONTH) AND a.invoice_date <= DATE(now()) and b.product_gid = '"+product+"' " +
                       " AND a.invoice_status NOT IN('Invoice Cancelled', 'Rejected')  AND a.invoice_flag NOT IN('Invoice Cancelled', 'Rejected')  " +
                       " GROUP BY YEAR(a.invoice_date), SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'), 1, 3) " +
                       " ,b.product_gid ORDER BY " +
                      " YEAR(a.invoice_date) DESC, " +
                      " MONTH(a.invoice_date) DESC";

                    msSQL1 = "select b.productgroup_name , a.customerproduct_code, a.product_name,a.mrp_price " +
                           " from pmr_mst_tproduct a left join pmr_mst_tproductgroup " +
                            " b on a.productgroup_gid = b.productgroup_gid where a.product_gid = '"+product+"'";
                }
                else
                {
                    msSQL = "SELECT YEAR(a.invoice_date) AS year,SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'), 1, 3) AS months, " +
                       " COUNT(b.qty_invoice) AS productcount " +
                       " FROM rbl_trn_tinvoice a LEFT JOIN rbl_trn_tinvoicedtl b ON b.invoice_gid = a.invoice_gid " +
                       " WHERE a.invoice_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH) AND a.invoice_date <= DATE(NOW()) " +
                       " AND a.invoice_status NOT IN('Invoice Cancelled', 'Rejected') " +
                       " AND a.invoice_flag NOT IN('Invoice Cancelled', 'Rejected') " +
                       " GROUP BY YEAR(a.invoice_date), SUBSTRING(DATE_FORMAT(a.invoice_date, '%M'), 1, 3) " +
                       " ORDER BY a.invoice_date DESC ";
                }
               
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetproductssellingForLastSixMonths_List = new List<GetproductssellingForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetproductssellingForLastSixMonths_List.Add(new GetproductssellingForLastSixMonths_List
                        {

                            months = (dt["months"].ToString()),
                            year = (dt["year"].ToString()),
                            productcounts = (dt["productcount"].ToString()),

                        });
                        values.GetproductssellingForLastSixMonths_List = GetproductssellingForLastSixMonths_List;
                    }

                }
                dt_datatable = objdbconn.GetDataTable(msSQL1);
                var GetproudctssellingDetailSummarylist = new List<GetproudctssellingDetailSummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetproudctssellingDetailSummarylist.Add(new GetproudctssellingDetailSummarylist
                        {

                            productgroup_name = (dt["productgroup_name"].ToString()),
                            customerproduct_code = (dt["customerproduct_code"].ToString()),
                            product_name = (dt["product_name"].ToString()),
                            mrp_price = (dt["mrp_price"].ToString()),

                        });
                        values.GetproudctssellingDetailSummarylist = GetproudctssellingDetailSummarylist;
                    }

                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaProductGroupwiseChart(Mdlproductreports values)
        {
            try
            {

                msSQL = "select SUM(a.Grandtotal) as amount,c.productgroup_name from smr_trn_tsalesorder a" +
                   " left join smr_trn_tsalesorderdtl b on a.salesorder_gid=b.salesorder_gid " +
                   " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid " +
                   " where a.salesorder_date > date_add(now(), interval-5 month) and a.salesorder_date<=date(now()) " +
                   " and  a.salesorder_status not in('SO Amended','Canceled','Rejected') GROUP BY c.productgroup_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var ProductGroupwiseChart_list = new List<ProductGroupwiseChart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        ProductGroupwiseChart_list.Add(new ProductGroupwiseChart_list
                        {
                            amount = dt["amount"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),

                        });
                        values.ProductGroupwiseChart_list = ProductGroupwiseChart_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaProductGroupwiseChartSearch(Mdlproductreports values, string from_date, string to_date)
        {
            try
            {

                if (from_date == null && to_date == null)
                {
                    msSQL = "select SUM(a.Grandtotal) as amount,c.productgroup_name from smr_trn_tsalesorder a" +
                                " left join smr_trn_tsalesorderdtl b on a.salesorder_gid=b.salesorder_gid " +
                                " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid " +
                                " where a.salesorder_date > date_add(now(), interval-5 month) and a.salesorder_date<=date(now()) " +
                                " and  a.salesorder_status not in('SO Amended','Canceled','Rejected') GROUP BY c.productgroup_gid";
                }
                else
                {
                    msSQL = " select SUM(a.Grandtotal) as amount,c.productgroup_name from smr_trn_tsalesorder a " +
                            " left join smr_trn_tsalesorderdtl b on a.salesorder_gid = b.salesorder_gid " +
                            " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid" +
                            " where a.salesorder_date > date('" + from_date + "') and a.salesorder_date < date('" + to_date + "')" +
                            " and a.salesorder_status not in('SO Amended', 'Canceled', 'Rejected') GROUP BY c.productgroup_gid ";



                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<ProductGroupwiseChart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProductGroupwiseChart_list
                        {
                            amount = dt["amount"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.ProductGroupwiseChart_list = getModuleList;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetProductConsumptionReport(string employee_gid, Mdlproductreports values)
        {
            try
            {

                msSQL = "select branch_gid from hrm_mst_temployee where employee_gid='" + employee_gid + "'";

                lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select sum(a.product_qtydelivered-a.qty_returned) as product_quantity,a.product_gid,concat(a.product_gid,'$',a.product_uom_gid) as productuom, " +
                        " b.product_name,b.product_code,c.productgroup_name,d.productuom_name, " +
                        " (select ifnull(sum(m.stock_qty)+sum(m.amend_qty)-sum(m.damaged_qty)-sum(m.issued_qty)-sum(m.transfer_qty),0) as available_quantity " +
                        " from ims_trn_tstock m where m.stock_flag='Y' and m.product_gid=a.product_gid  and m.branch_gid='" + lsbranch_gid + "') as available_quantity " +
                        " from smr_trn_tdeliveryorderdtl a " +
                        " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid " +
                        " left join pmr_mst_tproductuom d on d.productuom_gid=a.product_uom_gid where 1=1 " +
                        " group by a.product_gid,a.product_uom_gid order by b.product_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductConsumptionReport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProductConsumptionReport_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_quantity = dt["product_quantity"].ToString(),
                            available_quantity = dt["available_quantity"].ToString()
                        });
                        values.ProductConsumptionReport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product consumption report";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaProductReportgrid(string product_gid, Mdlproductreports values)
        {
            try
            {

                msSQL = " select a.customer_name,SUM(c.product_qtydelivered - c.qty_returned) AS product_qtydelivered " +
                   " from crm_mst_tcustomer a " +
                   " left join smr_trn_tdeliveryorder b ON b.customer_gid = a.customer_gid " +
                   " left join smr_trn_tdeliveryorderdtl c ON c.directorder_gid = b.directorder_gid " +
                   " where c.product_gid = '" + product_gid + "'  group by a.customer_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductReportgrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ProductReportgrid_list
                        {
                            customer_name = dt["customer_name"].ToString(),
                            product_qtydelivered = dt["product_qtydelivered"].ToString(),

                        });
                        values.ProductReportgrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



    }

   

}