using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Services.Description;


namespace ems.pmr.DataAccess
{
    public class DaPmrTrnPurchaseRequisition
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string hierarchy_flag;
        Image branch_logo;
        string company_logo_path, authorized_sign_path;
        Image company_logo;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, msGetGID, msGetGid, lsCode, msPOGetGID, msINGetGID, msGetGID1, msPO1GetGID;
        int mnResult, i;
        DataTable mail_datatable, dt_datatable;
        OdbcDataReader objOdbcDataReader;
        // Purchase Requisition Summary

        public void DaGetPmrTrnPurchaseRequisition(MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                 
                msSQL = " select distinct a.purchaserequisition_gid,DATE_FORMAT(a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, " +
                " a.materialrequisition_gid," +
                " a.purchaserequisition_flag,replace(a.purchaserequisition_remarks,'PI','PR') as purchaserequisition_remarks, " +
                " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag " +
                " when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                " when a.purchaserequisition_flag='PR Pending Approval' then 'PR Pending Approval' " +
                " when a.purchaserequisition_flag='PR Approved' then 'PR Approved' " +
                " when a.purchaserequisition_flag='PR Rejected' then 'PR Rejected' " +
                " else a.purchaserequisition_flag end as 'overall_status', " +
                " b.user_firstname,d.department_name,e.branch_prefix AS branch_name, d.department_name,replace(a.purchaserequisition_referencenumber,'PI','PR') as purchaserequisition_referencenumber,g.costcenter_name " +
                " from pmr_trn_tpurchaserequisition a " +
                " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                " left join hrm_mst_tbranch e on a.branch_gid = e.branch_gid " +
                " left join adm_mst_tmodule2employee  f on f.employee_gid = c.employee_gid " +
                " left join pmr_mst_tcostcenter g on a.costcenter_gid=g.costcenter_gid " +
                " where 0=0 Order by a.purchaserequisition_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<request_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new request_list
                        {
                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            purchaserequisition_remarks = dt["purchaserequisition_remarks"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString()



                        });
                        values.purchaserequest_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting in PR!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetBranch1(MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                 
                msSQL = " Select branch_name,branch_gid " +
                    " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranch1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranch1
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetBranch1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PR!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }



        //togetbranchname and employeename

        public void DaGetuserdtl(string user_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                
                msSQL = " SELECT concat(a.user_firstname,' ',a.user_lastname) as employee_name ,b.employee_gid,c.department_name FROM adm_mst_tuser a " +
                " left join hrm_mst_temployee b on b.user_gid=a.user_gid " +
                " left join hrm_mst_tdepartment c on c.department_gid=b.department_gid " +
                " where a.user_gid= '" + user_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getuserdtl>();
                if (dt_datatable.Rows.Count != 0)

                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getuserdtl
                        {
                            employee_name = dt["employee_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.Getuserdtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whilegetting user details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        //cost center
        public void DaGetcostcenter(string user_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                 
                msSQL = " select costcenter_gid,concat_ws('-',costcenter_name,costcenter_code) as costcenter_name,   " +
            " budget_available AS available_amount from pmr_mst_tcostcenter ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcostcenter>();
                if (dt_datatable.Rows.Count != 0)

                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcostcenter
                        {
                            costcenter_gid = dt["costcenter_gid"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            available_amount = dt["available_amount"].ToString(),
                        });
                        values.Getcostcenter = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting cost center!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetOnChangecostcenter(string costcenter_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                
                msSQL = " select costcenter_gid,concat_ws('-',costcenter_name,costcenter_code) as costcenter_name,   " +
        " budget_available AS available_amount from pmr_mst_tcostcenter ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcostcenter>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcostcenter
                        {
                            available_amount = dt["available_amount"].ToString(),

                        });
                        values.Getcostcenter = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing cost center!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetProductCode1(MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                

                msSQL = " Select product_code,product_gid,product_name from pmr_mst_tproduct  where 1=1 and status=1  ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductCode1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductCode1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                        });
                        values.GetProductCode1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while gettting product code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             

        }

        public void DaGetProduct1(MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                
                msSQL = " Select product_gid, product_name from pmr_mst_tproduct   where 1=1 and status=1 " +
" order by product_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProduct1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProduct1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                        });
                        values.GetProduct1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetOnChangeProductCode(string product_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                
                if (product_gid != null)
                {
                    msSQL = " Select a.product_name, a.product_code, b.productuom_gid,b.productuom_name,c.productgroup_name,a.product_desc  from pmr_mst_tproduct a  " +
                     " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductCode1>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductCode1
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                display_field = dt["product_desc"].ToString(),

                            });
                            values.GetProductCode1 = getModuleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing product code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaGetOnChangeProductName(string product_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                 
                if (product_gid != null)
                {
                    msSQL = " Select a.product_gid, a.product_name, a.product_code, b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid,a.product_desc  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                    " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductCode1>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductCode1
                            {
                                product_gid = dt["product_gid"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                display_field = dt["product_desc"].ToString(),

                            });
                            values.GetProductCode1 = getModuleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changingproduct name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }

        public void DaPostOnAdd(string user_gid, productlist1 values)
        {
            try
            {
                
                msGetGid = objcmnfunctions.GetMasterGID("PODC");
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                {
                    msSQL = " insert into pmr_tmp_tpurchaserequisition ( " +
                            " product_gid," +
                            " product_code," +
                            " product_name," +
                            " productgroup_name," +
                            " productuom_name," +
                            " uom_gid," +
                            " created_by ," +
                            " display_field ," +
                            " qty_requested" +
                            " ) values ( " +
                            "'" + values.product_gid + "'," +
                            "'" + values.product_code + "'," +
                            "'" + lsproductName + "'," +
                            "'" + values.productgroup_name + "'," +
                            "'" + values.productuom_name + "'," +
                            "'" + lsproductuomgid + "'," +
                            "'" + user_gid + "',";
                            if (values.display_field == null || values.display_field == "")
                            {
                                msSQL += "'',";
                            }
                            else
                            {
                                msSQL += "'" + values.display_field.Replace("'", "\\\'") + "',";
                            }
                            msSQL +="'" + values.qty_requested + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaProductSummary(string user_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                

                msSQL = " Select distinct a.tmppurchaserequisition_gid, c.productgroup_name,a.display_field," +
                    " b.product_code, b.product_gid, b.product_name, " +
                    " format(a.qty_requested,2) as qty_requested, d.productuom_name " +
                    " from pmr_tmp_tpurchaserequisition a " +
                    " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                    " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuom d on d.productuom_gid = a.uom_gid" +
                    " where a.created_by='" + user_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummary_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new productsummary_list1
                        {

                            tmppurchaserequisition_gid = dt["tmppurchaserequisition_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            display_field = dt["display_field"].ToString(),




                        });
                        values.productsummary_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaGetDeletePrProductSummary(string user_gid, string tmppurchaserequisition_gid, productsummary_list1 values)
        {
            try
            {
                 
                msSQL = "delete from pmr_tmp_tpurchaserequisition where tmppurchaserequisition_gid='" + tmppurchaserequisition_gid + "' and created_by ='" + user_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product  Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while delete product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
        }
        public void DaPostPurchaseRequisition(string employee_gid, string user_gid, PostAllPR values)
        {
            try
            {
                 
                string uiDateStr = values.purchaserequisition_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string purchaserequisition_date = uiDate.ToString("yyyy-MM-dd");



                msINGetGID = objcmnfunctions.GetMasterGID("PPRP");

                msSQL = "select count(*) as mnCtr from pmr_tmp_tpurchaserequisition where created_by = '" + user_gid + "'";
                string mnCtr = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " Insert into pmr_trn_tpurchaserequisition " +
                        " (purchaserequisition_gid, " +
                        " branch_gid, " +
                        " purchaserequisition_date, " +
                        " purchaserequisition_remarks, " +
                        " purchaserequisition_referencenumber, " +
                        " created_by, " +
                        " created_date, " +
                        " purchaserequisition_flag, " +
                        " purchaserequisition_status, " +
                        " product_count, " +
                        " enquiry_raised, " +
                        " type, " +
                        " purchaseorder_raised, " +
                        " priority, " +
                        " costcenter_gid)" +
                        " values (" +
                        "'" + msINGetGID + "', " +
                        "'" + values.branch_gid + "'," +
                        "'" + purchaserequisition_date + "', " +
                        "'" + values.purchaserequisition_remarks.Replace("'", "\\\'") + "', " +
                        "'" + values.purchaserequisition_referencenumber + "', " +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'PR Approved', " +
                        "'PR Approved', " +
                        "'" + mnCtr + "', " +
                        "'N', " +
                        "'Opex', " +
                        "'N', " +
                        "'" + values.priority_remarks.Replace("'", "\\\'")+ "', " +
                        "'" + values.costcenter_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 1)
                {
                    msSQL = " Select distinct a.tmppurchaserequisition_gid, c.productgroup_name,c.productgroup_gid, " +
                            " b.product_code, b.product_gid, b.product_name,a.display_field,b.product_price, " +
                            " format(a.qty_requested,2) as qty_requested, d.productuom_name ,d.productuom_gid" +
                            " from pmr_tmp_tpurchaserequisition a " +
                            " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                            " left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                            " left join pmr_mst_tproductuom d on d.productuom_gid = a.uom_gid" +
                            " where a.created_by='" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PPDC");

                        msSQL = " Insert into pmr_trn_tpurchaserequisitiondtl " +
                           " (purchaserequisitiondtl_gid," +
                           " purchaserequisition_gid , " +
                           " product_gid," +
                           " product_code," +
                           " product_name," +
                           " uom_gid," +
                           " productuom_name," +
                           " productgroup_gid," +
                           " productgroup_name," +
                           " qty_requested, " +
                           " seq_no, " +
                           " user_gid, " +
                           " display_field, " +
                           " pr_originalqty," +
                           " pr_newproductstatus," +
                           " requested_by," +
                           " product_price " +
                           " )values (" +
                           "'" + msGetGid + "'," +
                           "'" + msINGetGID + "'," +
                            "'" + dt["product_gid"].ToString() + "'," +
                            "'" + dt["product_code"].ToString() + "'," +
                            "'" + dt["product_name"].ToString() + "'," +
                            "'" + dt["productuom_gid"].ToString() + "'," +
                            "'" + dt["productuom_name"].ToString() + "'," +
                            "'" + dt["productgroup_gid"].ToString() + "'," +
                            "'" + dt["productgroup_name"].ToString() + "'," +
                            "'" + dt["qty_requested"].ToString() + "'," +
                            "'" + i + "'," +
                            "'" + user_gid + "'," +
                            "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "', " +
                            "'" + dt["qty_requested"].ToString() + "'," +
                            "   'Y' ," +
                            "'" + values.employee_name + "'," +
                            "'" + dt["product_price"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Purchase Requisition Submited Successfully";
                        msSQL = " delete from pmr_tmp_tpurchaserequisition where created_by='" + user_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Purchase Requisition";
                    }

                    dt_datatable.Dispose();

                    msGetGID1 = objcmnfunctions.GetMasterGID("PODC");

                    msSQL = " insert into pmr_trn_tapproval ( " +
                        " approval_gid, " +
                        " approved_by, " +
                        " approved_date, " +
                        " submodule_gid, " +
                        " pr_gid " +
                        " ) values ( " +
                        "'" + msGetGID1 + "'," +
                        " '" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'PMRPROAPR'," +
                        "'" + msINGetGID + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update pmr_trn_tapproval set " +
                       " approval_flag = 'Y', " +
                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                       " where approved_by = '" + user_gid + "'" +
                       " and pr_gid = '" + msINGetGID + "' and submodule_gid='PMRPROAPR'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding in purchase requisition!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetProductdetails(string purchaserequisition_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                
                msSQL = "select product_code,product_name,qty_requested,pr_originalqty from pmr_trn_tpurchaserequisitiondtl where purchaserequisition_gid='" + purchaserequisition_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productlistdetailspr>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productlistdetailspr
                        {

                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            pr_originalqty = dt["pr_originalqty"].ToString(),
                        });
                        values.productlistdetailspr = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product details in PR!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        // PURCHASE REQUISITION VIEW

        public void DaGetPurchaseRequisitionView(string purchaserequisition_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {

                msSQL = " select a.purchaserequisition_gid, a.type,n.user_firstname as requested_by,  " +
                        " date_format(a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date, " +
                        " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag " +
                        " when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                        " else a.purchaserequisition_flag end as 'overall_status', " +
                        " a.purchaserequisition_remarks,a.purchaserequisition_referencenumber,CASE WHEN a.priority = 'N' THEN 'Low' ELSE 'High' END AS priority, a.approver_remarks, a.purchaserequisition_flag, " +
                        " concat_ws(' - ',b.user_firstname,b.user_lastname) as user_firstname, e.user_firstname as approved_by, f.branch_name,d.department_name, " +
                        " a.costcenter_gid,format(g.budget_available, 2) as budget_available," +
                        " format((g.budget_allocated - g.amount_used - g.provisional_amount),2) as available_amount," +
                        " concat_ws('-',g.costcenter_code,g.costcenter_name) as costcenter_name,g.costcenter_gid, l.user_firstname as approvername,g.costcenter_name, " +
                        " date_format(a.approved_date,'%d-%m-%Y') as approved_date,format(a.provisional_amount,2)as provisional_amount  " +
                        " from pmr_trn_tpurchaserequisition a " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                        " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment d on c.department_gid = d.department_gid " +
                        " left join adm_mst_tuser e on e.user_gid = a.approved_by " +
                        " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid " +
                        " left join pmr_mst_tcostcenter g on g.costcenter_gid = a.costcenter_gid " +
                        " left join adm_mst_tuser l on l.user_gid = a.approved_by " +
                        " left join pmr_trn_tpurchaserequisitiondtl m on m.purchaserequisition_gid = a.purchaserequisition_gid  " +
                        " left join adm_mst_tuser n on m.requested_by = n.user_gid  " +
                        " where a.purchaserequisition_gid = '" + purchaserequisition_gid + "' group by a.purchaserequisition_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<purchaserequestitionviewlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new purchaserequestitionviewlist
                        {

                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_remarks = dt["purchaserequisition_remarks"].ToString(),
                            purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            requested_by = dt["requested_by"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            priority_remarks = dt["priority"].ToString(),
                            //product_name = dt["product_name"].ToString(),
                            //product_code = dt["product_code"].ToString(),
                            //qty_requested = dt["qty_requested"].ToString(),
                            //productgroup_name = dt["productgroup_name"].ToString(),
                            //productuom_name = dt["productuom_name"].ToString(),
                            //display_field = dt["display_field"].ToString(),

                        });
                        values.purchaserequestitionview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PR view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
           public void DaGetPurchaseRequisitionproduct(string purchaserequisition_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {
                 
                msSQL = "select a.product_remarks, a. qty_requested ,  "+
                       " format(a.qty_ordered, 2) as qty_ordered, format(a.qty_received, 2) as qty_received,"+
                       "a.product_name, a.product_code, a.productgroup_name, a.productuom_name,  " +
                       " date_format(e.purchaserequisition_date, '%d-%m-%Y') as purchaserequisition_date,a.display_field,a.pr_originalqty "+
                       "  from pmr_trn_tpurchaserequisitiondtl a "+
                      "   left join pmr_mst_tproduct b on a.product_gid = b.product_gid "+
                     "    left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid "+
                      "   left join pmr_mst_tproductuom d on d.productuom_gid = a.uom_gid "+
                      "   left join pmr_trn_tpurchaserequisition e on e.purchaserequisition_gid = a.purchaserequisition_gid "+
                      "   where a.purchaserequisition_gid= '" + purchaserequisition_gid + "'and " +
                       "  qty_requested != 0.00 order by a.seq_no";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<purchaserequestitionviewlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new purchaserequestitionviewlist
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),

                        });
                        values.purchaseprproduct_list  = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PR view!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
           $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        //Report
        public void DaGetPurchaserequisitionrpt(string branch_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {



                msSQL = " select distinct a.purchaserequisition_gid, a.purchaserequisition_status,g.branch_name, " +
                        " date_format( a.purchaserequisition_date,'%d-%m-%Y') as purchaserequisition_date,g.branch_gid," +
                        " a.approved_date, (e.user_firstname) as approved_by, " +
                        " CASE when a.grn_flag <> 'GRN Pending' then a.grn_flag " +
                        " when a.purchaseorder_flag <> 'PO Pending' then a.purchaseorder_flag " +
                        " when a.purchaserequisition_flag='PR Pending Approval' then 'PR Pending Approval' " +
                        " when a.purchaserequisition_flag='PR Approved' then 'PR Approved' " +
                        " when a.purchaserequisition_flag='PR Rejected' then 'PR Rejected' " +
                        " else a.purchaserequisition_flag end as 'overall_status', " +
                        " a.purchaserequisition_flag, a.purchaseorder_flag, a.grn_flag, " +
                        " b.user_firstname,c.department_name,replace(a.purchaserequisition_referencenumber,'PI','PR') as purchaserequisition_referencenumber" +
                        " from pmr_trn_tpurchaserequisition a " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                        " left join hrm_mst_temployee d on d.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment c on d.department_gid = c.department_gid " +
                        " left join adm_mst_tuser e on e.user_gid = a.approved_by " +
                        " left join hrm_mst_tbranch g on g.branch_gid=a.branch_gid" +
                        " where 0 = 0 and a.branch_gid = '" + branch_gid + "'" +
                        " Order by a.purchaserequisition_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchaserequisition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchaserequisition_list
                        {

                            purchaserequisition_gid = dt["purchaserequisition_gid"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            created_by = dt["user_firstname"].ToString(),
                            purchaserequisition_date = dt["purchaserequisition_date"].ToString(),
                            purchaserequisition_status = dt["purchaserequisition_status"].ToString(),
                            //costcenter_name = dt["costcenter_name"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            //purchaserequisition_remarks = dt["purchaserequisition_remarks"].ToString(),
                            pr_raisedby = dt["user_firstname"].ToString(),
                            //mr_raisedby = dt["mr_raisedby"].ToString(),
                            purchaserequisition_referencenumber = dt["purchaserequisition_referencenumber"].ToString(),


                        });
                        values.Getpurchaserequisition_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }

        public void DaGetpurchaserequisitionBranch(MdlPmrTrnPurchaseRequisition values)
        {
            try
            {

                msSQL = " Select branch_name,branch_gid " +
                " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetpurchaserequisitionBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetpurchaserequisitionBranch
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetpurchaserequisitionBranch = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPurchaserequisitionexcel(string branch_gid, MdlPmrTrnPurchaseRequisition values)
        {
            try
            {



                msSQL = " select date_format(a.approved_date,'%d-%m-%Y') as approved_date,a.purchaserequisition_gid as Purchase_Req_No, n.product_code as Product_Code, " +
                        " n.product_name as Product_Name,o.productuomclass_name as Unit,m.qty_requested as Requested_Qty,a.purchaserequisition_status as Status " +
                        " from pmr_trn_tpurchaserequisition a " +
                        " left join adm_mst_tuser b on a.created_by = b.user_gid " +
                        " left join hrm_mst_temployee d on d.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment c on d.department_gid = c.department_gid " +
                        " left join adm_mst_tuser e on e.user_gid = a.approved_by " +
                        " left join pmr_trn_tpurchaserequisitiondtl m on a.purchaserequisition_gid = m.purchaserequisition_gid " +
                        " left join pmr_mst_tproduct n on m.product_gid = n.product_gid " +
                        " left join pmr_mst_tproductuomclass o on o.productuomclass_gid = n.productuomclass_gid" +
                        " where 0 = 0  and a.branch_gid = '" + branch_gid + "'" +
                        " Order by  a.purchaserequisition_date desc, a.purchaserequisition_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchaserequisitionexcel_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchaserequisitionexcel_list
                        {

                            approved_date = dt["approved_date"].ToString(),
                            Purchase_Req_No = dt["Purchase_Req_No"].ToString(),
                            Product_Code = dt["Product_Code"].ToString(),
                            Product_Name = dt["Product_Name"].ToString(),
                            Unit = dt["Unit"].ToString(),                          
                            Requested_Qty = dt["Requested_Qty"].ToString(),
                            Status = dt["Status"].ToString(),


                        });
                        values.Getpurchaserequisitionexcel_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }



        public Dictionary<string, object> DaGetPurchaseRequistRpt(string purchaserequisition_gid, string branch_gid, MdlPmrTrnPurchaseRequisition values)

        {
            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = " select  a.purchaserequisition_gid as preferenceno,date_format(a.purchaserequisition_date,'%d-%m-%Y')as purchaserequisition_date ," +
                    " a.purchaserequisition_remarks,a.approved_by, date_format(a.approved_date,'%d-%m-%Y')as approved_date,  a.created_by," +
                    " date_format(a.created_date,'%d-%m-%Y')as created_date,b.user_firstname as approval_name," +
                    " c.user_firstname as created_name,d.costcenter_name," +
                    " a.costcenter_gid, a.approver_remarks,  e.branch_name,e.address1 as  DataColumn16,e.city as  DataColumn17," +
                    " e.state as  DataColumn18,e.postal_code as  DataColumn19,e.contact_number as  DataColumn20,e.email as branch_gid , e.branch_logo" +
                    " from pmr_trn_tpurchaserequisition a" +
                    " left join adm_mst_tuser b on a.approved_by=b.user_gid" +
                    " left join adm_mst_tuser c on a.created_by=c.user_gid" +
                    " left join pmr_mst_tcostcenter d on a.costcenter_gid=d.costcenter_gid" +
                    " left join hrm_mst_tbranch e on a.branch_gid=e.branch_gid" +
                    " WHERE a.purchaserequisition_gid='" + purchaserequisition_gid + "' " +
                    " group by a.purchaserequisition_gid ";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select a.purchaserequisitiondtl_gid, a.purchaserequisition_gid, a.product_gid,a.pr_originalqty," +
                    " a.qty_requested, a.uom_gid, a.display_field, a.product_name, a.productgroup_name, a.productuom_name, a.product_code" +
                    " from pmr_trn_tpurchaserequisitiondtl a" +
                    " left join pmr_trn_tpurchaserequisition e on a.purchaserequisition_gid=e.purchaserequisition_gid" +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid" +
                    " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid" +
                    " WHERE a.purchaserequisition_gid='" + purchaserequisition_gid + "'";
            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = "select  (branch_logo_path) as company_logo  from hrm_mst_tbranch where branch_gid = '" + branch_gid + "' and branch_logo_path is not null";
            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable3.Columns.Add("company_logo", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path))
                    {
                        //Convert  Image Path to Byte
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));
                        DataRow newRow = DataTable3.NewRow();
                        newRow["company_logo"] = branch_logo_bytes;

                        DataTable3.Rows.Add(newRow);
                    }
                }
            }
            dt1.Dispose();
            DataTable3.TableName = "DataTable3";
            myDS.Tables.Add(DataTable3);

            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_purchase"].ToString(), "pmr_purchase_request.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Purchase Reqestion_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }



    }
}