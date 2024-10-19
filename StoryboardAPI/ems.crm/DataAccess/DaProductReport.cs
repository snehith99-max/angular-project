﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;



namespace ems.crm.DataAccess
{
    public class DaProductReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        string lsbranch_gid;
        public void DaGetProductConsumptionReport(string employee_gid, MdlProductReport values)
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
        public void DaProductReportgrid(string product_gid, MdlProductReport values)
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
        public void DaProductGroupwiseChart(MdlProductReport values)
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
        public void DaProductGroupwiseChartSearch(MdlProductReport values, string from_date, string to_date)
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
        public void DaGetProductdropdown(MdlProductReport values)
        {
            try
            {
                 
                msSQL = "select product_gid,product_name from pmr_mst_tproduct";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<productdropdown_list>();

                if (dt_datatable.Rows.Count != 0)
                {


                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productdropdown_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString()

                        });
                        values.productdropdown_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetReportProductSummary(MdlProductReport values,string product_name)
        {
            try
            {
                 
                msSQL = " select distinct date_format(a.purchaseorder_date,'%d-%m-%Y')as purchaseorder_date,d.vendor_companyname, " +
             " c.product_name,e.productuom_name,b.product_price from pmr_trn_tpurchaseorder a " +
             " left join  pmr_trn_tpurchaseorderdtl b on a.purchaseorder_gid=b.purchaseorder_gid" +
             " left join pmr_mst_tproduct c on b.product_gid=c.product_gid " +
             " left join pmr_mst_tproductuom e on e.productuom_gid=b.uom_gid " +
             " left join acp_mst_tvendor d on a.vendor_gid=d.vendor_gid " +
             "  where b.product_gid='" + product_name + "' order by purchaseorder_date desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<productpurchaseorder_list>();

                if (dt_datatable.Rows.Count != 0)
                {


                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productpurchaseorder_list
                        {
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),

                        });
                        values.productpurchaseorder_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary report";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }

        public void DaGetReportSalesOrderSummary(MdlProductReport values, string product_name)
        {
            try
            {
                 
                msSQL = " select date_format(a.salesorder_date,'%d-%m-%Y')as salesorder_date,d.customer_name,c.product_name," +
               "e.productuom_name,b.product_price From smr_trn_tsalesorder a " +
               " left join smr_trn_tsalesorderdtl b on a.salesorder_gid=b.salesorder_gid " +
               " left join pmr_mst_tproduct c on b.product_gid=c.product_gid " +
               " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
               " left join pmr_mst_tproductuom e on e.productuom_gid=b.uom_gid " +
               " where b.product_gid='" + product_name + "'order by salesorder_date desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<productSaleseorder_list>();

                if (dt_datatable.Rows.Count != 0)
                {


                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productSaleseorder_list
                        {
                            salesorder_date = dt["salesorder_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),

                        });
                        values.productSaleseorder_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting salea order summary report";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
    }
}