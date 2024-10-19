using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using static ems.inventory.Models.MdlImsTrnRaiseMI;
using System.Runtime.Remoting;
using System.Windows.Media.Media3D;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnStoreRequisition
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objTbl;
        string msGetGid, lsbranch_gid, lsqtyrequested, msGetStockGID, msGetStockGID1, lsproduct_price, lscustomer_gid, lsstockqty, msGetGID1, msGetDtlGID, msGetGIDP;
        int mnResult, mnResult1;
        string issued_qty;
        int qtyrequested, finalQty;
        string base_url, api_key = string.Empty;
        double lsbudgetallocated, lsprovisional,  lsamtused, lsavailable, lsreq, lstolrequest, lsreq1, lsrequested;
        string lsproduct_gid, lsproductuom_gid, msGetGID, lsproductuom_gid1, lsproduct_remarks, lsavailable2, lsproduct_code, lsproduct_name, lsproductuom_name, lsproductgroup_gid, lsproductgroup_name,
            lsqty_requested, lsdisplay_field;

        public void DaGetSRSummary(MdlImsTrnStoreRequisition values)
        {
            try
            {

                msSQL = " select a.storerequisition_gid,a.storerequisition_gid as store,a.storerequisition_status,a.branch_gid, date_format(a.storerequisition_date,'%d-%m-%Y') as storerequisition_date ,  " +
                        " b.branch_name,b.branch_prefix,a.created_by,c.user_firstname,date_format(a.created_date,'%d-%m-%Y') as created_date from ims_trn_tstorerequisition a " +
                        " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                        " left join hrm_mst_tbranch b on b.branch_gid = a.branch_gid " +
                        " where 0 = 0 order by date(a.storerequisition_date) desc,a.storerequisition_date asc, a.storerequisition_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<srsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new srsummary_list
                        {
                            storerequisition_gid = dt["storerequisition_gid"].ToString(),
                            store = dt["store"].ToString(),
                            storerequisition_status = dt["storerequisition_status"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            storerequisition_date = dt["storerequisition_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.srsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting SR Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetProductrolGroup(MdlImsTrnStoreRequisition values)
        {
            msSQL = " select b.productgroup_gid, b.productgroup_name from   ims_mst_trol a" +
                    " left join pmr_mst_tproductgroup b on b.productgroup_gid =a.productgroup_gid" +
                    " group by productgroup_gid";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<getrolproductgroup_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new getrolproductgroup_list
                    {
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                    });
                    values.getrolproductgroup_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetOnrolProductGroup(string productgroup_gid, MdlImsTrnStoreRequisition values)
        {
            try
            {


                if (productgroup_gid != null)
                {
                    msSQL = " select b.product_gid,b.product_name from ims_mst_trol a " +
                           " left join pmr_mst_tproduct b on b.product_gid=a.product_gid  " +
                           " where a.productgroup_gid= '" + productgroup_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<getrolproduct_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new getrolproduct_list
                            {
                                product_name = dt["product_name"].ToString(),
                                product_gid = dt["product_gid"].ToString()

                            });
                            values.getrolproduct_list = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Changing Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetrolProductSummary( MdlImsTrnStoreRequisition values)
        {
            try
            {
                    msSQL = " select a.rol_gid,a.reorder_level,a.product_gid,a.productuom_gid,b.product_code,b.product_name,c.productgroup_name,c.productgroup_gid,b.product_desc, " +
                           "  d.productuom_name,ifnull(sum(e.stock_qty + amend_qty - damaged_qty - issued_qty - transfer_qty), 0) as available_quantity from ims_mst_trol a " +
                           "  left join pmr_mst_tproduct b on b.product_gid = a.product_gid " + 
                           "  left join pmr_mst_tproductgroup c on c.productgroup_gid = a.productgroup_gid " +
                           "  left join pmr_mst_tproductuom d on d.productuom_gid = a.productuom_gid " +
                           "  left join ims_trn_tstock e on e.product_gid = a.product_gid group by a.product_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<getrolproduct1_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new getrolproduct1_list
                            {
                                rol_gid = dt["rol_gid"].ToString(),
                                reorder_level = dt["reorder_level"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                available_quantity = dt["available_quantity"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                product_desc = dt["product_desc"].ToString(),

                            });
                            values.getrolproduct1_list = getModuleList;
                        }
                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Changing Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostOnSRproduct(string user_gid, srproductsingle_list values)
        {
            try
            {
                msSQL = " select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                lsproductuom_gid1 = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select qty_requested,display_field from ims_tmp_tstorerequisition where " +
                        " product_gid = '" + values.product_gid + "' and " +
                        " productuom_gid = '" + lsproductuom_gid1 + "' and " +
                        " user_gid = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsrequested = (double)dt["qty_requested"];
                        lsreq1 = values.qty_requested;
                        lstolrequest = lsreq1 + lsrequested;
                        msSQL = " update ims_tmp_tstorerequisition " +
                                " set qty_requested ='" + lstolrequest + "'" +
                                " where  " +
                                " product_gid = '" + values.product_gid + "' and " +
                                " productuom_gid = '" + lsproductuom_gid1 + "' and " +
                                " display_field='" + (string.IsNullOrEmpty(values.display_field) ? "" : values.display_field.Replace("'", "\\\'")) + "' and" +
                                " user_gid = '" + user_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Product Qty Updated Successfully!!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Product Qty!!";
                        }
                    }
                }
                else
                {
                    msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                    string product_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL="select productgroup_name from pmr_mst_tproductgroup where productgroup_gid='"+values.productgroup_name + "'";
                    string productgroup_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " insert into ims_tmp_tstorerequisition( " +
                            " product_gid, " +
                            " productuom_gid,  " +
                            " productgroup_gid,  " +
                            " product_code,  " +
                            " product_name,  " +
                            " productuom_name,  " +
                            " reorder_level,  " +
                            " productgroup_name,  " +
                            " qty_requested, " +
                            " user_gid, " +
                            " display_field ) " +
                            " values( " +
                            " '" + values.product_gid + "'," +
                            " '" + lsproductuom_gid1 + "'," +
                            " '" + values.productgroup_name + "'," +
                            " '" + values.product_code + "'," +
                            " '" + product_name + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + values.reorder_level + "'," +
                            " '" + productgroup_name + "'," +
                            " '" + values.qty_requested + "'," +
                            " '" + user_gid + "'," +
                            "'" + (string.IsNullOrEmpty(values.product_desc) ? "" : values.product_desc.Replace("'", "\\\'"))  +"'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product!!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DatmprolProductSummary(string user_gid, MdlImsTrnStoreRequisition values)
        {
            try
            {

                msSQL = " Select distinct a.tmpsr_gid,  b.product_name,c.productgroup_name,b.product_gid,d.productuom_name,a.reorder_level,  "+
                        " b.product_code, b.product_name as product, a.qty_requested, a.product_remarks,b.product_desc as display_field," +
                        " ifnull(sum(e.stock_qty + amend_qty - damaged_qty - issued_qty - transfer_qty), 0) as available_quantity " +
                        " from ims_tmp_tstorerequisition a " +
                        "  left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                        "  LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = b.productuom_gid " +
                        " left join ims_trn_tstock e on e.product_gid = a.product_gid "+
                        " where a.user_gid = '" + user_gid + "' group by tmpsr_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tmpsrproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tmpsrproduct_list
                        {
                            tmpsr_gid = dt["tmpsr_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product = dt["product"].ToString(),
                            qty_requested = Convert.ToDecimal(dt["qty_requested"]).ToString("F2"),
                            product_remarks = dt["product_remarks"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            reorder_level = dt["reorder_level"].ToString(),
                            available_quantity = dt["available_quantity"].ToString(),
                        });
                        values.tmpsrproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting tmp product  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostSR(string user_gid,string branch_gid, storeRequisition_list values)
        {
            try
            {
                msSQL= "select count(tmpsr_gid) as count from ims_tmp_tstorerequisition where user_gid='"+ user_gid + "'";
                string count =objdbconn.GetExecuteScalar(msSQL);
                msGetGid = objcmnfunctions.GetMasterGID("STRI");
                msSQL = " Insert into ims_trn_tstorerequisition " +
                       " (storerequisition_gid, " +
                       " branch_gid, " +
                       " storerequisition_date, " +
                       " storerequisition_remarks, " +
                       " storerequisition_referencenumber, " +
                       " created_by, " +
                       " created_date, " +
                       " storerequisition_flag, " +
                       " product_count, " +
                       " enquiry_raised, " +
                       " purchasereq_type, " +
                       " type, " +
                       " purchaseorder_raised, " +
                       " rol_flag,priority_remarks,storerequisition_status)  " +
                       " values (" +
                       "'" + msGetGid + "', " +
                       "'" + branch_gid+ "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                       " '', " +
                       " '', " +
                       " '" + user_gid + "', " +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                       " 'SR Approved', " +
                       " '" + count + "', " +
                       " 'N', " +
                       " 'Open', " +
                       " '', " +
                       "'N', " +
                       "'Y'," +
                       "'" +values.storerequisition_remarks + "'," +
                       "'SR Approved')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if(mnResult !=0)
                {
                    msGetGIDP = objcmnfunctions.GetMasterGID("PPRP");
                    msSQL = " Insert into pmr_trn_tpurchaserequisition " +
                               " (purchaserequisition_gid, " +
                               " branch_gid, " +
                               " purchaserequisition_date, " +
                               " purchaserequisition_remarks, " +
                               " created_by, " +
                               " created_date, " +
                               " purchaserequisition_flag, " +
                               " product_count, " +
                               " enquiry_raised, " +
                               " purchasereq_type, " +
                               " purchaseorder_raised) " +
                               " values (" +
                               "'" + msGetGIDP + "'," +
                               "'" + branch_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                               "'" + (string.IsNullOrEmpty(values.storerequisition_remarks) ? "" : values.storerequisition_remarks.Replace("'", "\\\'"))  + "'," +
                               "'" + user_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                               "'PR Approved'," +
                               "'" + count + "'," +
                               "'N', " +
                               "'Open', " +
                               "'N' )";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult1 != 0)
                    {
                        msSQL = " Select distinct a.tmpsr_gid,  b.product_name,c.productgroup_name,b.product_gid,d.productuom_name,a.reorder_level,d.productuom_gid,c.productgroup_gid,   " +
                                " b.product_code, b.product_name as product, a.qty_requested, a.product_remarks,a.display_field  from ims_tmp_tstorerequisition a  " +
                                " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                                " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid  " +
                                " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = b.productuom_gid  " +
                                " where a.user_gid ='" + user_gid + "' group by tmpsr_gid ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                lsproduct_gid = dt["product_gid"].ToString();
                                lsproductuom_gid = dt["productuom_gid"].ToString();
                                lsproduct_remarks = dt["product_remarks"].ToString();
                                lsproduct_code = dt["product_code"].ToString();
                                lsproduct_name = dt["product_name"].ToString();
                                lsproductuom_name = dt["productuom_name"].ToString();
                                lsproductgroup_gid = dt["productgroup_gid"].ToString();
                                lsproductgroup_name = dt["productgroup_name"].ToString();
                                lsqty_requested = dt["qty_requested"].ToString();
                                lsdisplay_field = dt["display_field"].ToString();

                                msGetGID1 = objcmnfunctions.GetMasterGID("STRD");
                                msSQL = " Insert into ims_trn_tstorerequisitiondtl " +
                                        " (storerequisitiondtl_gid," +
                                        " storerequisition_gid , " +
                                        " product_gid," +
                                        " uom_gid," +
                                        " qty_requested, " +
                                        " user_gid, " +
                                        " display_field ) " +
                                        " values (" +
                                        "'" + msGetGID1 + "'," +
                                        "'" + msGetGid + "'," +
                                        "'" + lsproduct_gid + "'," +
                                        "'" + lsproductuom_gid + "'," +
                                        "'" + lsqty_requested + "', " +
                                        "'" + user_gid + "', " +
                                        "'" + (string.IsNullOrEmpty(lsdisplay_field) ? "" : lsdisplay_field.Replace("'", "\\\'"))  + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    msGetDtlGID = objcmnfunctions.GetMasterGID("PPDC");
                                    msSQL = " Insert into pmr_trn_tpurchaserequisitiondtl " +
                                            " (purchaserequisitiondtl_gid," +
                                            " purchaserequisition_gid , " +
                                            " product_gid," +
                                            " uom_gid," +
                                            " product_code," +
                                            " product_name," +
                                            " productuom_name," +
                                            " productgroup_gid," +
                                            " productgroup_name," +
                                            " requested_by," +
                                            " display_field," +
                                            " product_remarks," +
                                            " qty_requested, " +
                                            " user_gid) " +
                                            " values( " +
                                            "'" + msGetDtlGID + "'," +
                                            "'" + msGetGIDP + "'," +
                                            "'" + lsproduct_gid + "'," +
                                            "'" + lsproductuom_gid + "'," +
                                            "'" + lsproduct_code + "'," +
                                            "'" + lsproduct_name + "'," +
                                            "'" + lsproductuom_name + "'," +
                                            "'" + lsproductgroup_gid + "'," +
                                            "'" + lsproductgroup_name + "'," +
                                            "'" + user_gid + "'," +
                                            "'" + (string.IsNullOrEmpty(lsdisplay_field) ? "" : lsdisplay_field.Replace("'", "\\\'")) + "', " +
                                            "'" + (string.IsNullOrEmpty(lsdisplay_field) ? "" : lsdisplay_field.Replace("'", "\\\'")) + "', " +
                                            "'" + lsqty_requested + "', " +
                                            "'" + user_gid + "')";
                                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if(mnResult1==1)
                                    {
                                        msSQL = "delete from ims_tmp_tstorerequisition";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        values.status = true;
                                        values.message = "store Requisition raised!";

                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Inserting purchase Requisition dtl!";

                                    }

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Inserting store Requisition dtl!";
                                }
                            }
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Inserting Purchase Requisition !";
                    }

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Inserting store Requisition !";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaDeletetmpProductSummary(string tmpsr_gid, MdlImsTrnStoreRequisition values)
        {
            try
            {

                msSQL = "  delete from ims_tmp_tstorerequisition where tmpsr_gid='" + tmpsr_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Delete Successfully!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting product !";
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while deleting product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetSRViewProduct(string storerequisition_gid, MdlImsTrnStoreRequisition values)
        {
            try
            {

                msSQL = " SELECT a.storerequisition_gid,a.storerequisitiondtl_gid, a.product_gid,b.product_code,   "+
                         " a.qty_requested, b.product_name, c.productgroup_name " +
                         " FROM ims_trn_tstorerequisitiondtl a " +
                         " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                         " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                         " where a.storerequisition_gid = '"+ storerequisition_gid + "'" +
                         " order by a.storerequisitiondtl_gid asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<storeRequisitionproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new storeRequisitionproduct_list
                        {
                            storerequisition_gid = dt["storerequisition_gid"].ToString(),
                            storerequisitiondtl_gid = dt["storerequisitiondtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                        values.storeRequisitionproduct_list = getModuleList;
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
        public void DaGetStoreView(string storerequisition_gid, MdlImsTrnStoreRequisition values)
        {
            try
            {
                msSQL = " select a.storerequisition_gid,a.branch_gid,date_format(a.storerequisition_date, '%d-%m-%Y') as storerequisition_date, e.department_name,a.priority_remarks, " +
                         " b.branch_name,a.created_by,c.user_firstname from ims_trn_tstorerequisition a " +
                         " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                         " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                         " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
                         " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                         " where storerequisition_gid = '"+ storerequisition_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetStoreView_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetStoreView_list
                        {
                            storerequisition_gid = dt["storerequisition_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            storerequisition_date = dt["storerequisition_date"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            priority_remarks = dt["priority_remarks"].ToString(),
                        });
                        values.GetStoreView_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetstoreViewProduct(string storerequisition_gid, MdlImsTrnStoreRequisition values)
        {
            try
            {
                msSQL = " select a.storerequisitiondtl_gid, a.storerequisition_gid, a.product_gid,b.product_name,b.product_code,  "+
                         " c.productgroup_name,d.productuom_name,a.qty_requested, a.display_field from ims_trn_tstorerequisitiondtl a " +
                         " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                         " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                         " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
                         " where storerequisition_gid = '" + storerequisition_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetstoreViewProduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetstoreViewProduct_list
                        {
                            storerequisitiondtl_gid = dt["storerequisitiondtl_gid"].ToString(),
                            storerequisition_gid = dt["storerequisition_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            display_field = dt["display_field"].ToString(),
                        });
                        values.GetstoreViewProduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}