using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using System.Data.SqlClient;
using System.Web.Services.Description;
using System.Globalization;
using System.Net.NetworkInformation;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnDirectIssueMaterial
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2, mnResult1, mnResult3;
        string msGetGid, msGetGid1, lsempoyeegid, mnCtr, lsbranch, lscostcenter, msIssueGID, lsemployeegid, lslocation, msGetGID, msGetImRC, msGetPodc, mcGetGID, lsStockGid, lsStockQty, msstockdtlGid, msGetStockTrackerGID,lsmaterialissued_date;
        double lsbudgetallocated, lsprovisional, lsamtused, lsavailable, lsreq, lstolrequest, lsreq1, lsrequested;
        string lsproduct_gid,lsproductuom_gid ,lsproduct_remarks , lsavailable2,lsproduct_code, lsproduct_name,lsproductuom_name,lsproductgroup_gid,lsproductgroup_name,
            lsqty_requested,lsdisplay_field;
        List<int> quantities = new List<int>();
        public void DaGetimslocation(MdlImsTrnDirectIssueMaterial values)
        {
            msSQL = " select location_gid, location_name, location_code "+
                    " from ims_mst_tlocation ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<imslocation_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new imslocation_list
                    {
                        location_gid = dt["location_gid"].ToString(),
                        location_name = dt["location_name"].ToString(),
                        location_code = dt["location_code"].ToString(),
                    });
                    values.imslocation_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetimscostcenter(string user_gid,MdlImsTrnDirectIssueMaterial values)
        {
            msSQL = "select a.costcenter_gid, a.costcenter_name from pmr_mst_tcostcenter a " +
                    "left join pmr_trn_tcostcenter2department b on a.costcenter_gid = b.costcenter_gid " +
                    "left join hrm_mst_temployee c on b.department_gid = c.department_gid  " +
                    "left join adm_mst_tuser d on c.user_gid = d.user_gid  where d.user_gid ='"+ user_gid +"'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<imscostenter_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new imscostenter_list
                    {
                        costcenter_gid = dt["costcenter_gid"].ToString(),
                        costcenter_name = dt["costcenter_name"].ToString(),
                    });
                    values.imscostenter_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetimsdisummary(string user_gid, MdlImsTrnDirectIssueMaterial values)
        {
            msSQL = "select concat_ws(' - ',a.user_firstname,a.user_lastname) as user_firstname," +
                    "b.branch_gid, b.department_gid, d.branch_name,d.mainbranch_flag,c.department_name " +
                    "from adm_mst_tuser a left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                    "left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                    "left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid where a.user_gid = '" + user_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<imsdirectissue_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new imsdirectissue_list
                    {
                        user_firstname = dt["user_firstname"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        department_name = dt["department_name"].ToString(),
                    });
                    values.imsdirectissue_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetimsavailable(string costcenter_gid, MdlImsTrnDirectIssueMaterial values)
        {
            string msSQL = "SELECT a.costcenter_gid, a.costcenter_code, a.costcenter_name, FORMAT(a.budget_allocated, 2) AS budget_allocated " +
                           "FROM pmr_mst_tcostcenter a " +
                           "WHERE costcenter_gid='" + costcenter_gid + "'";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsbudgetallocated = Convert.ToDouble(objODBCDataReader["budget_allocated"]);
            }
            msSQL = "SELECT a.costcenter_gid, FORMAT(SUM(CASE WHEN b.provisional_amount IS NULL THEN 0.00 ELSE b.provisional_amount END), 2) AS provisional_amount " +
                    "FROM pmr_mst_tcostcenter a " +
                    "LEFT JOIN pmr_trn_tpurchaserequisition b ON b.costcenter_gid=a.costcenter_gid " +
                    "WHERE a.costcenter_gid='" + costcenter_gid + "' AND b.purchaseorder_flag <>'PO raised' AND b.purchaserequisition_flag <> 'PR Pending Approval' GROUP BY b.costcenter_gid";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsprovisional = Convert.ToDouble(objODBCDataReader["provisional_amount"]);
            }

            msSQL = "SELECT a.costcenter_gid, FORMAT(SUM(CASE WHEN b.total_amount IS NULL THEN 0.00 ELSE b.total_amount END), 2) AS total_amount " +
                    "FROM pmr_mst_tcostcenter a " +
                    "LEFT JOIN pmr_trn_tpurchaseorder b ON b.costcenter_gid=a.costcenter_gid " +
                    "WHERE a.costcenter_gid='" + costcenter_gid + "' AND b.purchaseorder_flag = 'PO Approved' GROUP BY b.costcenter_gid";
            objODBCDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsamtused = Convert.ToDouble(objODBCDataReader["total_amount"]);
            }
             lsavailable = lsbudgetallocated - lsamtused - lsprovisional;
            //var getModuleList = new List<imsavailable_list>();
            //if (lsavailable != 0)
            //{
            //    getModuleList.Add(new imsavailable_list
            //    {
            //        available_amount = lsavailable.ToString(),
            //    });
            //    values.imsavailable_list = getModuleList;
            //}
               lsavailable2 = lsavailable.ToString("F2");
              values.available_amount = lsavailable;
        }

        public void DaGetimsproducttype(MdlImsTrnDirectIssueMaterial values)
        {
            try
            {

                msSQL = " select producttype_gid, producttype_code , producttype_name from pmr_mst_tproducttype";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<imsproducttype_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new imsproducttype_list
                        {
                            producttype_gid = dt["producttype_gid"].ToString(),
                            producttype_code = dt["producttype_code"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                        });
                        values.imsproducttype_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImsProductSummary(string branch_gid,string producttype_gid, string product_name,MdlImsTrnDirectIssueMaterial values)
        {
            try
            {
                msSQL = "  Select distinct sum(f.stock_qty+f.amend_qty-f.issued_qty-f.damaged_qty-f.transfer_qty) as stock_quantity, a.product_desc, " +
                        "  a.product_gid,a.product_code,a.product_name,a.productgroup_gid,d.productgroup_name,c.productuom_gid,c.productuom_name "+
                        "  from pmr_mst_tproduct a left join pmr_mst_tproductuom c on a.productuom_gid= c.productuom_gid "+
                        "  left join ims_trn_tstock f on a.product_gid=f.product_gid  "+
                        "  left join pmr_mst_tproductgroup d on a.productgroup_gid=d.productgroup_gid  "+
                        "  left join pmr_mst_tproducttype e on e.producttype_gid = a.producttype_gid " +
                        "  where  (f.stock_qty-f.issued_qty ) >= 0 and f.stock_flag='Y' and f.branch_gid='"+branch_gid+"' ";

                if (!string.IsNullOrEmpty(producttype_gid) && producttype_gid != "undefined" && producttype_gid != "null")
                {
                  msSQL+= " AND a.producttype_gid = '" + producttype_gid + "'";
                }

                if (!string.IsNullOrEmpty(product_name) && product_name != "null")
                {
                   msSQL+=" AND a.product_name LIKE '%" + product_name +"%'";
                }

                msSQL += "and e.producttype_name<>'Services' group by a.product_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<imsproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new imsproductsummary_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            display_field = dt["product_desc"].ToString(),
                        });
                        values.imsproductsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetImsProduct( string product_gid, MdlImsTrnDirectIssueMaterial values)
        {
            try
            {
                msSQL = "  Select distinct sum(f.stock_qty+f.amend_qty-f.issued_qty-f.damaged_qty-f.transfer_qty) as stock_quantity,a.product_desc, " +
                        "  a.product_gid,a.product_code,a.product_name,a.productgroup_gid,d.productgroup_name,c.productuom_gid,c.productuom_name " +
                        "  from pmr_mst_tproduct a left join pmr_mst_tproductuom c on a.productuom_gid= c.productuom_gid " +
                        "  left join ims_trn_tstock f on a.product_gid=f.product_gid  " +
                        "  left join pmr_mst_tproductgroup d on a.productgroup_gid=d.productgroup_gid  " +
                        "  where  (f.stock_qty-f.issued_qty ) >= 0 and f.stock_flag='Y' ";
                if (!string.IsNullOrEmpty(product_gid) && product_gid != "null")
                {
                    msSQL += " AND a.product_gid LIKE '%" + product_gid + "%'";
                }

                msSQL += "group by a.product_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<imsproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new imsproductsummary_list
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_quantity = dt["stock_quantity"].ToString(),
                            display_field = dt["product_desc"].ToString(),
                        });
                        values.imsproductsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }







        public void DaPostOnIssuematerial(string user_gid, imsproductissue_list values)
        {
            try
            {
       

                msSQL = " select qty_requested,display_field from ims_tmp_tmaterialrequisition where " +
                        " product_gid = '" + values.product_name + "' and " +
                        " productuom_name = '" + values.productuom_name + "' and " +
                        " display_field = '" +values.display_field.Replace("'", "\\\'") + "' and " +
                        " user_gid = '" + user_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsrequested= (double)dt["qty_requested"];
                        lsreq1 = values.qty_requested;
                        lstolrequest = lsreq1 + lsrequested;
                        lsreq = values.stock_quantity;
                        if (lsreq < lstolrequest)
                        {
                            values.status = false;
                            values.message = "Requested Quantity should not be higher than available stock!";
                        }
                        else
                        {
                            msSQL = " update ims_tmp_tmaterialrequisition " +
                                    " set qty_requested ='" + lstolrequest + "', " +
                                    " display_field ='" +values.display_field.Replace("'", "\\\'") + "' " +
                                    " where  " +
                                    " product_gid = '" + values.product_name + "' and " +
                                    " productuom_name = '" + values.productuom_name + "' and " +
                                    " display_field='" +values.display_field.Replace("'", "\\\'") + "' and" +
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
                }
                else
                {
                    msSQL = " insert into ims_tmp_tmaterialrequisition( " +
                            " product_gid, " +
                            " productuom_gid,  " +
                            " productgroup_gid,  " +
                            " product_code,  " +
                            " product_name,  " +
                            " productuom_name,  " +
                            " productgroup_name,  " +
                            " qty_requested, " +
                            " user_gid, " +
                            " display_field ) " +
                            " values( " +
                            " '" + values.product_name + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + values.productgroup_name + "'," +
                            " '" + values.product_code + "'," +
                            " '" + values.product_gid + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + values.productgroup_gid + "'," +
                            " '" + values.qty_requested + "'," +
                            " '" + user_gid + "'," +
                            " '" +values.display_field.Replace("'", "\\\'") + "'" + ")";
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
        public void DaPostIssuematerial(string user_gid, POissuemetrial values)
        {
            try
            {
                for (int i = 0; i < values.imsproductissue_list.Count; i++)
                {
                    msSQL = " select qty_requested,display_field from ims_tmp_tmaterialrequisition where " +
                            " product_gid = '" + values.imsproductissue_list[i].product_gid + "' and " +
                            " productuom_gid = '" + values.imsproductissue_list[i].productuom_gid + "' and " +
                            " display_field = '" + values.imsproductissue_list[i].display_field.Replace("'", "\\\'") + "' and " +
                            " user_gid = '" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            lsrequested = (double)dt["qty_requested"];
                            lsreq1 = values.imsproductissue_list[i].qty_requested;
                            lstolrequest = lsreq1 + lsrequested;
                            lsreq = values.imsproductissue_list[i].stock_quantity;
                            if (lsreq < lstolrequest)
                            {
                                values.status = false;
                                values.message = "Requested Quantity should not be higher than available stock!";
                            }
                            else
                            {
                                msSQL = " update ims_tmp_tmaterialrequisition " +
                                        " set qty_requested ='" + lstolrequest + "', " +
                                        " display_field ='" + values.imsproductissue_list[i].display_field.Replace("'", "\\\'") + "' " +
                                        " where product_gid = '" + values.imsproductissue_list[i].product_gid + "' and " +
                                        " productuom_gid = '" + values.imsproductissue_list[i].productuom_gid + "' and " +
                                        " display_field='" + values.imsproductissue_list[i].display_field.Replace("'", "\\\'") + "' and" +
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
                    }
                    else
                    {
                        msSQL = " insert into ims_tmp_tmaterialrequisition( " +
                                " product_gid, " +
                                " productuom_gid,  " +
                                " productgroup_gid,  " +
                                " product_code,  " +
                                " product_name,  " +
                                " productuom_name,  " +
                                " productgroup_name,  " +
                                " qty_requested, " +
                                " user_gid, " +
                                " display_field ) " +
                                " values (" +
                                " '" + values.imsproductissue_list[i].product_gid + "'," +
                                " '" + values.imsproductissue_list[i].productuom_gid + "'," +
                                " '" + values.imsproductissue_list[i].productgroup_gid + "'," +
                                " '" + values.imsproductissue_list[i].product_code + "'," +
                                " '" + values.imsproductissue_list[i].product_name + "'," +
                                " '" + values.imsproductissue_list[i].productuom_name + "'," +
                                " '" + values.imsproductissue_list[i].productgroup_name + "'," +
                                " '" + values.imsproductissue_list[i].qty_requested + "'," +
                                " '" + user_gid + "'," +
                                " '" + values.imsproductissue_list[i].display_field.Replace("'", "\\\'") + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }


               

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
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGettmpProductSummary(string user_gid,  MdlImsTrnDirectIssueMaterial values)
        {
            try
            {
                msSQL= "select a.branch_gid  from hrm_mst_temployee a left join adm_mst_tuser  b on b.user_gid=a.user_gid where a.user_gid='"+user_gid+"'";
                string lsbranch=objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select a.tmpmaterialrequisition_gid,ROUND(SUM(g.stock_qty + g.amend_qty - g.issued_qty - g.damaged_qty - g.transfer_qty), 2) AS stock_quantity, a.product_remarks, Format(a.qty_requested,2) as qty_requested, " +
                        " Format(a.qty_issued,2) as qty_issued, a.product_gid," +
                        " b.product_name, b.product_code, c.productgroup_name, " +
                        " a.productuom_name,  a.display_field  from ims_tmp_tmaterialrequisition a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid  " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_mst_tproductuomclass d on d.productuomclass_gid = b.productuomclass_gid " +
                        " left join pmr_mst_tproductuom f on f.productuom_gid = a.productuom_gid" +
                        " LEFT JOIN ims_trn_tstock g ON b.product_gid = g.product_gid where a.user_gid = '"+user_gid+"' " +
                        " AND g.branch_gid = '"+lsbranch+ "' AND(g.stock_qty - g.issued_qty) >= 0 AND g.stock_flag = 'Y' group by a.tmpmaterialrequisition_gid ";
    

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tmpproductsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tmpproductsummary_list
                        {
                            tmpmaterialrequisition_gid = dt["tmpmaterialrequisition_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product = dt["product_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_quantity = Convert.ToDecimal(dt["stock_quantity"]).ToString("F2"),
                        });
                        values.tmpproductsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaDeletetmpProductSummary(string tmpmaterialrequisition_gid, MdlImsTrnDirectIssueMaterial values)
        {
            try
            {

                msSQL = "  delete from ims_tmp_tmaterialrequisition where tmpmaterialrequisition_gid='" + tmpmaterialrequisition_gid + "'  ";
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
                values.message = "Exception occured while deleting product in PO!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaMaterialIssue(string user_gid, issuematerial_list values)
        {
            try
            {
                msSQL = "select count(*) as mnCtr from ims_tmp_tmaterialrequisition where user_gid ='"+ user_gid + "'";
                mnCtr=objdbconn.GetExecuteScalar(msSQL);
                msSQL= "select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name +"'";
                lsbranch = objdbconn.GetExecuteScalar(msSQL);
                if (values.costcenter_name != "")
                {
                    msSQL = "select costcenter_gid from pmr_mst_tcostcenter where costcenter_name='" + values.costcenter_name + "'";
                    lscostcenter = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.location_name!="")
                {
                    msSQL = "select location_gid from ims_mst_tlocation where location_name = '" + values.location_name + "'";
                    lslocation = objdbconn.GetExecuteScalar(msSQL);

                }
                 DateTime uiDate = DateTime.ParseExact(values.materialissued_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                lsmaterialissued_date = uiDate.ToString("yyyy-MM-dd HH:mm:ss");

                msGetGid = objcmnfunctions.GetMasterGID("IMRP");
                msSQL = " Insert into ims_trn_tmaterialrequisition " +
                       " (materialrequisition_gid, " +
                       " branch_gid, " +
                       " materialrequisition_date, " +
                       " materialrequisition_remarks, " +
                       " materialrequisition_reference, " +
                       " user_gid, " +
                       " created_date, " +
                       " materialrequisition_status, " +
                       " product_count, " +
                       " materialrequisition_type, " +
                       " material_status, " +
                       " priority, " +
                       " priority_remarks, " +
                       " costcenter_gid ) " +
                       " values (" +
                       "'" + msGetGid + "', " +
                       "'" + lsbranch + "'," +
                       "'" + lsmaterialissued_date + "', " +
                       "'" + values.materialrequisition_remarks.Replace("'", "\\\'") + "', " +
                       "'" + values.materialrequisition_reference + "', " +
                       "'" + user_gid + "', " +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                       "' Pending', " +
                       "'" + mnCtr + "', " +
                       "'PT00010001204' , " +
                       "'PR Not Raised', " +
                       "'N', " +
                       "'Null', " +
                       "'" + lscostcenter + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)

                {
                    msIssueGID = objcmnfunctions.GetMasterGID("IMIT");
                    msSQL = " Insert into ims_trn_tmaterialissued (" +
                              " materialissued_gid, " +
                              " materialissued_date, " +
                              " branch_gid, " +
                              " materialrequisition_gid, " +
                              " materialrequisition_type, " +
                              " materialissued_status, " +
                              " materialissued_reference, " +
                              " user_gid, " +
                              " priority, " +
                              " materialissued_remarks, " +
                              " location_gid, " +
                              " created_date) " +
                              " values ( " +
                              "'" + msIssueGID + "'," +
                              "'" + lsmaterialissued_date + "', " +
                              "'" + lsbranch + "'," +
                              "'" + msGetGid + "'," +
                              "'PT00010001204'," +
                              "'Issued Accept Pending'," +
                              "'" + values.materialrequisition_reference + "', " +
                              "'" + user_gid + "'," +
                              "'N', " +
                              "'" + values.materialrequisition_remarks.Replace("'", "\\\'") + "', " +
                              "'" + lslocation + "', " +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " select a.product_gid, productuom_gid, a.product_remarks,a.product_code,a.product_name," +
                                " a.productuom_name,a.productgroup_gid,a.productgroup_name," +
                                " a.qty_requested,a.display_field from ims_tmp_tmaterialrequisition a" +
                                " where a.user_gid ='"+ user_gid +"'";
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

                                msGetImRC = objcmnfunctions.GetMasterGID("IMRC");

                                msSQL = " Insert into ims_trn_tmaterialrequisitiondtl " +
                                          " (materialrequisitiondtl_gid," +
                                          " materialrequisition_gid , " +
                                          " product_gid," +
                                          " productuom_gid, " +
                                          " productgroup_gid,  " +
                                          " product_code,  " +
                                          " product_name,  " +
                                          " productuom_name,  " +
                                          " productgroup_name,  " +
                                          " qty_requested, " +
                                          " qty_issued, " +
                                          " user_gid, " +
                                          " display_field , " +
                                          " requested_by , " +
                                          " mr_originalqty," +
                                          " mr_newproductstatus" +
                                          " )values (" +
                                          "'" + msGetImRC + "'," +
                                          "'" + msGetGid + "'," +
                                          "'" + lsproduct_gid + "'," +
                                          "'" + lsproductuom_gid + "'," +
                                          "'" + lsproductgroup_gid + "'," +
                                          "'" + lsproduct_code + "'," +
                                          "'" + lsproduct_name + "'," +
                                          "'" + lsproductuom_name + "'," +
                                          "'" + lsproductgroup_name + "'," +
                                          "'" + lsqty_requested + "', " +
                                          "'" + lsqty_requested + "', " +
                                          "'" + user_gid + "', " +
                                          "'" + lsdisplay_field.Replace("'", "\\\'") + "'," +
                                          "'" + values.employee_name + "'," +
                                          "'" + lsqty_requested + "'," +
                                          "'N')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 0)
                                {
                                    values.status = false;
                                    values.message = "Error While Inserting material details!";
                                }

                                //if (!string.IsNullOrEmpty(lslocation))
                                //{
                                //    msSQL = " SELECT (stock_qty+amend_qty-issued_qty-damaged_qty-transfer_qty) as stock_qty, issued_qty, " +
                                //            " stock_gid FROM ims_trn_tstock WHERE (stock_qty > issued_qty) AND " +
                                //            " product_gid = '" + lsproduct_gid + "' AND branch_gid = '" + lsbranch + "' " +
                                //            "  and stock_flag='Y' ";
                                //    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                //}
                                //else
                                //{
                                //    msSQL = " SELECT (stock_qty+amend_qty-issued_qty-damaged_qty-transfer_qty) as stock_qty, " +
                                //            " issued_qty, stock_gid FROM ims_trn_tstock WHERE (stock_qty > issued_qty)" +
                                //            " AND product_gid = '" + lsproduct_gid + "' and stock_flag='Y'" +
                                //            " AND branch_gid = '" + lsbranch + "' ";
                                //    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                //}

                                //while(objODBCDataReader.HasRows)
                                //{
                                //    int stockQty = Convert.ToInt32(objODBCDataReader["stock_qty"]);
                                //    int issueQty = Convert.ToInt32(objODBCDataReader["issued_qty"]);
                                //    string stockgid = objODBCDataReader["stock_gid"].ToString();
                                //    int qtyrest = Convert.ToInt32(lsqty_requested);
                                //    int quantityForThisRow = Math.Min(qtyrest, stockQty);

                                //    quantities.Add(quantityForThisRow);
                                //    while (objODBCDataReader.Read() && quantities.Count > 0)
                                //    {
                                //        if (!string.IsNullOrEmpty(lslocation))
                                //        {
                                //            msSQL = " UPDATE ims_trn_tstock " +
                                //                   " SET issued_qty = " + (issueQty + quantityForThisRow) + " " +
                                //                   " WHERE product_gid = '" + lsproduct_gid + "' " +
                                //                   " AND branch_gid = '" + lsbranch + "' " +
                                //                   " AND stock_gid = '" + stockgid + "'";
                                //        }
                                //        else
                                //        {
                                //            msSQL = " UPDATE ims_trn_tstock " +
                                //                    " SET issued_qty = " + (issueQty + quantityForThisRow) + " " +
                                //                    " WHERE product_gid = '" + lsproduct_gid + "' " +
                                //                    " AND branch_gid = '" + lsbranch + "' " +
                                //                    " AND stock_gid = '" + stockgid + "'";

                                //        }
                                //        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                //    }
                                //    objODBCDataReader.Close();
                                //}
                                // Step 1: Select and Calculate Quantities for Each Row
                                if (!string.IsNullOrEmpty(lslocation))
                                {
                                    msSQL = "SELECT (stock_qty + amend_qty - issued_qty - damaged_qty - transfer_qty) AS stock_qty, " +
                                            "issued_qty, stock_gid " +
                                            "FROM ims_trn_tstock " +
                                            "WHERE (stock_qty > issued_qty) AND product_gid = '" + lsproduct_gid + "' " +
                                            "AND branch_gid = '" + lsbranch + "' AND stock_flag = 'Y'";
                                }
                                else
                                {
                                    msSQL = "SELECT (stock_qty + amend_qty - issued_qty - damaged_qty - transfer_qty) AS stock_qty, " +
                                            "issued_qty, stock_gid " +
                                            "FROM ims_trn_tstock " +
                                            "WHERE (stock_qty > issued_qty) AND product_gid = '" + lsproduct_gid + "' " +
                                            "AND branch_gid = '" + lsbranch + "' AND stock_flag = 'Y'";
                                }

                                objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                List<int> quantities = new List<int>();
                                int qtyrest = Convert.ToInt32(lsqty_requested);

                                while (objODBCDataReader.Read())
                                {
                                    int stockQty = Convert.ToInt32(objODBCDataReader["stock_qty"]);
                                    int quantityForThisRow = Math.Min(qtyrest, stockQty);

                                    quantities.Add(quantityForThisRow);
                                    qtyrest -= quantityForThisRow;

                                    if (qtyrest <= 0)
                                    {
                                        break; // Exit the loop if the requested quantity has been fully accounted for
                                    }
                                }

                                objODBCDataReader.Close();

                                // Step 2: Update the Rows with Calculated Quantities
                                objODBCDataReader = objdbconn.GetDataReader(msSQL); // Reset the data reader to the beginning
                                int index = 0;

                                while (objODBCDataReader.Read() && index < quantities.Count)
                                {
                                    int issueQty = Convert.ToInt32(objODBCDataReader["issued_qty"]);
                                    string stockgid = objODBCDataReader["stock_gid"].ToString();
                                    int quantityForThisRow = quantities[index];
                                    index++;

                                    if (!string.IsNullOrEmpty(lslocation))
                                    {
                                        msSQL = "UPDATE ims_trn_tstock " +
                                                "SET issued_qty = " + (issueQty + quantityForThisRow) + " " +
                                                "WHERE product_gid = '" + lsproduct_gid + "' " +
                                                "AND branch_gid = '" + lsbranch + "' " +
                                                "AND stock_gid = '" + stockgid + "'";
                                    }
                                    else
                                    {
                                        msSQL = "UPDATE ims_trn_tstock " +
                                                "SET issued_qty = " + (issueQty + quantityForThisRow) + " " +
                                                "WHERE product_gid = '" + lsproduct_gid + "' " +
                                                "AND branch_gid = '" + lsbranch + "' " +
                                                "AND stock_gid = '" + stockgid + "'";
                                    }

                                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                                   }

                                objODBCDataReader.Close();




                                if (mnResult1 != 0)
                                {
                                    mcGetGID = objcmnfunctions.GetMasterGID("IMNL");
                                    msSQL = "  Select  distinct  sum(f.stock_qty+f.amend_qty-f.issued_qty-f.damaged_qty-f.transfer_qty) As stock_quantity," +
                                            "  stock_gid from ims_trn_tstock f " +
                                            "  where f.product_gid ='" + lsproduct_gid + "'  and f.branch_gid='" + lsbranch + "' group by f.product_gid ";
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objODBCDataReader.HasRows)
                                    {
                                        if (objODBCDataReader.Read())
                                        {
                                            lsStockQty = objODBCDataReader["stock_quantity"].ToString();
                                            lsStockGid = objODBCDataReader["stock_gid"].ToString();
                                            objODBCDataReader.Close();
                                        }
                                    }
                                    msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");
                                    msSQL = "insert into ims_trn_tstockdtl(" +
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
                                              ") values ( " +
                                              "'" + msstockdtlGid + "'," +
                                              "'" + lsStockGid + "'," +
                                              "'" + lsbranch + "'," +
                                              "'" + lsproduct_gid + "'," +
                                              "'" + lsproductuom_gid + "'," +
                                              "'" + lsqty_requested + "'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'" + msIssueGID + "'," +
                                              "'Issued'," +
                                              "''," +
                                              "'" + user_gid + "'," +
                                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                              "'')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        values.status = false;
                                        values.message = "Error Occured While Inserting Stockdtl!";

                                    }
                                    msGetStockTrackerGID = objcmnfunctions.GetMasterGID("ISTK");

                                    msSQL = " Insert into ims_trn_tstocktracker (" +
                                           " stocktracker_gid," +
                                           " stock_gid, " +
                                           " branch_gid," +
                                           " product_gid," +
                                           " productuom_gid," +
                                           " display_field," +
                                           " qty_issued," +
                                           " stocktype_gid," +
                                           " created_by," +
                                           " created_date," +
                                           " remarks, " +
                                           " mrdtl_gid," +
                                           " mr_gid," +
                                           " reference_gid) " +
                                           " values ( " +
                                           " '" + msGetStockTrackerGID + "', " +
                                           " '" + lsStockGid + "'," +
                                           " '" + lsbranch + "'," +
                                           " '" + lsproduct_gid + "', " +
                                           " '" + lsproductuom_gid + "', ";
                                            if (!string.IsNullOrEmpty(values.display_field) && values.display_field.Contains("'"))
                                            {
                                                msSQL += "'" + values.display_field.Replace("'", "\\\'") + "',";
                                            }
                                            else
                                            {
                                                msSQL += "'" + values.display_field + "', ";
                                            }
                                    
                                         msSQL +=  "'" + lsqty_requested + "'," +
                                           " 'SY0905270006'," +
                                           " '" + user_gid + "'," +
                                           " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                           " 'Material Issued', " +
                                           " '" + msGetImRC + "', " +
                                           " '" + msGetGid + "'," +
                                           " '" + msIssueGID + "')";
                                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                   
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Inserting Stock!";
                                }

                            }
                        }
                        if (mnResult2 != 0)
                        {
                            msSQL = " select b.employee_gid from adm_mst_tuser a " +
                                    " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                                    " where a.user_gid ='" + user_gid + "'";
                            lsemployeegid = objdbconn.GetExecuteScalar(msSQL);

                            msGetPodc = objcmnfunctions.GetMasterGID("PODC");
                            msSQL = " insert into ims_trn_tmrapproval ( " +
                                    " approval_gid, " +
                                    " approved_by, " +
                                    " approved_date, " +
                                    " submodule_gid, " +
                                    " approval_flag, " +
                                    " mrapproval_gid " +
                                    " ) values ( " +
                                    "'" + msGetPodc + "'," +
                                    "'" + lsemployeegid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'IMSTRNMRA'," +
                                    "'Y'," +
                                    "'" + msGetGid + "') ";
                                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult3 != 0)
                            {
                                msSQL = " Update ims_trn_tmaterialrequisition Set " +
                                        " materialrequisition_status = 'ApproveToIssue', " +
                                        " approved_by = '" + user_gid + "', " +
                                        " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                        " where materialrequisition_gid = '" + msGetGid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if(mnResult != 0)
                                {
                                    msSQL = " update ims_trn_tmrapproval set " +
                                            " approval_remarks = 'Self Approved', " +
                                            " approval_flag = 'Y', " +
                                            " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd")+ "'" +
                                            " where approved_by = '" + lsemployeegid + "'" +
                                            " and mrapproval_gid = '" + msGetGid + "' and submodule_gid='IMSTRNMRA'";
                                            mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult2 != 0)
                                    {
                                        msSQL = " delete from ims_tmp_tmaterialrequisition " +
                                                " where user_gid = '" + user_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Direct Material Issued successfully";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While Inserting Material Status!";
                            }
                          
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Inserting Stock Tracker!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Inserting Material Issue !";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Inserting Material Requisition !";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}