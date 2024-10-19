using ems.inventory.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Web;
using static ems.inventory.Models.MdlImsTrnRaiseMI;

namespace ems.inventory.DataAccess
{
    public class DaImsTrnRaiseMI
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2, mnResult1, mnResult3;
        string msGetGid, msGetGid1, lsempoyeegid, mnCtr, lsbranch, lscostcenter, msIssueGID, lsemployeegid, lslocation, msGetGID, msGetImRC, msGetPodc, mcGetGID, lsStockGid, lsStockQty, msstockdtlGid, msGetStockTrackerGID, lsmaterialissued_date, lsexpected_date;
        double lsbudgetallocated, lsprovisional, lsamtused, lsavailable, lsreq, lstolrequest, lsreq1, lsrequested;
        string lsproduct_gid, lsproductuom_gid, lsproductuom_gid1, lsproduct_remarks, lsavailable2, lsproduct_code, lsproduct_name, lsproductuom_name, lsproductgroup_gid, lsproductgroup_name,
            lsqty_requested, lsdisplay_field;
        List<int> quantities = new List<int>();
        public void DaGetMIsummary(string user_gid, MdlImsTrnRaiseMI values)
        {
            msSQL = " delete from ims_tmp_tmaterialrequisition " +
                    " where user_gid = '" + user_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msSQL = "select concat_ws(' - ',a.user_firstname,a.user_lastname) as user_firstname," +
                    "b.branch_gid, b.department_gid, d.branch_name,d.mainbranch_flag,c.department_name " +
                    "from adm_mst_tuser a left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                    "left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                    "left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid where a.user_gid = '" + user_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<imsraiseMi_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new imsraiseMi_list
                    {
                        user_firstname = dt["user_firstname"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        department_name = dt["department_name"].ToString(),
                    });
                    values.imsraiseMilist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetMIproducttype(MdlImsTrnRaiseMI values)
        {
            try
            {
                msSQL = " select producttype_gid, producttype_code , producttype_name from pmr_mst_tproducttype";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<imsmiproducttypelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new imsmiproducttypelist
                        {
                            producttype_gid = dt["producttype_gid"].ToString(),
                            producttype_code = dt["producttype_code"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),
                        });
                        values.imsmiproducttype_list = getModuleList;
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
        public void DaGetMIProductSummary(string producttype_gid, string product_name, MdlImsTrnRaiseMI values)
        {
            try
            {
                msSQL = " SELECT a.product_name,  CASE WHEN a.customerproduct_code IS NULL THEN a.product_code ELSE a.customerproduct_code END AS product_code, " +
                        " a.product_gid, a.product_desc,a.mrp_price,a.cost_price, b.productuom_gid, d.producttype_name, b.productuom_name, c.productgroup_name, " +
                        " c.productgroup_gid,a.productuom_gid, d.producttype_gid FROM pmr_mst_tproduct a" +
                        " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid" +
                        " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid" +
                        " LEFT JOIN pmr_mst_tproducttype d ON d.producttype_gid = a.producttype_gid " +
                        " WHERE 1=1 and a.status=1 and d.producttype_name<>'Services'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<imsMIproductsummary_list>();


                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new imsMIproductsummary_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["product_desc"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_percentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);

                    }
                    values.imsMIproductsummarylist = getModuleList;
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostOnmiproduct(string user_gid, imsproductsingle_list values)
        {
            try
            {
                msSQL = " select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                lsproductuom_gid1= objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select qty_requested,display_field from ims_tmp_tmaterialrequisition where " +
                        " product_gid = '" + values.product_gid + "' and " +
                        " productuom_gid = '" + lsproductuom_gid1 + "' and " +
                        " display_field = '" + (string.IsNullOrEmpty(values.display_field) ? "" : values.display_field.Replace("'", "\\\'"))  + "' and " +
                        " user_gid = '" + user_gid + "'";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsrequested = (double)dt["qty_requested"];
                        lsreq1 = values.qty_requested;
                        lstolrequest = lsreq1 + lsrequested;
                            msSQL = " update ims_tmp_tmaterialrequisition " +
                                    " set qty_requested ='" + lstolrequest + "', " +
                                    " display_field ='" + (string.IsNullOrEmpty(values.display_field) ? "" : values.display_field.Replace("'", "\\\'")) + "' " +
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
                    string product_name= objdbconn.GetExecuteScalar(msSQL); 

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
                            " '" + values.product_gid + "'," +
                            " '" + lsproductuom_gid1 + "'," +
                            " '" + values.productgroup_gid + "'," +
                            " '" + values.product_code + "'," +
                            " '" + product_name + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + values.productgroup_name + "'," +
                            " '" + values.qty_requested + "'," +
                            " '" + user_gid + "'," +
                            " '" + (string.IsNullOrEmpty(values.display_field) ? "" : values.display_field.Replace("'", "\\\'")) + "'" + ")";
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
        public void DaPostMIProduct(string user_gid, MIproductbulk values)
        {
            try
            {
                for (int i = 0; i < values.imsproductMI_list.Count; i++)
                {
                    msSQL = " select qty_requested,display_field from ims_tmp_tmaterialrequisition where " +
                            " product_gid = '" + values.imsproductMI_list[i].product_gid + "' and " +
                            " productuom_gid = '" + values.imsproductMI_list[i].productuom_gid + "' and " +
                            " display_field = '" + values.imsproductMI_list[i].display_field.Replace("'", "\\\'") + "' and " +
                            " user_gid = '" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            lsrequested = (double)dt["qty_requested"];
                            lsreq1 = values.imsproductMI_list[i].qty_requested;
                            lstolrequest = lsreq1 + lsrequested;
                            msSQL = " update ims_tmp_tmaterialrequisition " +
                                        " set qty_requested ='" + lstolrequest + "', " +
                                        " display_field ='" + values.imsproductMI_list[i].display_field.Replace("'", "\\\'") + "' " +
                                        " where product_gid = '" + values.imsproductMI_list[i].product_gid + "' and " +
                                        " productuom_gid = '" + values.imsproductMI_list[i].productuom_gid + "' and " +
                                        " display_field='" + values.imsproductMI_list[i].display_field.Replace("'", "\\\'") + "' and" +
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
                                " '" + values.imsproductMI_list[i].product_gid + "'," +
                                " '" + values.imsproductMI_list[i].productuom_gid + "'," +
                                " '" + values.imsproductMI_list[i].productgroup_gid + "'," +
                                " '" + values.imsproductMI_list[i].product_code + "'," +
                                " '" + values.imsproductMI_list[i].product_name + "'," +
                                " '" + values.imsproductMI_list[i].productuom_name + "'," +
                                " '" + values.imsproductMI_list[i].productgroup_name + "'," +
                                " '" + values.imsproductMI_list[i].qty_requested + "'," +
                                " '" + user_gid + "'," +
                                " '" + (string.IsNullOrEmpty(values.imsproductMI_list[i].display_field) ? "" : values.imsproductMI_list[i].display_field.Replace("'", "\\\'")) + "'" + ")";
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
        public void DatmpProductSummary(string user_gid, MdlImsTrnRaiseMI values)
        {
            try
            {

                msSQL = " Select distinct a.tmpmaterialrequisition_gid,  b.product_name,c.productgroup_name,b.product_gid,d.productuom_name,  " +
                        " b.product_code, b.product_name as product, a.qty_requested, a.product_remarks,a.display_field  " +
                        " from ims_tmp_tmaterialrequisition a  " +
                        " left join pmr_mst_tproduct b on b.product_gid = a.product_gid   " +
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid  " +
                        " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = b.productuom_gid " +
                        " where a.user_gid = '" + user_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tmpproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tmpproduct_list
                        {
                            tmpmaterialrequisition_gid = dt["tmpmaterialrequisition_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product = dt["product"].ToString(),
                            qty_requested = Convert.ToDecimal(dt["qty_requested"]).ToString("F2"),
                            product_remarks = dt["product_remarks"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productuom_name = dt["productuom_name"].ToString()
                        });
                        values.tmpproductlist = getModuleList;
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

        public void DaMaterialIndent(string user_gid, issuematerial_list values)
        {
            try
            {
                msSQL = "select count(*) as mnCtr from ims_tmp_tmaterialrequisition where user_gid ='" + user_gid + "'";
                mnCtr = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name + "'";
                lsbranch = objdbconn.GetExecuteScalar(msSQL);
                if (values.costcenter_name != "")
                {
                    msSQL = "select costcenter_gid from pmr_mst_tcostcenter where costcenter_name='" + values.costcenter_name + "'";
                    lscostcenter = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.location_name != "")
                {
                    msSQL = "select location_gid from ims_mst_tlocation where location_name = '" + values.location_name + "'";
                    lslocation = objdbconn.GetExecuteScalar(msSQL);

                }
                DateTime uiDate = DateTime.ParseExact(values.materialissued_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                lsmaterialissued_date = uiDate.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime uiDate1 = DateTime.ParseExact(values.expected_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                lsexpected_date = uiDate1.ToString("yyyy-MM-dd HH:mm:ss");

                msGetGid = objcmnfunctions.GetMasterGID("IMRP");
                msSQL = " Insert into ims_trn_tmaterialrequisition " +
                       " (materialrequisition_gid, " +
                       " branch_gid, " +
                       " materialrequisition_date, " +
                       " materialrequisition_remarks, " +
                       " materialrequisition_reference, " +
                       " user_gid, " +
                       " created_by, " +
                       " created_date, " +
                       " materialrequisition_status, " +
                       " product_count, " +
                       " materialrequisition_type, " +
                       " material_status, " +
                       " priority, " +
                       " expected_date, " +
                       " priority_remarks, " +
                       " costcenter_gid ) " +
                       " values (" +
                       "'" + msGetGid + "', " +
                       "'" + lsbranch + "'," +
                       "'" + lsmaterialissued_date + "', " +
                       "'" + (string.IsNullOrEmpty(values.materialrequisition_remarks) ? "" : values.materialrequisition_remarks.Replace("'", "\\\'")) + "', " +
                       "'" + values.materialrequisition_reference + "', " +
                       "'" + user_gid + "', " +
                       "'" + user_gid + "', " +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                       "'Pending', " +
                       "'" + mnCtr + "', " +
                       "'PT00010001204' , " +
                       "'PR Not Raised', " +
                       "'"+ values.priority + "', " +
                       "'"+ lsexpected_date + "', " +
                       "'Null', " +
                       "'" + lscostcenter + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {  
                        msSQL = " select a.product_gid, productuom_gid, a.product_remarks,a.product_code,a.product_name," +
                                " a.productuom_name,a.productgroup_gid,a.productgroup_name," +
                                " a.qty_requested,a.display_field from ims_tmp_tmaterialrequisition a" +
                                " where a.user_gid ='" + user_gid + "'";
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
                                          " created_by, " +
                                          " created_date, " +
                                          " display_field , " +
                                          " mr_originalqty," +
                                          " requested_by," +
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
                                          "'" + "0" + "', " +
                                          "'" + user_gid + "', " +
                                          "'" + user_gid + "', " +
                                          "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                          "'" + (string.IsNullOrEmpty(lsdisplay_field) ? "" : lsdisplay_field.Replace("'", "\\\'"))  + "'," +
                                          "'" + lsqty_requested + "'," +
                                          "'" + values.employee_name + "'," +
                                          "'N')";
                                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult == 0)
                                {
                                    values.status = false;
                                    values.message = "Error While Inserting material details!";
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
                                    " created_by, " +
                                    " created_date, " +
                                    " submodule_gid, " +
                                    " approval_flag, " +
                                    " mrapproval_gid " +
                                    " ) values ( " +
                                    "'" + msGetPodc + "'," +
                                    "'" + lsemployeegid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'" + user_gid + "', " +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                    "'IMSTRNMRA'," +
                                    "'Y'," +
                                    "'" + msGetGid + "') ";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult3 != 0)
                            {
                            //msSQL = "select poapproval_flag from adm_mst_tcompany";
                            //string lsapproval_flage = objdbconn.GetExecuteScalar(msSQL);

                            //if (lsapproval_flage == "" || lsapproval_flage == "N" || lsapproval_flage == null)
                            //{
                            //    msSQL = " Update ims_trn_tmaterialrequisition Set " +
                            //            " materialrequisition_status = 'Approved To Issue', " +
                            //            " approved_by = '" + user_gid + "', " +
                            //            " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                            //            " where materialrequisition_gid = '" + msGetGid + "'";
                            //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            //}
                            //else
                            //{
                            //    msSQL = " Update ims_trn_tmaterialrequisition Set " +
                            //            " materialrequisition_status = 'Pending', " +
                            //            " approved_by = '" + user_gid + "', " +
                            //            " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                            //            " where materialrequisition_gid = '" + msGetGid + "'";
                            //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            //}

                                //if (mnResult != 0)
                                //{
                                //    msSQL = " update ims_trn_tmrapproval set " +
                                //            " approval_remarks = 'Self Approved', " +
                                //            " approval_flag = 'Y', " +
                                //            " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                //            " where approved_by = '" + lsemployeegid + "'" +
                                //            " and mrapproval_gid = '" + msGetGid + "' and submodule_gid='IMSTRNMRA'";
                                //    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    //if (mnResult2 != 0)
                                    //{
                                        msSQL = " delete from ims_tmp_tmaterialrequisition " +
                                                " where user_gid = '" + user_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Material Indent Raised successfully";
                                        }
                                    //}
                                //}
                            }
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
        public void DaDeletetmpProductSummary(string tmpmaterialrequisition_gid, MdlImsTrnRaiseMI values)
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
                values.message = "Exception occured while deleting product in MI!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

    }
}