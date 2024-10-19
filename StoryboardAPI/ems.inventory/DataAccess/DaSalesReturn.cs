using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ems.inventory.DataAccess
{
    public class DaSalesReturn
    {
        string msSQL, msgetdtlGID, msgetGID, msGetStockGID = string.Empty;
        DataSet ds_dataset;
        DataTable dt_datatable;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        int mnResult;
        OdbcDataReader objodbcDatareader;
        public void DaGetSalesReturnSummary(MdlSalesReturn values)
        {
            try
            {
                msSQL = " select distinct a.salesreturn_gid,date_format(a.salesreturn_date, '%d-%m-%Y') as salesreturn_date ," +
                " a.deliveryorder_gid, c.customer_name, b.delivery_status,d.branch_prefix," +
                " concat(b.customer_contactperson,' / ',b.customer_contactnumber,' / ',b.customer_emailid)  as contact " +
                " from smr_trn_tsalesreturn a " +
                " left join smr_trn_tdeliveryorder b on a.deliveryorder_gid = b.directorder_gid  " +
                " left join crm_mst_tcustomer c on b.customer_gid = c.customer_gid " +
                "  left join hrm_mst_tbranch d on d.branch_gid = b.customerbranch_gid " +
                " where a.deliveryorder_gid <> ''" +
                " order by date(a.salesreturn_date) desc,a.salesreturn_date asc,a.salesreturn_gid desc";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tsalesreturn");
                var GetSalesReturn = new List<GetSalesReturn_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetSalesReturn.Add(new GetSalesReturn_list
                        {
                            salesreturn_gid = ds["salesreturn_gid"].ToString(),
                            salesreturn_date = ds["salesreturn_date"].ToString(),
                            deliveryorder_gid = ds["deliveryorder_gid"].ToString(),
                            customer_name = ds["customer_name"].ToString(),
                            delivery_status = ds["delivery_status"].ToString(),
                            contact = ds["contact"].ToString(),
                            branch_prefix = ds["branch_prefix"].ToString(),
                        });
                        values.GetSalesReturn_list = GetSalesReturn;
                    }
                }
            }
            catch (Exception ex) 
            {

            }
        }
        public void DaGetSalesReturnViewSummary(string salesreturn_gid, MdlSalesReturn values)
        {
            try
            {
                msSQL = " SELECT date_format(b.directorder_date,'%d-%m-%Y') as deliveryorder_date,b.directorder_gid, " +
                        " a.salesreturn_gid,c.customer_name, date_format(a.salesreturn_date,'%d-%m-%Y') as salesreturn_date, " +
                        " b.customer_contactperson,d.mobile,b.customer_address, " +
                        " case when b.customer_contactnumber is null then d.mobile when b.customer_contactnumber is not null then b.customer_contactnumber end as mobile,a.return_type,a.remarks " +
                        " FROM smr_trn_tsalesreturn a " +
                        " left join smr_trn_tdeliveryorder b on a.deliveryorder_gid = b.directorder_gid " +
                        " left join crm_mst_tcustomer c on b.customer_gid = c.customer_gid" +
                        " left join crm_mst_tcustomercontact d on d.customer_gid=c.customer_gid " +
                        " where a.salesreturn_gid = '" + salesreturn_gid + "' order by a.salesreturn_gid asc ";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tsalesreturn");
                var GetSalesReturnView = new List<GetSalesReturnView_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetSalesReturnView.Add(new GetSalesReturnView_list
                        {
                            customer_name = ds["customer_name"].ToString(),
                            directorder_gid = ds["directorder_gid"].ToString(),
                            deliveryorder_date = ds["deliveryorder_date"].ToString(),
                            salesreturn_gid = ds["salesreturn_gid"].ToString(),
                            salesreturn_date = ds["salesreturn_date"].ToString(),
                            mobile = ds["mobile"].ToString(),
                            customer_address = ds["customer_address"].ToString(),
                            customer_contactperson = ds["customer_contactperson"].ToString(),
                            return_type = ds["return_type"].ToString(),
                            remarks = ds["remarks"].ToString(),
                        });
                        values.GetSalesReturnView_list = GetSalesReturnView;
                    }
                }
            }
            catch (Exception ex) 
            { 
            
            }
        }
        public void DaGetSalesReturnViewDetails(string salesreturn_gid, string directorder_gid, MdlSalesReturn values)
        {
            try
            {
                msSQL = " select  a.salesreturndtl_gid ,a.salesreturn_gid , b.product_gid ,  b.product_name,a.qty_returned,d.product_qtydelivered," +
                        " c.productuom_name, b.product_code,b.product_desc,g.productgroup_name from smr_trn_tsalesreturndtl a " +
                        " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                        " left join pmr_mst_tproductuom c on c.productuom_gid = a.uom_gid  " +
                        " left join smr_trn_tdeliveryorderdtl d on d.product_gid = a.product_gid "+
                        " left join pmr_mst_tproductgroup g on g.productgroup_gid = b.productgroup_gid " +
                        " where a.salesreturn_gid = '"+ salesreturn_gid + "' and " +
                        " d.directorder_gid = '"+ directorder_gid + "' order by a.salesreturndtl_gid asc";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tsalesreturndtl");
                var GetSalesReturnViewDetails = new List<GetSalesReturnViewDetails_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetSalesReturnViewDetails.Add(new GetSalesReturnViewDetails_list 
                        {
                            salesreturndtl_gid = ds["salesreturndtl_gid"].ToString(),
                            salesreturn_gid = ds["salesreturn_gid"].ToString(),
                            product_gid = ds["product_gid"].ToString(),
                            product_name = ds["product_name"].ToString(),
                            qty_returned = ds["qty_returned"].ToString(),
                            product_qtydelivered = ds["product_qtydelivered"].ToString(),
                            productuom_name = ds["productuom_name"].ToString(),
                            product_code = ds["product_code"].ToString(),
                            product_desc = ds["product_desc"].ToString(),
                            productgroup_name = ds["productgroup_name"].ToString(),
                        });
                        values.GetSalesReturnViewDetails_list = GetSalesReturnViewDetails;
                    }
                }
            }
            catch (Exception ex) 
            {

            }
        }
        public void DaGetSalesReturnAddSummary(MdlSalesReturn values)
        {
            try
            {
                msSQL = " SELECT date_format(a.directorder_date,'%d-%m-%Y') as directorder_date, a.directorder_gid, c.salesorder_gid,a.delivery_status,a.customer_name, " +
                " case when a.customer_contactnumber is null then  concat(d.customercontact_name,'/',d.mobile,'/',d.email) " +
                " when a.customer_contactnumber is not null then concat(a.customer_contactperson,'/',a.customer_contactnumber,'/',a.customer_emailid) end as contact " +
                " FROM smr_trn_tdeliveryorder a " +
                " left join smr_trn_tsalesorder c on c.salesorder_gid= a.salesorder_gid " +
                " left join crm_mst_tcustomercontact d on d.customer_gid=a.customer_gid " +
                 " where  a.delivery_status='Delivery Completed' and a.invoice_status in ('Invoice Raised','Invoice Pending') order by directorder_date desc";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tdeliveryorder");
                var GetSalesReturnAdd = new List<GetSalesReturnAdd_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetSalesReturnAdd.Add(new GetSalesReturnAdd_list
                        {
                            directorder_date = ds["directorder_date"].ToString(),
                            directorder_gid = ds["directorder_gid"].ToString(),
                            customer_name = ds["customer_name"].ToString(),
                            contact = ds["contact"].ToString(),
                            delivery_status = ds["delivery_status"].ToString(),
                        });
                        values.GetSalesReturnAdd_list = GetSalesReturnAdd;
                    }
                }
            }
            catch (Exception ex) 
            {

            }
        }
        public void DaGetSalesReturnAddselect(string directorder_gid, MdlSalesReturn values)
        {
            try
            {
                msSQL = " SELECT date_format(a.directorder_date,'%d-%m-%Y') as directorder_date,a.directorder_gid," +
                " a.delivery_status,a.salesorder_gid,c.customer_name,  " +
                " a.customer_contactperson,a.customer_address,a.customer_emailid, " +
                " case when a.customer_contactnumber is null then b.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile " +
                " FROM smr_trn_tdeliveryorder a " +
                " left join smr_trn_tsalesorder d on d.salesorder_gid= a.salesorder_gid " +
                " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                " left join crm_mst_tcustomercontact b on b.customer_gid=a.customer_gid " +
                " where a.directorder_gid = '" + directorder_gid + "'";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tdeliveryorder");
                var GetSalesReturnAddSelect = new List<GetSalesReturnAddSelect_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetSalesReturnAddSelect.Add(new GetSalesReturnAddSelect_list
                        {
                            customer_name = ds["customer_name"].ToString(),
                            directorder_gid = ds["directorder_gid"].ToString(),
                            directorder_date = ds["directorder_date"].ToString(),
                            salesorder_gid = ds["salesorder_gid"].ToString(),
                            customer_contactperson = ds["customer_contactperson"].ToString(),
                            mobile = ds["mobile"].ToString(),
                            customer_emailid = ds["customer_emailid"].ToString(),
                            customer_address = ds["customer_address"].ToString(),
                        });
                        values.GetSalesReturnAddSelect_list = GetSalesReturnAddSelect;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DaGetSalesReturnAddDetails(string directorder_gid, string salesorder_gid, MdlSalesReturn values)
        {
            try
            {
                msSQL = "  SELECT distinct a.qty_returned ,a.product_qty as qty_quoted,a.product_qtydelivered,a.product_gid,a.product_uom_gid," +
                        "  a.directorderdtl_gid,c.product_code,c.product_name,d.productuom_name, a.product_description,c.stockable,c.serial_flag," +
                        "  a.directorderdtl_gid,a.salesorderdtl_gid,c.product_desc,f.productgroup_name FROM smr_trn_tdeliveryorderdtl a " +
                        "  left join smr_trn_tdeliveryorder b on a.directorder_gid=b.directorder_gid  " +
                        "  left join pmr_mst_tproduct c on a.product_gid = c.product_gid  " +
                        "  left join pmr_mst_tproductuom d on d.productuom_gid = a.product_uom_gid " +
                        "  left join pmr_mst_tproductgroup f on f.productgroup_gid=c.productgroup_gid where " +
                        "  a.directorder_gid = '"+ directorder_gid + "' and b.salesorder_gid='"+ salesorder_gid + "' order by directorderdtl_gid asc";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tdeliveryorder");
                var GetSalesReturnAddDetails = new List<GetSalesReturnAddDetails_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetSalesReturnAddDetails.Add(new GetSalesReturnAddDetails_list
                        {
                            product_name = ds["product_name"].ToString(),
                            productuom_name = ds["productuom_name"].ToString(),
                            qty_quoted = ds["qty_quoted"].ToString(),
                            product_qtydelivered = ds["product_qtydelivered"].ToString(),
                            qty_returned = ds["qty_returned"].ToString(),
                            product_uom_gid = ds["product_uom_gid"].ToString(),
                            product_description = ds["product_description"].ToString(),
                            directorderdtl_gid = ds["directorderdtl_gid"].ToString(),
                            salesorderdtl_gid = ds["salesorderdtl_gid"].ToString(),
                            product_gid = ds["product_gid"].ToString(),
                            product_code = ds["product_code"].ToString(),
                            product_desc = ds["product_desc"].ToString(),
                            productgroup_name = ds["productgroup_name"].ToString(),
                            qty_returnsales = 0,
                        });
                        values.GetSalesReturnAddDetails_list = GetSalesReturnAddDetails;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DaPostsalesReturn(string user_gid, string employee_gid, PostSalesReturn_list values)
        {
            try
            {
                msSQL = "select branch_gid from hrm_mst_temployee where employee_gid='" + employee_gid + "'";
                string branch_gid = objdbconn.GetExecuteScalar(msSQL);

                msgetGID = objcmnfunctions.GetMasterGID("VSRT");
                for (int i=0; i < values.Productlist.ToArray().Length; i++)
                {
                    msgetdtlGID = objcmnfunctions.GetMasterGID("VSRD");
                    msSQL = " insert into smr_trn_tsalesreturndtl ( " +
                            " salesreturndtl_gid , " +
                            " salesreturn_gid , " +
                            " product_gid , " +
                            " uom_gid," +
                            " qty_returned )" +
                            " values (" +
                            "'" + msgetdtlGID + "', " +
                            "'" + msgetGID + "'," +
                            "'" + values.Productlist[i].product_gid + "'," +
                            "'" + values.Productlist[i].product_uom_gid + "'," +
                            "'" + values.Productlist[i].qty_returnsales + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update smr_trn_tdeliveryorderdtl set qty_returned= qty_returned + '" + values.Productlist[i].qty_returnsales + "'" +
                        " where directorder_gid='" + values.directorder_gid + "' and product_gid='" + values.Productlist[i].product_gid + "' and product_uom_gid='" + values.Productlist[i].product_uom_gid + "'  " +
                        " and directorderdtl_gid='" + values.Productlist[i].directorderdtl_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = "update smr_trn_tsalesorderdtl set product_delivered= product_delivered - '" + values.Productlist[i].qty_returnsales + "'" +
                           " where salesorder_gid='" + values.salesorder_gid + "' and product_gid='" + values.Productlist[i].product_gid + "' and uom_gid='" + values.Productlist[i].product_uom_gid + "'" +
                           " and salesorderdtl_gid='" + values.Productlist[i].salesorderdtl_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                    msSQL = " insert into ims_trn_tstock (" +
                     " stock_gid," +
                     " branch_gid," +
                     " product_gid," +
                     " uom_gid," +
                     " stock_qty," +
                     " grn_qty," +
                     " rejected_qty," +
                     " unit_price," +
                     " stocktype_gid," +
                     " reference_gid," +
                     " stock_flag," +
                     " remarks," +
                     " created_by," +
                     " created_date," +
                     " issued_qty," +
                     " amend_qty," +
                     " damaged_qty," +
                     " adjusted_qty," +
                     " display_field" +
                     " )values(" +
                     " '" + msGetStockGID + "'," +
                     " '" + branch_gid  + "'," +
                     " '" + values.Productlist[i].product_gid + "'," +
                     " '" + values.Productlist[i].product_uom_gid + "'," +
                     "'" + values.Productlist[i].qty_returnsales + "', " +
                     " '0'," +
                     " '0'," +
                     " '0'," +
                     " 'SY0905270005'," +
                     " '" + msgetGID + "'," +
                     " 'N'," +
                     " 'From Sales Return'," +
                     " '" + user_gid + "'," +
                     " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                     " '0'," +
                     " '0'," +
                     " '0'," +
                     " '0'," +
                     " '" + values.product_description + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "select serial_gid from smr_tmp_tdeliveryorderserials where reference_gid='" + values.Productlist[i].directorderdtl_gid + "'";
                    objodbcDatareader = objdbconn.GetDataReader(msSQL);
                    if (objodbcDatareader.HasRows == true)
                    {
                        objodbcDatareader.Read();
                        msSQL = "update smr_trn_tserials set issued_flag='N', issued_from='Sales Return' , " +
                            "reference_gid='" + msgetGID + "',referencedtl_gid='" + msgetdtlGID + "'  where " +
                            "serial_gid='" + objodbcDatareader["serial_gid"].ToString() + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = "update pmr_trn_tserials set issued_flag='N', issued_from='Sales Return', " +
                                "reference_gid='" + msgetGID + "',referencedtl_gid='" + msgetdtlGID + "', " +
                                "stock_gid='" + msGetStockGID + "' " +
                                "where serial_gid='" + objodbcDatareader["serial_gid"].ToString() + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        objodbcDatareader.Close();
                    }
                }
                msSQL = " insert into smr_trn_tsalesreturn (" +
                        " salesreturn_gid, " +
                        " salesreturn_date, " +
                        " deliveryorder_gid," +
                        " approval_flag," +
                        " return_type," +
                        " remarks," +
                        " created_by," +
                        " created_date)" +
                        " values (" +
                        "'" + msgetGID + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + values.directorder_gid + "'," +
                        "'Approve Pending'," +
                        "'"+ values.return_type + "'," +
                        "'" + values.Remarks + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = " Sales Return added Successfully.";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while adding sales return.";
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void DaGetViewSRProduct(string salesreturn_gid, MdlSalesReturn values)
        {
            try
            {

                msSQL = " select a.qty_returned,b.product_name,c.productuom_name,b.product_code,d.productgroup_name,a.product_remarks,b.product_desc from smr_trn_tsalesreturndtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid "+
                        " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid "+
                        " left join pmr_mst_tproductgroup d on d.productgroup_gid = b.productgroup_gid "+
                        " where a.salesreturn_gid = '"+ salesreturn_gid + "' order by a.salesreturndtl_gid asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getGetViewSRProduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getGetViewSRProduct_list
                        {
                            qty_returned = dt["qty_returned"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                        });
                        values.getGetViewSRProduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Request Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetViewreturnProduct(string directorder_gid, MdlSalesReturn values)
        {
            try
            {

                msSQL = " select a.product_qty,b.product_name,a.product_qtydelivered,a.qty_returned," +
                        " (a.product_qtydelivered-a.qty_returned) as actual, c.productuom_name,a.productgroup_name,a.product_description,b.product_desc  from smr_trn_tdeliveryorderdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid" +
                        " left join pmr_mst_tproductuom c on a.product_uom_gid = c.productuom_gid  " +
                        " where a.directorder_gid = '"+ directorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewreturnProduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewreturnProduct_list
                        {
                            product_qty = dt["product_qty"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_description = dt["product_description"].ToString(),
                            product_qtydelivered = dt["product_qtydelivered"].ToString(),
                            qty_returned = dt["qty_returned"].ToString(),
                            actual = dt["actual"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                        });
                        values.GetViewreturnProduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Request Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }





    }
}